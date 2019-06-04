using System.Net;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using putSharp.DataTypes;
using Newtonsoft.Json.Linq;

namespace putSharp
{
    public class Files
    {
        private string _accessToken = string.Empty;
        private static string _baseURL = "https://api.put.io/v2/files/";
        private static string _baseEventURL = "https://api.put.io/v2/events/";
        private static string _baseUploadURL = "https://upload.put.io/v2/files/";
        private putSharpWebClient _client = new putSharpWebClient();

        public Files(string accessToken)
        {
            _accessToken = accessToken;
        }

        public FileListResult List(long parentID = 0)
        {
            string url = _baseURL + "list?parent_id="+ parentID +"&oauth_token=" + _accessToken;

            string json = _client.DownloadString(url);

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            
            return FileListParser(json);
        }

        public static FileListResult List(string accessToken, long parentID = 0)
        {
            string url = _baseURL + "list?parent_id=" + parentID + "&oauth_token=" + accessToken;

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                return FileListParser(json);
            }
        }

        public SearchResult Search(string query, Enums.eSearchType type, Enums.eSearchExt ext, int page = 1)
        {
            string url = $"{_baseURL}search/{query}/page/{page}?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);
            
            return SearchResultParser(json);
        }

        public static SearchResult Search(string accessToken, string query, Enums.eSearchType type, Enums.eSearchExt ext, int page)
        {
            string url = $"{_baseURL}search/{query}/page/{page}?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                return SearchResultParser(json);
            }
        }

        public DataTypes.File Upload(string file, string fileName, long parentID, string filePath)
        {
            string url = $"{_baseUploadURL}?file={file}&filename={fileName}&parent_id={parentID}&oauth_token={_accessToken}";

            bool isTorrent = false;
            if (filePath.EndsWith("torrent"))
            {
                isTorrent = true;
            }

            byte[] json = _client.UploadFile(url, filePath);

            string jsonString = Encoding.ASCII.GetString(json);
            
            if (isTorrent)
            {
                return null;
            }
            else
            {
                return SingleFileParser(jsonString);
            }
        }

        public static DataTypes.File Upload(string accessToken, string file, string fileName, long parentID, string filePath)
        {
            string url = $"{_baseUploadURL}?file={file}&filename={fileName}&parent_id={parentID}&oauth_token={accessToken}";

            bool isTorrent = false;
            if (filePath.EndsWith("torrent"))
            {
                isTorrent = true;
            }

            using (WebClient client = new WebClient())
            {
                byte[] json = client.UploadFile(url, filePath);

                string jsonString = Encoding.ASCII.GetString(json);

                if (isTorrent)
                {
                    return null;
                }
                else
                {
                    return SingleFileParser(jsonString);
                }
            }
        }

        public DataTypes.File CreateFolder(string folderName, long parentID)
        {
            string url = $"{_baseURL}create-folder?oauth_token={_accessToken}";
            string paramaters = $"name={folderName}&parent_id={parentID}";

            NameValueCollection par = new NameValueCollection();
            par.Add("name", folderName);
            par.Add("parent_id", parentID.ToString());

            byte[] jsonBytes = _client.UploadValues(url, par);
            string json = Encoding.ASCII.GetString(jsonBytes);

            return SingleFileParser(json);
        }

        public static DataTypes.File CreateFolder(string accessToken, string folderName, long parentID)
        {
            string url = $"{_baseURL}create-folder?oauth_token={accessToken}";
            string paramaters = $"name={folderName}&parent_id={parentID}";

            NameValueCollection par = new NameValueCollection();
            par.Add("name", folderName);
            par.Add("parent_id", parentID.ToString());

            using (WebClient client = new WebClient())
            {
                byte[] jsonBytes = client.UploadValues(url, par);
                string json = Encoding.ASCII.GetString(jsonBytes);

                return SingleFileParser(json);
            }
        }

        public DataTypes.File Get(long fileID)
        {
            string url = $"{_baseURL}{fileID}?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return SingleFileParser(json);

        }

        public static DataTypes.File Get(string accessToken, long fileID)
        {
            string url = $"{_baseURL}{fileID}?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                return SingleFileParser(json);
            }
        }

        public string Delete(IEnumerable<long> fileIDs)
        {
            string url = $"{_baseURL}delete?oauth_token={_accessToken}";

            string idString = "";
            foreach(long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);

            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
        }

        public static string Delete(string accessToken, IEnumerable<long> fileIDs)
        {
            string url = $"{_baseURL}delete?oauth_token={accessToken}";

            string idString = "";
            foreach (long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public string Rename(long fileID, string name)
        {
            string url = $"{_baseURL}rename?oauth_token={_accessToken}";
            
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_id", fileID.ToString());
            parameters.Add("name", name);

            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
        }

        public static string Rename(string accessToken, long fileID, string name)
        {
            string url = $"{_baseURL}delete?oauth_token={accessToken}";
            
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_id", fileID.ToString());
            parameters.Add("name", name);

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public string Move(IEnumerable<long> fileIDs, long parentID)
        {
            string url = $"{_baseURL}move?oauth_token={_accessToken}";

            string idString = "";
            foreach (long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);
            parameters.Add("parent_id", parentID.ToString());

            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
        }

        public static string Move(string accessToken, IEnumerable<long> fileIDs, long parentID)
        {
            string url = $"{_baseURL}move?oauth_token={accessToken}";

            string idString = "";
            foreach (long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);
            parameters.Add("parent_id", parentID.ToString());

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public string ConvertToMP4(long fileID)
        {
            string url = $"{_baseURL}{fileID}/mp4?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);
            
            return StatusParser(json);
        }

        public static string ConvertToMP4(string accessToken, long fileID)
        {
            string url = $"{_baseURL}{fileID}/mp4?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                
                return StatusParser(json);
            }
        }

        public MP4Status GetMP4Status(long fileID)
        {
            string url = $"{_baseURL}{fileID}/mp4?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return MP4StatusParser(json);
        }

        public static MP4Status GetMP4Status(string accessToken, long fileID)
        {
            string url = $"{_baseURL}{fileID}/mp4?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                
                return MP4StatusParser(json);
            }
        }

        public void Download(int streams, long fileID, string downloadPath)
        {
            string url = $"{_baseURL}{fileID}/download?oauth_token={_accessToken}";

            _client.DownloadString(url);
            string downloadURL = _client.ResponseHeaders["Location"];

            if(streams > 1)
            {
                ParallelDownload result = ParallelDownloader.Download(downloadURL, downloadPath, streams);
            }
            else
            {
                byte[] data = _client.DownloadData(downloadURL);

                string disposition = _client.ResponseHeaders["Content-Disposition"];
                string fileName = disposition.Substring(disposition.IndexOf("filename") + 9);

                if (!downloadPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    downloadPath += Path.DirectorySeparatorChar;
                }

                downloadPath += fileName;

                if (System.IO.File.Exists(downloadPath))
                {
                    System.IO.File.Delete(downloadPath);
                }

                FileStream stream = new FileStream(downloadPath, FileMode.OpenOrCreate);

                stream.Write(data, 0, data.Length);

                stream.Flush();
                stream.Close();
            }
        }

        public static void Download(int streams, long fileID, string downloadPath, string accessToken)
        {
            string url = $"{_baseURL}{fileID}/download?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                client.DownloadString(url);
                string downloadURL = client.ResponseHeaders["Location"];

                if (streams > 1)
                {
                    ParallelDownload result = ParallelDownloader.Download(downloadURL, downloadPath, streams);
                }
                else
                {
                    byte[] data = client.DownloadData(downloadURL);

                    string disposition = client.ResponseHeaders["Content-Disposition"];
                    string fileName = disposition.Substring(disposition.IndexOf("filename") + 9);

                    if (!downloadPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        downloadPath += Path.DirectorySeparatorChar;
                    }

                    downloadPath += fileName;

                    if (System.IO.File.Exists(downloadPath))
                    {
                        System.IO.File.Delete(downloadPath);
                    }

                    FileStream stream = new FileStream(downloadPath, FileMode.OpenOrCreate);

                    stream.Write(data, 0, data.Length);

                    stream.Flush();
                    stream.Close();
                }
            }
        }

        public string Share(IEnumerable<long> fileIDs, IEnumerable<string> friends)
        {
            string url = $"{_baseURL}share?oauth_token={_accessToken}";

            string idString = "";
            foreach (long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            string friendString = "";
            foreach (string friend in friends)
            {
                friendString += $"{friend},";
            }
            friendString = friendString.Substring(0, friendString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);
            parameters.Add("friends", friendString);

            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
        }

        public static string Share(string accessToken, IEnumerable<long> fileIDs, IEnumerable<string> friends)
        {
            string url = $"{_baseURL}share?oauth_token={accessToken}";

            string idString = "";
            foreach (long id in fileIDs)
            {
                idString += $"{id},";
            }
            idString = idString.Substring(0, idString.Length - 1);

            string friendString = "";
            foreach (string friend in friends)
            {
                friendString += $"{friend},";
            }
            friendString = friendString.Substring(0, friendString.Length - 1);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("file_ids", idString);
            parameters.Add("friends", friendString);

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public SharedFilesList SharedFiles()
        {
            string url = $"{_baseURL}shared?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return SharedFilesParser(json);
        }

        public static SharedFilesList SharedFiles(string accessToken)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{_baseURL}shared?oauth_token={accessToken}";

                string json = client.DownloadString(url);

                return SharedFilesParser(json);
            }
        }

        public UserList SharedWith(long id)
        {
            string url = $"{_baseURL}{id}/shared-with?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return UserListParser(json);
        }

        public static UserList SharedWith(string accessToken, long id)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{_baseURL}{id}/shared-with?oauth_token={accessToken}";

                string json = client.DownloadString(url);

                return UserListParser(json);
            }
        }

        public string Unshare(long id, IEnumerable<long> shareIds, bool everyone)
        {
            string url = $"{_baseURL}share?oauth_token={_accessToken}";

            string shareIdString = "";

            if (everyone)
            {
                shareIdString = "everyone";
            }
            else
            {
                foreach (long shareId in shareIds)
                {
                    shareIdString += $"{shareId},";
                }
                shareIdString = shareIdString.Substring(0, shareIdString.Length - 1);
            }
            
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("shares", shareIdString);
            
            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
            
        }

        public static string Unshare(string accessToken, long id, IEnumerable<long> shareIds, bool everyone)
        {
            string url = $"{_baseURL}share?oauth_token={accessToken}";

            string shareIdString = "";

            if (everyone)
            {
                shareIdString = "everyone";
            }
            else
            {
                foreach (long shareId in shareIds)
                {
                    shareIdString += $"{shareId},";
                }
                shareIdString = shareIdString.Substring(0, shareIdString.Length - 1);
            }

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("shares", shareIdString);

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public SubtitlesList Subtitles(long id)
        {
            string url = $"{_baseURL}{id}/subtitles?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return SubtitlesParser(json);
        }

        public static SubtitlesList Subtitles(string accessToken, long id)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{_baseURL}{id}/subtitles?oauth_token={accessToken}";

                string json = client.DownloadString(url);

                return SubtitlesParser(json);
            }
        }

        public void DownloadSubtitle(long id, string subtitleKey, Enums.eSubtitleFormat format, string path)
        {
            string url = $"{_baseURL}{id}/subtitles/{subtitleKey}?fomrat={format}&oauth_token={_accessToken}";

            _client.DownloadFile(url, path);
        }

        public static void DownloadSubtitle(string accessToken, long id, string subtitleKey, Enums.eSubtitleFormat format, string path)
        {
            string url = $"{_baseURL}{id}/subtitles/{subtitleKey}?fomrat={format}&oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }

        public void HLSPlaylist(long id, string subtitleKey, bool all = false)
        {
            string url = $"{_baseURL}{id}/hls/media.m3u8?subtitle_key={subtitleKey}&oauth_token={_accessToken}";

            if (all)
            {
                url = $"{_baseURL}{id}/hls/media.m3u8?subtitle_key=all&oauth_token={_accessToken}";
            }

            string hlsData = _client.DownloadString(url);
        }

        public static void HLSPlaylist(string accessToken, long id, string subtitleKey, bool all = false)
        {
            string url = $"{_baseURL}{id}/hls/media.m3u8?subtitle_key={subtitleKey}&oauth_token={accessToken}";

            if (all)
            {
                url = $"{_baseURL}{id}/hls/media.m3u8?subtitle_key=all&oauth_token={accessToken}";
            }

            using (WebClient client = new WebClient())
            {
                string hlsData = client.DownloadString(url);
            }
        }

        public EventsList Events()
        {
            string url = $"{_baseEventURL}list?oauth_token={_accessToken}";

            string json = _client.DownloadString(url);

            return EventsParser(json);
        }

        public static EventsList Events(string accessToken)
        {
            string url = $"{_baseEventURL}list?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                return EventsParser(json);
            }            
        }

        public string DeleteEvents()
        {
            string url = $"{_baseEventURL}delete?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string DeleteEvents(string accessToken)
        {
            string url = $"{_baseEventURL}delete?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        public string SetVideoPosition(long id, double time)
        {
            string url = $"{_baseURL}{id}/start-from?time={time}&oauth_token={_accessToken}";

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("time", time.ToString());

            byte[] returnBytes = _client.UploadValues(url, parameters);

            string status = Encoding.ASCII.GetString(returnBytes);

            return StatusParser(status);
        }

        public static string SetVideoPosition(string accessToken, long id, double time)
        {
            string url = $"{_baseURL}{id}/start-from?time={time}&oauth_token={accessToken}";

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("time", time.ToString());

            using (WebClient client = new WebClient())
            {
                byte[] returnBytes = client.UploadValues(url, parameters);

                string status = Encoding.ASCII.GetString(returnBytes);

                return StatusParser(status);
            }
        }

        public string DeleteVideoPosition(long id)
        {
            string url = $"{_baseURL}{id}/start-from/delete?oauth_token={_accessToken}";

            string json = _client.UploadString(url, "");

            return StatusParser(json);
        }

        public static string DeleteVideoPosition(string accessToken, long id)
        {
            string url = $"{_baseURL}{id}/start-from/delete?oauth_token={accessToken}";

            using (WebClient client = new WebClient())
            {
                string json = client.UploadString(url, "");

                return StatusParser(json);
            }
        }

        #region Parsers
        private static FileListResult FileListParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            FileListResult result = new FileListResult();

            object[] files = ((JArray) jsonObj["files"]).ToObject<object[]>(); 

            foreach (JObject fileData in files)
            {
                result.Files.Add(fileData.ToObject<Dictionary<string,object>>());
            }

            result.ParentFile = ((JObject)jsonObj["parent"]).ToObject<Dictionary<string, object>>();
            result.Status = jsonObj["status"];

            return result;
        }

        private static SearchResult SearchResultParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);
            
            SearchResult result = new SearchResult();

            object[] files = ((JArray)jsonObj["files"]).ToObject<object[]>();

            foreach (JObject fileData in files)
            {
                result.Files.Add(fileData.ToObject<Dictionary<string, object>>());
            }

            result.Status = jsonObj["status"];
            result.NextPageURL = jsonObj["next"];
            return result;
        }

        private static DataTypes.File SingleFileParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            DataTypes.File file = new DataTypes.File();

            file.Data = ((JObject)jsonObj["file"]).ToObject<Dictionary<string, object>>();
            file.Status = jsonObj["status"];

            return file;
        }

        private static string StatusParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            return jsonObj["status"];
        }

        private static MP4Status MP4StatusParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            MP4Status status = new MP4Status();

            status.Mp4 = ((JObject)jsonObj["mp4"]).ToObject<Dictionary<string, object>>();
            status.Status = jsonObj["status"];

            return status;
        }

        private static SharedFilesList SharedFilesParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            SharedFilesList list = new SharedFilesList();

            object[] files = ((JArray)jsonObj["shared"]).ToObject<object[]>();

            foreach (JObject fileData in files)
            {
                list.Files.Add(fileData.ToObject<Dictionary<string, object>>());
            }

            list.Status = jsonObj["status"];

            return list;
        }

        private static UserList UserListParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            UserList list = new UserList();

            object[] users = ((JArray)jsonObj["shared-with"]).ToObject<object[]>();

            foreach (JObject userData in users)
            {
                list.Users.Add(userData.ToObject<Dictionary<string, object>>());
            }

            list.Status = jsonObj["status"];

            return list;
        }

        private static SubtitlesList SubtitlesParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            SubtitlesList list = new SubtitlesList();

            object[] subtitles = ((JArray)jsonObj["subtitles"]).ToObject<object[]>();

            foreach (JObject subtitle in subtitles)
            {
                list.Subtitles.Add(subtitle.ToObject<Dictionary<string, object>>());
            }

            list.Status = jsonObj["status"];
            list.Default = jsonObj["default"];

            return list;
        }

        private static EventsList EventsParser(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

            EventsList list = new EventsList();

            object[] events = ((JArray)jsonObj["events"]).ToObject<object[]>();

            foreach (JObject evnt in events)
            {
                list.Events.Add(evnt.ToObject<Dictionary<string, object>>());
            }

            list.Status = jsonObj["status"];

            return list;
        }
        #endregion
    }
}
