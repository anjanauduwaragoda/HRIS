<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmTrainingRequestOfTraining.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingRequestOfTraining" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
    </script>
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        
        .InformationBox
        {
            background: rgb(165,200,255);
        }
        .Question
        {
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(165,204,255);
        }
        .Answer
        {
            padding-left: 20px;
            padding-right: 10px;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-top: 5px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(216,216,216);
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Training Request of Training
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <table style="margin: auto;">
                <tr>
                    <td style="vertical-align: top;">
                        <table style="width: 400px;">
                            <tr>
                                <td style="text-align: right;">
                                    Search Training
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTrainingID" ClientIDMode="Static" ReadOnly="true" runat="server"></asp:TextBox>
                                    <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTrainingID')" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 21px;">
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 21px;">
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 250px; vertical-align: top;">
                                    <div id="dv1" runat="server">
                                    <strong>Training Details</strong>
                                    <table style="margin: auto; width: 100%; background-color: #E9E9E9;">
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Training Name
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblTrainingName" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Training Code
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTrainingCode" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Training Program
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTrainingProgram" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Training Type
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTrainingType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Planned Participants
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlannedParticipants" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Planned Start Date
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlannedStartDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Planned End Date
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlannedEndDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>                                    
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="3" style="height: 220px; vertical-align: top;">--%>
                                <td colspan="3" style="height: 305px; vertical-align: top;">

                                <table style="width: 400px;">
                            <tr>
                                <td style="text-align: right;" colspan="2">
                                    Search Training Request
                                </td>
                                <td>
                                    :
                                    <asp:TextBox ID="txtTrainingRequestID" ReadOnly="true" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    <img alt="" src="../Images/Common/Search.jpg" id="imgRequestSearch" runat="server" height="20" width="20" onclick="openLOVWindow('../TrainingAndDevelopment/frmRequestSearch.aspx','search','txtTrainingRequestID')" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 21px;">
                                    <asp:CheckBox ID="chkUnAssignRequests" AutoPostBack="true" Text="List all unassigned training requests"
                                        runat="server" OnCheckedChanged="chkUnAssignRequests_CheckedChanged" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 21px;">
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 250px; vertical-align: top;">
                                    <div id="dv2" runat="server">
                                    <strong>Training Requests</strong>
                                    <asp:GridView ID="grdvTrainingRequest" AutoGenerateColumns="false" Style="width: 380px;"
                                        AllowPaging="true" PageSize="5" runat="server" OnPageIndexChanging="grdvTrainingRequest_PageIndexChanging"
                                        OnRowDataBound="grdvTrainingRequest_RowDataBound" OnSelectedIndexChanged="grdvTrainingRequest_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="REQUEST_ID" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%"
                                                HeaderText=" Training Request ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblExclude" runat="server" Text="Include All"></asp:Label>
                                                    <br />
                                                    <asp:CheckBox ID="chkisIncludeAll" AutoPostBack="true" runat="server" OnCheckedChanged="chkisIncludeAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkisInclude" AutoPostBack="true" runat="server" OnCheckedChanged="chkisInclude_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="40%" HeaderText=" Request Description " />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Button ID="btnAddToTraining" Visible="false" Width="125px" runat="server" Text="<< Add to Training"
                                        OnClick="btnAddToTraining_Click" />
                                        </div>
                                        <br />
                                    <asp:Button ID="btnSaveMain" Width="100px" runat="server" Text="Save" />
                                    <asp:Button ID="btnClearMain" Width="100px" runat="server" Text="Clear" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                <div id="Div4" runat="server">
                                    <strong>Training Request Details</strong>
                                    <table style="margin: auto; width: 100%; background-color: #E9E9E9;">
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Training Category
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblTrainingCategory" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Training Type
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblRequestTrainingType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Company
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Department
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Division
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Branch
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Request Type
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblTrainingRequestType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Employee
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblEmployee" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Designation
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDesignation" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Email
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Request Reason
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblRequestReason" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Description
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lbldescription" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Skills Expected
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblSkillsExpected" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                No. of participants
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="NoOfParticipants" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>

                        <br />

                                <div id="dv3" runat="server">
                                    <strong>Assigned Training Requests</strong>
                                    <asp:GridView ID="grdvAssignedRequests" AutoGenerateColumns="false" Style="width: 380px;"
                                        AllowPaging="true" PageSize="5" runat="server" OnPageIndexChanging="grdvAssignedRequests_PageIndexChanging"
                                        OnRowDataBound="grdvAssignedRequests_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="REQUEST_ID" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%"
                                                HeaderText=" Training Request ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="40%" HeaderText=" Request Description " />
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" MaxLength="300" Width="100%" TextMode="MultiLine"
                                                        runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="40%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblExclude" runat="server" Text="Exclude All"></asp:Label>
                                                    <br />
                                                    <asp:CheckBox ID="chkisExcludeAll" AutoPostBack="true" runat="server" OnCheckedChanged="chkisExcludeAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkisExclude" AutoPostBack="true" runat="server" OnCheckedChanged="chkisExclude_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td style="text-align: right;">
                                <div id="Div5" runat="server">
                                    <asp:Button ID="btnSave" runat="server" Width="125px" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" Width="125px" Text="Clear" OnClick="btnClear_Click" />
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <%--<table style="width: 400px;">
                            <tr>
                                <td style="text-align: right;" colspan="2">
                                    Search Training Request
                                </td>
                                <td>
                                    :
                                    <asp:TextBox ID="txtTrainingRequestID" ReadOnly="true" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    <img alt="" src="../Images/Common/Search.jpg" id="imgRequestSearch" runat="server"
                                        height="20" width="20" onclick="openLOVWindow('../TrainingAndDevelopment/frmRequestSearch.aspx','search','txtTrainingRequestID')" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 21px;">
                                    <asp:CheckBox ID="chkUnAssignRequests" AutoPostBack="true" Text="List all unassigned training requests"
                                        runat="server" OnCheckedChanged="chkUnAssignRequests_CheckedChanged" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 21px;">
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 250px; vertical-align: top;">
                                    <div id="dv2" runat="server">
                                    <strong>Training Requests</strong>
                                    <asp:GridView ID="grdvTrainingRequest" AutoGenerateColumns="false" Style="width: 380px;"
                                        AllowPaging="true" PageSize="5" runat="server" OnPageIndexChanging="grdvTrainingRequest_PageIndexChanging"
                                        OnRowDataBound="grdvTrainingRequest_RowDataBound" OnSelectedIndexChanged="grdvTrainingRequest_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="REQUEST_ID" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%"
                                                HeaderText=" Training Request ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblExclude" runat="server" Text="Include All"></asp:Label>
                                                    <br />
                                                    <asp:CheckBox ID="chkisIncludeAll" AutoPostBack="true" runat="server" OnCheckedChanged="chkisIncludeAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkisInclude" AutoPostBack="true" runat="server" OnCheckedChanged="chkisInclude_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="40%" HeaderText=" Request Description " />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Button ID="btnAddToTraining" Visible="false" Width="125px" runat="server" Text="<< Add to Training"
                                        OnClick="btnAddToTraining_Click" />
                                        </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                <div id="Div4" runat="server">
                                    <strong>Training Request Details</strong>
                                    <table style="margin: auto; width: 100%; background-color: #E9E9E9;">
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Training Category
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblTrainingCategory" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Training Type
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblRequestTrainingType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Company
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Department
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Division
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Branch
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Request Type
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblTrainingRequestType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Employee
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblEmployee" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Designation
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblDesignation" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Email
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Request Reason
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblRequestReason" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Description
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lbldescription" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                Skills Expected
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="lblSkillsExpected" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 34%;">
                                                No. of participants
                                            </td>
                                            <td style="width: 1%;">
                                                :
                                            </td>
                                            <td style="width: 65%;">
                                                <asp:Label ID="NoOfParticipants" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                    <div id="Div6" runat="server">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>