using Microsoft.AspNetCore.Mvc;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private static readonly List<MovieModel> Movies = new()
        {
            new MovieModel
            {
                Id = 1,
                Title = "Interstellar",
                Description = "Khi Trái Đất dần trở nên không thể sống được, một nhóm nhà hành tinh học du hành qua lỗ sâu để tìm kiếm ngôi nhà mới cho nhân loại.",
                Poster = "https://images.unsplash.com/photo-1534447677768-be436bb09401?w=600&auto=format&fit=crop",
                Trailer = "https://www.youtube.com/watch?v=zSWdZVtXT7E",
                ReleaseDate = "2014-11-07",
                Duration = 169,
                Director = "Christopher Nolan",
                Genre = "Sci-Fi"
            },
            new MovieModel
            {
                Id = 2,
                Title = "Inception",
                Description = "Kẻ trộm tài ba chuyên đột nhập vào giấc mơ của người khác để đánh cắp bí mật kinh doanh.",
                Poster = "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=600&auto=format&fit=crop",
                Trailer = "https://www.youtube.com/watch?v=YoHD9XEInc0",
                ReleaseDate = "2010-07-16",
                Duration = 148,
                Director = "Christopher Nolan",
                Genre = "Sci-Fi / Action"
            },
            new MovieModel
            {
                Id = 3,
                Title = "The Dark Knight",
                Description = "Batman đối đầu với Joker - kẻ hỗn loạn đe dọa biến thành phố Gotham thành bình địa.",
                Poster = "https://images.unsplash.com/photo-1509198397868-475647b2a1e5?w=600&auto=format&fit=crop",
                Trailer = "https://www.youtube.com/watch?v=EXeTwQWrcwY",
                ReleaseDate = "2008-07-18",
                Duration = 152,
                Director = "Christopher Nolan",
                Genre = "Action"
            },
            new MovieModel
            {
                Id = 4,
                Title = "Avatar: The Way of Water",
                Description = "Jake Sully cùng gia đình Na'vi bảo vệ hành tinh Pandora trước cuộc xâm lược mới.",
                Poster = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=600&auto=format&fit=crop",
                Trailer = "https://www.youtube.com/watch?dqv1=d9MyW72ELq0",
                ReleaseDate = "2022-12-16",
                Duration = 192,
                Director = "James Cameron",
                Genre = "Sci-Fi / Adventure"
            }
        };

        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok(Movies);
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound(new { message = "Không tìm thấy phim." });
            return Ok(movie);
        }

        [HttpGet("search")]
        public IActionResult SearchMovies([FromQuery] string? title, [FromQuery] string? genre)
        {
            var query = Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));

            return Ok(query.ToList());
        }

        [HttpPost]
        public IActionResult AddMovie([FromBody] MovieModel movie)
        {
            movie.Id = Movies.Count + 1;
            movie.CreatedAt = DateTime.UtcNow;
            Movies.Add(movie);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] MovieModel movie)
        {
            var existing = Movies.FirstOrDefault(m => m.Id == id);
            if (existing == null) return NotFound();

            existing.Title = movie.Title;
            existing.Description = movie.Description;
            existing.Director = movie.Director;
            existing.Genre = movie.Genre;
            existing.Duration = movie.Duration;
            existing.Poster = movie.Poster;
            existing.Trailer = movie.Trailer;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var existing = Movies.FirstOrDefault(m => m.Id == id);
            if (existing == null) return NotFound();

            Movies.Remove(existing);
            return Ok(new { message = "Đã xóa phim thành công." });
        }
    }

    public class MovieModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Trailer { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Director { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
