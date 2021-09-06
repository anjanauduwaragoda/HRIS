<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmMultipleRosterAssignment.aspx.cs" Inherits="GroupHRIS.Roster.webFrmMultipleRosterAssignment"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">


        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            color: #3333CC;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <br />
            <span>Multiple Roster Assignment</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Style="text-align: right" Text="Employee"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="styleTableCell2TextBox"
                            MaxLength="8"></asp:TextBox>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfEmpId" runat="server" />
                        <asp:HiddenField ID="hfCompCode" runat="server" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="txtEmploeeId"
                            ErrorMessage="Employee is required" ForeColor="Red" ValidationGroup="Add">*</asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 300px">
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Roster"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlRosterID" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvRoster" runat="server" ControlToValidate="ddlRosterID"
                            ErrorMessage="Roster is required" ForeColor="Red" ValidationGroup="Add">*</asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 300px">
                        <asp:Label ID="lblCompany" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <div>
                            <asp:Calendar ID="calRoster" runat="server" Width="256px" BackColor="#BAC0C9" OnSelectionChanged="calRoster_SelectionChanged">
                            </asp:Calendar>
                        </div>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td style="width: 300px">                        
                        <div>
                            <span class="style1"><b>Exclude Days</b></span><br /><br />
                            <asp:CheckBox ID="chkSunday" runat="server" Text="Sunday" BackColor="#FFE391" /><br />
                            <asp:CheckBox ID="chkMonday" runat="server" Text="Monday" /><br />
                            <asp:CheckBox ID="chkTuesday" runat="server" Text="Tuesday" /><br />
                            <asp:CheckBox ID="chkWednesday" runat="server" Text="Wednesday" /><br />
                            <asp:CheckBox ID="chkThursday" runat="server" Text="Thursday" /><br />
                            <asp:CheckBox ID="chkFriday" runat="server" Text="Friday" /><br />
                            <asp:CheckBox ID="chkSaturday" runat="server" Text="Saturday" BackColor="#FFE391" /><br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="125px" ValidationGroup="Add"
                            Style="height: 26px" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Deselect" Width="125px" Style="height: 26px"
                            OnClick="btnClear_Click" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td style="width: 300px">
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <asp:ValidationSummary ID="vsRoster" runat="server" ValidationGroup="Add" ForeColor="#FF3300" />
            <asp:GridView Style="margin-left: 200px" ID="gvRoster" runat="server" AutoGenerateColumns="False"
                OnRowDataBound="gvRoster_RowDataBound" AllowPaging="True" 
                onpageindexchanging="gvRoster_PageIndexChanging" PageSize="20">
                <Columns>
                    <asp:BoundField DataField="ROSTR_ID" HeaderText="ROSTR_ID" />
                    <asp:BoundField DataField="ROSTR_TIME" HeaderText="ROSTR_TIME" />
                    <asp:BoundField DataField="DUTY_DATE" HeaderText="DUTY_DATE" />
                    <asp:BoundField DataField="SYSTEM_FEEDBACK" HeaderText="SYSTEM_FEEDBACK" />
                    <asp:TemplateField HeaderText="IS_EXCLUDE">
                        <ItemTemplate>
                            <asp:CheckBox ID="EXCLUDE" runat="server" Checked='<%# bool.Parse(Eval("IS_EXCLUDE").ToString() == "1" ? "True": "False") %>'
                                Enabled="true" AutoPostBack="true" OnCheckedChanged="EXCLUDE_OnCheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <table style="height: 40px; margin-top: -10px">
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: 250px">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td style="width: 300px">
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left; width: 255px">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="Add"
                            Style="height: 26px" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" Width="125px" Style="height: 26px"
                            OnClick="btnClean_Click" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td style="width: 300px">
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblMessage" runat="server" Text="" style="margin-left:200px"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <span>Rosters </span>Saved
            <hr />
            <asp:GridView style="margin-left:200px" ID="gvSavedRosters" runat="server" 
                AutoGenerateColumns="False" AllowPaging="True" 
                onpageindexchanging="gvSavedRosters_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ROSTR_ID" HeaderText="ROSTR_ID" />
                    <asp:BoundField DataField="ROSTR_TIME" HeaderText="ROSTR_TIME" />
                    <asp:BoundField DataField="DUTY_DATE" HeaderText="DUTY_DATE" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
