using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingRequestofTrainingDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                T.TRAINING_ID, 
                                                T.TRAINING_NAME, 
                                                T.TRAINING_CODE,
                                                TP.PROGRAM_NAME,
                                                TT.TYPE_NAME,
                                                T.PLANNED_PARTICIPANTS,
                                                CONVERT(T.PLANNED_START_DATE, CHAR) AS 'PLANNED_START_DATE',
                                                CONVERT(T.PLANNED_END_DATE, CHAR) AS 'PLANNED_END_DATE' 
                                            FROM 
                                                TRAINING T,
                                                TRAINING_PROGRAM TP,
                                                TRAINING_TYPE TT
                                            WHERE
                                                T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID AND
                                                T.TRAINING_TYPE = TT.TRAINING_TYPE_ID AND 
                                                T.TRAINING_ID = @TRAINING_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateTrainingCompanies(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TC.TRAINING_ID,
                                                TC.COMPANY_ID,
                                                C.COMP_NAME,
                                                TC.PLANNED_PARTICIPANTS
                                            FROM
                                                TRAINING_COMPANY TC,
                                                COMPANY C
                                            WHERE
                                                TC.COMPANY_ID = C.COMPANY_ID
                                                    AND TC.TRAINING_ID = @TRAINING_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateTrainingRequests(string TrainingRequestID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TR.REQUEST_ID, TR.DESCRIPTION_OF_TRAINING, TR.REMARKS
                                            FROM
                                                TRAINING_REQUEST TR
                                            WHERE
                                                TR.STATUS_CODE = @STATUS_CODE 
                                                    AND TR.REQUEST_ID = @REQUEST_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", TrainingRequestID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateTrainingRequest()
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                REQUEST_ID, 
                                                DESCRIPTION_OF_TRAINING  
                                            FROM 
                                                TRAINING_REQUEST 
                                            WHERE 
                                                IS_RECOMENDED = @IS_RECOMENDED AND 
                                                IS_APPROVED = @IS_APPROVED AND 
                                                STATUS_CODE = @STATUS_CODE AND 
                                                (INCLUDED_FOR_TRAINING = @INCLUDED_FOR_TRAINING OR INCLUDED_FOR_TRAINING IS NULL);                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_RECOMENDED", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_APPROVED", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@INCLUDED_FOR_TRAINING", Constants.CON_INACTIVE_STATUS));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable SearchTrainingRequest(string TrainingRequestID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                REQUEST_ID, DESCRIPTION_OF_TRAINING, REMARKS
                                            FROM
                                                TRAINING_REQUEST
                                            WHERE
                                                REQUEST_ID = @REQUEST_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", TrainingRequestID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateAssignedTrainingRequest(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TR.REQUEST_ID, 
                                                TR.DESCRIPTION_OF_TRAINING,
                                                TTR.DESCRIPTION AS 'REMARKS'
                                            FROM 
                                                TRAINING T,
                                                TRAINING_REQUEST TR,
                                                TRAINING_TRAINING_REQUESTS TTR
                                            WHERE
                                                T.TRAINING_ID = TTR.TRAINING_ID AND 
                                                TR.REQUEST_ID = TTR.TRAINING_REQUEST_ID AND 
                                                T.TRAINING_ID = @TRAINING_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateTrainingRequest(string TrainingRequestID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                REQUEST_ID, 
                                                DESCRIPTION_OF_TRAINING, 
                                                IS_RECOMENDED, 
                                                IS_APPROVED, 
                                                STATUS_CODE, 
                                                INCLUDED_FOR_TRAINING 
                                            FROM 
                                                TRAINING_REQUEST 
                                            WHERE 
                                                REQUEST_ID = @REQUEST_ID ;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", TrainingRequestID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateTrainingRequestDetails(string TrainingRequestID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TR.REQUEST_ID, 
                                                TC.CATEGORY_NAME, 
                                                TSC.TYPE_NAME, 
                                                C.COMP_NAME, 
                                                D.DEPT_NAME, 
                                                DV.DIV_NAME, 
                                                CB.BRANCH_NAME, 
                                                RT.TYPE_NAME, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                ED.DESIGNATION_NAME, 
                                                TR.EMAIL, 
                                                TR.REASON, 
                                                TR.DESCRIPTION_OF_TRAINING, 
                                                TR.SKILLS_EXPECTED, 
                                                TR.NUMBER_OF_PARTICIPANTS
                                            FROM 
                                                TRAINING_REQUEST TR, 
                                                TRAINING_CATEGORY TC, 
                                                TRAINING_SUB_CATEGORY TSC, 
                                                COMPANY C, 
                                                DEPARTMENT D, 
                                                DIVISION DV, 
                                                COMPANY_BRANCH CB,
                                                REQUEST_TYPE RT, 
                                                EMPLOYEE E, 
                                                EMPLOYEE_DESIGNATION ED
                                            WHERE
                                                TR.TRAINING_CATEGORY = TC.TRAINING_CATEGORY_ID AND 
                                                TR.TRAINING_SUB_CATEGORY_ID = TSC.TYPE_ID AND 
                                                TR.COMPANY_ID = C.COMPANY_ID AND 
                                                TR.DEPARTMENT_ID = D.DEPT_ID AND 
                                                TR.DIVISION_ID = DV.DIVISION_ID AND
                                                TR.BRANCH_ID = CB.BRANCH_ID AND 
                                                TR.REQUEST_TYPE = RT.REQUEST_TYPE_ID AND 
                                                TR.REQUESTED_BY = E.EMPLOYEE_ID AND
                                                TR.DESIGNATION = ED.DESIGNATION_ID AND 
                                                TR.REQUEST_ID = @REQUEST_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", TrainingRequestID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public Boolean Insert(string TrainingID ,DataTable dtTrainingRequests, string AddedBy)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < dtTrainingRequests.Rows.Count; i++)
                    {
                        string RequestID = dtTrainingRequests.Rows[i]["REQUEST_ID"].ToString();

                        string Qry = @" DELETE FROM TRAINING_TRAINING_REQUESTS WHERE TRAINING_ID = '" + RequestID + "'; ";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();

                        Qry = @" UPDATE TRAINING_REQUEST SET INCLUDED_FOR_TRAINING = '" + Constants.CON_INACTIVE_STATUS + "' WHERE REQUEST_ID = '" + RequestID + "' ";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }

                    for (int i = 0; i < dtTrainingRequests.Rows.Count; i++)
                    {
                        mySqlCmd.Parameters.Clear();

                        string InsertQry = @"
                                            INSERT INTO 
                                                TRAINING_TRAINING_REQUESTS
                                                    (
                                                        TRAINING_ID, 
                                                        TRAINING_REQUEST_ID, 
                                                        DESCRIPTION, 
                                                        STATUS_CODE, 
                                                        ADDED_BY, 
                                                        ADDED_DATE, 
                                                        MODIFIED_BY, 
                                                        MODIFIED_DATE
                                                    ) 
                                                    VALUES
                                                    (
                                                        @TRAINING_ID, 
                                                        @TRAINING_REQUEST_ID, 
                                                        @DESCRIPTION, 
                                                        @STATUS_CODE, 
                                                        @ADDED_BY, 
                                                        NOW(), 
                                                        @ADDED_BY, 
                                                        NOW()
                                                    )
                                           ";

                        string RequestID = dtTrainingRequests.Rows[i]["REQUEST_ID"].ToString();
                        string isExclude = dtTrainingRequests.Rows[i]["isExclude"].ToString();
                        string Remarks = dtTrainingRequests.Rows[i]["REMARKS"].ToString();

                        if (isExclude == Constants.CON_INACTIVE_STATUS)
                        {
                            mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_REQUEST_ID", RequestID.Trim() == "" ? (object)DBNull.Value : RequestID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_ACTIVE_STATUS.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));


                            mySqlCmd.CommandText = InsertQry;
                            mySqlCmd.ExecuteNonQuery();
                            mySqlCmd.Parameters.Clear();
                        }


                        if (isExclude == Constants.CON_INACTIVE_STATUS)
                        {
                            //mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                            //mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_REQUEST_ID", RequestID.Trim() == "" ? (object)DBNull.Value : RequestID.Trim()));
                            //mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                            //mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_ACTIVE_STATUS.Trim()));
                            //mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));


                            //mySqlCmd.CommandText = InsertQry;
                            //mySqlCmd.ExecuteNonQuery();
                            //mySqlCmd.Parameters.Clear();


                            mySqlCmd.Parameters.Add(new MySqlParameter("@INCLUDED_FOR_TRAINING", Constants.CON_ACTIVE_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_ACTIVE_STATUS.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", RequestID.Trim() == "" ? (object)DBNull.Value : RequestID.Trim()));
                            InsertQry = @"UPDATE TRAINING_REQUEST SET INCLUDED_FOR_TRAINING = @INCLUDED_FOR_TRAINING WHERE REQUEST_ID = @REQUEST_ID";

                            mySqlCmd.CommandText = InsertQry;
                            mySqlCmd.ExecuteNonQuery();
                            mySqlCmd.Parameters.Clear();


                        }
                        else
                        {
                            mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_REQUEST_ID", RequestID.Trim() == "" ? (object)DBNull.Value : RequestID.Trim()));
                            InsertQry = @"DELETE FROM TRAINING_TRAINING_REQUESTS WHERE TRAINING_REQUEST_ID = @TRAINING_REQUEST_ID";

                            mySqlCmd.CommandText = InsertQry;
                            mySqlCmd.ExecuteNonQuery();
                            mySqlCmd.Parameters.Clear();

                            mySqlCmd.Parameters.Add(new MySqlParameter("@INCLUDED_FOR_TRAINING", Constants.CON_INACTIVE_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_INACTIVE_STATUS.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@REQUEST_ID", RequestID.Trim() == "" ? (object)DBNull.Value : RequestID.Trim()));
                            InsertQry = @"UPDATE TRAINING_REQUEST SET INCLUDED_FOR_TRAINING = @INCLUDED_FOR_TRAINING WHERE REQUEST_ID = @REQUEST_ID";

                            mySqlCmd.CommandText = InsertQry;
                            mySqlCmd.ExecuteNonQuery();
                            mySqlCmd.Parameters.Clear();
                        }
                    }

                    oMySqlTransaction.Commit();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
                Status = true;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public DataTable PopulateSavedTrainingRequests(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TTR.TRAINING_REQUEST_ID AS 'REQUEST_ID',
                                                TTR.DESCRIPTION AS 'DESCRIPTION_OF_TRAINING',
                                                TR.REMARKS,
                                                '" + Constants.CON_INACTIVE_STATUS + @"' AS 'isExclude'
                                            FROM
                                                TRAINING_TRAINING_REQUESTS TTR,
                                                TRAINING_REQUEST TR
                                            WHERE
                                                TTR.TRAINING_REQUEST_ID = TR.REQUEST_ID
                                                    AND TTR.TRAINING_ID = @TRAINING_ID ;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
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
