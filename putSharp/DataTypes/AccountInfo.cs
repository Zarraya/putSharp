using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class AccountInfo
    {
        private Dictionary<string, object> _info = new Dictionary<string, object>();
        private string status = "";

        public Dictionary<string, object> Info { get => _info; set => _info = value; }
        public string Status { get => status; set => status = value; }
    }
}
