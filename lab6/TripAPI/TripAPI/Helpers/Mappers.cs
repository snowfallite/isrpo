using CruiseCompany.Models;
using CruiseCompany.DTOs;

namespace CruiseCompany.Helpers;

public static class Mappers
{
    public static RouteDto ToDto(this CruiseRoute r) => new RouteDto
    {
        Id = r.Id,
        Name = r.Name,
        DurationDays = r.DurationDays,
        Liners = r.RouteLiners.Select(rl => new LinerBriefDto
        {
            Id = rl.Liner.Id,
            Name = rl.Liner.Name
        }).ToList()
    };



    public static LinerDto ToDto(this Liner l) => new LinerDto
    {
        Id = l.Id,
        Name = l.Name,
        Capacity = l.Capacity,
        Class = l.Class,
        YearBuilt = l.YearBuilt,
        Routes = l.RouteLiners.Select(rl => new RouteBriefDto
        {
            Id = rl.Route.Id,
            Name = rl.Route.Name
        }).ToList()
    };
}
