<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="WebFrmPayrollDocuments.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmPayrollDocuments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<b>Payroll Documents</b>
<hr />
<br />
<br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
       

<table style="margin:auto;">
    <tr>
        <td style="text-align:right;">Company</td>
        <td>:</td>
        <td>
            <asp:DropDownList ID="ddlCompany" Width="150px" runat="server" 
                AutoPostBack="True" onselectedindexchanged="ddlCompany_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">Month</td>
        <td>:</td>
        <td>
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlYear_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlMonth_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">Download</td>
        <td>:</td>
        <td>
            <asp:HyperLink ID="hyplnkDownload" runat="server"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <img src="../Images/ProBar/720.GIF" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </td>
    </tr>
</table>
            <br />            
            <br />
            <asp:GridView ID="grdvPayrollData" Style="margin: auto;" runat="server" AutoGenerateColumns="False" >
                <Columns>
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category " />
                    <asp:BoundField DataField="EPF_NO" HeaderText=" EPF Number " />
                    <asp:BoundField DataField="TYPE_ID" HeaderText=" Type ID " />
                    <asp:BoundField DataField="FINALIZED_AMOUNT" HeaderText=" Finalized Amount " />
                </Columns>
            </asp:GridView>


             
        </ContentTemplate>
    </asp:UpdatePanel>

    <br />

</asp:Content>
