<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpior.aspx.cs" Inherits="GroupHRIS.Login.SessionExpior" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>EAP HIRS</title>
   <link rel="shortcut icon" href="../Images/Common/logo.ico" />
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js" defer="defer" type="text/javascript"></script>
<style type="text/css">
.signinagainstyle
{
    text-decoration:none;
    color:Yellow;
    font-variant:small-caps;
    font-weight:bold ;
}
a:link {
    text-decoration: none;
}

a:visited {
    text-decoration: none;
}

a:hover {
    text-decoration: none;
}

a:active {
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
<body id = "MainLogOutBody" runat="server" onkeydown = "return (event.keyCode!=13)">

    <form id="form1" runat="server" >

    <table class="HeadingTop" >
    <tr>
    <td  align="left" valign="top" class="styletopleft">
        <img src="../Images/MasterPage/logo.png" alt="" /></td>
    <td  align="left" valign="top"> </td>
    </tr>
    </table>

    <table class="MainBodyTB" >
    <tr>
    <td class="styleTDMiddleGap" align="center">
        <asp:Label ID="lblerror" runat="server" Font-Size="14pt" ForeColor="#66FFFF"></asp:Label>
        <br />
        <a href="MainLogin.aspx"><span id="blink" class="signinagainstyle"> Click here to sign in again</span></a> 
    </td>
    </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td class="MainBodyTBLeftTD" rowspan="2">
                            
                             <div id="slider">
                  
                <img src="../Images/Login/0.jpg" alt="Welcome to E.A.P. HRIS" />
                <img src="../Images/Login/1.jpg" alt="Recruitment Management" />
                <img src="../Images/Login/2.jpg" alt="Employee Management" />
                <img src="../Images/Login/3.jpg" alt="Attendence Management" />
                <img src="../Images/Login/4.jpg" alt="Leave Management" />
                <img src="../Images/Login/5.jpg" alt="Shift Management" />
                <img src="../Images/Login/6.jpg" alt="Payroll Management" />
                        </div>
                            
                            </td>
                        
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
        <td class="FotterLogin" align="center">
            
            <asp:LinkButton ID="lnkbutton" runat="server" Font-Size="8pt" ForeColor="White" Font-Underline="False" >LinkButton</asp:LinkButton>
            <br />
            <br />
            <asp:Label ID="lblcopyright" runat="server" ForeColor="White"></asp:Label>
            
        </td>
        </tr>
    </table>
    
    </form>
</body>
</html>
