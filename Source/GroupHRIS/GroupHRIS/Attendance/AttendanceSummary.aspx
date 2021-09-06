<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="AttendanceSummary.aspx.cs" Inherits="GroupHRIS.Attendance.AttendanceSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("txtemployee").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("txtemployee").value;
            document.getElementById(ctl).value = sRetVal;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTb">
                <span style="font-weight: 700">HRIS Employee Summary</span>
            </td>
        </tr>
        <tr>
            <td class="styleMainTb">
                <hr />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleMainTb">
                <tr>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        Company :
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:DropDownList ID="dpCompID" runat="server" Width="200px" OnSelectedIndexChanged="dpCompID_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        From Date :
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate"
                            Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        To Date :
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:TextBox ID="txttodate" runat="server" MaxLength="10" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <asp:CalendarExtender ID="cetodate" runat="server" TargetControlID="txttodate" Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ftetodate" runat="server" TargetControlID="txttodate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        Employee :
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:TextBox ID="txtemployee" runat="server" ClientIDMode="Static" Width="150px"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtemployee')" />
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:CheckBox ID="chkemployee" runat="server" AutoPostBack="True" Font-Bold="False"
                            ForeColor="Blue" OnCheckedChanged="chkemployee_CheckedChanged" Text="Selected Employee Only" />
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td class="attensummaryTDRight">
                        <asp:Button ID="btngeneratecalendar" runat="server" OnClick="btngeneratecalendar_Click"
                            Text="Process Summary" Width="149px" />
                        <asp:Button ID="btnclose" runat="server" OnClick="btnclose_Click" Text="Clear" Width="97px" />
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                    <td class="attensummaryTDRight">
                        &nbsp;
                    </td>
                    <td class="attensummaryTD">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table class="styleMainTb">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
