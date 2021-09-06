<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmEmployeeGoals.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmEmployeeGoals" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
            document.getElementById("hfVerify").value = "";
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;
            document.getElementById("hfVerify").value = id;
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
    <asp:HiddenField ID="hfVerify" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
    <span>Employee Goal Details</span><span style="font-weight: 700;"> </span>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfupfdategoal" ClientIDMode="Static" runat="server" />
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right;">
                        Employee
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployee" Width="200" runat="server" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Employee is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtEmployee"></asp:RequiredFieldValidator>
                        <img alt="" src="../../Images/Common/Search.jpg" id="EmpSearch" runat="server" height="20" width="20" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtEmployee')" />
                        <asp:ImageButton ID="HistoryImageButton" runat="server" Height="20px" Visible="false"
                            Width="20px" ImageUrl="~/Images/Common/history.png" ToolTip="Select/View Previous Goals"
                            OnClientClick="openLOVWindow('/PerformanceManagement/WebFrmPreviousEmployeeGoals.aspx','PreviousGoals','hfupfdategoal')" />
                        <br />
                    </td>
                    <td style="text-align: left;">
                    </td>
                    <td colspan="3" style="text-align: left;">
                        <asp:Label ID="lblEmployeeName" runat="server" Style="color: #0000FF"></asp:Label>
                    </td>
                </tr>
                <%--<tr>
                    <td style="text-align: right;">
                    </td>
                    <td>
                    </td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>--%>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Year of Goal
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlYear" Width="200" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblYearofGoal" Visible="false" runat="server" Style="font-weight: 700;
                            color: #000000"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Goal Group
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGoalGroup" Width="200" runat="server" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Goal Group is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="ddlGoalGroup"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: text-top;">
                        Goal Area
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtGoalArea" TextMode="MultiLine" Width="200" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Goal Area is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtGoalArea"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: text-top;">
                        Goal Description
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="200" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Description is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: top;">
                        Measurements
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtMeasurements" runat="server" Width="200" TextMode="MultiLine"
                            ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Measurement is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtMeasurements"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Weights
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtWeights" runat="server" Width="200" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Weight is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtWeights"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="ftetxtWeights" runat="server" TargetControlID="txtWeights"
                            FilterType="Custom, Numbers" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblBalaceWeightText" runat="server" Text="Cumulative Weight : " Visible="false"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTotalWeightValue" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Status
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" Width="200" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Status is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTotalWeightText" runat="server" Text="Balanced Weight : " Visible="false"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblBalanceWeightValue" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="99" ValidationGroup="Main" Text="Save"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Width="99" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMaximumWeightText" runat="server" Text="Total Weight :" Visible="false"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMaximumWeightValue" runat="server" Text="100%" Visible="false"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="text-align: center;">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                            <ProgressTemplate>
                                <img src="/Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
            </table>
            Employee Goals
            <hr />
            <table style="margin: auto; min-width: 700px;">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdvEmployeeGoals" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="4" runat="server" OnPageIndexChanging="grdvEmployeeGoals_PageIndexChanging"
                            OnRowDataBound="grdvEmployeeGoals_RowDataBound" OnSelectedIndexChanged="grdvEmployeeGoals_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText=" Year of Goal " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_GROUP_ID" HeaderText=" Goal Group ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="DISPLAY_GOAL_AREA" HtmlEncode="false" HeaderText=" Goal Area" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText=" Goal Description " />
                                <asp:BoundField DataField="MEASUREMENTS" HeaderText=" Measurements " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="WEIGHT" HeaderText=" Weight " ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status Code" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_ID" HeaderText=" Goal ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE_TEXT" HeaderText=" Status " />
                                <asp:BoundField DataField="GOAL_AREA" HeaderText=" Goal Area " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn"  />                                    
                                <asp:BoundField DataField="SUPERVISOR_AGREE" HeaderText=" Supervisor (Dis)/Agree " />             
                                <asp:BoundField DataField="EMPLOYEE_AGREE" HeaderText=" Employee (Dis)/Agree " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>