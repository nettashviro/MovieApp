using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Data;
using MovieApp.Models;
using Tweetinvi.Core.Extensions;

namespace MovieApp.Controllers
{
    public class TweetsController : Controller
    {
        private readonly MovieAppContext _context;

        public TweetsController(MovieAppContext context)
        {
            _context = context;
        }

        public List<Tweet> GetTweet( int movieId, string? authorId, Tweet.TweetType? tweetType)
        {
            List<Tweet> tweet;
            if (authorId == null || tweetType == null )
            {
                tweet =_context.Tweet.Where(x => x.MovieId == movieId).ToList();

            } else
            {
                tweet = _context.Tweet.Where(x => x.MovieId == movieId && x.Author == authorId && x.Type == tweetType).ToList();
            }

            return tweet;
        }

    }
}
