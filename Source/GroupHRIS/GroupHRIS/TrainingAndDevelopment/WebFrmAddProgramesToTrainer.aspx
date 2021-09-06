<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmAddProgramesToTrainer.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmAddProgramesToTrainer"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            width: 244px;
        }
        .ProgrameDetailTitle
        {
            max-width: 200px;
        }
        .style3
        {
            text-align: right;
            max-width: 200px;
            width: 156px;
            font-weight: bold;
        }
        .style4
        {
            width: 156px;
        }
        .style7
        {
            width: 350px;
        }
        .style8
        {
            width: 115px;
        }
        .style9
        {
            width: 365px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table width="100%" runat="server" id="tblGraphs" clientidmode="Static">
        <tr>
            <td style="min-width: 49%; background-color: #f2f4f4; padding-top: 0;" align="center"
                valign="top">
                <br />
                <table id="tblInstituteDetail" style="margin-top: 0;">
                    <tr>
                        <td colspan="2">
                            <br />
                            <span>Trainer Details</span>
                            <hr />
                        </td>
                    </tr>
                    <%--name--%>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNameTitle" runat="server" Text="Name : "></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%--Contact No.--%>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblContactTitle" runat="server" Text="Contact No. : "></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblContact" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%--Address--%>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="lblAddressTitle" runat="server" Text="Address : "></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
            <td style="background-color: white;">
            </td>
            <td style="min-width: 49%; background-color: #f2f4f4; vertical-align: top;" align="center">
                <br />
                <table id="Table2">
                    <tr>
                        <td colspan="2">
                            <br />
                            <span>Added Programmes </span>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="gvAddedProgrames" runat="server" Style="width: 400px" AutoGenerateColumns="false"
                                OnRowDataBound="gvAddedProgrames_RowDataBound" OnSelectedIndexChanged="gvAddedProgrames_SelectedIndexChanged"
                                OnPageIndexChanging="gvAddedProgrames_PageIndexChanging" AllowPaging="true" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="PROGRAM_ID" HeaderText=" Programe Id" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="PROGRAM_CODE" HeaderText=" Programe Code" />
                                    <asp:BoundField DataField="PROGRAM_NAME" HeaderText=" Name" />
                                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
        </tr>
    </table>
    <br />
    <table id="tblProgrameDetails" style="background-color: #ebf5fb; min-width: 99.7%;
        padding-left: 5px;" align="center" runat="server" clientidmode="Static">
        <tr>
            <td colspan="3">
                <br />
                Programme Details
            </td>
            <td align="right">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/close_button.png"
                    Height="18px" Width="20px" OnClick="ImageButton1_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <hr width="100%" />
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label1" runat="server" Text="Programme Code : "></asp:Label>
            </td>
            <td class="style9">
                <asp:Label ID="lblCode" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label3" runat="server" Text="Minimum Batch Size : "></asp:Label>
            </td>
            <td class="style7" align="left">
                <asp:Label ID="lblMinimum" runat="server" Text="20"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label2" runat="server" Text="Programme Name : "></asp:Label>
            </td>
            <td class="style9">
                <asp:Label ID="lblPrgmName" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label4" runat="server" Text="Maximum Batch Size : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblMaximum" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label5" runat="server" Text="Duration : "></asp:Label>
            </td>
            <td class="style9">
                <asp:Label ID="lblDuration" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label6" runat="server" Text="Description : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblDesc" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label7" runat="server" Text="Programme Type : "></asp:Label>
            </td>
            <td class="style9">
                <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label8" runat="server" Text="Objectives : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblObjectives" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3" valign="top" style="padding-top: 5px;">
                <br />
                <asp:Label ID="Label10" runat="server" Text="Description : "></asp:Label>
            </td>
            <td class="style9" valign="bottom" style="padding-top: 5px;">
                <br />
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Height="52px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description is required"
                    ControlToValidate="txtDescription" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
            <td colspan="2">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="style8">
                            <br />
                            <asp:Label ID="Label11" ClientIDMode="Static" runat="server" Text="Cost Per Session :"></asp:Label>
                        </td>
                        <td class="style7" colspan="2">
                            <br />
                            <asp:TextBox ID="txtCost" runat="server" ClientIDMode="Static" Width="119px"></asp:TextBox>&nbsp;(Rs.)&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Cost Per Session is required"
                                ControlToValidate="txtCost" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr style="">
                        <td class="style8" valign="bottom">
                            <asp:Label ID="lblStatus" ClientIDMode="Static" runat="server" Text="Status :"></asp:Label>
                        </td>
                        <td class="style11" valign="bottom" style="padding-top: 5px; min-width: 140px; max-width: 140px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125px" ClientIDMode="Static"
                                Style="">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is required"
                                ControlToValidate="ddlStatus" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" Width="75px" OnClick="btnAdd_Click"
                                ValidationGroup="gvTrainerDetails" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" Width="75px" OnClick="btnClear_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--<tr>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style7">
                &nbsp;
            </td>
            <td class="style3">
                <br />
                <asp:Label ID="lblStatus" ClientIDMode="Static" runat="server" Text="Status :"></asp:Label>
            </td>
            <td class="style7">
                <br />
                <asp:DropDownList ID="ddlStatus" runat="server" Width="125px" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="80px" OnClick="btnAdd_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="80px" OnClick="btnClear_Click" />
            </td>
        </tr>--%>
        <tr>
            <td class="style4">
                <br />
            </td>
        </tr>
    </table>
    <table width="855px">
        <tr>
            <td>
                <asp:LinkButton ID="lbFindTrainer" runat="server" Style="text-decoration: none;"> <<< Find Trainer </asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <span>Programmes </span>
    <hr />
    <table id="Table1" width="855px">
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblTrainingType" runat="server" Text="Tarining Type : "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingType" runat="server" Width="180px" OnSelectedIndexChanged="ddlTrainingType_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Tarining Category : "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingCategory" runat="server" Width="180px" OnSelectedIndexChanged="ddlTrainingCategory_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="min-width: 125px;">
                            <%--<asp:Button ID="btnFilter" runat="server" Text="Filter" Width="125px" 
                                />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="tblProgramGrid" width="855px">
        <tr>
            <td align="center" colspan="6">
                <asp:GridView ID="gvAllProgrammes" runat="server" Width="600px" AutoGenerateColumns="false"
                    OnRowDataBound="gvAllProgrammes_RowDataBound" OnSelectedIndexChanged="gvAllProgrammes_SelectedIndexChanged"
                    OnPageIndexChanging="gvAllProgrammes_PageIndexChanging" AllowPaging="true" PageSize="10">
                    <Columns>
                        <asp:BoundField DataField="PROGRAM_ID" HeaderText="programId" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="PROGRAM_CODE" HeaderText="Program Code" />
                        <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                        <asp:BoundField DataField="PROGRAM_TYPE" HeaderText="Program Type" />
                        <asp:BoundField DataField="MINIMUM_BATCH_SIZE" HeaderText="Min. Batch" />
                        <asp:BoundField DataField="MAXIMUM_BATCH_SIZE" HeaderText="Max. Batch" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Programs Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfTrainerId" runat="server" ClientIDMode="Static" />
</asp:Content>
