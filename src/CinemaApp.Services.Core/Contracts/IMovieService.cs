using CinemaApp.Services.Models.Movie;
using CinemaApp.Web.ViewModels.Movie;

namespace CinemaApp.Services.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync(string? userId = null);

        Task CreateMovieAsync(MovieDetailsDto movieDetailsDto);

        Task<MovieDetailsDto?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieDetailsDto?> GetMovieFormByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieDetailsDto movieFormViewModel);

        Task SoftDeleteMovieAsync(Guid id);
        Task HardDeleteMovieAsync(Guid id);
    }
}
