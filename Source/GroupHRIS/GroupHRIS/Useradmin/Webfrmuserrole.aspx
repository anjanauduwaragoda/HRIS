<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Webfrmuserrole.aspx.cs" Inherits="GroupHRIS.Useradmin.Webfrmuserrole" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style2
        {
            text-align: right;
            width: 30%;
            height: 24px;
        }
        .style3
        {
            height: 24px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
 <table class="styleMainTb">
<tr>
<td class="styleMainTb">
<span style="font-weight: 700">HRIS Roles</span>
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
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                <asp:Label ID="lblrefcode" runat="server" ForeColor="White"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                User Role Name : </td>
            <td>
                <asp:TextBox ID="txtuserrole" runat="server" Width="284px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtuserrole_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtuserrole"  ValidChars="0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcedfghijklmnopqrstuvwxyz " >
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                User Role Status : </td>
            <td>
                <asp:DropDownList ID="ddlrolestatus" runat="server" Height="20px" Width="115px"   >
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2">
                User Role Category : </td>
            <td class="style3">
                <asp:DropDownList ID="ddlrolecategory" runat="server" Height="20px" 
                    Width="184px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="0">User Role</asp:ListItem>
                    <asp:ListItem Value="1">Common User Role</asp:ListItem>
                </asp:DropDownList>
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
                <asp:Button ID="btnupdate" runat="server" Text="Update" Width="112px" 
                    onclick="btnupdate_Click" />
                <asp:Button ID="btnclose" runat="server" Text="Clear" Width="97px" 
                    onclick="btnclose_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                <asp:HiddenField ID="hfuserrole" runat="server" />
            </td>
        </tr>
    </table>
<table class="styleMainTb">
<tr>
<td class="styleMainTb" align="center">
   
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
   
</td>
</tr>
<tr>
<td class="styleMainTb" align="center">
   
    <hr />
   
</td>
</tr>
<tr>
<td class="styleMainTb" align="center">
   
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    onpageindexchanging="GridView1_PageIndexChanging" PageSize="8" 
                        onrowdatabound="GridView1_RowDataBound" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <PagerSettings PageButtonCount="2" />
                </asp:GridView>
   
</td>
</tr>
</table>

</asp:Content>
