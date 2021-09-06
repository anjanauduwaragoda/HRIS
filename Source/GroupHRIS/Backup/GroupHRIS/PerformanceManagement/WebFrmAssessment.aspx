<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmAssessment.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmAssessment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 28%;
        }
        
        .hiddencol
        {
            display: none;
        }
        .style7
        {
            width: 40%;
        }
        .style8
        {
            width: 20%;
            min-width: 138.133px;
            max-width: 138.133px;
        }
        .style13
        {
            width: 256px;
        }
        .style14
        {
            width: 135px;
        }
        .style15
        {
            width: 40%;
        }
        .styleEmptyData
        {
            width: 300px;
        }
        .style25
        {
            width: 16%;
        }
        .style26
        {
            width: 37%;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindow(file) {


            //            txb = ctlName;
            //            childWindow = open(file, window, 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes');
            //            //if (childWindow.opener == null) childWindow.opener = self;

            //            document.getElementById("hfCaller").value = ctlName;
            var e = document.getElementById("ddlCompany");
            var selectedCompany = e.options[e.selectedIndex].value;

            if (selectedCompany != '') {
                window.open(file, 'Search', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes');
            }


        }

        function getValuefromChild(sRetVal, companyId) {

            //var actionPurpose = document.getElementById("HiddenField1").value = "dataCaptured"
            document.getElementById("HiddenField1").value = sRetVal;
            document.getElementById("hiddenSelectedCompanyId").value = companyId;

            //alert(sRetVal);
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
    <br />
    <span>Assessment Details</span>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="vertical-align: top; width: 35%; min-width: 437px;">
                        <table class="styleMainTb">
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label10" runat="server" Text="Company:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:DropDownList ID="ddlCompany" runat="server" Width="250px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCompany"
                                        ErrorMessage="Company name is required " ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr class="styleMainTb">
                                <td align="right" class="style26">
                                    <asp:Label ID="Label9" runat="server" Text="Year of Assessment:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:DropDownList ID="ddlYear" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlYear"
                                        ErrorMessage="Year is required " ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label6" runat="server" Text="Status:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="250px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Status is required" ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label1" runat="server" Text="Assessment Name:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:TextBox ID="txtName" runat="server" Width="244px" MaxLength="75"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtName_FilteredTextBoxExtender" runat="server"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" "
                                        TargetControlID="txtName">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Assessment name is required " ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Assessment Type:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:DropDownList ID="ddlAssessmentType" runat="server" Width="250px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlAssessmentType"
                                        ErrorMessage="Assessment type is required " ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    &nbsp;<asp:Label ID="Label7" runat="server" Text="Cut off Date:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:TextBox ID="txtCutOffDate" runat="server" Width="244px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCutOffDate"
                                        Format="yyyy/MM/dd">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCutOffDate"
                                        ErrorMessage="Cut off Date is required " ForeColor="Red" ValidationGroup="vgAssessment">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label3" runat="server" Text="Remarks:"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="244px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label4" runat="server" Text="Year of Assessment:" Visible="false"></asp:Label>
                                </td>
                                <td align="left" class="style7">
                                    <asp:Label ID="lblYearOfAssessment" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    &nbsp;
                                </td>
                                <td align="left" class="style7" style="padding-right: 12px; padding-top: 10px; padding-bottom: 10px;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="openLOVWindow('/PerformanceManagement/WebFrmAddAssessedEmployees.aspx')"
                                        OnClick="linkAddAssessedEmployees_click">
                            Add Assessed Employees</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    &nbsp;
                                </td>
                                <td align="right" class="style7" style="padding-right: 12px;">
                                    <hr style="width: 100%" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                    <asp:Label ID="Label5" runat="server" Text="Assessment Purpose:"></asp:Label>
                                </td>
                                <td align="left" class="style15">
                                    <asp:DropDownList ID="ddlAssessmentPurpose" runat="server" Width="250px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvAssessmentPurpose" runat="server" ValidationGroup="vgAssessmentPurpose"
                                        ErrorMessage="Assessment Purpose is required" ForeColor="Red" ControlToValidate="ddlAssessmentPurpose"> * </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style26" align="right">
                                    Status:
                                </td>
                                <td class="style7" style="padding-left: 0; margin-left: 0; border-left: 0;">
                                    <table style="">
                                        <tr>
                                            <td class="style14" style="vertical-align: middle; padding-left: 0;">
                                                <asp:DropDownList ID="ddlPurposeStatus" runat="server" Width="100px" padding-left="0px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvAssessmentPurposeStatus" runat="server" ValidationGroup="vgAssessmentPurpose"
                                                    ErrorMessage="Status is required" ForeColor="Red" ControlToValidate="ddlPurposeStatus"> * </asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" class="style13">
                                                <asp:Button ID="btnAddPurpose" runat="server" Width="60px" Text="Add" OnClick="btnAddPurpose_Click"
                                                    ValidationGroup="vgAssessmentPurpose" />
                                                <asp:Button ID="btnClearPurpose" runat="server" Text="Reset" OnClick="btnClearPurpose_Click"
                                                    Width="50px" />&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="style26">
                                </td>
                                <td class="style7" align="left" style="padding-right: 12px">
                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:HiddenField ID="HiddenSelectedIndex" runat="server" />
                                    <asp:ValidationSummary ID="AssessmentPurposeValidationSummery" runat="server" ValidationGroup="vgAssessmentPurpose"
                                        ForeColor="Red" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="style7" colspan="2" align="right" style="padding-right: 12px">
                                    <asp:GridView ID="PurposeGridView" runat="server" Style="width: 250px;" OnSelectedIndexChanged="PurposeGridView_SelectedIndexChanged"
                                        OnRowDataBound="PurposeGridView_RowDataBound" OnPageIndexChanging="PurposeGridView_PageIndexChanging"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="PURPOSE_ID" HeaderText="Purpose Id" ItemStyle-CssClass="hiddencol"
                                                HeaderStyle-CssClass="hiddencol" />
                                            <asp:BoundField DataField="NAME" HeaderText="Purpose" />
                                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style26">
                                </td>
                                <td class="style7" style="padding-right: 12px">
                                    <hr style="width: 100%" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style8" style="padding-right: 12px; text-align: right">
                                </td>
                                <td align="right" class="style8" style="padding-right: 12px; text-align: center">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin: auto; width: 200px">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <%--<td class="style1">
                <asp:GridView ID="GridViewSelectedEmployees" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="emp_id" HeaderText="Employee Id" ItemStyle-CssClass="hiddencol"
                            HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="emp_name" HeaderText="Name" />
                        
                    </Columns>
                </asp:GridView>
            </td>--%>
                            </tr>
                            <tr>
                                <td class="style26">
                                </td>
                                <%--style="padding-right: 12px"--%>
                                <td align="left" class="style7">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgAssessment"
                                        Width="125px" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="125px" />
                                    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2" style="padding-right: 10px;">
                                    <asp:Label ID="lblErrorMsg2" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style26">
                                </td>
                                <td align="left">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgAssessment"
                                        ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-right: 12px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-right: 12px;">
                                    Assessments
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-right: 12px;">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <%--<td style="width:50%;">
                            <asp:Label ID="Label7" runat="server" Text="Company :"></asp:Label>
                            <asp:DropDownList ID="ddlSearchCompany" runat="server">
                            </asp:DropDownList>
                        </td>--%>
                                <%--<td colspan="2">
                            <table>
                                <tr>
                                    <td style="padding-right: 12px; padding-left: 15px; width: 60%">
                                        <asp:Label ID="Label7" runat="server" Text="Company :"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchCompany" runat="server" Width="120px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" style="padding-right: 12px; width: 40%">
                                        <asp:Label ID="Label8" runat="server" Text="Year :"></asp:Label>
                                        <asp:TextBox ID="txtSearchYear" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td align="right" style="padding-right: 12px; width: 50%">
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../Images/Common/Search.jpg"
                                            Height="20px" Width="20px" OnClick="btnSearch_Click" />
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" style="padding-right: 12px;">
                                    <asp:GridView ID="GridViewAssessment" runat="server" AutoGenerateColumns="False"
                                        OnRowDataBound="gridViewAssessment_OnRowDataBound" OnSelectedIndexChanged="gridViewAssessment_SelectedIndexChanged"
                                        OnPageIndexChanging="gridViewAssessment_PageIndexChanging" AllowPaging="true">
                                        <Columns>
                                            <asp:BoundField DataField="ASSESSMENT_ID" HeaderText="AssessmentId" ItemStyle-CssClass="hiddencol"
                                                HeaderStyle-CssClass="hiddencol" />
                                            <asp:BoundField DataField="ASSESSMENT_TYPE_ID" HeaderText="AssessmentTypeId" ItemStyle-CssClass="hiddencol"
                                                HeaderStyle-CssClass="hiddencol" />
                                            <asp:BoundField DataField="ASSESSMENT_NAME" HeaderText="Assessment" />
                                            <asp:BoundField DataField="ASSESSMENT_TYPE_NAME" HeaderText="Type" />
                                            <asp:BoundField DataField="YEAR_OF_ASSESSMENT" HeaderText="Year" />
                                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top; width: 58%; max-width: 435px;">
                        <table>
                            <tr style="width: 100%;">
                                <td align="left">
                                    Assessed Employees
                                    <%--<hr style="width: 124px" align="left" />--%>
                                </td>
                            </tr>
                            <tr style="width: 100%;">
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    <asp:GridView ID="GridViewSelectedEmployees" Style="width: 430px;" runat="server"
                                        AutoGenerateColumns="False" AllowPaging="True" PageSize="20" OnRowDataBound="gridViewSelectedEmployees_OnRowDataBound"
                                        OnPageIndexChanging="gridViewSelectedEmployees_PageIndexChanging" EmptyDataRowStyle-Width="300px">
                                        <Columns>
                                            <asp:BoundField DataField="emp_id" HeaderText="Employee Id" ItemStyle-CssClass="hiddencol"
                                                HeaderStyle-CssClass="hiddencol">
                                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="epf_no" HeaderText="EPF No" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="emp_name" HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Goal">
                                                <HeaderTemplate>
                                                    Goal &nbsp;
                                                    <%--OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged"--%>
                                                    <asp:CheckBox ID="goalHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged" /></HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="goalChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="goalChildCheckBox_OnCheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comp">
                                                <HeaderTemplate>
                                                    Comp&nbsp;
                                                    <asp:CheckBox ID="competencyHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="competencyHeaderCheckBox_OnCheckedChanged" /></HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="competencyChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="competencyChildCheckBox_OnCheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Self">
                                                <HeaderTemplate>
                                                    Self &nbsp;
                                                    <asp:CheckBox ID="selfHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="selfHeaderCheckBox_OnCheckedChanged" /></HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="selfChildCheckBox" runat="server" OnCheckedChanged="selfChildCheckBox_OnCheckedChanged"
                                                        AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Exclude">
                                                <HeaderTemplate>
                                                    Exclude &nbsp;
                                                    <%--OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged"--%>
                                                    <asp:CheckBox ID="excludeHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="excludeHeaderCheckBox_OnCheckedChanged" /></HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="excludeChildCheckBox" runat="server" OnCheckedChanged="excludeChildCheckBox_OnCheckedChanged"
                                                        AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle HorizontalAlign="Center" />
                                        <EmptyDataRowStyle Width="300px" />
                                        <EmptyDataTemplate>
                                            No Employees Selected
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnExclude" runat="server" Text="Exclude" OnClick="btnExclude_Click"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hiddenAssessmentId" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenPreviouseAssessmentYear" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenPreviouseStatus" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenSelectedCompanyId" runat="server" Value="" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
