using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Reports
{
    public class LeaveBalanceReportDataHandler : TemplateDataHandler
    {
        //Report Data Tables
        public DataTable Populate(string Year)
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@Year", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));
                sMySqlString = @"
                                    SELECT 
                                                CP.COMP_NAME,
												E.FULL_NAME,
                                                LT.LEAVE_TYPE_NAME,
                                                CASE 
	                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
	                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
	                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
	                                                ELSE LSI.NUMBER_OF_DAYS
                                                END AS NUMBER_OF_DAYS,
                                                LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year) AS LEAVE_TAKEN,
                                                LEAVE_BALANCE(
				                                                CASE 
					                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
					                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
					                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
					                                                ELSE LSI.NUMBER_OF_DAYS
				                                                END, LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year)
                                                              ) AS BALANCE,EPF_NO,INITIALS_NAME,DEPT_NAME,BRANCH_NAME,DESIGNATION_NAME
                                    FROM 
                                                LEAVE_SCHEME_ITEM LSI, 
                                                EMPLOYEE_LEAVE_SCHEME ELS, 
                                                LEAVE_TYPE LT,
                                                EMPLOYEE E,
                                                DEPARTMENT d ,
                                                COMPANY_BRANCH cb ,
                                                EMPLOYEE_DESIGNATION ed,
												COMPANY CP,
                                                LEAVE_SCHEME LS
                                    WHERE 
                                                ELS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND 
                                                E.EMPLOYEE_ID = ELS.EMPLOYEE_ID AND 
                                                LSI.LEAVE_TYPE_ID = LT.LEAVE_TYPE_ID AND 
                                                E.DEPT_ID = d.DEPT_ID AND
                                                E.BRANCH_ID = cb.BRANCH_ID AND 
                                                E.DESIGNATION_ID = ed.DESIGNATION_ID AND
												E.COMPANY_ID = CP.COMPANY_ID AND
												cb.COMPANY_ID = CP.COMPANY_ID AND
												LS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND
                                                LS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND
                                                ELS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND E.EMPLOYEE_STATUS='S001' 
                                    ORDER BY  
                                                E.FULL_NAME ASC, LT.LEAVE_TYPE_NAME;
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

        public DataTable Populate(string EmployeeID, string Year)
        {
            string sMySqlString = "";

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Year", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT 
                                                CP.COMP_NAME,
												E.FULL_NAME,
                                                LT.LEAVE_TYPE_NAME,
                                                CASE 
	                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
	                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
	                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
	                                                ELSE LSI.NUMBER_OF_DAYS
                                                END AS NUMBER_OF_DAYS,
                                                LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year) AS LEAVE_TAKEN,
                                                LEAVE_BALANCE(
				                                                CASE 
					                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
					                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
					                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
					                                                ELSE LSI.NUMBER_OF_DAYS
				                                                END, LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year)
                                                              ) AS BALANCE,EPF_NO,INITIALS_NAME,DEPT_NAME,BRANCH_NAME,DESIGNATION_NAME
                                    FROM 
                                                LEAVE_SCHEME_ITEM LSI, 
                                                EMPLOYEE_LEAVE_SCHEME ELS, 
                                                LEAVE_TYPE LT,
                                                EMPLOYEE E,
                                                DEPARTMENT d ,
                                                COMPANY_BRANCH cb ,
                                                EMPLOYEE_DESIGNATION ed,
												COMPANY CP,
                                                LEAVE_SCHEME LS
                                    WHERE 
                                                ELS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND 
                                                E.EMPLOYEE_ID = ELS.EMPLOYEE_ID AND 
                                                LSI.LEAVE_TYPE_ID = LT.LEAVE_TYPE_ID AND 
                                                ELS.EMPLOYEE_ID = @EmployeeID AND
                                                E.DEPT_ID = d.DEPT_ID AND
                                                E.BRANCH_ID = cb.BRANCH_ID AND 
                                                E.DESIGNATION_ID = ed.DESIGNATION_ID AND
												E.COMPANY_ID = CP.COMPANY_ID AND
												cb.COMPANY_ID = CP.COMPANY_ID AND
												LS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND
                                                LS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND
                                                ELS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND E.EMPLOYEE_STATUS='S001' 
                                    ORDER BY  
                                                E.FULL_NAME ASC, LT.LEAVE_TYPE_NAME;
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

        public DataTable PopulateByCompany(string EmployeeID, string Year, string CompanyID)
        {
            string sMySqlString = "";

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Year", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyID", CompanyID.Trim() == "" ? (object)DBNull.Value : CompanyID.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT 
                                                CP.COMP_NAME,
												E.FULL_NAME,
                                                LT.LEAVE_TYPE_NAME,
                                                CASE 
	                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
	                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
	                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
	                                                ELSE LSI.NUMBER_OF_DAYS
                                                END AS NUMBER_OF_DAYS,
                                                LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year) AS LEAVE_TAKEN,
                                                LEAVE_BALANCE(
				                                                CASE 
					                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
					                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
					                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
					                                                ELSE LSI.NUMBER_OF_DAYS
				                                                END, LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year)
                                                              ) AS BALANCE,EPF_NO,INITIALS_NAME,DEPT_NAME,BRANCH_NAME,DESIGNATION_NAME
                                    FROM 
                                                LEAVE_SCHEME_ITEM LSI, 
                                                EMPLOYEE_LEAVE_SCHEME ELS, 
                                                LEAVE_TYPE LT,
                                                EMPLOYEE E,
                                                DEPARTMENT d ,
                                                COMPANY_BRANCH cb ,
                                                EMPLOYEE_DESIGNATION ed,
												COMPANY CP,
                                                LEAVE_SCHEME LS
                                    WHERE 
                                                ELS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND 
                                                E.EMPLOYEE_ID = ELS.EMPLOYEE_ID AND 
                                                LSI.LEAVE_TYPE_ID = LT.LEAVE_TYPE_ID AND 
                                                E.COMPANY_ID = @CompanyID AND
                                                E.DEPT_ID = d.DEPT_ID AND
                                                E.BRANCH_ID = cb.BRANCH_ID AND 
                                                E.DESIGNATION_ID = ed.DESIGNATION_ID AND
												E.COMPANY_ID = CP.COMPANY_ID AND
												cb.COMPANY_ID = CP.COMPANY_ID AND
												LS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND
                                                LS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND
                                                ELS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND E.EMPLOYEE_STATUS='S001' 
                                    ORDER BY 
                                                E.FULL_NAME ASC, LT.LEAVE_TYPE_NAME;
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

        public DataTable PopulateByDepartment(string EmployeeID, string Year, string DepartmentID)
        {
            string sMySqlString = "";

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Year", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DepartmentID", DepartmentID.Trim() == "" ? (object)DBNull.Value : DepartmentID.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT 
                                                CP.COMP_NAME,
												E.FULL_NAME,
                                                LT.LEAVE_TYPE_NAME,
                                                CASE 
	                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
	                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
	                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
	                                                ELSE LSI.NUMBER_OF_DAYS
                                                END AS NUMBER_OF_DAYS,
                                                LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year) AS LEAVE_TAKEN,
                                                LEAVE_BALANCE(
				                                                CASE 
					                                                WHEN LSI.LEAVE_TYPE_ID = 'ANNUAL' THEN ENTITLED_ANNUAL_LEAVES(ELS.EMPLOYEE_ID," + Year + @"," + Constants.CON_DAYS_ANNAM + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 + @"," + Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 + @")
					                                                WHEN LSI.LEAVE_TYPE_ID = 'CASUAL' THEN ENTITLED_CASUAL_LEAVES(ELS.EMPLOYEE_ID)
					                                                WHEN LSI.NUMBER_OF_DAYS IS NULL THEN 0
					                                                ELSE LSI.NUMBER_OF_DAYS
				                                                END, LEAVE_TAKEN(ELS.EMPLOYEE_ID, LSI.LEAVE_TYPE_ID, @Year)
                                                              ) AS BALANCE,EPF_NO,INITIALS_NAME,DEPT_NAME,BRANCH_NAME,DESIGNATION_NAME
                                    FROM 
                                                LEAVE_SCHEME_ITEM LSI, 
                                                EMPLOYEE_LEAVE_SCHEME ELS, 
                                                LEAVE_TYPE LT,
                                                EMPLOYEE E,
                                                DEPARTMENT d ,
                                                COMPANY_BRANCH cb ,
                                                EMPLOYEE_DESIGNATION ed,
												COMPANY CP,
                                                LEAVE_SCHEME LS
                                    WHERE 
                                                ELS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND 
                                                E.EMPLOYEE_ID = ELS.EMPLOYEE_ID AND 
                                                LSI.LEAVE_TYPE_ID = LT.LEAVE_TYPE_ID AND 
                                                E.DEPT_ID = @DepartmentID AND
                                                E.DEPT_ID = d.DEPT_ID AND
                                                E.BRANCH_ID = cb.BRANCH_ID AND 
                                                E.DESIGNATION_ID = ed.DESIGNATION_ID AND
												E.COMPANY_ID = CP.COMPANY_ID AND
												cb.COMPANY_ID = CP.COMPANY_ID AND
												LS.LEAVE_SCHEME_ID = LSI.LEAVE_SCHEME_ID AND
                                                LS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND
                                                ELS.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND E.EMPLOYEE_STATUS='S001' 
                                    ORDER BY  
                                                E.FULL_NAME ASC, LT.LEAVE_TYPE_NAME;
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
        //--
        public string GetEmployeeName(string EmployeeID)
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                sMySqlString = @"
                                    SELECT 
                                            FULL_NAME
                                    FROM 
                                            EMPLOYEE
                                    WHERE 
                                            EMPLOYEE_ID = @EmployeeID;
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                string EmployeeName = mySqlCmd.ExecuteScalar().ToString();

                return EmployeeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public string GetCompanyName(string EmployeeID)
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME
                                    FROM
                                        EMPLOYEE E,
                                        COMPANY C
                                    WHERE
                                        E.COMPANY_ID = C.COMPANY_ID
                                            AND E.EMPLOYEE_ID = @EmployeeID;
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter da = new MySqlDataAdapter(mySqlCmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                return dt.Rows[0]["COMP_NAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateCompany()
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            COMPANY_ID, 
                                            COMP_NAME 
                                    FROM 
                                            COMPANY;
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

        public DataTable PopulateDepartments(string CompanyID)
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyID", CompanyID.Trim() == "" ? (object)DBNull.Value : CompanyID.Trim()));
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            DEPT_ID, 
                                            DEPT_NAME 
                                    FROM 
                                            DEPARTMENT 
                                    WHERE 
                                            COMPANY_ID = @CompanyID;
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

        public DataTable PopulateDivisions(string DepartmentID)
        {
            string sMySqlString = "";

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@DepartmentID", DepartmentID.Trim() == "" ? (object)DBNull.Value : DepartmentID.Trim()));
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            DIVISION_ID, 
                                            DIV_NAME 
                                    FROM 
                                            DIVISION 
                                    WHERE 
                                            DEPT_ID = @DepartmentID;
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
