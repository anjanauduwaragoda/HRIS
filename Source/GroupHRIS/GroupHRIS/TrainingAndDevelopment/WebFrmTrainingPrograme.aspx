<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainingPrograme.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingPrograme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            margin-left: 2px;
        }
        .style1
        {
            width: 303px;
        }
        .style2
        {
            width: 62px;
        }
        .style3
        {
            width: 292px;
        }
        .hiddencol
        {
            display: none;
        }
        .durationCol
        {
            text-align: right;
            padding-right: 30px;
        }
        .style4
        {
            width: 183px;
            text-align: right;
        }
        .style11
        {
            width: 148px;
        }
        .style18
        {
            width: 99px;
        }
        .style22
        {
            width: 119px;
        }
        .style23
        {
            width: 142px;
        }
        .style25
        {
            width: 143px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Training Program Details</span>
            <hr />
            <br />
            <table>
                <tr>
                    <td style="min-width: 450px;">
                        <table>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDurationHrs"
                    ErrorMessage="Programe duration is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label8" runat="server" Text="Program Code :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtProgrameCode" runat="server" Width="250px" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                        ValidChars="-, " TargetControlID="txtProgrameCode">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProgrameCode"
                                        ErrorMessage="Program Code is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Programe Type--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label1" runat="server" Text="Program Name :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtProgrameName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="ftProgrameName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                        ValidChars=" " TargetControlID="txtProgrameName">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvProgrameName" runat="server" ControlToValidate="txtProgrameName"
                                        ErrorMessage="Program name is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- Min batch size--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label2" runat="server" Text="Training Type :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlTrainingType" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvTrainingType" runat="server" ControlToValidate="ddlTrainingType"
                                        ErrorMessage="Training Type is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- category--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label3" runat="server" Text="Training Category :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlTrainingCategory" runat="server" Width="250px" 
                                        onselectedindexchanged="ddlTrainingCategory_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvTrainingCategory" runat="server" ControlToValidate="ddlTrainingCategory"
                                        ErrorMessage="Training Category is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- sub category--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label14" runat="server" Text="Training Subcategory :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlTrainingSubcategory" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTrainingSubcategory"
                                        ErrorMessage="Training Subcategory is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Duration--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label4" runat="server" Text="Program Duration :"></asp:Label>
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:TextBox ID="txtDurationHrs" runat="server" Width="100px" MaxLength="3" Style="float: left;"></asp:TextBox>
                                    &nbsp; Hrs &nbsp;
                                    <asp:DropDownList ID="ddlDurationMins" runat="server" Width="66px">
                                    </asp:DropDownList>
                                    &nbsp; mins
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    
                                    <asp:FilteredTextBoxExtender ID="ftDurationHrs" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtDurationHrs">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--program type--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label5" runat="server" Text="Program Type :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlProgrameType" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvProgmeType" runat="server" ControlToValidate="ddlProgrameType"
                                        ErrorMessage="Program Type is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Add/ View Trainers--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label6" runat="server" Text="Minimum Batch Size :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtMinBatch" runat="server" Width="250px" MaxLength="11"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtMinBatch">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <%--<asp:RequiredFieldValidator ID="rfvMinBatch" runat="server" ControlToValidate="txtMinBatch"
                                        ErrorMessage="Minimum Batch Size is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>--%>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--buttons--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label7" runat="server" Text="Maximum Batch Size :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtMaxBatch" runat="server" Width="250px" MaxLength="11"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtMaxBatch">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvMaxBatch" runat="server" ControlToValidate="txtMaxBatch"
                                        ErrorMessage="Maximum Batch Size is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Description--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label10" runat="server" Text="Description :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="250px" MaxLength="500"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Objectives--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label16" runat="server" Text="Objectives :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtObjectives" runat="server" TextMode="MultiLine" Width="250px" MaxLength="500"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Training Status--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label9" runat="server" Text="Training Status :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Training Status is required " ForeColor="Red" ValidationGroup="vgPrograme">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--buttons--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="124px" Style="float: left;"
                                        OnClick="btnSave_Click" ValidationGroup="vgPrograme" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="124px" Style="float: left;
                                        margin-left: 0;" OnClick="btnClear_Click" />
                                    <br />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--message lable--%>
                            <tr class="styleTableRow">
                                <td colspan="2" align="right">
                                    <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:ValidationSummary ID="validationSummery1" runat="server" Style="text-align: left;"
                                        ValidationGroup="vgPrograme" ForeColor="Red" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="style1" valign="top" style="min-width: 400px; background-color: #d7dbdd;
                        padding-left: 15px;">
                        <table style="float: left; margin-left: 0px; width: 370px;">
                            <tr>
                                <td class="style2" colspan="2" align="left" valign="top" style="padding-left: 4px;
                                    padding-top: 10px;">
                                    Training Outcomes
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="float: left; margin-left: 0px;">
                            <tr>
                                <td class="style2" align="right" valign="top">
                                    <asp:Label ID="Label11" runat="server" Text="Outcome : "></asp:Label>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="txtOutCome" runat="server" Width="296px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvOutcome" runat="server" ErrorMessage="Outcome is required"
                                        ValidationGroup="vgOutcome" ControlToValidate="txtOutCome" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" align="right">
                                    <asp:Label ID="Label12" runat="server" Text="Status : "></asp:Label>
                                </td>
                                <td class="style3">
                                    <asp:DropDownList ID="ddlOutcomeStatus" runat="server" Width="148px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvOutcomeStatus" runat="server" ErrorMessage="Outcome status is required"
                                        ValidationGroup="vgOutcome" ControlToValidate="ddlOutcomeStatus" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                </td>
                                <td align="right" class="style3">
                                    <asp:Button ID="btnAddOutcome" runat="server" Text="Add" Width="144px" Style="float: left;"
                                        OnClick="btnAddOutcome_Click" ValidationGroup="vgOutcome" />
                                    <asp:Button ID="btnClearOutcome" runat="server" Text="Clear" Width="144px" OnClick="btnClearOutcome_Click" />
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="style2">
                                </td>--%>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblErrorMsg2" runat="server" Text=""></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgOutcome"
                                        ForeColor="Red" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="gvOutcomes" runat="server" Style="width: 375px;" AutoGenerateColumns="false"
                            AllowPaging="true" PageSize="3" OnPageIndexChanging="gvOutcomes_PageIndexChanging"
                            OnRowDataBound="gvOutcomes_RowDataBound" OnSelectedIndexChanged="gvOutcomes_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="outcomeId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField DataField="outcome" HeaderText="Outcome" />
                                <asp:BoundField DataField="outcomeStatus" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField DataField="outcomeStatusText" HeaderText="Status" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="tempId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfProgrameId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfOutcomeTempId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <%--Add/ View Trainers--%>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <br />
            </td>
            <td class="style4">
                <asp:LinkButton ID="lbAddViewTrainers" runat="server" Style="text-decoration: none;"
                    OnClick="lbAddViewTrainers_Click"> Add / View Trainers >>>   </asp:LinkButton>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <br />
            <table style="" >
                <tr>
                    <td>
                        <span>Training Programs</span>
                        <hr />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                
                            <tr>
                                <td align="center">
                                    <table width="870px">
                                        <tr>
                                        <%--class="styleDdlSmall"--%>
                                            <td class="style18" valign="middle" align="right">
                                                <asp:Label ID="lblTrainingType" runat="server" Text="Training Type : "></asp:Label>
                                            </td>
                                            <td class="style11" align="left">
                                                <asp:DropDownList ID="ddlTrainingTypeSearch" runat="server" Width="140px" 
                                                    AutoPostBack="false" 
                                                    onselectedindexchanged="ddlTrainingTypeSearch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <%--<td class="style5">
                                            
                                            </td>--%>
                                            <td class="style22" align="right" valign="middle">
                                                <asp:Label ID="Label13" runat="server" Text="Training Category : "></asp:Label>
                                            </td>
                                            <td align="left" class="style23">
                                                <asp:DropDownList ID="ddlTrainingCategorySearch" runat="server" Width="140px" 
                                                    AutoPostBack="true" 
                                                    onselectedindexchanged="ddlTrainingCategorySearch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>

                                            <td class="style23" align="right" valign="middle">
                                                <asp:Label ID="Label15" runat="server" Text="Training Subcategory : "></asp:Label>
                                            </td>
                                            <td align="left" class="style25">
                                                <asp:DropDownList ID="ddlTrainingSubcategorySearch" runat="server" Width="140px" >
                                                </asp:DropDownList>
                                            </td>
                                            <td style="">
                                                <asp:ImageButton ID="iBtnSearch" runat="server" Height="30px" 
                                    ImageUrl="~/Images/Search.png" Width="30px" onclick="iBtnSearch_Click" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvTrainingProgrames" runat="server" AutoGenerateColumns="false"
                            OnRowDataBound="gvTrainingProgrames_RowDataBound" OnSelectedIndexChanged="gvTrainingProgrames_SelectedIndexChanged"
                            Style="width: 850px;" OnPageIndexChanging="gvTrainingProgrames_PageIndexChanging"
                            AllowPaging="true" PageSize="10">
                            <Columns>
                                <asp:BoundField DataField="PROGRAM_ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField DataField="PROGRAM_CODE" HeaderText="Program Code" />
                                <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                                <asp:BoundField DataField="PROGRAM_DURATION" HeaderText="Duration (Hrs)" HeaderStyle-Width="105px"
                                    ItemStyle-CssClass="durationCol" />
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
