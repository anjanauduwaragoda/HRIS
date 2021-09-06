using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
namespace DataHandler.PerformanceManagement
{
    public class AssessmentGroupDataHandler : TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                GROUP_ID, GROUP_NAME, DESCRIPTION, STATUS_CODE
                                            FROM
                                                ASSESSMENT_GROUP;                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                dataTable.Columns.Add("STATUS_CODE_VALUE");

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string Status = dataTable.Rows[i]["STATUS_CODE"].ToString();
                        if (Constants.STATUS_ACTIVE_VALUE == Status)
                        {
                            dataTable.Rows[i]["STATUS_CODE_VALUE"] = Constants.STATUS_ACTIVE_TAG;
                        }
                        else if (Constants.STATUS_INACTIVE_VALUE == Status)
                        {
                            dataTable.Rows[i]["STATUS_CODE_VALUE"] = Constants.STATUS_INACTIVE_TAG;
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

        public DataTable PopulateEmployeeRoles()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                ER.ROLE_ID,
                                                (SELECT AGR.GROUP_ID FROM ASSESSMENT_GROUP_ROLES AGR WHERE AGR.ROLE_ID = ER.ROLE_ID) AS 'GROUP_ID',
                                                (SELECT AG.GROUP_NAME FROM ASSESSMENT_GROUP AG, ASSESSMENT_GROUP_ROLES AGR  WHERE AG.GROUP_ID = AGR.GROUP_ID AND AGR.ROLE_ID = ER.ROLE_ID) AS 'GROUP_NAME',
                                                ER.ROLE_NAME,
                                                ER.DESCRIPTION
                                            FROM  
                                                EMPLOYEE_ROLE ER
                                            WHERE
                                                ER.STATUS = '" + Constants.CON_ACTIVE_STATUS + @"'
                                            ORDER BY
                                                ROLE_NAME;                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable.Copy();
            }
            finally
            {
                dataTable = null;
            }
        }

        public DataTable PopulateAssessmentGroupRoles()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                AGR.ROLE_ID, AGR.GROUP_ID, AGR.GROUP_ID, AG.GROUP_NAME
                                            FROM
                                                ASSESSMENT_GROUP_ROLES AGR,
                                                ASSESSMENT_GROUP AG
                                            WHERE
                                                AGR.GROUP_ID = AG.GROUP_ID;                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable.Copy();
            }
            finally
            {
                dataTable = null;
            }
        }

        public DataTable PopulateActiveCompetencyProfiles(string GroupID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Clear();


                string sMySqlString = @"
                                            SELECT 
                                                PROFILE_NAME 
                                            FROM 
                                                COMPETENCY_PROFILE 
                                            WHERE 
                                                GROUP_ID = @GROUP_ID AND 
                                                STATUS_CODE = @STATUS_CODE;                                            
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", GroupID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateActiveSelfAssessmentProfiles(string GroupID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Clear();


                string sMySqlString = @"
                                            SELECT 
                                                PROFILE_NAME 
                                            FROM 
                                                SELF_ASSESSMENT_PROFILE 
                                            WHERE 
                                                GROUP_ID = @GROUP_ID AND 
                                                STATUS_CODE = @STATUS_CODE;                                            
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", GroupID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isInactiveGroupAvailability(string RoleID)
        {
            Boolean Status = false;
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                AG.STATUS_CODE 
                                            FROM 
                                                ASSESSMENT_GROUP_ROLES AGR, 
                                                ASSESSMENT_GROUP AG 
                                            WHERE 
                                                AG.GROUP_ID = AGR.GROUP_ID AND 
                                                AGR.ROLE_ID = '" + RoleID + @"';                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    string status = dataTable.Rows[0]["STATUS_CODE"].ToString();

                    if (status == Constants.CON_ACTIVE_STATUS)
                    {
                        Status = false;
                    }
                    else
                    {
                        Status = true;
                    }
                }
                else
                {
                    Status = true;
                }
            }
            finally
            {
                dataTable = null;
            }
            return Status;
        }

        public Boolean Insert(string GroupName, string Description, string StatusCode, string AddedBy, DataRow[] EmployeeRoles)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string AssessmentGroupID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    AssessmentGroupID = serialHandler.getserila(mySqlCon, Constants.ASSESSMENT_GROUP_ID);


                    string Qry = @"
                                        INSERT INTO ASSESSMENT_GROUP
                                            (
                                                GROUP_ID,
                                                GROUP_NAME,
                                                DESCRIPTION,
                                                STATUS_CODE,
                                                ADDED_BY,
                                                ADDED_DATE,
                                                MODIFIED_BY,
                                                MODIFIED_DATE
                                            )
                                            VALUES
                                            (
                                                @GROUP_ID,
                                                @GROUP_NAME,
                                                @DESCRIPTION,
                                                @STATUS_CODE,
                                                @ADDED_BY,
                                                NOW(),
                                                @ADDED_BY,
                                                NOW()
                                            );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", AssessmentGroupID.Trim() == "" ? (object)DBNull.Value : AssessmentGroupID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_NAME", GroupName.Trim() == "" ? (object)DBNull.Value : GroupName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < EmployeeRoles.Length; i++)
                    {
                        string roleID = EmployeeRoles[i]["ROLE_ID"].ToString();
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ROLE_ID", roleID.Trim() == "" ? (object)DBNull.Value : roleID.Trim()));
                        Qry = @"DELETE FROM ASSESSMENT_GROUP_ROLES WHERE ROLE_ID = @ROLE_ID;";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }

                    for (int i = 0; i < EmployeeRoles.Length; i++)
                    {
                        string roleID = EmployeeRoles[i]["ROLE_ID"].ToString();
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", AssessmentGroupID.Trim() == "" ? (object)DBNull.Value : AssessmentGroupID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ROLE_ID", roleID.Trim() == "" ? (object)DBNull.Value : roleID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                        Qry = @"
                                    INSERT INTO 
                                        ASSESSMENT_GROUP_ROLES
                                            (GROUP_ID, ROLE_ID, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE) 
                                        VALUES
                                            (@GROUP_ID, @ROLE_ID, '" + Constants.CON_ACTIVE_STATUS + @"', @ADDED_BY, NOW(), @ADDED_BY, NOW())
                                ";
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

        public Boolean Update(string GroupName, string Description, string StatusCode, string ModifiedBy, string AssessmentGroupID, DataRow[] EmployeeRoles)
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
                                            ASSESSMENT_GROUP
                                        SET
                                                GROUP_NAME = @GROUP_NAME,
                                                DESCRIPTION = @DESCRIPTION,
                                                STATUS_CODE = @STATUS_CODE,
                                                MODIFIED_BY = @MODIFIED_BY,
                                                MODIFIED_DATE = NOW()
                                        WHERE
                                                GROUP_ID = @GROUP_ID;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", AssessmentGroupID.Trim() == "" ? (object)DBNull.Value : AssessmentGroupID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_NAME", GroupName.Trim() == "" ? (object)DBNull.Value : GroupName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", AssessmentGroupID.Trim() == "" ? (object)DBNull.Value : AssessmentGroupID.Trim()));
                    Qry = @"DELETE FROM ASSESSMENT_GROUP_ROLES WHERE GROUP_ID = @GROUP_ID;";
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();
                    if (StatusCode == Constants.CON_ACTIVE_STATUS)
                    {
                        for (int i = 0; i < EmployeeRoles.Length; i++)
                        {
                            string roleID = EmployeeRoles[i]["ROLE_ID"].ToString();
                            mySqlCmd.Parameters.Add(new MySqlParameter("@GROUP_ID", AssessmentGroupID.Trim() == "" ? (object)DBNull.Value : AssessmentGroupID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ROLE_ID", roleID.Trim() == "" ? (object)DBNull.Value : roleID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                            Qry = @"
                                    INSERT INTO 
                                        ASSESSMENT_GROUP_ROLES
                                            (GROUP_ID, ROLE_ID, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE) 
                                        VALUES
                                            (@GROUP_ID, @ROLE_ID, '" + Constants.CON_ACTIVE_STATUS + @"', @MODIFIED_BY, NOW(), @MODIFIED_BY, NOW())
                                ";
                            mySqlCmd.CommandText = Qry;
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

        public Boolean CheckAssessmentGroupNameExsistance(string groupName)
        {
            Boolean isExsists = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                string queryStr = "SELECT * FROM ASSESSMENT_GROUP WHERE GROUP_NAME ='" + groupName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
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

        public Boolean CheckAssessmentGroupNameExsistance(string groupName, string id)
        {

            dataTable = new DataTable();
            Boolean isExsists = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string queryStr = "SELECT * FROM ASSESSMENT_GROUP WHERE GROUP_NAME ='" + groupName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["GROUP_ID"].ToString() == id)
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

    }
}