using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class System
    {
        private string _name { get; set; }
        private ObservableCollection<AD_Tree> _children { get; set; } = new ObservableCollection<AD_Tree>();
        private string _id;
        private double[,] _enterprise_matrix;
        private double[,] _technology_matrix;
        private double[,] _integration_matrix;
        private int _enterprise_grade;
        private int _technology_grade;
        private int _integration_grade;

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
        public ObservableCollection<AD_Tree> Nodes
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
        public double[,] integration_matrix
        {
            get { return _integration_matrix; }
            set { _integration_matrix = value; }
        }

        public int enterprise_grade
        {
            get { return _enterprise_grade; }
            set { _enterprise_grade = value; }
        }

        public int technology_grade
        {
            get { return _technology_grade; }
            set { _technology_grade = value; }
        }

        public int integration_grade
        {
            get { return _integration_grade; }
            set { _integration_grade = value; }
        }
    }
}
