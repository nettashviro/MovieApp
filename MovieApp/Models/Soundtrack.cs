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
        public string Name { get; set; }

        // TODO: parse int to display minutes
        public double Duration { get; set; }
        
        public string TrailerUrl { get; set; }

        [Display(Name = "סרטים בהם מנוגן")]
        public ICollection<SoundtrackOfMovie> SoundtrackOfMovies { get; set; }

        public Official Writer { get; set; }

        public Official Performer { get; set; }
    }
}
