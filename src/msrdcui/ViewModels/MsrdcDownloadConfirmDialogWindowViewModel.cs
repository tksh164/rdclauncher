﻿using System;
using System.Windows;
using System.Windows.Media;

namespace rdclauncher.ViewModels
{
    public enum MsrdcDownloadDialogResult
    {
        Cancel,
        OpenDownloadSite
    }

    public sealed class MsrdcDownloadConfirmDialogWindowViewModel : ViewModelBase
    {
        public MsrdcDownloadConfirmDialogWindowViewModel()
        {
            OpenDownloadSiteCommand = new RelayCommand(ExecuteOpenDownloadSiteCommand);
            IconImage = ShellStockIcon.GetIconImage(ShellStockIcon.StockIconKind.Info);
            LinkTextOnDownloadSite = GetLinkTextOnDownloadSite();
            DialogResult = MsrdcDownloadDialogResult.Cancel;
        }

        public RelayCommand OpenDownloadSiteCommand { get; private set; }

        public MsrdcDownloadDialogResult DialogResult { get; private set; }

        private ImageSource _iconImage;
        public ImageSource IconImage
        {
            get => _iconImage;
            private set => SetProperty(ref _iconImage, value);
        }

        private string _linkTextOnDownloadSite;
        public string LinkTextOnDownloadSite
        {
            get => _linkTextOnDownloadSite;
            private set => SetProperty(ref _linkTextOnDownloadSite, value);
        }

        private void ExecuteOpenDownloadSiteCommand(object obj)
        {
            DialogResult = MsrdcDownloadDialogResult.OpenDownloadSite;
            Window dialogWindow = obj as Window;
            dialogWindow.Close();
        }

        private string GetLinkTextOnDownloadSite()
        {
            return Environment.Is64BitOperatingSystem ? "Windows 64-bit" : "Windows 32-bit";

        }
    }
}
