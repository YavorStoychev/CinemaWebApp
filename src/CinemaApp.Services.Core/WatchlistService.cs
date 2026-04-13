using AutoMapper;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Contracts;
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
        private readonly IMapper mapper;

        public WatchlistService(IWatchlistRepository watchlistRepository, IMapper mapper)
        {
            this.watchlistRepository = watchlistRepository;
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
    }
}
