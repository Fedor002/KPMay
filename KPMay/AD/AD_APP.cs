using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace KPMay
{
    public class AD_APP
    {
        public void CreateBasicTables(string path,string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
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

                using (var cmd = new SQLiteCommand(cmd_user, connection))
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

                using (var cmd = new SQLiteCommand(cmd_enterprise, connection))
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

                using (var cmd = new SQLiteCommand(cmd_job, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertOrUpdateUser(string value, string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = @"
            INSERT INTO users (id, FIO, job_id, created_at)
            VALUES (1, @value, @null, CURRENT_TIMESTAMP)
            ON CONFLICT(id) DO UPDATE SET
                FIO = excluded.FIO,
                created_at = CURRENT_TIMESTAMP;
        ";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.Parameters.AddWithValue("@null", 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertOrUpdateJob(string value, string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = @"
            INSERT INTO jobs (id, name, created_at)
            VALUES (1, @value, CURRENT_TIMESTAMP)
            ON CONFLICT(id) DO UPDATE SET
                name = excluded.name,
                created_at = CURRENT_TIMESTAMP;
        ";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertOrUpdateEnterprise(string value, string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = @"
            INSERT INTO enterprise (id, name, created_at)
            VALUES (1, @value, CURRENT_TIMESTAMP)
            ON CONFLICT(id) DO UPDATE SET
                name = excluded.name,
                created_at = CURRENT_TIMESTAMP;
        ";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<(int id, string FIO, int jobId, DateTime createdAt)> ReadAllUsers(string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            var result = new List<(int, string, int, DateTime)>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = "SELECT id, FIO, job_id, created_at FROM users;";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string fio = reader.GetString(1);
                        int jobId = reader.GetInt32(2);
                        DateTime createdAt = reader.GetDateTime(3);

                        result.Add((id, fio, jobId, createdAt));
                    }
                }
            }

            return result;
        }

        public List<(int id, string name, DateTime createdAt)> ReadAllEnterprises(string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            var result = new List<(int, string, DateTime)>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = "SELECT id, name, created_at FROM enterprise;";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        DateTime createdAt = reader.GetDateTime(2);

                        result.Add((id, name, createdAt));
                    }
                }
            }

            return result;
        }

        public List<(int id, string name, DateTime createdAt)> ReadAllJobs(string path, string dbName)
        {
            string _path = AD_General.ConvertEnviromentPatToPath(path) + "\\" + dbName + ".db";
            var result = new List<(int, string, DateTime)>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;"))
            {
                connection.Open();

                string query = "SELECT id, name, created_at FROM jobs;";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        DateTime createdAt = reader.GetDateTime(2);

                        result.Add((id, name, createdAt));
                    }
                }
            }

            return result;
        }

    }
}
