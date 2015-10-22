using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserAdminApp
{
    public partial class ServerConfigForm : Form
    {
        public ServerConfigForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.Text = "L&T Infotech";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
            cbServerName.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbServerName.Items.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"]));
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Servers Loaded");
        }

        private void btnLoadDB_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                clearDBCombobox();
                string connection = getConnectionString();
                if (isDBConnected(connection))
                {
                    try
                    {
                        foreach (string item in getDatabases(connection))
                        {
                            cbDatabaseName.Items.Add(item);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Could not establish connection, check credentials!");
                        clearDBCombobox();
                        ex.Message.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Server Not Found!");
                    clearDBCombobox();
                    
                }
            }
            catch (NullReferenceException nre)
            {
                MessageBox.Show("Server Name Missing or Incorrect!");
                clearDBCombobox();
                nre.Message.ToString();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string serverName = getServerName();
            string databaseName = getDatabaseName();
            if (serverName.Equals(""))
            {
                MessageBox.Show("Please Check Server Name");
            }
            else if (databaseName.Equals(""))
            {
                MessageBox.Show("Please Check Database Name");
            }
            else if (isDBConnected(getConnectionString(databaseName)))
            {
                if (chbSave.Checked)
                {
                    Server s = new Server();
                    s.InstanceName = serverName;
                    s.DbName = databaseName;
                    s.UserName = txtUserName.Text;
                    s.Password = txtPassword.Text;
                    s.IntegratedSecurity = rbWindowsAuthentication.Checked;
                    Extension.writeStringToPath(Program.configPath, Extension.Serialize<Server>(s));
                }
                Program.connString = getConnectionString(databaseName);
                this.Dispose();
                Program.OpenMainUIFlag = true;
            }
            else
            {
                MessageBox.Show("Database Connectivity Failed, try again!");
            }
            Cursor.Current = Cursors.Default;
        }

        private void rbChanged(object sender, EventArgs e)
        {
            if (rbWindowsAuthentication.Checked)
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void serverNameChanged(object sender, EventArgs e)
        {
            clearDBCombobox();
        }

        private List<string> getDatabases(string strConn)
        {
            List<string> databases = new List<string>();
            try
            {
                SqlConnection sqlConn = new SqlConnection(strConn);
                sqlConn.Open();
                DataTable tblDatabases = sqlConn.GetSchema("Databases");
                sqlConn.Close();
                clearDBCombobox();
                foreach (DataRow row in tblDatabases.Rows)
                {
                    String strDatabaseName = row["database_name"].ToString();
                    databases.Add(strDatabaseName);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Could not establish connection, check credentials!");
                ex.Message.ToString();
            }
            return databases;
        }

        private string getConnectionString()
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = cbServerName.Text.Equals("") ? cbServerName.SelectedItem.ToString() : cbServerName.Text;
            if (rbWindowsAuthentication.Checked)
            {
                connection.IntegratedSecurity = true;
            }
            else
            {
                connection.UserID = txtUserName.Text;
                connection.Password = txtPassword.Text;
            }
            connection.InitialCatalog = "master";
            return connection.ToString();
        }

        private string getConnectionString(string dbName)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = cbServerName.Text.Equals("") ? cbServerName.SelectedItem.ToString() : cbServerName.Text;
            if (rbWindowsAuthentication.Checked)
            {
                connection.IntegratedSecurity = true;
            }
            else
            {
                connection.UserID = txtUserName.Text;
                connection.Password = txtPassword.Text;
            }
            connection.InitialCatalog = dbName;
            return connection.ToString();
        }

        private string getServerName()
        {
            string serverName = "";
            try
            {
                serverName = cbServerName.Text.Equals("") ? cbServerName.SelectedItem.ToString() : cbServerName.Text;
            }
            catch (NullReferenceException nre)
            {
                nre.Message.ToString();
            }
            return serverName;
        }

        private string getDatabaseName()
        {
            string databaseName = "";
            try
            {
                databaseName = cbDatabaseName.Text.Equals("") ? cbDatabaseName.SelectedItem.ToString() : cbDatabaseName.Text;
            }
            catch (NullReferenceException nre)
            {
                nre.Message.ToString();
            }
            return databaseName;
        }

        private bool isDBConnected(string strConn)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            try
            {
                sqlConn.Open();
                sqlConn.Close();
            }
            catch (SqlException ex)
            {
                ex.Message.ToString();
                return false;
            }
            
            return true;
        }

        private void clearDBCombobox()
        {
            cbDatabaseName.Items.Clear();
            cbDatabaseName.Items.Add("Test");
            cbDatabaseName.Items.Clear();
        }
    }
}
