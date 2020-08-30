using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models.TMDB
{
    public class MovieVideosResult
    {
        public string id { get; set; }
        public string iso_639_1 { get; set; }
        public string iso_3166_1 { get; set; }
        public string key  { get; set; }

        public string name { get; set; }
        public string site  { get; set; }

        public int size { get; set; }

        public TYPE_VIDEO type { get; set; }

        public enum TYPE_VIDEO
        {
            Trailer, 
            Teaser,
            Clip,
            Featurette,
            [Description("Behind the Scenes")]
            BTS,
            Bloopers
        }
    }
}
