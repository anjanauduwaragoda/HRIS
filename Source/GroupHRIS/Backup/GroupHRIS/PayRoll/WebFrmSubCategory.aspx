<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmSubCategory.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmSubCategory"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700">Subcategory</span><br />

            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Category :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:DropDownList ID="ddlCategory" runat="server" Width="200px" 
                            DataTextField="CATEGORY" AutoPostBack="True" 
                            onselectedindexchanged="ddlCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCategory" ValidationGroup="SubCategory" runat="server"
                            Text="*" ForeColor="Red" ErrorMessage="Category is required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Subcategory Name :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:TextBox ID="txtSubcategory" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers,Custom,LowercaseLetters,UppercaseLetters" ValidChars="-_" TargetControlID="txtSubcategory" runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="fvSubcategory" ValidationGroup="SubCategory" runat="server"
                            Text="*" ForeColor="Red" ErrorMessage="Subcategory Name is required" ControlToValidate="txtSubcategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Remarks :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:TextBox ID="txtSubcategoryRemarks" runat="server" Width="200px" 
                            TextMode = "MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Status :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:DropDownList ID="ddlStstus" runat="server" Width="100px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ValidationGroup="SubCategory" 
                            ErrorMessage="Status is required" Text = "*" ForeColor="Red" 
                            ControlToValidate="ddlStstus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="SubCategory"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="SubCategory"
                            ForeColor="RED" />
                            <asp:Label ID="lblmsg" runat="server" Font-Size="10pt" ForeColor="Blue" 
                            style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            
            <asp:GridView ID="gvSubcategory" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnSelectedIndexChanged="gvSubcategory_SelectedIndexChanged"
                OnPageIndexChanging="gvSubcategory_PageIndexChanging" OnRowDataBound="gvSubcategory_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category" />
                    <asp:BoundField DataField="SUB_CATEGORY" HeaderText="Subcategory" SortExpression="SUB_CATEGORY" />
                    <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
