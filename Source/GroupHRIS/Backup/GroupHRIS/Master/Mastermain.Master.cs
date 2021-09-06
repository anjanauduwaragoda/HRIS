using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.UserAdministration;
using System.Data;
using Common;
using DataHandler.Employee;
using System.Drawing;

namespace HRIS
{
    public partial class Mastermain : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["KeyLOGOUT_STS"].Equals("0"))
                    {
                        Response.Redirect("MainLogout.aspx", false);
                    }
                    else
                    {
                        string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                        string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);
                        string KeyUSER_FIRSTNAME = (string)(Session["KeyUSER_FIRSTNAME"]);
                        string KeyHRIS_ROLE = (string)(Session["KeyHRIS_ROLE"]);
                        string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                        string KeyLOGOUT_STS = (string)(Session["KeyLOGOUT_STS"]);
                        string KeyEMPLOYEE_GENDER = (string)(Session["KeyEMPLOYEE_GENDER"]);

                        get_usersummary();
                        GetuserPrivilages(KeyHRIS_ROLE);
                        GetuserGender(KeyEMPLOYEE_ID);
                        GetProfileBgImage(KeyEMPLOYEE_ID);
                        //GetScroller(KeyCOMP_ID);
                        lblwelcome.Text = "Welcome ! " + KeyUSER_FIRSTNAME + "   ";
                        lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT;
                    }

                }
            }
            catch (Exception)
            {
                logoutme();
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
        }

        private void get_usersummary()
        {
            //Label lblusersummary = new Label();

            //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            //string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);
            //string KeyUSER_FIRSTNAME = (string)(Session["KeyUSER_FIRSTNAME"]);
            //string KeyHRIS_ROLE = (string)(Session["KeyHRIS_ROLE"]);
            //string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
            //string KeyLOGOUT_STS = (string)(Session["KeyLOGOUT_STS"]);

            //lblusersummary.Text = "<br>" + "Employee ID : " + KeyEMPLOYEE_ID;
            //lblusersummary.Text = lblusersummary.Text + "<br>" + "User ID     : " + KeyUSER_ID;
            //lblusersummary.Text = lblusersummary.Text + "<br>" + "User Name   : " + KeyUSER_FIRSTNAME;
            //lblusersummary.Text = lblusersummary.Text + "<br>" + "Role ID     : " + KeyHRIS_ROLE;
            //lblusersummary.Text = lblusersummary.Text + "<br>" + "Company ID  : " + KeyCOMP_ID;
            //phusersummary.Controls.Add(lblusersummary);

        }

        private void GetuserGender(string sEMPLOYEE_ID)
        {
            string KeyEMPLOYEE_GENDER = (string)(Session["KeyEMPLOYEE_GENDER"]);
            if (KeyEMPLOYEE_GENDER == Constants.CON_GENDER_FEMALE)
            {
                // imgbtnuser.ImageUrl = "~/Images/MasterPage/woman-icon.png";
            }
            else if (KeyEMPLOYEE_GENDER == Constants.CON_GENDER_MALE)
            {
                //  imgbtnuser.ImageUrl = "~/Images/MasterPage/man-icon.png";
            }
        }

        private void GetuserPrivilages(string sROLE_ID)
        {            
            string mMainnode = "";
            string mMainNodeDescrip = "";
            string mSubNodeDescrip = "";
            DataTable mUserPrivilage = new DataTable();
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();

            try
            {
                //if (Session["KeyAccessData"] == null)
                //{
                //    mUserPrivilage = UserAdministration.populateuserprivilage(sROLE_ID);
                //    Session["KeyAccessData"] = mUserPrivilage;
                //}
                //else
                //{
                //      mUserPrivilage = (DataTable)Session["KeyAccessData"];
                //}

                 mUserPrivilage = UserAdministration.populateuserprivilage(sROLE_ID);

                DataTable mMainnodeTb = mUserPrivilage.Copy();

                int j = 0;
                for (int i = 0; i < mUserPrivilage.Rows.Count; i++)
                {
                    mMainNodeDescrip = mUserPrivilage.Rows[i][1].ToString();

                    if (mUserPrivilage.Rows[i][0].ToString().ToUpper() != mMainnode)
                    {
                        mMainnode = mUserPrivilage.Rows[i][0].ToString();

                        TreeNode ParentNode = new TreeNode();
                        ParentNode.Collapse();
                        ParentNode.Text = mMainNodeDescrip;
                        trvAccess.Nodes.Add(ParentNode);
                        DataRow[] result = mMainnodeTb.Select("tmaincode = '" + mMainnode + "' ");
                        foreach (DataRow row in result)
                        {
                            mSubNodeDescrip = mMainnodeTb.Rows[j][2].ToString();
                            TreeNode ChildNode1 = new TreeNode();
                            ChildNode1.Text = mSubNodeDescrip;
                            ChildNode1.NavigateUrl = mMainnodeTb.Rows[j][3].ToString();
                            ParentNode.ChildNodes.Add(ChildNode1);
                            //ChildNode1.ImageUrl = "/Images/MasterPage/LinkIcoChild.gif";
                            j++;
                        }
                    }
                }
                mMainnodeTb.Dispose();
            }
            catch (Exception)
            {

                logoutme();
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
            finally
            {
                UserAdministration = null;
                mUserPrivilage.Dispose();
            }
        }

        protected void imgbtnlogout_Click(object sender, ImageClickEventArgs e)
        {
            logoutme();
            Response.Redirect("~/Login/MainLogout.aspx", false);

        }

        private void GetScroller(string sCOMP_ID)
        {
            string strMARQUEE1 = "";

            strMARQUEE1 = strMARQUEE1 + "<div id='scroller'>";
            strMARQUEE1 = strMARQUEE1 + "<div id='wn'>";
            strMARQUEE1 = strMARQUEE1 + "<div id='lyr'>";

            DataHandler.MetaData.CompanyDataHandler companydatahandler = new DataHandler.MetaData.CompanyDataHandler();
            DataTable companymotto = companydatahandler.getCompanyMotto(sCOMP_ID);
            if (companymotto.Rows.Count > 0)
            {
                strMARQUEE1 = strMARQUEE1 + "<div class='block'><font size='3'>" + companymotto.Rows[0][0].ToString() + "</font></div>";
                strMARQUEE1 = strMARQUEE1 + "<div class='block'>" + companymotto.Rows[0][1].ToString() + "</div>";
                strMARQUEE1 = strMARQUEE1 + "<div class='block'>" + companymotto.Rows[0][2].ToString() + "</div>";
                strMARQUEE1 = strMARQUEE1 + "<div class='block'><font size='3'>" + companymotto.Rows[0][3].ToString() + "</font></div>";
                strMARQUEE1 = strMARQUEE1 + "<div id='rpt' class='block'><font size='3'>" + companymotto.Rows[0][0].ToString() + "</font></div>";
                strMARQUEE1 = strMARQUEE1 + "</div></div></div>";
                lblmerqee.BackColor = System.Drawing.Color.Transparent;
                lblmerqee.Text = strMARQUEE1;

            }
        }

        private void logoutme()
        {
            Session["KeyLOGOUT_STS"] = "0";
            Session["KeyEMPLOYEE_ID"] = "";
            Session["KeyUSER_ID"] = "";
            Session["KeyUSER_FIRSTNAME"] = "";
            Session["KeyHRIS_ROLE"] = "";
            Session["KeyCOMP_ID"] = "";
            //Session["KeyAccessData"] = null;
        }

        private void GetProfileBgImage(string KeyEMPLOYEE_ID)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable employeeBgImage = new DataTable();
            string backgroundImage = Constants.CON_DEFAULT_BG_IMAGE;
            string sFontsize = "";
            string sFontColor = "";
            bool sFontBold = false;
            bool SfontItalic = false;
            string sHtmlColor = "";

            try
            {
                employeeBgImage = employeeDataHandler.populateEmpBgImage(KeyEMPLOYEE_ID);
                if (employeeBgImage.Rows.Count > 0)
                {
                    DataRow dr = employeeBgImage.Rows[0];
                    backgroundImage = dr["IMAGEPATH"].ToString() + dr["IMAGENAME"].ToString();
                    sFontColor = dr["MENU_FONT_COLOR"].ToString();
                    sFontsize = dr["MENU_FONT_SIZE"].ToString();
                    if (dr["MENU_FONT_BLOD"].ToString() == "Y")
                    {
                        sFontBold = true;
                    }
                    if (dr["MENU_FONT_ITALIC"].ToString() == "Y")
                    {
                        SfontItalic = true;
                    }

                    dr = null;
                    MasterBody.Style.Add("background-image", "" + backgroundImage + "");
                    Color col = ColorTranslator.FromHtml(String.Format("#{0}", sFontColor));
                    sHtmlColor = System.Drawing.ColorTranslator.ToHtml(col);
                    trvAccess.LeafNodeStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(sHtmlColor);
                    trvAccess.NodeStyle.Font.Size = int.Parse(sFontsize);
                    trvAccess.NodeStyle.Font.Bold = sFontBold;
                    trvAccess.NodeStyle.Font.Italic = SfontItalic;
                }
                else
                {
                    MasterBody.Style.Add("background-image", "" + backgroundImage + "");
                    MasterBody.Style.Add("background-repeat", "repeat-x");
                    trvAccess.LeafNodeStyle.ForeColor = Color.Aqua;

                }
            }
            catch (Exception)
            {
                logoutme();
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
            finally
            {
                employeeDataHandler = null;
                employeeBgImage.Dispose();
                employeeBgImage = null;

            }
        }
    }
}