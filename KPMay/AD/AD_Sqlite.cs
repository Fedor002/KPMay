using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

//Это класс с обобщенными методами для работы SQLite, точечные, то есть для приложения в AD_APP
namespace KPMay
{
    public class AD_Sqlite
    {
        public void CreateDBFile(string path, string dbName)
        {
            string dbPath = AD_General.ConvertEnviromentPatToPath(path);
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }
            dbPath += "\\" + dbName + ".db";
            if (!File.Exists(dbPath))
            {
                    SQLiteConnection.CreateFile(dbPath);
            }
        }


    }
}
