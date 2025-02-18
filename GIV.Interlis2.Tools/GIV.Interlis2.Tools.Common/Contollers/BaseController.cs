using GIV.Interlis2.Tools.Common.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GIV.Interlis2.Tools.Common.Contollers
{
    /// <summary>
    /// Base Class for all Controllers
    /// Not for run a command
    /// </summary>
    public class BaseController : IController
    {
        #region Propertys and Attributs
        private string controllerName = "unknow";

        private string helpText = String.Empty;

        public string LogPath { get; set; }

        public List<string> LoggerMessages { get; private set; } = new List<string>();

        private int errorCount;
        /// <summary>
        /// Count of errors.
        /// </summary>
        public int ErrorCount => errorCount;

        /// <summary>
        /// Returns true if HasErrors count > 0
        /// </summary>
        public bool HasErrors => errorCount > 0;

        private int warningCount;
        /// <summary>
        /// Count of warnings.
        /// </summary>
        public int WarningCount => warningCount;

        /// <summary>
        /// Returns true if HasWarnings Count > 0
        /// </summary>
        public bool HasWarnings => warningCount > 0;
        private int infoCount;
        #endregion

        /// <summary>
        /// Base Controller
        /// </summary>
        /// <param name="name">name of controller</param>
        public BaseController(string name)
        {
            controllerName = name;
            LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{Guid.NewGuid()}.log");
        }

        #region Name and Help-Text
        /// <summary>
        /// Name of Controller for Display an Logging
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return controllerName;
        }

        /// <summary>
        /// Set the text for show in help
        /// </summary>
        /// <param name="text">text for help</param>
        public void SetHelpText(string text)
        {
            helpText = text;
        }

        /// <summary>
        /// Get the text for show help
        /// </summary>
        /// <returns>the help text</returns>
        public string GetHelpText()
        {
            return helpText;
        }

        /// <summary>
        /// Return the Command text for show in help
        /// </summary>
        /// <returns>command text without leading chars</returns>
        public virtual string GetCommandText()
        {
            return "unknow";
        }
        #endregion

        #region Execute
        /// <summary>
        /// Controller is not ready for working, override in explizit controllers
        /// </summary>
        /// <returns>trow allway a error</returns>
        public virtual bool Execute()
        {
            throw new NotImplementedException("Controller is not implemented");
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseController()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region LogMessages
        /// <summary>
        /// Create Header-Informations for Log-File
        /// </summary>
        internal void AddLogFileHeader()
        {
            AddLogMessage($"** GIV - Interlis2 Tool ({GetAssemblyVersionString()}) **");
            AddLogMessage($"** {DateTime.Now:dd.MM.yyyy HH:mm:ss} - {Resources.ConvertLogMessageHeaderInfo} **");
            AddLogMessage("** ------------------------------------------------------------------------------------------ **");
            AddLogMessage(String.Empty);
        }

        internal void AddLogFileFooter()
        {
            AddLogMessage(String.Empty);
            AddLogMessage("** ------------------------------------------------------------------------------------------ **");
            AddLogMessage($"** {DateTime.Now} - {((HasErrors || HasWarnings) ? Resources.ConvertLogWriterFooterMessageError : Resources.ConvertLogWriterFooterMessageSuccess)} **");
            AddLogMessage("** ========================================================================================== **");
        }

        /// <summary>
        /// Clear Log-Messages
        /// </summary>
        internal void ClearLogMessages()
        {
            LoggerMessages.Clear();
        }
        /// <summary>
        /// Write Information
        /// </summary>
        internal void LogInfo(string message)
        {
            AddLogMessage($"INF: {message}");
            infoCount++;
        }
        /// <summary>
        /// Write Time
        /// </summary>
        internal void LogTime()
        {
            AddLogMessage($"TIM: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        }
        /// <summary>
        /// Write Warning
        /// </summary>
        /// <param name="message"></param>
        internal void LogWarning(string message)
        {
            AddLogMessage($"WRN: {message}");
            warningCount++;
        }
        /// <summary>
        /// Write Error
        /// </summary>
        /// <param name="message"></param>
        internal void LogError(string message)
        {
            AddLogMessage($"ERR: {message}");
            errorCount++;
        }

        internal void LogStartConvert()
        {
            AddLogMessage($"INF: {DateTime.Now:dd.MM.yyyy HH:mm: ss} - {Resources.ConvertLogMessageStartConvert}");
        }

        private void AddLogMessage(string message)
        {
            LoggerMessages.Add(message);
        }
        #endregion

        #region Get Assembly-Version
        // Return the actual version from EXE to Display in Form
        private string GetAssemblyVersionString()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return String.Format("Version: {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
        #endregion Get Assembly-Version

    }
}