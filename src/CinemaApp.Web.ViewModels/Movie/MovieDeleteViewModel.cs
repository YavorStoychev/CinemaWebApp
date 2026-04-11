using CinemaApp.Services.Mapping;
using CinemaApp.Services.Models.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class MovieDeleteViewModel : IMapFrom<MovieDetailsDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
