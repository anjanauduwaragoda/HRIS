<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfileBg.aspx.cs"
    Inherits="GroupHRIS.EmployeeProfile.EditProfileBg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <table class="EmpProfilePhotoTB">
            <tr>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    <a href="Editprofile.aspx">MY PROFILE</a>
                </td>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    <a href="EditProfilePhoto.aspx">MY PHOTO</a>
                </td>
                <td class="EmpProfilePhotoTD" bgcolor="#6699FF">
                    <a href="EditProfileBg.aspx">MY BACKGROUND</a>
                </td>
            </tr>
        </table>
        <table class="EmpProfilePhotoTB">
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Backgroung Image :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
                <td class="EmpProfilePhotoSUBTD" rowspan="9" valign="top">
                    <asp:Image ID="imgme" runat="server" Height="100px" Width="90px" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Image Repeat-X :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Repeat-X" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Image Repeat-Y :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Repeat-Y" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Menu Font Color :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:TextBox ID="txttopiccolor" runat="server" Width="100px" Columns="7" MaxLength="7"></asp:TextBox>
                    <asp:ColorPickerExtender ID="ColorPickerExtender2" TargetControlID="txttopiccolor"
                        PopupButtonID="btnPickColor" PopupPosition="TopRight" SampleControlID="txttopiccolor"
                        Enabled="True" runat="server">
                    </asp:ColorPickerExtender>
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Menu Font Size :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:DropDownList ID="ddlfontsize" runat="server">
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Menu Font Style :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:CheckBox ID="chkfontbold" runat="server" Text="Menu Font Bold " />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:CheckBox ID="chkfontitalic" runat="server" Text="Menu Font Italic" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:CheckBox ID="chkdefault" runat="server" Text="Set as Default" Font-Bold="True"
                        ForeColor="#000099" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:Button ID="btnupdate" runat="server" Text="Update Background Image" Width="200px"
                        OnClick="btnupdate_Click" />
                    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" Width="100px" OnClick="btnrefresh_Click" />
                </td>
            </tr>
        </table>
    </div>
    <table class="EmpProfilePhotoTB">
        <tr>
            <td class="EmpProfilePhotoTD" align="center">
                <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>