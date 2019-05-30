using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class SharedFilesList
    {
        private List<Dictionary<string, object>> _files = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Files { get => _files; set => _files = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
