using RatingService.Contracts;
using RatingService.Models;

namespace RatingService.Services
{
    public interface IRatingService
    {
        Rating AddOrUpdateRating(RatingDto dto);
        MovieRatingSummaryDto GetMovieRating(int movieId);
        MovieRatingStatisticsDto GetMovieRatingStatistics(int movieId);
    }
}
