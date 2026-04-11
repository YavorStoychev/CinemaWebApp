using AutoMapper;
using CinemaApp.Services.Mapping;
using CinemaApp.Services.Models.Movie;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static CinemaApp.GCommon.ApplicationConstants;
namespace CinemaApp.Web.ViewModels.Movie
{
    public class AllMoviesIndexViewModel : IMapFrom<MovieAllDto>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        [Required]
        public string ReleaseDate { get; set; } = null!;

        public string Director { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieAllDto, AllMoviesIndexViewModel>()
                .ForMember(d => d.ReleaseDate,
                cfg => cfg.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)));
        }
    }
}
