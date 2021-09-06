using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class ViewEmployeeCompetencyProfileDataHandler : TemplateDataHandler
    {
        public DataTable getProfileCompetencies(string roleId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                PS.SCHEME_ID,
                                PS.SCHEME_NAME,
                                CP.COMPETENCY_PROFILE_ID,
                                CP.GROUP_ID,
                                CP.PROFICIENCY_SCHEME_ID,
                                CP.PROFILE_NAME,
                                AG.GROUP_NAME,
                                AGR.GROUP_ID,
                                AGR.ROLE_ID,
                                PL.RATING,
                                PL.WEIGHT,PL.REMARKS
                            FROM
                                PROFICIENCY_SCHEME PS,
                                PROFICIENCY_LEVELS PL,
                                COMPETENCY_PROFILE CP,
                                ASSESSMENT_GROUP AG,
                                ASSESSMENT_GROUP_ROLES AGR
                            WHERE
                                AGR.GROUP_ID = AG.GROUP_ID
                                    AND AG.GROUP_ID = CP.GROUP_ID
                                    AND CP.PROFICIENCY_SCHEME_ID = PS.SCHEME_ID
                                    AND PS.SCHEME_ID = PL.SCHEME_ID
                                    AND AGR.ROLE_ID = '" + roleId + "' AND CP.STATUS_CODE = '1'";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public String getEmpRole(string empId)
        {
            string role = "";

            mySqlCmd.CommandText = @"SELECT ROLE_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "'";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    role = mySqlCmd.ExecuteScalar().ToString();
                }
                mySqlCon.Close();
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return role;
        }

        public DataTable getExpectedProfileCompetencieRatings(string roleId)
        {
            try
            {
                //dataTable.Clear();
                DataTable dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    cp.PROFILE_NAME,
                                    pc.COMPETENCY_ID,cb.COMPETENCY_NAME,
                                    pc.EXPECTED_PROFICIENCY_RATING,
                                    pc.EXPECTED_PROFICIENCY_WEIGHT,cb.DESCRIPTION,cp.COMPETENCY_PROFILE_ID
    
                                FROM
                                    ASSESSMENT_GROUP_ROLES agr,
                                    ASSESSMENT_GROUP ag,
                                    COMPETENCY_PROFILE cp,
                                    PROFILE_COMPETENCIES pc,
                                    COMPETENCY_BANK cb
                                WHERE
                                    agr.ROLE_ID = '" + roleId + @"'
                                        AND ag.GROUP_ID = agr.GROUP_ID
                                        AND ag.GROUP_ID = cp.GROUP_ID
                                        AND pc.COMPETENCY_PROFILE_ID = cp.COMPETENCY_PROFILE_ID
                                        AND cb.COMPETENCY_ID = pc.COMPETENCY_ID
                                        AND ag.STATUS_CODE = '1' AND cp.STATUS_CODE = '1';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable getCompetencieLevel(string roleId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    PL.RATING,
                                    PL.WEIGHT
                                FROM
                                    PROFICIENCY_SCHEME PS,
                                    PROFICIENCY_LEVELS PL,
                                    COMPETENCY_PROFILE CP,
                                    ASSESSMENT_GROUP AG,
                                    ASSESSMENT_GROUP_ROLES AGR
                                WHERE
                                    AGR.GROUP_ID = AG.GROUP_ID
                                        AND AG.GROUP_ID = CP.GROUP_ID
                                        AND CP.PROFICIENCY_SCHEME_ID = PS.SCHEME_ID
                                        AND PS.SCHEME_ID = PL.SCHEME_ID
                                        AND AGR.ROLE_ID = '" + roleId + "' AND  CP.STATUS_CODE = '1'";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable getempDetails(string empId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                e.EMPLOYEE_ID, e.FULL_NAME, r.ROLE_NAME, c.COMP_NAME
                            FROM
                                EMPLOYEE e,
                                COMPANY c,
                                EMPLOYEE_ROLE r
                            WHERE
                                c.COMPANY_ID = e.COMPANY_ID
                                    AND r.ROLE_ID = e.ROLE_ID
                                    AND e.EMPLOYEE_ID = '" + empId + "';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public Boolean Insert(string compProId, string assId, string empId, string assYear, DataTable assmtAnswers)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                if (assmtAnswers.Rows.Count > 0)
                {
                    SerialHandler nserialcode = new SerialHandler();
                    string tokenId = nserialcode.getserila(mySqlCon, "CT");

                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@compProId", compProId.Trim() == "" ? (object)DBNull.Value : compProId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assId", assId.Trim() == "" ? (object)DBNull.Value : assId.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@tokenId", tokenId.Trim() == "" ? (object)DBNull.Value : tokenId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@user", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assYear", assYear.Trim() == "" ? (object)DBNull.Value : assYear.Trim()));

                    string qry_esa = @"INSERT INTO EMPLOYEE_COMPITANCY_ASSESSMENT(COMPETENCY_PROFILE_ID,ASSESSMENT_TOKEN,ASSESSMENT_ID,EMPLOYEE_ID,YEAR_OF_ASSESSMENT,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                  VALUES(@compProId,'" + tokenId + "',@assId,@user,@assYear,'" + Constants.CON_ACTIVE_STATUS + "',@user,now(),@user,now())";

                    mySqlCmd.CommandText = qry_esa;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in assmtAnswers.Rows)
                    {
                        string compId = dr["COMPETENCY_ID"].ToString();
                        string rating = dr["EMPLOYEE_RATING"].ToString();
                        string weight = dr["EMPLOYEE_WEIGHT"].ToString();

                        string Qry = @"INSERT INTO COMPETENCY_ASSESSMENT_DETAILS(ASSESSMENT_TOKEN,COMPETENCY_ID,EMPLOYEE_RATING,EMPLOYEE_WEIGHT) 
                                  VALUES('" + tokenId + "','" + compId + "','" + rating + "','" + weight + "')";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                    }


                    mySqlTrans.Commit();

                    mySqlTrans.Dispose();
                    mySqlCmd.Dispose();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public String getAssessmentToken(string assId, string yer, string empId)
        {
            string tkId = "";

            mySqlCmd.CommandText = @"SELECT 
                                    ASSESSMENT_TOKEN
                                FROM
                                    EMPLOYEE_COMPITANCY_ASSESSMENT
                                WHERE
                                    ASSESSMENT_ID = '" + assId + @"'
                                        AND YEAR_OF_ASSESSMENT = '" + yer + @"'
                                        AND EMPLOYEE_ID = '" + empId + "';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    tkId = mySqlCmd.ExecuteScalar().ToString();
                }
                mySqlCon.Close();
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return tkId;
        }

        public DataTable getAssessmentDetails(string tokenId)
        {
            try
            {
                DataTable dataTable = new DataTable();

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    COMPETENCY_ID, EMPLOYEE_RATING, EMPLOYEE_WEIGHT
                                FROM
                                    COMPETENCY_ASSESSMENT_DETAILS
                                WHERE
                                    ASSESSMENT_TOKEN = '" + tokenId + "';";

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public Boolean InsertifExist(string tokenId, DataTable assmtAnswers)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                if (assmtAnswers.Rows.Count > 0)
                {

                    string qry_esa = @"DELETE FROM COMPETENCY_ASSESSMENT_DETAILS 
                                        WHERE
                                            ASSESSMENT_TOKEN = '" + tokenId + "';";

                    mySqlCmd.CommandText = qry_esa;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in assmtAnswers.Rows)
                    {
                        string compId = dr["COMPETENCY_ID"].ToString();
                        string rating = dr["EMPLOYEE_RATING"].ToString();
                        string weight = dr["EMPLOYEE_WEIGHT"].ToString();

                        string Qry = @"INSERT INTO COMPETENCY_ASSESSMENT_DETAILS(ASSESSMENT_TOKEN,COMPETENCY_ID,EMPLOYEE_RATING,EMPLOYEE_WEIGHT) 
                                  VALUES('" + tokenId + "','" + compId + "','" + rating + "','" + weight + "')";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                    }


                    mySqlTrans.Commit();

                    mySqlTrans.Dispose();
                    mySqlCmd.Dispose();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public String getAssessmentStatus(string proId, string assId, string yer, string empId)
        {
            string AssStatus = "";

            mySqlCmd.CommandText = @"SELECT 
                                        STATUS_CODE
                                    FROM
                                        EMPLOYEE_COMPITANCY_ASSESSMENT
                                    WHERE
                                        COMPETENCY_PROFILE_ID = '" + proId + @"'
                                            AND ASSESSMENT_ID = '" + assId + @"'
                                            AND EMPLOYEE_ID = '" + empId + @"'
                                            AND YEAR_OF_ASSESSMENT = '" + yer + @"';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    AssStatus = mySqlCmd.ExecuteScalar().ToString();
                }
                mySqlCon.Close();
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return AssStatus;
        }

        public Boolean FinalizedifExist(string tokenId, double score, string user)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                string qry_esa = @"UPDATE EMPLOYEE_COMPITANCY_ASSESSMENT 
                                    SET 
                                        STATUS_CODE = '" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + @"',
                                        EMPLOYEE_COMPLETED_DATE = DATE(now()),
                                        TOTAL_SELF_SCORE = '" + score + @"',
                                        MODIFIED_BY = '" + user + @"',
                                        MODIFIED_DATE = now()
                                    WHERE
                                        ASSESSMENT_TOKEN = '" + tokenId + "';";

                mySqlCmd.CommandText = qry_esa;
                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                status = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public DataTable Populate(string role,string assId,string empId,string assYer)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();


                string sMySqlString = @"
                                
                                        SELECT 
                                            CP.COMPETENCY_PROFILE_ID,
                                            PC.COMPETENCY_ID,
                                            CB.COMPETENCY_NAME,
                                            CB.DESCRIPTION, 
                                            PC.EXPECTED_PROFICIENCY_RATING,
                                            PC.EXPECTED_PROFICIENCY_WEIGHT,PC.EXPECTED_PROFICIENCY_RATING AS SUPERVISOR_RATING
                                        FROM
                                            COMPETENCY_PROFILE CP,
                                            PROFILE_COMPETENCIES PC,
                                            COMPETENCY_BANK CB,
                                            ASSESSMENT_GROUP AG,
                                            ASSESSMENT_GROUP_ROLES AGR
                                        WHERE
                                            CP.COMPETENCY_PROFILE_ID = PC.COMPETENCY_PROFILE_ID
                                                AND PC.COMPETENCY_ID = CB.COMPETENCY_ID
                                                AND CP.GROUP_ID = AG.GROUP_ID
                                                AND AGR.GROUP_ID = AG.GROUP_ID
                                                AND AGR.ROLE_ID = '" + role +@"'
                                                AND CB.STATUS_CODE = '1'
                                        ORDER BY CB.COMPETENCY_NAME ASC; ";

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateRatings(string role)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                string sMySqlString = @"                                            
                                            SELECT 
                                                PL.RATING,
                                                PL.WEIGHT
                                            FROM
                                                PROFICIENCY_SCHEME PS,
                                                PROFICIENCY_LEVELS PL,
                                                COMPETENCY_PROFILE CP,
                                                ASSESSMENT_GROUP AG,
                                                ASSESSMENT_GROUP_ROLES AGR
                                            WHERE
                                                AGR.GROUP_ID = AG.GROUP_ID
                                                    AND AG.GROUP_ID = CP.GROUP_ID
                                                    AND CP.PROFICIENCY_SCHEME_ID = PS.SCHEME_ID
                                                    AND PS.SCHEME_ID = PL.SCHEME_ID
                                                    AND AGR.ROLE_ID = '" + role + @"'
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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable populateIfExist(string role, string tokenId)
        {
            try
            {

                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                string sMySqlString = @" SELECT 
                                            CP.COMPETENCY_PROFILE_ID,
                                            PC.COMPETENCY_ID,
                                            CB.COMPETENCY_NAME,
                                            CB.DESCRIPTION, 
                                            PC.EXPECTED_PROFICIENCY_RATING,
                                            PC.EXPECTED_PROFICIENCY_WEIGHT,
                                            ECA.ASSESSMENT_TOKEN,
                                            ECA.ASSESSMENT_TOKEN,
                                            CAD.EMPLOYEE_RATING,
                                            CAD.EMPLOYEE_WEIGHT,CAD.SUPERVISOR_RATING
                                        FROM
                                            COMPETENCY_PROFILE CP,
                                            PROFILE_COMPETENCIES PC,
                                            COMPETENCY_BANK CB,
                                            ASSESSMENT_GROUP AG,
                                            ASSESSMENT_GROUP_ROLES AGR,
                                            EMPLOYEE_COMPITANCY_ASSESSMENT ECA,
                                            COMPETENCY_ASSESSMENT_DETAILS CAD
                                        WHERE
                                            CP.COMPETENCY_PROFILE_ID = PC.COMPETENCY_PROFILE_ID
                                                AND PC.COMPETENCY_ID = CB.COMPETENCY_ID
                                                AND AGR.GROUP_ID = AG.GROUP_ID
                                                AND AGR.ROLE_ID = '" + role + @"'
                                                AND CB.STATUS_CODE = '1'
                                                AND ECA.COMPETENCY_PROFILE_ID = CP.COMPETENCY_PROFILE_ID
                                                AND CB.COMPETENCY_ID = CAD.COMPETENCY_ID
                                                AND ECA.ASSESSMENT_TOKEN = CAD.ASSESSMENT_TOKEN
                                                AND ECA.ASSESSMENT_TOKEN = '" + tokenId + @"'
                                        ORDER BY CB.COMPETENCY_NAME ASC; ";
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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

    }
}
