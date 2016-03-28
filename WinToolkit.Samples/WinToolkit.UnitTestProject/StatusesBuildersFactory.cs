using WinToolkit.Http;

namespace WinToolkit.UnitTestProject
{
    public class TwitterUrlBuildersFactory
    {
        public UrlBuilder Create() => new UrlBuilder("api.twitter.com", "1.1");
    }

    public sealed class StatusesBuildersFactory: TwitterUrlBuildersFactory
    {
        public UrlBuilder CreateGetUserTimelineBuilder(string screenName, int count)
        {
            UrlBuilder builder = Create();
            builder.WithPath("statuses/user_timeline.json");
            builder.WithParam("screen_name", "twitterapi");
            builder.WithParam("count", count.ToString());

            return builder;
        }

        public UrlBuilder CreateGetRetweetsUrlBuilder(string id, int count)
        {
            UrlBuilder builder = Create();
            builder.WithPath($"statuses/retweets/{id}.json");
            builder.WithParam("count", count.ToString());
            return builder;
        }
    }
}
