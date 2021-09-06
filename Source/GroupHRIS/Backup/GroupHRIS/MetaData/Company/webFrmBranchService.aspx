<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="webFrmBranchService.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmBranchService" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
  
  
    
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
                <tr>
            <td   >
                <strong>Branch/Service Center</strong></td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
                <tr>
            <td colspan="2"   >
                <hr /></td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Company : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompID" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlCompID_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfCompID" runat="server" 
                    ControlToValidate="ddlCompID" ErrorMessage="Company ID is required." 
                    ForeColor="Red" ValidationGroup="vgBranchCenter">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Branch : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBranchName" runat="server" Width="400px" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="BranchName" runat="server" 
                    ControlToValidate="txtBranchName" ErrorMessage="Branch Name is required." 
                    ForeColor="Red" ValidationGroup="vgBranchCenter">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Branch Code :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtbranchcode" runat="server" MaxLength="15" Width="120px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Land Phone : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLandNo" runat="server" MaxLength="10" Width="120px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                <asp:TextBox ID="txtLandNo2" runat="server" MaxLength="10" Width="120px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfLandPhone" runat="server" 
                    ControlToValidate="txtLandNo" ErrorMessage="Land Phone Number is required." 
                    ForeColor="Red" ValidationGroup="vgBranchCenter">*</asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID = "fteLandPhone2" runat="server" TargetControlID="txtLandNo" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                <asp:FilteredTextBoxExtender ID = "fteLandPhone" runat="server" TargetControlID="txtLandNo2" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="reLandPhone1" runat="server" 
                    ControlToValidate="txtLandNo" 
                    ErrorMessage="Ten Characters required for Land Phone Number." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgBranchCenter">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="Landphone2" runat="server" 
                    ControlToValidate="txtLandNo2" 
                    ErrorMessage="Ten Characters required for Land Phone number2." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgBranchCenter">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Fax :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID = "fteFaxNo" runat="server" TargetControlID="txtFaxNo" FilterType="Numbers"></asp:FilteredTextBoxExtender>

                <asp:RegularExpressionValidator ID="reFaxNo" runat="server" 
                    ControlToValidate="txtFaxNo" 
                    ErrorMessage="Ten Characters required for Fax Number." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgBranchCenter">*</asp:RegularExpressionValidator>

            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Branch Address : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBranchAdd1" runat="server" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfBranchAdd" runat="server" 
                    ControlToValidate="txtBranchAdd1" ErrorMessage="Branch Address is required." 
                    ForeColor="Red" ValidationGroup="vgBranchCenter">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBranchAdd2" runat="server" Width="400px" onkeydown = "return (event.keyCode!=13);"
                    style="margin-bottom: 0px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBranchAdd3" runat="server" Width="400px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBranchAdd4" runat="server" Width="400px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="120px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatus" runat="server" 
                    ControlToValidate="ddlStatus" ErrorMessage="Status Code is required." 
                    ForeColor="Red" ValidationGroup="vgBranchCenter">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Contact Person : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtContactPerson" runat="server" Width="400px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:HiddenField ID="hfBranchID" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                    ValidationGroup="vgBranchCenter" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="100px" 
                    onclick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="vsBranchService" runat="server" ForeColor="Red" 
                    ValidationGroup="vgBranchCenter" />
                </td>
        </tr>

        <tr>
            <td  colspan="2">
                <hr /></td>
        </tr>
        <tr>
            <td colspan="2" align="center"   >
                 <div class="stylediv"> 

                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                         AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" onselectedindexchanged="GridView1_SelectedIndexChanged" 
                         onrowdatabound="GridView1_RowDataBound">
                         <Columns>
                             <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch ID" />
                             <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" />
                             <asp:BoundField DataField="BRANCH_NAME" HeaderText="Branch Name" />
                             <asp:BoundField DataField="BRANCH_CODE" HeaderText="Branch Code" />
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
