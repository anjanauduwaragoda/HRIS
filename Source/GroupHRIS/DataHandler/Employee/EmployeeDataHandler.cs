using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;


namespace DataHandler.Employee
{
    public class EmployeeDataHandler : TemplateDataHandler 
    {

        public DataTable populateEmpNIc(string sEMP_NIC)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT a.EMP_NIC,a.EMAIL,b.USER_ID " +
                                      " FROM EMPLOYEE a,HRIS_USER b " +
                                      " where a.EMPLOYEE_ID = b.EMPLOYEE_ID " +
                                      " and a.EMP_NIC = '" + sEMP_NIC + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

                
        public Boolean Insert(String companyId,
                              String departmentId,
                              String divisionId,
                              String employeeStatus,
                              String employeeType,
                              String employeeRoleId,
                              String title,
                              String initials,
                              String firstName,
                              String middleName,
                              String lastName,
                              String fullName,
                              String gender,
                              String nicNo,
                              String passportNo,
                              String dateOfBirth,
                              String dateOfJoin,
                              String maritalStatus,
                              String nationality,
                              String religon,
                              String eMail,
                              String epfNo,
                              String etfNo,
                              String permanentAddress,
                              String currentAddress,
                              String landPhone,
                              String personalMobile,
                              String officeMobile,
                              String fuelCardNo,
                              String reportTo1,
                              String reportTo2,
                              String reportTo3,
                              String city,
                              String distance,
                              String remarks,
                              String addedBy,
                              String costCenter,
                              String profitCenter,
                              String isWelfare,                              
                              String resignedDate,
                              String designationId,
                              String branchId,
                              String locationId,
                              String nameInitials,
                              String knownName,
                              String modCategory,
                              String probConEndDate,
                              String isOTEligible,
                              String isRoster,
                              String isMailExclude,
                              String otSession)
        {
            Boolean blInserted = false;

            SerialHandler serialHandler = new SerialHandler();

            string employeeId = "";

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@departmentId", departmentId.Trim() == "" ? (object)DBNull.Value : departmentId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@divisionId", divisionId.Trim() == "" ? (object)DBNull.Value : divisionId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeStatus", employeeStatus.Trim() == "" ? (object)DBNull.Value : employeeStatus.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeType", addedBy.Trim() == "" ? (object)DBNull.Value : employeeType.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeRoleId", employeeRoleId.Trim() == "" ? (object)DBNull.Value : employeeRoleId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@title", title.Trim() == "" ? (object)DBNull.Value : title.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@initials", initials.Trim() == "" ? (object)DBNull.Value : initials.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@firstName", employeeStatus.Trim() == "" ? (object)DBNull.Value : firstName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@middleName", middleName.Trim() == "" ? (object)DBNull.Value : middleName.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@lastName", lastName.Trim() == "" ? (object)DBNull.Value : lastName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fullName", fullName.Trim() == "" ? (object)DBNull.Value : fullName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@gender", gender.Trim() == "" ? (object)DBNull.Value : gender.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@nicNo", nicNo.Trim() == "" ? (object)DBNull.Value : nicNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@passportNo", passportNo.Trim() == "" ? (object)DBNull.Value : passportNo.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@dateOfBirth", dateOfBirth.Trim() == "" ? (object)DBNull.Value : dateOfBirth.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dateOfJoin", dateOfJoin.Trim() == "" ? (object)DBNull.Value : dateOfJoin.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@maritalStatus", maritalStatus.Trim() == "" ? (object)DBNull.Value : maritalStatus.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@nationality", nationality.Trim() == "" ? (object)DBNull.Value : nationality.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@religon", religon.Trim() == "" ? (object)DBNull.Value : religon.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@eMail", eMail.Trim() == "" ? (object)DBNull.Value : eMail.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@epfNo", epfNo.Trim() == "" ? (object)DBNull.Value : epfNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@etfNo", etfNo.Trim() == "" ? (object)DBNull.Value : etfNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@permanentAddress", permanentAddress.Trim() == "" ? (object)DBNull.Value : permanentAddress.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@currentAddress", currentAddress.Trim() == "" ? (object)DBNull.Value : currentAddress.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@landPhone", landPhone.Trim() == "" ? (object)DBNull.Value : landPhone.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@personalMobile", personalMobile.Trim() == "" ? (object)DBNull.Value : personalMobile.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@officeMobile", officeMobile.Trim() == "" ? (object)DBNull.Value : officeMobile.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fuelCardNo", fuelCardNo.Trim() == "" ? (object)DBNull.Value : fuelCardNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo1", reportTo1.Trim() == "" ? (object)DBNull.Value : reportTo1.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo2", reportTo2.Trim() == "" ? (object)DBNull.Value : reportTo2.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo3", reportTo3.Trim() == "" ? (object)DBNull.Value : reportTo3.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@city", city.Trim() == "" ? (object)DBNull.Value : city.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@distance", distance.Trim() == "" ? (object)DBNull.Value : distance.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@costCenter", costCenter.Trim() == "" ? (object)DBNull.Value : costCenter.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@profitCenter", profitCenter.Trim() == "" ? (object)DBNull.Value : profitCenter.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@isWelfare", isWelfare.Trim() == "" ? (object)DBNull.Value : isWelfare.Trim()));
            

            mySqlCmd.Parameters.Add(new MySqlParameter("@resignedDate", resignedDate.Trim() == "" ? (object)DBNull.Value : resignedDate.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@designationId", designationId.Trim() == "" ? (object)DBNull.Value : designationId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@branchId", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@locationId", locationId.Trim() == "" ? (object)DBNull.Value : locationId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@nameInitials", nameInitials.Trim() == "" ? (object)DBNull.Value : nameInitials.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@knownName", knownName.Trim() == "" ? (object)DBNull.Value : knownName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@modCategory", modCategory.Trim() == "" ? (object)DBNull.Value : modCategory.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@probConEndDate", probConEndDate.Trim() == "" ? (object)DBNull.Value : probConEndDate.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@oTEligibility", isOTEligible.Trim() == "" ? (object)DBNull.Value : isOTEligible.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isRoster", isRoster.Trim() == "" ? (object)DBNull.Value : isRoster.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isMailExclude", isMailExclude.Trim() == "" ? (object)DBNull.Value : isMailExclude.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@otSession", otSession.Trim() == "" ? (object)DBNull.Value : otSession.Trim()));
            
            
            
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                employeeId = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_ID_STAMP);

                string attRegNo_ = employeeId.Substring(3);

                string attRegNo = int.Parse(attRegNo_).ToString();

                mySqlCmd.Parameters.Add(new MySqlParameter("@attRegNo", attRegNo.Trim() == "" ? (object)DBNull.Value : attRegNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));

                sMySqlString = "INSERT INTO EMPLOYEE(EMPLOYEE_ID,COMPANY_ID,DEPT_ID,DIVISION_ID," +
                               "EMPLOYEE_STATUS,EMP_TYPE_ID,ROLE_ID,TITLE,EMP_INITIALS,FIRST_NAME,MIDDLE_NAMES," +
                               "LAST_NAME,FULL_NAME,GENDER,EMP_NIC,PASSPORT_NUMBER,DOB,DOJ," +
                               "MARITAL_STATUS,NAIONALITY,RELIGION,EMAIL,EPF_NO,ETF_NO,PERMANENT_ADDRESS," +
                               "CURRENT_ADDRESS,LAND_PHONE,MOBILE_PHONE_PERSONAL,MOBILE_PHONE_COMPANY," +
                               "FUEL_CARD_NUMBER,REPORT_TO_1,REPORT_TO_2,REPORT_TO_3,CITY,DISTANCE_TO_OFFICE," +
                               "REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,STATUS_CODE,COST_CENTER," +
                               "PROFIT_CENTER,IS_WELFARE,ATT_REG_NO,RESIGNED_DATE,BRANCH_ID,LOCATION_ID,DESIGNATION_ID," +
                               "INITIALS_NAME,KNOWN_NAME,MOD_CAT_ID,PROBATION_CONTRACT_ENDDATE,IS_OT_ELIGIBLE,IS_ROSTER,EXCLUDE_EMAIL,OT_SESSION) " +
                               " VALUES(@employeeId,@companyId,@departmentId,@divisionId,@employeeStatus," +
                               "@employeeType,@employeeRoleId,@title,@initials,@firstName,@middleName," +
                               "@lastName,@fullName,@gender,@nicNo,@passportNo,@dateOfBirth,@dateOfJoin," +
                               "@maritalStatus,@nationality,@religon,@eMail,@epfNo,@etfNo,@permanentAddress," +
                               "@currentAddress,@landPhone,@personalMobile,@officeMobile,@fuelCardNo," +
                               "@reportTo1,@reportTo2,@reportTo3,@city,@distance,@remarks,@addedBy," +
                               "now(),@addedBy,now(),'1',@costCenter,@profitCenter,@isWelfare,@attRegNo," +
                               "@resignedDate,@branchId,@locationId,@designationId,@nameInitials,@knownName,@modCategory,@probConEndDate,@oTEligibility,@isRoster,@isMailExclude,@otSession)";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;
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
                serialHandler = null;
            }
            return blInserted;
        }

        public Boolean Update(String employeeId,
                              String companyId,
                              String departmentId,
                              String divisionId,
                              String employeeStatus,
                              String employeeType,
                              String employeeRoleId,
                              String title,
                              String initials,
                              String firstName,
                              String middleName,
                              String lastName,
                              String fullName,
                              String gender,
                              String nicNo,
                              String passportNo,
                              String dateOfBirth,
                              String dateOfJoin,
                              String maritalStatus,
                              String nationality,
                              String religon,
                              String eMail,
                              String epfNo,
                              String etfNo,
                              String permanentAddress,
                              String currentAddress,
                              String landPhone,
                              String personalMobile,
                              String officeMobile,
                              String fuelCardNo,
                              String reportTo1,
                              String reportTo2,
                              String reportTo3,
                              String city,
                              String distance,
                              String remarks,
                              String addedBy,
                              String costCenter,
                              String profitCenter,
                              String isWelfare,                              
                              String resignedDate,
                              String designationId,
                              String branchId,
                              String locationId,
                              String nameInitials,
                              String knownName,
                              String modCategory,
                              String probConEndDate,
                              String isOTEligible,
                              String isRoster,
                              String isMailExclude,
                              String otSession)

        {
            Boolean blInserted = false;

            SerialHandler serialHandler = new SerialHandler();            

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@departmentId", departmentId.Trim() == "" ? (object)DBNull.Value : departmentId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@divisionId", divisionId.Trim() == "" ? (object)DBNull.Value : divisionId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeStatus", employeeStatus.Trim() == "" ? (object)DBNull.Value : employeeStatus.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeType", addedBy.Trim() == "" ? (object)DBNull.Value : employeeType.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeRoleId", employeeRoleId.Trim() == "" ? (object)DBNull.Value : employeeRoleId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@title", title.Trim() == "" ? (object)DBNull.Value : title.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@initials", initials.Trim() == "" ? (object)DBNull.Value : initials.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@firstName", employeeStatus.Trim() == "" ? (object)DBNull.Value : firstName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@middleName", middleName.Trim() == "" ? (object)DBNull.Value : middleName.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@lastName", lastName.Trim() == "" ? (object)DBNull.Value : lastName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fullName", fullName.Trim() == "" ? (object)DBNull.Value : fullName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@gender", gender.Trim() == "" ? (object)DBNull.Value : gender.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@nicNo", nicNo.Trim() == "" ? (object)DBNull.Value : nicNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@passportNo", passportNo.Trim() == "" ? (object)DBNull.Value : passportNo.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@dateOfBirth", dateOfBirth.Trim() == "" ? (object)DBNull.Value : dateOfBirth.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dateOfJoin", dateOfJoin.Trim() == "" ? (object)DBNull.Value : dateOfJoin.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@maritalStatus", maritalStatus.Trim() == "" ? (object)DBNull.Value : maritalStatus.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@nationality", nationality.Trim() == "" ? (object)DBNull.Value : nationality.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@religon", religon.Trim() == "" ? (object)DBNull.Value : religon.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@eMail", eMail.Trim() == "" ? (object)DBNull.Value : eMail.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@epfNo", epfNo.Trim() == "" ? (object)DBNull.Value : epfNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@etfNo", etfNo.Trim() == "" ? (object)DBNull.Value : etfNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@permanentAddress", permanentAddress.Trim() == "" ? (object)DBNull.Value : permanentAddress.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@currentAddress", currentAddress.Trim() == "" ? (object)DBNull.Value : currentAddress.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@landPhone", landPhone.Trim() == "" ? (object)DBNull.Value : landPhone.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@personalMobile", personalMobile.Trim() == "" ? (object)DBNull.Value : personalMobile.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@officeMobile", officeMobile.Trim() == "" ? (object)DBNull.Value : officeMobile.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fuelCardNo", fuelCardNo.Trim() == "" ? (object)DBNull.Value : fuelCardNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo1", reportTo1.Trim() == "" ? (object)DBNull.Value : reportTo1.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo2", reportTo2.Trim() == "" ? (object)DBNull.Value : reportTo2.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reportTo3", reportTo3.Trim() == "" ? (object)DBNull.Value : reportTo3.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@city", city.Trim() == "" ? (object)DBNull.Value : city.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@distance", distance.Trim() == "" ? (object)DBNull.Value : distance.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@costCenter", costCenter.Trim() == "" ? (object)DBNull.Value : costCenter.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@profitCenter", profitCenter.Trim() == "" ? (object)DBNull.Value : profitCenter.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@isWelfare", isWelfare.Trim() == "" ? (object)DBNull.Value : isWelfare.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@attRegNo", isWelfare.Trim() == "" ? (object)DBNull.Value : attRegNo.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@resignedDate", resignedDate.Trim() == "" ? (object)DBNull.Value : resignedDate.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@designationId", designationId.Trim() == "" ? (object)DBNull.Value : designationId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@branchId", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@locationId", locationId.Trim() == "" ? (object)DBNull.Value : locationId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@nameInitials", nameInitials.Trim() == "" ? (object)DBNull.Value : nameInitials.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@knownName", knownName.Trim() == "" ? (object)DBNull.Value : knownName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@modCategory", modCategory.Trim() == "" ? (object)DBNull.Value : modCategory.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@probConEndDate", probConEndDate.Trim() == "" ? (object)DBNull.Value : probConEndDate.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@oTEligibility", isOTEligible.Trim() == "" ? (object)DBNull.Value : isOTEligible.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isRoster", isRoster.Trim() == "" ? (object)DBNull.Value : isRoster.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isMailExclude", isMailExclude.Trim() == "" ? (object)DBNull.Value : isMailExclude.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@otSession", otSession.Trim() == "" ? (object)DBNull.Value : otSession.Trim()));
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();




                sMySqlString = "UPDATE EMPLOYEE SET COMPANY_ID =@companyId,DEPT_ID=@departmentId,DIVISION_ID=@divisionId," +
                               "EMPLOYEE_STATUS=@employeeStatus,EMP_TYPE_ID=@employeeType,ROLE_ID=@employeeRoleId,TITLE=@title," +
                               "EMP_INITIALS=@initials,FIRST_NAME=@firstName,MIDDLE_NAMES=@middleName," +
                               "LAST_NAME=@lastName,FULL_NAME=@fullName,GENDER=@gender,EMP_NIC=@nicNo," +
                               "PASSPORT_NUMBER=@passportNo,DOB=@dateOfBirth,DOJ=@dateOfJoin," +
                               "MARITAL_STATUS=@maritalStatus,NAIONALITY=@nationality,RELIGION=@religon," +
                               "EMAIL=@eMail,EPF_NO=@epfNo,ETF_NO=@etfNo,PERMANENT_ADDRESS=@permanentAddress," +
                               "CURRENT_ADDRESS=@currentAddress,LAND_PHONE=@landPhone," +
                               "MOBILE_PHONE_PERSONAL=@personalMobile,MOBILE_PHONE_COMPANY=@officeMobile," +
                               "FUEL_CARD_NUMBER=@fuelCardNo,REPORT_TO_1=@reportTo1,REPORT_TO_2=@reportTo2," +
                               "REPORT_TO_3=@reportTo3,CITY=@city,DISTANCE_TO_OFFICE=@distance," +
                               "REMARKS=@remarks,MODIFIED_BY=@addedBy,MODIFIED_DATE=now(),STATUS_CODE='1', " +
                               "COST_CENTER=@costCenter,PROFIT_CENTER=@profitCenter," +
                               "IS_WELFARE=@isWelfare,RESIGNED_DATE=@resignedDate,BRANCH_ID=@branchId," +
                               "LOCATION_ID=@locationId,DESIGNATION_ID=@designationId," +
                               "INITIALS_NAME=@nameInitials,KNOWN_NAME=@knownName,MOD_CAT_ID=@modCategory,PROBATION_CONTRACT_ENDDATE=@probConEndDate,IS_OT_ELIGIBLE=@oTEligibility,IS_ROSTER=@isRoster,EXCLUDE_EMAIL=@isMailExclude,OT_SESSION=@otSession " +
                               " WHERE EMPLOYEE_ID=@employeeId";



                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;
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
            return blInserted;
        }


        //----------------------------------------------------------------------------------------
        ///<summary>
        ///To be used in Employee Transferes 
        ///</summary>
        ///<param name="companyId">New Company ID</param>
        ///<param name="departmentId">New Department ID</param>
        ///<param name="divisionId">New Division ID</param>
        ///<param name="branchId">New Branch ID</param>
        //----------------------------------------------------------------------------------------
        public Boolean UpdateTransfer(MySqlConnection mySqlCon_,
                      String employeeId,
                      String companyId,
                      String departmentId,
                      String divisionId,
                      String branchId,
                      String userID,
                      String toCC,
                      String toPC,
                      String toDesignation,
                      String toepf,
                      String trpt1,
                      String trpt2,
                      String trpt3)
        {
            Boolean blInserted = false;

            SerialHandler serialHandler = new SerialHandler();

            MySqlCommand cmd = mySqlCon_.CreateCommand();
            string sMySqlString = "";

            //MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@departmentId", departmentId.Trim() == "" ? (object)DBNull.Value : departmentId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@divisionId", divisionId.Trim() == "" ? (object)DBNull.Value : divisionId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@branchId", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toCCC", toCC.Trim() == "" ? (object)DBNull.Value : toCC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toPCC", toPC.Trim() == "" ? (object)DBNull.Value : toPC.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDesignation", toDesignation.Trim() == "" ? (object)DBNull.Value : toDesignation.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toepf", toepf.Trim() == "" ? (object)DBNull.Value : toepf.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt1", trpt1.Trim() == "" ? (object)DBNull.Value : trpt1.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt2", trpt2.Trim() == "" ? (object)DBNull.Value : trpt2.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@trpt3", trpt3.Trim() == "" ? (object)DBNull.Value : trpt3.Trim()));
           
            try
            {
                //mySqlCon.Open();

                //mySqlTrans = mySqlCon.BeginTransaction();




                sMySqlString = "UPDATE EMPLOYEE SET COMPANY_ID =@companyId, DEPT_ID=@departmentId, DIVISION_ID=@divisionId, BRANCH_ID=@branchId,COST_CENTER=@toCCC,PROFIT_CENTER=@toPCC, " +
                               " MODIFIED_BY=@userID, MODIFIED_DATE=now(), STATUS_CODE='1', DESIGNATION_ID=@toDesignation, EPF_NO=@toepf, ETF_NO=@toepf, REPORT_TO_1 = @trpt1, REPORT_TO_2 = @trpt2, REPORT_TO_3 = @trpt3 " +
                               " WHERE EMPLOYEE_ID=@employeeId";


                //mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                //mySqlTrans.Commit();

                //mySqlCon.Close();
                //mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;
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
            return blInserted;
        }



        public DataRow populate(string employeeId)
        {
            
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EMPLOYEE_ID,COMPANY_ID,DEPT_ID,DIVISION_ID,EMPLOYEE_STATUS,EMP_TYPE_ID, " +
                                      " ROLE_ID,TITLE,EMP_INITIALS,FIRST_NAME,MIDDLE_NAMES,LAST_NAME,FULL_NAME, " +
                                      " GENDER,EMP_NIC,PASSPORT_NUMBER,DOB,DOJ,MARITAL_STATUS,NAIONALITY,RELIGION, " +
                                      " EMAIL,EPF_NO,ETF_NO,PERMANENT_ADDRESS,CURRENT_ADDRESS,LAND_PHONE, " +
                                      " MOBILE_PHONE_PERSONAL,MOBILE_PHONE_COMPANY,FUEL_CARD_NUMBER,REPORT_TO_1,RESIGNED_DATE, " +
                                      " REPORT_TO_2,REPORT_TO_3,CITY,DISTANCE_TO_OFFICE,REMARKS,IS_WELFARE,ATT_REG_NO,COST_CENTER,PROFIT_CENTER, " +
                                      " BRANCH_ID,LOCATION_ID,DESIGNATION_ID,INITIALS_NAME,KNOWN_NAME,MOD_CAT_ID,PROBATION_CONTRACT_ENDDATE,ifnull(IS_OT_ELIGIBLE,'0') as IS_OT_ELIGIBLE,ifnull(IS_ROSTER,'0') as IS_ROSTER,ifnull(EXCLUDE_EMAIL,'0') as EXCLUDE_EMAIL,ifnull(OT_SESSION,'') as OT_SESSION  " +
                                      " FROM EMPLOYEE " +
                                      " where EMPLOYEE_ID ='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                
                mySqlDa.Fill(dataTable);
                DataRow dr = null;
                if (dataTable.Rows.Count > 0)
                {
                     dr = dataTable.Rows[0];
                }                            

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateEmpNIcName(string sEMP_NIC, string sEmpsts)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT a.EMPLOYEE_ID,a.EMP_NIC,a.EPF_NO,CONCAT(a.TITLE,a.EMP_INITIALS,'.',a.LAST_NAME) as mEmpName,a.EMAIL,b.USER_ID,a.COMPANY_ID " +
                                      " FROM EMPLOYEE a LEFT OUTER JOIN HRIS_USER b " +
                                      " ON a.EMPLOYEE_ID = b.EMPLOYEE_ID " +
                                      " WHERE a.EMP_NIC = '" + sEMP_NIC + "' and a.STATUS_CODE = '" + sEmpsts  + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public DataTable populateByEmpID(string employeeId, string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EMPLOYEE_ID, FIRST_NAME " +
                                      " FROM EMPLOYEE " +
                                      " where EMPLOYEE_ID ='" + employeeId.Trim() + "' and COMPANY_ID = '" + sCompID.Trim() + "'" ;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByComID( string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EMPLOYEE_ID, FIRST_NAME " +
                                      " FROM EMPLOYEE " +
                                      " where COMPANY_ID = '" + sCompID.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByDepId(string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EMPLOYEE_ID, FIRST_NAME " +
                                      " FROM EMPLOYEE " +
                                      " where DEPT_ID = '" + sDepID.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public DataTable populateEmpPhoto(string sEmployeeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT IMAGEPATH,IMAGENAME,IMAGEWIDTH,IMAGEHEIGHT " +
                                      " FROM EMPLOYEE_PHOTO " +
                                      " where EMPLOYEE_ID = '" + sEmployeeID + "' and IMAGES_STATUS ='" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateEmpBgImage(string sEmployeeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT IMAGEPATH,IMAGENAME,MENU_FONT_COLOR,MENU_FONT_SIZE,MENU_FONT_BLOD,MENU_FONT_ITALIC " +
                                      " FROM EMPLOYEE_PROFILE " +
                                      " where EMPLOYEE_ID = '" + sEmployeeID + "' and IMAGES_STATUS ='" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getEmployeeEmail(string employeeId)
        {

            string eMailAddress = "";

            mySqlCmd.CommandText = "SELECT ifnull(EMAIL,'') FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    eMailAddress = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }
            return eMailAddress;
        }

        public string getEmployeeKnownName(string employeeId)
        {

            string empName = "";

            mySqlCmd.CommandText = "SELECT ifnull(KNOWN_NAME,'') FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    empName = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }
            return empName;
        }

        public string getEmployeeName(string employeeId)
        {

            string empName = "";

            mySqlCmd.CommandText = "SELECT FULL_NAME as empName FROM EMPLOYEE  WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    empName = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return empName;
        }

        public DataRow getEmployeeCompanyAndName(string employeeId)
        {
            DataRow dr = null;
            dataTable.Rows.Clear();
            string sMySqlString = "";
            sMySqlString = "SELECT e.COMPANY_ID,c.COMP_NAME FROM EMPLOYEE as e,COMPANY as c WHERE c.COMPANY_ID =e.COMPANY_ID and e.EMPLOYEE_ID = '" + employeeId.Trim() + "'";
                
            try
            {
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }

                return dr;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }            
        }

        public Boolean isEpfExist(string companyId,string epfNo)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EPF_NO FROM EMPLOYEE WHERE COMPANY_ID='" + companyId.Trim() + "' and EPF_NO='" + epfNo.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isEpfExist(string companyId, string epfNo, string employeeId)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EPF_NO FROM EMPLOYEE WHERE COMPANY_ID='" + companyId.Trim() + "' and EPF_NO='" + epfNo.Trim() + "' and EMPLOYEE_ID <> '" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isEtfExist(string companyId, string etfNo)
        {
            try
            {
                Boolean isExist = false;
                try
                {
                    dataTable.Rows.Clear();
                    string sMySqlString = " SELECT ETF_NO FROM EMPLOYEE WHERE COMPANY_ID='" + companyId.Trim() + "' and ETF_NO ='" + etfNo.Trim() + "'";

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        isExist = true;
                    }

                    return isExist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isEtfExist(string companyId, string etfNo, string employeeId)
        {
            try
            {
                Boolean isExist = false;
                try
                {
                    dataTable.Rows.Clear();
                    string sMySqlString = " SELECT ETF_NO FROM EMPLOYEE WHERE COMPANY_ID='" + companyId.Trim() + "' and ETF_NO ='" + etfNo.Trim() + "' and EMPLOYEE_ID <> '" + employeeId.Trim() + "'";

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        isExist = true;
                    }

                    return isExist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isNicExist(string Nic)
        {
            try
            {
                Boolean isExist = false;
                try
                {
                    dataTable.Rows.Clear();
                    string sMySqlString = " SELECT EMP_NIC FROM EMPLOYEE where EMP_NIC='" + Nic.Trim() + "'";

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        isExist = true;
                    }

                    return isExist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isNicExist(string Nic, string employeeId)
        {
            try
            {
                Boolean isExist = false;
                try
                {
                    dataTable.Rows.Clear();
                    string sMySqlString = " SELECT EMP_NIC FROM EMPLOYEE where EMP_NIC='" + Nic.Trim() + "' and EMPLOYEE_ID <> '" + employeeId.Trim() + "'";

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        isExist = true;
                    }

                    return isExist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean isActiveEmployee(string employeeId)
        {
            try
            {
                Boolean isExist = false;
                try
                {
                    dataTable.Rows.Clear();
                    string sMySqlString = " SELECT * FROM EMPLOYEE where EMPLOYEE_ID = '" + employeeId.Trim() + "' and EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + "'" ;

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        isExist = true;
                    }

                    return isExist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string getEmployeeSupervisor(string employeeId)
        {

            string supervisor = "";

            mySqlCmd.CommandText = "SELECT IFNULL(REPORT_TO_1,'') REPORT_TO_1 FROM EMPLOYEE  WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    supervisor = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return supervisor;
        }


        public string getCompany(string employeeId)
        {

            string company = "";

            mySqlCmd.CommandText = "SELECT COMPANY_ID FROM EMPLOYEE  WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    company = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return company;
        }



        public DataTable populateModificationCategory()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DESCRIPTION,MOD_CAT_ID FROM EMPLOYEE_MODIFICATION_CATEGORY order by DESCRIPTION";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string getNextEpfNo(string companyId)
        {

            string epfNo = "";

            mySqlCmd.CommandText = "SELECT ifnull((cast(max(cast(EPF_NO as SIGNED)) + 1 as char)),'')  maxEpf FROM EMPLOYEE where COMPANY_ID ='" + companyId.Trim() + "'";
            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    epfNo = mySqlCmd.ExecuteScalar().ToString();

                    mySqlCmd.CommandText = "SELECT count(cast(EPF_NO as SIGNED)) FROM EMPLOYEE where COMPANY_ID ='" + companyId.Trim() + "'";
                    if (mySqlCmd.ExecuteScalar() != null)
                    {
                        if (mySqlCmd.ExecuteScalar().ToString() == "0")
                        {
                            epfNo = "1";
                        }
                    }
                    
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return epfNo;
        }

        public string getEmployeeCompany(string employeeId)
        {

            string sEmployeeId = "";

            mySqlCmd.CommandText = "SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sEmployeeId = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return sEmployeeId;
        }

        public DataTable getOtSessions()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT SESSOIN_ID,DESCRIPTION FROM OT_SESSOINS order by SESSOIN_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getDepartmentNameByEmployeeId(string employeeId)
        {
            string deptName = "";

            mySqlCmd.CommandText = "SELECT d.DEPT_NAME FROM EMPLOYEE e, DEPARTMENT d where e.DEPT_ID = d.DEPT_ID and EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    deptName = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return deptName;
        }

        public string getBranchNameByEmployeeId(string employeeId)
        {
            string branchName = "";

            mySqlCmd.CommandText = " SELECT b.BRANCH_NAME FROM EMPLOYEE e, COMPANY_BRANCH b where e.BRANCH_ID = b.BRANCH_ID and e.EMPLOYEE_ID = '" + employeeId.Trim() + "'";                
                
                

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    branchName = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return branchName;
        }


        public string getCompanyNameByEmployeeId(string employeeId)
        {
            string compName = "";

            mySqlCmd.CommandText = "SELECT c.COMP_NAME FROM EMPLOYEE e, COMPANY c where e.COMPANY_ID = c.COMPANY_ID and e.EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    compName = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return compName;
        }

        public DataRow getEmployeeDesignationAndEmail(string employeeId)
        {
            DataRow dr = null;
            dataTable.Rows.Clear();
            string sMySqlString = "";

            sMySqlString = " SELECT e.EMAIL,e.DESIGNATION_ID FROM EMPLOYEE e" +
                           " where e.EMPLOYEE_ID='" + employeeId.Trim() + "'";     
            try
            {
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
