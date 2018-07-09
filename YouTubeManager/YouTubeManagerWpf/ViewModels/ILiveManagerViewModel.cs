using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using YouTubeManager.Models;
using YouTubeManager.Models.MediaStreams;
using YouTubeManagerWpf.Models;

namespace YouTubeManagerWpf.ViewModels
{
    public interface ILiveManagerViewModel
    {
        bool IsBusy { get; }
        string InputQuery { get; set; }
        string Query02 { get; set; }

        Video Video { get; }
        IList<StreamInfo> StreamInfos { get; set; }

        MediaStreamInfoSet MediaStreamInfos { get; }

        bool IsDataAvailable { get; }
        double Progress { get; }
        bool IsProgressIndeterminate { get; }


        RelayCommand AddStreamCommand { get; }
        RelayCommand EditStreamCommand { get; }
        RelayCommand DeleteStreamCommand { get; }
        RelayCommand RunStreamCommand { get; }
        RelayCommand RunAllCommand { get; }
        RelayCommand AddDetailsCommand { get; }

        RelayCommand OpenInputCommand { get; }
        RelayCommand OpenImagesCommand { get; }
        RelayCommand ResetSetingsCommand { get; }

    }
}
