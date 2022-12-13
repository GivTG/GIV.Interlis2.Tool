using GIV.Interlis2.Tools.App.Settings;
using GIV.Interlis2.Tools.App.ViewModels;
using GIV.Interlis2.Tools.Common.Contollers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GIV.Interlis2.Tools.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsController settingsController;
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            settingsController = new SettingsController();

            // Set Title and Version infos
            WindowTitleLabel.Content = settingsController.GetAssemblyTitleString();
            InfoVersionLabel.Content = settingsController.GetAssemblyVersionString();

            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
        }


        #region window function (close, minimize,drag, restore)
        // Close Window
        private void MainWindow_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            OnClosed(e);
        }
        // Minimize Window
        private void MainWindow_Minimize(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        // Drag Window (relocate)
        private void MainWindow_Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        //
        private void MainWindow_Restore(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                RestoreButtonTextBlock.Text = "1";
                WindowState = WindowState.Normal;
            }
            else
            {
                RestoreButtonTextBlock.Text = "2";
                WindowState = WindowState.Maximized;
            }
        }

        #endregion

        private void InputFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel?.SelectInputFilePath();
        }

        private void OutputFileSelectButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel?.SelectOutputFilePath();
        }

        private void LogFileSelectButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel?.SelectLogFilePath();
        }

        private void FunctionTypeDssToTgmelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(mainWindowViewModel is null) return;

            mainWindowViewModel.ActionType = ConvertDSS2TGMEL.FUNCTIONTYPE;
        }

        private void FunctionTypeDssToTggepRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWindowViewModel is null) return;

            mainWindowViewModel.ActionType = ConvertDSS2TGGEP.FUNCTIONTYPE;
        }

        private void FunctionTypeTgmelFilterRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWindowViewModel is null) return;

            mainWindowViewModel.ActionType = SplitDSS2Melio.FUNCTIONTYPE;
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            await mainWindowViewModel?.Execute();
        }
    }
}
