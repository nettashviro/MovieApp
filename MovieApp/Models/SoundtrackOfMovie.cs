using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class SoundtrackOfMovie
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int SoundtrackId { get; set; }
        public Soundtrack Soundtrack { get; set; }
    }
}
