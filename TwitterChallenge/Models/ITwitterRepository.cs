using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveTwitter.Models
{
    public interface ITwitterRepository
    {
        IEnumerable<TwitterApi> GetTweets();
        IEnumerable<TwitterApi> GetTweets(string search);
    }
}
