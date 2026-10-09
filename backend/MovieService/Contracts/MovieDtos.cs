namespace MovieService.Contracts
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Trailer { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Director { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }

    public class UpdateMovieDto : CreateMovieDto
    {
    }
}
