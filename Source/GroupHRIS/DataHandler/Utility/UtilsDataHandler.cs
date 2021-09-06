using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;


namespace DataHandler.Utility
{
    public class UtilsDataHandler : TemplateDataHandler
    {
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Anjana 23/08/2016 
        /// For Checking Duplicate Entries
        ///</summary>
        ///<param name="sValue">Pass the value string to query for checking similar existance </param>
        ///<param name="sValueColumn">Pass the Name of the Column string that value belongs </param>
        ///<param name="sTable">Pass the Name of the Table string </param>        
        //----------------------------------------------------------------------------------------
        public Boolean isDuplicateExist(string sValue, string sValueColumn, string sTable)
        {
             
             Boolean isExsists = false;

             string sUpperValue = "";             

             try
             {                
                 mySqlCon.Open();
                
                 dataTable = new DataTable();

                 sUpperValue = sValue.Replace(" ", "").ToUpper().Trim();


                 string queryStr = " SELECT * FROM " + sTable + " where upper(REPLACE(" + sValueColumn + ", ' ', '')) = '" + sUpperValue.Trim() + "'";

                 MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
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

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Anjana 23/08/2016 
        /// For Checking Duplicate Entries
        ///</summary>
        ///<param name="sValue">Pass the value string to query for checking similar existance </param>
        ///<param name="sValueColumn">Pass the Name of the Column string that value belongs </param>
        ///<param name="sTable">Pass the Name of the Table string </param>
        ///<param name="sIdValue">Pass the value of the primary key string </param>
        ///<param name="sIdColumn">Pass the Name of the Primary key column </param>         
        //----------------------------------------------------------------------------------------
        public Boolean isDuplicateExist(string sValue, string sValueColumn, string sTable, string sIdValue, string sIdColumn)
        {

            Boolean isExsists = false;
            string sUpperValue = "";

            try
            {
                mySqlCon.Open();

                dataTable = new DataTable();               

                sUpperValue = sValue.Replace(" ", "").ToUpper().Trim();

                string queryStr = " SELECT * FROM " + sTable + " where upper(REPLACE(" + sValueColumn.Trim() + ", ' ', '')) = '" + sUpperValue.Trim() + "' and " + sIdColumn.Trim() + " <> '" + sIdValue.Trim() + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
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



        /* <Update Assessment Status>
         * 2016_09_29
         * YASINTHA
         * To call stored Procedure "call sp_UpdateAssessmentStatus" 
         * Passing values : Assessment Id, Employee Id, Year of Assessmen
         * This is used to update ASSESSED_EMPLOYEES Table Status code according to Availability 
         * of each assessment for each employee and assessment current status.
         */

        public Boolean updateAssessmentStatus(string assessmentId, string employeeId, string Year)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_UpdateAssessmentStatus";
                mySqlCmd.CommandType = CommandType.StoredProcedure;

                //MySqlParameter param = new MySqlParameter();
                //param = mySqlCmd.Parameters.Add("@SeqName", SqlDbType.NVarChar);
                //param.Direction = ParameterDirection.Input;
                //param.Value = "SeqName"; // var result = returnParameter.Value;

                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("assessmentId", assessmentId));
                mySqlCmd.Parameters.Add(new MySqlParameter("empId", employeeId));
                mySqlCmd.Parameters.Add(new MySqlParameter("yearId", Year));

                mySqlCmd.CommandType = CommandType.StoredProcedure;
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
