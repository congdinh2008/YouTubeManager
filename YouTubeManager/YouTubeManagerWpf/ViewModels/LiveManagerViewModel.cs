using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using YouTubeManager;
using YouTubeManager.Models;
using YouTubeManager.Models.MediaStreams;
using YouTubeManagerWpf.Models;
using YouTubeManagerWpf.Extensions;

namespace YouTubeManagerWpf.ViewModels
{
    public class LiveManagerViewModel: ViewModelBase, ILiveManagerViewModel, ITabViewModel
    {

        #region Fields

        private readonly YouTubeService _youTubeService;

        private bool _isBusy;
        private string _inputQuery;
        private string _nameStream;
        private string _cropText;
        private string _imagePath;
        private string _addImages;
        private string _query02;

        public static bool BooleanTrue = true;
        public static bool BooleanFalse = false;

        private Video _video;
        public StreamInfo _streamInfo;
        public IList<StreamInfo> _streamInfos;

        private MediaStreamInfoSet _mediaStreamInfos;

        private double _progress;
        private bool _isProgressIndeterminate;


        #endregion

        #region Commands
        public RelayCommand AddStreamCommand { get; }
        public RelayCommand EditStreamCommand { get; }
        public RelayCommand DeleteStreamCommand { get; }
        public RelayCommand RunStreamCommand { get; }
        public RelayCommand RunAllCommand { get; }
        public RelayCommand AddDetailsCommand { get; }

        public RelayCommand OpenInputCommand { get; }
        public RelayCommand OpenImagesCommand { get; }
        public RelayCommand ResetSetingsCommand { get; }
        #endregion

        #region Properties
        public string Header { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                OpenInputCommand.RaiseCanExecuteChanged();
            }
        }

        public string InputQuery
        {
            get => _inputQuery;
            set
            {
                Set(ref _inputQuery, value);
                OpenInputCommand.RaiseCanExecuteChanged();
            }
        }
        public string CropText
        {
            get => _cropText;
            set
            {
                Set(ref _cropText, value);
                AddStreamCommand.RaiseCanExecuteChanged();
            }
        }
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                Set(ref _imagePath, value);
                AddStreamCommand.RaiseCanExecuteChanged();
            }
        }
        public string NameStream
        {
            get => _nameStream;
            set
            {
                Set(ref _nameStream, value);
                AddStreamCommand.RaiseCanExecuteChanged();
            }
        }

        public string AddImages
        {
            get => _addImages;
            set
            {
                Set(ref _addImages, value);
                OpenImagesCommand.RaiseCanExecuteChanged();
            }
        }

        public string Query02
        {
            get => _query02;
            set
            {
                Set(ref _query02, value);
                OpenInputCommand.RaiseCanExecuteChanged();
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

        public StreamInfo StreamInfo
        {
            get => _streamInfo;
            set
            {
                Set(ref _streamInfo, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public IList<StreamInfo> StreamInfos
        {
            get => _streamInfos;
            set
            {
                Set(ref _streamInfos, value);
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

        public bool IsDataAvailable => Video != null && MediaStreamInfos != null;

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

        #region Combobox
        #region Live Type
        private string _selectedLiveType;

        public string SelectedLiveType
        {
            get { return _selectedLiveType; }
            set
            {
                if (_selectedLiveType != value)
                {
                    _selectedLiveType = value;
                    RaisePropertyChanged("SelectedLiveType");
                }
            }
        }

        private ObservableCollection<string> _liveTypes = new ObservableCollection<string>()
        {
            "File",
            "Folder",
            "Text",
            "Channel",
            "Playlist",
            "Video",
            "Live",
            "Link"
        };

        public ObservableCollection<string> LiveTypes
        {
            get
            {
                return _liveTypes;
            }
            set
            {
                if (_liveTypes != value)
                {
                    _liveTypes = value;
                    RaisePropertyChanged("LiveTypes");
                }
            }
        }
        #endregion

        #region Filter

        private bool _booleanFilterProperty;

        public bool BooleanFilterProperty
        {
            get => _booleanFilterProperty;
            set
            {
                if (_booleanFilterProperty != value)
                {
                    _booleanFilterProperty = value;
                    RaisePropertyChanged("BooleanFilterProperty");
                }
            }
        }
        #endregion

        #region Loop

        private bool _booleanLoopProperty;

        public bool BooleanLoopProperty
        {
            get => _booleanLoopProperty;
            set
            {
                if (_booleanLoopProperty != value)
                {
                    _booleanLoopProperty = value;
                    RaisePropertyChanged("BooleanLoopProperty");
                }
            }
        }
        #endregion

        #region Crop

        private bool _booleanCropProperty;

        public bool BooleanCropProperty
        {
            get => _booleanCropProperty;
            set
            {
                if (_booleanCropProperty != value)
                {
                    _booleanCropProperty = value;
                    RaisePropertyChanged("BooleanCropProperty");
                }
            }
        }
        #endregion

        #region Add Images

        private bool _booleanAddImagesProperty;

        public bool BooleanAddImagesProperty
        {
            get => _booleanAddImagesProperty;
            set
            {
                if (_booleanAddImagesProperty != value)
                {
                    _booleanAddImagesProperty = value;
                    RaisePropertyChanged("BooleanAddImagesProperty");
                }
            }
        }
        #endregion

        #region Presets
        private string _selectedPresets;

        public string SelectedPresets
        {
            get { return _selectedPresets; }
            set
            {
                if (_selectedPresets != value)
                {
                    _selectedPresets = value;
                    RaisePropertyChanged("SelectedPresets");
                }
            }
        }

        private ObservableCollection<string> _presets = new ObservableCollection<string>()
        {
            "ultrafast",
            "superfast",
            "veryfast",
            "faster",
            "fast",
            "medium",
            "slow",
            "slower",
            "veryslow",
            "placebo"
        };

        public ObservableCollection<string> Presets
        {
            get
            {
                return _presets;
            }
            set
            {
                if (_presets != value)
                {
                    _presets = value;
                    RaisePropertyChanged("Presets");
                }
            }
        }
        #endregion

        #region Sizes
        private string _selectedSizes;

        public string SelectedSizes
        {
            get { return _selectedSizes; }
            set
            {
                if (_selectedSizes != value)
                {
                    _selectedSizes = value;
                    RaisePropertyChanged("SelectedSizes");
                }
            }
        }

        private ObservableCollection<string> _sizes = new ObservableCollection<string>()
        {
            "1920x1080",
            "1280x720",
            "7680x4320",
            "4096x2160",
            "3840x2160",
            "2560x1440",
            "852x480",
            "720x480",
            "640x480"
            
        };

        public ObservableCollection<string> Sizes
        {
            get
            {
                return _sizes;
            }
            set
            {
                if (_sizes != value)
                {
                    _sizes = value;
                    RaisePropertyChanged("Sizes");
                }
            }
        }
        #endregion

        #region Bitrates
        private string _selectedBitrates;

        public string SelectedBitrates
        {
            get { return _selectedBitrates; }
            set
            {
                if (_selectedBitrates != value)
                {
                    _selectedBitrates = value;
                    RaisePropertyChanged("SelectedBitrates");
                }
            }
        }

        private ObservableCollection<string> _bitrates = new ObservableCollection<string>()
        {
            "500k",
            "1000k",
            "1500k",
            "2000k",
            "2500k",
            "3000k",
            "3500k",
            "4000k",
            "4500k",
            "5000k"

        };

        public ObservableCollection<string> Bitrates
        {
            get
            {
                return _bitrates;
            }
            set
            {
                if (_bitrates != value)
                {
                    _bitrates = value;
                    RaisePropertyChanged("Bitrates");
                }
            }
        }
        #endregion

        #region Framerate
        private string _selectedFramerates;

        public string SelectedFramerates
        {
            get { return _selectedFramerates; }
            set
            {
                if (_selectedFramerates != value)
                {
                    _selectedFramerates = value;
                    RaisePropertyChanged("SelectedFramerate");
                }
            }
        }

        private ObservableCollection<int> _framerates = new ObservableCollection<int>(Enumerable.Range(10, 120));

        public ObservableCollection<int> Framerates
        {
            get
            {
                return _framerates;
            }
            set
            {
                if (_framerates != value)
                {
                    _framerates = value;
                    RaisePropertyChanged("Framerates");
                }
            }
        }
        #endregion

        #region Upload Speeds
        private string _selectedUSpeeds;

        public string SelectedUSpeeds
        {
            get { return _selectedUSpeeds; }
            set
            {
                if (_selectedUSpeeds != value)
                {
                    _selectedUSpeeds = value;
                    RaisePropertyChanged("SelectedUSpeeds");
                }
            }
        }

        private ObservableCollection<string> _uSpeeds = new ObservableCollection<string>()
        {
            "1",
            "1.5",
            "2",
            "2.5",
            "3",
            "3.5",
            "4"

        };

        public ObservableCollection<string> USpeeds
        {
            get
            {
                return _uSpeeds;
            }
            set
            {
                if (_uSpeeds != value)
                {
                    _uSpeeds = value;
                    RaisePropertyChanged("USpeeds");
                }
            }
        }
        #endregion

        #region Threads
        private string _selectedThreads;

        public string SelectedThreads
        {
            get { return _selectedThreads; }
            set
            {
                if (_selectedThreads != value)
                {
                    _selectedThreads = value;
                    RaisePropertyChanged("SelectedThreads");
                }
            }
        }

        private ObservableCollection<string> _threads = new ObservableCollection<string>()
        {
            "0",
            "1",
            "2",
            "4"
        };

        public ObservableCollection<string> Threads
        {
            get
            {
                return _threads;
            }
            set
            {
                if (_threads != value)
                {
                    _threads = value;
                    RaisePropertyChanged("Threads");
                }
            }
        }
        #endregion

        #region CPUs
        private string _selectedCPUs;

        public string SelectedCPUs
        {
            get { return _selectedCPUs; }
            set
            {
                if (_selectedCPUs != value)
                {
                    _selectedCPUs = value;
                    RaisePropertyChanged("SelectedCPUs");
                }
            }
        }

        private ObservableCollection<string> _cpus = new ObservableCollection<string>()
        {
            "1",
            "2",
            "3",
            "4"
        };

        public ObservableCollection<string> CPUs
        {
            get
            {
                return _cpus;
            }
            set
            {
                if (_cpus != value)
                {
                    _cpus = value;
                    RaisePropertyChanged("CPUs");
                }
            }
        }
        #endregion

        #region Blurs
        private string _selectedBlurs;

        public string SelectedBlurs
        {
            get { return _selectedBlurs; }
            set
            {
                if (_selectedBlurs != value)
                {
                    _selectedBlurs = value;
                    RaisePropertyChanged("SelectedBlurs");
                }
            }
        }

        private ObservableCollection<int> _blurs = new ObservableCollection<int>(Enumerable.Range(0,101));

        public ObservableCollection<int> Blurs
        {
            get
            {
                return _blurs;
            }
            set
            {
                if (_blurs != value)
                {
                    _blurs = value;
                    RaisePropertyChanged("Blurs");
                }
            }
        }
        #endregion

        #region Speeds
        private string _selectedSpeeds;

        public string SelectedSpeeds
        {
            get { return _selectedSpeeds; }
            set
            {
                if (_selectedSpeeds != value)
                {
                    _selectedSpeeds = value;
                    RaisePropertyChanged("SelectedSpeeds");
                }
            }
        }

        private ObservableCollection<int> _speeds = new ObservableCollection<int>(Enumerable.Range(1, 201));

        public ObservableCollection<int> Speeds
        {
            get
            {
                return _speeds;
            }
            set
            {
                if (_speeds != value)
                {
                    _speeds = value;
                    RaisePropertyChanged("Speeds");
                }
            }
        }
        #endregion

        #region Volumes
        private string _selectedVolumes;

        public string SelectedVolumes
        {
            get { return _selectedVolumes; }
            set
            {
                if (_selectedVolumes != value)
                {
                    _selectedVolumes = value;
                    RaisePropertyChanged("SelectedVolumes");
                }
            }
        }

        private ObservableCollection<int> _volumes = new ObservableCollection<int>(Enumerable.Range(1, 201));

        public ObservableCollection<int> Volumes
        {
            get
            {
                return _volumes;
            }
            set
            {
                if (_volumes != value)
                {
                    _volumes = value;
                    RaisePropertyChanged("Volumes");
                }
            }
        }
        #endregion

        #region Outputs
        private string _selectedOutputItem;

        private ObservableCollection<string> _outputItems = new ObservableCollection<string>();

        public IEnumerable OutputItems
        {
            get
            {
                string path = "OutputItems.txt";
                string[] tempOutputItems = File.ReadAllLines(path);

                foreach (var item in tempOutputItems)
                {
                    _outputItems.Add(item);
                }

                return _outputItems;
            }
        }

        public string SelectedOutputItem
        {
            get => _selectedOutputItem;
            set
            {
                _selectedOutputItem = value;
                RaisePropertyChanged("SelectedOutputItem");
            }
        }

        public string NewItem
        {
            set
            {
                if (SelectedOutputItem != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    _outputItems.Add(value);
                    SelectedOutputItem = value;
                    string path = "OutputItems.txt";
                    File.WriteAllLines(path, _outputItems);
                }
            }
        }

        #endregion
        #endregion

        #region Constructor
        public LiveManagerViewModel()
        {
            _youTubeService = new YouTubeService();

            // Commands
            OpenInputCommand = new RelayCommand(OpenInputDialog, () => SelectedLiveType.Contains("File") || SelectedLiveType.Contains("Folder") || SelectedLiveType.Contains("Text"));

            OpenImagesCommand = new RelayCommand(OpenImagesDialog, () => BooleanAddImagesProperty);
            AddStreamCommand = new RelayCommand(AddStreamAsync, () => !IsBusy && InputQuery.IsNotBlank() && SelectedOutputItem.IsNotBlank());
        }



        #endregion

        #region Methods

        //Add Stream
        private async void AddStreamAsync()
        {
            



            if (SelectedLiveType.Contains("Video"))
            {
                if (!YouTubeService.TryParseVideoId(InputQuery, out var videoId))
                    videoId = InputQuery;

                var streamInfoSet = await _youTubeService.GetVideoMediaStreamInfosAsync(videoId);
                var streamUrl = streamInfoSet.Muxed.WithHighestVideoQuality().Url;
                var nameStream = NameStream;
                var liveType = SelectedLiveType;
                var inputQuery = InputQuery;
                var output = SelectedOutputItem;
                var filter = BooleanFilterProperty.ToString();
                var loop = BooleanLoopProperty.ToString();
                //var cropCode = CropText;
                //var imagePath = ImagePath;
                //var preset = SelectedPresets;
                //var size = SelectedSizes;
                //var bitrate = SelectedBitrates;
                //var framerate = SelectedFramerates;
                //var uSpeed = SelectedUSpeeds;
                //var thread = SelectedThreads;
                //var cpu = SelectedCPUs;
                //var blur = SelectedBlurs;
                //var speed = SelectedSpeeds;
                //var volume = SelectedVolumes;
                var status = "aaaa";

                var streamCode = "-ffmpeg -re -stream_loop -1";
                var createdDate = DateTime.Now;

                var stream = new StreamInfo(nameStream, liveType, inputQuery, streamUrl, output, loop, streamCode, status, createdDate);

                StreamInfos.Add(stream);
            }
        }

        //OpenInputDialog
        private void OpenInputDialog()
        {
            IsBusy = true;
            if (SelectedLiveType.Contains("Text"))
            {
                FileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                fileDialog.ShowDialog();
                InputQuery = fileDialog.FileName;
            }
            else if (SelectedLiveType.Contains("File"))
            {
                FileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Media files (*.mp4;*.mov;*.avi;*.mkv;*.flv)|*.mp4;*.mov;*.avi;*.mkv;*.flv|All files (*.*)|*.*";
                fileDialog.ShowDialog();
                InputQuery = fileDialog.FileName;
            }
            else if(SelectedLiveType.Contains("Folder"))
            {
                var dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                InputQuery = dialog.SelectedPath;

            }
            else
            {
                System.Windows.MessageBox.Show("Please input URL ^_^!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            IsBusy = false;
        }

        //Open Images Dialog
        private void OpenImagesDialog()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg;*.png;*.jpeg;*.gif)|*.jpg;*.png;*.jpeg;*.gif|All files (*.*)|*.*";
            fileDialog.ShowDialog();
            AddImages = fileDialog.FileName;
        }

        #endregion
    }
}
