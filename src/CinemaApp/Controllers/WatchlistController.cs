using AutoMapper;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Services.Models.Watchlist;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;
        private readonly IMapper mapper;
        public WatchlistController(IWatchlistService watchlistService, IMapper mapper)
        {
            this.watchlistService = watchlistService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId() ?? string.Empty;

            IEnumerable<WatchlistMovieDto> watchlistMovieDtos = await watchlistService
                .GetUserWatchlistByIdAsync(userId);

            IEnumerable<WatchlistMovieViewModel> watchlistMovieViewModels = mapper
                .Map<IEnumerable<WatchlistMovieViewModel>>(watchlistMovieDtos);

            return View(watchlistMovieViewModels);
        }
    }
}
