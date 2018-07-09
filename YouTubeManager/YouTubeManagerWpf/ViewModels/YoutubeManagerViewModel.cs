using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using YouTubeManager;
using YouTubeManager.Models;
using YouTubeManager.Models.ClosedCaptions;
using YouTubeManager.Models.MediaStreams;
using YouTubeManagerWpf.Extensions;

namespace YouTubeManagerWpf.ViewModels
{
    public class YouTubeManagerViewModel : ViewModelBase, IYouTubeManagerViewModel, ITabViewModel
    {
        #region Field
        private readonly YouTubeService _youTubeService;

        private bool _isBusy;
        private string _query01;
        private string _query02;
        private int _maxPages;

        private Video _video;
        private Channel _channel;
        private IReadOnlyList<Video> _videos;
        private IReadOnlyList<Channel> _channels;

        private MediaStreamInfoSet _mediaStreamInfos;
        private IReadOnlyList<ClosedCaptionTrackInfo> _closedCaptionTrackInfos;

        private double _progress;
        private bool _isProgressIndeterminate;

        public RelayCommand GetDataCommand { get; }
        public RelayCommand DownloadMediaStreamCommand { get; }
        public RelayCommand ExportExcelCommand { get; }
        public RelayCommand SettingsCommand { get; }
        #endregion

        #region Properties

        public string Header { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                GetDataCommand.RaiseCanExecuteChanged();
            }
        }

        public string Query01
        {
            get => _query01;
            set
            {
                Set(ref _query01, value);
                GetDataCommand.RaiseCanExecuteChanged();
            }
        }

        public string Query02
        {
            get => _query02;
            set
            {
                Set(ref _query02, value);
                GetDataCommand.RaiseCanExecuteChanged();
            }
        }

        public int MaxPages
        {
            get => _maxPages;
            set
            {
                Set(ref _maxPages, value);
                GetDataCommand.RaiseCanExecuteChanged();
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

        public IReadOnlyList<Video> Videos
        {
            get => _videos;
            private set
            {
                Set(ref _videos, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public IReadOnlyList<Channel> Channels
        {
            get => _channels;
            private set
            {
                Set(ref _channels, value);
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

        public bool IsDataAvailable => Video != null && Channel != null && MediaStreamInfos != null && ClosedCaptionTrackInfos != null;

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

        #endregion

        #region Radio button list videos

        public static bool BooleanTrue = true;
        public static bool BooleanFalse = false;

        private bool _booleanProperty;

        public bool BooleanProperty
        {
            get => _booleanProperty;
            set
            {
                if (_booleanProperty != value)
                {
                    _booleanProperty = value;
                    RaisePropertyChanged("RdoListVideos");
                }
            }
        }

        #endregion

        #region SearchType
        private string _selectedSearchType;

        public string SelectedSearchType
        {
            get { return _selectedSearchType; }
            set
            {
                if (_selectedSearchType != value)
                {
                    _selectedSearchType = value;
                    RaisePropertyChanged("SelectedSearchType");
                }
            }
        }

        private ObservableCollection<string> _searchTypes = new ObservableCollection<string>()
        {
            "Channel",
            "Playlist",
            "Keyword"
        };

        public ObservableCollection<string> SearchTypes
        {
            get
            {
                return _searchTypes;
            }
            set
            {
                if (_searchTypes != value)
                {
                    _searchTypes = value;
                    RaisePropertyChanged("SearchTypes");
                }
            }
        }
        #endregion

        #region SearchOn
        public IEnumerable<string> SearchOn
        {
            get
            {
                yield return "YouTube";
                yield return "Vimeo";
                yield return "Twitch";
            }
        }
        #endregion

        #region Constructor
        public YouTubeManagerViewModel()
        {
            _youTubeService = new YouTubeService();

            // Commands
            GetDataCommand = new RelayCommand(GetDataAsync,
                () => !IsBusy && Query01.IsNotBlank() || Query02.IsNotBlank());

            DownloadMediaStreamCommand = new RelayCommand(DownloadMediaStream, () => !IsBusy && Query01.IsNotBlank() || Query02.IsNotBlank());

            ExportExcelCommand = new RelayCommand(ExportExcel, () => !IsBusy && Query01.IsNotBlank() || Query02.IsNotBlank());
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Get Data
        /// </summary>
        private async void GetDataAsync()
        {
            IsBusy = true;
            IsProgressIndeterminate = true;
            Progress = 0;


            // Get videos of channel
            if (SelectedSearchType.Contains("Channel") == true && BooleanProperty == false)
            {
                try
                {
                    if (!YouTubeService.TryParseChannelId(Query01, out var channelId))
                        channelId = Query01;

                    if(MaxPages == 0)
                    {
                        Videos = await _youTubeService.GetChannelUploadsAsync(channelId);
                    }
                    else
                    {
                        Videos = await _youTubeService.GetChannelUploadsAsync(channelId, MaxPages);
                    }
                }
                catch
                {
                    MessageBox.Show("Wrong URL! Please input a channel URL", "Error! - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }


            // Get videos of playlist
            if (SelectedSearchType.Contains("Playlist") == true && BooleanProperty == false)
            {
                try
                {
                    if (!YouTubeService.TryParsePlaylistId(Query01, out var playlistId))
                        playlistId = Query01;

                    if (MaxPages == 0)
                    {
                        var playlistInfo = await _youTubeService.GetPlaylistAsync(playlistId);
                        Videos = playlistInfo.Videos;
                    }
                    else
                    {
                        var playlistInfo = await _youTubeService.GetPlaylistAsync(playlistId, MaxPages);
                        Videos = playlistInfo.Videos;
                    }
                }
                catch
                {
                    MessageBox.Show("Wrong URL! Please input a playlist URL", "Error! - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            //Get videos by keyword
            if (SelectedSearchType.Contains("Keyword") == true && BooleanProperty == false)
            {
                try
                {
                    if(MaxPages == 0)
                    {
                        Videos = await _youTubeService.SearchVideosAsync(Query01);
                    }
                    else
                    {
                        Videos = await _youTubeService.SearchVideosAsync(Query01, MaxPages);
                    }
                }
                catch
                {
                    MessageBox.Show("Wrong URL! Please input a playlist URL", "Error! - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }

            // Get videos from list videos
            if (BooleanProperty == true)
            {
                List<string> listVideos = new List<string>(Query02.Split("\r\n"));

                List<Video> items = new List<Video>();

                try
                {
                    foreach (var video in listVideos)
                    {
                        if (!YouTubeService.TryParseVideoId(video, out var videoId))
                            videoId = video;

                        items.Add(await _youTubeService.GetVideoAsync(videoId));
                    }

                    // Convert List<Video> to IReadOnlyList<Video>
                    Videos = items.AsReadOnly();
                }
                catch
                {
                    MessageBox.Show("Wrong URL! Please input a list of videos", "Error! - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                
            }

            Progress = 0;
            IsProgressIndeterminate = false;
            IsBusy = false;

        }

        /// <summary>
        /// Download Videos
        /// </summary>
        private async void DownloadMediaStream()
        {
            IsBusy = true;
            IsProgressIndeterminate = true;
            try
            {
                string path = @"C:\Users\congd\source\repos\YouTubeManager\YouTubeManagerWpf\bin\Debug\temp\";
                foreach (var video in Videos)
                {
                    var streamInfoSet = await _youTubeService.GetVideoMediaStreamInfosAsync(video.Id);

                    var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();

                    var normalizedFileSize = NormalizeFileSize(streamInfo.Size);

                    // Compose file name, based on metadata
                    var fileExtension = streamInfo.Container.GetFileExtension();
                    var fileName = $"{video.Title}.{fileExtension}";

                    // Replace illegal characters in file name

                    fileName = fileName.Replace(Path.GetInvalidFileNameChars(), '_');
                    fileName = path + fileName;

                    // Download to file
                    Progress = 0;

                    var progressHandler = new Progress<double>(p => Progress = p);
                    await _youTubeService.DownloadMediaStreamAsync(streamInfo, fileName, progressHandler);

                    Progress = 0;
                }
            }
            catch
            {
                MessageBox.Show("Please Get Data First.", "Error! - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            IsProgressIndeterminate = false;
            IsBusy = false;
        }

        private static string NormalizeFileSize(long fileSize)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double size = fileSize;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {units[unit]}";
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        private void ExportExcel()
        {
            string filePath = "";

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel|*xlsx;*csv|Excel|*xls";
            dialog.DefaultExt = "xlsx";
            dialog.InitialDirectory = @"C:\Users\congd\source\repos\YouTubeManager\YouTubeManagerWpf\bin\Debug\temp\";
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter the path of file!", "Export Data - YouTube Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Author = "YouTube Manager";

                    p.Workbook.Properties.Title = filePath;

                    p.Workbook.Worksheets.Add("YouTube Manager");

                    ExcelWorksheet workSheetIndex = p.Workbook.Worksheets[1];


                    workSheetIndex.Name = "YouTube Manager";

                    string[] arrColumnHeader = { "Author", "ChannelId", "VideoId", "Title", "Description", "Keywords", "UploadDate", "ViewCount", "Duration", "LikeCount", "DislikeCount" };

                    var countColHeader = arrColumnHeader.Count();



                    int colIndex = 1;
                    int rowIndex = 1;

                    foreach (var item in arrColumnHeader)
                    {
                        var cell = workSheetIndex.Cells[rowIndex, colIndex];

                        cell.Value = item;
                        colIndex++;
                    }

                    foreach (var video in Videos)
                    {
                        colIndex = 1;

                        rowIndex++;

                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Author;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.AuthorId;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Id;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Title;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Description;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Keywords.JoinToString(",");
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.UploadDate;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Statistics.Views;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Duration.TotalSeconds;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Statistics.Likes;
                        workSheetIndex.Cells[rowIndex, colIndex++].Value = video.Statistics.Dislikes;
                    }

                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                MessageBox.Show("Export Successful!", "Export Data - Youtube Manager", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Export Failed!", "Export Data - YouTube Manager", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
