using BlazorApp.Models;

namespace BlazorApp.Services;

public class SensorUpdateService : IHostedService, IDisposable
{
    private readonly InstallationService _installationService;
    private readonly System.Timers.Timer _timer;
    private readonly Random _random = new();

    public event Action? Ticked;

    public SensorUpdateService(InstallationService installationService)
    {
        _installationService = installationService;
        _timer = new System.Timers.Timer(2000);
        _timer.Elapsed += OnTick;
        _timer.AutoReset = true;
    }

    private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        foreach (var inst in _installationService.Installations)
        {
            foreach (var sensor in inst.Sensors)
            {
                sensor.CurrentValue = GenerateValue(sensor.Type, sensor.CurrentValue);
                if (sensor.IsRecording)
                {
                    sensor.History.Add((DateTime.UtcNow, sensor.CurrentValue));
                    // cap history
                    var overflow = sensor.History.Count - sensor.MaxHistory;
                    if (overflow > 0)
                    {
                        sensor.History.RemoveRange(0, overflow);
                    }
                }
            }
        }
        _installationService.NotifyUpdated();
        Ticked?.Invoke();
    }

    private double GenerateValue(SensorType type, double prev)
    {
        // Basic random walk within reasonable ranges
        double step = (_random.NextDouble() - 0.5) * 2; // -1..1
        return type switch
        {
            SensorType.Temp => Clamp(prev == 0 ? 20 + _random.Next(-5, 6) : prev + step, -20, 80),
            SensorType.Speed => Clamp(prev == 0 ? _random.Next(0, 100) : prev + step * 5, 0, 300),
            SensorType.Torque => Clamp(prev == 0 ? _random.Next(0, 50) : prev + step * 2, 0, 500),
            _ => prev + step
        };
    }

    private static double Clamp(double v, double min, double max) => Math.Max(min, Math.Min(max, v));

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
