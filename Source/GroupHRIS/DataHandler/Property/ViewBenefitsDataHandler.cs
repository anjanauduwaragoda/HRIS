using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Property
{
    public class ViewBenefitsDataHandler : TemplateDataHandler
    {
        public DataTable viewBenefit(DataTable propertyBucket)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                DataTable dt = new DataTable();

                dt.Columns.Add("DESCRIPTION");
                dt.Columns.Add("REFERENCE_NO");
                dt.Columns.Add("MODEL");
                dt.Columns.Add("SERIAL_NO");
                dt.Columns.Add("PROPERTY_ID");
                dt.Columns.Add("PROPERTY_TYPE_ID");

                foreach (DataRow dr in propertyBucket.Rows)
                {
                    string propertyId = dr["PROPERTY_ID"].ToString();
                    string propertyTypeId = dr["PROPERTY_TYPE_ID"].ToString();

                    string Qry = @"SELECT 
                                        pt.DESCRIPTION, p.REFERENCE_NO, p.MODEL, p.SERIAL_NO,p.PROPERTY_ID,p.PROPERTY_TYPE_ID
                                    FROM
                                        PROPERTY p,
                                        PROPERTY_TYPE pt
                                    WHERE
                                        pt.TYPE_ID = p.PROPERTY_TYPE_ID
                                            AND p.PROPERTY_ID = '" + propertyId +"'AND p.PROPERTY_TYPE_ID = '"+ propertyTypeId +"';";

                    mySqlCmd.CommandText = Qry;
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                    dataAdapter.Fill(dataTable);

                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow drv = dt.NewRow();
                    drv["DESCRIPTION"] = dataTable.Rows[i]["DESCRIPTION"].ToString();
                    drv["REFERENCE_NO"] = dataTable.Rows[i]["REFERENCE_NO"].ToString();
                    drv["MODEL"] = dataTable.Rows[i]["MODEL"].ToString();
                    drv["SERIAL_NO"] = dataTable.Rows[i]["SERIAL_NO"].ToString();
                    drv["PROPERTY_ID"] = dataTable.Rows[i]["PROPERTY_ID"].ToString();
                    drv["PROPERTY_TYPE_ID"] = dataTable.Rows[i]["PROPERTY_TYPE_ID"].ToString();
                    dt.Rows.Add(drv);
                }

                return dt;
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

        public bool isAvailable(DataTable dt, string tId, string pId)
        {
            bool status = false;

            try
            {
                string qry = @"SELECT count(*) FROM dt WHERE PROPERTY_TYPE_ID = '" + tId + "' AND PROPERTY_ID = '" + pId + "'";
                return status;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool removeBenefit(string lineId, string user, string propertyId,string remarks)
        { 
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                string updateEmpProperty = @"UPDATE EMPLOYEE_PROPERTY_DETAILS 
                                        SET 
                                            PROPERTY_STATUS = '" + Constants.CON_REMOVE_STATUS + @"',
                                            REMOVED_REASON = '" + remarks + @"',
                                            MODIFIED_BY = '" + user+@"',
                                            MODIFIED_DATE = NOW()
                                        WHERE
                                            LINE_ID = '" + lineId + "';";

                mySqlCmd.CommandText = updateEmpProperty;

                mySqlCmd.ExecuteNonQuery();

                string updateProperty = @"UPDATE PROPERTY p
		                                    JOIN EMPLOYEE_PROPERTY_DETAILS e
		                                    ON e.PROPERTY_ID = p.PROPERTY_ID
		                                    SET p.STATUS_CODE = '"+ Constants.CON_UTILIZED_STATUS+@"'
		                                    WHERE e.PROPERTY_ID = '" + propertyId + "';";

                mySqlCmd.CommandText = updateProperty;

                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Transaction = mySqlTrans;

                status = true;
            }
            catch (Exception ex)
            {
                // return ex;
            }
            //finally
            //{
            //    mySqlCmd.Parameters.Clear();
            //    if (mySqlCon.State == ConnectionState.Open)
            //    {
            //        mySqlCon.Close();
            //    }
            //}

            mySqlTrans.Commit();
            mySqlTrans.Dispose();
            mySqlCmd.Dispose();
            mySqlCon.Close();

            return status;
        }

    }
}
