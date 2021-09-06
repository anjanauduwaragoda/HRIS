using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Reports
{
    public class ReportDataHandler : TemplateDataHandler
    {
        public DataTable populateEmployeeStatus()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"

                                            SELECT 
                                                STATUS_CODE, DESCRIPTION
                                            FROM
                                                EMPLOYEE_STATUS;
                                            
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

        public DataTable populateDepartments(string companyCode)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                DEPT_ID, DEPT_NAME
                                            FROM
                                                DEPARTMENT
                                            WHERE
                                                COMPANY_ID = '" + companyCode + @"' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                            
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

        public DataTable populateBranch(string companyCode)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                BRANCH_ID, BRANCH_NAME
                                            FROM
                                                COMPANY_BRANCH
                                            WHERE
                                                COMPANY_ID = '" + companyCode + @"' AND STATUS_CODE = '"+Constants.STATUS_ACTIVE_VALUE+"' ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateDepartmentName(string deptID)
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
                                                DEPT_ID = '" + deptID + @"';
                                            
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

        public DataTable populateBranchName(string branchID)
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
                                                BRANCH_ID = '" + branchID + @"';
                                            
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

        public string populateCompanyName(string compID)
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
                                                COMPANY_ID = '" + compID + @"';
                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COMP_NAME"].ToString();
                }
                else
                {
                    return " ";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getReportName(string ReportID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                description
                                            FROM
                                                REPORTS
                                            WHERE
                                                repcode = '" + ReportID + @"';
                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["description"].ToString();
                }
                else
                {
                    return " ";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateEmployeeName(string empID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                FULL_NAME
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empID + @"';
                                            
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

        public string getEmployeeName(string empID)
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
                                                EMPLOYEE_ID = '" + empID + @"';
                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["INITIALS_NAME"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getEmployeeCompany(string empID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"SELECT 
                                            c.COMP_NAME
                                        FROM
                                            COMPANY c,
                                            EMPLOYEE e
                                        WHERE
                                            e.EMPLOYEE_ID = '" + empID + @"'
                                                AND e.COMPANY_ID = c.COMPANY_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COMP_NAME"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getEmployeeCompanyID(string empID)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"SELECT 
                                            c.COMPANY_ID
                                        FROM
                                            COMPANY c,
                                            EMPLOYEE e
                                        WHERE
                                            e.EMPLOYEE_ID = '" + empID + @"'
                                                AND e.COMPANY_ID = c.COMPANY_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["COMPANY_ID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isConfigAttendance(string EmployeeID, string Date, string Time)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"
                                            SELECT 
                                                *
                                            FROM
                                                ATTENDANCE A,
                                                ATTENDANCE_LOG AG
                                            WHERE
                                                AG.EMPLOYEE_ID = '" + EmployeeID + @"'
                                                    AND AG.ATT_DATE = '" + Date + @"'
                                                    AND AG.ATT_TIME = '" + Time + @"'
                                                    AND A.EMPLOYEE_ID = AG.EMPLOYEE_ID
                                                    AND A.ATT_DATE = AG.ATT_DATE
                                                    AND A.ATT_TIME = AG.ATT_TIME;
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


        public DataTable populateReports(string ROLE_ID)
        {
            try
            {
                dataTable = new DataTable();
                dataTable.Rows.Clear();
                //string sMySqlString = "SELECT repcode as REPCODE,description as DESCRIP FROM REPORTS where status = 'C' order by repcode";
                string sMySqlString = @"
                                            SELECT 
                                                R.repcode as REPCODE, R.description as DESCRIP 
                                            FROM 
                                                REPORTS R, REPORT_PRIVILAGES RP 
                                            WHERE 
                                                R.status = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND R.repcode = RP.REPORT_ID AND RP.STATUS = '" + Constants.STATUS_ACTIVE_VALUE + @"' AND RP.ROLE_ID = '" + ROLE_ID + @"'
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

        public DataTable populaterep0001(string fromdate, string todate, string sCOMPANY_ID, string mempcode, string deptcode)
        {
            string stamp = deptcode.Trim();
            if (stamp != "")
            {
                stamp = stamp[0].ToString() + stamp[1].ToString();
            }
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,CAST(OT_HOURS as DECIMAL(9,2)) AS OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME' " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = 'S001' " +
                                       " order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,CAST(OT_HOURS as DECIMAL(9,2)) AS OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME' " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = 'S001' " +
                                       " and ats.EMPLOYEE_ID= '" + mempcode.ToString() + "' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (deptcode == "")
                        {
                            sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,CAST(OT_HOURS as DECIMAL(9,2)) AS OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME' " +
                                           " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and " +
                                           " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = 'S001' " +
                                            " and ats.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                        }
                        else
                        {
                            stamp = deptcode.Trim();
                            if (stamp != "")
                            {
                                stamp = stamp[0].ToString() + stamp[1].ToString();
                            }
                            if (stamp == Constants.DEPARTMENT_ID_STAMP)
                            {
                                sMySqlString = @"
                                                SELECT 
                                                    IN_DATE,    IN_TIME,    LATE_MINUTES,    IN_LOCATION,    OUT_DATE,    OUT_TIME,    EARLY_MINUTES,    OUT_LOCATION,    ats.COMPANY_ID as COMPANY_ID,
                                                    ats.EMPLOYEE_ID as EMPLOYEE_ID,    CAST(OT_HOURS as DECIMAL (9 , 2 )) AS OT_HOURS,    REMARK,    INITIALS_NAME,    KNOWN_NAME,
                                                    EPF_NO,    COMP_NAME,    DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME'
                                                FROM
                                                    ATTENDANCE_SUMMARY ats,    EMPLOYEE e,    COMPANY c,	DEPARTMENT dpt,    EMPLOYEE_DESIGNATION ed
                                                where
                                                    ats.EMPLOYEE_ID = e.EMPLOYEE_ID        and ats.COMPANY_ID = c.COMPANY_ID        and e.DESIGNATION_ID = ed.DESIGNATION_ID  and e.EMPLOYEE_STATUS = 'S001'
                                                        and ats.IN_DATE >= '" + fromdate + @"'        and ats.IN_DATE <= '" + todate + @"'        and ats.COMPANY_ID = '" + sCOMPANY_ID.Trim() + @"'
                                                        and e.DEPT_ID = dpt.DEPT_ID        and e.DEPT_ID = '" + deptcode.Trim() + @"'order by ats.EMPLOYEE_ID , ats.IN_DATE
                                            ";
                            }
                            else
                            {
                                sMySqlString = @"
                                                SELECT 
                                                    IN_DATE,    IN_TIME,    LATE_MINUTES,    IN_LOCATION,    OUT_DATE,    OUT_TIME,    EARLY_MINUTES,    OUT_LOCATION,    ats.COMPANY_ID as COMPANY_ID,
                                                    ats.EMPLOYEE_ID as EMPLOYEE_ID,    CAST(OT_HOURS as DECIMAL (9 , 2 )) AS OT_HOURS,    REMARK,    INITIALS_NAME,    KNOWN_NAME,
                                                    EPF_NO,    COMP_NAME,    DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME'
                                                FROM
                                                    ATTENDANCE_SUMMARY ats,    EMPLOYEE e,    COMPANY c,	DEPARTMENT dpt,    EMPLOYEE_DESIGNATION ed
                                                where
                                                    ats.EMPLOYEE_ID = e.EMPLOYEE_ID        and ats.COMPANY_ID = c.COMPANY_ID        and e.DESIGNATION_ID = ed.DESIGNATION_ID  and e.EMPLOYEE_STATUS = 'S001'
                                                        and ats.IN_DATE >= '" + fromdate + @"'        and ats.IN_DATE <= '" + todate + @"'        and ats.COMPANY_ID = '" + sCOMPANY_ID.Trim() + @"'
                                                        and e.DEPT_ID = dpt.DEPT_ID        and e.BRANCH_ID = '" + deptcode.Trim() + @"'order by ats.EMPLOYEE_ID , ats.IN_DATE
                                            ";
                            }
                        }
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,CAST(OT_HOURS as DECIMAL(9,2)) AS OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME, (SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_ID = ats.DEPT_ID) AS 'DEPT_NAME', (SELECT DIV_NAME FROM DIVISION WHERE DIVISION_ID = ats.DIVISION_ID) AS 'DIV_NAME', (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = (SELECT BRANCH_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = ats.EMPLOYEE_ID)) AS 'BRANCH_NAME' " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                       " and ats.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' and ats.EMPLOYEE_ID= '" + mempcode.ToString() + "' and e.EMPLOYEE_STATUS = 'S001' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }

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

        //-- 	Late Commers Report
        public DataTable populaterep0002(string fromdate, string todate, string sCOMPANY_ID, string empCode, string depCode)
        {
            string stamp = depCode.Trim();
            if (stamp != "")
            {
                stamp = stamp[0].ToString() + stamp[1].ToString();
            }
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (empCode == "")
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                                           "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b, DEPARTMENT d, EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                           "and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME ,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                                               "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b , DEPARTMENT d , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                               "and e.EMPLOYEE_ID='" + empCode + "' and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID  order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                }
                else
                {
                    if (empCode == "")
                    {
                        if (depCode == "")
                        {
                            //sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME ,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                            //                   "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b , DEPARTMENT d , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                            //                   "and ats.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID  order by  ats.EMPLOYEE_ID,ats.IN_DATE ";


                            sMySqlString = @"SELECT 
                                            ATS.IN_DATE, ATS.IN_TIME, ATS.LATE_MINUTES, ATS.IN_LOCATION, ATS.COMPANY_ID, ATS.EMPLOYEE_ID, ATS.REMARK, 
                                            (SELECT E.INITIALS_NAME FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = ATS.EMPLOYEE_ID) AS 'INITIALS_NAME', 
                                            (SELECT E.KNOWN_NAME FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = ATS.EMPLOYEE_ID) AS 'KNOWN_NAME', 
                                            (SELECT C.COMP_NAME FROM COMPANY C WHERE C.COMPANY_ID = ATS.COMPANY_ID) AS 'COMP_NAME', 
                                            ATS.OUT_TIME, 
                                            (SELECT E.EPF_NO FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = ATS.EMPLOYEE_ID) AS 'EPF_NO',
                                            (SELECT CB.BRANCH_NAME FROM COMPANY_BRANCH CB WHERE CB.BRANCH_ID = (SELECT E.BRANCH_ID FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = ATS.EMPLOYEE_ID)) AS 'BRANCH_NAME',
                                            (SELECT D.DEPT_NAME FROM DEPARTMENT D WHERE D.DEPT_ID = ATS.DEPT_ID) AS 'DEPT_NAME',
                                            (SELECT ED.DESIGNATION_NAME FROM EMPLOYEE_DESIGNATION ED WHERE ED.DESIGNATION_ID = (SELECT E.DESIGNATION_ID FROM EMPLOYEE E WHERE E.EMPLOYEE_ID = ATS.EMPLOYEE_ID)) AS 'DESIGNATION_NAME'
                                            FROM 
                                            ATTENDANCE_SUMMARY ATS
                                            WHERE 
                                            (ATS.IN_DATE BETWEEN '" + fromdate + @"' AND '" + todate + @"') AND ATS.COMPANY_ID = '" + sCOMPANY_ID.Trim() + @"' AND ATS.LATE_MINUTES > 0 
                                            ORDER BY ATS.EMPLOYEE_ID , ATS.IN_DATE";

                        }
                        else
                        {
                            stamp = depCode.Trim();
                            if (stamp != "")
                            {
                                stamp = stamp[0].ToString() + stamp[1].ToString();
                            }

                            if (stamp == Constants.DEPARTMENT_ID_STAMP)
                            {
                                sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME ,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                                                   "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b , DEPARTMENT d , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                                   "and ats.DEPT_ID='" + depCode + "' and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID  order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                            }
                            else
                            {
                                sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME ,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                                                      "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b , DEPARTMENT d , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                                      "and e.BRANCH_ID ='" + depCode.Trim() + "' and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID  order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                            }
                        }
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,REMARK,INITIALS_NAME,KNOWN_NAME,COMP_NAME ,OUT_TIME,EPF_NO,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME " +
                                            "FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c ,COMPANY_BRANCH b , DEPARTMENT d , EMPLOYEE_DESIGNATION ed where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' " +
                                            "and e.EMPLOYEE_ID='" + empCode + "' and LATE_MINUTES > 0 and b.BRANCH_ID = e.BRANCH_ID and d.DEPT_ID = e.DEPT_ID and ed.DESIGNATION_ID = e.DESIGNATION_ID  order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
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

        //EMPLOYEE DETAILS REPORT START


        public DataTable populaterep0003IND(string sEMP_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                        E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                        ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                    FROM 
                                        EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                    WHERE 
                                        E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                        E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                        E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND E.EMPLOYEE_ID='" + sEMP_ID + @"'

                                    ORDER BY 
                                        C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
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

        public DataTable populaterep0003(string sCOMPANY_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {

                    sMySqlString = @"
                                            SELECT 
                                                C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                                E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                                ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                            FROM 
                                                EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                            WHERE 
                                                E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                                E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                                E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                                E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID

                                            ORDER BY 
                                                C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
                                    ";

                }
                else
                {

                    sMySqlString = @"
                                        SELECT 
                                            C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                            E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                            ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                        WHERE 
                                            E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                            E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                            E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                            E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND C.COMPANY_ID = '" + sCOMPANY_ID + @"'

                                        ORDER 
                                            BY C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
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

        public DataTable populaterep0003(string sCOMPANY_ID, string sDEPT_ID)
        {
            string stamp = sDEPT_ID.Trim();
            if (stamp != "")
            {
                stamp = stamp[0].ToString() + stamp[1].ToString();
            }
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                stamp = sDEPT_ID.Trim();
                if (stamp != "")
                {
                    stamp = stamp[0].ToString() + stamp[1].ToString();
                }

                if (stamp == Constants.DEPARTMENT_ID_STAMP)
                {

                    sMySqlString = @"
                                        SELECT 
                                            C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                            E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                            ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                        WHERE 
                                            E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                            E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                            E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                            E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND C.COMPANY_ID = '" + sCOMPANY_ID + @"' AND DPT.DEPT_ID='" + sDEPT_ID + @"'

                                        ORDER BY 
                                            C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
                                    ";
                }
                else
                {

                    sMySqlString = @"
                                        SELECT 
                                            C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                            E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                            ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                        WHERE 
                                            E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                            E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                            E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                            E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND C.COMPANY_ID = '" + sCOMPANY_ID + @"' AND E.BRANCH_ID='" + sDEPT_ID + @"'

                                        ORDER BY 
                                            C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
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

        //Other States

        public DataTable opopulaterep0003IND(string sEMP_ID, string statuscode)
        {
            string sMySqlString = "";
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                        E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                        ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                    FROM 
                                        EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                    WHERE 
                                        E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                        E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                        E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND E.EMPLOYEE_ID='" + sEMP_ID + @"' AND ES.STATUS_CODE = '" + statuscode + @"'

                                    ORDER BY 
                                        C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
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

        public DataTable opopulaterep0003(string sCOMPANY_ID, string statuscode)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"
                                        SELECT 
                                            C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                            E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                            ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                        WHERE 
                                            E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                            E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                            E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                            E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND ES.STATUS_CODE = '" + statuscode + @"'

                                        ORDER BY 
                                            C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
                                    ";
                }
                else
                {

                    sMySqlString = @"
                                        SELECT 
                                            C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                            E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                            ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                        WHERE 
                                            E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                            E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                            E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                            E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND C.COMPANY_ID='" + sCOMPANY_ID + @"' AND ES.STATUS_CODE = '" + statuscode + @"'

                                        ORDER BY 
                                            C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME
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

        public DataTable opopulaterep0003(string sCOMPANY_ID, string sDEPT_ID, string statuscode)
        {
            string stamp = sDEPT_ID.Trim();
            stamp = stamp[0].ToString() + stamp[1].ToString();

            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();


                stamp = sDEPT_ID.Trim();
                stamp = stamp[0].ToString() + stamp[1].ToString();

                if (stamp == Constants.DEPARTMENT_ID_STAMP)
                {

                    sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                        E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                        ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                    FROM 
                                        EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                    WHERE 
                                        E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                        E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                        E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND DPT.DEPT_ID='" + sDEPT_ID + @"' AND C.COMPANY_ID='" + sCOMPANY_ID + @"' AND ES.STATUS_CODE = '" + statuscode + @"'

                                    ORDER BY 
                                        C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME      
                                ";
                }
                else
                {

                    sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, DPT.DEPT_NAME, DIVS.DIV_NAME, E.EPF_NO, E.INITIALS_NAME, E.FULL_NAME, ED.DESIGNATION_NAME, 
                                        E.EMP_NIC, CONVERT( E.DOB , CHAR) AS 'DOB', CONVERT( E.DOJ , CHAR) AS 'DOJ', ES.DESCRIPTION, E.PERMANENT_ADDRESS, E.COST_CENTER, E.PROFIT_CENTER, ET.TYPE_NAME,
                                        ER.ROLE_NAME,E.REPORT_TO_1 AS SUPERVISOR, E.GENDER
 
                                    FROM 
                                        EMPLOYEE E, COMPANY C, DEPARTMENT DPT, DIVISION DIVS, EMPLOYEE_DESIGNATION ED, EMPLOYEE_STATUS ES, EMPLOYEE_TYPE ET, EMPLOYEE_ROLE ER 

                                    WHERE 
                                        E.COMPANY_ID = C.COMPANY_ID AND E.DEPT_ID = DPT.DEPT_ID AND 
                                        E.DIVISION_ID = DIVS.DIVISION_ID AND DPT.DEPT_ID = DIVS.DEPT_ID AND
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_STATUS = ES.STATUS_CODE AND 
                                        E.EMP_TYPE_ID = ET.EMP_TYPE_ID AND E.ROLE_ID = ER.ROLE_ID AND E.BRANCH_ID='" + sDEPT_ID + @"' AND C.COMPANY_ID='" + sCOMPANY_ID + @"' AND ES.STATUS_CODE = '" + statuscode + @"'

                                    ORDER BY 
                                        C.COMPANY_ID , DPT.DEPT_NAME , DIVS.DIV_NAME      
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
        //

        public DataTable getSupervisors()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = "SELECT E.EMPLOYEE_ID, E.INITIALS_NAME FROM EMPLOYEE E";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getSupervisorDesignations()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = "SELECT E.EMPLOYEE_ID,E.INITIALS_NAME,ED.DESIGNATION_NAME FROM EMPLOYEE E,EMPLOYEE_DESIGNATION ED WHERE E.DESIGNATION_ID = ED.DESIGNATION_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getEmployeeReportDataTable(DataTable EmployeeDetails)
        {
            try
            {
                EmployeeDetails.Columns.Add("SUPERVISOR_DESIGNATION_NAME");

                DataTable Supervisours = new DataTable();
                DataTable SupervisourDesignations = new DataTable();

                Supervisours = getSupervisors().Copy();
                SupervisourDesignations = getSupervisorDesignations().Copy();


                for (int i = 0; i < EmployeeDetails.Rows.Count; i++)
                {
                    string SupervisorEmpID = String.Empty;
                    string SupervisorName = String.Empty;
                    string SupervisorDesignation = String.Empty;

                    SupervisorEmpID = EmployeeDetails.Rows[i]["SUPERVISOR"].ToString();

                    if (SupervisorEmpID != String.Empty)
                    {
                        try
                        {
                            DataRow[] rowsName = Supervisours.Select("EMPLOYEE_ID = '" + SupervisorEmpID + "'");
                            SupervisorName = rowsName[0]["INITIALS_NAME"].ToString();

                            DataRow[] rowsDesignation = SupervisourDesignations.Select("EMPLOYEE_ID = '" + SupervisorEmpID + "'");
                            SupervisorDesignation = rowsDesignation[0]["DESIGNATION_NAME"].ToString();

                            EmployeeDetails.Rows[i]["SUPERVISOR"] = SupervisorName.Trim();
                            EmployeeDetails.Rows[i]["SUPERVISOR_DESIGNATION_NAME"] = SupervisorDesignation.Trim();
                        }
                        catch
                        {
                            EmployeeDetails.Rows[i]["SUPERVISOR"] = String.Empty;
                            EmployeeDetails.Rows[i]["SUPERVISOR_DESIGNATION_NAME"] = String.Empty;
                        }
                    }
                    else
                    {
                        EmployeeDetails.Rows[i]["SUPERVISOR"] = String.Empty;
                        EmployeeDetails.Rows[i]["SUPERVISOR_DESIGNATION_NAME"] = String.Empty;
                    }
                }


                return EmployeeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //EMPLOYEE DETAILS REPORT END

        public DataTable populaterep0004(string fromdate, string todate, string sCOMPANY_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            string mval = "";
            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@val", mval.Trim() == "" ? (object)DBNull.Value : mval.Trim()));

                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "SELECT c.EMPLOYEE_ID as EMPLOYEE_ID,c.INITIALS_NAME as INITIALS_NAME,c.IN_DATE as IN_DATE,consec_set,COUNT(1) AS consec_count_Days,com.COMP_NAME as COMP_NAME FROM " +
                                   "(SELECT e.EMPLOYEE_ID,e.INITIALS_NAME,e.COMPANY_ID, a.IN_DATE,a.NUMBER_OF_DAYS, a.REMARK,a.IS_ABSENT, " +
                                   "IF(a.NUMBER_OF_DAYS is null, @val:=@val+1, @val+ 900000000000) AS consec_set FROM EMPLOYEE e CROSS JOIN (SELECT @val:=0) var_init " +
                                   "JOIN ATTENDANCE_SUMMARY a ON e.EMPLOYEE_ID = a.EMPLOYEE_ID " +
                                   "and a.IN_DATE + interval 1 day WHERE a.REMARK like 'Work%' and a.is_absent = '" + Constants.STATUS_ACTIVE_VALUE + "' and a.IN_DATE >= '" + fromdate + "' and a.IN_DATE <= '" + todate + "'   order by e.EMPLOYEE_ID, a.IN_DATE ) c " +
                                   "inner join COMPANY com on com.COMPANY_ID = c.COMPANY_ID where c.COMPANY_ID = com. COMPANY_ID " +
                                   "GROUP BY c.consec_set HAVING COUNT(1) >= 3";
                }
                else
                {
                    sMySqlString = "SELECT c.EMPLOYEE_ID as EMPLOYEE_ID,c.INITIALS_NAME as INITIALS_NAME,c.IN_DATE as IN_DATE,consec_set,COUNT(1) AS consec_count_Days,com.COMP_NAME as COMP_NAME FROM " +
                                   "(SELECT e.EMPLOYEE_ID,e.INITIALS_NAME,e.COMPANY_ID, a.IN_DATE,a.NUMBER_OF_DAYS, a.REMARK,a.IS_ABSENT, " +
                                   "IF(a.NUMBER_OF_DAYS is null, @val:=@val+1, @val+ 900000000000) AS consec_set FROM EMPLOYEE e CROSS JOIN (SELECT @val:=0) var_init " +
                                   "JOIN ATTENDANCE_SUMMARY a ON e.EMPLOYEE_ID = a.EMPLOYEE_ID " +
                                   "and a.IN_DATE + interval 1 day WHERE a.REMARK like 'Work%' and a.is_absent = '" + Constants.STATUS_ACTIVE_VALUE + "'  and e.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' and  a.IN_DATE >= '" + fromdate + "' and a.IN_DATE <= '" + todate + "' order by e.EMPLOYEE_ID, a.IN_DATE ) c " +
                                   "inner join COMPANY com on com.COMPANY_ID = c.COMPANY_ID where c.COMPANY_ID = com. COMPANY_ID " +
                                   "GROUP BY c.consec_set HAVING COUNT(1) >= 3";
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

        public DataTable populaterep0005(string fromdate, string todate, string sCOMPANY_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "SELECT DEPT_NAME,(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE e where   e.dept_id = d.dept_id and  e.doj < '" + fromdate + "' ) as APPCARDER, " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE e where   e.dept_id = d.dept_id and  e.doj >= '" + fromdate + "' and e.doj <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as TOTNEWREC , " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE em where   em.dept_id = d.dept_id and em.RESIGNED_DATE >= '" + fromdate + "' and em.RESIGNED_DATE <= '" + todate + "' and EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "') as TOTRESIGN, " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE_TRNSFERS et where et.FROM_DEPT_ID = d.dept_id and et.START_DATE >= '" + fromdate + "' and et.START_DATE <= '" + todate + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "') as TOTTRANSFER " +
                                   " FROM EMPLOYEE e , DEPARTMENT d where e.dept_id = d.dept_id  group by d.dept_id";
                }
                else
                {
                    sMySqlString = "SELECT DEPT_NAME,(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE e where   e.dept_id = d.dept_id and  e.doj < '" + fromdate + "' ) as APPCARDER, " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE e where   e.dept_id = d.dept_id and  e.doj >= '" + fromdate + "' and e.doj <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as TOTNEWREC , " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE em where   em.dept_id = d.dept_id and em.RESIGNED_DATE >= '" + fromdate + "' and em.RESIGNED_DATE <= '" + todate + "' and EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "') as TOTRESIGN, " +
                                   "(SELECT count(EMPLOYEE_ID) FROM EMPLOYEE_TRNSFERS et where et.FROM_DEPT_ID = d.dept_id and et.START_DATE >= '" + fromdate + "' and et.START_DATE <= '" + todate + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "') as TOTTRANSFER " +
                                   " FROM EMPLOYEE e , DEPARTMENT d where e.dept_id = d.dept_id and e.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' group by d.dept_id";
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

        public DataTable populaterep0007(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "Select er.ROLE_NAME as ROLENAME, es.DESCRIPTION as EMP_STATUS,e.EMPLOYEE_ID AS EMPLOYEE_ID,e.EPF_NO AS EPF_NO,e.EMP_NIC as EMP_NIC,e.INITIALS_NAME AS INITIALS_NAME , e.DOJ as DOJ," +
                                    "c.COMP_NAME as COMP_NAME, dp.DEPT_NAME as DEPT_NAME, dv.DIV_NAME as DIV_NAME, ed.DESIGNATION_NAME as DESIGNATION_NAME " +
                                    "FROM COMPANY c, DEPARTMENT dp, DIVISION dv, EMPLOYEE_STATUS es, EMPLOYEE_DESIGNATION ed, EMPLOYEE_ROLE er, EMPLOYEE e " +
                                    "WHERE e.COMPANY_ID = c.COMPANY_ID " +
                                    "AND e.DEPT_ID = dp.DEPT_ID and e.DIVISION_ID = dv.DIVISION_ID and e.EMPLOYEE_STATUS = es.STATUS_CODE " +
                                    "AND e.DESIGNATION_ID = ed.DESIGNATION_ID AND e.ROLE_ID = er.ROLE_ID " +
                                    "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' " +
                                    "order by c.COMPANY_ID,dp.DEPT_NAME,e.DOJ";

                }
                else
                {
                    sMySqlString = "Select er.ROLE_NAME as ROLENAME, es.DESCRIPTION as EMP_STATUS,e.EMPLOYEE_ID AS EMPLOYEE_ID,e.EPF_NO AS EPF_NO,e.EMP_NIC as EMP_NIC,e.INITIALS_NAME AS INITIALS_NAME , e.DOJ as DOJ, " +
                                   "c.COMP_NAME as COMP_NAME, dp.DEPT_NAME as DEPT_NAME, dv.DIV_NAME as DIV_NAME, ed.DESIGNATION_NAME as DESIGNATION_NAME " +
                                   "FROM COMPANY c, DEPARTMENT dp, DIVISION dv, EMPLOYEE_STATUS es, EMPLOYEE_DESIGNATION ed, EMPLOYEE_ROLE er, EMPLOYEE e " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID " +
                                   "AND e.DEPT_ID = dp.DEPT_ID AND e.DIVISION_ID = dv.DIVISION_ID AND e.EMPLOYEE_STATUS = es.STATUS_CODE " +
                                   "AND e.DESIGNATION_ID = ed.DESIGNATION_ID AND e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' " +
                                   "AND c.COMPANY_ID = '" + sCOMPANY_ID + "' order by dp.DEPT_NAME,e.DOJ";

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

        public DataTable populaterep0007_Sub(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALREC " +
                                    "FROM  EMPLOYEE_ROLE er, EMPLOYEE e " +
                                    "WHERE  e.ROLE_ID = er.ROLE_ID " +
                                    "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' " +
                                    "group by er.ROLE_NAME";

                }
                else
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALREC " +
                                   "FROM  EMPLOYEE_ROLE er, EMPLOYEE e " +
                                   "WHERE  e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' group by er.ROLE_NAME";

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


        public DataTable populaterep0008(string fromdate, string todate, string sCOMPANY_ID, string mempcode)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME,DEPT_NAME " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed , DEPARTMENT d where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and e.DEPT_ID = d.DEPT_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "' and  OT_HOURS > 0 " +
                                       " order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME,DEPT_NAME " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed , DEPARTMENT d where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and e.DEPT_ID = d.DEPT_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "'  and  OT_HOURS > 0 " +
                                       " and ats.EMPLOYEE_ID= '" + mempcode.ToString() + "' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME,DEPT_NAME " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed , DEPARTMENT d where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and e.DEPT_ID = d.DEPT_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "'  and  OT_HOURS > 0 " +
                                        " and ats.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }
                    else
                    {
                        sMySqlString = "SELECT IN_DATE,IN_TIME,LATE_MINUTES,IN_LOCATION,OUT_DATE,OUT_TIME,EARLY_MINUTES,OUT_LOCATION,ats.COMPANY_ID as COMPANY_ID,ats.EMPLOYEE_ID as EMPLOYEE_ID,OT_HOURS,REMARK,INITIALS_NAME,KNOWN_NAME,EPF_NO,COMP_NAME,DESIGNATION_NAME,DEPT_NAME " +
                                       " FROM ATTENDANCE_SUMMARY  ats , EMPLOYEE e, COMPANY c , EMPLOYEE_DESIGNATION ed , DEPARTMENT d where ats.EMPLOYEE_ID = e.EMPLOYEE_ID and ats.COMPANY_ID = c.COMPANY_ID and e.DESIGNATION_ID = ed.DESIGNATION_ID and e.DEPT_ID = d.DEPT_ID and " +
                                       " ats.IN_DATE >= '" + fromdate + "' and  ats.IN_DATE <= '" + todate + "'  and  OT_HOURS > 0 " +
                                       " and ats.COMPANY_ID='" + sCOMPANY_ID.Trim() + "' and ats.EMPLOYEE_ID= '" + mempcode.ToString() + "' order by  ats.EMPLOYEE_ID,ats.IN_DATE ";
                    }

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

        public DataTable populaterep0009(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "Select IFNULL((s.BASIC_AMOUNT + s.BUDGETARY_ALLOWANCE_AMOUNT),'0') as TOTBASICSAL,er.ROLE_NAME as ROLENAME, es.DESCRIPTION as EMP_STATUS,e.EMPLOYEE_ID AS EMPLOYEE_ID,e.EPF_NO AS EPF_NO,e.EMP_NIC as EMP_NIC,e.INITIALS_NAME AS INITIALS_NAME , e.DOJ as DOJ,e.RESIGNED_DATE as RESIGNED_DATE," +
                                   "c.COMP_NAME as COMP_NAME, dp.DEPT_NAME as DEPT_NAME, dv.DIV_NAME as DIV_NAME, ed.DESIGNATION_NAME as DESIGNATION_NAME " +
                                   "FROM COMPANY c, DEPARTMENT dp, DIVISION dv, EMPLOYEE_STATUS es, EMPLOYEE_DESIGNATION ed, EMPLOYEE_ROLE er, EMPLOYEE e left outer join SALARY s on e.EMPLOYEE_ID = s.EMPLOYEE_ID " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID " +
                                   "AND e.DEPT_ID = dp.DEPT_ID and e.DIVISION_ID = dv.DIVISION_ID and e.EMPLOYEE_STATUS = es.STATUS_CODE " +
                                   "AND e.DESIGNATION_ID = ed.DESIGNATION_ID AND e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.RESIGNED_DATE >= '" + fromdate + "' and e.RESIGNED_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "' " +
                                   "order by c.COMPANY_ID,dp.DEPT_NAME,e.DOJ";
                }
                else
                {
                    sMySqlString = "Select IFNULL((s.BASIC_AMOUNT + s.BUDGETARY_ALLOWANCE_AMOUNT),'0')  as TOTBASICSAL,er.ROLE_NAME as ROLENAME, es.DESCRIPTION as EMP_STATUS,e.EMPLOYEE_ID AS EMPLOYEE_ID,e.EPF_NO AS EPF_NO,e.EMP_NIC as EMP_NIC,e.INITIALS_NAME AS INITIALS_NAME , e.DOJ as DOJ,e.RESIGNED_DATE as RESIGNED_DATE," +
                                   "c.COMP_NAME as COMP_NAME, dp.DEPT_NAME as DEPT_NAME, dv.DIV_NAME as DIV_NAME, ed.DESIGNATION_NAME as DESIGNATION_NAME " +
                                   "FROM COMPANY c, DEPARTMENT dp, DIVISION dv, EMPLOYEE_STATUS es, EMPLOYEE_DESIGNATION ed, EMPLOYEE_ROLE er, EMPLOYEE e left outer join SALARY s on e.EMPLOYEE_ID = s.EMPLOYEE_ID " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID " +
                                   "AND e.DEPT_ID = dp.DEPT_ID and e.DIVISION_ID = dv.DIVISION_ID and e.EMPLOYEE_STATUS = es.STATUS_CODE " +
                                   "AND e.DESIGNATION_ID = ed.DESIGNATION_ID AND e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.RESIGNED_DATE >= '" + fromdate + "' and e.RESIGNED_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "' " +
                                   "AND c.COMPANY_ID = '" + sCOMPANY_ID + "' order by dp.DEPT_NAME,e.DOJ";
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

        public DataTable populaterep0009_Sub(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALREC " +
                                    "FROM  EMPLOYEE_ROLE er, EMPLOYEE e " +
                                    "WHERE  e.ROLE_ID = er.ROLE_ID " +
                                    "AND e.RESIGNED_DATE >= '" + fromdate + "' and e.RESIGNED_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "' " +
                                    "group by er.ROLE_NAME";
                }
                else
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALREC " +
                                   "FROM  EMPLOYEE_ROLE er, EMPLOYEE e " +
                                   "WHERE  e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.RESIGNED_DATE >= '" + fromdate + "' and e.RESIGNED_DATE <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_RESIGN + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' group by er.ROLE_NAME";
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

        public DataTable populaterep0010_Employeetype(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "Select et.TYPE_NAME as TYPE_NAME,count(EMPLOYEE_ID) as TOTALEMP , COMP_NAME FROM EMPLOYEE e, EMPLOYEE_TYPE et , COMPANY c " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID  and e.EMP_TYPE_ID = et.EMP_TYPE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "group by e.COMPANY_ID,et.EMP_TYPE_ID";
                }
                else
                {
                    sMySqlString = "Select et.TYPE_NAME as TYPE_NAME,count(EMPLOYEE_ID) as TOTALEMP , COMP_NAME FROM EMPLOYEE e, EMPLOYEE_TYPE et , COMPANY c " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID  and e.EMP_TYPE_ID = et.EMP_TYPE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' and e.DOJ <= '" + todate + "' and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' " +
                                   "group by e.COMPANY_ID,et.EMP_TYPE_ID";
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

        public DataTable populaterep0010_GenderAnalysis(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "Select count(distinct EMPLOYEE_ID)  as TOTEMP , GENDER,COMP_NAME FROM EMPLOYEE e ,COMPANY c  " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID  " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "group by e.COMPANY_ID,gender";
                }
                else
                {
                    sMySqlString = "Select count(distinct EMPLOYEE_ID)  as TOTEMP , GENDER,COMP_NAME FROM EMPLOYEE e ,COMPANY c " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID  " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' " +
                                   "group by e.COMPANY_ID,gender";
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

        public DataTable populaterep0010_DesignationAnalysis(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "Select count(distinct EMPLOYEE_ID)  as TOTEMP , DESIGNATION_NAME,COMP_NAME FROM EMPLOYEE e ,COMPANY c , EMPLOYEE_DESIGNATION d " +
                                   "where e.COMPANY_ID = c.COMPANY_ID and  e.DESIGNATION_ID = d.DESIGNATION_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "group by e.COMPANY_ID,d.DESIGNATION_NAME";
                }
                else
                {
                    sMySqlString = "Select count(distinct EMPLOYEE_ID)  as TOTEMP , DESIGNATION_NAME,COMP_NAME FROM EMPLOYEE e ,COMPANY c , EMPLOYEE_DESIGNATION d " +
                                   "where e.COMPANY_ID = c.COMPANY_ID and  e.DESIGNATION_ID = d.DESIGNATION_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' " +
                                   "group by e.COMPANY_ID,d.DESIGNATION_NAME";
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

        public DataTable populaterep0010_StaffStrength(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALSS ,COMP_NAME " +
                                   "FROM  EMPLOYEE_ROLE er, EMPLOYEE e ,  COMPANY c " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID and e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "group by er.ROLE_NAME";
                }
                else
                {
                    sMySqlString = "select er.ROLE_NAME as ROLENAME, count(e.EMPLOYEE_ID) AS TOTALSS ,COMP_NAME " +
                                   "FROM  EMPLOYEE_ROLE er, EMPLOYEE e ,  COMPANY c " +
                                   "WHERE e.COMPANY_ID = c.COMPANY_ID and e.ROLE_ID = er.ROLE_ID " +
                                   "AND e.DOJ >= '" + fromdate + "' AND e.DOJ <= '" + todate + "' AND e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' " +
                                   "AND e.COMPANY_ID = '" + sCOMPANY_ID + "' group by er.ROLE_NAME";
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

        public DataTable populaterep0010_StaffTenure(string sCOMPANY_ID, string fromdate, string todate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = "SELECT  COMP_NAME, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) >= 0 ) and (Year('" + todate + "') - Year(e.doj) <= 5  )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot05, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 5  ) and (Year('" + todate + "') - Year(e.doj) <= 10 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot10, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 10 ) and (Year('" + todate + "') - Year(e.doj) <= 15 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot15, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 15 ) and (Year('" + todate + "') - Year(e.doj) <= 20 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot20, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 20 ) and (Year('" + todate + "') - Year(e.doj) <= 25 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot25, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 25 ) and (Year('" + todate + "') - Year(e.doj) <= 30 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot30, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 30 ) and (Year('" + todate + "') - Year(e.doj) <= 35 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot35, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 35 ) and (Year('" + todate + "') - Year(e.doj) <= 40 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot40, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 40 ) and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot45 " +
                                   "FROM EMPLOYEE e , COMPANY c where e.COMPANY_ID = c.COMPANY_ID group by e.COMPANY_ID ";
                }
                else
                {
                    sMySqlString = "SELECT  COMP_NAME, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) >= 0 ) and (Year('" + todate + "') - Year(e.doj) <= 5  )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot05, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 5  ) and (Year('" + todate + "') - Year(e.doj) <= 10 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot10, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 10 ) and (Year('" + todate + "') - Year(e.doj) <= 15 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot15, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 15 ) and (Year('" + todate + "') - Year(e.doj) <= 20 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot20, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 20 ) and (Year('" + todate + "') - Year(e.doj) <= 25 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot25, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 25 ) and (Year('" + todate + "') - Year(e.doj) <= 30 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot30, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 30 ) and (Year('" + todate + "') - Year(e.doj) <= 35 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot35, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 35 ) and (Year('" + todate + "') - Year(e.doj) <= 40 )  and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot40, " +
                                   "(select count(EMPLOYEE_ID)  From EMPLOYEE e where e.COMPANY_ID = c.COMPANY_ID and (Year('" + todate + "') - Year(e.doj) > 40 ) and e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' ) as  tot45 " +
                                   "FROM EMPLOYEE e , COMPANY c where e.COMPANY_ID = c.COMPANY_ID AND e.COMPANY_ID = '" + sCOMPANY_ID + "' group by e.COMPANY_ID ";
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

        //REPORT 0010 NEW METHODS
        public DataTable populaterep0010EmploymanetTypeAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, ET.TYPE_NAME, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_TYPE ET,
                                        COMPANY C
                                    WHERE
                                        ET.EMP_TYPE_ID = E.EMP_TYPE_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID
                                            AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                    GROUP BY C.COMP_NAME , ET.TYPE_NAME
                                    ORDER BY C.COMP_NAME ASC , ET.TYPE_NAME ASC
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

        public DataTable populaterep0010GenderAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, E.GENDER, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        COMPANY C
                                    WHERE
                                        E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                    GROUP BY C.COMP_NAME , E.GENDER
                                    ORDER BY C.COMP_NAME ASC , E.GENDER DESC
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

        public DataTable populaterep0010DesignationAnalysisAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME,
                                        ED.DESIGNATION_NAME,
                                        COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_DESIGNATION ED,
                                        COMPANY C
                                    WHERE
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                    GROUP BY C.COMP_NAME , ED.DESIGNATION_NAME
                                    ORDER BY C.COMP_NAME ASC , ED.DESIGNATION_NAME ASC
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

        public DataTable populaterep0010StaffCategoryAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, ER.ROLE_NAME, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_ROLE ER,
                                        COMPANY C
                                    WHERE
                                        E.ROLE_ID = ER.ROLE_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'
                                    GROUP BY C.COMP_NAME , ER.ROLE_NAME
                                    ORDER BY C.COMP_NAME ASC , ER.ROLE_NAME ASC
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

        public DataTable populaterep0010StaffTenureAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT C.COMP_NAME,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) >= 0 ) and (Year(NOW()) - YEAR(E.DOJ) <= 5  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot05,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 5 ) and (Year(NOW()) - YEAR(E.DOJ) <= 10  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot10,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 10 ) and (Year(NOW()) - YEAR(E.DOJ) <= 15  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot15,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 20 ) and (Year(NOW()) - YEAR(E.DOJ) <= 25  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot20,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 25 ) and (Year(NOW()) - YEAR(E.DOJ) <= 30  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot25,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 30 ) and (Year(NOW()) - YEAR(E.DOJ) <= 35  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot30,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 35 ) and (Year(NOW()) - YEAR(E.DOJ) <= 40  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot35,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 40 ) and (Year(NOW()) - YEAR(E.DOJ) <= 45  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot40,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 45 ) and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot45
                                    FROM EMPLOYEE E , COMPANY C 
                                    WHERE E.COMPANY_ID = C.COMPANY_ID 
                                    GROUP BY E.COMPANY_ID
                                    ORDER BY C.COMP_NAME ASC
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



        public DataTable populaterep0010EmploymanetTypeINDCompany(string compID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, ET.TYPE_NAME, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_TYPE ET,
                                        COMPANY C
                                    WHERE
                                        ET.EMP_TYPE_ID = E.EMP_TYPE_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID
                                            AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND E.COMPANY_ID = '" + compID + @"' 
                                    GROUP BY C.COMP_NAME , ET.TYPE_NAME
                                    ORDER BY C.COMP_NAME ASC , ET.TYPE_NAME ASC
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

        public DataTable populaterep0010GenderINDCompany(string compID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, E.GENDER, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        COMPANY C
                                    WHERE
                                        E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND E.COMPANY_ID = '" + compID + @"'
                                    GROUP BY C.COMP_NAME , E.GENDER
                                    ORDER BY C.COMP_NAME ASC , E.GENDER DESC
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

        public DataTable populaterep0010DesignationAnalysisINDCompany(string compID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME,
                                        ED.DESIGNATION_NAME,
                                        COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_DESIGNATION ED,
                                        COMPANY C
                                    WHERE
                                        E.DESIGNATION_ID = ED.DESIGNATION_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND E.COMPANY_ID = '" + compID + @"'
                                    GROUP BY C.COMP_NAME , ED.DESIGNATION_NAME
                                    ORDER BY C.COMP_NAME ASC , ED.DESIGNATION_NAME ASC
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

        public DataTable populaterep0010StaffCategoryINDCompany(string compID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        C.COMP_NAME, ER.ROLE_NAME, COUNT(E.EMPLOYEE_ID) AS 'TOTAL', 
((COUNT(E.EMPLOYEE_ID)/(SELECT COUNT(EMPLOYEE_ID) FROM EMPLOYEE  WHERE COMPANY_ID = E.COMPANY_ID AND EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"'))*100) AS 'PR'
                                    FROM
                                        EMPLOYEE E,
                                        EMPLOYEE_ROLE ER,
                                        COMPANY C
                                    WHERE
                                        E.ROLE_ID = ER.ROLE_ID
                                            AND E.COMPANY_ID = C.COMPANY_ID AND E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' AND E.COMPANY_ID = '" + compID + @"'
                                    GROUP BY C.COMP_NAME , ER.ROLE_NAME
                                    ORDER BY C.COMP_NAME ASC , ER.ROLE_NAME ASC
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

        public DataTable populaterep0010StaffTenureINDCompany(string compID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT C.COMP_NAME,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) >= 0 ) and (Year(NOW()) - YEAR(E.DOJ) <= 5  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot05,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 5 ) and (Year(NOW()) - YEAR(E.DOJ) <= 10  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot10,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 10 ) and (Year(NOW()) - YEAR(E.DOJ) <= 15  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot15,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 20 ) and (Year(NOW()) - YEAR(E.DOJ) <= 25  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot20,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 25 ) and (Year(NOW()) - YEAR(E.DOJ) <= 30  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot25,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 30 ) and (Year(NOW()) - YEAR(E.DOJ) <= 35  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot30,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 35 ) and (Year(NOW()) - YEAR(E.DOJ) <= 40  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot35,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 40 ) and (Year(NOW()) - YEAR(E.DOJ) <= 45  )  and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot40,
                                    (SELECT COUNT(E.EMPLOYEE_ID)  FROM EMPLOYEE E WHERE E.COMPANY_ID = C.COMPANY_ID and (YEAR(NOW()) - YEAR(E.DOJ) > 45 ) and E.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + @"' ) as  tot45
                                    FROM EMPLOYEE E , COMPANY C 
                                    WHERE E.COMPANY_ID = C.COMPANY_ID  AND E.COMPANY_ID = '" + compID + @"'
                                    GROUP BY E.COMPANY_ID
                                    ORDER BY C.COMP_NAME ASC
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

        //


        //-- 	Absent Report report
        public DataTable populaterep0012_UOAbsent(string sCOMPANY_ID, string fromdate, string todate, string empCode, string depCode)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                //sMySqlString = " SELECT Att.EMPLOYEE_ID,e.INITIALS_NAME,d.DIV_NAME, CAST(Att.IN_DATE as char) IN_DATE,CAST(Att.OUT_DATE as char) OUT_DATE,BRANCH_NAME,DEPT_NAME, DESIGNATION_NAME  " +
                //                " FROM ATTENDANCE_SUMMARY as Att,EMPLOYEE as e,DIVISION d ,COMPANY_BRANCH b , DEPARTMENT dep , EMPLOYEE_DESIGNATION ed " +
                //                " where  " +
                //                " Att.EMPLOYEE_ID = e.EMPLOYEE_ID and " +
                //                " e.DIVISION_ID = d.DIVISION_ID and " +
                //                " e.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' and" +
                //                " e.COMPANY_ID = '" + sCOMPANY_ID + "' and " +
                //                " DATE(Att.IN_DATE) >= DATE('" + fromdate + "') and DATE(Att.IN_DATE) <= DATE('" + todate + "') and " +
                //                " Att.NUMBER_OF_DAYS is null and " +
                //                " Att.IS_ABSENT='1' " +
                //                " and b.BRANCH_ID = e.BRANCH_ID " +
                //                " and dep.DEPT_ID = e.DEPT_ID " +
                //                " and ed.DESIGNATION_ID = e.DESIGNATION_ID " +
                //                " Order by d.DIV_NAME,Att.EMPLOYEE_ID";

                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (empCode == "")
                    {
                        sMySqlString = "sp_AbsentReportAllCompany";
                    }
                    else
                    {
                        sMySqlString = "sp_AbsentReportForIndividual";
                    }
                }
                else
                {
                    if (empCode == "")
                    {
                        if (depCode == "")
                        {
                            sMySqlString = "sp_AbsentReport";
                        }
                        else
                        {
                            sMySqlString = "sp_AbsentReportByDepartment";
                        }
                    }
                    else
                    {
                        sMySqlString = "sp_AbsentReportForIndividual";
                    }
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("dept", depCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("empCode", empCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("company", sCOMPANY_ID));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", fromdate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", todate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Employee Extra Hours Worked report
        public DataTable populaterep0013_OT(string sCOMPANY_ID, string fromDate, string toDate, string empCode, string deptCode)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (empCode == "")
                    {
                        if (deptCode == "")
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND
                                            d.DEPT_ID = e.DEPT_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }
                        else
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND e.DEPT_ID = '" + deptCode + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }
                    }
                    else
                    {
                        sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND s.EMPLOYEE_ID = '" + empCode + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                    }
                }
                else
                {
                    if (deptCode == "")
                    {
                        if (empCode == "")
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND c.COMPANY_ID = '" + sCOMPANY_ID + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }
                        else
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND s.EMPLOYEE_ID = '" + empCode + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }

                    }
                    else
                    {
                        string prefix = deptCode[0].ToString() + deptCode[1].ToString();

                        if (prefix == Constants.DEPARTMENT_ID_STAMP)
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND e.DEPT_ID = '" + deptCode + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }
                        else
                        {
                            sMySqlString = @"SELECT
                                                c.COMP_NAME,d.DEPT_NAME,e.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT( s.IN_DATE , CHAR) AS IN_DATE,
                                            s.IN_TIME,CONVERT( s.OUT_DATE , CHAR) AS OUT_DATE,s.OUT_TIME,s.LATE_MINUTES,s.EARLY_MINUTES,CAST(s.OT_HOURS as DECIMAL(9,2)) AS EXTRA_HOURS
                                        FROM
                                            ATTENDANCE_SUMMARY as s,
                                            EMPLOYEE as e , COMPANY c ,DEPARTMENT d
                                        where s.EMPLOYEE_ID = e.EMPLOYEE_ID AND
                                                c.COMPANY_ID = e.COMPANY_ID AND e.EMPLOYEE_STATUS = 'S001' AND
                                            d.DEPT_ID = e.DEPT_ID AND e.BRANCH_ID = '" + deptCode + @"' AND
                                            (DATE(s.IN_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";
                        }
                    }
                }

                mySqlCon.ConnectionString = @"Server=10.100.101.38;User Id=hrisadmin;Password=hrisadmin;Database=HRIS;Allow User Variables=True";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Property Available in each company
        public DataTable populaterep0014_Property(string sCOMPANY_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,
			                            case
				                            when p.STATUS_CODE = '1' then 'Avilable'
				                            when p.STATUS_CODE = '2' then 'Assigned'
				                            when p.STATUS_CODE = '3' then 'Disposed'
			                            End as STATUS_CODE 
	                            FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c
	                            WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
                                        c.COMPANY_ID = p.COMPANY_ID;";
                }
                else
                {
                    sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,
			                            case
				                            when p.STATUS_CODE = '1' then 'Avilable'
				                            when p.STATUS_CODE = '2' then 'Assigned'
				                            when p.STATUS_CODE = '3' then 'Disposed'
			                            End as STATUS_CODE 
	                            FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c
	                            WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
                                        c.COMPANY_ID = p.COMPANY_ID AND 
                                        p.COMPANY_ID = '" + sCOMPANY_ID + "';";
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

        //-- Vacant Property list
        public DataTable populaterep0015_Property(string sCOMPANY_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO
	                                FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c
	                                WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
                                            c.COMPANY_ID = p.COMPANY_ID AND 
                                            p.STATUS_CODE = '1';";
                }
                else
                {
                    sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO
	                                FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c
	                                WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
                                            c.COMPANY_ID = p.COMPANY_ID AND
											c.COMP_NAME = '" + sCOMPANY_ID + @"' AND
                                            p.STATUS_CODE = '1';";
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

        //Property Assign per individuals
        public DataTable populaterep0016_Property(string sCOMPANY_ID, string sEMPLOYEE_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (sEMPLOYEE_ID == "")
                    {
                        //All Company
                        sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,CONVERT(ep.ASSIGNED_DATE,CHAR) AS ASSIGNED_DATE,CONVERT(ep.RETURNED_DATE,CHAR) AS RETURNED_DATE
	                                    FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c,EMPLOYEE_PROPERTY_DETAILS ep
	                                    WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
			                                    c.COMPANY_ID = p.COMPANY_ID AND 
			                                    p.PROPERTY_ID = ep.PROPERTY_ID;";
                    }
                    else
                    {
                        //per individual
                        sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,CONVERT(ep.ASSIGNED_DATE,CHAR) AS ASSIGNED_DATE,CONVERT(ep.RETURNED_DATE,CHAR) AS RETURNED_DATE
	                                    FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c,EMPLOYEE_PROPERTY_DETAILS ep
	                                    WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
			                                    c.COMPANY_ID = p.COMPANY_ID AND 
			                                    p.PROPERTY_ID = ep.PROPERTY_ID AND 
			                                    ep.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "';";
                    }
                }
                else
                {
                    //Company
                    sMySqlString = @"SELECT c.COMP_NAME,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,CONVERT(ep.ASSIGNED_DATE,CHAR) AS ASSIGNED_DATE,CONVERT(ep.RETURNED_DATE,CHAR) AS RETURNED_DATE
	                                FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c,EMPLOYEE_PROPERTY_DETAILS ep
	                                WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
			                                c.COMPANY_ID = p.COMPANY_ID AND 
			                                p.PROPERTY_ID = ep.PROPERTY_ID AND 
			                                c.COMPANY_ID = '" + sCOMPANY_ID + "';";
                } // Employee name, department, division

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee Resignation for individual company
        public DataTable populaterep0017_EmpResignation(string sCOMPANY_ID, string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,e.EPF_NO,e.INITIALS_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(CAST(e.RESIGNED_DATE AS DATE),CHAR) AS RESIGNED_DATE,es.DESCRIPTION 
                                    FROM EMPLOYEE e , COMPANY c , DEPARTMENT d , EMPLOYEE_DESIGNATION ed, EMPLOYEE_STATUS es ,COMPANY_BRANCH b
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND es.STATUS_CODE = e.EMPLOYEE_STATUS AND  
		                                    d.DEPT_ID = e.DEPT_ID 
                                            AND b.BRANCH_ID = e.BRANCH_ID AND
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    e.COMPANY_ID = '" + sCOMPANY_ID + @"' AND 
                                            (DATE(e.RESIGNED_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populaterep0017_EmpResignationByDepartment(string department, string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,e.EPF_NO,e.INITIALS_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(CAST(e.RESIGNED_DATE AS DATE),CHAR) AS RESIGNED_DATE,es.DESCRIPTION 
                                    FROM EMPLOYEE e , COMPANY c , DEPARTMENT d , EMPLOYEE_DESIGNATION ed, EMPLOYEE_STATUS es ,COMPANY_BRANCH b
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND es.STATUS_CODE = e.EMPLOYEE_STATUS AND  
		                                    d.DEPT_ID = e.DEPT_ID  
                                            AND b.BRANCH_ID = e.BRANCH_ID AND
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    e.DEPT_ID = '" + department + @"' AND 
                                            (DATE(e.RESIGNED_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee Resignation for individual Employee
        public DataTable populaterep0017_EmpResignationByEmployee(string empCode, string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,e.EPF_NO,e.INITIALS_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(CAST(e.RESIGNED_DATE AS DATE),CHAR) AS RESIGNED_DATE,es.DESCRIPTION 
                                    FROM EMPLOYEE e , COMPANY c , DEPARTMENT d , EMPLOYEE_DESIGNATION ed, EMPLOYEE_STATUS es ,COMPANY_BRANCH b
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND es.STATUS_CODE = e.EMPLOYEE_STATUS AND  
		                                    d.DEPT_ID = e.DEPT_ID AND 
                                            b.BRANCH_ID = e.BRANCH_ID AND
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    e.EMPLOYEE_ID = '" + empCode + "';"; // +@"' AND 
                //(DATE(e.RESIGNED_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "');";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee Resignation for all company
        public DataTable populaterep0017_EmpResignationGroupByCompany(string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,e.EPF_NO,e.INITIALS_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(CAST(e.RESIGNED_DATE AS DATE),CHAR) AS RESIGNED_DATE,es.DESCRIPTION
                                    FROM EMPLOYEE e , COMPANY c , DEPARTMENT d , EMPLOYEE_DESIGNATION ed, EMPLOYEE_STATUS es ,COMPANY_BRANCH b
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND es.STATUS_CODE = e.EMPLOYEE_STATUS AND  
		                                    d.DEPT_ID = e.DEPT_ID AND 
                                            b.BRANCH_ID = e.BRANCH_ID AND
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND
                                            (DATE(e.RESIGNED_DATE) BETWEEN '" + fromDate + "' AND '" + toDate + "') GROUP BY e.COMPANY_ID,e.EPF_NO,e.EMPLOYEE_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee new recrutements in company
        public DataTable populaterep0018_NewRecrutments(string sCOMPANY_ID, string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {//
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME, (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = e.BRANCH_ID) AS BRANCH_NAME,e.EPF_NO,e.FULL_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,er.ROLE_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(e.DOB,CHAR) AS DOB,e.EMP_NIC,e.PERMANENT_ADDRESS,e.CURRENT_ADDRESS,e.INITIALS_NAME,e.COST_CENTER,e.PROFIT_CENTER,t.TYPE_NAME 
                                    FROM EMPLOYEE e , COMPANY c ,COMPANY_BRANCH b,DEPARTMENT d,EMPLOYEE_DESIGNATION ed,EMPLOYEE_ROLE er,EMPLOYEE_TYPE t
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND  
                                            d.COMPANY_ID = e.COMPANY_ID AND 
		                                    d.DEPT_ID = e.DEPT_ID AND 
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    er.ROLE_ID = e.ROLE_ID AND 
                                            e.COMPANY_ID = '" + sCOMPANY_ID + @"'AND
		                                    (DATE(e.DOJ) BETWEEN '" + fromDate + "' AND '" + toDate + "') AND e.EMP_TYPE_ID = t.EMP_TYPE_ID GROUP BY e.COMPANY_ID,e.EPF_NO,e.EMPLOYEE_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee new recrutements for all company
        public DataTable populaterep0018_NewRecrutmentsGroupByCompany(string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME, (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = e.BRANCH_ID) AS BRANCH_NAME,e.EPF_NO,e.FULL_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,er.ROLE_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(e.DOB,CHAR) As DOB,e.EMP_NIC,e.PERMANENT_ADDRESS,e.CURRENT_ADDRESS,e.INITIALS_NAME,e.COST_CENTER,e.PROFIT_CENTER,t.TYPE_NAME
                                    FROM EMPLOYEE e , COMPANY c ,COMPANY_BRANCH b,DEPARTMENT d,EMPLOYEE_DESIGNATION ed,EMPLOYEE_ROLE er,EMPLOYEE_TYPE t
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND d.COMPANY_ID = e.COMPANY_ID AND
		                                    d.DEPT_ID = e.DEPT_ID AND 
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    er.ROLE_ID = e.ROLE_ID AND
		                                    (DATE(e.DOJ) BETWEEN '" + fromDate + "' AND '" + toDate + "') AND e.EMP_TYPE_ID = t.EMP_TYPE_ID GROUP BY e.COMPANY_ID,e.EPF_NO,e.EMPLOYEE_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee new recrutements for an individual
        public DataTable populaterep0018_NewRecrutmentsByEmployee(string department, string fromDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT c.COMP_NAME, (SELECT BRANCH_NAME FROM COMPANY_BRANCH WHERE BRANCH_ID = e.BRANCH_ID) AS BRANCH_NAME,e.EPF_NO,e.FULL_NAME,d.DEPT_NAME,ed.DESIGNATION_NAME,er.ROLE_NAME,CONVERT(e.DOJ,CHAR) AS DOJ,CONVERT(e.DOB,CHAR) As DOB,e.EMP_NIC,e.PERMANENT_ADDRESS,e.CURRENT_ADDRESS,e.INITIALS_NAME,e.COST_CENTER,e.PROFIT_CENTER,t.TYPE_NAME
                                    FROM EMPLOYEE e , COMPANY c ,COMPANY_BRANCH b,DEPARTMENT d,EMPLOYEE_DESIGNATION ed,EMPLOYEE_ROLE er,EMPLOYEE_TYPE t
                                    WHERE e.COMPANY_ID = c.COMPANY_ID AND d.COMPANY_ID = e.COMPANY_ID AND
		                                    d.DEPT_ID = e.DEPT_ID AND  
		                                    ed.DESIGNATION_ID = e.DESIGNATION_ID AND 
		                                    er.ROLE_ID = e.ROLE_ID AND
                                            e.DEPT_ID = '" + department + @"'AND
		                                    (DATE(e.DOJ) BETWEEN '" + fromDate + "' AND '" + toDate + "') AND e.EMP_TYPE_ID = t.EMP_TYPE_ID GROUP BY e.COMPANY_ID,e.EPF_NO,e.EMPLOYEE_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Letter Of Employeement
        public DataTable populaterep0020_LetterOfEmployeement(string sEMPLOYEE_ID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT s.SALARY_ID,e.KNOWN_NAME, e.EMP_NIC,(SELECT em.KNOWN_NAME FROM EMPLOYEE em WHERE em.EMPLOYEE_ID = e.REPORT_TO_1) AS REPORT_TO_1,
		                                SUBSTRING_INDEX(SUBSTRING_INDEX(e.PERMANENT_ADDRESS, ',', 1), ',', -1) as No ,
		                                SUBSTRING_INDEX(SUBSTRING_INDEX(e.PERMANENT_ADDRESS, ',', 2), ',', -1) as Road ,
		                                SUBSTRING_INDEX(SUBSTRING_INDEX(e.PERMANENT_ADDRESS, ',', 3), ',', -1) as Town ,
		                                ed.DESIGNATION_NAME ,s.BASIC_AMOUNT,s.OTHER_AMOUNT,s.BUDGETARY_ALLOWANCE_AMOUNT,s.OTHER_AMOUNT,CONVERT(s.EFFECT_FROM , CHAR) AS EFFECT_FROM,sd.AMOUNT
                                FROM EMPLOYEE e , EMPLOYEE_DESIGNATION ed ,SALARY s,SALARY_DETAIL sd
                                WHERE e.DESIGNATION_ID = ed.DESIGNATION_ID AND 
		                                s.SALARY_ID = sd.SALARY_ID AND 
                                        s.STATUS_CODE = '1' AND
										sd.STATUS_CODE = '1' AND
		                                e.COMPANY_ID = ed.COMPANY_ID AND
		                                e.EMPLOYEE_ID = s.EMPLOYEE_ID AND 
		                                e.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Monthly roster for company
        public DataTable populaterep0021_MonthlyRoster(string eCOMPANY_ID, string depId, string empId, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            DataTable dateTable = new DataTable();
            DataTable rosterTable = new DataTable();
            try
            {
                dataTable.Rows.Clear();

                if (depId == "")
                {
                    if (empId == "")
                    {
                        sMySqlString = @"SELECT e.INITIALS_NAME,DATE_FORMAT(CONVERT(r.DUTY_DATE,CHAR),'%a,%d') AS DUTY_DATE ,CONVERT(r.DUTY_DATE,CHAR) AS DUTY_DATE2,
		                                CONCAT(s.FROM_TIME ,' - ', s.TO_TIME) as roster_time,d.DEPT_NAME
	                              FROM EMPLOYEE_ROSTER_DATE r,EMPLOYEE e,ROSTER s ,DEPARTMENT d
	                              WHERE e.EMPLOYEE_ID = r.EMPLOYEE_ID AND (r.DUTY_DATE BETWEEN '" + frmDate + @"' AND '" + toDate + @"') AND
			                              r.ROSTR_ID = s.ROSTR_ID AND  
                                          e.DEPT_ID = d.DEPT_ID AND
			                              e.COMPANY_ID = '" + eCOMPANY_ID + "' GROUP BY e.EMPLOYEE_ID,r.DUTY_DATE,r.ROSTR_ID;"; // ORDER BY r.DUTY_DATE
                    }
                    else
                    {
                        //for individual employee
                        sMySqlString = @"SELECT 
                                            e.INITIALS_NAME,
                                            DATE_FORMAT(CONVERT( r.DUTY_DATE , CHAR), '%a,%d') AS DUTY_DATE,
                                            CONVERT( r.DUTY_DATE , CHAR) AS DUTY_DATE2,
                                            CONCAT(s.FROM_TIME, ' - ', s.TO_TIME) as roster_time,
                                            d.DEPT_NAME
                                        FROM 
                                            EMPLOYEE_ROSTER_DATE r,EMPLOYEE e,ROSTER s,DEPARTMENT d
                                        WHERE
                                            e.EMPLOYEE_ID = r.EMPLOYEE_ID
                                                AND (r.DUTY_DATE BETWEEN '" + frmDate + "' AND '" + toDate + @"')
                                                AND r.ROSTR_ID = s.ROSTR_ID
                                                AND e.DEPT_ID = d.DEPT_ID
		                                        AND e.EMPLOYEE_ID = '" + empId + @"'
                                        GROUP BY e.EMPLOYEE_ID , r.DUTY_DATE , r.ROSTR_ID;";
                    }
                }
                else
                {
                    //for department
                    sMySqlString = @"SELECT 
                                            e.INITIALS_NAME,
                                            DATE_FORMAT(CONVERT( r.DUTY_DATE , CHAR), '%a,%d') AS DUTY_DATE,
                                            CONVERT( r.DUTY_DATE , CHAR) AS DUTY_DATE2,
                                            CONCAT(s.FROM_TIME, ' - ', s.TO_TIME) as roster_time,
                                            d.DEPT_NAME
                                        FROM
                                            EMPLOYEE_ROSTER_DATE r,EMPLOYEE e,ROSTER s,DEPARTMENT d
                                        WHERE
                                            e.EMPLOYEE_ID = r.EMPLOYEE_ID
                                                AND (r.DUTY_DATE BETWEEN '" + frmDate + "' AND '" + toDate + @"')
                                                AND r.ROSTR_ID = s.ROSTR_ID
                                                AND e.DEPT_ID = d.DEPT_ID
                                                AND e.COMPANY_ID = '" + eCOMPANY_ID + @"'
		                                        AND e.DEPT_ID = '" + depId + @"'
                                        GROUP BY e.EMPLOYEE_ID , r.DUTY_DATE , r.ROSTR_ID;";
                }


                MySqlDataAdapter mySqlRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlRoster.Fill(rosterTable);
                return rosterTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Missing Employeement Report for All company
        public DataTable populaterep0022_MissingEmployee(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
                                    CONVERT(a.IN_DATE,CHAR) AS IN_DATE,e.EPF_NO,a.EMPLOYEE_ID,e.INITIALS_NAME,c.COMP_NAME,d.DEPT_NAME,a.IS_ABSENT,a.IN_TIME,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME
                                FROM
                                    ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
                                    a.DEPT_ID = d.DEPT_ID AND
                                    a.IS_ABSENT = '2' AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"' 
                                    AND b.BRANCH_ID = e.BRANCH_ID
		                            AND e.DESIGNATION_ID = ed.DESIGNATION_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Missing Employeement Report for company
        public DataTable populaterep0022_MissingEmployeeForCompany(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                sMySqlString = @"SELECT 
                                    CONVERT(a.IN_DATE,CHAR) AS IN_DATE,e.EPF_NO,a.EMPLOYEE_ID,e.INITIALS_NAME,c.COMP_NAME,d.DEPT_NAME,a.IS_ABSENT, a.IN_TIME,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME
                                FROM
                                    ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
                                    a.DEPT_ID = d.DEPT_ID AND a.COMPANY_ID = '" + eCOMPANY_ID + @"' AND
                                    a.IS_ABSENT = '2' AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"' 
                                    AND b.BRANCH_ID = e.BRANCH_ID
		                            AND e.DESIGNATION_ID = ed.DESIGNATION_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Missing Employeement Report for department
        public DataTable populaterep0022_MissingEmployeeForDepartment(string deptId, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                sMySqlString = @"SELECT 
                                    CONVERT(a.IN_DATE,CHAR) AS IN_DATE,e.EPF_NO,a.EMPLOYEE_ID,e.INITIALS_NAME,c.COMP_NAME,d.DEPT_NAME,a.IS_ABSENT, a.IN_TIME,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME
                                FROM
                                    ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
                                    a.DEPT_ID = d.DEPT_ID AND a.DEPT_ID = '" + deptId + @"' AND
                                    a.IS_ABSENT = '2' AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"' 
                                    AND b.BRANCH_ID = e.BRANCH_ID
		                            AND e.DESIGNATION_ID = ed.DESIGNATION_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //-- Missing Employeement Report for Branch
        public DataTable populaterep0022_MissingEmployeeForBranch(string BranchId, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                sMySqlString = @"SELECT 
                                    CONVERT( a.IN_DATE , CHAR) AS IN_DATE,e.EPF_NO,a.EMPLOYEE_ID,e.INITIALS_NAME,
                                    c.COMP_NAME,d.DEPT_NAME,a.IS_ABSENT,a.IN_TIME,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME
                                FROM 
                                    ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE
                                    a.EMPLOYEE_ID = e.EMPLOYEE_ID
                                        AND a.COMPANY_ID = c.COMPANY_ID
                                        AND a.DEPT_ID = d.DEPT_ID
                                        AND b.BRANCH_ID = '" + BranchId + @"'
                                        AND a.IS_ABSENT = '2'
                                        AND a.IN_DATE >= '" + frmDate + @"'
                                        AND a.IN_DATE <= '" + toDate + @"'
                                        AND b.BRANCH_ID = e.BRANCH_ID
                                        AND e.DESIGNATION_ID = ed.DESIGNATION_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Missing in/out for Employee
        public DataTable populaterep0022_MissingEmployeeForEmployee(string frmDate, string toDate, string empName)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();

                sMySqlString = @"SELECT 
                                    CONVERT(a.IN_DATE,CHAR) AS IN_DATE,e.EPF_NO,a.EMPLOYEE_ID,e.INITIALS_NAME,c.COMP_NAME,d.DEPT_NAME,a.IS_ABSENT, a.IN_TIME,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME
                                FROM
                                    ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
                                    e.EMPLOYEE_ID = '" + empName + @"' AND
                                    a.DEPT_ID = d.DEPT_ID AND
                                    a.IS_ABSENT = '2' AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"' 
                                    AND b.BRANCH_ID = e.BRANCH_ID
		                            AND e.DESIGNATION_ID = ed.DESIGNATION_ID;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-- Early Off Report
        public DataTable populaterep0023_EarlyOff(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
	                                CONVERT(a.IN_DATE,CHAR) AS IN_DATE,a.EARLY_MINUTES,e.EPF_NO,e.KNOWN_NAME,c.COMP_NAME,d.DEPT_NAME,a.IN_TIME,CONVERT(a.OUT_DATE,CHAR) AS OUT_DATE,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME,e.INITIALS_NAME
                                FROM ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c ,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE 
	                                a.EARLY_MINUTES IS NOT NULL AND
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
	                                a.DEPT_ID = d.DEPT_ID AND
	                                a.COMPANY_ID = d.COMPANY_ID AND
	                                a.EARLY_MINUTES != 0 AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"'AND c.COMPANY_ID = b.COMPANY_ID
		                            AND c.COMPANY_ID = e.COMPANY_ID AND b.BRANCH_ID = e.BRANCH_ID AND e.DESIGNATION_ID = ed.DESIGNATION_ID";

                MySqlDataAdapter mysqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mysqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Early Off Report By Company Id
        public DataTable populaterep0023_EarlyOffByCompany(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();


            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
	                                CONVERT(a.IN_DATE,CHAR) AS IN_DATE,a.EARLY_MINUTES,e.EPF_NO,e.KNOWN_NAME,c.COMP_NAME,d.DEPT_NAME ,a.IN_TIME,CONVERT(a.OUT_DATE,CHAR) AS OUT_DATE,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME,e.INITIALS_NAME
                                FROM ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c ,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE 
	                                a.EARLY_MINUTES IS NOT NULL AND
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
	                                a.DEPT_ID = d.DEPT_ID AND
	                                a.COMPANY_ID = d.COMPANY_ID AND 
                                    a.COMPANY_ID = '" + eCOMPANY_ID + @"' AND
	                                a.EARLY_MINUTES != 0 AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"'AND c.COMPANY_ID = b.COMPANY_ID
		                            AND c.COMPANY_ID = e.COMPANY_ID AND b.BRANCH_ID = e.BRANCH_ID AND e.DESIGNATION_ID = ed.DESIGNATION_ID";

                MySqlDataAdapter mysqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mysqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Early Off Report By Department Id
        public DataTable populaterep0023_EarlyOffByDepartment(string depId, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
	                                CONVERT(a.IN_DATE,CHAR) AS IN_DATE,a.EARLY_MINUTES,e.EPF_NO,e.KNOWN_NAME,c.COMP_NAME,d.DEPT_NAME ,a.IN_TIME,CONVERT(a.OUT_DATE,CHAR) AS OUT_DATE,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME,e.INITIALS_NAME
                                FROM ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c ,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE 
	                                a.EARLY_MINUTES IS NOT NULL AND
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
	                                a.DEPT_ID = d.DEPT_ID AND
	                                a.COMPANY_ID = d.COMPANY_ID AND 
                                    a.DEPT_ID = '" + depId + @"' AND
	                                a.EARLY_MINUTES != 0 AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"'AND c.COMPANY_ID = b.COMPANY_ID
		                            AND c.COMPANY_ID = e.COMPANY_ID AND b.BRANCH_ID = e.BRANCH_ID AND e.DESIGNATION_ID = ed.DESIGNATION_ID";

                MySqlDataAdapter mysqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mysqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Early Off Report By Employee
        public DataTable populaterep0023_EarlyOffByEmployee(string empCode, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
	                                CONVERT(a.IN_DATE,CHAR) AS IN_DATE,a.EARLY_MINUTES,e.EPF_NO,e.KNOWN_NAME,c.COMP_NAME,d.DEPT_NAME ,a.IN_TIME,CONVERT(a.OUT_DATE,CHAR) AS OUT_DATE,a.OUT_TIME,b.BRANCH_NAME,ed.DESIGNATION_NAME,e.INITIALS_NAME
                                FROM ATTENDANCE_SUMMARY a,EMPLOYEE e,COMPANY c ,DEPARTMENT d,COMPANY_BRANCH b,EMPLOYEE_DESIGNATION ed
                                WHERE 
	                                a.EARLY_MINUTES IS NOT NULL AND
	                                a.EMPLOYEE_ID = e.EMPLOYEE_ID AND
	                                a.COMPANY_ID = c.COMPANY_ID AND
	                                a.DEPT_ID = d.DEPT_ID AND
	                                a.COMPANY_ID = d.COMPANY_ID AND 
                                    e.EMPLOYEE_ID = '" + empCode + @"' AND
	                                a.EARLY_MINUTES != 0 AND a.IN_DATE >= '" + frmDate + "' AND a.IN_DATE <= '" + toDate + @"'AND c.COMPANY_ID = b.COMPANY_ID
		                            AND c.COMPANY_ID = e.COMPANY_ID AND b.BRANCH_ID = e.BRANCH_ID AND e.DESIGNATION_ID = ed.DESIGNATION_ID";

                MySqlDataAdapter mysqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mysqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Nopay Report for all company
        public DataTable populaterep0024_Nopay(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = "sp_nopayReport";
                mySqlCmd.Parameters.Add(new MySqlParameter("company", eCOMPANY_ID));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", frmDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", toDate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {
                throw;
            }
        }

        //-- Nopay Report for company
        public DataTable populaterep0024_NopayForCompany(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = "sp_nopayReportByCompany";
                mySqlCmd.Parameters.Add(new MySqlParameter("company", eCOMPANY_ID));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", frmDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", toDate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Nopay Report for department
        public DataTable populaterep0024_NopayForDepartment(string deptId, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = "sp_nopayReportByDepartment";
                mySqlCmd.Parameters.Add(new MySqlParameter("dept", deptId));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", frmDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", toDate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Nopay Report for employee
        public DataTable populaterep0024_NopayForEmployee(string employee, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = "sp_nopayReportByEmployee";
                mySqlCmd.Parameters.Add(new MySqlParameter("empId", employee));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", frmDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", toDate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //-- Nopay Report for Branch
        public DataTable populaterep0024_NopayForBranch(string branch, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = "sp_nopayReportByBranch";
                mySqlCmd.Parameters.Add(new MySqlParameter("branch", branch));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", frmDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", toDate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Attendance device Registration
        public DataTable populaterep0025_AttReg(string eCOMPANY_ID, string frmDate, string toDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable.Rows.Clear();
                sMySqlString = @"SELECT 
                                    e.EMPLOYEE_ID, d.DEPT_NAME, e.INITIALS_NAME, e.KNOWN_NAME, e.ATT_REG_NO
                                FROM
                                    EMPLOYEE e,DEPARTMENT d
                                WHERE 
	                                d.DEPT_ID = e.DEPT_ID AND 
                                    e.COMPANY_ID = '" + eCOMPANY_ID + "' and DOJ between '" + frmDate.Trim() + "' and '" + toDate.Trim() + "'";

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Continuous Absent Report
        public DataTable populaterep0026_ContinuousAbsent(string eCOMPANY_ID, string dept, string frmDate, string toDate, string mempcode)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                dataTable.Rows.Clear();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                string qryDelete = "TRUNCATE TEMP_CONTINUOUS_ABSENT;";
                mySqlCmd.CommandText = qryDelete;
                mySqlCmd.ExecuteNonQuery();
                mySqlCmd.Parameters.Clear();

                if (eCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,d.DEPT_NAME,dv.DIV_NAME,att.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT(att.IN_DATE,CHAR) AS IN_DATE,att.IN_TIME,att.OUT_TIME,att.REMARK
		                                        FROM
		                                        (
		                                        SELECT a.EMPLOYEE_ID,a.IN_DATE,a.COMPANY_ID,SUM(IF(a.IN_TIME IS NOT NULL, 1, 0)) AS IN_TIME,SUM(IF(a.OUT_TIME IS NOT NULL, 1, 0)) as OUT_TIME,a.REMARK
					                                        FROM
						                                        ATTENDANCE_SUMMARY a
					                                        WHERE 
						                                        a.IN_DATE >= '" + frmDate + @"' AND 
						                                        a.IN_DATE <= '" + toDate + @"' AND 
						                                        (a.NUMBER_OF_DAYS = '0.25' or a.NUMBER_OF_DAYS = '0' or a.NUMBER_OF_DAYS is null)
					                                        GROUP BY a.IN_DATE,a.EMPLOYEE_ID
					                                        ORDER BY a.EMPLOYEE_ID ASC ,a.IN_DATE
		
		                                        ) att,EMPLOYEE e,COMPANY_BRANCH b,DEPARTMENT d,DIVISION dv ,COMPANY c
		                                        WHERE att.IN_TIME = 0 AND 
			                                          att.OUT_TIME = 0 AND  
			                                          att.EMPLOYEE_ID = e.EMPLOYEE_ID AND 
			                                          att.COMPANY_ID = e.COMPANY_ID AND 
			                                          e.BRANCH_ID = b.BRANCH_ID AND 
                                                      e.COMPANY_ID = c.COMPANY_ID AND
			                                          e.DEPT_ID = d.DEPT_ID AND 
			                                          e.DIVISION_ID = dv.DIVISION_ID;";

                    }
                    else
                    {
                        sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,d.DEPT_NAME,dv.DIV_NAME,att.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT(att.IN_DATE,CHAR) AS IN_DATE,att.IN_TIME,att.OUT_TIME,att.REMARK
		                                        FROM
		                                        (
		                                        SELECT a.EMPLOYEE_ID,a.IN_DATE,a.COMPANY_ID,SUM(IF(a.IN_TIME IS NOT NULL, 1, 0)) AS IN_TIME,SUM(IF(a.OUT_TIME IS NOT NULL, 1, 0)) as OUT_TIME,a.REMARK
					                                        FROM
						                                        ATTENDANCE_SUMMARY a
					                                        WHERE
						                                        a.EMPLOYEE_ID = '" + mempcode + @"' AND 
						                                        a.IN_DATE >= '" + frmDate + @"' AND 
						                                        a.IN_DATE <= '" + toDate + @"' AND 
						                                        (a.NUMBER_OF_DAYS = '0.25' or a.NUMBER_OF_DAYS = '0' or a.NUMBER_OF_DAYS is null)
					                                        GROUP BY a.IN_DATE,a.EMPLOYEE_ID
					                                        ORDER BY a.EMPLOYEE_ID ASC ,a.IN_DATE
		
		                                        ) att,EMPLOYEE e,COMPANY_BRANCH b,DEPARTMENT d,DIVISION dv ,COMPANY c
		                                        WHERE att.IN_TIME = 0 AND 
			                                          att.OUT_TIME = 0 AND  
			                                          att.EMPLOYEE_ID = e.EMPLOYEE_ID AND 
			                                          att.COMPANY_ID = e.COMPANY_ID AND 
			                                          e.BRANCH_ID = b.BRANCH_ID AND
                                                      c.COMPANY_ID = att.COMPANY_ID AND 
			                                          e.DEPT_ID = d.DEPT_ID AND 
			                                          e.DIVISION_ID = dv.DIVISION_ID;";
                    }
                }
                else
                {
                    if (mempcode == "")
                    {

                        if (dept == "")
                        {
                            sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,d.DEPT_NAME,dv.DIV_NAME,att.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT(att.IN_DATE,CHAR) AS IN_DATE,att.IN_TIME,att.OUT_TIME,att.REMARK
		                                        FROM
		                                        (
		                                        SELECT a.EMPLOYEE_ID,a.IN_DATE,a.COMPANY_ID,SUM(IF(a.IN_TIME IS NOT NULL, 1, 0)) AS IN_TIME,SUM(IF(a.OUT_TIME IS NOT NULL, 1, 0)) as OUT_TIME,a.REMARK
					                                        FROM 
						                                        ATTENDANCE_SUMMARY a
					                                        WHERE
						                                        a.COMPANY_ID = '" + eCOMPANY_ID + @"' AND 
						                                        a.IN_DATE >= '" + frmDate + @"' AND 
						                                        a.IN_DATE <= '" + toDate + @"' AND 
						                                        (a.NUMBER_OF_DAYS = '0.25' or a.NUMBER_OF_DAYS = '0' or a.NUMBER_OF_DAYS is null)
					                                        GROUP BY a.IN_DATE,a.EMPLOYEE_ID
					                                        ORDER BY a.EMPLOYEE_ID ASC ,a.IN_DATE
		
		                                        ) att,EMPLOYEE e,COMPANY_BRANCH b,DEPARTMENT d,DIVISION dv ,COMPANY c
		                                        WHERE att.IN_TIME = 0 AND 
			                                          att.OUT_TIME = 0 AND  
			                                          att.EMPLOYEE_ID = e.EMPLOYEE_ID AND 
			                                          att.COMPANY_ID = e.COMPANY_ID AND 
			                                          e.BRANCH_ID = b.BRANCH_ID AND 
                                                      c.COMPANY_ID = '" + eCOMPANY_ID + @"' AND
			                                          e.DEPT_ID = d.DEPT_ID AND 
			                                          e.DIVISION_ID = dv.DIVISION_ID;";
                        }
                        else
                        {
                            sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,d.DEPT_NAME,dv.DIV_NAME,att.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT(att.IN_DATE,CHAR) AS IN_DATE,att.IN_TIME,att.OUT_TIME,att.REMARK
		                                        FROM
		                                        (
		                                        SELECT a.EMPLOYEE_ID,a.IN_DATE,a.COMPANY_ID,SUM(IF(a.IN_TIME IS NOT NULL, 1, 0)) AS IN_TIME,SUM(IF(a.OUT_TIME IS NOT NULL, 1, 0)) as OUT_TIME,a.REMARK
					                                        FROM
						                                        ATTENDANCE_SUMMARY a
					                                        WHERE
						                                        a.DEPT_ID = '" + dept + @"' AND 
						                                        a.IN_DATE >= '" + frmDate + @"' AND 
						                                        a.IN_DATE <= '" + toDate + @"' AND 
						                                        (a.NUMBER_OF_DAYS = '0.25' or a.NUMBER_OF_DAYS = '0' or a.NUMBER_OF_DAYS is null)
					                                        GROUP BY a.IN_DATE,a.EMPLOYEE_ID
					                                        ORDER BY a.EMPLOYEE_ID ASC ,a.IN_DATE
		
		                                        ) att,EMPLOYEE e,COMPANY_BRANCH b,DEPARTMENT d,DIVISION dv ,COMPANY c
		                                        WHERE att.IN_TIME = 0 AND 
			                                          att.OUT_TIME = 0 AND  
			                                          att.EMPLOYEE_ID = e.EMPLOYEE_ID AND 
			                                          att.COMPANY_ID = e.COMPANY_ID AND 
                                                      c.COMPANY_ID = att.COMPANY_ID AND
			                                          e.BRANCH_ID = b.BRANCH_ID AND 
			                                          e.DEPT_ID = d.DEPT_ID AND 
			                                          e.DIVISION_ID = dv.DIVISION_ID;";
                        }
                    }
                    else
                    {
                        sMySqlString = @"SELECT c.COMP_NAME,b.BRANCH_NAME,d.DEPT_NAME,dv.DIV_NAME,att.EMPLOYEE_ID,e.EPF_NO,e.INITIALS_NAME,CONVERT(att.IN_DATE,CHAR) AS IN_DATE,att.IN_TIME,att.OUT_TIME,att.REMARK
		                                        FROM
		                                        (
		                                        SELECT a.EMPLOYEE_ID,a.IN_DATE,a.COMPANY_ID,SUM(IF(a.IN_TIME IS NOT NULL, 1, 0)) AS IN_TIME,SUM(IF(a.OUT_TIME IS NOT NULL, 1, 0)) as OUT_TIME,a.REMARK
					                                        FROM
						                                        ATTENDANCE_SUMMARY a
					                                        WHERE
						                                        a.EMPLOYEE_ID = '" + mempcode + @"' AND 
						                                        a.IN_DATE >= '" + frmDate + @"' AND 
						                                        a.IN_DATE <= '" + toDate + @"' AND 
						                                        (a.NUMBER_OF_DAYS = '0.25' or a.NUMBER_OF_DAYS = '0' or a.NUMBER_OF_DAYS is null)
					                                        GROUP BY a.IN_DATE,a.EMPLOYEE_ID
					                                        ORDER BY a.EMPLOYEE_ID ASC ,a.IN_DATE
		
		                                        ) att,EMPLOYEE e,COMPANY_BRANCH b,DEPARTMENT d,DIVISION dv ,COMPANY c
		                                        WHERE att.IN_TIME = 0 AND 
			                                          att.OUT_TIME = 0 AND  
			                                          att.EMPLOYEE_ID = e.EMPLOYEE_ID AND 
			                                          att.COMPANY_ID = e.COMPANY_ID AND 
                                                      c.COMPANY_ID = att.COMPANY_ID AND 
			                                          e.BRANCH_ID = b.BRANCH_ID AND 
			                                          e.DEPT_ID = d.DEPT_ID AND 
			                                          e.DIVISION_ID = dv.DIVISION_ID;";
                    }
                }

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlData.Fill(dataTable);

                mySqlTrans.Commit();

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                return dataTable;

            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();

            }
        }

        //Insert to temp table
        public Boolean Insert(string company, string branch, string department, string division, string empId, string epf, string name, int count, DateTime fromDate, DateTime toDate) //branch, department, division, empId, epf, name, indate string branch,string department,string division,string empId,string epf,string name,DateTime indate, 
        {
            bool status = false;
            string frmDate = fromDate.ToString("dd/MM/yyyy");
            string todate = toDate.ToString("dd/MM/yyyy");
            string sMySqlString = "";
            try
            {
                mySqlCon.Open();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@company", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@department", department.Trim() == "" ? (object)DBNull.Value : department.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@division", division.Trim() == "" ? (object)DBNull.Value : division.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@epf", epf.Trim() == "" ? (object)DBNull.Value : epf.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@count", count.ToString().Trim() == "" ? (object)DBNull.Value : count.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", todate.ToString().Trim() == "" ? (object)DBNull.Value : todate.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", frmDate.ToString().Trim() == "" ? (object)DBNull.Value : frmDate.ToString().Trim()));

                sMySqlString = @"INSERT INTO TEMP_CONTINUOUS_ABSENT(COMPANY,BRANCH,DEPARTMENT,DIVISITION,EMPLOYEE_ID,EPF_NO,EMP_NAME,FROM_DATE,TO_DATE,NUMBER) 
                                                VALUES(@company,@branch,@department,@division,@empId,@epf,@name,@fromDate,@toDate,@count);";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                status = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            finally
            {
                mySqlCon.Close();
            }

            return status;
        }

        //Select * from tempTable
        public DataTable tempContinuousAbsent()
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                dataTable = new DataTable();
                sMySqlString = @"SELECT COMPANY,BRANCH,DEPARTMENT,DIVISITION,EMPLOYEE_ID,EPF_NO,EMP_NAME,CONVERT(FROM_DATE,CHAR) AS FROM_DATE,CONVERT(TO_DATE,CHAR) AS TO_DATE,NUMBER FROM TEMP_CONTINUOUS_ABSENT;";

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlData.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Leave Detail
        public DataTable populaterep0027_LeaveDetailofAnEmployee(string eCOMPANY_ID, string dept, string employeeId, string frmDate, string toDate)
        {
            DataTable dtLeaves = new DataTable();

            string sMySqlString = "";
            try
            {
                dtLeaves.Rows.Clear();

                if (eCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (employeeId == "")
                    {
                        //for all company
                        sMySqlString = @" 
                                                        SELECT 
                                                            CAST(ls.LEAVE_DATE AS char) LEAVE_DATE,
                                                            e.INITIALS_NAME,
                                                            c.COMP_NAME,
                                                            d.DEPT_NAME,
                                                            di.DIV_NAME,
                                                            ls.LEAVE_TYPE_ID,
                                                            ls.FROM_TIME,
                                                            ec.INITIALS_NAME as COVERED_BY,
                                                            er.INITIALS_NAME as RECOMMAND_BY,
                                                            ls.TO_TIME,
                                                            CASE
                                                                when lst.LEAVE_STATUS = '0' then 'Rejected'
                                                                when lst.LEAVE_STATUS = '1' then 'Pending'
                                                                when lst.LEAVE_STATUS = '2' then 'Covered'
                                                                when lst.LEAVE_STATUS = '3' then 'Recommended'
                                                                when lst.LEAVE_STATUS = '4' then 'HR Approved'
                                                                when lst.LEAVE_STATUS = '9' then 'Discarded'
                                                            END AS LEAVE_STATUS
                                                        FROM
                                                            EMPLOYEE er,
                                                            EMPLOYEE ec,
                                                            COMPANY c,
                                                            DEPARTMENT d,
                                                            DIVISION di,
                                                            LEAVE_SHEET lst,
                                                            EMPLOYEE_LEAVE_SCHEDULE ls
                                                                INNER JOIN
                                                            EMPLOYEE e ON ls.EMPLOYEE_ID = e.EMPLOYEE_ID
                                                        WHERE
                                                            ls.LEAVE_DATE >= '" + frmDate.Trim() + @"' 
                                                        AND ls.LEAVE_DATE <= '" + toDate.Trim() + @"' 
                                                        AND e.COMPANY_ID = c.COMPANY_ID
                                                        AND di.DIVISION_ID = e.DIVISION_ID
                                                        AND e.DEPT_ID = d.DEPT_ID
                                                        AND (ec.EMPLOYEE_ID = ls.COVERED_BY)
                                                        AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY)
                                                        AND ls.LEAVE_SHEET_ID = lst.LEAVE_SHEET_ID
                                                ORDER BY ls.LEAVE_DATE , e.EMPLOYEE_ID;";
                    }
                    else
                    {
                        //For employee
                        //sMySqlString = " SELECT CAST(ls.LEAVE_DATE AS char) LEAVE_DATE, ls.LEAVE_TYPE_ID,ls.FROM_TIME,ec.INITIALS_NAME as COVERED_BY,er.INITIALS_NAME as RECOMMAND_BY,ls.TO_TIME, " +
                        //            " CASE when LEAVE_STATUS = '0' then 'Rejected' " +
                        //            "         when LEAVE_STATUS = '1' then 'Pending' " +
                        //            "         when LEAVE_STATUS = '2' then 'Covered' " +
                        //            "         when LEAVE_STATUS = '3' then 'Recommended' " +
                        //            "         when LEAVE_STATUS = '4' then 'HR Approved' " +
                        //            "         when LEAVE_STATUS = '9' then 'Discarded' " +
                        //            "    END AS LEAVE_STATUS " +
                        //            " FROM " +
                        //            "     EMPLOYEE_LEAVE_SCHEDULE ls, " +
                        //            "     EMPLOYEE ec, " +
                        //            "     EMPLOYEE er " +
                        //            " WHERE " +
                        //            "     ls.EMPLOYEE_ID = '" + employeeId.Trim() + "' " +
                        //            "         AND (ec.EMPLOYEE_ID = ls.COVERED_BY) " +
                        //            "         AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY) " +
                        //            "         AND ls.LEAVE_DATE >= '" + frmDate.Trim() + "'" +
                        //            "         AND ls.LEAVE_DATE <= '" + toDate.Trim() + "'" +
                        //            " GROUP BY ls.APPROVED_BY , ls.RECOMMAND_BY , ls.LEAVE_DATE " +
                        //            " order by ls.LEAVE_DATE";
                        sMySqlString = @" 
                                                SELECT 
                                                    CAST(ls.LEAVE_DATE AS char) LEAVE_DATE,
                                                    e.INITIALS_NAME,
                                                    c.COMP_NAME,
                                                    d.DEPT_NAME,
                                                    di.DIV_NAME,
                                                    ls.LEAVE_TYPE_ID,
                                                    ls.FROM_TIME,
                                                    ec.INITIALS_NAME as COVERED_BY,
                                                    er.INITIALS_NAME as RECOMMAND_BY,
                                                    ls.TO_TIME,
                                                    CASE
                                                        when lst.LEAVE_STATUS = '0' then 'Rejected'
                                                        when lst.LEAVE_STATUS = '1' then 'Pending'
                                                        when lst.LEAVE_STATUS = '2' then 'Covered'
                                                        when lst.LEAVE_STATUS = '3' then 'Recommended'
                                                        when lst.LEAVE_STATUS = '4' then 'HR Approved'
                                                        when lst.LEAVE_STATUS = '9' then 'Discarded'
                                                    END AS LEAVE_STATUS
                                                FROM
                                                    EMPLOYEE er,
                                                    EMPLOYEE ec,
                                                    COMPANY c,
                                                    DEPARTMENT d,
                                                    DIVISION di,
                                                    LEAVE_SHEET lst,
                                                    EMPLOYEE_LEAVE_SCHEDULE ls
                                                        INNER JOIN
                                                    EMPLOYEE e ON ls.EMPLOYEE_ID = e.EMPLOYEE_ID
                                                WHERE
                                                    ls.LEAVE_DATE >= '" + frmDate.Trim() + @"' AND ls.LEAVE_DATE <= '" + toDate.Trim() + @"' 
                                                        AND e.EMPLOYEE_ID = '" + employeeId + @"'
                                                        AND e.COMPANY_ID = c.COMPANY_ID
                                                        AND di.DIVISION_ID = e.DIVISION_ID
                                                        AND e.DEPT_ID = d.DEPT_ID
                                                        AND (ec.EMPLOYEE_ID = ls.COVERED_BY)
                                                        AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY)
                                                        AND ls.LEAVE_SHEET_ID = lst.LEAVE_SHEET_ID
                                                ORDER BY ls.LEAVE_DATE , e.EMPLOYEE_ID;";

                    }
                }
                else
                {
                    if (dept == "")
                    {
                        if (employeeId == "")
                        {
                            sMySqlString = @" 
                                               SELECT 
                                                    CAST(ls.LEAVE_DATE AS char) LEAVE_DATE,e.INITIALS_NAME,
                                                    c.COMP_NAME,d.DEPT_NAME,di.DIV_NAME,ls.LEAVE_TYPE_ID,ls.FROM_TIME,
                                                    ec.INITIALS_NAME as COVERED_BY,er.INITIALS_NAME as RECOMMAND_BY,
                                                    ls.TO_TIME,
                                                    CASE
                                                        when lst.LEAVE_STATUS = '0' then 'Rejected'
                                                        when lst.LEAVE_STATUS = '1' then 'Pending'
                                                        when lst.LEAVE_STATUS = '2' then 'Covered'
                                                        when lst.LEAVE_STATUS = '3' then 'Recommended'
                                                        when lst.LEAVE_STATUS = '4' then 'HR Approved'
                                                        when lst.LEAVE_STATUS = '9' then 'Discarded'
                                                    END AS LEAVE_STATUS
                                                FROM
                                                    EMPLOYEE er,EMPLOYEE ec,COMPANY c,DEPARTMENT d,DIVISION di,
                                                    LEAVE_SHEET lst,EMPLOYEE_LEAVE_SCHEDULE ls
                                                        INNER JOIN
                                                    EMPLOYEE e ON ls.EMPLOYEE_ID = e.EMPLOYEE_ID
                                                WHERE
                                                    ls.LEAVE_DATE >= '" + frmDate.Trim() + @"' AND ls.LEAVE_DATE <= '" + toDate.Trim() + @"' 
                                                        AND e.COMPANY_ID = '" + eCOMPANY_ID + @"'
                                                        AND e.COMPANY_ID = c.COMPANY_ID
                                                        AND di.DIVISION_ID = e.DIVISION_ID
                                                        AND e.DEPT_ID = d.DEPT_ID
                                                        AND (ec.EMPLOYEE_ID = ls.COVERED_BY)
                                                        AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY)
                                                        AND ls.LEAVE_SHEET_ID = lst.LEAVE_SHEET_ID
                                                ORDER BY ls.LEAVE_DATE,e.EMPLOYEE_ID;";
                        }
                        else
                        {
                            //employee
                            sMySqlString = @" 
                                                SELECT 
                                                    CAST(ls.LEAVE_DATE AS char) LEAVE_DATE,
                                                    e.INITIALS_NAME,
                                                    c.COMP_NAME,
                                                    d.DEPT_NAME,
                                                    di.DIV_NAME,
                                                    ls.LEAVE_TYPE_ID,
                                                    ls.FROM_TIME,
                                                    ec.INITIALS_NAME as COVERED_BY,
                                                    er.INITIALS_NAME as RECOMMAND_BY,
                                                    ls.TO_TIME,
                                                    CASE
                                                        when lst.LEAVE_STATUS = '0' then 'Rejected'
                                                        when lst.LEAVE_STATUS = '1' then 'Pending'
                                                        when lst.LEAVE_STATUS = '2' then 'Covered'
                                                        when lst.LEAVE_STATUS = '3' then 'Recommended'
                                                        when lst.LEAVE_STATUS = '4' then 'HR Approved'
                                                        when lst.LEAVE_STATUS = '9' then 'Discarded'
                                                    END AS LEAVE_STATUS
                                                FROM
                                                    EMPLOYEE er,
                                                    EMPLOYEE ec,
                                                    COMPANY c,
                                                    DEPARTMENT d,
                                                    DIVISION di,
                                                    LEAVE_SHEET lst,
                                                    EMPLOYEE_LEAVE_SCHEDULE ls
                                                        INNER JOIN
                                                    EMPLOYEE e ON ls.EMPLOYEE_ID = e.EMPLOYEE_ID
                                                WHERE
                                                 ls.LEAVE_DATE >= '" + frmDate.Trim() + @"' 
                                                        AND ls.LEAVE_DATE <= '" + toDate.Trim() + @"' 
                                                        AND e.EMPLOYEE_ID = '" + employeeId + @"'
                                                        AND e.COMPANY_ID = c.COMPANY_ID
                                                        AND di.DIVISION_ID = e.DIVISION_ID
                                                        AND e.DEPT_ID = d.DEPT_ID
                                                        AND (ec.EMPLOYEE_ID = ls.COVERED_BY)
                                                        AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY)
                                                        AND ls.LEAVE_SHEET_ID = lst.LEAVE_SHEET_ID
                                             ORDER BY ls.LEAVE_DATE,e.EMPLOYEE_ID;";
                        }
                    }
                    else
                    {
                        //department
                        sMySqlString = @" 
                                                SELECT 
                                                    CAST(ls.LEAVE_DATE AS char) LEAVE_DATE,
                                                    e.INITIALS_NAME,
                                                    c.COMP_NAME,
                                                    d.DEPT_NAME,
                                                    di.DIV_NAME,
                                                    ls.LEAVE_TYPE_ID,
                                                    ls.FROM_TIME,
                                                    ec.INITIALS_NAME as COVERED_BY,
                                                    er.INITIALS_NAME as RECOMMAND_BY,
                                                    ls.TO_TIME,
                                                    CASE
                                                        when lst.LEAVE_STATUS = '0' then 'Rejected'
                                                        when lst.LEAVE_STATUS = '1' then 'Pending'
                                                        when lst.LEAVE_STATUS = '2' then 'Covered'
                                                        when lst.LEAVE_STATUS = '3' then 'Recommended'
                                                        when lst.LEAVE_STATUS = '4' then 'HR Approved'
                                                        when lst.LEAVE_STATUS = '9' then 'Discarded'
                                                    END AS LEAVE_STATUS
                                                FROM
                                                    EMPLOYEE er,
                                                    EMPLOYEE ec,
                                                    COMPANY c,
                                                    DEPARTMENT d,
                                                    DIVISION di,
                                                    LEAVE_SHEET lst,
                                                    EMPLOYEE_LEAVE_SCHEDULE ls
                                                        INNER JOIN
                                                    EMPLOYEE e ON ls.EMPLOYEE_ID = e.EMPLOYEE_ID
                                                WHERE
                                                 ls.LEAVE_DATE >= '" + frmDate.Trim() + @"' AND ls.LEAVE_DATE <= '" + toDate.Trim() + @"' 
                                                    AND e.DEPT_ID = '" + dept + @"'
                                                    AND e.COMPANY_ID = c.COMPANY_ID
                                                    AND di.DIVISION_ID = e.DIVISION_ID
                                                    AND e.DEPT_ID = d.DEPT_ID
                                                    AND (ec.EMPLOYEE_ID = ls.COVERED_BY)
                                                    AND (er.EMPLOYEE_ID = ls.RECOMMAND_BY)
                                                    AND ls.LEAVE_SHEET_ID = lst.LEAVE_SHEET_ID
                                            ORDER BY ls.LEAVE_DATE , e.EMPLOYEE_ID;";
                    }
                }

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlData.Fill(dtLeaves);
                return dtLeaves;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable populaterep0028IND(string EMP_ID, string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND 
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND
                                            (E.IS_ROSTER='0' OR E.IS_ROSTER IS NULL) AND (ATTS.REMARK IS NOT NULL AND ATTS.REMARK <> 'Working Day') AND
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND E.EMPLOYEE_ID='" + EMP_ID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                     ) 
                                     UNION
                                     (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND (ATTS.ROSTER_ID IS NULL OR ROSTER_ID = '0') AND
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND 
                                            E.IS_ROSTER='1' AND 
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND E.EMPLOYEE_ID='" + EMP_ID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                    )
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

        public DataTable populaterep0028AllCompany(string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND 
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND
                                            (E.IS_ROSTER='0' OR E.IS_ROSTER IS NULL) AND (ATTS.REMARK IS NOT NULL AND ATTS.REMARK <> 'Working Day') AND
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"'  
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                     ) 
                                     UNION
                                     (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND (ATTS.ROSTER_ID IS NULL OR ROSTER_ID = '0') AND
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND 
                                            E.IS_ROSTER='1' AND 
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                    )
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

        public DataTable populaterep0028INDCompany(string CompanyID, string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND 
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND
                                            (E.IS_ROSTER='0' OR E.IS_ROSTER IS NULL) AND (ATTS.REMARK IS NOT NULL AND ATTS.REMARK <> 'Working Day') AND
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND ATTS.COMPANY_ID = '" + CompanyID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                     ) 
                                     UNION
                                     (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND (ATTS.ROSTER_ID IS NULL OR ROSTER_ID = '0') AND
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND 
                                            E.IS_ROSTER='1' AND 
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND ATTS.COMPANY_ID = '" + CompanyID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                    )
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

        public DataTable populaterep0028INDDepartment(string DepartmantID, string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND 
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND
                                            (E.IS_ROSTER='0' OR E.IS_ROSTER IS NULL) AND (ATTS.REMARK IS NOT NULL AND ATTS.REMARK <> 'Working Day') AND
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND ATTS.DEPT_ID = '" + DepartmantID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                     ) 
                                     UNION
                                     (
                                        SELECT 
                                            C.COMP_NAME,DPT.DEPT_NAME,DV.DIV_NAME,ED.DESIGNATION_NAME,E.EPF_NO,E.INITIALS_NAME,E.FULL_NAME,CONVERT(ATTS.IN_DATE,CHAR ) AS 'WORKING_DATE',
                                            CONVERT(ATTS.IN_TIME,CHAR) AS 'IN_TIME',CONVERT(ATTS.OUT_TIME,CHAR) AS 'OUT_TIME',ATTS.REMARK 
                                        FROM 
                                            ATTENDANCE_SUMMARY ATTS, EMPLOYEE E,COMPANY C,DEPARTMENT DPT,DIVISION DV,EMPLOYEE_DESIGNATION ED
                                        WHERE 
                                            E.EMPLOYEE_ID = ATTS.EMPLOYEE_ID AND ATTS.COMPANY_ID = C.COMPANY_ID AND DPT.DEPT_ID = ATTS.DEPT_ID AND DV.DIVISION_ID = ATTS.DIVISION_ID AND 
                                            ED.DESIGNATION_ID = E.DESIGNATION_ID AND (ATTS.ROSTER_ID IS NULL OR ROSTER_ID = '0') AND
                                            (ATTS.IN_TIME IS NOT NULL OR ATTS.OUT_TIME IS NOT NULL) AND 
                                            E.IS_ROSTER='1' AND 
                                            ATTS.IN_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"' AND ATTS.DEPT_ID = '" + DepartmantID + @"' 
                                        ORDER BY 
                                            C.COMP_NAME ASC,DPT.DEPT_NAME ASC,DV.DIV_NAME ASC,ATTS.IN_DATE ASC
                                    )
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

        public DataTable populaterep0029Company(string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            DataTable dtOpeningHeadCount = new DataTable();
            DataTable dtTransferCount = new DataTable();

            DataTable dtNewRecruitments = new DataTable();
            DataTable dtNewRecruitmentTransferCount = new DataTable();

            DataTable dtTransferIn = new DataTable();
            DataTable dtResigned = new DataTable();
            DataTable dtTransferOut = new DataTable();
            DataTable dtClosingHeadCount = new DataTable();

            try
            {

                //Get Opening Head Count
                //                sMySqlString = @" 
                //
                //                                        SELECT 
                //                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'OPENING_HEAD_COUNT'  
                //                                        FROM 
                //                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                //                                        WHERE 
                //                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND E.DOJ <= '" + FromDate + @"' AND (E.RESIGNED_DATE > '" + FromDate + @"' OR E.RESIGNED_DATE IS NULL)
                //                                        GROUP BY 
                //                                            C.COMP_NAME, E.DEPT_ID;
                //                            
                //                                ";

                //                sMySqlString = @" 
                //
                //                                        SELECT 
                //                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'OPENING_HEAD_COUNT'  
                //                                        FROM 
                //                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                //                                        WHERE 
                //                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                //                                            E.COMPANY_ID = '" + CompanyID + @"' AND E.DOJ <= '" + FromDate + @"' AND (E.RESIGNED_DATE > '" + FromDate + @"' OR E.RESIGNED_DATE IS NULL)
                //                                        GROUP BY 
                //                                            C.COMP_NAME, E.DEPT_ID;
                //                            
                //                                ";
                sMySqlString = @" 

                                        SELECT 
                                            T.COMPANY_ID, T.COMP_NAME, T.DEPT_ID, T.DEPT_NAME, T.OPENING_HEAD_COUNT
                                        FROM 
                                            (
                                                (
                                                SELECT 
                                                    E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'OPENING_HEAD_COUNT' 
                                                FROM 
                                                    EMPLOYEE E, COMPANY C, DEPARTMENT D
                                                WHERE 
                                                    E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                                                    E.DOJ <= '" + FromDate + @"' AND (E.RESIGNED_DATE > '" + FromDate + @"' OR E.RESIGNED_DATE IS NULL)
                                                GROUP BY 
                                                    C.COMP_NAME, E.DEPT_ID
                                                )
                                                UNION
                                                (
                                                    SELECT 
                                                        D.COMPANY_ID,C.COMP_NAME, D.DEPT_ID, D.DEPT_NAME, 0 AS 'OPENING_HEAD_COUNT' 
                                                    FROM 
                                                         COMPANY C, DEPARTMENT D
                                                    WHERE 
                                                        D.COMPANY_ID = C.COMPANY_ID AND D.DEPT_ID 
                                                        NOT IN 
                                                        (
                                                            SELECT DISTINCT 
                                                                EE.DEPT_ID 
                                                            FROM 
                                                                EMPLOYEE EE 
                                                            WHERE 
                                                                EE.DOJ <= '" + FromDate + @"' AND 
                                                                (EE.RESIGNED_DATE > '" + FromDate + @"' OR EE.RESIGNED_DATE IS NULL)
                                                        )
                                                )
                                            ) T
                                        ORDER BY 
                                            T.COMP_NAME, T.DEPT_NAME;
                            
                                ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtOpeningHeadCount);

                //Get Transfer Count
                sMySqlString = @" 
                                        SELECT 
                                            ET.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, ET.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(ET.EMPLOYEE_ID) AS 'TRANSFER_COUNT'
                                        FROM 
                                            EMPLOYEE_TRNSFERS ET, COMPANY C, DEPARTMENT D, EMPLOYEE E 
                                        WHERE 
                                            ET.FROM_COMPANY_ID = C.COMPANY_ID AND ET.FROM_DEPT_ID = D.DEPT_ID AND (ET.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')  AND ET.EMPLOYEE_ID = E.EMPLOYEE_ID AND E.DOJ <= '" + FromDate + @"' 
                                        GROUP BY 
                                            ET.FROM_COMPANY_ID, ET.FROM_DEPT_ID;                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferCount);

                //Get New Recruitment
                sMySqlString = @" 
                                        SELECT 
                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'NEW_RECRUITMENTS'  
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND (E.DOJ BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtNewRecruitments);

                //Get New Recruitment Transfer Count
                sMySqlString = @" 
                                        SELECT 
                                            ET.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, ET.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(ET.EMPLOYEE_ID) AS 'TRANSFER_COUNT'   
                                        FROM  
                                            EMPLOYEE_TRNSFERS ET, COMPANY C, DEPARTMENT D, EMPLOYEE E
                                        WHERE 
                                            ET.FROM_COMPANY_ID = C.COMPANY_ID AND ET.FROM_DEPT_ID = D.DEPT_ID AND ET.EMPLOYEE_ID = E.EMPLOYEE_ID AND
                                            E.DOJ > '" + FromDate + @"' AND E.DOJ <= '" + ToDate + @"' AND ET.START_DATE > '" + FromDate + @"' AND ET.START_DATE <= '" + ToDate + @"'
                                        GROUP BY 
                                            ET.FROM_COMPANY_ID, ET.FROM_DEPT_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtNewRecruitmentTransferCount);


                //Get Transfer In
                sMySqlString = @" 
                                        SELECT 
                                            E.TO_COMPANY_ID AS 'COMPANY_ID',C.COMP_NAME,E.TO_DEPT_ID AS 'DEPT_ID',D.DEPT_NAME,COUNT(E.EMPLOYEE_ID) AS 'TRANSFER_IN'  
                                        FROM 
                                            EMPLOYEE_TRNSFERS E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.TO_COMPANY_ID=C.COMPANY_ID AND E.TO_DEPT_ID=D.DEPT_ID AND (E.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.TO_DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferIn);


                //Get Resigned
                sMySqlString = @" 
                                        SELECT 
                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'RESIGNED'   
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND (E.RESIGNED_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
                                        GROUP BY 
                                            E.DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtResigned);


                //Get Transfer Out
                sMySqlString = @" 
                                        SELECT 
                                            E.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, E.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'TRANSFER_OUT'  
                                        FROM 
                                            EMPLOYEE_TRNSFERS E, COMPANY C, DEPARTMENT D
                                        WHERE 
                                            E.FROM_COMPANY_ID=C.COMPANY_ID AND E.FROM_DEPT_ID = D.DEPT_ID AND (E.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.FROM_DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferOut);


                //Get Closing Head Count
                //                sMySqlString = @" 
                //                                        SELECT 
                //                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'CLOSING_HEAD_COUNT'   
                //                                        FROM 
                //                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                //                                        WHERE 
                //                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND E.DOJ <= '" + ToDate + @"' 
                //                                        GROUP BY 
                //                                            E.DEPT_ID, C.COMPANY_ID;
                //                            
                //                                ";
                //                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                //                mySqlDa.Fill(dtClosingHeadCount);

                dataTable = ConfigureStaffStrengthDataTable(dtOpeningHeadCount.Copy(), dtTransferCount.Copy(), dtNewRecruitments.Copy(), dtTransferIn.Copy(), dtResigned.Copy(), dtTransferOut.Copy(), dtClosingHeadCount.Copy(), dtNewRecruitmentTransferCount.Copy()).Copy();


                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populaterep0029INDCompany(string CompanyID, string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            DataTable dtOpeningHeadCount = new DataTable();
            DataTable dtTransferCount = new DataTable();

            DataTable dtNewRecruitments = new DataTable();
            DataTable dtNewRecruitmentTransferCount = new DataTable();

            DataTable dtTransferIn = new DataTable();
            DataTable dtResigned = new DataTable();
            DataTable dtTransferOut = new DataTable();
            DataTable dtClosingHeadCount = new DataTable();

            try
            {

                //Get Opening Head Count

                //                sMySqlString = @" 
                //
                //                                        SELECT 
                //                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'OPENING_HEAD_COUNT'  
                //                                        FROM 
                //                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                //                                        WHERE 
                //                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                //                                            E.COMPANY_ID = '" + CompanyID + @"' AND E.DOJ <= '" + FromDate + @"' AND (E.RESIGNED_DATE > '" + FromDate + @"' OR E.RESIGNED_DATE IS NULL)
                //                                        GROUP BY 
                //                                            C.COMP_NAME, E.DEPT_ID;
                //                            
                //                                ";
                sMySqlString = @" 

                                        SELECT 
                                            T.COMPANY_ID, T.COMP_NAME, T.DEPT_ID, T.DEPT_NAME, T.OPENING_HEAD_COUNT
                                        FROM 
                                            (
                                                (
                                                SELECT 
                                                    E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'OPENING_HEAD_COUNT' 
                                                FROM 
                                                    EMPLOYEE E, COMPANY C, DEPARTMENT D
                                                WHERE 
                                                    E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                                                    E.COMPANY_ID = '" + CompanyID + @"' AND E.DOJ <= '" + FromDate + @"' AND (E.RESIGNED_DATE > '" + FromDate + @"' OR E.RESIGNED_DATE IS NULL)
                                                GROUP BY 
                                                    C.COMP_NAME, E.DEPT_ID
                                                )
                                                UNION
                                                (
                                                    SELECT 
                                                        D.COMPANY_ID,C.COMP_NAME, D.DEPT_ID, D.DEPT_NAME, 0 AS 'OPENING_HEAD_COUNT' 
                                                    FROM 
                                                         COMPANY C, DEPARTMENT D
                                                    WHERE 
                                                        D.COMPANY_ID = C.COMPANY_ID and C.COMPANY_ID ='" + CompanyID + @"' and D.DEPT_ID 
                                                        NOT IN 
                                                        (
                                                            SELECT DISTINCT 
                                                                EE.DEPT_ID 
                                                            from 
                                                                EMPLOYEE EE 
                                                            where 
                                                                EE.COMPANY_ID = '" + CompanyID + @"' AND EE.DOJ <= '" + FromDate + @"' AND 
                                                                (EE.RESIGNED_DATE > '" + FromDate + @"' OR EE.RESIGNED_DATE IS NULL)
                                                        )
                                                )
                                            ) T
                                        ORDER BY 
                                            T.COMP_NAME, T.DEPT_NAME;
                            
                                ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtOpeningHeadCount);

                //Get Transfer Count
                sMySqlString = @" 
                                        SELECT 
                                            ET.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, ET.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(ET.EMPLOYEE_ID) AS 'TRANSFER_COUNT'
                                        FROM 
                                            EMPLOYEE_TRNSFERS ET, COMPANY C, DEPARTMENT D, EMPLOYEE E
                                        WHERE 
                                            ET.FROM_COMPANY_ID = C.COMPANY_ID AND ET.FROM_DEPT_ID = D.DEPT_ID AND (ET.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') AND ET.FROM_COMPANY_ID = '" + CompanyID + @"' AND ET.EMPLOYEE_ID = E.EMPLOYEE_ID AND E.DOJ <= '" + FromDate + @"'
                                        GROUP BY 
                                            ET.FROM_COMPANY_ID, ET.FROM_DEPT_ID;                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferCount);

                //Get New Recruitment
                sMySqlString = @" 
                                        SELECT 
                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'NEW_RECRUITMENTS'  
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                                            E.COMPANY_ID = '" + CompanyID + @"' AND (E.DOJ BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtNewRecruitments);

                //Get New Recruitment Transfer Count
                sMySqlString = @" 
                                        SELECT 
                                            ET.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, ET.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(ET.EMPLOYEE_ID) AS 'TRANSFER_COUNT'   
                                        FROM  
                                            EMPLOYEE_TRNSFERS ET, COMPANY C, DEPARTMENT D, EMPLOYEE E
                                        WHERE 
                                            ET.FROM_COMPANY_ID = C.COMPANY_ID AND ET.FROM_DEPT_ID = D.DEPT_ID AND ET.EMPLOYEE_ID = E.EMPLOYEE_ID AND ET.FROM_COMPANY_ID = '" + CompanyID + @"' AND
                                            E.DOJ > '" + FromDate + @"' AND E.DOJ <= '" + ToDate + @"' AND ET.START_DATE > '" + FromDate + @"' AND ET.START_DATE <= '" + ToDate + @"'
                                        GROUP BY 
                                            ET.FROM_COMPANY_ID, ET.FROM_DEPT_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtNewRecruitmentTransferCount);


                //Get Transfer In
                sMySqlString = @" 
                                        SELECT 
                                            E.TO_COMPANY_ID AS 'COMPANY_ID',C.COMP_NAME,E.TO_DEPT_ID AS 'DEPT_ID',D.DEPT_NAME,COUNT(E.EMPLOYEE_ID) AS 'TRANSFER_IN'  
                                        FROM 
                                            EMPLOYEE_TRNSFERS E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.TO_COMPANY_ID=C.COMPANY_ID AND E.TO_DEPT_ID=D.DEPT_ID AND 
                                            E.TO_COMPANY_ID = '" + CompanyID + @"' AND (E.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.TO_DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferIn);


                //Get Resigned
                sMySqlString = @" 
                                        SELECT 
                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'RESIGNED'   
                                        FROM 
                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                                        WHERE
                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                                            E.COMPANY_ID = '" + CompanyID + @"' AND (E.RESIGNED_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
                                        GROUP BY 
                                            E.DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtResigned);


                //Get Transfer Out
                sMySqlString = @" 
                                        SELECT 
                                            E.FROM_COMPANY_ID AS 'COMPANY_ID', C.COMP_NAME, E.FROM_DEPT_ID AS 'DEPT_ID', D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'TRANSFER_OUT'  
                                        FROM 
                                            EMPLOYEE_TRNSFERS E, COMPANY C, DEPARTMENT D
                                        WHERE 
                                            E.FROM_COMPANY_ID=C.COMPANY_ID AND E.FROM_DEPT_ID = D.DEPT_ID AND
                                            E.FROM_COMPANY_ID = '" + CompanyID + @"' AND (E.START_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') 
                                        GROUP BY 
                                            E.FROM_DEPT_ID, C.COMPANY_ID;
                            
                                ";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtTransferOut);


                //Get Closing Head Count
                //                sMySqlString = @" 
                //                                        SELECT 
                //                                            E.COMPANY_ID,C.COMP_NAME, E.DEPT_ID, D.DEPT_NAME, COUNT(E.EMPLOYEE_ID) AS 'CLOSING_HEAD_COUNT'   
                //                                        FROM 
                //                                            EMPLOYEE E, COMPANY C, DEPARTMENT D
                //                                        WHERE 
                //                                            E.COMPANY_ID=C.COMPANY_ID AND E.DEPT_ID=D.DEPT_ID AND 
                //                                            E.COMPANY_ID = '" + CompanyID + @"' AND E.DOJ <= '" + ToDate + @"' 
                //                                        GROUP BY 
                //                                            E.DEPT_ID, C.COMPANY_ID;
                //                            
                //                                ";
                //                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                //                mySqlDa.Fill(dtClosingHeadCount);

                dataTable = ConfigureStaffStrengthDataTable(dtOpeningHeadCount.Copy(), dtTransferCount.Copy(), dtNewRecruitments.Copy(), dtTransferIn.Copy(), dtResigned.Copy(), dtTransferOut.Copy(), dtClosingHeadCount.Copy(), dtNewRecruitmentTransferCount.Copy()).Copy();


                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getAllCompany()
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                sMySqlString = @" 
                                        SELECT 
                                            COMPANY_ID, COMP_NAME
                                        FROM
                                            COMPANY
                                        WHERE
                                            STATUS_CODE = '1';
                            
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

        public DataTable getAllDepartments(string CompanyID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();

            try
            {
                sMySqlString = @" 
                                        SELECT 
                                            D.COMPANY_ID AS 'COMPANY_ID', (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = D.COMPANY_ID) AS 'COMP_NAME',
                                            D.DEPT_ID,D.DEPT_NAME
                                        FROM  
                                            DEPARTMENT D
                                        WHERE 
                                            D.STATUS_CODE = '1' AND D.COMPANY_ID = '" + CompanyID + @"'
                            
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

        DataTable ConfigureStaffStrengthDataTable(DataTable OpeningheadCount, DataTable dtTransferCount, DataTable NewRecruitments, DataTable TransferIn, DataTable Resigned, DataTable TransferOut, DataTable ClosingHeadCount, DataTable NewRecruitmentTransferCount)
        {
            DataTable dtStaffStrength = new DataTable();

            dtStaffStrength.Columns.Add("COMPANY_ID", typeof(string));
            dtStaffStrength.Columns.Add("COMP_NAME", typeof(string));
            dtStaffStrength.Columns.Add("DEPT_ID", typeof(string));
            dtStaffStrength.Columns.Add("DEPT_NAME", typeof(string));
            dtStaffStrength.Columns.Add("OPENING_HEAD_COUNT", typeof(Int32));
            dtStaffStrength.Columns.Add("NEW_RECRUITMENTS", typeof(Int32));
            dtStaffStrength.Columns.Add("TRANSFER_IN", typeof(Int32));
            dtStaffStrength.Columns.Add("RESIGNED", typeof(Int32));
            dtStaffStrength.Columns.Add("TRANSFER_OUT", typeof(Int32));
            dtStaffStrength.Columns.Add("CLOSING_HEAD_COUNT", typeof(Int32));
            dtStaffStrength.Columns.Add("STAFF_STRENGTH", typeof(Double));
            dtStaffStrength.Columns.Add("STAFF_TURN_OVER", typeof(Double));

            for (int i = 0; i < OpeningheadCount.Rows.Count; i++)
            {
                string companyID = OpeningheadCount.Rows[i]["COMPANY_ID"].ToString();
                string deptID = OpeningheadCount.Rows[i]["DEPT_ID"].ToString();

                DataRow dr = dtStaffStrength.NewRow();

                dr["COMPANY_ID"] = OpeningheadCount.Rows[i]["COMPANY_ID"].ToString();
                dr["COMP_NAME"] = OpeningheadCount.Rows[i]["COMP_NAME"].ToString();
                dr["DEPT_ID"] = OpeningheadCount.Rows[i]["DEPT_ID"].ToString();
                dr["DEPT_NAME"] = OpeningheadCount.Rows[i]["DEPT_NAME"].ToString();

                int openingHeadCount = Convert.ToInt32(OpeningheadCount.Rows[i]["OPENING_HEAD_COUNT"].ToString());

                try//Add Transfer Employees to Opening Head Count
                {
                    DataRow[] result = dtTransferCount.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        openingHeadCount += Convert.ToInt32(result[0]["TRANSFER_COUNT"].ToString());
                        dr["OPENING_HEAD_COUNT"] = openingHeadCount;
                    }
                    else
                    {
                        dr["OPENING_HEAD_COUNT"] = openingHeadCount;
                    }
                }
                catch
                {
                    dr["OPENING_HEAD_COUNT"] = openingHeadCount;
                }


                int newRecruitments = 0;
                try//Merge New Recruitments With Main Table
                {
                    DataRow[] result = NewRecruitments.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        newRecruitments = Convert.ToInt32(result[0]["NEW_RECRUITMENTS"].ToString());
                        dr["NEW_RECRUITMENTS"] = newRecruitments;
                    }
                    else
                    {
                        dr["NEW_RECRUITMENTS"] = newRecruitments;
                    }
                }
                catch
                {
                    dr["NEW_RECRUITMENTS"] = newRecruitments;
                }

                int newRecruitmentsTransfer = 0;
                try//Merge New Recruitment Transfers With Main Table
                {
                    DataRow[] result = NewRecruitmentTransferCount.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        newRecruitmentsTransfer = Convert.ToInt32(result[0]["TRANSFER_COUNT"].ToString());
                        newRecruitments += newRecruitmentsTransfer;
                        dr["NEW_RECRUITMENTS"] = newRecruitments;
                    }
                    else
                    {
                        dr["NEW_RECRUITMENTS"] = newRecruitments;
                    }
                }
                catch
                {
                    dr["NEW_RECRUITMENTS"] = newRecruitments;
                }

                int transferIN = 0;
                try//Merge Transfer In With Main Table
                {
                    DataRow[] result = TransferIn.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        transferIN = Convert.ToInt32(result[0]["TRANSFER_IN"].ToString());
                        dr["TRANSFER_IN"] = transferIN;
                    }
                    else
                    {
                        dr["TRANSFER_IN"] = transferIN;
                    }
                }
                catch
                {
                    dr["TRANSFER_IN"] = transferIN;
                }

                int resigned = 0;
                try//Merge Resign With Main Table
                {
                    DataRow[] result = Resigned.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        resigned = Convert.ToInt32(result[0]["RESIGNED"].ToString());
                        dr["RESIGNED"] = resigned;
                    }
                    else
                    {
                        dr["RESIGNED"] = resigned;
                    }
                }
                catch
                {
                    dr["RESIGNED"] = resigned;
                }

                int transferOUT = 0;
                try//Merge Transfer Out With Main Table
                {
                    DataRow[] result = TransferOut.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                    if (result.Length > 0)
                    {
                        transferOUT = Convert.ToInt32(result[0]["TRANSFER_OUT"].ToString());
                        dr["TRANSFER_OUT"] = transferOUT;
                    }
                    else
                    {
                        dr["TRANSFER_OUT"] = transferOUT;
                    }
                }
                catch
                {
                    dr["TRANSFER_OUT"] = transferOUT;
                }

                int closingHeadCount = 0;
                //try//Merge Closing Head Count With Main Table
                //{
                //    DataRow[] result = ClosingHeadCount.Select("COMPANY_ID = '" + companyID + "' AND DEPT_ID = '" + deptID + "'");
                //    if (result.Length > 0)
                //    {
                //        closingHeadCount = Convert.ToInt32(result[0]["CLOSING_HEAD_COUNT"].ToString());
                //        dr["CLOSING_HEAD_COUNT"] = closingHeadCount;
                //    }
                //    else
                //    {
                //        dr["CLOSING_HEAD_COUNT"] = closingHeadCount;
                //    }
                //}
                //catch
                //{
                //    dr["CLOSING_HEAD_COUNT"] = closingHeadCount;
                //}

                closingHeadCount = (newRecruitments + transferIN + openingHeadCount) - (resigned + transferOUT);
                dr["CLOSING_HEAD_COUNT"] = closingHeadCount;

                Double A = Convert.ToDouble(newRecruitments);
                Double B = Convert.ToDouble(transferIN);
                Double C = Convert.ToDouble(openingHeadCount);
                Double D = Convert.ToDouble(closingHeadCount);
                Double E = Convert.ToDouble(resigned);
                Double F = Convert.ToDouble(transferOUT);



                Double staffStrength = ((A + B) / (((C + D) / 2))) * 100.00;
                if (Double.IsNaN(staffStrength))
                {
                    staffStrength = 0;
                }
                Double staffTurnOver = ((E + F) / (((C + D) / 2))) * 100.00;
                if (Double.IsNaN(staffTurnOver))
                {
                    staffTurnOver = 0;
                }


                staffStrength = Math.Round(staffStrength, 2);
                staffTurnOver = Math.Round(staffTurnOver, 2);


                dr["STAFF_STRENGTH"] = staffStrength;
                dr["STAFF_TURN_OVER"] = staffTurnOver;
                dtStaffStrength.Rows.Add(dr);

            }

            //Remove Empty Rows (Inctive Departments,..)
            for (int j = dtStaffStrength.Rows.Count - 1; j >= 0; j--)
            {
                DataRow dr2 = dtStaffStrength.Rows[j];
                if ((dr2["OPENING_HEAD_COUNT"].ToString() == "0") && (dr2["CLOSING_HEAD_COUNT"].ToString() == "0"))
                {
                    dr2.Delete();
                }
            }
            return dtStaffStrength;
        }



        public DataTable populaterep0030All()
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, C.COMP_NAME, CB.BRANCH_NAME, DT.DEPT_NAME, DV.DIV_NAME, ER.ROLE_NAME, ED.DESIGNATION_NAME, E.EPF_NO, E.INITIALS_NAME
                                    FROM 
                                        EMPLOYEE E, COMPANY C, COMPANY_BRANCH CB, DEPARTMENT DT, DIVISION DV, EMPLOYEE_ROLE ER, EMPLOYEE_DESIGNATION ED
                                    WHERE 
                                        E.DEPT_ID = DT.DEPT_ID AND E.BRANCH_ID = CB.BRANCH_ID AND E.DIVISION_ID = DV.DIVISION_ID AND E.COMPANY_ID = C.COMPANY_ID AND 
                                        ER.ROLE_ID = E.ROLE_ID AND E.DESIGNATION_ID = ED.DESIGNATION_ID
 AND
((E.EMPLOYEE_ID IN (SELECT EMPLOYEE_ID FROM PREVIOUS_EMPLOYEMENT)) OR (E.EMPLOYEE_ID IN (SELECT HE.EMPLOYEE_ID FROM HIGHER_EDUCATION HE WHERE HE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')) OR
(E.EMPLOYEE_ID IN (SELECT SE.EMPLOYEE_ID FROM SECONDARY_EDUCATION SE WHERE SE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')))

;
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

        public DataTable populaterep0030IND(string empID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, C.COMP_NAME, CB.BRANCH_NAME, DT.DEPT_NAME, DV.DIV_NAME, ER.ROLE_NAME, ED.DESIGNATION_NAME, E.EPF_NO, E.INITIALS_NAME
                                    FROM 
                                        EMPLOYEE E, COMPANY C, COMPANY_BRANCH CB, DEPARTMENT DT, DIVISION DV, EMPLOYEE_ROLE ER, EMPLOYEE_DESIGNATION ED
                                    WHERE 
                                        E.DEPT_ID = DT.DEPT_ID AND E.BRANCH_ID = CB.BRANCH_ID AND E.DIVISION_ID = DV.DIVISION_ID AND E.COMPANY_ID = C.COMPANY_ID AND 
                                        ER.ROLE_ID = E.ROLE_ID AND E.DESIGNATION_ID = ED.DESIGNATION_ID AND E.EMPLOYEE_ID = '" + empID + @"';
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

        public DataTable populaterep0030Department(string deptID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, C.COMP_NAME, CB.BRANCH_NAME, DT.DEPT_NAME, DV.DIV_NAME, ER.ROLE_NAME, ED.DESIGNATION_NAME, E.EPF_NO, E.INITIALS_NAME
                                    FROM 
                                        EMPLOYEE E, COMPANY C, COMPANY_BRANCH CB, DEPARTMENT DT, DIVISION DV, EMPLOYEE_ROLE ER, EMPLOYEE_DESIGNATION ED
                                    WHERE 
                                        E.DEPT_ID = DT.DEPT_ID AND E.BRANCH_ID = CB.BRANCH_ID AND E.DIVISION_ID = DV.DIVISION_ID AND E.COMPANY_ID = C.COMPANY_ID AND 
                                        ER.ROLE_ID = E.ROLE_ID AND E.DESIGNATION_ID = ED.DESIGNATION_ID AND DT.DEPT_ID = '" + deptID + @"'
 AND
((E.EMPLOYEE_ID IN (SELECT EMPLOYEE_ID FROM PREVIOUS_EMPLOYEMENT)) OR (E.EMPLOYEE_ID IN (SELECT HE.EMPLOYEE_ID FROM HIGHER_EDUCATION HE WHERE HE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')) OR
(E.EMPLOYEE_ID IN (SELECT SE.EMPLOYEE_ID FROM SECONDARY_EDUCATION SE WHERE SE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')))

;
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

        public DataTable populaterep0030Company(string companyID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, C.COMP_NAME, CB.BRANCH_NAME, DT.DEPT_NAME, DV.DIV_NAME, ER.ROLE_NAME, ED.DESIGNATION_NAME, E.EPF_NO, E.INITIALS_NAME
                                    FROM 
                                        EMPLOYEE E, COMPANY C, COMPANY_BRANCH CB, DEPARTMENT DT, DIVISION DV, EMPLOYEE_ROLE ER, EMPLOYEE_DESIGNATION ED
                                    WHERE 
                                        E.DEPT_ID = DT.DEPT_ID AND E.BRANCH_ID = CB.BRANCH_ID AND E.DIVISION_ID = DV.DIVISION_ID AND E.COMPANY_ID = C.COMPANY_ID AND 
                                        ER.ROLE_ID = E.ROLE_ID AND E.DESIGNATION_ID = ED.DESIGNATION_ID AND C.COMPANY_ID = '" + companyID + @"'
 AND
((E.EMPLOYEE_ID IN (SELECT EMPLOYEE_ID FROM PREVIOUS_EMPLOYEMENT)) OR (E.EMPLOYEE_ID IN (SELECT HE.EMPLOYEE_ID FROM HIGHER_EDUCATION HE WHERE HE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')) OR
(E.EMPLOYEE_ID IN (SELECT SE.EMPLOYEE_ID FROM SECONDARY_EDUCATION SE WHERE SE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"')))

;
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

        public DataTable populaterep0030PreviousExperience(string empID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, PE.ORGANIZATION, PE.DESIGNATION, CONVERT(PE.FROM_DATE,CHAR) AS 'PE_FROM_DATE',
                                        CONVERT(PE.TO_DATE,CHAR) AS 'PE_TO_DATE', TIMESTAMPDIFF(MONTH,PE.FROM_DATE,PE.TO_DATE) AS 'PE_DURATION'
                                    FROM 
                                        EMPLOYEE E, PREVIOUS_EMPLOYEMENT PE
                                    WHERE 
                                        PE.EMPLOYEE_ID = E.EMPLOYEE_ID AND E.EMPLOYEE_ID = '" + empID + @"';
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

        public DataTable populaterep0030HigherEducation(string empID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, HE.PROGRAM,HE.INSTITUTE, CONVERT(HE.FROM_YEAR,CHAR) AS 'HE_FROM_YEAR', 
                                        CONVERT(HE.TO_YEAR,CHAR) AS 'HE_TO_YEAR', HE.DURATION
                                    FROM 
                                        EMPLOYEE E, HIGHER_EDUCATION HE
                                    WHERE 
                                        HE.EMPLOYEE_ID = E.EMPLOYEE_ID AND E.EMPLOYEE_ID = '" + empID + @"' AND HE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
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

        public DataTable populaterep0030SecondaryEducation(string empID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID, (CASE (SE.IS_AL) WHEN 'Y' THEN 'Advanced Aevel' WHEN 'N' THEN 'Ordinary Level' END) AS'IS_AL', 
                                        SE.ATTEMPT, SE.ATTEMPTED_YEAR, SE.SUBJECT_NAME, SE.GRADE 
                                    FROM 
                                        EMPLOYEE E, SECONDARY_EDUCATION SE
                                    WHERE 
                                        SE.EMPLOYEE_ID = E.EMPLOYEE_ID AND E.EMPLOYEE_ID = '" + empID + @"' AND SE.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
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

        //ETI OT PROCESS CHATHURA NAWAGAMUWA 2017/09/11

        public DataTable populaterep0031ETI_OT_ALL(string FromDate, string ToDate, string CompanyID)
        {
            DataTable dtOTRecords = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("FROM_YEARMONTH", FromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("TO_YEARMONTH", ToDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("COMPANY", CompanyID));

                mySqlCmd.CommandText = "ETI_OT_REPORT_ALL";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtOTRecords);  
                
                return dtOTRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection.Close();
                dtOTRecords = null;
            }
        }

        public DataTable populaterep0031ETI_OT_BRANCH(string FromDate, string ToDate, string CompanyID, string BranchID)
        {
            DataTable dtOTRecords = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("FROM_YEARMONTH", FromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("TO_YEARMONTH", ToDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("COMPANY", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("BRANCH", BranchID));

                mySqlCmd.CommandText = "ETI_OT_REPORT_BRANCH";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtOTRecords);

                return dtOTRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection.Close();
                dtOTRecords = null;
            }
        }

        public DataTable populaterep0031ETI_OT_DEPARTMENT(string FromDate, string ToDate, string CompanyID, string DepartmentID)
        {
            DataTable dtOTRecords = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("FROM_YEARMONTH", FromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("TO_YEARMONTH", ToDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("COMPANY", CompanyID));
                mySqlCmd.Parameters.Add(new MySqlParameter("DEPARTMENT", DepartmentID));

                mySqlCmd.CommandText = "ETI_OT_REPORT_DEPARTMENT";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtOTRecords);

                return dtOTRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection.Close();
                dtOTRecords = null;
            }
        }

        public DataTable populaterep0031ETI_OT_INDIVIDUAL(string FromDate, string ToDate, string EmployeeID)
        {
            DataTable dtOTRecords = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("FROM_YEARMONTH", FromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("TO_YEARMONTH", ToDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("EMPLOYEE", EmployeeID));

                mySqlCmd.CommandText = "ETI_OT_REPORT_INDIVIDUAL";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtOTRecords);

                return dtOTRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection.Close();
                dtOTRecords = null;
            }
        }

        //

        //Over time process
        public DataTable populaterep0031Overtime(string sCOMPANY_ID, string fromdate, string todate, string empCode, string depCode)
        {
            string stamp = depCode.Trim();
            if (stamp != "")
            {
                stamp = stamp[0].ToString() + stamp[1].ToString();
            }

            string sMySqlString = "";
            dataTable = new DataTable();
            if (empCode != "")
            {
                sCOMPANY_ID = getEmployeeCompanyID(empCode);
            }

            try
            {
                dataTable.Rows.Clear();

                if (sCOMPANY_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (empCode == "")
                    {
                        //all company
                        sMySqlString = "sp_OverTimeProcessForAllCompany";
                    }
                    else
                    {
                        //individual
                        //sCOMPANY_ID = getEmployeeCompanyID(empCode);
                        //sMySqlString = "sp_OvertimeByIndividual";
                    }
                }
                else
                {
                    if (empCode == "")
                    {
                        if (depCode == "")
                        {
                            //for company
                            sMySqlString = "sp_CompanyOvertime";
                        }
                        else
                        {
                            stamp = depCode.Trim();
                            if (stamp != "")
                            {
                                stamp = stamp[0].ToString() + stamp[1].ToString();
                            }

                            if (stamp == Constants.DEPARTMENT_ID_STAMP)
                            {
                                //by department
                                sMySqlString = "sp_overTimeByDepartment";
                            }
                            else
                            {   //by Branch
                                sMySqlString = "sp_overTimeByBranch";
                            }
                        }
                        //}
                    }
                    else
                    {
                        // individual
                        sMySqlString = "sp_OvertimeByIndividual";
                    }
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("dept", depCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("empId", empCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("company", sCOMPANY_ID));
                mySqlCmd.Parameters.Add(new MySqlParameter("sdate", fromdate));
                mySqlCmd.Parameters.Add(new MySqlParameter("eDate", todate));
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Attendance Transfer Log
        public DataTable populaterep0032AllCompany(string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        ML.BRAND_NAME,
                                        CONVERT(ATL.ATT_DATE,CHAR) AS 'ATT_DATE',
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        ATL.IP_ADDRESS,
                                        CASE 
		                                    WHEN ATL.CONNECTION_STATUS = '1' THEN 'Connected'
		                                    WHEN ATL.CONNECTION_STATUS = '0' THEN 'Connection Failed'
	                                    END AS 'CONNECTION_STATUS',
                                        ATL.RECORDS_FOUND,
                                        ATL.RECORDS_INSERTED,                                        
                                        CASE 
		                                    WHEN ATL.EMAIL_SENT = '1' THEN 'Yes'
		                                    WHEN ATL.EMAIL_SENT = '0' THEN 'No'
	                                    END AS 'EMAIL_SENT',
                                        ATL.LOG_ID,
                                        ATL.TRANSFERED_TIME
                                    FROM
                                        ATTENDANCE_TRANSFER_LOG ATL,
                                        MACHINE_LOCATION ML,
                                        COMPANY_BRANCH CB,
                                        COMPANY C
                                    WHERE
                                        ML.id = ATL.MACHINE_ID
                                            AND CB.BRANCH_ID = ML.LOCATION
                                            AND C.COMPANY_ID = CB.COMPANY_ID
                                            AND (ATL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
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

        public DataTable populaterep0032Company(string FromDate, string ToDate, string CompanyID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        ML.BRAND_NAME,
                                        CONVERT(ATL.ATT_DATE,CHAR) AS 'ATT_DATE',
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        ATL.IP_ADDRESS,
                                        CASE 
		                                    WHEN ATL.CONNECTION_STATUS = '1' THEN 'Connected'
		                                    WHEN ATL.CONNECTION_STATUS = '0' THEN 'Connection Failed'
	                                    END AS 'CONNECTION_STATUS',
                                        ATL.RECORDS_FOUND,
                                        ATL.RECORDS_INSERTED,                                        
                                        CASE 
		                                    WHEN ATL.EMAIL_SENT = '1' THEN 'Yes'
		                                    WHEN ATL.EMAIL_SENT = '0' THEN 'No'
	                                    END AS 'EMAIL_SENT',
                                        ATL.LOG_ID,
                                        ATL.TRANSFERED_TIME
                                    FROM
                                        ATTENDANCE_TRANSFER_LOG ATL,
                                        MACHINE_LOCATION ML,
                                        COMPANY_BRANCH CB,
                                        COMPANY C
                                    WHERE
                                        ML.id = ATL.MACHINE_ID
                                            AND CB.BRANCH_ID = ML.LOCATION
                                            AND C.COMPANY_ID = CB.COMPANY_ID
                                            AND (ATL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') AND CB.COMPANY_ID = '" + CompanyID + @"'
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

        public DataTable populaterep0032Branch(string FromDate, string ToDate, string BranchID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        ML.BRAND_NAME,
                                        CONVERT(ATL.ATT_DATE,CHAR) AS 'ATT_DATE',
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        ATL.IP_ADDRESS,
                                        CASE 
		                                    WHEN ATL.CONNECTION_STATUS = '1' THEN 'Connected'
		                                    WHEN ATL.CONNECTION_STATUS = '0' THEN 'Connection Failed'
	                                    END AS 'CONNECTION_STATUS',
                                        ATL.RECORDS_FOUND,
                                        ATL.RECORDS_INSERTED,                                        
                                        CASE 
		                                    WHEN ATL.EMAIL_SENT = '1' THEN 'Yes'
		                                    WHEN ATL.EMAIL_SENT = '0' THEN 'No'
	                                    END AS 'EMAIL_SENT',
                                        ATL.LOG_ID,
                                        ATL.TRANSFERED_TIME
                                    FROM
                                        ATTENDANCE_TRANSFER_LOG ATL,
                                        MACHINE_LOCATION ML,
                                        COMPANY_BRANCH CB,
                                        COMPANY C
                                    WHERE
                                        ML.id = ATL.MACHINE_ID
                                            AND CB.BRANCH_ID = ML.LOCATION
                                            AND C.COMPANY_ID = CB.COMPANY_ID
                                            AND (ATL.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"') AND CB.BRANCH_ID = '" + BranchID + @"'
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

        //Row Attendance
        public DataTable populaterep0033AllCompany(string FromDate, string ToDate)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID,
                                        C.COMPANY_ID,
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        D.DEPT_NAME,
                                        E.EPF_NO,
                                        CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                        CONVERT( ATT.ATT_DATE , CHAR) AS 'ATT_DATE',
                                        CONVERT( ATT.ATT_TIME , CHAR) AS 'ATT_TIME',
                                        CONVERT( ATT.LOGTIME , CHAR) AS 'LOGTIME',
                                        ATT.DIRECTION,
										CASE WHEN ATT.DIRECTION = '1' THEN 'IN' WHEN ATT.DIRECTION = '0' THEN 'OUT' END AS 'DIRECTION_NAME'
                                    FROM
                                        ATTENDANCE ATT,
                                        COMPANY C,
                                        COMPANY_BRANCH CB,
                                        EMPLOYEE E,
                                        DEPARTMENT D
                                    WHERE
                                        ATT.COMPANY_ID = C.COMPANY_ID
                                            AND ATT.BRANCH_ID = CB.BRANCH_ID
                                            AND E.EMPLOYEE_ID = ATT.EMPLOYEE_ID
                                            AND E.DEPT_ID = D.DEPT_ID
                                            AND (ATT.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + FromDate + @"')
                                    ORDER BY ATT.COMPANY_ID , ATT_DATE ASC , ATT.ATT_TIME ASC
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

        public DataTable populaterep0033Company(string FromDate, string ToDate, string CompanyID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();

                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID,
                                        C.COMPANY_ID,
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        D.DEPT_NAME,
                                        E.EPF_NO,
                                        CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                        CONVERT( ATT.ATT_DATE , CHAR) AS 'ATT_DATE',
                                        CONVERT( ATT.ATT_TIME , CHAR) AS 'ATT_TIME',
                                        CONVERT( ATT.LOGTIME , CHAR) AS 'LOGTIME',
                                        ATT.DIRECTION,
										CASE WHEN ATT.DIRECTION = '1' THEN 'IN' WHEN ATT.DIRECTION = '0' THEN 'OUT' END AS 'DIRECTION_NAME'
                                    FROM
                                        ATTENDANCE ATT,
                                        COMPANY C,
                                        COMPANY_BRANCH CB,
                                        EMPLOYEE E,
                                        DEPARTMENT D
                                    WHERE
                                        ATT.COMPANY_ID = C.COMPANY_ID
                                            AND ATT.BRANCH_ID = CB.BRANCH_ID
                                            AND E.EMPLOYEE_ID = ATT.EMPLOYEE_ID
                                            AND E.DEPT_ID = D.DEPT_ID
                                            AND (ATT.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
		                                    AND ATT.COMPANY_ID = '" + CompanyID + @"'
                                    ORDER BY ATT.COMPANY_ID , ATT_DATE ASC , ATT.ATT_TIME ASC
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

        public DataTable populaterep0033Branch(string FromDate, string ToDate, string BranchID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID,
                                        C.COMPANY_ID,
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        D.DEPT_NAME,
                                        E.EPF_NO,
                                        CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                        CONVERT( ATT.ATT_DATE , CHAR) AS 'ATT_DATE',
                                        CONVERT( ATT.ATT_TIME , CHAR) AS 'ATT_TIME',
                                        CONVERT( ATT.LOGTIME , CHAR) AS 'LOGTIME',
                                        ATT.DIRECTION,
										CASE WHEN ATT.DIRECTION = '1' THEN 'IN' WHEN ATT.DIRECTION = '0' THEN 'OUT' END AS 'DIRECTION_NAME'
                                    FROM
                                        ATTENDANCE ATT,
                                        COMPANY C,
                                        COMPANY_BRANCH CB,
                                        EMPLOYEE E,
                                        DEPARTMENT D
                                    WHERE
                                        ATT.COMPANY_ID = C.COMPANY_ID
                                            AND ATT.BRANCH_ID = CB.BRANCH_ID
                                            AND E.EMPLOYEE_ID = ATT.EMPLOYEE_ID
                                            AND E.DEPT_ID = D.DEPT_ID
                                            AND (ATT.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
		                                    AND ATT.BRANCH_ID = '" + BranchID + @"'
                                    ORDER BY ATT.COMPANY_ID , ATT_DATE ASC , ATT.ATT_TIME ASC
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

        public DataTable populaterep0033Department(string FromDate, string ToDate, string DepartmentID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID,
                                        C.COMPANY_ID,
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        D.DEPT_NAME,
                                        E.EPF_NO,
                                        CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                        CONVERT( ATT.ATT_DATE , CHAR) AS 'ATT_DATE',
                                        CONVERT( ATT.ATT_TIME , CHAR) AS 'ATT_TIME',
                                        CONVERT( ATT.LOGTIME , CHAR) AS 'LOGTIME',
                                        ATT.DIRECTION,
										CASE WHEN ATT.DIRECTION = '1' THEN 'IN' WHEN ATT.DIRECTION = '0' THEN 'OUT' END AS 'DIRECTION_NAME'
                                    FROM
                                        ATTENDANCE ATT,
                                        COMPANY C,
                                        COMPANY_BRANCH CB,
                                        EMPLOYEE E,
                                        DEPARTMENT D
                                    WHERE
                                        ATT.COMPANY_ID = C.COMPANY_ID
                                            AND ATT.BRANCH_ID = CB.BRANCH_ID
                                            AND E.EMPLOYEE_ID = ATT.EMPLOYEE_ID
                                            AND E.DEPT_ID = D.DEPT_ID
                                            AND (ATT.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
		                                    AND E.DEPT_ID = '" + DepartmentID + @"'
                                    ORDER BY ATT.COMPANY_ID , ATT_DATE ASC , ATT.ATT_TIME ASC
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

        public DataTable populaterep0033Employee(string FromDate, string ToDate, string EmployeeID)
        {
            string sMySqlString = "";
            dataTable = new DataTable();
            try
            {
                dataTable = new DataTable();


                sMySqlString = @"
                                    SELECT 
                                        E.EMPLOYEE_ID,
                                        C.COMPANY_ID,
                                        C.COMP_NAME,
                                        CB.BRANCH_NAME,
                                        D.DEPT_NAME,
                                        E.EPF_NO,
                                        CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                        CONVERT( ATT.ATT_DATE , CHAR) AS 'ATT_DATE',
                                        CONVERT( ATT.ATT_TIME , CHAR) AS 'ATT_TIME',
                                        CONVERT( ATT.LOGTIME , CHAR) AS 'LOGTIME',
                                        ATT.DIRECTION,
										CASE WHEN ATT.DIRECTION = '1' THEN 'IN' WHEN ATT.DIRECTION = '0' THEN 'OUT' END AS 'DIRECTION_NAME'
                                    FROM
                                        ATTENDANCE ATT,
                                        COMPANY C,
                                        COMPANY_BRANCH CB,
                                        EMPLOYEE E,
                                        DEPARTMENT D
                                    WHERE
                                        ATT.COMPANY_ID = C.COMPANY_ID
                                            AND ATT.BRANCH_ID = CB.BRANCH_ID
                                            AND E.EMPLOYEE_ID = ATT.EMPLOYEE_ID
                                            AND E.DEPT_ID = D.DEPT_ID
                                            AND (ATT.ATT_DATE BETWEEN '" + FromDate + @"' AND '" + ToDate + @"')
		                                    AND ATT.EMPLOYEE_ID = '" + EmployeeID + @"'
                                    ORDER BY ATT.COMPANY_ID , ATT_DATE ASC , ATT.ATT_TIME ASC
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

        //Training Needs Report
        public DataTable getTrainingRequest(string companyId, string departmentId, string divisionId, string branchId, string financialYear, 
            string employeeId, string fromDate, string toDate, string departmentHeadStatus, string ceoStatus)
        {
            try
            {
                DataTable resultTable = new DataTable();

                string sqlString = @"SELECT 
                                            TR.REQUEST_ID,
                                            TR.TRAINING_CATEGORY,
	                                        TC.CATEGORY_NAME,
                                            TR.TRAINING_SUB_CATEGORY_ID,
	                                        TSC.TYPE_NAME AS SUB_CATEGORY_NAME,
                                            TR.COMPANY_ID,
	                                        C.COMP_NAME AS COMPANY_NAME,
                                            TR.DEPARTMENT_ID,
	                                        D.DEPT_NAME,
                                            TR.DIVISION_ID,
	                                        DI.DIV_NAME AS DIVISION_NAME,
                                            TR.BRANCH_ID,
	                                        BR.BRANCH_NAME,
                                            TR.REQUEST_TYPE,
	                                        RT.TYPE_NAME,
                                            TR.REQUESTED_BY,
                                            TR.DESIGNATION,
                                            TR.EMAIL,
                                            TR.REASON,
                                            TR.DESCRIPTION_OF_TRAINING,
                                            TR.SKILLS_EXPECTED,
                                            CONVERT( NUMBER_OF_PARTICIPANTS , char (10)) as NUMBER_OF_PARTICIPANTS,
                                            DATE_FORMAT(REQUESTED_DATE, '%Y/%m/%d') as REQUESTED_DATE,
                                            TR.REMARKS,
                                            TR.TO_RECOMMEND,
                                            TR.TO_APPROVE,
                                            DATE_FORMAT(RECOMENDED_DATE, '%Y/%m/%d') as RECOMENDED_DATE,
                                            TR.RECOMENDED_REASON,
                                            TR.IS_RECOMENDED,
                                            TR.APPROVED_BY,
                                            DATE_FORMAT(APPROVED_DATE, '%Y/%m/%d') as APPROVED_DATE,
                                            TR.APPROVED_REASON,
                                            TR.IS_APPROVED,
                                            TR.FINANCIAL_YEAR,
                                            TR.STATUS_CODE
                                        FROM
                                            TRAINING_REQUEST TR
                                        LEFT JOIN
	                                        TRAINING_CATEGORY TC ON TR.TRAINING_CATEGORY = TC.TRAINING_CATEGORY_ID
                                        LEFT JOIN
	                                        TRAINING_SUB_CATEGORY TSC ON TR.TRAINING_SUB_CATEGORY_ID = TSC.TYPE_ID
                                        LEFT JOIN
	                                        COMPANY C ON TR.COMPANY_ID = C.COMPANY_ID
                                        LEFT JOIN
	                                        DEPARTMENT D ON TR.DEPARTMENT_ID = D.DEPT_ID
                                        LEFT JOIN
	                                        DIVISION DI ON TR.DIVISION_ID = DI.DIVISION_ID
                                        LEFT JOIN
	                                        COMPANY_BRANCH BR ON TR.BRANCH_ID = BR.BRANCH_ID
                                        LEFT JOIN
	                                        REQUEST_TYPE RT ON TR.REQUEST_TYPE = RT.REQUEST_TYPE_ID 
                                        WHERE 
                                            TR.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                if (!String.IsNullOrEmpty(companyId))
                {
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        sqlString += " AND TR.COMPANY_ID = '" + companyId + "' ";
                    }
                }
                if (!String.IsNullOrEmpty(departmentId))
                {
                    sqlString += " AND TR.DEPARTMENT_ID = '" + departmentId + "' ";
                }
                if (!String.IsNullOrEmpty(branchId))
                {
                    sqlString += " AND TR.BRANCH_ID = '" + branchId + "' ";
                }
                if (!String.IsNullOrEmpty(divisionId))
                {
                    sqlString += " AND TR.DIVISION_ID = '" + divisionId + "' ";
                }
                if (!String.IsNullOrEmpty(financialYear))
                {
                    sqlString += " AND TR.FINANCIAL_YEAR = '" + financialYear + "' ";
                }
                if (!String.IsNullOrEmpty(employeeId))
                {
                    sqlString += " AND TR.REQUESTED_BY ='" +employeeId+ "' ";
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    sqlString += " AND TR.ADDED_DATE >= '" + fromDate + "' ";
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    sqlString += " AND TR.ADDED_DATE <='" + toDate + "' ";
                }
                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    sqlString += " AND TR.IS_RECOMENDED ='" + departmentHeadStatus + "' ";
                }
                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    sqlString += " AND TR.IS_APPROVED ='" + ceoStatus + "' ";
                }

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sqlString, mySqlCon);

                mySqlDa.Fill(resultTable);

                mySqlDa.Dispose();
                mySqlCon.Close();

                return resultTable;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public DataTable getAllTainingReports()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlString = @"   SELECT idno, repcode, description FROM REPORTS
                                        WHERE status ='" + Constants.CON_ACTIVE_STATUS + "' AND REPORT_GROUP ='" + Constants.CON_REPORT_GROUP_TRAINING_REPORTS+"' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                mySqlDataAdapter.Dispose();

                return resultTable;                
            }
            catch (Exception)
            {

                throw;
            }
            finally 
            { 
                resultTable.Dispose();
                mySqlCon.Close();
                
            }
        }

        public DataTable populateDivisions(string selectedDepartment)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                DIVISION_ID, DIV_NAME
                                            FROM
                                                DIVISION
                                            WHERE
                                                DEPT_ID = '" + selectedDepartment + @"' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                            
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

        public DataTable populateDivisionName(string divisionId)
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
                                                DIVISION_ID = '" + divisionId + @"';
                                            
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

        /// <summary>
        /// Training Schedule Report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="divisionId"></param>
        /// <param name="branchId"></param>
        /// <param name="financialYear"></param>
        /// <param name="employeeId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="departmentHeadStatus"></param>
        /// <param name="ceoStatus"></param>
        /// <returns></returns>
        public DataTable getTrainingSchedules(string companyId, string departmentId, string divisionId, string branchId, string financialYear, string employeeId, string fromDate, string toDate, string departmentHeadStatus, string ceoStatus)
        {
            try
            {
                DataTable resultTable = new DataTable();

                string sqlString = @"SELECT 
                                        TS.TRAINING_ID,
                                        T.TRAINING_NAME,
                                        T.TRAINING_PROGRAM_ID,
                                        TP.PROGRAM_NAME,
                                        TC.COMPANY_ID,
                                        C.COMP_NAME,
                                        CAST(TS.PLANNED_SCHEDULE_DATE AS CHAR) AS PLANNED_SCHEDULE_DATE ,
                                        CAST(TS.ACTUAL_DATE AS CHAR) AS ACTUAL_DATE,
                                        TS.PLANNED_FROM_TIME,
                                        TS.PLANNED_TO_TIME,
                                        TS.LOCATION_ID, 
                                        TS.ACTUAL_FROM_TIME,
                                        TS.ACTUAL_TO_TIME, 
                                        TS.STATUS_CODE

                                        FROM TRAINING_SCHEDULE TS
                                        LEFT JOIN TRAINING T
                                            ON TS.TRAINING_ID = T.TRAINING_ID

                                        LEFT JOIN TRAINING_PROGRAM TP
                                            ON T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID

                                        LEFT JOIN TRAINING_COMPANY TC
                                            ON TS.TRAINING_ID = TC.TRAINING_ID

                                        LEFT JOIN COMPANY C
                                            ON TC.COMPANY_ID = C.COMPANY_ID

                                        LEFT JOIN TRAINING_PARTICIPANTS TPAR
                                            ON TS.TRAINING_ID = TPAR.TRAINING_ID

                                        WHERE 
                                            TS.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                if (!String.IsNullOrEmpty(companyId))
                {
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        sqlString += " AND TC.COMPANY_ID = '" + companyId + "' ";
                    }
                }
                if (!String.IsNullOrEmpty(departmentId))
                {
                    sqlString += " AND TPAR.DEPARTMENT_ID = '" + departmentId + "' ";
                }
                if (!String.IsNullOrEmpty(branchId))
                {
                    sqlString += " AND TPAR.BRANCH_ID = '" + branchId + "' ";
                }
                if (!String.IsNullOrEmpty(divisionId))
                {
                    sqlString += " AND TPAR.DIVISION_ID = '" + divisionId + "' ";
                }
                if (!String.IsNullOrEmpty(financialYear))
                {
                    sqlString += " AND TR.FINANCIAL_YEAR = '" + financialYear + "' ";
                }
                if (!String.IsNullOrEmpty(employeeId))
                {
                    sqlString += " AND TR.REQUESTED_BY ='" + employeeId + "' ";
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    sqlString += " AND T.PLANNED_START_DATE >= '" + fromDate + "' ";
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    sqlString += " AND T.PLANNED_END_DATE <='" + toDate + "' ";

                }
                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    sqlString += " AND TR.IS_RECOMENDED ='" + departmentHeadStatus + "' ";
                }
                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    sqlString += " AND TR.IS_APPROVED ='" + ceoStatus + "' ";
                }

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sqlString, mySqlCon);

                mySqlDa.Fill(resultTable);

                mySqlDa.Dispose();
                mySqlCon.Close();

                return resultTable;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        /// <summary>
        /// Completed Training Report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="divisionId"></param>
        /// <param name="branchId"></param>
        /// <param name="financialYear"></param>
        /// <param name="employeeId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="departmentHeadStatus"></param>
        /// <param name="ceoStatus"></param>
        /// <returns></returns>
        public DataTable getCompletedTraining(string companyId, string departmentId, string divisionId, string branchId, string financialYear, string employeeId, string fromDate, string toDate, string departmentHeadStatus, string ceoStatus)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlString = @"SELECT 
                                        T.TRAINING_ID, T.TRAINING_NAME, T.TRAINING_PROGRAM_ID, TPR.PROGRAM_NAME, TP.COMPANY_ID, C.COMP_NAME, 
	                                    TP.DEPARTMENT, D.DEPT_NAME, TC.ACTUAL_PARTICIPANTS, TC.PLANNED_PARTICIPANTS,
	                                    CAST(T.PLANNED_START_DATE AS CHAR) AS PLANNED_START_DATE,
                                        CAST(T.ACTUAL_START_DATE AS CHAR) AS ACTUAL_START_DATE, 
                                        CAST(T.PLANNED_END_DATE AS CHAR) AS PLANNED_END_DATE,
                                        CAST(T.ACTUAL_END_DATE AS CHAR) AS ACTUAL_END_DATE,
                                        T.PLANNED_TOTAL_HOURS, T.ACTUAL_TOTAL_HOURS
                                    FROM
	                                    TRAINING_PARTICIPANTS TP
		                                    LEFT JOIN
	                                    TRAINING T ON TP.TRAINING_ID = T.TRAINING_ID
		                                    LEFT JOIN
	                                    COMPANY C ON TP.COMPANY_ID = C.COMPANY_ID
		                                    LEFT JOIN
	                                    DEPARTMENT D ON TP.DEPARTMENT = D.DEPT_ID
		                                    LEFT JOIN
	                                    TRAINING_PROGRAM TPR ON T.TRAINING_PROGRAM_ID = TPR.PROGRAM_ID
		                                    LEFT JOIN
	                                    TRAINING_COMPANY TC ON TP.COMPANY_ID = TC.COMPANY_ID AND TP.TRAINING_ID = TC.TRAINING_ID
                                    WHERE
	                                    T.STATUS_CODE = '" + Constants.CON_TRAINING_COMPLETED_VALUE + "' ";
                                    

                if (!String.IsNullOrEmpty(companyId))
                {
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        sqlString += " AND TP.COMPANY_ID = '" + companyId + "' ";
                    }
                }
                if (!String.IsNullOrEmpty(departmentId))
                {
                    sqlString += " AND TP.DEPARTMENT = '" + departmentId + "' ";
                }
                if (!String.IsNullOrEmpty(branchId))
                {
                    sqlString += " AND TP.BRANCH = '" + branchId + "' ";
                }
                if (!String.IsNullOrEmpty(divisionId))
                {
                    sqlString += " AND TP.DIVISION = '" + divisionId + "' ";
                }
                //if (!String.IsNullOrEmpty(financialYear))
                //{
                //    sqlString += " AND TR.FINANCIAL_YEAR = '" + financialYear + "' ";
                //}
                if (!String.IsNullOrEmpty(employeeId))
                {
                    sqlString += " AND TP.EMPLOYEE_ID ='" + employeeId + "' ";
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    sqlString += " AND T.PLANNED_START_DATE >= '" + fromDate + "' ";
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    sqlString += " AND T.PLANNED_END_DATE <='" + toDate + "' ";

                }

                sqlString += "GROUP BY T.TRAINING_ID, TP.COMPANY_ID, TP.DEPARTMENT";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Pending Training Report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="divisionId"></param>
        /// <param name="branchId"></param>
        /// <param name="financialYear"></param>
        /// <param name="employeeId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="departmentHeadStatus"></param>
        /// <param name="ceoStatus"></param>
        /// <returns></returns>
        public DataTable getPendingTraining(string companyId, string departmentId, string divisionId, string branchId, string financialYear, string employeeId, string fromDate, string toDate, string departmentHeadStatus, string ceoStatus)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlString = @"SELECT 
                                        T.TRAINING_ID, T.TRAINING_NAME, T.TRAINING_PROGRAM_ID, TPR.PROGRAM_NAME, TP.COMPANY_ID, C.COMP_NAME, 
	                                    TP.DEPARTMENT, D.DEPT_NAME, TC.PLANNED_PARTICIPANTS,
	                                    CAST(T.PLANNED_START_DATE AS CHAR) AS PLANNED_START_DATE, 
                                        CAST(T.PLANNED_END_DATE AS CHAR) AS PLANNED_END_DATE,
                                        T.PLANNED_TOTAL_HOURS
                                    FROM
	                                    TRAINING_PARTICIPANTS TP
		                                    LEFT JOIN
	                                    TRAINING T ON TP.TRAINING_ID = T.TRAINING_ID
		                                    LEFT JOIN
	                                    COMPANY C ON TP.COMPANY_ID = C.COMPANY_ID
		                                    LEFT JOIN
	                                    DEPARTMENT D ON TP.DEPARTMENT = D.DEPT_ID
		                                    LEFT JOIN
	                                    TRAINING_PROGRAM TPR ON T.TRAINING_PROGRAM_ID = TPR.PROGRAM_ID
		                                    LEFT JOIN
	                                    TRAINING_COMPANY TC ON TP.COMPANY_ID = TC.COMPANY_ID AND TP.TRAINING_ID = TC.TRAINING_ID
                                    WHERE
	                                    T.STATUS_CODE = '" + Constants.CON_TRAINING_PENDING_VALUE + "' ";


                if (!String.IsNullOrEmpty(companyId))
                {
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        sqlString += " AND TP.COMPANY_ID = '" + companyId + "' ";
                    }
                }
                if (!String.IsNullOrEmpty(departmentId))
                {
                    sqlString += " AND TP.DEPARTMENT = '" + departmentId + "' ";
                }
                if (!String.IsNullOrEmpty(branchId))
                {
                    sqlString += " AND TP.BRANCH = '" + branchId + "' ";
                }
                if (!String.IsNullOrEmpty(divisionId))
                {
                    sqlString += " AND TP.DIVISION = '" + divisionId + "' ";
                }
                //if (!String.IsNullOrEmpty(financialYear))
                //{
                //    sqlString += " AND TR.FINANCIAL_YEAR = '" + financialYear + "' ";
                //}
                if (!String.IsNullOrEmpty(employeeId))
                {
                    sqlString += " AND TP.EMPLOYEE_ID ='" + employeeId + "' ";
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    sqlString += " AND T.PLANNED_START_DATE >= '" + fromDate + "' ";
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    sqlString += " AND T.PLANNED_END_DATE <='" + toDate + "' ";

                }

                sqlString += " GROUP BY T.TRAINING_ID, TP.COMPANY_ID, TP.DEPARTMENT ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;

            }
            catch (Exception)
            {

                throw;
            }
        }

        //Balance Budget Report

        public DataTable GetExpenseHeader(string TrainingID)
        {
            DataTable BalanceBudgetHeader = new DataTable();
            mySqlCmd = new MySqlCommand();
            MySqlDataAdapter mySqlDA;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string Query = @"
                                        SELECT DISTINCT
                                            T.TRAINING_NAME,
                                            T.TRAINING_CODE,
                                            EE.TOTAL_EXPENSE AS 'EXPECTED_TOTAL',
                                            EE.PER_PERSON_COST AS 'EXPECTED_PER_PERSON_COST',
                                            AE.TOTAL_EXPENSE AS 'ACTUAL_TOTAL_EXPENSE',
                                            AE.TOTAL_DISCOUNT AS 'ACTUAL_TOTAL_DISCOUNT',
                                            AE.GRAND_TOTAL AS 'ACTUAL_GRAND_TOTAL',
                                            AE.PER_PERSON_COST AS 'ACTUAL_PER_PERSON_COST'
                                        FROM
                                            TRAINING T,
                                            EXPECTED_EXPENSE EE,
                                            ACTUAL_EXPENSES AE
                                        WHERE
                                            T.TRAINING_ID = EE.TRAINING_ID
                                                AND EE.TRAINING_ID = AE.TRAINING_ID
                                                AND EE.TRAINING_ID = @TRAINING_ID
                                                AND AE.STATUS_CODE = @STATUS_CODE
                                                AND EE.STATUS_CODE = @STATUS_CODE
                                ";

                mySqlCmd.CommandText = Query;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlDA = new MySqlDataAdapter(mySqlCmd);
                mySqlDA.Fill(BalanceBudgetHeader);

                return BalanceBudgetHeader.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
                BalanceBudgetHeader.Dispose();
                mySqlDA = null;
            }
        }

        public DataTable GetExpectedExpenseDetails(string TrainingID)
        {
            DataTable ExpectedExpenseDetails = new DataTable();
            mySqlCmd = new MySqlCommand();
            MySqlDataAdapter mySqlDA;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string Query = @"
                                        SELECT 
                                            EC.CATEGORY_NAME, EED.AMOUNT, EED.DESCRIPTION, EED.REMARKS
                                        FROM
                                            EXPECTED_EXPENSE_DETAILS EED,
                                            EXPECTED_EXPENSE EE,
                                            EXPENSE_CATEGORY EC
                                        WHERE
                                            EED.EX_EXPENSE_ID = EE.EXPECTED_EXPENSE_ID
                                                AND EE.TRAINING_ID = @TRAINING_ID
                                                AND EE.STATUS_CODE = @STATUS_CODE
                                                AND EED.STATUS_CODE = @STATUS_CODE
                                                AND EED.EXPENSE_CATEGORY_ID = EC.EXPENSE_CATEGORY_ID
                                        ORDER BY EC.CATEGORY_NAME ASC
                                ";

                mySqlCmd.CommandText = Query;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlDA = new MySqlDataAdapter(mySqlCmd);
                mySqlDA.Fill(ExpectedExpenseDetails);

                return ExpectedExpenseDetails.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
                ExpectedExpenseDetails.Dispose();
                mySqlDA = null;
            }
        }

        public DataTable GetActualExpenseDetails(string TrainingID)
        {
            DataTable ActualExpenseDetails = new DataTable();
            mySqlCmd = new MySqlCommand();
            MySqlDataAdapter mySqlDA;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string Query = @"
                                        SELECT 
                                            EC.CATEGORY_NAME,
                                            AED.AMOUNT,
                                            AED.DISCOUNT,
                                            AED.NET_AMOUNT,
                                            CASE
                                                WHEN AED.IS_PAID = '" + Constants.CON_ACTIVE_STATUS + @"' THEN 'Yes'
                                                ELSE 'No'
                                            END AS 'IS_PAID',
                                            AED.PAYMENT_DESCRIPTION,
                                            AED.REMARKS
                                        FROM
                                            ACTUAL_EXPENSE_DETAILS AED,
                                            ACTUAL_EXPENSES AE,
                                            EXPENSE_CATEGORY EC
                                        WHERE
                                            AED.AC_EXPENSE_ID = AE.AC_EXPENSE_ID
                                                AND AE.TRAINING_ID = @TRAINING_ID
                                                AND AE.STATUS_CODE = @STATUS_CODE
                                                AND AED.STATUS_CODE = @STATUS_CODE
                                                AND AED.EXPENSE_CATEGORY_ID = EC.EXPENSE_CATEGORY_ID
                                        ORDER BY EC.CATEGORY_NAME ASC
                                ";

                mySqlCmd.CommandText = Query;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlDA = new MySqlDataAdapter(mySqlCmd);
                mySqlDA.Fill(ActualExpenseDetails);

                return ActualExpenseDetails.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
                ActualExpenseDetails.Dispose();
                mySqlDA = null;
            }
        }
    }
}