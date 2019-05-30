using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class SubtitlesList
    {
        private List<Dictionary<string, object>> _subtitles = new List<Dictionary<string, object>>();
        private string _status = "";
        private string _default = "";

        public List<Dictionary<string, object>> Subtitles { get => _subtitles; set => _subtitles = value; }
        public string Status { get => _status; set => _status = value; }
        public string Default { get => _default; set => _default = value; }
    }
}
