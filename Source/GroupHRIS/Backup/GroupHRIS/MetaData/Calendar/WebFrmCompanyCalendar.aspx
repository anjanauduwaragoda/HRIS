<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmCompanyCalendar.aspx.cs"
    Inherits="GroupHRIS.MetaData.Calendar.WebFrmCompanyCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/StyleProfile.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 46px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table class="ProfileMainTable">
            <tr>
                <td align="center">
                    <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: 200px">
                        <ProgressTemplate>
                            <img src="../../Images/ProBar/720.GIF" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Literal ID="LiteralSE1" runat="server"></asp:Literal>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" class="style1">
                    <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
