<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmCompanyPropertyCategory.aspx.cs" Inherits="GroupHRIS.MetaData.Company.webFrmCompanyPropertyCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span style="font-weight: 700">Cpmpany Property Category</span><br />
    <hr />
    <br />
    <table class="styleMainTb">
     <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Property Id :
            </td>
            <td>
               <%-- <asp:Label ID="txtRemarks" runat="server" ></asp:Label>--%>
                <asp:Label ID="lblPropertyId" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Property Name :
            </td>
            <td>
                <asp:TextBox ID="txtProperty" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrperty" ValidationGroup="Property" runat="server"
                    Text="*" ForeColor="Red" ErrorMessage="Property is Required" ControlToValidate="txtProperty"></asp:RequiredFieldValidator>
            </td>
        </tr>
       
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStstus" runat="server" Width="100px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="Status is required" ForeColor="Red" ControlToValidate="ddlStstus"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Property"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Property"
                    ForeColor="RED" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:GridView ID="gvProperty" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
        AllowPaging="true" OnPageIndexChanging="gvProperty_PageIndexChanging" OnRowDataBound="gvProperty_RowDataBound"
        OnSelectedIndexChanged="gvProperty_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property Id " />
            <asp:BoundField DataField="DESCRIPTION" HeaderText=" Property Name " />
            <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
        </Columns>
    </asp:GridView>
</asp:Content>
