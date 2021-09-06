using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Reports
{
    public class InOutConfigurationReportDataHandler : TemplateDataHandler
    {
        //All Company
        public DataTable populateAll(string FromDate, string ToDate)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID =  (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'COMP_NAME',
                                        (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID =(SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'BRANCH_NAME',
                                        (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID =(SELECT DEPT_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DEPT_NAME',
                                        (SELECT DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION WHERE DESIGNATION_ID =(SELECT DESIGNATION_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DESIGNATION_NAME',
                                        (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'EPF_NO',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.OFFICE_CODE) AS 'SUPERVISOR',
                                        CONVERT(AL.ATT_DATE, CHAR) AS 'ATT_DATE', CONVERT(AL.ATT_TIME, CHAR) AS 'ATT_TIME',
                                        CASE WHEN AL.DIRECTION = '1' THEN 'IN'  WHEN AL.DIRECTION = '0' THEN 'OUT' END AS DIRECTION,
                                        CASE 
                                         WHEN AL.STATUS_CODE = '0' THEN 'PENDING'  
                                         WHEN AL.STATUS_CODE = '1' THEN 'APPROVED'  
                                         WHEN AL.STATUS_CODE = '8' THEN 'CANCEL'   
                                         WHEN AL.STATUS_CODE = '9' THEN 'DISCARD' 
                                       END AS 'STATUS_CODE',
                                        AL.REASON,AL.REMARK AS 'REMARKS'
                                    FROM  
                                        ATTENDANCE_LOG AL
                                    WHERE 
                                        AL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        
        // Selected Company
        public DataTable populate(string FromDate, string ToDate, string CompanyID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID =  (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'COMP_NAME',
                                        (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID =(SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'BRANCH_NAME',
                                        (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID =(SELECT DEPT_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DEPT_NAME',
                                        (SELECT DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION WHERE DESIGNATION_ID =(SELECT DESIGNATION_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DESIGNATION_NAME',
                                        (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'EPF_NO',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.OFFICE_CODE) AS 'SUPERVISOR',
                                        CONVERT(AL.ATT_DATE, CHAR) AS 'ATT_DATE', CONVERT(AL.ATT_TIME, CHAR) AS 'ATT_TIME',
                                        CASE WHEN AL.DIRECTION = '1' THEN 'IN'  WHEN AL.DIRECTION = '0' THEN 'OUT' END AS DIRECTION,
                                        CASE 
                                         WHEN AL.STATUS_CODE = '0' THEN 'PENDING'  
                                         WHEN AL.STATUS_CODE = '1' THEN 'APPROVED'  
                                         WHEN AL.STATUS_CODE = '8' THEN 'CANCEL'   
                                         WHEN AL.STATUS_CODE = '9' THEN 'DISCARD' 
                                       END AS 'STATUS_CODE',
                                        AL.REASON,AL.REMARK AS 'REMARKS'
                                    FROM  
                                        ATTENDANCE_LOG AL
                                    WHERE 
                                        AL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND AL.COMPANY_ID = '" + CompanyID + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        // Selected Department
        public DataTable populateDep(string FromDate, string ToDate, string DepartmentID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID =  (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'COMP_NAME',
                                        (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID =(SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'BRANCH_NAME',
                                        (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID =(SELECT DEPT_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DEPT_NAME',
                                        (SELECT DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION WHERE DESIGNATION_ID =(SELECT DESIGNATION_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DESIGNATION_NAME',
                                        (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'EPF_NO',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.OFFICE_CODE) AS 'SUPERVISOR',
                                        CONVERT(AL.ATT_DATE, CHAR) AS 'ATT_DATE', CONVERT(AL.ATT_TIME, CHAR) AS 'ATT_TIME',
                                        CASE WHEN AL.DIRECTION = '1' THEN 'IN'  WHEN AL.DIRECTION = '0' THEN 'OUT' END AS DIRECTION,
                                        CASE 
                                         WHEN AL.STATUS_CODE = '0' THEN 'PENDING'  
                                         WHEN AL.STATUS_CODE = '1' THEN 'APPROVED'  
                                         WHEN AL.STATUS_CODE = '8' THEN 'CANCEL'   
                                         WHEN AL.STATUS_CODE = '9' THEN 'DISCARD' 
                                       END AS 'STATUS_CODE',
                                        AL.REASON,AL.REMARK AS 'REMARKS'
                                    FROM  
                                        ATTENDANCE_LOG AL, EMPLOYEE E
                                    WHERE 
                                        AL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND E.EMPLOYEE_ID = AL.EMPLOYEE_ID AND E.DEPT_ID = '" + DepartmentID + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        // Selected Employee
        public DataTable populateIND(string FromDate, string ToDate, string EmployeeID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID =  (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'COMP_NAME',
                                        (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID =(SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'BRANCH_NAME',
                                        (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID =(SELECT DEPT_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DEPT_NAME',
                                        (SELECT DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION WHERE DESIGNATION_ID =(SELECT DESIGNATION_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID)) AS 'DESIGNATION_NAME',
                                        (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'EPF_NO',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                        (SELECT INITIALS_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = AL.OFFICE_CODE) AS 'SUPERVISOR',
                                        CONVERT(AL.ATT_DATE, CHAR) AS 'ATT_DATE', CONVERT(AL.ATT_TIME, CHAR) AS 'ATT_TIME',
                                        CASE WHEN AL.DIRECTION = '1' THEN 'IN'  WHEN AL.DIRECTION = '0' THEN 'OUT' END AS DIRECTION,
                                        CASE 
                                         WHEN AL.STATUS_CODE = '0' THEN 'PENDING'  
                                         WHEN AL.STATUS_CODE = '1' THEN 'APPROVED'  
                                         WHEN AL.STATUS_CODE = '8' THEN 'CANCEL'   
                                         WHEN AL.STATUS_CODE = '9' THEN 'DISCARD' 
                                       END AS 'STATUS_CODE',
                                        AL.REASON,AL.REMARK AS 'REMARKS'
                                    FROM  
                                        ATTENDANCE_LOG AL, DEPARTMENT D
                                    WHERE 
                                        AL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND AL.EMPLOYEE_ID = '" + EmployeeID + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public DataTable populateCompany()
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                

                sMySqlString = @"
                                    SELECT COMPANY_ID,COMP_NAME FROM COMPANY;
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string employeeName(string employeeID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID ='" + employeeID + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["FULL_NAME"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateDepartments(string companyID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT DEPT_ID, DEPT_NAME FROM DEPARTMENT WHERE COMPANY_ID = '" + companyID + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
