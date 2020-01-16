using System;
using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class File
    {
        private SerializableDict<string, object> _data = new SerializableDict<string, object>();
        private string _status = "";

        public SerializableDict<string, object> Data { get => _data; set => _data = value; }
        public string Status { get => _status; set => _status = value; }

        public string ContentType => _data.ContainsKey("content_type")? (string)_data["content_type"] : null;
        public string Crc32 => _data.ContainsKey("crc32")? (string)_data["crc32"] : null;
        public DateTime CreatedAt => _data.ContainsKey("created_at") && _data["created_at"] != null ? (DateTime)_data["created_at"]: DateTime.MinValue;
        public string Extension => _data.ContainsKey("extension")? (string)_data["extension"]: null;
        public string FileType => _data.ContainsKey("file_type")? (string)_data["file_type"] : null;
        public DateTime FirstAccessedAt => _data.ContainsKey("first_accessed_at") && _data["first_accessed_at"] != null? (DateTime)_data["first_accessed_at"] : DateTime.MinValue;
        public string FolderType => _data.ContainsKey("folder_type")? (string)_data["folder_type"] : null;
        public string Icon => _data.ContainsKey("icon") ? (string)_data["icon"] : null;
        public long Id => _data.ContainsKey("id") ? long.Parse(_data["id"].ToString()): -1;
        public bool IsHidden => (bool)_data["is_hidden"];
        public bool IsMp4Available => (bool)_data["is_mp4_available"];
        public bool IsShared => (bool)_data["is_shared"];
        public string Name => _data.ContainsKey("name") ? (string)_data["name"]: null;
        public string OpenSubtitlesHash => _data.ContainsKey("opensubtitles_hash") ? (string)_data["opensubtitles_hash"]: null;
        public long ParentId => (long)_data["parent_id"];
        public string Screenshot => _data.ContainsKey("screenshot") ? (string)_data["screenshot"] : null;
        public long Size => (long)_data["size"];

        public long StartFrom
        {
            get
            {
                if (!_data.ContainsKey("start_from"))
                {
                    return 0;
                }

                return (long) _data["start_from"];
            }
        }
        public DateTime UpdatedAt => (DateTime)_data["updated_at"];
        public List<File> Children { get; } = new List<File>();


        #region Overrides of Object

        public override string ToString()
        {
            return Name;
        }

        #endregion
          
        public bool IsFolder
        {
            get
            {
                if(FileType == "FOLDER")
                {
                    return true;
                }

                return false;
            }
        }
    }
}