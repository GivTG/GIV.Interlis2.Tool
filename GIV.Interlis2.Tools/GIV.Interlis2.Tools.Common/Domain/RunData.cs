using GIV.Interlis2.Tools.Common.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.Common.Domain
{
    /// <summary>
    /// Run Data object for console and ui
    /// </summary>
    public class RunData
    {
        /// <summary>
        /// All Datas are ready for run
        /// </summary>
        public bool IsReady { get; private set; } = false;

        /// <summary>
        /// Command type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Input Path with Filename
        /// </summary>
        public string Input { get; private set; }

        /// <summary>
        /// Output Path with Filename
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Output File can be overwriten without user confirmation 
        /// </summary>
        public bool OutputOverwrite { get; private set; }

        /// <summary>
        /// File Path to Log-File
        /// </summary>
        public string LogFile { get; private set; }

        /// <summary>
        /// Log File can be overwriten without user confirmation 
        /// </summary>
        public bool LogFileOverwrite { get; private set; }

        /// <summary>
        /// Message with result from check for overwrite the output and log-file
        /// </summary>
        public string FileOverwriteMessage { get; private set; }

        /// <summary>
        /// Set command type
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            Type = type;
            CheckData();
        }

        /// <summary>
        /// Set and check input file
        /// </summary>
        /// <param name="input">Path and File-Name</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public void SetInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }
            if (!File.Exists(input))
            {
                throw new FileNotFoundException("Datei nicht gefunden.", input);
            }

            Input = input;
            CheckData();
        }

        /// <summary>
        /// Set and check output file
        /// </summary>
        /// <param name="output">Path and File-Name</param>
        /// <param name="canOverwrite">if TRUE overwrite without user confirmation</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void SetOutput(string output, bool canOverwrite)
        {
            if (string.IsNullOrEmpty(output))
            {
                throw new ArgumentNullException("output");
            }
            if (CheckIsFileReadOnly(output))
            {
                throw new Exception(Resources.OutputFileReadOnlyMessage);
            }

            Output = output;
            OutputOverwrite = canOverwrite;
            CheckData();
        }

        /// <summary>
        /// Set and check log file
        /// </summary>
        /// <param name="logfile">Path and File-Name</param>
        /// <param name="canOverwrite">if TRUE overwrite without user confirmation</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void SetLogFile(string logfile, bool canOverwrite)
        {
            if (string.IsNullOrEmpty(logfile))
            {
                throw new ArgumentNullException("logfile");
            }
            if (CheckIsFileReadOnly(logfile))
            {
                throw new Exception(Resources.LogFileReadOnlyMessage);
            }

            LogFile = logfile;
            LogFileOverwrite = canOverwrite;
            CheckData();
        }

        /// <summary>
        /// Check are all output-files confirmed for overwrite
        /// </summary>
        /// <returns></returns>
        public bool FileOverwriteIsConfirmed()
        {
            FileOverwriteMessage = Resources.OverwriteMessageOk;
            if (OutputOverwrite && LogFileOverwrite) return true;

            if (!OutputOverwrite && LogFileOverwrite)
            {
                FileOverwriteMessage = Resources.OverwriteMeassgeOutputFile;
            }
            else if (OutputOverwrite && !LogFileOverwrite)
            {
                FileOverwriteMessage = Resources.OverwriteMessageLogFile;
            }
            else
            {
                FileOverwriteMessage = Resources.OverwriteMessageAllFiles;
            }

            return false;
        }


        private void CheckData()
        {
            if (
                string.IsNullOrEmpty(Type) ||
                string.IsNullOrEmpty(Input) ||
                string.IsNullOrEmpty(Output) ||
                string.IsNullOrEmpty(LogFile)
                )
            {
                IsReady = false;
                return;
            }

            IsReady = true;
        }

        private bool CheckIsFileReadOnly(string path)
        {
            if (!File.Exists(path)) return false;

            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.IsReadOnly;
        }
    }
}