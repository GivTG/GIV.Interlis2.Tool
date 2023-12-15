using GIV.Interlis2.Tools.Common.Domain;
using GIV.Interlis2.Tools.Common.IO;
using GIV.Interlis2.Tools.Common.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GIV.Interlis2.Tools.Common.Contollers
{
    public class SplitDSS2Melio : BaseController
    {
        // DEBUG
        // --type splitDSS2melio --input "C:\_repos\GIV.Interlis2.Tool\DebugData\Muster_DSS_2020.xtf" --output "C:\_repos\GIV.Interlis2.Tool\DebugData\TG_2020_SPLIT.xtf" --log "C:\_repos\GIV.Interlis2.Tool\DebugData\dss2splitmelgep.log"

        #region Propertys and Attributs
        /// <summary>
        /// Contains all classes from VSA DSS 2020 not relevant for melioration
        /// </summary>
        private readonly HashSet<string> irreleventClasses = new HashSet<string>()
        {
            //// Filter Klassen
            //"Kanal",
            //"Abwasserknoten",

            //// Haltung
            //"Haltung",
            //"Haltung_Alternativverlauf",
            //"Haltung_Text",
            //"Haltungspunkt",
            //"Rohrprofil",
            //"Rohrprofil_Geometrie",

            //// Abwasserbauwerke
            //"Abwasserbauwerk",
            //"Abwasserbauwerk_Symbol",
            //"Abwasserbauwerk_Text",

            //"Abflusslose_Toilette",
            //"Einleitstelle",
            //"Entsorgung",
            //"KLARA",
            //"Normschacht",
            //"Spezialbauwerk",
            //"Versickerungsanlage",

            //// Bauwerksteile
            //"Bankett",
            //"Beckenentleerung",
            //"Beckenreinigung",
            //"Deckel",
            //"Einstiegshilfe",
            //"ElektrischeEinrichtung",
            //"ElektromechanischeAusruestung",
            //"Feststoffrueckhalt",
            //"Rueckstausicherung",
            //"Spuelstutzen",
            //"Trockenwetterfallrohr",
            //"Trockenwetterrinne",

            // VSA
            "Massnahme",
            "Mutation",
            "MechanischeVorreinigung", // -> ref Abwasserbauwerk
            "Retentionskoerper", // -> ref Versickerungsanlage

            // ARA
            "Abwasserreinigungsanlage",
            "Abwasserbehandlung",
            "ARABauwerk",
            "ARAEnergienutzung",
            "Steuerungszentrale",
            "Schlammbehandlung",

            // Anschlussobjekte
            "Anschlussobjekt",
            "Brunnen",
            "Einzelflaeche",
            "Gebaeude",
            "Reservoir",

            // Entsorgung
            "Landwirtschaftsbetrieb",
            "Gebaeudegruppe",
            "Gebaeudegruppe_BAUGWR",

            // Einzugsgebiet
            "Einzugsgebiet",
            "Einzugsgebiet_Text",
            "Gesamteinzugsgebiet",
            "Oberflaechenabflussparameter",
            "EZG_PARAMETER_ALLG",
            "EZG_PARAMETER_MOUSE1",

            // Hydraulik
            "Hydr_Geometrie",
            "Hydr_GeomRelation",
            "Hydr_Kennwerte",

            // Messung
            "Messgeraet",
            "Messreihe",
            "Messresultat",
            "Messstelle",

            // KEK - Erhaltungsereignis
            "Erhaltungsereignis",
            "Biol_oekol_Gesamtbeurteilung",
            "Unterhalt",
            "Stammkarte",

            // Ueberlauf
            "Absperr_Drosselorgan",
            "FoerderAggregat",
            "Leapingwehr",
            "Streichwehr",
            "Ueberlauf",
            "Ueberlaufcharakteristik",
            "HQ_Relation",

            // ZONE
            "Zone",
            "Entwaesserungssystem",
            "Versickerungsbereich",
        };
        
        private RunData runData;

        /// <summary>
        /// Name of the filter to apply
        /// </summary>
        private const string FILTERNAME = "\"nur Meliorationsleitungen\"";

        /// <summary>
        /// Name of Controller for Display and Logging
        /// </summary>
        private const string CONTROLLERNAME = "Filtert DSS in TG-MEL";

        /// <summary>
        /// Name of FunctionType
        /// Key: must be unique
        /// </summary>
        public const string FUNCTIONTYPE = "splitDSS2melio";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for split
        /// </summary>
        /// <param name="data">runtime data with all settings</param>
        public SplitDSS2Melio(RunData data) : base(CONTROLLERNAME)
        {
            runData = data;
            InitHelpText();
        }

        /// <summary>
        /// Empty Constructor for create Instance with Activation
        /// Not using for work, only for write Help
        /// </summary>
        public SplitDSS2Melio() : base(CONTROLLERNAME)
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
            SetHelpText(Resources.SplitControllerHelpTextMelioration);
        }
        #endregion

        #region Execute
        public override bool Execute()
        {
            ClearLogMessages();
            AddLogFileHeader();
            LogInfo(String.Format(Resources.SplitLogMessageTitle, FILTERNAME));
            LogInfo($"{Resources.ConvertLogMessageInputFile} {runData.Input}");
            LogInfo($"{Resources.ConvertLogMessageOutputFile} {runData.Output}");
            LogStartConvert();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(runData.Input);

            foreach (XmlNode rootNode in xmlDocument.ChildNodes)
            {
                if (rootNode.Name == "TRANSFER")
                {
                    foreach (XmlNode secondLevelNode in rootNode.ChildNodes)
                    {
                        if (secondLevelNode.Name == "DATASECTION")
                        {
                            // loop all topic nodes -> remove irrelvant childs
                            foreach (XmlNode topicNode in secondLevelNode.ChildNodes)
                            {
                                // Remove irrelevante Nodes
                                var irrelevantNodes = GetIrrelevantChildNodes(topicNode);
                                RemoveChildNodes(topicNode, irrelevantNodes);
                                // Handle other Nodesl
                                HandleWasteWaterNodes(topicNode);
                            }

                            break; // DATASECTION node processed -> no need to loop further trough the TRANSFER node
                        }
                    }

                    break; // TRANSFER node processed -> no need to loop further trough the root node
                }
            }

            XmlWriteHelper.WriteXmlDocument(xmlDocument, runData.Output);
            //xmlDocument.Save(runData.Output);

            AddLogFileFooter();

            return true;
        }
        #endregion

        #region Private Functions

        List<string> sewerOIDs = new List<string>();
        List<string> wastewaterNodes = new List<string>();
        List<string> wastewaterSites = new List<string>();
        List<XmlNode> removableChildes = new List<XmlNode>();

        private void HandleWasteWaterNodes(XmlNode topicNode)
        {
            // Search all Sewers and Nodes with "Funktion*Melioration"
            foreach(XmlNode node in topicNode)
            {
                var className = GetNodeClassName(node.Name);

                switch(className)
                {
                    case "Kanal":
                        HandleSewer(node);
                        break;
                    case "Abwasserknoten":
                        HandleWastewaterNode(node);
                        break;
                    default:
                        break;
                };
            }

            // Handle Sections Nodes
            HandleSections(topicNode);

            // Handle Wastewater Site and Constructions
            HandleSiteAndConstructions(topicNode);

            // Remove Childs
            RemoveChildNodes(topicNode, removableChildes);

        }

        private void HandleSewer(XmlNode node)
        {
            var oid = node.Attributes["TID"].Value;

            bool isMelio = false;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "FunktionMelioration")
                {
                    isMelio = true;
                    break;
                }
            }

            if (isMelio)
            {
                sewerOIDs.Add(oid);
                wastewaterSites.Add(oid);
                return;
            }

            removableChildes.Add(node);
        }

        private void HandleSections(XmlNode topicNode)
        {
            //"Haltung",
            //"Haltung_Alternativverlauf",
            //"Haltung_Text",
            //"Haltungspunkt",
            //"Rohrprofil",
            //"Rohrprofil_Geometrie",

            List<string> sectionsOIDList = new List<string>();
            List<string> pipeProfilsOIDList = new List<string>();
            List<string> sectionPointsOIDList = new List<string>();

            // Collect Sections
            foreach(XmlNode node in topicNode)
            {
                var className = GetNodeClassName(node.Name);

                if (className == "Haltung")
                {
                    if (!CheckAndAddRemovableNode(node, "AbwasserbauwerkRef", sewerOIDs))
                    {
                        var oid = node.Attributes["TID"].Value;
                        sectionsOIDList.Add(oid);
                        AddRefOIDToList(node, pipeProfilsOIDList, "RohrprofilRef");
                        AddRefOIDToList(node, sectionPointsOIDList, "vonHaltungspunktRef");
                        AddRefOIDToList(node, sectionPointsOIDList, "nachHaltungspunktRef");
                    }
                }
            }

            // Collect Releted Sections Objects
            foreach (XmlNode node in topicNode)
            {
                var className = GetNodeClassName(node.Name);

                switch (className)
                {
                    case "Haltung_Alternativverlauf":
                    case "Haltung_Text":
                        CheckAndAddRemovableNode(node, "HaltungRef", sectionsOIDList);
                        break;
                    case "Rohrprofil":
                        CheckAndAddRemovableNode(node, pipeProfilsOIDList);
                        break;
                    case "Rohrprofil_Geometrie":
                        CheckAndAddRemovableNode(node, "RohrprofilRef", pipeProfilsOIDList);
                        break;
                    case "Haltungspunkt":
                        CheckAndAddRemovableNode(node, sectionPointsOIDList);
                        break;
                    default:
                        break;
                };
            }

        }

        private void HandleWastewaterNode(XmlNode node)
        {
            var oid = node.Attributes["TID"].Value;

            bool isMelio = false;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Funktion_Knoten_Melioration")
                {
                    isMelio = true;
                    break;
                }
            }

            if (isMelio)
            {
                wastewaterNodes.Add(oid);
                AddRefOIDToList(node, wastewaterSites, "AbwasserbauwerkRef");
                return;
            }

            removableChildes.Add(node);
        }


        private void HandleSiteAndConstructions(XmlNode topicNode)
        {
            // Collect Releted Sections Objects
            foreach (XmlNode node in topicNode)
            {
                var className = GetNodeClassName(node.Name);

                switch (className)
                {
                    case "Abflusslose_Toilette":
                    case "Einleitstelle":
                    case "Entsorgung":
                    case "KLARA":
                    case "Normschacht":
                    case "Spezialbauwerk":
                    case "Versickerungsanlage":
                        CheckAndAddRemovableNode(node, wastewaterSites);
                        break;
                    case "Bankett":
                    case "Beckenentleerung":
                    case "Beckenreinigung":
                    case "Deckel":
                    case "Einstiegshilfe":
                    case "ElektrischeEinrichtung":
                    case "ElektromechanischeAusruestung":
                    case "Feststoffrueckhalt":
                    case "Rueckstausicherung":
                    case "Spuelstutzen":
                    case "Trockenwetterfallrohr":
                    case "Trockenwetterrinne":
                        CheckAndAddRemovableNode(node, "AbwasserbauwerkRef", wastewaterSites);
                        break;
                    case "Abwasserbauwerk_Symbol":
                    case "Abwasserbauwerk_Text":
                        CheckAndAddRemovableNode(node, "AbwasserbauwerkRef", wastewaterSites);
                        break;
                    case "Abwasserbauwerk":
                        CheckAndAddRemovableNode(node, wastewaterSites);
                        break;
                    default:
                        break;
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listWithOIDs"></param>
        /// <returns></returns>
        private bool CheckAndAddRemovableNode(
            XmlNode node /* node for check */,
            List<string> listWithOIDs /* list with valid oids (not removabel) */)
        {
            var oid = node.Attributes["TID"].Value;

            if (!listWithOIDs.Contains(oid))
            {
                removableChildes.Add(node);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Check Node is removable and add to list for removing
        /// </summary>
        /// <param name="node"></param>
        /// <param name="relName"></param>
        /// <param name="listWithOIDs"></param>
        /// <returns>true = is removable, false = is NOT removable</returns>
        private bool CheckAndAddRemovableNode(
            XmlNode node /* node for check */, 
            string relName /* name of related XML Node */, 
            List<string> listWithOIDs /* list with valid oids (not removabel) */)
        {
            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == relName)
                {
                    var value = childNode.Attributes["REF"].Value;
                    if (!listWithOIDs.Contains(value))
                    {
                        removableChildes.Add(node);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the class node is relevant for meloration.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsMeliorationRelevant(XmlNode node)
        {
            var className = GetNodeClassName(node.Name);

            if (irreleventClasses.Contains(className, StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get all child class nodes not relevant for melioration.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private List<XmlNode> GetIrrelevantChildNodes(XmlNode xmlNode)
        {
            var irrelevantNodes = new List<XmlNode>();

            // loop all class nodes
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!IsMeliorationRelevant(childNode))
                {
                    irrelevantNodes.Add(childNode);
                }
            }

            return irrelevantNodes;
        }

        /// <summary>
        /// Remove the passed irrelevant child nodes from the node.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="removableChildNodes"></param>
        private void RemoveChildNodes(XmlNode xmlNode, List<XmlNode> removableChildNodes)
        {
            // remove irrelevant nodes
            foreach (XmlNode removableChildNode in removableChildNodes)
            {
                LoggerMessages.Add(string.Format(Resources.SplitLogMessageClassRemoved, removableChildNode.Name, removableChildNode.Attributes["TID"].Value));
                xmlNode.RemoveChild(removableChildNode); // remove irrelevant class nodes from topic node
            }
        }


        private string GetNodeClassName(string fullNodeName /*"ModelName.TopicName.ClassName"*/)
        {
            var nodeNameSplited = fullNodeName.Split('.'); // { "ModelName", "TopicName", "ClassName" }
            return nodeNameSplited.Last(); // "ClassName"
        }


        private void AddRefOIDToList(XmlNode node, List<string> listToAdd, string nodeName)
        {
            foreach(XmlNode child in node.ChildNodes)
            {
                if (child.Name == nodeName & CheckAttributExists(child, "REF"))
                {
                    string oid = child.Attributes["REF"].Value;
                    if (!listToAdd.Contains(oid)) listToAdd.Add(oid);
                    return;
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