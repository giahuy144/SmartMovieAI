namespace RatingService.Contracts
{
    public class RatingDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; }
    }

    public class MovieRatingSummaryDto
    {
        public int MovieId { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }

    public class MovieRatingStatisticsDto
    {
        public int MovieId { get; set; }
        public double AverageScore { get; set; }
        public int TotalCount { get; set; }
        public Dictionary<int, int> StarsBreakdown { get; set; } = new();
    }
}
