using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.Contollers
{
    /// <summary>
    /// Base Class for all Controllers
    /// Not for run a command
    /// </summary>
    internal class BaseController : IController
    {
        #region Propertys and Attributs
        private string controllerName = "unknow";

        private string helpText = String.Empty;

        public string LogPath { get; private set; } = @"C:\Temp\CreaterLogFile.txt";

        public List<string> LoggerMessages { get; private set; } = new List<string>(); 
        #endregion

        /// <summary>
        /// Base Controller
        /// </summary>
        /// <param name="name">name of controller</param>
        public BaseController(string name)
        {
            controllerName = name;
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
    }
}