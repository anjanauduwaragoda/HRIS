<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="EmployeeOnlineLeave.aspx.cs" Inherits="GroupHRIS.EmployeeProfile.EmployeeOnlineLeave"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        
        function openLOVWindow(file, window, ctlName , mtext) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfcoveredbyid").value = ctlName;
            document.getElementById("hfCaller").value = mtext;
        }

        function getValueFromChild(sEmpId, sName) {

            if (document.getElementById("hfCaller").value == 1) {

                document.getElementById("txtCoveredBy").value = sEmpId;
                document.getElementById("txtcoveredbyname").value = sName;

               writeToHiddenFieldsCover(sEmpId, sName);
            }

            if (document.getElementById("hfCaller").value == 2) {

                document.getElementById("txtRecommendBy").value = sEmpId;
                document.getElementById("txtrecbybyname").value = sName;

               writeToHiddenFieldsRecom(sEmpId, sName);
            }
//            DoPostBack();
        }

        function writeToHiddenFieldsCover(sEmpId, sName) {
            document.getElementById("hfcoveredbyid").value = sEmpId;
            document.getElementById("hfcoveredbyname").value = sName;

        }

        function writeToHiddenFieldsRecom(sEmpId, sName) {
            document.getElementById("hfrecombyid").value = sEmpId;
            document.getElementById("hfrecombyname").value = sName;

        }

        function DoPostBack() {
            __doPostBack("txtCoveredBy", "TextChanged");
        }

    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style2
        {
            width: 300px;
            text-align: left;
        }
        .style3
        {
            width: 134px;
            text-align: right;
        }
        .style8
        {
            height: 39px;
        }
        .style9
        {
            height: 22px;
        }
        .style22
        {
            width: 16px;
            text-align: center;
        }
        .style23
        {
            width: 16px;
        }
        .style24
        {
            text-align: right;
        }
        .style26
        {
            width: 235px;
            text-align: right;
        }
        .style27
        {
            width: 235px;
        }
        .style28
        {
            color: #FF3300;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <table class="styleTabaleMain">
                <tr>
                    <td>
                        <table width="100%">
                            <table>
                                <tr class="styleTableRow">
                                    <td class="style24" style="text-align: right" colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26" style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Covered By :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2">
                                        <asp:TextBox ID="txtCoveredBy" runat="server" ClientIDMode="Static" CssClass="styleTableCell2TextBox" MaxLength="8"></asp:TextBox>
                                    </td>
                                    <td class="styleTableCell3">
                                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtCoveredBy','1')" />
                                    </td>
                                    <td class="style23">
                                        <asp:RequiredFieldValidator ID="rfvCoveredBy" runat="server" ControlToValidate="txtCoveredBy"
                                            ErrorMessage="Covered by Required" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style2">
                                        <asp:TextBox ID="txtcoveredbyname" runat="server" BorderStyle="None" ClientIDMode="Static"
                                            ForeColor="#3333FF" ReadOnly="True" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26" align="right" style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Recommend By :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2">
                                        <asp:TextBox ID="txtRecommendBy" runat="server" CssClass="styleTableCell2TextBox" MaxLength="8" ClientIDMode="Static" ></asp:TextBox>
                                    </td>
                                    <td class="styleTableCell3">
                                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtRecommendBy','2')" />
                                    </td>
                                    <td class="style22">
                                        <asp:RequiredFieldValidator ID="rfvRecommendBy" runat="server" ErrorMessage="RecommendBy is required"
                                            ControlToValidate="txtRecommendBy" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style2">
                                        <asp:TextBox ID="txtrecbybyname" runat="server" BorderStyle="None" ClientIDMode="Static"
                                        ForeColor="#FF6600" ReadOnly="True" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="width: 490px">
                                    <td style="text-align: right" class="style27">
                                        <asp:Label ID="Label34" runat="server" Text="From Date :"></asp:Label>
                                    </td>
                                    <td style="width: 250px; text-align: right">
                                        <asp:TextBox ID="txtFDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                                        <asp:CalendarExtender ID="ceFDate" runat="server" TargetControlID="txtFDate" Format="yyyy/MM/dd">
                                        </asp:CalendarExtender>
                                        <!--<asp:FilteredTextBoxExtender ID="fteFDate" runat="server" TargetControlID="txtFDate" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender>-->
                                    </td>
                                    <td style="width: 20px">
                                        &nbsp;
                                    </td>
                                    <td class="style23">
                                        <asp:RequiredFieldValidator ID="rfvFDate" runat="server" ErrorMessage="From Date is required"
                                            ControlToValidate="txtFDate" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="txtEmploeeId" runat="server" BorderStyle="None" ClientIDMode="Static"
                                            CssClass="styleTableCell2TextBox" ForeColor="White" MaxLength="8" ReadOnly="True"
                                            Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="width: 490px">
                                    <td style="text-align: right" class="style27">
                                        <asp:Label ID="Label35" runat="server" Text="To Date :"></asp:Label>
                                    </td>
                                    <td style="width: 250px; text-align: right">
                                        <asp:TextBox ID="txtTDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                                        <asp:CalendarExtender ID="ceTDate" runat="server" TargetControlID="txtTDate" Format="yyyy/MM/dd">
                                        </asp:CalendarExtender>
                                        <asp:FilteredTextBoxExtender ID="fteTDate" runat="server" TargetControlID="txtTDate"
                                            FilterType="Custom, Numbers" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td class="style23">
                                        <asp:RequiredFieldValidator ID="rfvTDate" runat="server" ErrorMessage="To Date is required"
                                            ControlToValidate="txtTDate" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style3" rowspan="6">
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26">
                                        <asp:Label ID="Label8" runat="server" Text="Half Day / Short Leave From :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2" style="text-align: left">
                                        <asp:DropDownList ID="ddlFromHH" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlFromMM" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        <span>To</span>
                                        <asp:DropDownList ID="ddlToHH" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlToMM" runat="server" CssClass="styleDdlhh">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleTableCell3">
                                    </td>
                                    <td class="style22">
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26">
                                        <asp:Label ID="Label3" runat="server" Text="Is Half Day :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2" style="text-align: left">
                                        <asp:CheckBox ID="chkhalfDay" runat="server" Text="" />
                                    </td>
                                    <td class="styleTableCell3">
                                    </td>
                                    <td class="style22">
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26">
                                        <asp:Label ID="Label7" runat="server" Text="Is Short Leave :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2" style="text-align: left">
                                        <asp:CheckBox ID="chkSL" runat="server" Text="" />
                                    </td>
                                    <td class="styleTableCell3">
                                    </td>
                                    <td class="style22">
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26">
                                        <asp:Label ID="Label2" runat="server" Text="Reason :"></asp:Label>
                                    </td>
                                    <td class="styleTableCell2" style="text-align: left">
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td class="styleTableCell3">
                                    </td>
                                    <td class="style22">
                                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ErrorMessage="Reason is required"
                                            ControlToValidate="txtReason" ValidationGroup="addGrid" ForeColor="Red" 
                                            style="text-align: center">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="styleTableRow">
                                    <td class="style26">
                                    </td>
                                    <td class="styleTableCell2" style="text-align: left">
                                        <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" 
                                            ValidationGroup="addGrid" style="height: 26px" />
                                    </td>
                                    <td class="styleTableCell3">
                                    </td>
                                    <td class="style22">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style8">
                                        &nbsp;</td>
                                    <td align="center" class="style8">
                                        &nbsp;</td>
                                    <td align="center" class="style8">
                                        &nbsp;</td>
                                    <td align="center" class="style8">
                                        &nbsp;</td>
                                    <td align="center" class="style8">
                                        <span class="style28">Note: If leave count is more than 2.5 Days please select 
                                        Annual Leaves</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style8" colspan="5">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style9" colspan="5">
                                        <asp:ValidationSummary ID="vsApply0" runat="server" ForeColor="Red" 
                                            ValidationGroup="addGrid" style="text-align: left" />
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
            <table style="height: 40px; margin: auto; margin-top: -10px">
                <%--<tr>
                    <td>
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: 200px">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" alt= "" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>--%>
            </table>
            <asp:GridView ID="gvLeaveSheet" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvLeaveSheet_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" />
                    <asp:BoundField DataField="LEAVE_DATE" HeaderText="LEAVE_DATE" />
                    <asp:TemplateField HeaderText="LEAVE_TYPE">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlLeaveType" runat="server" AutoPostBack="True" Width="150px"
                                OnSelectedIndexChanged="ddlLeaveType_Update">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ROSTR_ID" HeaderText="ROSTR_ID" />
                    <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_DAYS" />
                    <asp:BoundField DataField="FROM_TIME" HeaderText="FROM_TIME" />
                    <asp:BoundField DataField="TO_TIME" HeaderText="TO_TIME" />
                    <asp:BoundField DataField="COVERED_BY" HeaderText="COVERED_BY" />
                    <asp:BoundField DataField="RECOMMEND_BY" HeaderText="RECOMMEND_BY" />
                    <asp:BoundField DataField="SCHEME_LINE_NO" HeaderText="SCHEME_LINE_NO" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IS_HALFDAY" HeaderText="IS_HALFDAY" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LEAVE_STATUS" HeaderText="LEAVE_STATUS" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="EXCLUDE">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIS_DAY_OFF" runat="server" Checked='<%# bool.Parse(Eval("IS_DAY_OFF").ToString() == "1" ? "True": "False") %>'
                                Enabled="true" AutoPostBack="true" OnCheckedChanged="chkIS_DAY_OFF_OnCheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <table width="100%">
                <tr>
                    <td style="text-align: right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click" />
                    </td>
                    <td style="text-align: left">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" OnClick="btnClear_Click" />
                    </td>
                </tr>
            </table>
            <span style="font-weight: 700">Leave History</span>
            <hr />
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvLeaveHistory" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvLeaveHistory_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="LEAVE_SHEET_ID" HeaderText="LEAVE_SHEET_ID" />
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" />
                                <asp:BoundField DataField="FROM_DATE" HeaderText="FROM_DATE" />
                                <asp:BoundField HeaderText="TO_DATE" DataField="TO_DATE" />
                                <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_DAYS" />
                                <asp:BoundField DataField="LEAVE_STATUS" HeaderText="STATUS_CODE" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="LEAVE_STATUS" />
                                <asp:TemplateField HeaderText="MAKE_INACTIVE" 
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-Width="0px" Visible="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDiscard" runat="server" AutoPostBack="true" OnCheckedChanged="chkDiscard_OnCheckedChanged" Visible="false" Width="0px"/>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="hideGridColumn" />
                                    <ItemStyle Width="0px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvLeaveSummary" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="LEAVE TYPE NAME" />
                    <asp:BoundField DataField="NUMBER_OF_DAYS" HeaderText="NUMBER OF DAYS">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="leaves_taken" HeaderText="LEAVES TAKEN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Leves_Remain" HeaderText="LEAVES REMAIN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblLSDetails" runat="server" Text="Leave Sheet Details"></asp:Label>
            <asp:GridView ID="gvLeaveSheetDetails" runat="server">
            </asp:GridView>
            <asp:LinkButton ID="lbtnClear" runat="server" onclick="lbtnClear_Click">Clear Leave Sheet Details</asp:LinkButton>
            <br />
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfEmpId" runat="server" />
            <asp:HiddenField ID="hfLeaveSheetId" runat="server" />
            <asp:HiddenField ID="hfcoveredbyname" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfcoveredbyid" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfrecombyid" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfrecombyname" runat="server" ClientIDMode="Static" />
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
