namespace CinemaApp.Services.Models.Movie
{
    using Mapping;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using AutoMapper;
    using static GCommon.ApplicationConstants;
    using System.Globalization;
    using System.Reflection.PortableExecutable;

    public class MovieAllDto : IMapFrom<Movie>, IMapTo<Movie>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public string Director { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieAllDto, Movie>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
