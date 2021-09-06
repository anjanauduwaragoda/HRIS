using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.UserAdministration;
using System.Data;
using DataHandler.Property;
using GroupHRIS.Utility;

namespace GroupHRIS.Property
{
    public partial class WebFrmDefaultPropertySet : System.Web.UI.Page
    {
        #region  events 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getUserRoles();
            }
        }

        protected void ddlUserrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblerror);
            chklistrestricted.Items.Clear();
            chkActive.Checked = false;
            hfroleId.Value = ddlUserrole.SelectedValue.ToString();

            try
            {
                DefaultPropertyDataHandler oDefaultPropertyDataHandler = new DefaultPropertyDataHandler();
                DataTable propertyTable = new DataTable();

                propertyTable = oDefaultPropertyDataHandler.getPropertyList().Copy();

                for(int x = 0; x < propertyTable.Rows.Count; x++)
                {
                    string id = propertyTable.Rows[x][0].ToString();
                    string text = propertyTable.Rows[x][1].ToString();

                    chklistrestricted.Items.Add(text);

                    DataTable checkedTable = new DataTable();
                    checkedTable = oDefaultPropertyDataHandler.getSelectedData(hfroleId.Value).Copy();
                    Session["checkedCount"] = checkedTable.Rows.Count;
                   // int xx = Convert.ToInt32(Session["checkedCount"]); ;

                    bool exists = checkedTable.Select().ToList().Exists(row => row["TYPE_ID"].ToString().ToUpper() == id);
                    if (exists == true)
                    {
                        chklistrestricted.Items[x].Selected = true;
                    } 

                    if (checkedTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in checkedTable.Rows)
                        {
                            string value = row["STATUS_CODE"].ToString();
                            if (value == "1")
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                        }
                    }

                }

                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            DefaultPropertyDataHandler oDefaultPropertyDataHandler = new DefaultPropertyDataHandler();
            DataTable propertyTable = new DataTable();

            propertyTable = oDefaultPropertyDataHandler.getPropertyList().Copy();

            string userRole = hfroleId.Value;
            string status = "0";
            string logUser = Session["KeyUSER_ID"].ToString();

            if (chkActive.Checked == true)
            {
                status = "1";
            }

            int x = 0;

            int c = 0;

            try
            {
                foreach (ListItem item in chklistrestricted.Items)
                {
                    if (item.Selected == true)
                    {
                        string property = propertyTable.Rows[x][0].ToString();
                        Boolean IsInserted = oDefaultPropertyDataHandler.insert(userRole, property, logUser, status);
                     }
                    else if (item.Selected == false)
                    {
                        string property = propertyTable.Rows[x][0].ToString();
                        Boolean IsInserted = oDefaultPropertyDataHandler.delete(userRole, property);
                        c++;
                    }
                    x++;
                }

                if (chklistrestricted.Items.Count == c && Convert.ToInt32(Session["checkedCount"]) == 0)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "At Least One Benefit Required", lblerror);
                    return;
                }

                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblerror);
            }
            catch (Exception ex)
            {
                //Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Alredy Exist", lblMessage);
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

            //chkActive.Checked = false;
        }

        #endregion

        #region

        private void getUserRoles()
        {
            DefaultPropertyDataHandler oDefaultPropertyDataHandler = new DefaultPropertyDataHandler();
            DataTable propertyTable = new DataTable();

            try
            {
                propertyTable = oDefaultPropertyDataHandler.getRoleList().Copy();

                ddlUserrole.Items.Clear();

                if (propertyTable.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlUserrole.Items.Add(Item);

                    foreach (DataRow dataRow in propertyTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["ROLE_NAME"].ToString();
                        listItem.Value = dataRow["ROLE_ID"].ToString();

                        ddlUserrole.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

        }

        #endregion

    }
}