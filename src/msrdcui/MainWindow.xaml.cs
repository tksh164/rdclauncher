using System.Windows;

namespace msrdcui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel()
            {
                PortNumber = Properties.Settings.Default.PortNumber,
                IsFitSessionToWindowEnabled = Properties.Settings.Default.DefaultFitSessionToWindowEnabled,
                IsUpdateResolutionOnResizeEnabled = Properties.Settings.Default.DefaultUpdateResolutionOnResizeEnabled,
                IsFullScreenEnabled = Properties.Settings.Default.DefaultFullScreenEnabled,
            };
        }
    }
}
