<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reminders.aspx.cs" Inherits="GroupHRIS.ExtraOptions.Reminders"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <table style="width: 900px">
            <tr>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    ADD REMINDERS
                </td>
            </tr>
        </table>
        <table style="width: 900px">
            <tr>
                <td style="width: 100px">
                    Reminder Date :
                </td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtbdate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtbdate" Format="yyyy/MM/dd">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtbdate"
                        FilterType="Custom, Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                    <asp:Label ID="lblcode" runat="server" Visible="False"></asp:Label>
                </td>
                <td style="width: 400px" rowspan="5" valign="top">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                        OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                        AutoGenerateColumns="False" PageSize="5" Style="width: 400px;">
                        <Columns>
                            <asp:BoundField DataField="idno" HeaderText="ID No.">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EXPDATE" HeaderText="Reminder Date">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description/Reminder">
                                <ItemStyle Width="300px" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings PageButtonCount="2" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Reminder Details :
                </td>
                <td>
                    <asp:TextBox ID="txtdescription" runat="server" Height="150px" TextMode="MultiLine"
                        Width="300px" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Reminder Status :
                </td>
                <td>
                    <asp:DropDownList ID="txtstatus" runat="server" Width="100px">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">Cancel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnupdate" runat="server" Text="Save" Width="100px" OnClick="btnupdate_Click" />
                    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" Width="91px" Height="26px"
                        OnClick="btnrefresh_Click" />
                </td>
            </tr>
        </table>
    </div>
    <table class="EmpProfilePhotoTB">
        <tr>
            <td class="EmpProfilePhotoTD" align="center">
                <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
