<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="WebFrmCompanyRole.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmCompanyRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span><strong>Employee Role and Over Time Category Mapping</strong> </span>
            <br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" DataTextField="CATEGORY"
                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCompany" ValidationGroup="CompanyRole"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Company is required" ControlToValidate="ddlCompany"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        OT Category :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOTCategory" runat="server" Width="200px" 
                            DataTextField="COMPANY_OT_CATEGORY_NAME" 
                            onselectedindexchanged="ddlOTCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="fvOTCategory" ValidationGroup="CompanyRole"
                            runat="server" Text="*" ForeColor="Red" 
                            ErrorMessage="OT Category is required" 
                            ControlToValidate="ddlOTCategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Employee Role :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRole" runat="server" Width="200px" 
                            DataTextField="ROLE_NAME" 
                            onselectedindexchanged="ddlRole_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRole" ValidationGroup="CompanyRole"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Employee Role is required"
                            ControlToValidate="ddlRole"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td class="styleMainTbLeftTD">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text = "*" ValidationGroup="CompanyRole"
                            ErrorMessage="Status is required " ControlToValidate="ddlStatus" 
                            ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>

               <%-- <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td><asp:CheckBox ID="chkIsActive" runat="server" Text = " Is Active"></asp:CheckBox>
                     </td>
                </tr>--%>

                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="CompanyRole"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label><br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CompanyRole"
                            ForeColor="RED" />
                    </td></tr>
                </table>

            <br />
            <br />
            <asp:GridView ID="gvCompanyRoleType" Style="margin: auto;" runat="server"
                AutoGenerateColumns="False" AllowPaging="true" 
        onpageindexchanging="gvCompanyRoleType_PageIndexChanging" 
        onrowdatabound="gvCompanyRoleType_RowDataBound" 
        onselectedindexchanged="gvCompanyRoleType_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="COMPANY_ID" HeaderText="Company Name " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="OT_CATEGORY_NAME" HeaderText=" OT Category " />
                    <asp:BoundField DataField="ROLE_NAME" HeaderText=" Employee Role " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status"/>
                </Columns>
            </asp:GridView>
</asp:Content>
