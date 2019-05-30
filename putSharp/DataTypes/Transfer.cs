using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class Transfer
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public Dictionary<string, object> Data { get => _data; set => _data = value; }
    }
}
