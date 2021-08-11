using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.Domain
{
    /// <summary>
    /// Parse argumnts from input array
    /// </summary>
    internal static class ArgumentParser
    {
        /// <summary>
        /// Parse all arguments
        /// </summary>
        /// <param name="arguments">array of arguments (key, value pairs)</param>
        /// <returns>Values from arguments as object</returns>
        internal static RunData Parse(string[] arguments)
        {
            var runData = new RunData();

            int i = 0;
            foreach(string argument in arguments)
            {
                switch(argument)
                {
                    // ToDo: Check missing index in Array
                    // ToDo: Check next value begin without leading -- or -
                    case "--type":
                    case "-t":
                        runData.SetType(arguments[i + 1]);
                        break;
                    case "--input":
                    case "-i":
                        runData.SetInput(arguments[i + 1]);
                        break;
                    case "--output":
                    case "-o":
                        runData.SetOutput(arguments[i + 1]);
                        break;
                    case "--log":
                    case "-log":
                        runData.SetLogFile(arguments[i + 1]);
                        break;
                }
                i++;
            }

            return runData;
        }
    }
}