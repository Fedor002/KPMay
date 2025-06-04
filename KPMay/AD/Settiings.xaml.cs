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
using System.Windows.Shapes;

namespace KPMay
{
    /// <summary>
    /// Логика взаимодействия для Settiings.xaml
    /// </summary>
    public partial class Settiings : Window
    {
        AD_APP ad_app = new AD_APP();
        ProjectModel model;
        public Settiings()
        {
            InitializeComponent();
            tb_fio.Text = ad_app.ReadAllUsers(model.dbPath, model.dbName).FirstOrDefault() is var user && user != default ? user.FIO : string.Empty;
            tb_enterprise.Text = ad_app.ReadAllEnterprises(model.dbPath, model.dbName).FirstOrDefault() is var ent && ent != default ? ent.name : string.Empty;
            tb_job_title.Text = ad_app.ReadAllJobs(model.dbPath, model.dbName).FirstOrDefault() is var job && job != default ? job.name : string.Empty;
        }

        public Settiings(ProjectModel model)
        {
            this.model = model;
            InitializeComponent();
            this.DataContext = model;
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            ad_app.InsertOrUpdateEnterprise(tb_enterprise.Text, model.dbPath, model.dbName);
            ad_app.InsertOrUpdateUser(tb_fio.Text, model.dbPath, model.dbName);
            ad_app.InsertOrUpdateJob(tb_job_title.Text, model.dbPath, model.dbName);
            MessageBox.Show("Данные успешно сохранены!");
        }
    }
}
