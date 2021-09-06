<%@ Page Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmProcessNopay.aspx.cs" EnableEventValidation="false" Inherits="GroupHRIS.PayRoll.WebFrmProcessNopay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<span style="font-weight: 700">No Pay/Over Time Process</span><br />--%>
            <span style="font-weight: 700">Overtime Process</span><br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Month :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Label ID="lblMonth" runat="server" Text="lblMonth"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="150px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" Style="height: 22px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company is required "
                            Text="*" ForeColor="Red" ControlToValidate="ddlCompany" ValidationGroup="process"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                </table>
                  
                <br /><%--Nopay/OT Process 
                <hr /> --%>
                <table runat="server" id="tblProcess">
                <%--<tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Nopay Process :
                    </td>
                    <td>
                        <asp:Button ID="btnNoPay" runat="server" Text="Run"  OnClick="btnNoPay_Click"
                            ValidationGroup="process" style="height: 26px" />
                        <asp:Label ID="lblNoPay" runat="server"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        OT Payments Process :
                    </td>
                    <td>
                        <asp:Button ID="btnOT" runat="server" Text="Run" Width="100px"  OnClick="btnOT_Click"
                            ValidationGroup="process" />
                        &nbsp;<asp:Label ID="lblOT" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="process" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align:center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />                            
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
           <br />
           <br />
           Overtime and Special Payment Information
           <hr />
            <%--<asp:Label ID="lblmsg" runat="server" Text="Overtime and Special Payment Information"></asp:Label>--%>
            
            <br />
            
            <br />
            <asp:GridView ID="gvOvertime" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                >
                <Columns>
                    <asp:BoundField DataField="EPF_NO" HeaderText=" EPF Number " />
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category " />
                    <asp:BoundField DataField="SUB_CATEGORY" HeaderText=" Sub Category " />
                    <asp:BoundField DataField="TYPE_ID" HeaderText=" Type ID " />
                    <asp:BoundField DataField="AMOUNT" HeaderText=" Amount" />
                </Columns>
            </asp:GridView>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
