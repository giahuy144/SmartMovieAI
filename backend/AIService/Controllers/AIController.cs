using Microsoft.AspNetCore.Mvc;

namespace AIService.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        [HttpPost("analyze-sentiment")]
        public IActionResult AnalyzeSentiment([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Text))
                return BadRequest("Text is required.");

            string textLower = request.Text.ToLower();
            string sentiment = "Neutral";
            double confidence = 0.85;

            if (textLower.Contains("hay") || textLower.Contains("xuất sắc") || textLower.Contains("tuyệt vời") || textLower.Contains("thích") || textLower.Contains("good") || textLower.Contains("awesome") || textLower.Contains("đẹp"))
            {
                sentiment = "Positive";
                confidence = 0.95;
            }
            else if (textLower.Contains("dở") || textLower.Contains("tệ") || textLower.Contains("chán") || textLower.Contains("bad") || textLower.Contains("khó hiểu") || textLower.Contains("tốn thời gian"))
            {
                sentiment = "Negative";
                confidence = 0.91;
            }

            return Ok(new
            {
                text = request.Text,
                sentiment = sentiment,
                confidence = confidence,
                analyzedAt = DateTime.UtcNow
            });
        }

        [HttpPost("summarize-review")]
        public IActionResult SummarizeReview([FromBody] SummaryRequest request)
        {
            if (request?.Reviews == null || request.Reviews.Count == 0)
            {
                return Ok(new
                {
                    movieId = request?.MovieId ?? 0,
                    summary = "Chưa có đủ nhận xét để tóm tắt.",
                    pros = new List<string>(),
                    cons = new List<string>()
                });
            }

            return Ok(new
            {
                movieId = request.MovieId,
                totalReviews = request.Reviews.Count,
                summary = "Phim nhận được phản hồi tích cực từ khán giả về hình ảnh sống động và kỹ xảo sắc nét, tuy nhiên thời lượng có phần hơi kéo dài.",
                pros = new List<string>
                {
                    "Nội dung hấp dẫn và giàu cảm xúc",
                    "Kỹ xảo hình ảnh và âm thanh đỉnh cao",
                    "Diễn xuất của dàn nhân vật chính ấn tượng"
                },
                cons = new List<string>
                {
                    "Thời lượng phim tương đối dài (~3 tiếng)",
                    "Một số đoạn tình tiết diễn biến hơi chậm"
                }
            });
        }

        [HttpPost("recommend-movies")]
        public IActionResult RecommendMovies([FromBody] RecommendationRequest request)
        {
            var recommendedMovies = new List<object>
            {
                new { id = 1, title = "Interstellar", genre = "Sci-Fi", matchScore = 0.98, reason = "Phù hợp với sở thích phim Khoa học viễn tưởng & khám phá vũ trụ của bạn" },
                new { id = 2, title = "Inception", genre = "Sci-Fi / Action", matchScore = 0.94, reason = "Có cùng đạo diễn Christopher Nolan và cốt truyện giật gân" },
                new { id = 3, title = "The Dark Knight", genre = "Action", matchScore = 0.91, reason = "Phim có điểm rating cực cao từ cộng đồng người dùng tương đồng" }
            };

            return Ok(new
            {
                userId = request?.UserId ?? 1,
                favoriteGenre = request?.FavoriteGenre ?? "Sci-Fi",
                recommendations = recommendedMovies
            });
        }

        [HttpPost("chat")]
        public IActionResult Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return BadRequest("Message is required.");

            string msg = request.Message.ToLower();
            string reply;

            if (msg.Contains("interstellar") || msg.Contains("khoa học viễn tưởng") || msg.Contains("sci-fi"))
            {
                reply = "Nếu bạn thích thể loại Khoa học viễn tưởng như Interstellar, tôi đề xuất các phim: Inception, Tenet, The Martian, và Dune!";
            }
            else if (msg.Contains("hành động") || msg.Contains("action"))
            {
                reply = "Dưới đây là một số phim Hành động đỉnh cao: John Wick 4, Mad Max: Fury Road, và The Dark Knight!";
            }
            else if (msg.Contains("dưới 2 tiếng") || msg.Contains("ngắn"))
            {
                reply = "Các phim hay có thời lượng dưới 2 tiếng: Whiplash (1h 47m), Spider-Man: Into the Spider-Verse (1h 57m), A Quiet Place (1h 30m).";
            }
            else
            {
                reply = $"Chào bạn! Smart Movie AI RAG Assistant rất vui được hỗ trợ. Bạn có thể hỏi tôi về các thể loại phim, phim tương tự hoặc yêu cầu gợi ý phim theo thời lượng!";
            }

            return Ok(new
            {
                userMessage = request.Message,
                botReply = reply,
                timestamp = DateTime.UtcNow
            });
        }
    }

    public class SentimentRequest
    {
        public string Text { get; set; } = string.Empty;
    }

    public class SummaryRequest
    {
        public int MovieId { get; set; }
        public List<string> Reviews { get; set; } = new();
    }

    public class RecommendationRequest
    {
        public int UserId { get; set; }
        public string FavoriteGenre { get; set; } = string.Empty;
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
