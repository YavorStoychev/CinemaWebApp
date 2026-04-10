using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Contracts;
using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
using static CinemaApp.GCommon.ApplicationConstants;

namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }


        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync()
        {
            IEnumerable<Movie> allMoviesDb = await movieRepository
                .GetAllMoviesNoTrackingWithProjectionAsync(m => new Movie()
                {              
                        Id = m.Id,
                        Title = m.Title,
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate,
                        Director = m.Director,
                        ImageUrl = m.ImageUrl                  
                });

            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = allMoviesDb
                .Select(m => new AllMoviesIndexViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre.ToString(),
                    ReleaseDate = m.ReleaseDate.ToString(DefaultDateFormat,CultureInfo.InvariantCulture),
                    Director = m.Director,
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .ThenBy(m => m.Director)
                .ToArray();

            return allMoviesViewModel;
        }
        public async Task CreateMovieAsync(MovieFormViewModel movieFormModel)
        {
           Movie movie = new Movie()
           {
                Title = movieFormModel.Title,
                Genre = movieFormModel.Genre,
                ReleaseDate = movieFormModel.ReleaseDate,
                Director = movieFormModel.Director,
                Duration = movieFormModel.Duration,
                Description = movieFormModel.Description,
                ImageUrl = movieFormModel.ImageUrl,
                
           };

            bool successAdd = await movieRepository.AddMovieAsync(movie);

            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                return null;
            }

            return new MovieDetailsViewModel()
            {
                Id = movieDb.Id,
                Title = movieDb.Title,
                Genre = movieDb.Genre,
                ReleaseDate = movieDb.ReleaseDate.ToString(DefaultDateFormat,CultureInfo.InvariantCulture),
                Director = movieDb.Director,
                Duration = movieDb.Duration,
                Description = movieDb.Description,
                ImageUrl = movieDb.ImageUrl ?? DefaultImageUrl
            };
        }

        public async Task<MovieFormViewModel?> GetMovieFormByIdAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                return null;
            }

            return new MovieFormViewModel()
            {
                Title = movieDb.Title,
                Genre = movieDb.Genre,
                ReleaseDate = movieDb.ReleaseDate,
                Director = movieDb.Director,
                Duration = movieDb.Duration,
                Description = movieDb.Description,
                ImageUrl = movieDb.ImageUrl ?? DefaultImageUrl
            };
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await movieRepository.ExistsByIdAsync(id);    
               
        }

        public async Task EditMovieAsync(Guid id, MovieFormViewModel movieFormViewModel)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            movieDb.Title = movieFormViewModel.Title;
            movieDb.Genre = movieFormViewModel.Genre;
            movieDb.ReleaseDate = movieFormViewModel.ReleaseDate;
            movieDb.Description = movieFormViewModel.Description;
            movieDb.Duration = movieFormViewModel.Duration;
            movieDb.Director = movieFormViewModel.Director;
            movieDb.ImageUrl = movieFormViewModel.ImageUrl;

            bool editSuccess = await movieRepository.EditMovieAsync(movieDb);

            if (!editSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
