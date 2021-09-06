<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Mastermain.Master"
    EnableEventValidation="false" CodeBehind="WebFormEmployeeSelfTrainingEvaluationDashboard.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFormEmployeeSelfTrainingEvaluation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hiddenColumn
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
         function open(file, window, ctlName) {
             txb = ctlName;
             childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

             document.getElementById("hfCaller").value = ctlName;
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <span>Training Evaluation - Employee Dashboard</span>
    <hr />
    <br />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="center">
                <asp:GridView ID="gvEvaluations" runat="server" Style="width: 800px" 
                    AutoGenerateColumns="false" onrowdatabound="gvEvaluations_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="TRAINING_ID" HeaderText="Training Id" HeaderStyle-CssClass="hiddenColumn"
                            ItemStyle-CssClass="hiddenColumn" />
                        <asp:BoundField DataField="TRAINING_NAME" HeaderText="Training Name" />
                        <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                        <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation Id" HeaderStyle-CssClass="hiddenColumn"
                            ItemStyle-CssClass="hiddenColumn" />
                        <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation Name" />
                        <asp:BoundField DataField="MCQ_INCLUDED" HeaderText="MCQ_INCLUDED" HeaderStyle-CssClass="hiddenColumn"
                            ItemStyle-CssClass="hiddenColumn" />
                        <asp:BoundField DataField="MCQ" HeaderText="MCQ" HtmlEncode="false" />
                        <asp:BoundField DataField="ESSAY" HeaderText="Essay" HtmlEncode="false" />
                        <asp:BoundField DataField="RATING" HeaderText="Rating" HtmlEncode="false" />
                        <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                        <asp:BoundField DataField="POST_EVALUATION" HeaderText="Is Post Evaluation" />
                        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" />
                        <asp:BoundField DataField="END_DATE" HeaderText="End Date" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <asp:HiddenField ID="hfCaller" runat="server" />
</asp:Content>
