<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="WebFrmTrainingSchedule.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            Training Schedule Details
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Training
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtTrainingID" Width="250px" ReadOnly="true" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RegularExpressionValidator2" ValidationGroup="Main" ControlToValidate="txtTrainingID" runat="server" ForeColor="Red" Text="*" ErrorMessage="Training ID is required"></asp:RequiredFieldValidator>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTrainingID')"
                            id="imgEditSearch" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Planned Scheduled Date
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtPlannedlDate" placeholder="DD/MM/YYYY" MaxLength="10" Width="250px"
                            runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPlannedlDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPlannedlDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>                        
                        <asp:RequiredFieldValidator ID="RegularExpressionValidator3" ValidationGroup="Main" ControlToValidate="txtPlannedlDate" runat="server" ForeColor="Red" Text="*" ErrorMessage="Planned Scheduled Date is required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid Planned Scheduled Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                            ControlToValidate="txtPlannedlDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Actual Date
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtActualDate" placeholder="DD/MM/YYYY" MaxLength="10" Width="250px"
                            runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActualDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtActualDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid Actual Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                            ControlToValidate="txtActualDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Planned From Time
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                    <asp:DropDownList ID="ddlPlannedFromTimeHrs" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;hrs.<asp:RequiredFieldValidator ID="RegularExpressionValidator5" ValidationGroup="Main" ControlToValidate="ddlPlannedFromTimeHrs" runat="server" ForeColor="Red" Text="*" ErrorMessage="Planned from Time is required"></asp:RequiredFieldValidator>
                        &nbsp;<asp:DropDownList ID="ddlPlannedFromTimeMinutes" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;mins.<asp:RequiredFieldValidator ID="RegularExpressionValidator6" ValidationGroup="Main" ControlToValidate="ddlPlannedFromTimeMinutes" runat="server" ForeColor="Red" Text="*" ErrorMessage="Planned from Time is required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Planned To Time
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">

                    <asp:DropDownList ID="ddlPlannedToTimeHrs" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;hrs.<asp:RequiredFieldValidator ID="RegularExpressionValidator7" ValidationGroup="Main" ControlToValidate="ddlPlannedToTimeHrs" runat="server" ForeColor="Red" Text="*" ErrorMessage="Planned to Time is required"></asp:RequiredFieldValidator>
                        &nbsp;<asp:DropDownList ID="ddlPlannedToTimeMins" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;mins.<asp:RequiredFieldValidator ID="RegularExpressionValidator8" ValidationGroup="Main" ControlToValidate="ddlPlannedToTimeMins" runat="server" ForeColor="Red" Text="*" ErrorMessage="Planned to Time is required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Location
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlLocation" Width="250px" runat="server">
                        </asp:DropDownList><asp:RequiredFieldValidator ID="RegularExpressionValidator4" ValidationGroup="Main" ControlToValidate="ddlLocation" runat="server" ForeColor="Red" Text="*" ErrorMessage="Location is required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Actual from Time
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                    <asp:DropDownList ID="ddlActualFromTimeHours" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        
                        &nbsp;hrs.&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlActualFromTimeTimeMins" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;mins.
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Actual to Time
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                    <asp:DropDownList ID="ddlActualToTimeHours" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;hrs.&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlActualToTimeMins" Style="text-align: right;" Width="85px"
                            runat="server">
                        </asp:DropDownList>
                        &nbsp;mins.
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Trainer
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlTrainer" Width="250px" runat="server">
                        </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Main" ControlToValidate="ddlTrainer" runat="server" ForeColor="Red" Text="*" ErrorMessage="Trainer is required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Satus
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlSatusCode" Width="250px" runat="server">
                        </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Main" ControlToValidate="ddlSatusCode" runat="server" ForeColor="Red" Text="*" ErrorMessage="Status is required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                    </td>
                    <td style="vertical-align: top;">
                    </td>
                    <td style="vertical-align: top; text-align: center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
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
                        <asp:Button ID="btnSave" Width="125px" ValidationGroup="Main" runat="server" Text="Save"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Width="125px" runat="server" Text="Clear" OnClick="btnClear_Click" />
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
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="Main" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            Training Shedules
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvTrainingSchedule" AutoGenerateColumns="false" Style="width: 850px;"
                            AllowPaging="true" PageSize="10" runat="server" OnPageIndexChanging="grdvTrainingSchedule_PageIndexChanging"
                            OnRowDataBound="grdvTrainingSchedule_RowDataBound" OnSelectedIndexChanged="grdvTrainingSchedule_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="RECORD_ID" ItemStyle-HorizontalAlign="Left" HeaderText=" Record ID"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="TRAINING_ID" ItemStyle-HorizontalAlign="Left" HeaderText=" Training ID"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="TRAINING_NAME" ItemStyle-HorizontalAlign="Left" HeaderText=" Training " />
                                <asp:BoundField DataField="PLANNED_SCHEDULE_DATE" ItemStyle-HorizontalAlign="Left"
                                    HeaderText=" Planned Date " />
                                <asp:BoundField DataField="ACTUAL_DATE" ItemStyle-HorizontalAlign="Left" HeaderText=" Actual Date " />
                                <asp:BoundField DataField="LOCATION_NAME" ItemStyle-HorizontalAlign="Left" HeaderText=" Location " />
                                <asp:BoundField DataField="PLANNED_FROM_TIME" HeaderText=" Planned from Time " />
                                <asp:BoundField DataField="PLANNED_TO_TIME" HeaderText=" planned to time " />
                                <asp:BoundField DataField="ACTUAL_FROM_TIME" HeaderText=" Actual from Time " />
                                <asp:BoundField DataField="ACTUAL_TO_TIME" HeaderText=" Actual to Time " />
                                <asp:BoundField DataField="STATUS_CODE_TEXT" ItemStyle-HorizontalAlign="Left" HeaderText=" Status Code " />
                                <asp:BoundField DataField="TRAINER_ID" ItemStyle-HorizontalAlign="Left" HeaderText=" Trainer ID " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="NAME_WITH_INITIALS" ItemStyle-HorizontalAlign="Left" HeaderText=" Trainer " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>