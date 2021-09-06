<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmDefaultPropertySet.aspx.cs" Inherits="GroupHRIS.Property.WebFrmDefaultPropertySet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTb">
                <span style="font-weight: 700">Default Benefit Set</span></td>
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
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="stylestbTbTD1">
                Employee Role :
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlUserrole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUserrole_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlUserrole" ForeColor="Red" ValidationGroup="Main" runat="server" Text="*" ErrorMessage="Employee Role is Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="stylestbTbTD1">
                &nbsp;
            </td>
            <td colspan="2">
                <asp:HiddenField ID="hfroleId" runat="server" />
            </td>
        </tr>
        <%-- <tr>
            <td  class="stylestbTbTD1">
                &nbsp;</td>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#3333FF" 
                    Text="Company Properties"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td class="stylestbTbTD1">
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </td>
            <td class="stylestbTbTD2" valign="top" colspan = "2">
                <asp:CheckBoxList ID="chklistrestricted" runat="server" CellPadding="5" CellSpacing="25"
                    RepeatColumns="4" RepeatDirection="Horizontal" RepeatLayout="Table" >
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="stylestbTbTD1">
                &nbsp;
            </td>
            <td class="stylestbTbTD2">
                <asp:ValidationSummary ForeColor="Red" ID="ValidationSummary1" ValidationGroup="Main" runat="server" />
                <asp:Literal ID="ltlproperty" runat="server"></asp:Literal>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            </td>
            <td class="stylestbTbTD3">
                <asp:CheckBox ID="chkActive" runat="server" Text="Is Active" />
            </td>
        </tr>
        <tr>
            <td class="stylestbTbTD1">
                &nbsp;
            </td>
            <td class="stylestbTbTD2">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
            <td class="stylestbTbTD3">
                <asp:Button ID="btnupdate" runat="server" Text="Update" Width="112px"  ValidationGroup="Main" OnClick="btnupdate_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
