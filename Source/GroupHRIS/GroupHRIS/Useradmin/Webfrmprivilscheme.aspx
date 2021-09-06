<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"  EnableEventValidation ="false" CodeBehind="Webfrmprivilscheme.aspx.cs" Inherits="GroupHRIS.Useradmin.Webfrmprivilscheme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="styleMainTb">
<tr>
<td class="styleMainTb">
<span style="font-weight: 700">HRIS Main Privileges</span>
</td>
</tr>
<tr>
<td class="styleMainTb">
    <hr />
</td>
</tr>
</table>
 <table class="styleMainTb">
        <tr>
            <td class="styleMainTbCenterTD" colspan="2">
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div class="stylediv" >
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    onpageindexchanging="GridView1_PageIndexChanging" PageSize="8" 
                        onrowdatabound="GridView1_RowDataBound" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" >
                    <PagerSettings PageButtonCount="2" />
                </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
               Privilege Scheme Code :</td>
            <td>
                <asp:Label ID="lblnodecode" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
               Privilege Scheme [ Main Nodes ] : </td>
            <td>
                <asp:TextBox ID="txtmainnodes" runat="server" Width="350px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnupdate" runat="server" Text="Save" Width="112px" 
                    onclick="btnupdate_Click" />
                <asp:Button ID="btnclose" runat="server" Text="Clear" Width="97px" 
                    onclick="btnclose_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
