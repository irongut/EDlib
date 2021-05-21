using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>A helper class for reading an http response that can decompress data in gzip or deflate streams.</summary>
    public static class HttpHelper
    {
        /// <summary>Gets the content of an http response which may be compressed.</summary>
        /// <param name="response">The http response.</param>
        /// <returns>Task&lt;string&gt;</returns>
        public static async Task<string> ReadContentAsync(HttpResponseMessage response)
        {
            string content;

            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
            {
                Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using (DeflateStream gzipStream = new DeflateStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            else
            {
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return content;
        }
    }
}
