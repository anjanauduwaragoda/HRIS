<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="webFrmLeaveType.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmLeaveType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="styleMainTb">
        <tr>
            <td colspan="2">
                <span style="font-weight: 700">Leave Type Details</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Leave Type ID :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLeaveID" runat="server" MaxLength="15" Width="230px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfLeaveID" runat="server" ControlToValidate="txtLeaveID"
                    ErrorMessage="Leave Type ID is required." ForeColor="Red" ValidationGroup="vgLeaveInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Leave Type :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLeaveType" runat="server" Width="230px" Height="20px" MaxLength="45"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfLeaveType" runat="server" ControlToValidate="txtLeaveType"
                    ErrorMessage="Leave Type is required." ForeColor="Red" ValidationGroup="vgLeaveInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="109px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatus" runat="server" ControlToValidate="ddlStatus"
                    ErrorMessage="Status Code is required." ForeColor="Red" ValidationGroup="vgLeaveInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Leave Category :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCategory" runat="server" Height="20px" Width="109px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="A">Annual</asp:ListItem>
                    <asp:ListItem Value="C">Casual</asp:ListItem>
                    <asp:ListItem Value="M">Medical</asp:ListItem>
                    <asp:ListItem Value="O">Other</asp:ListItem>
                </asp:DropDownList>
                 <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory"
                    ErrorMessage="Leave Category is required." ForeColor="Red" ValidationGroup="vgLeaveInfo">*</asp:RequiredFieldValidator>
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
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="91px" ValidationGroup="vgLeaveInfo"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="88px" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="vsLeaveType" runat="server" ForeColor="Red" ValidationGroup="vgLeaveInfo" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div class="stylediv">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="LEAVE_TYPE_ID" HeaderText="Leave Type ID" />
                            <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="Leave Type" />
                            <asp:BoundField DataField="CATEGORY" HeaderText="Leave Category" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>