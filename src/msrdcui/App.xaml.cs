using System;
using System.Windows;
using rdclauncher.Views;

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
            var exceptionReportWindow = new ExceptionReportWindow(ExceptionReportBuilder.GetReportText(e.ExceptionObject as Exception));
            exceptionReportWindow.ShowDialog();
        }
    }
}
