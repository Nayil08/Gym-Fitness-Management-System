using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
namespace GymAndFitnessManagementSystem.Data
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabase()
        {
            string master = ConfigurationManager.ConnectionStrings["GymFitnessMaster"].ConnectionString;
            using(var c=new SqlConnection(master)){ c.Open(); using(var cmd=new SqlCommand("IF DB_ID('GymFitnessDB') IS NULL CREATE DATABASE GymFitnessDB;",c)) cmd.ExecuteNonQuery(); }
            string path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"database","schema.sql");
            if(!File.Exists(path)) path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"schema.sql");
            if(!File.Exists(path)) return;
            string sql=File.ReadAllText(path);
            // schema.sql is idempotent. Run only the database-specific part after USE.
            using(var c=Db.Open())
            {
                foreach(var batch in Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline|RegexOptions.IgnoreCase))
                {
                    var b=batch.Trim(); if(b.Length==0 || b.StartsWith("CREATE DATABASE",StringComparison.OrdinalIgnoreCase) || b.StartsWith("USE ",StringComparison.OrdinalIgnoreCase)) continue;
                    using(var cmd=new SqlCommand(b,c)){ cmd.CommandTimeout=120; cmd.ExecuteNonQuery(); }
                }
            }
        }
    }
}
