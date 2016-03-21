using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinToolkit.Samples
{
    public sealed partial class ImageViewExamplePage : Page
    {
        public ImageViewExamplePage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                var httpClient = new HttpClient();
                HttpResponseMessage responseMsg = await httpClient.GetAsync("https://api.zalando.com/articles?brandFamily=adidas");
                string json = await responseMsg.Content.ReadAsStringAsync();
                ArticlesResponse data = JsonConvert.DeserializeObject<ArticlesResponse>(json);
                ArticlesListView.ItemsSource = data.Content.Select(article => article.Media.Images.FirstOrDefault()).Where(img => img != null);
            }
            catch(HttpRequestException exp)
            {
                System.Diagnostics.Debug.WriteLine(exp);
            }
        }
    }

    public class ArticlesResponse
    {
        public Article[] Content { get; set; }
    }

    public class Article
    {
        public Media Media { get; set; }
    }

    public class Media
    {
        public Image[] Images { get; set; }
    }

    public class Image
    {
        public string SmallHdUrl { get; set; }
    }

}
