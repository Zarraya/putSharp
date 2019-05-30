using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    internal class ParallelDownloadStreamRange
    {
        public long Start { get; set; }
        public long End { get; set; }

        public override string ToString()
        {
            return $"{Start} -> {End}";
        }
    }
}
