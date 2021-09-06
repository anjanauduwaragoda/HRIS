
﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmTrainerSearch.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainerSearch" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trainer Search</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {

            _width = 950;
            _height = window.screen.availHeight - 20;

            window.resizeTo(_width, _height)
            window.focus();
        }

        function sendValueToParent_() {
            var sRetVal = document.getElementById("txtTrId").value;
            
            window.opener.getValueFromChild(sRetVal);

            window.close();
        }

        function sendValueToParent() {
            var sTrId = document.getElementById("txtTrId").value;
            var sRetVal1 = document.getElementById("hfTrainerName").value;
            window.opener.getValueFromChild(sTrId, sRetVal1);
            window.close();
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 236px;
        }
        .style2
        {
            text-align: left;
        }
        .style3
        {
            width: 109px;
            text-align: right;
        }
        .style4
        {
            width: 205px;
        }
        .style5
        {
            width: 7px;
        }
        .style6
        {
            width: 8px;
        }
        
        table.GridView tr:hover
        {
            background-color:#a9a9a9;
            }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
        <asp:HiddenField ID="hfTrainerName" runat="server" />
        <table style="margin: auto;">
            <tr>
                <td class="style2" style="text-align:right;">
                    Name
                </td>
                <td class="style6">
                    :
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtName" Width="200px" runat="server"></asp:TextBox>
                </td>
                <td class="style3">
                    NIC
                </td>
                <td class="style5">
                    :
                </td>
                <td class="style4">
                    <asp:TextBox ID="txtNIC" Width="200px" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Mobile Number
                </td>
                <td class="style6">
                    :
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtContactNumber" Width="200px" runat="server"></asp:TextBox>
                </td>
                <td class="style3">
                    Is External Trainer
                </td>
                <td class="style5">
                   :
                </td>
                <td class="style4">
                    <asp:CheckBox ID="chkIsExternal" runat="server" />
                </td>
                <td>
                    <asp:ImageButton ID="imgbtnSearch" runat="server" 
                        ImageUrl="~/Images/Common/user-search-icon.png" onclick="imgbtnSearch_Click"  />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="4">
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:TextBox ID="txtTrId" style="text-align: left;" runat="server" Width="200px" BorderStyle="None" Font-Bold="True"
                                ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                            <br />
                            <asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" 
                                Visible="False" OnClientClick="sendValueToParent()" onclick="btnSelect_Click"/>
                </td>
                <td class="style5">
                </td>
                <td class="style4">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        Trainers
        <hr />
        <table style="margin:auto;">
            <tr>
                <td>
                    <asp:GridView ID="grdvTrainers" AutoGenerateColumns="false" CssClass="GridView" Width="700px" AllowPaging="true" PageSize="10" runat="server" onpageindexchanging="grdvTrainers_PageIndexChanging" onrowdatabound="grdvTrainers_RowDataBound" onselectedindexchanged="grdvTrainers_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="TRAINER_ID" HeaderText="Trainer ID" />
                            <asp:BoundField DataField="NAME_WITH_INITIALS" HeaderText=" Name With Initials " />
                            <asp:BoundField DataField="FULL_NAME" HeaderText="Full Name" />
                            <asp:BoundField DataField="NIC" HeaderText="NIC" />
                            <asp:BoundField DataField="CONTACT_MOBILE" HeaderText="Mobile Number" />
                            <asp:BoundField DataField="CONTACT_LAND" HeaderText="Land Number" />
                            <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
