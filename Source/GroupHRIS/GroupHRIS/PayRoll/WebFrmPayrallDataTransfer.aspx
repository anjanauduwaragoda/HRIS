<%@ Page Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmPayrallDataTransfer.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmPayrallDataTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span style="font-weight: 700">Payrall Data Transfer</span><br />
    <hr />
    <br />
    <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company Name :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcompany" runat="server" 
                            onselectedindexchanged="ddlcompany_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" 
                            ErrorMessage="Company is required" Text = "*" ControlToValidate="ddlcompany" 
                            ForeColor="Red" ValidationGroup="transaction"></asp:RequiredFieldValidator>
                       </td>
                </tr>
                <tr>
                <td></td><td>
                    <asp:Button ID="btnTransfer" runat="server" Text="Transfer" 
                        onclick="btnTransfer_Click" ValidationGroup="transaction" /></td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="transaction"
                            ForeColor="RED" />
                    </td>
                </tr>
                </table>

                <br />
            <br />
            <asp:GridView ID="gvPayrallTransfer" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true">
                <Columns>
                    <asp:BoundField DataField="EPF_NO" HeaderText=" EPF NO " />
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category " />
                    <asp:BoundField DataField="SUB_CATEGORY" HeaderText=" Sub category " />
                    <asp:BoundField DataField="TYPE_ID" HeaderText=" Type Id" />
                    <asp:BoundField DataField="FINALIZED_AMOUNT" HeaderText=" Finalized Amount" />
                </Columns>
            </asp:GridView>
</asp:Content>
