using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WinToolkit.Samples
{
    internal class DataService
    {
        public async Task<Image[]> GetImages()
        {
            
            var httpClient = new HttpClient();
            HttpResponseMessage responseMsg = await httpClient.GetAsync("https://api.zalando.com/articles?brandFamily=adidas");
            string json = await responseMsg.Content.ReadAsStringAsync();
            ArticlesResponse data = JsonConvert.DeserializeObject<ArticlesResponse>(json);

            return data.Content.Select(article => article.Media.Images.FirstOrDefault()).Where(img => img != null).ToArray();
        }
    }
}
