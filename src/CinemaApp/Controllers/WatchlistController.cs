using AutoMapper;
using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Services.Models.Watchlist;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static CinemaApp.GCommon.OutputMessages.Watchlist;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;
        private readonly IMapper mapper;
        private readonly ILogger<WatchlistController> logger;
        public WatchlistController(IWatchlistService watchlistService, IMapper mapper, ILogger<WatchlistController> logger)
        {
            this.watchlistService = watchlistService;
            this.mapper = mapper;
            this.logger = logger;
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

        public async Task<IActionResult> Add([FromRoute(Name = "id")]Guid movieId)
        {
            string userId = GetUserId()!;
            try
            {
                await watchlistService.AddMovieToUserWatchlistAsync(userId, movieId);

            }
            catch (EntityAlreadyExistsException eaee)
            {
                logger.LogError(eaee, string.Format(MovieAlreadyInWatchlistMessage, movieId, userId));
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, AddToWatchlistFailureMessage);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
