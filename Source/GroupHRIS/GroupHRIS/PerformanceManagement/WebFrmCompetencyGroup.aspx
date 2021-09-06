<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCompetencyGroup.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmCompetencyGroup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
   
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Competency Group Details</span>
    <hr />
    <table>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label1" runat="server" Text="Competency Group Name :"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtGroupName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ErrorMessage="Group name is required"
                    ControlToValidate="txtGroupName" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="fteGroupName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars = " " TargetControlID="txtGroupName">
                </asp:FilteredTextBoxExtender>
            </td>
            <td class="styleTableCell5">
            </td >
            <td class="styleTableCell6">                
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label2" runat="server" Text="Description :"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtDescription" runat="server" Width="250px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
               
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">                
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label3" runat="server" Text="Status :"></asp:Label>
            </td>
            <td class="styleTableCell2" style="text-align: left">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is required"
                    ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">                
            </td>
        </tr>        
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2" style="text-align: left">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                    ValidationGroup="CGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">                
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td colspan="5" class="styleTableCell2" style="text-align: left">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td colspan="5">
                <asp:ValidationSummary ID="vsCGroup" runat="server" ForeColor="#CC3300" ValidationGroup="CGroup" />
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfCompetencyGroupId" runat="server" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
            <td class="styleTableCell6">                
            </td>
        </tr>
    </table>
    <br />
    <span>Competency Groups</span>
    <hr />
    
        <table style="margin: auto">
            <tr>
                <td>
                    <asp:GridView ID="gvCGroups" runat="server" AutoGenerateColumns="False" 
                        OnSelectedIndexChanged="gvCGroups_SelectedIndexChanged" 
                        onrowdatabound="gvCGroups_RowDataBound" AllowPaging="True" 
                        style="margin: auto;" onpageindexchanging="gvCGroups_PageIndexChanging" >
                        <Columns>
                            <asp:BoundField DataField="COMPETENCY_GROUP_ID" HeaderText="Group Id" HeaderStyle-CssClass="hideGridColumn"

                                ItemStyle-CssClass="hideGridColumn">
<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Center"></HeaderStyle>

<ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="COMPETENCY_GROUP_NAME" 
                                HeaderText="Competency Group Name">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="STATUS" HeaderText="Status">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>

</asp:Content>
