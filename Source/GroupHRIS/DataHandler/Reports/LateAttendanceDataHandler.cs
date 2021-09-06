using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Reports
{
    public class LateAttendanceDataHandler : TemplateDataHandler
    {
        public DataTable populateAllByDate(string Date)
        {
            string sMySqlString = "";
            string[] DateArr = Date.Split('/');
            Date = DateArr[2] + '/' + DateArr[1] + '/' + DateArr[0];

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@Date", Date.Trim() == "" ? (object)DBNull.Value : Date.Trim()));
                
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT
                                            E.EPF_NO,
                                            E.FULL_NAME,
                                            DV.DIV_NAME,
                                            ES.IN_TIME,
                                            ES.LATE_MINUTES,
                                            ES.OUT_TIME,
                                            CAST(TIME_FORMAT(TIMEDIFF('17:00',ES.OUT_TIME),'%i:%s') AS CHAR) AS EARLY_OUT_MINUTES,
                                            CAST(TIME_FORMAT(TIMEDIFF(ES.OUT_TIME,ES.IN_TIME),'%h:%i') AS CHAR) AS WORKING_HOURS,
                                            ES.LATE_MINUTES + TIME_FORMAT(TIMEDIFF('17:00',ES.OUT_TIME),'%i:%s') AS SHORTAGE
                                    FROM 
                                            ATTENDANCE_SUMMARY ES,
                                            EMPLOYEE E,
                                            DIVISION DV
                                    WHERE 
                                            ES.EMPLOYEE_ID = E.EMPLOYEE_ID AND 
                                            E.DIVISION_ID = DV.DIVISION_ID AND 
                                            ES.IN_TIME > '08:30' AND 
                                            OUT_TIME < '17:00' AND 
                                            ES.IN_DATE = @Date
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateAllByDateNCompany(string Date,string CompanyID)
        {
            string sMySqlString = "";
            string[] DateArr = Date.Split('/');
            Date = DateArr[2] + '/' + DateArr[1] + '/' + DateArr[0];

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@Date", Date.Trim() == "" ? (object)DBNull.Value : Date.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyID", CompanyID.Trim() == "" ? (object)DBNull.Value : CompanyID.Trim()));

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT
                                            E.EPF_NO,
                                            E.FULL_NAME,
                                            DV.DIV_NAME,
                                            ES.IN_TIME,
                                            ES.LATE_MINUTES,
                                            ES.OUT_TIME,
                                            CAST(TIME_FORMAT(TIMEDIFF('17:00',ES.OUT_TIME),'%i:%s') AS CHAR) AS EARLY_OUT_MINUTES,
                                            CAST(TIME_FORMAT(TIMEDIFF(ES.OUT_TIME,ES.IN_TIME),'%h:%i') AS CHAR) AS WORKING_HOURS,
                                            ES.LATE_MINUTES + TIME_FORMAT(TIMEDIFF('17:00',ES.OUT_TIME),'%i:%s') AS SHORTAGE
                                    FROM 
                                            ATTENDANCE_SUMMARY ES,
                                            EMPLOYEE E,
                                            DIVISION DV
                                    WHERE 
                                            ES.EMPLOYEE_ID = E.EMPLOYEE_ID AND 
                                            E.DIVISION_ID = DV.DIVISION_ID AND 
                                            ES.IN_TIME > '08:30' AND 
                                            OUT_TIME < '17:00' AND 
                                            ES.IN_DATE = @Date AND
                                            ES.COMPANY_ID = @CompanyID
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateCompanies()
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                sMySqlString = @"
                                    SELECT 
                                        C.COMPANY_ID, C.COMP_NAME
                                    FROM
                                        COMPANY C;
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
