﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmRatingQuestionPopup.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmRatingQuestionPopup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
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
    <title></title>
</head>
<body class="popupsearch">
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <br />
                <span>Rating Question</span>
                <hr />
                <table>
                    <tr class="styleTableRow">
                        <td class="styleTableCell1">
                            Program evaluation :
                        </td>
                        <td class="styleTableCell2">
                            <asp:TextBox ID="txtEvalId" runat="server" Width="250px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="styleTableCell3">
                            <%--<asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                            Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmEvaluationSearch.aspx','search','txtEvalId')"
                            ImageUrl="~/Images/Common/search-icon-01.png" />--%>
                        </td>
                        <td class="styleTableCell4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
                                ForeColor="Red" ErrorMessage="Program Evaluation is required." ValidationGroup="RQGroup"
                                ControlToValidate="txtEvalId"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblRatingScheme" runat="server"></asp:Label>
                        </td>
                    </tr>
                   <tr><td class="styleTableCell1">
                                    Evaluation Name :
                                </td>
                                <td>
                                    <asp:Label ID="lblEvalName" runat="server"></asp:Label>
                                </td><td></td><td></td><td rowspan="5">
                        <asp:GridView ID="grdRatingDetails" runat="server" AutoGenerateColumns="false" Style="width: 80%;">
                            <Columns>
                                <asp:BoundField DataField="RATING" HeaderText="Rating" />
                                <asp:BoundField DataField="WEIGHT" HeaderText="Weight" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                            </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr class="styleTableRow">
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
                                ErrorMessage="Question is required." ValidationGroup="RQGroup" Text="*" ControlToValidate="txtQuestion"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--  <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Number of answers :
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlNoOfAns" runat="server" Width="255px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Number of answers is required."
                            ValidationGroup="RQGroup" Text="*" ForeColor="Red" ControlToValidate="ddlNoOfAns"></asp:RequiredFieldValidator>
                    </td>
                </tr>--%>
                    <tr class="styleTableRow">
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Status is required."
                                Text="*" ValidationGroup="RQGroup" ForeColor="Red" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="styleTableRow">
                        <td class="styleTableCell1">
                        </td>
                        <td class="styleTableCell2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="RQGroup"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" OnClick="btnClear_Click" />
                        </td>
                        <td class="styleTableCell3">
                        </td>
                        <td class="styleTableCell4">
                        </td>
                    </tr>
                    <tr class="styleTableRow">
                        <td class="styleTableCell1">
                        </td>
                        <td class="styleTableCell2" style="text-align: left" colspan="4">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="styleTableRow">
                        <td class="styleTableCell1">
                        </td>
                        <td colspan="4" style="text-align: left">
                            <asp:ValidationSummary ID="rqGroup" runat="server" ForeColor="Red" ValidationGroup="RQGroup"
                                Style="text-align: left" />
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
                        <td>
                        </td>
                    </tr>
                </table>
                <div>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:GridView ID="grdQuestion" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdQuestion_PageIndexChanging"
                                    OnRowDataBound="grdQuestion_RowDataBound" AllowPaging="true" OnSelectedIndexChanged="grdQuestion_SelectedIndexChanged"
                                    PageSize="7">
                                    <Columns>
                                        <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation ID" HeaderStyle-CssClass="hideGridColumn"
                                            ItemStyle-CssClass="hideGridColumn" />
                                        <asp:BoundField DataField="RQ_ID" HeaderText="Question ID" />
                                        <asp:BoundField DataField="QUESTION" HeaderText="Question" />
                                        <%--<asp:BoundField DataField="NO_OF_ANSWERS" HeaderText="Number of Answers">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>--%>
                                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfEvalName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
