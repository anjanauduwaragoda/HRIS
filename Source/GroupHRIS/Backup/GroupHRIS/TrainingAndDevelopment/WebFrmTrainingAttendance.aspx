<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmTrainingAttendance.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        .HideGridColumn
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
            Training Attendance
            <hr />
            <table style="width: 100%;" align="center">
                <tr>
                    <td style="text-align: right">
                        Training Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTrainingType" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        Training Code
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrainingCode" Width="200px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        Training Program
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTrainingProgram" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        Status
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTrainingStatus" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnSearchTraining" runat="server" Width="150px" Text="Search" OnClick="btnSearchTraining_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            Trainings
            <hr />
            <asp:GridView ID="grdvTraining" Style="margin: auto;" AutoGenerateColumns="false"
                AllowPaging="true" PageSize="5" runat="server" OnPageIndexChanging="grdvTraining_PageIndexChanging"
                OnRowDataBound="grdvTraining_RowDataBound" OnSelectedIndexChanged="grdvTraining_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="TRAINING_ID" HeaderText="Training ID" HeaderStyle-CssClass="HideGridColumn"
                        ItemStyle-CssClass="HideGridColumn" />
                    <asp:BoundField DataField="TRAINING_NAME" HeaderText="Training Name" />
                    <asp:BoundField DataField="TRAINING_CODE" HeaderText="Training Code" />
                    <asp:BoundField DataField="PLANNED_START_DATE" HeaderText="Planned Start Date" />
                    <asp:BoundField DataField="PLANNED_END_DATE" HeaderText="Planned End Date" />
                </Columns>
            </asp:GridView>

            <br />

            <table style=" margin:auto;">
                <tr>
                    <td style="text-align: right;">
                        Training Name</td>
                    <td>
                        :</td>
                    <td>
                        &nbsp;
                        <asp:Label ID="lblTrainingName" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        Training Schedule</td>
                    <td>
                        :&nbsp;
                    </td>
                    <td>
                        &nbsp;
                        <asp:DropDownList ID="ddlTrainingSchedule" Width="200px" runat="server" 
                            onselectedindexchanged="ddlTrainingSchedule_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            Attendance
            <hr />
            <br />

            


            <asp:GridView ID="grdvTrainingAttendance" Style="margin: auto;Width:850px;" AutoGenerateColumns="false"
                AllowPaging="false" PageSize="5" runat="server"
                onrowdatabound="grdvTrainingAttendance_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="Employee" />
                    <asp:TemplateField HeaderText = "Actual Date">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlActualDateYear" AutoPostBack="true" Width="75px" runat="server" onselectedindexchanged="ddlActualDateYear_SelectedIndexChanged"> </asp:DropDownList>/
                            <asp:DropDownList ID="ddlActualDateMonth" AutoPostBack="true" Width="50px" runat="server" onselectedindexchanged="ddlActualDateMonth_SelectedIndexChanged"> </asp:DropDownList>/
                            <asp:DropDownList ID="ddlActualDateDate" Width="50px" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ACTUAL_DATE" HeaderText="Actual Date" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:TemplateField HeaderText = "Arrived Time">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlArrivedTimeH" Width="50px" runat="server"> </asp:DropDownList> : 
                            <asp:DropDownList ID="ddlArrivedTimeM" Width="50px" runat="server"> </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ARRIVED_TIME" HeaderText="Arrived Time" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:TemplateField HeaderText = "Departure Time">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDepartureTimeH" Width="50px" runat="server"> </asp:DropDownList> : 
                            <asp:DropDownList ID="ddlDepartureTimeM" Width="50px" runat="server"> </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DEPARTURE_TIME" HeaderText="Departure Time" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:BoundField DataField="REMARKS" HeaderText="Remarks" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:TemplateField HeaderText = "Remarks">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "is Attend" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsAttendance" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "Exclude" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkExclude" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IS_ATTEND" HeaderText="is Attend" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                </Columns>
            </asp:GridView>
            <br />

            <div style="text-align:right;">
                <asp:Button ID="btnSave" Width="100px" runat="server" Text="Save" 
                    onclick="btnSave_Click" />  
                <asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" 
                    onclick="btnClear_Click" />  
            </div>

            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <img src="../Images/ProBar/720.GIF" />
                </ProgressTemplate>
            </asp:UpdateProgress>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
