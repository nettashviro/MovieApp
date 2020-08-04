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
        public long Name { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public int Year { get; set; }
        public MovieGenre Genre { get; set; }

        public int Duration { get; set; }

        public int TrailerUrl { get; set; }

        [Range(0, 5)]
        public float Rating { get; set; }

        [ForeignKey("Id_Director")]
        public Director Director { get; set; }

        public ICollection<Soundtrack> Soundtracks { get; set; }

        public string ImageUrl { get; set; }
    }
}

public enum MovieGenre
{
    Horror,
    Drama,
    Comedy,
    Action,
    Romance,
    Animation
}
