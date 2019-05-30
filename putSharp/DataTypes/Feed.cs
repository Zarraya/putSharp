using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class Feed
    {
        private Dictionary<string, object> _feed = new Dictionary<string, object>();
        private string _status = "";

        public Dictionary<string, object> FeedData { get => _feed; set => _feed = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
