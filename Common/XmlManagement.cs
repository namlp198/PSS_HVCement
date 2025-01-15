using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml;

namespace PSS_HVCement.Common
{
    public class XmlManagement : XmlDocument
    {
        private string m_sXmlFilePath;

        private XPathNavigator m_xpathNavigator;

        private bool m_bIsXmlValid = true;

        public string XmlFilePath
        {
            get
            {
                return m_sXmlFilePath;
            }
            set
            {
                m_sXmlFilePath = value;
            }
        }

        public bool Open(string sXmlFilePath)
        {
            try
            {
                if (!File.Exists(sXmlFilePath))
                {
                    return false;
                }

                Close();
                m_sXmlFilePath = sXmlFilePath;
                using (FileStream fileStream = new FileStream(m_sXmlFilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    Load(fileStream);
                    fileStream.Close();
                }

                m_xpathNavigator = CreateNavigator();
                return IsXmlValidToItsXSD(sXmlFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool OpenFromXml(string sXmlData)
        {
            try
            {
                if (string.IsNullOrEmpty(sXmlData))
                {
                    return false;
                }

                Close();
                m_sXmlFilePath = "";
                LoadXml(sXmlData);
                m_xpathNavigator = CreateNavigator();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Create(string sXmlFilePath, string sStartElementName)
        {
            try
            {
                m_sXmlFilePath = sXmlFilePath;
                XmlTextWriter xmlTextWriter = new XmlTextWriter(m_sXmlFilePath, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument(standalone: false);
                xmlTextWriter.WriteStartElement(sStartElementName);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                return Open(m_sXmlFilePath);
            }
            catch
            {
                return false;
            }
        }

        private bool IsXmlValidToItsXSD(string sXmlFilePath)
        {
            try
            {
                m_bIsXmlValid = true;
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.ValidationType = ValidationType.Schema;
                xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                xmlReaderSettings.ValidationEventHandler += XmlValidationErrorEventHandler;
                using (XmlReader xmlReader = XmlReader.Create(sXmlFilePath, xmlReaderSettings))
                {
                    while (xmlReader.Read())
                    {
                    }

                    xmlReader.Close();
                }
            }
            catch
            {
                m_bIsXmlValid = false;
            }

            return m_bIsXmlValid;
        }

        public bool IsXmlModified()
        {
            Save("tmp.xml");
            string text = File.ReadAllText(XmlFilePath);
            string text2 = File.ReadAllText("tmp.xml");
            File.Delete("tmp.xml");
            if (text == text2)
            {
                return false;
            }

            return true;
        }

        public void XmlValidationErrorEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
            {
                m_bIsXmlValid = false;
            }
        }

        public bool Save()
        {
            if (m_sXmlFilePath != null)
            {
                return Save(m_sXmlFilePath);
            }

            return false;
        }

        public new bool Save(string sFileName)
        {
            try
            {
                base.Save(sFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            m_xpathNavigator = null;
            m_sXmlFilePath = "";
        }

        public int GetRootChildCount()
        {
            m_xpathNavigator.MoveToChild(XPathNodeType.Element);
            XPathNodeIterator xPathNodeIterator = m_xpathNavigator.SelectChildren(XPathNodeType.Element);
            ResetNavigator();
            return xPathNodeIterator.Count;
        }

        public string GetNodeValueFromXPath(string sXPath)
        {
            XmlNode xmlNode = SelectSingleNode(sXPath);
            if (xmlNode != null)
            {
                return xmlNode.InnerXml;
            }

            return "";
        }

        public int GetNodeChildrenCount(string sXPath)
        {
            return m_xpathNavigator.SelectSingleNode(sXPath)?.SelectChildren(XPathNodeType.Element).Count ?? 0;
        }

        public XPathNodeIterator GetNodeChildrenIterator(string sXPath)
        {
            return m_xpathNavigator.SelectSingleNode(sXPath)?.SelectChildren(XPathNodeType.Element);
        }

        public XmlNodeList GetNodeChildren(string sXPath)
        {
            return SelectSingleNode(sXPath)?.ChildNodes;
        }

        public string GetAttributeValueFromXPath(string sXPath, string sAttributeName)
        {
            return GetAttributeValueFromNode(SelectSingleNode(sXPath), sAttributeName);
        }

        public string GetAttributeValueFromNode(XmlNode node, string sAttributeName)
        {
            try
            {
                if (node != null && node.Attributes != null && node.Attributes[sAttributeName] != null)
                {
                    return node.Attributes[sAttributeName].InnerXml;
                }
            }
            catch
            {
            }

            return string.Empty;
        }

        public string GetAttributeValueFromXPathWithDefaultValue(string sXPath, string sAttributeName, string sDefaultValue)
        {
            try
            {
                XmlNode xmlNode = SelectSingleNode(sXPath);
                if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes[sAttributeName] != null)
                {
                    return xmlNode.Attributes[sAttributeName].InnerXml;
                }
            }
            catch
            {
            }

            return sDefaultValue;
        }

        public int GetAttributeCount(string sXPath)
        {
            try
            {
                XmlNode xmlNode = SelectSingleNode(sXPath);
                if (xmlNode != null && xmlNode.Attributes != null)
                {
                    return xmlNode.Attributes.Count;
                }
            }
            catch
            {
            }

            return 0;
        }

        public XmlAttributeCollection GetAttributeList(string sXPath)
        {
            return SelectSingleNode(sXPath)?.Attributes;
        }

        public int SelectAttributeValueFromXPath(string sXPath, string sAttributeName, string startingId)
        {
            XmlNodeList xmlNodeList = SelectNodes(sXPath);
            List<int> list = new List<int>();
            foreach (XmlNode item in xmlNodeList)
            {
                if (startingId == "1")
                {
                    if (Convert.ToInt32(item.Attributes[sAttributeName].InnerXml.ToString()) < 1000)
                    {
                        list.Add(Convert.ToInt32(item.Attributes[sAttributeName].InnerXml.ToString()));
                    }
                }
                else if (Convert.ToInt32(item.Attributes[sAttributeName].InnerXml.ToString()) > 1000)
                {
                    list.Add(Convert.ToInt32(item.Attributes[sAttributeName].InnerXml.ToString()));
                }
            }

            return list.OrderByDescending((int n) => n).First() + 1;
        }

        public bool SetAttributeValueFromXPath(string sXPath, string sAttributeName, string sAttributeValue)
        {
            try
            {
                XmlNode xmlNode = SelectSingleNode(sXPath);
                if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes[sAttributeName] != null)
                {
                    xmlNode.Attributes[sAttributeName].InnerXml = sAttributeValue;
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public bool SetNodeValueFromXPath(string sXPath, string sNodeValue)
        {
            XmlNode node = SelectSingleNode(sXPath);
            return SetNodeValueFromNode(node, sNodeValue);
        }

        public bool SetNodeValueFromXPath(string sXPath, byte[] data)
        {
            return SetNodeValueFromXPath(sXPath, GetXmlStringFromBytesArray(data));
        }

        public bool SetNodeValueFromNode(XmlNode node, string sNodeValue)
        {
            if (node != null)
            {
                node.InnerXml = sNodeValue;
                return true;
            }

            return false;
        }

        public XmlNodeList GetNodeListFromXPath(string sXPath)
        {
            return SelectNodes(sXPath);
        }

        public XmlNode AddChildNode(string sXPathParent, string sNodeName)
        {
            try
            {
                XmlNode xmlNode = SelectSingleNode(sXPathParent);
                XmlNode xmlNode2 = CreateNode(XmlNodeType.Element, sNodeName, "");
                xmlNode.AppendChild(xmlNode2);
                return xmlNode2;
            }
            catch
            {
                return null;
            }
        }

        public XmlNode AddChildNode(XmlNode parentNode, string sNodeName)
        {
            try
            {
                XmlNode xmlNode = CreateNode(XmlNodeType.Element, sNodeName, "");
                parentNode.AppendChild(xmlNode);
                return xmlNode;
            }
            catch
            {
                return null;
            }
        }

        public void AddAttributeToNode(XmlNode node, string sAttributeName, string sAttributeValue)
        {
            try
            {
                XmlAttribute xmlAttribute = CreateAttribute(sAttributeName);
                xmlAttribute.Value = sAttributeValue;
                node.Attributes.Append(xmlAttribute);
            }
            catch
            {
            }
        }

        public void AddAttributeToNode(string sXPath, string sAttributeName, string sAttributeValue)
        {
            try
            {
                XmlNode xmlNode = SelectSingleNode(sXPath);
                XmlAttribute xmlAttribute = CreateAttribute(sAttributeName);
                xmlAttribute.Value = sAttributeValue;
                xmlNode.Attributes.Append(xmlAttribute);
            }
            catch
            {
            }
        }

        public void CopyChildNodes(XmlNode SrcNode, XmlNode DestNode)
        {
            foreach (XmlNode childNode in SrcNode.ChildNodes)
            {
                XmlNode newChild = childNode.CloneNode(deep: true);
                DestNode.AppendChild(newChild);
            }
        }

        public void RemoveChildNodes(XmlNode BaseNode)
        {
            XmlNodeList childNodes = BaseNode.ChildNodes;
            for (int num = childNodes.Count - 1; num >= 0; num--)
            {
                XmlNode xmlNode = childNodes[num];
                RemoveChildNodes(xmlNode);
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }
        }

        public void RemoveNodes(string sXPath)
        {
            XmlNodeList xmlNodeList = SelectNodes(sXPath);
            for (int num = xmlNodeList.Count - 1; num >= 0; num--)
            {
                XmlNode xmlNode = xmlNodeList[num];
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }
        }

        public bool IsNodeExisting(string sXPath)
        {
            return SelectSingleNode(sXPath) != null;
        }

        public bool IsAttributeExisting(string sXPath, string sAttributeName)
        {
            XmlNode xmlNode = SelectSingleNode(sXPath);
            if (xmlNode == null)
            {
                return false;
            }

            return xmlNode.Attributes[sAttributeName] != null;
        }

        public bool IsAttributeExisting(XmlNode node, string sAttributeName)
        {
            if (node == null)
            {
                return false;
            }

            return node.Attributes[sAttributeName] != null;
        }

        public byte[] GetByteArrayFromXPath(string sXPath)
        {
            string text = RemoveCDATA(GetNodeValueFromXPath(sXPath));
            if (text != "")
            {
                return Encryption.Base64DecodeToBytes(text);
            }

            return null;
        }

        public string RemoveCDATA(string sValue)
        {
            return sValue.Replace("&lt;", "<").Replace("&gt;", ">").Replace("<![CDATA[", "")
                .Replace("]]>", "");
        }

        public XmlNode SearchNode(string sXPath)
        {
            return SelectSingleNode(sXPath);
        }

        private void ResetNavigator()
        {
            m_xpathNavigator.MoveToRoot();
            m_xpathNavigator.MoveToChild(XPathNodeType.Element);
        }

        public static int GetNodeChildrenCount(XmlNode parentNode)
        {
            return parentNode.CreateNavigator()?.SelectChildren(XPathNodeType.Element).Count ?? 0;
        }

        public static XPathNodeIterator GetNodeChildrenIterator(XmlNode parentNode)
        {
            return parentNode.CreateNavigator()?.SelectChildren(XPathNodeType.Element);
        }

        public static string GetXmlStringFromBytesArray(byte[] data)
        {
            if (data == null)
            {
                return "";
            }

            string text = "<![CDATA[";
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                text += Encryption.Base64EncodeStream(memoryStream);
                text += "]]>";
                memoryStream.Close();
                return text;
            }
        }

        public static string IndentXMLString(string xml)
        {
            _ = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(xml);
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlDocument.WriteContentTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    return new StreamReader(memoryStream).ReadToEnd();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
