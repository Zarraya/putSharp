using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace putSharp
{
  public class Downpour
  {
    private readonly Dictionary<string, string> _regexPatterns = new Dictionary<string, string>()
    {
      {
        "pretty",
        "S\\d{1,2}[\\-\\.\\s_]?E\\d{1,2}"
      },
      {
        "tricky",
        "[^\\d]\\d{1,2}[X\\-\\.\\s_]\\d{1,2}([^\\d]|$)"
      },
      {
        "combined",
        "(?:S)?\\d{1,2}[EX\\-\\.\\s_]\\d{1,2}([^\\d]|$)"
      },
      {
        "altSeason",
        "Season \\d{1,2} Episode \\d{1,2}"
      },
      {
        "altSeasonSingle",
        "Season \\d{1,2}"
      },
      {
        "altEpisodeSingle",
        "Episode \\d{1,2}"
      },
      {
        "altSeason2",
        "[\\s_\\.\\-\\[]\\d{3}[\\s_\\.\\-\\]]"
      },
      {
        "year",
        "[\\(?:\\.\\s_\\[](?:19|(?:[2-9])(?:[0-9]))\\d{2}[\\]\\s_\\.\\)]"
      }
    };
    private readonly string[] _musicExtensions = new string[34]
    {
      "aa",
      "aac",
      "aax",
      "act",
      "aiff",
      "alac",
      "amr",
      "ape",
      "au",
      "awb",
      "dct",
      "dss",
      "dvf",
      "flac",
      "gsm",
      "iklax",
      "ivs",
      "m4a",
      "m4b",
      "m4p",
      "mmf",
      "mp3",
      "mpc",
      "msv",
      "oga",
      "mogg",
      "opus",
      "ra",
      "sln",
      "tta",
      "vox",
      "wav",
      "wma",
      "wv"
    };
    private readonly string[] _videoExtensions = new string[32]
    {
      "mkv",
      "flv",
      "vob",
      "ogv",
      "drc",
      "gifv",
      "mng",
      "avi",
      "mov",
      "qt",
      "wmv",
      "yuv",
      "rmvb",
      "asf",
      "amv",
      "mp4",
      "m4p",
      "m4v",
      "mpg",
      "mp2",
      "mpeg",
      "mpe",
      "mpv",
      "svi",
      "3g2",
      "mx4",
      "roq",
      "nsv",
      "f4v",
      "f4p",
      "f4a",
      "f4b"
    };
    private readonly string[] _subtitleExtensions = new string[5]
    {
      "srt",
      "smi",
      "ssa",
      "ass",
      "vtt"
    };
    private readonly string _rawString;

    public Downpour(string fileName)
    {
      this._rawString = fileName;
    }

    public string GetSeasonEpisode()
    {
      Match match1 = Regex.Match(this._rawString, this._regexPatterns["pretty"], RegexOptions.IgnoreCase);
      if (match1.Success && !this.IsSurroundSoundType(match1.Value))
        return match1.Value;
      Match match2 = Regex.Match(this._rawString, this._regexPatterns["tricky"], RegexOptions.IgnoreCase);
      if (match2.Success)
      {
        string s = match2.Value.Substring(1);
        if (!this.IsSurroundSoundType(s))
          return s;
      }
      Match match3 = Regex.Match(this._rawString, this._regexPatterns["combined"], RegexOptions.IgnoreCase);
      if (match3.Success && !this.IsSurroundSoundType(match3.Value))
        return match3.Value;
      Match match4 = Regex.Match(this._rawString, this._regexPatterns["altSeason"], RegexOptions.IgnoreCase);
      if (match4.Success && !this.IsSurroundSoundType(match4.Value))
        return match4.Value;
      Match match5 = Regex.Match(this._rawString, this._regexPatterns["altSeason2"], RegexOptions.IgnoreCase);
      if (match5.Success)
      {
        string s = this.CleanedString(match5.Value);
        if (s.Length < 4 || this.IsSurroundSoundType(s))
          return (string) null;
        if (!((IEnumerable<string>) new string[2]
        {
          "264",
          "720"
        }).Contains<string>(s.Substring(1, 3)))
          return s;
      }
      return (string) null;
    }

    public string GetSeason()
    {
      string seasonEpisode = this.GetSeasonEpisode();
      if (string.IsNullOrEmpty(seasonEpisode))
        return (string) null;
      if (seasonEpisode.Length > 7)
        return Regex.Match(this._rawString, this._regexPatterns["altSeasonSingle"], RegexOptions.IgnoreCase).Value.Remove(0, 6);
      if (seasonEpisode.Length == 3)
        return this.CleanedString(seasonEpisode.Substring(0, 1));
      string s = ((IEnumerable<string>) seasonEpisode.Split("eExX-._ ".ToCharArray())).ToList<string>()[0];
      if (s.Length <= 2 && s.Length >= 1)
        return this.CleanedString(s);
      return this.CleanedString(s.Substring(1));
    }

    public string GetEpisode()
    {
      string seasonEpisode = this.GetSeasonEpisode();
      if (string.IsNullOrEmpty(seasonEpisode))
        return (string) null;
      if (seasonEpisode.Length > 7)
        return Regex.Match(this._rawString, this._regexPatterns["altEpisodeSingle"], RegexOptions.IgnoreCase).Value.Remove(0, 6);
      if (seasonEpisode.Length == 3)
        return this.CleanedString(seasonEpisode.Substring(1, 1));
      List<string> list = ((IEnumerable<string>) seasonEpisode.Split("eExX-._ ".ToCharArray())).ToList<string>();
      int index = 1;
      while (string.IsNullOrEmpty(list[index]) && index < list.Count)
        ++index;
      return this.CleanedString(list[index]);
    }

    public DownpourType GetDownpourType()
    {
      string str = Path.GetExtension(this._rawString);
      if (!string.IsNullOrEmpty(str))
        str = str.Remove(0, 1);
      if (((IEnumerable<string>) this._videoExtensions).Contains<string>(str) || ((IEnumerable<string>) this._subtitleExtensions).Contains<string>(str))
        return this.GetSeason() != null && this.GetEpisode() != null ? DownpourType.Tv : DownpourType.Movie;
      if (((IEnumerable<string>) this._musicExtensions).Contains<string>(str))
        return DownpourType.Music;
      return this.GetSeason() != null && this.GetEpisode() != null ? DownpourType.Tv : DownpourType.Movie;
    }

    public string GetYear()
    {
      DownpourType downpourType = this.GetDownpourType();
      if (downpourType == DownpourType.Movie || downpourType == DownpourType.Tv)
      {
        Match match = Regex.Match(this._rawString, this._regexPatterns["year"], RegexOptions.IgnoreCase);
        if (match.Success)
          return this.CleanedString(match.Value);
      }
      return (string) null;
    }

    public string GetTitle()
    {
      string str1 = (string) null;
      switch (this.GetDownpourType())
      {
        case DownpourType.Tv:
          int length = this._rawString.IndexOf(this.GetSeasonEpisode(), StringComparison.Ordinal);
          if (length > 0)
          {
            string str2 = this._rawString.Substring(0, length);
            string year = this.GetYear();
            if (year != null)
              str2 = this._rawString.Substring(0, this._rawString.IndexOf(year, StringComparison.Ordinal));
            str1 = str2;
            break;
          }
          break;
        case DownpourType.Movie:
          string str3 = (string) null;
          string year1 = this.GetYear();
          if (year1 != null)
            str3 = this._rawString.Substring(0, this._rawString.IndexOf(year1, StringComparison.Ordinal));
          str1 = str3;
          break;
      }
      if (str1 == null)
        return (string) null;
      string input = this.CleanedString(str1);
      Match match1 = Regex.Match(str1, "\\d+\\.\\d+");
      Match match2 = Regex.Match(input, "\\d+ \\d+");
      if (match1.Success && match2.Success)
        input = input.Replace(match2.Value, match1.Value);
      return input;
    }

    private string CleanedString(string s)
    {
      return s.Trim(" -.([]{}))_".ToCharArray()).Replace(".", " ");
    }

    private bool IsSurroundSoundType(string s)
    {
      string str = s.Trim(" -.([]{}))_".ToCharArray());
      return str == "2.1" || str == "5.1" || str == "7.1";
    }
  }

  public enum DownpourType
    {
        Tv,
        Movie,
        Music
    }
}
