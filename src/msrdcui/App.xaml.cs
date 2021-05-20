using System;
using System.Windows;

namespace rdclauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            SetupUnhandledExceptionHandling();
        }

        private void SetupUnhandledExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
        }
    }
}
