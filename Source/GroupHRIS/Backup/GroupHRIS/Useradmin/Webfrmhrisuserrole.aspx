<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Webfrmhrisuserrole.aspx.cs" Inherits="GroupHRIS.Useradmin.Webfrmhrisuserrole" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">    </asp:ToolkitScriptManager>
<table class="styleMainTb">
<tr>
<td class="styleMainTb">
<span style="font-weight: 700">HRIS User Role</span>
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
                            E.P.F. No
                     </td>
                     <td class="divsearchTD">
                        <asp:TextBox ID="txtepfno" runat="server" MaxLength="8" Width="89px"></asp:TextBox>
                         <asp:FilteredTextBoxExtender ID="txtepfno_FilteredTextBoxExtender" 
                             runat="server" TargetControlID="txtepfno" FilterType="Numbers">
                         </asp:FilteredTextBoxExtender>
                     </td>
                     <td class="divsearchTD">
                         Company</td>
                     <td class="divsearchTD2">
                         <asp:DropDownList ID="dpCompID" runat="server" Width="180px" 
                             AutoPostBack="True" onselectedindexchanged="dpCompID_SelectedIndexChanged">
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
                 
                 
                         <asp:ImageButton ID="imgbtnSearch" runat="server" 
                             ImageUrl="~/Images/Common/user-search-icon.png" 
                             onclick="imgbtnSearch_Click"   />
                 
                 
                     </td>
                     </tr>
                     <tr>
                     <td class="divsearchTD">
                            NIC. No.
                     </td>
                     <td class="divsearchTD">
                        <asp:TextBox ID="txtnicno" runat="server" MaxLength="10" Width="89px"></asp:TextBox>
                         <asp:FilteredTextBoxExtender ID="txtnicno_FilteredTextBoxExtender" 
                             runat="server" TargetControlID="txtnicno" FilterType="Numbers,Custom" ValidChars="V">
                         </asp:FilteredTextBoxExtender>
                     </td>
                     <td class="divsearchTD">
                         Known Name</td>
                     <td class="divsearchTD2">
                        <asp:TextBox ID="txtlastname" runat="server" MaxLength="10" Width="180px"></asp:TextBox>
                         <asp:FilteredTextBoxExtender ID="txtlastname_FilteredTextBoxExtender" 
                             runat="server" TargetControlID="txtlastname" FilterType="UppercaseLetters,LowercaseLetters">
                         </asp:FilteredTextBoxExtender>
                     </td>
                     <td class="divsearchTD">
                         &nbsp;</td>
                     <td class="divsearchTD2">
                         &nbsp;</td>
                     <td class="divsearchTD">
                 
                 
                         &nbsp;</td>
                     </tr>
                     </table>
               </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" >
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" >
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee ID : </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblemployeeid" runat="server" Font-Bold="True" 
                    ForeColor="#3366CC"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;User ID : </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lbluserid" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee 
                Name : </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblname" runat="server" ForeColor="#0066FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Description : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtdescription" runat="server" Width="400px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;User Role : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlUserrole" runat="server" Height="25px" Width="289px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnupdate" runat="server" Text="Save" Width="112px" 
                    onclick="btnupdate_Click" />
                <asp:Button ID="btnclose" runat="server" Text="Clear" Width="97px" 
                    onclick="btnclose_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <table class="styleMainTb">
    <tr>
            <td class="styleMainTb" align="center" >

                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>

            </td>
        </tr>
    <tr>
            <td class="styleMainTb" align="center" >

                <hr />

            </td>
        </tr>
    <tr>
            <td class="styleMainTb" align="center" >

                <asp:GridView ID="GridView1" runat="server" AllowPaging="True"  PageSize="8" 
                        onpageindexchanging="GridView1_PageIndexChanging" 
                        onrowdatabound="GridView1_RowDataBound" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" >
                    <PagerSettings PageButtonCount="2"  />
                </asp:GridView>

            </td>
        </tr>
    </table>
</asp:Content>
