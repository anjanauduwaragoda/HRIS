<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false" 
    CodeBehind="WebFrmEmployeePropertyDetails.aspx.cs" Inherits="GroupHRIS.Property.wWebFrmEmployeePropertyDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
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

        function displayData(val) {

            document.getElementById("hfisInclude").value = val;

            DoPostBack();
        }


        function DoPostBack() {
            __doPostBack();
        }
    </script>
        <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <span style="font-weight: 700">Employee Benefit Information</span><br />
    <hr />
    <br />
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="width: 200px; text-align: right">
                            Employee Id :
                        </td>
                        <td style="width: 250px; text-align: left">
                            <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static"
                                Width="200px" OnTextChanged="txtEmploeeId_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" Text=" * " ErrorMessage="Employee Id is required"
                                ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="Property"></asp:RequiredFieldValidator>
                            <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            Employee Name :
                        </td>
                        <td>
                            <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            Assigned Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssignDate" runat="server" Width="200px"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server" ValidationGroup="Property" ErrorMessage="Assign date is required"
                                Text=" * " ForeColor="Red" ControlToValidate="txtAssignDate"></asp:RequiredFieldValidator>
                            <asp:FilteredTextBoxExtender ID="fteAssignDate" FilterType="Custom, Numbers" ValidChars="/,-"
                                runat="server" TargetControlID="txtAssignDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="ceAssignDate" runat="server" TargetControlID="txtAssignDate"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                                Text="*" ErrorMessage="Invalied date format" ValidationGroup="Property" ControlToValidate="txtAssignDate"
                                ForeColor="Red"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                        </td>
                        <td style="text-align: left">
                            <%--<span>(DD/MM/YYYY)</span>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            <%-- Returned Date :--%>
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="txtReturnedDate" runat="server" Width="200px" Visible="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fteRuturnedDate" FilterType="Custom, Numbers" ValidChars="/"
                                runat="server" TargetControlID="txtReturnedDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="ceReturnedDate" runat="server" TargetControlID="txtReturnedDate"
                                Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="styleMainTbLeftTD">
                        </td>
                        <td style="text-align: left">
                            <span>(YYYY/MM/DD)</span>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            Clearance Mail :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="200px" ValidationGroup="Property"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Property"
                                ErrorMessage="Clearance mail is required" Text=" * " ForeColor="Red" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="Property"
                                ControlToValidate="txtEmail" ForeColor="Red" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Text="*" ErrorMessage="Invalid e-mail address"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            Remarks :
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="txtRemarks" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan= "3" align = "center">
                            <asp:GridView ID="grdViewBenefits" runat="server"  
                                Style="margin: auto;width:350px" AutoGenerateColumns="false"
                                AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Benefit Name " />
                                    <asp:BoundField DataField="REFERENCE_NO" HeaderText=" Referemce No " />
                                    <asp:BoundField DataField="MODEL" HeaderText=" Model " />
                                    <asp:BoundField DataField="SERIAL_NO" HeaderText=" Serial No " />
                                    <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property id " HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Property Type id" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleMainTbLeftTD">
                            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                            <asp:HiddenField ID="hfisInclude" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Text="Save" Width="100px" OnClick="btnUpdate_Click"
                                ValidationGroup="Property" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr><td></td><td><asp:Label ID="lblMessage" runat="server"></asp:Label></td></tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="StatusLabel" runat="server"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Property"
                                ForeColor="RED" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top" rowspan="20" height="100" width="200">
                <div class="MenuBar">
                    <br />
                    <asp:Literal ID="litDefaultProperty" runat="server"></asp:Literal>
                </div>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvemployeeProperty" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
        AllowPaging="true" OnRowDataBound="gvemployeeProperty_RowDataBound" 
        onselectedindexchanged="gvemployeeProperty_SelectedIndexChanged" 
        onpageindexchanging="gvemployeeProperty_PageIndexChanging" >
        <Columns>
            <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Name " />
            <asp:BoundField DataField="ASSIGNED_DATE" HeaderText=" Assign Date " />
            <asp:BoundField DataField="RETURNED_DATE" HeaderText=" Return Date " />
            <asp:BoundField DataField="CLEARANCE_MAIL" HeaderText=" Clearance mail " />
            <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
            <asp:BoundField DataField="PROPERTY_STATUS" HeaderText=" Status " />
            <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property Id "  HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Property Type Id "  HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="LINE_ID" HeaderText=" Line Id "  HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
            
            <asp:BoundField DataField="REMOVED_REASON" HeaderText=" Removed Reason " />
        </Columns>
    </asp:GridView>
</asp:Content>
