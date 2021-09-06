using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.MetaData
{
    public class DesignationEmployeeDataHandler :TemplateDataHandler
    {
        public DataTable getDesignationsForCompany(string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT DESIGNATION_ID,DESIGNATION_NAME FROM  EMPLOYEE_DESIGNATION WHERE COMPANY_ID =  '" + sCompID + "'; ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
