using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DataHandler;
using DataHandler.EmployeeLeave;

namespace DomainConstraints
{
    public class LeaveConstrains
    {
        public decimal availableAnnualLeaves(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            decimal dNumAnnualLeaves = 0;

            decimal dMonths = leaveScheduleDataHandler.getServiceMonthsofEmployee(employeeId);
            decimal dServiceDays = leaveScheduleDataHandler.getServiceDaysOfEmployee(employeeId);
            int iJoinedMonth = leaveScheduleDataHandler.getJoindMonthofEmployee(employeeId);
            int iJoinedYear = leaveScheduleDataHandler.getJoindYearofEmployee(employeeId);
            int iCurrentYear = System.DateTime.Now.Year;

            if (dServiceDays <= Constants.CON_DAYS_ANNAM)
            {
                dNumAnnualLeaves = 0;

            }
            else if ((dServiceDays > Constants.CON_DAYS_ANNAM) && ((iJoinedYear + 1) <= iCurrentYear)) 
            {
                if ((iJoinedMonth >= 1) && (iJoinedMonth < 4))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1;

                else if ((iJoinedMonth >= 4) && (iJoinedMonth < 7))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1;

                else if ((iJoinedMonth >= 7) && (iJoinedMonth < 10))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1;

                else if (iJoinedMonth >= 10)
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1;
            }

            else if ((iJoinedYear + 1) > iCurrentYear)
            {
                dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1;
            }

            return dNumAnnualLeaves;
        }

        public decimal availableAnnualLeaves(string employeeId, int iYear)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            decimal dNumAnnualLeaves = 0;

            decimal dMonths = leaveScheduleDataHandler.getServiceMonthsofEmployee(employeeId);
            decimal dServiceDays = leaveScheduleDataHandler.getServiceDaysOfEmployee(employeeId);
            int iJoinedMonth = leaveScheduleDataHandler.getJoindMonthofEmployee(employeeId);
            int iJoinedYear = leaveScheduleDataHandler.getJoindYearofEmployee(employeeId);
            int iCurrentYear = iYear;

            if (dServiceDays <= Constants.CON_DAYS_ANNAM)
            {
                dNumAnnualLeaves = 0;

            }
            else if ((dServiceDays > Constants.CON_DAYS_ANNAM) && ((iJoinedYear + 1) == iCurrentYear))
            {
                if ((iJoinedMonth >= 1) && (iJoinedMonth < 4))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1;

                else if ((iJoinedMonth >= 4) && (iJoinedMonth < 7))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1;

                else if ((iJoinedMonth >= 7) && (iJoinedMonth < 10))
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1;

                else if (iJoinedMonth >= 10)
                    dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1;
            }

            else if ((iJoinedYear + 1) < iCurrentYear)
            {
                dNumAnnualLeaves = Constants.CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1;
            }

            return dNumAnnualLeaves;
        }


        public decimal availableCasualLeaves(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            decimal dNumCasualLeaves = 0;

            decimal dMonths = leaveScheduleDataHandler.getServiceMonthsofEmployee(employeeId);            
            int iJoinedYear = leaveScheduleDataHandler.getJoindYearofEmployee(employeeId);
            int iCurrentYear = System.DateTime.Now.Year;

            if (iJoinedYear < iCurrentYear)
            {
                dNumCasualLeaves = 7;
            }
            else
            {
                dNumCasualLeaves = Decimal.Round(dMonths, 0) * Constants.CON_MONTHLY_CASUAL_LEAVES;
            }
            
            return dNumCasualLeaves;
        }

        public decimal availableCasualLeaves(string employeeId, int iYear)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            decimal dNumCasualLeaves = 0;

            decimal dMonths = leaveScheduleDataHandler.getServiceMonthsofEmployee(employeeId);
            int iJoinedYear = leaveScheduleDataHandler.getJoindYearofEmployee(employeeId);
            int iCurrentYear = iYear;

            if (iJoinedYear < iCurrentYear)
            {
                dNumCasualLeaves = 7;
            }
            else
            {
                dNumCasualLeaves = Decimal.Round(dMonths, 0) * Constants.CON_MONTHLY_CASUAL_LEAVES;
            }

            return dNumCasualLeaves;
        }


    }
}
 