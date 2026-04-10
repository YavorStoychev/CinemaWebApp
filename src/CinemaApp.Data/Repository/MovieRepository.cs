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
    public class MovieRepository : IMovieRepository, IDisposable
    {
        private bool isDisposed = false;
        private readonly CinemaAppDbContext dbContext;
        public MovieRepository(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null)
        {
            IQueryable<Movie> moviesFetchQuery = dbContext
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
            return await dbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await dbContext.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            isDisposed = true;
        }
        private async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();  
        }

       
    }
}
