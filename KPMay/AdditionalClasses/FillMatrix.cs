using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPMay.Math;
using static KPMay.Redaction;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace KPMay.AdditionalClasses
{
    public class MatrixRow
    {
        public string RowName { get; set; }
        public double[] Values { get; set; }
    }
    public class FillMatrix
    {
        private Dictionary<string, double> _vectorValues;
        private SquareMatrix MatrixContext;
        private TreeView treeView;

        public FillMatrix(TreeView treeView)
        {
            this.treeView = treeView;
        }

        public void CreateSquareMatrix(object sender, RoutedEventArgs e)
        {
            // Получаем названия корневых узлов
            var rootNames = new List<string>();
            foreach (AD_Tree node in treeView.Items)
            {
                rootNames.Add(node.Name);
            }

            int size = rootNames.Count;

            if (size == 0)
            {
                MessageBox.Show("Нет корневых узлов в дереве!");
                return;
            }


            MatrixContext = new SquareMatrix(size, rootNames);


            // Заполняем нулями (или другими значениями по умолчанию)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    MatrixContext[i, j] = 0;
                }
            }

            ShowMatrix(MatrixContext);
        }

        private void ShowMatrix(SquareMatrix matrix)
        {
            // Получаем названия корневых узлов
            var rootNames = new List<string>();
            foreach (AD_Tree node in treeView.Items)
            {
                rootNames.Add(node.Name); // Используем свойство Name из AD_Tree
            }

            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                HeadersVisibility = DataGridHeadersVisibility.All
            };

            // Добавляем колонку с названиями строк
            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Системы",
                Binding = new Binding("RowName"),
                IsReadOnly = true
            });

            // Добавляем колонки с названиями корневых узлов
            for (int j = 0; j < matrix.Size; j++)
            {
                grid.Columns.Add(new DataGridTextColumn
                {
                    Header = rootNames[j],
                    Binding = new Binding($"Values[{j}]")
                });
            }

            // Создаем данные для привязки
            var items = new List<MatrixRow>();
            for (int i = 0; i < matrix.Size; i++)
            {
                var values = new double[matrix.Size];
                for (int j = 0; j < matrix.Size; j++)
                {
                    values[j] = matrix[i, j];
                }

                items.Add(new MatrixRow
                {
                    RowName = rootNames[i], // Название строки
                    Values = values
                });
            }

            grid.ItemsSource = items;
            grid.CellEditEnding += (s, e) =>
            {
                if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() != "Системы")
                {
                    int rowIndex = e.Row.GetIndex();
                    int columnIndex = e.Column.DisplayIndex - 1; // -1 потому что первая колонка - названия

                    var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);
                    matrix[rowIndex, columnIndex] = newValue;
                }
            };

            var button = new Button
            {
                Content = "Перейти к заполнению вектора",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(grid);
            stackPanel.Children.Add(button);

            var matrixWindow = new Window
            {
                Title = $"Матрица связей систем (порядок {matrix.Size})",
                Content = stackPanel,
                Width = 800,
                Height = 650
            };

            button.Click += (s, e) =>
            {
                matrixWindow.Close();
                ShowVectorInput(matrix);
            };

            matrixWindow.Show();
        }

        private void ShowVectorInput(SquareMatrix matrix)
        {
            var rootNames = new List<string>();
            foreach (AD_Tree node in treeView.Items)
            {
                rootNames.Add(node.Name);
            }

            // Инициализация вектора (значения по умолчанию 0)
            _vectorValues = rootNames.ToDictionary(name => name, _ => 0.0);

            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                ItemsSource = _vectorValues.Select(v => new VectorItem
                {
                    SystemName = v.Key,
                    Value = v.Value
                }).ToList(),
                Margin = new Thickness(10)
            };

            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Система",
                Binding = new Binding("SystemName"),
                IsReadOnly = true
            });

            grid.Columns.Add(new DataGridTextColumn
            {
                Header = "Значение",
                Binding = new Binding("Value")
            });

            // Обновляем вектор при изменении ячейки
            grid.CellEditEnding += (sender, e) =>
            {
                if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Значение")
                {
                    var editedItem = (VectorItem)e.Row.Item;
                    var newValue = Convert.ToDouble(((TextBox)e.EditingElement).Text);

                    _vectorValues[editedItem.SystemName] = newValue; // Обновляем значение в словаре
                }
            };

            var vectorWindow = new Window
            {
                Title = "Вектор значений систем",
                Content = grid,
                Width = 400,
                Height = 500
            };

            vectorWindow.Show();
        }

        public Dictionary<string, double> GetResultVect()
        {
            return _vectorValues;
        }

        public SquareMatrix GetResultMatr()
        {
            return MatrixContext;
        }
    }
}
