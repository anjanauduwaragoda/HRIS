<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="WebFormEmpleeProfile.aspx.cs" Inherits="GroupHRIS.Employee.WebFormEmpleeProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
//        function openpopupSecondary(file, window) {
//            childWindow = open(file, window, 'resizable=no,width=850,height=450,scrollbars=yes,top=150,left=250,status=yes');
//        }

//        function openpopupTransfer(file, window) {
//            childWindow = open(file, window, 'resizable=no,width=900,height=450,scrollbars=yes,top=150,left=220,status=yes');
//        }

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//            document.getElementById("hfCaller").value = ctlName;
//        }

//        function getValueFromChild(sRetVal) {
//            var ctl = document.getElementById("hfCaller").value;
//            document.getElementById(ctl).value = sRetVal;
//            DoPostBack();
//        }

//        function DoPostBack() {
//            __doPostBack("txtEmployeeID", "TextChanged");
//        }

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

        function DoPostBack() {
            __doPostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table class="ProfileMainTable">
        <tr>
            <td class="ProfileMainTableTDCenter">
                 <img src="../Images/HRISMain/Secondary.png" alt="" onclick="openpopupSecondary('/EmployeeProfile/SecondaryEducation.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
                  
            </td>
            <td class="ProfileMainTableTDCenter">
                <img  src="../Images/HRISMain/Higher.png"  alt="" onclick="openpopupSecondary('/EmployeeProfile/HigherEducation.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter" colspan="2">
                <img src="../Images/HRISMain/work.png"  alt="" onclick="openpopupSecondary('/EmployeeProfile/PreviousEmployment.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDCenter">
                <img src="../Images/HRISMain/Transfers.png"  alt=""
                    onclick="openpopupTransfer('/EmployeeProfile/EmployeeTransfers.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img src="../Images/HRISMain/Secondments .png"  alt=""
                    onclick="openpopupSecondary('/EmployeeProfile/EmployeeSecondments.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td class="ProfileMainTableTDCenter" colspan="2"> 
                <img src="../Images/HRISMain/roster.png"  alt="" onclick="openpopupSecondary('/EmployeeProfile/EmployeeRosters.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
        </tr>
        <tr>
            <td class="ProfileMainTableTDCenter">
                <img src="../Images/HRISMain/Salary.png"  alt=""  />
            </td>
            <td class="ProfileMainTableTDCenter">
                <img src="../Images/HRISMain/attendance.png" alt=""
                    onclick="openpopupSecondary('/EmployeeProfile/EmployeeSummary.aspx?mEmpProfileID=<%= mEmpProfileID %>')" />
            </td>
            <td  align="right">
               <asp:TextBox ID="txtEmployeeID" runat="server" Width="100px" 
                    ClientIDMode="Static" style="height: 22px" AutoPostBack="True" ReadOnly="true" 
                    ontextchanged="txtEmployeeID_TextChanged"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
            <td  align="left">
            <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" /></td>
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
                &nbsp;</td>
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
                Known Name :
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
    <asp:HiddenField ID="hfcallervalue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
</asp:Content>
