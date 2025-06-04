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
        string project_file_path =  AD_General.ConvertEnviromentPatToPath("%TEMP%\\KorabelProFit\\");
        string db_path = AD_General.ConvertEnviromentPatToPath("%APPDATA%\\KorabelProFit\\");
        string db_name = "KBPdb";
        ProjectModel model = new ProjectModel();
        public MainWindow()
        {
            ad_sqlite.CreateDBFile(db_path, db_name);
            ad_app.CreateBasicTables(db_path, db_name);
            if (!Directory.Exists(project_file_path)) Directory.CreateDirectory(project_file_path);
            model.project_properties.FIO = ad_app.ReadAllUsers(db_path, db_name).FirstOrDefault() is var user && user != default ? user.FIO : string.Empty;
            model.project_properties.enterprise = ad_app.ReadAllEnterprises(db_path, db_name).FirstOrDefault() is var ent && ent != default ? ent.name : string.Empty;
            model.project_properties.job = ad_app.ReadAllJobs(db_path, db_name).FirstOrDefault() is var job && job != default ? job.name : string.Empty;
            model.dbName = db_name;
            model.dbPath = db_path;
            model.tempPath = System.IO.Path.Combine(project_file_path, "combineTemp.xml");
            InitializeComponent();
            this.DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Redaction taskWindow = new Redaction();
            taskWindow.Show();
        }

        private void bt_setttings_Click(object sender, RoutedEventArgs e)
        {
            Settiings settiings = new Settiings(model);
            settiings.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {           
            AD_XML ad_xml = new AD_XML();
            ad_xml.CreateXML(model.tempPath);
            Redaction taskWindow = new Redaction(model);
            taskWindow.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {                     
            model.currentPath = AD_APP.OpenAdFile(model.tempPath);

            if (model.currentPath != null)
            {
                Redaction taskWindow = new Redaction(model);
                taskWindow.Show();
            }

        }
    }
}
