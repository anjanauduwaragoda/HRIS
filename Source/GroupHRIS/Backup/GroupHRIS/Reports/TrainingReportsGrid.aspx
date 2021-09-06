<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="TrainingReportsGrid.aspx.cs" Inherits="GroupHRIS.Reports.TrainingReportsGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    Training Reports
    <hr />
    <br />
    <center>
        <asp:Label runat="server" ID="lblMessage"></asp:Label>
    </center>
    <center>
        <asp:GridView ID="gvTrainingReports" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            OnRowDataBound="gvTrainingReports_RowDataBound" OnSelectedIndexChanged="gvTrainingReports_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="idno" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="repcode" HeaderText="Report Code" HeaderStyle-CssClass="" />
                <asp:BoundField DataField="description" HeaderText="Report Name" HeaderStyle-CssClass="" />
            </Columns>
        </asp:GridView>
    </center>
</asp:Content>
