<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Webfrmpassword.aspx.cs" Inherits="GroupHRIS.Login.Webfrmpassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="../Images/Common/logo.ico" />
    <title>EAP HRIS</title>
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js" defer="defer" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            height: 21px;
        }
    </style>
</head>
<body onkeydown = "return (event.keyCode!=13)" >

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
    
        <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="White"></asp:Label>
    
    </td>
    </tr>
        <tr>
            <td align="center">
            <div class="divMainBodyTBInner">
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
                        <td>
                            <table class="MainBodyTBRightTD">
                                <tr>
                                    <td align="left" >
                                        <asp:Label ID="lbluserid" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" >
                                        &nbsp;</td>
                                     
                                </tr>
                                <tr>
                                    <td align="left" >
                                <input id="txtpassword1" type="password"  class="inputsLogin" placeholder="New Password" 
                                            RunAt="server" maxlength="10"/></td>
                                     
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                <input id="txtpassword2" type="password" class="inputsLogin" placeholder="Re-Type Password" 
                                            RunAt="server" maxlength="10"/></td>
                                     
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:ImageButton ID="lnkimagemain" runat="server" 
                                            ImageUrl="~/Images/Login/gotomain.png" 
                                            PostBackUrl="~/Login/MainLogin.aspx" />
                                    &nbsp;<asp:ImageButton ID="lnkimgregister" runat="server" 
                                            ImageUrl="~/Images/Login/LoginRegister.png" 
                                            onclick="lnkimgregister_Click" /> 
                                        &nbsp;<asp:ImageButton ID="lnkimgcancel" runat="server" 
                                            ImageUrl="~/Images/Login/logincancel.png" onclick="lnkimgcancel_Click" />
                                    </td>
                                     
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="MainBodyTBRightTD">
                                <tr>
                                    <td align="left" class="style1"> </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
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
