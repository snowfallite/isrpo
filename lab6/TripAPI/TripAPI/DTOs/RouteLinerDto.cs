namespace TripAPI.DTOs
{
    public class RouteLinerDto
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; } = null!;
        public int LinerId { get; set; }
        public string LinerName { get; set; } = null!;
        public string? Season { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
