using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Employee
{
    public class PreviousExperienceVerificationDataHandler : TemplateDataHandler
    {
        public string GetKnownNameFromEmployeeID(string EmployeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT KNOWN_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + EmployeID + @"';
                                    ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["KNOWN_NAME"].ToString();
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

        public DataTable populate(string CompanyID, string DepartmentID, string DivisionID, string EmployeeID, string Status)
        {
            try
            {
                dataTable = new DataTable();

                string QueryFilter = String.Empty;

                if (CompanyID != "")
                {
                    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        QueryFilter += "";
                    }
                    else
                    {
                        QueryFilter += " AND E.COMPANY_ID = '" + CompanyID + @"'";
                    }
                }

                if (DepartmentID != "")
                {
                    QueryFilter += " AND E.DEPT_ID = '" + DepartmentID + @"'";
                }

                if (DivisionID != "")
                {
                    QueryFilter += " AND E.DIVISION_ID = '" + DivisionID + @"'";
                }

                if (Status != "")
                {
                    QueryFilter += " AND PE.RECORD_STATUS = '" + Status + @"'";
                }

                if (EmployeeID != "")
                {
                    QueryFilter += " AND E.EMPLOYEE_ID = '" + EmployeeID + @"'";
                }

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT E.KNOWN_NAME FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY',
                                            (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = (SELECT DEPT_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'DEPARTMENT',
                                            (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = (SELECT DIVISION_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'DIVISION'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE, EMPLOYEE E
                                        WHERE 
	                                        PE.EMPLOYEE_ID = E.EMPLOYEE_ID " + QueryFilter + @"
                                        GROUP BY EMPLOYEE_ID
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployee(string Status)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.RECORD_STATUS = '" + Status + @"'
                                        GROUP BY EMPLOYEE_ID
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployee()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE 
                                        GROUP BY EMPLOYEE_ID
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployeeIND(string Status,string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.RECORD_STATUS = '" + Status + @"' AND PE.EMPLOYEE_ID = '" + EmployeeID + @"' 
                                        GROUP BY EMPLOYEE_ID
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployeeINDStateless(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.EMPLOYEE_ID = '" + EmployeeID + @"' 
                                        GROUP BY EMPLOYEE_ID
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployeeINDCompany(string Status,string companyID, string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE, EMPLOYEE EMP
                                        WHERE
                                            PE.RECORD_STATUS = '" + Status + @"' AND PE.EMPLOYEE_ID = EMP.EMPLOYEE_ID AND EMP.COMPANY_ID = '" + companyID + @"' AND PE.EMPLOYEE_ID ='" + EmployeeID + @"' 
                                        GROUP BY PE.EMPLOYEE_ID 
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployeeINDCompanyStateless(string companyID, string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE, EMPLOYEE EMP
                                        WHERE
                                            PE.EMPLOYEE_ID = EMP.EMPLOYEE_ID AND EMP.COMPANY_ID = '" + companyID + @"' AND PE.EMPLOYEE_ID ='" + EmployeeID + @"' 
                                        GROUP BY PE.EMPLOYEE_ID 
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployee(string Status, string companyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE, EMPLOYEE EMP
                                        WHERE
                                            PE.RECORD_STATUS = '" + Status + @"' AND PE.EMPLOYEE_ID = EMP.EMPLOYEE_ID AND EMP.COMPANY_ID = '" + companyID + @"' 
                                        GROUP BY PE.EMPLOYEE_ID 
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployeeStateLess(string companyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.EMPLOYEE_ID,
                                            (SELECT EPF_NO FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = PE.EMPLOYEE_ID) AS 'INITIALS_NAME',
                                            (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = PE.EMPLOYEE_ID)) AS 'COMPANY'
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE, EMPLOYEE EMP
                                        WHERE
                                            PE.EMPLOYEE_ID = EMP.EMPLOYEE_ID AND EMP.COMPANY_ID = '" + companyID + @"' 
                                        GROUP BY PE.EMPLOYEE_ID 
                                        ORDER BY PE.EMPLOYEE_ID ASC;
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

        public DataTable populateEmployments(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.LINE_NO,
                                            PE.DESIGNATION,
                                            PE.ORGANIZATION,
                                            CONVERT( PE.FROM_DATE , CHAR) AS 'FROM_DATE',
                                            CONVERT( PE.TO_DATE , CHAR) AS 'TO_DATE',
                                            PE.PHONE_NUMBER,
                                            PE.ADDRESS
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.EMPLOYEE_ID = '" + EmployeeID + @"'
                                        ORDER BY PE.TO_DATE DESC;
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

        public DataTable populateEmployments(string EmployeeID, string Status)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.LINE_NO,
                                            PE.DESIGNATION,
                                            PE.ORGANIZATION,
                                            CONVERT( PE.FROM_DATE , CHAR) AS 'FROM_DATE',
                                            CONVERT( PE.TO_DATE , CHAR) AS 'TO_DATE',
                                            PE.PHONE_NUMBER,
                                            PE.ADDRESS,
                                            PE.VERIFIED_BY_SERVICE_LETTER,
                                            PE.RECORD_STATUS
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.EMPLOYEE_ID = '" + EmployeeID + @"' AND PE.RECORD_STATUS = '" + Status + @"'  
                                        ORDER BY PE.TO_DATE DESC;
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

        public DataTable populateEmploymentStateLess(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            PE.LINE_NO,
                                            PE.DESIGNATION,
                                            PE.ORGANIZATION,
                                            CONVERT( PE.FROM_DATE , CHAR) AS 'FROM_DATE',
                                            CONVERT( PE.TO_DATE , CHAR) AS 'TO_DATE',
                                            PE.PHONE_NUMBER,
                                            PE.ADDRESS,
                                            PE.VERIFIED_BY_SERVICE_LETTER,
                                            PE.RECORD_STATUS
                                        FROM
                                            PREVIOUS_EMPLOYEMENT PE
                                        WHERE
                                            PE.EMPLOYEE_ID = '" + EmployeeID + @"' 
                                        ORDER BY PE.TO_DATE DESC;
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

        public void Update(string ModifiedBy, DataTable ExperienceDetails)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < ExperienceDetails.Rows.Count; i++)
                    {
                        string ComponentQry = @"
                                                    UPDATE 
                                                        PREVIOUS_EMPLOYEMENT 
                                                    SET 
                                                        VERIFIED_BY = @VERIFIED_BY, 
                                                        VERIFIED_DATE = NOW(), 
                                                        VERIFIED_BY_SERVICE_LETTER = @VERIFIED_BY_SERVICE_LETTER,
                                                        REJECT_REASON = @REJECT_REASON, 
                                                        RECORD_STATUS = @RECORD_STATUS 
                                                    WHERE 
                                                        LINE_NO = @LINE_NO;
                                                ";

                        string LINE_NO = ExperienceDetails.Rows[i]["LineNumber"].ToString();
                        string RECORD_STATUS = ExperienceDetails.Rows[i]["AssessmentStatusCode"].ToString();
                        string VERIFIED_BY_SERVICE_LETTER = ExperienceDetails.Rows[i]["ServiceLetter"].ToString();
                        string REJECT_REASON = ExperienceDetails.Rows[i]["RejectReason"].ToString();

                        mySqlCmd.Parameters.Clear();
                        mySqlCmd.Parameters.Add(new MySqlParameter("@VERIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@VERIFIED_BY_SERVICE_LETTER", VERIFIED_BY_SERVICE_LETTER.Trim() == "" ? (object)DBNull.Value : VERIFIED_BY_SERVICE_LETTER.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_STATUS", RECORD_STATUS.Trim() == "" ? (object)DBNull.Value : RECORD_STATUS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@LINE_NO", LINE_NO.Trim() == "" ? (object)DBNull.Value : LINE_NO.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@REJECT_REASON", REJECT_REASON.Trim() == "" ? (object)DBNull.Value : REJECT_REASON.Trim()));
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

        public DataTable getEmailFromEmployeeID(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                        SELECT 
                                            E.EMAIL 
                                        FROM 
                                            EMPLOYEE E
                                        WHERE 
                                            E.EMPLOYEE_ID = '" + EmployeeID + @"';
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

        public DataTable populateCompanies(string CompanyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = String.Empty;

                if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                        SELECT COMPANY_ID, COMP_NAME FROM COMPANY;
                                    ";
                }
                else
                {
                    sMySqlString = @"
                                        SELECT COMPANY_ID, COMP_NAME FROM COMPANY WHERE COMPANY_ID = '" + CompanyID + @"';
                                    ";
                }
                
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateDepartments(string CompanyID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = String.Empty;

                sMySqlString = @"
                                    SELECT DEPT_ID, DEPT_NAME FROM DEPARTMENT WHERE COMPANY_ID = '" + CompanyID + @"';
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

        public DataTable populateDivisions(string DepartmentID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = String.Empty;

                sMySqlString = @"
                                    SELECT DIVISION_ID, DIV_NAME FROM DIVISION WHERE DEPT_ID = '" + DepartmentID + @"';
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
    }
}
