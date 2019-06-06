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
    public class Feeds
    {
        private string _accessToken = string.Empty;

        private static string _baseURL = "https://api.put.io/v2/rss/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Feeds(string accessToken)
        {
            _accessToken = accessToken;
        }

        public Feed Create(string title, string rssSourceURL, long parentDirID, bool deleteOldFiles = false,
                            bool dontProcessWholeFeed = false, IEnumerable<string> keywords = null, 
                            IEnumerable<string> unwantedKeywords = null, bool paused = false)
        {
            string url = $"{_baseURL}create?oauth_token={_accessToken}";

            string keywordString = "";
            foreach (string keyword in keywords)
            {
                keywordString += keyword + ",";
            }
            keywordString = keywordString.Substring(0, keywordString.Length - 1);

            string unKeywordString = "";
            foreach (string keyword in unwantedKeywords)
            {
                unKeywordString += keyword + ",";
            }
            unKeywordString = unKeywordString.Substring(0, unKeywordString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("title", title);
            par.Add("rss_source_url", rssSourceURL);
            par.Add("parent_dir_id", parentDirID.ToString());

            if (deleteOldFiles)
            {
                par.Add("delete_old_files", "true");
            }
            else
            {
                par.Add("delete_old_files", "false");
            }

            if (dontProcessWholeFeed)
            {
                par.Add("dont_process_whole_feed", "true");
            }
            else
            {
                par.Add("dont_process_whole_feed", "false");
            }

            par.Add("keyword", keywordString);
            par.Add("unwanted_keywords", unKeywordString);

            if (paused)
            {
                par.Add("paused", "true");
            }
            else
            {
                par.Add("paused", "false");
            }

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return FeedParser(json);
            
        }

        public static Feed Create(string accessToken, string title, string rssSourceURL, long parentDirID, bool deleteOldFiles = false,
                            bool dontProcessWholeFeed = false, IEnumerable<string> keywords = null,
                            IEnumerable<string> unwantedKeywords = null, bool paused = false)
        {
            string url = $"{_baseURL}create?oauth_token={accessToken}";

            string keywordString = "";
            foreach (string keyword in keywords)
            {
                keywordString += keyword + ",";
            }
            keywordString = keywordString.Substring(0, keywordString.Length - 1);

            string unKeywordString = "";
            foreach (string keyword in unwantedKeywords)
            {
                unKeywordString += keyword + ",";
            }
            unKeywordString = unKeywordString.Substring(0, unKeywordString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("title", title);
            par.Add("rss_source_url", rssSourceURL);
            par.Add("parent_dir_id", parentDirID.ToString());

            if (deleteOldFiles)
            {
                par.Add("delete_old_files", "true");
            }
            else
            {
                par.Add("delete_old_files", "false");
            }

            if (dontProcessWholeFeed)
            {
                par.Add("dont_process_whole_feed", "true");
            }
            else
            {
                par.Add("dont_process_whole_feed", "false");
            }

            par.Add("keyword", keywordString);
            par.Add("unwanted_keywords", unKeywordString);

            if (paused)
            {
                par.Add("paused", "true");
            }
            else
            {
                par.Add("paused", "false");
            }

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return FeedParser(json);
            }
        }

        public FeedList List()
        {
            string url = $"{_baseURL}list?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return FeedListParser(json);
        }

        public static FeedList List(string accessToken)
        {
            string url = $"{_baseURL}list?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return FeedListParser(json);
            }
        }

        public Feed Get(long id)
        {
            string url = $"{_baseURL}{id}?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return FeedParser(json);
        }

        public static Feed Get(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.DownloadString(url);

                return FeedParser(json);
            }
        }

        public Feed Update(long id, string title, string rssSourceURL, long parentDirID, bool deleteOldFiles = false,
                            bool dontProcessWholeFeed = false, IEnumerable<string> keywords = null,
                            IEnumerable<string> unwantedKeywords = null, bool paused = false)
        {
            string url = $"{_baseURL}{id}?oauth_token={_accessToken}";

            string keywordString = "";
            foreach (string keyword in keywords)
            {
                keywordString += keyword + ",";
            }
            keywordString = keywordString.Substring(0, keywordString.Length - 1);

            string unKeywordString = "";
            foreach (string keyword in unwantedKeywords)
            {
                unKeywordString += keyword + ",";
            }
            unKeywordString = unKeywordString.Substring(0, unKeywordString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("title", title);
            par.Add("rss_source_url", rssSourceURL);
            par.Add("parent_dir_id", parentDirID.ToString());

            if (deleteOldFiles)
            {
                par.Add("delete_old_files", "true");
            }
            else
            {
                par.Add("delete_old_files", "false");
            }

            if (dontProcessWholeFeed)
            {
                par.Add("dont_process_whole_feed", "true");
            }
            else
            {
                par.Add("dont_process_whole_feed", "false");
            }

            par.Add("keyword", keywordString);
            par.Add("unwanted_keywords", unKeywordString);

            if (paused)
            {
                par.Add("paused", "true");
            }
            else
            {
                par.Add("paused", "false");
            }

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return FeedParser(json);
        }

        public static Feed Update(string accessToken, long id, string title, string rssSourceURL, long parentDirID, bool deleteOldFiles = false,
                            bool dontProcessWholeFeed = false, IEnumerable<string> keywords = null,
                            IEnumerable<string> unwantedKeywords = null, bool paused = false)
        {
            string url = $"{_baseURL}{id}?oauth_token={accessToken}";

            string keywordString = "";
            foreach (string keyword in keywords)
            {
                keywordString += keyword + ",";
            }
            keywordString = keywordString.Substring(0, keywordString.Length - 1);

            string unKeywordString = "";
            foreach (string keyword in unwantedKeywords)
            {
                unKeywordString += keyword + ",";
            }
            unKeywordString = unKeywordString.Substring(0, unKeywordString.Length - 1);

            NameValueCollection par = new NameValueCollection();
            par.Add("title", title);
            par.Add("rss_source_url", rssSourceURL);
            par.Add("parent_dir_id", parentDirID.ToString());

            if (deleteOldFiles)
            {
                par.Add("delete_old_files", "true");
            }
            else
            {
                par.Add("delete_old_files", "false");
            }

            if (dontProcessWholeFeed)
            {
                par.Add("dont_process_whole_feed", "true");
            }
            else
            {
                par.Add("dont_process_whole_feed", "false");
            }

            par.Add("keyword", keywordString);
            par.Add("unwanted_keywords", unKeywordString);

            if (paused)
            {
                par.Add("paused", "true");
            }
            else
            {
                par.Add("paused", "false");
            }

            using (putSharpWebClient client = new putSharpWebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return FeedParser(json);
            }
        }

        public string Pause(long id)
        {
            string url = $"{_baseURL}{id}/pause?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Pause(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}/pause?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string Resume(long id)
        {
            string url = $"{_baseURL}{id}/resume?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Resume(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}/resume?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string Delete(long id)
        {
            string url = $"{_baseURL}{id}/delete?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string Delete(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}/delete?oauth_token={accessToken}";

            using (putSharpWebClient client = new putSharpWebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        #region Parsers
        private static Feed FeedParser(string json)
        {
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);

            Feed feedResult = new Feed();

            Dictionary<string, object> feed = ((JObject)jsonObj["feed"]).ToObject<Dictionary<string, object>>();

            foreach (KeyValuePair<string, object> data in feed)
            {
                feedResult.FeedData.Add(data.Key, data.Value);
            }

            feedResult.Status = jsonObj["status"].ToObject<string>();

            return feedResult;
        }

        private static FeedList FeedListParser(string json)
        {
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);

            FeedList feeds = new FeedList();

            object[] feedsData = ((JArray)jsonObj["feeds"]).ToObject<object[]>();

            foreach (JObject feed in feedsData)
            {
                feeds.Feeds.Add(feed.ToObject<Dictionary<string, object>>());
            }

            feeds.Status = jsonObj["status"].ToObject<string>();

            return feeds;
        }

        private static string StatusParser(string json)
        {
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);

            return jsonObj["status"].ToObject<string>();
        }
        #endregion
    }
}
