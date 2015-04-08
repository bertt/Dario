using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dario.Models
{
    class ImageFetcher
    {
        readonly HttpClient _httpClient = new HttpClient();

        public async Task<Image> Fetch(int col, int row, string level, string urlTemplate)
        {
            var url = (string)urlTemplate.Clone();
            url = url.Replace("{x}", col.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{y}", row.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{z}", level);
            return Image.FromStream(await _httpClient.GetStreamAsync(url).ConfigureAwait(false));
        }
    }
}
