<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="AttInOutForIndividual.aspx.cs" EnableEventValidation="false" Inherits="GroupHRIS.Attendance.AttInOutForIndividual" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .styleDdlhh
        {
            width: 50px;
            color: Blue;
        }
        .style3
        {
            height: 27px;
        }
        .styleTableCell2TextBox
        {
            width: 250px;
            text-align: left;
        }
    </style>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style4
        {
            width: 200px;
        }
        .style5
        {
            height: 21px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        // -------

        var TotalChkBx;
        var Counter;

        window.onload = function () {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAttendance.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }

        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAttendance.ClientID %>');
            var TargetChildControl = "EXCLUDE";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }

        function ChildClick(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox = document.getElementById(HCheckBox);

            //Modifiy Counter; 
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;

            //Change state of the header CheckBox.
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
        //------


        //-- END


        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sEmpId, sName) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById("hfVal").value = sEmpId
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
            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Employee :
                    </td>
                    <td>
                        <asp:TextBox ID="txtemployee" runat="server" ReadOnly="true" ClientIDMode="Static"
                            Width="150" Font-Bold="true" ForeColor="#009900" AutoPostBack="True" 
                            Enabled="False" ></asp:TextBox>
                        <%--<asp:Label ID="txtemployee" runat="server" Font-Bold="True" ForeColor="#009900"></asp:Label>--%>
                        <%--<asp:Label ID="txtemployee" runat="server" Font-Bold="True" ForeColor="#009900"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Recommend By :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRecommendBy" runat="server" ReadOnly="true" MaxLength="8" Width="150"
                            ClientIDMode="Static" AutoPostBack="True"></asp:TextBox>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtRecommendBy')" />
                        <asp:RequiredFieldValidator ID="rfvRecomendedBy" runat="server" Text="*" ErrorMessage="Recomended by is required"
                            ValidationGroup="inout" ForeColor="Red" ControlToValidate="txtRecommendBy"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRecommendByName" runat="server" BorderStyle="None" ClientIDMode="Static"
                            ForeColor="Blue" Width="442px"></asp:TextBox>
                        <%-- <asp:Label ID="txtRecommendByName" runat="server"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="style5">
                        Company :
                    </td>
                    <td class="style5">
                        <asp:Label ID="txtCompID" runat="server"></asp:Label>
                        <%-- <asp:DropDownList ID="ddlCompID" runat="server" OnSelectedIndexChanged="ddlCompID_SelectedIndexChanged"
                    AutoPostBack="True">--%>
                        <%-- </asp:DropDownList>--%>
                    </td>
                    <td class="style5">
                        <%-- <asp:RequiredFieldValidator ID="rfvCompany" runat="server" Text="*" ErrorMessage="Select Company is required "
                    ControlToValidate="ddlCompID" ValidationGroup="Company" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan='2'>
                        <asp:Label ID="lblmsg" runat="server" Style="color: #FF0000"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <hr />
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%; vertical-align: text-top;">
                        <fieldset style="display: block; height: 500px">
                            <legend><b>Add In/Out&nbsp; for Single Day</b></legend>
                            <table>
                                <tr>
                                    <td style="text-align: right;">
                                        Date :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="195px" onkeydown="return (event.keyCode!=13);"
                                            OnTextChanged="txtDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:FilteredTextBoxExtender ID="fteDate" runat="server" TargetControlID="txtDate"
                                            FilterType="Custom, Numbers" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                        ErrorMessage="(DD/MM/YYYY)" ControlToValidate="txtDate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        IN / OUT:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlinout" runat="server" Height="22px" Width="200px" OnSelectedIndexChanged="ddlinout_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="2">IN/OUT</asp:ListItem>
                                            <asp:ListItem Value="1">IN</asp:ListItem>
                                            <asp:ListItem Value="0">OUT</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="6" style="vertical-align: top;">
                                        <asp:GridView ID="grdattendance" Style="max-width: 100px; overflow: scroll;" runat="server"
                                            AutoGenerateColumns="False" AllowPaging="True">
                                            <Columns>
                                                <asp:BoundField DataField="ATT_TIME" HeaderText=" Recorded Time" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20px">
                                    </td>
                                    <td class="style4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblRoster" runat="server" Text="Roster Time : " Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRoster" runat="server" Height="22px" Width="200px" Visible="false"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlRoster_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReguler" runat="server" Text="Reguler Time : " Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblregulerTime" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <%--<tr>
                            <td style="text-align: right;">
                                In Date :
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" Visible="false" runat="server" MaxLength="10" Width="195px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate"
                                    Format="yyyy/MM/dd">
                                </asp:CalendarExtender>
                                <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="revFromDate" Visible="false" runat="server" ValidationExpression="^(([0-9][0-9])|([1-2][0,9][0-9][0-9]))\/(([0-9])|([0-2][0-9])|(3[0-1]))\/(([1-9])|(0[1-9])|(1[0-2]))$"
                        ErrorMessage="(YYYY/MM/DD)" ControlToValidate="txtfromdate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>
                            </td>
                        </tr>--%>
                                <tr>
                                    <td style="text-align: right;">
                                        In Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dpInlocation" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        In Time :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFromHH" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlFromMM" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        &nbsp;(HH:MM)
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Out Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOutDate" runat="server" MaxLength="10" onkeydown="return (event.keyCode!=13);"
                                            Width="195px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOutDate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:FilteredTextBoxExtender ID="ftoutDate" runat="server" TargetControlID="txtOutDate"
                                            FilterType="Custom, Numbers" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <%--<asp:RegularExpressionValidator ID="revtoDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                        ErrorMessage="(DD/MM/YYYY)" ControlToValidate="txtOutDate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Out Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOutLocation" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Out Time :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOutHH" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlOutMM" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        &nbsp;(HH:MM)
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Reason :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReason1" runat="server" Height="22px" Width="200px">
                                            <asp:ListItem Value="2">Official Reason</asp:ListItem>
                                            <asp:ListItem Value="3">Personal Reason</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr><td height="20px"></td><td></td></tr>--%>
                                <tr>
                                    <td style="text-align: right;">
                                        Remarks :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks1" runat="server" Height="80px" MaxLength="50" Width="197px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- <tr>
                            <td height="20px">
                            </td>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin: auto; width: auto">
                                    <ProgressTemplate>
                                        <img src="../Images/ProBar/720.GIF" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>--%>
                                <tr>
                                    <td height="20px">
                                    </td>
                                    <td style="vertical-align: middle;" colspan="2">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px"
                                            OnClick="btnAdd_Click" />
                                        <asp:Button ID="btnclr" runat="server" Text="Clear" Width="100px" OnClick="btnclr_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </fieldset>
                    </td>
                    <td>
                        <fieldset style="display: block; height: 500px;">
                            <legend><b>Add In/Out for Date Range</b></legend>
                            <table>
                                <tr>
                                    <td style="text-align: right;">
                                        In Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMultipleInLocation" runat="server" Width="200px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        In Time :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMultipleInHH" runat="server" CssClass="styleDdlhh" Height="22px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMultipleInMM" runat="server" CssClass="styleDdlhh" Height="22px">
                                        </asp:DropDownList>
                                        &nbsp;(HH:MM)
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Out Location :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMultipleOutLocation" runat="server" Width="200px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Out Time :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMultipleOutHH" runat="server" CssClass="styleDdlhh" Height="22px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMultipleOutMM" runat="server" CssClass="styleDdlhh" Height="22px">
                                        </asp:DropDownList>
                                        &nbsp;(HH:MM)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <div>
                                            <asp:Calendar ID="calDates" runat="server" Width="200px" BackColor="#BAC0C9" OnSelectionChanged="calDates_SelectionChanged">
                                            </asp:Calendar>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <span class="style1"><b>Exclude Days</b></span><br />
                                            <asp:CheckBox ID="chkSunday" runat="server" Text="Sunday" BackColor="#FFE391" /><br />
                                            <asp:CheckBox ID="chkMonday" runat="server" Text="Monday" /><br />
                                            <asp:CheckBox ID="chkTuesday" runat="server" Text="Tuesday" /><br />
                                            <asp:CheckBox ID="chkWednesday" runat="server" Text="Wednesday" /><br />
                                            <asp:CheckBox ID="chkThursday" runat="server" Text="Thursday" /><br />
                                            <asp:CheckBox ID="chkFriday" runat="server" Text="Friday" /><br />
                                            <asp:CheckBox ID="chkSaturday" runat="server" Text="Saturday" BackColor="#FFE391" /><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btnDeselect" runat="server" Text="Deselect" OnClick="btnDeselect_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Reason :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreason" runat="server" Height="22px" Width="200px">
                                            <asp:ListItem Value="2">Official Reason</asp:ListItem>
                                            <asp:ListItem Value="3">Personal Reason</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Remarks :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtremark" runat="server" Height="60px" MaxLength="50" Width="197px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddMultipleDates" runat="server" Text="Add" Width="100px" Style="height: 26px"
                                            OnClick="btnAddMultipleDates_Click" />
                                        <asp:Button ID="btnDeselectMultipleDates" runat="server" Text="Clear" Width="100px"
                                            OnClick="btnDeselectMultipleDates_Click1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <br />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Company"
                ForeColor="Red" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="inout"
                ForeColor="Red" />
            <br />
            <table>
                <tr>
                    <td align="center" style="width: 100%">
                        <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="7" OnRowDataBound="gvAttendance_RowDataBound" OnPageIndexChanging="gvAttendance_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="ATT_DATE" HeaderText="Attendance Date" />
                                <asp:BoundField DataField="ATT_TIME" HeaderText="Attendance Time" />
                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                                <asp:TemplateField HeaderText="IS_EXCLUDE">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="EXCLUDE" runat="server" OnCheckedChanged="EXCLUDE_OnCheckedChanged"
                                            Checked='<%# bool.Parse(Eval("IS_EXCLUDE").ToString() == "1" ? "True": "False") %>'
                                            Enabled="true" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <table align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnsave" runat="server" OnClick="btnSave_Click" Text="Save" Width="100px"
                            ValidationGroup="inout" />
                        <asp:Button ID="btnclose" runat="server" OnClick="btnclose_Click" Text="Clear" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style3">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <br />
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label><br />
                        <asp:Label ID="lblmail" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewhide" Visible="true" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="ATT_DATE" HeaderText="Attendance Date" />
                                <asp:BoundField DataField="ATT_TIME" HeaderText="Attendance Time" />
                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <table align="center">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblhistory" runat="server" Style="text-align: center; color: #000000;
                            font-size: 16px; font-family: 'Times New Roman';"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 100%">
                        <asp:GridView ID="grdRequests" runat="server" AllowPaging="True" PageSize="15" AutoGenerateColumns="False"
                            OnPageIndexChanging="grdRequests_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                                <asp:BoundField DataField="ATT_DATE" HeaderText="Date" />
                                <asp:BoundField DataField="ATT_TIME" HeaderText="Time" />
                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                                <%-- <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />--%>
                                <%--<asp:BoundField DataField="BRANCH_ID" HeaderText="Branch_ID"/>--%>
                                <asp:BoundField DataField="REASON" HeaderText="Reason" />
                                <asp:BoundField DataField="STATUS" HeaderText="Status" />
                                <asp:ButtonField HeaderText="Cancel Record" Text="Cancel" CommandName="cancelrow"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblExclude" runat="server" Text="Cancel Request"></asp:Label><br />
                                        <asp:CheckBox ID="chkBxHeader" runat="server" CommandName="Exclude" OnCheckedChanged="chkBxHeader_CheckedChanged"
                                            AutoPostBack="True" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" AutoPostBack="true" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ATT_LOG_ID" HeaderText="log_id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <div>
                            <asp:Label ID="lblnote" runat="server" Style="color: #000000; text-align: right;
                                font-size: 16px; font-family: 'Times New Roman';"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Button ID="btnRequest" runat="server" Text="Obsolete" OnClick="btnRequest_Click" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

