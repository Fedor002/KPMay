using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace KPMay
{
    public class AD_Excel
    {
        string path = "";
        _excel.Application eApp = new _excel.Application();
        _excel.Workbook eBook;
        _excel.Worksheet eSheet;

        public Excel(string path, int sheet)
        {
            this.path = path;
            eBook = eApp.Workbooks.Open(path);
            eSheet = eBook.Worksheets[sheet];
        }

        public void CreateNewFile()
        {
            eBook = eApp.Workbooks.Add();
            eSheet = eBook.Worksheets[1];
        }

        public void Save()
        {
            eBook.Save();
        }

        public void SaveAs(string path)
        {
            eBook.SaveAs(path);
        }

        public void Load()
        {
            eBook = eApp.Workbooks.Open(path);
        }

        public void CreateNewSheet()
        {
            _excel.Worksheet tempSheet = eBook.Worksheets.Add();
            eSheet = tempSheet;
        }

        public void selectSheet(int sheet)
        {
            eSheet = eBook.Worksheets[sheet];
        }

        public void DeleteSheet(int sheet)
        {
            eBook.Worksheets[sheet].Delete();
        }

        public void Close()
        {
            eBook.Close();
            eApp.Quit();
            Marshal.ReleaseComObject(eSheet);
            Marshal.ReleaseComObject(eBook);
            Marshal.ReleaseComObject(eApp);
        }

        public void protectSheet()
        {
            eSheet.Protect();
        }

        public void unprotectSheet()
        {
            eSheet.Unprotect();
        }

        public void protectSheet(string password)
        {
            eSheet.Protect(password);
        }

        public void unprotectSheet(string password)
        {
            eSheet.Unprotect(password);
        }
        public string ReadCell(int row, int col) //1 ячейка - 1;1
        {
            row++;
            col++;
            if (eSheet.Cells[row, col].Value2 != null)
            {
                return eSheet.Cells[row, col].Value2.ToString();
            }
            else
            {
                return "";
            }
        }
        public void WriteCell(int row, int col, string text)
        {
            row++;
            col++;
            eSheet.Cells[row, col].Value2 = text;
        }
        public string[,] CleanEmptyRowsAndColumns(string[,] data)
        {
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);
            bool[] rowHasData = new bool[rowCount];
            bool[] colHasData = new bool[colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (!string.IsNullOrWhiteSpace(data[i, j]))
                    {
                        rowHasData[i] = true;
                        colHasData[j] = true;
                    }
                }
            }

            int row = rowHasData.Count(r => r);
            int col = colHasData.Count(c => c);

            string[,] clearData = new string[row, col];

            int irow = 0;
            for (int i = 0; i < rowCount; i++)
            {
                if (rowHasData[i])
                {
                    int icol = 0;
                    for (int j = 0; j < colCount; j++)
                    {
                        if (colHasData[j])
                        {
                            clearData[irow, icol] = data[i, j];
                            icol++;
                        }
                    }
                    irow++;
                }
            }
            return clearData;
        }

        public string[,] ReadRange(int startRow, int startCol, int endRow, int endCol)
        {
            _excel.Range range = eSheet.Range[eSheet.Cells[startRow, startCol], eSheet.Cells[endRow, endCol]];
            object[,] holder = range.Value2;
            string[,] returner = new string[endRow - startRow + 1, endCol - startCol + 1];
            for (int i = 1; i <= endRow - startRow + 1; i++)
            {
                for (int j = 1; j <= endCol - startCol + 1; j++)
                {
                    if (holder[i, j] != null)
                    {
                        returner[i - 1, j - 1] = holder[i, j].ToString();
                    }
                    else
                    {
                        returner[i - 1, j - 1] = "";
                    }
                }
            }
            return CleanEmptyRowsAndColumns(returner);
        }

        public void WriteRange(int startRow, int startCol, string[,] text)
        {
            startRow++;
            startCol++;
            _excel.Range range = eSheet.Range[eSheet.Cells[startRow, startCol], eSheet.Cells[startRow + text.GetLength(0) - 1, startCol + text.GetLength(1) - 1]];
            range.Value2 = text;
        }

        public int GetRowsCount()
        {
            return eSheet.UsedRange.Rows.Count;
        }

        public int GetColumnsCount()
        {
            return eSheet.UsedRange.Columns.Count;
        }
    }
}
