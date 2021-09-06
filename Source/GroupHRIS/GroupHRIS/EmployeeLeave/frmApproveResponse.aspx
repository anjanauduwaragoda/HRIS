<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmApproveResponse.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.frmApproveResponse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js"  defer="defer"  type="text/javascript"></script>
    <style type="text/css">
        .signinagainstyle
        {
            text-decoration: none;
            color: Yellow;
            font-variant: small-caps;
            font-weight: bold;
        }
        a:link
        {
            text-decoration: none;
        }
        
        a:visited
        {
            text-decoration: none;
        }
        
        a:hover
        {
            text-decoration: none;
        }
        
        a:active
        {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        window.onload = blinkOn;
        function blinkOn() {
            document.getElementById("blink").style.color = "#00CCFF"
            setTimeout("blinkOff()", 500)
        }

        function blinkOff() {
            document.getElementById("blink").style.color = ""
            setTimeout("blinkOn()", 500)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table class="HeadingTop">
        <tr>
            <td align="left" valign="top" class="styletopleft">
                <img src="../Images/MasterPage/logo.png" alt="" />
            </td>
        </tr>
    </table>
    <table class="MainBodyTB">
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="14pt" ForeColor="White" Text="Thank You"></asp:Label><br />
            </td>
        </tr>        
        <tr>
            <td class="FotterLogin" align="center">
                <br />
                <asp:Label ID="lblcopyright" runat="server" ForeColor="White"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
