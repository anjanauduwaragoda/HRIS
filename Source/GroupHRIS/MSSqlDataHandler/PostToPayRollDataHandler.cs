
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace MSSqlDataHandler
{
    public class PostToPayRollDataHandler : MSSqlTemplateDataHandler
    {
        public PostToPayRollDataHandler()
        { 
        
        }

        public PostToPayRollDataHandler(string sCompanyId)
            : base(sCompanyId)
        {
            
        }

        public Boolean TransferTransaction(string companyId, DataTable table)
        {
            try
            {
                Boolean status = false;
                if (SqlCon.State == ConnectionState.Closed)
                {
                    SqlCon.Open();
                }

                //Delete all records from data base Acc
                string Qry = "DELETE FROM Imports;";
                SqlCmd.CommandText = Qry;
                SqlCmd.ExecuteNonQuery();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string SempID = Convert.ToString(table.Rows[i]["EPF_NO"]);
                    string SType = Convert.ToString(table.Rows[i]["CATEGORY"]);
                    string StypeID = Convert.ToString(table.Rows[i]["TYPE_ID"]);
                    string Svalue = Convert.ToString(table.Rows[i]["FINALIZED_AMOUNT"]);

                    Qry = "INSERT INTO Imports(Type,EmpId,TypeId,Value) VALUES('" + SType + "','" + SempID + "'," + StypeID + "," + Svalue + ");";
                    SqlCmd.CommandText = Qry;
                    SqlCmd.ExecuteNonQuery();

                }
                    status = true;
                    return status;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }
        }

    }
}
