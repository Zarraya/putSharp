using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class DownloadResult
    {
        private byte[] _data = null;
        private HttpContentHeaders _headers = null;

        public byte[] Data { get => _data; set => _data = value; }
        public HttpContentHeaders Headers { get => _headers; set => _headers = value; }
    }
}
