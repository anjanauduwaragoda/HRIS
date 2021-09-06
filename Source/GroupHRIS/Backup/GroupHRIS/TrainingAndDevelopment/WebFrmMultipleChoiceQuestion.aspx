<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmMultipleChoiceQuestion.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmMultipleChoiceQuestion" %>

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

        function getValueFromChild(sEvalId, sEvalName) {
            var ctl = document.getElementById("hfCaller").value;

            document.getElementById("hfVal").value = sEvalId;
            document.getElementById("hfEvalName").value = sEvalName;
            // alert(hfEvalName);
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
            <span>Multiple Choice Question</span>
            <hr />
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="styleTableCell1">
                                    Evaluation :
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtEvalId" runat="server" Width="250px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                                        Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmEvaluationSearch.aspx','search','txtEvalId')"
                                        ImageUrl="~/Images/Common/search-icon-01.png" />
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
                                        ForeColor="Red" ErrorMessage="Program Evaluation is required." ValidationGroup="MCGroup"
                                        ControlToValidate="txtEvalId"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblEvalName" runat="server" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1">
                                    Question :
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtQuestion" runat="server" Width="250px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                        ErrorMessage="Question is required." ValidationGroup="MCGroup" Text="*" ControlToValidate="txtQuestion"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1">
                                    Choices :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlChoice" runat="server" Width="255px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlChoice_SelectedIndexChanged">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="MCGroup"
                                        ControlToValidate="ddlChoice" ErrorMessage="Choice is required" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1">
                                    Status :
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="255px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                        ErrorMessage="Question Status is required." Text="*" ValidationGroup="MCGroup"
                                        ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 100px;">
                                </td>
                                <td>
                                    <asp:Label ID="lblChoice" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Answer :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAnswer" runat="server" Width="250px" TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="answer"
                                        ErrorMessage="Answer is required." Text="*" ControlToValidate="txtAnswer" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                                <td rowspan="10" style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Status :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAnsStatus" runat="server" Width="255px" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="answer"
                                        ErrorMessage="Answer status is required." Text="*" ControlToValidate="ddlAnsStatus"
                                        ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsAnswer" runat="server" Text="Is Answer" Enabled="False" />
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
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" ValidationGroup="answer" OnClick="btnAdd_Click"
                                        Enabled="False" Width="65px" />
                                    <asp:Button ID="btnAnsClear" runat="server" Text="Clear" OnClick="btnAnsClear_Click"
                                        Enabled="False" Width="65px" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2">
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" align="center">
                        <asp:GridView ID="grdAnswer" runat="server" AutoGenerateColumns="false" Style="width: 320px;"
                            OnRowDataBound="grdAnswer_RowDataBound" OnSelectedIndexChanged="grdAnswer_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ANSWER_ID" HeaderText="Answer Id" HeaderStyle-Width="10px" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn"/>
                                <asp:BoundField DataField="ANSWER" HeaderText="Answer" />
                                <asp:BoundField DataField="IS_ANSWER" HeaderText="Is Answer" HeaderStyle-Width="10px" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" HeaderStyle-Width="50px" />
                            </Columns>
                            <%-- <EmptyDataTemplate>
                                NO ANSWERS FOUND FOR SELECTED QUESTION
                            </EmptyDataTemplate>--%>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="MCGroup"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:ValidationSummary ID="rqGroup" runat="server" ForeColor="Red" ValidationGroup="MCGroup"
                            Style="text-align: left" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="answer"
                            Style="text-align: left" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:GridView ID="grdQuestion" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdQuestion_PageIndexChanging"
                            Style="width: 600px" AllowPaging="true" PageSize="5" OnRowDataBound="grdQuestion_RowDataBound"
                            OnSelectedIndexChanged="grdQuestion_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="MCQ_ID" HeaderText="Question Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="QUESTION" HeaderText="Question" />
                                <asp:BoundField DataField="CHOICES" HeaderText="Choices" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfEvalName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
