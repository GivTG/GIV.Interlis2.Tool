using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GIV.Interlis2.Tools.IO
{
    /// <summary>
    /// Helper for write XML Document
    /// </summary>
    public class XmlWriteHelper
    {
        /// <summary>
        /// Write XML Nodes to file UTF-8 without BOM
        /// </summary>
        /// <param name="xmlDocument">XML Document</param>
        /// <param name="fileNameAndPath">Path and File-Name (e.g. C:\Temp\GIV_ILI.xtf)</param>
        /// <returns></returns>
        public static bool WriteXmlDocument(XmlDocument xmlDocument, string fileNameAndPath)
        {
            try
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Encoding = new UTF8Encoding(false);
                xmlSettings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(fileNameAndPath, xmlSettings))
                {
                    xmlDocument.Save(writer);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
