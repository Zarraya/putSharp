using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace putSharp
{
    public class Downpour
    {
        private readonly string _rawString;

        private readonly Dictionary<string, string> _regexPatterns = new Dictionary<string, string>()
        {
            {"pretty", "S\\d{1,2}[\\-\\.\\s_]?E\\d{1,2}"},
            {"tricky", "[^\\d]\\d{1,2}[X\\-\\.\\s_]\\d{1,2}([^\\d]|$)"},
            {"combined", "(?:S)?\\d{1,2}[EX\\-\\.\\s_]\\d{1,2}([^\\d]|$)"},
            {"altSeason", "Season \\d{1,2} Episode \\d{1,2}"},
            {"altSeasonSingle", "Season \\d{1,2}"},
            {"altEpisodeSingle", "Episode \\d{1,2}"},
            {"altSeason2", "[\\s_\\.\\-\\[]\\d{3}[\\s_\\.\\-\\]]"},
            {"year", "[\\(?:\\.\\s_\\[](?:19|(?:[2-9])(?:[0-9]))\\d{2}[\\]\\s_\\.\\)]"}
        };

        private readonly string[] _musicExtensions = new[]
        {
            "aa", "aac", "aax", "act", "aiff",
            "alac", "amr", "ape", "au", "awb",
            "dct", "dss", "dvf", "flac", "gsm",
            "iklax", "ivs", "m4a", "m4b", "m4p",
            "mmf", "mp3", "mpc", "msv", "oga",
            "mogg", "opus", "ra", "sln", "tta",
            "vox", "wav", "wma", "wv"
        };

        private readonly string[] _videoExtensions = new[]
        {
            "mkv", "flv", "vob", "ogv", "drc",
            "gifv", "mng", "avi", "mov", "qt",
            "wmv", "yuv", "rmvb", "asf", "amv",
            "mp4", "m4p", "m4v", "mpg", "mp2",
            "mpeg", "mpe", "mpv", "svi", "3g2",
            "mx4", "roq", "nsv", "f4v", "f4p",
            "f4a", "f4b"
        };

        private readonly string[] _subtitleExtensions = new[]
        {
            "srt", "smi", "ssa", "ass", "vtt"
        };


        public Downpour(string fileName)
        {
            _rawString = fileName;
        }

        public string GetSeasonEpisode()
        {
            Match match = Regex.Match(_rawString, _regexPatterns["pretty"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                if (!IsSurroundSoundType(match.Value))
                {
                    return match.Value;
                }
            }

            match = Regex.Match(_rawString, _regexPatterns["tricky"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string value = match.Value.Substring(1);
                if (!IsSurroundSoundType(value))
                {
                    return value;
                }
            }

            match = Regex.Match(_rawString, _regexPatterns["combined"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                if (!IsSurroundSoundType(match.Value))
                {
                    return match.Value;             
                }
            }

            match = Regex.Match(_rawString, _regexPatterns["altSeason"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                if (!IsSurroundSoundType(match.Value))
                {
                    return match.Value;
                }
            }

            match = Regex.Match(_rawString, _regexPatterns["altSeason2"], RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string str = CleanedString(match.Value);
                if (!IsSurroundSoundType(str))
                {
                    string[] vals = new[] { "264", "720" };
                    if (!vals.Contains(str.Substring(1, 3)))
                    {
                        return str;
                    }
                }

            }

            return null;
        }

        public string GetSeason()
        {
            string both = GetSeasonEpisode();
            if (!string.IsNullOrEmpty(both))
            {
                if (both.Length > 7)
                {
                    Match match = Regex.Match(_rawString, _regexPatterns["altSeasonSingle"], RegexOptions.IgnoreCase);
                    string str = match.Value;
                    return str.Remove(0, 6);
                }

                if (both.Length == 3)
                {
                    return CleanedString(both.Substring(0, 1));
                }

                List<string> pieces = both.Split("eExX-._ ".ToCharArray()).ToList();

                string chars = pieces[0];

                if (chars.Length <= 2 && chars.Length >= 1)
                {
                    return CleanedString(chars);
                }
                else
                {
                    return CleanedString(chars.Substring(1));
                }
            }

            return null;
        }

        public string GetEpisode()
        {
            string both = GetSeasonEpisode();
            if (!string.IsNullOrEmpty(both))
            {
                if (both.Length > 7)
                {
                    Match match = Regex.Match(_rawString, _regexPatterns["altEpisodeSingle"], RegexOptions.IgnoreCase);
                    string str = match.Value;
                    return str.Remove(0, 6);
                }

                if (both.Length == 3)
                {
                    return CleanedString(both.Substring(1, 1));
                }

                List<string> pieces = both.Split("eExX-._ ".ToCharArray()).ToList();

                int i = 1;
                while (string.IsNullOrEmpty(pieces[i]) && i < pieces.Count)
                {
                    i += 1;
                }

                return CleanedString(pieces[i]);
            }

            return null;
        }

        public DownpourType GetDownpourType()
        {
            string ext = Path.GetExtension(_rawString);
            if (!string.IsNullOrEmpty(ext))
            {
                ext = ext.Remove(0, 1);
            }

            if (_videoExtensions.Contains(ext) || _subtitleExtensions.Contains(ext))
            {
                if (GetSeason() != null && GetEpisode() != null)
                {
                    return DownpourType.Tv;
                }

                return DownpourType.Movie;
            }
            else if (_musicExtensions.Contains(ext))
            {
                return DownpourType.Music;
            }
            else
            {
                if (GetSeason() != null && GetEpisode() != null)
                {
                    return DownpourType.Tv;
                }

                return DownpourType.Movie;
            }
        }

        public string GetYear()
        {
            DownpourType type = GetDownpourType();
            if (type == DownpourType.Movie || type == DownpourType.Tv)
            {
                Match match = Regex.Match(_rawString, _regexPatterns["year"], RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return CleanedString(match.Value);
                }
            }

            return null;
        }

        public string GetTitle()
        {
            string title = null;
            DownpourType type = GetDownpourType();

            if (type == DownpourType.Tv)
            {
                string seasonEpisode = GetSeasonEpisode();
                int seasonEpisodeIndex = _rawString.IndexOf(seasonEpisode, StringComparison.Ordinal);
                if (seasonEpisodeIndex > 0)
                {
                    string str = _rawString.Substring(0, seasonEpisodeIndex);
                    string year = GetYear();
                    if (year != null)
                    {
                        str = _rawString.Substring(0, _rawString.IndexOf(year, StringComparison.Ordinal));
                    }

                    title = str;
                }
            }
            else if (type == DownpourType.Movie)
            {
                string str = null;
                string year = GetYear();
                if (year != null)
                {
                    str = _rawString.Substring(0, _rawString.IndexOf(year, StringComparison.Ordinal));
                }

                title = str;
            }

            if (title != null)
            {
                string clean = CleanedString(title);

                Match uncleanMatch = Regex.Match(title, "\\d+\\.\\d+");
                Match tooCleanMatch = Regex.Match(clean, "\\d+ \\d+");
                if (uncleanMatch.Success && tooCleanMatch.Success)
                {
                    clean = clean.Replace(tooCleanMatch.Value, uncleanMatch.Value);
                }

                return clean;
            }
            else
            {
                return CleanedString(_rawString);
            }

        }

        private string CleanedString(string s)
        {
            string cleaned = s;
            cleaned = cleaned.Trim(" -.([]{}))_".ToCharArray());
            cleaned = cleaned.Replace(".", " ");
            return cleaned;
        }

        private bool IsSurroundSoundType(string s)
        {
            string cleaned = s.Trim(" -.([]{}))_".ToCharArray());

            if (cleaned == "2.1" || cleaned == "5.1" || cleaned == "7.1")
            {
                return true;
            }

            return false;
        }
    }

    public enum DownpourType
    {
        Tv,
        Movie,
        Music
    }
}