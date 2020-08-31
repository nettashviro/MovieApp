using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Tweet
    {
        [Key]
        public long Id { get; set; }

        public long TweetId { get; set; }
        public string Author { get; set; }

        [ForeignKey("Movie")]
        public int  MovieId { get; set; }

        public TweetType Type { get; set; }

        public enum TweetType
        {
            MovieWatched,
            MovieAdded,
        }

    }
}
