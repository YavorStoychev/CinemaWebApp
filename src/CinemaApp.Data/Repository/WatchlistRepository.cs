using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository
{
    public class WatchlistRepository :BaseRepository, IWatchlistRepository
    {
        public WatchlistRepository(CinemaAppDbContext dbContext)
            :base(dbContext)
        {
            
        }
        public async Task<IEnumerable<UserMovie>> GetAllUserMoviesAsync()
        {
           IEnumerable<UserMovie> userMovies = await DbContext
                .UsersMovies
                .AsNoTracking()
                .Include(um => um.Movie)
                .ToArrayAsync();

            return userMovies;
        }
    }
}
