using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEDC.MoviesApp.Database;
using SEDC.MoviesApp.Dtos;
using SEDC.MoviesApp.Models;
using System.Reflection.Metadata;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SEDC.MoviesApp.Models.Movie;

namespace SEDC.MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet] //api/movies
        public ActionResult<IEnumerable<MovieDto>> Get()
        {
            return Ok(StaticDb.Movies);
        }

        [HttpGet("{id}")] //api/movies/2
        public ActionResult<MovieDto> Get(int id)
        {
            if (id < 0) 
            {
                return BadRequest("Invalid movie ID");
            }

            var movie = StaticDb.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);   
        }
            [HttpGet("queryString")] //api/movies/queryString?index=1
            public ActionResult<MovieDto> GetById([FromQuery]int id)
            {
            if (id < 0)
            {
                return BadRequest("Invalid movie ID");
            }

            var movie = StaticDb.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet("filter")]   //api/movies/filter?genre=1&year=2022  
        public ActionResult<List<MovieDto>> FilterNotesFromQuery([FromQuery] int? genre, [FromQuery] int? year)
        {
            //throw new NotImplementedException();
            try
            {
                var movie = StaticDb.Movies.Where(m => m.Genre == (GenreEnum)genre && m.Year == year).ToList();

                if (movie.Count == 0)
                {
                    return NotFound("Movie not found");
                }
                return Ok(movie);

            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto movie)
        { 
                UpdateMovieDto movieDto = new UpdateMovieDto()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    Year = movie.Year
                };
                return Ok(movieDto);
        }

        [HttpDelete]
        public IActionResult DeleteMovie([FromBody] int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid movie ID");
            }

            var movieToDelete = StaticDb.Movies.FirstOrDefault(m => m.Id == id);
            if (movieToDelete == null)
            {
                return NotFound();
            }

            StaticDb.Movies.Remove(movieToDelete);
            return Ok(movieToDelete);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            if(id < 0)
            {
                return BadRequest("Invalid movie ID");
            }
            var movieToDel = StaticDb.Movies.FirstOrDefault(m => m.Id == id);

            if (movieToDel == null)
            {
                return NotFound();  
            }
            StaticDb.Movies.Remove(movieToDel);

            return Ok("Movie deleted successfully");
        }

        [HttpPost("create")]
        public IActionResult AddMovie([FromBody] Movie movie) 
        {
            if (movie == null)
            {
                return BadRequest();
            }
            else
            {
                Movie newMovie = new Movie()
                {
                    Id = ++StaticDb.MovieId,
                    Title = movie.Title,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    Year = movie.Year
                };

                StaticDb.Movies.Add(newMovie);
                return Ok("Movie created successfully");    
            }
        }
    }
}
