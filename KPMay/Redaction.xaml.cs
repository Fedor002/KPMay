using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using _io = System.IO;
using System.Xml.Linq;
using KPMay.MathHolder;
using KPMay;
using Excel = Microsoft.Office.Interop.Excel;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Xceed;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для Redaction.xaml
    /// </summary>
    /// 

    public partial class Redaction : Window
    {
        private void ExpandAll(ItemsControl parent)
        {
            foreach (var item in parent.Items)
            {
                if (parent.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi)
                {
                    tvi.IsExpanded = true;
                    ExpandAll(tvi); // рекурсивно
                }
            }
        }

        string _system_xml_path = _io.Path.Combine(AppContext.BaseDirectory, "test.xml");
        string _system_report_path = _io.Path.Combine(AppContext.BaseDirectory, "test.docx");
        string _system_integration_level_path = _io.Path.Combine(AppContext.BaseDirectory, "integration_readiness_level.jpg");
        string _system_technology_level_path = _io.Path.Combine(AppContext.BaseDirectory, "technology_readiness_level.jpg");
        string _system_production_level_path = _io.Path.Combine(AppContext.BaseDirectory, "production_readiness_level.jpg");
        custom_system selectedNodeC;

        ProjectModel model;
        AD_XML XML = new AD_XML();
        ObservableCollection<custom_system> nodes;
        private Dictionary<string, double> _vectorValues;
        private SquareMatrix MatrixContext;
        private CustomTags ct = new CustomTags();
        public Redaction(ProjectModel model)
        {
            InitializeComponent();

            this.model = model;
            this.DataContext = model;
            XML.LoadXML(model.tempPath);

            //custom_system tree = XML.GetSystemFromXml();
            //nodes = tree.Nodes;
            List<string> ids = XML.GetAllAttributeValues(new[] { ct.system, ct.subsystem }, ct.id);
            // treeView1.ItemsSource = nodes;
            this.Loaded += (s, e) => ExpandAll(treeView1);
        }

        string filePath = _io.Path.Combine(AppContext.BaseDirectory, "InstactionsForFillingMatrix.docx");

        public Redaction()
        {
            InitializeComponent();

            XML.LoadXML(_system_xml_path);

            custom_system tree = XML.GetSystemFromXml();
            nodes = tree.Nodes;
            List<string> ids = XML.GetAllAttributeValues(new[] { ct.system, ct.subsystem },ct.id);
            treeView1.ItemsSource = nodes;
            foreach (var item in treeView1.Items)
            {
                var tvi = treeView1.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (tvi != null)
                    tvi.IsExpanded = true; // или true, если нужно развернуть
            }
        }

        // Вспомогательный класс для элемента вектора
        public class VectorItem
        {
            public string SystemName { get; set; }
            public double Value { get; set; }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CalcTechnoE(object sender, RoutedEventArgs e)
        {
            MathMatrix calc = new MathMatrix(MatrixContext, _vectorValues);

            double N = calc.MakeTheFunny();

            custom_system selectedNode = selectedNodeC;

            int i = 0;

            List<double> vector = new List<double>();


            foreach (var element in _vectorValues)
            {
                vector.Add(element.Value);

            }

            foreach (custom_system el in selectedNode.Nodes)
            {
                XML.AddUniqueChildToNodeById((ct.enterprise_grade, vector[i].ToString()), (ct.id, el.Id));
                XML.SaveXML();
                i++;
            }


            XML.AddUniqueChildToNodeById((ct.enterprise_grade, N.ToString()), (ct.id, selectedNode.Id));
            XML.SaveXML();
            ReloadTreeView();
        }

        private void CalcTechnoT(object sender, RoutedEventArgs e)
        {
            MathMatrix calc = new MathMatrix(MatrixContext, _vectorValues);

            double N = calc.MakeTheFunny();

            custom_system selectedNode = selectedNodeC;

            int i = 0;

            List<double> vector = new List<double>();


            foreach (var element in _vectorValues)
            {
                vector.Add(element.Value);

            }

            foreach (custom_system el in selectedNode.Nodes)
            {
                XML.AddUniqueChildToNodeById((ct.technology_grade, vector[i].ToString()), (ct.id, el.Id));
                XML.SaveXML();
                i++;
            }

            XML.AddUniqueChildToNodeById((ct.technology_grade, N.ToString()), (ct.id, selectedNode.Id));
            XML.SaveXML();
            ReloadTreeView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string newNodeName = ElementTextBox.Text;
            List<string> ids = XML.GetAllAttributeValues(new[] { ct.system, ct.subsystem }, ct.id);
            string newId = Guid.NewGuid().ToString();
            while (ids.Contains(newId))
            {
                newId = Guid.NewGuid().ToString();
            }
            if (selectedNodeC != null)
            {
                custom_system selectedNode = selectedNodeC;
                XmlElement newNode = XML.doc.CreateElement(ct.subsystem);
                XmlNode name = XML.doc.CreateElement(ct.name);
                name.InnerText = newNodeName;

                XML.SetAttributeToElement(newNode, (ct.id, newId));

                newNode.AppendChild(name);

                XmlNode lvl = XML.doc.CreateElement(ct.lvl);
                lvl.InnerText = ((int)selectedNode.lvl + 1).ToString();
                newNode.AppendChild(lvl);

                XmlNode critT = XML.doc.CreateElement(ct.critical_technology);
                critT.InnerText = model.systems.criticalTechnology;
                newNode.AppendChild(critT);

                XML.GetNodeByKey((ct.id, selectedNode.Id)).AppendChild(newNode);
                XML.SaveXML();
                MessageBox.Show("Новый узел успешно добавлен!");
            }
            else if(!XML.TagExist(ct.system))
            {
                XmlElement newNode = XML.doc.CreateElement(ct.system);
                XmlNode name = XML.doc.CreateElement(ct.name);
                name.InnerText = newNodeName;
                XML.SetAttributeToElement(newNode, (ct.id, "0"));
                newNode.AppendChild(name);
                XML.doc.DocumentElement.AppendChild(newNode);

                XmlNode lvl = XML.doc.CreateElement(ct.lvl);
                lvl.InnerText = "1";
                newNode.AppendChild(lvl);

                XmlNode critT = XML.doc.CreateElement(ct.critical_technology);
                critT.InnerText = "critical_technology";
                newNode.AppendChild(critT);

                XML.SaveXML();
                MessageBox.Show("Новый узел успешно добавлен!");
            }
            ReloadTreeView();
            selectedNodeC = (custom_system)treeView1.Items[0];
            ElementTextBox.Text = string.Empty;
        }

        private void ReloadTreeView()
        {
            model.systems = XML.GetSystemFromXml();
            ExpandAll(treeView1);
        }

        private void Grid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

            PopUp taskWindow = new PopUp();
            taskWindow.Show();

        }
        
        /*private void Grid_MouseLeftButtonDownIn1(object sender, RoutedEventArgs e)
        {
            

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Важно для открытия через ассоциированное приложение
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
            }

        }

        private void Grid_MouseLeftButtonDownIn2(object sender, RoutedEventArgs e)
        {

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Важно для открытия через ассоциированное приложение
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
            }

        }

        private void Grid_MouseLeftButtonDownIn3(object sender, RoutedEventArgs e)
        {

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Важно для открытия через ассоциированное приложение
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}");
            }

        }*/

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        int GetRootNodesCount(TreeView treeView)
        {
            return treeView.Items.Count;
        }


        private void CreateSquareMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем названия корневых узлов
            var rootNames = new List<string>();
            custom_system selectedNode = selectedNodeC;
            foreach (custom_system node in selectedNode.Nodes)
            {
                rootNames.Add(node.Name);
            }

            int size = rootNames.Count;

            if (size == 0)
            {
                MessageBox.Show("Нет корневых узлов в дереве!");
                return;
            }
            
            MatrixContext = new SquareMatrix(size, rootNames);

                string senderName = (sender as FrameworkElement)?.Name;
                if (senderName == "enterprise" && selectedNode.enterprise_matrix != null)
                    MatrixContext._matrix = selectedNode.enterprise_matrix;
                else if (senderName == "technology" && selectedNode.technology_matrix != null)
                    MatrixContext._matrix = selectedNode.technology_matrix;

 /*           else
            {
                double[,] matrixEmpty = new double[size, size]
                {
                    { 1, 2 },
                    { 3, 4 }
                };
                MatrixContext._matrix = matrixEmpty;
            }
 */

            // Заполняем нулями (или другими значениями по умолчанию)
            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        MatrixContext[i, j] = 0;
            //    }
            //}

            ShowMatrix(MatrixContext, sender);
        }

        private void ShowMatrix(SquareMatrix matrix, object sender)
        {
            // Получаем названия корневых узлов
            var rootNames = new List<string>();
            custom_system selectedNode = selectedNodeC;
            foreach (custom_system node in selectedNode.Nodes) 
            { 
                rootNames.Add(node.Name); // Используем свойство Name из AD_Tree
            }

            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                HeadersVisibility = DataGridHeadersVisibility.All
            };

            // Добавляем колонку с названиями строк
            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Системы",
                Binding = new Binding("RowName"),
                IsReadOnly = true
            });

            // Добавляем колонки с названиями корневых узлов
            for (int j = 0; j < matrix.Size; j++)
            {
                grid.Columns.Add(new DataGridTextColumn
                {
                    Header = rootNames[j],
                    Binding = new Binding($"Values[{j}]")
                });
            }

            // Создаем данные для привязки
            var items = new List<MatrixRow>();
            for (int i = 0; i < matrix.Size; i++)
            {
                var values = new double[matrix.Size];
                for (int j = 0; j < matrix.Size; j++)
                {
                    values[j] = matrix[i, j];
                }

                items.Add(new MatrixRow
                {
                    RowName = rootNames[i], // Название строки
                    Values = values
                });
            }

            grid.ItemsSource = items;
            grid.CellEditEnding += (s, e) =>
            {
                if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() != "Системы")
                {
                    int rowIndex = e.Row.GetIndex();
                    int columnIndex = e.Column.DisplayIndex - 1; // -1 потому что первая колонка - названия

                    var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);
                    matrix[rowIndex, columnIndex] = newValue;
                }
            };

            var button = new Button
            {
                Content = "Перейти к заполнению вектора",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(grid);
            stackPanel.Children.Add(button);

            var matrixWindow = new Window
            {
                Title = $"Матрица связей систем (порядок {matrix.Size})",
                Content = stackPanel,
                Width = 800,
                Height = 650
            };

            button.Click += (s, e) =>
            {
                string senderName = (sender as FrameworkElement)?.Name;
                if (senderName == "enterprise")
                    XML.AddMatrixToNode((ct.enterprise_matrix, matrix._matrix), (ct.id, selectedNode.Id));
                else if (senderName == "technology")
                    XML.AddMatrixToNode((ct.technology_matrix, matrix._matrix), (ct.id, selectedNode.Id));

                XML.SaveXML();
                matrixWindow.Close();
                ShowVectorInput(matrix, sender);
            };

            matrixWindow.Show();
        }

        private void ShowVectorInput(SquareMatrix matrix, object sender)
        {
            string senderName = (sender as FrameworkElement)?.Name;
            var rootNames = new List<string>();
            custom_system selectedNode = selectedNodeC;
            foreach (custom_system node in selectedNode.Nodes)
            {
                rootNames.Add(node.Name);
            }

            // Инициализация вектора (значения по умолчанию 0)
            _vectorValues = new Dictionary<string, double>();
            foreach (custom_system node in selectedNode.Nodes)
            {
                double value = 0.0;
                if (!string.IsNullOrEmpty(Convert.ToString(node.enterprise_grade)) && senderName == "enterprise")
                    value = node.enterprise_grade;
                else if (!string.IsNullOrEmpty(Convert.ToString(node.technology_grade)) && senderName == "technology")
                    value = node.technology_grade;
                _vectorValues[node.Name] = value;
            }

            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                ItemsSource = _vectorValues.Select(v => new VectorItem
                {
                    SystemName = v.Key,
                    Value = v.Value
                }).ToList(),
                Margin = new Thickness(10)
            };

            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Система",
                Binding = new Binding("SystemName"),
                IsReadOnly = true
            });

            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Значение",
                Binding = new Binding("Value")
            });

            grid.CellEditEnding += (s, e) =>
            {
                if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Значение")
                {
                    var editedItem = (VectorItem)e.Row.Item;
                    var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);

                    _vectorValues[editedItem.SystemName] = newValue;
                }
            };

            var vectorWindow = new Window
            {
                Title = "Вектор значений систем",
                Content = grid,
                Width = 400,
                Height = 500
            };

            // Выбор метода по Name у sender

            if (senderName == "enterprise")
                vectorWindow.Closed += (s, e) => CalcTechnoE(null, null);
            else if (senderName == "technology")
                vectorWindow.Closed += (s, e) => CalcTechnoT(null, null);

            vectorWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Создание отчёта
            custom_system tree = model.systems;

            using (DocX document = DocX.Create(_system_report_path))
            {
                // Добавляем заголовок
                document.InsertParagraph("Отчёт").FontSize(18).Bold().Alignment = Alignment.center;

                document.InsertParagraph("");
                document.InsertParagraph("Данные о классе").FontSize(16).Italic();

                // Создаём таблицу
                Xceed.Document.NET.Table class_table = document.AddTable(2, 2); // +1 для заголовка
                class_table.Design = TableDesign.TableGrid;

                // Заголовки
                class_table.Rows[0].Cells[0].Paragraphs.First().Append("Класс ВВСТ").Bold();
                class_table.Rows[1].Cells[0].Paragraphs.First().Append("Наименование перспективного образца ВВСИ").Bold();

                class_table.Rows[0].Cells[1].Paragraphs.First().Append(model.project_properties.VVST_class);
                class_table.Rows[1].Cells[1].Paragraphs.First().Append(model.project_properties.VVST_name);

                document.InsertTable(class_table);

                document.InsertParagraph("");
                document.InsertParagraph("Структурная функционально-техническая схема система").FontSize(16).Italic();

                int need_level;

                if (_ChooseLevel.SelectedItem != null)
                {
                    string[] parts = _ChooseLevel.SelectedItem.ToString().Split(' ');
                    need_level = int.Parse(parts[2]);
                    need_level += 1;
                }
                else
                {
                    need_level = 2;
                }

                var (count_by_level, value_by_level) = GetNodesByLevel(model.systems.Nodes, need_level);
                var nodesWithParents = GetNodesWithParentsByLevel(model.systems.Nodes, targetLevel: need_level, currentParent: null);
                var nodesWithParents_IT = GetNodesWithParentsByLevel_It(model.systems.Nodes, targetLevel: need_level, currentParent: null);

                // Создаём таблицу
                Xceed.Document.NET.Table big_assessment = document.AddTable(2+count_by_level, 3); // +1 для заголовка
                big_assessment.Design = TableDesign.TableGrid;

                big_assessment.Rows[0].Cells[0].Paragraphs.First().Append("Наименование типового образца ВВТ").Bold();
                big_assessment.Rows[0].Cells[1].Paragraphs.First().Append("Наименование составной части").Bold();
                big_assessment.Rows[1].Cells[0].Paragraphs.First().Append(tree.Nodes[0].Name);
                big_assessment.Rows[0].Cells[1].Width = big_assessment.Rows[1].Cells[1].Width;
                big_assessment.Rows[1].Cells[1].Paragraphs.First().Append((need_level - 1).ToString() + "-й уровень");
                big_assessment.Rows[1].Cells[2].Paragraphs.First().Append((need_level).ToString() + "-й уровень");

                big_assessment.MergeCellsInColumn(0, 0, 1);
                big_assessment.MergeCellsInColumn(0, 1, big_assessment.RowCount-1);
                big_assessment.Rows[0].MergeCells(1, 2);

                for (int i = 0; i < count_by_level; i++)
                {
                    big_assessment.Rows[2 + i].Cells[1].Paragraphs.First().Append(nodesWithParents[i].ParentValue);

                    big_assessment.Rows[2 + i].Cells[2].Paragraphs.First().Append(nodesWithParents[i].NodeValue);
                }

                big_assessment.MergeCellsInColumn(0, 0, 1);
                big_assessment.MergeCellsInColumn(0, 1, big_assessment.RowCount - 1);
                big_assessment.Rows[0].MergeCells(1, 2);

                document.InsertTable(big_assessment);
                document.InsertParagraph("");

                Xceed.Document.NET.Table final_assessment = document.AddTable(2+count_by_level, 7); // +1 для заголовка
                final_assessment.Design = TableDesign.TableGrid;

                document.InsertParagraph("Комплексная оценка проектных и производственных уровней готовности").FontSize(16).Italic();

                // Заголовки
                final_assessment.Rows[0].Cells[0].Paragraphs.First().Append("Наименование составной части образца ВВСТ").Bold();
                final_assessment.Rows[1].Cells[0].Paragraphs.First().Append((need_level - 1).ToString() + "-й уровень").Bold();
                final_assessment.Rows[1].Cells[1].Paragraphs.First().Append("Расчёт УГПТ").Bold();
                final_assessment.Rows[1].Cells[2].Paragraphs.First().Append((need_level).ToString() + "-й уровень").Bold();
                final_assessment.Rows[1].Cells[3].Paragraphs.First().Append("Расчёт УГН").Bold();
                final_assessment.Rows[0].Cells[4].Paragraphs.First().Append("Критические технологии").Bold();
                final_assessment.Rows[0].Cells[5].Paragraphs.First().Append("Оценка УГТ").Bold();
                final_assessment.Rows[0].Cells[6].Paragraphs.First().Append("Оценка УГП").Bold();

                for (int i = 0; i < count_by_level; i++)
                {
                    final_assessment.Rows[2 + i].Cells[0].Paragraphs.First().Append(nodesWithParents_IT[i].Parent.Name);
                    final_assessment.Rows[2 + i].Cells[1].Paragraphs.First().Append(nodesWithParents_IT[i].Parent.enterprise_grade.ToString());
                    final_assessment.Rows[2 + i].Cells[2].Paragraphs.First().Append(nodesWithParents_IT[i].Node.Name);
                    final_assessment.Rows[2 + i].Cells[3].Paragraphs.First().Append(nodesWithParents_IT[i].Node.enterprise_grade.ToString());
                    final_assessment.Rows[2 + i].Cells[4].Paragraphs.First().Append(nodesWithParents_IT[i].Node.criticalTechnology);
                    final_assessment.Rows[2 + i].Cells[5].Paragraphs.First().Append(nodesWithParents_IT[i].Node.technology_grade.ToString());
                    final_assessment.Rows[2 + i].Cells[6].Paragraphs.First().Append(nodesWithParents_IT[i].Node.enterprise_grade.ToString());
                }

                final_assessment.MergeCellsInColumn(4, 0, 1);
                final_assessment.MergeCellsInColumn(5, 0, 1);
                final_assessment.MergeCellsInColumn(6, 0, 1);
                final_assessment.MergeCellsInColumn(0, 2, (2+count_by_level-1));
                final_assessment.MergeCellsInColumn(1, 2, (2 + count_by_level - 1));
                final_assessment.Rows[0].MergeCells(0, 3);


                document.InsertTable(final_assessment);
                document.InsertParagraph("");

                document.InsertParagraph("Данные об эксперте").FontSize(16).Italic();

                // Создаём таблицу
                Xceed.Document.NET.Table user_info = document.AddTable(2, 4); // +1 для заголовка
                user_info.Design = TableDesign.TableGrid;

                // Заголовки
                user_info.Rows[0].Cells[0].Paragraphs.First().Append("ФИО Эксперта").Bold();
                user_info.Rows[0].Cells[1].Paragraphs.First().Append("Должность").Bold();
                user_info.Rows[0].Cells[2].Paragraphs.First().Append("Организация").Bold();
                user_info.Rows[0].Cells[3].Paragraphs.First().Append("Подпись").Bold();

                user_info.Rows[1].Cells[0].Paragraphs.First().Append(model.project_properties.FIO);
                user_info.Rows[1].Cells[1].Paragraphs.First().Append(model.project_properties.job);
                user_info.Rows[1].Cells[2].Paragraphs.First().Append(model.project_properties.enterprise);
                user_info.Rows[1].Cells[3].Paragraphs.First().Append("");

                document.InsertTable(user_info);
                document.Save();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            custom_system selectedNode = selectedNodeC;

            XmlNode node = XML.doc.SelectSingleNode($"//*[@id='{selectedNode.Id}']");
            if (node != null && node.ParentNode != null)
            {
                node.ParentNode.RemoveChild(node);
                XML.SaveXML();
                ReloadTreeView();
            }
            else
            {
                throw new Exception($"Узел с id={selectedNode.Id} не найден или не имеет родителя.");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // 1. Собираем все системы и подсистемы
            custom_system root = XML.GetSystemFromXml();
            var systems = new List<custom_system>();
            void CollectSystems(custom_system node)
            {
                systems.Add(node);
                foreach (var child in node.Nodes)
                    CollectSystems(child);
            }
            CollectSystems(root);

            // 2. Запускаем Excel и создаём новую книгу
            var excelApp = new Excel.Application();
            excelApp.Visible = false;
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Excel.Worksheet)workbook.Worksheets[1];

            // 3. Заполняем заголовки
            worksheet.Cells[1, 1] = "Название";
            worksheet.Cells[1, 2] = "Оценка (technology)";
            worksheet.Cells[1, 3] = "Оценка (enterprise)";
            worksheet.Cells[1, 4] = "Уровень (lvl)";

            // 4. Заполняем строки
            for (int i = 0; i < systems.Count; i++)
            {
                worksheet.Cells[i + 2, 1] = systems[i].Name;
                worksheet.Cells[i + 2, 2] = systems[i].technology_grade.ToString();
                worksheet.Cells[i + 2, 3] = systems[i].enterprise_grade.ToString();
                worksheet.Cells[i + 2, 3] = systems[i].lvl.ToString();
            }

            // 5. Сохраняем файл
            string excelPath = _io.Path.Combine(AppContext.BaseDirectory, "export_from_xml.xlsx");
            workbook.SaveAs(excelPath);
            workbook.Close();
            excelApp.Quit();

            // 6. Освобождаем ресурсы
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            MessageBox.Show("Данные успешно экспортированы в Excel!");
        }

        private void mi_save_file_Click(object sender, RoutedEventArgs e)
        {
            
            AD_APP.SaveAdFile(model.currentPath, model.tempPath);
        }

        private void ShowImageModal(string imagePath, string title)
        {
            // Создаем новое модальное окно
            Window imageWindow = new Window
            {
                Title = title,
                Width = 1200,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this, // Устанавливаем владельца (главное окно)
                ResizeMode = ResizeMode.NoResize // Запрещаем изменение размера
            };

            // Создаем элемент Image
            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Stretch = System.Windows.Media.Stretch.Uniform
            };

            try
            {
                // Загружаем изображение из файла
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                image.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Добавляем Image в окно
            imageWindow.Content = image;

            // Показываем окно как модальное
            imageWindow.ShowDialog();
        }

        private void Grid_MouseLeftButtonDownIn1(object sender, MouseButtonEventArgs e)
        {
            ShowImageModal(_system_technology_level_path, "Уровни готовности технологий");
        }

        private void Grid_MouseLeftButtonDownIn2(object sender, MouseButtonEventArgs e)
        {
            ShowImageModal(_system_production_level_path, "Уровни готовности производства");
        }

        private void Grid_MouseLeftButtonDownIn3(object sender, MouseButtonEventArgs e)
        {
            ShowImageModal(_system_integration_level_path, "Уровни готовности интаграций");
        }

        public int CountLeafNodes(ObservableCollection<custom_system> rootNodes)
        {
            int count = 0;

            foreach (var node in rootNodes)
            {
                if (node.Nodes == null || node.Nodes.Count == 0)
                {
                    // Это конечный элемент (лист)
                    count++;
                }
                else
                {
                    // Рекурсивно считаем листья в дочерних узлах
                    count += CountLeafNodes(node.Nodes);
                }
            }

            return count;
        }

        public List<string> GetLeafEndValues(ObservableCollection<custom_system> rootNodes)
        {
            var leafValues = new List<string>();

            foreach (var node in rootNodes)
            {
                if (node.Nodes == null || node.Nodes.Count == 0)
                {
                    // Это лист, добавляем его значение в список
                    leafValues.Add(node.Name); // Предполагаем, что значение хранится в свойстве Value
                }
                else
                {
                    // Рекурсивно собираем листья из дочерних узлов
                    leafValues.AddRange(GetLeafEndValues(node.Nodes));
                }
            }

            return leafValues;
        }

        public (int Count, List<string> Values) GetNodesByLevel(ObservableCollection<custom_system> nodes, int targetLevel)
        {
            var matchingValues = new List<string>();
            int count = 0;

            foreach (var node in nodes)
            {
                // Если уровень текущего узла совпадает с искомым
                if (node.lvl == targetLevel)
                {
                    matchingValues.Add(node.Name);
                    count++;
                }

                // Рекурсивно проверяем дочерние узлы (если они есть)
                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    var (childCount, childValues) = GetNodesByLevel(node.Nodes, targetLevel);
                    count += childCount;
                    matchingValues.AddRange(childValues);
                }
            }

            return (count, matchingValues);
        }

        public List<(string ParentValue, string NodeValue)> GetNodesWithParentsByLevel(ObservableCollection<custom_system> nodes, int targetLevel, custom_system currentParent = null)
        {
            var result = new List<(string, string)>();

            foreach (var node in nodes)
            {
                if (node.lvl == targetLevel)
                {
                    string parentValue = currentParent != null ? currentParent.Name : "Нет родителя";
                    result.Add((parentValue, node.Name));
                }

                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    // Рекурсивно передаём текущий узел как родитель для детей
                    result.AddRange(GetNodesWithParentsByLevel(node.Nodes, targetLevel, node));
                }
            }

            return result;
        }

        public List<(custom_system Parent, custom_system Node)> GetNodesWithParentsByLevel_It(
    ObservableCollection<custom_system> nodes,
    int targetLevel,
    custom_system currentParent = null)
        {
            var result = new List<(custom_system, custom_system)>();

            foreach (var node in nodes)
            {
                if (node.lvl == targetLevel)
                {
                    result.Add((currentParent, node)); // Добавляем объекты, а не строки
                }

                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    // Рекурсивно передаём текущий узел как родитель для детей
                    result.AddRange(GetNodesWithParentsByLevel_It(node.Nodes, targetLevel, node));
                }
            }

            return result;
        }

        private void mi_open_file_Click(object sender, RoutedEventArgs e)
        {
            model.currentPath = AD_APP.OpenAdFile(model.tempPath);
            XML.LoadXML(model.tempPath);
            nodes = XML.GetSystemFromXml().Nodes;
            ReloadTreeView();
        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedNodeC = (custom_system)treeView1.SelectedItem;
        }
    }

    // Вспомогательный класс для отображения строк матрицы
    public class MatrixRow
        {
            public string RowName { get; set; }
            public double[] Values { get; set; }
        }
    
}
