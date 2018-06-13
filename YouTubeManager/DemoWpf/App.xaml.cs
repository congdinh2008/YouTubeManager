using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Locator.Init();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Locator.Cleanup();
        }
    }
}
