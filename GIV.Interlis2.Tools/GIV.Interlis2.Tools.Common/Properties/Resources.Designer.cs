﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GIV.Interlis2.Tools.Common.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GIV.Interlis2.Tools.Common.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Konvertiert ein Interlis2-Datendatei vom Modell {0} zum Modell {1}..
        /// </summary>
        internal static string ConvertControllerHelpTextModelToModel {
            get {
                return ResourceManager.GetString("ConvertControllerHelpTextModelToModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Konvertierung erfolgreich abgeschlossen..
        /// </summary>
        internal static string ConvertLogMessageConvertEndSuccess {
            get {
                return ResourceManager.GetString("ConvertLogMessageConvertEndSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input-Datei:.
        /// </summary>
        internal static string ConvertLogMessageInputFile {
            get {
                return ResourceManager.GetString("ConvertLogMessageInputFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Output-Datei:.
        /// </summary>
        internal static string ConvertLogMessageOutputFile {
            get {
                return ResourceManager.GetString("ConvertLogMessageOutputFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start der Konvertierung von {0} zu {1}.
        /// </summary>
        internal static string ConvertLogMessageTitle {
            get {
                return ResourceManager.GetString("ConvertLogMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start der Konvertierung von {0} zu {1}.
        /// </summary>
        internal static string ConvertLogMessageTitle1 {
            get {
                return ResourceManager.GetString("ConvertLogMessageTitle1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Filtert die nicht Meliorations relevanten Daten aus einer DSS 2020 Datei..
        /// </summary>
        internal static string SplitControllerHelpTextMelioration {
            get {
                return ResourceManager.GetString("SplitControllerHelpTextMelioration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Irrelevante Objektklasse entfernen: {0} [{1}].
        /// </summary>
        internal static string SplitLogMessageClassRemoved {
            get {
                return ResourceManager.GetString("SplitLogMessageClassRemoved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Filtern erfolgreich abgeschlossen..
        /// </summary>
        internal static string SplitLogMessageConvertEndSuccess {
            get {
                return ResourceManager.GetString("SplitLogMessageConvertEndSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Anwenden des Filter {0}.
        /// </summary>
        internal static string SplitLogMessageTitle {
            get {
                return ResourceManager.GetString("SplitLogMessageTitle", resourceCulture);
            }
        }
    }
}