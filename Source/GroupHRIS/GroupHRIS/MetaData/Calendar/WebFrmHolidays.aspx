<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="WebFrmHolidays.aspx.cs" EnableEventValidation="false" Inherits="GroupHRIS.MetaData.Calendar.WebFrmHolidays" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../../Styles/StyleCalendar.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3
        {
            text-align : right;
        }
        .style4
        {
            height: 51px;
            
        }
        .style5
        {
            height: 17px;
        }
        .style6
        {
            width: 26%;
            text-align : left;
        }
        .style7
        {
            text-align : right;
            height: 26px;
        }
        .style8
        {
            width: 26%;
            text-align : left;
            height: 26px;
        }
        .style9
        {
            width: 25%;
            text-align : left;
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
        
       <table class="styleMainTb">
        <tr>
            <td  colspan="3">
                <span style="font-weight: 700">Company Holidays</span></td>
        </tr>
        <tr>
            <td class="style3" colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="style3">
                Holiday Type / Description : </td>
            <td class="style6">
                <asp:TextBox ID="txthoidaytype" runat="server" MaxLength="45" Width="300px" ></asp:TextBox>
            </td>
            <td class="styleMainTbTDRight">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txthoidaytype" 
                    ErrorMessage="Holiday Type / Description is required." ForeColor="Red" 
                    ValidationGroup="vgHolidayGroup">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style3">
                Date Type :</td>
            <td class="style6">
                <asp:TextBox ID="txtdatetype" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
            </td>
            <td class="styleMainTbTDRight">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtdatetype" ErrorMessage="Date Type Code  is required." 
                    ForeColor="Red" ValidationGroup="vgHolidayGroup">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style7">
                Calendar Display Color : </td>
            <td class="style8">
                <asp:TextBox ID="txtcolor" runat="server" Columns="7" MaxLength="7" 
                    Width="100px"></asp:TextBox>
                    <asp:ColorPickerExtender ID="ColorPickerExtender2" 
                    TargetControlID = "txtcolor"  PopupButtonID="btnPickColor"
            PopupPosition="TopRight"
            SampleControlID="txtcolor"           
            Enabled="True"  runat="server">
        </asp:ColorPickerExtender>
            </td>
            <td class="style9">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtcolor" ErrorMessage="Display Color  is required." ForeColor="Red" 
                    ValidationGroup="vgHolidayGroup">*</asp:RequiredFieldValidator>
                </td>
        </tr>
        <tr>
            <td class="style3">
                Status : </td>
            <td class="style6">
                <asp:DropDownList ID="ddlrolestatus" runat="server" Height="20px" Width="115px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
                </td>
            <td class="styleMainTbTDRight">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style6">
                &nbsp;</td>
            <td class="styleMainTbTDRight">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style6">
                 <asp:Button ID="btnsave" runat="server"  Text="Save" 
                     Width="113px" onclick="btnsave_Click" ValidationGroup="vgHolidayGroup" />
                 <asp:Button ID="btnclear" runat="server"  Text="Clear" 
                     Width="113px" onclick="btnclear_Click" />
            </td>
            <td class="styleMainTbTDRight">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style4" align="left" colspan="3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                    ValidationGroup="vgHolidayGroup" Height="39px" Width="517px" />
            </td>
        </tr>
        <tr>
            <td class="style4" align="center" colspan="3">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style5" align="center" colspan="3">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True"  PageSize="12" 
                        onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged"    >
                    <PagerSettings PageButtonCount="2" Mode="NumericFirstLast" />
                </asp:GridView>
            </td>
        </tr>
        </table>
</asp:Content>

