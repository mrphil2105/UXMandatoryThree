using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CafeAnalog;

#if DEBUG
// Modified version from this blog post: https://levelup.gitconnected.com/use-sass-in-an-asp-net-mvc-application-207811b61048
internal sealed class NpmWatchHostedService : IHostedService, IDisposable
{
    private readonly bool _isEnabled;
    private readonly ILogger<NpmWatchHostedService> _logger;

    private Process? _process;

    public NpmWatchHostedService(bool isEnabled, ILogger<NpmWatchHostedService> logger)
    {
        _isEnabled = isEnabled;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_isEnabled)
        {
            StartProcess();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_process == null)
        {
            return Task.CompletedTask;
        }

        _process.Close();
        _process.Dispose();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _process?.Dispose();
        _process = null;
    }

    private void StartProcess()
    {
        _process = new Process();
        _process.StartInfo = new ProcessStartInfo
        {
            FileName =
                Path.Join(Directory.GetCurrentDirectory(),
                    "node_modules/.bin/sass" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".cmd" : "")),
            Arguments = "--watch Styles:wwwroot/css",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = Directory.GetCurrentDirectory()
        };

        _process.EnableRaisingEvents = true;

        _process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                _logger.LogInformation(e.Data);
            }
        };

        _process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                _logger.LogError(e.Data);
            }
        };

        _process.Exited += HandleProcessExit;

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _logger.LogInformation("Started NPM Watch");
    }

    private async void HandleProcessExit(object? sender, object args)
    {
        _process!.Dispose();
        _process = null;

        _logger.LogWarning("NPM Watch exited, restarting in 1 second...");

        await Task.Delay(1000);
        StartProcess();
    }
}
#endif
