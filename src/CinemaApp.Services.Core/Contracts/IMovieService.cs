using CinemaApp.Services.Models.Movie;
using CinemaApp.Web.ViewModels.Movie;

namespace CinemaApp.Services.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync();

        Task CreateMovieAsync(MovieFormViewModel movieFormModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieFormViewModel?> GetMovieFormByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieFormViewModel movieFormViewModel);

        Task SoftDeleteMovieAsync(Guid id);
        Task HardDeleteMovieAsync(Guid id);
    }
}
