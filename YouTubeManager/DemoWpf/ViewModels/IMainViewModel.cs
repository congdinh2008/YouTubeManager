using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using YouTubeManager.Models;
using YouTubeManager.Models.ClosedCaptions;
using YouTubeManager.Models.MediaStreams;

namespace DemoWpf.ViewModels
{
    public interface IMainViewModel
    {
        bool IsBusy { get; }
        string Query { get; set; }

        Video Video { get; }
        Channel Channel { get; }
        MediaStreamInfoSet MediaStreamInfos { get; }
        IReadOnlyList<ClosedCaptionTrackInfo> ClosedCaptionTrackInfos { get; }
        bool IsDataAvailable { get; }

        double Progress { get; }
        bool IsProgressIndeterminate { get; }

        RelayCommand GetVideoInfoCommand { get; }
        RelayCommand<MediaStreamInfo> DownloadMediaStreamCommand { get; }
        RelayCommand<ClosedCaptionTrackInfo> DownloadClosedCaptionTrackCommand { get; }
    }
}