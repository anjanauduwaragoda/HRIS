<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployeeLeaveSheet.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.webFrmEmployeeLeaveSheet"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        //        function openLOVWindow(file, window, ctlName) {
        //            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
        //            //if (childWindow.opener == null) childWindow.opener = self;

        //            document.getElementById("hfCaller").value = ctlName;
        //        }

        //        function getValueFromChild(sRetVal) {

        //            var ctl = document.getElementById("hfCaller").value;
        //            document.getElementById(ctl).value = sRetVal;


        //        }

        //        function getValueFromChild(sEmpId, sName, sCompanyId, sDepartmentId, sDivisionId) {
        //            var ctl = document.getElementById("hfCaller").value;
        //            document.getElementById(ctl).value = sEmpId;

        //            DoPostBack();
        //        }

        //        function DoPostBack() {
        //            __doPostBack();
        //        }
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Employee Leave Sheet</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Employee Id :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" CssClass="styleTableCell2TextBox" MaxLength="8"
                            ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <asp:image alt="" runat= "server" ID="Search" src="../Images/Common/Search.jpg"  height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee Id is required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblEmployeeName" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfEmpId" runat="server" />
                        <asp:HiddenField ID="hfLeaveSheetId" runat="server" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
            </table>
            <span>Leave Summary</span>
            <hr />
            <asp:GridView ID="gvLeaveSummary" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="LEAVE TYPE NAME" />
                    <asp:BoundField DataField="NUMBER_OF_DAYS" HeaderText="NUMBER OF LEAVE">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="leaves_taken" HeaderText="LEAVE TAKEN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Leves_Remain" HeaderText="LEAVE REMAIN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <span>Enter Leave Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label5" runat="server" Text="Covered By :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtCoveredBy" runat="server" ReadOnly="true"    
                            CssClass="styleTableCell2TextBox" MaxLength="8"
                            ClientIDMode="Static" TabIndex="1"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtCoveredBy')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvCoveredBy" runat="server" ErrorMessage="Covered by is required"
                            ControlToValidate="txtCoveredBy" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblCoveredByName" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label6" runat="server" Text="Recommend By :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtRecommendBy" runat="server" ReadOnly="true" CssClass="styleTableCell2TextBox"
                            MaxLength="8" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <%--<img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtRecommendBy')" />--%>
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvRecommendBy" runat="server" ErrorMessage="Recommand by Required"
                            ControlToValidate="txtRecommendBy" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblRecomendedByName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
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
                    <td style="width: 20px" class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvFDate" runat="server" ErrorMessage="From Date is required"
                            ControlToValidate="txtFDate" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
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
                    <td style="width: 20px" class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvTDate" runat="server" ErrorMessage="To Date is required"
                            ControlToValidate="txtTDate" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Reason :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250" MaxLength="150"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ErrorMessage="Reason is required"
                            ControlToValidate="txtReason" ValidationGroup="addGrid" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td >
                    </td>
                </tr>


                <tr class="styleTableRow">
                    <td class="styleTableCell1">                        
                    </td>
                    <td class="styleTableCell2" style="text-align: left"> 
                        <asp:Label ID="Label9" runat="server" Text="Half Day / Short Leave" Font-Bold="true" ForeColor= "Blue"></asp:Label>                       
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">                        
                    </td>
                    <td >
                    </td>
                </tr>                
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label3" runat="server" Text="Is Half Day :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:CheckBox ID="chkhalfDay" runat="server" Text=" " AutoPostBack="True" 
                            oncheckedchanged="chkhalfDay_CheckedChanged" />
                        <asp:RadioButton ID="rbM" runat="server" AutoPostBack="True" 
                            oncheckedchanged="rbM_CheckedChanged" Text="First Half" Font-Bold= "true" 
                            ForeColor="Orange"  />
                        <asp:RadioButton ID="rbE" runat="server" AutoPostBack="True" 
                            oncheckedchanged="rbE_CheckedChanged" Text="Second Half" Font-Bold= "true" 
                            ForeColor="Brown"/>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label7" runat="server" Text="Is Short Leave :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:CheckBox ID="chkSL" runat="server" Text="" AutoPostBack="True" 
                            oncheckedchanged="chkSL_CheckedChanged" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>




                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblSelectRoster" runat="server" Text="Select Roster:" 
                            Visible="False"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlRoster" runat="server" Width="256px" Visible="False">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">                       
                    </td>
                    <td >
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="lblWorkingTime" runat="server" Text="Working Time:" 
                            Visible="False"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Label ID="lblCompanyTime" runat="server" Text="" 
                            Visible="False"></asp:Label>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>




                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label8" runat="server" Text="From :"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlFromHH" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlFromMM" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                        <span style="width:50px">To</span>
                        <asp:DropDownList ID="ddlToHH" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToMM" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">                        
                    </td>
                    <td class="styleTableCell2" style="text-align: left">                        
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" 
                            ValidationGroup="addGrid" style="height: 26px" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
                <tr class="styleTableRow">
                    
                    <td colspan="5" style="height: 10px;text-align:center">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td colspan="4" style="height: 10px">
                        <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td colspan="4" style="height: 10px; color: #FF0000;">
                        <span> Note: If leave count is more than 2.5 Days please select Annual Leaves</span>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vsApply" runat="server" ForeColor="Red" ValidationGroup="addGrid" />           
            <table style ="height:40px;margin:auto;margin-top:-10px">
                <tr>
                    <td>
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin:auto;width:200px" >
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>        
            <asp:GridView ID="gvLeaveSheet" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvLeaveSheet_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID"  HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LEAVE_DATE" HeaderText="LEAVE_DATE" />
                    <asp:TemplateField HeaderText="LEAVE_TYPE">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlLeaveType" runat="server" AutoPostBack="True" Width="150px"
                                OnSelectedIndexChanged="ddlLeaveType_Update">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ROSTR_ID" HeaderText="ROSTR_ID" />
                    <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_LEAVE" />
                    <asp:BoundField DataField="FROM_TIME" HeaderText="FROM_TIME" />
                    <asp:BoundField DataField="TO_TIME" HeaderText="TO_TIME" />
                    <asp:BoundField DataField="COVERED_BY" HeaderText="COVERED_BY" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="RECOMMEND_BY" HeaderText="RECOMMEND_BY" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                         <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                    </asp:BoundField>
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
                    <asp:BoundField DataField="COM_ROS_TIME" HeaderText="WORKING TIME" />
                    <asp:BoundField DataField="STATUS" HeaderText="STATUS" />
                </Columns>
            </asp:GridView>
            <br />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td style="text-align: left; width: 125px">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click" />
                    </td>
                    <td style="text-align: left; width: 125px">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" 
                            onclick="btnClear_Click" style="height: 26px" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td >
                    </td>
                </tr>
            </table>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <span>Leave History (<strong>MAKE_INACTIVE</strong> is Only for <strong>Managers</strong>/<strong>HR 
            Staff</strong>)</span>
            <hr />
            <asp:GridView ID="gvLeaveHistory" runat="server" AutoGenerateColumns="False" 
                OnRowDataBound="gvLeaveHistory_RowDataBound" 
                onselectedindexchanged="gvLeaveHistory_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="LEAVE_SHEET_ID" HeaderText="LEAVE_SHEET_ID" />
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" />
                    <asp:BoundField DataField="FROM_DATE" HeaderText="FROM_DATE" />
                    <asp:BoundField DataField="TO_DATE" HeaderText="TO_DATE" />
                    <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_LEAVE" />
                    <asp:BoundField DataField="LEAVE_STATUS" HeaderStyle-CssClass="hideGridColumn" 
                        HeaderText="STATUS_CODE" ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn" />
                    <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STATUS_CODE" HeaderText="LEAVE_STATUS" />
                    <asp:TemplateField HeaderText="MAKE_INACTIVE">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDiscard" runat="server" AutoPostBack="true" 
                                OnCheckedChanged="chkDiscard_OnCheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblLSDetails" runat="server" Text="Leave Sheet Details"></asp:Label>
            <hr />
            <asp:GridView ID="gvLeaveSheetDetails" runat="server">
            </asp:GridView>
            <br />
            <asp:LinkButton ID="lbtnClear" runat="server" onclick="lbtnClear_Click">Clear Leave Sheet Details</asp:LinkButton>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
            <br/>
                        
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>--%>
</asp:Content>
