using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMay.Math
{
    public class SquareMatrix
    {
        public double[,] _matrix;
        public int Size { get; }
        public List<string> RowNames { get; }

        public SquareMatrix(int size, List<string> names)
        {
            Size = size;
            _matrix = new double[size, size];
            RowNames = names;
        }

        public double this[int row, int col]
        {
            get => _matrix[row, col];
            set => _matrix[row, col] = value;
        }

        public double this[string rowName, string colName]
        {
            get
            {
                int row = RowNames.IndexOf(rowName);
                int col = RowNames.IndexOf(colName);
                return _matrix[row, col];
            }
            set
            {
                int row = RowNames.IndexOf(rowName);
                int col = RowNames.IndexOf(colName);
                _matrix[row, col] = value;
            }
        }
    }
}
