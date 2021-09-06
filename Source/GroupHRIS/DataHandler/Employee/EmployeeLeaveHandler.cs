using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Employee
{
    public class EmployeeLeaveHandler : TemplateDataHandler
    {
        public DataTable populateLeaveBalance(string sEmployee_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT IFNULL(sum(a.NO_OF_DAYS),'0') as NO_OF_DAYS,b.LEAVE_TYPE_NAME " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE a  left outer join LEAVE_TYPE b " +
                                      " on a.LEAVE_TYPE_ID = b.LEAVE_TYPE_ID where a.EMPLOYEE_ID = '" + sEmployee_ID + "' group by b.LEAVE_TYPE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getEmployeeName(string EmployeeID)
        {
            string EmployeeName = String.Empty;
            try
            {
                mySqlCmd = new MySqlCommand();
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.CommandText = @"
                                                    SELECT 
                                                        CASE
                                                            WHEN
                                                                (TITLE IS NOT NULL
                                                                    AND INITIALS_NAME IS NOT NULL)
                                                            THEN
                                                                CONCAT(CONCAT(TITLE, ' '), INITIALS_NAME)
                                                            WHEN
                                                                (TITLE IS NULL
                                                                    AND INITIALS_NAME IS NOT NULL)
                                                            THEN
                                                                INITIALS_NAME
                                                            ELSE FULL_NAME
                                                        END AS 'NAME'
                                                    FROM
                                                        EMPLOYEE
                                                    WHERE
                                                        EMPLOYEE_ID = @EMPLOYEE_ID
                                        ";

                //EmployeeName = mySqlCmd.ExecuteScalar().ToString();

                EmployeeName = System.Convert.ToString(mySqlCmd.ExecuteScalar());
                return EmployeeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Parameters.Clear();
                EmployeeName = String.Empty;
            }
        }
    }
}
