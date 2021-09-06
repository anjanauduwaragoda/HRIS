using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ConnectionManager;

namespace MSSqlDataHandler
{
    public class MSSqlTemplateDataHandler
    {

        protected SqlConnection SqlCon;
        protected SqlCommand SqlCmd;
        protected DataTable dataTable;

        public MSSqlTemplateDataHandler()
        { 
        
        }

        public MSSqlTemplateDataHandler(string sCompanyId)
        {
            try
            {
                SqlCon = MSSqlConnectionManager.getConnection(sCompanyId);
                SqlCmd = SqlCon.CreateCommand();
                dataTable = new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
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
