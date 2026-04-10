using CinemaApp.Web.ViewModels.Movie;

namespace CinemaApp.Services.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync();

        Task CreateMovieAsync(MovieFormViewModel movieFormModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieFormViewModel?> GetMovieFormByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieFormViewModel movieFormViewModel);
    }
}
