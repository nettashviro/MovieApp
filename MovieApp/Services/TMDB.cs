using Microsoft.AspNetCore.Mvc.Routing;
using MovieApp.Models;
using MovieApp.Models.TMDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public class TMDB
    {
        private const string url = "https://api.themoviedb.org/3/";
        private string apiKey = "c660a2bbcb461b092b1e04c1618e0a92";

        public TMDB()
        {

        }

    
        public List<MovieReviewsResult> GetMovieReviewsById(string id)
        {
            //int movieId = 0;
            List<MovieReviewsResult> movieResult = new List<MovieReviewsResult>();
            string currentUrl = $"{url}movie/{id}/reviews?api_key={apiKey}";

            try
            {
                JObject jsonResult = Get(currentUrl);
                movieResult = JsonConvert.DeserializeObject<List<MovieReviewsResult>>(jsonResult["results"].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in TMDB Service: There was a problem with extracting movie reviews by id. The problem is: {e.Message}");
            }

            return movieResult;
        }

        public List<MovieSearchResult> GetMovieIdByName(string name)
        {
            //int movieId = 0;
            List<MovieSearchResult> movieResult = new List<MovieSearchResult>();
            string currentUrl = $"{url}search/movie?query={name}&language=he&api_key={apiKey}";
            

            try
            {
                JObject jsonResult = Get(currentUrl);
                movieResult = JsonConvert.DeserializeObject<List<MovieSearchResult>>(jsonResult["results"].ToString());                             
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in TMDB Service: There was a problem with extracting movie id by name. The problem is: {e.Message}");
            }

            return movieResult;           
        }


        protected JObject Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string resInString = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            JObject json = JObject.Parse(resInString);
            return (json);
        }

        protected JObject Post(string UrlParams, string Path = "")
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string resInString = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            JObject json = JObject.Parse(resInString);
            return (json);
        }

    }
}
