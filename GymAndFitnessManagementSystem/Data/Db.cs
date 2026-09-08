using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace GymAndFitnessManagementSystem.Data
{
    public static class Db
    {
        public static SqlConnection Open()
        {
            var c = new SqlConnection(ConfigurationManager.ConnectionStrings["GymFitnessDb"].ConnectionString); c.Open(); return c;
        }
        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using(var c=Open()) using(var cmd=new SqlCommand(sql,c)) { if(parameters!=null) cmd.Parameters.AddRange(parameters); using(var da=new SqlDataAdapter(cmd)){var t=new DataTable(); da.Fill(t); return t;} }
        }
        public static object Scalar(string sql, params SqlParameter[] parameters)
        { using(var c=Open()) using(var cmd=new SqlCommand(sql,c)){ if(parameters!=null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteScalar(); } }
        public static int Execute(string sql, params SqlParameter[] parameters)
        { using(var c=Open()) using(var cmd=new SqlCommand(sql,c)){ if(parameters!=null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteNonQuery(); } }
        public static SqlParameter P(string name, object value) => new SqlParameter(name, value ?? DBNull.Value);
    }
}
