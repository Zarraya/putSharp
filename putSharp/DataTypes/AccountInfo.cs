using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace putSharp.DataTypes
{
    public class AccountInfo
    {
        private Dictionary<string, object> _info = new Dictionary<string, object>();
        private string status = "";

        public Dictionary<string, object> Info { get => _info; set => _info = value; }
        public string Status { get => status; set => status = value; }

        public string Username => (string) _info["username"];
        public DateTime PlanExpirationDate => (DateTime) _info["plan_expiration_date"];
        public long AvailableBytes => (long) ((JObject)_info["disk"])["avail"];
        public long UsedBytes => (long) ((JObject)_info["disk"])["used"];
        public long TotalBytes => (long) ((JObject)_info["disk"])["size"];

    }
}
