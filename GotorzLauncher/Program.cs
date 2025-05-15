using System.Diagnostics;
using System.Net.Sockets;
using System.Net.NetworkInformation;

class Program
{
    public static async Task Main()
    {
        //------------------------------------------------------------------
        // Path Plumbing
        //------------------------------------------------------------------
        string launcherDir = AppContext.BaseDirectory;

        string solutionRoot = Path.GetFullPath(Path.Combine(launcherDir, "..", "..", "..", ".."));
        string composeFolder = Path.Combine(solutionRoot, "GotorzProjectMain", "GotorzProjectMain", "Docker");
        string composeFile = Path.Combine(composeFolder, "docker-compose.yml");

        //------------------------------------------------------------------
        // 2) Docker‑compose up
        //------------------------------------------------------------------
        Console.Write("Creating and/or starting container");
        var upTask = Task.Run(() => Run("docker", $"compose -f \"{composeFile}\" up -d", composeFolder));
        while (!upTask.IsCompleted) { DotWait(); }
        Console.WriteLine("\n  DONE");

        //------------------------------------------------------------------
        // 3) Wait until SQL container is healthy
        //------------------------------------------------------------------
        Console.Write("Waiting for SQL container to become healthy");
        while (true)
        {
            string status = Capture("docker", "compose ps --format \"{{.Service}}\t{{.State}}\t{{.Health}}\"", composeFolder);
            var lines = status.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            bool healthy = lines.Any(l => l.Contains("sqlserver") && l.Contains("running") && l.Contains("healthy"));
            bool unhealthy = lines.Any(l => l.Contains("sqlserver") && l.Contains("unhealthy"));
            bool exited = lines.Any(l => l.Contains("sqlserver") && l.Contains("exited"));

            if (healthy) { Console.WriteLine("\n  HEALTHY"); break; }
            if (unhealthy || exited)
            {
                Console.WriteLine("\nContainer failed to start (unhealthy or exited).");
                Environment.Exit(1);
            }
            DotWait();
        }

        //------------------------------------------------------------------
        // 4) Run the ASP.NET Core app
        //------------------------------------------------------------------
        Console.WriteLine("Starting Application");
        string gotorzSignalFile = Path.Combine(solutionRoot, "gotorzLaunch.trigger");
        File.WriteAllText(gotorzSignalFile, "go");
        Thread.Sleep(2000);
        File.Delete(gotorzSignalFile);
    }

    //------------------------------------------------------------------
    // Helpers
    //------------------------------------------------------------------
    private static void DotWait()
    {
        Console.Write(".");
        Task.Delay(3000).Wait();
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