using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.HRISAlerts
{
    public class HRISAlertsDataHandler : TemplateDataHandler
    {
        public DataTable BirthdayAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                            SELECT 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = E.COMPANY_ID) AS 'COMPANY',
                                                (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = E.DEPT_ID) AS 'DEPARTMENT'
                                            FROM 
                                                EMPLOYEE E
                                            WHERE 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                MONTH(E.DOB) = MONTH(CURDATE()) AND 
                                                DAY(E.DOB) = DAY(CURDATE());

                                        ";
                }
                else
                {
                    sMySqlString = @"
                                            SELECT 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = E.COMPANY_ID) AS 'COMPANY',
                                                (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = E.DEPT_ID) AS 'DEPARTMENT'
                                            FROM 
                                                EMPLOYEE E
                                            WHERE 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                MONTH(E.DOB) = MONTH(CURDATE()) AND 
                                                E.COMPANY_ID = '" + CompanyCode + @"' AND 
                                                DAY(E.DOB) = DAY(CURDATE());

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

        public DataTable CurrentYearCompanyCalenderAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                            SELECT 
                                                YEAR(CURDATE()) AS 'YEAR',
                                                COMP_NAME 
                                            FROM 
                                                COMPANY 
                                            WHERE 
                                                STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND COMPANY_ID 
                                            NOT IN
                                                (
                                                    SELECT 
                                                        DISTINCT(COMPANY_ID) 
                                                    FROM 
                                                        COMPANY_CALENDAR 
                                                    WHERE 
                                                        YEAR(CALENDAR_DATE) = YEAR(CURDATE())
                                                );

                                        ";
                }
                else
                {
                    sMySqlString = @"
                                            SELECT 
                                                YEAR(CURDATE()) AS 'YEAR',
                                                COMP_NAME 
                                            FROM 
                                                COMPANY 
                                            WHERE 
	                                            STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND COMPANY_ID = '" + CompanyCode + @"' AND 
                                                COMPANY_ID 
                                            NOT IN
                                                (
                                                    SELECT 
                                                        DISTINCT(COMPANY_ID) 
                                                    FROM 
                                                        COMPANY_CALENDAR 
                                                    WHERE 
                                                        YEAR(CALENDAR_DATE) = YEAR(CURDATE())
                                                );

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

        public DataTable NextYearCompanyCalenderAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {

                    sMySqlString = @"
                                            SELECT 
                                                YEAR(DATE_ADD(CURDATE(), INTERVAL 1 YEAR)) AS 'YEAR',
                                                COMP_NAME 
                                            FROM 
                                                COMPANY 
                                            WHERE 
                                                STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND MONTH(CURDATE()) = '" + Constants.CON_COMPANY_CALENDER_ALERT_MONTH + @"' AND 
                                                COMPANY_ID 
                                            NOT IN
                                                (
                                                    SELECT 
                                                        DISTINCT(COMPANY_ID) 
                                                    FROM 
                                                        COMPANY_CALENDAR 
                                                    WHERE 
                                                        YEAR(CALENDAR_DATE) > YEAR(CURDATE())
                                                );

                                        ";
                }
                else
                {

                    sMySqlString = @"
                                            SELECT 
                                                YEAR(DATE_ADD(CURDATE(), INTERVAL 1 YEAR)) AS 'YEAR',
                                                COMP_NAME 
                                            FROM 
                                                COMPANY 
                                            WHERE
	                                            STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND COMPANY_ID = '" + CompanyCode + @"' AND 
                                                MONTH(CURDATE()) = '" + Constants.CON_COMPANY_CALENDER_ALERT_MONTH + @"' AND 
                                                COMPANY_ID 
                                            NOT IN
                                                (
                                                    SELECT 
                                                        DISTINCT(COMPANY_ID) 
                                                    FROM 
                                                        COMPANY_CALENDAR 
                                                    WHERE 
                                                        YEAR(CALENDAR_DATE) > YEAR(CURDATE())
                                                );

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

        public DataTable ReportTo1InactiveAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP1.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT',
                                                RP1.EMPLOYEE_ID, 
                                                RP1.TITLE, 
                                                RP1.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP1.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP1, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                E.REPORT_TO_1 = RP1.EMPLOYEE_ID AND 
                                                RP1.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP1.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP1.DESIGNATION_ID AND 
                                                RP1.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP1.EMPLOYEE_ID, 
                                                RP1.TITLE, 
                                                RP1.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP1.EMPLOYEE_STATUS;

                                        ";
                }
                else
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP1.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT',
                                                RP1.EMPLOYEE_ID, 
                                                RP1.TITLE, 
                                                RP1.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP1.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP1, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                C1.COMPANY_ID = '" + CompanyCode + @"' AND 
                                                E.REPORT_TO_1 = RP1.EMPLOYEE_ID AND 
                                                RP1.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP1.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP1.DESIGNATION_ID AND 
                                                RP1.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP1.EMPLOYEE_ID, 
                                                RP1.TITLE, 
                                                RP1.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP1.EMPLOYEE_STATUS;

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

        public DataTable ReportTo1InactiveEmployees(string InacitveReportTo1EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                E.EPF_NO,
                                                E.EMPLOYEE_ID, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                C.COMP_NAME, 
                                                D.DEPT_NAME, 
                                                E.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                COMPANY C, 
                                                DEPARTMENT D
                                            WHERE 
                                                E.COMPANY_ID = C.COMPANY_ID AND 
                                                E.DEPT_ID = D.DEPT_ID AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.REPORT_TO_1 = '" + InacitveReportTo1EmployeeID + @"';
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

        public DataTable ReportTo2InactiveAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP2.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT', 
                                                RP2.EMPLOYEE_ID, 
                                                RP2.TITLE, 
                                                RP2.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP2.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP2, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                E.REPORT_TO_2 = RP2.EMPLOYEE_ID AND 
                                                RP2.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP2.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP2.DESIGNATION_ID AND 
                                                RP2.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP2.EMPLOYEE_ID, 
                                                RP2.TITLE, 
                                                RP2.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP2.EMPLOYEE_STATUS;
                                        ";
                }
                else
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP2.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT', 
                                                RP2.EMPLOYEE_ID, 
                                                RP2.TITLE, 
                                                RP2.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP2.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP2, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                C1.COMPANY_ID = '" + CompanyCode + @"' AND 
                                                E.REPORT_TO_2 = RP2.EMPLOYEE_ID AND 
                                                RP2.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP2.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP2.DESIGNATION_ID AND 
                                                RP2.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP2.EMPLOYEE_ID, 
                                                RP2.TITLE, 
                                                RP2.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP2.EMPLOYEE_STATUS;
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

        public DataTable ReportTo2InactiveEmployees(string InacitveReportTo2EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                E.EPF_NO,
                                                E.EMPLOYEE_ID, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                C.COMP_NAME, 
                                                D.DEPT_NAME, 
                                                E.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                COMPANY C, 
                                                DEPARTMENT D
                                            WHERE 
                                                E.COMPANY_ID = C.COMPANY_ID AND 
                                                E.DEPT_ID = D.DEPT_ID AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.REPORT_TO_2 = '" + InacitveReportTo2EmployeeID + @"';
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

        public DataTable ReportTo3InactiveAlert(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = String.Empty;

                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP3.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT', 
                                                RP3.EMPLOYEE_ID, 
                                                RP3.TITLE, 
                                                RP3.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP3.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP3, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                E.REPORT_TO_3 = RP3.EMPLOYEE_ID AND 
                                                RP3.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP3.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP3.DESIGNATION_ID AND 
                                                RP3.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP3.EMPLOYEE_ID, 
                                                RP3.TITLE, 
                                                RP3.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP3.EMPLOYEE_STATUS;
                                        ";
                }
                else
                {
                    sMySqlString = @"
                                            SELECT 
                                                COUNT(RP3.EMPLOYEE_ID) AS 'SUBORDINATE_COUNT', 
                                                RP3.EMPLOYEE_ID, 
                                                RP3.TITLE, 
                                                RP3.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP3.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE RP3, 
                                                COMPANY C1, 
                                                DEPARTMENT D1, 
                                                EMPLOYEE_DESIGNATION ED1
                                            WHERE 
                                                C1.COMPANY_ID = '" + CompanyCode + @"' AND 
                                                E.REPORT_TO_3 = RP3.EMPLOYEE_ID AND 
                                                RP3.COMPANY_ID = C1.COMPANY_ID AND 
                                                RP3.DEPT_ID = D1.DEPT_ID AND 
                                                ED1.DESIGNATION_ID = RP3.DESIGNATION_ID AND 
                                                RP3.EMPLOYEE_STATUS <> '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                            GROUP BY 
                                                RP3.EMPLOYEE_ID, 
                                                RP3.TITLE, 
                                                RP3.INITIALS_NAME, 
                                                ED1.DESIGNATION_NAME, 
                                                C1.COMP_NAME, 
                                                D1.DEPT_NAME, 
                                                RP3.EMPLOYEE_STATUS;
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

        public DataTable ReportTo3InactiveEmployees(string InacitveReportTo3EmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                E.EPF_NO,
                                                E.EMPLOYEE_ID, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                C.COMP_NAME, 
                                                D.DEPT_NAME, 
                                                E.EMPLOYEE_STATUS
                                            FROM 
                                                EMPLOYEE E, 
                                                COMPANY C, 
                                                DEPARTMENT D
                                            WHERE 
                                                E.COMPANY_ID = C.COMPANY_ID AND 
                                                E.DEPT_ID = D.DEPT_ID AND 
                                                E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND 
                                                E.REPORT_TO_3 = '" + InacitveReportTo3EmployeeID + @"';
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

        public DataTable LeavePendingForCoveringAlert(string CoveringPersonEmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                EL.EMPLOYEE_ID, 
                                                EL.TITLE, 
                                                EL.INITIALS_NAME, 
                                                CONVERT(ELS.LEAVE_DATE, CHAR) AS 'LEAVE_DATE', 
                                                ELS.COVERED_BY, 
                                                EC.TITLE, 
                                                EC.INITIALS_NAME, 
                                                ELS.LEAVE_STATUS 
                                            FROM 
                                                EMPLOYEE_LEAVE_SCHEDULE ELS, 
                                                EMPLOYEE EL, 
                                                EMPLOYEE EC
                                            WHERE 
                                                ELS.EMPLOYEE_ID = EL.EMPLOYEE_ID AND 
                                                ELS.COVERED_BY = EC.EMPLOYEE_ID AND 
                                                ELS.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_ACTIVE_VALUE + @"' AND 
                                                ELS.COVERED_BY = '" + CoveringPersonEmployeeID + @"'
                                            ORDER BY 
                                                ELS.LEAVE_DATE DESC;
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

        public DataTable LeavePendingForRecommendAlert(string RecommendPersonEmployeeID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                EL.EMPLOYEE_ID, 
                                                EL.TITLE, 
                                                EL.INITIALS_NAME, 
                                                CONVERT(ELS.LEAVE_DATE, CHAR) AS 'LEAVE_DATE', 
                                                ELS.RECOMMAND_BY, 
                                                ER.TITLE, 
                                                ER.INITIALS_NAME, 
                                                ELS.LEAVE_STATUS 
                                            FROM 
                                                EMPLOYEE_LEAVE_SCHEDULE ELS, 
                                                EMPLOYEE EL, 
                                                EMPLOYEE ER
                                            WHERE 
                                                ELS.EMPLOYEE_ID = EL.EMPLOYEE_ID AND 
                                                ELS.RECOMMAND_BY = ER.EMPLOYEE_ID AND 
                                                ELS.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_COVERED + @"' AND 
                                                ELS.RECOMMAND_BY = '" + RecommendPersonEmployeeID + @"'
                                            ORDER BY 
                                                ELS.LEAVE_DATE DESC;
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

        public DataTable LeavePendingForHRApproveAlert()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                EL.EMPLOYEE_ID, 
                                                EL.TITLE, 
                                                EL.INITIALS_NAME, 
                                                CONVERT(ELS.LEAVE_DATE, CHAR) AS 'LEAVE_DATE', 
                                                ELS.RECOMMAND_BY, 
                                                ER.TITLE, 
                                                ER.INITIALS_NAME, 
                                                ELS.LEAVE_STATUS 
                                            FROM 
                                                EMPLOYEE_LEAVE_SCHEDULE ELS, 
                                                EMPLOYEE EL, 
                                                EMPLOYEE ER
                                            WHERE 
                                                ELS.EMPLOYEE_ID = EL.EMPLOYEE_ID AND 
                                                ELS.RECOMMAND_BY = ER.EMPLOYEE_ID AND 
                                                ELS.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_RECOMMAND + @"'
                                            ORDER BY 
                                                ELS.LEAVE_DATE DESC
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

        public DataTable NonRosterEmployeesOnRoseterHeader(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                if (CompanyCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {

                    sMySqlString = @"
                                        SELECT 
                                            C.COMPANY_ID, 
                                            C.COMP_NAME, 
                                            COUNT(ELD.EMPLOYEE_ID) AS 'EMPLOYEE_COUNT'
                                        FROM 
                                            EMPLOYEE_ROSTER_DATE ELD, 
                                            EMPLOYEE E, 
                                            ROSTER R, 
                                            COMPANY C, 
                                            DEPARTMENT D
                                        WHERE 
                                            E.EMPLOYEE_ID = ELD.EMPLOYEE_ID AND 
                                            R.ROSTR_ID = ELD.ROSTR_ID AND 
                                            ELD.DUTY_DATE >= CURDATE() AND 
                                            E.IS_ROSTER = '" + Constants.CON_NON_ROSTER_EMPLOYEE + @"' AND 
                                            E.COMPANY_ID = C.COMPANY_ID AND 
                                            D.DEPT_ID = E.DEPT_ID AND
                                            E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                        GROUP BY 
                                            C.COMPANY_ID, 
                                            C.COMP_NAME;
                                  ";

                }
                else
                {
                    sMySqlString = @"
                                        SELECT 
                                            C.COMPANY_ID, 
                                            C.COMP_NAME, 
                                            COUNT(ELD.EMPLOYEE_ID) AS 'EMPLOYEE_COUNT'
                                        FROM 
                                            EMPLOYEE_ROSTER_DATE ELD, 
                                            EMPLOYEE E, 
                                            ROSTER R, 
                                            COMPANY C, 
                                            DEPARTMENT D
                                        WHERE 
                                            E.EMPLOYEE_ID = ELD.EMPLOYEE_ID AND 
                                            R.ROSTR_ID = ELD.ROSTR_ID AND 
                                            ELD.DUTY_DATE >= CURDATE() AND 
                                            E.IS_ROSTER = '" + Constants.CON_NON_ROSTER_EMPLOYEE + @"' AND 
                                            E.COMPANY_ID = C.COMPANY_ID AND 
                                            E.COMPANY_ID = '" + CompanyCode + @"' AND 
                                            D.DEPT_ID = E.DEPT_ID AND
                                            E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                        GROUP BY 
                                            C.COMPANY_ID, 
                                            C.COMP_NAME;
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

        public DataTable NonRosterEmployeesOnRoseterNames(string CompanyCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;


                sMySqlString = @"

                                        SELECT 
                                            E.EMPLOYEE_ID, E.TITLE, E.KNOWN_NAME, D.DEPT_NAME, COUNT(ELD.EMPLOYEE_ID) AS 'EMPLOYEE_COUNT'
                                        FROM 
                                            EMPLOYEE_ROSTER_DATE ELD, 
                                            EMPLOYEE E, 
                                            ROSTER R, 
                                            COMPANY C, 
                                            DEPARTMENT D
                                        WHERE 
                                            E.EMPLOYEE_ID = ELD.EMPLOYEE_ID AND 
                                            R.ROSTR_ID = ELD.ROSTR_ID AND 
                                            ELD.DUTY_DATE >= CURDATE() AND 
                                            E.IS_ROSTER = '" + Constants.CON_NON_ROSTER_EMPLOYEE + @"' AND 
                                            E.COMPANY_ID = C.COMPANY_ID AND 
                                            D.DEPT_ID = E.DEPT_ID  AND 
                                            E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + @"' AND 
                                            E.COMPANY_ID='" + CompanyCode + @"'
                                        GROUP BY 
                                            E.EMPLOYEE_ID, E.TITLE, E.KNOWN_NAME, D.DEPT_NAME;
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

        public DataTable NonRosterEmployeesOnRoseterDetails(string EmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = String.Empty;
                

                    sMySqlString = @"

                                        SELECT 
                                            ELD.EMPLOYEE_ID, 
                                            ELD.ROSTR_ID, 
                                            C.COMP_NAME, 
                                            D.DEPT_NAME, 
                                            E.TITLE, 
                                            E.KNOWN_NAME, 
                                            CONVERT(ELD.DUTY_DATE, CHAR) AS 'DUTY_DATE', 
                                            CONVERT(R.FROM_TIME, CHAR) AS 'FROM_TIME', 
                                            CONVERT(R.TO_TIME, CHAR) AS 'TO_TIME'
                                        FROM 
                                            EMPLOYEE_ROSTER_DATE ELD, 
                                            EMPLOYEE E, 
                                            ROSTER R, 
                                            COMPANY C, 
                                            DEPARTMENT D
                                        WHERE 
                                            E.EMPLOYEE_ID = ELD.EMPLOYEE_ID AND 
                                            R.ROSTR_ID = ELD.ROSTR_ID AND 
                                            ELD.DUTY_DATE >= CURDATE() AND 
                                            E.IS_ROSTER = '" + Constants.CON_NON_ROSTER_EMPLOYEE + @"' AND 
                                            E.COMPANY_ID = C.COMPANY_ID AND 
                                            D.DEPT_ID = E.DEPT_ID  AND 
                                            E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + @"' AND 
                                            E.EMPLOYEE_ID='" + EmployeeID + @"';
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


        public Boolean CheckRoleEligibility(string AlertID, string RoleID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT * FROM ALERT_PRIVILEGES WHERE ALERT_ID = '" + AlertID + @"' AND HRIS_ROLE_ID = '" + RoleID + @"' AND STATUS = '" + Constants.CON_ACTIVE_STATUS + @"';
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    return true;
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

        //ASSIGN ALERT PRIVILEGES

        public DataTable populateCompanies()
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                        ROLE_ID, ROLE_NAME
                                    FROM
                                        HRIS_ROLE
                                    WHERE
                                        STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(string RoleID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        (SELECT 
                                                ALP.PRIVILEGE_ID
                                            FROM
                                                ALERT_PRIVILEGES ALP
                                            WHERE
                                                ALP.ALERT_ID = AL.ALERT_ID
                                                    AND ALP.HRIS_ROLE_ID = '" + RoleID + @"') AS 'PRIVILEGE_ID',
                                        AL.ALERT_ID,
                                        AL.ALERT_NAME,
                                        (SELECT 
                                                ALP.STATUS
                                            FROM
                                                ALERT_PRIVILEGES ALP
                                            WHERE
                                                ALP.ALERT_ID = AL.ALERT_ID
                                                    AND ALP.HRIS_ROLE_ID = '" + RoleID + @"') AS 'STATUS'
                                    FROM
                                        ALERTS AL;
                                ";

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    sMySqlString = @"
                                    SELECT 
                                        (SELECT PRIVILAGE_ID FROM REPORT_PRIVILAGES WHERE REPORT_ID = R.repcode AND ROLE_ID = '" + RoleID + @"') AS 'PRIVILAGE_ID',
                                        R.repcode AS 'REPORT_ID',
                                        R.description AS 'REPORT_NAME',
                                        (SELECT STATUS FROM REPORT_PRIVILAGES WHERE REPORT_ID = R.repcode AND ROLE_ID = '" + RoleID + @"') AS 'STATUS'
                                    FROM 
                                        REPORTS R
                                    WHERE 
                                         R.status = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                ";

                    mySqlCmd.Connection = mySqlCon;
                    mySqlCmd.CommandText = sMySqlString;

                    MySqlDataAdapter mySqlDa1 = new MySqlDataAdapter(mySqlCmd);
                    mySqlDa1.Fill(dataTable);
                }

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void insert(string ROLE_ID, string ADDED_BY, DataTable PrivilageData)
        {
            SerialHandler serialHandler = new SerialHandler();
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                for (int i = 0; i < PrivilageData.Rows.Count; i++)
                {
                    mySqlCmd.Parameters.Clear();

                    string PRIVILEGE_ID = PrivilageData.Rows[i]["PRIVILEGE_ID"].ToString();
                    string ALERT_ID = PrivilageData.Rows[i]["ALERT_ID"].ToString();
                    string ALERT_NAME = PrivilageData.Rows[i]["ALERT_NAME"].ToString();
                    string STATUS = PrivilageData.Rows[i]["STATUS"].ToString();

                    if (PRIVILEGE_ID == "")
                    {
                        PRIVILEGE_ID = serialHandler.getserila(mySqlCon, Constants.ALERT_PRIVILAGE_ID_STAMP);


                        mySqlCmd.Parameters.Add(new MySqlParameter("@PRIVILEGE_ID", PRIVILEGE_ID.Trim() == "" ? (object)DBNull.Value : PRIVILEGE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ROLE_ID", ROLE_ID.Trim() == "" ? (object)DBNull.Value : ROLE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ALERT_ID", ALERT_ID.Trim() == "" ? (object)DBNull.Value : ALERT_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS", STATUS.Trim() == "" ? (object)DBNull.Value : STATUS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", ADDED_BY.Trim() == "" ? (object)DBNull.Value : ADDED_BY.Trim()));

                        sMySqlString = @"
                                            INSERT INTO ALERT_PRIVILEGES (PRIVILEGE_ID, ALERT_ID, HRIS_ROLE_ID, STATUS, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE)
                                            VALUES (@PRIVILEGE_ID, @ALERT_ID, @ROLE_ID, @STATUS, @ADDED_BY, NOW(), @ADDED_BY, NOW());
                                        ";

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PRIVILEGE_ID", PRIVILEGE_ID.Trim() == "" ? (object)DBNull.Value : PRIVILEGE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS", STATUS.Trim() == "" ? (object)DBNull.Value : STATUS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ADDED_BY.Trim() == "" ? (object)DBNull.Value : ADDED_BY.Trim()));

                        sMySqlString = @"
                                            UPDATE ALERT_PRIVILEGES 
                                            SET  STATUS = @STATUS, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = NOW()
                                            WHERE PRIVILEGE_ID = @PRIVILEGE_ID
                                        ";

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
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