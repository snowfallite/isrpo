
namespace CruiseCompany.Models;

public class CruiseRoute
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }

    public ICollection<RouteLiner> RouteLiners { get; set; } = new List<RouteLiner>();
}
