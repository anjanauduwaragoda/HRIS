<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="Hrismain.aspx.cs" Inherits="GroupHRIS.Login.Hrismain" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleReminder.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/AlertStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dw_paus_scroller.js" type="text/javascript"></script>
    <script src="../../Scripts/HomePage/Chart.min.js" type="text/javascript"></script>
    <script src="../../Scripts/HomePage/Chart.HorizontalBar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function openpopupPass(file, window) {
            childWindow = open(file, window, 'resizable=no,width=850,height=450,scrollbars=yes,top=150,left=250,status=yes');
        }
        function openpopupProfile(file, window) {
            childWindow = open(file, window, 'resizable=no,width=900,height=450,scrollbars=yes,top=150,left=250,status=yes');
        }
        function openpopupLeave(file, window) {
            childWindow = open(file, window, 'resizable=no,width=900,height=550,scrollbars=yes,top=150,left=250,status=yes');
        }
    </script>
    <script type="text/javascript">

        if (DYN_WEB.Scroll_Div.isSupported()) {

            DYN_WEB.Event.add(window, 'load', function () {
                var wndo = new DYN_WEB.Scroll_Div('wn', 'lyr');

                var options = {
                    axis: 'v',
                    bRepeat: true,
                    repeatId: 'rpt',
                    dur: 800, // duration of glide-scroll
                    bPauseResume: true,
                    distance: 50, // distance of glide-scroll
                    pauseDelay: 5000,
                    resumeDelay: 300
                };

                wndo.makePauseAuto(options);

            });
        }

    </script>
    <%=LEAVE_DETAILS %>
    <%=LEAVE_TAKEN_DETAILS%>
    <%=LEAVE_BALANCE_DETAILS%>
    <%--<table class="styleMainTb" style="background-image: url('/Images/HRISMain/bgprofile.png');
        background-repeat: no-repeat; height: 300px; width: 880px">
        <tr>
            <td style="height: 300px; width: 580px; padding-left: 50px">
                <asp:Label ID="lblfirstname" runat="server" Height="15px" Font-Size="12pt"></asp:Label>
                <asp:Label ID="lbllastname" runat="server" Height="15px" Font-Size="12pt"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblemail" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lblnic" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lbldoj" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lblcompmob" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lblpermob" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lblepf" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lbldob" runat="server" Height="15px"></asp:Label>
                <br />
                <asp:Label ID="lblfunarea" runat="server" ForeColor="White" Font-Bold="False"></asp:Label>
            </td>
            <td align="center" style="height: 300px; width: 300px; padding-left: 50px">
                <asp:Image ID="imgme" runat="server" Height="250px" Width="230px" />
            </td>
        </tr>
    </table>--%>
    <table class="styleMainTb">
        <tr>
            <td align="center" valign="top">
                <br />
                <table style="margin: auto;">
                    <tr>
                        <td>
                            <table class="ManinTable">
                                <tr>
                                    <th>
                                        NOTIFICATIONS
                                    </th>
                                </tr>
                                <tr>
                                    <td style="vertical-align: text-top; text-align: left;" class="BirthdayListFrame">
                                        <div>
                                            <asp:TreeView ID="trvNotification" ForeColor="White" runat="server">
                                            </asp:TreeView>
                                            <%--<ul>
                                                <%=NotificationOutput%>
                                            </ul>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table class="ManinTable">
                                <tr>
                                    <th>
                                        BIRTHDAYS
                                    </th>
                                </tr>
                                <tr>
                                    <td style="vertical-align: text-top;" class="BirthdayListFrame">
                                        <div>
                                            <%=BirthdayOutput%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table class="styleMainTb" style="width: 880px">
                    <tr>
                        <td style="color: White; padding-left: 20px; background-image: url('/Images/HRISMain/bgprofilebottom.png');
                            background-repeat: no-repeat; width: 220px; height: 150px">
                            To keep your login safe, create<br />
                            a strong password.<br />
                            <br />
                            <br />
                            <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/HRISMain/changepasslink.png"
                                onclick="openpopupPass('/EmployeeProfile/Changepassword.aspx','Search')" />
                        </td>
                        <td style="color: White; padding-left: 20px; background-image: url('/Images/HRISMain/bgprofilebottom.png');
                            background-repeat: no-repeat; width: 220px; height: 150px">
                            Your can change your profile details First name, Last name,<br />
                            photo etc.<br />
                            <br />
                            <asp:Image ID="Image7" runat="server" ImageUrl="/Images/HRISMain/editprofile.png"
                                onclick="openpopupProfile('/EmployeeProfile/Editprofile.aspx','Search')" />
                        </td>
                        <td style="color: White; padding-left: 20px; background-image: url('/Images/HRISMain/bgprofilebottom.png');
                            background-repeat: no-repeat; width: 220px; height: 150px">
                            Check your leave balance and access to request leave.
                            <br />
                            <br />
                            <br />
                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/HRISMain/leave.png"
                    PostBackUrl="~/EmployeeLeave/webFrmEmployeeLeaveSheet.aspx" 
                               />
                            <%--<asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/HRISMain/leave.png"
                                PostBackUrl="/EmployeeLeave/webFrmEmployeeLeaveSheet.aspx" />--%>
                        </td>
                        <td style="color: White; padding-left: 20px; background-image: url('/Images/HRISMain/bgprofilebottom.png');
                            background-repeat: no-repeat; width: 220px; height: 150px">
                            Find all about YOU !<br />
                            <br />
                            <br />
                            <br />
                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/HRISMain/Employee.png"
                                PostBackUrl="~/EmployeeProfile/EmployeeManagement.aspx" />
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%; font-variant: small-caps; font-weight: bold; font-size: medium"
                            align="center">
                            Leave Details
                        </td>
                        <td style="width: 30%; font-variant: small-caps; font-weight: bold; font-size: medium"
                            align="center">
                            Leave Taken
                        </td>
                        <td style="width: 30%; font-variant: small-caps; font-weight: bold; font-size: medium"
                            align="center">
                            Leave Balance
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                            <%--<asp:Chart ID="chrtLeaveDetails" runat="server" BackGradientStyle="Center" Height="200px"
                                Width="250px">
                                <Series>
                                    <asp:Series BackGradientStyle="HorizontalCenter" BackSecondaryColor="Aqua" BorderColor="White"
                                        Color="0, 0, 192" IsValueShownAsLabel="True" Name="noofleave" ChartType="Bar">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea BackColor="Transparent" Name="ChartArea1">
                                        <AxisY Enabled="False" LineColor="Transparent">
                                            <MajorGrid LineDashStyle="NotSet" />
                                        </AxisY>
                                        <AxisX IsLabelAutoFit="False" LabelAutoFitMaxFontSize="8" LineColor="Blue" LineWidth="2">
                                            <MajorGrid LineDashStyle="NotSet" />
                                            <MinorGrid LineDashStyle="NotSet" />
                                            <LabelStyle Font="Microsoft Sans Serif, 6pt" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>--%>
                            <div style="width: 200px;">
                                <canvas id="canvasLeaveDetails" height="250px" width="250px"></canvas>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <%--<asp:Chart ID="chrtLeaveTaken" runat="server" BackGradientStyle="LeftRight" Height="200px"
                                Width="250px">
                                <Series>
                                    <asp:Series BackGradientStyle="HorizontalCenter" BackSecondaryColor="255, 255, 128"
                                        BorderColor="White" Color="255, 128, 0" IsValueShownAsLabel="True" Name="noofleave"
                                        ChartType="Bar">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea BackColor="Transparent" Name="ChartArea1">
                                        <AxisY Enabled="False">
                                            <MajorGrid LineDashStyle="NotSet" />
                                        </AxisY>
                                        <AxisX IsLabelAutoFit="False" LabelAutoFitMaxFontSize="8" LineColor="255, 128, 0"
                                            LineWidth="2">
                                            <MajorGrid LineDashStyle="NotSet" />
                                            <MinorGrid LineDashStyle="NotSet" />
                                            <LabelStyle Font="Microsoft Sans Serif, 6pt" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>--%>
                            <div style="width: 200px;">
                                <canvas id="canvasLeaveTakenDetails" height="250px" width="250px"></canvas>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <%--<asp:Chart ID="chrtLeaveBalance" runat="server" BackGradientStyle="Center" Height="200px"
                                Width="250px" BorderlineColor="Green">
                                <Series>
                                    <asp:Series BackGradientStyle="HorizontalCenter" BackSecondaryColor="Lime" BorderColor="White"
                                        Color="DarkGreen" IsValueShownAsLabel="True" Name="noofleave" ChartType="Bar">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea BackColor="Transparent" Name="ChartArea1">
                                        <AxisY Enabled="False">
                                            <MajorGrid LineDashStyle="NotSet" />
                                        </AxisY>
                                        <AxisX IsLabelAutoFit="False" LabelAutoFitMaxFontSize="8" LineColor="0, 192, 0" LineWidth="2">
                                            <MajorGrid LineDashStyle="NotSet" />
                                            <MinorGrid LineDashStyle="NotSet" />
                                            <LabelStyle Font="Microsoft Sans Serif, 6pt" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>--%>
                            <div style="width: 200px;">
                                <canvas id="canvasLeaveBalanceDetails" height="250px" width="250px"></canvas>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <hr />
            </td>
        </tr>
    </table>
    <table class="styleMainTb" style="height: 200px">
        <tr>
            <td style="background-image: url('/Images/HRISMain/bgscrollnotice.png'); background-repeat: repeat-x;
                height: 50px; padding-left: 10px" align="left" colspan="4">
                <asp:Label ID="lblremscroller" runat="server" ForeColor="Yellow"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <hr />
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" colspan="4">
                <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>