using Domain;

namespace DataAccess.Tests;

[TestClass]
public class MovieRepositoryTests
{
    private AppDbContext _context;
    private InMemoryAppContextFactory _contextFactory;
    private MovieRepository _movieRepository;
    private Movie _movie;
    private Movie _movie_two;

    [TestInitialize]
    public void SetUp()
    {
        _contextFactory = new InMemoryAppContextFactory();
        _context = _contextFactory.CreateDbContext();
        _movieRepository = new MovieRepository(_context);
        _movie = new Movie(1, "Meet Joe Black", "Martin Brest", new DateTime(1998, 11, 2), 90000000);
        _movie_two = new Movie(2, "Vanilla Sky", "Cameron Crowe", new DateTime(2001, 12, 10), 68000000);
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WhenAddIsInvokedWithAnEmptyMovie_ThenThrowException()
    {
        // arrange
        // act & assert
        Assert.ThrowsException<Exception>(() =>
        {
            _movieRepository.AddMovie(new Movie());
        });
    }

    [TestMethod]
    public void Add_WhenAddIsInvoked_ThenTheMovieIsAdded()
    {
        //arrange
        //act
        _movieRepository.AddMovie(_movie);
        //assert
        List<Movie> movies = _movieRepository.GetMovies();
        Assert.AreEqual(1, movies.Count);
        Assert.AreEqual("Meet Joe Black", movies[0].Title);
    }

    [TestMethod]
    public void GetAll_WhenGetAllIsInvoked_ThenAllMoviesAreReturned()
    {
        //arrange
        _movieRepository.AddMovie(_movie);
        _movieRepository.AddMovie(_movie_two);
        //act
        List<Movie> movies = _movieRepository.GetMovies();
        //assert
        Assert.AreEqual(2, movies.Count);
    }

    [TestMethod]
    public void Get_WhenGetIsInvoked_ThenTheMovieIsReturned()
    {
        //arrange
        _movieRepository.AddMovie(_movie);
        //act
        Movie? result = _movieRepository.GetMovie(m => m.Id == 1);
        //assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Meet Joe Black", result.Title);
        Assert.AreEqual("Martin Brest", result.Director);
        Assert.AreEqual(90000000, result.Budget);
    }

    [TestMethod]
    public void Delete_WhenDeleteIsInvokedAndMovieNotExist_ThenThrowException()
    {
        //arrange
        Movie aMovie = new Movie(990, "Unknown Movie", "ADirector", DateTime.Now, 1000);
        //act
        Assert.ThrowsException<Exception>(() =>
        {
            _movieRepository.DeleteMovie(_movie);
        });
    }

    [TestMethod]
    public void Delete_WhenDeleteIsInvoked_ThenTheMovieIsRemoved()
    {
        //arrange
        _movieRepository.AddMovie(_movie);
        //act
        _movieRepository.DeleteMovie(_movie);
        //assert
        List<Movie> result = _movieRepository.GetMovies();
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Update_WhenUpdateIsInvokedWithADetachedMovie_ThenThrowException()
    {
        //arrange
        Movie aMovie = new Movie(99, "Unknown Movie", "ADirector", DateTime.Now, 100);
        //we simulate the movie is not tracked
        //act
        Assert.ThrowsException<Exception>(() =>
        {
            _movieRepository.UpdateMovie(aMovie);
        });
        //it tries to update an object is not in the context
    }

    [TestMethod]
    public void Update_WhenUpdateIsInvoked_ThenTheMovieIsUpdated()
    {
        //arrange
        _movieRepository.AddMovie(_movie);
        _movie.Title = "Updated Title";
        _movie.Director = "Updated Director";
        //act
        _movieRepository.UpdateMovie(_movie);
        //assert
        Movie? movieUpdated = _movieRepository.GetMovie(m => m.Id == 1);
        Assert.AreEqual("Updated Title", movieUpdated.Title);
        Assert.AreEqual("Updated Director", movieUpdated.Director);
    }
}