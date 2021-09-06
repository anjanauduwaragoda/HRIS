<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmProfileCompetencies.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmProfileCompetencies" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style2
        {
            width: 127px;
        }
        .style3
        {
            width: 360px;
        }
    </style>
    <script type="text/javascript">

        function sendToParent() {
            var dataCaptured = "1";
            //alert();
            window.opener.getValuefromChild(dataCaptured);
            window.close();

        }

    </script>
</head>
<body class="popupsearch">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="vertical-align:middle; width:100%">

            </div>
            <div>
            <table>
            <tr>
                        <td class="style3">
                        <%--<asp:Button ID="btnAddCompetencies" runat="server" Text="<< Add" Width="125px" Style="height: 26px;"
                                OnClientClick="sendToParent()" />--%>
                        </td>
                        <td class="style2">
                        <br />
                            
                        </td>
                    </tr>
            </table>
            
            </div>
            <br />

            <b>Competency Details </b>
            <hr />

            <br />
            <%--Rating Scheme :
            <asp:GridView ID="gvProficiencyScheme" runat="server" Style="padding-left: 50px; width:150px;" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="RATING" HeaderText="RATING"></asp:BoundField>
                    <asp:BoundField DataField="WEIGHT" HeaderText="WEIGHT"></asp:BoundField>
                </Columns>
            </asp:GridView>--%>
            <br />
            
            <table border="0" cellpadding="0" cellspacing="0" style="margin:0 auto 0 auto;">
            <tr>
            <td></td>
            <td><asp:Button ID="btnAddCompetencies" runat="server" Text="<< Add" Width="125px" Style="height: 26px;"
                                OnClientClick="sendToParent()" />
                                </td>
            <td align="right">Competency Group : <asp:DropDownList  ID="ddlCompetencyGroup" runat="server" Width="150px" OnSelectedIndexChanged="ddlCompetencyGroup_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList></td>
            <td></td>
            </tr>
            <tr>
            <td>&nbsp;</td>
            <td></td>
            <td></td>
            <td></td>
            </tr>
                <tr>
                <td></td>
                    <td colspan="2" style="min-width:800px;">
                    <asp:GridView ID="gvCompetency" style="width:800px;" runat="server" AutoGenerateColumns="false" HorizontalAlign="Center"
                OnPageIndexChanging="gvCompetency_PageIndexChanging" OnRowDataBound="gvCompetency_RowDataBound"
                AllowPaging="true">
                <Columns>
                    <asp:BoundField DataField="COMPETENCY_ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                    </asp:BoundField>
                    <asp:BoundField DataField="COMPETENCY_GROUP_ID" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                    <asp:BoundField DataField="COMPETENCY_NAME" HeaderText="Competency Name"></asp:BoundField>
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="400px">
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <HeaderTemplate>
                            Expected Level
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlProficiencyLevels" runat="server" Width="60%" style="border-style:none; float:right;" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProficiencyLevels_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <HeaderTemplate>
                            Include
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="includeChildCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="includeChildCheckBox_OnCheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <center>competencies for the selected competency group</center> 
                </EmptyDataTemplate>
            </asp:GridView>
                    </td>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%">
        <tr style="width: 100%">
            <td id="ratingScheme" style="width: 100%; text-align: center" runat="server">
            <br />
                Rating Scheme :
            </td>
        </tr>
    </table>

    
    <table style="width: 100%">
        <tr style="width: 100%">
            <td style="width: 100%; text-align: center">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin: auto; width: 200px">
                    <ProgressTemplate>
                        <img src="../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
