﻿using System.Windows;
using rdclauncher.ViewModels;

namespace rdclauncher.Views
{
    /// <summary>
    /// Interaction logic for ExceptionReportWindow.xaml
    /// </summary>
    public partial class ExceptionReportWindow : Window
    {
        public ExceptionReportWindow(string reportText)
        {
            InitializeComponent();
            DataContext = GetInitializedViewModel(reportText);
        }

        private ExceptionReportWindowViewModel GetInitializedViewModel(string reportText)
        {
            return new ExceptionReportWindowViewModel(this.Close)
            {
                ExceptionReportText = reportText,
            };
        }
    }
}
