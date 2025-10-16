using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models;

public class IndustrialInstallation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public Guid? PreviousInstallationId { get; set; }

    public List<Sensor> Sensors { get; set; } = new();
}
