using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserAdminApp
{
    public partial class AppUI : Form
    {
        string sourcePath = "";
        string addUserLog = @"Add_User_Log.txt";
        string deleteUserLog = @"Delete_User_Log.txt";
        string logPath = @"log.txt";

        public AppUI()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            //Thread.Sleep(2000);
            InitializeComponent();
            this.Text = "L&T Infotech";
        }

        #region Browse File

        private void browseExcel_Click(object sender, EventArgs e)
        {
            browseExcelFile();
        }

        public void browseExcelFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\10614075\Desktop";
            openFileDialog.Filter = "Excel Files (.xlsx)|*.xlsx";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.sourcePath = openFileDialog.FileName;
                try
                {
                    ReadExcel excelFile = new ReadExcel(sourcePath);
                    try
                    {
                        excelFile.OpenSheet("AddUser");
                        txtLog.AppendText(Environment.NewLine + "Sheet AddUser opened..");
                        string log = this.validateExcel(this.sourcePath);
                        if (!log.Equals(""))
                        {
                            AddData.Enabled = false;
                            DeleteData.Enabled = false;
                            txtLog.AppendText(Environment.NewLine + "Browse the excel sheet again after data rectification..");
                            this.sourcePath = "";
                            txtFilePath.Text = "";
                            Extension.writeStringToPath(logPath, log);//this will write logs to a txt file
                            MessageBox.Show("File cannot be imported, Inconsisent data found. \nPlease check log file at log.txt for details.");
                            excelFile.CloseExcel();
                        }
                        else
                        {
                            AddData.Enabled = true;
                            DeleteData.Enabled = true;
                            txtFilePath.Text = this.sourcePath;
                            txtLog.AppendText(Environment.NewLine + Path.GetFileName(this.sourcePath) + " Imported.." + Environment.NewLine);
                            excelFile.CloseExcel();
                        }
                    }
                    catch (COMException nre)
                    {
                        nre.Message.ToString();
                        MessageBox.Show("Add User Sheet not found");
                    }
                }
                catch (COMException ex)
                {
                    MessageBox.Show(ex.Message.ToString().Replace("find", "find File"));
                    this.sourcePath = "";
                }
            }
            else
            {
                //might be needed in future.
            }

        }

        #endregion

        #region Add Data

        private void AddData_Click(object sender, EventArgs e)
        {
            string log = "";
            try
            {
                ReadExcel excelFile = new ReadExcel(sourcePath);
                try
                {
                    excelFile.OpenSheet("AddUser");
                    List<User> users = new List<User>();
                    users = excelFile.getAddUserList();
                    string xmlData = "";
                    DBConnect dbc = new DBConnect();
                    foreach (User user in users)
                    {
                        user.xml = Extension.Serialize<User>(user);
                        xmlData += Environment.NewLine + user.xml;
                        string str = "Working for user " + user.userName + ".." + Environment.NewLine;
                        str += Extension.decodeAddUserLog(dbc.addUserFromXml(user));
                        log += str + Environment.NewLine + Environment.NewLine;
                        txtLog.AppendText(Environment.NewLine + str);
                        Thread.Sleep(500);
                    }
                    Extension.writeStringToPath("xmlData.xml",xmlData);
                    Extension.writeStringToPath(this.addUserLog, log);
                    excelFile.CloseExcel();
                    MessageBox.Show("Operation Complete. Check \"" + addUserLog + "\" file for more details.");
                }
                catch (COMException nre)
                {
                    Console.WriteLine(nre.Message.ToString());
                    MessageBox.Show("Sheet not found");
                    this.sourcePath = "";
                }
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message.ToString().Replace("find", "find File"));
                this.sourcePath = "";
            }
        }

        #endregion

        #region Delete Data

        private void DeleteData_Click(object sender, EventArgs e)
        {
            string log = "";
            try
            {
                ReadExcel excelFile = new ReadExcel(sourcePath);
                try
                {
                    excelFile.OpenSheet("DeleteUser");
                    List<User> users = new List<User>();
                    users = excelFile.getDeleteUserDataList();

                    DBConnect dbc = new DBConnect();
                    foreach (User user in users)
                    {
                        user.xml = Extension.Serialize<User>(user);
                        string str = "Working for user " + user.userName + ".." + Environment.NewLine;
                        str += Extension.decodeDeleteUserLog(dbc.deleteUserFromXml(user));
                        log += str + Environment.NewLine + Environment.NewLine;
                        txtLog.AppendText(Environment.NewLine + str);
                        Thread.Sleep(500);
                    }
                    Extension.writeStringToPath(this.deleteUserLog, log);
                    excelFile.CloseExcel();
                    MessageBox.Show("Operation Complete. Check \"" + deleteUserLog + "\" file for more details.");
                }
                catch (COMException nre)
                {
                    Console.WriteLine(nre.Message.ToString());
                    MessageBox.Show("Sheet not found");
                    this.sourcePath = "";
                }
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message.ToString().Replace("find", "find File"));
                this.sourcePath = "";
            }
        }

        #endregion

        #region Functions

        public string validateExcel(string sourcePath)
        {
            ReadExcel excelFile = new ReadExcel(sourcePath);
            excelFile.OpenSheet("AddUser");
            List<User> users = excelFile.getAddUserList();
            string validatePersonalDetailsLog = excelFile.validatePersonalDetails();
            string validateEntityDetailsLog = excelFile.validateEntityDetails(users);
            string validateConsistencyLog = excelFile.validateHomeEntityConsistency(users);
            if (validatePersonalDetailsLog.Equals(""))
            {
                txtLog.AppendText(Environment.NewLine + "Personal Details Validated..");
            }
            else
            {
                txtLog.AppendText(Environment.NewLine + "Personal Details Validation Failed..");
            }
            Thread.Sleep(200);
            if (validateEntityDetailsLog.Equals(""))
            {
                txtLog.AppendText(Environment.NewLine + "Entity Details Validated..");
            }
            else
            {
                txtLog.AppendText(Environment.NewLine + "Entity Details Validation Failed..");
            }
            Thread.Sleep(200);
            if (validateConsistencyLog.Equals(""))
            {
                txtLog.AppendText(Environment.NewLine + "Data Consistency Validated..");
            }
            else
            {
                txtLog.AppendText(Environment.NewLine + "Inconsistent Data Found..");
            }
            Thread.Sleep(200);
            excelFile.CloseExcel();
            return validatePersonalDetailsLog + validateEntityDetailsLog + validateConsistencyLog;
        }

        #endregion

        #region UI Operations

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            //try
            //{
            //    excelFile.CloseExcel();
            //}
            //catch (NullReferenceException nre)
            //{
            //    nre.Message.ToString();
            //}
            //catch (InvalidCastException ice)
            //{
            //    ice.Message.ToString();
            //}
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            txtLog.Text = "Application Running..";
        }

        #endregion
    }
}
