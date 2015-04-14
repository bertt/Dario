using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dario.Models
{
    class ImageFetcher
    {
        public async Task<byte[]> Fetch(int col, int row, string level, string urlTemplate)
        {
            var url = (string)urlTemplate.Clone();
            url = url.Replace("{x}", col.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{y}", row.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{z}", level);

            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetByteArrayAsync(url).ConfigureAwait(false);
            }
        }
    }
}
