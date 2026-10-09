using AIService.Contracts;

namespace AIService.Services
{
    public interface IAIService
    {
        SentimentResponse AnalyzeSentiment(SentimentRequest request);
        SummaryResponse SummarizeReview(SummaryRequest request);
        RecommendationResponse RecommendMovies(RecommendationRequest request);
        ChatResponse Chat(ChatRequest request);
    }
}
