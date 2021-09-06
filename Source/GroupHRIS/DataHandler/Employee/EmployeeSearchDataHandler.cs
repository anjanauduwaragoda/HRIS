using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.Employee
{
    public class EmployeeSearchDataHandler : TemplateDataHandler
    {

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get all employees 
        ///</summary>
        ///<param name="sStatus">Pass a blank string to query for all the existing employee status codes</param>
        //----------------------------------------------------------------------------------------
        public DataTable populate(string sStatus, string sName, string sDesignationId)
        {
            string sStatusFilter = "";
            string sNameFilter = "";
            string sDesignationFilter = "";

            if (sStatus.Trim().Length > 0)
                    sStatusFilter = " where EMPLOYEE_STATUS = '" + sStatus + "'";

            if (sName.Trim().Length > 0)
                if (sStatus.Trim().Length > 0) 
                    sNameFilter = " and upper(KNOWN_NAME) LIKE '%" + sName.ToUpper() + "%'";
                else
                    sNameFilter = " where upper(KNOWN_NAME) LIKE '%" + sName.ToUpper() + "%'";


            if (sDesignationId.Trim().Length > 0)
                if ( (sStatus.Trim().Length > 0)  || (sName.Trim().Length > 0) )
                    sDesignationFilter = " and DESIGNATION_ID = '" + sDesignationId + "'";
                else
                    sDesignationFilter = " where DESIGNATION_ID = '" + sDesignationId + "'";


            try
            {
                dataTable.Rows.Clear();
                /*
                string sMySqlString = " SELECT EMPLOYEE_ID, EPF_NO, TITLE, FULL_NAME, EMP_NIC, DESCRIPTION, COMP_NAME, DEPT_NAME, DIV_NAME   " +
                                      " FROM v_employee_search " + sStatusFilter;
                */
                string sMySqlString = " SELECT *  " +
                                      " FROM v_employee_search "
                                      + sStatusFilter
                                      + sNameFilter
                                      + sDesignationFilter;
                                      

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get employees for a given company
        ///</summary>
        /// <param name="sStatus">Pass a blank string to query for all the existing employee status codes</param>
        //----------------------------------------------------------------------------------------
        public DataTable populate(string sCompanyId, string sStatus, string sName, string sDesignationId)
        {
            string sStatusFilter = "";
            string sNameFilter = "";
            string sDesignationFilter = "";

            if (sStatus.Trim().Length > 0)
                sStatusFilter = " and EMPLOYEE_STATUS = '" + sStatus + "'";

            if (sName.Trim().Length > 0)
                sNameFilter = " and upper(KNOWN_NAME) LIKE '%" + sName.ToUpper() + "%'";

            if (sDesignationId.Trim().Length > 0)
                sDesignationFilter = " and DESIGNATION_ID = '" + sDesignationId + "'";


            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT * " +
                                      " FROM v_employee_search " +
                                      " where COMPANY_ID = '" + sCompanyId.Trim() + "'"
                                      + sStatusFilter
                                      + sNameFilter
                                      + sDesignationFilter;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get employees for a given set of company , department
        ///</summary>
        ///<param name="sStatus">Pass a blank string to query for all the existing employee status codes</param>
        //----------------------------------------------------------------------------------------
        public DataTable populate(string sCompanyId, string sDeptId, string sStatus, string sName, string sDesignationId)
        {
            string sStatusFilter = "";
            string sNameFilter = "";
            string sDesignationFilter = "";

            if (sStatus.Trim().Length > 0)
                sStatusFilter = " and EMPLOYEE_STATUS = '" + sStatus + "'";

            if (sName.Trim().Length > 0)
                sNameFilter = " and upper(KNOWN_NAME) LIKE '%" + sName.ToUpper() + "%'";

            if (sDesignationId.Trim().Length > 0)
                sDesignationFilter = " and DESIGNATION_ID = '" + sDesignationId + "'";


            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT * " +
                                      " FROM v_employee_search " +
                                      " where COMPANY_ID = '" + sCompanyId.Trim() + "'" +
                                      " and DEPT_ID = '" + sDeptId.Trim() + "'"
                                      + sStatusFilter
                                      + sNameFilter
                                      + sDesignationFilter;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get employees for a given set of company, department , division
        ///</summary>
        ///<param name="sStatus">Pass a blank string to query for all the existing employee status codes</param>
        //----------------------------------------------------------------------------------------
        public DataTable populate(string sCompanyId, string sDeptId, string sDivision, string sStatus, string sName, string sDesignationId)
        {
            string sStatusFilter = "";
            string sNameFilter = "";
            string sDesignationFilter = "";

            if (sStatus.Trim().Length > 0)
                sStatusFilter = " and EMPLOYEE_STATUS = '" + sStatus + "'";

            if (sName.Trim().Length > 0)
                sNameFilter = " and upper(KNOWN_NAME) LIKE '%" + sName.ToUpper() + "%'";

            if (sDesignationId.Trim().Length > 0)
                sDesignationFilter = " and DESIGNATION_ID = '" + sDesignationId + "'";

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT * " +
                                      " FROM v_employee_search " +
                                      " where COMPANY_ID = '" + sCompanyId.Trim() + "'" +
                                      " and DEPT_ID = '" + sDeptId.Trim() + "'" +
                                      " and DIVISION_ID = '" + sDivision.Trim() + "'" 
                                      + sStatusFilter
                                      + sNameFilter
                                      + sDesignationFilter;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get Employee by EPF Number (All the other search parameters are ignored) 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public DataTable populateByEPF(string sEPFNo)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT *   " +
                                      " FROM v_employee_search " +
                                      " WHERE EPF_NO = '" + sEPFNo +"'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get Employee by NIC Number (All the other search parameters are ignored) 
        ///</summary>
        ///<param name="sNICNo">Wildcard search enabled (LIKE %)'</param>
        //----------------------------------------------------------------------------------------
        public DataTable populateByNIC(string sNICNo)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT *  " +
                                      " FROM v_employee_search " +
                                      " WHERE upper(EMP_NIC) LIKE upper('" + sNICNo + "%') ";

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
