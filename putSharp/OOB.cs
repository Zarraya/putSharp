using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace putSharp
{
    public static class OOB
    {
        private static string _baseURL = "https://api.put.io/v2/oauth2/oob/code";

        public static string GetOOBCode(string appID)
        {
            string url = $"{_baseURL}?app_id={appID}";
            string code = "";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);
                code = jsonObj["code"]?.ToObject<string>();
            }

            return code;
        }

        public static string GetOAuth(string OOB)
        {
            string url = $"{_baseURL}/{OOB}";
            string oauth = "";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);
                oauth = jsonObj["oauth_token"]?.ToObject<string>();
            }

            return oauth;
        }
    }
}