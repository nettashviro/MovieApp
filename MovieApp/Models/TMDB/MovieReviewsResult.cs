using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models.TMDB
{
    public class MovieReviewsResult
    {
        public string author { get; set; }
        public string content { get; set; }
        public string id { get; set; }
        public string url { get; set; }
    }
}
