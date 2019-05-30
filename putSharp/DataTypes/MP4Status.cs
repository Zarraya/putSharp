using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class MP4Status
    {
        private Dictionary<string, object> _mp4 = new Dictionary<string, object>();
        private string _status = "";

        public Dictionary<string, object> Mp4 { get => _mp4; set => _mp4 = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
