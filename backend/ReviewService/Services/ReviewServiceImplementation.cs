using ReviewService.Contracts;
using ReviewService.Models;

namespace ReviewService.Services
{
    public class ReviewServiceImplementation : IReviewService
    {
        private static readonly List<Review> Reviews = new()
        {
            new Review { Id = 1, UserId = 1, MovieId = 1, Content = "Phim rất hay, hình ảnh đẹp và diễn xuất tuyệt vời.", Rating = 5, Sentiment = "Positive", CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Review { Id = 2, UserId = 2, MovieId = 1, Content = "Phim có kỹ xảo hoành tráng nhưng một số đoạn hơi khó hiểu.", Rating = 4, Sentiment = "Neutral", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new Review { Id = 3, UserId = 1, MovieId = 2, Content = "Ý tưởng giấc mơ trong giấc mơ cực kỳ sáng tạo và lôi cuốn!", Rating = 5, Sentiment = "Positive", CreatedAt = DateTime.UtcNow.AddHours(-10) }
        };

        public Review CreateReview(CreateReviewDto dto)
        {
            // Simple rule-based sentiment detection
            string textLower = dto.Content.ToLower();
            string sentiment = "Neutral";
            if (textLower.Contains("hay") || textLower.Contains("tuyệt vời") || textLower.Contains("xuất sắc") || textLower.Contains("thích") || textLower.Contains("đẹp"))
                sentiment = "Positive";
            else if (textLower.Contains("dở") || textLower.Contains("tệ") || textLower.Contains("chán") || textLower.Contains("khó hiểu"))
                sentiment = "Negative";

            var review = new Review
            {
                Id = Reviews.Count > 0 ? Reviews.Max(r => r.Id) + 1 : 1,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                Content = dto.Content,
                Rating = dto.Rating,
                Sentiment = sentiment,
                CreatedAt = DateTime.UtcNow
            };

            Reviews.Add(review);
            return review;
        }

        public IEnumerable<Review> GetReviewsByMovie(int movieId)
        {
            return Reviews.Where(r => r.MovieId == movieId).OrderByDescending(r => r.CreatedAt).ToList();
        }

        public IEnumerable<Review> GetReviewsByUser(int userId)
        {
            return Reviews.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedAt).ToList();
        }

        public bool DeleteReview(int id)
        {
            var review = Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null) return false;

            Reviews.Remove(review);
            return true;
        }
    }
}
