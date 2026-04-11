namespace CinemaApp.Services.Models.Movie
{
    using Mapping;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using AutoMapper;
    using static GCommon.ApplicationConstants;
    using System.Globalization;

    public class MovieAllDto : IMapFrom<Movie>, IHaveCustomMappings
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
            configuration.CreateMap<Movie, MovieAllDto>()
                .ForMember(d => d.ReleaseDate, 
                cfg => cfg.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat,CultureInfo.InvariantCulture)));
        }
    }
}
