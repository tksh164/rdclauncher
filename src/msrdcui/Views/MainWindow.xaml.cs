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

            vm.SelectedSessionResolution = new SessionsResolution();
            vm.SessionResolutionList = new ObservableCollection<SessionsResolution>
            {
                vm.SelectedSessionResolution,
                new SessionsResolution(2560, 1440),
                new SessionsResolution(1920, 1200),
                new SessionsResolution(1920, 1080),
                new SessionsResolution(1600, 1200),
                new SessionsResolution(1280, 1024),
                new SessionsResolution(1024, 768),
                new SessionsResolution(800, 600)
            };

            return vm;
        }
    }
}
