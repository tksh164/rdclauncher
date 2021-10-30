using System.Windows;
using System.Windows.Media;

namespace rdclauncher.ViewModels
{
    public sealed class HistoryClearConfirmDialogWindowViewModel : ViewModelBase
    {
        public HistoryClearConfirmDialogWindowViewModel()
        {
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            IconImage = ShellStockIcon.GetIconImage(ShellStockIcon.StockIconKind.Warning);
        }

        public RelayCommand ClearCommand { get; private set; }

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }

        private ImageSource _iconImage;
        public ImageSource IconImage
        {
            get => _iconImage;
            private set => SetProperty(ref _iconImage, value);
        }

        private void ExecuteClearCommand(object obj)
        {
            Window dialogWindow = obj as Window;
            dialogWindow.DialogResult = true;
            dialogWindow.Close();
        }
    }
}
