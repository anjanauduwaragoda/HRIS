<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmEmployeeMarksStandardization.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmEmployeeMarksStandardization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Charts/Chart.bundle.js" type="text/javascript"></script>
    <script src="../Scripts/Charts/jquery.min.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span>Employee Marks Standardization</span><span style="font-weight: 700;"> </span>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfFinancialYear" runat="server" />
            <asp:HiddenField ID="hfFinancialYearDetails" runat="server" />
            <table style="margin: auto; height: 50px; padding-left: 10px; padding-right: 10px;
                padding-bottom: 5px;" class="divsearch">
                <tr>
                    <td style="text-align: right;" class="style1">
                        Company&nbsp; :
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlCompany" Width="175px" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style2">
                    </td>
                    <td style="text-align: right;" class="style1">
                        Department :
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlDepartment" Width="175px" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style2">
                    </td>
                    <td style="text-align: right;" class="style1">
                        Division :
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlDivision" Width="175px" runat="server" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style3">
                    </td>
                    <td class="style1">
                        <asp:ImageButton ID="imgbtnSearch" runat="server" ToolTip="Search" ImageUrl="~/Images/Common/user-search-icon.png"
                            OnClick="imgbtnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Year :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYearOfAssessment" Width="175px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlYearOfAssessment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        Assessment :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAssessment" Width="175px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssessment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />

            <table style="margin: auto; min-width: 815px;">
                <tr>
                    <td style="min-height:250px;">
                        <asp:GridView ID="grdvAssessment" AutoGenerateColumns="false" Style="width: 815px;"
                            PageSize="10" AllowPaging="false" runat="server" OnRowDataBound="grdvAssessment_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="NAME" HeaderText=" Employee " />
                                <asp:BoundField DataField="DESIGNATION_NAME" HeaderText=" Designation " />
                                <asp:BoundField DataField="KPI_SCORE" ItemStyle-HorizontalAlign="Right" HeaderText=" Goal Score " />
                                <asp:BoundField DataField="COMPITANCY_SCORE" ItemStyle-HorizontalAlign="Right" HeaderText=" Competency Score " />
                                <asp:BoundField DataField="TOTAL" ItemStyle-HorizontalAlign="Right" HeaderText=" Total Score " />
                                <asp:BoundField DataField="STANDARDIZED_TOTAL" ItemStyle-HorizontalAlign="Right" HeaderText=" Standardized Total " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:TemplateField HeaderText="Total Score [new]">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotalScore" Width="98%" Style="text-align: right" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnApply" runat="server" Width="100px" Text="Apply" onclick="btnApply_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <%--<tr>
                    <td style="text-align: right;">
                        <asp:Button ID="btnSave" Width="150px" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Width="150px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <table style="margin: auto;">
                            <tr>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
          
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
