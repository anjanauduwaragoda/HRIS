<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="TainingReportsGenerator.aspx.cs" Inherits="GroupHRIS.Reports.ReportFilter.TainingReportsGenerator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .title
        {
            text-align: right;
            height: 25px;
            vertical-align: middle;
        }
    </style>

    <script type="text/javascript">
        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfEmployeeId" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />

    <%--    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Training Reports Generator</span>
    <hr />
    <br />
    <asp:HiddenField ID="hfReportCode" runat="server" ClientIDMode="Static" />
    <table width="100%">
        <tr>
            <td style="min-width: 50%">
                <table width="90%">
                    <tr>
                        <td class="title">
                            Company :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompany" runat="server" Width="250px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>    
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Department :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="250px" AutoPostBack="true" 
                                onselectedindexchanged="ddlDepartment_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                        <td>    
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Division :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDivision" runat="server" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>    
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Branch :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>    
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Financial Year :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFinancialYear" runat="server" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>    
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Employee :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeID" runat="server" Width="244px" ReadOnly="true">
                            </asp:TextBox>
                        </td>
                        <td>
                            <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../../Employee/webFrmEmployeeSearch.aspx','Search','txtRecPerson')"
                            id="imgSearch1" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="min-width: 50%; vertical-align: top;">
                <table width="95%">
                    <tr>
                        <td class="title">
                            Department Head Status :
                        </td>
                        <td>
                            &nbsp; &nbsp; Recommended
                        </td>
                        <td>
                            <asp:RadioButton ID="rbDeptHeadRecommend" runat="server" GroupName="deptHeadStatus" />
                        </td>
                        <td>
                            Rejected
                        </td>
                        <td>
                            <asp:RadioButton ID="rbDeptHeadReject" runat="server" GroupName="deptHeadStatus" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            CEO Status :
                        </td>
                        <td>
                            &nbsp; &nbsp; Recommended
                        </td>
                        <td>
                            <asp:RadioButton ID="rbCEORecommend" runat="server" GroupName="ceoStatus" />
                        </td>
                        <td>
                            Rejected
                        </td>
                        <td>
                            <asp:RadioButton ID="rbCEOReject" runat="server" GroupName="ceoStatus" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            Training Status :
                        </td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlTrainingStatus" runat="server" Width="140px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            From :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtFromDate" runat="server" Width="134px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            To :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtToDate" runat="server" Width="134px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></center>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" 
                        Width="120px" onclick="btnGenerate_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear Filters" Width="120px" />
                </center>
            </td>
        </tr>
    </table>
</asp:Content>
