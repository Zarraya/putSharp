﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class TransfersList
    {
        private List<Dictionary<string, object>> _transfers = new List<Dictionary<string, object>>();
        private string _status = "";

        public List<Dictionary<string, object>> Transfers { get => _transfers; set => _transfers = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
