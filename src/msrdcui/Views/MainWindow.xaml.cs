using System.Windows;
using System.Collections.ObjectModel;
using rdclauncher.ViewModels;

namespace rdclauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = GetInitializedViewModel();
        }

        private static MainWindowViewModel GetInitializedViewModel()
        {
            var vm = new MainWindowViewModel()
            {
                DefaultPortNumber = Properties.Settings.Default.DefaultPortNumber,
                IsFitSessionToWindowEnabled = Properties.Settings.Default.DefaultFitSessionToWindowEnabled,
                IsUpdateResolutionOnResizeEnabled = Properties.Settings.Default.DefaultUpdateResolutionOnResizeEnabled,
                IsFullScreenEnabled = Properties.Settings.Default.DefaultFullScreenEnabled,
            };

            var remoteComputerHistory = PersistentUserSettings.ReadRemoteComputerHistory();
            foreach (var historyItem in remoteComputerHistory)
            {
                vm.RemoteComputerHistory.Add(historyItem);
            }
            vm.RemoteComputer = remoteComputerHistory.Count > 0 ? remoteComputerHistory[0] : "";

            var rdcWindowTitleHistory = PersistentUserSettings.ReadRdcWindowTitleHistory();
            foreach (var historyItem in rdcWindowTitleHistory)
            {
                vm.RdcWindowTitleHistory.Add(historyItem);
            }

            vm.SelectedSessionScreenSize = new SessionScreenSize();
            vm.SessionScreenSizeList = new ObservableCollection<SessionScreenSize>
            {
                vm.SelectedSessionScreenSize,
                new SessionScreenSize(2560, 1440),
                new SessionScreenSize(1920, 1200),
                new SessionScreenSize(1920, 1080),
                new SessionScreenSize(1600, 1200),
                new SessionScreenSize(1280, 1024),
                new SessionScreenSize(1024, 768),
                new SessionScreenSize(800, 600)
            };

            return vm;
        }
    }
}
