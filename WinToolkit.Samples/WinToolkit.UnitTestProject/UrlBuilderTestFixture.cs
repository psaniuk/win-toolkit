using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WinToolkit.Http;

namespace WinToolkit.UnitTestProject
{
    [TestClass]
    public class UrlBuilderTestFixture
    {
        [TestMethod]
        public void BuildTest()
        {
            var urlBuilder = new UrlBuilder("api.twitter.com", "1.1");
            urlBuilder.WithPath("statuses/user_timeline.json");
            urlBuilder.WithParam("screen_name", "twitterapi");
            urlBuilder.WithParam("count", "2");

            string url = urlBuilder.Build();
            Assert.IsFalse(string.IsNullOrEmpty(url));
            Assert.AreEqual(url, "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=twitterapi&count=2");

        }

        [TestMethod]
        public void CreateGetUserTimelineBuilderTest()
        {
            var factory = new StatusesBuildersFactory();
            UrlBuilder builder = factory.CreateGetUserTimelineBuilder("twitterapi", 2);
            Assert.AreEqual(builder.Build(), "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=twitterapi&count=2");
        }

        [TestMethod]
        public void CreateGetRetweetsUrlBuilderTest()
        {
            var factory = new StatusesBuildersFactory();
            UrlBuilder builder = factory.CreateGetRetweetsUrlBuilder("509457288717819904", 10);
            Assert.AreEqual(builder.Build(), "https://api.twitter.com/1.1/statuses/retweets/509457288717819904.json?count=10");
        }
    }
}
