using System.Diagnostics;

static class Program
{
    // Path Plumbing
    private static readonly string _launcherDir = AppContext.BaseDirectory;
    private static readonly string _solutionRoot = Path.GetFullPath(Path.Combine(_launcherDir, "..", "..", "..", ".."));
    private static readonly string _composeFolder = Path.Combine(_solutionRoot, "GotorzProjectMain", "GotorzProjectMain", "Docker");
    private static readonly string _composeFile = Path.Combine(_composeFolder, "docker-compose.yml");

    private static void Main()
    {
        ComposeDocker();
        ContainerHealthCheck();
        FreeMainApp();
    }

    private static void ComposeDocker()
    {
        Console.Write("Creating and/or starting container");
        var upTask = Task.Run(() => Run("docker", $"compose -f \"{_composeFile}\" up -d", _composeFolder));
        while (!upTask.IsCompleted) { DotWait(); }
        Console.WriteLine("\n  DONE");
    }
    private static void ContainerHealthCheck()
    {
        Console.Write("Waiting for SQL container to become healthy");
        while (true)
        {
            string status = Capture("docker", "compose ps --format \"{{.Service}}\t{{.State}}\t{{.Health}}\"", _composeFolder);
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
    }
    private static void FreeMainApp()
    {
        Console.WriteLine("Starting Application");
        string gotorzSignalFile = Path.Combine(_solutionRoot, "gotorzLaunch.trigger");
        File.WriteAllText(gotorzSignalFile, "go");
        Thread.Sleep(2000);
        File.Delete(gotorzSignalFile);
    }

    // Helper methods
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