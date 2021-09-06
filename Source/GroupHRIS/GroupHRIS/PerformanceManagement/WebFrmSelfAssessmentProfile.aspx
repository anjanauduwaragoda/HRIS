<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmSelfAssessmentProfile.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmSelfAssessmentProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }


        function displayData(val) {

            document.getElementById("hfisInclude").value = val;

            DoPostBack();
        }


        function DoPostBack() {
            __doPostBack();
        }

        function show(id) {
            alert(id);
            // Do something as your need
        }
    </script>
    <style type="text/css">
        /* The Modal (background) */
        .modal
        {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }
        .hideGridColumn
        {
            display: none;
        }
        
        .custom-popup
        {
            display: inline-block;
            width: 300px;
            height: 120px;
            text-align: center;
            vertical-align: top;
            font-size: 11px;
            border: 0px;
        }
    </style>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Employee Self-Assessment Profile Details</span>
            <hr />
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%; vertical-align: text-top;">
                        <table style="margin: auto">
                            <tr>
                                <td align="right">
                                    Assessment Group :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAssessmentGroup" runat="server" Width="205px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlAssessmentGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="assessmentPro"
                                        ErrorMessage="Assessment Group is Required" Text="*" ControlToValidate="ddlAssessmentGroup"
                                        ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Profile Name :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProName" runat="server" Width="200px" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" runat="server"
                                        ValidationGroup="assessmentPro" ErrorMessage="Profile Name is Required" ControlToValidate="txtProName"
                                        ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters"
                                        ValidChars="/,-,' '" runat="server" TargetControlID="txtProName">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Description :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"
                                        MaxLength="150"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Status :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="assessmentPro"
                                        ErrorMessage="Status is Required" Text="*" ControlToValidate="ddlStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="MenuBar">
                                        <br />
                                        <asp:Literal ID="litDefaultQuestionList" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                                        ValidationGroup="assessmentPro" /><asp:Button ID="btnclear" runat="server" Text="Clear"
                                            Width="100px" OnClick="btnclear_Click" />
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
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hfisInclude" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="assessmentPro" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align: top">
                        <span>Self Assessment Questions</span>
                        <br />
                        <asp:GridView ID="gvviewQuestions" runat="server" AutoGenerateColumns="False" Style="width: 80%;"
                            AllowPaging="true" OnPageIndexChanging="gvviewQuestions_PageIndexChanging" OnRowDataBound="gvviewQuestions_RowDataBound"
                            PageSize="5" OnSelectedIndexChanged="gvviewQuestions_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="QUESTION_ID" HeaderText="Question Id " />
                                <asp:BoundField DataField="QUESTION" HeaderText=" Question " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="NO_OF_ANSWERS" HeaderText="No Of Answers" ><ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                <asp:TemplateField HeaderText="Exclude">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBxExclude" runat="server" Enabled="true" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                NO SELF ASSESSMENT QUESTION FOUND.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <span>Employee Self-Assessment Profiles </span>
            <hr />
            <div style="width: 100%">
                <table style="margin: auto">
                    <tr>
                        <td >
                            <asp:GridView ID="grdAssessmentProfile" runat="server" AutoGenerateColumns="false"
                                Style="margin: auto;" ShowHeaderWhenEmpty="true" OnRowDataBound="grdAssessmentProfile_RowDataBound"
                                AllowPaging="true" OnSelectedIndexChanged="grdAssessmentProfile_SelectedIndexChanged"
                                PageSize="10" OnPageIndexChanging="grdAssessmentProfile_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="SELF_ASSESSMENT_PROFILE_ID" HeaderText="Profile Id" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="PROFILE_NAME" HeaderText="Profile Name" />
                                    <asp:BoundField DataField="GROUP_NAME" HeaderText=" Assessment Group" />
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description" />
                                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
                                </Columns>
                                <EmptyDataTemplate>
                                    NO ASSESSMENT PROFILE FOUND.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
