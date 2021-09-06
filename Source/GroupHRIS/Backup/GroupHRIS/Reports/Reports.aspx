<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs" Inherits="GroupHRIS.Reports.Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("txtemployee").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("txtemployee").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function MyPopUpWin(url, width, height) {
            var leftPosition, topPosition;
            //Allow for borders.
            leftPosition = (window.screen.width / 2) - ((width / 2) + 10);
            //Allow for title and status bars.
            topPosition = (window.screen.height / 2) - ((height / 2) + 50);
            //Open the window.
            window.open(url, "Report Center",
    "status=no,height=" + height + ",width=" + width + ",resizable=yes,left="
    + leftPosition + ",top=" + topPosition + ",screenX=" + leftPosition + ",screenY="
    + topPosition + ",toolbar=no,menubar=no,scrollbars=no,location=no,directories=no");
        }

        function openpopupreport(file, window) {
            childWindow = open(file, window, 'resizable=no,width=900,height=500,scrollbars=yes,top=150,left=250,status=yes');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td align="center" colspan="3">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="stylestbTbTD3">
                &nbsp;</td>
            <td class="stylestbTbTD2">
                &nbsp;</td>
            <td class="styleMainTbCenterTD">
                &nbsp;</td>
        </tr>
         
        
        <tr>
            <td class="stylestbTbTD3">
                <span style="font-weight: 700">Report Center</span>
            </td>
            <td class="stylestbTbTD2">
            </td>
            <td class="styleMainTbCenterTD">
            </td>
        </tr>
         
        
        <tr>
            <td align="center" colspan="3">
                &nbsp;</td>
        </tr>
         
        
        <tr>
            <td align="center" colspan="3">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                    BorderColor="#0099FF" BorderStyle="Groove" BorderWidth="2px" CellPadding="3"
                    CellSpacing="1" ForeColor="Black" OnRowDataBound="GridView1_RowDataBound">
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="#0099FF" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#CCCCCC" BorderColor="#0099FF" BorderWidth="1px" ForeColor="Black"
                        HorizontalAlign="Left" />
                    <RowStyle BackColor="White" BorderColor="#3366FF" BorderWidth="1px" />
                    <SelectedRowStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="REPCODE" HeaderText="Report Code" />
                        <asp:BoundField DataField="DESCRIP" HeaderText="Report Name" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlkreport" runat="server" Width="120px" Text="Generate Report">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
