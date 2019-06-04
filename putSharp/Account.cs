using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using putSharp.DataTypes;

namespace putSharp
{
    public class Account
    {
        private string _accessToken = string.Empty;

        private static string _baseURL = "https://api.put.io/v2/account/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Account(string accessToken)
        {
            _accessToken = accessToken;
        }

        public AccountInfo Info()
        {
            string url = $"{_baseURL}info?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return AccountInfoParser(json);
        }

        public static AccountInfo Info(string accessToken)
        {
            string url = $"{_baseURL}info?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return AccountInfoParser(json);
            }
        }

        public AccountSettings Settings()
        {
            string url = $"{_baseURL}settings?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return AccountSettingsParser(json);
        }

        public static AccountSettings Settings(string accessToken)
        {
            string url = $"{_baseURL}settings?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return AccountSettingsParser(json);
            }
        }

        public string UpdateSettings(Dictionary<string, object> settingsValues)
        {
            string url = $"{_baseURL}settings?oauth_token={_accessToken}";
            
            NameValueCollection par = new NameValueCollection();
            foreach(KeyValuePair<string, object> setting in settingsValues)
            {
                par.Add(setting.Key, setting.Value.ToString().ToLower());
            }

            byte[] jsonBytes = _client.UploadValues(url, par);
            return Encoding.ASCII.GetString(jsonBytes);
        }

        public static string UpdateSettings(string accessToken, Dictionary<string, object> settingsValues)
        {
            string url = $"{_baseURL}settings?oauth_token={accessToken}";

            NameValueCollection par = new NameValueCollection();
            foreach (KeyValuePair<string, object> setting in settingsValues)
            {
                par.Add(setting.Key, setting.Value.ToString().ToLower());
            }

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                return Encoding.ASCII.GetString(jsonBytes);
            }
        }

        #region Parsers
        private static AccountInfo AccountInfoParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            AccountInfo accountInfo = new AccountInfo();

            Dictionary<string, object> info = ((JObject) jsonObj["info"]).ToObject<Dictionary<string, object>>();

            foreach (KeyValuePair<string, object> data in info)
            {
                accountInfo.Info.Add(data.Key, data.Value);
            }

            accountInfo.Status = jsonObj["status"];

            return accountInfo;
        }

        private static AccountSettings AccountSettingsParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            AccountSettings accountSettings = new AccountSettings();

            Dictionary<string, object> settings = ((JObject)jsonObj["settings"]).ToObject<Dictionary<string, object>>();

            foreach (KeyValuePair<string, object> setting in settings)
            {
                accountSettings.Settings.Add(setting.Key, setting.Value);
            }

            accountSettings.Status = jsonObj["status"];

            return accountSettings;
        }
        #endregion
    }
}
