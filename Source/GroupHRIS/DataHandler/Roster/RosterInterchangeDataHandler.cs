using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;
using NLog;

namespace DataHandler.Roster
{
    public class RosterInterchangeDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();





        public bool isOverlappingExistForInterchange(string empIdOrig, string rosterIdOrig, string dateOrig, string fromTimeOrig, string toTimeOrig,
                                                     string empIdInter, string rosterIdInter, string dateInter, string fromTimeInter, string toTimeInter, string reason, string userId, string rosterType, string inerchangerRosterType)
        {
            bool bRetVal = false;

            EmpRosterAssignmentDataHandler dhEmpRoster = new EmpRosterAssignmentDataHandler();

            try
            {
                //---------------------------------------------------------------------------------------------
                //Check wheather original emp has o/l rosters for interchange date
                //---------------------------------------------------------------------------------------------
                if ((inerchangerRosterType == "1") && (dhEmpRoster.checkForOverlappingRostersRegular(empIdOrig, dateInter, fromTimeInter, toTimeInter)))
                    bRetVal = true;

                //---------------------------------------------------------------------------------------------
                //Check wheather interchanger emp has o/l rosters for orig date
                //---------------------------------------------------------------------------------------------
                else if ((rosterType == "1") && (dhEmpRoster.checkForOverlappingRostersRegular(empIdInter, dateOrig, fromTimeOrig, toTimeOrig)))
                    bRetVal = true;

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
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }

            return bRetVal;
        }



        public bool isOverNightOverlappingExistForInterchange(string empIdOrig, string rosterIdOrig, string dateOrig, string fromTimeOrig, string toTimeOrig,
                                                              string empIdInter, string rosterIdInter, string dateInter, string fromTimeInter, string toTimeInter, string reason, string userId, string rosterType, string inerchangerRosterType)
        {
            bool bRetVal = false;

            EmpRosterAssignmentDataHandler dhEmpRoster = new EmpRosterAssignmentDataHandler();

            try
            {
                //---------------------------------------------------------------------------------------------
                //Check wheather original emp has o/l rosters for interchange date
                //---------------------------------------------------------------------------------------------
                if ((inerchangerRosterType == "2") && (dhEmpRoster.checkForOverlappingRostersOverNight(empIdOrig, dateInter, fromTimeInter, toTimeInter)))
                    bRetVal = true;

                //---------------------------------------------------------------------------------------------
                //Check wheather interchanger emp has o/l rosters for orig date
                //---------------------------------------------------------------------------------------------
                else if ((inerchangerRosterType == "2") && (dhEmpRoster.checkForOverlappingRostersOverNight(empIdInter, dateOrig, fromTimeOrig, toTimeOrig)))
                    bRetVal = true;

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
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }

            return bRetVal;
        }




        public bool interchangeRoster(string empIdOrig, string rosterIdOrig, string dateOrig, string fromTimeOrig, string toTimeOrig,
                                      string empIdInter, string rosterIdInter, string dateInter, string fromTimeInter, string toTimeInter, string reason, string userId)
        {
            bool bRetVal    = false;
            bool bInserted  = false;
            bool bUpdated   = false;

            

            string interchangeNumber = "";

            MySqlTransaction mySqlTrans = null;
            SerialHandler hSerial = new SerialHandler();


            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();


                interchangeNumber = hSerial.getserila(mySqlCon, Constants.INTERCHANGE_ID_STAMP);

                //--------------------------------------------------------------------------
                //Update original employees record for the original date (Setting Inactive)
                //--------------------------------------------------------------------------

                bUpdated = updateRosterAssignment(mySqlCon,
                                        rosterIdOrig,
                                        empIdOrig,
                                        dateOrig,
                                        String.Empty,
                                        Constants.CON_NOT_SUMMARIZED,
                                        interchangeNumber,
                                        empIdInter,
                                        reason,
                                        userId,
                                        Constants.STATUS_INACTIVE_VALUE);

                
                //--------------------------------------------------------------------------
                //insert interchanger employees record for the original date (Active record)
                //--------------------------------------------------------------------------

                if (bUpdated)
                {
                    bInserted = insertRosterAssignment(mySqlCon,
                                            rosterIdOrig,
                                            empIdInter,
                                            dateOrig,
                                            String.Empty,
                                            Constants.CON_NOT_SUMMARIZED,
                                            interchangeNumber,
                                            String.Empty,
                                            reason,
                                            userId,
                                            Constants.STATUS_ACTIVE_VALUE);
                }


                //--------------------------------------------------------------------------------
                //Update interchanger employees record for the interchange date (Setting Inactive)
                //---------------------------------------------------------------------------------

                if ((bUpdated) && (bInserted))
                {
                    bUpdated = updateRosterAssignment(mySqlCon,
                                                    rosterIdInter,
                                                    empIdInter,
                                                    dateInter,
                                                    String.Empty,
                                                    Constants.CON_NOT_SUMMARIZED,
                                                    interchangeNumber,
                                                    empIdOrig,
                                                    reason,
                                                    userId,
                                                    Constants.STATUS_INACTIVE_VALUE);
                }

                //---------------------------------------------------------------------------
                //Insert original employees record for the interchange date  (Active record)
                //---------------------------------------------------------------------------
                if ((bUpdated) && (bInserted))
                {
                    bInserted = insertRosterAssignment(mySqlCon,
                                                        rosterIdInter,
                                                        empIdOrig,
                                                        dateInter,
                                                        String.Empty,
                                                        Constants.CON_NOT_SUMMARIZED,
                                                        interchangeNumber,
                                                        String.Empty,
                                                        reason,
                                                        userId,
                                                        Constants.STATUS_ACTIVE_VALUE);
                }


            }
            catch (Exception ex)
            {
                bInserted = false;
                bUpdated = false;

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {
                if ((!bInserted) || (!bUpdated))
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                hSerial = null;

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }

            return bRetVal;
        }

        private Boolean updateRosterAssignment(MySqlConnection mySqlCon_,
                                              String rosterID,
                                              String employeeID,
                                              String dutyDate,
                                              String isOffDay,
                                              String isSummarized,
                                              String interchangeNumber,
                                              String dutyCoverredBy,
                                              String reason,
                                              String userID,
                                              String statusCode)
        {
            Boolean bUpdated = false;

            MySqlCommand cmd = mySqlCon_.CreateCommand();
            string sMySqlString = "";


            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isOffDay", isOffDay.Trim() == "" ? (object)DBNull.Value : isOffDay.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyCoverredBy", dutyCoverredBy.Trim() == "" ? (object)DBNull.Value : dutyCoverredBy.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


            try
            {
                sMySqlString = " UPDATE EMPLOYEE_ROSTER_DATE " +
                                " SET " +
                                " STATUS_CODE = @statusCode, " +
                                " IS_OFF_DAY = @isOffDay, " +
                                " IS_SUMMARIZED = @isSummarized, " +
                                " INTERCHANGE_NUMBER = @interchangeNumber, " +
                                " DUTY_COVERED_BY = @dutyCoverredBy, " +
                                " REASON = @reason, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now()  " +
                                " WHERE ROSTR_ID = @rosterId AND EMPLOYEE_ID = @employeeID AND DUTY_DATE = @dutyDate ";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Dispose();

                bUpdated = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon_.State == ConnectionState.Open)
                {
                    mySqlCon_.Close();
                }

                throw ex;
            }
            finally
            {

            }
            return bUpdated;
        }
        

        private Boolean insertRosterAssignment(MySqlConnection mySqlCon_,
                                              String rosterID,
                                              String employeeID,
                                              String dutyDate,
                                              String isOffDay,
                                              String isSummarized,
                                              String interchangeNumber,
                                              String dutyCoverredBy,
                                              String reason,
                                              String userID,
                                              String statusCode)
        {
            Boolean bUpdated = false;

            MySqlCommand cmd = mySqlCon_.CreateCommand();
            string sMySqlString = "";


            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterID_", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID_", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate_", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isOffDay_", isOffDay.Trim() == "" ? (object)DBNull.Value : isOffDay.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized_", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber_", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyCoverredBy_", dutyCoverredBy.Trim() == "" ? (object)DBNull.Value : dutyCoverredBy.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reason_", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID_", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode_", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));



            try
            {
                sMySqlString = " INSERT INTO EMPLOYEE_ROSTER_DATE  " +
                                " (ROSTR_ID,  " +
                                " EMPLOYEE_ID,  " +
                                " DUTY_DATE,  " +
                                " STATUS_CODE,  " +
                                " IS_OFF_DAY,  " +
                                " IS_SUMMARIZED,  " +
                                " INTERCHANGE_NUMBER,  " +
                                " DUTY_COVERED_BY,  " +
                                " REASON,  " +
                                " ADDED_BY,  " +
                                " ADDED_DATE,  " +
                                " MODIFIED_BY,  " +
                                " MODIFIED_DATE)  " +
                                " VALUES  " +
                                " (@rosterID_,  " +
                                " @employeeID_,  " +
                                " @dutyDate_,  " +
                                " @statusCode_,  " +
                                " @isOffDay_,  " +
                                " @isSummarized_,  " +
                                " @interchangeNumber_,  " +
                                " @dutyCoverredBy_,  " +
                                " @reason_,  " +
                                " @userID_,  " +
                                " now() ,  " +
                                " @userID_,  " +
                                " now() )  ";


                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Dispose();

                bUpdated = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon_.State == ConnectionState.Open)
                {
                    mySqlCon_.Close();
                }

                throw ex;
            }
            finally
            {

            }
            return bUpdated;
        }


        public string getOppositeEmpOfInterchange(string empID, string interchangeNumber)
        {
            //bool isOverlapping = false;
            string oppositeEmp = "";

            try
            {
                string sMySqlString = "SELECT EMPLOYEE_ID, STATUS_CODE FROM EMPLOYEE_ROSTER_DATE " +
                                      " where INTERCHANGE_NUMBER = @interchangeNumber " +
                                      " and STATUS_CODE = @status " +
                                      " and EMPLOYEE_ID <> @empID ";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empID", empID.Trim() == "" ? (object)DBNull.Value : empID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", Constants.STATUS_ACTIVE_VALUE));

                mySqlCon.Open();

                oppositeEmp = mySqlCmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }


            return oppositeEmp;
        }




    }
}
