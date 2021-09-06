<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveManagement.aspx.cs"
    Inherits="GroupHRIS.EmployeeProfile.LeaveManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
        <table class="LeaveManageMainTB">
            <tr>
                <td class="LeaveManageMainTDLeft">
                    &nbsp;</td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;</td>
                <td class="LeaveManageMainTDRight" rowspan="10" valign="bottom">
                <asp:Chart ID="chrtLeaveBalance" runat="server" Height="200px"
                    Width="250px" BorderlineColor="Transparent">
                    <Series>
                        <asp:Series BackGradientStyle="HorizontalCenter" 
                            BackSecondaryColor="GreenYellow" BorderColor="ForestGreen"
                            Color="GreenYellow" IsValueShownAsLabel="True" Name="noofleave" 
                            ChartType="Bar">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea BackColor="Transparent" Name="ChartArea1" 
                            BackSecondaryColor="Transparent">
                            <AxisY Enabled="False">
                                <MajorGrid LineDashStyle="NotSet" />
                            </AxisY>
                            <AxisX IsLabelAutoFit="False" LabelAutoFitMaxFontSize="8" LineColor="0, 192, 0" LineWidth="2">
                                <MajorGrid LineDashStyle="NotSet" />
                                <MinorGrid LineDashStyle="NotSet" />
                                <LabelStyle Font="Microsoft Sans Serif, 6pt" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    &nbsp;</td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    &nbsp;
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:Label ID="lblrefcode" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    Employee ID :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:Label ID="lblepf" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    Name :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:Label ID="lblfirstname" runat="server"></asp:Label>
                    &nbsp;<asp:Label ID="lbllastname" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    From Date :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate"
                        Format="yyyy-MM-dd">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate"
                        FilterType="Custom, Numbers" ValidChars="-">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    To Date :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:TextBox ID="txttodate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    <asp:CalendarExtender ID="cetodate" runat="server" TargetControlID="txttodate" Format="yyyy-MM-dd">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="ftetodate" runat="server" TargetControlID="txttodate"
                        FilterType="Custom, Numbers" ValidChars="-">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    No of Days :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:TextBox ID="TextBox1" runat="server" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    Reason :
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:DropDownList ID="ddlreason" runat="server" Width="256px" Height="20px">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                </td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                </td>
                <td class="LeaveManageMainTDRight">
                    <asp:Button ID="btnaddleave" runat="server" OnClick="btngeneratecalendar_Click" Text="Add Leave"
                        Width="100px" />
                </td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="LeaveManageMainTDLeft">
                    &nbsp;
                </td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;
                </td>
                <td class="LeaveManageMainTDRight">
                    &nbsp;</td>
            </tr>
        </table>
        <table class="EmpProfilePhotoTB">
            <tr>
                <td class="EmpProfilePhotoTD" align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                        BorderColor="#0099FF" BorderStyle="Groove" BorderWidth="2px" CellPadding="3"
                        CellSpacing="1" ForeColor="Black" OnRowDataBound="GridView2_RowDataBound" >
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="#0099FF" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#CCCCCC" BorderColor="#0099FF" BorderWidth="1px" ForeColor="Black"
                            HorizontalAlign="Left" />
                        <RowStyle BackColor="White" BorderColor="#3366FF" BorderWidth="1px" />
                        <SelectedRowStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                        <Columns>
                            <asp:BoundField HeaderText="Leave Date" DataField="LeaveDate" />
                            <asp:BoundField HeaderText="From - To" DataField="Fromto" />
                            <asp:BoundField HeaderText="Day" DataField="Dayofweek" />
                            <asp:BoundField DataField="Isholiday" HeaderText="Holiday" />
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlscore" runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mark">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkmark" runat="server">
                                    </asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoTD" align="center">
                    <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
