<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PreviousEmploymentVerification.aspx.cs" Inherits="GroupHRIS.Employee.PreviousEmploymentVerification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Previous Experience Verification</span>
    <hr />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <table style="margin: auto">
                <tr>
                    <td style="text-align: right;">
                        Employee
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtemployee" runat="server" ClientIDMode="Static" ReadOnly="true"
                            Width="200px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtemployee')" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Company</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" Width="200px" runat="server" 
                            onselectedindexchanged="ddlCompany_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Department</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlDepartment" Width="200px" runat="server" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlDepartment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Division</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlDivision" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Experience Status
                    </td>
                    <td>
                        :
                    </td>
                    <td><%--
                        <asp:DropDownList ID="ddlExperienceStatus" Width="200px" runat="server" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlExperienceStatus_SelectedIndexChanged">--%>
                        <asp:DropDownList ID="ddlExperienceStatus" Width="200px" runat="server" AutoPostBack="True" >
                        </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="Experience Status is Required" ForeColor="Red" Text="*" ControlToValidate="ddlExperienceStatus" ValidationGroup="Filter"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" Width="100px" ValidationGroup="Filter" runat="server" Text="Show" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td style="width:700px;" colspan="4">
                    <%--Employee
                    <hr />--%>
                        <asp:GridView ID="grdvEmployee" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                            AllowPaging="true" BorderColor="#0099FF" BorderStyle="Groove" BorderWidth="2px"
                            CellPadding="3" PageSize="10" CellSpacing="1" ForeColor="Black" OnRowDataBound="grdvEmployee_RowDataBound"
                            OnSelectedIndexChanged="grdvEmployee_SelectedIndexChanged" OnPageIndexChanging="grdvEmployee_PageIndexChanging">
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="#0099FF" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#CCCCCC" BorderColor="#0099FF" BorderWidth="1px" ForeColor="Black"
                                HorizontalAlign="Left" />
                            <RowStyle BackColor="White" BorderColor="#3366FF" BorderWidth="1px" />
                            <SelectedRowStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                    HeaderText="EMPLOYEE ID" >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EPF_NO" HeaderText="EPF NO" />
                                <asp:BoundField DataField="INITIALS_NAME" HeaderText="NAME" />
                                <asp:BoundField DataField="COMPANY" HeaderText="COMPANY" />
                                <asp:BoundField DataField="DEPARTMENT" HeaderText="DEPARTMENT" />
                                <asp:BoundField DataField="DIVISION" HeaderText="DIVISION" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                <td colspan="4"></td>
                </tr>
                <tr>
                    <td style="width:700px;" colspan="4">
                    <asp:Label ID="lblPreviousExperience" runat="server" Visible="false" Text="Previous Experience"></asp:Label>

                    <hr />
                        <asp:GridView ID="grdvPreviousEmployment" runat="server" AutoGenerateColumns="False"
                            BackColor="#CCCCCC" AllowPaging="True" BorderColor="#0099FF" BorderStyle="Groove"
                            BorderWidth="2px" CellPadding="3" CellSpacing="1" ForeColor="Black" OnRowDataBound="grdvPreviousEmployment_RowDataBound">
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="#0099FF" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#CCCCCC" BorderColor="#0099FF" BorderWidth="1px" ForeColor="Black"
                                HorizontalAlign="Left" />
                            <RowStyle BackColor="White" BorderColor="#3366FF" BorderWidth="1px" />
                            <SelectedRowStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="LINE_NO" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                    HeaderText="LINE NO">
                                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESIGNATION" HeaderText="DESIGNATION" />
                                <asp:BoundField DataField="ORGANIZATION" HeaderText="ORGANIZATION" />
                                <asp:BoundField DataField="FROM_DATE" HeaderText="FROM DATE" />
                                <asp:BoundField DataField="TO_DATE" HeaderText="TO DATE" />
                                <asp:BoundField DataField="PHONE_NUMBER" HeaderText="PHONE NUMBER" />
                                <asp:BoundField DataField="ADDRESS" HeaderText="ADDRESS" />
                                <asp:TemplateField HeaderText="VERIFICATION STATUS">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rdVerify" Text="Accept" AutoPostBack="true" GroupName="rdBtn"
                                                        runat="server" OnCheckedChanged="rdVerify_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdReject" Text="Reject" GroupName="rdBtn" AutoPostBack="true"
                                                        runat="server" OnCheckedChanged="rdReject_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2'>                                                
                                                    <asp:CheckBox ID="chkServiceLetter" AutoPostBack="true" Text="Verified By Service Letter" Visible="false" runat="server" oncheckedchanged="chkServiceLetter_CheckedChanged" />
                                                    <asp:TextBox ID="txtRejectReason" placeholder="Enter Reject Reason Here ..." TextMode="MultiLine" MaxLength="150" Visible="false" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtRejectReason" ValidationGroup="Main" runat="server" ForeColor="Red" Text="Reject Reason is Required" ErrorMessage="Reject Reason is Required"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        <table style="margin:auto;">
                        <tr>
                        <td><asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Filter" ForeColor="Red" runat="server" /></td>
                        </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" ValidationGroup="Main" Visible="false" runat="server" Width="100px" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClearAll" runat="server" Width="100px" Visible="false" Text="Clear" OnClick="btnClearAll_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>