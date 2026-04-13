using CinemaApp.Services.Models.Watchlist;

namespace CinemaApp.Services.Core.Contracts
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistMovieDto>> GetUserWatchlistByIdAsync(string userId);
    }
}
