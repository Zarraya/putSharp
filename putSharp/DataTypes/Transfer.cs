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

        public long Availability => _data.ContainsKey("availability") && _data["availability"] != null ? (long)_data["availability"] : 0;
        public DateTime CreatedAt => _data.ContainsKey("created_at") && _data["created_at"] != null ? (DateTime) _data["created_at"] : DateTime.MinValue;
        public long Downloaded => _data.ContainsKey("downloaded") && _data["downloaded"] != null ? (long) _data["downloaded"] : 0;
        public long Uploaded => _data.ContainsKey("uploaded") && _data["uploaded"] != null ? (long) _data["uploaded"] : 0;
        public DateTime FinishedAt => _data.ContainsKey("finished_at") && _data["finished_at"] != null ? (DateTime) _data["finished_at"] : DateTime.MinValue;
        public long Id => _data.ContainsKey("id") && _data["id"] != null ? (long) _data["id"] : 0;
        public string Name => _data.ContainsKey("name") && _data["name"] != null ? (string) _data["name"] : "";
        public long PeersGettingFromUs => _data.ContainsKey("peers_getting_from_us") && _data["peers_getting_from_us"] != null ? (long) _data["peers_getting_from_us"] : 0;
        public long PeersSendingToUs => _data.ContainsKey("peers_sending_to_us") && _data["peers_sending_to_us"] != null ? (long) _data["peers_sending_to_us"] : 0;
        public long PercentDone => _data.ContainsKey("percent_done") && _data["percent_done"] != null ? (long) _data["percent_done"] : 0;
        public long SecondsSeeding => _data.ContainsKey("seconds_seeding") && _data["seconds_seeding"] != null ? (long) _data["seconds_seeding"] : 0;
        public long Size => _data.ContainsKey("size") && _data["size"] != null ? (long) _data["size"] : 1;
        public string Status => _data.ContainsKey("status") && _data["status"] != null ? (string) _data["status"] : "";

        #region Overrides of Object

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
