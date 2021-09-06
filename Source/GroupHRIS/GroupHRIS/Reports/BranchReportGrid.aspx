<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="BranchReportGrid.aspx.cs" Inherits="GroupHRIS.Reports.BranchReportGrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <span style="font-weight: 700">Report Center</span>
            </td>
        </tr>
    </table>
    <hr />
    <table style="margin:auto;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                    BorderColor="#0099FF" BorderStyle="Groove" BorderWidth="2px" CellPadding="3"
                    CellSpacing="1" ForeColor="Black" OnRowDataBound="GridView1_RowDataBound" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <EmptyDataTemplate>
                        BRANCH REPORTS NOT AVAILABLE FOR YOUR USER ROLE. PLEASE CONTACT GROUP HR FOR 
                        MORE INFORMATION.
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="#0099FF" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#CCCCCC" BorderColor="#0099FF" BorderWidth="1px" ForeColor="Black"
                        HorizontalAlign="Left" />
                    <RowStyle BackColor="White" BorderColor="#3366FF" BorderWidth="1px" />
                    <SelectedRowStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="REPCODE" HeaderText="Report Code" />
                        <asp:BoundField DataField="DESCRIP" HeaderText="Report Name" />
                    </Columns>
                </asp:GridView>
                </td>
        </tr>
        <tr>
            <td style="text-align:center;">
            <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
