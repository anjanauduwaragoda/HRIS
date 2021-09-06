<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainLogin.aspx.cs" Inherits="GroupHRIS.Login.MainLogin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="../Images/Common/logo.ico" />
    <title>EAP HRIS | LOGIN</title>
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


        function ValidateNIC() {
            var nicNumber = document.getElementById('txtnic').value;
            var nicLength = nicNumber.length;
            if (nicLength != '0') {
                //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
            }
            else {
                //document.getElementById('lblMessage').innerHTML = '';
            }

            if (nicLength < 10) {
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                    nicNumber = nicNumber.slice(0, -1);
                    nicLength = nicNumber.length;

                    document.getElementById('txtnic').value = nicNumber;
                    if (nicLength != '0') {
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                    }
                    else {
                        //document.getElementById('lblMessage').innerHTML = '';
                    }
                }
            }

            if (nicLength == '10') {
                document.getElementById('txtnic').setAttribute('maxlength', 12);
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != 'x') && (LastChar != 'X') && (LastChar != 'v') && (LastChar != 'V')) {
                    if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                        nicNumber = nicNumber.slice(0, -1);
                        nicLength = nicNumber.length;
                        document.getElementById('txtnic').value = nicNumber;
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                        document.getElementById('txtnic').setAttribute('maxlength', 12);
                    }
                }
                else {
                    document.getElementById('txtnic').setAttribute('maxlength', 12);
                    LastChar = nicNumber.slice(-1);
                    if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                        document.getElementById('txtnic').setAttribute('maxlength', 10);
                    }
                    else {
                        document.getElementById('txtnic').setAttribute('maxlength', 12);
                    }
                }
            }

            if (nicLength > 10) {
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                    nicNumber = nicNumber.slice(0, -1);
                    nicLength = nicNumber.length;

                    document.getElementById('txtnic').value = nicNumber;
                    if (nicLength != '0') {
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                    }
                    else {
                        //document.getElementById('lblMessage').innerHTML = '';
                    }

                }
            }
        }
		</script>
</head>
<body id = "MainLoginBody" runat="server" onkeydown = "return (event.keyCode!=13)">

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
    
        <asp:Label ID="lblerror" runat="server"></asp:Label>
    
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
                                    <td colspan="2" align="left" >
                                <input id="txtuserid" type="text"  class="inputsLogin" placeholder="User Name"  value = ""
                                            RunAt="server" tabindex="0"/></td>
                                </tr>
                                <tr>
                                    <td align="left" >
                                <input id="txtpasswordlogin" type="password" maxlength="10" class="inputsLogin2" 
                                            placeholder="Password"  value ="" RunAt="server"/></td>
                                    <td valign="top" align="left"  >
                                        <asp:ImageButton ID="lnkimgsignin" runat="server" 
                                            onclick="lnkimgsignin_Click" Height="28px" Width="73px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" >
                                <asp:LinkButton ID="LinkButton1"  runat="server" Font-Bold="True" 
                                    Font-Size="8pt" Font-Underline="False" ForeColor="Black" 
                                            PostBackUrl="~/Login/Forgotpassword.aspx">Forgot Your Password ?</asp:LinkButton>
                                    </td>
                                    <td valign="top" align="left">
                                        </td>
                                </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="MainBodyTBRightTD">
                                <tr>
                                    <td align="left">
                                        New to E.A.P. HRIS ? Sign up  </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                <input id="txtname" type="text" class="inputsLogin" placeholder="First Name" RunAt="server" /></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                <input id="txtnic" clientidmode="Static" type="text" class="inputsLogin" placeholder="NIC No." RunAt="server" 
                                            maxlength="12"  onkeyup="ValidateNIC()" /> </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                <input id="txtpassword" type="password" class="inputsLogin"   title="Maximum Length : 10 Characters" placeholder="Password" 
                                            RunAt="server" maxlength="10" /></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:ImageButton ID="lnkimgregister" runat="server" 
                                            onclick="lnkimgregister_Click" Height="35px" Width="200px" />
                                    </td>
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
