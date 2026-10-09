namespace ReviewService.Contracts
{
    public class CreateReviewDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
    }
}
