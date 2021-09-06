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

        public const String SYSTEM_USER = "Admin";


        public const Int32 CON_SMJ_EMPNO_LENGTH = 6;
        public const Int32 CON_SFS_EMPNO_LENGTH = 4;
        public const Int32 CON_SSL_EMPNO_LENGTH = 6;

        public const String CON_SMJ_COMPANY_ID = "CP06";
        public const String CON_SFS_COMPANY_ID = "CP04";
        public const String CON_SSL_COMPANY_ID = "CP01";

        public const String CON_SAVE_BUTTON_TEXT = "Save";
        public const String CON_UPDATE_BUTTON_TEXT = "Update";

        public const String CON_UNIVERSAL_COMPANY_CODE = "CP00";
        public const String CON_UNIVERSAL_COMPANY_NAME = "ALL Companies";

        public const String CON_ROSTER_TYPE_OVER_NIGHT = "OVER_NIGHT";
        public const String CON_ROSTER_OVER_NIGHT_CODE = "2";
        public const String CON_IS_ROSTER = "R";
        public const String CON_IS_NORMAL = "N";

        public const String CON_CALENDER_WROK_DAY_CODE = "W";
        public const String CON_CALENDER_WROK_DAY = "Working Day";
        public const String CON_CALENDER_NON_WROK_DAY = "Holiday";

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

        public const String STATUS_ACTIVE_TAG = "Active";
        public const String STATUS_INACTIVE_TAG = "Inactive";
        public const String STATUS_OBSOLETE_TAG = "Obsolete";

        public const String STATUS_INACTIVE_VALUE = "0";
        public const String STATUS_ACTIVE_VALUE = "1";
        public const String STATUS_OBSOLETE_VALUE = "9";

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
        public const double CON_SL = 0.25;

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

        public const int CON_ATTENDANCE_VIEW_PERIOD = -7;

        public const char CON_ROSTER_EXCLUDE_YES = '1';
        public const char CON_ROSTER_EXCLUDE_NO = '0';
        

    }
}
