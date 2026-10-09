using MovieService.Contracts;
using MovieService.Models;

namespace MovieService.Services
{
    public interface IMovieService
    {
        IEnumerable<Movie> GetAllMovies();
        Movie? GetMovieById(int id);
        IEnumerable<Movie> SearchMovies(string? title, string? genre);
        Movie CreateMovie(CreateMovieDto dto);
        Movie? UpdateMovie(int id, UpdateMovieDto dto);
        bool DeleteMovie(int id);
    }
}
