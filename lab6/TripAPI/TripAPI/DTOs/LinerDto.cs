using System.Text.Json.Serialization;

namespace CruiseCompany.DTOs;

public class LinerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }

    [JsonPropertyName("_class")]
    public string Class { get; set; } = null!;
    public int YearBuilt { get; set; }
    public List<RouteBriefDto> Routes { get; set; } = new();
}
