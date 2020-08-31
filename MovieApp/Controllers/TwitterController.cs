using System;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Models;
using Tweetinvi;
using System.Collections.Generic;
using Tweetinvi.Parameters;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MovieApp.Controllers
{
    [Authorize]
    public class TwitterController : Controller
    {
        private const string _consumerKey = "aZYeww6uaKDI2uLBq6faQwXgu";
        private const string _consumerSecret = "UNdvPJvtY4IE0OUs1NQYOJ00dfG6cS1fZNb88VhMUS9ug7j5bn";
        private const string _accessToken = "1297967587763064841-XahS8NvITJcjvMICVqfPGykoYf3CSb"; // app user
        private const string _accessTokenSecret = "JaFBO2EQqZalgDkkEbgGZxxdzMvEaiEnqn7XHg3ECx1uG"; // app user
        private readonly ITwitterCredentials _credentials = 
            new TwitterCredentials(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret); // app page credentials

        private readonly MovieAppContext _context;
        private TweetsController _tweetsController;

        public TwitterController(MovieAppContext context)
        {
            _context = context;
            _tweetsController = new TweetsController(context);
            Auth.SetUserCredentials(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret);
        }

        //------------ USER AUTH ----------------
        // Login user if not enter already
        public ActionResult LoginUser()
        {
            try
            {
                var user = Tweetinvi.User.GetAuthenticatedUser();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("TwitterAuth");
            }
        }

        // Create user auth object
        public ActionResult TwitterAuth()
        {
            var appCreds = new ConsumerCredentials( _consumerKey, _consumerSecret);
         
            var redirectURL = "http://localhost:51854/Twitter/ValidateTwitterAuth";
            IAuthenticationContext _authenticationContext = AuthFlow.InitAuthentication(appCreds, redirectURL);

            return new RedirectResult(_authenticationContext.AuthorizationURL);
        }

        [AllowAnonymous]
        // Validate user auth object
        public ActionResult ValidateTwitterAuth()
        {
            // Get some information back from the URL
            var verifierCode = this.Request.Query["oauth_verifier"];
            var authorizationId = this.Request.Query["authorization_id"];

            // Create the user credentials
            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);

            // Do whatever you want with the user now!
            ViewBag.User = Tweetinvi.User.GetAuthenticatedUser(userCreds);

            return View();
        }
    
        // Get connected user twitter
        public IAuthenticatedUser GetConnectedUser()
        {
            try
            {
                var user = Tweetinvi.User.GetAuthenticatedUser();
                return user;
            } catch(Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
                return null;
            }
        }

        //----------------- Tweets Handler --------------------
        [HttpPost]
        public async Task<ActionResult> PublishTweetAsync(string userId, int movieId, string message, string filePath, Models.Tweet.TweetType tweetType)
        {
            try
            {
                byte[] imageBytes;
                var publishTweetParameters = new PublishTweetParameters(message);

                if (filePath.StartsWith("http://") || filePath.StartsWith("https://"))
                {
                   
                    var webClient = new WebClient();
                    imageBytes = webClient.DownloadData(filePath);

                } else
                {

                    imageBytes =  System.IO.File.ReadAllBytes(filePath);
                }


                if (imageBytes != null)
                {
                    var media = Upload.UploadBinary(imageBytes);
                    publishTweetParameters.Medias.Add(media);
                }

                var tweet = Auth.ExecuteOperationWithCredentials(_credentials, () =>
                {
                    return Tweetinvi.Tweet.PublishTweet(publishTweetParameters);
                });

               
                Models.Tweet tweetNew = new Models.Tweet()
                {
                    TweetId = tweet.Id,
                    Type = tweetType,
                    Author = userId,
                    MovieId = movieId
                };

                _context.Tweet.Add(tweetNew);
                await _context.SaveChangesAsync();
             
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", tweet?.Id },
                    { "author", tweet?.CreatedBy.ScreenName },
                    { "actionPerformed", "Publish" },
                    { "success", tweet != null }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }

            catch (Exception)
            {
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", null },
                    { "author", null },
                    { "actionPerformed", "Publish" },
                    { "success", false }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }
         }
        
        [HttpPost]
        public async Task<ActionResult> DeleteTweetAsync(int movieId, string authorId = null, Models.Tweet.TweetType? tweetType = null)
        {
            List<Models.Tweet> tweetsList = _tweetsController.GetTweet(movieId, authorId, tweetType);
            if(tweetsList == null)
            {
                return NotFound();
            }

            try
            {
                foreach (Models.Tweet tweetItem in tweetsList)
                {
                    var tweetFound = Auth.ExecuteOperationWithCredentials(_credentials, () =>
                    {
                        return Tweetinvi.Tweet.GetTweet(tweetItem.TweetId);
                    });

                    await tweetFound.DestroyAsync();
                    _context.Tweet.Remove(tweetItem);

                }

                await _context.SaveChangesAsync();

                return View();
            }
            catch (Exception)
            {
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", "ids" },
                    { "actionPerformed", "Delete" },
                    { "success", false }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }

        }

        public List<ITweet> GetAppPosts()
        {

            var user = GetConnectedUser();
            var tweets = Timeline.GetUserTimeline(user.UserIdentifier).ToList();

            return tweets;
        }

        public ActionResult TweetPublished(long? id, string author, string actionPerformed, bool success = true)
        {
            ViewBag.TweetId = id;
            ViewBag.Author = author;
            ViewBag.ActionType = actionPerformed;
            ViewBag.Success = success;
            return View();
        }
    }
}
