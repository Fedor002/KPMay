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
using System.Xml.Linq;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для Redaction.xaml
    /// </summary>
    /// 

    public partial class Redaction : Window
    {
        AD_XML XML = new AD_XML();
        ObservableCollection<AD_Tree> nodes;
        public Redaction()
        {
            InitializeComponent();

            XML.LoadXML(@"D:\Downloads\Telegram Desktop\test.xml");

            AD_Tree tree = XML.GetTreeFrom_xml();
            nodes = tree.Nodes;
            treeView1.ItemsSource = nodes;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\Downloads\Telegram Desktop\test.xml"); 

            string newNodeName = ElementTextBox.Text;

            XmlElement newNode = xmlDoc.CreateElement("node");
            newNode.SetAttribute("name", newNodeName);

            XmlElement root = xmlDoc.DocumentElement;
            root.AppendChild(newNode);

            xmlDoc.Save(@"D:\Downloads\Telegram Desktop\test.xml"); 

            MessageBox.Show("Новый узел успешно добавлен!");

            ReloadTreeView();

            ElementTextBox.Text = string.Empty;
        }

        private void ReloadTreeView()
        {
            XML.LoadXML(@"D:\Downloads\Telegram Desktop\test.xml");

            AD_Tree tree = XML.GetTreeFrom_xml();
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

        public class SquareMatrix
        {
            private double[,] _matrix;
            public int Size { get; }
            public List<string> RowNames { get; }

            public SquareMatrix(int size, List<string> names)
            {
                Size = size;
                _matrix = new double[size, size];
                RowNames = names;
            }

            public double this[int row, int col]
            {
                get => _matrix[row, col];
                set => _matrix[row, col] = value;
            }

            public double this[string rowName, string colName]
            {
                get
                {
                    int row = RowNames.IndexOf(rowName);
                    int col = RowNames.IndexOf(colName);
                    return _matrix[row, col];
                }
                set
                {
                    int row = RowNames.IndexOf(rowName);
                    int col = RowNames.IndexOf(colName);
                    _matrix[row, col] = value;
                }
            }
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

            // Создаем матрицу с передачей имен
            var matrix = new SquareMatrix(size, rootNames);

            // Заполняем нулями (или другими значениями по умолчанию)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = 0;
                }
            }

            ShowMatrix(matrix);
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

            var matrixWindow = new Window
            {
                Title = $"Матрица связей систем (порядок {matrix.Size})",
                Content = grid,
                Width = 800,
                Height = 600
            };

            matrixWindow.Show();
        }

        // Вспомогательный класс для отображения строк матрицы
        public class MatrixRow
        {
            public string RowName { get; set; }
            public double[] Values { get; set; }
        }
    }
}
