using Domain;
using Services.Interfaces.Repositories;

namespace DataAccess;

public class InMemoryMovieRepository : IMovieRepository
{
    private List<Movie> Movies { get; }

    public InMemoryMovieRepository()
    {
        Movies = new List<Movie>();
        LoadDefaultMovies();
    }

    public List<Movie> GetMovies()
    {
        return Movies;
    }

    public Movie? GetMovie(Func<Movie, bool> filter)
    {
        return Movies.Where(filter).FirstOrDefault();
    }

    public void AddMovie(Movie movie)
    {
        Movies.Add(movie);
    }

    public void DeleteMovie(Movie movie)
    {
        Movies.Remove(movie);
    }

    public void UpdateMovie(Movie movieToUpdate)
    {
        Movie? movie = Movies.Find(m => m.Title == movieToUpdate.Title);
        var movieToUpdateIndex = Movies.IndexOf(movie);
        Movies[movieToUpdateIndex] = movieToUpdate;
    }

    private void LoadDefaultMovies()
    {
        Movies.Add(new Movie("Cast Away", "Robert Zemeckis", new DateTime(2000, 12, 22), 200000));
    }
}