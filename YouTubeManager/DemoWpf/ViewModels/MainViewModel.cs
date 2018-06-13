using DemoWpf.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using YouTubeManager;
using YouTubeManager.Models;
using YouTubeManager.Models.ClosedCaptions;
using YouTubeManager.Models.MediaStreams;

namespace DemoWpf.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly YouTubeService _youTubeService;

        private bool _isBusy;
        private string _query;
        private Video _video;
        private Channel _channel;
        private MediaStreamInfoSet _mediaStreamInfos;
        private IReadOnlyList<ClosedCaptionTrackInfo> _closedCaptionTrackInfos;
        private double _progress;
        private bool _isProgressIndeterminate;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                GetVideoInfoCommand.RaiseCanExecuteChanged();
                DownloadMediaStreamCommand.RaiseCanExecuteChanged();
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                Set(ref _query, value);
                GetVideoInfoCommand.RaiseCanExecuteChanged();
            }
        }

        public Video Video
        {
            get => _video;
            private set
            {
                Set(ref _video, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public Channel Channel
        {
            get => _channel;
            private set
            {
                Set(ref _channel, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public MediaStreamInfoSet MediaStreamInfos
        {
            get => _mediaStreamInfos;
            private set
            {
                Set(ref _mediaStreamInfos, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public IReadOnlyList<ClosedCaptionTrackInfo> ClosedCaptionTrackInfos
        {
            get => _closedCaptionTrackInfos;
            private set
            {
                Set(ref _closedCaptionTrackInfos, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public bool IsDataAvailable => Video != null && Channel != null
                                       && MediaStreamInfos != null && ClosedCaptionTrackInfos != null;

        public double Progress
        {
            get => _progress;
            private set => Set(ref _progress, value);
        }

        public bool IsProgressIndeterminate
        {
            get => _isProgressIndeterminate;
            private set => Set(ref _isProgressIndeterminate, value);
        }

        // Commands
        public RelayCommand GetVideoInfoCommand { get; }
        public RelayCommand<MediaStreamInfo> DownloadMediaStreamCommand { get; }
        public RelayCommand<ClosedCaptionTrackInfo> DownloadClosedCaptionTrackCommand { get; }

        public MainViewModel()
        {
            _youTubeService = new YouTubeService();

            // Commands
            GetVideoInfoCommand = new RelayCommand(GetData,
                () => !IsBusy && Query.IsNotBlank());
            DownloadMediaStreamCommand = new RelayCommand<MediaStreamInfo>(DownloadMediaStream,
                _ => !IsBusy);
            DownloadClosedCaptionTrackCommand = new RelayCommand<ClosedCaptionTrackInfo>(
                DownloadClosedCaptionTrack, _ => !IsBusy);
        }

        private async void GetData()
        {
            IsBusy = true;
            IsProgressIndeterminate = true;

            // Reset data
            Video = null;
            Channel = null;
            MediaStreamInfos = null;
            ClosedCaptionTrackInfos = null;

            // Parse URL if necessary
            if (!YouTubeService.TryParseVideoId(Query, out var videoId))
                videoId = Query;

            if (YouTubeService.ValidateVideoId(videoId) == true)
            {
                try
                {
                    // Get data
                    Video = await _youTubeService.GetVideoAsync(videoId);
                    Channel = await _youTubeService.GetVideoAuthorChannelAsync(videoId);
                    MediaStreamInfos = await _youTubeService.GetVideoMediaStreamInfosAsync(videoId);
                    ClosedCaptionTrackInfos = await _youTubeService.GetVideoClosedCaptionTrackInfosAsync(videoId);
                }
                catch
                {
                    MessageBox.Show($"Something went wrong! Cannot progress with video ID: [{videoId}]", "Error! - YouTube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong! Please input a video ID or URL", "Error! - YouTube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            

            IsBusy = false;
            IsProgressIndeterminate = false;
        }

        private async void DownloadMediaStream(MediaStreamInfo info)
        {
            // Create dialog
            var fileExt = info.Container.GetFileExtension();
            var defaultFileName = $"{Video.Title}.{fileExt}"
                .Replace(Path.GetInvalidFileNameChars(), '_');
            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = fileExt,
                FileName = defaultFileName,
                Filter = $"{info.Container} Files|*.{fileExt}|All Files|*.*"
            };

            // Select file path
            if (sfd.ShowDialog() != true)
                return;

            var filePath = sfd.FileName;

            // Download to file
            IsBusy = true;
            Progress = 0;

            var progressHandler = new Progress<double>(p => Progress = p);
            await _youTubeService.DownloadMediaStreamAsync(info, filePath, progressHandler);

            IsBusy = false;
            Progress = 0;
        }

        private async void DownloadClosedCaptionTrack(ClosedCaptionTrackInfo info)
        {
            // Create dialog
            var fileExt = $"{Video.Title}.{info.Language.Name}.srt"
                .Replace(Path.GetInvalidFileNameChars(), '_');
            var filter = "SRT Files|*.srt|All Files|*.*";
            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "srt",
                FileName = fileExt,
                Filter = filter
            };

            // Select file path
            if (sfd.ShowDialog() != true)
                return;

            var filePath = sfd.FileName;

            // Download to file
            IsBusy = true;
            Progress = 0;

            var progressHandler = new Progress<double>(p => Progress = p);
            await _youTubeService.DownloadClosedCaptionTrackAsync(info, filePath, progressHandler);

            IsBusy = false;
            Progress = 0;
        }
    }
}
