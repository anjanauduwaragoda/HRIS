<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="Webfrmpriviledges.aspx.cs" Inherits="GroupHRIS.Useradmin.Wedfrmpriviledges" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="styleMainTb">
<tr>
<td class="styleMainTb">
<span style="font-weight: 700">HRIS User Privileges</span>
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
            <td class="styleMainTbCenterTD" colspan="3">
                &nbsp;</td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                User Role : </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlUserrole" runat="server" Height="25px" Width="289px" 
                    AutoPostBack="True" onselectedindexchanged="ddlUserrole_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#3333FF" 
                    Text="User Roles / Functions"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td class="stylestbTbTD2" valign="top">
                <asp:RadioButtonList ID="rdolistmainnode" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="rdolistmainnode_SelectedIndexChanged"   >
                </asp:RadioButtonList>
            </td>
            <td  class="stylestbTbTD3" valign="top">
                <asp:CheckBoxList ID="chklistrestricted" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td class="stylestbTbTD2">
                &nbsp;</td>
            <td  class="stylestbTbTD3">
                &nbsp;</td>
        </tr>
        <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td class="stylestbTbTD2">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
            <td  class="stylestbTbTD3">
                <asp:Button ID="btnupdate" runat="server" Text="Update" Width="112px" 
                    onclick="btnupdate_Click" />
                </td>
        </tr>
    </table>
 
</asp:Content>
