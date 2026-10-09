using MovieService.Contracts;
using MovieService.Models;

namespace MovieService.Services
{
    public class MovieServiceImplementation : IMovieService
    {
        private static readonly List<Movie> Movies = new()
        {
            new Movie
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
            new Movie
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
            new Movie
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
            new Movie
            {
                Id = 4,
                Title = "Avatar: The Way of Water",
                Description = "Jake Sully cùng gia đình Na'vi bảo vệ hành tinh Pandora trước cuộc xâm lược mới.",
                Poster = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=600&auto=format&fit=crop",
                Trailer = "https://www.youtube.com/watch?v=d9MyW72ELq0",
                ReleaseDate = "2022-12-16",
                Duration = 192,
                Director = "James Cameron",
                Genre = "Sci-Fi / Adventure"
            }
        };

        public IEnumerable<Movie> GetAllMovies() => Movies;

        public Movie? GetMovieById(int id) => Movies.FirstOrDefault(m => m.Id == id);

        public IEnumerable<Movie> SearchMovies(string? title, string? genre)
        {
            var query = Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));

            return query.ToList();
        }

        public Movie CreateMovie(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Id = Movies.Count > 0 ? Movies.Max(m => m.Id) + 1 : 1,
                Title = dto.Title,
                Description = dto.Description,
                Poster = dto.Poster,
                Trailer = dto.Trailer,
                ReleaseDate = dto.ReleaseDate,
                Duration = dto.Duration,
                Director = dto.Director,
                Genre = dto.Genre,
                CreatedAt = DateTime.UtcNow
            };
            Movies.Add(movie);
            return movie;
        }

        public Movie? UpdateMovie(int id, UpdateMovieDto dto)
        {
            var existing = Movies.FirstOrDefault(m => m.Id == id);
            if (existing == null) return null;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Director = dto.Director;
            existing.Genre = dto.Genre;
            existing.Duration = dto.Duration;
            existing.Poster = dto.Poster;
            existing.Trailer = dto.Trailer;
            existing.ReleaseDate = dto.ReleaseDate;

            return existing;
        }

        public bool DeleteMovie(int id)
        {
            var existing = Movies.FirstOrDefault(m => m.Id == id);
            if (existing == null) return false;

            Movies.Remove(existing);
            return true;
        }
    }
}
