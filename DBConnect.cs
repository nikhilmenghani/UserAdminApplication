using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace UserAdminApp
{
    public class DBConnect
    {
        SqlConnection con = null;

        public DBConnect()
        {
            this.con = new SqlConnection(Program.connString);
        }

        public SqlCommand getCommand(string query)
        {
            SqlCommand cmd = new SqlCommand(query, this.con);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public string addUserFromXml(User user)
        {
            string status = "";
            string data = "";
            SqlCommand cmd = getCommand("Custom_AddUserFromXml");
            cmd.Parameters.AddWithValue("@xml", user.xml);
            this.con.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader.GetValue(0).ToString();
                        data = reader.GetValue(1).ToString();
                    }
                }
            }
            catch (SqlException e)
            {
                status = "Exception Occured while adding user " + user.userName + "\n Message : " + e.Message.ToString();
            }
            
            this.con.Close();
            return status + "|" + data;
        }

        public string deleteUserFromXml(User user)
        {
            string status = "";
            string data = "";
            SqlCommand cmd = getCommand("Custom_DeleteUserFromXml");
            cmd.Parameters.AddWithValue("@xml", user.xml);
            this.con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    status = reader.GetValue(0).ToString();
                    data = reader.GetValue(1).ToString();
                }
            }
            this.con.Close();
            return status + "|" + data;
        }
    }
}
