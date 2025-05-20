using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using KPMay.Math;

namespace KPMay
{
    public class MathMatrix
    {
        protected SquareMatrix _matrix;
        private Dictionary<string, double> _vector;

        public MathMatrix(SquareMatrix matrix, Dictionary<string, double> vector)
        {
            _matrix = matrix;
            _vector = vector;
        }

        public double CalcTechLvl() {

            List<double> vector = new List<double>();


            foreach (var element in _vector)
            {
                vector.Add(element.Value);

            }

            double[] result = MultiplyMatrixByVector(vector);

            int n_i = 2*_matrix.Size -1;
            double sum =0;

            foreach (var element in result)
            {
                sum += element/n_i;
            }

            return sum/(_matrix.Size * _matrix.Size);

        }

        public double[] MultiplyMatrixByVector(List<double> vector)
        {
            double[,] matrix = _matrix._matrix;
            if (matrix.GetLength(1) != vector.Count)
            {
                throw new ArgumentException("Количество столбцов матрицы должно быть равно количеству элементов вектора.");
            }

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[] result = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return result;
        }
    }
}

