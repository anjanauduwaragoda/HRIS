﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceApprove.aspx.cs"
    Inherits="GroupHRIS.Attendance.AttendanceApprove" %>

<link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/StyleLogin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/js-image-slider.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js-image-slider.js" defer="defer" type="text/javascript"></script>
    <!--    New code for responsive stuff -->
    <script src="../ResponsiveJS/vendor/modernizr.min.js" type="text/javascript"></script>
    <script src="../ResponsiveJS/vendor/respond.min.js" type="text/javascript"></script>
    <script src="../ResponsiveJS/vendor/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">        window.jQuery || document.write('<script type="text/javascript" src="js\/vendor\/1.7.2.jquery.min"><\/script>')</script>
    <script src="../ResponsiveJS/vendor/lightbox/js/lightbox.js" type="text/javascript"></script>
    <script src="../ResponsiveJS/vendor/prefixfree.min.js" type="text/javascript"></script>
    <script src="../ResponsiveJS/vendor/jquery.slides.min.js" type="text/javascript"></script>
    <script src="../ResponsiveJS/script.js" type="text/javascript"></script>
    <!-- end of responsive stuff -->
    <style type="text/css">
        .signinagainstyle
        {
            text-decoration: none;
            color: Yellow;
            font-variant: small-caps;
            font-weight: bold;
        }
        a:link
        {
            text-decoration: none;
        }
        
        a:visited
        {
            text-decoration: none;
        }
        
        a:hover
        {
            text-decoration: none;
        }
        
        a:active
        {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        window.onload = blinkOn;
        function blinkOn() {
            document.getElementById("blink").style.color = "#00CCFF"
            setTimeout("blinkOff()", 500)
        }

        function blinkOff() {
            document.getElementById("blink").style.color = ""
            setTimeout("blinkOn()", 500)
        }
    </script>
    <!-- Responsive script -->
    <script>
        $(function () {
            $('#slides').slidesjs({
                height: 235,
                navigation: false,
                pagination: false,
                effect: {
                    fade: {
                        speed: 400
                    }
                },
                callback: {
                    start: function (number) {
                        $("#slider_content1,#slider_content2,#slider_content3").fadeOut(500);
                    },
                    complete: function (number) {
                        $("#slider_content" + number).delay(500).fadeIn(1000);
                    }
                },
                play: {
                    active: false,
                    auto: true,
                    interval: 6000,
                    pauseOnHover: false,
                    effect: "fade"
                }
            });
        });
    </script>
    <!-- end -->
</head>
<html xmlns="http://www.w3.org/1999/xhtml">
<body id="MailAttendanceApproveBody" runat="server">
    <form id="form1" runat="server">
    <div style="width: 100%; text-align: center">
        <section id="boxcontent">
        <h2 class="hidden">&nbsp;</h2>
            <article>  
                <table class="HeadingTop">
                    <tr>
                        <td align="left" valign="top" class="styletopleft">
                            <img src="../Images/MasterPage/logo.png" alt="" />
                        </td>
                    </tr>
                </table>
                <table align="center">
                <tr><td><asp:Label ID="lblPending" runat="server" Text="To Approve/Reject : "></asp:Label></td>
                
                </tr><tr>
                <td><asp:GridView ID="grdpending" runat="server" AutoGenerateColumns="False" >
                <Columns>
                                                <asp:BoundField DataField="ATT_DATE" HeaderText=" Attendance Time" />
                                                <asp:BoundField DataField="ATT_TIME" HeaderText=" Attendance Time" />
                                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                                                <asp:BoundField DataField="REMARK" HeaderText="Remarks" />
                                            </Columns></asp:GridView></td>
                                    
                </tr>
                </table>

                <table align="center" >
                <tr><td><asp:Label ID="lblObsoleted" runat="server" Text="Obsoleted Records : "></asp:Label></td></tr>
                <tr><td  style="vertical-align: top;"><asp:GridView ID="grdObsolete" runat="server" 
                        AutoGenerateColumns="False">
                <Columns>
                                                <asp:BoundField DataField="ATT_DATE" HeaderText=" Attendance Time" />
                                                <asp:BoundField DataField="ATT_TIME" HeaderText=" Attendance Time" />
                                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                                                <asp:BoundField DataField="REMARK" HeaderText="Remarks" />
                                            </Columns></asp:GridView></td></tr>
                </table>
                <table class="MainBodyTB">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblerror" runat="server" Font-Size="14pt" ForeColor="White"></asp:Label><br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" 
                                            Font-Size="Small" Height="30px" Width="100px" onclick="btnApprove_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExit" runat="server" Text="Exit"  
                                            Font-Size="Small" Height="30px" Width="100px" onclick="btnReject_Click"  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>        
                    <tr>
                        <td class="FotterLogin" align="center">
                            <br />
                            <asp:Label ID="lblcopyright" runat="server" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                </table>
            </article>
        </section>
    </div>
    </form>
</body>
</html>
