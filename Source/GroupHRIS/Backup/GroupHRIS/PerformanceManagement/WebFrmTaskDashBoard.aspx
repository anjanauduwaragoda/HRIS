<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTaskDashBoard.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmTaskDashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sEmpId, sName) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById("hfVal").value = sEmpId
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Employee Task - Dashboard</span><hr />
            <table class="styleMainTb" style="text-align: center;">
                <tr>
                    <td class="divsearch">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEmp" runat="server" Style="vertical-align: super;">Employee Id : </asp:Label><asp:TextBox
                                        Style="vertical-align: super;" ID="txtEmpId" runat="server" Width="180px"></asp:TextBox>
                                    <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                                        Height="25px" Width="25px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtEmpId')"
                                        ImageUrl="~/Images/Common/search-icon-01.png" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td align="left">
                                    Year :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" Width="180px" AutoPostBack="True" 
                                        onselectedindexchanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <%-- <td>
                            Status :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="180px">
                            </asp:DropDownList>
                        </td>--%>
                                <td>
                                    <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                        OnClick="btnSearch_Click" />
                                </td>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="400px">
            <tr><td style="background-color: #aed6f1;vertical-align:middle;height:40px;">
                <asp:Label ID="Label1" runat="server" Text="Subordinate : " Width="80px" style="text-align:right;vertical-align:top;"></asp:Label>
                <asp:DropDownList ID="ddlempStatus" runat="server" Width="120px" 
                    onselectedindexchanged="ddlempStatus_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>  
                <asp:Label ID="Label2" runat="server" Text="Supervisor :" Width="80px" style="text-align:right;vertical-align:top;"></asp:Label>
                <asp:DropDownList ID="ddlsupStatus" runat="server" Width="120px" 
                    onselectedindexchanged="ddlsupStatus_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
                <asp:ImageButton ID="imgRefresh" runat="server" Height="20px" Width="25px" 
                    ImageUrl="~/Images/Common/refresh.png" style = "vertical-align:bottom;" 
                    onclick="imgRefresh_Click" ToolTip="Reload"/>
               <%-- <asp:Image ID="imgRefresh1" runat="server" alt="" Height="20px" Width="25px"
                                        ImageUrl="~/Images/Common/refresh.png" style = "vertical-align:bottom;"/>--%>
            </td><td style="vertical-align: top;" rowspan="3">
                        <div>
                            <table style="display: block; background-color: #aed6f1; width: 480px;">
                                <tr>
                                    <td>
                                        <b>Task Details</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px;">
                                        Goal Area :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGoal" runat="server" TextMode="MultiLine" ReadOnly="True" Width="200px"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Task Name :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTask" runat="server" ReadOnly="True" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Description :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" ReadOnly="True" Width="200px" TextMode="MultiLine"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Start Date :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" runat="server" Height="22px" ReadOnly="True" Width="200px"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Target Date :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTargetDate" runat="server" ReadOnly="True" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--</table>--%>
                                <%--</div>
                <div>--%>
                                <%-- <table style="width: 450px;display: block;">--%>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hfTaskId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkAgree" runat="server" Text="Agree" AutoPostBack="True" OnCheckedChanged="chkAgree_CheckedChanged"
                                            Width="100px" />
                                        <asp:CheckBox ID="chkDisagree" runat="server" Text="Disagree" AutoPostBack="True"
                                            OnCheckedChanged="chkDisagree_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Reason :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnSave" runat="server" Text="Update" Width="100px" OnClick="btnSave_Click"/>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <table style="display: block; width: 480px;background-color: #aed6f1;">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkApprove" runat="server" Text="Confirm" OnCheckedChanged="chkApprove_CheckedChanged"
                                        AutoPostBack="True" Width="100px" />
                                    <asp:CheckBox ID="chkReject" runat="server" Text="Reject" OnCheckedChanged="chkReject_CheckedChanged"
                                        AutoPostBack="True" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblsComment" runat="server" Width="150px">Supervisor's Comment : </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupervisorComment" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                   
                                </td>
                            </tr>
                            <tr><td></td><td></td></tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnsUpdate" runat="server" Text="Update" Width="100px" 
                                        OnClick="btnsUpdate_Click" />
                                    <asp:Button ID="btnsClear" runat="server" Text="Clear" Width="100px" OnClick="btnsClear_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr><td></td><td></td></tr>
                        </table>
                    </td></tr>
                <tr>
                    <td style="vertical-align: top;">
                    
                        <asp:GridView ID="grdTask" runat="server" AutoGenerateColumns="False" Style="width: 450px;"
                            OnRowDataBound="grdTask_RowDataBound" OnPageIndexChanging="grdTask_PageIndexChanging"
                            OnSelectedIndexChanged="grdTask_SelectedIndexChanged" PageSize="7">
                            <Columns>
                                <asp:BoundField DataField="GOAL_ID" HeaderText="Goal Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TASK_ID" HeaderText="Task Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                <asp:BoundField DataField="TASK_NAME" HeaderText="Task" />
                                <asp:BoundField DataField="PLAN_START_DATE" HeaderText="Start Date" />
                                <asp:BoundField DataField="TARGET_DATE" HeaderText="End Date" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="IS_AGREE" HeaderText="Employee (Dis)/Agree" />
                                <asp:BoundField DataField="REASON" HeaderText="Reason" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="SUPERVISOR_AGREE" HeaderText="Supervisor (Dis)/Agree" />
                                <asp:BoundField DataField="SUPERVISOR_REASON" HeaderText="Supervisor Reason" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                            <EmptyDataTemplate>
                                NO TASK FOUND.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
