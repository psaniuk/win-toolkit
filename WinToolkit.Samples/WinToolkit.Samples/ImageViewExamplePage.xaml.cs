using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinToolkit.Samples
{
    public sealed partial class ImageViewExamplePage : Page
    {
        private readonly DataService _dataService = new DataService();
        public ImageViewExamplePage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                ArticlesListView.ItemsSource = await _dataService.GetImages();
            }
            catch(HttpRequestException exp)
            {
                System.Diagnostics.Debug.WriteLine(exp);
            }
        }
    }
}
