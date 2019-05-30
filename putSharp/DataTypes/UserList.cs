using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class UserList
    {
        private List<Dictionary<string, object>> _users = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Users { get => _users; set => _users = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
