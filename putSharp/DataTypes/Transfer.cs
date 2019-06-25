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

        public int Availability => (int)_data["availability"];
        public DateTime CreatedAt => (DateTime) _data["created_at"];
        public double CurrentRatio => (double) _data["current_ratio"];
        public long Downloaded => (long) _data["downloaded"];
        public long Uploaded => (long) _data["uploaded"];
        public long DownSpeed => (long) _data["down_speed"];
        public long UpSpeed => (long) _data["up_speed"];
        public string ErrorMesssage => (string) _data["error_message"];
        public int EstimatedTime => (int) _data["estimated_time"];
        public long FileId => (long) _data["file_id"];
        public DateTime FinishedAt => (DateTime) _data["finished_at"];
        public long Id => (long) _data["id"];
        public bool IsPrivate => (bool) _data["is_private"];
        public string Name => (string) _data["name"];
        public long PeersConnected => (long) _data["peers_connected"];
        public long PercentDone => (long) _data["percent_done"];
        public long SaveParentId => (long) _data["save_parent_id"];
        public long SecondsSeeding => (long) _data["seconds_seeding"];
        public long Size => (long) _data["size"];
        public string Source => (string) _data["source"];
        public string Status => (string) _data["status"];
        public string StatusMessage => (string) _data["status_message"];
        public long SubscriptionId => (long) _data["subscription_id"];
        public string TrackerMessage => (string) _data["tracker_message"];
        public string Type => (string) _data["type"];
        public long CompletionPercent => (long) _data["completion_percent"];

        #region Overrides of Object

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
