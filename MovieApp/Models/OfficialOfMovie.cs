using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class OfficialOfMovie
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int OfficialId { get; set; }
        public Official Official { get; set; }
    }
}
