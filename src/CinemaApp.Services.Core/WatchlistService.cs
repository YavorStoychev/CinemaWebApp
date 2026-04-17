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
            bool userWatchlistEntryExists = await watchlistRepository
                         .ExistsAsync(userId, movieId);

            if (userWatchlistEntryExists)
            {
                throw new EntityAlreadyExistsException();
            }

            bool movieExists = await movieRepository
                .ExistsByIdAsync(movieId);

            if (!movieExists)
            {
                throw new EntityNotFoundException();
            }

            UserMovie newUserMovie = new UserMovie()
            {
                UserId = userId,
                MovieId = movieId
            };

            bool successAdd = watchlistRepository
                .AddUserMovieAsync(newUserMovie).GetAwaiter().GetResult();

            if (!successAdd)
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

    }
}
