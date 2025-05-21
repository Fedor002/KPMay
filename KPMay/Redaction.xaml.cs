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
using KPMay.Math;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для Redaction.xaml
    /// </summary>
    /// 

    public partial class Redaction : Window
    {
        string _system_xml_path = _io.Path.Combine(AppContext.BaseDirectory, "test.xml");
        AD_XML XML = new AD_XML();
        ObservableCollection<custom_system> nodes;
        private Dictionary<string, double> _vectorValues;
        private SquareMatrix MatrixContext;
        public Redaction()
        {
            InitializeComponent();

            XML.LoadXML(_system_xml_path);

            custom_system tree = XML.GetSystemFromXml();
            nodes = tree.Nodes;
            treeView1.ItemsSource = nodes;
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

        private void CalcTechno(object sender, RoutedEventArgs e)
        {
            MathMatrix calc = new MathMatrix(MatrixContext, _vectorValues);

            double N = calc.MakeTheFunny();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string newNodeName = ElementTextBox.Text;
            if (treeView1.SelectedItem != null)
            {
                custom_system selectedNode = (custom_system)treeView1.SelectedItem;
                XmlElement newNode = XML.doc.CreateElement("system");
                newNode.SetAttribute("name", newNodeName);
                XML.GetNodeByKey(("id", selectedNode.Id)).AppendChild(newNode);
                XML.SaveXML();
                MessageBox.Show("Новый узел успешно добавлен!");
            }
            else if(!XML.TagExist("system"))
            {
                XmlElement newNode = XML.doc.CreateElement("system");
                newNode.SetAttribute("name", newNodeName);
                XML.doc.DocumentElement.AppendChild(newNode);
                XML.SaveXML();
                MessageBox.Show("Новый узел успешно добавлен!");
            }
            else
            {
                XmlElement newNode = XML.doc.CreateElement("subsystem");
                newNode.SetAttribute("name", newNodeName);
                XML.doc.DocumentElement.SelectSingleNode("system").AppendChild(newNode);
                XML.SaveXML();
                MessageBox.Show("Новый узел успешно добавлен!");
            }
            ReloadTreeView();
            ElementTextBox.Text = string.Empty;
        }

        private void ReloadTreeView()
        {
            custom_system tree = XML.GetSystemFromXml();
            nodes = tree.Nodes;

            treeView1.ItemsSource = null; 
            treeView1.ItemsSource = nodes; 
        }

        private void Grid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

            PopUp taskWindow = new PopUp();
            taskWindow.Show();

        }

        private void Grid_MouseLeftButtonDownIn1(object sender, RoutedEventArgs e)
        {

            string filePath = @"D:\VSWPF\KPMay\Instactions\test.docx"; // Укажите путь к файлу

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

            string filePath = @"D:\VSWPF\KPMay\Instactions\test.docx"; // Укажите путь к файлу

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

            string filePath = @"D:\VSWPF\KPMay\Instactions\test.docx"; // Укажите путь к файлу

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedOBj = treeView1.SelectedItem as AD_Tree;

            Subsystem subsystem = new Subsystem();
            subsystem.DataContext = selectedOBj;
            this.Close();
            subsystem.Show();

        }

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
            foreach (AD_Tree node in treeView1.Items)
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


            // Заполняем нулями (или другими значениями по умолчанию)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    MatrixContext[i, j] = 0;
                }
            }

            ShowMatrix(MatrixContext);
        }

        private void ShowMatrix(SquareMatrix matrix)
        {
            // Получаем названия корневых узлов
            var rootNames = new List<string>();
            foreach (AD_Tree node in treeView1.Items)
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
                matrixWindow.Close();
                ShowVectorInput(matrix);
            };

            matrixWindow.Show();
        }

        private void ShowVectorInput(SquareMatrix matrix)
        {
            var rootNames = new List<string>();
            foreach (AD_Tree node in treeView1.Items)
            {
                rootNames.Add(node.Name);
            }

            // Инициализация вектора (значения по умолчанию 0)
            _vectorValues = rootNames.ToDictionary(name => name, _ => 0.0);

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

            // Обновляем вектор при изменении ячейки
            grid.CellEditEnding += (sender, e) =>
            {
                if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Значение")
                {
                    var editedItem = (VectorItem)e.Row.Item;
                    var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);

                    _vectorValues[editedItem.SystemName] = newValue; // Обновляем значение в словаре
                }
            };

            var vectorWindow = new Window
            {
                Title = "Вектор значений систем",
                Content = grid,
                Width = 400,
                Height = 500
            };

            vectorWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double[,] matrix = new double[2, 2]
            {
                { 1, 2 },
                { 3, 4 }
            };

            double[,] matrix2 = new double[2, 3]
            {
                { 11, 21, 14 },
                { 35, 14, 15 }
            };
            XML.AddMatrixToNode(("enterprise_matrix", matrix),("id", "0"));
            XML.AddMatrixToNode(("integration_matrix", matrix2), ("id", "0"));
            XML.AddUniqueChildToNodeById(("enterprise_grade", "5"), ("id", "0"));
            XML.AddUniqueChildToNodeById(("technology_grade", "6"), ("id", "0"));
            XML.SaveXML();
        }
    }

    // Вспомогательный класс для отображения строк матрицы
    public class MatrixRow
        {
            public string RowName { get; set; }
            public double[] Values { get; set; }
        }
    
}
