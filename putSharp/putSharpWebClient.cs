using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace putSharp
{
    /// <summary>
    /// This custom WebClient is to disable automatic redirecting.
    /// </summary>
    public class putSharpWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.AllowAutoRedirect = false;
            return request;
        }

        public byte[] UploadFileAndParameters(string address, string filePath, NameValueCollection values)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "POST";
            request.UserAgent = "curl/7.33.0";

            return null;


            //r request = WebRequest.Create(address);
            //request.Method = "POST";
            //var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            //request.ContentType = "multipart/form-data; boundary=" + boundary;
            //boundary = "--" + boundary;

            //using (var requestStream = request.GetRequestStream())
            //{
            //    // Write the values
            //    foreach (string name in values.Keys)
            //    {
            //        var buffer1 = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
            //        requestStream.Write(buffer1, 0, buffer1.Length);
            //        buffer1 = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
            //        requestStream.Write(buffer1, 0, buffer1.Length);
            //        buffer1 = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
            //        requestStream.Write(buffer1, 0, buffer1.Length);
            //    }

            //    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
            //    requestStream.Write(buffer, 0, buffer.Length);
            //    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", Path.GetFileNameWithoutExtension(filePath), Path.GetFileName(filePath), Environment.NewLine));
            //    requestStream.Write(buffer, 0, buffer.Length);
            //    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "application/x-bittorrent", Environment.NewLine));
            //    //buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}", "text/plain", Environment.NewLine));
            //    requestStream.Write(buffer, 0, buffer.Length);
            //    File.OpenRead(filePath).CopyTo(requestStream);
            //    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
            //    requestStream.Write(buffer, 0, buffer.Length);


            //    var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
            //    requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            //}

            //using (var response = request.GetResponse())
            //using (var responseStream = response.GetResponseStream())
            //using (var stream = new MemoryStream())
            //{
            //    responseStream.CopyTo(stream);
            //    return stream.ToArray();
            //}
        }
    }
}
