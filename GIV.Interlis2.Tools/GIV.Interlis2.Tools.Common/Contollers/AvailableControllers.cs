using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.Common.Contollers
{
    /// <summary>
    /// All Controllers for Help and Execution
    /// </summary>
    public class AvailableControllers
    {
        /// <summary>
        /// Register for all Controllers
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string /*Key: FunctionType*/, Type /*Value: Controller-Class-Type*/> Get()
        {
            var availableControllers = new Dictionary<string, Type>();

            availableControllers.Add(ConvertDSS2TGMEL.FUNCTIONTYPE, typeof(ConvertDSS2TGMEL));
            availableControllers.Add(ConvertDSS2TGGEP.FUNCTIONTYPE, typeof(ConvertDSS2TGGEP));
            availableControllers.Add(SplitDSS2Melio.FUNCTIONTYPE, typeof(SplitDSS2Melio));

            return availableControllers;
        }
    }
}