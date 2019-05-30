using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class EventsList
    {
        private List<Dictionary<string, object>> _events = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Events { get => _events; set => _events = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
