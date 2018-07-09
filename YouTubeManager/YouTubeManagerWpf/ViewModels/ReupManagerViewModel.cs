using GalaSoft.MvvmLight;

namespace YouTubeManagerWpf.ViewModels
{
    public class ReupManagerViewModel : ViewModelBase, IReupManagerViewModel, ITabViewModel
    {
        #region Properties
        public string Header { get; set; }
        #endregion
        public ReupManagerViewModel()
        {

        }
    }
}