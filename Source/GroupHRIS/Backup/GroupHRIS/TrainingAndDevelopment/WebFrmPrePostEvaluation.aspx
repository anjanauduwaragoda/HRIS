<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmPrePostEvaluation.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmPrePostEvaluation" %>

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
    <script type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sTrId, sTrName, sProName) {
            var ctl = document.getElementById("hfCaller").value;

            //alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            document.getElementById("hfTrName").value = sTrName;
            document.getElementById("hfProName").value = sProName;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

        $('table[id$=grdParticipantDetails]').find('input:checkbox').change(function (e) {
            var tr = $(this).closest('tr');
            if (tr.find('input:checkbox:checked').length == 2) {
                alert('You can\'t select 1 checkbox per row ');
                this.checked = false;
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <span>Training Pre/Post Evaluation Details</span><hr />
            <table style="background-color: #aed6f1; width: 100%">
                <table width="100%">
                    <tr>
                        <td align="right">
                            Training Program :
                        </td>
                        <td>
                            <asp:TextBox ID="txtTraining" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                                Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTraining')"
                                ImageUrl="~/Images/Common/search-icon-01.png" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    runat="server" ErrorMessage="Training is required" Text="*" ForeColor="Red" ControlToValidate="txtTraining"
                                    ValidationGroup="evaluation"></asp:RequiredFieldValidator>
                            <%--  </td>
                        <td rowspan="2" style="vertical-align: top;">--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Training Name :
                        </td>
                        <td>
                            <asp:Label ID="lblTraining" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Evaluation Name :
                        </td>
                        <td>
                            <asp:Label ID="lblEvaluationName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            MCQ Include :
                        </td>
                        <td>
                            <asp:Label ID="lblMCQInclude" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Essay Question Include :
                        </td>
                        <td>
                            <asp:Label ID="lblEssayQuestion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Rating Question Include :
                        </td>
                        <td>
                            <asp:Label ID="lblRating" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Rating Scheme :
                        </td>
                        <td>
                            <asp:Label ID="lblRatingScheme" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: right;">
                            Pre Evaluation :
                        </td>
                        <%-- <td>
                            <asp:RadioButtonList ID="rblPrePost" runat="server">
                                <asp:ListItem Value="0">Pre-Evaluation</asp:ListItem>
                                <asp:ListItem Value="1">Post-Evaluation</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>--%>
                        <td>
                            <asp:CheckBox ID="chkPreEvaluation" runat="server" Text="Pre-Evaluation" AutoPostBack="True"
                                OnCheckedChanged="chkPreEvaluation_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Post Evaluation
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPostEvaluation" runat="server" Text="Post-Evaluation" AutoPostBack="True"
                                OnCheckedChanged="chkPostEvaluation_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div>
                    <%--<table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblMCQ" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEssyQuestion" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblRatingQuestion" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr><td></td><td></td></tr>


                        <tr>
                            <td align="center" colspan="3">
                                <asp:GridView ID="grdProEvaluation" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdProEvaluation_PageIndexChanging"
                                    OnRowDataBound="grdProEvaluation_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation Id" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn"/>
                                        <asp:BoundField DataField="RS_NAME" HeaderText="Rating Scheme" />
                                        <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation Name" />
                                        <asp:BoundField DataField="MCQ_INCLUDED" HeaderText="MCQ Include" />
                                       
                                        <asp:BoundField DataField="EQ_INCLUDED" HeaderText="Essay Question Include" />
                                        <asp:BoundField DataField="RQ_INCLUDED" HeaderText="Rating Question Include" />
                                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Pre-Evaluation">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPreEval" runat="server" AutoPostBack="true" OnCheckedChanged="chkPreEval_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Post-Evaluation">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPostEval" runat="server" AutoPostBack="true" OnCheckedChanged="chkPostEval_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        NO PROGRAM EVALUATIONS FOUND
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>--%>
                    <span>Training Participant Details</span>
                    <hr />
                    <table align="center">
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="grdParticipantDetails" runat="server" AutoGenerateColumns="false"
                                    AllowPaging="true" OnPageIndexChanging="grdParticipantDetails_PageIndexChanging"
                                    OnRowDataBound="grdParticipantDetails_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="COMPANY_ID" HeaderText="Company Id" HeaderStyle-CssClass="hideGridColumn"
                                            ItemStyle-CssClass="hideGridColumn" />
                                        <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee Id" HeaderStyle-CssClass="hideGridColumn"
                                            ItemStyle-CssClass="hideGridColumn" />
                                        <asp:BoundField DataField="KNOWN_NAME" HeaderText="Known Name" />
                                        <asp:BoundField DataField="EPF" HeaderText="EPF" />
                                        <asp:BoundField DataField="DESIGNATION_NAME" HeaderText="Designation" />
                                        <asp:BoundField DataField="DEPT_NAME" HeaderText="Department" />
                                        <asp:BoundField DataField="DIV_NAME" HeaderText="Division" />
                                        <asp:BoundField DataField="BRANCH" HeaderText="Branch" ItemStyle-CssClass="hideGridColumn"
                                            HeaderStyle-CssClass="hideGridColumn" />
                                        <asp:BoundField DataField="REPORTING_HEAD" HeaderText="Reporting Head" />
                                        <asp:TemplateField HeaderText="Supervisor Evaluation">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeaderSupervisor" runat="server" Text="Supervisor Evaluation"
                                                    OnCheckedChanged="chkSupervisorEval_CheckedChanged" AutoPostBack="True" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSupervisor" runat="server" AutoPostBack="true" OnCheckedChanged="chkSupervisor_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Evaluation">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeaderEmp" runat="server" Text="Employee Evaluation" AutoPostBack="True"
                                                    OnCheckedChanged="chkHeaderEmp_CheckedChanged1" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEmployee" runat="server" AutoPostBack="True" OnCheckedChanged="chkEmployee_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        NO PARTICIPANT FOUND
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <%--  <td align="right">
                                Select Evaluater :
                            </td>--%>
                            <td align="left" style="width: 200px;">
                                <%--<asp:RadioButton ID="rbSupervisor" runat="server" Text="Supervisor" 
                                    AutoPostBack="True" oncheckedchanged="rbSupervisor_CheckedChanged" />
                                <asp:RadioButton ID="rbEmployee" runat="server" Text="Employee" 
                                    AutoPostBack="True" oncheckedchanged="rbEmployee_CheckedChanged" />--%>
                                <%--<asp:RadioButtonList ID="rblEvaluater" RepeatColumns="2" runat="server" RepeatDirection="Horizontal"
                                    ValidationGroup="evaluation" Width="200px">
                                    <asp:ListItem>Supervisor</asp:ListItem>
                                    <asp:ListItem>Employee</asp:ListItem>
                                </asp:RadioButtonList>--%>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Evaluation Statrt Date :
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDate"
                                    Format="yyyy/MM/dd">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Start date is required"
                                    Text="*" ForeColor="Red" ValidationGroup="evaluation" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Evaluation End Date :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" runat="server" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                    Format="yyyy/MM/dd">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="End date is required"
                                    Text="*" ForeColor="Red" ValidationGroup="evaluation" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Comments :
                            </td>
                            <td>
                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Status :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="204px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="evaluation"
                                    ErrorMessage="Status is required" Text="*" ControlToValidate="ddlStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="evaluation"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hfProgramId" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfTrName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfProName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfrecordVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfPreEvaluation" runat="server" ClientIDMode="Static" Value=""
                    ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfPostEvaluation" runat="server" ClientIDMode="Static" Value=""
                    ViewStateMode="Enabled" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
