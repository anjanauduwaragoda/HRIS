<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewQuestions.aspx.cs"
    EnableEventValidation="false" Inherits="GroupHRIS.PerformanceManagement.WebFrmViewQuestions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Self Assessment Questions</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(60, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }

        //        function dosposeWindow() {
        //            DoPostBack();
        //            window.close();
        //        }

        function sendValueToParent() {
            try {
                var x = 'popup';
                window.opener.displayData(x);
                window.close();
            }
            catch (err) {
                alert(err.Message);
            }
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
</head>
<body class="popupsearch" onload="changeScreenSize()" onunload="fnclose()">
    <form id="form1" runat="server">
    <span style="font-weight: 700">View Question Details</span>
    <hr />
    <br />
    <table style="margin: auto">
    <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSelect" runat="server" Text="Close" OnClientClick="sendValueToParent();"
                    align="Left" />
                <asp:Button ID="btnAdd" runat="server" Text="<< Add Questions" OnClick="btnAdd_Click"
                    align="right" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:GridView ID="grdAssessmentQuestion" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" OnPageIndexChanging="grdAssessmentQuestion_PageIndexChanging"
                    OnRowDataBound="grdAssessmentQuestion_RowDataBound"  Width = "800px">
                    <Columns>
                        <asp:BoundField DataField="QUESTION_ID" HeaderText="Question ID" />
                        <asp:BoundField DataField="QUESTION" HeaderText="Question" />
                        <asp:BoundField DataField="REMARKS" HeaderText="Remark" />
                        <asp:TemplateField HeaderText="No Of Answers">
                            <ItemTemplate >
                                <asp:DropDownList ID="ddlCount" runat="server" Width="100%" SelectedValue='<%# Eval("NO_OF_ANSWERS") %>' style="width:100%; border:0px;text-align:center;">
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value="1"></asp:ListItem>
                                    <asp:ListItem Value="2"></asp:ListItem>
                                    <asp:ListItem Value="3"></asp:ListItem>
                                    <asp:ListItem Value="4"></asp:ListItem>
                                    <asp:ListItem Value="5"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField><asp:TemplateField HeaderText="Include">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBxSelect" runat="server" Enabled="true" Checked='<%# bool.Parse(Eval("EXCLUDE").ToString() == "True" ? "True": "False") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:HiddenField ID="hfDataTable" runat="server" />
            </td>
            <td style="text-align:center;">
                <asp:Label ID="lblMessagex" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <%--<asp:GridView ID="gvviewQuestions" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                    AllowPaging="true">
                    <Columns>
                   
                        <asp:BoundField DataField="QUESTION_ID" HeaderText=" Question Id "  />
                        <asp:BoundField DataField="QUESTION" HeaderText=" Question " />
                       <asp:BoundField DataField="NO_OF_ANSWERS" HeaderText=" No Of Answers " />
                       <asp:TemplateField HeaderText="No Of Answers">
                                <ItemTemplate >
                                   <asp:DropDownList ID="ddlCount" runat="server" Width="75px" >
                                   <asp:ListItem  Value=""></asp:ListItem>
                                   <asp:ListItem  Value="1"></asp:ListItem>
                                   <asp:ListItem  Value="2"></asp:ListItem>
                                   <asp:ListItem  Value="3"></asp:ListItem>
                                   <asp:ListItem  Value="4"></asp:ListItem>
                                   <asp:ListItem  Value="5"></asp:ListItem>
                                   </asp:DropDownList>
                                   
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exclude">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBxExclude" runat="server" Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        NO QUESTIOn FOUND.
                    </EmptyDataTemplate>
                </asp:GridView>--%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
