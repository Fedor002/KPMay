﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay
{
    public class custom_system
    {
        private string _name { get; set; }
        private ObservableCollection<custom_system> _children { get; set; } = new ObservableCollection<custom_system>();
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
    public class CustomTags 
    {
        private string _enterprise_matrix = "enterprise_matrix";
        private string _technology_matrix = "technology_matrix";
        private string _integration_matrix = "integration_matrix";
        private string _enterprise_grade = "enterprise_grade";
        private string _technology_grade = "technology_grade";
        private string _integration_grade = "integration_grade";
        private string _system = "system";
        private string _subsystem = "subsystem";
        private string _id = "id";
        private string _name = "name";

        public string enterprise_matrix
        {
            get { return _enterprise_matrix; }
        }
        public string technology_matrix
        {
            get { return _technology_matrix; }
        }
        public string integration_matrix
        {
            get { return _integration_matrix; }
        }
        public string enterprise_grade
        {
            get { return _enterprise_grade; }
        }
        public string technology_grade
        {
            get { return _technology_grade; }
        }
        public string integration_grade
        {
            get { return _integration_grade; }
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
    }
}
