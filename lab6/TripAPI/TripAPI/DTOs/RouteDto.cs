namespace CruiseCompany.DTOs;

public class RouteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }
    public List<LinerBriefDto> Liners { get; set; } = new();
}
