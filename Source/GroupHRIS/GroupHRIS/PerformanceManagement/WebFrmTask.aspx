<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTask.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmTask" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

        function CheckOne(obj) {
            var grid = obj.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }  
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Employee Task Details</span><hr />
            <table width="100%">
                <%--<tr class="styleTableRow">
    <td class="styleTableCell1">Goal Id : </td><td><asp:Label ID="lblgoalId" runat="server" ></asp:Label></td>
    </tr>--%>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Employee Id :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static"
                            Width="200px" AutoPostBack="True"></asp:TextBox>
                        <%--<img id="searchEmp" runat="server" alt="" src="../Images/Common/Search.jpg"  height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />--%>
                        <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/Search.jpg"
                            Height="20px" Width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtEmploeeId')" />
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" Text=" * " ErrorMessage="Employee Id is required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="task"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5" rowspan="12">
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Task Year :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Task Year is Required "
                            Text="*" ControlToValidate="ddlYear" ForeColor="Red" ValidationGroup="task"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Task Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaskname" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters"
                            ValidChars="/,-,' '" runat="server" TargetControlID="txtTaskname">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvTaskName" runat="server" ErrorMessage="Task Name Required"
                            Text="*" ForeColor="#FF3300" ValidationGroup="task" ControlToValidate="txtTaskname"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Planed Start Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlanStDate" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Custom, Numbers"
                            ValidChars="/,-" runat="server" TargetControlID="txtPlanStDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPlanStDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="rfvpsd" runat="server" ErrorMessage="Planed Start Date Required"
                            Text="*" ForeColor="#FF3300" ValidationGroup="task" ControlToValidate="txtPlanStDate"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Actual Start Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtactualStDate" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Custom, Numbers"
                            ValidChars="/,-" runat="server" TargetControlID="txtactualStDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtactualStDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Target Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTargetDate" runat="server" Width="200px"> </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteAssignDate" FilterType="Custom, Numbers" ValidChars="/,-"
                            runat="server" TargetControlID="txtTargetDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="ceTargetDate" runat="server" TargetControlID="txtTargetDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="revTargetDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            Text="*" ErrorMessage="Invalied Date(DD/MM/YYYY)" ValidationGroup="task" ControlToValidate="txtTargetDate"
                            ForeColor="Red"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                runat="server" ErrorMessage="Target Date is Required" Text="*" ValidationGroup="task"
                                ControlToValidate="txtTargetDate" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Extended Target Date :
                    </td>
                    <td>
                        <asp:TextBox ID="lblExtendedTDate" runat="server" Width="200px" ReadOnly="True" Enabled="False"
                            EnableTheming="False"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Total Completion :
                    </td>
                    <td>
                        <asp:TextBox ID="lblCompletion" runat="server" Width="200px" ReadOnly="True" Enabled="False"></asp:TextBox>
                        (%)
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Description :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"
                            MaxLength="150"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="200px" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="204px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is Required"
                            Text="*" ForeColor="Red" ValidationGroup="task" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                            ValidationGroup="task" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                    <div class="MenuBar">
                        <br />
                        <asp:Literal ID="litDefaultQuestionList" runat="server"></asp:Literal>
                    </div>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="task"
                            ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="width: 100px; position: absolute; left: 775px; top: 158px;">
                <span>Employee Goals</span>
            </div>
            <asp:GridView ID="grdGoalArea" runat="server" Style="width: 310px; position: absolute;
                left: 775px; top: 190px;" AutoGenerateColumns="false" AllowPaging="true" PageSize="3"
                OnPageIndexChanging="grdGoalArea_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="GOAL_ID" HeaderText="Goal ID" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="GOAL_AREA" HeaderText="Goal Name" />
                    <asp:TemplateField HeaderText="Include">
                        <ItemTemplate>
                            <%--  <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" onclick="CheckOne(this)"/>--%>
                            <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" AutoPostBack="true"
                                OnCheckedChanged="chkBxSelect_CheckedChanged" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    NO GOALS FOUND.
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <span>Employee Task : </span>&nbsp;&nbsp;&nbsp; <span style="text-align: right;">
                <asp:LinkButton ID="linkviewAlltsk" runat="server" OnClick="linkviewAlltsk_Click">View All Task For Selected Employee</asp:LinkButton></span><hr />
            <br />
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:GridView ID="grdTask" runat="server" Style="margin: auto; width: 800px;" AutoGenerateColumns="false"
                            AllowPaging="true" OnPageIndexChanging="grdTask_PageIndexChanging" OnRowDataBound="grdTask_RowDataBound"
                            OnSelectedIndexChanged="grdTask_SelectedIndexChanged" PageSize="5">
                            <Columns>
                                <asp:BoundField DataField="GOAL_AREA" HeaderText="Goal Name" />
                                <asp:BoundField DataField="TASK_NAME" HeaderText="Task Name" />
                                <asp:BoundField DataField="TASK_YEAR" HeaderText="Task Year" />
                                <asp:BoundField DataField="PLAN_START_DATE" HeaderText="Planed Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="ACTUAL_START_DATE" HeaderText="Actual Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="TARGET_DATE" HeaderText="Target Date" />
                                <asp:BoundField DataField="EXTENDED_TARGET_DATE" HeaderText="Extended Date" />
                                <asp:BoundField DataField="TOTAL_COMPLETION" HeaderText="Total Completion (%)">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                <asp:BoundField DataField="TASK_ID" HeaderText="TskId" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_ID" HeaderText="GlId" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="IS_AGREE" HeaderText="Employee (Dis)/Agree" />
                                <asp:BoundField DataField="SUPERVISOR_AGREE" HeaderText="Supervisor (Dis)/Agree" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
