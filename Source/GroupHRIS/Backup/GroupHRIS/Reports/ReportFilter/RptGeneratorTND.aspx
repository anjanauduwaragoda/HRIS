<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="RptGeneratorTND.aspx.cs" Inherits="GroupHRIS.Reports.ReportFilter.RptGeneratorTND" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../../Styles/StyleReports.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    var txb;
    function openLOVWindow(file, window, ctlName) {
        txb = ctlName;
        childWindow = open(file, window, 'resizable=no,width=1366,height=768,scrollbars=yes,top=50,left=200,status=yes');
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
        

    <div>
        <table style="margin:auto;">
            <tr>
                <td align="center" colspan="3">
                    
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Report Code :</td>
                <td>
                    <asp:Label ID="lblrepname" runat="server" Font-Bold="True"></asp:Label>
                </td>
                <td class="reportRightTD">
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Training :
                </td>
                <td>
                    <asp:TextBox ID="txtTrainingID" runat="server" ClientIDMode="Static" ReadOnly="true" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTrainingID')" id="imgEditSearch" />
                    <br />
                    <asp:Label ID="lblTrainingName" runat="server" style="font-weight: 700" ></asp:Label>
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                </td>
                <td>
                    <asp:HiddenField ID="hfRepID" runat="server" />
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    &nbsp;
                </td>
                <td>
                    <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                        <ProgressTemplate>
                            <img src="/Images/ProBar/720.GIF" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnGenerateReport" runat="server" Text="Run Report" 
                        style="height: 26px" onclick="btnGenerateReport_Click" />
                    <asp:Button ID="btnClear" runat="server"  
                        Text="Clear" style="height: 26px" Width="100" onclick="btnClear_Click" />
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
