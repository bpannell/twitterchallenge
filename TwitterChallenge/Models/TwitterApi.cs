using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveTwitter.Models
{
    public class TwitterApi
    {
        public User user { get; set; }
        public string full_text { get; set; }
        public long retweet_count { get; set; }
        public string created_at { get; set; }
        public Retweet retweeted_status { get; set; }

        public string showDateTime()
        {
            DateTimeOffset dateTime = DateTimeOffset.ParseExact(created_at, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
            return dateTime.ToString("MM/dd/yyyy hh:mm tt");
        }

        public string textToShow()
        {
            if (retweeted_status != null)
                return "RT: " + retweeted_status.full_text;

            return full_text;
        }

        public class User
        {
            public string name { get; set; }
            public string screen_name { get; set; }
            public string profile_image_url { get; set; }
        }

        public class Retweet
        {
            public string full_text { get; set; }
        }

        public class TwitResponse
        {
            public string token_type { get; set; }
            public string access_token { get; set; }
        }
    }
}
