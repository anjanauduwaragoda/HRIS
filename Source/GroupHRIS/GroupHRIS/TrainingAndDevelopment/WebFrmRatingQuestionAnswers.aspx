<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmRatingQuestionAnswers.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmRatingQuestionAnswers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Rating Question Answers
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate><%----%>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvTrainingEvaluations" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="5" runat="server" OnPageIndexChanging="grdvTrainingEvaluations_PageIndexChanging"
                            OnRowDataBound="grdvTrainingEvaluations_RowDataBound" OnSelectedIndexChanged="grdvTrainingEvaluations_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="TRAINING_ID" HeaderText="Training ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation" />
                                <asp:BoundField DataField="TRAINING_NAME" HeaderText="Training" />
                                <asp:BoundField DataField="TRAINING_CODE" HeaderText="Training Code" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <ol id="olQpaper" runat="server" enableviewstate="true">
            </ol>
            <br />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="100px" Text="Save" 
                            onclick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" 
                            onclick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress><%----%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel><%----%>
</asp:Content>