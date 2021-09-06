using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class EmployeemarksStandardizationDataHandler : TemplateDataHandler
    {
        public DataTable PopulateCompanies()
        {
            try
            {
                mySqlCon.Open();

                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                string queryString = @"
                                            SELECT 
                                                COMPANY_ID, COMP_NAME
                                            FROM
                                                COMPANY
                                            WHERE
                                                STATUS_CODE = @STATUS_CODE 
                                        ";

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
                mySqlCon.Close();
            }
        }

        public DataTable PopulateDivisions(string DepartmentID)
        {
            try
            {
                mySqlCon.Open();

                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", DepartmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                string queryString = @"
                                            SELECT 
                                                DIVISION_ID, DIV_NAME
                                            FROM
                                                DIVISION
                                            WHERE
                                                DEPT_ID = @DEPT_ID
                                                    AND STATUS_CODE = @STATUS_CODE; 
                                        ";

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
                mySqlCon.Close();
            }
        }

        public DataTable PopulateAssessments(string CompanyID, string YearOfAssessment)
        {
            try
            {
                mySqlCon.Open();

                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS", Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSNEMT_CEO_FINALIZED_STATUS", Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));

                string queryString = @"
                                            SELECT 
                                                ASSESSMENT_ID, ASSESSMENT_NAME
                                            FROM
                                                ASSESSMENT
                                            WHERE
                                                COMPANY_ID = @COMPANY_ID
                                                    AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND (STATUS_CODE = @ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS OR STATUS_CODE = @ASSESSNEMT_CEO_FINALIZED_STATUS) 
                                            ORDER BY ASSESSMENT_ID DESC; 
                                        ";

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
                mySqlCon.Close();
            }
        }

        public DataTable PopulateEmployeeMarks(string CompanyID, string YearOfAssessment, string AssessmentID, string DepartmentID, string DivisionID)
        {
            try
            {
                mySqlCon.Open();

                dataTable = new DataTable();
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));

                string queryString = @"
                                            SELECT 
                                                AE.EMPLOYEE_ID,
                                                CASE
                                                    WHEN (E.TITLE IS NULL OR E.TITLE = '') THEN E.INITIALS_NAME
                                                    ELSE CONCAT(CONCAT(CONVERT( E.TITLE , CHAR), ' '),
                                                            E.INITIALS_NAME)
                                                END AS 'NAME',
                                                ED.DESIGNATION_NAME, 
                                                CONVERT(AE.COMPITANCY_SCORE,CHAR) AS 'COMPITANCY_SCORE',
                                                CONVERT(AE.KPI_SCORE,CHAR) AS 'KPI_SCORE',
                                                CONVERT(AE.TOTAL,CHAR) AS 'TOTAL', 
                                                CONVERT(AE.STANDARDIZED_TOTAL, CHAR) AS 'STANDARDIZED_TOTAL'
                                            FROM
                                                ASSESSED_EMPLOYEES AE,
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED
                                            WHERE
                                                AE.COMPANY = @COMPANY_ID 
                                                    AND AE.ASSESSMENT_ID = @ASSESSMENT_ID
                                                    AND AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                                    AND AE.EMPLOYEE_ID = E.EMPLOYEE_ID
                                                    AND E.DESIGNATION_ID = ED.DESIGNATION_ID                                                   
                                                    
                                        ";

                if (DepartmentID != String.Empty)
                {
                    queryString += @" AND E.DEPT_ID = @DEPT_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", DepartmentID));
                }

                if (DivisionID != String.Empty)
                {
                    queryString += @" AND E.DIVISION_ID = @DIVISION_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DIVISION_ID", DivisionID));
                }

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
                mySqlCon.Close();
            }
        }

        public void Update(DataTable EmployeeMarks, string AssessmentID, string YearOfAssessment, string ModifiedBy)
        {
            mySqlCon.Open();
            MySqlTransaction oMySqlTransaction = mySqlCon.BeginTransaction();
            try
            {                

                for (int i = 0; i < EmployeeMarks.Rows.Count; i++)
                {
                    mySqlCmd.Parameters.Clear();

                    string TotalMarks = EmployeeMarks.Rows[i]["TOTAL"].ToString().Trim();
                    string EmployeeID = EmployeeMarks.Rows[i]["EMPLOYEE_ID"].ToString().Trim();

                    mySqlCmd.CommandText = @"
                                                UPDATE ASSESSED_EMPLOYEES 
                                                SET 
                                                    STANDARDIZED_TOTAL = @TOTAL,
                                                    MODIFIED_BY = @MODIFIED_BY,
                                                    MODIFIED_DATE = NOW()
                                                WHERE
                                                    ASSESSMENT_ID = @ASSESSMENT_ID AND EMPLOYEE_ID = @EMPLOYEE_ID
                                                        AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                                            ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL", TotalMarks == "" ? (object)DBNull.Value : TotalMarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID == "" ? (object)DBNull.Value : AssessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment == "" ? (object)DBNull.Value : YearOfAssessment.Trim()));

                    mySqlCmd.ExecuteNonQuery();
                }

                oMySqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                oMySqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public void Update(string NewTotalMarks, string EmployeeID, string AssessmentID, string YearOfAssessment, string ModifiedBy)
        {
            mySqlCon.Open();
            MySqlTransaction oMySqlTransaction = mySqlCon.BeginTransaction();
            try
            {

                mySqlCmd.CommandText = @"
                                                UPDATE ASSESSED_EMPLOYEES 
                                                SET 
                                                    STANDARDIZED_TOTAL = @TOTAL,
                                                    MODIFIED_BY = @MODIFIED_BY,
                                                    MODIFIED_DATE = NOW()
                                                WHERE
                                                    ASSESSMENT_ID = @ASSESSMENT_ID AND EMPLOYEE_ID = @EMPLOYEE_ID
                                                        AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                                            ";
                mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL", NewTotalMarks == "" ? (object)DBNull.Value : NewTotalMarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID == "" ? (object)DBNull.Value : AssessmentID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment == "" ? (object)DBNull.Value : YearOfAssessment.Trim()));

                mySqlCmd.ExecuteNonQuery();

                oMySqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                oMySqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

    }
}
