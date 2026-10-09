using Microsoft.AspNetCore.Mvc;
using RatingService.Contracts;
using RatingService.Services;

namespace RatingService.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public IActionResult AddOrUpdateRating([FromBody] RatingDto dto)
        {
            if (dto.Score < 1 || dto.Score > 5)
                return BadRequest("Score must be between 1 and 5.");

            var rating = _ratingService.AddOrUpdateRating(dto);
            return Ok(rating);
        }

        [HttpGet("movie/{movieId}")]
        public IActionResult GetMovieRating(int movieId)
        {
            var result = _ratingService.GetMovieRating(movieId);
            return Ok(result);
        }

        [HttpGet("movie/{movieId}/statistics")]
        public IActionResult GetMovieRatingStatistics(int movieId)
        {
            var result = _ratingService.GetMovieRatingStatistics(movieId);
            return Ok(result);
        }
    }
}
