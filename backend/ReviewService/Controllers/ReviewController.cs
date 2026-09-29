using Microsoft.AspNetCore.Mvc;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private static readonly List<ReviewModel> Reviews = new()
        {
            new ReviewModel { Id = 1, UserId = 1, MovieId = 1, Content = "Phim rất hay, hình ảnh đẹp và diễn xuất tuyệt vời.", Rating = 5, Sentiment = "Positive", CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new ReviewModel { Id = 2, UserId = 2, MovieId = 1, Content = "Phim có kỹ xảo hoành tráng nhưng một số đoạn hơi khó hiểu.", Rating = 4, Sentiment = "Neutral", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new ReviewModel { Id = 3, UserId = 1, MovieId = 2, Content = "Ý tưởng giấc mơ trong giấc mơ cực kỳ sáng tạo và lôi cuốn!", Rating = 5, Sentiment = "Positive", CreatedAt = DateTime.UtcNow.AddHours(-10) }
        };

        [HttpPost]
        public IActionResult CreateReview([FromBody] CreateReviewDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            // Analyze sentiment simple rule (simulating call to AI Service)
            string textLower = dto.Content.ToLower();
            string sentiment = "Neutral";
            if (textLower.Contains("hay") || textLower.Contains("tuyệt vời") || textLower.Contains("xuất sắc") || textLower.Contains("thích") || textLower.Contains("đẹp"))
                sentiment = "Positive";
            else if (textLower.Contains("dở") || textLower.Contains("tệ") || textLower.Contains("chán") || textLower.Contains("khó hiểu"))
                sentiment = "Negative";

            var review = new ReviewModel
            {
                Id = Reviews.Count + 1,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                Content = dto.Content,
                Rating = dto.Rating,
                Sentiment = sentiment,
                CreatedAt = DateTime.UtcNow
            };

            Reviews.Add(review);
            return Ok(review);
        }

        [HttpGet("movie/{movieId}")]
        public IActionResult GetReviewsByMovie(int movieId)
        {
            var movieReviews = Reviews.Where(r => r.MovieId == movieId).OrderByDescending(r => r.CreatedAt).ToList();
            return Ok(movieReviews);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetReviewsByUser(int userId)
        {
            var userReviews = Reviews.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedAt).ToList();
            return Ok(userReviews);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null) return NotFound();

            Reviews.Remove(review);
            return Ok(new { message = "Đã xóa review." });
        }
    }

    public class ReviewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Sentiment { get; set; } = "Neutral";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CreateReviewDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
    }
}
