using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class CEODashboadDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string YearOfAssessment, string AssessmentStatusCode, string AssessmentID, string CEOCompanyID, string DepartmentID, string DivisionID, string SuperVisorEmpID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CEOCompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@REPORT_TO_3", SuperVisorEmpID));

                string queryString = @"
                                            SELECT 
                                                D.DEPT_NAME,
                                                DV.DIV_NAME,
                                                AE.ASSESSMENT_ID,
                                                E.EMPLOYEE_ID, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                AE.YEAR_OF_ASSESSMENT, 
                                                AE.INCLUDE_GOAL_ASSESSMENT,
                                                (SELECT EGA.STATUS_CODE FROM EMPLOYEE_GOAL_ASSESSMENT EGA WHERE EGA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND EGA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND EGA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'GOAL_ASSESSMENT_STATUS', 
                                                AE.INCLUDE_COMPITANCY_ASSESSMENT, 
                                                (SELECT ECA.STATUS_CODE FROM EMPLOYEE_COMPITANCY_ASSESSMENT ECA WHERE ECA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND ECA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND ECA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'COMPETENCY_ASSESSMENT_STATUS', 
                                                AE.INCLUDE_SELF_ASSESSMENT, 
                                                (SELECT ESA.STATUS_CODE FROM EMPLOYEE_SELF_ASSESSMENT ESA WHERE ESA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND ESA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND ESA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'SELF_ASSESSMENT_STATUS', 
                                                AE.STATUS_CODE,
                                                AE.IS_EMPLOYEE_AGREED  
                                            FROM  
                                                ASSESSED_EMPLOYEES AE, 
                                                EMPLOYEE E,
                                                ASSESSMENT A,
                                                DEPARTMENT D,
                                                DIVISION DV
                                            WHERE 
                                                E.REPORT_TO_3 = @REPORT_TO_3 AND
                                                E.DEPT_ID = D.DEPT_ID AND 
                                                E.DIVISION_ID = DV.DIVISION_ID AND 
                                                AE.EMPLOYEE_ID = E.EMPLOYEE_ID AND 
                                                AE.ASSESSMENT_ID = A.ASSESSMENT_ID AND 
                                                AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND 
                                                E.COMPANY_ID = @COMPANY_ID 
                                        ";
                if (AssessmentStatusCode != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", AssessmentStatusCode));
                    queryString += @" AND A.STATUS_CODE = @STATUS_CODE ";
                }
                if (AssessmentID != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                    queryString += @" AND AE.ASSESSMENT_ID = @ASSESSMENT_ID ";
                }
                if (DepartmentID != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", DepartmentID));
                    queryString += @" AND E.DEPT_ID = @DEPT_ID ";
                }
                if (DivisionID != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DIVISION_ID", DivisionID));
                    queryString += @" AND E.DIVISION_ID = @DIVISION_ID ";
                }

                queryString += @" ORDER BY D.DEPT_NAME ASC, DV.DIV_NAME ASC ";

                mySqlCmd.CommandText = queryString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateYears()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = "SELECT DISTINCT(YEAR_OF_ASSESSMENT) FROM ASSESSED_EMPLOYEES ORDER BY YEAR_OF_ASSESSMENT DESC;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateDepartments(string CompanyID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"
                                        SELECT 
                                            D.DEPT_ID, 
                                            D.DEPT_NAME 
                                        FROM 
                                            DEPARTMENT D 
                                        WHERE 
                                            D.COMPANY_ID = '" + CompanyID + @"'
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateDivisions(string DepartmentID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"
                                        SELECT 
                                            D.DIVISION_ID, 
                                            D.DIV_NAME
                                        FROM  
                                            DIVISION D
                                        WHERE 
                                            D.DEPT_ID = '" + DepartmentID + @"';
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateAssessments(string YearOfAssessment, string StatusCode, string Appraser)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

                string queryString = @"
                                        SELECT 
                                            DISTINCT(A.ASSESSMENT_ID) AS 'ASSESSMENT_ID', 
                                            A.ASSESSMENT_NAME 
                                        FROM 
                                            ASSESSMENT A,
                                            ASSESSED_EMPLOYEES AE,
                                            HRIS_USER HU
                                        WHERE 
                                            A.COMPANY_ID = HU.COMPANY_ID AND 
                                            HU.EMPLOYEE_ID = '" + Appraser + @"' AND
                                            A.ASSESSMENT_ID = AE.ASSESSMENT_ID AND
                                            A.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT AND
                                            A.STATUS_CODE = '" + StatusCode + @"' AND 
                                            A.YEAR_OF_ASSESSMENT = '" + YearOfAssessment + @"';
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public string GetSupervisorWorkingCompany(string EmployeeID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"
                                        SELECT 
                                            COMPANY_ID 
                                        FROM 
                                            HRIS_USER 
                                        WHERE 
                                            EMPLOYEE_ID = '" + EmployeeID + @"';
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COMPANY_ID"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }
    }
}