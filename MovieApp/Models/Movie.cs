using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        [ForeignKey("Id_Official")]
        public Official Official { get; set; }

        public ICollection<Soundtrack> Soundtracks { get; set; }

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

    }


}
