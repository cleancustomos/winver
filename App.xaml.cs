using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System.Globalization;
using Windows.Globalization;
using WinUIEx;

namespace winver
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .BuildServiceProvider());

            var culture = CultureInfo.CurrentUICulture.Name;

            ApplicationLanguages.PrimaryLanguageOverride = culture;
        }

        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            m_window.ExtendsContentIntoTitleBar = true;
            m_window.CenterOnScreen();
        }

        private Window m_window;
    }
}
