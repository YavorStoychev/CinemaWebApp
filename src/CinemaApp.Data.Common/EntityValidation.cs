namespace CinemaApp.Data.Common
{
    public static class EntityValidation
    {

        public class Movie
        {
            public const int TitleMaxLength = 100;

            public const int GenreMaxLength = 50;

            public const int DirectorMaxLength = 50;

            public const int DescriptionMaxLength = 1000;

            public const int ImageUrlMaxLengh = 2048;
        }
    }
}
