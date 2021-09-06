<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmLeaveApproval.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.webFrmLeaveApproval"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .hideGridColumn
    {
        display:none;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700">HRIS Employee Leave Approval</span>  
    <br />
    <br />
    <asp:GridView ID="gvLeaves" runat="server" AutoGenerateColumns="False" OnRowCommand="gvLeaves_RowCommand">
        <Columns>
            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employe Id" />
            <asp:BoundField DataField="empName" HeaderText="Employee Name" />
            <asp:BoundField DataField="LEAVE_DATE" HeaderText="Leave Date" />
            <asp:BoundField DataField="LEAVE_TYPE_ID" HeaderText="Leave Type" />
            <asp:BoundField DataField="SCHEME_LINE_NO" HeaderText="SL" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
            <asp:BoundField DataField="FROM_TIME" HeaderText="From Time" />
            <asp:BoundField DataField="TO_TIME" HeaderText="To Time" />
            <asp:BoundField DataField="covName" HeaderText="Covered By" />
            <asp:BoundField DataField="NO_OF_DAYS" HeaderText="No of Days" />
            <asp:ButtonField HeaderText="Approve Leave" Text="Approve" CommandName="Approve" />
            <asp:ButtonField HeaderText="Reject Leave" Text="Reject" CommandName="Reject" />
        </Columns>
    </asp:GridView>
</asp:Content>
