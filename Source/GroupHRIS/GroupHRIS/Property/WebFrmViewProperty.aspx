<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewProperty.aspx.cs" EnableEventValidation="false" 
    Inherits="GroupHRIS.Property.WebFrmViewProperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Property Search</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(60, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }

        function dosposeWndow() {

            window.close();
        }

        function fnclose() {
            window.opener.DoPostBack();
//            window.close();
        }

        function sendValueToParent() {
            try {
                var x = 'true';
                window.opener.displayData(x);
                //window.close();
            }
            catch (err) {
                alert(err.Message);
            }
        }

        
    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()" onunload="fnclose()">
    <form id="form1" runat="server">
    <span style="font-weight: 700">View Benifit Details</span>
    <hr />
    <br />
    <table style="margin: auto; min-width: 700;"><tr><td style="text-align: right;">
    <asp:GridView ID="grdViewBenefits" runat="server" Style="margin: auto;text-align:left" AutoGenerateColumns="false"
        AllowPaging="true">
        <Columns>
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Benefit Name " />
            <asp:BoundField DataField="REFERENCE_NO" HeaderText=" Referemce No " />
            <asp:BoundField DataField="MODEL" HeaderText=" Model " />
            <asp:BoundField DataField="SERIAL_NO" HeaderText=" Serial No " />
            <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property id " HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Property Type id" HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
            <asp:TemplateField HeaderText="IS_EXCLUDE">
                <ItemTemplate>
                    <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView></td></tr></table>
    
    <table style="margin: auto; min-width: 700;">
        <tr>
            <td style="text-align: right;">
                <asp:Button ID="btnExclude" runat="server" Text="Exclude From Cart" OnClick="btnExclude_Click" />
            </td>
        </tr>
    </table>
    <table id="tblBenefits" style="margin: auto; min-width: 700;" runat="server">
    <tr><td></td><td><asp:Label ID="lblExclude" runat="server" Text="Remove Benefites : "></asp:Label></td></tr>
        <tr>
            <td>
               
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="text-align: center;" colspan = "2">
                <asp:GridView ID="gvemployeeProperty" Style="margin: auto;text-align:left" runat="server" AutoGenerateColumns="False"
                    AllowPaging="true" OnRowDataBound="gvemployeeProperty_RowDataBound" 
                    onselectedindexchanged="gvemployeeProperty_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Name " />
                        <asp:BoundField DataField="ASSIGNED_DATE" HeaderText=" Assign Date " />
                        <asp:BoundField DataField="RETURNED_DATE" HeaderText=" Return Date " />
                        <asp:BoundField DataField="CLEARANCE_MAIL" HeaderText=" Clearance mail " />
                        <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
                        <asp:BoundField DataField="PROPERTY_STATUS" HeaderText=" Status " />
                        <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Property Type Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="LINE_ID" HeaderText=" Line Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td></td><td class="styleMainTbLeftTD">
                Benefit Name :
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td><td class="style1">
                Assign Date :
            </td>
            <td class="style2">
                <asp:TextBox ID="txtDate" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td><td class="styleMainTbLeftTD">
                Clearance e-mail :
            </td>
            <td>
                <asp:TextBox ID="txtMail" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr><td></td><td class="styleMainTbLeftTD">Status : </td><td>
            <asp:Label ID="lblStatus" runat="server" ></asp:Label></td></tr>
        <tr>
            <td></td><td class="styleMainTbLeftTD">
                Removed Reason :
            </td>
            <td>
                <asp:TextBox ID="txtReason" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Removed reason is required" Text = "*" 
                    ControlToValidate="txtReason" ForeColor="Red" ValidationGroup = "remove"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td><td></td>
            <td>
                <asp:Button ID="btnRemove" runat="server" Text="Remove" Width="100px" OnClick="btnRemove_Click" ValidationGroup = "remove" OnClientClick = "sendValueToParent"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td></td><td style="text-align: left;">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server"  
                    ValidationGroup = "remove" ForeColor="Red" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
