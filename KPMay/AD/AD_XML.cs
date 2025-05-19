using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using KPMay;

namespace KPMay
{
    internal class AD_XML
    {
        private XmlDocument _doc  ;
        private string _path  ;
        private AD_Tree GetTreeFromXml(XmlNode xmlNode)
        {
            AD_Tree tree = new AD_Tree
            {
                Name = xmlNode.Attributes?["name"]?.Value ?? xmlNode.Name
            };

            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    tree.Nodes.Add(GetTreeFromXml(child));
                }
            }

            return tree;
        }
        public void LoadXML(string path)
        {
            _path = path;
            _doc = new XmlDocument();
            try
            {
                _doc.Load(_path);
            }
            catch (Exception ex)
            {
                _doc = null;
                throw new InvalidOperationException($"Ошибка при чтении xml: {path}", ex);
            }
        }

        public AD_Tree GetTreeFrom_xml()
        {
            AD_Tree tree = GetTreeFromXml(_doc.DocumentElement);
            return tree;
        }
    }


}