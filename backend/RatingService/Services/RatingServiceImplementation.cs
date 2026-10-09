using RatingService.Contracts;
using RatingService.Models;

namespace RatingService.Services
{
    public class RatingServiceImplementation : IRatingService
    {
        private static readonly List<Rating> Ratings = new()
        {
            new Rating { Id = 1, UserId = 1, MovieId = 1, Score = 5 },
            new Rating { Id = 2, UserId = 2, MovieId = 1, Score = 4 },
            new Rating { Id = 3, UserId = 3, MovieId = 1, Score = 5 },
            new Rating { Id = 4, UserId = 1, MovieId = 2, Score = 5 },
            new Rating { Id = 5, UserId = 2, MovieId = 2, Score = 4 }
        };

        public Rating AddOrUpdateRating(RatingDto dto)
        {
            var existing = Ratings.FirstOrDefault(r => r.UserId == dto.UserId && r.MovieId == dto.MovieId);
            if (existing != null)
            {
                existing.Score = dto.Score;
                existing.CreatedAt = DateTime.UtcNow;
                return existing;
            }

            var rating = new Rating
            {
                Id = Ratings.Count > 0 ? Ratings.Max(r => r.Id) + 1 : 1,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                Score = dto.Score,
                CreatedAt = DateTime.UtcNow
            };
            Ratings.Add(rating);
            return rating;
        }

        public MovieRatingSummaryDto GetMovieRating(int movieId)
        {
            var movieRatings = Ratings.Where(r => r.MovieId == movieId).ToList();
            double avg = movieRatings.Any() ? movieRatings.Average(r => r.Score) : 0.0;

            return new MovieRatingSummaryDto
            {
                MovieId = movieId,
                AverageRating = Math.Round(avg, 1),
                TotalRatings = movieRatings.Count
            };
        }

        public MovieRatingStatisticsDto GetMovieRatingStatistics(int movieId)
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

            return new MovieRatingStatisticsDto
            {
                MovieId = movieId,
                AverageScore = Math.Round(avg, 1),
                TotalCount = movieRatings.Count,
                StarsBreakdown = breakdown
            };
        }
    }
}
