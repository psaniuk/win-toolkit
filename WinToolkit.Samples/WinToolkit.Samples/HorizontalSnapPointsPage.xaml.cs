using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace WinToolkit.Samples
{

    public sealed partial class HorizontalSnapPointsPage : Page
    {
        private readonly DataService _dataService = new DataService();

        public HorizontalSnapPointsPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            List<Image> images = (await _dataService.GetImages()).ToList();
        //    images.Insert(0, Image.Empty);
          //  images.Insert(images.Count, Image.Empty);

            ImagesList.ItemsSource = images;
            //ImagesList.ScrollIntoView(ImagesList.Items[1]);
        }
    }
}
