using AutoMapper;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Contracts;
using CinemaApp.GCommon.Exceptions;
using CinemaApp.Services.Core.Contracts;
using CinemaApp.Services.Models.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class WatchlistService : IWatchlistService
    {
        private IWatchlistRepository watchlistRepository;
        private IMovieRepository movieRepository;
        private readonly IMapper mapper;

        public WatchlistService(IWatchlistRepository watchlistRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            this.watchlistRepository = watchlistRepository;
            this.movieRepository = movieRepository;
            this.mapper = mapper;   
        }
        public async Task<IEnumerable<WatchlistMovieDto>> GetUserWatchlistByIdAsync(string userId)
        {
           IEnumerable<Movie> userWatchlist =  watchlistRepository
                  .GetAllUserMoviesAsync()
                  .GetAwaiter()
                  .GetResult()
                  .Where(um => um.UserId.ToLower() == userId.ToLower())
                  .Select(um => um.Movie)
                  .ToArray();

         
            IEnumerable<WatchlistMovieDto> watchlistMovieDto = mapper
                .Map<IEnumerable<WatchlistMovieDto>>(userWatchlist);

            return watchlistMovieDto;
        }

        public async Task AddMovieToUserWatchlistAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await watchlistRepository
                 .GetUserIncludeDeleteAsync(userId, movieId);

            if (userMovie != null && userMovie.IsDeleted == false)
            {
                throw new EntityNotFoundException();
            }

            bool movieExists = await movieRepository
                .ExistsByIdAsync(movieId);

            if (!movieExists)
            {
                throw new EntityNotFoundException();
            }

            bool successPersist = false;

            if (userMovie == null)
            {
                UserMovie newUserMovie = new UserMovie()
                {
                    UserId = userId,
                    MovieId = movieId
                };

                successPersist = watchlistRepository
                    .AddUserMovieAsync(newUserMovie).GetAwaiter().GetResult();          
            }
            else
            {
                userMovie.IsDeleted = false;

                successPersist = await watchlistRepository
                    .UpdateUserAsync(userMovie);
            }

            if (!successPersist)
            {
                throw new EntityPersistFailureException();
            }
        }
        public async Task<bool> MovieIsInUserWatchlistAsync(string userId, Guid movieId)
        {
            try
            {
                    bool userWatchlistEntryExists = await watchlistRepository
                        .ExistsAsync(userId, movieId);

                return userWatchlistEntryExists;
            }
            catch (NullReferenceException nre)
            {
                
                throw new EntityKeyNullOrEmptyException(nre.Message);
            }
        }

        public async Task RemoveMovieFromUserWatchlistAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await watchlistRepository
                 .GetUserMovieAsync(userId, movieId);

            if (userMovie == null)
            {
                throw new EntityNotFoundException();
            }

           bool successDelete = await watchlistRepository
                .SoftDeleteUserMovieAsync(userMovie);

            if (!successDelete)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
