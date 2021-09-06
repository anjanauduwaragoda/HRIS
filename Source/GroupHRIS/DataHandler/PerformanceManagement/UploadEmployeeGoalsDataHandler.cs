using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class UploadEmployeeGoalsDataHandler : TemplateDataHandler
    {
        public DataTable PopulateEmployeeDetails(DataTable EmployeeIDs)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                string Qry = @"
                                SELECT 
                                    E.EMPLOYEE_ID,
                                    C.COMP_NAME,
                                    E.EPF_NO,
                                    CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME'
                                FROM
                                    EMPLOYEE E,
                                    COMPANY C
                                WHERE
                                    E.COMPANY_ID = C.COMPANY_ID
                                AND  ( 
                            ";

                for (int i = 0; i < EmployeeIDs.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Qry += " E.EMPLOYEE_ID = '" + EmployeeIDs.Rows[i]["EMPLOYEE_ID"].ToString() + @"' ";
                    }
                    else
                    {
                        Qry += " OR E.EMPLOYEE_ID = '" + EmployeeIDs.Rows[i]["EMPLOYEE_ID"].ToString() + @"' ";
                    }
                }

                Qry += " ) ";

                mySqlCmd.CommandText = Qry;

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateGoalGorupDetails(DataTable GoalNames)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                string Qry = @"
                                SELECT 
                                    GOAL_GROUP_ID, GROUP_NAME, STATUS_CODE
                                FROM
                                    GOAL_GROUP
                                WHERE 
                            ";

                for (int i = 0; i < GoalNames.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Qry += " GROUP_NAME = '" + GoalNames.Rows[i]["GROUP_NAME"].ToString().Trim() + @"' ";
                    }
                    else
                    {
                        Qry += " OR GROUP_NAME = '" + GoalNames.Rows[i]["GROUP_NAME"].ToString().Trim() + @"' ";
                    }
                }

                mySqlCmd.CommandText = Qry;

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
        }

        public DataTable[] Insert(DataTable EmployeeGoals, string AddedBy)
        {
            mySqlCon.Open();
            MySqlDataReader reader = null;
            string EmployeeGoalID = String.Empty;
            SerialHandler serialHandler = new SerialHandler();
            DataTable dtSuccessRecords = new DataTable();
            DataTable dtFaildRecords = new DataTable();
            DataTable[] dtResponse = new DataTable[2];
            try
            {
                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();

                dtSuccessRecords.Columns.Add("YEAR_OF_GOAL");
                dtSuccessRecords.Columns.Add("EMPLOYEE_ID");
                dtSuccessRecords.Columns.Add("EPF_NO");
                dtSuccessRecords.Columns.Add("EMP_NAME");
                dtSuccessRecords.Columns.Add("COMP_NAME");

                dtFaildRecords.Columns.Add("YEAR_OF_GOAL");
                dtFaildRecords.Columns.Add("EMPLOYEE_ID");
                dtFaildRecords.Columns.Add("EPF_NO");
                dtFaildRecords.Columns.Add("EMP_NAME");
                dtFaildRecords.Columns.Add("COMP_NAME");
                dtFaildRecords.Columns.Add("FAILED_REASON");

                try
                {
                    for (int i = 0; i < EmployeeGoals.Rows.Count; i++)
                    {
                        string EmployeeID = EmployeeGoals.Rows[i]["EMPLOYEE_ID"].ToString();
                        string Year = EmployeeGoals.Rows[i]["YEAR_OF_GOAL"].ToString();
                        string GroupName = EmployeeGoals.Rows[i]["GROUP_NAME"].ToString();
                        string GoalArea = EmployeeGoals.Rows[i]["GOAL_AREA"].ToString();
                        string Description = EmployeeGoals.Rows[i]["DESCRIPTION"].ToString();
                        string Measurements = EmployeeGoals.Rows[i]["MEASUREMENTS"].ToString();
                        string Weight = EmployeeGoals.Rows[i]["WEIGHT"].ToString();
                        string InvalidYear = EmployeeGoals.Rows[i]["INVALID_YEAR"].ToString();
                        string InvalidWeight = EmployeeGoals.Rows[i]["INVALID_WEIGHT"].ToString();
                        string CompanyName = EmployeeGoals.Rows[i]["COMP_NAME"].ToString();
                        string EPFNumber = EmployeeGoals.Rows[i]["EPF_NO"].ToString();
                        string EmployeeName = EmployeeGoals.Rows[i]["EMP_NAME"].ToString();
                        string CumulativeWeight = EmployeeGoals.Rows[i]["CUM_WEIGHT"].ToString();
                        string InvalidCumulativeWeight = EmployeeGoals.Rows[i]["INVALID_CUM_WEIGHT"].ToString();
                        string GoalGroupID = EmployeeGoals.Rows[i]["GOAL_GROUP_ID"].ToString();
                        string StausCode = EmployeeGoals.Rows[i]["STATUS_CODE"].ToString();

                        //Check Record Existance
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_GOAL", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StausCode.Trim() == "" ? (object)DBNull.Value : StausCode.Trim()));

                        string CheckQuery = @"SELECT COUNT(*) AS 'RECORD_COUNT' FROM EMPLOYEE_GOALS WHERE EMPLOYEE_ID = @EMPLOYEE_ID AND YEAR_OF_GOAL = @YEAR_OF_GOAL AND STATUS_CODE = @STATUS_CODE;";
                        mySqlCmd.Connection = mySqlCon;
                        mySqlCmd.CommandText = CheckQuery;

                        reader = mySqlCmd.ExecuteReader();
                        int Count = 0;
                        if (reader.Read())
                        {
                            Count = Convert.ToInt32(reader.GetString("RECORD_COUNT"));
                            reader.Close();
                            reader.Dispose();
                        }
                        mySqlCmd.Parameters.Clear();
                        if (Count > 0)
                        {
                            DataRow drFail = dtFaildRecords.NewRow();
                            drFail["YEAR_OF_GOAL"] = Year;
                            drFail["EMPLOYEE_ID"] = EmployeeID;
                            drFail["EPF_NO"] = EPFNumber;
                            drFail["EMP_NAME"] = EmployeeName;
                            drFail["COMP_NAME"] = CompanyName;
                            drFail["FAILED_REASON"] = "Record(s) already exists";
                            dtFaildRecords.Rows.Add(drFail);
                        }
                        else
                        {
                            mySqlCmd.Parameters.Clear();

                            EmployeeGoalID = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_GOAL_ID_STAMP);
                            string InsertQry = @"
                                                    INSERT INTO EMPLOYEE_GOALS
                                                    (
                                                        GOAL_ID, 
                                                        GOAL_GROUP_ID, 
                                                        EMPLOYEE_ID, 
                                                        YEAR_OF_GOAL, 
                                                        GOAL_AREA, 
                                                        DESCRIPTION, 
                                                        MEASUREMENTS, 
                                                        WEIGHT, 
                                                        STATUS_CODE, 
                                                        ADDED_BY, 
                                                        ADDED_DATE, 
                                                        MODIFIED_BY, 
                                                        MODIFIED_DATE
                                                    ) 
                                                    VALUES
                                                    (
                                                        @GOAL_ID, 
                                                        @GOAL_GROUP_ID, 
                                                        @EMPLOYEE_ID, 
                                                        @YEAR_OF_GOAL, 
                                                        @GOAL_AREA, 
                                                        @DESCRIPTION, 
                                                        @MEASUREMENTS, 
                                                        @WEIGHT, 
                                                        @STATUS_CODE, 
                                                        @ADDED_BY, 
                                                        NOW(), 
                                                        @ADDED_BY, 
                                                        NOW()
                                                    );
                                                ";
                            mySqlCmd.CommandText = InsertQry;
                            mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", EmployeeGoalID.Trim() == "" ? (object)DBNull.Value : EmployeeGoalID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_GROUP_ID", GoalGroupID.Trim() == "" ? (object)DBNull.Value : GoalGroupID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_GOAL", Year.Trim() == "" ? (object)DBNull.Value : Year.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_AREA", GoalArea.Trim() == "" ? (object)DBNull.Value : GoalArea.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@MEASUREMENTS", Measurements.Trim() == "" ? (object)DBNull.Value : Measurements.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@WEIGHT", Weight.Trim() == "" ? (object)DBNull.Value : Weight.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StausCode.Trim() == "" ? (object)DBNull.Value : StausCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));


                            mySqlCmd.ExecuteNonQuery();

                            DataRow drSuccess = dtSuccessRecords.NewRow();
                            drSuccess["YEAR_OF_GOAL"] = Year;
                            drSuccess["EMPLOYEE_ID"] = EmployeeID;
                            drSuccess["EPF_NO"] = EPFNumber;
                            drSuccess["EMP_NAME"] = EmployeeName;
                            drSuccess["COMP_NAME"] = CompanyName;
                            dtSuccessRecords.Rows.Add(drSuccess);
                        }
                    }
                    oMySqlTransaction.Commit();
                    dtResponse[0] = dtSuccessRecords;
                    dtResponse[1] = dtFaildRecords;

                    return dtResponse;
                }
                catch (Exception e)
                {
                    oMySqlTransaction.Rollback();
                    throw e;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtSuccessRecords.Dispose();
                dtFaildRecords.Dispose();
                serialHandler = null;
                EmployeeGoalID = String.Empty;
                mySqlCon.Close();
                reader.Close();
                reader.Dispose();
            }
        }
    }
}
