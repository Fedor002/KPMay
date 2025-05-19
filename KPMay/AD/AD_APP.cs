using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace KPMay
{
    public class AD_APP
    {
        public void CreateBasicTables(string path,string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SqliteConnection connection = new SqliteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string cmd_user = @"
                    CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FIO TEXT NOT NULL,
                    job_id INTEGER NOT NULL,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                ";

                using (var cmd = new SqliteCommand(cmd_user, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                string cmd_enterprise = @"
                    CREATE TABLE IF NOT EXISTS enterprise (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                ";

                using (var cmd = new SqliteCommand(cmd_enterprise, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                string cmd_job = @"
                    CREATE TABLE IF NOT EXISTS jobs (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                ";

                using (var cmd = new SqliteCommand(cmd_job, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
