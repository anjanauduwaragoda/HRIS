using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainerCompetencyAreaDataHandler : TemplateDataHandler
    {

        public bool Insert(string competencyArea,string description,string status, string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sCOMPETENCY_ID = nserialcode.getserila(mySqlCon, "TR");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCOMPETENCY_ID", sCOMPETENCY_ID.Trim() == "" ? (object)DBNull.Value : sCOMPETENCY_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyArea", competencyArea.Trim() == "" ? (object)DBNull.Value : competencyArea.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"INSERT INTO TRAINER_COMPETENCY_AREA (COMPETENCY_ID,NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
						               VALUES(@sCOMPETENCY_ID,@competencyArea,@description,@status,@user,now(),@user,now());";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isInsert = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isInsert;
        }

        public bool Update(string competencyId, string competencyArea, string description, string status, string user)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyArea", competencyArea.Trim() == "" ? (object)DBNull.Value : competencyArea.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"UPDATE TRAINER_COMPETENCY_AREA SET NAME= @competencyArea,
                                    DESCRIPTION = @description,
                                    STATUS_CODE = @status,
                                    MODIFIED_BY = @user,
                                    MODIFIED_DATE = now()
                                    WHERE COMPETENCY_ID = @competencyId;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

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

        public DataTable getAllCompetencies()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    COMPETENCY_ID, NAME, DESCRIPTION,CASE WHEN STATUS_CODE='1' THEN 'Active' WHEN STATUS_CODE='0' THEN 'Inactive' END AS STATUS_CODE
                                FROM
                                    TRAINER_COMPETENCY_AREA ORDER BY NAME;";

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
