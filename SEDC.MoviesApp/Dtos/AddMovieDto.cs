using SEDC.MoviesApp.Models;
using static SEDC.MoviesApp.Models.Movie;

namespace SEDC.MoviesApp.Dtos
{
    public class AddMovieDto
    {
       public string Title { get; set; }    

        public string Description { get; set; } 

        public int Year { get; set; }   

        public GenreEnum Genre { get; set; }    
    }
}
