using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace putSharp
{
    public static class DownloadManager
    {
        static HttpClient _client = new HttpClient();
        
        public static async Task<byte[]> DownloadAsync(
            string url,
            IProgress<double> progress = default(IProgress<double>), 
            CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException($"the {nameof(url)} parameters can't be null.");
            
            using (HttpResponseMessage response = await _client.GetAsync(url,HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error in download: {response.StatusCode}");

                long total = response.Content.Headers.ContentLength ?? -1L;

                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    long totalRead = 0L;
                    byte[] buffer = new byte[2048];
                    bool isMoreToRead = true;
                    //FileStream output = new FileStream(fileWriteTo, FileMode.Create);
                    MemoryStream output = new MemoryStream();
                    do
                    {
                        token.ThrowIfCancellationRequested();

                        int read = await streamToReadFrom.ReadAsync(buffer, 0, buffer.Length, token);

                        if (read == 0)
                            isMoreToRead = false;

                        else
                        {
                            await output.WriteAsync(buffer, 0, read, token);

                            totalRead += read;

                            progress?.Report((totalRead * 1d) / (total * 1d) * 100);
                        }

                    } while (isMoreToRead);

                    output.Close();
                    return output.ToArray();
                }
            }
        }
    }
}