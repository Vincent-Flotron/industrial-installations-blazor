using BlazorApp.Models;

namespace BlazorApp.Services;

public class InstallationService
{
    private readonly List<IndustrialInstallation> _installations = new();

    public event Action? Updated;

    public IReadOnlyList<IndustrialInstallation> Installations => _installations;

    public void NotifyUpdated()
    {
        Updated?.Invoke();
    }

    public IndustrialInstallation CreateInstallation(string name, string? location = null, Guid? previousId = null)
    {
        var inst = new IndustrialInstallation
        {
            Name = name,
            Location = location,
            PreviousInstallationId = previousId
        };
        _installations.Add(inst);
        NotifyUpdated();
        return inst;
    }

    public void RemoveInstallation(Guid id)
    {
        var inst = _installations.FirstOrDefault(i => i.Id == id);
        if (inst is null) return;
        _installations.Remove(inst);
        // Clear previous links referencing this one
        foreach (var i in _installations.Where(x => x.PreviousInstallationId == id))
        {
            i.PreviousInstallationId = null;
        }
        NotifyUpdated();
    }

    public void AddSensor(Guid installationId, string name, SensorType type)
    {
        var inst = _installations.FirstOrDefault(i => i.Id == installationId);
        if (inst is null) return;
        inst.Sensors.Add(new Sensor { Name = name, Type = type });
        NotifyUpdated();
    }

    public void RemoveSensor(Guid installationId, Guid sensorId)
    {
        var inst = _installations.FirstOrDefault(i => i.Id == installationId);
        if (inst is null) return;
        var s = inst.Sensors.FirstOrDefault(s => s.Id == sensorId);
        if (s is null) return;
        inst.Sensors.Remove(s);
        NotifyUpdated();
    }
}
