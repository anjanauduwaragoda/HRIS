<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="EmployeeManagement.aspx.cs" Inherits="GroupHRIS.EmployeeProfile.EmployeeManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function openpopupSecondary(file, window) {
            childWindow = open(file, window, 'resizable=no,width=850,height=450,scrollbars=yes,top=150,left=250,status=yes');
        }

        function openpopupTransfer(file, window) {
            childWindow = open(file, window, 'resizable=no,width=900,height=450,scrollbars=yes,top=150,left=220,status=yes');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="ProfileMainTable">
        <tr>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/Secondary.png"
                    onclick="openpopupSecondary('/EmployeeProfile/SecondaryEducation.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/Higher.png" onclick="openpopupSecondary('/EmployeeProfile/HigherEducation.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/work.png" onclick="openpopupSecondary('/EmployeeProfile/PreviousEmployment.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/Transfers.png"
                    onclick="openpopupTransfer('/EmployeeProfile/EmployeeTransfers.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/Secondments .png"
                    onclick="openpopupSecondary('/EmployeeProfile/EmployeeSecondments.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/roster.png" onclick="openpopupSecondary('/EmployeeProfile/EmployeeRosters.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/Salary.png" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img alt="" src="../Images/HRISMain/attendance.png"
                    onclick="openpopupSecondary('/EmployeeProfile/EmployeeSummary.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <table class="ProfileMainTable" style="border-color: Blue; border-width: 1px; border-style: double">
        <tr>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Full Name :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblfullname" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Company :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblcompany" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Last Name :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbllastname" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Employee Id :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblemployeeid" runat="server" Font-Bold="True" ForeColor="#3366FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Middle Name :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblmiddlename" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Profit Center :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblprofitcenter" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                First Name :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblfirstname" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Cost Center :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblcostcenter" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Initials :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblinitials" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Distance to Office :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbldistance" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Division :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbldivision" runat="server" Font-Bold="True" ForeColor="#33CC33"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                City :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblcity" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Department :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbldepartment" runat="server" Font-Bold="True" ForeColor="#33CC33"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Report To 1 :
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Gender :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblgender" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Report To 2 :
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                NIC :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblnic" runat="server" Font-Bold="True"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Report To 3 :
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Passport No :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblpassport" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Fual Card No :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblfualcardno" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Date of Birth :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbldob" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Mobiel No. Company :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblcompmobile" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Date of Join :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbldoj" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Mobile No Personal :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblmobpersonal" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Religion :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblreligion" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Land Phone :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lbllandphone" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Nationality :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblnationality" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                EPF No :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblepf" runat="server" Font-Bold="True" ForeColor="#3366FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Email :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblemail" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Status :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblstatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                Marital Status :
            </td>
            <td class="ProfileMainTableTDLeft">
                <asp:Label ID="lblmaritalstaus" runat="server"></asp:Label>
            </td>
            <td class="ProfileMainTableTDRight">
                Permenent Address :
            </td>
            <td class="ProfileMainTableTDLeft" rowspan="2" valign="top">
                <asp:Label ID="lblpermenetaddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDRight">
                Current Address :
            </td>
            <td class="ProfileMainTableTDLeft" rowspan="2" valign="top">
                <asp:Label ID="lblcurrentaddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDRight">
                &nbsp;
            </td>
            <td class="ProfileMainTableTDLeft">
                &nbsp;
            </td>
        </tr>
    </table>
    <table class="ProfileMainTable">
        <tr>
            <td class="ProfileMainTable" align="center">
                <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
