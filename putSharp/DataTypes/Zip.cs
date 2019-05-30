using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class Zip
    {
        private string _status = "";
        private string _url = "";
        private long _size = -1;
        private List<Dictionary<string, object>> _missingFiles = new List<Dictionary<string, object>>();

        public string Status { get => _status; set => _status = value; }
        public string Url { get => _url; set => _url = value; }
        public long Size { get => _size; set => _size = value; }
        public List<Dictionary<string, object>> MissingFiles { get => _missingFiles; set => _missingFiles = value; }
    }
}
