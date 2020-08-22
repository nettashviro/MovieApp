using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public abstract class HttpHandlerModel
    {

        public String Url
        {
            get;
            protected set;
        }

        public String ApiKey
        {
            get;
            protected set;
        }

        public HttpHandlerModel(string Url, string ApiKey)
        {
            this.ApiKey = ApiKey;
            this.Url = Url;
        }

        protected JObject Get(string UrlParams, string Path = "")
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.
                Create($"{Url}{Path}?{UrlParams}");
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
            HttpWebRequest req = (HttpWebRequest)WebRequest.
                Create($"{Url}{Path}?{UrlParams}");
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
