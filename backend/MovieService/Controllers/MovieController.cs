using Microsoft.AspNetCore.Mvc;
using MovieService.Contracts;
using MovieService.Services;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok(_movieService.GetAllMovies());
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound(new { message = "Không tìm thấy phim." });
            return Ok(movie);
        }

        [HttpGet("search")]
        public IActionResult SearchMovies([FromQuery] string? title, [FromQuery] string? genre)
        {
            return Ok(_movieService.SearchMovies(title, genre));
        }

        [HttpPost]
        public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
        {
            var created = _movieService.CreateMovie(movieDto);
            return CreatedAtAction(nameof(GetMovieById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
        {
            var updated = _movieService.UpdateMovie(id, movieDto);
            if (updated == null) return NotFound(new { message = "Không tìm thấy phim cần cập nhật." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var success = _movieService.DeleteMovie(id);
            if (!success) return NotFound(new { message = "Không tìm thấy phim cần xóa." });
            return Ok(new { message = "Đã xóa phim thành công." });
        }
    }
}
