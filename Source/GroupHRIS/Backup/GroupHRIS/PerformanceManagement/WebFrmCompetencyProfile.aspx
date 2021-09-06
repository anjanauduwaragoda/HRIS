<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCompetencyProfile.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmCompetencyProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .leftTableLeftSide
        {
            width: 120px;
            margin-left: 0;
        }
        .leftTableRightSide
        {
            max-width: 250px;
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openLOVWindow(file) {

            var e = document.getElementById("ddlProficiencyScheme");
            var selectedProficiency = e.options[e.selectedIndex].value;
            if (selectedProficiency != '') {
                //document.getElementById("lblErrorMsg").innerHTML('new');
                //lbl.innerHTML("new");
                window.open(file, 'Search', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes');
            }




        }

        function getValuefromChild(dataCaptured) {
            document.getElementById("HiddenDataCaptured").value = dataCaptured;
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
    <br />
    <span>Competency Profile Details</span>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" id="mainTable">
                <tr>
                    <td style="width: 50%; padding-left: 10px; vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Profile Name :"></asp:Label>
                                </td>
                                <td class="leftTableRightSide">
                                    <asp:TextBox ID="txtName" runat="server" Style="width: 244px" MaxLength="75"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                        ValidChars=" " TargetControlID="txtName">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="rfvName" ControlToValidate="txtName" runat="server"
                                        ErrorMessage="Profile Name is required" ForeColor="Red" ValidationGroup="vgCompetencyProfile">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                    <asp:Label ID="Label2" runat="server" Text="Proficiency Scheme :"></asp:Label>
                                </td>
                                <td class="leftTableRightSide">
                                    <asp:DropDownList ID="ddlProficiencyScheme" runat="server" Style="width: 250px" OnSelectedIndexChanged="ddlProficiencyScheme_SelectedIndexChanged"
                                        AutoPostBack="true" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDdlSchme" ControlToValidate="ddlProficiencyScheme"
                                        runat="server" ErrorMessage="Proficiency Scheme is required" ForeColor="Red"
                                        ValidationGroup="vgCompetencyProfile">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                    <asp:Label ID="Label3" runat="server" Text="Assessment Group :"></asp:Label>
                                </td>
                                <td class="leftTableRightSide">
                                    <asp:DropDownList ID="ddlAssessmentGroup" runat="server" Style="width: 250px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDdlAssessmentGroup" ControlToValidate="ddlAssessmentGroup"
                                        runat="server" ErrorMessage="Assessment Group is required" ForeColor="Red" ValidationGroup="vgCompetencyProfile">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                    <asp:Label ID="Label4" runat="server" Text="Description :"></asp:Label>
                                </td>
                                <td class="leftTableRightSide">
                                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" Style="width: 244px"
                                        MaxLength="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                    <asp:Label ID="Label5" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td class="leftTableRightSide">
                                    <asp:DropDownList ID="ddlProfileStatus" runat="server" Style="width: 250px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlProfileStatus"
                                        runat="server" ErrorMessage="Status is required" ForeColor="Red" ValidationGroup="vgCompetencyProfile">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftTableLeftSide" align="right">
                                </td>
                                <td class="leftTableRightSide" align="left" style="padding-right: 50px; padding-top: 10px;
                                    padding-bottom: 10px;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="openLOVWindow('/PerformanceManagement/WebFrmProfileCompetencies.aspx')"
                                        OnClick="LinkButtonSelectCompetencies_Click"> Select Competencies</asp:LinkButton>
                                </td>
                            </tr>
                            <%--onclick="LinkButtonSelectCompetencies_Click"--%>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;" class="leftTableRightSide">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="123px" OnClick="btnSave_Click"
                                        ValidationGroup="vgCompetencyProfile" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="123px" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <%--<tr>
                                        <td>&nbsp;</td>
                        <td style="text-align: left; padding-right:0px;" class="leftTableRightSide">
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgCompetencyProfile" ForeColor="Red"/>

                    </td>
                    </tr>--%>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    Selected Competencies
                                </td>
                            </tr>
                            <tr style="width: 100%">
                                <td style="width: 100%; min-width: 430px; padding-right: 10px;" align="right">
                      
                                    <asp:GridView ID="gvSelectedCompetencies" runat="server" Style="width: 430px;" AutoGenerateColumns="false"
                                        OnPageIndexChanging="gvCompetency_PageIndexChanging" OnRowDataBound="gvCompetency_RowDataBound"
                                        AllowPaging="true" PageSize="6">
                                        <Columns>
                                            <asp:BoundField DataField="COMPETENCY_ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COMPETENCY_GROUP_ID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            <asp:BoundField DataField="COMPETENCY_NAME" HeaderText="Competency Name" ControlStyle-Width="190px">
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <HeaderTemplate>
                                                    Expected Level
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlProficiencyLevels" runat="server" Width="90px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlProficiencyLevels_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <HeaderTemplate>
                                                    Exclude
                                                    <asp:CheckBox ID="excludeHeaderCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="excludeHeaderCheckBox_OnCheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--OnCheckedChanged="includeChildCheckBox_OnCheckedChanged"--%>
                                                    <asp:CheckBox ID="excludeChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="excludeChildCheckBox_OnCheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Competencies Selected
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="leftTableLeftSide">
                        &nbsp;
                    </td>
                    <td style="text-align: left; padding-left: 12px;" class="">
                        <asp:Label ID="lblErrorMsg" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgCompetencyProfile"
                            ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr style="width: 100%">
                    <td style="width: 100%; text-align: center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin: auto; width: 200px">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr style="width: 100%">
                    <td>
                    </td>
                </tr>
                <tr style="width: 100%">
                    <td align="left">
                        Competency Profiles
                    </td>
                </tr>
                <tr style="width: 100%">
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr style="width: 100%">
                    <td style="width: 100%" align="center">
                        <asp:GridView ID="gvCompetencyProfile" runat="server" AutoGenerateColumns="false"
                            Width="100%" OnPageIndexChanging="gvCompetencyProfile_PageIndexChanging" OnRowDataBound="gvCompetencyProfile_RowDataBound"
                            OnSelectedIndexChanged="gvCompetencyProfile_SelectedIndexChanged" AllowPaging="true">
                            <Columns>
                                <asp:BoundField DataField="COMPETENCY_PROFILE_ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="PROFILE_NAME" HeaderText="Profile Name" />
                                <asp:BoundField DataField="GROUP_ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GROUP_NAME" HeaderText="Assessment Group Name" />
                                <asp:BoundField DataField="PROFICIENCY_SCHEME_ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="SCHEME_NAME" HeaderText="Proficiency Scheme Name" ItemStyle-Width="170px" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="230px" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <%--<table style="width: 100%">
        <tr style="width: 100%">
            <td style="width: 100%; text-align: center">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin: auto; width: 200px">
                    <ProgressTemplate>
                        <img src="../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>--%>
    <asp:HiddenField ID="HiddenDataCaptured" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenProfileId" runat="server" Value="" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
