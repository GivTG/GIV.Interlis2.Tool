using System;
using System.Collections.Generic;

namespace GIV.Interlis2.Tools.Common.Contollers
{
    /// <summary>
    /// Interface for all Controllers to work a command-type
    /// </summary>
    public interface IController : IDisposable
    {
        /// <summary>
        /// Get the name (type) for this controller
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Get Text for show help
        /// </summary>
        /// <returns>string with help text</returns>
        string GetHelpText();
        /// <summary>
        /// Get the Command for Show in Help
        /// </summary>
        /// <returns></returns>
        string GetCommandText();

        #region Logger
        /// <summary>
        /// Messages from Execution
        /// </summary>
        List<string> LoggerMessages { get; }
        /// <summary>
        /// Path to loggerfile
        /// </summary>
        string LogPath { set;  get; }
        #endregion

        /// <summary>
        /// run the command
        /// </summary>
        /// <returns></returns>
        bool Execute();
    }
}
