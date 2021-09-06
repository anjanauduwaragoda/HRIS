<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Changepassword.aspx.cs"
    Inherits="GroupHRIS.Changepassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .class0
        {
            background-color: Red;
        }
        .class1
        {
            background-color: Purple;
        }
        .class2
        {
            background-color: Orange;
        }
        .class3
        {
            background-color: Aqua;
        }
        .class4
        {
            background-color: Lime;
        }
        .class5
        {
            background-color: Green;
        }
          
        #txtpassword1
        {
            width: 270px;
            height:30px;
        }
        #txtpassword2
        {
            width: 270px;
            height:30px;
        }
          
    </style>
    <script type="text/javascript">

        function returnPasswordStrength(password) {

            var msg = new Array("Very Weak", "Weak", "Better", "Medium", "Strong", "Strongest");
            var strength = 0;

            // If password has more than 6 characters add one to strength
            if (password.length > 6) strength++;

            //if password has both lower and uppercase characters add one to strength
            if ((password.match(/[a-z]/)) && (password.match(/[A-Z]/))) strength++;

            //if password has at least one numeral add one to strength
            if (password.match(/\d+/)) strength++;

            //if password has at least one special character add one to strength
            if (password.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,),'\s']/)) strength++;

            //If password has more than 9 characters add one to strength
            if (password.length > 9) strength++;

            document.getElementById("lblstreanght").innerHTML = msg[strength];
            // To chnage the bg color
            document.getElementById("pa1").className = "class" + strength;

        }
 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table class="heismainininnerTableLeft" style="width: 800px">
        <tr>
            <td class="heismainpasswordleft" rowspan="7" align="center" 
                style="width: 180px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/HRISMain/user_login.png"
                    Width="184px" />
            </td>
            <td class="heismainpasswordright">
                New Password
            </td>
            <td class="heismainpasswordright">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="heismainpasswordright" align="left" rowspan="2">
            <input id="txtpassword1" type="password"  runat="server"  maxlength="10" onkeyup="returnPasswordStrength(this.value)" /><br />
                (Maximum Length : 10 Characters)<br />
                <br />
            </td>
            <td class="heismainpasswordright" style="width: 150px">
                <asp:Label ID="lblstreanght" class="default" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="7pt" ForeColor="White"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="heismainpasswordright" style="width: 150px">
                <asp:Label ID="pa1" runat="server" class="default" Height="15px" 
        Width="100px" Font-Names="Verdana" Font-Size="7pt" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="heismainpasswordright">
                Re-Type New Password
            </td>
            <td class="heismainpasswordright">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="heismainpasswordright" align="left">
                                <input id="txtpassword2" type="password" runat="server" maxlength="10"/><br />
                                (Maximum Length : 10 Characters)<br />
                                <br />
            </td>
            <td class="heismainpasswordright">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="heismainpasswordleft">
                &nbsp;
            </td>
            <td class="heismainpasswordleft">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="heismainpasswordright" align="left">
                <asp:ImageButton ID="imgbtnupdate" runat="server" ImageUrl="~/Images/Login/Changepassword.png"
                    OnClick="imgbtnupdate_Click" />
                &nbsp;<asp:ImageButton ID="imgbtncancel" runat="server" ImageUrl="~/Images/Login/logincancel.png"
                    OnClick="imgbtncancel_Click" />
            </td>
            <td class="heismainpasswordright">
                &nbsp;</td>
        </tr>
    </table>
    <table class="heismainininnerTableLeft">
        <tr>
            <td style="width: 200px" colspan="2">
                &nbsp;
            </td>
            <td style="width: 600px">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="height: 30px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#000066" Text="Creating a strong password"></asp:Label>
            </td>
            <td>
    <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 30px">
                To keep your HRIS user account safe, here are few tips on how to create a strong
                password:</td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                <strong>Use a different password</strong> for each of your important accounts, 
                like your email and online banking accounts.
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image6" runat="server" 
                    ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                Do not use <strong>your user name, real name, company name or personal 
                information</strong></td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image5" runat="server" 
                    ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                At least <strong>eight characters long</strong></td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image8" runat="server" 
                    ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                Must different from <strong>previous passwords</strong></td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                <strong>Use a mix of letters, numbers, and symbols</strong> in your password.</td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/HRISMain/LinkIcoChild.gif" />
            </td>
            <td colspan="2">
                <strong>Try using a phrase that only you know</strong>. For example, “My Friends 
                Tom and Jerry send me a funny email once a day” and then use numbers and letters
                to recreate it. “MfT&amp;Jsmafe1@d” is a password with lots of variations.</td>
        </tr>
    </table>
    </form>
</body>
</html>
