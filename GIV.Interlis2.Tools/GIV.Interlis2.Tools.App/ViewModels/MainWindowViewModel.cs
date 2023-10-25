using GIV.Interlis2.Tools.App.Properties;
using GIV.Interlis2.Tools.Common.Contollers;
using GIV.Interlis2.Tools.Common.Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace GIV.Interlis2.Tools.App.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// All Available Controllers
        /// </summary>
        private static Dictionary<string, Type> availableControllers = AvailableControllers.Get();

        private string actionType = ConvertDSS2TGMEL.FUNCTIONTYPE;
        public string ActionType
        {
            get { return actionType; }
            set
            {
                actionType = value;
                UpdateOutputFilePath();
                UpdateLogFilePath();
                UpdateConvertEnabled(); // needs to be last
                OnPropertyChanged(nameof(InputFilePath));
            }
        }

        private string inputFilePath;
        public string InputFilePath
        {
            get { return inputFilePath; }
            set
            {
                inputFilePath = value;
                UpdateOutputFilePath();
                UpdateLogFilePath();
                UpdateConvertEnabled(); // needs to be last
                OnPropertyChanged(nameof(InputFilePath));
            }
        }
        private bool outputFileCanOverwrite = false;
        private string outputFilePath;
        public string OutputFilePath
        {
            get { return outputFilePath; }
            set
            {
                outputFilePath = value;
                UpdateConvertEnabled();
                OnPropertyChanged(nameof(OutputFilePath));
            }
        }

        private bool logFileCanOverwrite = false;
        private string logFilePath;
        public string LogFilePath
        {
            get { return logFilePath; }
            set
            {
                logFilePath = value;
                UpdateConvertEnabled();
                OnPropertyChanged(nameof(LogFilePath));
            }
        }

        private string viewLogContent;
        public string ViewLogContent
        {
            get { return viewLogContent; }
            set
            {
                viewLogContent = value;
                OnPropertyChanged(nameof(ViewLogContent));
            }
        }

        private bool progressIsVisible = false;
        public bool ProgressIsVisible
        {
            get { return progressIsVisible; }
            set
            {
                progressIsVisible = value;
                OnPropertyChanged(nameof(ProgressIsVisible));
            }
        }

        private bool convertEnabled = false;
        public bool ConvertEnabled
        {
            get { return convertEnabled; }
            set
            {
                convertEnabled = value;
                OnPropertyChanged(nameof(ConvertEnabled));
            }
        }

        public void SelectInputFilePath()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Interlis-Datei (*.xtf)|*.xtf|Alle Dateien (*.*)|*.*",
                RestoreDirectory = true
            };

            if (!string.IsNullOrEmpty(InputFilePath))
            {
                try
                {
                    var initialDirectory = Path.GetDirectoryName(InputFilePath);
                    var initialFileName = Path.GetFileName(InputFilePath);

                    openFileDialog.InitialDirectory = initialDirectory;
                    openFileDialog.FileName = initialFileName;
                }
                catch { } // do nothing
            }

            if (openFileDialog.ShowDialog() ?? false)
            {
                outputFileCanOverwrite = false;
                logFileCanOverwrite = false;
                InputFilePath = openFileDialog.FileName;
            }
        }

        public void SelectOutputFilePath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Interlis-Datei (*.xtf)|*.xtf|Alle Dateien (*.*)|*.*",
                RestoreDirectory = true,
                OverwritePrompt = true
            };

            if (!string.IsNullOrEmpty(OutputFilePath))
            {
                try
                {
                    var initialDirectory = Path.GetDirectoryName(OutputFilePath);
                    var initialFileName = Path.GetFileName(OutputFilePath);

                    saveFileDialog.InitialDirectory = initialDirectory;
                    saveFileDialog.FileName = initialFileName;
                }
                catch { } // do nothing
            }

            if (saveFileDialog.ShowDialog() ?? false)
            {
                OutputFilePath = saveFileDialog.FileName;
                outputFileCanOverwrite = true;
            }
        }

        public void SelectLogFilePath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Log-Datei (*.log)|*.log",
                RestoreDirectory = true,
                OverwritePrompt = true
            };

            if (!string.IsNullOrEmpty(LogFilePath))
            {
                try
                {
                    var initialDirectory = Path.GetDirectoryName(LogFilePath);
                    var initialFileName = Path.GetFileName(LogFilePath);

                    saveFileDialog.InitialDirectory = initialDirectory;
                    saveFileDialog.FileName = initialFileName;
                }
                catch { } // do nothing
            }

            if (saveFileDialog.ShowDialog() ?? false)
            {
                LogFilePath = saveFileDialog.FileName;
                logFileCanOverwrite = true;
            }
        }

        public async Task Execute()
        {
            ProgressIsVisible= true;
            UpdateConvertEnabled();

            var runData = new RunData();

            try
            {
                runData.SetType(ActionType);
                runData.SetInput(InputFilePath);
                runData.SetOutput(OutputFilePath, outputFileCanOverwrite);
                runData.SetLogFile(LogFilePath, logFileCanOverwrite);
            }
            catch (Exception ex)
            {
                ViewLogWriteLine(Resources.GeneralRuntimeMessage, ex.Message);
                ProgressIsVisible = false;
                UpdateConvertEnabled();
                return;
            }

            if (!availableControllers.ContainsKey(runData.Type))
            {
                ViewLogWriteLine(Resources.TypeNotFoundErrorMessage, runData.Type);
                return;
            }

            if (!runData.FileOverwriteIsConfirmed()) {
                MessageBoxResult dialogResult = MessageBox.Show(runData.FileOverwriteMessage, Resources.MessageBoxFileFoundWillOverwrite, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (dialogResult == MessageBoxResult.Cancel)
                {
                    ViewLogWriteLine(Resources.AbortFromUser);
                    ProgressIsVisible = false;
                    UpdateConvertEnabled();
                    return;
                }
            }

            try
            {
                using (IController controller = (IController)Activator.CreateInstance(availableControllers[actionType], new[] { runData }))
                {
                    ViewLogWriteLine(Resources.StartCommandMessage, controller.GetName());
                    // Execute command
                    if (await Task.Run(() => controller.Execute()).ConfigureAwait(false))
                    {
                        ViewLogWriteLine(Resources.EndCommandSuccessMessage, controller.GetName());
                    }
                    else
                    {
                        ViewLogWriteLine(Resources.EndCommandErrorMessage, controller.GetName());
                    }
                    // Write Log Infos
                    File.WriteAllLines(runData.LogFile, controller.LoggerMessages);
                    ViewLogWriteLine($"{Resources.LogFileMessage} {runData.LogFile}");
                }
            }
            catch (Exception ex)
            {
                // ToDo #16 - Message as Dialog-BOX
                ViewLogWriteLine(Resources.GeneralRuntimeMessage, ex.Message);
            }

            ProgressIsVisible = false;
            UpdateConvertEnabled();
        }

        private void UpdateConvertEnabled()
        {
            ConvertEnabled = !string.IsNullOrEmpty(inputFilePath) && !string.IsNullOrEmpty(outputFilePath) && !string.IsNullOrEmpty(logFilePath) && !ProgressIsVisible;
        }

        private void ViewLogWriteLine(string line)
        {
            ViewLogContent = $"{viewLogContent}{DateTime.Now:H:mm:ss} - {line}{Environment.NewLine}";
        }

        private void ViewLogWriteLine(string format, object arg0)
        {
            ViewLogWriteLine(string.Format(format, arg0));
        }

        /// <summary>
        /// Create Output File Path and Name
        /// </summary>
        private void UpdateOutputFilePath()
        {
            if (string.IsNullOrEmpty(inputFilePath)) return;

            try
            {
                var extension = Path.GetExtension(inputFilePath);
                if (string.IsNullOrEmpty(extension)) return;

                var directoryName = Path.GetDirectoryName(inputFilePath);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);

                // use outputFilePath (not OutputFilePath) to avoid multiple calls of UpdateConvertEnabled()
                outputFilePath = Path.Combine(directoryName, $"{fileNameWithoutExtension}_{GetActionNameShort()}{extension}"); 
                OnPropertyChanged(nameof(OutputFilePath));
            }
            catch { } // do nothing
        }
        /// <summary>
        /// Create Log File Path and Name
        /// </summary>
        private void UpdateLogFilePath()
        {
            if (string.IsNullOrEmpty(inputFilePath)) return;

            try
            {
                var extension = Path.GetExtension(inputFilePath);
                if (string.IsNullOrEmpty(extension)) return;

                var directoryName = Path.GetDirectoryName(inputFilePath);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);

                // use logFilePath (not LogFilePath) to avoid multiple calls of UpdateConvertEnabled()
                logFilePath = Path.Combine(directoryName, $"{fileNameWithoutExtension}_GIV_{GetActionNameShort()}.log");
                OnPropertyChanged(nameof(LogFilePath));
            }
            catch { } // do nothing
        }

        private string GetActionNameShort()
        {
            switch (actionType)
            {
                case ConvertDSS2TGMEL.FUNCTIONTYPE:
                    return "TGMEL";
                case ConvertDSS2TGGEP.FUNCTIONTYPE:
                    return "TGGEP";
                case SplitDSS2Melio.FUNCTIONTYPE:
                    return "TGMEL";
                default:
                    return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}