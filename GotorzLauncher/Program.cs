// Program.cs – GotorzLauncher (updated to suppress crash dialog and add EF migration if missing)
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

static class NativeMethods
{
    [Flags]
    public enum ErrorModes : uint
    {
        SEM_FAILCRITICALERRORS = 0x0001,
        SEM_NOGPFAULTERRORBOX = 0x0002,
        SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
        SEM_NOOPENFILEERRORBOX = 0x8000
    }

    [DllImport("kernel32.dll")]
    public static extern ErrorModes SetErrorMode(ErrorModes uMode);
}

class Program
{
    static void Main()
    {
        // Suppress Windows error dialogs for child processes
        NativeMethods.SetErrorMode(
            NativeMethods.ErrorModes.SEM_NOGPFAULTERRORBOX |
            NativeMethods.ErrorModes.SEM_FAILCRITICALERRORS |
            NativeMethods.ErrorModes.SEM_NOOPENFILEERRORBOX
        );

        //------------------------------------------------------------------
        // Resolve paths
        //------------------------------------------------------------------
        string launcherDir = AppContext.BaseDirectory;
        string config = Directory.GetParent(launcherDir).Parent!.Name;
        string tfm = Directory.GetParent(launcherDir).Name;

        string solutionRoot = Path.GetFullPath(Path.Combine(launcherDir, "..\\..\\..\\..\\"));
        string composeFolder = Path.Combine(solutionRoot, "GotorzProjectMain\\GotorzProjectMain\\Docker");
        string composeFile = Path.Combine(composeFolder, "docker-compose.yml");
        string mainOutDir = Path.Combine(solutionRoot, "GotorzProjectMain\\GotorzProjectMain\\bin", config, tfm);
        string projectDir = Path.Combine(solutionRoot, "GotorzProjectMain\\GotorzProjectMain");

        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        string exePath = Path.Combine(mainOutDir, "GotorzProjectMain.exe");
        string dllPath = Path.Combine(mainOutDir, "GotorzProjectMain.dll");
        string mainCmd = isWindows ? exePath : "dotnet";
        string mainArgs = isWindows ? "" : $"\"{dllPath}\"";

        ////------------------------------------------------------------------
        //// Check and create migration if missing
        ////------------------------------------------------------------------
        //var migrationsPath = Path.Combine(projectDir, "Migrations");
        //if (!Directory.Exists(migrationsPath) ||
        //    !Directory.EnumerateFiles(migrationsPath, "*.cs").Any())
        //{
        //    Console.WriteLine("No EF migrations found. Creating initial migration 'init'...");
        //    // run from projectDir so EF outputs to the right folder
        //    Run("dotnet", "ef migrations add init", projectDir);
        //}

        //------------------------------------------------------------------
        // Bring container up
        //------------------------------------------------------------------
        Console.Write("Creating and/or starting container");

        var upTask = Task.Run(() =>
        {
            try
            {
                Run("docker", $"compose -f \"{composeFile}\" up -d", composeFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFailed to start container: {ex.Message}");
                Environment.Exit(1);
            }
        });

        while (!upTask.IsCompleted)
        {
            Console.Write(".");
            Thread.Sleep(3000);
        }

        Console.WriteLine("\n  DONE");

        //------------------------------------------------------------------
        // Wait for health-check
        //------------------------------------------------------------------
        Console.Write("Waiting for SQL container to become healthy");
        while (true)
        {
            string ps = Capture("docker",
                "compose ps --format \"{{.Service}}\t{{.State}}\t{{.Health}}\"",
                composeFolder);

            var lines = ps.Split('\n');

            bool isHealthy = lines.Any(line =>
                line.Contains("sqlserver") &&
                line.Contains("running") &&
                line.Contains("healthy"));

            if (isHealthy)
            {
                Console.WriteLine("\n\n  HEALTHY");
                break;
            }

            bool isUnhealthy = lines.Any(line => line.Contains("sqlserver") && line.Contains("unhealthy"));
            bool isExited = lines.Any(line => line.Contains("sqlserver") && line.Contains("exited"));

            if (isUnhealthy || isExited)
            {
                Console.WriteLine("\nContainer failed to start (unhealthy or exited).");
                Environment.Exit(1);
            }

            Console.Write(".");
            Thread.Sleep(3000);
        }

        Console.WriteLine("\n  HEALTHY");

        //------------------------------------------------------------------
        // Launch main web app via dotnet
        //------------------------------------------------------------------
        var projectPath = Path.Combine(solutionRoot, "GotorzProjectMain", "GotorzProjectMain", "GotorzProjectMain.csproj");

        Console.WriteLine($"> dotnet run --no-build --project \"{projectPath}\"");

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --no-build --project \"{projectPath}\" --launch-profile \"https\"",
            WorkingDirectory = solutionRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false
        };
        //startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";

        using var app = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        bool browserLaunched = false;
        string httpsUrlCandidate = null;
        object launchLock = new object();

        void TryLaunchBrowser(string url)
        {
            if (browserLaunched || string.IsNullOrWhiteSpace(url)) return;
            lock (launchLock)
            {
                if (browserLaunched) return;
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                    browserLaunched = true;
                    Console.WriteLine($"Opened browser at {url}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to launch browser: {ex.Message}");
                }
            }
        }

        // pump stdout and detect when Kestrel reports the listening URL
        app.OutputDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            Console.WriteLine(e.Data);

            if (e.Data.Contains("Now listening on:", StringComparison.OrdinalIgnoreCase))
            {
                var httpIdx = e.Data.IndexOf("http", StringComparison.OrdinalIgnoreCase);
                if (httpIdx < 0) return;
                var url = e.Data.Substring(httpIdx).Trim();

                // Prefer plain HTTP; fall back to HTTPS if that's all we have.
                if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    TryLaunchBrowser(url);
                }
                else if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    httpsUrlCandidate = url; // store, but keep waiting for http
                }
            }
        };

        app.ErrorDataReceived += (_, e) => { if (e.Data != null) Console.Error.WriteLine(e.Data); };

        app.Start();
        app.BeginOutputReadLine();
        app.BeginErrorReadLine();

        // If only HTTPS is available, launch it after a short delay (race‑safe)
        _ = Task.Run(async () =>
        {
            await Task.Delay(4000);
            if (!browserLaunched)
            {
                TryLaunchBrowser(httpsUrlCandidate);
            }
        });

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            if (!app.HasExited)
            {
                Console.WriteLine("Stopping web app...");
                app.Kill(entireProcessTree: true);
            }
        };

        // wait until server exits (Ctrl+C or process killed)
        app.WaitForExit();

        if (app.ExitCode != 0)
        {
            Console.WriteLine($"App exited with error code {app.ExitCode}");
            Console.WriteLine("Database likely failed to initialize. Consider wiping it or handling migration errors in code.");
        }

        //------------------------------------------------------------------
        // Stop (not remove) the container
        //------------------------------------------------------------------
        Console.Write("> docker compose stop\n");
        Run("docker", $"compose -f \"{composeFile}\" stop", composeFolder);
    }

    //------------------------------------------------------------------------
    // Helpers
    //------------------------------------------------------------------------
    static void Run(string cmd, string args, string? workDir = null)
    {
        var psi = new ProcessStartInfo(cmd, args)
        {
            WorkingDirectory = workDir ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        p.WaitForExit();
    }

    static string Capture(string cmd, string args, string? workDir = null)
    {
        var psi = new ProcessStartInfo(cmd, args)
        {
            WorkingDirectory = workDir ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var p = Process.Start(psi)!;
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return output.Trim();
    }
}
