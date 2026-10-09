using Microsoft.AspNetCore.Mvc;
using AIService.Contracts;
using AIService.Services;

namespace AIService.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("analyze-sentiment")]
        public IActionResult AnalyzeSentiment([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Text))
                return BadRequest("Text is required.");

            var result = _aiService.AnalyzeSentiment(request);
            return Ok(result);
        }

        [HttpPost("summarize-review")]
        public IActionResult SummarizeReview([FromBody] SummaryRequest request)
        {
            var result = _aiService.SummarizeReview(request);
            return Ok(result);
        }

        [HttpPost("recommend-movies")]
        public IActionResult RecommendMovies([FromBody] RecommendationRequest request)
        {
            var result = _aiService.RecommendMovies(request);
            return Ok(result);
        }

        [HttpPost("chat")]
        public IActionResult Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return BadRequest("Message is required.");

            var result = _aiService.Chat(request);
            return Ok(result);
        }
    }
}
