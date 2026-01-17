namespace CruiseCompany.Models;

public class Liner
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public string Class { get; set; } = null!;
    public int YearBuilt { get; set; }

    public ICollection<RouteLiner> RouteLiners { get; set; } = new List<RouteLiner>();
}
