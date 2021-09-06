using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Payroll
{
    public class OvertimeProcessDataHandler : TemplateDataHandler
    {
        
        public DataTable GetEmployeeCompany(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetCompanyBasedOnEmployee";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable GetEmployeeName(string empName)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetEmployeeName";

                mySqlCmd.Parameters.Add(new MySqlParameter("EmpName", empName));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable GetEmployeeData(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "sp_EmployeeNoPayOvertime";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;

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

        public Boolean UpdateTransaction(string empId, string amount, string remarks, string user, string category, string subcategory)
        {
            Boolean Status = false;
            dataTable.Rows.Clear();
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                String Qry = "sp_UpdateOvertimeProcess";

                mySqlCmd.Parameters.Add(new MySqlParameter("empId", empId));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("amount", amount));
                mySqlCmd.Parameters.Add(new MySqlParameter("remarks", remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));

                mySqlCmd.CommandText = Qry;
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

        public string GetAmount(string YearMonth, string EmployeeID, string Category, string TypeID, out bool IsEnabled, out string AddedBy, out string AddedDate)
        {
            string NormalOTHours = "";
            IsEnabled = false;
            AddedBy = String.Empty;
            AddedDate = String.Empty;
            try
            {
                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = mySqlCon;
                CMD.Parameters.Add(new MySqlParameter("@TRANS_MONTH", YearMonth));
                CMD.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                CMD.Parameters.Add(new MySqlParameter("@CATEGORY", Category));
                CMD.Parameters.Add(new MySqlParameter("@TYPE_ID", TypeID));

                string Query = @"

                                    SELECT 
                                        FINALIZED_AMOUNT AS 'AMOUNT', STATUS_CODE, ADDED_BY, CONVERT(ADDED_DATE, CHAR) AS 'ADDED_DATE'
                                    FROM
                                        TRANSACTIONS
                                    WHERE
                                        TRANS_MONTH = @TRANS_MONTH
                                            AND EMPLOYEE_ID = @EMPLOYEE_ID
                                            AND CATEGORY = @CATEGORY
                                            AND TYPE_ID = @TYPE_ID                                    

                                ";
                CMD.CommandText = Query;
                MySqlDataAdapter DA = new MySqlDataAdapter(CMD);
                DataTable DT = new DataTable();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    NormalOTHours = DT.Rows[0]["AMOUNT"].ToString().Trim();

                    if ((DT.Rows[0]["STATUS_CODE"].ToString().Trim()) == "1")
                    {
                        IsEnabled = true;

                        AddedBy = DT.Rows[0]["ADDED_BY"].ToString().Trim();
                        AddedDate = DT.Rows[0]["ADDED_DATE"].ToString().Trim();
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }

            return NormalOTHours;
        }

        public string GetRoleOTCategory(string EmployeeID)
        {
            string RoleOTCategory = "";
            try
            {
                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = mySqlCon;
                CMD.Parameters.Add(new MySqlParameter("@EMPLOYEE_STATUS", Constants.CON_EMPLOYEE_STS));
                CMD.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));

                string Query = @"

                                    SELECT 
                                        erc.OT_CATEGORY_NAME
                                    FROM
                                        EMPLOYEE_ROLE_CATEGORY erc,
                                        EMPLOYEE e
                                    WHERE
                                        e.EMPLOYEE_STATUS = @EMPLOYEE_STATUS
                                            AND e.ROLE_ID = erc.ROLE_ID
                                            AND e.EMPLOYEE_ID = @EMPLOYEE_ID                             

                                ";
                CMD.CommandText = Query;
                MySqlDataAdapter DA = new MySqlDataAdapter(CMD);
                DataTable DT = new DataTable();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    RoleOTCategory = DT.Rows[0]["OT_CATEGORY_NAME"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return RoleOTCategory;
        }

        public string GetEPFNumber(string EmployeeID)
        {
            string EPF = "";
            try
            {
                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = mySqlCon;
                CMD.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));

                string Query = @"

                                    SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = @EMPLOYEE_ID                             

                                ";
                CMD.CommandText = Query;
                MySqlDataAdapter DA = new MySqlDataAdapter(CMD);
                DataTable DT = new DataTable();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    EPF = DT.Rows[0]["EPF_NO"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return EPF;
        }

        public string GetRemarks(string EmployeeID, string TransMonth)
        {
            string Remarks = "";
            try
            {
                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = mySqlCon;
                CMD.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                CMD.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth));

                string Query = @"

                                    SELECT DISTINCT REMARKS FROM TRANSACTIONS WHERE EMPLOYEE_ID = @EMPLOYEE_ID AND TRANS_MONTH = @TRANS_MONTH AND REMARKS IS NOT NULL AND REMARKS <> ''                 

                                ";
                CMD.CommandText = Query;
                MySqlDataAdapter DA = new MySqlDataAdapter(CMD);
                DataTable DT = new DataTable();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    Remarks = DT.Rows[0]["REMARKS"].ToString().Trim();
                }

                CMD.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return Remarks;
        }



        public void Insert(string TransMonth, string EmployeeID, string EPF, string NormalOTAmount, string DoubleOTAmount, string AttendanceIncentiveAmount, Boolean NormalOTStatus, Boolean DoubleOTStatus, Boolean AttendanceIncentiveStatus, string Remarks, string ModifiedBy, string PreviousNormalOT, string PreviousDoubleOT, string PreviousAI, string NOTPreAddedBy, string NOTPreAddedDate, string DOTPreAddedBy, string DOTPreAddedDate, string AIPreAddedBy, string AIPreAddedDate)
        {
            MySqlTransaction mySqlTrans = null;
            string commText = String.Empty;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;


                if (NormalOTAmount != String.Empty)
                {
                    mySqlCmd.Parameters.Clear();
                    commText = @" 
                                    REPLACE INTO 
                                        TRANSACTIONS
                                            (
                                                TRANS_MONTH, 
                                                EMPLOYEE_ID, 
                                                CATEGORY, 
                                                SUB_CATEGORY, 
                                                TYPE_ID, 
                                                EPF_NO, 
                                                AMOUNT,
                                                STATUS_CODE, 
                                                ADDED_BY, 
                                                ADDED_DATE, 
                                                MODIFIED_BY, 
                                                MODIFIED_DATE, 
                                                IS_UPLOADED, 
                                                FINALIZED_AMOUNT, 
                                                REMARKS
                                            ) 
                                        VALUES
                                            (
                                                @TRANS_MONTH, 
                                                @EMPLOYEE_ID, 
                                                @CATEGORY, 
                                                @SUB_CATEGORY, 
                                                @TYPE_ID, 
                                                @EPF_NO,  
                                                @AMOUNT, 
                                                @STATUS_CODE, 
                                                @ADDED_BY, 
                                                @ADDED_DATE, 
                                                @MODIFIED_BY, 
                                                NOW(), 
                                                @IS_UPLOADED, 
                                                @FINALIZED_AMOUNT, 
                                                @REMARKS
                                            )
                                ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth.Trim() == "" ? (object)DBNull.Value : TransMonth.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@CATEGORY", Constants.CON_OT_CATEGORY_OVERTIME_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_OVERTIME_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@SUB_CATEGORY", Constants.CON_OT_SUB_CATEGORY_NORMAL_OVERTIME_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_SUB_CATEGORY_NORMAL_OVERTIME_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TYPE_ID", Constants.CON_OT_CATEGORY_OVERTIME_NORMAL_OT_ID.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_OVERTIME_NORMAL_OT_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EPF_NO", EPF.Trim() == "" ? (object)DBNull.Value : EPF.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AMOUNT", PreviousNormalOT.Trim() == "" ? (object)DBNull.Value : PreviousNormalOT.Trim()));
                    if (NormalOTStatus)
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_ACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_ACTIVE_VALUE.Trim()));
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_INACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_INACTIVE_VALUE.Trim()));
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_UPLOADED", Constants.CON_LEAVE_PENDING_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_LEAVE_PENDING_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@FINALIZED_AMOUNT", NormalOTAmount.Trim() == "" ? (object)DBNull.Value : NormalOTAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", NOTPreAddedBy.Trim() == "" ? (object)DBNull.Value : NOTPreAddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_DATE", NOTPreAddedDate.Trim() == "" ? (object)DBNull.Value : NOTPreAddedDate.Trim()));


                    mySqlCmd.CommandText = commText;
                    mySqlCmd.ExecuteNonQuery();
                }



                if (DoubleOTAmount != String.Empty)
                {
                    mySqlCmd.Parameters.Clear();
                    commText = @" 
                                    REPLACE INTO 
                                        TRANSACTIONS
                                            (
                                                TRANS_MONTH, 
                                                EMPLOYEE_ID, 
                                                CATEGORY, 
                                                SUB_CATEGORY, 
                                                TYPE_ID, 
                                                EPF_NO, 
                                                AMOUNT,
                                                STATUS_CODE, 
                                                ADDED_BY, 
                                                ADDED_DATE, 
                                                MODIFIED_BY, 
                                                MODIFIED_DATE, 
                                                IS_UPLOADED, 
                                                FINALIZED_AMOUNT, 
                                                REMARKS
                                            ) 
                                        VALUES
                                            (
                                                @TRANS_MONTH, 
                                                @EMPLOYEE_ID, 
                                                @CATEGORY, 
                                                @SUB_CATEGORY, 
                                                @TYPE_ID, 
                                                @EPF_NO,  
                                                @AMOUNT, 
                                                @STATUS_CODE, 
                                                @ADDED_BY, 
                                                @ADDED_DATE, 
                                                @MODIFIED_BY, 
                                                NOW(), 
                                                @IS_UPLOADED, 
                                                @FINALIZED_AMOUNT, 
                                                @REMARKS
                                            )
                                ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth.Trim() == "" ? (object)DBNull.Value : TransMonth.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@CATEGORY", Constants.CON_OT_CATEGORY_OVERTIME_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_OVERTIME_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@SUB_CATEGORY", Constants.CON_OT_SUB_CATEGORY_DOUBLE_OVERTIME_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_SUB_CATEGORY_DOUBLE_OVERTIME_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TYPE_ID", Constants.CON_OT_CATEGORY_OVERTIME_DOUBLE_OT_ID.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_OVERTIME_DOUBLE_OT_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EPF_NO", EPF.Trim() == "" ? (object)DBNull.Value : EPF.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AMOUNT", PreviousDoubleOT.Trim() == "" ? (object)DBNull.Value : PreviousDoubleOT.Trim()));
                    if (DoubleOTStatus)
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_ACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_ACTIVE_VALUE.Trim()));
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_INACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_INACTIVE_VALUE.Trim()));
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_UPLOADED", Constants.CON_LEAVE_PENDING_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_LEAVE_PENDING_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@FINALIZED_AMOUNT", DoubleOTAmount.Trim() == "" ? (object)DBNull.Value : DoubleOTAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", DOTPreAddedBy.Trim() == "" ? (object)DBNull.Value : DOTPreAddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_DATE", DOTPreAddedDate.Trim() == "" ? (object)DBNull.Value : DOTPreAddedDate.Trim()));


                    mySqlCmd.CommandText = commText;
                    mySqlCmd.ExecuteNonQuery();
                } 
                
                
                
                if (AttendanceIncentiveAmount != String.Empty)
                {
                    mySqlCmd.Parameters.Clear();
                    commText = @" 
                                    REPLACE INTO 
                                        TRANSACTIONS
                                            (
                                                TRANS_MONTH, 
                                                EMPLOYEE_ID, 
                                                CATEGORY, 
                                                SUB_CATEGORY, 
                                                TYPE_ID, 
                                                EPF_NO, 
                                                AMOUNT,
                                                STATUS_CODE, 
                                                ADDED_BY, 
                                                ADDED_DATE, 
                                                MODIFIED_BY, 
                                                MODIFIED_DATE, 
                                                IS_UPLOADED, 
                                                FINALIZED_AMOUNT, 
                                                REMARKS
                                            ) 
                                        VALUES
                                            (
                                                @TRANS_MONTH, 
                                                @EMPLOYEE_ID, 
                                                @CATEGORY, 
                                                @SUB_CATEGORY, 
                                                @TYPE_ID, 
                                                @EPF_NO,  
                                                @AMOUNT, 
                                                @STATUS_CODE, 
                                                @ADDED_BY, 
                                                @ADDED_DATE, 
                                                @MODIFIED_BY, 
                                                NOW(), 
                                                @IS_UPLOADED, 
                                                @FINALIZED_AMOUNT, 
                                                @REMARKS
                                            )
                                ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth.Trim() == "" ? (object)DBNull.Value : TransMonth.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@CATEGORY", Constants.CON_OT_CATEGORY_ALLOWANCE_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_ALLOWANCE_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@SUB_CATEGORY", Constants.CON_OT_SUB_CATEGORY_ALLOWANCE_ATTENDANCE_INCENTIVE_TAG.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_SUB_CATEGORY_ALLOWANCE_ATTENDANCE_INCENTIVE_TAG.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TYPE_ID", Constants.CON_OT_CATEGORY_ATTENDANCE_INCENTIVE_ID.Trim() == "" ? (object)DBNull.Value : Constants.CON_OT_CATEGORY_ATTENDANCE_INCENTIVE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EPF_NO", EPF.Trim() == "" ? (object)DBNull.Value : EPF.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AMOUNT", PreviousAI.Trim() == "" ? (object)DBNull.Value : PreviousAI.Trim()));
                    if (AttendanceIncentiveStatus)
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_ACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_ACTIVE_VALUE.Trim()));
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.STATUS_INACTIVE_VALUE.Trim() == "" ? (object)DBNull.Value : Constants.STATUS_INACTIVE_VALUE.Trim()));
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_UPLOADED", Constants.CON_LEAVE_PENDING_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.CON_LEAVE_PENDING_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@FINALIZED_AMOUNT", AttendanceIncentiveAmount.Trim() == "" ? (object)DBNull.Value : AttendanceIncentiveAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AIPreAddedBy.Trim() == "" ? (object)DBNull.Value : AIPreAddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_DATE", AIPreAddedDate.Trim() == "" ? (object)DBNull.Value : AIPreAddedDate.Trim()));


                    mySqlCmd.CommandText = commText;
                    mySqlCmd.ExecuteNonQuery();
                }       

                    

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {

            }
            
        }

    }
}
