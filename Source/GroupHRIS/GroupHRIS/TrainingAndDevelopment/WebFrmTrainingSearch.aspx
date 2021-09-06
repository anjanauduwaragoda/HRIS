<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmTrainingSearch.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

        function sendValueToParent_() {
            var sRetVal = document.getElementById("txtTrId").value;

            window.opener.getValueFromChild(sRetVal);

            window.close();
        }

        function sendValueToParent() {
            var sTrId = document.getElementById("txtTrId").value;
            var sTrName = document.getElementById("hfTrainingName").value;
            var sProName = document.getElementById("hfProgram").value;
            //                      alert(sTrId);
            window.opener.getValueFromChild(sTrId, sTrName, sProName);

            window.close();
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 21px;
        }
    </style>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table width="100%">
        <tr>
            <td>
                <span style="font-weight: 700">Training</span>
                <hr />
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;">
                            Training Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td style="text-align: right;">
                            Training Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtTraining" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            Training Start Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtStDate" runat="server" Width="200px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftetendedDate" FilterType="Custom, Numbers" ValidChars="/,-"
                                runat="server" TargetControlID="txtStDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="ceextendedDate" runat="server" TargetControlID="txtStDate"
                                Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                        </td>
                        <td style="text-align: right;">
                            Training End Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server" Width="200px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers"
                                ValidChars="/,-" runat="server" TargetControlID="txtEndDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                        </td><td><asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                OnClick="btnSearch_Click" /></td>
                    </tr>
                    <tr>
                        
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                        <td class="style1">
                        </td>
                        <td class="style1">
                        </td>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSelect" runat="server" Text="&lt;&lt; Select" Width="100px" OnClientClick="sendValueToParent()"
                                Visible="False"/>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:TextBox ID="txtTrId" style="text-align: right;" runat="server" Width="200px" BorderStyle="None" Font-Bold="True"
                                ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTrainingName" runat="server" Width="200px" BorderStyle="None"
                                Font-Bold="True" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
                <table style="margin:auto; width:100%">
                    <tr>
                        <td style="text-align:center;">
                            <asp:GridView ID="grdTrainingDetails" runat="server" style="Width:100%" AutoGenerateColumns="false"
                                AllowPaging="True" OnRowDataBound="grdTrainingDetails_RowDataBound" OnSelectedIndexChanged="grdTrainingDetails_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="TRAINING_ID" HeaderText="Training ID" />
                                    <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Program Name" />
                                    <asp:BoundField DataField="TRAINING_CODE" HeaderText="Training Code" />
                                    <asp:BoundField DataField="TRAINING_NAME" HeaderText="Training" />
                                    <asp:BoundField DataField="TYPE_NAME" HeaderText="Training Type" />
                                    <asp:BoundField DataField="PLANNED_START_DATE" HeaderText="Planned Start Date">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PLANNED_END_DATE" HeaderText="Planned End Date">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PLANNED_PARTICIPANTS" HeaderText="Planned Participant">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PLANNED_TOTAL_HOURS" HeaderText="Planned Total Hours">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

    <div style="text-align: center;">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:HiddenField ID="hfTrainingName" runat="server" />
        <asp:HiddenField ID="hfProgram" runat="server" />
    </div>
    </form>
</body>
</html>
