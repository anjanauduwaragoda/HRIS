<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmCompetencyAssessment.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmCompetencyAssessment" %>

    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<style type="text/css" >
 .hideGridColumn
        {
            display: none;
        }
        </style>
    <title>Competency Assessment</title>
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
            //_width = window.screen.availWidth - 10;
            _width = 650;
            _height = window.screen.availHeight - 100;

            window.moveTo(60, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }

     

    </script>
    <span style="font-weight: 700">Competency - Assessment</span>
    <hr />
    <br />
</head>
<body class="popupsearch" onload="changeScreenSize()">

    <form id="form1" runat="server" style="overflow: auto">
    <div style="text-align: center; margin: center;">
        <span></span>
        <br />
        <table width="900px" class="beta" style="border: none;" cellspacing="10px">
            <tr>
                <th>
                    Employee Information
                </th>
                <th>
                    Criteria for Evaluation
                </th>
            </tr>
            <tr>
                <td width="450px"  style="background-color: #aed6f1;vertical-align:top;">
                    <div style="vertical-align:top;" >
                        <table class="beta"  >
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="lblDetails" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td width="450px" style="background-color: #aed6f1">
                  
                        <table class="beta">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCriteria" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                   
                </td>
            </tr>
            <tr><td></td></tr>
            <tr><td colspan="2" align="left">
            
    <asp:Label ID="lblGoalChart" runat="server" ></asp:Label>
            <%--<div>Assessment Instructions :- <br/>
            1 . You can partially evaluate competiences and save/update given ratings without evaluating all the competencies at the one step.
            <br />
2. After evaluating all the competencies you need to click finalize button to finalize evaluation process. After finalize you are not allow to modifies competencies. </div>--%></td></tr>
        </table>
    </div>
    <br />
    <%--<table width = "900px" style="border: none;"><tr><td colspan = "2">  </td></tr></table>--%>
    <%--<div style="width:900px"> <asp:Label ID="lbltbl" runat="server"></asp:Label> </div>--%>
    <br />
    <div>
    <asp:GridView ID="grdvCompetencies" ClientIDMode="Static"  AllowPaging="false" Style="width: 900px;"
                    AutoGenerateColumns="false" runat="server" 
            onrowdatabound="grdvCompetencies_RowDataBound">
                    <Columns>
                        <%--<asp:BoundField DataField="ASSESSMENT_TOKEN" HeaderText="Assessment Token" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />--%>
                        <asp:BoundField DataField="COMPETENCY_ID" HeaderText="Competency ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMPETENCY_NAME" HeaderText="Competency"/>
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description"/>
                        <asp:BoundField DataField="EXPECTED_PROFICIENCY_RATING" HeaderText="Expected Rating" ItemStyle-HorizontalAlign = "Center"/>
                        <asp:TemplateField  ItemStyle-HorizontalAlign="Right" HeaderText="Employee Rating">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlEmployeeRating" Width="100%" runat="server" style = "border:0px;text-align:center;">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        
                        <%--<asp:BoundField DataField="EMPLOYEE_WEIGHT" HeaderText="Employee Weight" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn"/>--%>
                        <asp:BoundField DataField="COMPETENCY_PROFILE_ID" HeaderText="Competency Profile ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" ><ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" /></asp:BoundField>
                        <asp:BoundField DataField="SUPERVISOR_RATING" HeaderText="Supervisor Rating"><ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" /></asp:BoundField>
                    </Columns>
                </asp:GridView>
                <br />
    </div>
    <table width="900px" class="beta" style="border: none;">
    
        <tr>
            <td align="left">
                <asp:Button ID="btnFinalized" runat="server" Text="Finalize" with="105px" OnClick="btnFinalized_Click" />
            </td>
            <td align="right">
                <asp:Button ID="btnSave" runat="server" Text="Save" align="right" Width="105px" OnClick="btnSave_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>

</body>
</html>
