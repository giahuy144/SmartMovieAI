using Microsoft.AspNetCore.Mvc;
using ReviewService.Contracts;
using ReviewService.Services;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] CreateReviewDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            var review = _reviewService.CreateReview(dto);
            return Ok(review);
        }

        [HttpGet("movie/{movieId}")]
        public IActionResult GetReviewsByMovie(int movieId)
        {
            return Ok(_reviewService.GetReviewsByMovie(movieId));
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetReviewsByUser(int userId)
        {
            return Ok(_reviewService.GetReviewsByUser(userId));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var success = _reviewService.DeleteReview(id);
            if (!success) return NotFound();

            return Ok(new { message = "Đã xóa review." });
        }
    }
}
