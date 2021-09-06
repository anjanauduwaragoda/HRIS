using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Salary
{
    public class SalaryDetailDataHandler : TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                                SD.SALARY_ID,
                                                (SELECT COMPONENT_NAME FROM SALARY_COMPONENTS WHERE COMPONENT_ID=SD.COMPONENT_ID) as COMPONENT_NAME,
                                                SD.AMOUNT,
                                                SD.STATUS_CODE 
                                       FROM     SALARY_DETAIL SD;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (dataTable.Rows[i][3].ToString() == "1")
                    {
                        dataTable.Rows[i][3] = "Active";
                    }
                    else
                    {
                        dataTable.Rows[i][3] = "Inactive";
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Populate(string SalaryID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                //                string sMySqlString = @"SELECT  (SELECT COMPONENT_NAME FROM SALARY_COMPONENTS WHERE COMPONENT_ID=(SD.COMPONENT_ID)) AS COMPONENT_NAME,SD.AMOUNT,SD.STATUS_CODE,SD.ADDED_BY,DATE_FORMAT(ADDED_DATE, '%Y/%m/%d') AS ADDED_DATE,SD.MODIFIED_BY,SD.MODIFIED_DATE
                //                                        FROM    SALARY_DETAIL SD
                //                                        WHERE   SD.SALARY_ID=@SalaryID;";
                string sMySqlString = @"SELECT  (SELECT COMPONENT_NAME FROM SALARY_COMPONENTS WHERE COMPONENT_ID=(SD.COMPONENT_ID)) AS COMPONENT_NAME,SD.AMOUNT,SD.STATUS_CODE,SD.ADDED_BY,DATE_FORMAT(ADDED_DATE, '%Y-%m-%d %T') AS ADDED_DATE,SD.MODIFIED_BY,DATE_FORMAT(SD.MODIFIED_DATE, '%Y-%m-%d %T') AS MODIFIED_DATE
                                                        FROM    SALARY_DETAIL SD
                                                        WHERE   SD.SALARY_ID=@SalaryID;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (dataTable.Rows[i][2].ToString() == "1")
                    {
                        dataTable.Rows[i][2] = "Active";
                    }
                    else
                    {
                        dataTable.Rows[i][2] = "Inactive";
                    }
                }
                dataTable.Columns.Add("EDITED", typeof(string));
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateCheckList(string SalaryID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT  SC.COMPONENT_NAME,SD.AMOUNT,SD.STATUS_CODE 
                                        FROM    SALARY_DETAIL SD,SALARY_COMPONENTS SC
                                        WHERE   SD.SALARY_ID=@SalaryID AND SD.COMPONENT_ID=SC.COMPONENT_ID;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Insert(string SalaryID, string ComponentID, string Amount, string StatusCode, string AddedBy, MySqlConnection SQLConnection)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"INSERT INTO SALARY_DETAIL(
                                                            SALARY_ID,
                                                            COMPONENT_ID,
                                                            AMOUNT,
                                                            STATUS_CODE,
                                                            ADDED_BY,
                                                            ADDED_DATE,
                                                            MODIFIED_BY
                                                        ) 
                                                  VALUES(
                                                            @SalaryID,
                                                            @ComponentID,
                                                            @Amount,
                                                            @AssessmentStatusCode,
                                                            @ModifiedBy,
                                                            (SELECT now()),
                                                            @ModifiedBy
                                                        );";

                mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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

        public Boolean Update(string Amount, string StatusCode, string ModifiedBy, string SalaryID, string ComponentID)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE SALARY_DETAIL SET 
                                                            AMOUNT=@Amount,
                                                            STATUS_CODE=@AssessmentStatusCode,
                                                            MODIFIED_BY=@ModifiedBy,
                                                            MODIFIED_DATE=now() 
                                                    WHERE   SALARY_ID=@SalaryID 
                                                    AND     COMPONENT_ID=@ComponentID;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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
    }
}