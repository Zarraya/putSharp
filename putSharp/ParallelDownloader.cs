using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using putSharp.DataTypes;
using System.Collections.Concurrent;

namespace putSharp
{
    //Credit to Dejan Stojanovic for his article at https://dejanstojanovic.net/aspnet/2018/march/download-file-in-chunks-in-parallel-in-c/
    public static class ParallelDownloader
    {
        static ParallelDownloader()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.MaxServicePointIdleTime = 1000;
        }

        public static ParallelDownload Download(string url, string downloadPath, int parallelStreams = 0, bool validateSSL = false)
        {
            if (!validateSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            ParallelDownload result = new ParallelDownload();
            result.FilePath = downloadPath;

            if(parallelStreams <= 0)
            {
                parallelStreams = Environment.ProcessorCount;
            }

            WebRequest request = WebRequest.Create(url);
            request.Method = "HEAD";

            string fileName;

            long responseLength;
            using (WebResponse response = request.GetResponse())
            {
                responseLength = long.Parse(response.Headers.Get("Content-Length"));
                result.Size = responseLength;
                string disposition = response.Headers["Content-Disposition"];
                fileName = disposition.Substring(disposition.IndexOf("filename") + 9);
            }
            
            if (!downloadPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                downloadPath += Path.DirectorySeparatorChar;
            }

            downloadPath += fileName;

            if (System.IO.File.Exists(downloadPath))
            {
                System.IO.File.Delete(downloadPath);
            }

            using (FileStream destinationStream = new FileStream(downloadPath, FileMode.Append))
            {
                ConcurrentDictionary<ParallelDownloadStreamRange, string> tempFiles = new ConcurrentDictionary<ParallelDownloadStreamRange, string>();

                List<ParallelDownloadStreamRange> readRanges = new List<ParallelDownloadStreamRange>();
                for(int chunk = 0; chunk < parallelStreams; chunk++)
                {
                    ParallelDownloadStreamRange range = new ParallelDownloadStreamRange();
                    range.Start = chunk * (responseLength / parallelStreams);
                    range.End = ((chunk + 1) * (responseLength / parallelStreams)) - 1;
                    readRanges.Add(range);
                }

                ParallelDownloadStreamRange endRange = new ParallelDownloadStreamRange();
                endRange.Start = readRanges.Any() ? readRanges.Last().End + 1 : 0;
                endRange.End = responseLength - 1;

                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                
                Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = parallelStreams }, readRange =>
                 {
                     HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                     webRequest.Method = "GET";
                     webRequest.AddRange(readRange.Start, readRange.End);

                     using(HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse)
                     {
                         string tempFilePath = Path.GetTempFileName();
                         
                         using(FileStream stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                         {
                             webResponse.GetResponseStream().CopyTo(stream);
                             tempFiles.TryAdd(readRange, tempFilePath);
                         }
                     }
                 });

                result.FileChunks = tempFiles.Count;
                timer.Stop();                
                result.DownloadedIn = timer.ElapsedMilliseconds;

                List<ParallelDownloadStreamRange> ranges = SortKeys(tempFiles.Keys);
                foreach(ParallelDownloadStreamRange range in ranges)
                {
                    string file = tempFiles[range];
                    byte[] bytes = System.IO.File.ReadAllBytes(file);
                    destinationStream.Write(bytes, 0, bytes.Length);
                    System.IO.File.Delete(file);
                }

                destinationStream.Flush();
                destinationStream.Close();

                return result;
            }
        }

        private static List<ParallelDownloadStreamRange> SortKeys(IEnumerable<ParallelDownloadStreamRange> ranges)
        {
            List<ParallelDownloadStreamRange> newRanges = new List<ParallelDownloadStreamRange>();

            List<ParallelDownloadStreamRange> tempRanges = new List<ParallelDownloadStreamRange>(ranges);
            while(tempRanges.Count > 0)
            {
                foreach(ParallelDownloadStreamRange range in ranges)
                {
                    if(newRanges.Count == 0)
                    {
                        if(range.Start == 0)
                        {
                            newRanges.Add(range);
                            tempRanges.Remove(range);
                        }
                    }
                    else
                    {
                        if(range.Start == newRanges.Last().End + 1)
                        {
                            newRanges.Add(range);
                            tempRanges.Remove(range);
                        }
                    }
                }
            }

            return newRanges;
        }
    }
}
