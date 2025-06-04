using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class custom_system : INotifyPropertyChanged
    {
        private string _name { get; set; }
        private ObservableCollection<custom_system> _children { get; set; } = new ObservableCollection<custom_system>();
        private string _id;
        private string _critical_technology; //Критическая технология
        private int _lvl; //Уровень вложенности
        private double[,] _enterprise_matrix; //УГИ УГПС
        private double[,] _technology_matrix; //УГИ УГТС
        private double _enterprise_grade; //УГП
        private double _technology_grade; //УГТ

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int lvl
        {
            get { return _lvl; }
            set { _lvl = value; }
        }

        public string criticalTechnology
        {
            get { return _critical_technology; }
            set { _critical_technology = value; }
        }
        public ObservableCollection<custom_system> Nodes
        {
            get { return _children; }
            set { _children = value; }
        }
        public double[,] enterprise_matrix
        {
            get { return _enterprise_matrix; }
            set { _enterprise_matrix = value; }
        }
        public double[,] technology_matrix
        {
            get { return _technology_matrix; }
            set { _technology_matrix = value; }
        }

        public double enterprise_grade
        {
            get { return _enterprise_grade; }
            set { _enterprise_grade = value; }
        }

        public double technology_grade
        {
            get { return _technology_grade; }
            set { _technology_grade = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class CustomTags 
    {
        private string _enterprise_matrix = "enterprise_matrix";
        private string _technology_matrix = "technology_matrix";
        private string _enterprise_grade = "enterprise_grade";
        private string _technology_grade = "technology_grade";
        private string _system = "system";
        private string _subsystem = "subsystem";
        private string _id = "id";
        private string _name = "name";
        private string _lvl = "lvl";
        private string _critical_technology = "critical_technology";

        public string enterprise_matrix
        {
            get { return _enterprise_matrix; }
        }
        public string technology_matrix
        {
            get { return _technology_matrix; }
        }
        public string enterprise_grade
        {
            get { return _enterprise_grade; }
        }
        public string technology_grade
        {
            get { return _technology_grade; }
        }
        public string system
        {
            get { return _system; }
        }
        public string subsystem
        {
            get { return _subsystem; }
        }
        public string id
        {
            get { return _id; }
        }
        public string name
        {
            get { return _name; }
        }
        public string lvl
        {
            get { return _lvl; }
        }
        public string critical_technology
        {
            get { return _critical_technology; }
        }
    }
}
