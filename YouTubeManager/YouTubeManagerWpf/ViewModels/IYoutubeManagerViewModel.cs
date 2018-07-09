using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using YouTubeManager.Models;
using YouTubeManager.Models.ClosedCaptions;
using YouTubeManager.Models.MediaStreams;

namespace YouTubeManagerWpf.ViewModels
{
    public interface IYouTubeManagerViewModel
    {
        bool IsBusy { get; }
        string Query01 { get; set; }
        string Query02 { get; set; }
        int MaxPages { get; set; }

        Video Video { get; }
        Channel Channel { get; }
        IReadOnlyList<Video> Videos { get; }
        IReadOnlyList<Channel> Channels { get; }

        MediaStreamInfoSet MediaStreamInfos { get; }
        IReadOnlyList<ClosedCaptionTrackInfo> ClosedCaptionTrackInfos { get; }
        bool IsDataAvailable { get; }

        double Progress { get; }
        bool IsProgressIndeterminate { get; }

        RelayCommand GetDataCommand { get; }
        RelayCommand DownloadMediaStreamCommand { get; }
        RelayCommand ExportExcelCommand { get; }
        RelayCommand SettingsCommand { get; }
    }
}
