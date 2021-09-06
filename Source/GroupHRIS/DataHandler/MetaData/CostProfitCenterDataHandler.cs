using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class CostProfitCenterDataHandler:TemplateDataHandler
    {
        public DataTable GetGidData()
        {
            try
            {
                dataTable.Rows.Clear();
                string mysqlQry = @"SELECT c.COMP_NAME,
		                                    CASE WHEN cc.IS_PROFIT_CENTER = '0' THEN 'Profit Center' 
						                                    WHEN cc.IS_PROFIT_CENTER = '1' THEN 'Cost Center' END AS IS_PROFIT_CENTER,
		                                    cc.COMP_COST_PROFIT_CENTER_CODE,COST_PROFIT_CENTER_NAME,
		                                    CASE WHEN cc.STATUS_CODE = '1' THEN 'Active'
			                                    WHEN cc.STATUS_CODE = '0' THEN 'Inactive' END AS STATUS_CODE
                                    FROM COMP_COST_PROFIT_CENTER cc,COMPANY c 
                                    WHERE cc.COMPANY_ID = c.COMPANY_ID;";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mysqlQry,mySqlCon);
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

        public Boolean Insert(DataTable dtcostProfitCenter,string user) //string companyId,string cpCenter,string cpCode,string cpName,string status,string user
        { 
            Boolean Status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                foreach (DataRow dr in dtcostProfitCenter.Rows)
                {
                    string isExclude = dr["IS_EXCLUDE"].ToString().Trim();

                    mySqlCmd.Parameters.Clear();
                    if (isExclude == "")
                    {
                        string code = dr["COMP_COST_PROFIT_CENTER_CODE"].ToString();
                        string name = dr["COST_PROFIT_CENTER_NAME"].ToString();
                        string isCP = dr["IS_PROFIT_CENTER"].ToString();
                        string company = dr["COMPANY_ID"].ToString();
                        string status = dr["STATUS_CODE"].ToString();

                        string isCostProfit = "";
                        if (isCP == "Cost Center")
                        {
                            isCostProfit = "0";
                        }
                        else if (isCP == "Profit Center")
                        {
                            isCostProfit = "1";
                        }

                        mySqlCmd.Parameters.Add(new MySqlParameter("@code", code));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@name", name));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@isCostProfit", isCostProfit));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@company", company));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@status", status));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@user", user));

                        
                        string Qry = @"INSERT INTO COMP_COST_PROFIT_CENTER(COMPANY_ID,IS_PROFIT_CENTER,COMP_COST_PROFIT_CENTER_CODE,COST_PROFIT_CENTER_NAME,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                                                VALUES(@company,@isCostProfit,@code,@name,@status,@user,NOW(),@user,NOW());";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
       

                        
                    }
                }
                mySqlTrans.Commit();

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
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

        public Boolean Update(string companyId, string cpCenter, string cpCode, string sesstioncode , string cpName, string status, string user)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE COMP_COST_PROFIT_CENTER 
                                SET 
                                    STATUS_CODE = @statusCode,
                                    COST_PROFIT_CENTER_NAME = @cpName,
                                    COMP_COST_PROFIT_CENTER_CODE = @cpCode,
                                    MODIFIED_BY = @user,
                                    MODIFIED_DATE = NOW()
                                WHERE
                                    COMPANY_ID = @companyId
                                        AND IS_PROFIT_CENTER = @cpCenter
                                        AND COMP_COST_PROFIT_CENTER_CODE = @sesstioncode;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId));
                mySqlCmd.Parameters.Add(new MySqlParameter("@cpCenter", cpCenter));
                mySqlCmd.Parameters.Add(new MySqlParameter("@cpCode", cpCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sesstioncode", sesstioncode));
                mySqlCmd.Parameters.Add(new MySqlParameter("@cpName", cpName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", status));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user));
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

        public DataTable GetCostProfitCenterByCompany(string company)
        {
            try
            {
                dataTable.Rows.Clear();
                string mySqlQry = @"SELECT 
                                    c.COMP_NAME,
                                    CASE
                                        WHEN cc.IS_PROFIT_CENTER = '0' THEN 'Profit Center'
                                        WHEN cc.IS_PROFIT_CENTER = '1' THEN 'Cost Center'
                                    END AS IS_PROFIT_CENTER,
                                    cc.COMP_COST_PROFIT_CENTER_CODE,
                                    COST_PROFIT_CENTER_NAME,
                                    CASE
                                        WHEN cc.STATUS_CODE = '1' THEN 'Active'
                                        WHEN cc.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    COMP_COST_PROFIT_CENTER cc,
                                    COMPANY c
                                WHERE
                                    cc.COMPANY_ID = c.COMPANY_ID
                                        AND cc.COMPANY_ID = '" + company + "';";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlQry, mySqlCon);
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

        public String getStatus(string code)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM COMP_COST_PROFIT_CENTER WHERE COMP_COST_PROFIT_CENTER_CODE = '" + code + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }
    }
}
