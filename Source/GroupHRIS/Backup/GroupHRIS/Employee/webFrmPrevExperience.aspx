<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="webFrmPrevExperience.aspx.cs" Inherits="GroupHRIS.Employee.webFrmPrevExperience" EnableEventValidation="false" %>
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
    <span>Previous Experience</span>
    <hr />
    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" 
                    Text="Employee Id" AssociatedControlID="txtEmployeeID"></asp:Label>
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
                    ErrorMessage="Employee Id is required" ValidationGroup="vgSubmit" 
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
                <asp:Label ID="Label3" runat="server" Text="Organization" 
                    AssociatedControlID="txtOrganization"></asp:Label>
                    </td>
            <td>
                <asp:TextBox ID="txtOrganization" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                    </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="txtOrganization" ErrorMessage="Organization  is required" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
        </tr>


        <tr>
            <td  align="right">
                <asp:Label ID="Label9" runat="server" Text="From Date" 
                    AssociatedControlID="txtFromDate"></asp:Label>
            </td>

            <td  align="left">
                             
                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" Width="250px" ></asp:TextBox>

                <asp:CalendarExtender ID="ceStartDate" runat="server" 
                    TargetControlID="txtFromDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>

                <asp:FilteredTextBoxExtender ID="fteFromDate" runat="server" TargetControlID="txtFromDate" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender> 
                             
            </td>

            <td  align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtFromDate" ErrorMessage="From Date  is required" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>


        <tr>
            <td  align="right">
                <asp:Label ID="Label11" runat="server" Text="To Date" 
                    AssociatedControlID="txtToDate"></asp:Label>
            </td>

            <td  align="left">
                             
                <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>

                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                    TargetControlID="txtToDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>

                <asp:FilteredTextBoxExtender ID="fteToDate" runat="server" TargetControlID="txtToDate" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender> 

                             
            </td>

            <td  align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="txtToDate" ErrorMessage="To Date is required" ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>



        <tr>
            <td align="right">
                <asp:Label ID="Label12" runat="server" Text="Designation"></asp:Label>
                    </td>
            <td>
                <asp:TextBox ID="txtDesignation" runat="server" Width="250px"></asp:TextBox>
                    </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ControlToValidate="txtDesignation" ErrorMessage="Designation is required" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
        </tr>


        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="Phone Number"></asp:Label>
                    </td>
            <td>
                <asp:TextBox ID="txtPhoneNumber" runat="server" Width="250px" MaxLength="15"></asp:TextBox>
                    </td>

                <asp:FilteredTextBoxExtender ID="fttxtPhoneNumber" runat="server" TargetControlID="txtPhoneNumber" FilterType="Custom, Numbers" ValidChars="+" >
                </asp:FilteredTextBoxExtender> 
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtPhoneNumber" ErrorMessage="Phone Number is required" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
        </tr>

                <tr>
            <td align="right">
                <asp:Label ID="Label13" runat="server" Text="Address" 
                    AssociatedControlID="txtAddress"></asp:Label>
                    </td>
            <td>
                             
                <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" Rows="4" 
                    TextMode="MultiLine" Width="250px"></asp:TextBox>
                             
                    </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="txtAddress" ErrorMessage="Address is required" ValidationGroup="vgSubmit" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
        </tr>



                <tr>
            <td align="right">
                <asp:Label ID="Label14" runat="server" Text="Remarks" 
                    AssociatedControlID="txtRemarks"></asp:Label>
                    </td>
            <td>
                             
                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="45" Rows="3" 
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


    <table  class="styleMainTb">
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
    <span>List of Previous Employment</span>
    <hr />
    <br />

    <div>
    
        <asp:GridView ID="gvPrevEmp" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="10" onpageindexchanging="gvPrevEmp_PageIndexChanging" onrowdatabound="gvPrevEmp_RowDataBound" onselectedindexchanged="gvPrevEmp_SelectedIndexChanged" AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"  />
                <asp:BoundField DataField="ORGANIZATION" HeaderText="Organization" />
                <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" />
                <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                <asp:BoundField DataField="FROM_DATE" HeaderText="From Date" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="TO_DATE" HeaderText="To Date" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"  />
                <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Phone" />
                <asp:BoundField DataField="RECORD_STATUS" HeaderText="Status" />                
            </Columns>
        </asp:GridView>
    
    </div>


</asp:Content>
