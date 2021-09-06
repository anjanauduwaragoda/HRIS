using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class FinancialYearsDataHandler : TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                FINANCIAL_YEAR,
                                                CONVERT( DATE_FORMAT(START_DATE, '%d/%m/%Y') , CHAR) AS 'START_DATE',
                                                CONVERT( DATE_FORMAT(END_DATE, '%d/%m/%Y') , CHAR) AS 'END_DATE',
                                                DESCRIPTION,
                                                STATUS_CODE
                                            FROM
                                                FINANCIAL_YEARS
                                            ORDER BY 
                                                FINANCIAL_YEAR DESC;                                              
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

        public Boolean Insert(string FinancialYear, string StartDate, string EndDate, string Description, string StatusCode, string AddedBy)
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
                                        INSERT INTO 
                                            FINANCIAL_YEARS
                                            (
                                                FINANCIAL_YEAR, 
                                                DESCRIPTION, 
                                                START_DATE, 
                                                END_DATE, 
                                                STATUS_CODE, 
                                                ADDED_BY, 
                                                ADDED_DATE, 
                                                MODIFIED_BY, 
                                                MODIFIED_DATE
                                            ) 
                                        VALUES
                                            (
                                                @FINANCIAL_YEAR, 
                                                @DESCRIPTION, 
                                                @START_DATE, 
                                                @END_DATE, 
                                                @STATUS_CODE, 
                                                @ADDED_BY, 
                                                NOW(), 
                                                @MODIFIED_BY, 
                                                NOW()
                                            );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@FINANCIAL_YEAR", FinancialYear.Trim() == "" ? (object)DBNull.Value : FinancialYear.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@START_DATE", StartDate.Trim() == "" ? (object)DBNull.Value : StartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@END_DATE", EndDate.Trim() == "" ? (object)DBNull.Value : EndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    oMySqlTransaction.Commit();

                    mySqlCmd.Parameters.Clear();
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

        public Boolean Update(string FinancialYear, string StartDate, string EndDate, string Description, string StatusCode, string ModifiedBy, string OldFinancialYear)
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
                                            FINANCIAL_YEARS 
                                        SET  
                                            FINANCIAL_YEAR = @FINANCIAL_YEAR,
                                            DESCRIPTION = @DESCRIPTION, 
                                            START_DATE = @START_DATE, 
                                            END_DATE = @END_DATE, 
                                            STATUS_CODE = @STATUS_CODE, 
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW()
                                        WHERE
                                            FINANCIAL_YEAR = @OLD_FINANCIAL_YEAR;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@FINANCIAL_YEAR", FinancialYear.Trim() == "" ? (object)DBNull.Value : FinancialYear.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@OLD_FINANCIAL_YEAR", OldFinancialYear.Trim() == "" ? (object)DBNull.Value : OldFinancialYear.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@START_DATE", StartDate.Trim() == "" ? (object)DBNull.Value : StartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@END_DATE", EndDate.Trim() == "" ? (object)DBNull.Value : EndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    oMySqlTransaction.Commit();

                    mySqlCmd.Parameters.Clear();
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

        public Boolean CheckFinancialYearExsistance(string FinancialYear)
        {
            Boolean isExsists = false;
            try
            {
                mySqlCon.Open();
                dataTable = new DataTable();

                mySqlCmd.Parameters.Clear();

                string queryStr = @"
                                    SELECT 
                                        *
                                    FROM
                                        FINANCIAL_YEARS
                                    WHERE
                                        FINANCIAL_YEAR = @FINANCIAL_YEAR;
                                    ";

                mySqlCmd.CommandText = queryStr;
                mySqlCmd.Connection = mySqlCon;

                mySqlCmd.Parameters.Add(new MySqlParameter("@FINANCIAL_YEAR", FinancialYear));

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

        public Boolean CheckFinancialYearExsistance(string CurrentFinancialYear, string OldFinancialYear)
        {

            dataTable = new DataTable();
            Boolean isExsists = false;

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Clear();

                string queryStr = @"
                                        SELECT 
                                            *
                                        FROM
                                            FINANCIAL_YEARS
                                        WHERE
                                            FINANCIAL_YEAR = @FINANCIAL_YEAR;
                                    ";
                mySqlCmd.CommandText = queryStr;
                mySqlCmd.Parameters.Add(new MySqlParameter("@FINANCIAL_YEAR", CurrentFinancialYear));
                mySqlCmd.Connection = mySqlCon;


                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(mySqlCmd);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["FINANCIAL_YEAR"].ToString() == OldFinancialYear)
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
