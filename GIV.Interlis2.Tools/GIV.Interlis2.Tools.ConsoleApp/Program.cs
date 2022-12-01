using GIV.Interlis2.Tools.Common.Contollers;
using GIV.Interlis2.Tools.Common.Domain;
using GIV.Interlis2.Tools.ConsoleApp.Properties;
using GIV.Interlis2.Tools.Domain;
using System;
using System.Collections.Generic;
using System.IO;

namespace GIV.Interlis2.Tools
{
    /*
     * DEBUG Command: 
     * --type convertDSS2TGMEL --input "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\Muster_DSS_2020.xtf" --output "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\TG_2020_MEL.xtf" --log "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\dss2tgmel.log"
     * --type convertDSS2TGGEP --input "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\Muster_DSS_2020.xtf" --output "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\TG_2020_GEP.xtf" --log "C:\_repos_GIT\GIV.Interlis2.Tools\DebugData\dss2tggep.log"
     */
    class Program
    {
        /// <summary>
        /// All Available Controllers
        /// </summary>
        private static Dictionary<string, Type> availableControllers = AvailableControllers.Get();

        static ExitCode exitCode = ExitCode.Error;

        static int Main(string[] args)
        {
            Console.WriteLine(Resources.MainConsoleTitle);
            Console.WriteLine(string.Empty);

            if (args == null && args.Length < 1)
            {
                // Call without arguments >> Exit with error
                return (int)exitCode;

            }
            else
            {
                // Check if Command for Help
                if (args.Length == 1)
                {
                    string argument = args[0];
                    if (argument.Equals("-h") || argument.Equals("--help"))
                    {
                        WriteHelp();
#if DEBUG
                        Console.WriteLine(Resources.MainConsolePressAnyKeyForExit);
                        Console.ReadKey();
#endif
                        exitCode = ExitCode.Success;
                        return (int)exitCode;
                    }

                    return (int)exitCode;
                }

                try
                {
                    // Parse Arguments
                    var runData = ArgumentParser.Parse(args);
                    // Run Command with parsed arguments
                    RunCommand(runData);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine(Resources.MainConsoleMissingArgumentErrorMessage, ex.ParamName);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(Resources.MainConsoleFileNotFoundErrorMessage, ex.FileName);
                }
                catch (Exception ex) {
                    Console.WriteLine(Resources.MainConsoleGeneralRuntimeMessage, ex.Message);
                }
#if DEBUG
                Console.WriteLine(Resources.MainConsolePressAnyKeyForExit);
                Console.ReadKey();
#endif
                return (int)exitCode;
            }

        }

        private static void RunCommand(RunData runData)
        {
            if (!runData.IsReady)
            {
                Console.WriteLine(Resources.MainConsoleCantNotStartConvertErrorMessage);
                WriteHelp();
                exitCode = ExitCode.Error;
                return;
            }

            // Check if type from Arguments available as controller
            if (!availableControllers.ContainsKey(runData.Type))
            {
                Console.WriteLine(Resources.MainConsoleTypeNotFoundErrorMessage, runData.Type);
                exitCode = ExitCode.Error;
                return;
            }
            try
            {
                using (IController controller = (IController)Activator.CreateInstance(availableControllers[runData.Type], new[] { runData }))
                {
                    Console.WriteLine(Resources.MainConsoleStartCommandMessage, controller.GetName());
                    // Execute command
                    if (controller.Execute())
                    {
                        Console.WriteLine(Resources.MainConsoleEndCommandSuccessMessage, controller.GetName());
                        exitCode = ExitCode.Success;
                    }
                    else
                    {
                        Console.WriteLine(Resources.MainConsoleEndCommandErrorMessage,controller.GetName());
                        exitCode = ExitCode.Error;
                    }
                    // Write Log Infos
                    File.AppendAllLines(runData.LogFile, controller.LoggerMessages);
                    Console.WriteLine($"{Resources.MainConsoleLogFileMessage} {runData.LogFile}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.MainConsoleGeneralRuntimeMessage, ex.Message);
                exitCode = ExitCode.Error;
                return;
            }
        }

        #region Write Help Command (-h | --help)
        private static void WriteHelp()
        {
            Console.WriteLine("Hilfe zum Tool");
            Console.WriteLine("========================================================");
            Console.WriteLine("Verfügbare Kommandos");
            Console.WriteLine("\t-h | --help");
            Console.WriteLine("\tZeigt diese Hilfe an.");
            Console.WriteLine("========================================================");
            Console.WriteLine("Konvertierungen mit folgenden Argumenten:");
            Console.WriteLine("--type [Typ] --input [Dateiname] --output [Dateiname] --log [Dateiname]");
            Console.WriteLine("Verfügbare Typen [Typ]:");

            foreach (var controllers in availableControllers)
                using (IController controller = (IController)Activator.CreateInstance(controllers.Value))
                {
                    Console.WriteLine($"{controller.GetCommandText()}: {controller.GetName()}");
                    Console.WriteLine($"\t{controller.GetHelpText()}");
                }
        } 
        #endregion
    }
}
