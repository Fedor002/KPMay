using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class ADProperties : INotifyPropertyChanged
    {
        private string _enterprise;
        private string _job;
        private string _FIO;
        private string _VVST_class;
        private string _VVST_name; 

        public string enterprise
        {
            get { return _enterprise; }
            set { _enterprise = value; }
        }

        public string job
        {
            get { return _job; }
            set { _job = value; } 
        }
        public string FIO
        {
            get { return _FIO; }
            set { _FIO = value; }
        }

        public string VVST_class
        {
            get { return _VVST_class; }
            set { _VVST_class = value; }
        }

        public string VVST_name
        {
            get { return _VVST_name; }
            set { _VVST_name = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public ADProperties() { }
    }
    public class ProjectModel : INotifyPropertyChanged
    {
        private string _currentPath = null;
        private string _tempPath = null;
        private string _dbName = null;
        private string _projectName = null;
        private string _dbPath = null;
        private custom_system _systems = new custom_system();
        private ADProperties _project_properties = new ADProperties();
        public ProjectModel() 
        { 

        }

        public string currentPath
        {
            get { return _currentPath; }
            set { _currentPath = value; }
        }

        public string tempPath
        {
            get { return _tempPath; }
            set { _tempPath = value; }
        }

        public string dbPath
        {
            get { return _dbPath; }
            set { _dbPath = value; }
        }
        public string dbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        public string projectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public custom_system systems
        {
            get { return _systems; }
            set { _systems = value; OnPropertyChanged(nameof(systems)); }
        }

        public ADProperties project_properties
        {
            get { return _project_properties; }
            set { _project_properties = value; OnPropertyChanged(nameof(project_properties)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
