using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ISDCore
{
    public class XmlHelper
    {
        public static string GetXmlNodeValue(ref XmlDocument xmlDocument, string xPath, ref bool exist)
        {
            string Ret = "";

            exist = false;
            XmlNode XmlNodeObject = xmlDocument.SelectSingleNode(xPath);
            if (null != XmlNodeObject)
            {
                exist = true;
                Ret = XmlNodeObject.InnerText;
            }

            return Ret;
        }

        public static VarType GetXmlNodeValue<VarType>(ref XmlDocument xmlDocument, string xPath, ref bool exist, VarType def)
        {
            VarType Ret = def;

            string ret = GetXmlNodeValue(ref xmlDocument, xPath, ref exist);
            if (exist)
                Ret = ConvertHelper.Convert<VarType>(ret, def);

            return Ret;
        }

        public static VarType GetXmlAttributeValue<VarType>(XmlNode XmlNode, string Name, ref bool Exist, VarType def)
        {
            VarType Ret = def;

            string ret = GetXmlAttributeValue(XmlNode, Name, ref Exist);
            if (Exist)
                Ret = ConvertHelper.Convert<VarType>(ret, def);

            return Ret;
        }
        public static string GetXmlAttributeValue(XmlNode XmlNode, string Name, ref bool Exist)
        {
            string Ret = "";

            XmlAttribute xmlAttributeTemp;
            xmlAttributeTemp = XmlNode.Attributes[Name];
            Exist = (null != xmlAttributeTemp);

            if (Exist)
                Ret = xmlAttributeTemp.Value;

            return Ret;
        }
        public static void InsertXmlValue(IDictionary<string, string> Dic, string path, string RootElement)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlElement ParentElement = xmlDoc.CreateElement(RootElement);
            ParentElement.SetAttribute("id", Dic["ID"]);
            foreach (var item in Dic)
            {
                XmlElement element = xmlDoc.CreateElement(item.Key);
                element.SetAttribute("Value", item.Value);
                ParentElement.AppendChild(element);
            }
            xmlDoc.DocumentElement.AppendChild(ParentElement);
            xmlDoc.Save(path);
        }
        public static void UpdateXmlValue(IDictionary<string, string> Dic, string path, string XPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode aNode = xmlDoc.SelectSingleNode(XPath);
            var ChildNodes = aNode.ChildNodes;
            foreach (var item in ChildNodes)
            {
                XmlNode node = (XmlNode)item;
                node.Attributes[node.Name].Value = Dic[node.Name];
            }
            xmlDoc.Save(path);
        }
    }
}
