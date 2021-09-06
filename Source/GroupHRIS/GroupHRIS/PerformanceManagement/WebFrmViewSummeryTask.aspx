<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewSummeryTask.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.PerformanceManagement.WebFrmViewSummeryTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            //window.moveTo(20, 20);
            window.resizeTo(_width, _height)
            window.focus();
        }



    </script>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        Employee Task List
        <hr />
        <table><tr><td>
        <table width="620px">
            <tr>
                <td>
                    Employee :
                    <asp:Label ID="lblEmployee" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Year :
                    <asp:Label ID="lblYear" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdTaskList" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdTaskList_PageIndexChanging"
                        OnRowDataBound="grdTaskList_RowDataBound" OnSelectedIndexChanged="grdTaskList_SelectedIndexChanged"
                        AllowPaging="true" PageSize="7">
                        <Columns>
                            <asp:BoundField DataField="GOAL_ID" HeaderText="Goal Id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="TASK_ID" HeaderText="Task id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="TASK_NAME" HeaderText="Task" />
                            <asp:BoundField DataField="PLAN_START_DATE" HeaderText="Planed Start Date" />
                            <asp:BoundField DataField="ACTUAL_START_DATE" HeaderText="Actual Start Date" />
                            <asp:BoundField DataField="TARGET_DATE" HeaderText="Target Date" />
                            <asp:BoundField DataField="EXTENDED_TARGET_DATE" HeaderText="Extended Target Date" />
                            <asp:BoundField DataField="TOTAL_COMPLETION" HeaderText="Total Completion" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </td></tr>
        <tr><td>
        <br />
        <hr />
        <table >
            <tr>
                <td >
                    Task Extentions
                </td>
                <td>
                </td>
                <td>
                    Task Progress
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdtskExtention" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="TASK_ID" HeaderText="Task id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="TASK_EXTENTION_ID" HeaderText="Extention Id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn"/>
                            <asp:BoundField DataField="EXTENDED_DATE" HeaderText="Extention Date" />
                            <asp:BoundField DataField="REASON" HeaderText="Reason" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                        <EmptyDataTemplate>
                            Task Extention not found
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
                <td>
                </td>
                <td>
                    <asp:GridView ID="grdTaskProgress" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="TASK_ID" HeaderText="Task id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="OBSERVED_DATE" HeaderText="Observe Date" />
                            <asp:BoundField DataField="PROGRESS" HeaderText="Progress" />
                            <asp:BoundField DataField="REMARKS" HeaderText="Reason" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                        <EmptyDataTemplate>
                            Task Progress not found
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </td></tr></table>
    </div>
    </form>
</body>
</html>
