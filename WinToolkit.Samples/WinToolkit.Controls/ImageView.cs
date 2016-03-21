using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinToolkit.Controls.Imaging;

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
                    await SetImageSourceAsync(buffer);
                
            }
            catch(HttpRequestException)
            {
                //hanlde http exception
            }
        }

        private async Task SetImageSourceAsync(BitmapDecoder decoder, byte[] bitmapData)
        {
            using (IRandomAccessStream outputStream = new InMemoryRandomAccessStream())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(outputStream, decoder);
                encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, decoder.PixelWidth, 
                    decoder.PixelHeight, decoder.DpiX, decoder.DpiY, bitmapData);
                await encoder.FlushAsync();

                
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(outputStream);

                _image.Source = bitmapImage;
            }
        }

        private async Task SetImageSourceAsync(byte[] buffer)
        {
            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer.AsBuffer());
                stream.Seek(0);

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                PixelDataProvider pixelProvider = await decoder.GetPixelDataAsync();

                var processor = new ImageBackgroundProcessor((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                byte[] pixelData = pixelProvider.DetachPixelData();
                processor.Process(pixelData);

                await SetImageSourceAsync(decoder, pixelData);
            }
        }

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue != null)
                ((ImageView)sender).OnSourceChanged();
        }
    }
}
