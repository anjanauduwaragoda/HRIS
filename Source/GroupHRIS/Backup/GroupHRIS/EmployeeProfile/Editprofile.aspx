<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editprofile.aspx.cs" Inherits="GroupHRIS.EmployeeProfile.Editprofile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .styleError
        {
            height: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <table class="EmpProfilePhotoTB">
            <tr>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    <a href="Editprofile.aspx" >MY PROFILE</a> 
                </td>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    <a href="EditProfilePhoto.aspx" >MY PHOTO</a> 
                </td>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                <a href="EditProfileBg.aspx" >MY BACKGROUND</a> 
                </td>
            </tr>
        </table>
        <table class="EmpProfilePhotoTB">
            <tr>
                <td valign="top">
                    <table class="heismaininnerTableLeft">
                        <tr>
                            <td class="heismaininnertLeft">
                                First Name
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lblfirstname" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Last Name
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lbllastname" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                User ID
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lbluserid" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                E.P.F. No.
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lblepf" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertbright" valign="top">
                                NIC No.
                            </td>
                            <td class="heismaininnertbright" valign="top">
                                :
                                <asp:Label ID="lblnic" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                First Name
                            </td>
                            <td class="heismaininnertbright">
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="45" Width="250px" ToolTip="First Name"
                                    Height="20px" ForeColor="#0066FF"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Last Name
                            </td>
                            <td class="heismaininnertbright">
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="45" Width="250px" ToolTip="Last Name"
                                    ForeColor="#0066FF"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Date of Birth
                            </td>
                            <td class="heismaininnertbright">
                                <asp:TextBox ID="txtbdate" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                                <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtbdate" Format="yyyy/MM/dd">
                                </asp:CalendarExtender>
                                <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtbdate"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Gender
                            </td>
                            <td class="heismaininnertbright">
                                <asp:DropDownList ID="ddlGender" runat="server" Width="256px" Height="20px">
                                    <asp:ListItem Value="M">Male</asp:ListItem>
                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Marital Status
                            </td>
                            <td class="heismaininnertbright">
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="256px" TabIndex="1">
                                    <asp:ListItem>Married</asp:ListItem>
                                    <asp:ListItem>Unmarried</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Religion
                            </td>
                            <td class="heismaininnertbright">
                                <asp:DropDownList ID="ddlreligion" runat="server" Width="256px" TabIndex="1">
                                    <asp:ListItem>Buddhist</asp:ListItem>
                                    <asp:ListItem>Christian</asp:ListItem>
                                    <asp:ListItem>Hidnu</asp:ListItem>
                                    <asp:ListItem>Islam</asp:ListItem>
                                    <asp:ListItem>Other</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table class="heismaininnerTableRight">
                        <tr>
                            <td class="heismaininnertLeft">
                                Department
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lbldepartment" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Division
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lbldivision" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Cost Center
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lblconstcenter" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Profit Center
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lblprofitcenter" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft">
                                Email
                            </td>
                            <td class="heismaininnertbright">
                                :
                                <asp:Label ID="lblemail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft" valign="top">
                                Type
                            </td>
                            <td class="heismaininnertbright" valign="top">
                                :
                                <asp:Label ID="lblemptype" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft" valign="top">
                                Role
                            </td>
                            <td class="heismaininnertbright" valign="top">
                                :
                                <asp:Label ID="lblemprole" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertLeft" valign="top">
                                Current Address
                            </td>
                            <td class="heismaininnertbright" valign="top">
                                :
                                <asp:Label ID="lblcurrentaddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="heismaininnertbLeft" valign="top">
                                Permanent Address
                            </td>
                            <td class="heismaininnertbright" valign="top">
                                :
                                <asp:Label ID="lblpermenanraddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="styleError">
                    <asp:Button ID="btnupdate" runat="server" Text="Update Profile" Width="100px" OnClick="btnupdate_Click" />
                    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" Width="100px" OnClick="btnrefresh_Click" />
                </td>
            </tr>
        </table>
         <table class="EmpProfilePhotoTB">
            <tr>
                <td class="EmpProfilePhotoTD" align="center"  >
                   <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red" ></asp:Label></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
