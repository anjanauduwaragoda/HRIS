<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmUploadPhoto.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmUploadPhoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 285px;
        }
    </style>
    <script type="text/javascript">
        function closeWindow() {
            var dataCaptured = "1";
            window.opener.getValuefromChild(dataCaptured);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<table border="0" cellpadding="0" cellspacing="0">
            <tr id="uploaderRow" runat="server">
                
                <td class="style1">
                    <asp:FileUpload ID="fuPhoto" ClientIDMode="Static" runat="server" Width="170px" Style="margin-bottom: 5px;"
                        accept=".png,.jpg,.jpeg" onchange="this.form.submit();" />
                    &nbsp;(500 Kb max)
                    <br />
                    <input id="btnUploadFile" type="button" value="Upload File" />
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:Image ID="imgPhoto" runat="server" Height="120px" Width="111px" ImageAlign="Left"
                        ImageUrl="../Images/Add_Person.png" />
                    <asp:ImageButton ID="imgRemoveImage" runat="server" ImageUrl="../Images/close_button.png"
                        Height="18px" Width="20px" OnClick="lbRemoveImage_Click" ToolTip="Remove Photo" />
                    
                </td>
            </tr>
            <tr>
                <td class="style1">
                    
                    <asp:Label ID="lblErrorMsgPhoto" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table> --%>
        <table width="450px" style="background-color: #f2f3f4;">
            <tr>
                <td colspan="2" align="center" style="height: 45px; background-color: #5dade2; font-weight: 700;">
                    <font size="6" color="white" >Upload Images </font>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <br />
                    <asp:FileUpload ID="fuPhoto" ClientIDMode="Static" runat="server" Width="200px" Style="margin-bottom: 5px;"
                        accept=".png,.jpg,.jpeg" onchange="this.form.submit();" />
                    &nbsp;&nbsp;(500 Kb max)
                    <br />
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:Image ID="imgPhoto" runat="server" Height="280px" Width="250px" ImageAlign="Left"
                        ImageUrl="../Images/Add_Person.png" />
                    <asp:ImageButton ID="imgRemoveImage" runat="server" ImageUrl="../Images/close_button.png"
                        Height="18px" Width="20px" OnClick="lbRemoveImage_Click" ToolTip="Remove Photo" />
                    <br />
                </td>
                <td valign="bottom" align="left" style="padding-left: 10px;">
                    <%--<input id="btnUploadFile" type="button" value="Upload File" />--%>
                    <asp:Button ID="btnUploadFile" runat="server" Text="Upload File" 
                        OnClientClick="closeWindow()" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrorMsgPhoto" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
