<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmQuestionBank.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmQuestionBank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span>Self-Assessment Question Details</span><span style="font-weight: 700;"> </span>
    <hr />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td style="vertical-align: text-top;">
                        Question
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuestion" Style="width: 300px; height: 100px;" TextMode="MultiLine"
                            runat="server">
                        </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Question is Required" ForeColor="Red" Text="*" ValidationGroup="Main"
                            ControlToValidate="txtQuestion"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top;">
                        Remarks
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" Style="width: 300px; height: 100px;" TextMode="MultiLine"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top;">
                        Status
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" Style="width: 300px;" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" ValidationGroup="Main" Text="Save" Width="149"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="148" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                            <ProgressTemplate>
                                <img src="/Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                        <br />
                        <table style="margin:auto;">
                            <tr>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblAssessmentProfiles" runat="server" ForeColor="Black" Font-Size="10pt"></asp:Label>
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
            </table>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
            </table>
            Self-Assessment Questions
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvQuestionBank" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="10" runat="server" OnPageIndexChanging="grdvQuestionBank_PageIndexChanging"
                            OnRowDataBound="grdvQuestionBank_RowDataBound" OnSelectedIndexChanged="grdvQuestionBank_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="QUESTION_ID" HeaderText=" Question ID "/>
                                <asp:BoundField DataField="QUESTION" HeaderText=" Question " />
                                <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status Code " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE_VALUE" HeaderText=" Status " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>