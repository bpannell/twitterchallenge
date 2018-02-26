using ILoveTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveTwitter.ViewModels
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public List<TwitterApi> Tweets { get; set; }
    }
}
