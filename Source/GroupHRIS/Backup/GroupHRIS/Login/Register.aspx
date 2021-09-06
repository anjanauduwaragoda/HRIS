<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GroupHRIS.Login.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="../Images/Common/logo.ico" />
    <title>EAP HRIS</title>
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleRegister.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js" defer="defer" type="text/javascript"></script>
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
    
        <asp:Label ID="lblerror" runat="server"></asp:Label>
    
    </td>
    </tr>
        <tr>
            <td align="center">
            <div class="divMainBodyTBInner">
                
                <table class="MainBodyTBInner">
                    <tr>
                        <td class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle" align="left">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="styleRegMainTbTDLeft" align="right">
                            Company : </td>
                        <td class="styleRegMainTbTDMiddle" align="left">
                            <asp:Label ID="lblcompanycode" runat="server" Font-Bold="True" 
                                ForeColor="#990000"></asp:Label>
                        </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="styleRegMainTbTDLeft" align="right">
                            NIC : </td>
                        <td class="styleRegMainTbTDMiddle" align="left">
                            <asp:Label ID="lblnic" runat="server" Font-Bold="True" ForeColor="#3333FF"></asp:Label>
                        </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            E.P.F. No : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:Label ID="lblepfno" runat="server" Font-Bold="True" ForeColor="#00CC00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             Name : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:Label ID="lblname" runat="server" Font-Bold="True" ForeColor="#3333FF"></asp:Label>
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            E-mail : </td>
                        <td  class="styleRegMainTbTDMiddle"  align="left">
                            <asp:Label ID="lblemail" runat="server" Font-Bold="True" ForeColor="#3333FF"></asp:Label>
                         </td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             User ID : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:Label ID="lbluserid" runat="server" Font-Bold="True" ForeColor="#3333FF"></asp:Label>
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            Employee Id : </td>
                        <td  class="styleRegMainTbTDMiddle"  align="left">
                            <asp:Label ID="lblemployeeid" runat="server" Font-Bold="True" 
                                ForeColor="#00CC00"></asp:Label>
                         </td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             New User Id : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:TextBox ID="txtuserid" runat="server" Height="20px" Width="162px" 
                                MaxLength="10"></asp:TextBox>
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right" rowspan="2">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDMiddle"  align="left" rowspan="6" valign="top">
                            <asp:Label ID="lblstsmessage" runat="server" Font-Bold="False" 
                                ForeColor="#FF9900" Text="Label"></asp:Label>
                         </td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             First Name : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:TextBox ID="txtfiratname" runat="server" Height="20px" Width="200px"></asp:TextBox>
                         </td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             Last Name : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:TextBox ID="txtlastname" runat="server" Height="20px" Width="250px" 
                                MaxLength="50"></asp:TextBox>
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             E-mail : </td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            <asp:TextBox ID="txtemail" runat="server" Height="20px" Width="250px" 
                                MaxLength="50"></asp:TextBox>
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                                        <asp:ImageButton ID="lnkimagemain" runat="server" 
                                            ImageUrl="~/Images/Login/gotomain.png" PostBackUrl="~/Login/MainLogin.aspx" />
                                    &nbsp;<asp:ImageButton ID="lnkbtnsendreq" runat="server" 
                                ImageUrl="~/Images/Login/sendrequest.png" onclick="lnkbtnsendreq_Click" />
                         </td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                    </tr>
                    <tr>
                         <td  class="styleRegMainTbTDLeft" align="right">
                             &nbsp;</td>
                        <td class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDLeft" align="right">
                            &nbsp;</td>
                        <td  class="styleRegMainTbTDMiddle"  align="left">
                            &nbsp;</td>
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
