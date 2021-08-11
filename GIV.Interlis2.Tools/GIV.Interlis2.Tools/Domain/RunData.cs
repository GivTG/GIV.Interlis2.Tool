using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.Domain
{
    internal class RunData
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
        /// File Path to Log-File
        /// </summary>
        public string LogFile { get; private set; }


        public void SetType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            Type = type;
            CheckData();
        }

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

        public void SetOutput(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                throw new ArgumentNullException("output");
            }

            Output = output;
            CheckData();
        }

        public void SetLogFile(string logfile)
        {
            if (string.IsNullOrEmpty(logfile))
            {
                throw new ArgumentNullException("logfile");
            }

            LogFile = logfile;
            CheckData();
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
    }
}