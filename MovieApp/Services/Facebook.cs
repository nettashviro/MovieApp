using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using Newtonsoft.Json.Linq;

namespace MovieApp.Services
{
    public class Facebook : HttpHandlerModel
    {
        private const string _apiUrl= "https://graph.facebook.com/";
        public const string _pageAccessToken = "EAADNYI6M4PUBACTUZC839oH0TbWUizCDdFxTZBhkR7mOKZAKeIGY2p7OffPysc2L4BgQ8CesHN11jjZBkGLsfpHL9WKV0oN3zLLT7BhZASxm3ISsec1ysSZARe23v7MfgFZB9fALBBt1WlGDZCljAphX2KetHYVMZAJsY4HXZBqqNygPPf2rIrT5Oobh6oGasTmtd3BLxadFZBoEAZDZD";
        readonly string _pageEdgeFeed = "feed";

        public Facebook() : base(_apiUrl, _pageAccessToken)
        {
        }

        public string PostNewPost(string message)
        {
            string UrlParams = $"message={message}&access_token={ApiKey}";
            JObject json = Post(UrlParams, _pageEdgeFeed);
            return json.ToString();
        }

    }
}
