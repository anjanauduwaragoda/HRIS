<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRequestSearch.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.frmRequestSearch" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Request Search</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 1100;
            _height = window.screen.availHeight - 20;

            //window.moveTo(20, 20);
            window.resizeTo(_width, _height)
            window.focus();
        }

        function sendValueToParent_() {
            var sRetVal = document.getElementById("txtRequestID").value;

            window.opener.getValueFromChild(sRetVal);

            window.close();
        }

        function sendValueToParent() {
            var sTrId = document.getElementById("txtRequestID").value;
            //            alert(sTrId);
            window.opener.getValueFromChild(sTrId);

            window.close();
        }
    </script>
    <style type="text/css">
        .cell1
        {
            width: 65px;
            text-align: right;
        }
        .cell2
        {
            width: 220px;
            text-align: left;
        }
        .cell3
        {
            width: 85px;
            text-align: right;
        }
        .cell4
        {
            width: 220px;
            text-align: left;
        }
        .cell5
        {
            width: 90px;
            text-align: right;
        }
        .cell6
        {
            width: 220px;
            text-align: left;
        }
        .cell7
        {
            width: 40px;
            text-align: center;
        }
    </style>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div>
        <span>Training Request Search</span>
        <hr />
        <br />
        <table>
            <tr>
                <td class="cell1">
                    <asp:Label ID="lblCompany" runat="server" Text="Company : "></asp:Label>
                </td>
                <td class="cell2">
                    <asp:DropDownList ID="ddlCompany" runat="server" Width="220px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="cell3">
                    <asp:Label ID="Label1" runat="server" Text="Department : "></asp:Label>
                </td>
                <td class="cell4">
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="220px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="cell5">
                    <asp:Label ID="Label2" runat="server" Text="Division : "></asp:Label>
                </td>
                <td class="cell6">
                    <asp:DropDownList ID="ddlDivision" runat="server" Width="220px">
                    </asp:DropDownList>
                </td>
                <td class="cell7">
                    <asp:ImageButton ID="iBtnReset" runat="server" Height="30px" ImageUrl="~/Images/refresh1.png"
                        Width="30px" OnClick="iBtnReset_Click" />
                </td>
            </tr>
            <tr>
                <td class="cell1">
                    <asp:Label ID="Label3" runat="server" Text="Branch : "></asp:Label>
                </td>
                <td class="cell2">
                    <asp:DropDownList ID="ddlBranch" runat="server" Width="220px">
                    </asp:DropDownList>
                </td>
                <td class="cell3">
                    <asp:Label ID="Label4" runat="server" Text="Category : "></asp:Label>
                </td>
                <td class="cell4">
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="220px">
                    </asp:DropDownList>
                </td>
                <td class="cell5">
                    <asp:Label ID="Label5" runat="server" Text="Request Type : "></asp:Label>
                </td>
                <td class="cell6">
                    <asp:DropDownList ID="ddlRequestType" runat="server" Width="220px">
                    </asp:DropDownList>
                </td>
                <td class="cell7">
                    <asp:ImageButton ID="iBtnSearch" runat="server" Height="30px" ImageUrl="~/Images/Search.png"
                        Width="30px" OnClick="iBtnSearch_Click" />
                </td>
            </tr>
            <td colspan="7">
                <asp:CheckBox ID="chkMultipleReq" ClientIDMode="Static" 
                    Text="Select Multiple Requests" runat="server" AutoPostBack="true"
                    oncheckedchanged="chkMultipleReq_CheckedChanged" />
            </td>
            <td colspan="7" style="text-align: center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </table>
    </div>
    <br />
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblRequest" runat="server" Text="Training Request Id" Width="150px"
                        Font-Bold="True" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtRequestID" runat="server" Width="100px" BorderStyle="None" Font-Bold="True"
                        ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                    &nbsp; &nbsp;
                    <asp:TextBox ID="txtDescription" runat="server" Width="100px" BorderStyle="None" Font-Bold="True"
                        ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" OnClientClick="sendValueToParent()"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdTrainingRequests" runat="server" Style="width: 100%" AutoGenerateColumns="false"
                        AllowPaging="True" PageSize="7" 
                        onpageindexchanging="grdTrainingRequests_PageIndexChanging" 
                        onrowdatabound="grdTrainingRequests_RowDataBound" 
                        onselectedindexchanged="grdTrainingRequests_SelectedIndexChanged" >
                        <Columns>
                            <asp:BoundField DataField="REQUEST_ID" HeaderText="Request ID" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="DESCRIPTION_OF_TRAINING" HeaderText="Description" />
                            <asp:BoundField DataField="SKILLS_EXPECTED" HeaderText="Skills Expected" />
                            <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                            <asp:BoundField DataField="NUMBER_OF_PARTICIPANTS" HeaderText="No. of Participants" />
                            <asp:TemplateField HeaderText="Select Multiple" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkInclude" runat="server" AutoPostBack="true" oncheckedchanged="chkInclude_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>