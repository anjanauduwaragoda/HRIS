<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation ="false" CodeBehind="webFrmRoster.aspx.cs" Inherits="GroupHRIS.Roster.webFrmRoster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <span>Roster</span>
    <hr />
    <br /> 
        
    <table class="styleMainTb">
        
        <tr>
            <td width="30%" align="right">
                Company</td>
            <td width="40%" align="left">
                <asp:DropDownList ID="ddlCompany" runat="server" 
                     Width="256px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Company is required." ControlToValidate="ddlCompany" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                Roster Type</td>
            <td align="left">
                <asp:DropDownList ID="ddlRosterType" runat="server" 
                     Width="256px" 
                    onselectedindexchanged="ddlRosterType_SelectedIndexChanged" >
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="Roster Type is required." ControlToValidate="ddlRosterType" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                From Time</td>
            <td align="left">
                             
                <asp:DropDownList ID="ddlFromTimeHH" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlFromTimeMM" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
            &nbsp;(HH:MM)</td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                To Time</td>
            <td align="left">
                             
                <asp:DropDownList ID="ddlToTimeHH" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlToTimeMM" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
            &nbsp;(HH:MM)</td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                Flexible Time</td>
            <td align="left">
                             
                <asp:DropDownList ID="ddlFlexibleHrs" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlFlexibleMins" runat="server" 
                     Width="40px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
                             
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                Status</td>
            <td align="left">
                             
                <asp:DropDownList ID="ddlStatus" runat="server" 
                     Width="80px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                Num. Days</td>
            <td align="left">
                             
                <asp:DropDownList ID="ddlNumDays" runat="server" 
                     Width="80px" onselectedindexchanged="ddlCompany_SelectedIndexChanged1">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ErrorMessage="Num. Days is required." ControlToValidate="ddlNumDays" 
                    ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
        </tr>

        <tr>
            <td align="right">
                &nbsp;</td>
            <td align="left">
                <asp:HiddenField ID="hfRosterID" runat="server" />
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                &nbsp;</td>
            <td align="left">
                <asp:HiddenField ID="hfCompCode" runat="server" />
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td  align="right">&nbsp;</td>

            <td  align="left">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" 
                    onclick="btnSave_Click" ValidationGroup="vgSubmit" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    onclick="btnCancel_Click" />
            </td>

            <td  align="left">
                &nbsp;</td>
        </tr>

    </table>


    <table  class="styleMainTb">
    <tr>
        <td align="center">
            <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
        </td>
    </tr>
    <tr> 
        <td align="left">
            <asp:ValidationSummary ID="vsSubmit" runat="server" 
                ValidationGroup="vgSubmit" ForeColor="Red" />
        </td>
    </tr>
    </table>


    <br />
    <span>List of Rosters</span>
    <hr />
    <br />

    <div>
    
        <asp:GridView ID="gvRosters" runat="server" AutoGenerateColumns="False" 
            Width="100%" PageSize="10" 
            onpageindexchanging="gvRosters_PageIndexChanging" 
            onrowdatabound="gvRosters_RowDataBound" 
            onselectedindexchanged="gvRosters_SelectedIndexChanged" AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="ROSTR_ID" HeaderText="Roster ID" />
                <asp:BoundField DataField="COMPANY_ID" HeaderText="CompanyID" />
                <asp:BoundField DataField="ROSTER_TYPE" HeaderText="TypeID" />
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Roster Type" />
                <asp:BoundField DataField="FROM_TIME" HeaderText="From Time" />
                <asp:BoundField DataField="TO_TIME" HeaderText="To Time" />
                <asp:BoundField DataField="ROSTER_TYPE" HeaderText="TypeID" Visible="False" />
                <asp:BoundField DataField="FLEXIBLE_TIME" HeaderText="Flex Time" />
                <asp:BoundField DataField="NUM_DAYS" HeaderText="Num. Days" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    
    </div>


</asp:Content>
