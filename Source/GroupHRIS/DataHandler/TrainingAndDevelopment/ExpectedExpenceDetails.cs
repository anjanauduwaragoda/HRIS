using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class ExpectedExpenceDetails : TemplateDataHandler
    {
        public DataTable getExpectedExpenses(string trainingId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    tc.TRAINING_ID,
                                    tc.COMPANY_ID,
                                    c.COMP_NAME,
                                    tc.PLANNED_PARTICIPANTS
                                FROM
                                    TRAINING_COMPANY tc,
                                    TRAINING t,
                                    COMPANY c
                                WHERE
                                    tc.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND t.TRAINING_ID = tc.TRAINING_ID
                                        AND t.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND tc.COMPANY_ID = c.COMPANY_ID AND tc.TRAINING_ID = '" + trainingId + "';";

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
        
        public DataTable getExpectedExpensesDetails(string trainingId, int count, double amount)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT 
                                    tc.TRAINING_ID,
                                    tc.COMPANY_ID,
                                    c.COMP_NAME,
                                    tc.PLANNED_PARTICIPANTS
                                FROM
                                    TRAINING_COMPANY tc,
                                    TRAINING t,
                                    COMPANY c
                                WHERE
                                    tc.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND t.TRAINING_ID = tc.TRAINING_ID
                                        AND t.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND tc.COMPANY_ID = c.COMPANY_ID AND tc.TRAINING_ID = '" + trainingId + "';";

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

        public DataTable getExpectedExpensesDetails(string TrId)
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
                                    End as STATUS_CODE,e.RECORD_ID
                                FROM
                                    EXPECTED_EXPENSE_DETAILS e,
                                    EXPENSE_CATEGORY c,
                                    EXPECTED_EXPENSE ee
                                WHERE
                                    e.EX_EXPENSE_ID = ee.EXPECTED_EXPENSE_ID
                                        AND ee.TRAINING_ID = '" + TrId + @"'
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

        public String isExistTraining(string trId)
        {
            string expId = "";

            mySqlCmd.CommandText = @"SELECT 
                                        EXPECTED_EXPENSE_ID
                                    FROM
                                        EXPECTED_EXPENSE
                                    WHERE
                                        TRAINING_ID = '" + trId + "';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    expId = mySqlCmd.ExecuteScalar().ToString();
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

            return expId;
        }

        public DataTable getExpenceDetails(string trId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                EXPECTED_EXPENSE_ID,DESCRIPTION,REMARKS,STATUS_CODE
                            FROM
                                EXPECTED_EXPENSE
                            WHERE
                                TRAINING_ID = '" + trId + "';";

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

        public DataTable getCompanyWiseExpenceDetails(string recId, string trainingId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ce.RECORD_ID,
                                    ce.EX_EXPENSE_ID,
                                    ce.COMPANY_ID,
                                    c.COMP_NAME,tc.PLANNED_PARTICIPANTS,
                                    ce.AMOUNT,
                                    ce.REMARKS,
                                    CASE
                                        WHEN ce.STATUS_CODE = '1' THEN 'Active'
                                        WHEN ce.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    COMPANY_WISE_EXPECTED_EXPENSE ce,
                                    COMPANY c,
                                    TRAINING_COMPANY tc
                                WHERE
                                    c.COMPANY_ID = ce.COMPANY_ID
                                        AND ce.RECORD_ID = '" + recId + @"'
                                        AND tc.COMPANY_ID = c.COMPANY_ID
                                        AND tc.TRAINING_ID = '" + trainingId + "';";

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

        //public bool Insert(string trainingId, string expDescription, string expStatus, string remark, string perPerson, string totcost, string logUser)
        //{
        //    Boolean status = false;
        //    MySqlTransaction mySqlTrans = null;

        //    try
        //    {
        //        mySqlCon.Open();
        //        mySqlTrans = mySqlCon.BeginTransaction();
        //        mySqlCmd.Transaction = mySqlTrans;

        //        SerialHandler nserialcode = new SerialHandler();
        //        string sExExpense_ID = nserialcode.getserila(mySqlCon, "EE");

        //        string query = "INSERT INTO EXPECTED_EXPENSE(EXPECTED_EXPENSE_ID,TRAINING_ID,TOTAL_EXPENSE,DESCRIPTION,PER_PERSON_COST,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
        //                        "VALUES('" + sExExpense_ID + "','" + trainingId + "','" + totcost + "','" + expDescription + "','" + perPerson + "','" + remark + "','" + expStatus + "','" + logUser + "',NOW(),'" + logUser + "',NOW())";

        //        mySqlCmd.CommandText = query;
        //        mySqlCmd.ExecuteNonQuery();

        //        mySqlTrans.Commit();

        //        mySqlTrans.Dispose();
        //        mySqlCmd.Dispose();


        //        status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //            mySqlCmd.Transaction.Rollback();
        //        }

        //    }
        //    finally
        //    {
        //        mySqlCon.Close();
        //    }
        //    return status;
        //}

//        public bool Update(string sExExpense_ID, string trainingId, string expDescription, string expStatus, string remark, string perPerson, string totcost, string logUser)
//        {
//            Boolean isUpdate = false;
//            MySqlTransaction mySqlTrans = null;

//            try
//            {
//                mySqlCon.Open();
//                mySqlTrans = mySqlCon.BeginTransaction();
//                mySqlCmd.Transaction = mySqlTrans;


//                string query = @"UPDATE EXPECTED_EXPENSE SET TRAINING_ID= '" + trainingId + @"',
//                                        TOTAL_EXPENSE = '" + totcost + @"',
//                                        DESCRIPTION = '" + expDescription + @"',
//                                        PER_PERSON_COST = '" + perPerson + @"',
//                                        REMARKS = '" + remark + @"',STATUS_CODE = '" + expStatus + @"',
//                                        MODIFIED_BY = '" + logUser + @"',
//                                        MODIFIED_DATE = now()
//                                        WHERE EXPECTED_EXPENSE_ID = '" + sExExpense_ID + "';";

//                mySqlCmd.CommandText = query;
//                mySqlCmd.ExecuteNonQuery();

//                mySqlTrans.Commit();

//                mySqlCon.Close();
//                mySqlTrans.Dispose();
//                mySqlCmd.Dispose();

//                isUpdate = true;

//            }
//            catch (Exception ex)
//            {
//                if (mySqlCon.State == ConnectionState.Open)
//                {
//                    mySqlCon.Close();
//                }

//                throw ex;
//            }

//            return isUpdate;
//        }

        public bool InsertDetails(string trainingId, string expDescriptiond, string expStatusx, string remarkx, string perPerson, string totcost, DataTable dtCompanyExpense, string expCategoryId, string expDescription, string expRemark, string cost, string expStatus, string logUser)
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
                string sExExpenseRecord_ID = nserialcode.getserila(mySqlCon, "CE");
                int tot = 0; //Int32.Parse(txtAmount.Text);

                //insert inintial expense
                string sExExpense_ID = nserialcode.getserila(mySqlCon, "EE");

                string query = "INSERT INTO EXPECTED_EXPENSE(EXPECTED_EXPENSE_ID,TRAINING_ID,TOTAL_EXPENSE,DESCRIPTION,PER_PERSON_COST,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                "VALUES('" + sExExpense_ID + "','" + trainingId + "','" + totcost + "','" + expDescriptiond + "','" + perPerson + "','" + remarkx + "','" + expStatusx + "','" + logUser + "',NOW(),'" + logUser + "',NOW())";

                mySqlCmd.CommandText = query;
                mySqlCmd.ExecuteNonQuery();

                foreach (DataRow dr in dtCompanyExpense.Rows)
                {

                    mySqlCmd.Parameters.Clear();
                    string companyId = dr["COMPANY_ID"].ToString();
                    string compName = dr["COMP_NAME"].ToString();
                    string participantCount = dr["PLANNED_PARTICIPANTS"].ToString();
                    string remark = dr["REMARK"].ToString();
                    string amount = dr["AMOUNT"].ToString();
                    string stCode = dr["STATUS_CODE"].ToString();
                    tot = Int32.Parse(amount);

                    mySqlCmd.Parameters.Add(new MySqlParameter("@expectedExId", sExExpense_ID.Trim() == "" ? (object)DBNull.Value : sExExpense_ID.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@participantCount", participantCount.Trim() == "" ? (object)DBNull.Value : participantCount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@amount", amount.Trim() == "" ? (object)DBNull.Value : amount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@remark", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@stCode", stCode.Trim() == "" ? (object)DBNull.Value : stCode.Trim()));

                    //Insert to Company Expense table
                    sMySqlString = @"INSERT INTO COMPANY_WISE_EXPECTED_EXPENSE(RECORD_ID,EX_EXPENSE_ID,COMPANY_ID,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                        VALUE('" + sExExpenseRecord_ID + "','" + sExExpense_ID + "',@companyId,@amount,@remark,@stCode,'" + logUser + "',NOW(),'" + logUser + "',NOW());";

                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                }

                //Insert in to Expected Expense Detail table
                string queryDetail = @"INSERT INTO EXPECTED_EXPENSE_DETAILS(RECORD_ID,EX_EXPENSE_ID,EXPENSE_CATEGORY_ID,DESCRIPTION,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                  VALUES('" + sExExpenseRecord_ID + "','" + sExExpense_ID + "','" + expCategoryId + "','" + expDescription + "','" + cost + "','" + expRemark + "','" + expStatus + "','" + logUser + "',NOW(),'" + logUser + "',NOW());";

                mySqlCmd.CommandText = queryDetail;
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

        public bool UpdateDetails(string sExExpense_ID, string trainingId, string expDescription, string expStatus, string remarkx, string perPerson, string totcost, string recId, string expCategory, string description, string amount, string remark, string status, string logUser, DataTable tdCompDetails)
        {
            Boolean isUpdate = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            string compRecId = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                if (recId != "")
                {
                    string delQuery = @"DELETE FROM COMPANY_WISE_EXPECTED_EXPENSE WHERE RECORD_ID = '" + recId + "';";
                    mySqlCmd.CommandText = delQuery;
                    mySqlCmd.ExecuteNonQuery();
                }
                else
                {
                    SerialHandler nserialcode = new SerialHandler();
                    compRecId = nserialcode.getserila(mySqlCon, "CE");
                }

                string queryex = @"UPDATE EXPECTED_EXPENSE SET TRAINING_ID= '" + trainingId + @"',
                                        TOTAL_EXPENSE = '" + totcost + @"',
                                        DESCRIPTION = '" + expDescription + @"',
                                        PER_PERSON_COST = '" + perPerson + @"',
                                        REMARKS = '" + remarkx + @"',STATUS_CODE = '" + expStatus + @"',
                                        MODIFIED_BY = '" + logUser + @"',
                                        MODIFIED_DATE = now()
                                        WHERE EXPECTED_EXPENSE_ID = '" + sExExpense_ID + "';";

                mySqlCmd.CommandText = queryex;
                mySqlCmd.ExecuteNonQuery();

                // inser or update expence details
                if (recId != "")
                {
                    //Update Expected Expense details
                    string query = @"UPDATE EXPECTED_EXPENSE_DETAILS 
                                            SET 
                                                EX_EXPENSE_ID = '" + sExExpense_ID + @"',
                                                EXPENSE_CATEGORY_ID = '" + expCategory + @"',
                                                DESCRIPTION = '" + description + @"',
                                                AMOUNT = '" + amount + @"',
                                                REMARKS = '" + remark + @"',
                                                STATUS_CODE = '" + status + @"',
                                                MODIFIED_BY = '" + logUser + @"',
                                                MODIFIED_DATE = NOW()
                                            WHERE
                                                RECORD_ID = '" + recId + "';";

                    mySqlCmd.CommandText = query;
                    mySqlCmd.ExecuteNonQuery();
                }
                else
                {
                    //insert Expected Expense details
                    string query = @"INSERT INTO EXPECTED_EXPENSE_DETAILS(RECORD_ID,EX_EXPENSE_ID,EXPENSE_CATEGORY_ID,DESCRIPTION,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                  VALUES('" + compRecId + "','" + sExExpense_ID + "','" + expCategory + "','" + description + "','" + amount + "','" + remark + "','" + status + "','" + logUser + "',NOW(),'" + logUser + "',NOW());";

                    mySqlCmd.CommandText = query;
                    mySqlCmd.ExecuteNonQuery();

                }
               
                    foreach (DataRow dr in tdCompDetails.Rows)
                    {
                        mySqlCmd.Parameters.Clear();
                        string companyId = dr["COMPANY_ID"].ToString();
                        string compName = dr["COMP_NAME"].ToString();
                        string participantCount = dr["PLANNED_PARTICIPANTS"].ToString();
                        string cmpRemark = dr["REMARK"].ToString();
                        string cmpAmount = dr["AMOUNT"].ToString();
                        string stCode = dr["STATUS_CODE"].ToString();


                        mySqlCmd.Parameters.Add(new MySqlParameter("@expextedExpenseId", sExExpense_ID.Trim() == "" ? (object)DBNull.Value : sExExpense_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@cmpRemark", cmpRemark.Trim() == "" ? (object)DBNull.Value : cmpRemark.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@cmpAmount", cmpAmount.Trim() == "" ? (object)DBNull.Value : cmpAmount.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@stCode", stCode.Trim() == "" ? (object)DBNull.Value : stCode.Trim()));

                   
                        if (recId != "")
                        {
                           

                            sMySqlString = @"INSERT INTO COMPANY_WISE_EXPECTED_EXPENSE(RECORD_ID,EX_EXPENSE_ID,COMPANY_ID,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                        VALUE('" + recId + "',@expextedExpenseId,@companyId,@cmpAmount,@cmpRemark,@stCode,'" + logUser + "',NOW(),'" + logUser + "',NOW());";

                            mySqlCmd.CommandText = sMySqlString;
                            mySqlCmd.ExecuteNonQuery();

                            
                        }
                        else
                        {
                           
                            sMySqlString = @"INSERT INTO COMPANY_WISE_EXPECTED_EXPENSE(RECORD_ID,EX_EXPENSE_ID,COMPANY_ID,AMOUNT,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                        VALUE('" + compRecId + "',@expextedExpenseId,@companyId,@cmpAmount,@cmpRemark,@stCode,'" + logUser + "',NOW(),'" + logUser + "',NOW());";

                            mySqlCmd.CommandText = sMySqlString;
                            mySqlCmd.ExecuteNonQuery();

                        }

                        
                    }

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


    }
}
