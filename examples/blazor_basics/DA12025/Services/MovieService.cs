using Domain;
using DataAccess;
using Services.Interfaces;

namespace Services
{
    public class MovieService : IMovieService
    {
        private readonly InMemoryMovieRepository _movieRepository;

        public MovieService(InMemoryMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void AddMovie(Movie movie)
        {
            ValidateUniqueTitle(movie.Title);
            _movieRepository.AddMovie(movie);
        }

        public List<Movie> GetMovies()
        {
            return _movieRepository.GetMovies();
        }

        public Movie GetMovie(string title)
        {
            Movie? movie = _movieRepository.GetMovie(title);
            if (movie == null)
            {
                throw new ArgumentException("Cannot find movie with this title");
            }

            return movie;
        }

        private void ValidateUniqueTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            string inputTitle = title.Trim();

            foreach (var movie in _movieRepository.GetMovies())
            {
                if (string.Equals(movie.Title?.Trim(), inputTitle, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("A movie with this title already exists.");
                }
            }
        }
    }
}