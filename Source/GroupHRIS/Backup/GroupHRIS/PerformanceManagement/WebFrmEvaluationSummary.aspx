<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmEvaluationSummary.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmEvaluationSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supervisor Competency Assessment</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Charts/Chart.bundle.js" type="text/javascript"></script>
    <script src="../Scripts/Charts/jquery.min.js" type="text/javascript"></script>
    <style>
    canvas{
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
    }
    </style>
    <script language="javascript" type="text/javascript">
        function changeScreenSize() {
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(200, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }
    </script>
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        
        .InformationBox
        {
            background: rgb(165,200,255);
        }
        .Question
        {
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(165,204,255);
        }
        .Answer
        {
            padding-left: 20px;
            padding-right: 10px;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-top: 5px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(216,216,216);
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body onload="changeScreenSize()">
            <div class="Title">
                <span style="font-weight: 700">Performance Evaluation</span>
            </div>
    
            <table style="margin: auto; width: 800px;">
                <tr>
                    <td><asp:Label ID="lblGoalChart" runat="server" ></asp:Label></td>
                </tr>
            </table>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfAssessmentID" runat="server" />
            <asp:HiddenField ID="hfYearOfAssessment" runat="server" />
            <asp:HiddenField ID="hfEmployeeID" runat="server" />
            <div id="dvSupervisor" runat="server">
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td style="width: 50%;">
                            <fieldset style="height: 100px;">
                                <legend>Evaluation Details</legend>
                                <table style="width: 100%; height: 80px;">
                                    <tr>
                                        <td>
                                            <table style="margin: auto;">
                                                <tr>
                                                    <td>
                                                        Evaluation
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEvaluationName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Evaluation Type
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEvaluationType" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="width: 50%;">
                            <fieldset style="height: 100%; height: 100px;">
                                <legend>Assessment Purposes</legend>
                                <table style="width: 100%; height: 80px;">
                                    <tr>
                                        <td>
                                            <table style="margin: auto;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAssessmentPurposes" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td style="width: 50%;">
                            <fieldset style="height: 100px;">
                                <legend>Subordinate Information </legend>
                                <table style="width: 100%; height: 80px;">
                                    <tr>
                                        <td>
                                            <table style="margin: auto;">
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSubordinateName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Designation
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSubordinateDesignation" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="width: 50%;">
                            <fieldset style="height: 100%; height: 100px;">
                                <legend>Supervisor Information </legend>
                                <table style="width: 100%; height: 80px;">
                                    <tr>
                                        <td>
                                            <table style="margin: auto;">
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSupervisorName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Designation
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSupervisorDesignation" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td style="width: 50%;">
                            <fieldset style="height: 200px;">
                                <legend>Proficiency Levels</legend>
                                <br />
                                <table style="width: 100%; height: 80px;">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="width: 100px;">
                                                        <strong>Rating</strong>
                                                    </td>
                                                    <td>
                                                        <strong>Description</strong>
                                                    </td>
                                                </tr>
                                                <asp:Label ID="lblProficiencyLevels" runat="server"></asp:Label>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="width: 50%;">
                            <fieldset style="height: 100%; height: 200px;">
                                <legend>Competency Assessment </legend>
                                <div style="overflow-y: scroll; height: 175px;">
                                    <table style="width: 100%; height: 80px;">
                                        <tr>
                                            <td>
                                                <table style="margin: auto; width: 100%;">
                                                    <tr>
                                                        <th style="text-align: left;">
                                                            Marks
                                                        </th>
                                                        <th style="text-align: right;">
                                                            Subordinate
                                                        </th>
                                                        <th style="text-align: right;">
                                                            Supervisor
                                                        </th>
                                                    </tr>
                                                    <asp:Label ID="lblCompetencyTable" runat="server"></asp:Label>
                                                    <tr>
                                                        <th style="text-align: left;">
                                                            Total
                                                        </th>
                                                        <th style="border-top-style: solid; border-bottom-style: double; border-top-width: 1px;
                                                            border-bottom-width: thick; border-top-color: #000000; border-bottom-color: #000000;
                                                            text-align: right;">
                                                            <asp:Label ID="lblSubTotalCompetency" Style="text-decoration: none;" runat="server"></asp:Label>
                                                        </th>
                                                        <th style="border-top-style: solid; border-bottom-style: double; border-top-width: 1px;
                                                            border-bottom-width: thick; border-top-color: #000000; border-bottom-color: #000000;
                                                            text-align: right;">
                                                            <asp:Label ID="lblSupTotalCompetency" Style="text-decoration: none;" runat="server"></asp:Label>
                                                        </th>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td style="width: 50%;">
                            <fieldset style="height: 120px;">
                                <legend>Goal/KPI Assessment</legend>
                                <br />
                                <table style="margin: auto;">
                                    <tr>
                                        <td>
                                            Subordinate&#39;s Total
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblSubordinatesTotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Supervisor&#39;s Total
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblSupervisorsTotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="width: 50%;">
                            <fieldset style="height: 120px;">
                                <legend>Average</legend>
                                <table style="margin: auto;">
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <strong>Average = (KPI Total+Competency Total)/2</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Subordinate&#39;s Average
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            Supervisor&#39;s Average
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblSubAvgForm" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblSupAvgForm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-top-style: solid; border-bottom-style: double; border-top-width: 1px;
                                            border-bottom-width: thick; border-top-color: #000000; border-bottom-color: #000000;
                                            text-align: right;">
                                            <asp:Label ID="lblSubordinateAverage" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="border-top-style: solid; border-bottom-style: double; border-top-width: 1px;
                                            border-bottom-width: thick; border-top-color: #000000; border-bottom-color: #000000;
                                            text-align: right;">
                                            <asp:Label ID="lblSupervisorAverage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Supervisor's Comments </legend>
                                <asp:TextBox ID="txtSupervisorComment" TextMode="MultiLine" Width="100%" Height="80px"
                                    runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSupervisorComment"
                                    runat="server" ValidationGroup="Supervisor" ForeColor="Red" ErrorMessage="Supervisor's Comment is Required"></asp:RequiredFieldValidator>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Recommendations </legend>
                                <asp:TextBox ID="txtSupervisorRecommendation" TextMode="MultiLine" Width="100%" Height="80px"
                                    runat="server"></asp:TextBox>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Training Needs If Any </legend>
                                <asp:TextBox ID="txtTrainingNeeds" TextMode="MultiLine" Width="100%" Height="80px"
                                    runat="server"></asp:TextBox>
                            </fieldset>
                            <br />
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblConsequenceDisagreements" runat="server" Text="Consequences of the discussion had on disagreements"></asp:Label></legend>
                                <asp:Label ID="lblConsequenceDisagreementDetail" runat="server" Text="Both Supervisor and the Subordinate discussed the disagreements and arrive at a compromise/ could not arrive at a compromise."></asp:Label>
                                <br />
                                <asp:TextBox ID="txtConsequenceDisagreements" TextMode="MultiLine" Width="100%" Height="80px"
                                    runat="server"></asp:TextBox>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td style="text-align: right;" colspan="2">
                            Feedback given to the employee :
                            <asp:RadioButton ID="rbtFeedbackGivenEmpYes" GroupName="rbtFeedbackGivenEmp" Text="Yes"
                                runat="server" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="rbtFeedbackGivenEmpNo" GroupName="rbtFeedbackGivenEmp" Text="No"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Button ID="btnFinalizeSupervisorEvaluation" runat="server" Text="Finalize" Width="125px"
                                OnClick="btnFinalizeSupervisorEvaluation_Click" ValidationGroup="Supervisor" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSupervisorComplete" runat="server" ValidationGroup="Supervisor"
                                Width="125px" Text="Complete" OnClick="btnSupervisorComplete_Click" />
                            <asp:Button ID="btnSupervisorClear" runat="server" Width="125px" Text="Clear" OnClick="btnSupervisorClear_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label ID="lblSupervisorMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvSubordinate" runat="server">
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>To be Filled by the Respective Employee</legend>
                                <br />
                                <asp:RadioButton ID="rbtEmployeeAgreed" GroupName="EmployeeAgreement" AutoPostBack="true"
                                    Text="I Agree with the comments made by the supervisor" runat="server" OnCheckedChanged="rbtEmployeeAgreed_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="rbtEmployeeDisagreed" GroupName="EmployeeAgreement" AutoPostBack="true"
                                    Text="I Disagree with the comments made by the supervisor" runat="server" OnCheckedChanged="rbtEmployeeDisagreed_CheckedChanged" />
                                <br />
                                <br />
                                <asp:Label ID="lblEmployeeDisagreeComment" runat="server" Text="I disagree with the following comments due to the reasons given therein"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtEmployeeDisagreeComment" Enabled="false" TextMode="MultiLine"
                                    Width="100%" Height="80px" runat="server"></asp:TextBox>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnEmployeeComplete" Width="125px" runat="server" Text="Complete"
                                OnClick="btnEmployeeComplete_Click" />
                            <asp:Button ID="btnEmployeeClear" Width="125px" runat="server" Text="Clear" OnClick="btnEmployeeClear_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label ID="lblSubordinateMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvCEO" runat="server">
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Obeservations/ Comments of the CEO, COO, GM</legend>
                                <asp:Label ID="lblCEOComments" runat="server" Text="I agree/disagree with the above remarks/ comments made by the responding officer. if the CEO, COO, GM disagrees with the comments/remarks he should give reasons for disagreement and his alternative proposal in detail."></asp:Label>
                                <br />
                                <asp:TextBox ID="txtCEOComments" TextMode="MultiLine" Width="100%" Height="80px"
                                    runat="server"></asp:TextBox>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="margin: auto; width: 800px;">
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Action Proposed by Chief Operation Officer/ Chief Executive Officer/ General
                                    Manager</legend>
                                <asp:Label ID="Label1" runat="server" Text="I agree/disagree with the above remarks/ comments made by the responding officer. if the CEO, COO, GM disagrees with the comments/remarks he should give reasons for disagreement and his alternative proposal in detail."></asp:Label>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIncrementGranted" Text="Increment is granted" runat="server"
                                                AutoPostBack="true" OnCheckedChanged="chkIncrementGranted_CheckedChanged" />
                                        </td>
                                        <td rowspan="5">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIncrementGranted" Width="50px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom,Numbers"
                                                ValidChars="." TargetControlID="txtIncrementGranted" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            %
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkToBeReviewed" Text="To be reviewed in" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkToBeReviewed_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToBeReviewed" Width="50px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                                                ValidChars="." TargetControlID="txtToBeReviewed" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Months.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkEmploymentConfrimed" Text="Employment confrimed" AutoPostBack="true"
                                                runat="server" OnCheckedChanged="chkEmploymentConfrimed_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbtEmploymentConfrimedYes" AutoPostBack="true" GroupName="rbtEmploymentConfrimed"
                                                Text="Yes" runat="server" OnCheckedChanged="rbtEmploymentConfrimedYes_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbtEmploymentConfrimedNo" AutoPostBack="true" GroupName="rbtEmploymentConfrimed"
                                                Text="No" runat="server" OnCheckedChanged="rbtEmploymentConfrimedNo_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkEndTraining" Text="End Training" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkEndTraining_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbtEndTrainingYes" GroupName="rbtEndTraining" Text="Yes" AutoPostBack="true"
                                                runat="server" OnCheckedChanged="rbtEndTrainingYes_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbtEndTrainingNo" GroupName="rbtEndTraining" Text="No" AutoPostBack="true"
                                                runat="server" OnCheckedChanged="rbtEndTrainingNo_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkProbationExtended" Text="Probation extended by" AutoPostBack="true"
                                                runat="server" OnCheckedChanged="chkProbationExtended_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProbationExtended" Width="50px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Custom,Numbers"
                                                ValidChars="." TargetControlID="txtProbationExtended" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Months.
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Button ID="btnCEOFinalize" runat="server" Text="Finalize" Width="125px" OnClick="btnCEOFinalize_Click" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btnCEOComplete" Width="125px" runat="server" Text="Complete" OnClick="btnCEOComplete_Click" />
                            <asp:Button ID="btnCEOClear" Width="125px" runat="server" Text="Clear" OnClick="btnCEOClear_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <table style="margin: auto; width: 800px;">
                <tr>
                    <td style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    </form>
</body>
</html>
