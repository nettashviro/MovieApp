using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "מדינה")]
        public string Country { get; set; }

        [Display(Name = "שפת מקור")]
        public string Language { get; set; }

        [Display(Name = "שנת יציאה")]
        public int Year { get; set; }

        [Display(Name = "סוגה")]
        public MovieGenre Genre { get; set; }

        [Display(Name = "משך בדקות")]
        public int Duration { get; set; }

        [Display(Name = "קישור לטריילר")]
        public string TrailerUrl { get; set; }

        [Range(0, 10)]
        [Display(Name = "דירוג")]
        public double Rating { get; set; }

        [Display(Name = "בעלי תפקידים בסרט")]
        public ICollection<OfficialOfMovie> OfficialOfMovies { get; set; }

        [Display(Name = "פסקול")]
        public ICollection<SoundtrackOfMovie> SoundtracksOfMovie { get; set; }

        [NotMapped]
        [Display(Name = "תמונה")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "מזהה בTMDB")]
        public int MovieIdInTMDB { get; set; }

        public enum MovieGenre
        {
            אימה,
            דרמה,
            קומדייה,
            אקשן,
            רומנטיקה,
            אנימציה
        }

        public enum MovieSpliceOptions
        {
            [Display(Name = "מדינה")]
            Country,
            [Display(Name = "סוגה")]
            Genre,
            [Display(Name = "שנת יציאה")]
            Year,
            [Display(Name = "משך בדקות")]
            Duration,
            [Display(Name = "שפת מקור")]
            Language,
            [Display(Name = "דירוג")]
            Rating
        }

        public enum MovieAverageOfOptions
        {
            [Display(Name = "משך בדקות")]
            Duration,
            [Display(Name = "דירוג")]
            Rating
        }

        public enum MovieAverageByOptions
        {
            [Display(Name = "מדינה")]
            Country,
            [Display(Name = "סוגה")]
            Genre,
            [Display(Name = "שנת יציאה")]
            Year,
            [Display(Name = "שפת מקור")]
            Language,
        }
    }
}
