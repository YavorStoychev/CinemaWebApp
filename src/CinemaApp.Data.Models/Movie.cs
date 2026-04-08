using System.ComponentModel.DataAnnotations;
using static CinemaApp.Data.Common.EntityValidation.Movie;

namespace CinemaApp.Data.Models
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(GenreMaxLength)]
        public string Genre { get; set; } = null!;

        public DateOnly ReleaseDate { get; set; }

        [Required]
        [MaxLength(DirectorMaxLength)]
        public string Director { get; set; } = null!;

        public int Duration { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLengh)]
        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; } = false;

        //public virtual ICollection<CinemaMovie> MovieCinemas { get; set; } 
        //    = new HashSet<CinemaMovie>();

        //public virtual ICollection<ApplicationUserMovie> MovieApplicationUsers { get; set; }
        //    = new HashSet<ApplicationUserMovie>();

        //public virtual ICollection<Ticket> Tickets { get; set; }
        //    = new HashSet<Ticket>();
    }
}