namespace ReviewService.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Sentiment { get; set; } = "Neutral";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
