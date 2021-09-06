<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmActivatePendingAssessment.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmActivatePendingAssessment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .detailTableColumn1
        {
            min-width: 150px;
            max-width: 150px;
            text-align: right;
        }
        .hiddencol
        {
            display: none;
        }
        .divBorder
        {
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            -khtml-border-radius: 10px;
        }
        .gridHeader
        {
            background-color: Gray;
            font-family: Arial;
            color: White;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 16px;
        }
        
        .gridRows
        {
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            min-height: 25px;
            text-align: left;
            border: none 0px transparent;
        }
        
        .GridPager a, .GridPager span
        {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }
        .GridPager a
        {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }
        .GridPager span
        {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--Summery of Pending Assessments
    <hr />--%>
            <br />
            <div id="summeryDiv" style="min-width: 100%; background-color: #aed6f1;" class="divBorder">
                <br />
                <center>
                    <b>Summery of Pending Assessments</b>
                </center>
                <br />
                <%--<asp:Table ID="summeryTable" runat="server" Width="500px" Style="margin: 0 auto;"
            BorderStyle="None" CellPadding="2">
            <asp:TableRow>
                <asp:TableHeaderCell BackColor="AliceBlue"> Company </asp:TableHeaderCell>
                <asp:TableHeaderCell Style="text-align: center;" BackColor="AliceBlue"> Year </asp:TableHeaderCell>
                <asp:TableHeaderCell Style="text-align: center;" BackColor="AliceBlue"> Pending </asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableFooterRow>
            </asp:TableFooterRow>
        </asp:Table>--%>
                <%--<table>
            <thead>
                <tr>
                    <td>
                        Company
                    </td>
                    <td>
                        Year
                    </td>
                    <td>
                        Pending
                    </td>
                </tr>
            </thead>
            <tbody id="summeryTableBody" runat="server">
            
            </tbody>
        </table>--%>
                <center>
                    <asp:GridView ID="gvSummery" Style="width: 500px; border: 0px solid #000000;" runat="server"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="5" HeaderStyle-CssClass="gridHeader"
                        HeaderStyle-BackColor="#80bfff" BorderStyle="Solid" OnPageIndexChanging="gvSummery_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="COMPANY_ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol" BackColor="#AED6F1"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="COMP_NAME" HeaderText="Company">
                                <FooterStyle BackColor="#AED6F1" BorderColor="#AED6F1" />
                                <HeaderStyle BackColor="#80bfff" BorderColor="White" BorderStyle="Solid" />
                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Width="335px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="YEAR_OF_ASSESSMENT" HeaderText="Year">
                                <FooterStyle BackColor="#AED6F1" BorderColor="#AED6F1" />
                                <HeaderStyle BackColor="#80bfff" BorderColor="White" BorderStyle="Solid" />
                                <ItemStyle BorderStyle="None" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PENDING" HeaderText="Pending">
                                <FooterStyle BackColor="#AED6F1" BorderColor="#AED6F1" />
                                <HeaderStyle BackColor="#80bfff" BorderColor="White" BorderStyle="Solid" />
                                <ItemStyle BorderStyle="None" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="#80bfff" CssClass="gridHeader"></HeaderStyle>
                        <RowStyle CssClass="gridRow"></RowStyle>
                        <FooterStyle BackColor="#AED6F1" BorderColor="#AED6F1" />
                        <%--<PagerStyle BackColor="black" BorderWidth="0px" BorderStyle="None"  HorizontalAlign="Left" ForeColor="#FF0066" />--%>
                        <PagerStyle BackColor="#80bfff" ForeColor="Black" />
                    </asp:GridView>
                </center>
                <br />
            </div>
            <br />
            <div id="detailsDiv" style="width: 100%; display: inline-block;">
                Activate Assessment
                <hr />
                <div id="leftDiv" style="max-width: 50%; float: left;">
                    <table>
                        <tr>
                            <td class="detailTableColumn1">
                                <asp:Label ID="Label2" runat="server" Text="Year of Assessment :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" Width="252px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1">
                                <asp:Label ID="Label1" runat="server" Text="Company :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" Width="252px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1">
                                <asp:Label ID="Label3" runat="server" Text="Assessment Name :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="246px" MaxLength="75" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1">
                                <asp:Label ID="Label4" runat="server" Text="Assessment Type :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAssessmentType" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1" valign="top" style="vertical-align: top;">
                                <asp:Label ID="Label8" runat="server" Text="Remarks :"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="150"
                                    Rows="5" Width="246px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1" valign="top" style="vertical-align: top;">
                            </td>
                            <td align="center">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <img src="../Images/ProBar/720.GIF" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td class="detailTableColumn1" valign="top" style="vertical-align: top;">
                            </td>
                            <td align="justify">
                                <asp:Button ID="btnActivate" runat="server" Text="Activate" OnClick="btnActivate_Click"
                                    Width="125px" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="125px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="rightDiv" style="max-width: 50%; min-width: 50%; float: right;">
                    <table width="412px" style="float: right; text-align: left;">
                        <tr style="height: 200px;">
                            <td class="detailTableColumn1" style="vertical-align: top; float: left; height: 100%;"
                                align="left">
                                <asp:Label ID="Label6" runat="server" Text="Assessment Purposes :"></asp:Label>
                            </td>
                            <td style="vertical-align: top; background-color: #aed6f1; padding-left: 15px; padding-top: 15px;
                                width: 100%;" id="purposesListCell" runat="server">
                                <ul id="purposeUl" style="padding-left: 15px; margin-left: 15px;" runat="server">
                                </ul>
                                <asp:Label ID="lblPurposesList" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr style="height: 200px;">
                    <td colspan="2" style="vertical-align: top; background-color: #aed6f1; padding-left: 15px;
                        padding-top: 15px;" id="purposesListCell" runat="server">
                        <ul id="purposeUl" style="padding-left: 15px; margin-left: 15px;" runat="server">
                        </ul>
                        <asp:Label ID="lblPurposesList" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>--%>
                        <tr>
                            <td class="detailTableColumn1" valign="top" style="vertical-align: top;">
                            </td>
                            <td align="left">
                                <asp:Label ID="lblErrorMsg2" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <br />
            <div id="gridDiv" style="width: 100%; display: inline-block;">
                <div id="Div1" style="max-width: 50%; display: inline-block;">
                    Pending Assessments
                    <hr />
                    <asp:GridView ID="GridViewAssessment" runat="server" AutoGenerateColumns="False"
                        Style="width: 412px;" OnRowDataBound="gridViewAssessment_OnRowDataBound" OnSelectedIndexChanged="gridViewAssessment_SelectedIndexChanged"
                        OnPageIndexChanging="gridViewAssessment_PageIndexChanging" AllowPaging="true">
                        <Columns>
                            <asp:BoundField DataField="ASSESSMENT_ID" HeaderText="AssessmentId" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ASSESSMENT_TYPE_ID" HeaderText="AssessmentTypeId" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ASSESSMENT_NAME" HeaderText="Assessment" />
                            <asp:BoundField DataField="ASSESSMENT_TYPE_NAME" HeaderText="Type" />
                            <asp:BoundField DataField="YEAR_OF_ASSESSMENT" HeaderText="Year" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataRowStyle Width="300px"></EmptyDataRowStyle>
                        <EmptyDataTemplate>
                            <center>
                                No Pending Assessments
                            </center>
                        </EmptyDataTemplate>
                        <%--<HeaderStyle BackColor="#0066CC" />
                <PagerStyle BackColor="#0059b3" />--%>
                    </asp:GridView>
                </div>
                <div id="Div2" style="max-width: 50%; display: inline-block; float: right;">
                    Selected Employees
                    <hr />
                    <asp:GridView ID="GridViewSelectedEmployees" Style="width: 412px; text-align: right"
                        runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="gridViewSelectedEmployees_OnRowDataBound"
                        OnPageIndexChanging="gridViewSelectedEmployees_PageIndexChanging" EmptyDataText="No data found"
                        EmptyDataRowStyle-Width="300px">
                        <Columns>
                            <asp:BoundField DataField="emp_id" HeaderText="Employee Id" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="epf_no" HeaderText="EPF No." ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_name" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                ItemStyle-Width="200px">
                                <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Goal">
                                <HeaderTemplate>
                                    Goal &nbsp;
                                    <%--OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged"--%>
                                    <asp:CheckBox ID="goalHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged"
                                        Enabled="false" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="goalChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="goalChildCheckBox_OnCheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COMP">
                                <HeaderTemplate>
                                    Comp&nbsp;
                                    <asp:CheckBox ID="competencyHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="competencyHeaderCheckBox_OnCheckedChanged"
                                        Enabled="false" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="competencyChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="competencyChildCheckBox_OnCheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SELF">
                                <HeaderTemplate>
                                    Self &nbsp;
                                    <asp:CheckBox ID="selfHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="selfHeaderCheckBox_OnCheckedChanged"
                                        Enabled="false" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selfChildCheckBox" runat="server" OnCheckedChanged="selfChildCheckBox_OnCheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EXCLUDE" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderTemplate>
                                    Exclude &nbsp;
                                    <%--OnCheckedChanged="goalHeaderCheckBox_OnCheckedChanged"--%>
                                    <asp:CheckBox ID="excludeHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="excludeHeaderCheckBox_OnCheckedChanged" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="excludeChildCheckBox" runat="server" OnCheckedChanged="excludeChildCheckBox_OnCheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="email" HeaderText="EMAIL" ItemStyle-HorizontalAlign="Left"
                                ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                        <RowStyle HorizontalAlign="Center" />
                        <EmptyDataRowStyle Width="300px"></EmptyDataRowStyle>
                        <EmptyDataTemplate>
                            <center>
                                No Employees Found
                            </center>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenAssessmentId" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenPreviouseAssessmentYear" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenPreviouseStatus" runat="server" Value="" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenSelectedCompanyId" runat="server" Value="" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
