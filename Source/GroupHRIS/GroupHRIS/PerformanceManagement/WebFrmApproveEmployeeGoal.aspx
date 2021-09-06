<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="WebFrmApproveEmployeeGoal.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmApproveEmployeeGoal" %>

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
            <span>Approve/Reject Employee Goals</span><hr />
            <table class="styleMainTb" style="text-align: center;" width="500px">
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
                                <td>
                                    <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                        OnClick="btnSearch_Click" />
                                </td>
                                <td style="width:125px;">
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
            <table>
                <tr>
                    <td style="vertical-align: top;">
                    
                        
                        <asp:GridView ID="grdGoal" runat="server" AutoGenerateColumns="False" 
                            Style="width: 450px;" onpageindexchanging="grdGoal_PageIndexChanging" 
                            onrowdatabound="grdGoal_RowDataBound" 
                            onselectedindexchanged="grdGoal_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="GOAL_ID" HeaderText="Goal Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField >
                                    <asp:BoundField DataField="GOAL_AREA"  HeaderText="Goal Area"></asp:BoundField>
                                <asp:BoundField DataField="WEIGHT" HeaderText="Weight"></asp:BoundField>
                                <asp:BoundField DataField="EMPLOYEE_AGREE" HeaderText="Employee (Dis)/Agree"></asp:BoundField>
                                <asp:BoundField DataField="SUPERVISOR_AGREE" HeaderText="Supervisor (Dis)/Agree">
                                </asp:BoundField>
                                
                            </Columns>
                            <EmptyDataTemplate>
                                NO TASK FOUND.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                    <td style="vertical-align: top;">
                        <div>
                            <table style="display: block; background-color: #aed6f1; width: 450px;">
                                <tr>
                                    <td align="right" style="width: 150px;vertical-align:top">
                                        Goal Area :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGoalArea" runat="server" Width="200px" TextMode="MultiLine" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px;vertical-align:top">
                                        Measurement :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMeasurement" runat="server" Width="200px" Enabled="False" HtmlEncode="false" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px;vertical-align:top">
                                        Weight :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWeight" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px;vertical-align:top;">
                                        Description :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" Width="200px" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAgree" runat="server" Text="Agree" Width="100px" 
                                            AutoPostBack="True" oncheckedchanged="chkAgree_CheckedChanged" /><asp:CheckBox
                                            ID="chkDisagree" runat="server" Text="Disagree" AutoPostBack="True" 
                                            oncheckedchanged="chkDisagree_CheckedChanged" 
                                            ValidationGroup="empReason" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 150px;vertical-align:top">
                                        Reason :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ErrorMessage="*" ControlToValidate="txtReason" ForeColor="Red" 
                                            ValidationGroup="empReason"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            <tr><td></td><td></td></tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px" 
                                            onclick="btnUpdate_Click" ValidationGroup="empReason" /><asp:Button
                                            ID="btnClear" runat="server" Text="Clear" Width="100px" 
                                            onclick="btnClear_Click" />
                                    </td>
                                </tr>
                            <tr><td></td><td></td></tr>
                            </table>
                        </div>
                        <br />
                        <table style="display: block; width: 450px;background-color: #aed6f1;">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chksConfirm" runat="server" Width="100px" Text="Confirm" 
                                        AutoPostBack="True" oncheckedchanged="chksConfirm_CheckedChanged" />
                                    <asp:CheckBox ID="chksDisagree" runat="server" Text="Reject" 
                                        AutoPostBack="True" oncheckedchanged="chksDisagree_CheckedChanged" 
                                        ValidationGroup="sComment" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="vertical-align:top;">
                                    <asp:Label ID="lblsComment" runat="server" Width="150px">Supervisor's Comment : </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsComment" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ErrorMessage="*" ControlToValidate="txtsComment" ForeColor="Red" 
                                        ValidationGroup="sComment"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnsUpdate" runat="server" Text="Update" Width="100px" 
                                        onclick="btnsUpdate_Click" ValidationGroup="sComment" /><asp:Button
                                        ID="btnsClear" runat="server" Text="Clear" Width="100px" 
                                        onclick="btnsClear_Click" />
                                </td>
                            </tr>
                            </table>
                            <br /><table style="display: block;text-align:left;width: 450px;">
                            <tr><td rowspan="2"><b><asp:Label ID="lblTotWeight" runat="server" Width = "250px"></asp:Label></b></td></tr>
                        <tr><td><asp:Button ID="btnlock" runat="server" Text="Finalize" Enabled="False" Width="100px" 
                            onclick="btnlock_Click" /></td></tr>
                        </table>
                    </td>
                </tr>
                <tr><td colspan = "2" style="text-align:center;">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label><asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" /></td></tr>
                        <asp:HiddenField ID="hfGoalId" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
