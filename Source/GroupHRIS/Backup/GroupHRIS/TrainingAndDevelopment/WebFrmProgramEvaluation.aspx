<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmProgramEvaluation.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmProgramEvaluation" %>

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

        function getValueFromChild(sTrId, sTrName) {
            var ctl = document.getElementById("hfCaller").value;

            // alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            document.getElementById("hfTrName").value = sTrName;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <span>Program Evaluation Details</span>
            <hr />
            <table width="100%">
                <tr>
                    <td align="right" style="width: 200px;">
                        Training Program :
                    </td>
                    <td style="width: 250px;">
                        <asp:TextBox ID="txtTraining" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                        <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                            Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingProgramSearch.aspx','search','txtTraining')"
                            ImageUrl="~/Images/Common/search-icon-01.png" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Training Program is required"
                            Text="*" ControlToValidate="txtTraining" ForeColor="Red" ValidationGroup="evaluation"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Training Name :
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblTraining" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Evaluation Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEvalName" runat="server" Width="200px"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ErrorMessage="Evaluation name is Required"
                            Text="*" ControlToValidate="txtEvalName" ForeColor="Red" ValidationGroup="evaluation"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Is MCQ Include :
                    </td>
                    <td>
                        <asp:CheckBox ID="chkMcq" runat="server" OnCheckedChanged="chkMcq_CheckedChanged"
                            AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:Label ID="lblMCQ" runat="server" Font-Bold="True" ForeColor="#0000CC"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Is Essay Question Include :
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEssay" runat="server" OnCheckedChanged="chkEssay_CheckedChanged"
                            AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:Label ID="lblEssyQuestion" runat="server" Font-Bold="True" ForeColor="#0000CC"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Is Rating Question Include :
                    </td>
                    <td>
                        <asp:CheckBox ID="chkRating" runat="server" AutoPostBack="True" OnCheckedChanged="chkRating_CheckedChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lblRatingQuestion" runat="server" Font-Bold="True" ForeColor="#0000CC"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Rating Scheme :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlScheme" runat="server" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:GridView ID="grdRatingDetails" runat="server" AutoGenerateColumns="false" Style="width: 80%;">
                            <Columns>
                                <asp:BoundField DataField="RATING" HeaderText="Rating" />
                                <asp:BoundField DataField="WEIGHT" HeaderText="Weight" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="200px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Status is required"
                            ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="evaluation">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                            ValidationGroup="evaluation" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="evaluation" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <span>Program Evaluations</span>
            <br />
            <hr />
            <br />
            <div>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:GridView ID="grdProEvaluation" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdProEvaluation_PageIndexChanging"
                                OnRowDataBound="grdProEvaluation_RowDataBound" OnSelectedIndexChanged="grdProEvaluation_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation Id" ItemStyle-CssClass="hideGridColumn"
                                        HeaderStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="RS_NAME" HeaderText="Rating Scheme" />
                                    <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation Name" />
                                    <asp:BoundField DataField="MCQ_INCLUDED" HeaderText="MCQ Include" />
                                    <asp:BoundField DataField="EQ_INCLUDED" HeaderText="Essay Question Include" />
                                    <asp:BoundField DataField="RQ_INCLUDED" HeaderText="Rating Question Include" />
                                    <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                </Columns>
                                <EmptyDataTemplate>
                                    NO PROGRAM EVALUATIONS FOUND
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfTrName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
