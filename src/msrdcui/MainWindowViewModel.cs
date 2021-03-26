using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace rdclauncher
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand OpenAboutThisAppUriCommand { get; private set; }
        public RelayCommand ClearRemoteComputerHistoryCommand { get; private set; }

        public MainWindowViewModel()
        {
            ConnectCommand = new RelayCommand(ExecuteConnect, CanExecuteConnect);
            OpenAboutThisAppUriCommand = new RelayCommand(ExecuteOpenAboutThisAppUri);
            ClearRemoteComputerHistoryCommand = new RelayCommand(ExecuteClearRemoteComputerHistory);
            WindowTitle = WindowTitleBuilder.GetWindowTitle();
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private bool _isInteractableElementsEnabled = true;
        public bool IsInteractableElementsEnabled
        {
            get => _isInteractableElementsEnabled;
            set => SetProperty(ref _isInteractableElementsEnabled, value);
        }

        private Visibility _closingMessageVisibility = Visibility.Collapsed;
        public Visibility ClosingMessageVisibility
        {
            get => _closingMessageVisibility;
            set => SetProperty(ref _closingMessageVisibility, value);
        }

        private string _remoteComputer = "";
        public string RemoteComputer
        {
            get => _remoteComputer;
            set
            {
                var trimedValue = value.Trim();
                if (SetProperty(ref _remoteComputer, trimedValue))
                {
                    ConnectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ObservableCollection<string> _remoteComputerHistory = new ObservableCollection<string>();
        public ObservableCollection<string> RemoteComputerHistory
        {
            get => _remoteComputerHistory;
            set => SetProperty(ref _remoteComputerHistory, value);
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

        private ObservableCollection<string> _rdcWindowTitleHistory = new ObservableCollection<string>();
        public ObservableCollection<string> RdcWindowTitleHistory
        {
            get => _rdcWindowTitleHistory;
            set => SetProperty(ref _rdcWindowTitleHistory, value);
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

        private async void ExecuteConnect(object obj)
        {
            // Disable and hide the interactable elements.
            IsInteractableElementsEnabled = false;
            ClosingMessageVisibility = Visibility.Visible;

            // Save a new history as user settings.
            PersistentUserSettings.SaveRemoteComputerHistory(RemoteComputer, RemoteComputerHistory);
            PersistentUserSettings.SaveRdcWindowTitleHistory(RdcWindowTitle, RdcWindowTitleHistory);

            try
            {
                await Task.Run(() =>
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
                });
            }
            catch (FileNotFoundException)
            {
                const string MsrdcExeNotFoundMessageText = "Couldn't found the Windows Desktop client (msrdc.exe) on this system. " +
                    "This application requires the Windows Desktop client installation." + "\n\n" +
                    "Do you want to download the Windows Desktop client? " +
                    "Click Yes if you want to open the Windows Desktop client installer download page, otherwise click No.";
                var result = MessageBox.Show(MsrdcExeNotFoundMessageText, WindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    const string WindowsDesktopClientDownloadUri = "https://docs.microsoft.com/en-us/windows-server/remote/remote-desktop-services/clients/windowsdesktop";
                    UriNavigator.Navigate(WindowsDesktopClientDownloadUri);
                }
            }

            Application.Current.Shutdown();
        }

        private void ExecuteOpenAboutThisAppUri(object obj)
        {
            const string AboutThisAppUri = "https://github.com/tksh164/rdclauncher";
            UriNavigator.Navigate(AboutThisAppUri);
        }

        private void ExecuteClearRemoteComputerHistory(object obj)
        {
            RemoteComputerHistory.Clear();
            PersistentUserSettings.ClearRemoteComputerHistory();
        }
    }
}
