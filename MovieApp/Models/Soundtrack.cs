using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Soundtrack
    {
        public int Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "משך בדקות")]
        public double Duration { get; set; }

        [Display(Name = "כתובת לפסקול")]
        public string SoundtrackUrl { get; set; }

        [Display(Name = "סרטים בהם מנוגן")]
        public ICollection<SoundtrackOfMovie> SoundtrackOfMovies { get; set; }

        [Display(Name = "כותב")]
        public Official Writer { get; set; }

        [Display(Name = "מבצע")]
        public Official Performer { get; set; }

        public enum SoundtrackAverageOfOptions
        {
            Duration
        }
    }
}
