using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class FriendList
    {
        private List<Dictionary<string, object>> _friends = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Friends { get => _friends; set => _friends = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
