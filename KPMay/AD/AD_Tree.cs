using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KPMay
{
    public class AD_Tree
    {
        private string _name { get; set; }
        private ObservableCollection<AD_Tree> _children { get; set; } = new ObservableCollection<AD_Tree>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ObservableCollection<AD_Tree> Nodes
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}