<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfilePhoto.aspx.cs"
    Inherits="GroupHRIS.EmployeeProfile.EditProfilePhoto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    Photo :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
                <td class="EmpProfilePhotoSUBTD" rowspan="7" valign="top">
                    <asp:Image ID="imgme" runat="server" Height="100px" Width="90px" />
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Photo Width (px) :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:TextBox ID="txtphotowidth" runat="server" MaxLength="3" Width="50px" ToolTip="Last Name"
                        ForeColor="#000099"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtphotowidth_FilteredTextBoxExtender" runat="server"
                        TargetControlID="txtphotowidth" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    Photo Height (px) :
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:TextBox ID="txtphotoheight" runat="server" MaxLength="3" Width="50px" ToolTip="Last Name"
                        ForeColor="#000099"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtphotoheight_FilteredTextBoxExtender" runat="server"
                        TargetControlID="txtphotoheight" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="EmpProfilePhotoSUBTDLeft">
                    &nbsp;
                </td>
                <td class="EmpProfilePhotoSUBTD">
                    <asp:Button ID="btnupdate" runat="server" Text="Update Photo" Width="100px" OnClick="btnupdate_Click" />
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
