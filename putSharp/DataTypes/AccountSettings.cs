using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class AccountSettings
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();
        private string _status = "";

        public Dictionary<string, object> Settings { get => _settings; set => _settings = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
