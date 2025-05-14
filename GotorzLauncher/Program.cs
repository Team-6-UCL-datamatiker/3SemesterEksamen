// Program.cs – GotorzLauncher (browser delay replaced with port‑poll)
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
    private const int ContainerPollDelayMs = 3_000;
    private const string SqlServiceName = "sqlserver";
    private const string LaunchUrl = "https://localhost:7064";

    // Windows exit code for process terminated by Ctrl‑C / console close (0xC000013A)
    private const int CtrlCExitCode = unchecked((int)0xC000013A);

    public static async Task Main()
    {
        // suppress Windows error UI so the launcher is always headless
        NativeMethods.SetErrorMode(
            NativeMethods.ErrorModes.SEM_NOGPFAULTERRORBOX |
            NativeMethods.ErrorModes.SEM_FAILCRITICALERRORS |
            NativeMethods.ErrorModes.SEM_NOOPENFILEERRORBOX);

        var launcherDir = AppContext.BaseDirectory;
        var config = Directory.GetParent(launcherDir).Parent!.Name;
        var tfm = Directory.GetParent(launcherDir).Name;

        var solutionRoot = Path.GetFullPath(Path.Combine(launcherDir, "..", "..", "..", ".."));
        var composeFolder = Path.Combine(solutionRoot, "GotorzProjectMain", "GotorzProjectMain", "Docker");
        var composeFile = Path.Combine(composeFolder, "docker-compose.yml");
        var projectPath = Path.Combine(solutionRoot, "GotorzProjectMain", "GotorzProjectMain", "GotorzProjectMain.csproj");

        //------------------------------------------------------------
        // 1) Spin‑up / health‑check SQL Server container
        //------------------------------------------------------------
        Console.Write("Creating and/or starting container");
        var upTask = Task.Run(() => Run("docker", $"compose -f \"{composeFile}\" up -d", composeFolder));
        while (!upTask.IsCompleted)
        {
            Console.Write(".");
            await Task.Delay(ContainerPollDelayMs);
        }
        Console.WriteLine("\n  DONE");

        Console.Write("Waiting for SQL container to become healthy");
        while (true)
        {
            var status = Capture("docker", "compose ps --format \"{{.Service}}\t{{.State}}\t{{.Health}}\"", composeFolder);
            var lines = status.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            bool healthy = lines.Any(l => l.Contains(SqlServiceName) && l.Contains("running") && l.Contains("healthy"));
            bool unhealthy = lines.Any(l => l.Contains(SqlServiceName) && l.Contains("unhealthy"));
            bool exited = lines.Any(l => l.Contains(SqlServiceName) && l.Contains("exited"));

            if (healthy)
            {
                Console.WriteLine("\n  HEALTHY");
                break;
            }
            if (unhealthy || exited)
            {
                Console.WriteLine("\nContainer failed to start (unhealthy or exited).");
                Environment.Exit(1);
            }
            Console.Write(".");
            await Task.Delay(ContainerPollDelayMs);
        }

        //------------------------------------------------------------
        // 2) Run the ASP.NET Core application
        //------------------------------------------------------------
        Console.WriteLine("Starting Application");

        var app = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --no-build --project \"{projectPath}\" --launch-profile \"https\"",
            WorkingDirectory = solutionRoot,
            UseShellExecute = true,
            CreateNoWindow = false
        })!;

        // Launch browser once port is reachable instead of fixed delay
        _ = PollAndLaunchAsync("localhost", 7064, LaunchUrl);

        app.WaitForExit();
        if (app.ExitCode != 0 && app.ExitCode != CtrlCExitCode)
        {
            Console.WriteLine($"App exited with error code {app.ExitCode}");
            Console.WriteLine("Database likely failed to initialize. Consider wiping it or handling migration errors in code.");
        }

        //------------------------------------------------------------
        // 3) Graceful shutdown of docker services
        //------------------------------------------------------------
        Console.Write("Stopping docker services");
        var stopTask = Task.Run(() => Run("docker", $"compose -f \"{composeFile}\" stop", composeFolder));
        while (!stopTask.IsCompleted)
        {
            Console.Write(".");
            await Task.Delay(ContainerPollDelayMs);
        }
        Console.WriteLine("\n  DONE");
    }

    //------------------------------------------------------------------
    // helpers
    //------------------------------------------------------------------
    private static async Task PollAndLaunchAsync(string host, int port, string url, int timeoutMs = 30_000, int stepMs = 500)
    {
        var sw = Stopwatch.StartNew();
        using var client = new TcpClient();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            try
            {
                await client.ConnectAsync(host, port);
                break; // success
            }
            catch
            {
                await Task.Delay(stepMs);
            }
        }
        if (client.Connected)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                Console.WriteLine($"Opened browser at {url}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to launch browser: {ex.Message}");
            }
        }
        else
        {
            Console.Error.WriteLine("Timed out waiting for web app to listen; browser not launched.");
        }
    }

    private static void Run(string cmd, string args, string? workDir = null)
    {
        using var p = Process.Start(new ProcessStartInfo(cmd, args)
        {
            WorkingDirectory = workDir ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        })!;
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        p.WaitForExit();
    }

    private static string Capture(string cmd, string args, string? workDir = null)
    {
        using var p = Process.Start(new ProcessStartInfo(cmd, args)
        {
            WorkingDirectory = workDir ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        })!;
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return output.Trim();
    }
}
