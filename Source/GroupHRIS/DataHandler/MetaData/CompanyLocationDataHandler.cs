using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class CompanyLocationDataHandler:TemplateDataHandler
    {

        //public bool isLocationExist(string location)
        //{ 
            
        //}

        public DataTable getLocationIdLocName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT LOCATION_NAME,LOCATION_ID FROM COMPANY_LOCATION where STATUS='1' AND COMPANY_ID='" + companyId.Trim() + "' order by LOCATION_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isCompanyLocationExist(string companyId, string locationName)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LOCATION_ID FROM COMPANY_LOCATION WHERE COMPANY_ID='" + companyId.Trim() + "' and UPPER(REPLACE(LOCATION_NAME,' ','')) = '" + locationName.Trim() + "'";

                //mySqlCmd.Parameters.Add(new MySqlParameter("@LocationName", locationName));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                mySqlCmd.Parameters.Clear();
                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT  CL.LOCATION_ID,CL.LOCATION_NAME,CL.LOCATION_ADDRESS,CL.PHONE_NUMBER,CL.REMARKS,C.COMP_NAME,CL.STATUS
                                        FROM    COMPANY_LOCATION CL,COMPANY C
                                        WHERE   C.COMPANY_ID=CL.COMPANY_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Status = dataTable.Rows[i]["STATUS"].ToString();
                    if (Status == Constants.STATUS_ACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_ACTIVE_TAG;
                    }
                    else if (Status == Constants.STATUS_INACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_INACTIVE_TAG;
                    }
                    dataTable.Rows[i]["STATUS"] = Status;
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Populate(string CompanyCode)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT  CL.LOCATION_ID,CL.LOCATION_NAME,CL.LOCATION_ADDRESS,CL.PHONE_NUMBER,CL.REMARKS,C.COMP_NAME,CL.STATUS
                                        FROM    COMPANY_LOCATION CL,COMPANY C
                                        WHERE   C.COMPANY_ID='" + CompanyCode + "' AND C.COMPANY_ID=CL.COMPANY_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Status = dataTable.Rows[i]["STATUS"].ToString();
                    if (Status == Constants.STATUS_ACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_ACTIVE_TAG;
                    }
                    else if (Status == Constants.STATUS_INACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_INACTIVE_TAG;
                    }
                    dataTable.Rows[i]["STATUS"] = Status;
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateCompany()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT COMPANY_ID,COMP_NAME FROM COMPANY;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateCompany(string CompanyCode)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT COMPANY_ID,COMP_NAME FROM COMPANY WHERE COMPANY_ID='" + CompanyCode + "';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(string LocationName,string  LocationAddress,string PhoneNumber,string Remarks,string CompanyID, string Status)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler oSerialHandler = new SerialHandler();
                string locationID = oSerialHandler.getserila(mySqlCon, Constants.COMPANY_LOCATION_ID_STAMP);


                string Qry = @"INSERT INTO COMPANY_LOCATION(LOCATION_ID,LOCATION_NAME,LOCATION_ADDRESS,PHONE_NUMBER,REMARKS,COMPANY_ID,STATUS) 
                                VALUES(@LOCATION_ID,@LOCATION_NAME,@LOCATION_ADDRESS,@PHONE_NUMBER,@REMARKS,@COMPANY_ID,@Status);";

                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", locationID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_NAME", LocationName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ADDRESS", LocationAddress));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PHONE_NUMBER", PhoneNumber));
                mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Status", Status));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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

        public void Update(string LocationName, string LocationAddress, string PhoneNumber, string Remarks, string CompanyID,string LocationID, string Status)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"UPDATE   COMPANY_LOCATION 
                                SET     LOCATION_NAME=@LOCATION_NAME,LOCATION_ADDRESS=@LOCATION_ADDRESS,PHONE_NUMBER=@PHONE_NUMBER,REMARKS=@REMARKS,COMPANY_ID=@COMPANY_ID,STATUS=@Status  
                                WHERE   LOCATION_ID=@LOCATION_ID";

                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_NAME", LocationName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ADDRESS", LocationAddress));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PHONE_NUMBER", PhoneNumber));
                mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", LocationID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Status", Status));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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
