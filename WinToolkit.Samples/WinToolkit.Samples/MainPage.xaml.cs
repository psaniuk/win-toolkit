using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinToolkit.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DropDownButtonSource = new[]
            {
                "Option 1",
                "Option 2",
                "Option 3",
                "Option 4"
            };
        }

        public string[] DropDownButtonSource { get; private set; }

        private void ImageViewPageButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ImageViewExamplePage));
        }

        private void SnapPointsButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HorizontalSnapPointsPage));
        }
    }
}