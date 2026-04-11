using AutoMapper;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Contracts;
using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Services.Models.Movie;
using CinemaApp.Web.ViewModels.Movie;
using System.Globalization;
using static CinemaApp.GCommon.ApplicationConstants;

namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly IMapper mapper;
        private readonly IMovieRepository movieRepository;
        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            this.movieRepository = movieRepository;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync()
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

            IEnumerable<MovieAllDto> allMoviesViewModel = mapper
                .Map<IEnumerable<MovieAllDto>>(allMoviesDb)
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .ThenBy(m => m.Director)
                .ToArray();

            return allMoviesViewModel;
        }
        public async Task CreateMovieAsync(MovieDetailsDto movieDetailsDto)
        {
            Movie newMovie = mapper
                .Map<Movie>(movieDetailsDto);
           

            bool successAdd = await movieRepository.AddMovieAsync(newMovie);

            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<MovieDetailsDto?> GetMovieDetailsByIdAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                return null;
            }

            return mapper.Map<MovieDetailsDto>(movieDb);
        }

        public async Task<MovieDetailsDto?> GetMovieFormByIdAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                return null;
            }

            return mapper.Map<MovieDetailsDto>(movieDb);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await movieRepository.ExistsByIdAsync(id);    
               
        }

        public async Task EditMovieAsync(Guid id, MovieDetailsDto movieDetailsDto)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            movieDb.Title = movieDetailsDto.Title;
            movieDb.Genre = movieDetailsDto.Genre;
            movieDb.ReleaseDate = movieDetailsDto.ReleaseDate;
            movieDb.Description = movieDetailsDto.Description;
            movieDb.Duration = movieDetailsDto.Duration;
            movieDb.Director = movieDetailsDto.Director;
            movieDb.ImageUrl = movieDetailsDto.ImageUrl;

            bool editSuccess = await movieRepository.EditMovieAsync(movieDb);

            if (!editSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task SoftDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

           bool deleteSuccess = await movieRepository.SoftDeleteMovieAsync(movieDb);

            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task HardDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
               .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            bool deleteSuccess = await movieRepository.HardDeleteMovieAsync(movieDb);

            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
