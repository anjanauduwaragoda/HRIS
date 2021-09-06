<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainingEvaluationDashBoard.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingEvaluationDashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Training Dashboard </span>
    <hr />
    <table class="styleMainTb" style="text-align: center;">
        <tr>
            <td class="divsearch">
                <table>
                    <tr>
                        <td style="text-align: right;">
                            Assignee :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAssignee" runat="server" Width="150px">
                                <asp:ListItem Value="0">&lt;&lt;me&gt;&gt;</asp:ListItem>
                                <asp:ListItem Value="1">not &lt;&lt;me&gt;&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            Training Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtTraining" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="text-align: right;">
                            Pre/Post Evaluation :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPrePostEvaluation" runat="server" Width="150px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="0">Pre - Evaluation</asp:ListItem>
                                <asp:ListItem Value="1">Post - Evaluation</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            From Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrmDate" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="text-align: right;">
                            To Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <span>Training Evaluation Details</span>
    <hr />
    <table>
        <tr>
            <td>
            </td>
            <td align="right">
                <asp:Label ID="lblMultipleChoiceQuestion" runat="server" Text="Multiple Choice Question : "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMCQ" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Width="80px"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblEssayQuestion" runat="server" Text="Essay Question : "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblEssay" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="right">
                <asp:Label ID="lblRatingQuestion" runat="server" Text="Rating Question : "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblRating" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <table style="margin: auto">
        <tr>
            <td>
                <asp:GridView ID="grdEvaluation" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdEvaluation_PageIndexChanging"
                    OnRowDataBound="grdEvaluation_RowDataBound" OnSelectedIndexChanged="grdEvaluation_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="TRAINING_ID" HeaderText="Training Id" />
                        <asp:BoundField DataField="TRAINING_NAME" HeaderText="Training" />
                        <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation Id" />
                        <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation" />
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee" />
                        <asp:BoundField DataField="EVALUATOR" HeaderText="Evaluator" />
                        <asp:BoundField DataField="IS_POST_EVALUATION" HeaderText="Post/Pre Evaluation" />
                        <asp:BoundField DataField="EVALUATION_START_DATE" HeaderText="Start Date" />
                        <asp:BoundField DataField="EVALUATION_END_DATE" HeaderText="End Date" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
