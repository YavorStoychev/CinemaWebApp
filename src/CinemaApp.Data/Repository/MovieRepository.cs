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
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        private bool isDisposed = false;
        public MovieRepository(CinemaAppDbContext dbContext)
            :base(dbContext)
        {
           
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null)
        {
            IQueryable<Movie> moviesFetchQuery = DbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title);

            if (projectFunc != null)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Select(m => projectFunc(m))
                    .AsQueryable();
            }

            return await moviesFetchQuery.ToArrayAsync();
        }
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await DbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            return await DbContext
                .Movies
                .FindAsync(id);
        }
        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await DbContext.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
        public async Task<bool> EditMovieAsync(Movie movie)
        {
            DbContext.Movies.Update(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
        public async Task<bool> SoftDeleteMovieAsync(Movie movie)
        {
            movie.IsDeleted = true;
            DbContext.Movies.Update(movie);

          
            int resultCount = await SaveChangesAsync();
            return resultCount == 1;
        }

        public async Task<bool> HardDeleteMovieAsync(Movie movie)
        {
            DbContext.Movies.Remove(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;

        }
        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await DbContext
                .Movies
                .AnyAsync(m => m.Id == id);
        }
       
      

       
    }
}
