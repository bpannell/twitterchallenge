using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ILoveTwitter.Models.TwitterApi;

namespace ILoveTwitter.Models
{
    public class TwitterRepository : ITwitterRepository
    {
        private List<TwitterApi> _tweets;
        public TwitterRepository()
        {
            if (_tweets == null)
            {
                GetLastTenTweets();
            }
        }

        public void GetLastTenTweets()
        {
            string TwitterAuthUrl = "https://api.twitter.com/oauth2/token";
            string TwitterApiUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=salesforce&count=10&tweet_mode=extended";
            string consumerKey = "3Ci5bsfcPBDGpskKNHA6gbRdH";
            string consumerKeySecret = "mG4JBbhY3Tj2uhvF7WxPVbjGued3dDjPIp7VMQRLkmslW0ufsU";

            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(consumerKey) + ":" + Uri.EscapeDataString(consumerKeySecret)));

            var postBody = "grant_type=client_credentials";
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(TwitterAuthUrl);
            tokenRequest.Headers.Add("Authorization", authHeader);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tokenRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            tokenRequest.Headers.Add("Accept-Encoding", "gzip");

            using (Stream stream = tokenRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            WebResponse tokenResponse = tokenRequest.GetResponse();
            TwitResponse twitResponse;
            using (tokenResponse)
            {
                using (var reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    var objectText = reader.ReadToEnd();
                    twitResponse = JsonConvert.DeserializeObject<TwitResponse>(objectText);
                }
            }

            HttpClient getTweets = new HttpClient();
            getTweets.DefaultRequestHeaders.Add("Authorization", twitResponse.token_type + " " + twitResponse.access_token);
            HttpResponseMessage gotTweets = getTweets.GetAsync(new Uri(TwitterApiUrl)).Result;
            if (!gotTweets.IsSuccessStatusCode)
                throw new Exception(String.Format("Twitter Api request did not return successfully; return {0}. Message: {1}", gotTweets.StatusCode.ToString(), gotTweets.ReasonPhrase));

            List<TwitterApi> tweets = new List<TwitterApi>();
            string json;
            using (Stream stream = gotTweets.Content.ReadAsStreamAsync().Result)
            {
                using (var sr = new StreamReader(stream))
                {
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    json = sr.ReadToEnd();
                    tweets = JsonConvert.DeserializeObject<List<TwitterApi>>(json, jsonSerializerSettings);
                }
            }

            _tweets = tweets;
        }

        public IEnumerable<TwitterApi> GetTweets()
        {
            return _tweets;
        }

        public IEnumerable<TwitterApi> GetTweets (string search)
        {
            if (_tweets == null || _tweets.Count == 0)
            {
                return _tweets;
            }

            if (search != null)
                return _tweets.Where(o => o.textToShow().Contains(search));

            GetLastTenTweets();
            return _tweets;

        }

    }
}
