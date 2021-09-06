 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common
{
    public class Constants
    {

        //public const string PasswordHash = "P@@Sw0rd";
        //public const string SaltKey = "S@LT&KEY";
        //public const string VIKey = "@1B2c3D4e5F6g7H8";

        public const String CON_SYSTEM_NAME = "Group HRIS System";

        public const String CON_ADMIN_PASSSWORD = "hr!s3@p";
        public const String CON_ADMIN_USER = "ADMIN";
        public const String CON_ADMIN_KeyEMPLOYEE_ID = "EP000000";
        public const String CON_ADMIN_KeyUSER_FIRSTNAME = "HRIS ADMIN";
        public const String CON_ADMIN_KeyHRIS_ROLE = "UR000001";
        public const String CON_ADMIN_KeyEMPLOYEE_GENDER = "M";
        public const string CON_COMMON_KeyHRIS_ROLE = "UR000003";

        public const String SYSTEM_USER = "Admin";


        public const Int32 CON_SMJ_EMPNO_LENGTH = 6;
        public const Int32 CON_SFS_EMPNO_LENGTH = 4;
        public const Int32 CON_SSL_EMPNO_LENGTH = 6;

        public const Boolean CON_ATTENDANCE_DIRECTION_SWAPPED_SSL = true;
        public const Boolean CON_ATTENDANCE_DIRECTION_SWAPPED_EAPH = true;

        public const String CON_SSL_COMPANY_ID = "CP01";
        public const String CON_EAPH_COMPANY_ID = "CP02";
        public const String CON_ETI_COMPANY_ID = "CP03";
        public const String CON_SMJ_COMPANY_ID = "CP06";
        public const String CON_SFS_COMPANY_ID = "CP04";
        //public const String CON_SSL_COMPANY_ID = "CP01";
        public const String CON_SAPPHIRE_COMPANY_ID = "CP22";
        public const String CON_CONCORD_COMPANY_ID  = "CP25";

        public const String CON_SAVE_BUTTON_TEXT = "Save";
        public const String CON_UPDATE_BUTTON_TEXT = "Update";
        public const String CON_COMPLETE_BUTTON_TEXT = "Complete";
        public const String CON_MODIFY_BUTTON_TEXT = "Modify";
        public const String CON_ADD_BUTTON_TEXT = "Add";

        public const String CON_UNIVERSAL_COMPANY_CODE = "CP00";
        public const String CON_UNIVERSAL_COMPANY_NAME = "ALL Companies";

        public const String CON_ROSTER_TYPE_OVER_NIGHT = "OVER_NIGHT";
        public const String CON_ROSTER_OVER_NIGHT_CODE = "2";
        public const String CON_NO_ROSTER_ID = "0";
        public const String CON_IS_ROSTER = "R";
        public const String CON_IS_NORMAL = "N";

        public const String CON_CALENDER_WROK_DAY_CODE = "W";
        public const String CON_CALENDER_WROK_DAY = "Working Day";
        public const String CON_CALENDER_NON_WROK_DAY = "Holiday";

        public const String DEPARTMENT_ID_STAMP = "DP";
        public const String TRAINING_ID_STAMP = "TRN";
        public const String DEPENDENT_ID_STAMP = "DPN";
        public const String BRANCH_ID_STAMP = "BR";
        public const String DESIGNATION_ID_STAMP = "DE";
        public const String EMPLOYEE_TYPE_ID_STAMP = "TY";
        public const String EMPLOYEE_ROLE_ID_STAMP = "RD";
        public const String SALARY_ID_STAMP = "SID";
        public const String EMPLOYEE_ID_STAMP = "EP";
        public const String CON_EMPLOYEE_STS = "S001";
        public const String ROSTER_ID_STAMP = "RO";
        public const String EMPLOYEE_STATUS_ID_STAMP = "S";
        public const String CON_EMPLOYEE_STATUS_ACTIVE = "S001";
        public const String CON_EMPLOYEE_STATUS_RESIGN = "S003";
        public const String INTERCHANGE_ID_STAMP = "IN";
        public const String LEAVE_SHEET_ID_STAMP = "LSN";
        public const String COMPANY_LOCATION_ID_STAMP = "CL";
        public const String BRANCH_REPORT_PRIVILAGE_ID_STAMP = "BRP";
        public const String ALERT_PRIVILAGE_ID_STAMP = "ALP";
        public const String EMPLOYEE_GOAL_ID_STAMP = "EG";
        public const String ASSESSMENT_GROUP_ID = "ASG";
        public const String QUESTION_BANK_ID = "QB";
        public const String GOAL_ASSESSMENT_TOKEN = "GT";

        public const String STATUS_ACTIVE_TAG = "Active";
        public const String STATUS_INACTIVE_TAG = "Inactive";
        public const String STATUS_MISSING_TAG = "Missing";
        public const String STATUS_OBSOLETE_TAG = "Obsolete";

        public const String STATUS_INACTIVE_VALUE = "0";
        public const String STATUS_ACTIVE_VALUE = "1";
        public const String STATUS_MISSING_VALUE = "2";
        public const String STATUS_OBSOLETE_VALUE = "9"; 
        public const String STATUS_CANCEL_VALUE = "8";

        public const String LEAVE_STATUS_REJECTED = "0";
        public const String LEAVE_STATUS_ACTIVE_VALUE = "1";
        public const String LEAVE_STATUS_COVERED = "2";
        public const String LEAVE_STATUS_RECOMMAND = "3";
        public const String LEAVE_STATUS_APPROVED = "4";
        public const String LEAVE_STATUS_DISCARDED = "9";
        public const String LEAVE_IS_OFF_DAY_YES = "1";
        public const String LEAVE_IS_OFF_DAY_NO = "0";

        public const String LEAVE_SHEET_COVER_APPROVAL_FLAG = "CA";
        public const String LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG = "RA";
        public const String LEAVE_SHEET_APPROVAL_FLAG = "APP";
        public const String LEAVE_SHEET_REJECT_FLAG = "REJ";

        public const double MONTHLY_SHORT_LEAVE_LIMIT = 2;

        public const String OT_ACTIVE_TAG = "Yes";
        public const String OT_INACTIVE_TAG = "No";
        public const String OT_ACTIVE_VALUE = "1";
        public const String OT_INACTIVE_VALUE = "0";


        public const String TRANSFER_TYPE_ORDINARY = "T";
        public const String TRANSFER_TYPE_INITIAL = "I";

        public const String KeyLOGOUT_STS = "";
        public const String KeyEMPLOYEE_ID = "";
        public const String KeyHRIS_ROLE = "";
        public const String KeyCOMP_ID = "";
        public const String KeyUSER_ID = "";
        public const String KeyUSER_FIRSTNAME = "";
        public const String KeyEMPLOYEE_GENDER = "";

        public const String CON_PRODUCT = "eHRIS SYSTEM";
        public const String CON_SENDER = "eHRIS";

        public const String CON_DEFAULT_USER_ROLE = "Common User Role";
        public const String CON_DEFAULT_PASSWORD = "0";


        public static char[] CON_SEPARATING_DELIMITERS = { ' ', ',', '.', ':' };
        public static String[] CON_SECONDARY_EDUCATION_GRADES = { "", "A", "B", "C", "D", "S", "W", "F", "ab" };
        public static char CON_FIELD_SEPARATOR = '^';

        public const int CON_NUM_ANNUAL_LEAVES_BEFORE_APRIL_1 = 14;
        public const int CON_NUM_ANNUAL_LEAVES_BEFORE_JULY_1 = 10;
        public const int CON_NUM_ANNUAL_LEAVES_BEFORE_OCT_1 = 7;
        public const int CON_NUM_ANNUAL_LEAVES_AFTER_OCT_1 = 4;
        public static decimal CON_MONTHLY_CASUAL_LEAVES = Decimal.Parse("0.5");

        public const int CON_DROP_DOWN_NUM_YEARS = 60;

        public const String CON_ANNUAL_LEAVE_CATEGORY = "A";
        public const String CON_CASUAL_LEAVE_CATEGORY = "C";
        public const String CON_MEDICAL_LEAVE_CATEGORY = "M";
        public const String CON_OTHER_LEAVE_CATEGORY = "O";
        public const decimal CON_DAYS_ANNAM = 365;

        public const String CON_ANNUAL_LEAVE_ID = "ANNUAL";
        public const String CON_CASUAL_LEAVE_ID = "CASUAL";
        public const String CON_MEDICAL_LEAVE_ID = "MEDICAL";
        public const String CON_SHORT_LEAVE_LEAVE_ID = "SL";
        public const String CON_DUTY_LEAVE_ID = "DUTY";
        public const String CON_LIEU_LEAVE_ID = "LIEU";
        public const String CON_NOPAY_LEAVE_ID = "NOPAY";
        public const String CON_STUDY_LEAVE_LEAVE_ID = "STUDY";
        public const String CON_MATINITY1_LEAVE_ID = "MATINITY1";
        public const String CON_MATINITY2_LEAVE_LEAVE_ID = "MATINITY2";
        public const String CON_SHORT_LEAVE_LEAVE_TYPE = "SHORT LEAVE";


        /*
         * following constants are not used but has used in a form which is not used
         * */
        public const String CON_LEAVE_PENDING_STATUS = "0";
        public const String CON_LEAVE_APPROVED_STATUS = "1";
        public const String CON_LEAVE_REJECTED_STATUS = "2";
        /*
         * */

        public const String CON_HALF_DAY_FLAG = "H";
        public const String CON_FULL_DAY_FLAG = "F";
        public const String CON_SHORT_LEAVE_FLAG = "S";

        public const String CON_WELFARE_ACCEPTED = "1";
        public const String CON_WELFARE_REJECTED = "0";

        public const double CON_FULL_DAY = 1;
        public const double CON_HALF_DAY = 0.5;
        public const double CON_SL = 0.18;

        public const String CON_GENDER_MALE = "M";
        public const String CON_GENDER_FEMALE = "F";

        public const String CON_DB_TRUE_CHAR = "Y";
        public const String CON_DB_FALSE_CHAR = "N";

        public const String CON_SUMMARIZED = "Y";
        public const String CON_NOT_SUMMARIZED = "N";

        public const String CON_DEFAULT_IMAGE_HEIGHT = "200";
        public const String CON_DEFAULT_IMAGE_WIDTH = "230";

        public const String CON_DEFAULT_MALE_IMAGE_PATH = "~/Images/HRISMain/Malep.png";
        public const String CON_DEFAULT_FEMALE_IMAGE_PATH = "~/Images/HRISMain/Femalep.png";
        public const String CON_DEFAULT_EEMPLOYEE_IMAGE_PATH = "~/Images/Employee/Photo/";
        public const String CON_DEFAULT_BG_IMAGE = "/Images/HRISMain/bg.jpg";
        public const String CON_DEFAULT_BG_IMAGE_PATH = "~/Images/Employee/Bg/";

        public const String CON_DEFAULT_FONT_BOLD = "Y";
        public const String CON_DEFAULT_FONT_ITALIC = "Y";
        public const String CON_DEFAULT_FONT_SIZE = "8";

        public const String CON_ATTENDANCE_IN = "1";
        public const String CON_ATTENDANCE_OUT = "0";

        public const char CON_YEAR_TOKEN = 'y';

        public const char CON_LEAVE_COVER = 'C';
        public const char CON_LEAVE_RECOMMEND = 'R';


        public const String CON_LEAVE_APPROVED_MESSAGE = "Successfully Completed";
        public const String CON_LEAVE_REJECTED_MESSAGE = "Successfully Completed";

        public const String CON_LEAVE_ALREADY_AGREED_MESSAGE = "You have already agreed for covering leaves";
        public const String CON_LEAVE_ALREADY_APPROVED_MESSAGE = "Leave sheet is already approved";
        public const String CON_LEAVE_ALREADY_REJECTED_MESSAGE = "Leave sheet is already rejected";

        public const int CON_ATTENDANCE_VIEW_PERIOD = -60;

        public const char CON_ROSTER_EXCLUDE_YES = '1';
        public const char CON_ROSTER_EXCLUDE_NO = '0';


        public const String CON_NON_ROSTER_EMPLOYEE = "0";

        public const char CON_SEC_EDUCATION_PENDING = '0';
        public const char CON_SEC_EDUCATION_VERIFIED = '1';
        public const char CON_SEC_EDUCATION_OBSERLETE = '8';
        public const char CON_SEC_EDUCATION_REJECTED = '9';

        public const char OT_ELIGIBLE = '1';
        public const char OT_NOT_ELIGIBLE = '0';

        public const String ROSTER_EMPLOYEE = "1";
        public const String REGULAR_EMPLOYEE = "0";

        public const string CON_ACTIVE_STATUS = "1";
        public const string CON_INACTIVE_STATUS = "0";
        public const string CON_EMPOLYEE_PREFIX = "EP";
        public const int CON_EMPOLYEE_ID_LENGTH = 8;
        public const int CON_EMPOLYEE_PREFIX_LENGTH = 2;


        public const String COMPANY_OT_CATEGORY = "CC";
        public const String PROPERTY_CATEGORY = "PC";
        public const String PROPERTY_ID = "PI";
        public const String ASSIGN_ID = "AI";

        public const string CON_AVILABLE_TAG = "Avilable";
        public const string CON_ASSIGNED_TAG = "Assigned";
        public const string CON_DISPOSED_TAG = "Disposed";

        public const string CON_AVILABLE_STATUS = "1";
        public const string CON_ASSIGNED_STATUS = "2";
        public const string CON_DISPOSED_STATUS = "3";


        public const string CON_RETURNED_TAG = "Returned";
        public const string CON_UTILIZED_TAG = "Utilized";
        public const string CON_DISCARD_TAG = "Discared";
        public const string CON_PENDING_TAG = "Pending";
        public const string CON_REMOVE_TAG = "Remove";


        public const string CON_RETURNED_STATUS = "0";
        public const string CON_UTILIZED_STATUS = "1";
        public const string CON_DISCARD_STATUS = "2";
        public const string CON_REMOVE_STATUS = "3";

        public const string CON_MAIL_EXCLUDE = "1";
        public const string CON_MAIL_INCLUDE = "0";

        public const string CON_IS_SUMMARIZED_YES = "Y";
        public const string CON_IS_SUMMARIZED_NO = "N";

        public enum EmployeeType
        {
            Managerial = 1,
            Executive,
            NonExecutive
        }


        public const String STATUS_COST_TAG = "Cost Center";
        public const String STATUS_PROFIT_TAG = "Profit Center";

        public const String STATUS_COST_VALUE = "1";
        public const String STATUS_PROFIT_VALUE = "0";

        public const String ATT_LOG = "AL";

        public const String CON_ATTENDANCE_APPROVE = "A";
        public const String CON_ATTENDANCE_REJECT = "R";


        public const String CON_COST_CENTER = "1";
        public const String CON_PROFIT_CENTER = "0";


        // used in leave sheet and online leve modules to denote invalid time period of a leave
        // Anjana Uduwaragoda 2015/10/28

        public const String CON_INVALID_TIME = "Invalid";
        public const String CON_VALID_TIME = "Valid";

        public const String CON_NLEAVE_MHALF    = "MH";
        public const String CON_NLEAVE_EHALF    = "EH";
        public const String CON_NLEAVE_FDAY     = "FD";
        public const String CON_NLEAVE_SL       = "SL";

        public const String CON_ON_LEAVE = "On-Leave";

        public const String CON_REGULAR_ROSTER_TYPE = "1";
        public const String CON_OVERNIGHT_ROSTER_TYPE = "2";

        public const String CON_PREVIOUS_EMPLOYMENT_PENDING_CODE = "0";
        public const String CON_PREVIOUS_EMPLOYMENT_PENDING_TEXT = "Pending";
        public const String CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE = "1";
        public const String CON_PREVIOUS_EMPLOYMENT_APPROVED_TEXT = "Approved";
        public const String CON_PREVIOUS_EMPLOYMENT_REJECTED_CODE = "2";
        public const String CON_PREVIOUS_EMPLOYMENT_REJECTED_TEXT = "Rejected";

        public const String CON_VERIFIED_WITH_SERVICE_LETTER = "1";
        public const String CON_VERIFIED_WITHOUT_SERVICE_LETTER = "0";

        public const int CON_COMPANY_CALENDER_ALERT_MONTH = 12;

        public const String CON_BIRTHDAY_ALERT_ID = "1";
        public const String CON_LEAVE_ALERT_ID = "2";
        public const String CON_COMPANY_CALENDAR_ALERT_ID = "3";
        public const String CON_SUPERVISOR_INACTIVE_ALERT_ID = "4";
        public const String CON_NON_ROSTER_EMPLOYEE_ALERT_ID = "5";


        //ID for Mearge company with Stored Procedure
        public const String MEARGE_ID = "SP";

        //Input stored procedure id
        public const String STORED_PROCEDURE_ID = "SPI";

        public const String CON_DIRECTION_IN_VALUE = "1";
        public const String CON_DIRECTION_OUT_VALUE = "0";

        public const String CON_DIRECTION_IN_TEXT = "IN";
        public const String CON_DIRECTION_OUT_TEXT = "OUT";

        public const string DATE_VALUE = "Saturday";


        public const String SPOUSE_ID = "FM000001";
        public const String SPOUSE_TEXT = "Spouse";

        public const String SON_ID = "FM000002";
        public const String SON_TEXT = "Son";

        public const String DAUGHTER_ID = "FM000003";
        public const String DAUGHTER_TEXT = "Daughter";

        public const String FATHER_ID = "FM000004";
        public const String FATHER_TEXT = "Father";

        public const String MOTHER_ID = "FM000005";
        public const String MOTHER_TEXT = "Mother";

        public const String GRAND_SON_ID = "FM000006";
        public const String GRAND_SON_TEXT = "Grand Son";

        public const String GRAND_DAUGHTER_ID = "FM000007";
        public const String GRAND_DAUGHTER_TEXT = "Grand Daughter";

        public const String GRAND_FATHER_ID = "FM000008";
        public const String GRAND_FATHER_TEXT = "Grand Father";

        public const String GRAND_MOTHER_ID = "FM000009";
        public const String GRAND_MOTHER_TEXT = "Grand Mother";

        public const String ASSESSNEMT_PENDING_STATUS = "0";
        public const String ASSESSNEMT_ACTIVE_STATUS = "1";
        public const String ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS = "2";
        public const String ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS = "3";
        public const String ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS = "4";
        public const String ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS = "7";
        public const String ASSESSNEMT_SUBORDINATE_AGREE_STATUS = "8";
        public const String ASSESSNEMT_CEO_FINALIZED_STATUS = "5";
        public const String ASSESSNEMT_CLOSED_STATUS = "6";
        public const String ASSESSNEMT_OBSOLETE_STATUS = "9";
        
        public const String ASSESSNEMT_PENDING_TAG = "Pending";
        public const String ASSESSNEMT_ACTIVE_TAG = "Active";
        public const String ASSESSNEMT_SUBORDINATE_FINALIZED_TAG = "Subordinate Finalized";
        public const String ASSESSNEMT_SUPERVISOR_COMPLETED_TAG = "Supervisor Completed";
        public const String ASSESSNEMT_SUPERVISOR_FINALIZED_TAG = "Supervisor Finalized";
        public const String ASSESSNEMT_SUBORDINATE_AGREE_TAG = "Subordinate Agreed";
        public const String ASSESSNEMT_SUBORDINATE_DISAGREE_TAG = "Subordinate Disagreed";
        public const String ASSESSNEMT_CEO_FINALIZED_TAG = "CEO Finalized";
        public const String ASSESSNEMT_CLOSED_TAG = "Closed";
        public const String ASSESSNEMT_OBSOLETE_TAG = "Obsolete";


        public const String ASSESSNEMT_PENDING_COLOR = "'rgb(242, 203, 145)'";
        public const String ASSESSNEMT_ACTIVE_COLOR = "'rgb(95, 255, 56)'";
        public const String ASSESSNEMT_SUBORDINATE_FINALIZED_COLOR = "'rgb(95, 255, 56)'";
        public const String ASSESSNEMT_SUPERVISOR_COMPLETED_COLOR = "'rgb(51, 144, 178)'";
        public const String ASSESSNEMT_SUPERVISOR_FINALIZED_COLOR = "'rgb(142, 51, 178)'";
        public const String ASSESSNEMT_SUBORDINATE_AGREE_COLOR = "'rgb(244, 137, 66)'";
        public const String ASSESSNEMT_SUBORDINATE_DISAGREE_COLOR = "'rgb(226, 102, 141)'";
        public const String ASSESSNEMT_CEO_FINALIZED_COLOR = "'rgb(5, 102, 21)'";
        public const String ASSESSNEMT_CLOSED_COLOR = "'rgb(229, 135, 135)'";
        public const String ASSESSNEMT_OBSOLETE_COLOR = "'rgb(204, 200, 191)'";
        public const String ASSESSNEMT_NA_COLOR = "'rgb(165, 167, 170)'";


        public const String STATUS_AGREE_TAG = "Agree";
        public const String STATUS_DISAGREE_TAG = "Disagree";

        public const String STATUS_AGREE_VALUE = "1";
        public const String STATUS_DISAGREE_VALUE = "0";

        public const String STATUS_CONFIRM_TAG = "Confirm";
        public const String STATUS_REJECT_TAG = "Reject";

        public const String STATUS_CONFIRM_VALUE = "1";
        public const String STATUS_REJECT_VALUE = "0";

        public const char CON_EXCLUDE_YES = '1';
        public const char CON_EXCLUDE_NO = '0';


        public const string PROGRAME_TYPE_LONG_TERM_VALUE = "1";
        public const string PROGRAME_TYPE_SHORT_TERM_VALUE = "2";
        public const string PROGRAME_TYPE_LONG_TERM_TAG = "Long Term";
        public const string PROGRAME_TYPE_SHORT_TERM_TAG = "Short Term";

        public const string TRAINER_EXTERNAL_TAG = "External";
        public const string TRAINER_INTERNAL_TAG = "Internal";
        public const string TRAINER_EXTERNAL_VALUE = "1";
        public const string TRAINER_INTERNAL_VALUE = "2";

        // used in training request
        public const String CON_APPROVED = "1";
        public const String CON_REJECTED = "0";


        public const String CON_PENDING_STRING = "Pending";
        public const String CON_RECOMENDED_STRING = "Recommended";
        public const String CON_REJECTED_STRING = "Rejected";
        public const String CON_APPROVED_STRING = "Approved";

        public const String CON_RECOMMAND_TEXT = "Recommend";
        public const String CON_APPROVE_TEXT = "Approve";

        public const double CON_ASSESSMENT_ASSESSED_EMPLOYEE_COMPLETION_DURATION = 10;


        ///Begin Report Groups ///
        public const string CON_REPORT_GROUP_BRANCH_REPORTS = "1";
        public const string CON_REPORT_GROUP_TRAINING_REPORTS = "2";
        ///End Report Groups ///

        ///Begin Training Status ///
        public const String CON_TRAINING_PENDING_VALUE = "0";
        public const String CON_TRAINING_ACTIVATED_VALUE = "1";
        public const String CON_TRAINING_COMPLETED_VALUE = "2";
        public const String CON_TRAINING_CLOSED_VALUE = "3";
        public const String CON_TRAINING_OBSOLETED_VALUE = "4";

        public const String CON_TRAINING_PENDING_TAG = "Pending";
        public const String CON_TRAINING_ACTIVATED_TAG = "Activated";
        public const String CON_TRAINING_COMPLETED_TAG = "Completed";
        public const String CON_TRAINING_CLOSED_TAG = "Closed";
        public const String CON_TRAINING_OBSOLETED_TAG = "Obsoleted";
        ///End Training Status ///
        ///


        //Payroll Module
        public const String CON_OT_CATEGORY_OVERTIME_TAG = "Overtime";
        public const String CON_OT_SUB_CATEGORY_NORMAL_OVERTIME_TAG = "NormalOT";
        public const String CON_OT_SUB_CATEGORY_DOUBLE_OVERTIME_TAG = "DoubleOT";
        public const String CON_OT_CATEGORY_ALLOWANCE_TAG = "Allowance";
        public const String CON_OT_SUB_CATEGORY_ALLOWANCE_ATTENDANCE_INCENTIVE_TAG = "AttendanceIncentive";

        public const String CON_OT_CATEGORY_OVERTIME_NORMAL_OT_ID = "2";
        public const String CON_OT_CATEGORY_OVERTIME_DOUBLE_OT_ID = "3";
        public const String CON_OT_CATEGORY_ATTENDANCE_INCENTIVE_ID = "5";

        public const String CON_ROLE_OT_CATEGORY_MANAGERIAL = "Managerial";
        public const String CON_ROLE_OT_CATEGORY_EXECUTIVE = "Executive";
        public const String CON_ROLE_OT_CATEGORY_NON_EXECUTIVE = "NonExecutive";

        public const String CON_PENDING_STATUS = "0";


        //
    }
}