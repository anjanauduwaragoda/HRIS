using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class EmployeeGoalDataHandler : TemplateDataHandler
    {
        public string getEmployeeName(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                TITLE, INITIALS_NAME
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + EmployeeID + @"';                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable.Rows[0]["TITLE"].ToString() + " " + dataTable.Rows[0]["INITIALS_NAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateGoalGroups()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                GOAL_GROUP_ID, GROUP_NAME
                                            FROM
                                                GOAL_GROUP
                                            WHERE
                                                STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateFinancialYears(string employeeID)
        {
            try
            {
                dataTable.Clear();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_ACTIVE_VALUE));

                string sMySqlString = @"
                                            
                                            SELECT 
                                                DISTINCT(YEAR_OF_GOAL) AS 'YEAR_OF_GOAL' 
                                            FROM 
                                                EMPLOYEE_GOALS 
                                            WHERE 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                STATUS_CODE = @STATUS_CODE 
                                            ORDER BY 
                                                YEAR_OF_GOAL DESC;                                           
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Insert(string EmployeeID, string yearOfGoal, string GoalGroup, string GoalArea, string Description, string Measurement, string Weight, string StatusCode, string AddedBy)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string EmployeeGoalID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    EmployeeGoalID = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_GOAL_ID_STAMP);


                        string Qry = @"
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
                                            @QuestionBankID,
                                            @GoalGroup,
                                            @EmployeeID,
                                            @yearOfGoal,
                                            @GoalArea,
                                            @Remarks,
                                            @Measurement,
                                            @Weight,
                                            @AssessmentStatusCode,
                                            @ModifiedBy,
                                            NOW(),
                                            @ModifiedBy,
                                            NOW()
                                        );

                                    ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@QuestionBankID", EmployeeGoalID.Trim() == "" ? (object)DBNull.Value : EmployeeGoalID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@yearOfGoal", yearOfGoal.Trim() == "" ? (object)DBNull.Value : yearOfGoal.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GoalGroup", GoalGroup.Trim() == "" ? (object)DBNull.Value : GoalGroup.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GoalArea", GoalArea.Trim() == "" ? (object)DBNull.Value : GoalArea.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Measurement", Measurement.Trim() == "" ? (object)DBNull.Value : Measurement.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Weight", Weight.Trim() == "" ? (object)DBNull.Value : Weight.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();

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

        public Boolean Insert(DataTable dtEmployeeGoals)
        {
            Boolean Status = false;

            try
            {
                

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string EmployeeGoalID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)
                    {

                        string EmployeeID = dtEmployeeGoals.Rows[i]["EmployeeID"].ToString();
                        string yearOfGoal = dtEmployeeGoals.Rows[i]["yearOfGoal"].ToString();
                        string GoalGroup = dtEmployeeGoals.Rows[i]["GoalGroup"].ToString();
                        string GoalArea = dtEmployeeGoals.Rows[i]["GoalArea"].ToString();
                        string Description = dtEmployeeGoals.Rows[i]["Remarks"].ToString();
                        string Measurement = dtEmployeeGoals.Rows[i]["Measurement"].ToString();
                        string Weight = dtEmployeeGoals.Rows[i]["Weight"].ToString();
                        string StatusCode = dtEmployeeGoals.Rows[i]["Status"].ToString();
                        string AddedBy = dtEmployeeGoals.Rows[i]["addedBy"].ToString();

                        mySqlCmd.Parameters.Clear();

                        EmployeeGoalID = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_GOAL_ID_STAMP);


                        string Qry = @"
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
                                            @QuestionBankID,
                                            @GoalGroup,
                                            @EmployeeID,
                                            @yearOfGoal,
                                            @GoalArea,
                                            @Remarks,
                                            @Measurement,
                                            @Weight,
                                            @AssessmentStatusCode,
                                            @ModifiedBy,
                                            NOW(),
                                            @ModifiedBy,
                                            NOW()
                                        );

                                    ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@QuestionBankID", EmployeeGoalID.Trim() == "" ? (object)DBNull.Value : EmployeeGoalID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@yearOfGoal", yearOfGoal.Trim() == "" ? (object)DBNull.Value : yearOfGoal.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GoalGroup", GoalGroup.Trim() == "" ? (object)DBNull.Value : GoalGroup.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GoalArea", GoalArea.Trim() == "" ? (object)DBNull.Value : GoalArea.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Measurement", Measurement.Trim() == "" ? (object)DBNull.Value : Measurement.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Weight", Weight.Trim() == "" ? (object)DBNull.Value : Weight.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();
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

        public DataTable Populate(string EmployeeID,string FinancialYear)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                EG.GOAL_ID,
                                                EG.GOAL_GROUP_ID,
                                                EG.EMPLOYEE_ID,
                                                EG.YEAR_OF_GOAL,
                                                EG.GOAL_AREA,
                                                EG.DESCRIPTION,
                                                EG.MEASUREMENTS,
                                                EG.WEIGHT,
                                                EG.STATUS_CODE,
                                                CASE 
                                                    WHEN 
                                                        EG.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"' 
                                                    THEN 
                                                        '" + Constants.STATUS_ACTIVE_TAG + @"' 
                                                    WHEN 
                                                        EG.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + @"' 
                                                    THEN 
                                                        '" + Constants.STATUS_INACTIVE_TAG + @"' 
                                                END AS 
                                                'STATUS_CODE_TEXT',
	                                            CONCAT(EG.GOAL_AREA,CONCAT('  <br/><b>[',CONCAT(GG.GROUP_NAME,']</b>'))) AS 'DISPLAY_GOAL_AREA',
	                                            EG.SUPERVISOR_AGREE,
	                                            EG.EMPLOYEE_AGREE
                                            FROM
                                                EMPLOYEE_GOALS EG,
                                                GOAL_GROUP GG
                                            WHERE
                                                GG.GOAL_GROUP_ID = EG.GOAL_GROUP_ID AND
                                                EG.EMPLOYEE_ID = '" + EmployeeID + @"' AND 
                                                EG.YEAR_OF_GOAL = '" + FinancialYear + @"';
                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string EmployeeAgreedStatus = dataTable.Rows[i]["EMPLOYEE_AGREE"].ToString();
                        string SupervisorAgreedStatus = dataTable.Rows[i]["SUPERVISOR_AGREE"].ToString();

                        if (EmployeeAgreedStatus == Constants.CON_ACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["EMPLOYEE_AGREE"] = "Agreed";
                        }
                        else if (EmployeeAgreedStatus == Constants.CON_INACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["EMPLOYEE_AGREE"] = "Disagreed";
                        }
                        else
                        {
                            dataTable.Rows[i]["EMPLOYEE_AGREE"] = "Pending";
                        }

                        if (SupervisorAgreedStatus == Constants.CON_ACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["SUPERVISOR_AGREE"] = "Agreed";
                        }
                        else if (SupervisorAgreedStatus == Constants.CON_INACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["SUPERVISOR_AGREE"] = "Disagreed";
                        }
                        else
                        {
                            dataTable.Rows[i]["SUPERVISOR_AGREE"] = "Pending";
                        }
                    }
                }
                
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Update(string EmployeeID, string yearOfGoal, string GoalGroup, string GoalArea, string Description, string Measurement, string Weight, string StatusCode, string AddedBy, string EmployeeGoalID)
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
                    mySqlCmd.Parameters.Clear();

                    string Qry = @"
                                        UPDATE 
                                            EMPLOYEE_GOALS
                                        SET
                                            GOAL_GROUP_ID = @GoalGroup,
                                            EMPLOYEE_ID = @EmployeeID,
                                            YEAR_OF_GOAL = @yearOfGoal,
                                            GOAL_AREA = @GoalArea,
                                            DESCRIPTION = @Remarks,
                                            MEASUREMENTS = @Measurement,
                                            WEIGHT = @Weight,
                                            STATUS_CODE = @AssessmentStatusCode,
                                            MODIFIED_BY = @ModifiedBy,
                                            MODIFIED_DATE = NOW()
                                        WHERE 
                                            GOAL_ID = @QuestionBankID;

                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@QuestionBankID", EmployeeGoalID.Trim() == "" ? (object)DBNull.Value : EmployeeGoalID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@yearOfGoal", yearOfGoal.Trim() == "" ? (object)DBNull.Value : yearOfGoal.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GoalGroup", GoalGroup.Trim() == "" ? (object)DBNull.Value : GoalGroup.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GoalArea", GoalArea.Trim() == "" ? (object)DBNull.Value : GoalArea.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Measurement", Measurement.Trim() == "" ? (object)DBNull.Value : Measurement.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Weight", Weight.Trim() == "" ? (object)DBNull.Value : Weight.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

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

        public double getCurrentTotalWeight(string EmployeeID, string yearOfGoal)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();


                string sMySqlString = @"
                                            SELECT 
                                                SUM(WEIGHT) AS 'WEIGHT'
                                            FROM
                                                EMPLOYEE_GOALS
                                            WHERE
                                                EMPLOYEE_ID = @EmployeeID
                                                    AND YEAR_OF_GOAL = @yearOfGoal
                                                        AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                            
                                        ";

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;


                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@yearOfGoal", yearOfGoal.Trim() == "" ? (object)DBNull.Value : yearOfGoal.Trim()));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                double weight = 0;
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["WEIGHT"].ToString() != "")
                    {
                        weight = Convert.ToDouble(dataTable.Rows[0]["WEIGHT"].ToString());
                    }
                    else
                    {
                        weight = 0;
                    }
                }

                return weight;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isPreviousGoalAvailable(string EmployeeID, string FinancialYear)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                *
                                            FROM
                                                EMPLOYEE_GOALS
                                            WHERE
                                                EMPLOYEE_ID = '" + EmployeeID + @"'
                                                    AND YEAR_OF_GOAL < '" + FinancialYear + @"';
                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isApprovedGoal(string GoalID)
        {
            Boolean Status = false;

            try
            {
                dataTable = new DataTable();

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection = mySqlCon;


                string sMySqlString = @"
                                            SELECT 
                                                *
                                            FROM
                                                EMPLOYEE_GOALS
                                            WHERE
                                                GOAL_ID = @GOAL_ID
                                                    AND IS_LOCKED = @IS_LOCKED;
                                            
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", GoalID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_LOCKED", Constants.CON_ACTIVE_STATUS));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Status;
        }

        public Boolean CheckGoalAreaExsistance(string EmployeeID, string YearOfGoal, string GoalArea)
        {
            Boolean isExsists = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

                mySqlCmd.Parameters.Clear();

                string queryStr = @"
                                    SELECT 
                                        *
                                    FROM
                                        EMPLOYEE_GOALS
                                    WHERE
                                        EMPLOYEE_ID = @EMPLOYEE_ID
                                            AND YEAR_OF_GOAL = @YEAR_OF_GOAL
                                            AND GOAL_AREA = @GOAL_AREA;
                                    ";

                mySqlCmd.CommandText = queryStr;

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_GOAL", YearOfGoal));
                mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_AREA", GoalArea));

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(mySqlCmd);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isExsists;
        }

        public Boolean CheckGoalAreaExsistance(string EmployeeID, string YearOfGoal, string GoalArea, string id)
        {

            dataTable = new DataTable();
            Boolean isExsists = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlCmd.Parameters.Clear();

                string queryStr = @"
                                        SELECT 
                                            *
                                        FROM
                                            EMPLOYEE_GOALS
                                        WHERE
                                            EMPLOYEE_ID = @EMPLOYEE_ID
                                                AND YEAR_OF_GOAL = @YEAR_OF_GOAL
                                                AND GOAL_AREA = @GOAL_AREA;
                                    ";
                mySqlCmd.CommandText = queryStr;
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_GOAL", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_AREA", EmployeeID));

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["GOAL_ID"].ToString() == id)
                        {
                            isExsists = false;
                        }
                        else
                        {
                            isExsists = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isExsists;
        }

        // --------------------------------------------------------------------------------------

        public DataTable PopulateYearofGoals(string EmployeeID, string FinancialYear)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT DISTINCT
                                                (YEAR_OF_GOAL) AS 'YEAR_OF_GOAL'
                                            FROM
                                                EMPLOYEE_GOALS
                                            WHERE
                                                YEAR_OF_GOAL < '" + FinancialYear + @"'
                                                    AND EMPLOYEE_ID = '" + EmployeeID + @"'
                                            ORDER BY YEAR_OF_GOAL DESC;                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
