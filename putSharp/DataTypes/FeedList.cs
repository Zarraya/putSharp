using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class FeedList
    {
        private List<Dictionary<string, object>> _feeds = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Feeds { get => _feeds; set => _feeds = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
