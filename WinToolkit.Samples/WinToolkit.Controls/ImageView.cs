using System;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinToolkit.Controls
{
    public class ImageView: Control
    {
        private Image _image;
        private readonly HttpClient _httpClient = new HttpClient();

        public DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(ImageView), 
            new PropertyMetadata(default(Uri), new PropertyChangedCallback(OnSourcePropertyChanged)));

        public ImageView()
        {
            DefaultStyleKey = typeof(ImageView);
        }

        public Uri Source
        {
            get
            {
                return (Uri)GetValue(SourceProperty);
            }

            set
            {
                SetValue(SourceProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _image = (Image) GetTemplateChild("ImageControl");
        }

        private async void OnSourceChanged()
        {
            if (Source == null)
                return;


            try
            {
                byte[] buffer = await _httpClient.GetByteArrayAsync(Source);
                if (buffer != null && buffer.Any())
                {

                }
            }
            catch(HttpRequestException)
            {

            }
        }

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue != null)
                ((ImageView)sender).OnSourceChanged();
        }
    }
}
