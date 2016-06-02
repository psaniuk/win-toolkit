using Windows.UI.Xaml.Controls;

namespace WinToolkit.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ImageViewPageButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ImageViewExamplePage));
        }

        private void SnapPointsButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HorizontalSnapPointsPage));
        }
    }
}