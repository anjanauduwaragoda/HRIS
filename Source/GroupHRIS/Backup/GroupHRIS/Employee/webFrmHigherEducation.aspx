<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmHigherEducation.aspx.cs" Inherits="GroupHRIS.Employee.webFrmHigherEducation" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=1366,height=768,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
<%--    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfCaller").value = ctlName;
        }


        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("txtEmployeeID").value = sEmpId;
            document.getElementById("txtName").value = sName;

            writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName);

            DoPostBack();
        }

        function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
        }

        function DoPostBack() {
            __doPostBack("txtEmployeeID", "TextChanged");
        }


    </script>--%>

    <style type="text/css">
    .hideGridColumn
    {
        display:none;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Higher Education</span>
    <hr />
    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" Text="Employee" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;
                <%if (isSearchable)%>                <%{%>
                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" 
                    onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" 
                    id="imgSearch" />
                <%}%>
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="Employee ID  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" 
                    ReadOnly="True" BorderStyle="None" ForeColor="Blue"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Institute" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtInstitute" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtInstitute"
                    ErrorMessage="Institute  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                Program</td>
            <td>
                <asp:TextBox ID="txtProgram" runat="server" Width="125px" MaxLength="25"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtProgram"
                    ErrorMessage="Program  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Program Name" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtProgramName" runat="server" Width="250px" MaxLength="45"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtProgramName"
                    ErrorMessage="Program Name  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" Text="Sector" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSector" runat="server" Width="250px" MaxLength="45"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSector"
                    ErrorMessage="Sector  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label7" runat="server" Text="Duration" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDurationYears" runat="server" Width="50px" MaxLength="1" ></asp:TextBox>(Years)
                <asp:FilteredTextBoxExtender ID="txtDurationYearsFilteredTextBoxExtender" runat="server" TargetControlID="txtDurationYears" FilterType="Numbers">
                </asp:FilteredTextBoxExtender>                

                <asp:TextBox ID="txtDurationMonths" runat="server" Width="50px" MaxLength="2" ></asp:TextBox>(Months)
                <asp:FilteredTextBoxExtender ID="txtDurationMonthsFilteredTextBoxExtender" runat="server" TargetControlID="txtDurationMonths" FilterType="Numbers">
                </asp:FilteredTextBoxExtender>   

            </td>
            <td>

            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label8" runat="server" Text="From Year / To Year" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlFromYear" runat="server" Width="59px">
                </asp:DropDownList>
            &nbsp;/
                <asp:DropDownList ID="ddlToYear" runat="server" Width="59px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlFromYear"
                    ErrorMessage="From Year  is required." ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Grade" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtGrade" runat="server" Width="125px" MaxLength="20"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="vertical-align:text-top;">
                <asp:Label ID="Label10" runat="server" Text="Remarks" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                             
                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" Rows="2" 
                    TextMode="MultiLine" Width="250px"></asp:TextBox>
                             
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfLineNo" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td align="left">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" 
                    OnClick="btnSave_Click" ValidationGroup="vgSubmit" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    OnClick="btnCancel_Click" />
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    <tr> 
        <td align="left">
            <asp:ValidationSummary ID="vsSubmit" runat="server" 
                ValidationGroup="vgSubmit" ForeColor="Red" />
        </td>
    </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
    </table>


    <br />
    <span>List of Higher Education Records</span>
    <hr />
    <br />

    <div>
        <asp:GridView ID="gvHighEdu" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" OnPageIndexChanging="gvSecEdu_PageIndexChanging" 
            onrowdatabound="gvHighEdu_RowDataBound" 
            onselectedindexchanged="gvHighEdu_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField HeaderText="Institute" DataField="INSTITUTE" />
                <asp:BoundField HeaderText="Program" DataField="PROGRAM" />
                <asp:BoundField HeaderText="Program Name" DataField="PROGRAME_NAME" />
                <asp:BoundField HeaderText="Sector" DataField="SECTOR" />
                <asp:BoundField HeaderText="Duration" DataField="DURATION" />
                <asp:BoundField HeaderText="From Year" DataField="FROM_YEAR" />
                <asp:BoundField HeaderText="To Year" DataField="TO_YEAR" />
                <asp:BoundField HeaderText="Grade" DataField="GRADE" />
                <asp:BoundField HeaderText="Remarks" DataField="REMARKS" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"  />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
