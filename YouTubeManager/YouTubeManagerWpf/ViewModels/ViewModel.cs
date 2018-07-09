using System.Collections.ObjectModel;

namespace YouTubeManagerWpf.ViewModels
{
    public sealed class ViewModel
    {
        public ObservableCollection<TabItem> TabItems { get; set; }

        public ViewModel()
        {
            TabItems = new ObservableCollection<TabItem>();
            TabItems.Add(new TabItem { Header = "YouTube Manager"/*, Content = "YouTubeManagerViewModel"*/ });
            TabItems.Add(new TabItem { Header = "Live Manager"/*, Content = "LiveManagerViewModel" */});
        }
        
    }
    public sealed class TabItem
    {
        public string Header { get; set; }
        //public string Content { get; set; }
    }
}
