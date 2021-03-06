using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace msrdcui
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand OpenAboutThisAppUriCommand { get; private set; }

        public MainWindowViewModel()
        {
            ConnectCommand = new RelayCommand(ExecuteConnect, CanExecuteConnect);
            OpenAboutThisAppUriCommand = new RelayCommand(ExecuteOpenAboutThisAppUri);

            WindowTitle = "Remote Desktop UI";
            PortNumber = "3389";
            IsUpdateResolutionOnResizeEnabled = true;
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private string _remoteComputer = "";
        public string RemoteComputer
        {
            get => _remoteComputer;
            set
            {
                if (SetProperty(ref _remoteComputer, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _portNumber = "";
        public string PortNumber
        {
            get => _portNumber;
            set
            {
                if (SetProperty(ref _portNumber, value))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _rdcWindowTitle = "";
        public string RdcWindowTitle
        {
            get => _rdcWindowTitle;
            set => SetProperty(ref _rdcWindowTitle, value);
        }

        private bool _isFitSessionToWindowEnabled = false;
        public bool IsFitSessionToWindowEnabled
        {
            get => _isFitSessionToWindowEnabled;
            set => SetProperty(ref _isFitSessionToWindowEnabled, value);
        }

        private bool _isUpdateResolutionOnResizeEnabled = false;
        public bool IsUpdateResolutionOnResizeEnabled
        {
            get => _isUpdateResolutionOnResizeEnabled;
            set => SetProperty(ref _isUpdateResolutionOnResizeEnabled, value);
        }

        private bool _isFullScreenEnabled = false;
        public bool IsFullScreenEnabled
        {
            get => _isFullScreenEnabled;
            set => SetProperty(ref _isFullScreenEnabled, value);
        }

        private bool CanExecuteConnect(object arg)
        {
            return !string.IsNullOrWhiteSpace(RemoteComputer) && !RemoteComputer.Contains(' ') && uint.TryParse(PortNumber, out _);
        }

        private void ExecuteConnect(object obj)
        {
            try
            {
                MsrdcExecution.LaunchMsrdc(new MsrdcLaunchSettings()
                {
                    RemoteComputer = RemoteComputer,
                    RemotePort = uint.Parse(PortNumber),
                    WindowTitle = RdcWindowTitle,
                    IsFitSessionToWindowEnabled = IsFitSessionToWindowEnabled,
                    IsUpdateResolutionOnResizeEnabled = IsUpdateResolutionOnResizeEnabled,
                    IsFullScreenEnabled = IsFullScreenEnabled,
                });
            }
            catch (FileNotFoundException)
            {
                const string MsrdcExeNotFoundMessageText = "This application requires the Windows Desktop client. " +
                    "But cannot found the Windows Desktop client (msrdc.exe) in this system. " +
                    "Click OK if you want to open the Windows Desktop client installer download page, otherwise click Cancel.";
                var result = MessageBox.Show(MsrdcExeNotFoundMessageText, WindowTitle, MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (result == MessageBoxResult.OK)
                {
                    const string WindowsDesktopClientDownloadUri = "https://docs.microsoft.com/en-us/windows-server/remote/remote-desktop-services/clients/windowsdesktop";
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = WindowsDesktopClientDownloadUri,
                        UseShellExecute = true,
                    });
                }
            }

            Application.Current.Shutdown();
        }

        private void ExecuteOpenAboutThisAppUri(object obj)
        {
            const string AboutThisAppUri = "https://github.com/tksh164/msrdcui";
            Process.Start(new ProcessStartInfo()
            {
                FileName = AboutThisAppUri,
                UseShellExecute = true,
            });
        }
    }
}
