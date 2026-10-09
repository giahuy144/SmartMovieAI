namespace AIService.Contracts
{
    public class SentimentRequest
    {
        public string Text { get; set; } = string.Empty;
    }

    public class SentimentResponse
    {
        public string Text { get; set; } = string.Empty;
        public string Sentiment { get; set; } = "Neutral";
        public double Confidence { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }

    public class SummaryRequest
    {
        public int MovieId { get; set; }
        public List<string> Reviews { get; set; } = new();
    }

    public class SummaryResponse
    {
        public int MovieId { get; set; }
        public int TotalReviews { get; set; }
        public string Summary { get; set; } = string.Empty;
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
    }

    public class RecommendationRequest
    {
        public int UserId { get; set; }
        public string FavoriteGenre { get; set; } = string.Empty;
    }

    public class MovieRecommendationItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public double MatchScore { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class RecommendationResponse
    {
        public int UserId { get; set; }
        public string FavoriteGenre { get; set; } = string.Empty;
        public List<MovieRecommendationItem> Recommendations { get; set; } = new();
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public string UserMessage { get; set; } = string.Empty;
        public string BotReply { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
