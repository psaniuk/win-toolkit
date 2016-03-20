using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace WinToolkit.Samples
{
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
