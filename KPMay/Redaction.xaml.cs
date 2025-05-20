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
using KPMay.Math;
using KPMay.AdditionalClasses;

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
        private Dictionary<string, double> _vectorValues;
        private SquareMatrix MatrixContext;
        public string _filePath = @"D:\Downloads\Telegram Desktop\test.xml"; // Путь к вашему XML файлу
        public FillMatrix fillMatrix;
        public Redaction()
        {
            InitializeComponent();

            XML.LoadXML(_filePath);

            AD_Tree tree = XML.GetTreeFrom_xml();
            nodes = tree.Nodes;
            treeView1.ItemsSource = nodes;
            fillMatrix = new FillMatrix(tree);
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
            MathMatrix calc = new MathMatrix(fillMatrix.GetResultMatr(), fillMatrix.GetResultVect());

            double N = calc.CalcTechLvl();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_filePath);

            // Найти нужный узел по имени
            XmlNode targetNode = xmlDoc.SelectSingleNode($"//node[@name='{treeView1.Name}']");
            if (targetNode != null)
            {
                XmlAttribute gradeAttr = xmlDoc.CreateAttribute("grade");
                gradeAttr.Value = Convert.ToString(N);
                targetNode.Attributes.Append(gradeAttr);

                xmlDoc.Save(_filePath);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_filePath); 

            string newNodeName = ElementTextBox.Text;

            XmlElement newNode = xmlDoc.CreateElement("node");
            newNode.SetAttribute("name", newNodeName);

            XmlElement root = xmlDoc.DocumentElement;
            root.AppendChild(newNode);

            xmlDoc.Save(_filePath); 

            MessageBox.Show("Новый узел успешно добавлен!");

            ReloadTreeView();

            ElementTextBox.Text = string.Empty;
        }

        private void ReloadTreeView()
        {
            XML.LoadXML(_filePath);

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

        private void CreateSquareMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            fillMatrix.CreateSquareMatrix(sender, e);
        }
    }
}
