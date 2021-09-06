using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Employee
{
    public class EmployeeDependentDataHandler : TemplateDataHandler
    {
        public string getNameWithInitials(string employeeId)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            CONCAT(CONCAT(TITLE, ' '), INITIALS_NAME) AS 'EMPLOYEE_NAME'
                                        FROM
                                            EMPLOYEE
                                        WHERE
                                            EMPLOYEE_ID = '" + employeeId.Trim() + @"'
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["EMPLOYEE_NAME"].ToString();
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

        public Boolean Insert(string EmployeeID, string AddedBy, DataTable Dependents)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string DependentID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < Dependents.Rows.Count; i++)
                    {

                        DependentID = serialHandler.getserila(mySqlCon, Constants.DEPENDENT_ID_STAMP);

                        string FULL_NAME = Dependents.Rows[i]["FULL_NAME"].ToString();
                        string NAME_WITH_INITIALS = Dependents.Rows[i]["NAME_WITH_INITIALS"].ToString();
                        string RELATIONSHIP_TO_EMPLOYEE = Dependents.Rows[i]["RELATIONSHIP_TO_EMPLOYEE"].ToString();
                        string GENDER = Dependents.Rows[i]["GENDER"].ToString();
                        string DOB = Dependents.Rows[i]["DOB"].ToString();
                        string NIC = Dependents.Rows[i]["NIC"].ToString();
                        string OCCUPATION = Dependents.Rows[i]["OCCUPATION"].ToString();
                        string PLACE_OF_WORK = Dependents.Rows[i]["PLACE_OF_WORK"].ToString();
                        string CONTACT_NUMBER_MOBILE = Dependents.Rows[i]["CONTACT_NUMBER_MOBILE"].ToString();
                        string CONTACT_NUMBER_LAND = Dependents.Rows[i]["CONTACT_NUMBER_LAND"].ToString();
                        string IS_EMRGENCY_CONTACT = Dependents.Rows[i]["IS_EMRGENCY_CONTACT"].ToString();


                        string Qry = @"
                                        INSERT INTO 
                                            DEPENDANTS
                                                (
                                                    EMPLOYEE_ID,
                                                    DEPENDANT_ID,
                                                    FULL_NAME,
                                                    NAME_WITH_INITIALS,
                                                    RELATIONSHIP_TO_EMPLOYEE,
                                                    GENDER,
                                                    DOB,
                                                    NIC,
                                                    OCCUPATION,
                                                    PLACE_OF_WORK,
                                                    CONTACT_NUMBER_MOBILE,
                                                    CONTACT_NUMBER_LAND,
                                                    IS_EMRGENCY_CONTACT,
                                                    ADDED_BY,
                                                    ADDED_DATE,
                                                    MODIFIED_BY,
                                                    MODIFIED_DATE
                                                ) 
                                            VALUES
                                                (
                                                    @EMPLOYEE_ID,
                                                    @DEPENDANT_ID,
                                                    @FULL_NAME,
                                                    @NAME_WITH_INITIALS,
                                                    @RELATIONSHIP_TO_EMPLOYEE,
                                                    @GENDER,
                                                    @DOB,
                                                    @NIC,
                                                    @OCCUPATION,
                                                    @PLACE_OF_WORK,
                                                    @CONTACT_NUMBER_MOBILE,
                                                    @CONTACT_NUMBER_LAND,
                                                    @IS_EMRGENCY_CONTACT,
                                                    @ADDED_BY,
                                                    NOW(),
                                                    @MODIFIED_BY,
                                                    NOW()
                                                );
                                    ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID.Trim() == "" ? (object)DBNull.Value : EmployeeID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DEPENDANT_ID", DependentID.Trim() == "" ? (object)DBNull.Value : DependentID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@FULL_NAME", FULL_NAME.Trim() == "" ? (object)DBNull.Value : FULL_NAME.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@NAME_WITH_INITIALS", NAME_WITH_INITIALS.Trim() == "" ? (object)DBNull.Value : NAME_WITH_INITIALS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@RELATIONSHIP_TO_EMPLOYEE", RELATIONSHIP_TO_EMPLOYEE.Trim() == "" ? (object)DBNull.Value : RELATIONSHIP_TO_EMPLOYEE.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GENDER", GENDER.Trim() == "" ? (object)DBNull.Value : GENDER.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DOB", DOB.Trim() == "" ? (object)DBNull.Value : DOB.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@NIC", NIC.Trim() == "" ? (object)DBNull.Value : NIC.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@OCCUPATION", OCCUPATION.Trim() == "" ? (object)DBNull.Value : OCCUPATION.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PLACE_OF_WORK", PLACE_OF_WORK.Trim() == "" ? (object)DBNull.Value : PLACE_OF_WORK.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@CONTACT_NUMBER_MOBILE", CONTACT_NUMBER_MOBILE.Trim() == "" ? (object)DBNull.Value : CONTACT_NUMBER_MOBILE.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@CONTACT_NUMBER_LAND", CONTACT_NUMBER_LAND.Trim() == "" ? (object)DBNull.Value : CONTACT_NUMBER_LAND.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@IS_EMRGENCY_CONTACT", IS_EMRGENCY_CONTACT.Trim() == "" ? (object)DBNull.Value : IS_EMRGENCY_CONTACT.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();
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

        public Boolean Update(string EmployeeID, string AddedBy, DataTable Dependents, string DependentCode)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string DependentID = DependentCode;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < Dependents.Rows.Count; i++)
                    {


                        string FULL_NAME = Dependents.Rows[i]["FULL_NAME"].ToString();
                        string NAME_WITH_INITIALS = Dependents.Rows[i]["NAME_WITH_INITIALS"].ToString();
                        string RELATIONSHIP_TO_EMPLOYEE = Dependents.Rows[i]["RELATIONSHIP_TO_EMPLOYEE"].ToString();
                        string GENDER = Dependents.Rows[i]["GENDER"].ToString();
                        string DOB = Dependents.Rows[i]["DOB"].ToString();
                        string NIC = Dependents.Rows[i]["NIC"].ToString();
                        string OCCUPATION = Dependents.Rows[i]["OCCUPATION"].ToString();
                        string PLACE_OF_WORK = Dependents.Rows[i]["PLACE_OF_WORK"].ToString();
                        string CONTACT_NUMBER_MOBILE = Dependents.Rows[i]["CONTACT_NUMBER_MOBILE"].ToString();
                        string CONTACT_NUMBER_LAND = Dependents.Rows[i]["CONTACT_NUMBER_LAND"].ToString();
                        string IS_EMRGENCY_CONTACT = Dependents.Rows[i]["IS_EMRGENCY_CONTACT"].ToString();


                        string Qry = @"
                                        UPDATE 
                                            DEPENDANTS 
                                        SET  
                                            FULL_NAME = @FULL_NAME, 
                                            NAME_WITH_INITIALS = @NAME_WITH_INITIALS, 
                                            RELATIONSHIP_TO_EMPLOYEE = @RELATIONSHIP_TO_EMPLOYEE, 
                                            GENDER = @GENDER, 
                                            DOB = @DOB, 
                                            NIC = @NIC,
                                            OCCUPATION = @OCCUPATION, 
                                            PLACE_OF_WORK = @PLACE_OF_WORK, 
                                            CONTACT_NUMBER_MOBILE = @CONTACT_NUMBER_MOBILE, 
                                            CONTACT_NUMBER_LAND = @CONTACT_NUMBER_LAND, 
                                            IS_EMRGENCY_CONTACT = @IS_EMRGENCY_CONTACT,
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW()
                                        WHERE 
                                            DEPENDANT_ID = @DEPENDANT_ID
                                    ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@DEPENDANT_ID", DependentID.Trim() == "" ? (object)DBNull.Value : DependentID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@FULL_NAME", FULL_NAME.Trim() == "" ? (object)DBNull.Value : FULL_NAME.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@NAME_WITH_INITIALS", NAME_WITH_INITIALS.Trim() == "" ? (object)DBNull.Value : NAME_WITH_INITIALS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@RELATIONSHIP_TO_EMPLOYEE", RELATIONSHIP_TO_EMPLOYEE.Trim() == "" ? (object)DBNull.Value : RELATIONSHIP_TO_EMPLOYEE.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GENDER", GENDER.Trim() == "" ? (object)DBNull.Value : GENDER.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DOB", DOB.Trim() == "" ? (object)DBNull.Value : DOB.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@NIC", NIC.Trim() == "" ? (object)DBNull.Value : NIC.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@OCCUPATION", OCCUPATION.Trim() == "" ? (object)DBNull.Value : OCCUPATION.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PLACE_OF_WORK", PLACE_OF_WORK.Trim() == "" ? (object)DBNull.Value : PLACE_OF_WORK.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@CONTACT_NUMBER_MOBILE", CONTACT_NUMBER_MOBILE.Trim() == "" ? (object)DBNull.Value : CONTACT_NUMBER_MOBILE.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@CONTACT_NUMBER_LAND", CONTACT_NUMBER_LAND.Trim() == "" ? (object)DBNull.Value : CONTACT_NUMBER_LAND.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@IS_EMRGENCY_CONTACT", IS_EMRGENCY_CONTACT.Trim() == "" ? (object)DBNull.Value : IS_EMRGENCY_CONTACT.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();
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

        public DataTable PopulateDependents(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                DEPENDANT_ID, 
                                                FULL_NAME, 
                                                NAME_WITH_INITIALS, 
                                                RELATIONSHIP_TO_EMPLOYEE, 
                                                GENDER, 
                                                CONVERT(DOB, CHAR) AS 'DOB', 
                                                NIC, 
                                                OCCUPATION, 
                                                PLACE_OF_WORK, 
                                                CONTACT_NUMBER_MOBILE, 
                                                CONTACT_NUMBER_LAND,
                                                CASE 
                                                    WHEN 
                                                        IS_EMRGENCY_CONTACT = '1' 
                                                    THEN 
                                                        'YES' 
                                                    WHEN 
                                                        IS_EMRGENCY_CONTACT = '0' 
                                                    THEN 
                                                        'NO' 
                                                END AS 
                                                'IS_EMRGENCY_CONTACT'
                                            FROM 
                                                DEPENDANTS 
                                            WHERE 
                                                EMPLOYEE_ID = '" + EmployeeID + @"';
                                            
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

        public Boolean IsEmployeeActive(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"SELECT E.EMPLOYEE_STATUS FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = '" + EmployeeID + @"';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["EMPLOYEE_STATUS"].ToString() == Constants.CON_EMPLOYEE_STATUS_ACTIVE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
