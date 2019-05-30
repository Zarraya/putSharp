namespace putSharp.DataTypes
{
    public class ParallelDownload
    {
        private long _size = long.MinValue;
        private string _filePath = string.Empty;
        private long _downloadedIn = long.MinValue;
        private int _fileChunks = 0;

        public long Size { get => _size; set => _size = value; }
        public string FilePath { get => _filePath; set => _filePath = value; }
        public long DownloadedIn { get => _downloadedIn; set => _downloadedIn = value; }
        public int FileChunks { get => _fileChunks; set => _fileChunks = value; }
    }
}
