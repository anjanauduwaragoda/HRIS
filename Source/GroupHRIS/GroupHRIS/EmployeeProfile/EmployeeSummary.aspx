<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSummary.aspx.cs"
    Inherits="GroupHRIS.EmployeeProfile.EmployeeSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="ProfileMainTable">
                    <tr>
                        <td class="EmpProfilePhotoTD">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#3366FF" Text="SUMMARY DETAILS"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="ProfileMainTable">
                    <tr>
                        <td class="ProfileMainTableTDCenter" valign="top" colspan="4">
                            <asp:Label ID="l2" runat="server" BackColor="#FFCC00" BorderWidth="1px" Height="20px"
                                Width="25px"></asp:Label>
                            &nbsp;<asp:Label ID="Label6" runat="server" Text="On Leave"></asp:Label>
                            &nbsp;

                            <asp:Label ID="l3" runat="server" BackColor="Red" BorderWidth="1px" Height="20px"
                                Width="25px"></asp:Label>
                            &nbsp;<asp:Label ID="Label5" runat="server" Text="Absent"></asp:Label>
                            &nbsp;

                            <asp:Label ID="l4" runat="server" BackColor="#009900" BorderWidth="1px" Height="20px"
                                Width="25px"></asp:Label>
                            &nbsp;<asp:Label ID="Label7" runat="server" Text="Missing Record"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            <asp:Label ID="Label2" runat="server" Text="From : "></asp:Label>
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            <asp:Label ID="Label3" runat="server" Text="To: "></asp:Label>
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            &nbsp;
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate"
                                Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            <asp:TextBox ID="txttodate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="cetodate" runat="server" TargetControlID="txttodate" Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="ftetodate" runat="server" TargetControlID="txttodate"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="ProfileMainTableTDCenter" valign="top">
                            <asp:Button ID="btnview" runat="server" OnClick="btnview_Click" Text="View Summary" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ProfileMainTableTDCenter" valign="top" colspan="4">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <img src="../Images/ProBar/720.GIF" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="4" align="center">
                            <asp:Literal ID="LiteralSE1" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table class="ProfileMainTable">
                    <tr>
                        <td class="EmpProfilePhotoTD" align="center">
                            <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
