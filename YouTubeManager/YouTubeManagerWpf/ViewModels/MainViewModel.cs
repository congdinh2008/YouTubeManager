using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace YouTubeManagerWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _selectedTabViewModel;
        private ObservableCollection<ViewModelBase> _tabViewModels = new ObservableCollection<ViewModelBase>();

        #region Properties
        public ObservableCollection<ViewModelBase> TabViewModels
        {
            get => _tabViewModels;
            private set
            {
                _tabViewModels = value;
                RaisePropertyChanged("TabViewModels");
            }
        }

        public ViewModelBase SelectedTabViewModel
        {
            get { return _selectedTabViewModel; }
            set
            {
                if (_selectedTabViewModel != value)
                {
                    _selectedTabViewModel = value;
                    RaisePropertyChanged("SelectedTabViewModel");
                }
            }
        }
        #endregion


        public MainViewModel()
        {
            TabViewModels.Add(new YouTubeManagerViewModel { Header = "YouTube Manager" });
            TabViewModels.Add(new LiveManagerViewModel { Header = "Live Manager" });
            TabViewModels.Add(new ReupManagerViewModel { Header = "Reup Manager" });
            SelectedTabViewModel = TabViewModels[1];
        }
    }
}
