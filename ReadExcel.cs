using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace UserAdminApp
{
    public class ReadExcel
    {

        #region Open Excel Operations

        string filePath = "";
        Application xlApp;
        Workbook xlWorkbook;
        Worksheet xlWorksheet;
        Range xlRange;
        public ReadExcel(string path)
        {
            this.filePath = path;
            OpenExcel();
        }

        public void OpenExcel()
        {
            this.xlApp = new Application();
            this.xlWorkbook = xlApp.Workbooks.Open(filePath);
        }

        public void OpenSheet(int sheetNo)
        {
            this.xlWorksheet = (Worksheet)xlWorkbook.Worksheets.get_Item(sheetNo);
            this.xlRange = xlWorksheet.UsedRange;
        }

        public void OpenSheet(string sheetTitle)
        {
            this.xlWorksheet = (Worksheet)xlWorkbook.Worksheets[sheetTitle];
            this.xlRange = xlWorksheet.UsedRange;
        }

        public void CloseExcel()
        {
            try
            {
                //releaseObject(xlWorksheet);
                //releaseObject(xlWorkbook);
                //releaseObject(xlApp);
                //Marshal.FinalReleaseComObject(xlRange);
                //Marshal.FinalReleaseComObject(xlWorksheet);
                
                xlWorkbook.Close(Type.Missing, Type.Missing, Type.Missing);
                //Marshal.FinalReleaseComObject(xlWorkbook);

                xlApp.Quit();
                //Marshal.FinalReleaseComObject(xlApp);
            }
            catch (COMException e)
            {
                e.Message.ToString();
                System.Windows.Forms.MessageBox.Show("File already closed");
            }
            catch (InvalidComObjectException icoe)
            {
                icoe.Message.ToString();
            }
            
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Windows.Forms.MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion

        #region Sheet Operations

        public int getColumnIndex(string columnName)
        {
            for (int i = 1; i <= this.xlRange.Columns.Count; i++)
            {
                string cellValue = Convert.ToString((this.xlRange.Cells[1, i] as Range).Value2);
                if (cellValue.Equals(columnName))
                {
                    return i;
                }
            }
            return -1;
        }

        public int getUserIndex(string userName, List<User> users)
        {
            int index = 0;
            foreach (User user in users)
            {
                if (user.userName.Equals(userName))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        #endregion

        #region Add User

        public List<User> getAddUserList()
        {
            List<User> users = new List<User>();
            //List<ExcelData> data = scanExcelData1();
            users = addPersonalDetails();
            users = addEntityDetails(users);
            users = addRoleDetails(users);
            return users;
        }

        // This will add personal details of user.
        public List<User> addPersonalDetails()
        {
            List<string> usersList = new List<string>();
            List<User> users = new List<User>();
            int passwordIndex = getColumnIndex("Password");
            for (int rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                bool skipRow = true; //if user already exists, we dont need to iterate through the rows.
                string username = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                if (!usersList.Contains(username)) //this means that the username is encountered for the first time.
                {
                    usersList.Add(username);
                    //new user discovered and thus we won't skip iterating the row from excel.
                    skipRow = false;
                }
                if (!skipRow)
                {
                    User user = new User();
                    for (int cCnt = 1; cCnt <= passwordIndex; cCnt++)
                    {
                        string columnName = Convert.ToString((this.xlRange.Cells[1, cCnt] as Range).Value2);
                        string cellValue = Convert.ToString((this.xlRange.Cells[rCnt, cCnt] as Range).Value2);
                        switch (columnName)
                        {
                            case "UserName":
                                user.userName = cellValue;
                                break;
                            case "FullName":
                                user.fullName = cellValue;
                                break;
                            case "EmailId":
                                user.email = cellValue;
                                break;
                            case "Password":
                                user.password = cellValue;
                                break;
                        }
                    }
                    users.Add(user);
                }
            }
            return users;
        }

        // This will add the entity specific details of user.
        public List<User> addEntityDetails(List<User> users)
        {
            int rCnt = 0;
            int cCnt = 0;
            int entityNameIndex = getColumnIndex("EntityName");
            int producerIndex = getColumnIndex("Producer");
            int homeEntityIndex = getColumnIndex("HomeEntity");
            string columnName = "";
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                bool skipEntity = false;
                string username = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                int userIndex = getUserIndex(username, users);
                string entityname = Convert.ToString((this.xlRange.Cells[rCnt, entityNameIndex] as Range).Value2);
                string isHomeEntity = Convert.ToString((this.xlRange.Cells[rCnt, homeEntityIndex] as Range).Value2);
                foreach (Entity item in users[userIndex].entities)
                {
                    if (item.entityName.Equals(entityname))
                    {
                        skipEntity = true;
                    }
                }
                if (!skipEntity)
                {
                    Entity entity = new Entity();
                    for (cCnt = entityNameIndex; cCnt <= producerIndex; cCnt++)
                    {
                        columnName = Convert.ToString((this.xlRange.Cells[1, cCnt] as Range).Value2);
                        string cellValue = Convert.ToString((this.xlRange.Cells[rCnt, cCnt] as Range).Value2);
                        switch (columnName)
                        {
                            case "EntityName":
                                entity.entityName = cellValue;
                                break;
                            case "HomeEntity":
                                entity.homeEntity = cellValue.ToLower().Equals("yes") ? 1 : 0;
                                break;
                            case "Producer":
                                entity.producer = cellValue.ToLower().Equals("yes") ? 1 : 0;
                                break;
                        }
                    }
                    entity.userName = username;
                    //find the user index and add entity to it.
                    users[userIndex].entities.Add(entity);
                }
            }
            return users;
        }

        // This will assign the roles to specific entity.
        public List<User> addRoleDetails(List<User> users)
        {
            int rCnt = 0;
            int userNameColIndex = getColumnIndex("UserName");
            int entityNameColIndex = getColumnIndex("EntityName");
            int roleColIndex = getColumnIndex("Role");
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                string username = Convert.ToString((this.xlRange.Cells[rCnt, userNameColIndex] as Range).Value2);
                string entityname = Convert.ToString((this.xlRange.Cells[rCnt, entityNameColIndex] as Range).Value2);
                string roleName = Convert.ToString((this.xlRange.Cells[rCnt, roleColIndex] as Range).Value2);

                int userIndex = getUserIndex(username, users);
                int entityIndex = 0;
                foreach (Entity item in users[userIndex].entities)
                {
                    if (item.entityName.Equals(entityname))
                    {
                        break;
                    }
                    entityIndex++;
                }

                Role role = new Role();
                role.role = roleName;
                role.entityName = entityname;
                role.userName = username;
                users[userIndex].entities[entityIndex].roles.Add(role);
            }
            return users;
        }

        #endregion

        #region Delete User Data

        public List<User> getDeleteUserDataList()
        {
            List<User> users = new List<User>();
            users = storeUserDetails();
            users = storeEntityDetails(users);
            users = storeRoleDetails(users);
            return users;
        }

        public List<User> storeUserDetails()
        {
            List<string> usersList = new List<string>();
            List<User> users = new List<User>();
            for (int rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                bool skipRow = true; //if user already exists, we dont need to iterate through the rows.
                string username = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                if (!usersList.Contains(username)) //this means that the username is encountered for the first time.
                {
                    usersList.Add(username);
                    //new user discovered and thus we won't skip iterating the row from excel.
                    skipRow = false;
                }
                if (!skipRow)
                {
                    User user = new User();
                    user.userName = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                    users.Add(user);
                }
            }
            return users;
        }

        public List<User> storeEntityDetails(List<User> users)
        {
            int rCnt = 0;
            int entityNameIndex = getColumnIndex("EntityName");
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                bool skipEntity = false;
                string username = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                int userIndex = getUserIndex(username, users);
                string entityname = Convert.ToString((this.xlRange.Cells[rCnt, getColumnIndex("EntityName")] as Range).Value2);
                foreach (Entity item in users[userIndex].entities)
                {
                    if (item.entityName.Equals(entityname))
                    {
                        skipEntity = true;
                    }
                }
                if (!skipEntity)
                {
                    Entity entity = new Entity();
                    entity.entityName = Convert.ToString((this.xlRange.Cells[rCnt, entityNameIndex] as Range).Value2);
                    entity.userName = username;
                    users[userIndex].entities.Add(entity);//find the user index and add entity to it.
                }
            }
            return users;
        }

        public List<User> storeRoleDetails(List<User> users)
        {
            int rCnt = 0;
            int userNameColIndex = getColumnIndex("UserName");
            int entityNameColIndex = getColumnIndex("EntityName");
            int roleColIndex = getColumnIndex("Role");
            int instructionColIndex = getColumnIndex("Instructions");
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                string username = Convert.ToString((this.xlRange.Cells[rCnt, userNameColIndex] as Range).Value2);
                string entityname = Convert.ToString((this.xlRange.Cells[rCnt, entityNameColIndex] as Range).Value2);
                string roleName = Convert.ToString((this.xlRange.Cells[rCnt, roleColIndex] as Range).Value2);
                string instruction = Convert.ToString((this.xlRange.Cells[rCnt, instructionColIndex] as Range).Value2);

                int userIndex = getUserIndex(username, users);
                int entityIndex = 0;
                foreach (Entity item in users[userIndex].entities)
                {
                    if (item.entityName.Equals(entityname))
                    {
                        break;
                    }
                    entityIndex++;
                }

                Role role = new Role();
                role.role = roleName;
                role.entityName = entityname;
                role.userName = username;
                role.instruction = instruction;
                users[userIndex].entities[entityIndex].roles.Add(role);
            }
            return users;
        }

        #endregion

        #region Validate Excel

        public string validatePersonalDetails()
        {
            int rCnt = 0;
            int cCnt = 0;
            int passwordIndex = getColumnIndex("Password");
            string log = "";
            List<User> allUsers = new List<User>();
            List<User> uniqueUsers = new List<User>();
            List<string> usersList = new List<string>();
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                User user = new User();
                for (cCnt = 1; cCnt <= passwordIndex; cCnt++)
                {
                    string columnName = Convert.ToString((this.xlRange.Cells[1, cCnt] as Range).Value2);
                    string cellValue = Convert.ToString((this.xlRange.Cells[rCnt, cCnt] as Range).Value2);
                    switch (columnName)
                    {
                        case "UserName":
                            user.userName = cellValue;
                            break;
                        case "FullName":
                            user.fullName = cellValue;
                            break;
                        case "EmailId":
                            user.email = cellValue;
                            break;
                        case "Password":
                            user.password = cellValue;
                            break;
                    }
                }
                string username = Convert.ToString((this.xlRange.Cells[rCnt, 1] as Range).Value2);
                if (!usersList.Contains(username))
                {
                    usersList.Add(username);
                    uniqueUsers.Add(user);
                }
                allUsers.Add(user);
            }

            foreach (User user in allUsers)
            {
                foreach (User uniqueUser in uniqueUsers)
                {
                    if (user.userName.Equals(uniqueUser.userName))
                    {
                        bool errorFound = false;
                        if (!user.fullName.Equals(uniqueUser.fullName))
                        {
                            log += "FullName mismatch for user " + user.userName + " found" + Environment.NewLine;
                            errorFound = true;
                        }

                        if (!user.email.Equals(uniqueUser.email))
                        {
                            log += "Email mismatch for user " + user.userName + " found" + Environment.NewLine;
                            errorFound = true;
                        }

                        if (!user.password.Equals(uniqueUser.password))
                        {
                            log += "Password mismatch for user " + user.userName + " found" + Environment.NewLine;
                            errorFound = true;
                        }
                        if (errorFound)
                        {
                            break;
                        }
                    }
                }
            }
            return log;
        }

        public string validateEntityDetails(List<User> users)
        {
            string log = "";
            foreach (User user in users)
            {
                if (!(getHomeEntityCount(user) == 1))
                {
                    log += "Multiple HomeEntities for user " + user.userName + " found. " +
                    "A user cannot have multiple home entities " + Environment.NewLine;
                }
            }
            return log;
        }

        public string validateHomeEntityConsistency(List<User> users)
        {
            string log = "";
            int rCnt = 0;
            int userNameIndex = getColumnIndex("UserName");// Convert.ToString((this.xlRange.Cells[rCnt, getColumnIndex("UserName")] as Range).Value2);
            int entityNameIndex = getColumnIndex("EntityName");// Convert.ToString((this.xlRange.Cells[rCnt, getColumnIndex("EntityName")] as Range).Value2);
            int homeEntityIndex = getColumnIndex("HomeEntity");// Convert.ToString((this.xlRange.Cells[rCnt, getColumnIndex("HomeEntity")] as Range).Value2);
            List<UserEntityRelationship> homeEntityTrueList = new List<UserEntityRelationship>();
            List<UserEntityRelationship> homeEntityFalseList = new List<UserEntityRelationship>();
            for (rCnt = 2; rCnt <= this.xlRange.Rows.Count; rCnt++)
            {
                UserEntityRelationship uer = new UserEntityRelationship();
                uer.userName = Convert.ToString((this.xlRange.Cells[rCnt, userNameIndex] as Range).Value2);
                uer.entityName = Convert.ToString((this.xlRange.Cells[rCnt, entityNameIndex] as Range).Value2);
                string homeEntity = Convert.ToString((this.xlRange.Cells[rCnt, homeEntityIndex] as Range).Value2);
                if (homeEntity.ToLower().Equals("yes"))
                {
                    homeEntityTrueList.Add(uer);
                }
                else if (homeEntity.ToLower().Equals("no"))
                {
                    homeEntityFalseList.Add(uer);
                }
            }

            foreach (UserEntityRelationship trueList in homeEntityTrueList)
            {
                foreach (UserEntityRelationship falseList in homeEntityFalseList)
                {
                    if (falseList.entityName.Equals(trueList.entityName) && falseList.userName.Equals(trueList.userName))
                    {
                        log += "Inconsistent Data Found with combination of user " + trueList.userName +
                            " and entity " + trueList.entityName + ", check value of HomeEntity." + Environment.NewLine;
                    }
                }
            }

            //check if entities which are present in Yes are present in No and vice versa.
            return log;
        }

        public int getHomeEntityCount(User user)
        {
            int homeEntityCount = 0;
            foreach (Entity entity in user.entities)
            {
                if (entity.homeEntity == 1)
                {
                    homeEntityCount++;
                }
            }
            return homeEntityCount;
        }

        #endregion

    }
}
