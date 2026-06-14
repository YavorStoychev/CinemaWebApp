namespace CinemaApp.Data
{
    using CinemaApp.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class CinemaWebAppDbContext : IdentityDbContext
    {
        public CinemaWebAppDbContext(DbContextOptions<CinemaWebAppDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(CinemaWebAppDbContext).Assembly);
        }
    }
}
