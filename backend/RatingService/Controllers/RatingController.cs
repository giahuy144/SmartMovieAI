using Microsoft.AspNetCore.Mvc;

namespace RatingService.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingController : ControllerBase
    {
        private static readonly List<RatingModel> Ratings = new()
        {
            new RatingModel { Id = 1, UserId = 1, MovieId = 1, Score = 5 },
            new RatingModel { Id = 2, UserId = 2, MovieId = 1, Score = 4 },
            new RatingModel { Id = 3, UserId = 3, MovieId = 1, Score = 5 },
            new RatingModel { Id = 4, UserId = 1, MovieId = 2, Score = 5 },
            new RatingModel { Id = 5, UserId = 2, MovieId = 2, Score = 4 }
        };

        [HttpPost]
        public IActionResult AddOrUpdateRating([FromBody] RatingModel dto)
        {
            if (dto.Score < 1 || dto.Score > 5)
                return BadRequest("Score must be between 1 and 5.");

            var existing = Ratings.FirstOrDefault(r => r.UserId == dto.UserId && r.MovieId == dto.MovieId);
            if (existing != null)
            {
                existing.Score = dto.Score;
                existing.CreatedAt = DateTime.UtcNow;
                return Ok(existing);
            }

            dto.Id = Ratings.Count + 1;
            dto.CreatedAt = DateTime.UtcNow;
            Ratings.Add(dto);
            return Ok(dto);
        }

        [HttpGet("movie/{movieId}")]
        public IActionResult GetMovieRating(int movieId)
        {
            var movieRatings = Ratings.Where(r => r.MovieId == movieId).ToList();
            double avg = movieRatings.Any() ? movieRatings.Average(r => r.Score) : 0.0;

            return Ok(new
            {
                movieId = movieId,
                averageRating = Math.Round(avg, 1),
                totalRatings = movieRatings.Count
            });
        }

        [HttpGet("movie/{movieId}/statistics")]
        public IActionResult GetMovieRatingStatistics(int movieId)
        {
            var movieRatings = Ratings.Where(r => r.MovieId == movieId).ToList();
            double avg = movieRatings.Any() ? movieRatings.Average(r => r.Score) : 0.0;

            var breakdown = new Dictionary<int, int>
            {
                { 5, movieRatings.Count(r => r.Score == 5) },
                { 4, movieRatings.Count(r => r.Score == 4) },
                { 3, movieRatings.Count(r => r.Score == 3) },
                { 2, movieRatings.Count(r => r.Score == 2) },
                { 1, movieRatings.Count(r => r.Score == 1) }
            };

            return Ok(new
            {
                movieId = movieId,
                averageScore = Math.Round(avg, 1),
                totalCount = movieRatings.Count,
                starsBreakdown = breakdown
            });
        }
    }

    public class RatingModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
