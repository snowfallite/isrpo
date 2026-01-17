namespace TripAPI.DTOs
{
    public class RouteLinerCreateDto
    {
        public int RouteId { get; set; }
        public int LinerId { get; set; }
        public string? Season { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
