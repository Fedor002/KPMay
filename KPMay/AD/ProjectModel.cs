using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class ProjectModel
    {
        private string _currentPath = null;
        private string _tempPath = null;
        private string _dbName = null;
        private string _projectName = null;
        private string _dbPath = null;

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
    }
}
