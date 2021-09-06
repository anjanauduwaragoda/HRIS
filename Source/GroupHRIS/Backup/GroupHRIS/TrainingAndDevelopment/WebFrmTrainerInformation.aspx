<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainerInformation.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainerInformation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script type="text/javascript">

        function openLOVWindow(file) {

            window.open(file, 'Search', 'resizable=no,width=500,height=500,scrollbars=yes,top=50,left=200,status=yes');
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Trainer Details</span>
            <hr />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table id="leftTable">
                            <%-- name in full --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label2" runat="server" Text="Full Name : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtFullName" runat="server" MaxLength="300" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:ImageButton ID="ibtnApply" runat="server" Height="25px" ImageUrl="~/Images/Common/apply.jpg"
                                        OnClick="ibtnApply_Click" Width="25px" />
                                </td>
                                <td class="styleTableCell4">
                                    <asp:FilteredTextBoxExtender ID="ftFullName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                        ValidChars=" " TargetControlID="txtFullName">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell5">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullName"
                                        ErrorMessage="Name in full is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%-- name with inintials --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label1" runat="server" Text="Name with Initials : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:HiddenField ID="hfTrainerId" runat="server" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="150" Width="250px" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="ftName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                        ValidChars="." TargetControlID="txtName">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvNAme" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Name with initials is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--NIC--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label3" runat="server" Text="NIC : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtNIC" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="ftNIC" runat="server" FilterType="UppercaseLetters, Numbers"
                                        ValidChars="0,1,2,3,4,5,6,7,8,9,V,X,x,v" TargetControlID="txtNIC">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvNIC" runat="server" ControlToValidate="txtNIC"
                                        ErrorMessage="NIC is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="\d{9}[xXvV]"
                                        ControlToValidate="txtNIC" ValidationGroup="vgTrainer" runat="server" Text="*"
                                        ForeColor="Red" ErrorMessage="Invalid NIC Format"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="revNicLength" runat="server" ValidationGroup="vgTrainer"
                                        ControlToValidate="txtNIC" ValidationExpression="^([\S\s]{10,10})$" ErrorMessage="Ten characters are required for the NIC"
                                        ForeColor="Red">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <%--Mobile No --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label5" runat="server" Text="Mobile No. : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtMobile" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtMobile">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobile"
                                        ErrorMessage="Mobile No. is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Land phone No --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label4" runat="server" Text="Landline No. : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtLandline" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="ftLandLine" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtLandline">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <%--<asp:RequiredFieldValidator ID="rfvLandLine" runat="server" ControlToValidate="txtLandline"
                                        ErrorMessage="Landline No. is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>--%>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- Address --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label6" runat="server" Text="Address : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                        ValidChars='-/", ' TargetControlID="txtAddress">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress"
                                        ErrorMessage="Address is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- Bank --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label7" runat="server" Text="Bank :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlBank" runat="server" Width="250px" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvBank" runat="server" ControlToValidate="ddlBank"
                                        ErrorMessage="Bank is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Branch --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label8" runat="server" Text="Branch :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlBranch" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBranch"
                                        ErrorMessage="Branch is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Account No--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label11" runat="server" Text="Account No :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtAccount" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="txtAccount"
                                        ErrorMessage="Account no. is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Payment--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label9" runat="server" Text="Payment Instructions :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtPaymentInstruction" runat="server" Width="250px" MaxLength="500"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPaymentInstruction"
                                        ErrorMessage="Payment Instructions are required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Description--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label12" runat="server" Text="Description : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox TextMode="MultiLine" ID="txtDescription" runat="server" MaxLength="500"
                                        Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Qualification--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label13" runat="server" Text="Qualifications : "></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox TextMode="MultiLine" ID="txtQualifications" runat="server" MaxLength="500"
                                        Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--training Nature--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label14" runat="server" Text="Training Nature :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlNature" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlNature"
                                        ErrorMessage="Training Nature is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Internal/ External--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label17" runat="server" Text="Internal / External :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlInternalExternal" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlInternalExternal"
                                        ErrorMessage="Internal / External is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Status--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label10" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Status is required " ForeColor="Red" ValidationGroup="vgTrainer">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--add Photo--%>
                            <%--<tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label20" runat="server" Text="Photp :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>--%>
                            <%--progress bar--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td align="center">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Buttons--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgTrainer"
                                        Style="height: 26px; float: left;" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" Style="height: 26px;
                                        float: left; margin-left: 0;" OnClick="btnClear_Click" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%-- Validation --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:Label ID="lblErrorMsg" runat="server" Style="float: left"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgTrainer"
                                        ForeColor="Red" Style="text-align: left;" />
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
                    <td valign="top" style="">
                        <table id="Table1" width="350px">
                            <tr>
                                <td style="padding-top: 15px; padding-left: 5px; padding-right: 10px; padding-bottom: 10px;
                                    background-color: #f2f3f4; padding-left: 15px;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="max-width: 350px;">
                                        <tr class="styleTableRow">
                                            <td class="" style="max-width: 80px; text-align: right" valign="top">
                                                <asp:Label ID="Label18" runat="server" Text="Photo : "></asp:Label>
                                            </td>
                                            <td class="styleTableCell2" style="min-width: 260px; text-align: left; padding-left: 15px;">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr id="uploaderRow" runat="server">
                                                        <td>
                                                            <%--<asp:FileUpload ID="fuPhoto" ClientIDMode="Static" runat="server" Width="170px" 
                                                                Style="margin-bottom: 5px;" accept=".png,.jpg,.jpeg"/>
                                                                &nbsp;(500 Kb max)--%>
                                                            <asp:LinkButton ID="lbUploadPhoto" runat="server" Width="200px" ClientIDMode="Static"
                                                                OnClientClick="openLOVWindow('/TrainingAndDevelopment/WebFrmUploadPhoto.aspx')"
                                                                ForeColor="Black">Add / Change Photo</asp:LinkButton>
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="imgPhoto" runat="server" Height="120px" Width="111px" ImageAlign="Left"
                                                                ImageUrl="../Images/Add_Person.png" />
                                                            <asp:ImageButton ID="imgRemoveImage" runat="server" ImageUrl="../Images/close_button.png"
                                                                Height="18px" Width="20px" OnClick="lbRemoveImage_Click" ToolTip="Remove Photo" />
                                                            <%--<img src="../Images/close_button.png" runat="server" id="lbRemoveImage" style="height: 0px;
                                                        width: 16px" alt="Remove Photo" onclick="lbRemoveImage_Click" />--%>
                                                            <%--<asp:LinkButton ID="lbRemoveImage" runat="server" OnClick="lbRemoveImage_Click" >Remove Image</asp:LinkButton>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<asp:HiddenField ID="hfImageFile" runat="server" ClientIDMode="Static" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="styleTableRow">
                                            <td colspan="2" align="center">
                                                <asp:Label ID="lblErrorMsgPhoto" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="rightTable" width="360px">
                            <tr class="styleTableRow">
                                <td colspan="3">
                                    Trainer Competencies
                                    <hr />
                                    <asp:HiddenField ID="hfCompetencyId" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <%-- Competency Grid --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label15" runat="server" Text="Competency :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlCompetency" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCompetency"
                                        ErrorMessage="Competency is required " ForeColor="Red" ValidationGroup="vgCompetency">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Add/ View Trainers--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label19" runat="server" Text="Description :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtCompetencyDesc" runat="server" TextMode="MultiLine" MaxLength="300"
                                        Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                            </tr>
                            <%--Status--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label16" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlCompetencyStatus" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlCompetencyStatus"
                                        ErrorMessage="Status is required " ForeColor="Red" ValidationGroup="vgCompetency">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Buttons--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <br />
                                    <asp:Button ID="btnAddComp" runat="server" Text="Add" Width="125px" ValidationGroup="vgCompetency"
                                        Style="height: 26px; float: left;" OnClick="btnAddComp_Click" />
                                    <asp:Button ID="btnClearComp" runat="server" Text="Clear" Width="125px" Style="height: 26px;
                                        float: left; margin-left: 0;" OnClick="btnClearComp_Click" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                            </tr>
                            <%-- Validation --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:Label ID="lblErrorMsg2" runat="server" Style="float: left"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vgCompetency"
                                        ForeColor="Red" Style="text-align: left;" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                            </tr>
                            <%-- Competency Grid --%>
                            <tr class="styleTableRow">
                                <td colspan="3">
                                    <br />
                                    <asp:GridView ID="gvCompetencies" runat="server" Style="width: 340px" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="5" OnPageIndexChanging="gvCompetencies_PageIndexChanging"
                                        OnRowDataBound="gvCompetencies_RowDataBound" OnSelectedIndexChanged="gvCompetencies_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField HeaderText="Competency Id" DataField="COMPETENCY_ID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField HeaderText="Competency " DataField="NAME" />
                                            <asp:BoundField HeaderText="Status Code" DataField="STATUS_CODE" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField HeaderText="Status" DataField="STATUS_TEXT" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--Add/ View Trainers--%>
            <%--<table>
                
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <br />
                    </td>
                    <td class="style4">
                        <asp:LinkButton ID="lbAddViewProgrames" runat="server" Style="text-decoration: none;"
                            OnClick="lbAddViewProgrames_Click"> Add / View Training Programes >>> </asp:LinkButton>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
            </table>--%>
            <br />
            <br />
            <table width="100%">
                <tr>
                    <td>
                        Trainers
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left: 135px;">
                        <asp:Label ID="Label20" runat="server" Text="Training Nature : "></asp:Label>
                        <asp:DropDownList ID="ddlTrainingNatureFilter" runat="server" Width="250px" OnSelectedIndexChanged="ddlTrainingNatureFilter_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvTrainers" runat="server" AutoGenerateColumns="false" Style="width: 600px;"
                            OnPageIndexChanging="gvTrainers_PageIndexChanging" OnRowDataBound="gvTrainers_RowDataBound"
                            OnSelectedIndexChanged="gvTrainers_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField HeaderText="Trainer Id" DataField="TRAINER_ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField HeaderText="Name" DataField="NAME" />
                                <asp:BoundField HeaderText="Mobile No." DataField="CONTACT_MOBILE" />
                                <asp:BoundField HeaderText="Training Nature" DataField="TRAINING_NATURE" />
                                <asp:BoundField HeaderText="Status" DataField="STATUS" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Trainers Available</EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenDataCaptured" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
