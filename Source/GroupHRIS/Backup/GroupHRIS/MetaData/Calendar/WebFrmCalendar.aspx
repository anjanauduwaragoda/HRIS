<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmCalendar.aspx.cs" Inherits="GroupHRIS.MetaData.Calendar.WebFrmCalendar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCalendar.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function openpopupSecondary(file, window) {
            childWindow = open(file, window, 'resizable=no,width=850,height=450,scrollbars=yes,top=150,left=250,status=yes');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td>
                <span style="font-weight: 700">Company Calendar</span>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTbTDLeft">
                Company Name :
            </td>
            <td class="styleMainTbTDMiddle">
                <asp:DropDownList ID="ddlCompID" runat="server" Height="20px" Width="300px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCompID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="styleMainTbTDRight">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Calander/Events-Calendar-icon.png"
                    onclick="openpopupSecondary('WebFrmCompanyCalendar.aspx','Search')" ToolTip="View Company Calendar" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft">
                Calendar Date From :
            </td>
            <td class="styleMainTbTDMiddle">
                <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px" 
                   ></asp:TextBox>
                <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate"
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="styleMainTbTDRight">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtfromdate"
                    ErrorMessage="Calendar From Date is required." ForeColor="Red" ValidationGroup="vgCalendarInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft">
                Calendar Date To :
            </td>
            <td class="styleMainTbTDMiddle">
                <asp:TextBox ID="txttodate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                <asp:CalendarExtender ID="cetodate" runat="server" TargetControlID="txttodate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="ftetodate" runat="server" TargetControlID="txttodate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="styleMainTbTDRight">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txttodate"
                    ErrorMessage="Calendar To Date is required." ForeColor="Red" ValidationGroup="vgCalendarInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft">
                &nbsp;
            </td>
            <td class="styleMainTbTDMiddle">
                <asp:Button ID="btngeneratecalendar" runat="server" OnClick="btngeneratecalendar_Click"
                    Text="Generate Calendar" Width="149px" ValidationGroup="vgCalendarInformation" />
            </td>
            <td class="styleMainTbTDRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="vgCalendarInformation"
                    Width="516px" />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: 200px">
                    <ProgressTemplate>
                        <img src="../../Images/ProBar/720.GIF" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td>
                <span style="font-weight: 700">Company Holidays</span>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTbTDLeft" valign="top">
                Holiday Type :
            </td>
            <td class="styleMainTbTDMiddle" valign="top">
                <asp:DropDownList ID="ddlholidaytype" runat="server" Height="25px" Width="255px">
                </asp:DropDownList>
            </td>
            <td class="styleMainTbTDRight" rowspan="5" align="center" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Calendar ID="calcompany" runat="server" BorderColor="Black" Height="16px" OnDayRender="calcompany_DayRender" Width="226px" BorderStyle="Double"
                            BorderWidth="1px" OnVisibleMonthChanged="calcompany_VisibleMonthChanged">
                            <DayHeaderStyle BackColor="#666699" ForeColor="White" />
                            <DayStyle BackColor="White" BorderColor="Black" BorderWidth="1px" />
                            <SelectedDayStyle BackColor="#00CC00" />
                            <WeekendDayStyle BackColor="#3366FF" ForeColor="White" />
                        </asp:Calendar>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft" valign="top">
                Company Holiday Date :
            </td>
            <td class="styleMainTbTDMiddle" valign="top">
                <asp:TextBox ID="lblday" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="lblday" Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="lblday"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Company Holiday Date  is required."
                    ForeColor="Red" ControlToValidate="lblday" 
                    ValidationGroup="vgCalendarIHoliday">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft">
                &nbsp;
            </td>
            <td class="styleMainTbTDMiddle">
               <asp:Label ID="lblmsg" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbTDLeft">
                &nbsp;
            </td>
            <td class="styleMainTbTDMiddle" valign="top">
                <asp:Button ID="btnholiday" runat="server" Text="Update Holidays" Width="149px" OnClick="btnholiday_Click"
                    ValidationGroup="vgCalendarIHoliday" />
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top" align="left">
                <table class="styleMainTb">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" Height="19px"
                                ValidationGroup="vgCalendarIHoliday" Width="516px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-weight: 700">Holiday Details</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                            <asp:Literal ID="ltlHolidays" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
