<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AttendanceInOut.aspx.cs" Inherits="GroupHRIS.Attendance.AttendanceInOut" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .styleDdlhh
        {
            width: 50px;
            color: Blue;
        }
        .style2
        {
            width: 25%;
            text-align: left;
            table-layout: fixed;
            color: #3366FF;
        }
        .style3
        {
            height: 27px;
        }
        .style5
        {
            width: 25%;
            text-align: left;
            table-layout: fixed;
            height: 26px;
        }
        .styleTableCell2TextBox
        {
            width: 250px;
            text-align: left;
        }
    </style>
    <script language="javascript" type="text/javascript">

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//        }

//        function getValueFromChild(sEmpId, sName) {
//            document.getElementById("txtRecommendBy").value = sEmpId;
//            document.getElementById("txtRecommendByName").value = sName;

//        }

        //        function DoPostBack() {
        //            __doPostBack("txtRecommendBy", "TextChanged");
        //        }


        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleMainTb">
        <tr>
            <td class="styleMainTb">
                <span style="font-weight: 700">HRIS Employee IN/OUT</span>
            </td>
        </tr>
        <tr>
            <td class="styleMainTb">
                <hr />
            </td>
        </tr>
    </table>
    
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
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
                Employee :
            </td>
            <td class="attensummaryTDRight">
                <asp:Label ID="txtemployee" runat="server" Font-Bold="True" ForeColor="#009900"></asp:Label>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                Recommend By :
            </td>
            <td >
                <asp:TextBox ID="txtRecommendBy" runat="server" ReadOnly="true" MaxLength="8" ClientIDMode="Static"
                    Width="198px"></asp:TextBox>
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtRecommendBy')" />
            </td>
            <td>
                <asp:TextBox ID="txtRecommendByName" runat="server" BorderStyle="None"  
                    ClientIDMode="Static" ForeColor="Blue"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                Company :
            </td>
            <td class="style5">
                <asp:DropDownList ID="dpCompID" runat="server" Width="200px" OnSelectedIndexChanged="dpCompID_SelectedIndexChanged"
                    AutoPostBack="True" Height="22px">
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                IN / OUT :
            </td>
            <td class="attensummaryTDRight">
                <asp:DropDownList ID="ddlinout" runat="server" Height="22px">
                    <asp:ListItem Value="1">IN</asp:ListItem>
                    <asp:ListItem Value="0">OUT</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;Date :
            </td>
            <td class="attensummaryTDRight">
                <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate" Format="yyyy/MM/dd">
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
                Location :
            </td>
            <td class="attensummaryTDRight">
                <asp:DropDownList ID="dplocation" runat="server" Width="200px" OnSelectedIndexChanged="dpCompID_SelectedIndexChanged"
                    Height="22px">
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="style2">
                <strong>Hours&nbsp;&nbsp;&nbsp;Minutes</strong>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                Time :
            </td>
            <td class="attensummaryTDRight">
                <asp:DropDownList ID="ddlFromHH" runat="server" CssClass="styleDdlhh" 
                    Height="22px">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlFromMM" runat="server" CssClass="styleDdlhh" 
                    Height="22px">
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                Reason :
            </td>
            <td class="attensummaryTDRight">
                <asp:DropDownList ID="ddlreason" runat="server" Width="200px" Height="22px">
                    <asp:ListItem Value="2">Official Reason</asp:ListItem>
                    <asp:ListItem Value="3">Personal Reason</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td  valign="top" align="right">
                Remark :</td>
            <td class="attensummaryTDRight">
                <asp:TextBox ID="txtremark" runat="server" Height="60px" MaxLength="50" 
                    TextMode="MultiLine" Width="270px"></asp:TextBox>
            </td>
            <td class="attensummaryTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTDRight">
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
                    Text="Submit" Width="100px" />
                <asp:Button ID="btnclose" runat="server" OnClick="btnclose_Click" Text="Clear" Width="100px" />
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
                &nbsp;</td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
    </table>
            <table style="vertical-align:middle">
        <tr>
            <td align="center" class="style3">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="8"  AutoGenerateColumns="False"
                 OnPageIndexChanging="GridView1_PageIndexChanging"  onrowcommand="GridView1_RowCommand" >
                    <PagerSettings PageButtonCount="2" />
                    <Columns>
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                        <asp:BoundField DataField="ATT_DATE" HeaderText="Date" />
                        <asp:BoundField DataField="ATT_TIME" HeaderText="Time" />
                        <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                        <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" />
                        <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch_ID"/>
                        <asp:BoundField DataField="REASON" HeaderText="Reason" />
                        <asp:BoundField DataField="STATUS" HeaderText="Status" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                        <asp:ButtonField HeaderText="Cancel Record" Text="Cancel" CommandName="cancelrow" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr />
            </td>
        </tr>
    </table>    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
