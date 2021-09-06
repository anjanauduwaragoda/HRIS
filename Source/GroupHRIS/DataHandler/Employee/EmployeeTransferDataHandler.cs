using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;
using NLog;

namespace DataHandler.Employee
{
    public class EmployeeTransferDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Transfer Employees 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String employeeId,
                      String fromCompanyCode,
                      String fromDepartmentID,
                      String frmDivisionID,
                      String toCompanyCode,
                      String toDepartmentID,
                      String toDivisionID,
                      String startDate,
                      String remarks,
                      String userID,
                      String transferType,
                      String fromBranchID,
                      String fromCC,
                      String fromPC,
                      String toBranchID,
                      String toCC,
                      String toPC,
                      String fromepf,
                      String fromDesignation,
                      String toepf,
                      String toDesignation,
                      String fromETF,
                      String frpt1,
                      String frpt2,
                      String frpt3,
                      String trpt1,
                      String trpt2,
                      String trpt3)
        {
            Boolean bInserted = false;
            Boolean bUpdated = false;


            String statusCode = Constants.STATUS_ACTIVE_VALUE;

            EmployeeDataHandler dhEmployee = new EmployeeDataHandler();

            mySqlCon.Open();
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;
            mySqlTrans = mySqlCon.BeginTransaction();
            mySqlCmd.Transaction = mySqlTrans;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromCompanyCode", fromCompanyCode.Trim() == "" ? (object)DBNull.Value : fromCompanyCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromDepartmentID", fromDepartmentID.Trim() == "" ? (object)DBNull.Value : fromDepartmentID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@frmDivisionID", frmDivisionID.Trim() == "" ? (object)DBNull.Value : frmDivisionID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromepf", fromepf.Trim() == "" ? (object)DBNull.Value : fromepf.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromDesignation", fromDesignation.Trim() == "" ? (object)DBNull.Value : fromDesignation.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toCompanyCode", toCompanyCode.Trim() == "" ? (object)DBNull.Value : toCompanyCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDepartmentID", toDepartmentID.Trim() == "" ? (object)DBNull.Value : toDepartmentID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDivisionID", toDivisionID.Trim() == "" ? (object)DBNull.Value : toDivisionID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toepf", toepf.Trim() == "" ? (object)DBNull.Value : toepf.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDesignation", toDesignation.Trim() == "" ? (object)DBNull.Value : toDesignation.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@startDate", startDate.Trim() == "" ? (object)DBNull.Value : startDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@transferType", transferType.Trim() == "" ? (object)DBNull.Value : transferType.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@fromBranchID", fromBranchID.Trim() == "" ? (object)DBNull.Value : fromBranchID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromCC", fromCC.Trim() == "" ? (object)DBNull.Value : fromCC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromPC", fromPC.Trim() == "" ? (object)DBNull.Value : fromPC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toBranchID", toBranchID.Trim() == "" ? (object)DBNull.Value : toBranchID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toCC", toCC.Trim() == "" ? (object)DBNull.Value : toCC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toPC", toPC.Trim() == "" ? (object)DBNull.Value : toPC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromETF", fromETF.Trim() == "" ? (object)DBNull.Value : fromETF.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@frpt1", frpt1.Trim() == "" ? (object)DBNull.Value : frpt1.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@frpt2", frpt2.Trim() == "" ? (object)DBNull.Value : frpt2.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@frpt3", frpt3.Trim() == "" ? (object)DBNull.Value : frpt3.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt1", trpt1.Trim() == "" ? (object)DBNull.Value : trpt1.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt2", trpt2.Trim() == "" ? (object)DBNull.Value : trpt2.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt3", trpt3.Trim() == "" ? (object)DBNull.Value : trpt3.Trim()));


           

            //@fromBranchID, @fromCC, @fromPC, @toBranchID, @toCC, @toPC

            try
            {


                
                //--------------------------------------------------------------------------------------------------------------
                //Insert to EMPLOYEE  table
                //--------------------------------------------------------------------------------------------------------------
                //bUpdated = dhEmployee.UpdateTransfer(mySqlCon, employeeId, toCompanyCode, toDepartmentID, toDivisionID, toBranchID, userID, toCC, toPC, toDesignation, toepf, trpt1, trpt2, trpt3);


                sMySqlString = "UPDATE EMPLOYEE SET COMPANY_ID ='" + toCompanyCode + "', DEPT_ID='" + toDepartmentID + "', DIVISION_ID='" + toDivisionID + "', BRANCH_ID='" + toBranchID + "',COST_CENTER='" + toCC + "',PROFIT_CENTER='" + toPC + "', " +
                               " MODIFIED_BY='" + userID + "', MODIFIED_DATE=now(), STATUS_CODE='1', DESIGNATION_ID='" + toDesignation + "', EPF_NO='" + toepf + "', ETF_NO='" + toepf + "', REPORT_TO_1 = '" + trpt1 + "', REPORT_TO_2 = '" + trpt2 + "', REPORT_TO_3 = '" + trpt3 + "' " +
                               " WHERE EMPLOYEE_ID='" + employeeId + "'";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                sMySqlString = "INSERT INTO EMPLOYEE_TRNSFERS(EMPLOYEE_ID,  FROM_COMPANY_ID,    FROM_DEPT_ID,       FROM_DIVISION_ID,   TO_COMPANY_ID,  TO_DEPT_ID,         TO_DIVISION_ID, START_DATE, TRANSFER_TYPE,  STATUS_CODE,    REMARKS,    FROM_BRANCH_ID, FROM_CC,    FROM_PC,    TO_BRANCH_ID,   TO_CC,  TO_PC,  ADDED_BY,   ADDED_DATE, MODIFIED_BY,    MODIFIED_DATE,  FROM_EPF,   TO_EPF, FROM_DESIGNATION,   TO_DESIGNATION, FROM_ETF,   TO_ETF, FROM_RPT_1, FROM_RPT_2, FROM_RPT_3, TO_RPT_1,   TO_RPT_2,   TO_RPT_3) " +
                                " VALUES                     (@employeeId,  @fromCompanyCode,   @fromDepartmentID,  @frmDivisionID,     @toCompanyCode, @toDepartmentID,    @toDivisionID,  @startDate, @transferType,  @statusCode,    @remarks,   @fromBranchID,  @fromCC,    @fromPC,    @toBranchID,    @toCC,  @toPC,  @userID,    now(),      @userID,        now(),          @fromepf,   @toepf, @fromDesignation,   @toDesignation, @fromETF,   @toepf, @frpt1,     @frpt2,     @frpt3,     @trpt1,     @trpt2,     @trpt3)";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                mySqlTrans.Commit();

                bInserted = true;
            }
            catch (Exception ex)
            {


                try
                {
                    mySqlTrans.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            finally
            {
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                dhEmployee = null;
            }

            return bInserted;
        }


        public void update(string empid, string transferid, string companyid, string deptid, string divid, string branchid, string epf, string etf, string designationid, string cc, string pc, string rpt1, string rpt2, string rpt3, string startdate, string remarks, string loggeduser)
        {
            mySqlCon.Open();
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;
            mySqlTrans = mySqlCon.BeginTransaction();
            mySqlCmd.Transaction = mySqlTrans;


            try
            {

                mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@companyid", companyid.Trim() == "" ? (object)DBNull.Value : companyid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@deptid", deptid.Trim() == "" ? (object)DBNull.Value : deptid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@divid", divid.Trim() == "" ? (object)DBNull.Value : divid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branchid", branchid.Trim() == "" ? (object)DBNull.Value : branchid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@epf", epf.Trim() == "" ? (object)DBNull.Value : epf.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@etf", etf.Trim() == "" ? (object)DBNull.Value : etf.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@designationid", designationid.Trim() == "" ? (object)DBNull.Value : designationid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@cc", cc.Trim() == "" ? (object)DBNull.Value : cc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@pc", pc.Trim() == "" ? (object)DBNull.Value : pc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt1", rpt1.Trim() == "" ? (object)DBNull.Value : rpt1.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt2", rpt2.Trim() == "" ? (object)DBNull.Value : rpt2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt3", rpt3.Trim() == "" ? (object)DBNull.Value : rpt3.Trim()));


                sMySqlString = @"

                                    UPDATE 
                                        EMPLOYEE 
                                    SET 
                                        COMPANY_ID = @companyid, DEPT_ID = @deptid, DIVISION_ID = @divid, BRANCH_ID = @branchid, EPF_NO = @epf, ETF_NO = @etf,
                                        DESIGNATION_ID = @designationid, COST_CENTER = @cc, PROFIT_CENTER = @pc, REPORT_TO_1 = @rpt1, REPORT_TO_2 = @rpt2, REPORT_TO_3 = @rpt3, MODIFIED_BY='" + loggeduser + @"', MODIFIED_DATE = NOW()
                                    WHERE 
                                        EMPLOYEE_ID = @empid;
                                    
                                ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@transferid", transferid.Trim() == "" ? (object)DBNull.Value : transferid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@companyid", companyid.Trim() == "" ? (object)DBNull.Value : companyid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@deptid", deptid.Trim() == "" ? (object)DBNull.Value : deptid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@divid", divid.Trim() == "" ? (object)DBNull.Value : divid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branchid", branchid.Trim() == "" ? (object)DBNull.Value : branchid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@epf", epf.Trim() == "" ? (object)DBNull.Value : epf.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@etf", etf.Trim() == "" ? (object)DBNull.Value : etf.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@designationid", designationid.Trim() == "" ? (object)DBNull.Value : designationid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@cc", cc.Trim() == "" ? (object)DBNull.Value : cc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@pc", pc.Trim() == "" ? (object)DBNull.Value : pc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt1", rpt1.Trim() == "" ? (object)DBNull.Value : rpt1.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt2", rpt2.Trim() == "" ? (object)DBNull.Value : rpt2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rpt3", rpt3.Trim() == "" ? (object)DBNull.Value : rpt3.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@startdate", startdate.Trim() == "" ? (object)DBNull.Value : startdate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));






                sMySqlString = @"

                                    UPDATE 
                                        EMPLOYEE_TRNSFERS 
                                    SET 
                                        TO_COMPANY_ID = @companyid, TO_DEPT_ID = @deptid, TO_DIVISION_ID = @divid, TO_BRANCH_ID = @branchid, TO_EPF = @epf, TO_ETF = @etf, 
                                        TO_DESIGNATION = @designationid, TO_CC = @cc, TO_PC = @pc, TO_RPT_1 = @rpt1, TO_RPT_2 = @rpt2, TO_RPT_3 = @rpt3, START_DATE = @startdate, REMARKS = @remarks, MODIFIED_BY = '" + loggeduser + @"', MODIFIED_DATE = NOW()
                                    WHERE 
                                        TRANS_ID = @transferid;	
                                    
                                ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                mySqlTrans.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    mySqlTrans.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            finally
            {
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public DataTable populate(string empId)
        {
            try
            {
                dataTable = new DataTable();
                //string sMySqlString = " SELECT * FROM EMPLOYEE_TRNSFERS " +
                //                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                //                      " order by TRANS_ID ";

                string sMySqlString = @"
                                        SELECT 
                                            ET.TRANS_ID, 
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = ET.FROM_COMPANY_ID) AS 'FROM_COMPANY',
                                            (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ET.FROM_DEPT_ID) AS 'FROM_DEPARTMENT',
                                            (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ET.FROM_DIVISION_ID) AS 'FROM_DIVISION',
                                            ET.FROM_EPF,ET.FROM_ETF,
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = ET.TO_COMPANY_ID) AS 'TO_COMPANY',
                                            (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ET.TO_DEPT_ID) AS 'TO_DEPARTMENT',
                                            (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ET.TO_DIVISION_ID) AS 'TO_DIVISION',
                                            ET.TO_EPF,ET.TO_ETF,CONVERT(ET.START_DATE,CHAR) AS 'TRANSFER_DATE'
                                        FROM 
                                            EMPLOYEE_TRNSFERS ET
                                        WHERE 
                                            ET.EMPLOYEE_ID='" + empId + @"'
                                        ORDER BY 
                                            ET.TRANS_ID DESC
                                    ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateSupervisors(string empId)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                REPORT_TO_1, REPORT_TO_2, REPORT_TO_3
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empId + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateFromDataCodes(string empId)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                E.COMPANY_ID,
                                                E.DEPT_ID,
                                                E.DIVISION_ID,
                                                E.BRANCH_ID,
                                                E.EPF_NO,
                                                E.ETF_NO,
                                                E.DESIGNATION_ID,
                                                E.COST_CENTER,
                                                E.PROFIT_CENTER
                                            FROM
                                                EMPLOYEE E
                                            WHERE
                                                E.EMPLOYEE_ID = '" + empId + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateTotransferDetails(string transID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                ET.TO_COMPANY_ID, ET.TO_DEPT_ID, ET.TO_DIVISION_ID, ET.TO_BRANCH_ID, ET.TO_EPF, ET.TO_DESIGNATION, ET.TO_CC, ET.TO_PC, 
                                                ET.TO_RPT_1,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = TO_RPT_1) AS 'TO_RPT_1_NAME', 
                                                ET.TO_RPT_2,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = TO_RPT_2) AS 'TO_RPT_2_NAME', 
                                                ET.TO_RPT_3,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = TO_RPT_3) AS 'TO_RPT_3_NAME', 
                                                CONVERT(ET.START_DATE,CHAR) AS 'START_DATE',ET.REMARKS
                                            FROM 
                                                EMPLOYEE_TRNSFERS ET 
                                            WHERE 
                                                ET.TRANS_ID = '" + transID + @"';
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataTable populateFromtransferDetails(string transID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                ET.EMPLOYEE_ID,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = ET.EMPLOYEE_ID) AS 'EMP_NAME',
                                                ET.FROM_RPT_1,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = ET.FROM_RPT_1) AS 'FROM_RPT_1_NAME',
                                                ET.FROM_RPT_2,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = ET.FROM_RPT_2) AS 'FROM_RPT_2_NAME',
                                                ET.FROM_RPT_3,(SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = ET.FROM_RPT_3) AS 'FROM_RPT_3_NAME',
                                                ET.FROM_COMPANY_ID,(SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = ET.FROM_COMPANY_ID) AS 'FROM_COMPANY_NAME',
                                                ET.FROM_DEPT_ID,(SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ET.FROM_DEPT_ID) AS 'FROM_DEPT_NAME',
                                                ET.FROM_DIVISION_ID,(SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ET.FROM_DIVISION_ID) AS 'FROM_DIVISION_NAME',
                                                ET.FROM_BRANCH_ID,(SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = ET.FROM_BRANCH_ID) AS 'FROM_BRANCH_NAME',
                                                ET.FROM_EPF,ET.FROM_ETF,
                                                ET.FROM_DESIGNATION,(SELECT DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION WHERE DESIGNATION_ID = ET.FROM_DESIGNATION) AS 'FROM_DESIGNATION_NAME',
                                                ET.FROM_CC,(SELECT COST_PROFIT_CENTER_NAME FROM COMP_COST_PROFIT_CENTER WHERE COMP_COST_PROFIT_CENTER_CODE = ET.FROM_CC) AS 'FROM_CC_NAME',
                                                ET.FROM_PC,(SELECT COST_PROFIT_CENTER_NAME FROM COMP_COST_PROFIT_CENTER WHERE COMP_COST_PROFIT_CENTER_CODE = ET.FROM_PC) AS 'FROM_PC_NAME'
                                            FROM 
                                                EMPLOYEE_TRNSFERS ET 
                                            WHERE 
                                                ET.TRANS_ID = '" + transID + @"';
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getEmpName(string empId)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                INITIALS_NAME
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empId + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["INITIALS_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCompanyName(string CompanyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                COMP_NAME
                                            FROM
                                                COMPANY
                                            WHERE
                                                COMPANY_ID = '" + CompanyID + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COMP_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getDepartmentName(string DepartmentID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                DEPT_NAME
                                            FROM
                                                DEPARTMENT
                                            WHERE
                                                DEPT_ID = '" + DepartmentID + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["DEPT_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getDivisionName(string Division)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                DIV_NAME
                                            FROM
                                                DIVISION
                                            WHERE
                                                DIVISION_ID = '" + Division + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["DIV_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getBranchName(string Branch)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                BRANCH_NAME
                                            FROM
                                                COMPANY_BRANCH
                                            WHERE
                                                BRANCH_ID = '" + Branch + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["BRANCH_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getDesignationName(string Designation)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                DESIGNATION_NAME
                                            FROM
                                                EMPLOYEE_DESIGNATION
                                            WHERE
                                                DESIGNATION_ID = '" + Designation + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["DESIGNATION_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCCPCName(string CCPCID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
    
                                            SELECT 
                                                COST_PROFIT_CENTER_NAME
                                            FROM
                                                COMP_COST_PROFIT_CENTER
                                            WHERE
                                                COMP_COST_PROFIT_CENTER_CODE = '" + CCPCID + @"';
                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COST_PROFIT_CENTER_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string getEmployeeStatus(string EmpID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"    
                                            SELECT 
                                                (
                                                    SELECT 
                                                        ES.DESCRIPTION 
                                                    FROM 
                                                        EMPLOYEE_STATUS ES 
                                                    WHERE 
                                                        ES.STATUS_CODE = E.EMPLOYEE_STATUS
                                                ) AS 'STATUS' 
                                            FROM 
                                                EMPLOYEE E 
                                            WHERE 
                                                E.EMPLOYEE_ID = '" + EmpID + @"';                                    
                                       ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["STATUS"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable populateEmpProfile(string empId)
        {

            try
            {
                dataTable = new DataTable();

                string sMySqlString = "SELECT 'F' as Trsts,TRANS_ID,start_date,COMP_NAME,DEPT_NAME,DIV_NAME,from_cc,from_pc,REMARKS " +
                                    " FROM EMPLOYEE_TRNSFERS,COMPANY,DEPARTMENT,DIVISION " +
                                    " where from_company_id = COMPANY.company_id " +
                                    " and from_dept_id = DEPARTMENT.DEPT_ID " +
                                    " and from_division_id = DIVISION.DIVISION_ID " +
                                    " and EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                    " and ((TRANSFER_TYPE = '" + Constants.TRANSFER_TYPE_ORDINARY.Trim() + "' and EMPLOYEE_TRNSFERS.status_code = '" + Constants.STATUS_ACTIVE_VALUE.Trim() + "') " +
                                    " or (TRANSFER_TYPE = '" + Constants.TRANSFER_TYPE_ORDINARY.Trim() + "' and EMPLOYEE_TRNSFERS.status_code = '" + Constants.STATUS_INACTIVE_VALUE.Trim() + "') ) " +
                                    " union all " +
                                    " select 'T'  as Trsts,TRANS_ID,start_date,COMP_NAME,DEPT_NAME,DIV_NAME,to_cc,to_pc,REMARKS " +
                                    " FROM EMPLOYEE_TRNSFERS,COMPANY,DEPARTMENT,DIVISION " +
                                    " where to_company_id = COMPANY.company_id " +
                                    " and to_dept_id = DEPARTMENT.DEPT_ID " +
                                    " and to_division_id = DIVISION.DIVISION_ID " +
                                    " and EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                    " and ((TRANSFER_TYPE = '" + Constants.TRANSFER_TYPE_ORDINARY.Trim() + "' and EMPLOYEE_TRNSFERS.status_code = '" + Constants.STATUS_ACTIVE_VALUE.Trim() + "') " +
                                    " or (TRANSFER_TYPE = '" + Constants.TRANSFER_TYPE_ORDINARY.Trim() + "' and EMPLOYEE_TRNSFERS.status_code = '" + Constants.STATUS_INACTIVE_VALUE.Trim() + "') )order by start_date,TRANS_ID,Trsts";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCostCenterByCompany(string companyId)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"SELECT COMP_COST_PROFIT_CENTER_CODE,COST_PROFIT_CENTER_NAME
                                        FROM COMP_COST_PROFIT_CENTER 
                                        WHERE IS_PROFIT_CENTER = '" + Constants.CON_COST_CENTER + "' AND STATUS_CODE = '1' AND COMPANY_ID = '" + companyId + "';";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sMySqlString,mySqlCon);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable getProfitCenterByCompany(string companyId)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"SELECT COMP_COST_PROFIT_CENTER_CODE ,COST_PROFIT_CENTER_NAME
                                        FROM COMP_COST_PROFIT_CENTER 
                                        WHERE IS_PROFIT_CENTER = '" + Constants.CON_PROFIT_CENTER + "' AND STATUS_CODE = '1' AND COMPANY_ID = '" + companyId + "';";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sMySqlString,mySqlCon);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public string getETF(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @" SELECT ETF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + EmployeeID.Trim() + "'; ";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sMySqlString, mySqlCon);
                dataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["ETF_NO"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string getCCPC(string CCPCID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @" SELECT COST_PROFIT_CENTER_NAME FROM COMP_COST_PROFIT_CENTER WHERE COMP_COST_PROFIT_CENTER_CODE = '" + CCPCID.Trim() + "'; ";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sMySqlString, mySqlCon);
                dataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COST_PROFIT_CENTER_NAME"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
