using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CinemaApp.GCommon.OutputMessages.Movie;
using static CinemaApp.GCommon.ApplicationConstants;
using CinemaApp.Data.Models;
using AutoMapper;
using CinemaApp.Services.Models.Movie;
namespace CinemaApp.Web.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IMapper mapper;

        private readonly ILogger<MovieController> logger;
        public MovieController(IMovieService movieService, IMapper mapper, ILogger<MovieController> logger)
        {
            this.movieService = movieService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();

            IEnumerable<MovieAllDto> movieAllDtos = await movieService
                 .GetAllMoviesOrderedByTitleAsync(userId);

            IEnumerable<AllMoviesIndexViewModel> allMoviesIndexViewModels = mapper
                .Map<IEnumerable<AllMoviesIndexViewModel>>(movieAllDtos);

            return View(allMoviesIndexViewModels);
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
                MovieDetailsDto movieDetailsDto = mapper
                    .Map<MovieDetailsDto>(formModel);

                await movieService.CreateMovieAsync(movieDetailsDto);
            }
            catch (EntityPersistFailureException e)
            {
                logger.LogError(e, string.Format(CrudMovieFailureMessage, nameof(Create)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage,"creating"));

                return View(formModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, CrudMovieFailureMessage);
                return View(formModel);
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            MovieDetailsDto? movieDetailsDto = await movieService
                    .GetMovieDetailsByIdAsync(id);

            if (movieDetailsDto == null)
            {
                return NotFound();
            }

            MovieDetailsViewModel movieDetailsVm = mapper
                .Map<MovieDetailsViewModel>(movieDetailsDto);

            return View(movieDetailsVm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            MovieDetailsDto? movieDetailsDto = await movieService
                .GetMovieFormByIdAsync(id);

            if (movieDetailsDto == null)
            {
                return NotFound();
            }
            MovieFormViewModel movieFormViewModel = mapper
                .Map<MovieFormViewModel>(movieDetailsDto);

            return View(movieFormViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]Guid id, MovieFormViewModel formModel)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            try
            {
                MovieDetailsDto movieDetailsDto = mapper
                    .Map<MovieDetailsDto>(formModel);

                await movieService.EditMovieAsync(id, movieDetailsDto);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch(EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Edit)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage, "editing"));

                return View(formModel);
            }

            return RedirectToAction(nameof(Details), new {id});
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieDetailsDto? movieDetailsDto = await movieService
                .GetMovieDetailsByIdAsync(id);

            if (movieDetailsDto == null)
            {
                return NotFound();
            }

            MovieDeleteViewModel movieDeleteVm = mapper
                .Map<MovieDeleteViewModel>(movieDetailsDto);

            return View(movieDeleteVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]Guid id, MovieDetailsViewModel? deleteDetailsVm)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                await movieService.SoftDeleteMovieAsync(id);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Delete)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage, "deleting"));
                return View(deleteDetailsVm);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
