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
    public class WatchlistRepository : BaseRepository, IWatchlistRepository
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
        public async Task<bool> ExistsAsync(string userId, Guid movieId)
        {
            bool watchlistEntryExists = await DbContext.UsersMovies
                .AnyAsync(um => um.UserId.ToLower() == userId.ToLower() && um.MovieId == movieId);

            return watchlistEntryExists;
        }

        public async Task<bool> AddUserMovieAsync(UserMovie userMovie)
        {
            await DbContext.UsersMovies.AddAsync(userMovie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1; 
        }

        public async Task<bool> UpdateUserAsync(UserMovie userMovie)
        {
            DbContext.UsersMovies.Update(userMovie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
        public async Task<bool> SoftDeleteUserMovieAsync(UserMovie userMovie)
        {
            userMovie.IsDeleted = true;
            DbContext.UsersMovies.Update(userMovie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<UserMovie?> GetUserMovieAsync(string userId, Guid movieId)
        {
           UserMovie? userMovie = await DbContext
                .UsersMovies
                .SingleOrDefaultAsync(um => um.UserId.ToLower() == userId.ToLower() && um.MovieId == movieId);

            return userMovie;
        }

        public async Task<UserMovie?> GetUserIncludeDeleteAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await DbContext
               .UsersMovies
               .IgnoreQueryFilters()
               .SingleOrDefaultAsync(um => um.UserId.ToLower() == userId.ToLower() && um.MovieId == movieId);

            return userMovie;
        }
    }
}
