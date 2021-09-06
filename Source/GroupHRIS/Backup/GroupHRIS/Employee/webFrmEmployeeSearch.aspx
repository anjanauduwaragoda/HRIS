<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webFrmEmployeeSearch.aspx.cs"
    Inherits="GroupHRIS.Employee.webFrmEmployeeSearch" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Employee</title>
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


        function sendValueToParent_() {
            var sRetVal = document.getElementById("txtEmployeeNo").value;

            window.opener.getValueFromChild(sRetVal);

            window.close();
            return false;
        }

        function sendValueToParent() {
            var sEmpId = document.getElementById("txtEmployeeNo").value;
            var sName = document.getElementById("hfName").value;
            var sCompId = document.getElementById("hfCompanyCode").value;
            var sCompName = document.getElementById("hfCompanyName").value;
            var sDepartmentId = document.getElementById("hfDepartmentID").value;
            var sDepartmentName = document.getElementById("hfDepartmentName").value;
            var sDivisionId = document.getElementById("hfDivisionID").value;
            var sDivisionName = document.getElementById("hfDivisionName").value;

            //2014-09-25
            var sBranchId = document.getElementById("hfBranchID").value;
            var sBranchName = document.getElementById("hfBranchName").value;
            var sCC = document.getElementById("hfCC").value;
            var sPC = document.getElementById("hfPC").value;

            //2015-08-03
            var sEPF = document.getElementById("hfEPF").value;
            var sDesignation = document.getElementById("hfDesignation").value;
            var sDesigName = document.getElementById("hfDesigName").value;

            //2016-11-02 Anjana Uduwaragoda
            var sTitle = document.getElementById("hfTitle").value;

            //window.opener.getValueFromChild(sEmpId, sName, sCompId, sCompName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName);
            window.opener.getValueFromChild(sEmpId, sName, sCompId, sCompName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName, sBranchId, sBranchName, sCC, sPC, sEPF, sDesignation, sDesigName, sTitle);

            window.close();
            return false;
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 81px;
        }
    </style>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td class="style1">
                    Company
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompany" runat="server" Width="220px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Department
                </td>
                <td>
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="180px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Division
                </td>
                <td>
                    <asp:DropDownList ID="ddlDivision" runat="server" Width="180px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Status
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="220px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    EPF No.
                </td>
                <td>
                    <asp:TextBox ID="txtEPFNo" runat="server" Width="175px" ClientIDMode="Static" OnTextChanged="txtEPFNo_TextChanged"></asp:TextBox>
                </td>
                <td>
                    NIC
                </td>
                <td>
                    <asp:TextBox ID="txtNIC" runat="server" Width="175px" ClientIDMode="Static" OnTextChanged="txtNIC_TextChanged"></asp:TextBox>
                </td>
                <td style="margin-left: 80px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Name
                </td>
                <td>
                    <asp:TextBox ID="txtSearchName" runat="server" Width="213px" ClientIDMode="Static"></asp:TextBox>
                </td>
                <td>
                    Designation
                </td>
                <td>
                    <asp:DropDownList ID="ddlDesignation" runat="server" Width="180px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="margin-left: 80px">
                    <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                        OnClick="imgbtnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblEmployee" runat="server" Text="Employee" Width="81px" Font-Bold="True" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtEmployeeNo" runat="server" Width="80px" BorderStyle="None" Font-Bold="True" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="txtEmpName" runat="server" Width="300px" BorderStyle="None" Font-Bold="True" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" OnClientClick="sendValueToParent()" Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-weight: 700">Employee Information</span>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:HiddenField ID="hfName" runat="server" />
                    <asp:HiddenField ID="hfCompanyCode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDepartmentID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDivisionID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfCompanyName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDepartmentName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDivisionName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfBranchID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfBranchName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfCC" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfPC" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfEPF" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDesignation" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDesigName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfTitle" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:GridView ID="gvEmployee" Style="margin: auto;" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" PageSize="7" OnPageIndexChanging="gvEmployee_PageIndexChanging"
            OnRowDataBound="gvEmployee_RowDataBound" OnSelectedIndexChanged="gvEmployee_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" />
                <asp:BoundField DataField="EPF_NO" HeaderText="EPF" />
                <asp:BoundField DataField="TITLE" HeaderText="Title" />
                <asp:BoundField DataField="KNOWN_NAME" HeaderText="Known Name" ItemStyle-Width="100px"
                    HeaderStyle-Width="100px">
                    <HeaderStyle Width="100px"></HeaderStyle>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EMP_NIC" HeaderText="NIC" />
                <asp:BoundField DataField="EMPLOYEE_STATUS" HeaderText="Status Code" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Status" />
                <asp:BoundField DataField="COMPANY_ID" HeaderText="Comp. Code" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="COMP_NAME" HeaderText="Company">
                    <ControlStyle Width="250px" />
                    <FooterStyle Width="250px" />
                    <HeaderStyle Width="250px" />
                    <ItemStyle Width="250px" />
                </asp:BoundField>
                <asp:BoundField DataField="DEPT_ID" HeaderText="Dept ID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DEPT_NAME" HeaderText="Department" />
                <asp:BoundField DataField="DIVISION_ID" HeaderText="Div ID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DIV_NAME" HeaderText="Division" />
                <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch ID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="BRANCH_NAME" HeaderText="Branch" />
                <asp:BoundField DataField="COST_CENTER" HeaderText="Cost Center" />
                <asp:BoundField DataField="PROFIT_CENTER" HeaderText="Prof Center" />
                <asp:BoundField DataField="DESIGNATION_ID" HeaderText="Desig ID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DESIGNATION_NAME" HeaderText="Designation" />
            </Columns>
            <PagerSettings PageButtonCount="15" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>