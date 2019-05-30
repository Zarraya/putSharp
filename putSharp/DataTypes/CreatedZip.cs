using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class CreatedZip
    {
        private string _status = "";
        private long _zipID = -1;

        public string Status { get => _status; set => _status = value; }
        public long ZipID { get => _zipID; set => _zipID = value; }
    }
}
