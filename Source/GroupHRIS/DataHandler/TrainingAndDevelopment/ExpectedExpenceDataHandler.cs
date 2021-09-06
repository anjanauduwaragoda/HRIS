using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class ExpectedExpenceDataHandler : TemplateDataHandler
    {
        public DataTable getAllExpCategory()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    EXPENSE_CATEGORY_ID, CATEGORY_NAME
                                FROM
                                    EXPENSE_CATEGORY
                                WHERE
                                    STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

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

        public bool Insert(string trainingId, DataTable dtExpense, string totExpense,string expDescription,string costpPerson,string expRemark,string expStatus,string logUser)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                SerialHandler nserialcode = new SerialHandler();
                string sExExpense_ID = nserialcode.getserila(mySqlCon, "EE");
                int tot = 0; //Int32.Parse(txtAmount.Text);

                foreach (DataRow dr in dtExpense.Rows)
                {
                    if (dr["IS_EXCLUDE"].ToString().Trim() == Constants.CON_EXCLUDE_NO.ToString().Trim())
                    {
                        mySqlCmd.Parameters.Clear();
                        string expCatId = dr["EXPENSE_CATEGORY_ID"].ToString();
                        string descrip = dr["DESCRIPTION"].ToString();
                        string amount = dr["AMOUNT"].ToString();
                        string remark = dr["REMARKS"].ToString();
                        string stCode = dr["STATUS_CODE"].ToString();
                        tot = Int32.Parse(amount);

                        if (stCode == "Active")
                        {
                            stCode = "1";
                        }
                        else
                        {
                            stCode = "0";
                        }

                        mySqlCmd.Parameters.Add(new MySqlParameter("@expCatId", expCatId.Trim() == "" ? (object)DBNull.Value : expCatId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@descrip", descrip.Trim() == "" ? (object)DBNull.Value : descrip.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@amount", amount.Trim() == "" ? (object)DBNull.Value : amount.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@remark", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@stCode", stCode.Trim() == "" ? (object)DBNull.Value : stCode.Trim()));

                        sMySqlString = "INSERT INTO EXPECTED_EXPENSE_DETAILS(EX_EXPENSE_ID,EXPENSE_CATEGORY_ID,DESCRIPTION,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                                                        "VALUES('" + sExExpense_ID + "',@expCatId,@descrip,@amount,@remark,@stCode,'" + logUser + "',NOW(),'" + logUser + "',NOW())";

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                }

                string query = "INSERT INTO EXPECTED_EXPENSE(EXPECTED_EXPENSE_ID,TRAINING_ID,TOTAL_EXPENSE,DESCRIPTION,PER_PERSON_COST,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                "VALUES('" + sExExpense_ID + "','" + trainingId + "','" + tot + "','" + expDescription + "','" + costpPerson + "','" + expRemark + "','" + expStatus + "','" + logUser + "',NOW(),'" + logUser + "',NOW())";

                mySqlCmd.CommandText = query;
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

            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public bool Update(string sExExpense_ID,string trainingId, DataTable dtExpense, string totExpense, string expDescription, string costpPerson, string expRemark, string expStatus, string logUser)
        {
            Boolean isUpdate = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                string delQuery = @"DELETE FROM EXPECTED_EXPENSE_DETAILS WHERE EX_EXPENSE_ID = '" + sExExpense_ID + "';";
                mySqlCmd.CommandText = delQuery;
                mySqlCmd.ExecuteNonQuery();

                foreach (DataRow dr in dtExpense.Rows)
                {
                    if (dr["IS_EXCLUDE"].ToString().Trim() == Constants.CON_EXCLUDE_NO.ToString().Trim())
                    {
                        mySqlCmd.Parameters.Clear();
                        string expCatId = dr["EXPENSE_CATEGORY_ID"].ToString();
                        string descrip = dr["DESCRIPTION"].ToString();
                        string amount = dr["AMOUNT"].ToString();
                        string remark = dr["REMARKS"].ToString();
                        string stCode = dr["STATUS_CODE"].ToString();

                        if (stCode == "Active")
                        {
                            stCode = "1";
                        }
                        else
                        {
                            stCode = "0";
                        }
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sExExpense_ID", sExExpense_ID.Trim() == "" ? (object)DBNull.Value : sExExpense_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@expCatId", expCatId.Trim() == "" ? (object)DBNull.Value : expCatId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@descrip", descrip.Trim() == "" ? (object)DBNull.Value : descrip.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@amount", amount.Trim() == "" ? (object)DBNull.Value : amount.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@remark", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@stCode", stCode.Trim() == "" ? (object)DBNull.Value : stCode.Trim()));

//                        sMySqlString = @"UPDATE EXPECTED_EXPENSE_DETAILS SET EXPENSE_CATEGORY_ID= @expCatId,
//                                        DESCRIPTION = @descrip,
//                                        AMOUNT = @amount,
//                                        REMARKS = @remark,
//                                        STATUS_CODE = @status,
//                                        MODIFIED_BY = @user,
//                                        MODIFIED_DATE = now()
//                                        WHERE EX_EXPENSE_ID = @sExExpense_ID;";
                        sMySqlString = "INSERT INTO EXPECTED_EXPENSE_DETAILS(EX_EXPENSE_ID,EXPENSE_CATEGORY_ID,DESCRIPTION,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                                                        "VALUES('" + sExExpense_ID + "',@expCatId,@descrip,@amount,@remark,@stCode,'" + logUser + "',NOW(),'" + logUser + "',NOW())";


                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                }

                string query = @"UPDATE EXPECTED_EXPENSE SET TRAINING_ID= '"+ trainingId+ @"',
                                        TOTAL_EXPENSE = '"+ totExpense +@"',
                                        DESCRIPTION = '" + expDescription + @"',
                                        PER_PERSON_COST = '"+ costpPerson +@"',
                                        REMARKS = '" + expRemark + @"',STATUS_CODE = '"+ expStatus +@"',
                                        MODIFIED_BY = '" + logUser +@"',
                                        MODIFIED_DATE = now()
                                        WHERE EXPECTED_EXPENSE_ID = @sExExpense_ID;";

                mySqlCmd.CommandText = query;
                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdate = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }

            return isUpdate;
        }

        public DataTable getExpectedExpenses()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    e.EXPECTED_EXPENSE_ID,
                                    e.TRAINING_ID,
                                    t.TRAINING_NAME,
                                    e.TOTAL_EXPENSE,
                                    e.DESCRIPTION,
                                    e.PER_PERSON_COST,
                                    e.REMARKS,e.REMARKS,
                                    CASE
                                        WHEN e.STATUS_CODE = '1' THEN 'Active'
                                        WHEN e.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    EXPECTED_EXPENSE e,
                                    TRAINING t
                                WHERE
                                    e.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS +@"'
                                        AND e.TRAINING_ID = t.TRAINING_ID;";

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

        public DataTable getExpectedExpensesDetails(string expenseId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    e.EXPENSE_CATEGORY_ID,
                                    c.CATEGORY_NAME,
                                    e.DESCRIPTION,
                                    e.AMOUNT,
                                    e.REMARKS,
                                    CASE
                                    WHEN e.STATUS_CODE = '1' then 'Active'
                                    WHEN e.STATUS_CODE = '0' then 'Inactive'
                                End as STATUS_CODE,
                                    CASE
                                    WHEN e.STATUS_CODE = '1' then '0'
                                    WHEN e.STATUS_CODE = '0' then '1'
                                End as IS_EXCLUDE
                                FROM
                                    EXPECTED_EXPENSE_DETAILS e,
                                    EXPENSE_CATEGORY c
                                WHERE
                                    e.EX_EXPENSE_ID = '" + expenseId +@"'
                                        AND e.EXPENSE_CATEGORY_ID = c.EXPENSE_CATEGORY_ID;";

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

    }
}
