<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCompanySubcategoryTypes.aspx.cs"
    Inherits="GroupHRIS.PayRoll.WebFrmCompanySubcategoryTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span><strong>Company Subcategory Type</strong> </span>
            <br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" DataTextField="CATEGORY"
                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCompany" ValidationGroup="CompanySubcategory"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Company is required" ControlToValidate="ddlCompany"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Category :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server" Width="200px" DataTextField="CATEGORY"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="fvCategory" ValidationGroup="CompanySubcategory"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Category is required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Subcategory :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSubcategory" runat="server" Width="200px" DataTextField="SUB_CATEGORY"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlSubcategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSubcategory" ValidationGroup="CompanySubcategory"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Subcategory is required"
                            ControlToValidate="ddlSubcategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Subcategory Type ID :
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtSubcategoryTypeId" runat="server" 
                            ontextchanged="txtSubcategoryTypeId_TextChanged" AutoPostBack="True"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlTypeId" runat="server" Width="100px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTypeId_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="fvSubcategoryType" ValidationGroup="CompanySubcategory"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Subcategory Type ID is required"
                            ControlToValidate="ddlTypeId"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text = "*" ValidationGroup="CompanySubcategory"
                            ErrorMessage="Status is required " ControlToValidate="ddlStatus" 
                            ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="CompanySubcategory"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label><br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CompanySubcategory"
                            ForeColor="RED" />
                    </td></tr>
            </table>
            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
            <hr />
            <br />
            <asp:Label ID="lblmsg" runat="server" Font-Size="10pt" ForeColor="Blue" 
                            style="font-weight: 700"></asp:Label>
            <br />
            <asp:GridView ID="gvCompanySubcategoryType" Style="margin: auto;" runat="server"
                AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="gvCompanySubcategoryType_PageIndexChanging"
                OnRowDataBound="gvCompanySubcategoryType_RowDataBound" OnSelectedIndexChanged="gvCompanySubcategoryType_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="COMP_NAME" HeaderText="Company  " />
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category " />
                    <asp:BoundField DataField="SUB_CATEGORY" HeaderText=" Subcategory " />
                    <asp:BoundField DataField="SUB_CAT_TYPE_ID" HeaderText=" Subcategory Type ID " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
