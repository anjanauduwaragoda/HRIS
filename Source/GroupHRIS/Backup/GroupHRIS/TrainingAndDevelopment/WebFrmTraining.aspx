<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmTraining.aspx.cs" EnableEventValidation="false" Inherits="GroupHRIS.TrainingAndDevelopment.Training" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />--%>
    <script language="javascript" type="text/javascript">
        function changeScreenSize() {
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(200, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }
    </script>
    <script type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sTrId) {
            var ctl = document.getElementById("hfCaller").value;

            //alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        } 
        
        function getValueFromChild(sTrId, trnName) {
            var ctl = document.getElementById("hfCaller").value;

            //alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            document.getElementById("hfTrnName").value = trnName;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        
        .InformationBox
        {
            background: rgb(165,200,255);
        }
        .Question
        {
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(165,204,255);
        }
        .Answer
        {
            padding-left: 20px;
            padding-right: 10px;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-top: 5px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(216,216,216);
        }
        .hideGridColumn
        {
            display: none;
        }
        
        table.GridView tr:hover {
  background-color: #CCCCCC;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    Training Details
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />            
            <asp:HiddenField ID="hfTrnName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <table style="margin: auto;">
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Training Name
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtTrainingName" Width="250px" MaxLength="300" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom" ValidChars="qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM/&-" TargetControlID="txtTrainingName" runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtTrainingName"
                                        runat="server" ForeColor="Red" ValidationGroup="Main" Text="*" ErrorMessage="Training Name is Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Training Code
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtTrainingCode" MaxLength="50" Width="250px" runat="server"></asp:TextBox>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtTrainingCode"
                                        ErrorMessage="Training Code is Required" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Training Program
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:DropDownList ID="ddlTrainingProgram" Width="250px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddlTrainingProgram"
                                        runat="server" ForeColor="Red" ValidationGroup="Main" Text="*" ErrorMessage="Training Program is Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Training Type
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:DropDownList ID="ddlTrainingType" Width="250px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddlTrainingType"
                                        runat="server" ForeColor="Red" ValidationGroup="Main" Text="*" ErrorMessage="Training Type is Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Planned Start Date
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtPlannedStartDate" placeholder="DD/MM/YYYY" Width="250px" MaxLength="10"
                                        runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtPlannedStartDate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtPlannedStartDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtPlannedStartDate"
                                        ErrorMessage="Planned Start Date is Required" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                                        ErrorMessage="Invalid Planned Start Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                                        ControlToValidate="txtPlannedStartDate" ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Planned End Date
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtPlannedEndDate" placeholder="DD/MM/YYYY" MaxLength="10" Width="250px"
                                        runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPlannedEndDate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPlannedEndDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPlannedEndDate"
                                        ErrorMessage="Planned End Date is Required" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                                        ErrorMessage="Invalid Planned End Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                                        ControlToValidate="txtPlannedEndDate" ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Planned Total Hours
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlannedTotalHours" Style="text-align: right;" Width="85px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtPlannedTotalHours"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;hrs.&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlPlannedTotalMins" Style="text-align: right;" Width="85px"
                                        runat="server">
                                    </asp:DropDownList>
                                    &nbsp;mins.
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" FilterType="Numbers" TargetControlID="txtPlannedTotalHours"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <%--<tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Planned Participants
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtPlannedParticipants" Width="250px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Numbers" TargetControlID="txtPlannedParticipants"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtPlannedParticipants"
                                        ErrorMessage="Planned Participants are Required" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Actual Start Date
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtActualStartDate" placeholder="DD/MM/YYYY" MaxLength="10" Width="250px"
                                        runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActualStartDate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtActualStartDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                                        ErrorMessage="Invalid Actual Start Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                                        ControlToValidate="txtActualStartDate" ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Actual End Date
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtActualEndDate" placeholder="DD/MM/YYYY" MaxLength="10" Width="250px"
                                        runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtActualEndDate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtActualEndDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                                        ErrorMessage="Invalid Actual End Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                                        ControlToValidate="txtActualEndDate" ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Actual Total Hours
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtActualTotalHours" Style="text-align: right;" Width="85px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtActualTotalHours"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;hrs.&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlActualTotalMins" Style="text-align: right;" Width="85px"
                                        runat="server">
                                    </asp:DropDownList>
                                    &nbsp;mins.
                                </td>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationExpression="^-?(([1-9]\d*)|0)(.0*[1-9](0*[1-9])*)?$"
                            ErrorMessage="Invalid Actual Total Hours" Text="*" ValidationGroup="Main" ControlToValidate="txtActualTotalHours"
                            ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <%--<tr>
                    <td style="text-align: right; vertical-align: top;">
                        Actual Participants
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtActualParticipants" Width="250px" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" FilterType="Numbers" TargetControlID="txtActualParticipants"
                            runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Is Out of Budget
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:CheckBox ID="chkIsOutOfBudget" runat="server" />
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Is Postponed
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:CheckBox ID="chkIsPostponed" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsPostponed_CheckedChanged" />
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Postponed Reason
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtPostpanedReason" Enabled="false" MaxLength="500" Width="250px"
                            TextMode="MultiLine" Height="75px" runat="server"></asp:TextBox>
                        <br />
                        <asp:TextBox ID="txtTempPostponedReason" Enabled="false" Text="txtTempPostponedReason"
                            Width="250px" Visible="false" runat="server"></asp:TextBox>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTempPostponedReason"
                            ForeColor="Red" ValidationGroup="Main" Text="*" ErrorMessage="Postponed Reason is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Status
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlStatusCode" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStatusCode"
                            ForeColor="Red" ValidationGroup="Main" Text="*" ErrorMessage="Status Code is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                            <ProgressTemplate>
                                <img src="/Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Button ID="btnSave" Width="120px" ValidationGroup="Main" runat="server" Text="Save"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Width="120px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td><a href="WebFrmTrainingParticipants.aspx" target="_blank">Training Participants</a>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top; height: 75px;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Select Training ID to Modify
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtTrainingID" Width="250px" ClientIDMode="Static" ReadOnly="true"
                            runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTrainingID')" id="imgEditSearch" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtTrainingID"
                            runat="server" ForeColor="Red" ValidationGroup="Sub" Text="*" ErrorMessage="Training ID is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Button ID="btnEdit" runat="server" ValidationGroup="Sub" Text="Edit" Width="120px"
                            OnClick="btnEdit_Click" />
                        <asp:Button ID="btnEditClear" runat="server" Text="Clear" Width="120px" OnClick="btnEditClear_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; vertical-align: top;" colspan="3">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="margin: auto;">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="Main"
                                        runat="server" />
                                    <asp:ValidationSummary ID="ValidationSummary2" ForeColor="Red" ValidationGroup="Sub"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            </td>
            <td style="vertical-align:top;">
            Training Companies
            <hr />
            <table style="margin:auto;">
                <tr>
                    <td style="text-align:right;vertical-align:top;">Company</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">                                    
                        <asp:DropDownList ID="ddlCompany" Width="250px" runat="server"> </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlCompany" runat="server" ForeColor="Red" ValidationGroup="Sub0" Text="*" ErrorMessage="Company is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Planned Participants</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:TextBox ID="txtPlannedCompanyparticipants" MaxLength="50" Width="250px" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" FilterType="Numbers" TargetControlID="txtPlannedCompanyparticipants" runat="server">
                        </asp:FilteredTextBoxExtender>                        
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtPlannedCompanyparticipants" runat="server" ForeColor="Red" ValidationGroup="Sub0" Text="*" ErrorMessage="Planned Participants are Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Actual Participants</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:TextBox ID="txtActualCompanyParticipants" MaxLength="50" Width="250px" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" FilterType="Numbers" TargetControlID="txtActualCompanyParticipants" runat="server">
                        </asp:FilteredTextBoxExtender>                                    
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Description</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:TextBox ID="txtDescription" MaxLength="50" TextMode="MultiLine" Width="250px" runat="server"></asp:TextBox>                                
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Status</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:DropDownList ID="ddlTrainingCompanyStatus" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="ddlTrainingCompanyStatus" runat="server" ForeColor="Red" ValidationGroup="Sub0" Text="*" ErrorMessage="Training Company Status is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;">
                        <asp:Button ID="btnCompanyAdd" Width="120px" runat="server" Text="Add" ValidationGroup="Sub0" onclick="btnCompanyAdd_Click" />
                        <asp:Button ID="btnCompanyClear" Width="120px" runat="server" Text="Clear" 
                            onclick="btnCompanyClear_Click" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;">
                        <asp:ValidationSummary ID="ValidationSummary3" ValidationGroup="Sub0" ForeColor="Red" runat="server" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="grdvCompany" AutoGenerateColumns="false" Style="width: 100%" 
                            AllowPaging="true" PageSize="5" runat="server" 
                            onpageindexchanging="grdvCompany_PageIndexChanging" 
                            onrowdatabound="grdvCompany_RowDataBound" 
                            onselectedindexchanged="grdvCompany_SelectedIndexChanged" >
                            <Columns>
                                <asp:BoundField DataField="COMPANY_ID" ItemStyle-HorizontalAlign="Left" HeaderText=" Company ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="COMP_NAME" ItemStyle-HorizontalAlign="Left" HeaderText=" Company " />
                                <asp:BoundField DataField="PLANNED_PARTICIPANTS" ItemStyle-HorizontalAlign="Right" HeaderText=" Planned Participats " />
                                <asp:TemplateField HeaderText = " Exclude " ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkExclude" AutoPostBack="true" runat="server" oncheckedchanged="chkExclude_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                       <br />
                        Trainers
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Trainer Name</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:TextBox ID="txtTrainerID" ReadOnly="true" Width="250px" runat="server"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblTrainerName" runat="server"></asp:Label>
                    </td>
                    <td style="vertical-align:top;text-align:left;">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtTrainerID" runat="server" ForeColor="Red" ValidationGroup="Sub1" Text="*" ErrorMessage="Trainer is Required"></asp:RequiredFieldValidator>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainerSearch.aspx','search','txtTrainerID')"
                            id="imgEditSearch0" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Selected Reason</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:TextBox ID="txtSelectedReason" Width="250px" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </td>
                    <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtSelectedReason" runat="server" ForeColor="Red" ValidationGroup="Sub1" Text="*" ErrorMessage="Selected Reason is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;">Status</td>
                    <td style="text-align:right;vertical-align:top;">:</td>
                    <td style="vertical-align:top;">
                        <asp:DropDownList ID="ddlTrainerStatus" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ControlToValidate="ddlTrainerStatus" runat="server" ForeColor="Red" ValidationGroup="Sub1" Text="*" ErrorMessage="Status is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;">
                        <asp:Button ID="btnTrainerAdd" Width="120px" ValidationGroup="Sub1" runat="server" Text="Add" 
                            onclick="btnTrainerAdd_Click"  />
                        <asp:Button ID="btnTrainerClear" Width="120px" runat="server" Text="Clear" 
                            onclick="btnTrainerClear_Click" />
                     </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;">
                    
                        <asp:ValidationSummary ID="ValidationSummary4" runat="server" ForeColor="Red" 
                            ValidationGroup="Sub1" />
                    
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="grdvTrainers" AutoGenerateColumns="false" AllowPaging="true" 
                            PageSize="5" style="Width:400px;" runat="server" CssClass="GridView" 
                            onrowdatabound="grdvTrainers_RowDataBound" 
                            onselectedindexchanged="grdvTrainers_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="TRAINING_ID" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderText=" Training ID " />
                                <asp:BoundField DataField="TRAINER_ID" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderText=" Trainer ID " />
                                <asp:BoundField DataField="NAME_WITH_INITIALS" HeaderText=" Trainer " />
                                <asp:BoundField DataField="SELECTED_REASON" HeaderText=" Selected Reason " />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" />
                                <asp:TemplateField HeaderText="Exclude" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkTrainerExclude" AutoPostBack="true" runat="server" oncheckedchanged="chkTrainerExclude_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="text-align:right;vertical-align:top;"></td>
                    <td style="vertical-align:top;"></td>
                    <td></td>
                </tr>
            </table>
            </td>
            </tr> </table> Recently Added Trainings
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvTraining" AutoGenerateColumns="false" Style="width: 800px;"
                            AllowPaging="true" PageSize="5" runat="server" OnPageIndexChanging="grdvTraining_PageIndexChanging"
                            OnRowDataBound="grdvTraining_RowDataBound" OnSelectedIndexChanged="grdvTraining_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="TRAINING_ID" ItemStyle-HorizontalAlign="Left" HeaderText=" Training ID"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="TRAINING_NAME" ItemStyle-HorizontalAlign="Left" HeaderText=" Training " />
                                <asp:BoundField DataField="TRAINING_CODE" ItemStyle-HorizontalAlign="Left" HeaderText=" Training Code " />
                                <asp:BoundField DataField="PROGRAM_NAME" ItemStyle-HorizontalAlign="Left" HeaderText=" Training Program " />
                                <asp:BoundField DataField="TYPE_NAME" HeaderText=" Training Type " />
                                <asp:BoundField DataField="STATUS_CODE_TEXT" ItemStyle-HorizontalAlign="Left" HeaderText=" Status Code " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>