<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEducationVerification.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEducationVerification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableEventValidation="false">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfCaller").value = ctlName;
        }


        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("txtEmployeeID").value = sEmpId;
            document.getElementById("txtName").value = sName;

            writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName);

            DoPostBack();
        }

        function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
        }

        function DoPostBack() {
            __doPostBack("txtEmployeeID", "TextChanged");
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
    <br />
    <span style="font-weight: 700">HRIS Employee Education Verification</span>
    <hr />
    <br />
    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" Text="Employee" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left" colspan="2">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                &nbsp;
                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" />
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="*" ForeColor="Red" ValidationGroup="vgSubmit"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"
                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td colspan="2">
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfLineNo" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <span><strong>Secondary Education</strong></span> :</td>
            <td colspan="2">
                <asp:Label ID="Label3" runat="server" ForeColor="#0000CC" 
                    Text="G.C.E. (O/L) Examination"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <span><strong>Attempt</strong></span> :</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlOLAttempt" runat="server" Width="54px" 
                    AutoPostBack="True" onselectedindexchanged="ddlOLAttempt_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;
            </td>
            <td colspan="2">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
        <asp:GridView ID="gvSecEduOL" runat="server" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" ItemStyle-Width="120px"/>
                <asp:BoundField DataField="IS_AL" HeaderText="is AL?" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="ATTEMPTED_YEAR" HeaderText="Year" ItemStyle-Width="120px" />
                <asp:BoundField DataField="SCHOOL" HeaderText="School" ItemStyle-Width="300px" />
                <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" ItemStyle-Width="250px" />
                <asp:BoundField DataField="GRADE" HeaderText="Grade" ItemStyle-Width="120px" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
            <td>
                <asp:RadioButtonList ID="rdol" runat="server" ForeColor="#FF3300" 
                    RepeatDirection="Horizontal" Visible="False" AutoPostBack="True" 
                    Font-Bold="False" onselectedindexchanged="rdol_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="1">Verify Details</asp:ListItem>
                    <asp:ListItem Value="9">Reject Details</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:Button ID="btnol" runat="server" onclick="btnol_Click" Text="Save" 
                    Visible="False" Width="100px" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Rejected Reason" Visible="False"></asp:Label> 
            </td>
            <td >
                <asp:TextBox ID="txtRejectedReason" runat="server" TextMode="MultiLine" 
                    Visible="False" Width="225px" MaxLength="200"></asp:TextBox >
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;
            </td>
            <td colspan="2">
                <asp:Label ID="lblMsgOL" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <span><strong>Secondary Education</strong></span> :
            </td>
            <td colspan="2">
                <asp:Label ID="Label4" runat="server" ForeColor="#0000CC" 
                    Text="G.C.E. (A/L) Examination"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <span><strong>Attempt</strong></span> :
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlALAttempt" runat="server" Width="54px" 
                    onselectedindexchanged="ddlALAttempt_SelectedIndexChanged" 
                    style="height: 22px" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left" colspan="4">
        <asp:GridView ID="gvSecEduAL" runat="server" AutoGenerateColumns="False" Width="100%" >
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" ItemStyle-Width="120px"/>
                <asp:BoundField DataField="IS_AL" HeaderText="is AL?" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="ATTEMPTED_YEAR" HeaderText="Year" ItemStyle-Width="120px" />
                <asp:BoundField DataField="SCHOOL" HeaderText="School" ItemStyle-Width="300px" />
                <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" ItemStyle-Width="250px" />
                <asp:BoundField DataField="GRADE" HeaderText="Grade" ItemStyle-Width="120px" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                <asp:RadioButtonList ID="rdal" runat="server" ForeColor="#FF3300" 
                    RepeatDirection="Horizontal" Visible="False" AutoPostBack="True" 
                    Font-Bold="False" onselectedindexchanged="rdal_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="1">Verify Details</asp:ListItem>
                    <asp:ListItem Value="9">Reject Details</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td align="left">
                <asp:Button ID="btnal" runat="server" onclick="btnal_Click" Text="Save" 
                    Visible="False" Width="100px" />
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" Text="Rejected Reason" Visible="False"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAlRejectedReason" runat="server" TextMode="MultiLine" 
                    Visible="False" Width="225px" MaxLength="200"></asp:TextBox >
            </td>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                <asp:Label ID="lblMsgAL" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
        </tr>
    </table>
    <br />
    
    <span><strong>Higher Education</strong></span>
    <br />
    <br />
    <div>
        <table>
    <tr>
        <td>
            <asp:GridView ID="gvHighEdu" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" OnPageIndexChanging="gvHighEdu_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="Institute" DataField="INSTITUTE" ItemStyle-Width="300px" />
                <asp:BoundField HeaderText="Program" DataField="PROGRAM" ItemStyle-Width="200px" />
                <asp:BoundField HeaderText="Program Name" DataField="PROGRAME_NAME" ItemStyle-Width="200px" />
                <asp:BoundField HeaderText="Sector" DataField="SECTOR" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="Duration" DataField="DURATION" ItemStyle-Width="100px"/>
                <asp:BoundField HeaderText="From" DataField="FROM_YEAR" ItemStyle-Width="80px" />
                <asp:BoundField HeaderText="To" DataField="TO_YEAR" ItemStyle-Width="80px" />
                <asp:BoundField HeaderText="Grade" DataField="GRADE" ItemStyle-Width="150px" />
                <asp:BoundField HeaderText="Remarks" DataField="REMARKS" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn" />
                <asp:TemplateField HeaderText="Accept">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkdelete" runat="server" OnClick="lnkHighVeryfy_Click">Accept</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reject">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkreject" runat="server" OnClick="lnkHighReject_Click">Reject</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlRejectReason" Visible="false" Width="100%" runat="server">
        <table style="margin:auto;">
            <tr>
                <td style="vertical-align:text-top;">Reject Reason</td>
                <td style="vertical-align:text-top;">:</td>
                <td >
                    <asp:TextBox ID="txtRejectReason" TextMode="MultiLine" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="vertical-align:text-top;"></td>
                <td style="vertical-align:text-top;"></td>
                <td style="vertical-align:text-top;text-align:right;">
                    <asp:Button ID="btnRejectSave" runat="server" Text="Reject" 
                        onclick="btnRejectSave_Click" />
                    <asp:Button ID="btnRejectClear" runat="server" Text="Clear" 
                        onclick="btnRejectClear_Click" />
                    <asp:Button ID="btnRejectCancel" runat="server" Text="Cancel" 
                        onclick="btnRejectCancel_Click" />
                </td>
            </tr>
        </table>
        </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <table class="styleMainTb">
        <tr>
            <td align="center" style="min-width:700px;">
                <asp:Label ID="lblMsgHr" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
    </table>
        </td>
    </tr>
    </table>     
    </div>    
</asp:Content>
