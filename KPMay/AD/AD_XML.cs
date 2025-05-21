using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace KPMay
{
    public class AD_XML
    {
        private XmlDocument _doc  ;
        private string _path  ;

        public XmlDocument doc
        {
            get { return _doc; }
        }
        public string path
        {
            get { return _path; }
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

        public void SaveXML()
        {
            try
            {
                _doc.Save(_path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении xml: {_path}", ex);
            }
        }
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
        /// <summary>
        /// Проверяет, существует ли тег в XML-файле по указанному пути.
        /// </summary>
        /// <returns>Возвращает false, если тег не найден, иначе true.</returns>
        public bool TagExist(string tag_name)
        {
            if (_doc.DocumentElement.SelectSingleNode(tag_name) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AD_Tree GetTreeFrom_xml()
        {
            AD_Tree tree = GetTreeFromXml(_doc.DocumentElement);
            return tree;
        }

        /// <summary>
        /// Метод читает значение первого попавшегося тега из XML-файла по указанному пути, подходит для xml с уникальными тегами.
        /// </summary>
        /// <param name="path">Путь к XML.</param>
        /// <returns>Возвращает строку с содержимым тега или пустую строку, если тег не найден.</returns>
        public static string read_value(string path, string tag_name)
        {
            string rezult = string.Empty;

            using (XmlReader fr = XmlReader.Create(path))
            {
                try
                {
                    while (fr.Read())
                    {
                        if (fr.NodeType == XmlNodeType.Element && fr.Name == tag_name)
                        {
                            rezult = fr.ReadElementContentAsString();
                            return rezult;
                        }
                    }
                }
                catch { }
            }
            return rezult;
        }

        /// <summary>
        /// Метод
        /// </summary>
        /// <param name="tag_name">Пример: @"//encryption_file/epath"</param>
        public static string read_value_by_path(string path, string tag_name)
        {
            string result = string.Empty;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader fr = XmlReader.Create(fs))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(fr);
                    XmlNode node = xml.SelectSingleNode(tag_name);

                    if (node != null)
                    {
                        result = node.InnerText;
                    }
                }
            }

            return result;
        }

        public static DataSet ReadAllValues(string path)
        {
            DataSet result = new DataSet();
            using (DataSet ds = new DataSet())
            {
                ds.ReadXml(path);
                result = ds;
            }
            return result;
        }
    }
}