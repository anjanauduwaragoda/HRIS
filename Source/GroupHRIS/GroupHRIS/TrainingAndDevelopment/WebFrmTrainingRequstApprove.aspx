<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainingRequstApprove.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingRequstApprove" %>

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
    <br />
    <span>Recommand/Approve Training Request </span>
    <hr />
    <table class="styleMainTb" style="text-align: center;">
        <tr>
            <td class="divsearch">
                <table>
                    <tr>
                        <td style="text-align: right;">
                            Year :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 50px; text-align: right;">
                            <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 450px; text-align: center; vertical-align: middle;">
                <div style="display: block; height: 110px; background-color: #aed6f1; text-align: center;">
                    <table width="400px">
                        <tr>
                            <td colspan="2">
                                <b>Recommandation</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px; text-align: right;">
                                Pending Requests :
                            </td>
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="lblrPending" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td colspan="2">
                                <asp:LinkButton ID="lbRecommend" runat="server" OnClick="lbRecommend_Click">View</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="width: 400px; text-align: center;">
                <div style="display: block; height: 110px; background-color: #aed6f1; text-align: center;">
                    <table width="450px">
                        <tr>
                            <td colspan="2">
                                <b>Approval</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px; text-align: right;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px; text-align: right;">
                                Pending Approval :
                            </td>
                            <td style="width: 200px; text-align: left;">
                                <asp:Label ID="lblapproveview" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td colspan="2">
                                <asp:LinkButton ID="lbApprove" runat="server" OnClick="lbApprove_Click">View</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="100%" style="text-align: center; margin: auto; width: 800px;">
        <tr>
            <td>
                <asp:GridView ID="grdTrainingRequest" runat="server" AutoGenerateColumns="False"
                    AllowPaging="True" OnPageIndexChanging="grdTrainingRequest_PageIndexChanging"
                    OnRowDataBound="grdTrainingRequest_RowDataBound" Width="800px"                     
                    onrowcommand="grdTrainingRequest_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="REQUEST_ID" HeaderText="Request Id" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" >
<HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="TYPE_NAME" HeaderText="Request Type" />--%>
                        <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" HeaderText="Training Description" />
                        <%--<asp:BoundField DataField="SKILLS_EXPECTED" HeaderText="Skill Expected" />--%>
                        <asp:BoundField DataField="NUMBER_OF_PARTICIPANTS" HeaderText="Number Of Participants" />
                        <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Request Date" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="To Do" />
                        <asp:ButtonField HeaderText="Action" Text="Update" CommandName = "Update"/>
                    </Columns>
                    <EmptyDataTemplate>
                        NO TRAINING REQUEST FOUND.
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:HiddenField ID="hfReqId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled"/>
                <asp:HiddenField ID="hfStatus" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled"/>
            </td>
        </tr>
    </table>
</asp:Content>
