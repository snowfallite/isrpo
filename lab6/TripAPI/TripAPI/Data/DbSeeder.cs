using CruiseCompany.Models;

namespace CruiseCompany.Data;

public static class DbSeeder
{
    public static void Seed(CruiseDbContext db)
    {
        if (db.CruiseRoutes.Any() || db.Liners.Any()) return; // если уже есть данные — выходим

        var r1 = new CruiseRoute { Name = "Балтийский круг", DurationDays = 7 };
        var r2 = new CruiseRoute { Name = "Средиземноморье", DurationDays = 10 };
        var r3 = new CruiseRoute { Name = "Северные фьорды", DurationDays = 8 };

        var l1 = new Liner { Name = "Aurora", Capacity = 2000, Class = "Premium", YearBuilt = 2015 };
        var l2 = new Liner { Name = "Baltic Star", Capacity = 1500, Class = "Standard", YearBuilt = 2010 };
        var l3 = new Liner { Name = "Nordic Pearl", Capacity = 1800, Class = "Luxury", YearBuilt = 2018 };

        db.CruiseRoutes.AddRange(r1, r2, r3);
        db.Liners.AddRange(l1, l2, l3);
        db.SaveChanges();

        db.RouteLiners.AddRange(
            new RouteLiner { RouteId = r1.Id, LinerId = l1.Id, Season = "Лето", BasePrice = 899m },
            new RouteLiner { RouteId = r2.Id, LinerId = l2.Id, Season = "Весна", BasePrice = 699m },
            new RouteLiner { RouteId = r3.Id, LinerId = l3.Id, Season = "Осень", BasePrice = 999m },
            new RouteLiner { RouteId = r1.Id, LinerId = l2.Id, Season = "Зима", BasePrice = 499m }
        );
        db.SaveChanges();
    }
}
