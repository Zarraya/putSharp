﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using putSharp.DataTypes;

namespace putSharp
{
    public class Friends
    {
        private string _accessToken = string.Empty;

        private static string _baseURL = "https://api.put.io/v2/friends/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Friends(string accessToken)
        {
            _accessToken = accessToken;
        }

        public FriendList List()
        {
            string url = $"{_baseURL}list?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return FriendListParser(json);
        }

        public static FriendList List(string accessToken)
        {
            string url = $"{_baseURL}list?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return FriendListParser(json);
            }
        }

        public FriendList FriendRequests()
        {
            string url = $"{_baseURL}waiting-requests?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return FriendListParser(json);
        }

        public static FriendList FriendRequests(string accessToken)
        {
            string url = $"{_baseURL}waiting-requests?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return FriendListParser(json);
            }
        }

        public string SendRequest(string userName)
        {
            string url = $"{_baseURL}{userName}/request?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string SendRequest(string accessToken, string userName)
        {
            string url = $"{_baseURL}{userName}/request?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string Approve(string userName)
        {
            string url = $"{_baseURL}{userName}/approve?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Approve(string accessToken, string userName)
        {
            string url = $"{_baseURL}{userName}/approve?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string Deny(string userName)
        {
            string url = $"{_baseURL}{userName}/deny?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Deny(string accessToken, string userName)
        {
            string url = $"{_baseURL}{userName}/deny?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string Unfriend(string userName)
        {
            string url = $"{_baseURL}{userName}/unfriend?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Unfriend(string accessToken, string userName)
        {
            string url = $"{_baseURL}{userName}/unfriend?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        #region Parsers
        private static FriendList FriendListParser(string json)
        {
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);

            FriendList list = new FriendList();

            object[] friends = ((JArray)jsonObj["friends"]).ToObject<object[]>();

            foreach (JObject friend in friends)
            {
                list.Friends.Add(friend.ToObject<Dictionary<string, object>>());
            }

            list.Status = jsonObj["status"].ToObject<string>();

            return list;
        }

        private static string StatusParser(string json)
        {
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);

            return jsonObj["status"].ToObject<string>();
        }
        #endregion
    }
}
