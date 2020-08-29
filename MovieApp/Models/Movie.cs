using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public int Year { get; set; }
        public MovieGenre Genre { get; set; }

        public int Duration { get; set; }

        public string TrailerUrl { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        [Display(Name = "בעלי תפקידים בסרט")]
        public ICollection<OfficialOfMovie> OfficialOfMovies { get; set; }

        [Display(Name = "שירים בסרט")]
        public ICollection<SoundtrackOfMovie> SoundtracksOfMovie { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "TMDB Id")]
        public int MovieIdInTMDB { get; set; }

        public enum MovieGenre
        {
            Horror,
            Drama,
            Comedy,
            Action,
            Romance,
            Animation
        }

    }


}
