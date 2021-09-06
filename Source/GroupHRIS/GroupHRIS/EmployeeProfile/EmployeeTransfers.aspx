<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeTransfers.aspx.cs" Inherits="GroupHRIS.EmployeeProfile.EmployeeTransfers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="ProfileMainTable">
            <tr>
                <td class="EmpProfilePhotoTD">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#3366FF" Text="TRANSFERS"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="ProfileMainTable">
            <tr>
                <td class="ProfileMainTableTDLeft"    >
                
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" >
                
                    <asp:Literal ID="LiteralSE1" runat="server"></asp:Literal>
                
                </td>
            </tr>
            </table>
            <table class="ProfileMainTable">
            <tr>
                <td class="EmpProfilePhotoTD" align="center">
                   <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>

</body>
</html>
