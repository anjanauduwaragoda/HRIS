<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmDataUpload.aspx.cs" Inherits="GroupHRIS.Employee.WebFrmDataUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 25%;
            text-align: right;
            table-layout: fixed;
            height: 21px;
        }
        .style2
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTb">
                <span style="font-weight: 700">HRIS Employee Data Upload</span>
            </td>
        </tr>
        <tr>
            <td class="styleMainTb">
                <hr />
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                Company :
            </td>
            <td class="attensummaryTDRight">
                <asp:DropDownList ID="dpCompID" runat="server" Width="250px" OnSelectedIndexChanged="dpCompID_SelectedIndexChanged"
                    Height="22px">
                </asp:DropDownList>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;Select File :
            </td>
            <td class="attensummaryTDRight">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTDRight">
                <asp:Label ID="Label1" runat="server" Font-Size="8pt" ForeColor="Red" Text="Rename excel sheet as &quot;Sheet1&quot; before upload data"></asp:Label>
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
            </td>
            <td align="left" class="style2">
                <asp:HyperLink ID="hypExcelTemplate" NavigateUrl="~/DocumentDownload/FileDownloader.ashx" runat="server">Download Excel Template</asp:HyperLink>
            </td>
            <td class="style1">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTDRight">
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="attensummaryTD">
                &nbsp;
            </td>
            <td class="attensummaryTDRight">
                <asp:Button ID="btnloaddata" runat="server" Text="Upload Data" Width="100px" 
                    OnClick="btnloaddata_Click" style="height: 26px" />
                <asp:Button ID="btnupload" runat="server" Text="Process" Width="80px" OnClick="btnupload_Click" />
                <asp:Button ID="btnclose" runat="server" OnClick="btnclose_Click" Text="Clear" Width="80px" />
            </td>
            <td class="attensummaryTD">
                &nbsp;
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
