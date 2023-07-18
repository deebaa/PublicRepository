using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PublicRepository
{
    public class DBHelper
    {
        ApplicationConfig config;
        public string spGetusers { get; set; }


        public DBHelper() 
        {
            config = new ApplicationConfig();
            spGetusers = "GetUsers";
        }

        public DataTable ExecuteStoredProcedure(string spName)
        {
            DataTable dtTable = new DataTable();
            using (var dbConn = new SqlConnection(config.employeeConnection))
            {
                var adapter = new SqlDataAdapter();
                var cmd = new SqlCommand();
                cmd.Connection=dbConn; ;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;

                adapter.SelectCommand = cmd;
                dbConn.Open();
                adapter.Fill(dtTable);
            }
            return dtTable;
        }

        public List<User> GetUsers()
        {
            List<User> lstUsers = new List<User>();

            DataTable dtUsers = ExecuteStoredProcedure(spGetusers);
            lstUsers = (from DataRow row in dtUsers.Rows
                        select new User()
                        {
                            user = row["username"].ToString() ?? "",
                            token = row["token"].ToString() ?? ""
                        }).ToList();
            return lstUsers;
        }
    }
}
