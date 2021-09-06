<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmSalarySearch.aspx.cs" Inherits="GroupHRIS.Salary.WebFrmSalarySearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=820,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
    Salary Search 
    <hr />
    <table style="margin:auto;">
        <tr>
            <td>
                Employee ID:
            </td>
            <td>
                <asp:TextBox ID="EmployeeIDTextBox" ClientIDMode="Static" runat="server"></asp:TextBox>
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','EmployeeIDTextBox')" />
            </td>
        </tr>
        <tr>
        <td></td>
        <td> <asp:Button ID="ViewButton" runat="server" Text="Get Salary Details" /> </td>
        </tr>
    </table>
</asp:Content>