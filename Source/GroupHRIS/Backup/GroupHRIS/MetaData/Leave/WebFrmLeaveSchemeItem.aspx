<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmLeaveSchemeItem.aspx.cs"
    Inherits="GroupHRIS.MetaData.Leave.WebFrmLeaveSchemeItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Leave Scheme Item</title>
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleMaster.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleScroller.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function sendValueToParent() {
            window.close();
            return false;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="background: white; margin-top: 50px; margin-left: 0px; left: 0px; width: 100%;
                height: 100%; position: absolute;">
                <table style="margin: auto; position: absolute; top: 100px; left: 35%;">
                    <tr>
                        <td>
                            Leave Scheme Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="LeaveSchemeTypeDropDownList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Number of Days :
                        </td>
                        <td>
                            <asp:TextBox ID="NumberOfDaysTextBox" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="NumberOfDaysFilteredTextBoxExtender" TargetControlID="NumberOfDaysTextBox"
                                FilterType="Numbers" runat="server">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remarks :
                        </td>
                        <td>
                            <asp:TextBox ID="RemarksTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td>
                            <asp:Button ID="AddButton" runat="server" Text="Add" OnClick="AddButton_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table style="margin: auto; margin-top: 200px;">
                    <tr>
                        <td>
                            <asp:GridView ID="LeaveSchemeItemGridView" runat="server">
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="ButtonPanel" Visible="false" runat="server">
                                <input id="Button1" type="button" value="Done" onclick="window.close();" />
                                <input id="CancelButton" type="button" value="Cancel" onclick="window.close();" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
