using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class AD_MyFile
    {
        private string _path = "";
        private string _name = "";
        private string _extension = "";

        public MyFile() { }
        public MyFile(string[] files)
        {
            _path = files[0];
            _name = Path.GetFileName(_path);
            _extension = Path.GetExtension(_path);
        }
        public string GetPath
        {
            get { return _path; }
        }
        public string GetName
        {
            get { return _name; }
        }
        public string GetExtension
        {
            get { return _extension; }
        }
    }
}
