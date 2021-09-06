<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCompanyOTCategory.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmCompanyOTCategory" %>

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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <span><strong>Company Employee OT Category</strong> </span>
    <br />
    <hr />
    <br />
    <table>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Company :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCompany" ValidationGroup="OtCategory" runat="server"
                    Text="*" ForeColor="Red" ErrorMessage="Company is Required" ControlToValidate="ddlCompany"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                OT Category Name :
            </td>
            <td>
                <asp:TextBox ID="txtOTcategory" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers,Custom,LowercaseLetters,UppercaseLetters"
                    ValidChars="-_" TargetControlID="txtOTcategory" runat="server">
                </asp:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="rfvOTCategoryName" ValidationGroup="OtCategory" runat="server"
                    Text="*" ForeColor="Red" ErrorMessage="OT Category is Required" ControlToValidate="txtOTcategory"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Description :
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:CheckBox ID="chkIsActive" runat="server" Text=" Is Active"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="OtCategory"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="StatusLabel" runat="server"> </asp:Label><br />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="OtCategory"
                    ForeColor="RED" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
    <br />
    <asp:GridView ID="gvCompanyOTCategory" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
        AllowPaging="true" OnPageIndexChanging="gvCompanyOTCategory_PageIndexChanging"
        OnRowDataBound="gvCompanyOTCategory_RowDataBound" OnSelectedIndexChanged="gvCompanyOTCategory_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="COMPANY_ID" HeaderText="Company Id " HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="COMPANY_OT_CATEGORY_NAME" HeaderText="Company OT Category Name " />
            <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " />
            <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
            <asp:BoundField DataField="ROLE_CATEGORY_ID" HeaderText="Role category Id " HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
        </Columns>
    </asp:GridView>
    <%--</ContentTemplate>--%>
</asp:Content>
