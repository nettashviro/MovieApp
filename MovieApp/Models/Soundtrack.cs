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

        // TODO: parse int to display minutes
        [Display(Name = "משך בדקות")]
        public int Duration { get; set; }

        [Display(Name = "כותב")]
        public string Writer { get; set; }

        [Display(Name = "מבצע")]
        public string Performer { get; set; }
    }
}
