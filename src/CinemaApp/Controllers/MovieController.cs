using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CinemaApp.GCommon.OutputMessages.Movie;
using static CinemaApp.GCommon.ApplicationConstants;
namespace CinemaApp.Web.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        private readonly ILogger<MovieController> logger;
        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            this.movieService = movieService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await movieService
                 .GetAllMoviesOrderedByTitleAsync();

            return View(allMovies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieFormViewModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }
            try
            {
                await movieService.CreateMovieAsync(formModel);
            }
            catch (EntityCreatePersistFailureException e)
            {
                logger.LogError(e, CreateMovieFailureMessage);
                ModelState.AddModelError(string.Empty, CreateMovieFailureMessage);

                return View(formModel);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, CreateMovieFailureMessage);
                return View(formModel);
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            MovieDetailsViewModel? movieDetailsViewModel = await movieService
                    .GetMovieDetailsByIdAsync(id);

            if (movieDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(movieDetailsViewModel);
        }
    }
}
