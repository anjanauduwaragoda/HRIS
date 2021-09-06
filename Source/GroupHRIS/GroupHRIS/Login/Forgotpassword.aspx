<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgotpassword.aspx.cs" Inherits="GroupHRIS.Login.Forgotpassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="../Images/Common/logo.ico" />
    <title>EAP HRIS</title>
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js"  defer="defer"  type="text/javascript"></script>
    <script type ="text/JavaScript">
        function nicCheck() {
            var f9 = document.getElementById('<%= txtnic.ClientID %>').value;
        if (f9.length <= 9) {
            f9 = parseInt(f9);
        }
        document.getElementById('<%= txtnic.ClientID %>').value = f9;
        if (document.getElementById('<%= txtnic.ClientID %>').value == 'NaN') {
            document.getElementById('<%= txtnic.ClientID %>').value = "";
        }
        if (f9.length >= 10) {
            f9 = parseInt(f9);
            f9 = f9.toString();
            var temp = '';
            for (var i = 0; i < 9; i++) {
                temp = temp + f9[i];
            }
            document.getElementById('<%= txtnic.ClientID %>').value = temp;
            f9 = temp;
        }
        if (f9.length == 9) {
            document.getElementById('<%= txtnic.ClientID %>').value = f9 + 'V';
        }
        }
		</script>
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
                        <td valign="top">
                            <table class="MainBodyTBRightTD">
                                <tr>
                                    <td align="left" >
                                <input id="txtuserid" type="text"  class="inputsLogin" placeholder="User ID" 
                                            RunAt="server" tabindex="0"/></td>
                                     
                                </tr>
                                <tr>
                                    <td align="left" >
                                <input id="txtnic" type="text"  class="inputsLogin" placeholder="NIC" 
                                            RunAt="server" maxlength="10"  onkeyup="nicCheck()"  /></td>
                                     
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                <input id="txtemail" type="text" class="inputsLogin" placeholder="E-Mail" 
                                            RunAt="server"/></td>
                                     
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        &nbsp;</td>
                                     
                                </tr>
                                <tr>
                                    <td align="right" valign="top" style="margin-left: 40px">
                                        <asp:ImageButton ID="lnkimagemain" runat="server" 
                                            ImageUrl="~/Images/Login/gotomain.png" 
                                            PostBackUrl="~/Login/MainLogin.aspx"  />
                                    &nbsp;<asp:ImageButton ID="lnkimgregister" runat="server" 
                                            ImageUrl="~/Images/Login/reset.png" onclick="lnkimgregister_Click"  /> 
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
                                    <td align="left" style="text-align: justify">
                                        <asp:Image ID="Image1" runat="server" 
                                            ImageUrl="~/Images/Login/Sign-Alert-icon.png" />
                                        <strong>&nbsp;For your security, we don&#39;t have access to passwords</strong>. 
                                        Enter above required fields to reset your password&nbsp; and you will receive an 
                                        email with a new password.</td>
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

