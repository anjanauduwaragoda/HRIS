using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.UserAdministration;
using System.Data;
using Common;

namespace GroupHRIS.Useradmin
{
    public partial class Wedfrmpriviledges : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUserRoles();
                string mUserrole = ddlUserrole.SelectedItem.Text.ToString();
                if (mUserrole != "")
                {
                    getmainnodes();
                    getsubnodes();
                }
                
            }
        }

        private void GetUserRoles()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable userrole = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlUserrole.Items.Add(listItemBlank);

                userrole = UserAdministration.populateuserRoles();
                for (int x = 0; x < userrole.Rows.Count; x++)
                {
                    ddlUserrole.Items.Add(userrole.Rows[x][0].ToString() + "-" + userrole.Rows[x][1].ToString());
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                UserAdministration = null;
                userrole.Dispose();
                userrole = null;
            }

        }

        private void getsubnodes()
        {
            DataTable maccessrights = new DataTable();
            DataTable msubnodes = new DataTable();
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();

            try
            {
                chklistrestricted.Items.Clear();

                string mMainNode = rdolistmainnode.SelectedItem.Text.Substring(0, 8).Trim();

                msubnodes = UserAdministration.populateuserSubNodesForAccess(mMainNode).Copy();
                int rcount = msubnodes.Rows.Count;

                for (int i = 0; i < msubnodes.Rows.Count; i++)
                {
                    string mSubNode = msubnodes.Rows[i][3].ToString();
                    string mSubNodedescription = msubnodes.Rows[i][1].ToString();
                    maccessrights = UserAdministration.populateaccessrights(ddlUserrole.Text.Substring(0, 8).ToString(), mMainNode, mSubNode);
                    if (maccessrights.Rows.Count > 0)
                    {
                        chklistrestricted.Items.Add(mSubNode + "-" + mSubNodedescription);
                        chklistrestricted.Items[i].Selected = true;
                    }
                    else
                    {
                        chklistrestricted.Items.Add(mSubNode + "-" + mSubNodedescription);
                    }
                }

                msubnodes.Dispose();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                UserAdministration = null;
                msubnodes.Dispose();
                msubnodes = null;
                maccessrights.Dispose();
                maccessrights = null;
            }
        }

        private void getmainnodes()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable Mmainnodes = new DataTable();
            try
            {
                Mmainnodes = UserAdministration.populateuserMainNodes();
                for (int x = 0; x < Mmainnodes.Rows.Count; x++)
                {
                    rdolistmainnode.Items.Add(Mmainnodes.Rows[x][0].ToString() + "-" + Mmainnodes.Rows[x][1].ToString());
                }
                rdolistmainnode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                UserAdministration = null;
                Mmainnodes.Dispose();
                Mmainnodes = null;
            }
        }        
      
        protected void btnupdate_Click(object sender, EventArgs e)
        {
           
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable mDtAccess = new DataTable();
            mDtAccess.Clear();
            mDtAccess.Columns.Add("Userrole", typeof(string));
            mDtAccess.Columns.Add("MainNode", typeof(string));
            mDtAccess.Columns.Add("SubNode", typeof(string));

            string madddate = DateTime.Today.ToString("yyyy/MM/dd");
            string mlogUser = (string)(Session["KeyUSER_ID"]);
            string mUserrole = ddlUserrole.Text.ToString();

            try
            {

                if (mUserrole == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User Role Can not Blank. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else{
                    mUserrole = ddlUserrole.Text.Substring(0, 8).ToString();
                    string mMainNode = rdolistmainnode.SelectedItem.Text.Substring(0, 8).Trim();
                    UserAdministration.UpdateAccess(mUserrole, mMainNode); 

                    foreach (ListItem Itemselect in chklistrestricted.Items)
                    {
                        if (Itemselect.Selected == true)
                        {
                            mDtAccess.Rows.Add(mUserrole, mMainNode, Itemselect.Text.Substring(0, 8).ToString());
                        }
                    }

                    UserAdministration.InsertUserAccess(mDtAccess, Constants.STATUS_ACTIVE_VALUE, "", mlogUser, madddate);
                    CommonVariables.MESSAGE_TEXT = "User role / function granted";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

            finally
            {
                mDtAccess.Dispose();
                mDtAccess =null;
                UserAdministration = null;
            }

        }

        protected void rdolistmainnode_SelectedIndexChanged(object sender, EventArgs e)
        {
                try
                {
                    Utility.Errorhandler.ClearError(lblerror);
                    getsubnodes();
                }
                catch (Exception ex)
                {
                    CommonVariables.MESSAGE_TEXT = ex.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
        }

        protected void ddlUserrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rdolistmainnode.Items.Clear();
                chklistrestricted.Items.Clear();


                Utility.Errorhandler.ClearError(lblerror);
                string mUserrole = ddlUserrole.SelectedItem.Text.ToString();
                if (mUserrole != "")
                {
                    getmainnodes();
                    getsubnodes();
                }
                else
                {
                    rdolistmainnode.Items.Clear();
                    chklistrestricted.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }        
    }
}