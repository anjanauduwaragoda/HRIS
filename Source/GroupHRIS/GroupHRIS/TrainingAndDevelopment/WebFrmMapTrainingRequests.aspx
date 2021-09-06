<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmMapTrainingRequests.aspx.cs" EnableEventValidation="false" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmMapTrainingRequests" %>

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
    Training Request of Trainings
    <hr />
    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
    <table style="margin: auto; width: 100%;">
        <tr>
            <td style="vertical-align:top;">
                <table>
                    <tr>
                        <td style="text-align: right">
                            Search Training
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchTraining" Width="200px" ClientIDMode="Static" runat="server"></asp:TextBox>
                            &nbsp;<img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtSearchTraining')" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSearchTraining" runat="server" ErrorMessage="Training is Required" Text="*" ForeColor="Red" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 250px; vertical-align: top;">
                            <div id="dv1" runat="server">
                                <strong>Training Details</strong>
                                <table style="margin: auto; width: 100%; background-color: #E9E9E9;">
                                    <tr>
                                        <td style="text-align: right;">
                                            Training Name
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
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
                    </tr>
                    <tr>
                        <td>Search Training Request</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="txtTrainingRequests" Width="200px" runat="server"></asp:TextBox>
                            &nbsp;<img alt="" src="../Images/Common/Search.jpg" id="imgRequestSearch" runat="server" height="20" width="20" onclick="openLOVWindow('../TrainingAndDevelopment/frmRequestSearch.aspx','search','txtTrainingRequests')" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtTrainingRequests" runat="server" ErrorMessage="Training Request is Required" Text="*" ForeColor="Red" ValidationGroup="Main"></asp:RequiredFieldValidator>
                            </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="">
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="">
                        <asp:GridView ID="grdvTrainingRequest" AutoGenerateColumns="false" 
                                Style="width: 380px;" AllowPaging="false" PageSize="5" runat="server" 
                                onrowdatabound="grdvTrainingRequest_RowDataBound" 
                                onselectedindexchanged="grdvTrainingRequest_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="REQUEST_ID" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%" HeaderText=" Training Request ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%" HeaderText=" Request Description " />
                                            <asp:BoundField DataField="REMARKS" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%" HeaderText=" Remarks " />

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblExclude" runat="server" Text="Exclude All"></asp:Label>
                                                    <br />
                                                    <asp:CheckBox ID="chkisIncludeAll" AutoPostBack="true" runat="server" OnCheckedChanged="chkisIncludeAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkisInclude" AutoPostBack="true" runat="server" OnCheckedChanged="chkisInclude_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="isExclude" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" ItemStyle-Width="40%" HeaderText=" isExclude " />

                                        </Columns>
                                    </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        
                        </td>
                        <td>
                        
                        </td>
                        <td style="text-align: right">
                        
                            <asp:Button ID="bntSave" Width="100px" runat="server" Text="Save" 
                                onclick="bntSave_Click"  ValidationGroup="Main"/>
                            <asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" 
                                onclick="btnClear_Click" />
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red"  ValidationGroup="Main" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align:top; width:450px;">
                <table style="width:100%;">
                    <tr>
                        <td>
                        Companies
                        <hr />
                            <asp:GridView ID="grdvTrainingCompanies" style="width:400px;" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="TRAINING_ID" HeaderText="Training ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                                    <asp:BoundField DataField="PLANNED_PARTICIPANTS" HeaderText="Planned Participants" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                        <div style="min-height:240px;"></div>Training Request Details
                        <hr />
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</asp:Content>