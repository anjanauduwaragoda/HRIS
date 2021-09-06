<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="webFrmDivison.aspx.cs" Inherits="GroupHRIS.MetaData.WebfrmDivison" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td  colspan="2">
                <span style="font-weight: 700">Division Details</span></td>
        </tr>

        <tr>
            <td  colspan="2">
                <hr /></td>
        </tr>

        <tr>
            <td class="styleMainTbLeftTD">
                Company : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompID" runat="server" AutoPostBack="True" 
                    Height="20px" onselectedindexchanged="ddlCompID_SelectedIndexChanged" 
                    Width="400px">
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="styleMainTbLeftTD">
                Department :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlDepID" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" onselectedindexchanged="ddlDepID_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfDepId" runat="server" 
                    ControlToValidate="ddlDepID" ErrorMessage="Department ID is required." 
                    ForeColor="Red" ValidationGroup="vgDivInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Division :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDivName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDivName" runat="server" 
                    ControlToValidate="txtDivName" ErrorMessage="Division Name is required." 
                    ForeColor="Red" ValidationGroup="vgDivInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Description :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDesc" runat="server" MaxLength="45" Width="400px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Land Phone No : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLandNo" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID = "fteLandNo" runat="server" TargetControlID="txtLandNo" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="reLandPhone" runat="server" 
                    ControlToValidate="txtLandNo" 
                    ErrorMessage="Ten Characters required for Land Phone Number." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgDivInfo">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="120px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatusCode" runat="server" 
                    ControlToValidate="ddlStatus" ErrorMessage="Status Code is required." 
                    ForeColor="Red" ValidationGroup="vgDivInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                SAP
                Cost Center Code :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtCostCenter" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtCostCenter" ErrorMessage="Cost Center Code is required." 
                    ForeColor="Red" ValidationGroup="vgDivInfo">*</asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID = "fteCostCenter" runat="server" TargetControlID="txtCostCenter" FilterType="Numbers"></asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                SAP
                Profit Center Code :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtProfitCenter" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtProfitCenter" 
                    ErrorMessage="Profit Center Code is required." ForeColor="Red" 
                    ValidationGroup="vgDivInfo">*</asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID = "fteProfitCenter" runat="server" TargetControlID="txtProfitCenter" FilterType="Numbers"></asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                    onclick="btnSave_Click" ValidationGroup="vgDivInfo" />
                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                    Text="Clear" Width="100px" />
            </td>
        </tr>
        
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                    ValidationGroup="vgDivInfo" />
            </td>
        </tr>
        
        <tr>
            <td   colspan="2">
                <hr />
             </td>
        </tr>
        <tr>
            <td  colspan="2" align="center">
                <div class="stylediv">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                        onpageindexchanging="GridView1_PageIndexChanging" 
                        onrowdatabound="GridView1_RowDataBound" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        Width="212px" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="DIVISION_ID" HeaderText="Division ID" />
                            <asp:BoundField DataField="DIV_NAME" HeaderText="Division Name" />
                            <asp:BoundField DataField="COST_CENTER_CODE" HeaderText="Cost Center" />
                            <asp:BoundField DataField="PROFIT_CENTER_CODE" HeaderText="Profit Center" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>

                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
