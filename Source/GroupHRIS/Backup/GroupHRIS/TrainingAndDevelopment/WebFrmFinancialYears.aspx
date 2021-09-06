<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmFinancialYears.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmFinancialYears" %>

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
    Financial Year Details
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Financial Year
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtFinancialYear" Width="250px" MaxLength="4" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID = "txtFinancialYear" FilterType="Numbers" runat="server">
                        </asp:FilteredTextBoxExtender>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Financial Year is Required"
                            ControlToValidate="txtFinancialYear" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Start Date
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtStartDate" Width="250px" MaxLength="10" placeholder="DD/MM/YYYY"
                            runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtStartDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtStartDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Start Date is Required"
                            ControlToValidate="txtStartDate" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid Start Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                            ControlToValidate="txtStartDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        End Date
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtEndDate" Width="250px" MaxLength="10" placeholder="DD/MM/YYYY"
                            runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtEndDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="End Date is Required"
                            ControlToValidate="txtEndDate" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid End Date Format (DD/MM/YYYY)" Text="*" ValidationGroup="Main"
                            ControlToValidate="txtEndDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Description
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtDescription" Width="250px" TextMode="MultiLine" Height="75px"
                            runat="server"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Status
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlStatusCode" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Status is Required"
                            ControlToValidate="ddlStatusCode" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        
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
                    <td>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btnSave" Width="125px" runat="server" ValidationGroup="Main" Text="Save"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Width="125px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="margin: auto;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            Financial Years
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvAssessmentGroup" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="10" runat="server" OnPageIndexChanging="grdvAssessmentGroup_PageIndexChanging"
                            OnRowDataBound="grdvAssessmentGroup_RowDataBound" OnSelectedIndexChanged="grdvAssessmentGroup_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="FINANCIAL_YEAR" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%" HeaderText=" Financial Year " />
                                <asp:BoundField DataField="START_DATE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%" HeaderText=" Start Date " />
                                <asp:BoundField DataField="END_DATE" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%" HeaderText=" End Date " />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " ItemStyle-Width="40%" />
                                <asp:BoundField DataField="STATUS_CODE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" HeaderText=" Status Code " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>