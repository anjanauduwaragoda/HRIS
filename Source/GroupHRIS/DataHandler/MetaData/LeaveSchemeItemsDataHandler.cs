using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class LeaveSchemeItemsDataHandler:TemplateDataHandler
    {
        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 08-07-2014
        // this function is written to used at webFrmEmploeeLeaveScheme.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave types and amounts  for a given leave scheme
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getLeveSchemeItems(string leaveSchemeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT lt.LEAVE_TYPE_NAME,lsi.NUMBER_OF_DAYS " +
                                      " FROM LEAVE_SCHEME_ITEM as lsi,LEAVE_TYPE as lt " +
                                      " WHERE lsi.LEAVE_TYPE_ID =lt.LEAVE_TYPE_ID and lsi.LEAVE_SCHEME_ID ='" + leaveSchemeId.Trim() + "'";

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
