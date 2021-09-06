<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmMeargeCompany.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmMeargeCompany" %>

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
    <span style="font-weight: 700">Mearge Company</span><br />
    <hr />
    <br />
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTbLeftTD">
                Company :
            </td>
            <td>
                <asp:DropDownList ID="ddlCompany" runat="server" Width="250px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ForeColor="Red" ControlToValidate="ddlCompany"
                    Text="*" ErrorMessage="Company is Required" ValidationGroup="mearge"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Query Name :
            </td>
            <td>
                <%--<asp:TextBox ID="txtQuery" runat="server" Width="200px"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlQuery" runat="server" Width="250px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvQury" runat="server" Text="*" ErrorMessage="Query Name is Required"
                    ValidationGroup="mearge" ForeColor="Red" ControlToValidate="ddlQuery"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Remarks :
            </td>
            <td>
                <asp:TextBox ID="txtRemarks" runat="server" Width="247px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                    ValidationGroup="mearge" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server"> </asp:Label><br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="mearge" />
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
            </td>
            <td align="center" style="width: 100%">
                <asp:GridView ID="gvMergeCompany" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grdMergeCompany_PageIndexChanging" OnRowDataBound="grdMergeCompany_RowDataBound"
                    OnSelectedIndexChanged="grdMergeCompany_SelectedIndexChanged" PageSize="7">
                    <Columns>
                        <asp:BoundField DataField="MEARGE_ID" HeaderText="Mearge" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                        <asp:BoundField DataField="PROCEDURE_NAME" HeaderText="Stored Procedure" />
                        <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
