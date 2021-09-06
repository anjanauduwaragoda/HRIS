<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmTrainingParticipants.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingParticipants" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function changeScreenSize() {
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(200, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }
    </script>
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
        .style1
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            Training Participants
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td style="vertical-align: top;">
                    Training Details
                    <hr />
                        <table id="tblCompany" runat="server">
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Training ID
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtTrainingID" Width="200px" ReadOnly="true" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RegularExpressionValidator2" ValidationGroup="Main"
                                        ControlToValidate="txtTrainingID" runat="server" ForeColor="Red" Text="*" ErrorMessage="Training ID is required"></asp:RequiredFieldValidator>
                                    <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTrainingID')"
                                        id="imgEditSearch" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Training Name
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblTrainingName" runat="server" CssClass="style1" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Training Code
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblTrainingCode" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Program Name    
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblProgramName" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Training&nbsp; Type</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblTrainingType" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Planned Participants</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblPlannedParticipants" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Planned Start Date</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblPlannedStartDate" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Planned End Date</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblPlannedEndDate" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Planned Total Hours</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblPlannedTotalHours" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Status</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblTrainingStatus" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <%--<table id="tblEmployee" runat="server">
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Employee ID
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:TextBox ID="txtEmployeeID" Width="200" runat="server" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Employee is Required"
                                        ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtEmployeeID"></asp:RequiredFieldValidator>
                                    <img alt="" src="../../Images/Common/Search.jpg" id="EmpSearch" runat="server" height="20"
                                        width="20" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Name
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    EPF
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Designation
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Department
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Division
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Branch
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Company
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Is Participated
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Reporting Head
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                    Participant Status</td>
                                <td style="vertical-align: top;">
                                    :</td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align: right;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                                <td style="vertical-align: top;">
                                </td>
                            </tr>
                        </table>--%>
                        Company Details
                        <hr />
                        <table>
                            <tr>
                                <td style="width:400px;">
                                    <asp:GridView ID="grdvCompanyDetails"  style="width:400px;" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField  DataField="COMPANY_ID" HeaderText=" Company ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                                        <asp:BoundField  DataField="COMP_NAME" HeaderText=" Company "/>
                                        <asp:BoundField  DataField="PLANNED_PARTICIPANTS" ItemStyle-HorizontalAlign="Right" HeaderText=" No. of Participants "/>
                                        <asp:BoundField  DataField="SELECTED_PARTICIPANTS" ItemStyle-HorizontalAlign="Right" HeaderText=" No. of Participants Selected "/>
                                    </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Searching Criterias
                        <hr />
                        <table style="margin:auto;width:100%;">
                            <tr>
                                <td style="text-align: right">Company</td>
                                <td>:</td>
                                <td><asp:DropDownList ID="ddlCompanySearch" Width="200px" runat="server" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlCompanySearch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td style="text-align: right">Department</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlDepartmentSearch" Width="200px" runat="server" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlDepartmentSearch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Division</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlDivisionSearch" Width="200px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td style="text-align: right">Branch</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlBranchSearch" Width="200px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Width="100px" Text="Search" 
                                        onclick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    <br />
                    Employees
                    <hr />
                    <asp:GridView ID="grdvSelectedEmployees" runat="server" AutoGenerateColumns="false" 
                            AllowPaging="true" Width="100%" 
                            onpageindexchanging="grdvSelectedEmployees_PageIndexChanging" 
                            onrowdatabound="grdvSelectedEmployees_RowDataBound">
                                    <Columns>
                                        <asp:BoundField  DataField="COMPANY_ID" HeaderText=" Company ID " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                                        <asp:BoundField  DataField="EMPLOYEE_ID" HeaderText=" Employee ID " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                                        <asp:BoundField  DataField="EMPLOYEE_NAME" HeaderText=" Employee "/>
                                        <asp:BoundField  DataField="DESIGNATION_NAME" HeaderText=" Designation "/>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkEmployeeIncludeAll" AutoPostBack="true" Text=" Include All " runat="server" oncheckedchanged="chkEmployeeIncludeAll_CheckedChanged" />
                                        </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEmployeeInclude" AutoPostBack= "true" runat="server" oncheckedchanged="chkEmployeeInclude_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>                       
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right;"> 
                    <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                            onclick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" 
                            onclick="btnClear_Click" />                        
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">
                    Participants
                    <hr />
                        <asp:GridView ID="grdvTrainingParticipants" runat="server" 
                            AutoGenerateColumns="false" AllowPaging="true" Width="100%" 
                            onpageindexchanging="grdvTrainingParticipants_PageIndexChanging"  >
                                    <Columns>
                                        <asp:BoundField  DataField="EMP_NAME" HeaderText=" Employee "/>
                                        <asp:BoundField  DataField="COMP_NAME" HeaderText=" Company "/>
                                        <asp:BoundField  DataField="EPF" HeaderText=" EPF "/>
                                        <asp:BoundField  DataField="DESIGNATION_NAME" HeaderText=" Designation "/>
                                    </Columns>
                                    </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
