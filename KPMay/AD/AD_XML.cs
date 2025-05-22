using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Media;

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

        public void CreateXML(string path, string root_name = "root")
        {
            _path = path;
            _doc = new XmlDocument();
            XmlDeclaration declaration = _doc.CreateXmlDeclaration("1.0", "utf-8", null);
            _doc.AppendChild(declaration);
            XmlElement root = _doc.CreateElement(root_name);
            _doc.AppendChild(root);
            try
            {
                _doc.Save(_path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при создании файла: {path}", ex);
            }
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
                return _doc.SelectSingleNode($"//*[@{Key.name}={Key.value}]"); 
            }
            catch
            {
                throw new Exception($"Узел <c атрибутом{Key.name}={Key.value}> не найден в XML.");
            }          
        }
        
        public XmlNode GetChildNode(XmlNode parent, string child_name)
        {
            return parent.SelectSingleNode(child_name);
        }
        public string GetNodeValue(XmlNode node)
        {
            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Метод преобразует XmlNode в XmlElement, если это возможно.
        /// </summary>
        public XmlElement ConvertNodeToElement(XmlNode node)
        { 
            if (node.NodeType == XmlNodeType.Element)
            {
                return (XmlElement)node;
            }
            else
            {
                throw new Exception($"Узел {node.Name} не является элементом XML.");
            }
        }

        public void AddCDataToNode(XmlElement node, string value)
        {
            XmlCDataSection cdata = _doc.CreateCDataSection(value);
            node.AppendChild(cdata);
        }
        /// <summary>
        /// Метод создает атрибут для элемента, если его нет, иначе устанавливает новое значение.
        /// </summary>
        /// <param name="node">Элемент, которому нужно установить значение, элемент XmlNode можно привести в XmlElement методом ConvertNodeToElement</param>
        /// <param name="attribute">Кортеж из названия атрибута и значения, наприимер.("id", "0")</param>
        public void SetAttributeToElement(XmlElement node,(string name, string value) attribute)
        {
            node.SetAttribute(attribute.name, attribute.value);
        }
        /// <summary>
        /// Метод добавляет узел в XML-файл для узла с указанным атрибутом и значением, если потомок уже существует, то его заменят.
        /// </summary>
        /// <param name="node">Кортеж из названия узла и значения наприимер.("Grade", "5")</param>
        /// <param name="id">Кортеж из названия узла и значения наприимер.("id", "0")</param>
        public void AddUniqueChildToNodeById((string name, string innerText) node, (string name, string value) id)
        {
            XmlNode Parent = _doc.SelectSingleNode($"//*[@{id.name}={id.value}]");
            if (Parent == null)
                throw new Exception($"Узел <c атрибутом{id.name}={id.value}> не найден в XML.");

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

            XmlNode Parent = _doc.SelectSingleNode($"//*[@{id.name}={id.value}]");
            if (Parent == null)
                throw new Exception($"Узел <c атрибутом{id.name}={id.value}> не найден в XML.");

            XmlElement matrix_e = _doc.CreateElement(node.name);
            matrix_e.SetAttribute("rows", rows.ToString());
            matrix_e.SetAttribute("cols", cols.ToString());

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

        public double[,] ConvertNodeToMatrix(XmlNode node)
        {
            int rows = 0;
            int cols = 0;
            double[,] matrix;
            if (node == null)
            {
                return null;
            }
            try 
            {
                rows = int.Parse(node.Attributes["rows"].Value);
            }
            catch(Exception ex)
            {
                throw new Exception($"не найден атрибут rows, возможно файл повреждён. {ex}");
            }
            try
            {
                cols = int.Parse(node.Attributes["cols"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"не найден атрибут cols, возможно файл повреждён. {ex}");
            }

            matrix = new double[rows, cols];

            XmlNodeList row_nodes = node.SelectNodes("row");

            for (int i = 0; i < rows; i++)
            {
                XmlNodeList cell_nodes = row_nodes[i].SelectNodes("cell");

                for (int j = 0; j < cols; j++)
                {
                    try 
                    {
                        double cell_value = double.Parse(cell_nodes[j].InnerText, System.Globalization.NumberStyles.Float,
                                         System.Globalization.CultureInfo.InvariantCulture);
                        matrix[i, j] = cell_value;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"Ошибка в ячейке [{i},{j}]: {ex}");
                    }
                }
            }
            return matrix;
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

        public List<string> GetAllAttributeValues(string[] tag_names, string attribute_name)
        {
            List<XmlNode> _general = new List<XmlNode>();
            List<string> _result = new List<string>();

            foreach (string tag in tag_names)
            {
                XmlNodeList nodes = _doc.GetElementsByTagName(tag);
                foreach (XmlNode node in nodes)
                {
                    _general.Add(node);
                }
            }
            foreach (XmlNode node in _general)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Name == attribute_name)
                    {
                        _result.Add(attribute.Value);
                    }
                }
            }
            return _result;
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
        //--------------------------------------------не универсальные методы-----------------------------------------------------------------------------------------
        private custom_system GetSystemFromXml(XmlNode xmlNode)
        {
            custom_system tree = new custom_system
            {
                Name = GetNodeValue(GetChildNode(xmlNode, "name")),
                Id = xmlNode.Attributes?["id"]?.Value ?? string.Empty,
                enterprise_matrix = ConvertNodeToMatrix(GetChildNode(xmlNode, "enterprise_matrix")),
                technology_matrix = ConvertNodeToMatrix(GetChildNode(xmlNode, "technology_matrix")),
                integration_matrix = ConvertNodeToMatrix(GetChildNode(xmlNode, "integration_matrix")),
                enterprise_grade = Convert.ToInt32(GetNodeValue(GetChildNode(xmlNode, "enterprise_grade"))),
                technology_grade = Convert.ToInt32(GetNodeValue(GetChildNode(xmlNode, "technology_grade"))),
                integration_grade = Convert.ToInt32(GetNodeValue(GetChildNode(xmlNode, "integration_grade")))
            };

            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element && new[] { "system", "subsystem" }.Contains(child.Name))
                {
                    tree.Nodes.Add(GetSystemFromXml(child));
                }
            }

            return tree;
        }
        public custom_system GetSystemFromXml()
        {
            custom_system tree = GetSystemFromXml(_doc.DocumentElement);
            return tree;
        } 
    }
}