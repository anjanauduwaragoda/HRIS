<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmPropertySearch.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.Property.WebFrmPropertySearch" %>

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

//        function dosposeWindow() {
//            DoPostBack();
//            window.close();
//        }

        function sendValueToParent() {
            try {
                var x = 'true';
                window.opener.displayData(x);
                window.close();
            }
            catch (err) {
                alert(err.Message);
            }
        }


//        function DoPostBack() {
//            __doPostBack();
//        }

    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            height: 30px;
        }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
    <span style="font-weight: 700">Benifit Details</span>
    <br />
    <br />
    <table class="styleMainTb" id="tblbenifits" runat="server" visible="false">
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Benifit Type :
            </td>
            <td>
                <asp:DropDownList ID="ddlPropertyType" runat="server" Width="130px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvPropertyType" runat="server" Text="*" ErrorMessage="Property Type is Required "
                    ForeColor="Red" ValidationGroup="Property" ControlToValidate="ddlPropertyType"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Reference No :
            </td>
            <td>
                <asp:TextBox ID="txtReference" runat="server" Width="130px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvreferenceNo" runat="server" Text="*" ErrorMessage="Reference No is Required"
                    ForeColor="Red" ValidationGroup="Property" ControlToValidate="txtReference"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Model :
            </td>
            <td>
                <asp:TextBox ID="txtModel" runat="server" Width="130px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvModel" runat="server" Text="*" ErrorMessage="Model is Required"
                    ForeColor="Red" ValidationGroup="Property" ControlToValidate="txtModel"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Serial No :
            </td>
            <td>
                <asp:TextBox ID="txtSerial" runat="server" Width="130px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="rfvSerialNo" ForeColor="Red" ControlToValidate="txtSerial"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStstus" runat="server" Width="130px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="Status is required" ForeColor="Red" ControlToValidate="ddlStstus"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" style="width: 250px">
                Company :
            </td>
            <td>
                <asp:DropDownList ID="ddlCompany" runat="server" Width="130px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                <asp:HiddenField ID="hfPropaertyId" runat="server" />
                <asp:HiddenField ID="hfDataTable" runat="server" />
            </td>
            <td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <%--<asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" 
                    Visible="False" onclick="btnSelect_Click" OnClientClick = "dosposeWndow()" />--%>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Property"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table align="center">
        <tr>
            <td>
                <asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" 
                     OnClientClick="sendValueToParent();" onclick="btnSelect_Click" /><%--OnClick="btnSelect_Click"--%>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Benefit Type :
            </td>
            <td>
                <asp:DropDownList ID="ddlbenefit" runat="server" Width="200px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlbenefit_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            
            </td>
            <td>
              
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="width: 700px;">
                <asp:GridView ID="gvPropertyDetails" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                    AllowPaging="true" OnPageIndexChanging="gvPropertyDetails_PageIndexChanging"
                    OnRowDataBound="gvPropertyDetails_RowDataBound" OnSelectedIndexChanged="gvPropertyDetails_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Benefit Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" >
<HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Benifit Type Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" >
<HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Type " />
                        <asp:BoundField DataField="REFERENCE_NO" HeaderText=" Reference No " />
                        <asp:BoundField DataField="MODEL" HeaderText=" Model " />
                        <asp:BoundField DataField="SERIAL_NO" HeaderText=" Serial No " />
                        <asp:BoundField DataField="COMP_NAME" HeaderText=" Company " />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
                         <asp:TemplateField HeaderText="IS_EXCLUDE">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        NO BENEFIT FOUND.
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr><td class="style1"></td><td style="text-align:right;" class="style1">
             <asp:Button ID="btnAdd" runat="server" Text="Add to cart" 
                onclick="btnAdd_Click"/>
                </td></tr>
                <tr><td></td><td><asp:Label ID="lblMessagex" runat="server" style="text-align:center;" ></asp:Label></td></tr>
                <tr><td></td><td>
                <asp:GridView ID="gvAddedBenefits" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                    AllowPaging="true">
                    <Columns>
                        <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Benefit Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" >
<HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PROPERTY_TYPE_ID" HeaderText=" Benifit Type Id " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" >
<HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Type " />
                        <asp:BoundField DataField="REFERENCE_NO" HeaderText=" Reference No " />
                        <asp:BoundField DataField="MODEL" HeaderText=" Model " />
                        <asp:BoundField DataField="SERIAL_NO" HeaderText=" Serial No " />
                       
                    </Columns>
                    <EmptyDataTemplate>
                        NO BENEFIT FOUND.
                    </EmptyDataTemplate>
                </asp:GridView></td></tr>
    </table>
    <div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Property"
            ForeColor="RED" />
    </div>
    </form>
</body>
</html>
