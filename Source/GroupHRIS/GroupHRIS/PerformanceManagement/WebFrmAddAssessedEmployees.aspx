<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmAddAssessedEmployees.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmAddAssessedEmployees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            //window.moveTo(20, 20);
            window.resizeTo(_width, _height)
            window.focus();


        }

        function closeWindow() {
            window.close();
        }

        window.onunload = function (e) {
            opener.sendToParent();

        };

        function sendToParent() {
            var msg = "dataCaptured";
            //var companyddl = document.getElementById("ddlCompany");
            //var companyId = companyddl.options[companyddl.selectedIndex].value

            window.opener.getValuefromChild(msg);
            window.close();

        }

        
    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        
        .style1
        {
            width: 50px;
            padding-left: 5px;
        }
        .style2
        {
            width: 26px;
            padding-left: 5px;
        }
    </style>
</head>
<body class="popupsearch">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <%--<td class="style1">
                        <asp:Label ID="Label1" runat="server" Text="Company" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" Visible="false">
                        </asp:DropDownList>
                    </td>--%>
                        <td class="style2" align="left">
                            Department
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="200px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style1" align="left">
                            Division
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDevision" runat="server" Width="200px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="style1" align="left">
                            EPF NO
                        </td>
                        <td>
                            <%--<asp:DropDownList ID="" runat="server" Width="180px" AutoPostBack="True">
                        </asp:DropDownList>--%>
                            <asp:TextBox ID="txtEpf" runat="server" Width="193px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                OnClick="imgbtnSearch_Click" />
                        </td>
                    </tr>
                    <%--<tr>
                <td class="style1">
                        EPF NO
                    </td>
                    <td>
                        
                        <asp:TextBox ID="txtEpf" runat="server" Width="193px"></asp:TextBox>
                    </td>

                    <td>
                        <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                            OnClick="imgbtnSearch_Click" />
                    </td>
                </tr>--%>
                    <tr>
                        <td class="style2" align="right">
                            <asp:Button ID="btnAdd" runat="server" Text="<< Add" Width="75px" Style="height: 26px;"
                                OnClientClick="sendToParent()" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <b>Employee Information : 
                <asp:Label ID="lblCompanyName" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                <hr />
            </b>
            <div>
                <asp:GridView ID="employeeGridView" runat="server" Style="margin: auto;" AllowPaging="True"
                    AutoGenerateColumns="false" OnSelectedIndexChanged="employeeGridView_SelectedIndexChanged"
                    OnPageIndexChanging="employeeGridView_PageIndexChanging" OnRowDataBound="employeeGridView_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" ItemStyle-Width="100px"
                            HeaderStyle-Width="50px" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="EPF_NO" HeaderText="EPF No" ItemStyle-Width="100px" HeaderStyle-Width="50px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TITLE" HeaderText="Title" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="KNOWN_NAME" HeaderText="Known Name" ItemStyle-Width="200px"
                            HeaderStyle-Width="200px">
                            <HeaderStyle Width="200px"></HeaderStyle>
                            <ItemStyle Width="200px" HorizontalAlign="Left"></ItemStyle>
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
                        <asp:TemplateField HeaderText="Competency">
                            <HeaderTemplate>
                                Competency &nbsp;
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
                        <asp:TemplateField HeaderText="Include">
                            <HeaderTemplate>
                                Include &nbsp;
                                <asp:CheckBox ID="includeHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="includeHeaderCheckBox_OnCheckedChanged"
                                    ClientIDMode="Static" /></HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="includeChildCheckBox" runat="server" OnCheckedChanged="includeChildCheckBox_OnCheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="COMPANY_ID" HeaderText="Comp. Code" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DEPT_ID" HeaderText="Dept ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DIVISION_ID" HeaderText="Div ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="REPORT_TO_1" HeaderText="Report to" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ROLE" HeaderText="ROle" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DESIGNATION_ID" HeaderText="Designation" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <RowStyle HorizontalAlign="Center" />
                </asp:GridView>
                <div>
                    <table>
                        <tr>
                            <td style="width: 30%">
                            </td>
                            <td style="width: 60%">
                            </td>
                            <td style="width: 30%">
                                <%--<asp:Button ID="btnAdd" runat="server" Text="Add" Width="125px"
                    Style="height: 26px;" OnClientClick = "sendToParent()"/>--%>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr style="width: 100%">
                            <td style="width: 100%;text-align:center">
                                <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: 200px">
                                    <ProgressTemplate>
                                        <img src="../Images/ProBar/720.GIF" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hfIncludeSelectAll" Value="" ClientIDMode="Static" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
