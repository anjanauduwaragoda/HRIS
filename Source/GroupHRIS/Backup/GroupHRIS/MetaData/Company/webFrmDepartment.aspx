<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation ="false" CodeBehind="webFrmDepartment.aspx.cs" Inherits="GroupHRIS.MetaData.WebfrmDepartment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <table class="styleMainTb">
        <tr>
            <td colspan="2"  >
                <span style="font-weight: 700">Department Details</span>
             </td>
        </tr>
        <tr>
            <td colspan="2"  >
               <hr /></td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class = "styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Company :</td>
            <td class = "styleMainTbRightTD" style="text-align:left">
                <asp:DropDownList ID="ddlCompID" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" 
                    onselectedindexchanged="dpCompID_SelectedIndexChanged" >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfComID" runat="server" 
                    ControlToValidate="ddlCompID" ErrorMessage="Company ID is required." 
                    ForeColor="Red" ValidationGroup="vgDepInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Department : </td>
            <td class = "styleMainTbRightTD">
                <asp:TextBox ID="txtDepName" runat="server" Width="400px" MaxLength="100" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtDepName" ErrorMessage="Department Name is required." 
                    ForeColor="Red" ValidationGroup="vgDepInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Land Phone No : </td>
            <td class = "styleMainTbRightTD">
                <asp:TextBox ID="txtLandNo" runat="server" Width="120px" MaxLength="10" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtLandNo" ErrorMessage="Land Phone No. is required." 
                    ForeColor="Red" ValidationGroup="vgDepInfo">*</asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="fteLandPhonr" runat="server" TargetControlID="txtLandNo" FilterType="Numbers" ValidChars="/" >
                    </asp:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="reLandPhone" runat="server" 
                    ControlToValidate="txtLandNo" 
                    ErrorMessage="Ten Characters required for Land Phone Number." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgDepInfo">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Description :</td>
            <td class = "styleMainTbRightTD">
                <asp:TextBox ID="txtDesc" runat="server" Width="400px" MaxLength="50"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status&nbsp; :</td>
            <td class = "styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="120px"  >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatus" runat="server" 
                    ControlToValidate="ddlStatus" ErrorMessage="Status Code is required." 
                    ForeColor="Red" ValidationGroup="vgDepInfo">*</asp:RequiredFieldValidator>
                </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                </td>
            <td class = "styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class = "styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Save" 
                    Width="100px" ValidationGroup="vgDepInfo" />
                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                    Text="Clear" Width="100px" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class = "styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ValidationGroup="vgDepInfo" ForeColor="Red" />
                </td>
        </tr>
        
        <tr>
            <td  colspan="2">
            <hr /></td>
        </tr>
        <tr>
            <td colspan="2" align="center" >
            <div class="stylediv" > 
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged" 
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="DEPT_ID" HeaderText="Department ID" />
                        <asp:BoundField DataField="COMP_NAME" HeaderText="Company Name" />
                        <asp:BoundField DataField="DEPT_NAME" HeaderText="Department Name" />
                        <asp:BoundField DataField="LAND_PHONE" HeaderText="Land Phone No" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                </asp:GridView>

            </div>
            </td>
                 
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class = "styleMainTbRightTD">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
