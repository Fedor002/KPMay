﻿using System;
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
using System.Xml;
using System.Xml.Linq;

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

        private void bt_setttings_Click(object sender, RoutedEventArgs e)
        {
            Settiings settiings = new Settiings(model);
            settiings.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProjectModel new_model = new ProjectModel();
            new_model.project_properties = model.project_properties;
            new_model.dbName = model.dbName;
            new_model.dbPath = model.dbPath;
            new_model.tempPath = model.tempPath;
            AD_XML ad_xml = new AD_XML();
            model = new_model;
            ad_xml.CreateXML(model.tempPath);

            XmlElement newNode = ad_xml.doc.CreateElement("project_properties");

            XmlNode FIO = ad_xml.doc.CreateElement("FIO");
            FIO.InnerText = model.project_properties.FIO;

            XmlNode enterprise = ad_xml.doc.CreateElement("enterprise");
            enterprise.InnerText = model.project_properties.enterprise;

            XmlNode VVST_name = ad_xml.doc.CreateElement("VVST_name");
            VVST_name.InnerText = model.project_properties.VVST_name;

            XmlNode job = ad_xml.doc.CreateElement("job");
            job.InnerText = model.project_properties.job;

            XmlNode VVST_class = ad_xml.doc.CreateElement("VVST_class");
            VVST_class.InnerText = model.project_properties.VVST_class;

            newNode.AppendChild(FIO);
            newNode.AppendChild(job);
            newNode.AppendChild(enterprise);
            newNode.AppendChild(VVST_class);
            newNode.AppendChild(VVST_name);

            ad_xml.doc.DocumentElement.AppendChild(newNode);

            ad_xml.SaveXML();

            Redaction taskWindow = new Redaction(model);
            taskWindow.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AD_XML xml = new AD_XML();
            model.currentPath = AD_APP.OpenAdFile(model.tempPath);
            if (model.currentPath != null)
            {
                xml.LoadXML(model.currentPath);
                model.project_properties.VVST_name = xml.GetTagValue("VVST_name", "project_properties");
                model.project_properties.VVST_class = xml.GetTagValue("VVST_class", "project_properties");
                model.project_properties.enterprise = xml.GetTagValue("enterprise", "project_properties");
                model.project_properties.FIO = xml.GetTagValue("FIO", "project_properties");
                model.project_properties.job = xml.GetTagValue("job", "project_properties");
                model.systems = xml.GetSystemFromXml();
                Redaction taskWindow = new Redaction(model);
                taskWindow.Show();
            }

        }
    }
}
