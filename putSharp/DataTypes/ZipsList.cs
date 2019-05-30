using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class ZipsList
    {
        private List<Dictionary<string, object>> _zips = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Zips { get => _zips; set => _zips = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
