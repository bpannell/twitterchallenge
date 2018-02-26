using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveTwitter.Models;
using ILoveTwitter.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ILoveTwitter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITwitterRepository _twitterRepo;

        public HomeController(ITwitterRepository twitterRepository)
        {
            _twitterRepo = twitterRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel()
            {
                Title = "Last 10 Tweets from Salesforce",
                Tweets = _twitterRepo.GetTweets().ToList()
            };
            return View(homeViewModel);
        }

        [HttpPost]
        public IActionResult Index(string searchString)
        {
            List<TwitterApi> found = _twitterRepo.GetTweets(searchString).ToList();
            var homeViewModel = new HomeViewModel()
            {
                Title = "Found " + found.Count + " From Last 10 Tweets that contain the string: " + searchString,
                Tweets = found
            };
            return View(homeViewModel);
        }
    }
}
