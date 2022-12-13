using GIV.Interlis2.Tools.App.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIV.Interlis2.Tools.App.Settings
{
    internal class SettingsController
    {

        #region Get Assembly-Title
        internal string GetAssemblyTitleString()
        {
            return Resources.AppTitle;
        }
        #endregion

        #region Get Assembly-Version

        // Return the actual version from EXE to Display in Form
        internal string GetAssemblyVersionString()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return String.Format("Version: {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        #endregion Get Assembly-Version
    }
}
