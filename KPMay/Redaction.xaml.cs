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
    }
}
