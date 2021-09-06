using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class ActualExpencesDataHandler : TemplateDataHandler
    {

        public DataTable getActualExpenseHeaderDetails(string trainingId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT AC_EXPENSE_ID, TRAINING_ID, TOTAL_EXPENSE, DESCRIPTION, TOTAL_DISCOUNT, GRAND_TOTAL, PER_PERSON_COST, STATUS_CODE
                                    FROM ACTUAL_EXPENSES
                                    WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' AND TRAINING_ID ='"+trainingId+"' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public DataTable getDetailedExpenses(string expenceId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT 
                                        AED.RECORD_ID,
                                        AED.EXPENSE_CATEGORY_ID,
                                        EC.CATEGORY_NAME,
                                        AED.DESCRIPTION,
                                        AMOUNT,
                                        DISCOUNT,
                                        NET_AMOUNT,
                                        IS_PAID,
                                        AED.REMARKS, " +
                                        " CASE " +
                                            "when AED.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                            "when AED.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        "end as STATUS_CODE ";
                           sqlQuery += @" FROM ACTUAL_EXPENSE_DETAILS AED
                                        LEFT JOIN 
                                            EXPENSE_CATEGORY EC
                                            ON AED.EXPENSE_CATEGORY_ID = EC.EXPENSE_CATEGORY_ID
                                        WHERE 
                                            AC_EXPENSE_ID ='"+expenceId+"' ";
                                        

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public DataTable getAllExpenseCategories()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string Qry = @"SELECT 
                                    EXPENSE_CATEGORY_ID, CATEGORY_NAME
                                FROM
                                    EXPENSE_CATEGORY
                                WHERE
                                    STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(Qry, mySqlCon);
                dataAdapter.Fill(resultTable);
                return resultTable;
            }

            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }  
            
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public bool checkExpenseExistance(string traingId)
        {
            DataTable resultTable = new DataTable();
            bool exist = false;
            try
            {
                string sqlQuery = @"SELECT AC_EXPENSE_ID, TRAINING_ID, TOTAL_EXPENSE, DESCRIPTION, TOTAL_DISCOUNT, GRAND_TOTAL, PER_PERSON_COST 
                                    FROM ACTUAL_EXPENSES
                                    WHERE TRAINING_ID ='"+traingId+"' AND STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count == 1)
                {
                    exist = true;
                }

                return exist;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                resultTable.Dispose();
            }
        }

        public bool updateActualExpense_InsertDetails_InsertCompany(string traingId, string expenseId, decimal total, 
                                                                    string trDescription, decimal totalDiscount, decimal grandTotal,
                                                                    decimal perPerson, string trStatus, string expCategory, 
                                                                    string description, decimal amount, decimal discount, 
                                                                    decimal netAmount, string isPaid, string paymentDescription, string remark,
                                                                    string status, string addedUser, DataTable dtCompanyExpenses)
        {
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                /// update ACTUAL_EXPENSES table
                
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add("@TOTAL_EXPENSE", MySqlDbType.Decimal).Value = total;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", trDescription.Trim() == "" ? (object)DBNull.Value : trDescription.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_DISCOUNT", MySqlDbType.Decimal).Value = totalDiscount;
                mySqlCmd.Parameters.Add("@GRAND_TOTAL", MySqlDbType.Decimal).Value = grandTotal;
                mySqlCmd.Parameters.Add("@PER_PERSON_COST", MySqlDbType.Decimal).Value = perPerson;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", trStatus.Trim() == "" ? (object)DBNull.Value : trStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlUpdateActualExpenceTable = @" UPDATE 
                                                            ACTUAL_EXPENSES
                                                        SET 
                                                            TOTAL_EXPENSE = @TOTAL_EXPENSE,
                                                            DESCRIPTION = @DESCRIPTION,
                                                            TOTAL_DISCOUNT = @TOTAL_DISCOUNT,
                                                            GRAND_TOTAL= @GRAND_TOTAL,
                                                            PER_PERSON_COST= @PER_PERSON_COST,
                                                            STATUS_CODE= @STATUS_CODE,
                                                            MODIFIED_BY= @ADDED_BY,
                                                            MODIFIED_DATE= now()
                                                        WHERE 
                                                            AC_EXPENSE_ID = '" + expenseId + "' AND " +
                                                            @" TRAINING_ID ='" + traingId + "' ";


                mySqlCmd.CommandText = sqlUpdateActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();

                /// insert into ACTUAL_EXPENSE_DETAILS table
                
                SerialHandler serialHandler = new SerialHandler();
                string actualExpenseDetailRecordId = serialHandler.getserilalReference(ref mySqlCon, "AED");

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", actualExpenseDetailRecordId.Trim() == "" ? (object)DBNull.Value : actualExpenseDetailRecordId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", expenseId.Trim() == "" ? (object)DBNull.Value : expenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EXPENSE_CATEGORY_ID", expCategory.Trim() == "" ? (object)DBNull.Value : expCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = amount;
                mySqlCmd.Parameters.Add("@DISCOUNT", MySqlDbType.Decimal).Value = discount;
                mySqlCmd.Parameters.Add("@NET_AMOUNT", MySqlDbType.Decimal).Value = netAmount;
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PAID", isPaid.Trim() == "" ? (object)DBNull.Value : isPaid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PAYMENT_DESCRIPTION", paymentDescription.Trim() == "" ? (object)DBNull.Value : paymentDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                string sqlInsertActualExpenceDetailTable = @"
                                                                INSERT INTO 
                                                                    ACTUAL_EXPENSE_DETAILS (
                                                                                            RECORD_ID,
                                                                                            AC_EXPENSE_ID,
                                                                                            EXPENSE_CATEGORY_ID,
                                                                                            DESCRIPTION,
                                                                                            AMOUNT,
                                                                                            DISCOUNT,
                                                                                            NET_AMOUNT,
                                                                                            IS_PAID,
                                                                                            PAYMENT_DESCRIPTION,
                                                                                            REMARKS,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                            )
                                                                                        VALUES(
                                                                                            @RECORD_ID,
                                                                                            @AC_EXPENSE_ID,
                                                                                            @EXPENSE_CATEGORY_ID,
                                                                                            @DESCRIPTION,
                                                                                            @AMOUNT,
                                                                                            @DISCOUNT,
                                                                                            @NET_AMOUNT,
                                                                                            @IS_PAID,
                                                                                            @PAYMENT_DESCRIPTION,
                                                                                            @REMARKS,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                        )

                                                                ";

                mySqlCmd.CommandText = sqlInsertActualExpenceDetailTable;
                mySqlCmd.ExecuteNonQuery();

                
                /// Insert Data into COMPANY_WISE_ACTUAL_EXPENSE table
                foreach (DataRow company in dtCompanyExpenses.Rows)
                {
                    string companyId = company["column0"].ToString();
                    string remarks = company["column3"].ToString();
                    decimal compAmount = Convert.ToDecimal(company["column4"].ToString());
                    //string compExpenseStatus = ;
                    mySqlCmd.Parameters.Clear();
                    
                    mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", actualExpenseDetailRecordId.Trim() == "" ? (object)DBNull.Value : actualExpenseDetailRecordId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", expenseId.Trim() == "" ? (object)DBNull.Value : expenseId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                    mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = compAmount;
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                    string insertCompanyWiseExpenseTable = @" INSERT INTO 
                                                                COMPANY_WISE_ACTUAL_EXPENSE(
                                                                                            RECORD_ID,
                                                                                            AC_EXPENSE_ID,
                                                                                            COMPANY_ID,
                                                                                            REMARKS,
                                                                                            AMOUNT,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                    )
                                                                                    VALUES(
                                                                                            @RECORD_ID,
                                                                                            @AC_EXPENSE_ID,
                                                                                            @COMPANY_ID,
                                                                                            @REMARKS,
                                                                                            @AMOUNT,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                    )
                                                                ";

                    mySqlCmd.CommandText = insertCompanyWiseExpenseTable;
                    mySqlCmd.ExecuteNonQuery();
                }


                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            
        }

        /// <summary>
        /// insert new record into ACTUAL_EXPENSE_DETAILS table
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="expCategory"></param>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="discount"></param>
        /// <param name="netAmount"></param>
        /// <param name="isPaid"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <param name="addedUser"></param>
        /// <returns></returns>
        private bool insertActualExpenseDetailTable(string expenseId, string expCategory, string description, decimal amount, decimal discount, decimal netAmount, string isPaid, string remark, string status, string addedUser)
        {
            bool actualExpenceInserted = false;

            try
            {

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", expenseId.Trim() == "" ? (object)DBNull.Value : expenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EXPENSE_CATEGORY_ID", expCategory.Trim() == "" ? (object)DBNull.Value : expCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = amount;
                mySqlCmd.Parameters.Add("@DISCOUNT", MySqlDbType.Decimal).Value = discount;
                mySqlCmd.Parameters.Add("@NET_AMOUNT", MySqlDbType.Decimal).Value = netAmount;
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PAID", isPaid.Trim() == "" ? (object)DBNull.Value : isPaid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                string sqlInsertActualExpenceDetailTable = @"
                                                                INSERT INTO 
                                                                    ACTUAL_EXPENSE_DETAILS (
                                                                                            AC_EXPENSE_ID,
                                                                                            EXPENSE_CATEGORY_ID,
                                                                                            DESCRIPTION,
                                                                                            AMOUNT,
                                                                                            DISCOUNT,
                                                                                            NET_AMOUNT,
                                                                                            IS_PAID,
                                                                                            REMARKS,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                            )
                                                                                        VALUES(
                                                                                            @AC_EXPENSE_ID,
                                                                                            @EXPENSE_CATEGORY_ID,
                                                                                            @DESCRIPTION,
                                                                                            @AMOUNT,
                                                                                            @DISCOUNT,
                                                                                            @NET_AMOUNT,
                                                                                            @IS_PAID,
                                                                                            @REMARKS,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                        )

                                                                ";

                mySqlCmd.CommandText = sqlInsertActualExpenceDetailTable;
                mySqlCmd.ExecuteNonQuery();

                actualExpenceInserted = true;
                return actualExpenceInserted;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }
        
        /// <summary>
        /// update ACTUAL_EXPENSE table
        /// </summary>
        /// <param name="traingId"></param>
        /// <param name="expenseId"></param>
        /// <param name="total"></param>
        /// <param name="trDescription"></param>
        /// <param name="totalDiscount"></param>
        /// <param name="grandTotal"></param>
        /// <param name="perPerson"></param>
        /// <param name="trStatus"></param>
        /// <param name="addedUser"></param>
        /// <returns></returns>
        private bool updateActualExpenseTable(string traingId, string expenseId, decimal total, string trDescription, decimal totalDiscount, decimal grandTotal, decimal perPerson, string trStatus, string addedUser)
        {
            /// do not open or close db connection within this function as its execute inside another function 
            bool actualExpenceUpdated = false;

            try
            {
                
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add("@TOTAL_EXPENSE", MySqlDbType.Decimal).Value = total;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", trDescription.Trim() == "" ? (object)DBNull.Value : trDescription.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_DISCOUNT", MySqlDbType.Decimal).Value = totalDiscount;
                mySqlCmd.Parameters.Add("@GRAND_TOTAL", MySqlDbType.Decimal).Value = grandTotal;
                mySqlCmd.Parameters.Add("@PER_PERSON_COST", MySqlDbType.Decimal).Value = perPerson;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", trStatus.Trim() == "" ? (object)DBNull.Value : trStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlUpdateActualExpenceTable = @" UPDATE 
                                                            ACTUAL_EXPENSES
                                                        SET 
                                                            TOTAL_EXPENSE = @TOTAL_EXPENSE,
                                                            DESCRIPTION = @DESCRIPTION,
                                                            TOTAL_DISCOUNT = @TOTAL_DISCOUNT,
                                                            GRAND_TOTAL= @GRAND_TOTAL,
                                                            PER_PERSON_COST= @PER_PERSON_COST,
                                                            STATUS_CODE= @STATUS_CODE,
                                                            MODIFIED_BY= @ADDED_BY,
                                                            MODIFIED_DATE= now()
                                                        WHERE 
                                                            AC_EXPENSE_ID = '" + expenseId+"' AND "+
                                                            @" TRAINING_ID ='"+traingId+"' ";


                mySqlCmd.CommandText = sqlUpdateActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();

                actualExpenceUpdated = true;
                return actualExpenceUpdated;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }                
                throw;
            }
        }

        /// <summary>
        /// Insert new records to ACTUAL_EXPENSES,ACTUAL_EXPENSE_DETAILS tables
        /// </summary>
        /// <param name="traingId"></param>
        /// <param name="total"></param>
        /// <param name="trDescription"></param>
        /// <param name="totalDiscount"></param>
        /// <param name="grandTotal"></param>
        /// <param name="perPerson"></param>
        /// <param name="trStatus"></param>
        /// <param name="expCategory"></param>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="discount"></param>
        /// <param name="netAmount"></param>
        /// <param name="isPaid"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <param name="addedUser"></param>
        /// <returns></returns>
        public bool insertActualExpense_InsertDetails_InsertCompany(string traingId, decimal total, string trDescription, decimal totalDiscount, decimal grandTotal, decimal perPerson, string trStatus, string expCategory, string description, decimal amount, decimal discount, decimal netAmount, string isPaid, string paymentDescription, string remark, string status, string addedUser, DataTable dtCompanyExpenses)
        {
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                SerialHandler serialHandler = new SerialHandler();
                string actualExpenseId = serialHandler.getserilalReference(ref mySqlCon, "AE"); 

                /// insert into ACTUAL_EXPENSES 
                
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", actualExpenseId.Trim() == "" ? (object)DBNull.Value : actualExpenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", traingId.Trim() == "" ? (object)DBNull.Value : traingId.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_EXPENSE", MySqlDbType.Decimal).Value = total;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", trDescription.Trim() == "" ? (object)DBNull.Value : trDescription.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_DISCOUNT", MySqlDbType.Decimal).Value = totalDiscount;
                mySqlCmd.Parameters.Add("@GRAND_TOTAL", MySqlDbType.Decimal).Value = grandTotal;
                mySqlCmd.Parameters.Add("@PER_PERSON_COST", MySqlDbType.Decimal).Value = perPerson;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", trStatus.Trim() == "" ? (object)DBNull.Value : trStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlInsertActualExpenceTable = @" INSERT INTO  
                                                            ACTUAL_EXPENSES(
                                                                AC_EXPENSE_ID,
                                                                TRAINING_ID,
                                                                TOTAL_EXPENSE,
                                                                DESCRIPTION,
                                                                TOTAL_DISCOUNT,
                                                                GRAND_TOTAL,
                                                                PER_PERSON_COST, 
                                                                STATUS_CODE,
                                                                ADDED_BY,
                                                                ADDED_DATE,
                                                                MODIFIED_BY,
                                                                MODIFIED_DATE
                                                            )
                                                            VALUES(
                                                                @AC_EXPENSE_ID,
                                                                @TRAINING_ID,                                                       
                                                                @TOTAL_EXPENSE,
                                                                @DESCRIPTION,
                                                                @TOTAL_DISCOUNT,
                                                                @GRAND_TOTAL,
                                                                @PER_PERSON_COST,
                                                                @STATUS_CODE,
                                                                @ADDED_BY,
                                                                now(),
                                                                @ADDED_BY,
                                                                now()
                                                            )";



                mySqlCmd.CommandText = sqlInsertActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();


                /// Insert into ACTUAL_EXPENSE_DETAILS ////
                string actualExpenseDetailRecordId = serialHandler.getserilalReference(ref mySqlCon, "AED");

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", actualExpenseDetailRecordId.Trim() == "" ? (object)DBNull.Value : actualExpenseDetailRecordId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", actualExpenseId.Trim() == "" ? (object)DBNull.Value : actualExpenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EXPENSE_CATEGORY_ID", expCategory.Trim() == "" ? (object)DBNull.Value : expCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = amount;
                mySqlCmd.Parameters.Add("@DISCOUNT", MySqlDbType.Decimal).Value = discount;
                mySqlCmd.Parameters.Add("@NET_AMOUNT", MySqlDbType.Decimal).Value = netAmount;
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PAID", isPaid.Trim() == "" ? (object)DBNull.Value : isPaid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PAYMENT_DESCRIPTION", paymentDescription.Trim() == "" ? (object)DBNull.Value : paymentDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                string sqlInsertActualExpenceDetailTable = @"
                                                                INSERT INTO 
                                                                    ACTUAL_EXPENSE_DETAILS (RECORD_ID,
                                                                                            AC_EXPENSE_ID,
                                                                                            EXPENSE_CATEGORY_ID,
                                                                                            DESCRIPTION,
                                                                                            AMOUNT,
                                                                                            DISCOUNT,
                                                                                            NET_AMOUNT,
                                                                                            IS_PAID,
                                                                                            PAYMENT_DESCRIPTION,
                                                                                            REMARKS,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                            )
                                                                                        VALUES(
                                                                                            @RECORD_ID,
                                                                                            @AC_EXPENSE_ID,
                                                                                            @EXPENSE_CATEGORY_ID,
                                                                                            @DESCRIPTION,
                                                                                            @AMOUNT,
                                                                                            @DISCOUNT,
                                                                                            @NET_AMOUNT,
                                                                                            @IS_PAID,
                                                                                            @PAYMENT_DESCRIPTION,
                                                                                            @REMARKS,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                        )

                                                                ";

                mySqlCmd.CommandText = sqlInsertActualExpenceDetailTable;
                mySqlCmd.ExecuteNonQuery();

                /// Insert Data into COMPANY_WISE_ACTUAL_EXPENSE table
                foreach (DataRow company in dtCompanyExpenses.Rows)
                {
                    string companyId = company["column0"].ToString();
                    string remarks = company["column3"].ToString();
                    decimal compAmount = Convert.ToDecimal(company["column4"].ToString());
                    //string compExpenseStatus = Constants.STATUS_ACTIVE_VALUE;
                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", actualExpenseDetailRecordId.Trim() == "" ? (object)DBNull.Value : actualExpenseDetailRecordId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", actualExpenseId.Trim() == "" ? (object)DBNull.Value : actualExpenseId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                    mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = compAmount;
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                    string insertCompanyWiseExpenseTable = @" INSERT INTO 
                                                                COMPANY_WISE_ACTUAL_EXPENSE(
                                                                                            RECORD_ID,
                                                                                            AC_EXPENSE_ID,
                                                                                            COMPANY_ID,
                                                                                            AMOUNT,
                                                                                            REMARKS,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                    )
                                                                                    VALUES(
                                                                                            @RECORD_ID,
                                                                                            @AC_EXPENSE_ID,
                                                                                            @COMPANY_ID,
                                                                                            @AMOUNT,
                                                                                            @REMARKS,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                    )
                                                                ";

                    mySqlCmd.CommandText = insertCompanyWiseExpenseTable;
                    mySqlCmd.ExecuteNonQuery();
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                serialHandler = null;
                return true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// insert into ACTUAL_EXPENSES table
        /// </summary>
        /// <param name="traingId"></param>
        /// <param name="total"></param>
        /// <param name="trDescription"></param>
        /// <param name="totalDiscount"></param>
        /// <param name="grandTotal"></param>
        /// <param name="perPerson"></param>
        /// <param name="trStatus"></param>
        /// <param name="addedUser"></param>
        /// <returns></returns>
        private bool insertActualExpenseTable(string traingId, decimal total, string trDescription, decimal totalDiscount, decimal grandTotal, decimal perPerson, string trStatus, string addedUser)
        {
            bool actualExpenceInserted = false;

            try
            {
                SerialHandler serialCode = new SerialHandler();
                string actualExpenseId = serialCode.getserila(mySqlCon, "AE");

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", actualExpenseId.Trim() == "" ? (object)DBNull.Value : actualExpenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", traingId.Trim() == "" ? (object)DBNull.Value : traingId.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_EXPENSE", MySqlDbType.Decimal).Value = total;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", trDescription.Trim() == "" ? (object)DBNull.Value : trDescription.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_DISCOUNT", MySqlDbType.Decimal).Value = totalDiscount;
                mySqlCmd.Parameters.Add("@GRAND_TOTAL", MySqlDbType.Decimal).Value = grandTotal;
                mySqlCmd.Parameters.Add("@PER_PERSON_COST", MySqlDbType.Decimal).Value = perPerson;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", trStatus.Trim() == "" ? (object)DBNull.Value : trStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlUpdateActualExpenceTable = @" INSERT INTO  
                                                            ACTUAL_EXPENSES(
                                                                AC_EXPENSE_ID,
                                                                TRAINING_ID,
                                                                TOTAL_EXPENSE,
                                                                DESCRIPTION,
                                                                TOTAL_DISCOUNT,
                                                                GRAND_TOTAL,
                                                                PER_PERSON_COST, 
                                                                STATUS_CODE,
                                                                ADDED_BY,
                                                                ADDED_DATE,
                                                                MODIFIED_BY,
                                                                MODIFIED_DATE
                                                            )
                                                            VALUES(
                                                                @AC_EXPENSE_ID,
                                                                @TRAINING_ID,                                                       
                                                                @TOTAL_EXPENSE,
                                                                @DESCRIPTION,
                                                                @TOTAL_DISCOUNT,
                                                                @GRAND_TOTAL,
                                                                @PER_PERSON_COST,
                                                                @STATUS_CODE,
                                                                @ADDED_BY,
                                                                now(),
                                                                @ADDED_BY,
                                                                now()
                                                            )";



                mySqlCmd.CommandText = sqlUpdateActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();

                actualExpenceInserted = true;
                return actualExpenceInserted;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Returns participant count for given training
        /// </summary>
        /// <param name="trainingId">string Training Id</param>
        /// <returns>int count</returns>
        public int getTrainingParticipantCount(string trainingId)
        {
            DataTable resultTable = new DataTable();
            try
            {                
                string sqlQuery = @"SELECT 
                                        PLANNED_PARTICIPANTS
                                    FROM 
                                        TRAINING
                                    WHERE 
                                        TRAINING_ID = '" + trainingId + "' AND STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return Int16.Parse(resultTable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }                     
        }

        /// <summary>
        /// Returns company wise planned participant count using TRAINING_COMPANY table
        /// </summary>
        /// <param name="selectedTraining"></param>
        /// <returns></returns>
        public DataTable getCompanyWiseParticipantCount(string trainingId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                  string sqlQuery = @"SELECT 
                                        TC.COMPANY_ID, C.COMP_NAME, TC.PLANNED_PARTICIPANTS
                                    FROM
										TRAINING T
									LEFT JOIN 
                                        TRAINING_COMPANY TC
											ON T.TRAINING_ID = TC.TRAINING_ID
                                    LEFT JOIN
                                        COMPANY C
                                            ON TC.COMPANY_ID = C.COMPANY_ID
                                    WHERE 
                                        T.TRAINING_ID = '" + trainingId + "' AND T.STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
									

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            
        }

        /// <summary>
        /// Check the existance of particular expense category for given training expense 
        /// </summary>
        /// <param name="selectedExpenseCategory"></param>
        /// <param name="trainingId"></param>
        /// <returns> true if category already exist</returns>
        public bool checkCategoryExistanceForExpense(string selectedExpenseCategory, string trainingId)
        {
            DataTable resultTable = new DataTable();
            bool exist = false;
            try
            {
                string sqlQuery = @"select AED.RECORD_ID
                                    From 
	                                    ACTUAL_EXPENSES AE
	                                    left join 
		                                    ACTUAL_EXPENSE_DETAILS AED on 
		                                    AE.AC_EXPENSE_ID = AED.AC_EXPENSE_ID
                                    where
	                                    AE.TRAINING_ID = '"+trainingId+"' ";
	                     sqlQuery += @" and
	                                    AE.STATUS_CODE = '"+Constants.STATUS_ACTIVE_VALUE+"' ";
                         sqlQuery += @" and
	                                    AED.EXPENSE_CATEGORY_ID = '"+selectedExpenseCategory+"' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count >0)
                {
                    exist = true;
                }

                return exist;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public DataTable getCategoryDetail(string expCategory, string traingId)
        {
            DataTable resultTable = new DataTable();            
            try
            {
                string sqlQuery = @"select AED.RECORD_ID, AED.EXPENSE_CATEGORY_ID, AED.AMOUNT, AED.NET_AMOUNT, AED.DISCOUNT, AED.DESCRIPTION, AED.IS_PAID, AED.REMARKS, AED.STATUS_CODE
                                    from
	                                    ACTUAL_EXPENSES AE
	                                    left join 
		                                    ACTUAL_EXPENSE_DETAILS AED on
		                                    AE.AC_EXPENSE_ID = AED.AC_EXPENSE_ID
                                    where
	                                    AE.TRAINING_ID = '" +traingId+"' ";


                        sqlQuery += @" and
	                                    AED.EXPENSE_CATEGORY_ID = '" + expCategory + "' ";

                        sqlQuery += @" and
	                                    AE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public DataTable getCategoryDetail(string recordId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"select AED.RECORD_ID, AED.EXPENSE_CATEGORY_ID, AED.AMOUNT, AED.NET_AMOUNT, AED.DISCOUNT,AED.DESCRIPTION, AED.IS_PAID, AED.REMARKS, AED.STATUS_CODE, AED.PAYMENT_DESCRIPTION
                                    from	                                  
		                                ACTUAL_EXPENSE_DETAILS AED 
                                    where
	                                    AED.RECORD_ID = '" + recordId + "' ";

//                        sqlQuery += @" and
//	                                    AE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";
                

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public DataTable getCompanyWiseExpense(string recordId, string trainingId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                 string sqlQuery = @"select CAE.RECORD_ID, AC_EXPENSE_ID, CAE.COMPANY_ID, C.COMP_NAME, CAE.AMOUNT, CAE.REMARKS, CAE.STATUS_CODE, TC.PLANNED_PARTICIPANTS
                                    from	                                  
		                                COMPANY_WISE_ACTUAL_EXPENSE CAE
                                        left join 
                                            TRAINING_COMPANY TC on
                                            CAE.COMPANY_ID = TC.COMPANY_ID
                                        left join 
                                            COMPANY C on
                                            CAE.COMPANY_ID = C.COMPANY_ID
                                    where
	                                    CAE.RECORD_ID = '" + recordId + "' ";
                        sqlQuery += @" and
	                                    TC.TRAINING_ID = '" + trainingId + "' ";
                        sqlQuery += @" and
	                                    TC.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";
                        sqlQuery += @" and
	                                    CAE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public bool updateActualExpense_UpdatetDetails_UpdateCompany(string traingId, string expenseId, decimal total, string trDescription, decimal totalDiscount, decimal grandTotal, decimal perPerson, string trStatus, string expCategory, string description, decimal amount, decimal discount, decimal netAmount, string isPaid, string paymentDescription, string remark, string status, string addedUser, DataTable dtCompanyExpenses, string recordId)
        {
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                /// update ACTUAL_EXPENSES table

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add("@TOTAL_EXPENSE", MySqlDbType.Decimal).Value = total;
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", trDescription.Trim() == "" ? (object)DBNull.Value : trDescription.Trim()));
                mySqlCmd.Parameters.Add("@TOTAL_DISCOUNT", MySqlDbType.Decimal).Value = totalDiscount;
                mySqlCmd.Parameters.Add("@GRAND_TOTAL", MySqlDbType.Decimal).Value = grandTotal;
                mySqlCmd.Parameters.Add("@PER_PERSON_COST", MySqlDbType.Decimal).Value = perPerson;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", trStatus.Trim() == "" ? (object)DBNull.Value : trStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlUpdateActualExpenceTable = @" UPDATE 
                                                            ACTUAL_EXPENSES
                                                        SET 
                                                            TOTAL_EXPENSE = @TOTAL_EXPENSE,
                                                            DESCRIPTION = @DESCRIPTION,
                                                            TOTAL_DISCOUNT = @TOTAL_DISCOUNT,
                                                            GRAND_TOTAL= @GRAND_TOTAL,
                                                            PER_PERSON_COST= @PER_PERSON_COST,
                                                            STATUS_CODE= @STATUS_CODE,
                                                            MODIFIED_BY= @ADDED_BY,
                                                            MODIFIED_DATE= now()
                                                        WHERE 
                                                            AC_EXPENSE_ID = '" + expenseId + "' AND " +
                                                            @" TRAINING_ID ='" + traingId + "' ";


                mySqlCmd.CommandText = sqlUpdateActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();

                //// Update ACTUAL_EXPENSE_DETAILS table
                mySqlCmd.Parameters.Clear();

                //mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", recordId.Trim() == "" ? (object)DBNull.Value : recordId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", expenseId.Trim() == "" ? (object)DBNull.Value : expenseId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EXPENSE_CATEGORY_ID", expCategory.Trim() == "" ? (object)DBNull.Value : expCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = amount;
                mySqlCmd.Parameters.Add("@DISCOUNT", MySqlDbType.Decimal).Value = discount;
                mySqlCmd.Parameters.Add("@NET_AMOUNT", MySqlDbType.Decimal).Value = netAmount;
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PAID", isPaid.Trim() == "" ? (object)DBNull.Value : isPaid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PAYMENT_DESCRIPTION", paymentDescription.Trim() == "" ? (object)DBNull.Value : paymentDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", remark.Trim() == "" ? (object)DBNull.Value : remark.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                string sqlUpdateActualExpenceDetailTable = @"
                                                            UPDATE 
                                                                ACTUAL_EXPENSE_DETAILS 
                                                            SET 
                                                                EXPENSE_CATEGORY_ID = @EXPENSE_CATEGORY_ID,                 
                                                                DESCRIPTION = @DESCRIPTION,
                                                                AMOUNT = @AMOUNT,
                                                                DISCOUNT = @DISCOUNT,
                                                                NET_AMOUNT = @NET_AMOUNT,
                                                                IS_PAID = @IS_PAID,
                                                                PAYMENT_DESCRIPTION = @PAYMENT_DESCRIPTION,
                                                                REMARKS = @REMARKS,
                                                                STATUS_CODE = @STATUS_CODE,
                                                                MODIFIED_BY = @ADDED_BY,
                                                                MODIFIED_DATE = now()
                                                            WHERE 
                                                                AC_EXPENSE_ID = '" + expenseId + "' AND " +
                                                                //" EXPENSE_CATEGORY_ID ='" + expCategory + "'  AND "+
                                                                " RECORD_ID ='"+ recordId+"' ";


                mySqlCmd.CommandText = sqlUpdateActualExpenceDetailTable;
                mySqlCmd.ExecuteNonQuery();

                string sqlDeleteCompanyWiseExpenses = @" DELETE 
                                                            FROM 
                                                                COMPANY_WISE_ACTUAL_EXPENSE 
                                                            WHERE 
                                                                AC_EXPENSE_ID = '" + expenseId + "' AND " +
                                                                " RECORD_ID = '" + recordId + "' ";

                mySqlCmd.CommandText = sqlDeleteCompanyWiseExpenses;
                mySqlCmd.ExecuteNonQuery();

                if (dtCompanyExpenses != null)
                {
                    /// Insert Data into COMPANY_WISE_ACTUAL_EXPENSE table
                    foreach (DataRow company in dtCompanyExpenses.Rows)
                    {
                        string companyId = company["column0"].ToString();
                        string remarks = company["column3"].ToString();
                        decimal compAmount = Convert.ToDecimal(company["column4"].ToString());
                        //string compExpenseStatus = ;
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", recordId.Trim() == "" ? (object)DBNull.Value : recordId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AC_EXPENSE_ID", expenseId.Trim() == "" ? (object)DBNull.Value : expenseId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                        mySqlCmd.Parameters.Add("@AMOUNT", MySqlDbType.Decimal).Value = compAmount;
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));

                        string insertCompanyWiseExpenseTable = @" INSERT INTO 
                                                                COMPANY_WISE_ACTUAL_EXPENSE(
                                                                                            RECORD_ID,
                                                                                            AC_EXPENSE_ID,
                                                                                            COMPANY_ID,
                                                                                            REMARKS,
                                                                                            AMOUNT,
                                                                                            STATUS_CODE,
                                                                                            ADDED_BY,
                                                                                            ADDED_DATE,
                                                                                            MODIFIED_BY,
                                                                                            MODIFIED_DATE
                                                                                    )
                                                                                    VALUES(
                                                                                            @RECORD_ID,
                                                                                            @AC_EXPENSE_ID,
                                                                                            @COMPANY_ID,
                                                                                            @REMARKS,
                                                                                            @AMOUNT,
                                                                                            @STATUS_CODE,
                                                                                            @ADDED_BY,
                                                                                            now(),
                                                                                            @ADDED_BY,
                                                                                            now()
                                                                                    )
                                                                ";

                        mySqlCmd.CommandText = insertCompanyWiseExpenseTable;
                        mySqlCmd.ExecuteNonQuery();
                    } 
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }

        public bool updateActualExpense(string trainingId, string expenseId, string masterDescription, string masterStatus, string addedUser)
        {
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                /// update ACTUAL_EXPENSES table

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", masterDescription.Trim() == "" ? (object)DBNull.Value : masterDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", masterStatus.Trim() == "" ? (object)DBNull.Value : masterStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                string sqlUpdateActualExpenceTable = @" UPDATE 
                                                            ACTUAL_EXPENSES
                                                        SET 
                                                            DESCRIPTION = @DESCRIPTION,
                                                            STATUS_CODE= @STATUS_CODE,
                                                            MODIFIED_BY= @ADDED_BY,
                                                            MODIFIED_DATE= now()
                                                        WHERE 
                                                            AC_EXPENSE_ID = '" + expenseId + "' AND " +
                                                            @" TRAINING_ID ='" + trainingId + "' ";


                mySqlCmd.CommandText = sqlUpdateActualExpenceTable;
                mySqlCmd.ExecuteNonQuery();

                ///// update ACTUAL_EXPENSE_DETAILS table

//                mySqlCmd.Parameters.Clear();

//                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", masterStatus.Trim() == "" ? (object)DBNull.Value : masterStatus.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

//                string sqlUpdateActualExpenceDetailsTable = @" UPDATE 
//                                                                ACTUAL_EXPENSE_DETAILS
//                                                            SET 
//                                                                STATUS_CODE= @STATUS_CODE,
//                                                                MODIFIED_BY= @ADDED_BY,
//                                                                MODIFIED_DATE= now()
//                                                            WHERE 
//                                                                AC_EXPENSE_ID = '" + expenseId + "' ";



//                mySqlCmd.CommandText = sqlUpdateActualExpenceDetailsTable;
//                mySqlCmd.ExecuteNonQuery();

//                /// update COMPANY_WISE_ACTUAL_EXPENSE table

//                mySqlCmd.Parameters.Clear();

//                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", masterStatus.Trim() == "" ? (object)DBNull.Value : masterStatus.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

//                string sqlUpdateCompanyExpenceDetailsTable = @" UPDATE 
//                                                                COMPANY_WISE_ACTUAL_EXPENSE
//                                                            SET 
//                                                                STATUS_CODE= @STATUS_CODE,
//                                                                MODIFIED_BY= @ADDED_BY,
//                                                                MODIFIED_DATE= now()
//                                                            WHERE 
//                                                                AC_EXPENSE_ID = '" + expenseId + "' ";



//                mySqlCmd.CommandText = sqlUpdateCompanyExpenceDetailsTable;
//                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }
    }
}
