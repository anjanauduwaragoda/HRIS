using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Reports
{
    public class TrainingNeedsReport : TemplateDataHandler
    {
        public DataTable getAllRequest()
        {
            try
            {
                DataTable resultTable = new DataTable();

                string sMySqlString = @"SELECT 
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
	                                        REQUEST_TYPE RT ON TR.REQUEST_TYPE = RT.REQUEST_TYPE_ID ";                                     

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

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
    }
}
