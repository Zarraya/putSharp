using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class File
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private string _status = "";

        public Dictionary<string, object> Data { get => _data; set => _data = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
