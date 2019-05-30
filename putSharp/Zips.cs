using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Script.Serialization;
using putSharp.DataTypes;

namespace putSharp
{
    public class Zips
    {
        private string _accessToken = string.Empty;

        private static string _baseURL = "https://api.put.io/v2/zips/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Zips(string accessToken)
        {
            _accessToken = accessToken;
        }

        public CreatedZip CreateZip(IEnumerable<long> IDs)
        {
            string url = $"{_baseURL}create?oauth_token={_accessToken}";

            string idString = "";
            foreach (long id in IDs)
            {
                idString += id + ",";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("file_ids", idString);
            
            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return CreatedZipParser(json);
        }

        public static CreatedZip CreateZip(string accessToken, IEnumerable<long> IDs)
        {
            string url = $"{_baseURL}create?oauth_token={accessToken}";

            string idString = "";
            foreach (long id in IDs)
            {
                idString += id + ",";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("file_ids", idString);

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return CreatedZipParser(json);
            }
        }

        public ZipsList ListZips()
        {
            string url = $"{_baseURL}list?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return ZipListParser(json);
        }

        public static ZipsList ListZips(string accessToken)
        {
            string url = $"{_baseURL}list?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return ZipListParser(json);
            }
        }

        public Zip GetZip(long zipID)
        {
            string url = $"{_baseURL}{zipID}?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return ZipParser(json);
        }

        public static Zip GetZip(string accessToken, long zipID)
        {
            string url = $"{_baseURL}{zipID}?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return ZipParser(json);
            }
        }

        #region Parsers
        private static CreatedZip CreatedZipParser(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic jsonObj = jsonSerializer.Deserialize<dynamic>(json);

            CreatedZip zip = new CreatedZip();            

            zip.Status = jsonObj["status"];
            zip.ZipID = jsonObj["zip_id"];

            return zip;
        }

        private static ZipsList ZipListParser(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic jsonObj = jsonSerializer.Deserialize<dynamic>(json);

            ZipsList list = new ZipsList();

            object[] zips = jsonObj["zips"];

            foreach (Dictionary<string, object> zip in zips)
            {
                list.Zips.Add(zip);
            }

            list.Status = jsonObj["status"];

            return list;
        }
        
        private static Zip ZipParser(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic jsonObj = jsonSerializer.Deserialize<dynamic>(json);

            Zip zip = new Zip();

            object[] files = jsonObj["zips"];

            foreach (Dictionary<string, object> file in files)
            {
                zip.MissingFiles.Add(file);
            }

            zip.Status = jsonObj["status"];
            zip.Url = jsonObj["url"];
            zip.Size = jsonObj["size"];

            return zip;
        }
        #endregion
    }
}
