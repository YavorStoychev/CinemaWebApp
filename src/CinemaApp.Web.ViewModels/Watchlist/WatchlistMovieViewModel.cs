using AutoMapper;
using CinemaApp.Services.Mapping;
using CinemaApp.Services.Models.Watchlist;
using System.Globalization;
using static CinemaApp.GCommon.ApplicationConstants;

namespace CinemaApp.Web.ViewModels.Watchlist
{
    public class WatchlistMovieViewModel : IMapFrom<WatchlistMovieDto>, IHaveCustomMappings
    {
        public Guid MovieId { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<WatchlistMovieDto, WatchlistMovieViewModel>()
                .ForMember(d => d.MovieId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.ReleaseDate, opt => opt.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)));
        }
    }
}
