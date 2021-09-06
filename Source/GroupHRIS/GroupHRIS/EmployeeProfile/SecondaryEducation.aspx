<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondaryEducation.aspx.cs"
    Inherits="GroupHRIS.EmployeeProfile.SecondaryEducation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Secondary Education</title>
    <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="ProfileMainTable">
            <tr>
                <td class="EmpProfilePhotoTD">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#3366FF" Text=" SECONDARY EDUCATION"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="ProfileMainTable">
            <tr>
                <td class="ProfileMainTableTDLeft"  >
                
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="A/L Examination"
                        Font-Size="Large"></asp:Label>
                
                </td>
                <td class="ProfileMainTableTDLeft">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="O/L Examination"
                        Font-Size="Large"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td class="ProfileMainTableTDLeft" valign="top" >
                
                    <asp:Literal ID="LiteralAL" runat="server"></asp:Literal>
                
                </td>
                <td class="ProfileMainTableTDLeft" valign="top">
                    <asp:Literal ID="LiteralOL" runat="server"></asp:Literal>
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
