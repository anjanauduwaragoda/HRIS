<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmLeaveSheetHRApproval.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.webFrmLeaveSheetHRApproval"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span>Approve Leave Sheets</span>
    <hr />
    <span>Company :</span><asp:Label ID="lblCompany" runat="server" Text=""></asp:Label>
    <br /><br />
    <asp:GridView ID="gvLeaves" runat="server" AutoGenerateColumns="False" 
        onrowcommand="gvLeaves_RowCommand">
        <Columns>
            <asp:BoundField DataField="LEAVE_SHEET_ID" HeaderText="LEAVE_SHEET_ID" />
            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
            <asp:BoundField DataField="empName" HeaderText="APPLICANT" />
            <asp:BoundField DataField="FROM_DATE" HeaderText="FROM_DATE" />
            <asp:BoundField DataField="TO_DATE" HeaderText="TO_DATE" />
            <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_DAYS" />
            <asp:BoundField DataField="coverdName" HeaderText="COVERED_BY" />
            <asp:BoundField DataField="recommandName" HeaderText="RECOMMEND_BY" />
            <asp:BoundField DataField="LEAVE_STATUS" HeaderText="LEAVE_STATUS" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="Action_Need" HeaderText="NEED_TO_DO"  HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
            <asp:ButtonField HeaderText="APPROVE" Text="Accept" CommandName="Accept" />
            <asp:ButtonField HeaderText="REJECT" Text="Reject" CommandName="Reject"/>
            <asp:ButtonField HeaderText="VIEW_DETAILS" Text="View" CommandName="View"/>             
        </Columns>
    </asp:GridView>
    <br />
    <asp:Label ID="lblLSDetail" runat="server" Text="Leave Sheet Details"></asp:Label>
    <hr />
    <asp:GridView ID="gvLSDetails" runat="server">
    </asp:GridView>
    <br />
            <asp:LinkButton ID="lbtnClear" runat="server" onclick="lbtnClear_Click">Clear Leave Sheet Details</asp:LinkButton>

</asp:Content>
