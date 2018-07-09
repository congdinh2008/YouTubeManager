using System;

namespace YouTubeManagerWpf.Models
{
    public class StreamInfo
    {
        public StreamInfo(string name, string liveType, string input, string streamUrl, string output, string loop, string code, string status, DateTime createdDate)
        {
            Name = name;
            LiveType = liveType;
            Input = input;
            StreamUrl = streamUrl;
            Output = output;
            Loop = loop;
            Code = code;
            Status = status;
            CreatedDate = createdDate;
        }

        public override string ToString() => Name;

        public StreamInfo(string name, string liveType, string input, string streamUrl, string output, string filter,
            string loop, string cropCode, string imagePath, string preset, string size, string bitrate, string framerate,
            string uSpeed, string thread, string cPU, string blur, string speed, string volume, string code, string status, DateTime createdDate)
        {
            Name = name;
            LiveType = liveType;
            Input = input;
            StreamUrl = streamUrl;
            Output = output;
            Filter = filter;
            Loop = loop;
            CropCode = cropCode;
            ImagePath = imagePath;
            Preset = preset;
            Size = size;
            Bitrate = bitrate;
            Framerate = framerate;
            USpeed = uSpeed;
            Thread = thread;
            CPU = cPU;
            Blur = blur;
            Speed = speed;
            Volume = volume;
            Code = code;
            Status = status;
            CreatedDate = createdDate;
        }

        public string Name { get; set; }
        public string LiveType { get; set; }
        public string Input { get; set; }
        public string StreamUrl { get; set; }
        public string Output { get; set; }
        public string Filter { get; set; }
        public string Loop { get; set; }
        public string CropCode { get; set; }
        public string ImagePath { get; set; }
        public string Preset { get; set; }
        public string Size { get; set; }
        public string Bitrate { get; set; }
        public string Framerate { get; set; }
        public string USpeed { get; set; }
        public string Thread { get; set; }
        public string CPU { get; set; }
        public string Blur { get; set; }
        public string Speed { get; set; }
        public string Volume { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
