using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using YouTubeManagerWpf.ViewModels;

namespace YouTubeManagerWpf
{
    public class Locator
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
        }

        public static void Cleanup()
        {
        }

        public IMainViewModel MainViewModel => ServiceLocator.Current.GetInstance<IMainViewModel>();
    }
}
