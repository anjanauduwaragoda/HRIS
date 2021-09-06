<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Webfrmprivilsubscheme.aspx.cs" Inherits="GroupHRIS.Useradmin.Webfrmprivilsubscheme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="styleMainTb">
<tr>
<td class="styleMainTb">
<span style="font-weight: 700">HRIS Sub </span><strong>Privileges </strong>
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
            <td class="styleMainTbCenterTD" colspan="2"/>
        </tr>
        <tr>
            
            <td colspan="2" align="center">
            <div class="stylediv">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    onpageindexchanging="GridView1_PageIndexChanging" PageSize="8" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged" 
                    onrowdatabound="GridView1_RowDataBound">
                    <PagerSettings PageButtonCount="2" />
                    <SelectedRowStyle Font-Bold="False" />
                </asp:GridView>
                </div>
                <br />
                <br />
            </td>
            
        </tr>

        </table>
        <br /><br /><br /><br /><br /><br />
        <table class="styleMainTb">

        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:HiddenField ID="hfsubprivilage" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" >
                Privilege Scheme : </td>
            <td>
                <asp:DropDownList ID="ddlMainnodes" runat="server" AutoPostBack="True" 
                    Height="20px" onselectedindexchanged="ddlMainnodes_SelectedIndexChanged" 
                    Width="300px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Sub Privilege Code : </td>
            <td>
                <asp:Label ID="lblsubnodecode" runat="server" Font-Bold="True" 
                    ForeColor="#0066FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Sub Privilege Scheme : </td>
            <td>
                <asp:TextBox ID="txtsubnode" runat="server" Width="350px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Re-Direct Url : </td>
            <td>
                <asp:TextBox ID="txturl" runat="server" Width="450px" MaxLength="100"></asp:TextBox>
            </td>
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
