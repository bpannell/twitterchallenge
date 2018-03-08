using ILoveTwitter.Models;
using NUnit.Framework;
using System.Linq;

namespace TwitterChallenge.Tests
{
    public class TwitterRepoTests
    {
        [Test]
        public void TestLastTenTweets()
        {
            TwitterRepository tw = new TwitterRepository();

            Assert.AreEqual(10, tw.GetTweets().ToList().Count);
        }

        [Test]
        public void TestSearchTweetsReturnNothing()
        {
            TwitterRepository tw = new TwitterRepository();

            Assert.AreEqual(0, tw.GetTweets("nowayeverthiswouldbeinatweet").ToList().Count);
        }

        [Test]
        public void TestShowDateTimeOfTweet()
        {
            TwitterRepository tw = new TwitterRepository();
            var firstTweet = tw.GetTweets().ToList().FirstOrDefault();
            firstTweet.created_at = "Thu Mar 01 08:01:58 +0000 2018";

            Assert.AreEqual("03/01/2018 08:01 AM", firstTweet.showDateTime());
        }

        [Test]
        public void TestTextToShowNotReTweet()
        {
            TwitterRepository tw = new TwitterRepository();
            var firstTweet = tw.GetTweets().Where(o=>o.retweeted_status==null).ToList().FirstOrDefault();

            Assert.AreNotEqual("RT ", firstTweet.textToShow().Substring(0, 3));
        }

        [Test]
        public void TestTextToShowReTweet()
        {
            TwitterRepository tw = new TwitterRepository();
            var firstTweet = tw.GetTweets().Where(o => o.retweeted_status != null).ToList().FirstOrDefault();

            Assert.AreNotEqual("RT ", firstTweet.textToShow().Substring(0, 3));
        }
    }
}