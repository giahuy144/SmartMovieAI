using ReviewService.Contracts;
using ReviewService.Models;

namespace ReviewService.Services
{
    public interface IReviewService
    {
        Review CreateReview(CreateReviewDto dto);
        IEnumerable<Review> GetReviewsByMovie(int movieId);
        IEnumerable<Review> GetReviewsByUser(int userId);
        bool DeleteReview(int id);
    }
}
