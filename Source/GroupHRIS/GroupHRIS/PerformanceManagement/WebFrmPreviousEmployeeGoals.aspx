<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmPreviousEmployeeGoals.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmPreviousEmployeeGoals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Previous Goals </title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            //window.moveTo(20, 20);
            window.resizeTo(_width, _height)
            window.focus();
        }

        function sendValueToParent_() {
            var sRetVal = document.getElementById("hfStatus").value;

            window.opener.getValueFromChild(sRetVal);

            window.close();
            return false;
        }
    </script>
</head>
<body onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div class="Title">
        <span style="font-weight: 700">Employee Previous Goals</span>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <table style="margin: auto; height: 50px; padding-left: 10px; padding-right: 10px;
                width: 700px;" class="divsearch">
                <tr>
                    <td>
                        Year of Goal :
                    </td>
                    <%--<td style="width: 30px;"></td>--%>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlYear" runat="server" Width="150">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlYear"
                            ErrorMessage="Year of Goal is Required" ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:Button ID="btnView1" runat="server" OnClick="btnView_Click" Text="View" ValidationGroup="Main"
                            Width="125px" />
                    </td>
                    <%--<td style="width: 30px;"></td>--%>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 30px;">
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btnClose" runat="server" OnClientClick="sendValueToParent_()" Text="Close"
                            ValidationGroup="Main" Width="125px" />
                    </td>
                </tr>
            </table>
            <table style="margin: auto; width: 700px;">
                <tr>
                    <td style="width: 40%; vertical-align: top;">
                        Select for This Year&nbsp;
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="&lt;&lt; Select"
                            Width="125px" />
                        <br />
                        <asp:Label ID="lblEmployeeName" runat="server" Style="color: #0000FF"></asp:Label>
                    </td>
                    <td style="width: 60%; vertical-align: top; text-align:right;">
                        <table style="margin-left:38%;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblBalaceWeightText" runat="server" Text="Cumulative Weight for Current Year : "></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblTotalWeightValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblTotalWeightText" runat="server" Text="Balanced Weight for Current Year : "></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblBalanceWeightValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblMaximumWeightText" runat="server" Text="Total Weight for Current Year :"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblMaximumWeightValue" runat="server" Text="100%"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="/Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
            </table>
            Previous Goals
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvEmployeeGoals" AutoGenerateColumns="false" runat="server" 
                            Style="width: 800px;" onrowdatabound="grdvEmployeeGoals_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText=" Year of Goal " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_GROUP_ID" HeaderText=" Goal Group ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_AREA" HeaderText=" Goal Area" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText=" Goal Description " />
                                <asp:BoundField DataField="MEASUREMENTS" HeaderText=" Measurements " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />

                                <asp:TemplateField HeaderText=" Weight ">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEmployeeWeight" style="text-align:right;" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                                
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status Code" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_ID" HeaderText=" Goal ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE_TEXT" HeaderText=" Status " />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblExclude" runat="server" Text="Include All"></asp:Label>
                                        <br />
                                        <asp:CheckBox ID="chkisIncludeAll" AutoPostBack="true" runat="server" OnCheckedChanged="chkisIncludeAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkisInclude" AutoPostBack="true" runat="server" oncheckedchanged="chkisInclude_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="WEIGHT" HeaderText=" Weight " HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfStatus" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
