namespace BlazorApp.Models;

public enum SensorType
{
    Temp,
    Speed,
    Torque
}

public class Sensor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public SensorType Type { get; set; }

    // Latest value updated every tick
    public double CurrentValue { get; set; }

    // Recording toggle and history buffer for plotting
    public bool IsRecording { get; set; }
    public int MaxHistory { get; set; } = 200;
    public List<(DateTime Timestamp, double Value)> History { get; } = new();
}
