<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="WebFrmAddPropertyDetails.aspx.cs" Inherits="GroupHRIS.Property.WebFrmAddPropertyDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span style="font-weight: 700">Benefit Information</span><br />
    <hr />
    <br />
    <table class="styleMainTb">
  
        <tr style="width: 490px">
            <td class="styleMainTbLeftTD">
                Benefits Type : 
            </td>
            <td>
                <asp:DropDownList ID="ddlPropertyType" runat="server" style="width: 200px" 
                    onselectedindexchanged="ddlPropertyType_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvPropertyType" runat="server" Text="*" 
                    ErrorMessage="Benefit type is required " ForeColor="Red" ValidationGroup="Property"
                    ControlToValidate="ddlPropertyType" ></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="width: 490px">
            <td class="styleMainTbLeftTD">
                Reference No :
            </td>
            <td>
                <asp:TextBox ID="txtReference" runat="server" style="width: 195px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvreferenceNo" runat="server" Text="*"
                    ErrorMessage="Reference No is required" ForeColor="Red" ValidationGroup="Property"
                    ControlToValidate="txtReference" ></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="width: 490px">
            <td class="styleMainTbLeftTD" >
                Model :
            </td>
            <td>
                <asp:TextBox ID="txtModel" runat="server" style="width: 195px"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="rfvModel" runat="server" Text="*"
                    ErrorMessage="Model is Required" ForeColor="Red" ValidationGroup="Property"
                    ControlToValidate="txtModel"  ></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr style="width: 490px">
            <td class="styleMainTbLeftTD" >
                Serial No :
            </td>
            <td>
                <asp:TextBox ID="txtSerial" runat="server" Width="195px"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="Serial number is required" ForeColor="Red" ControlToValidate="txtSerial" ></asp:RequiredFieldValidator>

            </td>
        </tr>
       
             <tr style="width: 490px">
            <td class="styleMainTbLeftTD">
                Company :
            </td>
            <td>
                <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" 
                    AutoPostBack="True" onselectedindexchanged="ddlCompany_SelectedIndexChanged">
                </asp:DropDownList>
                 <asp:RequiredFieldValidator ID="rfvCompany" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="Company name is required" ForeColor="Red" ControlToValidate="ddlCompany" ></asp:RequiredFieldValidator>

            </td>
        </tr>
        
        <tr style="width: 490px">
            <td class="styleMainTbLeftTD">
                Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStstus" runat="server" Width="200px">
                </asp:DropDownList>
                <asp:Label ID="lblstatus" runat="server"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text="*" ValidationGroup="Property"
                    ErrorMessage="Status is required" ForeColor="Red" ControlToValidate="ddlStstus"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Property"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr><td>
                <asp:HiddenField ID="hfPropaertyId" runat="server" /></td><td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Property"
                    ForeColor="RED" /></td></tr>
        </table>
    <br />
    <br />
    <asp:GridView ID="gvPropertyDetails" Style="margin: auto;" runat="server" AutoGenerateColumns="False" 
        AllowPaging="true" 
        onpageindexchanging="gvPropertyDetails_PageIndexChanging" 
        onrowdatabound="gvPropertyDetails_RowDataBound" 
        onselectedindexchanged="gvPropertyDetails_SelectedIndexChanged" >
        <Columns>
            <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Property Id " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
            <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Type " />
            <asp:BoundField DataField="REFERENCE_NO" HeaderText=" Reference No " />
            <asp:BoundField DataField="MODEL" HeaderText=" Model " />
            <asp:BoundField DataField="SERIAL_NO" HeaderText=" Serial No " />
            <asp:BoundField DataField="COMP_NAME" HeaderText=" Company " />
            <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
        </Columns>
    </asp:GridView>
</asp:Content>
