using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace putSharp.DataTypes
{
    public class Enums
    {
        public enum eSearchType
        {
            video,
            audio,
            image,
            iphone,
            all
        }

        public enum eSearchExt
        {
            mp3,
            avi,
            jpg,
            mp4,
            all
        }

        public enum eStatus
        {
            OK,
            ERROR,
            NOT_AVAILABLE,
            IN_QUEUE,
            PREPARING,
            CONVERTING,
            FINISHING,
            COMPLETED,
            WAITING,
            DOWNLOADING,
            COMPLETING,
            SEEDING            
        }

        public enum eSubtitleFormat
        {
            SRT,
            WEBVTT
        }
    }
}
