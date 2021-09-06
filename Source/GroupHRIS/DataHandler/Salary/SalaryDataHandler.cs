using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Salary
{
    public class SalaryDataHandler : TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT	SALARY_ID,
                                                EMPLOYEE_ID,
		                                        BASIC_AMOUNT,
		                                        BUDGETARY_ALLOWANCE_AMOUNT,
                                                OTHER_AMOUNT,
                                                IS_OT_APPLICABLE,
                                                DATE_FORMAT(EFFECT_FROM, '%Y/%m/%d') AS EFFECT_FROM,
		                                        REMARKS,
                                                STATUS_CODE
                                        FROM	SALARY;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string OT = null;
                    string Status = null;

                    if (dataTable.Rows[i]["IS_OT_APPLICABLE"].ToString() == "1")
                    {
                        OT = "Yes";
                    }
                    else
                    {
                        OT = "No";
                    }

                    if (dataTable.Rows[i]["STATUS_CODE"].ToString() == "1")
                    {
                        Status = "Active";
                    }
                    else
                    {
                        Status = "Inactive";
                    }
                    dataTable.Rows[i]["IS_OT_APPLICABLE"] = OT;
                    dataTable.Rows[i]["STATUS_CODE"] = Status;
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Populate(string EmployeeNumber)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT      SALARY_ID,EMPLOYEE_ID,BASIC_AMOUNT,BUDGETARY_ALLOWANCE_AMOUNT,OTHER_AMOUNT,IS_OT_APPLICABLE,DATE_FORMAT(EFFECT_FROM, '%Y/%m/%d') AS EFFECT_FROM,REMARKS,STATUS_CODE 
                                        FROM        SALARY
                                        WHERE       EMPLOYEE_ID='" + EmployeeNumber + @"'
                                        ORDER BY    STATUS_CODE DESC;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string OT = null;
                    string Status = null;

                    if (dataTable.Rows[i]["IS_OT_APPLICABLE"].ToString() == "1")
                    {
                        OT = "Yes";
                    }
                    else
                    {
                        OT = "No";
                    }

                    if (dataTable.Rows[i]["STATUS_CODE"].ToString() == "1")
                    {
                        Status = "Active";
                    }
                    else
                    {
                        Status = "Inactive";
                    }
                    dataTable.Rows[i]["IS_OT_APPLICABLE"] = OT;
                    dataTable.Rows[i]["STATUS_CODE"] = Status;
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string PopulateEmployeeName(string EmployeeID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlCmd.Parameters.Clear();
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT  KNOWN_NAME AS EMP_NAME
                                        FROM  EMPLOYEE
                                        WHERE EMPLOYEE_ID=@EmployeeID";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim()));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                string EmpName = dataTable.Rows[0]["EMP_NAME"].ToString();


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                return EmpName;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                dataTable.Rows.Clear();
            }
        }

        public Boolean Insert(string EmployeeID, string BasicAmount, string BudgetAmount, string OtherAmount, string IsOtApplicable, string EffectFrom, string Remarks, string StatusCode, string AddedBy, string AddedDate, DataTable SalaryComponents)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                int Count = SalaryComponents.Rows.Count;
                for (int i = 0; i < Count; i++)
                {
                    string ComponentName = SalaryComponents.Rows[i]["COMPONENT_NAME"].ToString();
                    SalaryComponents.Rows[i]["COMPONENT_NAME"] = GetSalaryComponentID(ComponentName);
                }
                SerialHandler serialHandler = new SerialHandler();
                string SalaryID = serialHandler.getserila(mySqlCon, Constants.SALARY_ID_STAMP);



                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    string Qry = @"INSERT INTO SALARY
				                                    (
					                                    SALARY_ID,
                                                        EMPLOYEE_ID,
					                                    BASIC_AMOUNT,
                                                        BUDGETARY_ALLOWANCE_AMOUNT,
                                                        OTHER_AMOUNT,
                                                        IS_OT_APPLICABLE,
                                                        EFFECT_FROM,
					                                    REMARKS,
                                                        STATUS_CODE,
					                                    ADDED_BY,
                                                        ADDED_DATE,
                                                        MODIFIED_BY
				                                    )
                                    VALUES
				                                    (
					                                    @SalaryID,
                                                        @EmployeeID,
					                                    @BasicAmount,
                                                        @BudgetAmount,
					                                    @OtherAmount,
                                                        @IsOtApplicable,
                                                        @EffectFrom,
					                                    @Remarks,
                                                        @AssessmentStatusCode,
					                                    @ModifiedBy,
                                                        (SELECT CURRENT_TIMESTAMP()),
                                                        @ModifiedBy
				                                    );";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@BasicAmount", BasicAmount.Trim() == "" ? (object)DBNull.Value : BasicAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@BudgetAmount", BudgetAmount.Trim() == "" ? (object)DBNull.Value : BudgetAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@OtherAmount", OtherAmount.Trim() == "" ? (object)DBNull.Value : OtherAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IsOtApplicable", IsOtApplicable.Trim() == "" ? (object)DBNull.Value : IsOtApplicable.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EffectFrom", EffectFrom.Trim() == "" ? (object)DBNull.Value : EffectFrom.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AddedDate", AddedDate.Trim() == "" ? (object)DBNull.Value : AddedDate.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    for (int i = 0; i < Count; i++)
                    {
                        string ComponentQry = @"INSERT INTO SALARY_DETAIL(
                                                            SALARY_ID,
                                                            COMPONENT_ID,
                                                            AMOUNT,
                                                            STATUS_CODE,
                                                            ADDED_BY,
                                                            ADDED_DATE,
                                                            MODIFIED_BY,
                                                            MODIFIED_DATE
                                                        ) 
                                                  VALUES(
                                                            @SalaryID,
                                                            @ComponentID,
                                                            @Amount,
                                                            @AssessmentStatusCode,
                                                            @ModifiedBy,
                                                            now(),
                                                            @ModifiedBy,
                                                            now()
                                                        );";

                        string ComponentID = SalaryComponents.Rows[i]["COMPONENT_NAME"].ToString();
                        string Amount = SalaryComponents.Rows[i]["AMOUNT"].ToString();

                        if (SalaryComponents.Rows[i]["STATUS_CODE"].ToString() == Constants.STATUS_ACTIVE_TAG)
                        {
                            SalaryComponents.Rows[i]["STATUS_CODE"] = Constants.STATUS_ACTIVE_VALUE;
                        }
                        else
                        {
                            SalaryComponents.Rows[i]["STATUS_CODE"] = Constants.STATUS_INACTIVE_VALUE;
                        }

                        string ComPonentStatusCode = SalaryComponents.Rows[i]["STATUS_CODE"].ToString();

                        mySqlCmd.Parameters.Clear();
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", ComPonentStatusCode.Trim() == "" ? (object)DBNull.Value : ComPonentStatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                        mySqlCmd.CommandText = ComponentQry;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    oMySqlTransaction.Commit();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
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

        public Boolean Update(string EmployeeID, string BasicAmount, string BudgetAmount, string OtherAmount, string IsOtApplicable, string EffectFrom, string Remarks, string StatusCode, string ModifiedBy, string SalaryID, DataTable SalaryComponents/*, List<string> ChangeStateList*/)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                int Count = SalaryComponents.Rows.Count;
                for (int i = 0; i < Count; i++)
                {
                    try
                    {
                        string ComponentName = SalaryComponents.Rows[i]["COMPONENT_NAME"].ToString();
                        SalaryComponents.Rows[i]["COMPONENT_NAME"] = GetSalaryComponentID(ComponentName);
                    }
                    catch { }
                }


                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    string Qry = @"UPDATE  SALARY 
                               SET     EMPLOYEE_ID=@EmployeeID,
                                       BASIC_AMOUNT=@BasicAmount,
                                       BUDGETARY_ALLOWANCE_AMOUNT=@BudgetAmount,
                                       OTHER_AMOUNT=@OtherAmount,
                                       IS_OT_APPLICABLE=@IsOtApplicable,
                                       EFFECT_FROM=@EffectFrom,
                                       REMARKS=@Remarks,
                                       STATUS_CODE=@AssessmentStatusCode,
                                       MODIFIED_BY=@ModifiedBy,
                                       MODIFIED_DATE=now()
                               WHERE   SALARY_ID=@SalaryID;";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@BasicAmount", BasicAmount.Trim() == "" ? (object)DBNull.Value : BasicAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@BudgetAmount", BudgetAmount.Trim() == "" ? (object)DBNull.Value : BudgetAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@OtherAmount", OtherAmount.Trim() == "" ? (object)DBNull.Value : OtherAmount.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IsOtApplicable", IsOtApplicable.Trim() == "" ? (object)DBNull.Value : IsOtApplicable.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EffectFrom", EffectFrom.Trim() == "" ? (object)DBNull.Value : EffectFrom.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ModifiedBy", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@SalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    Qry = @"DELETE FROM SALARY_DETAIL WHERE SALARY_ID=@SalaryID;";
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    for (int i = 0; i < SalaryComponents.Rows.Count; i++)
                    {
                        string ComponentID = SalaryComponents.Rows[i]["COMPONENT_NAME"].ToString();
                        string Amount = SalaryComponents.Rows[i]["AMOUNT"].ToString();
                        string CompStatusCode = SalaryComponents.Rows[i]["STATUS_CODE"].ToString();

                        if (CompStatusCode == Constants.STATUS_ACTIVE_TAG)
                        {
                            CompStatusCode = Constants.STATUS_ACTIVE_VALUE;
                        }
                        else
                        {
                            CompStatusCode = Constants.STATUS_INACTIVE_VALUE;
                        }


                        string CompAddedBy = SalaryComponents.Rows[i]["ADDED_BY"].ToString();
                        string CompoAddedDate = SalaryComponents.Rows[i]["ADDED_DATE"].ToString();
                        string CompModifiedBy = SalaryComponents.Rows[i]["MODIFIED_BY"].ToString();
                        string CompModifiedDate = SalaryComponents.Rows[i]["MODIFIED_DATE"].ToString();
                        string IsEdited = SalaryComponents.Rows[i]["EDITED"].ToString();


                        if (IsEdited == "")
                        {
                            mySqlCmd.Parameters.Clear();

                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompSalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompStatusCode", CompStatusCode.Trim() == "" ? (object)DBNull.Value : CompStatusCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompAddedBy", CompAddedBy.Trim() == "" ? (object)DBNull.Value : CompAddedBy.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompoAddedDate", CompoAddedDate.Trim() == "" ? (object)DBNull.Value : CompoAddedDate.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompModifiedBy", CompModifiedBy.Trim() == "" ? (object)DBNull.Value : CompModifiedBy.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompModifiedDate", CompModifiedDate.Trim() == "" ? (object)DBNull.Value : CompModifiedDate.Trim()));

                            Qry = @"INSERT INTO SALARY_DETAIL(SALARY_ID,COMPONENT_ID,AMOUNT,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                VALUES(@CompSalaryID,@ComponentID,@Amount,@CompStatusCode,@CompAddedBy,@CompoAddedDate,@CompModifiedBy,@CompModifiedDate);";

                            mySqlCmd.CommandText = Qry;
                            mySqlCmd.ExecuteNonQuery();
                        }
                        else if (IsEdited == "Update")
                        {
                            mySqlCmd.Parameters.Clear();

                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompSalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompStatusCode", CompStatusCode.Trim() == "" ? (object)DBNull.Value : CompStatusCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompAddedBy", CompAddedBy.Trim() == "" ? (object)DBNull.Value : CompAddedBy.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompoAddedDate", CompoAddedDate.Trim() == "" ? (object)DBNull.Value : CompoAddedDate.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompModifiedBy", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                            Qry = @"INSERT INTO SALARY_DETAIL(SALARY_ID,COMPONENT_ID,AMOUNT,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                VALUES(@CompSalaryID,@ComponentID,@Amount,@CompStatusCode,@CompAddedBy,@CompoAddedDate,@CompModifiedBy,now());";

                            mySqlCmd.CommandText = Qry;
                            mySqlCmd.ExecuteNonQuery();
                        }
                        else if (IsEdited == "New")
                        {
                            mySqlCmd.Parameters.Clear();

                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompSalaryID", SalaryID.Trim() == "" ? (object)DBNull.Value : SalaryID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentID", ComponentID.Trim() == "" ? (object)DBNull.Value : ComponentID.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@Amount", Amount.Trim() == "" ? (object)DBNull.Value : Amount.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompStatusCode", CompStatusCode.Trim() == "" ? (object)DBNull.Value : CompStatusCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompAddedBy", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@CompModifiedBy", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                            Qry = @"INSERT INTO SALARY_DETAIL(SALARY_ID,COMPONENT_ID,AMOUNT,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                VALUES(@CompSalaryID,@ComponentID,@Amount,@CompStatusCode,@CompAddedBy,now(),@CompModifiedBy,now());";

                            mySqlCmd.CommandText = Qry;
                            mySqlCmd.ExecuteNonQuery();
                        }

                    }
                    oMySqlTransaction.Commit();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
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

        private string GetSalaryComponentID(string SalaryComponentName)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                mySqlCmd.Connection = mySqlCon;
                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Clear();
                string sMySqlString = @"SELECT  COMPONENT_ID 
                                        FROM    SALARY_COMPONENTS
                                        WHERE   COMPONENT_NAME=@ComponentName;";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@ComponentName", SalaryComponentName.Trim() == "" ? (object)DBNull.Value : SalaryComponentName.Trim()));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean CheckPrevSalary(string EmployeeID)
        {
            Boolean State = false;

            try
            {
                MySqlCommand MSC = new MySqlCommand();
                MSC.Connection = mySqlCon;
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MSC.Parameters.Add(new MySqlParameter("@EmployeeID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                string sMySqlString = @"select * 
                                        from SALARY s
                                        where s.EMPLOYEE_ID=@EmployeeID and s.STATUS_CODE='1'";
                MSC.CommandText = sMySqlString;

                MySqlDataReader MDR = MSC.ExecuteReader();
                if (MDR.Read())
                {
                    State = true;
                }

                return State;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

    }
}