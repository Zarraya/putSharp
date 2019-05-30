using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using putSharp.DataTypes;

namespace putSharp
{
    public class Transfers
    {
        private string _accessToken = string.Empty;
        private static string _baseURL = "https://api.put.io/v2/transfers/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Transfers(string accessToken)
        {
            _accessToken = accessToken;
        }

        public TransfersList List()
        {
            string url = $"{_baseURL}list?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return TransferParser(json);
        }

        public static TransfersList List(string accessToken)
        {
            string url = $"{_baseURL}list?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return TransferParser(json);
            }
            
        }

        public Transfer Add(string magnetURL, long parentID = 0, string callbackURL = "")
        {
            string url = $"{_baseURL}add?oauth_token={_accessToken}";
            
            NameValueCollection par = new NameValueCollection();
            par.Add("url", magnetURL);            
            par.Add("save_parent_id", parentID.ToString());           

            if (!string.IsNullOrEmpty(callbackURL))
            {
                par.Add("callback_url", callbackURL);
            }

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return SingleTransferParser(json);
        }

        public static Transfer Add(string accessToken, string magnetURL, long parentID = 0, string callbackURL = "")
        {
            string url = $"{_baseURL}add?oauth_token={accessToken}";

            NameValueCollection par = new NameValueCollection();
            par.Add("url", magnetURL);
            par.Add("save_parent_id", parentID.ToString());
            
            if (!string.IsNullOrEmpty(callbackURL))
            {
                par.Add("callback_url", callbackURL);
            }

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return SingleTransferParser(json);
            }
        }

        public Transfer AddTorrent(string filePath, long parentID = 0, string callbackURL = "")
        {
            string url = $"{_baseURL}add?oauth_token={_accessToken}";

            NameValueCollection par = new NameValueCollection();
            par.Add("torrent", "true");
            par.Add("save_parent_id", parentID.ToString());

            if (!string.IsNullOrEmpty(callbackURL))
            {
                par.Add("callback_url", callbackURL);
            }

            byte[] jsonBytes = _client.UploadFileAndParameters(url, filePath, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return SingleTransferParser(json);
        }

        public static Transfer AddTorrent(string accessToken, string filePath, long parentID = 0, string callbackURL = "")
        {
            string url = $"{_baseURL}add?oauth_token={accessToken}";

            NameValueCollection par = new NameValueCollection();
            par.Add("url", filePath);
            par.Add("save_parent_id", parentID.ToString());

            if (!string.IsNullOrEmpty(callbackURL))
            {
                par.Add("callback_url", callbackURL);
            }

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return SingleTransferParser(json);
            }
        }

        public Transfer Get(long id)
        {
            string url = $"{_baseURL}{id}?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return SingleTransferParser(json);
        }

        public static Transfer Get(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return SingleTransferParser(json);
            }
        }

        public void Retry(long id)
        {
            string url = $"{_baseURL}retry?oauth_token={_accessToken}";

            NameValueCollection par = new NameValueCollection();
            par.Add("id", id.ToString());

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);
        }

        public static void Retry(string accessToken, long id)
        {
            string url = $"{_baseURL}retry?oauth_token={accessToken}";

            NameValueCollection par = new NameValueCollection();
            par.Add("id", id.ToString());

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);
            }
        }

        public string Cancel(IEnumerable<long> IDs)
        {
            string url = $"{_baseURL}cancel?oauth_token={_accessToken}";

            string idString = "";
            foreach(long id in IDs)
            {
                idString += id + ",";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("transfer_ids", idString);

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return json;
        }

        public static string Cancel(string accessToken, IEnumerable<long> IDs)
        {
            string url = $"{_baseURL}cancel?oauth_token={accessToken}";

            string idString = "";
            foreach (long id in IDs)
            {
                idString += id + ",";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("transfer_ids", idString);

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return json;
            }
        }

        public string Clean()
        {
            string url = $"{_baseURL}clean?oauth_token={_accessToken}";

            byte[] jsonBytes = _client.UploadValues(url, new NameValueCollection());
            string json = Encoding.ASCII.GetString(jsonBytes);

            return json;
        }

        public static string Clean(string accessToken)
        {
            string url = $"{_baseURL}clean?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, new NameValueCollection());
                string json = Encoding.ASCII.GetString(jsonBytes);

                return json;
            }
        }

        #region Parsers
        private static TransfersList TransferParser(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic jsonObj = jsonSerializer.Deserialize<dynamic>(json);

            TransfersList list = new TransfersList();

            object[] transfers = jsonObj["transfers"];

            foreach (Dictionary<string, object> transfer in transfers)
            {
                list.Transfers.Add(transfer);
            }

            list.Status = jsonObj["status"];

            return list;
        }

        private static Transfer SingleTransferParser(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic jsonObj = jsonSerializer.Deserialize<dynamic>(json);

            Transfer list = new Transfer();
            
            foreach (KeyValuePair<string, object> pair in jsonObj)
            {
                list.Data.Add(pair.Key, pair.Value);
            }

            return list;
        }
        #endregion
    }
}
