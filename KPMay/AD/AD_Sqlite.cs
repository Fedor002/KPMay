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

        public void CmdExecuteNonQuery(string path, string dbName, string cmd)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();
                using (SQLiteCommand _cmd = new SQLiteCommand(cmd, connection))
                {
                    _cmd.ExecuteNonQuery();
                }
            }
        }
//-------------------------------------------------------------------------------------------------------------------------------------
    }
}
