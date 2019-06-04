using System;
using System.Collections.Generic;

namespace putSharp.DataTypes
{
    public class File
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private string _status = "";

        public Dictionary<string, object> Data { get => _data; set => _data = value; }
        public string Status { get => _status; set => _status = value; }

        public string ContentType => (string)_data["content_type"];
        public string Crc32 => (string)_data["crc32"];
        public DateTime CreatedAt => (DateTime)_data["created_at"];
        public string Extension => (string)_data["extension"];
        public string FileType => (string)_data["file_type"];
        public DateTime FirstAccessedAt => (DateTime)_data["first_accessed_at"];
        public string FolderType => (string)_data["folder_type"];
        public string Icon => (string)_data["icon"];
        public long Id => (long)_data["id"];
        public bool IsHidden => (bool)_data["is_hidden"];
        public bool IsMp4Available => (bool)_data["is_mp4_available"];
        public bool IsShared => (bool)_data["is_shared"];
        public string Name => (string)_data["name"];
        public string OpenSubtitlesHash => (string)_data["opensubtitles_hash"];
        public long ParentId => (long)_data["parent_id"];
        public string Screenshot => (string)_data["screenshot"];
        public long Size => (long)_data["size"];
        public long StartFrom => (long)_data["start_from"];
        public DateTime UpdatedAt => (DateTime)_data["updated_at"];


        #region Overrides of Object

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}