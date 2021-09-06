<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="WebFrmTraingRequest.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTraingRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
                        
        }


        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sTrId) {
            var ctl = document.getElementById("hfCaller").value;

            //alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }



//        function getValueFromChild(sRetVal) {
//            var ctl = document.getElementById("hfCaller").value;
//            document.getElementById(ctl).value = sRetVal;

//            var id = document.getElementById(txb).value;
//            document.getElementById("hfVal").value = id;
//            //document.getElementById(ctl).value = sRetVal;

////            DoPostBack();
//        }

////        function DoPostBack() {
////            __doPostBack();
////        }
    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        #MasterBody
        {
            text-align: left;
        }
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <span>Training Request Details</span>
            <hr />
            <br />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblCompany" runat="server" Text="Company :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="256px" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company is required"
                            ControlToValidate="ddlCompany" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Department :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="256px" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ErrorMessage="Department is required"
                            ControlToValidate="ddlDepartment" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label3" runat="server" Text="Division :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlDivision" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvDivision" runat="server" ErrorMessage="Division is required"
                            ControlToValidate="ddlDivision" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Text="Branch :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlBranch" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ErrorMessage="Branch is required"
                            ControlToValidate="ddlBranch" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label5" runat="server" Text="Training Category :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlCategory" runat="server" Width="256px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ErrorMessage="Training category is required"
                            ControlToValidate="ddlCategory" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label6" runat="server" Text="Training Subcategory :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlSubcategory" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvSubcategory" runat="server" ErrorMessage="Training subcategory is required"
                            ControlToValidate="ddlSubcategory" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label7" runat="server" Text="Request Type :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlRequestType" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvRequestType" runat="server" ErrorMessage="Request type is required"
                            ControlToValidate="ddlRequestType" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label8" runat="server" Text="Description of Training :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description of training is required"
                            ControlToValidate="txtDescription" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label9" runat="server" Text="Reason :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtReason" runat="server" Width="250px" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Reason is required"
                            ControlToValidate="txtReason" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label10" runat="server" Text="Skills Expected :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtSkillsExpected" runat="server" Width="250px" MaxLength="500"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label11" runat="server" Text="Number of Participants :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtParticipants" runat="server" Width="125px" MaxLength="500" Style="text-align: left"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvParticipants" runat="server" ErrorMessage="Number of participannts is required"
                            ControlToValidate="txtParticipants" ForeColor="Red" ValidationGroup="CGroup"
                            EnableTheming="False">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        <asp:FilteredTextBoxExtender ID="fteParticipants" runat="server" FilterType="Numbers,Custom"
                            ValidChars="." TargetControlID="txtParticipants">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label12" runat="server" Text="Requested Date :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="125px" 
                            Style="text-align: left" ReadOnly="false"></asp:TextBox>
                        <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Requested date is required"
                            ControlToValidate="txtDate" ValidationGroup="CGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label13" runat="server" Text="To Recommend :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtRecPerson" runat="server" ReadOnly="true" CssClass="styleTableCell2TextBox"
                            MaxLength="8" ClientIDMode="Static"></asp:TextBox>                        
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtRecPerson')"
                            id="imgSearch1" runat="server" style="height:20px; width:20px;" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvToRecommend" runat="server" ErrorMessage="To recommend is required"
                            ControlToValidate="txtRecPerson" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        &nbsp;</td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label14" runat="server" Text="To Approve :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtToApprove" runat="server" ReadOnly="true" CssClass="styleTableCell2TextBox"
                            MaxLength="8" ClientIDMode="Static" TabIndex="1"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtToApprove')"
                            id="imgSearch2" runat="server" style="height:20px; width:20px;" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvToApprove" runat="server" ErrorMessage="To approve is required"
                            ControlToValidate="txtToApprove" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label15" runat="server" Text="Remarks :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="200" TextMode="MultiLine"
                            Height="60px"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label38" runat="server" Text="Status :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="Status is required"
                            ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Financial Year :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlFinancialYear" runat="server" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvFinancialYear" runat="server" ErrorMessage="Financial year  is required"
                            ControlToValidate="ddlFinancialYear" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblRecommendation" runat="server" Text="Recommendation :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <table>
                            <tr>
                                <td style="width: 125px; text-align: left">
                                    <asp:CheckBox ID="chkRecommended" runat="server" Text="Recommended" AutoPostBack="True"
                                        OnCheckedChanged="chkRecommended_CheckedChanged" />
                                </td>
                                <td style="width: 125px; text-align: left">
                                    <asp:CheckBox ID="chkRecRejected" runat="server" Text="Rejected" AutoPostBack="True"
                                        OnCheckedChanged="chkRecRejected_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label16" runat="server" Text="Recommended Date :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtRecDate" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                            TargetControlID="txtRecDate" Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblRecommendedReason" runat="server" Text="Recommended/Rejected Reason :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtRecommendedReason" runat="server" TextMode="MultiLine" MaxLength="500"
                            Width="250px" Height="100px"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblApproval" runat="server" Text="Approval :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <table>
                            <tr>
                                <td style="width: 125px; text-align: left">
                                    <asp:CheckBox ID="chkApproved" runat="server" Text="Approved" AutoPostBack="True"
                                        OnCheckedChanged="chkApproved_CheckedChanged" />
                                </td>
                                <td style="width: 125px; text-align: left">
                                    <asp:CheckBox ID="chkAppRejected" runat="server" Text="Rejected" AutoPostBack="True"
                                        OnCheckedChanged="chkAppRejected_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label17" runat="server" Text="Approved Date :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtAppDate" runat="server" Width="125px"></asp:TextBox> 
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy/MM/dd"
                            TargetControlID="txtAppDate" >
                        </asp:CalendarExtender>             
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblApprovedReason" runat="server" 
                            Text="Approved/Rejected Reason :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtApprovedReason" runat="server" TextMode="MultiLine" MaxLength="500"
                            Width="250px" Height="100px"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                            ValidationGroup="CGroup" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left" colspan="5">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                    <%--  <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">
            </td>--%>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left" colspan="5">
                        <asp:ValidationSummary ID="vsCGroup" runat="server" ForeColor="Red" ValidationGroup="CGroup" />
                    </td>
                    <%--<td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">
            </td>--%>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfTrainingRequestId" runat="server" />
                        <asp:HiddenField ID="hfAction" runat="server" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label37" runat="server" Text="Select request to modify :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtTrRequestId" runat="server" ReadOnly="true" Width="250px" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" onclick="openLOVWindow('frmRequestSearch.aspx','imgEditSearch','txtTrRequestId')"
                            id="imgEditSearch" height="20px" width="20px" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvTrRequestId" runat="server" ErrorMessage="Please select request to modify"
                            ControlToValidate="txtTrRequestId" ValidationGroup="vgSearch" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="125px" ValidationGroup="vgSearch"
                            OnClick="btnEdit_Click" />
                        <asp:Button ID="btnSCancel" runat="server" Text="Clear" Width="125px" OnClick="btnSCancel_Click" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left" colspan="5">
                        <asp:ValidationSummary ID="vsSearch" runat="server" ValidationGroup="vgSearch" ForeColor="Red" />
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                    <td class="styleTableCell7">
                    </td>
                </tr>
            </table>
            <br />
            <span>Training Requests by You </span>
            <asp:Label ID="lblNoRequestMessage" runat="server"></asp:Label>
            <hr />
            <br />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label18" runat="server" Text="Financial Year : "></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align:left;width:125px">
                        <asp:DropDownList ID="ddlFYear" runat="server" Width="125px" 
                            onselectedindexchanged="ddlFYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell2" style="text-align:right;width:125px">
                        <asp:Label ID="Label19" runat="server" Text="Status : "></asp:Label>
                    </td>
                    <td class="styleTableCell3" style="width:240px;text-align:left" colspan="4">
                        <asp:DropDownList ID="ddlRecStatus" runat="server" Width="150px" 
                            onselectedindexchanged="ddlRecStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>                    
                    <td class="styleTableCell7" style="text-align:center">
                        <asp:ImageButton ID="iBtnSearch" runat="server" Height="30px" 
                        ImageUrl="~/Images/Search.png" Width="30px" onclick="iBtnSearch_Click" />
                    </td>                    
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">                        
                    </td>
                    <td class="styleTableCell2" style="text-align:left;width:490px" colspan="6">                        
                        <asp:Label ID="lblSearchMessage" runat="server" Text=""></asp:Label>
                    </td>                                      
                    <td class="styleTableCell7" style="text-align:center">                        
                    </td>                    
                </tr>
            </table>
            <br />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="gvRequests" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" 
                            onpageindexchanging="gvRequests_PageIndexChanging" 
                            onrowdatabound="gvRequests_RowDataBound" 
                            onselectedindexchanged="gvRequests_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="REQUEST_ID" HeaderText="Request Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" 
                                    HeaderText="Description of Training">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NUMBER_OF_PARTICIPANTS" HeaderText="Participants">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Requested Date">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_RECOMENDED" HeaderText="Is Recommended">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_APPROVED" HeaderText="Is Approved">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
