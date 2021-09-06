<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Webfrmhrisusers.aspx.cs" Inherits="GroupHRIS.Useradmin.Webfrmhrisusers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTb">
                <span style="font-weight: 700">HRIS Users</span>
            </td>
        </tr>
        <tr>
            <td class="styleMainTb">
                <hr />
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td class="divsearch" colspan="2" align="center">
                <table class="styleMainTb">
                    <tr>
                        <td class="divsearchTD">
                            E.P.F. No.
                        </td>
                        <td class="divsearchTD">
                            <asp:TextBox ID="txtepfno" runat="server" MaxLength="8" Width="89px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtepfno_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtepfno" FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="divsearchTD">
                            Company
                        </td>
                        <td class="divsearchTD2">
                            <asp:DropDownList ID="dpCompID" runat="server" Width="180px" AutoPostBack="True"
                                OnSelectedIndexChanged="dpCompID_SelectedIndexChanged" Height="20px">
                            </asp:DropDownList>
                        </td>
                        <td class="divsearchTD">
                            Department
                        </td>
                        <td class="divsearchTD2">
                            <asp:DropDownList ID="dpDeptCode" runat="server" Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td class="divsearchTD">
                            <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                OnClick="imgbtnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="divsearchTD">
                            NIC. No.
                        </td>
                        <td class="divsearchTD">
                            <asp:TextBox ID="txtnicno" runat="server" MaxLength="10" Width="89px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtnicno_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtnicno" FilterType="Numbers,Custom" ValidChars="V">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="divsearchTD">
                            Known Name
                        </td>
                        <td class="divsearchTD2">
                            <asp:TextBox ID="txtlastname" runat="server" MaxLength="10" Width="180px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtlastname_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtlastname" FilterType="UppercaseLetters,LowercaseLetters">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="divsearchTD">
                            &nbsp;
                        </td>
                        <td class="divsearchTD2">
                            &nbsp;
                        </td>
                        <td class="divsearchTD">
                            &nbsp;
                        </td>
                    </tr>
                </table>
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
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee ID :
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblemployeeid" runat="server" Font-Bold="True" ForeColor="#3366CC"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Name with Initials :
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblfirstname" runat="server" ForeColor="#000066"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Known Name :
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lbllastname" runat="server" ForeColor="#000066"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;User ID :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtuserid" runat="server" ForeColor="#0000CC" MaxLength="10" Width="180px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Functional Area / Working Company :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="dpWorkCompID" runat="server" Width="298px" AutoPostBack="True"
                    Height="20px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Email :
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblemail" runat="server" Font-Bold="False" ForeColor="#0066CC"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnupdate" runat="server" Text="Save" Width="112px" OnClick="btnupdate_Click" />
                <asp:Button ID="btnclose" runat="server" Text="Clear" Width="97px" OnClick="btnclose_Click" />
            </td>
        </tr>
    </table>
    <table class="styleMainTb">
        <tr>
            <td class="styleMainTb" align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTb" align="center">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="styleMainTb" align="center">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="8" OnPageIndexChanging="GridView1_PageIndexChanging"
                    OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <PagerSettings PageButtonCount="2" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>