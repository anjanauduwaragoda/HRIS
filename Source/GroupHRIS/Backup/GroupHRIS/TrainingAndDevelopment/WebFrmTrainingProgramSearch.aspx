<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmTrainingProgramSearch.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingProgramSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Search</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {

            _width = 950;
            _height = window.screen.availHeight - 20;

            window.resizeTo(_width, _height)
            window.focus();
        }

        function sendValueToParent() {
            var sPrId = document.getElementById("hfProId").value;
            var sProName = document.getElementById("hfProgram").value;
            
            window.opener.getValueFromChild(sPrId, sProName);

            window.close();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <table id="tblProgrameDetails" style="background-color: #ebf5fb; min-width: 99.7%;
        padding-left: 5px;" align="center" runat="server" clientidmode="Static">
        <tr>
            <td colspan="3">
                <br />
                Program Details
            </td>
            <td align="right">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/close_button.png"
                    Height="18px" Width="20px" OnClick="ImageButton1_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <hr width="100%" />
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label1" runat="server" Text="Program Code : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblCode" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label3" runat="server" Text="Minimum Batch Size : "></asp:Label>
            </td>
            <td class="style7" align="left">
                <asp:Label ID="lblMinimum" runat="server" Text="20"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3" valign="top">
                <asp:Label ID="Label2" runat="server" Text="Program Name : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblPrgmName" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="Label4" runat="server" Text="Maximum Batch Size : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblMaximum" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label5" runat="server" Text="Duration : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblDuration" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3" valign="top">
                <asp:Label ID="Label6" runat="server" Text="Description : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblDesc" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <asp:Label ID="Label7" runat="server" Text="Program Type : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
            </td>
            <td class="style3" valign="top">
                <asp:Label ID="Label8" runat="server" Text="Objectives : "></asp:Label>
            </td>
            <td class="style7">
                <asp:Label ID="lblObjectives" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style7">
                &nbsp;
            </td>
            <td class="style3">
                <br />
            </td>
            <td class="style7">
                <br />
                <asp:Button ID="btnAdd" runat="server" Text="<< Select" Width="80px" OnClientClick="sendValueToParent()" />
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
    </table>
    <br />
    <table width="855px">
        <%--<tr>
            <td>
                <asp:TextBox ID="txtProId" runat="server" BorderStyle="None" Font-Bold="True" ForeColor="Blue"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtProgramName" runat="server" Width="200px" BorderStyle="None"
                    Font-Bold="True" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <span>Programs </span>
    <hr />
    <table id="Table1" width="855px">
        <tr>
            <td align="center">
                <table width="900px">
                    <tr>
                        <td>
                            <asp:Label ID="lblTrainingType" runat="server" Text="Training Type : "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingType" runat="server" Width="150px" OnSelectedIndexChanged="ddlTrainingType_SelectedIndexChanged"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Training Category : "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingCategory" runat="server" Width="150px" OnSelectedIndexChanged="ddlTrainingCategory_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="style10" align="right" valign="middle">
                            <asp:Label ID="Label15" runat="server" Text="Training Subcategory : "></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlTrainingSubcategorySearch" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 60px;">
                            <asp:ImageButton ID="iBtnSearch" runat="server" Height="30px" ImageUrl="~/Images/Search.png"
                                Width="30px" OnClick="iBtnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="tblProgramGrid" width="855px">
        <tr>
            <td align="center" colspan="6">
                <asp:GridView ID="gvAllProgrammes" runat="server" Width="600px" AutoGenerateColumns="false"
                    OnRowDataBound="gvAllProgrammes_RowDataBound" OnSelectedIndexChanged="gvAllProgrammes_SelectedIndexChanged"
                    OnPageIndexChanging="gvAllProgrammes_PageIndexChanging" AllowPaging="true" PageSize="10">
                    <Columns>
                        <asp:BoundField DataField="PROGRAM_ID" HeaderText="programId" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="PROGRAM_CODE" HeaderText="Program Code" />
                        <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                        <asp:BoundField DataField="PROGRAM_TYPE" HeaderText="Program Type" />
                        <asp:BoundField DataField="MINIMUM_BATCH_SIZE" HeaderText="Min. Batch" />
                        <asp:BoundField DataField="MAXIMUM_BATCH_SIZE" HeaderText="Max. Batch" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Programs Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfProId" runat="server" />
    <asp:HiddenField ID="hfProgram" runat="server" />
    </form>
</body>
</html>
