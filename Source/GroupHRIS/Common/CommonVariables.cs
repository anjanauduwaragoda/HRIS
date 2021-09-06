using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class CommonVariables
    {
        public static String MESSAGE_TEXT = "";
        public static String MESSAGE_WARNING = "2";
        public static String MESSAGE_SUCCESS = "1";
        public static String MESSAGE_ERROR = "0";

        public static String MESSAGE_STRING_SUCCESS_INSERTED = "Record(s) successfully saved.";
        public static String MESSAGE_STRING_SUCCESS_FINALIZED = "Successfully finalized.";
        public static String MESSAGE_STRING_SUCCESS_UPDATED = "Record(s) successfully updated.";
        public static String MESSAGE_STRING_SUCCESS_SAVED = "Record(s) successfully saved.";
        public static String MESSAGE_STRING_SUCCESS_DELETED = "Record(s) successfully deleted.";
        public static String MESSAGE_STRING_SUCCESS_OBSOLETED = "Record(s) successfully obsoleted.";
        public static String MESSAGE_STRING_SUCCESS_OBSOLETED_INT = "Record(s) successfully obsoleted including the interchanger's.";
        public static String MESSAGE_STRING_SUCCESS_VERIFIED = "Record(s) successfully verified.";
        public static String MESSAGE_STRING_SUCCESS_REJECTD = "Record(s) successfully rejected.";
        public static String MESSAGE_STRING_SUCCESS_INTERCHANGE = "Roster interchange successfully completed.";
        public static String MESSAGE_STRING_SUCCESS_COMPLETED = "Successfully completed.";


        public static String EMAIL_SUBJECT_SEC_EDU_ADDED = "Secondary Education Qualification Added";
        public static String EMAIL_SUBJECT_HIG_EDU_ADDED = "Higher Education Qualification Added";
        public static String EMAIL_SUBJECT_PRV_EXP_ADDED = "Previous Experience Added";


        public static String PASSWORD_CHARS = "abcdefghijklmnopqrstuvwxyz0123456789#!@&$%ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static Int32 PASSWORD_LENGTH = 8;


        public static String ERROR_COLOR_SUCCESS = "#FF0000";
        public static String ERROR_COLOR_FAIL = "#00CC00";
        public static String ERROR_BGCOLOR = "FFFFFF";

        public static String COPY_RIGHT_TEXT = "@ Copyright Swarna Solutions " + DateTime.Today.ToString("yyyy");
        public static String LINK_TEXT = " HOME | EAP GROUP | FINANCE | TRADING | ENTERTAINMENT | LEISURE | BOARD OF DIRECTORS ";

        public static String EMAIL_SUBJECT = "";
        public static String EMAIL_BODY = "";
        public static String COMMON_TEXT = "";

        public static String COMMON_MAIL_ADDRESS = "eHRM@eapholdings.lk";
    }
}