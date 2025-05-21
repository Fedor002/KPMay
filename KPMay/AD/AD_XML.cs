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

        /// <summary>
        /// Метод находит узел по ключевому атрибуту.
        /// </summary>
        /// <param name="Key">Кортеж из названия узла и значения наприимер.("id", "0")</param>
        public XmlNode GetNodeByKey((string name, string value) Key)
        {
            try
            { 
                return _doc.SelectSingleNode($"//*[@{Key.name}='{Key.value}']"); 
            }
            catch
            {
                throw new Exception($"Узел <c атрибутом{Key.name}='{Key.value}'> не найден в XML.");
            }          
        }

        /// <summary>
        /// Метод добавляет узел в XML-файл для узла с указанным атрибутом и значением, если потомок уже существует, то его заменят.
        /// </summary>
        /// <param name="node">Кортеж из названия узла и значения наприимер.("Grade", "5")</param>
        /// <param name="id">Кортеж из названия узла и значения наприимер.("id", "0")</param>
        public void AddUniqueChildToNodeById((string name, string innerText) node, (string name, string value) id)
        {
            XmlNode Parent = _doc.SelectSingleNode($"//*[@{id.name}='{id.value}']");
            if (Parent == null)
                throw new Exception($"Узел <c атрибутом{id.name}='{id.value}'> не найден в XML.");

            XmlElement child = _doc.CreateElement(node.name);
            child.InnerText = node.innerText;

            XmlNode check_child_exist = Parent.SelectSingleNode(node.name);
            if (check_child_exist != null)
            {
                Parent.ReplaceChild(child, check_child_exist);
            }
            else
            {
                Parent.AppendChild(child);
            }
        }


        public void AddMatrixToNode((string name,double[,] matrix) node, (string name, string value) id)
        {
            int rows = node.matrix.GetLength(0);
            int cols = node.matrix.GetLength(1);

            XmlNode Parent = _doc.SelectSingleNode($"//*[@{id.name}='{id.value}']");
            if (Parent == null)
                throw new Exception($"Узел <c атрибутом{id.name}='{id.value}'> не найден в XML.");

            XmlElement matrix_e = _doc.CreateElement(node.name);

            for (int i = 0; i < rows; i++)
            {
                XmlElement row = _doc.CreateElement("row");
                for (int j = 0; j < cols; j++)
                {
                    XmlElement cell = _doc.CreateElement("cell");
                    cell.InnerText = node.matrix[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                    row.AppendChild(cell);
                }
                matrix_e.AppendChild(row);
            }
            XmlNode check_matrix_exist = Parent.SelectSingleNode(node.name);
            if (check_matrix_exist != null)
            {
                Parent.ReplaceChild(matrix_e, check_matrix_exist);
            }
            else
            {
                Parent.AppendChild(matrix_e);
            }
        }

        private AD_Tree GetTreeFromXml(XmlNode xmlNode)
        {
            AD_Tree tree = new AD_Tree
            {
                Name = xmlNode.Attributes?["name"]?.Value ?? xmlNode.Name,
                Id = xmlNode.Attributes?["id"]?.Value ?? string.Empty
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
        /// <returns>Возвращает строку с содержимым тега или пустую строку, если тег не найден.</returns>
        public string read_value(string tag_name)
        {
            string rezult = string.Empty;

            using (XmlReader fr = XmlReader.Create(_path))
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
        public string read_value_by_path(string tag_name)
        {
            string result = string.Empty;

            using (FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
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

        public DataSet ReadAllValues()
        {
            DataSet result = new DataSet();
            using (DataSet ds = new DataSet())
            {
                ds.ReadXml(_path);
                result = ds;
            }
            return result;
        }
    }
}