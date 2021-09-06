<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation= "false" CodeBehind="webfrmCompanyCostProfitCenters.aspx.cs" Inherits="GroupHRIS.MetaData.Company.webfrmCompanyCostProfitCenters" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <span style="font-weight: 700">Company Cost/Profit Center</span><br />
    <hr />
    <br />

    <table class="styleMainTb">
    <tr>
    <td  class="styleMainTbLeftTD" style="width:250px">
                        Company :
    </td>
    <td>
        <asp:DropDownList ID="ddlcompany" runat="server" style="width: 200px" 
            onselectedindexchanged="ddlcompany_SelectedIndexChanged" 
            AutoPostBack="True" >
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" 
            ErrorMessage="Company is required "  Text="*" ForeColor="Red" 
            ValidationGroup = "costProfit" ControlToValidate="ddlcompany"></asp:RequiredFieldValidator>
    </td>
    </tr>
    <tr>
    <td  class="styleMainTbLeftTD" style="width:250px">
                        Type :
    </td>
    <td>
        <asp:DropDownList ID="ddlCostProfit" runat="server" style="width: 200px" >
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvcpCenter" runat="server" 
            ErrorMessage="Cost/Profit Center is required "  Text="*" ForeColor="Red" 
            ValidationGroup = "costProfit" ControlToValidate="ddlCostProfit"></asp:RequiredFieldValidator>
    </td>
    </tr>
    <tr>
    <td  class="styleMainTbLeftTD" style="width:250px">
                        Code :
    </td>
    <td>
        <asp:TextBox ID="txtcode" runat="server" style="width:197px"></asp:TextBox>
         <asp:FilteredTextBoxExtender ID="ftcode" runat="server" TargetControlID="txtcode" 
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="rfvcpCenterCode" runat="server" 
            ErrorMessage="Cost/Profit Center Code is required "  Text="*" ForeColor="Red" 
            ValidationGroup = "costProfit" ControlToValidate="txtcode"></asp:RequiredFieldValidator>
    </td>
    </tr>

    
    <tr>
    <td  class="styleMainTbLeftTD" style="width:250px">
                        Name :
    </td>
    <td>
        <asp:TextBox ID="txtName" runat="server" style="width:197px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
            ErrorMessage="Cost/Profit Center Name is required "  Text="*" ForeColor="Red" 
            ValidationGroup = "costProfit" ControlToValidate="txtName"></asp:RequiredFieldValidator>
    </td>
    </tr>

        <tr>
    <td  class="styleMainTbLeftTD" style="width:250px">
                        Status :
    </td>
    <td>
        <asp:DropDownList ID="ddlStatus" runat="server">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" 
            ErrorMessage="Status is required "  Text="*" ForeColor="Red" 
            ValidationGroup = "costProfit" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
    </td>
    </tr>

    <tr><td></td>
    <td>
        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" ValidationGroup = "costProfit"
            onclick="btnAdd_Click"/>
        <asp:Button ID="btnAddclear" runat="server" Text="Clear" Width="100px" 
            onclick="btnAddclear_Click"  />
    </td></tr>
    </table>
    <br /><br />
      <asp:GridView ID="gvAddcostProfitCenter" Style="margin: auto;" 
        runat="server" AutoGenerateColumns="False"
                AllowPaging="true" >
                <Columns>
                    <%--<asp:BoundField DataField="COMP_NAME" HeaderText=" Company Name " />--%>
                    <asp:BoundField DataField="IS_PROFIT_CENTER" HeaderText=" Cost/Profit Center " />
                    <asp:BoundField DataField="COMP_COST_PROFIT_CENTER_CODE" HeaderText=" Cost/Profit Center Code " />
                    <asp:BoundField DataField="COST_PROFIT_CENTER_NAME" HeaderText=" Cost/Profit Center Name " />
                    <%--<asp:BoundField DataField="STATUS_CODE" HeaderText=" Status" />--%>
                    <asp:TemplateField HeaderText="IS_EXCLUDE">
                            <ItemTemplate>
                                <asp:CheckBox ID="EXCLUDE" runat="server" OnCheckedChanged="EXCLUDE_OnCheckedChanged"
                                    Checked='<%# bool.Parse(Eval("IS_EXCLUDE").ToString() == "1" ? "True": "False") %>'
                                    Enabled="true" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
            </asp:GridView>
<br />
    <table>
     <tr>
            <td class="styleMainTbLeftTD" style="width:250px">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px"
                    onclick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" 
                    onclick="btnClear_Click" />
            </td>
        </tr>
         <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup = "costProfit"
                            ForeColor="RED" />
                    </td>
                </tr>
    </table>

     <br />
            <br />
            <asp:GridView ID="gvCostProfitCenter" Style="margin: auto;" 
        runat="server" AutoGenerateColumns="False"
                AllowPaging="true" 
        onpageindexchanging="gvCostProfitCenter_PageIndexChanging" 
        onrowdatabound="gvCostProfitCenter_RowDataBound" 
        onselectedindexchanged="gvCostProfitCenter_SelectedIndexChanged" >
                <Columns>
                    <asp:BoundField DataField="COMP_NAME" HeaderText=" Company Name " />
                    <asp:BoundField DataField="IS_PROFIT_CENTER" HeaderText=" Cost/Profit Center " />
                    <asp:BoundField DataField="COMP_COST_PROFIT_CENTER_CODE" HeaderText=" Cost/Profit Center Code " />
                    <asp:BoundField DataField="COST_PROFIT_CENTER_NAME" HeaderText=" Cost/Profit Center Name " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status" />
                </Columns>
            </asp:GridView>

</asp:Content>
