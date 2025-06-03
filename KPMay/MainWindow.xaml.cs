using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AD_Sqlite ad_sqlite = new AD_Sqlite();
        AD_APP ad_app = new AD_APP();
        string project_file_path =  AD_General.ConvertEnviromentPatToPath("%TEMP%\\KorabelProFit\\combineTemp.xml");
        string db_path = AD_General.ConvertEnviromentPatToPath("%APPDATA%\\KorabelProFit\\");
        string db_name = "KBPdb";
        public MainWindow()
        {
            ad_sqlite.CreateDBFile(db_path, db_name);
            ad_app.CreateBasicTables(db_path, db_name);
            if (!Directory.Exists(project_file_path)) Directory.CreateDirectory(project_file_path);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Redaction taskWindow = new Redaction();
            taskWindow.Show();
        }

        private void bt_setttings_Click(object sender, RoutedEventArgs e)
        {
            Settiings settiings = new Settiings();
            settiings.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProjectModel model = new ProjectModel(); 
            model.tempPath = project_file_path;
            model.dbPath = db_path;
            model.dbName = db_name;
            AD_XML ad_xml = new AD_XML();
            ad_xml.CreateXML(project_file_path);
            Redaction taskWindow = new Redaction(model);
            taskWindow.Show();
        }
    }
}
