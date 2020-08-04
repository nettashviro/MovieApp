using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Soundtrack
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // TODO: parse int to display minutes
        public int Duration { get; set; }

        public string Writer { get; set; }

        public string Performer { get; set; }
    }
}
