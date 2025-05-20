using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using KPMay.AdditionalClasses;
using KPMay.Math;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для Subsystem.xaml
    /// </summary>
    public partial class Subsystem : Window
    {
        AD_XML XML = new AD_XML();
        public object SelectedItem { get; set; }
        public FillMatrix fillMatrix;
        public string _filePath = @"D:\Downloads\Telegram Desktop\test.xml"; // Путь к вашему XML файлу

        public Subsystem()
        {
            InitializeComponent();
            this.Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Получаем данные из DataContext
            var data = this.DataContext as AD_Tree;

            if (data != null)
            {
                // Привязываем данные к TreeView
                treeView1.ItemsSource = data.Nodes;
            }


            fillMatrix = new FillMatrix(treeView1);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текущий DataContext
            var currentDataContext = this.DataContext as AD_Tree;
            if (currentDataContext == null)
            {
                MessageBox.Show("Не удалось получить текущий контекст данных!");
                return;
            }

            // Получаем имя нового узла из TextBox
            string newNodeName = ElementTextBox.Text;
            if (string.IsNullOrWhiteSpace(newNodeName))
            {
                MessageBox.Show("Введите имя узла!");
                return;
            }

            // Загружаем XML документ
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(_filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить XML файл: {ex.Message}");
                return;
            }

            // Создаем новый узел
            XmlElement newNode = xmlDoc.CreateElement("node");
            newNode.SetAttribute("name", newNodeName);

            // Находим родительский узел, соответствующий текущему DataContext
            XmlNode parentNode = FindParentNodeInXml(xmlDoc, currentDataContext);
            if (parentNode == null)
            {
                MessageBox.Show("Не удалось найти родительский узел в XML!");
                return;
            }

            // Добавляем новый узел
            parentNode.AppendChild(newNode);

            // Сохраняем изменения
            try
            {
                xmlDoc.Save(_filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить XML файл: {ex.Message}");
                return;
            }

            MessageBox.Show("Новый узел успешно добавлен!");
            ReloadTreeView();
            ElementTextBox.Text = string.Empty;
        }

        private XmlNode FindParentNodeInXml(XmlDocument xmlDoc, AD_Tree currentContext)
        {
            // Здесь нужно реализовать логику поиска узла в XML,
            // который соответствует currentContext
            // Это зависит от структуры вашего XML и класса AD_Tree

            // Примерная реализация (вам нужно адаптировать под вашу структуру):
            string xpath = $"//node[@name='{currentContext.Name}']"; // предполагая, что у AD_Tree есть свойство Name
            return xmlDoc.SelectSingleNode(xpath) ?? xmlDoc.DocumentElement;
        }

        private void ReloadTreeView()
        {
            // Получаем текущий контекст
            var currentContext = this.DataContext as AD_Tree;
            if (currentContext == null) return;

            // Загружаем XML
            XML.LoadXML(_filePath);
            AD_Tree fullTree = XML.GetTreeFrom_xml();

            // Находим соответствующий узел в новом дереве
            AD_Tree matchingNode = FindMatchingNode(fullTree, currentContext);
            if (matchingNode == null) return;

            // Обновляем текущий DataContext
            this.DataContext = matchingNode;

            // Обновляем TreeView, сохраняя текущее состояние раскрытия узлов
            var oldItemsSource = treeView1.ItemsSource as ObservableCollection<AD_Tree>;
            if (oldItemsSource != null)
            {
                oldItemsSource.Clear();
                foreach (var node in matchingNode.Nodes)
                {
                    oldItemsSource.Add(node);
                }
            }
            else
            {
                treeView1.ItemsSource = matchingNode.Nodes;
            }
        }
        private AD_Tree FindMatchingNode(AD_Tree searchIn, AD_Tree toFind)
        {
            // Простая проверка на соответствие (возможно, вам нужно добавить больше критериев)
            if (searchIn.Name == toFind.Name)
                return searchIn;

            foreach (var child in searchIn.Nodes)
            {
                var found = FindMatchingNode(child, toFind);
                if (found != null)
                    return found;
            }

            return null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Redaction redaction = new Redaction();
            this.Close();
            redaction.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            fillMatrix.CreateSquareMatrix(sender, e);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MathMatrix calc = new MathMatrix(fillMatrix.GetResultMatr(), fillMatrix.GetResultVect());

            double N = calc.CalcTechLvl();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_filePath);
            var neededName = DataContext as AD_Tree;

            // Найти нужный узел по имени
            XmlNode targetNode = xmlDoc.SelectSingleNode($"//node[@name='{neededName.Name}']");
            if (targetNode != null)
            {
                XmlAttribute gradeAttr = xmlDoc.CreateAttribute("grade");
                gradeAttr.Value = Convert.ToString(N);
                targetNode.Attributes.Append(gradeAttr);

                xmlDoc.Save(_filePath);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var selectedOBj = treeView1.SelectedItem as AD_Tree;

            Subsystem subsystem = new Subsystem();
            subsystem.DataContext = selectedOBj;
            this.Close();
            subsystem.Show();
        }
    }
}
