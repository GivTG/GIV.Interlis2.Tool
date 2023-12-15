using GIV.Interlis2.Tools.Common.Domain;
using GIV.Interlis2.Tools.Common.IO;
using GIV.Interlis2.Tools.Common.Properties;
using System;
using System.Xml;

namespace GIV.Interlis2.Tools.Common.Contollers
{
    public class ConvertDSS2TGMEL : BaseController
    {
        // DEBUG:
        // --type convertDSS2TGMEL --input "C:\_repos\GIV.Interlis2.Tool\DebugData\Muster_DSS_2020.xtf" --output "C:\_repos\GIV.Interlis2.Tool\DebugData\TG_2020_MEL.xtf" --log "C:\_repos\GIV.Interlis2.Tool\DebugData\dss2tgmel.log"

        #region Propertys and Attributs
        /// <summary>
        /// Data object with informations for running this controller
        /// </summary>
        private RunData runData;

        /// <summary>
        /// ILI-Tag from Input Modell
        /// </summary>
        private const string INPUTMODELL = "DSS_2020_LV95";
        /// <summary>
        /// ILI-Tag for Output Modell and ILI-Attribut Name in  in HEADERSECTION -> MODELL
        /// </summary>
        private const string OUTPUTMODELLNAME = "DSS_2020_LV95_MEL";
        /// <summary>
        /// ILI-Attribut Version in HEADERSECTION -> MODELL
        /// </summary>
        private const string OUTPUTMODELLVERSION = "25.06.2021";
        /// <summary>
        /// ILI-Attribut URI in HEADERSECTION -> MODELL
        /// </summary>
        private const string OUTPUTMODELLURI = "http://www.vsa.ch/models";

        /// <summary>
        /// Name of Controller for Display and Logging
        /// </summary>
        private const string CONTROLLERNAME = "Konvertiert DSS in TG-MEL";

        /// <summary>
        /// Name of FunctionType
        /// Key: must be unique
        /// </summary>
        public const string FUNCTIONTYPE = "convertDSS2TGMEL";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for convert
        /// </summary>
        /// <param name="data">runtime data with all settings</param>
        public ConvertDSS2TGMEL(RunData data) : base(CONTROLLERNAME)
        {
            runData = data;
            InitHelpText();
        }

        /// <summary>
        /// Empty Constructor for create Instance with Activation
        /// Not using for work, only for write Help
        /// </summary>
        public ConvertDSS2TGMEL() : base(CONTROLLERNAME)
        {
            InitHelpText();
        }
        #endregion

        #region Command Text
        public override string GetCommandText()
        {
            return FUNCTIONTYPE;
        }

        private void InitHelpText()
        {
            SetHelpText(String.Format(Resources.ConvertControllerHelpTextModelToModel, INPUTMODELL, OUTPUTMODELLNAME));
        }
        #endregion

        #region Execute
        public override bool Execute()
        {
            ClearLogMessages();
            AddLogFileHeader();
            LogInfo(String.Format(Resources.ConvertLogMessageTitle, INPUTMODELL, OUTPUTMODELLNAME));
            LogInfo($"{Resources.ConvertLogMessageInputFile} {runData.Input}");
            LogInfo($"{Resources.ConvertLogMessageOutputFile} {runData.Output}");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(runData.Input);

            foreach(XmlNode xmlNode in xmlDocument.ChildNodes)
            {
                if (xmlNode.Name == "TRANSFER")
                {
                    foreach(XmlNode childNode in xmlNode.ChildNodes)
                    {
                        ReadAndChange(childNode);
                    }
                }
            }

            XmlWriteHelper.WriteXmlDocument(xmlDocument, runData.Output);
            //xmlDocument.Save(runData.Output);

            AddLogFileFooter();

            return true;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Read Nodes and change the modelname
        /// </summary>
        /// <param name="xmlNode"></param>
        private void ReadAndChange(XmlNode xmlNode)
        {
            //Console.WriteLine($"Name: {xmlNode.Name} - {xmlNode.Value}");

            if (xmlNode.Name == "HEADERSECTION")
            {
                ChangeHeaderSection(xmlNode);
            }

            if (xmlNode.Name == "DATASECTION")
            {
                xmlNode.InnerXml = xmlNode.InnerXml.Replace(INPUTMODELL, OUTPUTMODELLNAME);
            }
            #region sample with node by node
            // ToDo make safe method to rewrite
            //if (xmlNode.Name.StartsWith("DSS_2020_LV95"))
            //{
            //    
            //    /*
            //            foreach (XmlNode stuffNode in stuffNodeList)
            //            {
            //                // get existing 'Content' node
            //                XmlNode contentNode = stuffNode.SelectSingleNode("Content");

            //                // create new (renamed) Content node
            //                XmlNode newNode = xmlDoc.CreateElement(contentNode.Name + stuffNode.Name);

            //                // [if needed] copy existing Content children
            //                //newNode.InnerXml = stuffNode.InnerXml;

            //                // replace existing Content node with newly renamed Content node
            //                stuffNode.InsertBefore(newNode, contentNode);
            //                stuffNode.RemoveChild(contentNode);
            //            }*/
            //

            //    //Console.WriteLine(xmlNode.Name.Replace("DSS_2020_LV95", "DSS_2020_LV95_MEL"));
            //}
            //string ns = xmlNode.LocalName;


            //if (xmlNode.HasChildNodes)
            //{
            //    foreach (XmlNode childNode in xmlNode.ChildNodes)
            //    {
            //        ReadAndChange(childNode);
            //    }
            //}
            #endregion

        }

        /// <summary>
        /// Change Headersection with Model-Name, Version and URI
        /// </summary>
        /// <param name="xmlNode"></param>
        private void ChangeHeaderSection(XmlNode xmlNode)
        {
            foreach (XmlNode innerNode in xmlNode.ChildNodes)
            {
                if (innerNode.Name == "MODELS")
                {
                    CheckAndChangeModel(innerNode);
                }
            }
        }
        /// <summary>
        /// Change the correct Model Entry
        /// </summary>
        /// <param name="modelNodes"></param>
        private void CheckAndChangeModel(XmlNode modelNodes)
        {
            foreach (XmlNode model in modelNodes.ChildNodes)
            {
                // <MODEL NAME="DSS_2020_LV95" VERSION="25.06.2021" URI="http://www.vsa.ch/models" />
                if (model.Attributes["NAME"].Value == INPUTMODELL)
                {
                    model.Attributes["NAME"].Value = OUTPUTMODELLNAME;
                    if (CheckAttributExists(model, "VERSION")) model.Attributes["VERSION"].Value = OUTPUTMODELLVERSION;
                    if (CheckAttributExists(model, "URI")) model.Attributes["URI"].Value = OUTPUTMODELLURI;
                }
            }
        }

        /// <summary>
        /// Check is attribut exists on this xml node
        /// </summary>
        /// <param name="node">XmlNode for check attribut</param>
        /// <param name="attributName">attribut name for check</param>
        /// <returns></returns>
        private bool CheckAttributExists(XmlNode node, string attributName)
        {
            foreach (XmlAttribute attribut in node.Attributes)
            {
                if (attribut.Name == attributName)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
