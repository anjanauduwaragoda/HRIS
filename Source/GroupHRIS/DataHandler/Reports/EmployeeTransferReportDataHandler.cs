using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Reports
{
    public class EmployeeTransferReportDataHandler : TemplateDataHandler
    {
        public DataTable PopulateCompany()
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
                                            COMPANY_ID,
                                            COMP_NAME
                                    FROM 
                                            COMPANY
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

        public string getEmployeeName(string EmpID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmpID", EmpID));
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            FULL_NAME 
                                    FROM 
                                            EMPLOYEE 
                                    WHERE 
                                            EMPLOYEE_ID = @EmpID
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
                    return dataTable.Rows[0]["FULL_NAME"].ToString(); ;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataTable PopulateDepartment(string CompanyID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyID", CompanyID));
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            DEPT_ID,
                                            DEPT_NAME 
                                    FROM 
                                            DEPARTMENT 
                                    WHERE 
                                            COMPANY_ID = @CompanyID
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

        public DataTable PopulateDivision(string DepartmentID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                mySqlCmd.Parameters.Add(new MySqlParameter("@DepartmentID", DepartmentID));
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                            DIVISION_ID,
                                            DIV_NAME 
                                    FROM 
                                            DIVISION 
                                    WHERE 
                                            DEPT_ID = @DepartmentID
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

        public DataTable PopulateEmployee(string EmployeeID,string whereString)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                //mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID));
                sMySqlString = @"
                                    SELECT 
                                            E.INITIALS_NAME AS 'FULL_NAME',
                                            CF.COMP_NAME AS FCOMP_NAME,
                                            DF.DEPT_NAME AS FDEPT_NAME,
                                            DVF.DIV_NAME AS FDIV_NAME,
                                            ET.FROM_CC,
                                            ET.FROM_PC,
                                            ET.FROM_EPF,
                                            CF.EMPLOYER_EPF AS FEMPLOYER_EPF,
                                            CT.COMP_NAME AS TCOMP_NAME,
                                            DT.DEPT_NAME AS TDEPT_NAME,
                                            DVT.DIV_NAME AS TDIV_NAME,
                                            ET.TO_CC,
                                            ET.TO_PC,
                                            ET.TO_EPF,
                                            CT.EMPLOYER_EPF AS TEMPLOYER_EPF,
                                            CAST(DATE_FORMAT(ET.START_DATE, '%d/%m/%Y') as char) as START_DATE,
                                            CAST(DATE_FORMAT(E.DOJ, '%d/%m/%Y') as char) as DOJ,
											ET.FROM_RPT_1, ET.FROM_RPT_2, ET.FROM_RPT_3, ET.TO_RPT_1, ET.TO_RPT_2, ET.TO_RPT_3
                                        FROM
                                            EMPLOYEE_TRNSFERS ET,
                                            EMPLOYEE E,
                                            COMPANY CF,
                                            DEPARTMENT DF,
                                            DIVISION DVF,
                                            COMPANY CT,
                                            DEPARTMENT DT,
                                            DIVISION DVT
                                        WHERE
                                            ET.EMPLOYEE_ID = E.EMPLOYEE_ID
                                                AND CF.COMPANY_ID = ET.FROM_COMPANY_ID
                                                AND ET.FROM_DEPT_ID = DF.DEPT_ID
                                                AND ET.FROM_DIVISION_ID = DVF.DIVISION_ID
                                                AND ET.TO_COMPANY_ID = CT.COMPANY_ID
                                                AND ET.TO_DEPT_ID = DT.DEPT_ID
                                                AND ET.TO_DIVISION_ID = DVT.DIVISION_ID  
                                            " + whereString + @"
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
