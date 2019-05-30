using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class SearchResult
    {
        private List<Dictionary<string, object>> _files = new List<Dictionary<string, object>>();
        private string _status = "";
        private string _nextPageURL = "";

        public List<Dictionary<string, object>> Files { get => _files; set => _files = value; }
        public string Status { get => _status; set => _status = value; }
        public string NextPageURL { get => _nextPageURL; set => _nextPageURL = value; }
    }
}
