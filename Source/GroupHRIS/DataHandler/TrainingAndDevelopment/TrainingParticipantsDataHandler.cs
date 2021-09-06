using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingParticipantsDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'NAME',
                                                E.EPF_NO,
                                                E.DESIGNATION_ID,
                                                ED.DESIGNATION_NAME,
                                                E.DEPT_ID,
                                                D.DEPT_NAME,
                                                E.DIVISION_ID,
                                                DV.DIV_NAME,
                                                E.BRANCH_ID,
                                                CB.BRANCH_NAME,
                                                E.COMPANY_ID,
                                                C.COMP_NAME
                                            FROM
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED,
                                                DEPARTMENT D,
                                                DIVISION DV,
                                                COMPANY_BRANCH CB,
                                                COMPANY C
                                            WHERE
                                                E.COMPANY_ID = C.COMPANY_ID
                                                    AND E.BRANCH_ID = CB.BRANCH_ID
                                                    AND E.DIVISION_ID = DV.DIVISION_ID
                                                    AND E.DEPT_ID = D.DEPT_ID
                                                    AND E.DESIGNATION_ID = ED.DESIGNATION_ID
                                                    AND E.EMPLOYEE_ID = @EMPLOYEE_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateTrainings(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                T.TRAINING_NAME,
                                                T.TRAINING_CODE,
                                                TP.PROGRAM_NAME,
                                                TT.TYPE_NAME AS 'TRAINING_TYPE',
                                                T.PLANNED_PARTICIPANTS,
                                                CONVERT( T.PLANNED_START_DATE , CHAR) AS 'PLANNED_START_DATE',
                                                CONVERT( T.PLANNED_END_DATE , CHAR) AS 'PLANNED_END_DATE',
                                                T.PLANNED_TOTAL_HOURS,
                                                TT.TYPE_NAME,
                                                CASE
                                                    WHEN (T.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"') THEN '" + Constants.STATUS_ACTIVE_TAG + @"'
                                                    ELSE '" + Constants.STATUS_ACTIVE_TAG + @"'
                                                END AS 'STATUS_CODE'
                                            FROM
                                                TRAINING T,
                                                TRAINING_PROGRAM TP,
                                                TRAINING_TYPE TT
                                            WHERE
                                                T.TRAINING_TYPE = TT.TRAINING_TYPE_ID
                                                    AND T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID
                                                    AND T.TRAINING_ID = @TRAINING_ID                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateTrainingCompanies(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                TC.COMPANY_ID, C.COMP_NAME, TC.PLANNED_PARTICIPANTS
                                            FROM
                                                TRAINING_COMPANY TC,
                                                COMPANY C
                                            WHERE
                                                TC.COMPANY_ID = C.COMPANY_ID
                                                    AND TC.TRAINING_ID = @TRAINING_ID                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateCompanies()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                COMPANY_ID, COMP_NAME
                                            FROM
                                                COMPANY
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'                                               
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateDepartments(string CompanyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                DEPT_ID, DEPT_NAME
                                            FROM
                                                DEPARTMENT
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND COMPANY_ID = @COMPANY_ID                                                
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateDivisions(string DepartmentID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                DIVISION_ID, DIV_NAME
                                            FROM
                                                DIVISION
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND DEPT_ID = @DEPT_ID                                                
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", DepartmentID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateBranches(string CompanyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                BRANCH_ID, BRANCH_NAME
                                            FROM
                                                COMPANY_BRANCH
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' AND COMPANY_ID = @COMPANY_ID                                                
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateEmployees(string CompanyID, string DepartmentID, string DivisionID, string BranchID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                E.COMPANY_ID,
                                                E.EMPLOYEE_ID,
                                                CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMPLOYEE_NAME', 
                                                ED.DESIGNATION_NAME
                                            FROM
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED 
                                            WHERE 
                                                E.DESIGNATION_ID = ED.DESIGNATION_ID AND 
                                                E.EMPLOYEE_STATUS = @EMPLOYEE_STATUS                                                
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_STATUS", Constants.CON_EMPLOYEE_STATUS_ACTIVE));

                if (CompanyID != String.Empty)
                {
                    mySqlCmd.CommandText += " AND E.COMPANY_ID = @COMPANY_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                }

                if (DepartmentID != String.Empty)
                {
                    mySqlCmd.CommandText += " AND E.DEPT_ID = @DEPT_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", DepartmentID));
                }

                if (DivisionID != String.Empty)
                {
                    mySqlCmd.CommandText += " AND E.DIVISION_ID = @DIVISION_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DIVISION_ID", DivisionID));
                }

                if (BranchID != String.Empty)
                {
                    mySqlCmd.CommandText += " AND E.BRANCH_ID = @BRANCH_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@BRANCH_ID", BranchID));
                }

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean Insert(string TrainingID, string[] Employees, String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                sMySqlString = @" DELETE FROM TRAINING_PARTICIPANTS WHERE TRAINING_ID = '" + TrainingID + @"'";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                for (int i = 0; i < Employees.Length; i++)
                {

                    sMySqlString = @"
                                            SELECT 
                                                            '" + TrainingID + @"' AS 'TRAINING_ID', 
                                                            E.COMPANY_ID, 
                                                            E.EMPLOYEE_ID, 
                                                            E.EPF_NO, 
                                                            E.DESIGNATION_ID, 
                                                            E.DEPT_ID, 
                                                            E.DIVISION_ID, 
                                                            CASE WHEN (E.BRANCH_ID IS NULL) THEN '' ELSE E.BRANCH_ID END AS 'BRANCH_ID', 
                                                            CASE WHEN (E.REPORT_TO_1 IS NULL) THEN '' ELSE E.REPORT_TO_1 END AS 'REPORT_TO_1', 
                                                            '" + Constants.CON_ACTIVE_STATUS + @"' AS 'STATUS_CODE', 
                                                            '" + addedBy + @"' AS 'ADDED_BY', 
                                                            NOW() AS 'ADDED_DATE', 
                                                            '" + addedBy + @"' AS 'MODIFIED_BY', 
                                                            NOW() AS 'MODIFIED_DATE'
                                                        FROM
                                                            EMPLOYEE E  
                                                        WHERE 
                                                            E.EMPLOYEE_ID = '" + Employees[i].Trim() + @"'
                                    ";
                    mySqlCmd.CommandText = sMySqlString;
                    MySqlDataReader rdr = mySqlCmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", rdr.GetString("TRAINING_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("TRAINING_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", rdr.GetString("COMPANY_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("COMPANY_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", rdr.GetString("EMPLOYEE_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("EMPLOYEE_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EPF_NO", rdr.GetString("EPF_NO").Trim() == "" ? (object)DBNull.Value : rdr.GetString("EPF_NO").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DESIGNATION_ID", rdr.GetString("DESIGNATION_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("DESIGNATION_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DEPT_ID", rdr.GetString("DEPT_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("DEPT_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DIVISION_ID", rdr.GetString("DIVISION_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("DIVISION_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@BRANCH_ID", rdr.GetString("BRANCH_ID").Trim() == "" ? (object)DBNull.Value : rdr.GetString("BRANCH_ID").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@REPORT_TO_1", rdr.GetString("REPORT_TO_1").Trim() == "" ? (object)DBNull.Value : rdr.GetString("REPORT_TO_1").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", rdr.GetString("STATUS_CODE").Trim() == "" ? (object)DBNull.Value : rdr.GetString("STATUS_CODE").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", rdr.GetString("ADDED_BY").Trim() == "" ? (object)DBNull.Value : rdr.GetString("ADDED_BY").Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", rdr.GetString("MODIFIED_BY").Trim() == "" ? (object)DBNull.Value : rdr.GetString("MODIFIED_BY").Trim()));
                        
                        sMySqlString = @"
                                        
                                            INSERT INTO 
                                                TRAINING_PARTICIPANTS
                                                (
                                                    TRAINING_ID, 
                                                    COMPANY_ID, 
                                                    EMPLOYEE_ID, 
                                                    EPF, 
                                                    DESIGNATION, 
                                                    DEPARTMENT, 
                                                    DIVISION, 
                                                    BRANCH, 
                                                    REPORTING_HEAD, 
                                                    STATUS_CODE, 
                                                    ADDED_BY, 
                                                    ADDED_DATE, 
                                                    MODIFIED_BY, 
                                                    MODIFIED_DATE
                                                )
                                                VALUES
                                                (
                                                    @TRAINING_ID,
                                                    @COMPANY_ID,
                                                    @EMPLOYEE_ID,
                                                    @EPF_NO,
                                                    @DESIGNATION_ID,
                                                    @DEPT_ID,
                                                    @DIVISION_ID,
                                                    @BRANCH_ID,
                                                    @REPORT_TO_1,
                                                    @STATUS_CODE,
                                                    @ADDED_BY,
                                                    NOW(),
                                                    @MODIFIED_BY,
                                                    NOW()
                                                );

                                    ";

                        rdr.Close();

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }
                    
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {

            }

            return blInserted;
        }

        public DataTable PopulateTrainingParticipants(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT
                                                TP.COMPANY_ID, 
                                                TP.EMPLOYEE_ID, 
                                                CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                                C.COMP_NAME,
                                                TP.EPF,
                                                ED.DESIGNATION_NAME
                                            FROM
                                                TRAINING_PARTICIPANTS TP,
                                                COMPANY C,
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED
                                            WHERE
                                                TP.DESIGNATION = ED.DESIGNATION_ID
                                                    AND TP.EMPLOYEE_ID = E.EMPLOYEE_ID
                                                    AND TP.COMPANY_ID = C.COMPANY_ID
                                                    AND TP.TRAINING_ID = @TRAINING_ID
                                            ORDER BY C.COMP_NAME ASC , TP.EPF ASC , ED.DESIGNATION_NAME ASC                                          
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }
    }
}
