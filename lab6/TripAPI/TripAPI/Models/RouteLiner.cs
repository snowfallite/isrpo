

namespace CruiseCompany.Models;

public class RouteLiner
{
    public int RouteId { get; set; }
    public CruiseRoute Route { get; set; } = null!;
    public int LinerId { get; set; }
    public Liner Liner { get; set; } = null!;
    public string? Season { get; set; }
    public decimal? BasePrice { get; set; }
}
