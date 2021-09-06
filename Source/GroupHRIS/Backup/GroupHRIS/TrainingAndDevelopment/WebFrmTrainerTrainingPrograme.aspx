<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainerTrainingPrograme.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainerTraining" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .style1
        {
            width: 246px;
        }
        .style2
        {
            width: 460px;
        }
        .style3
        {
            text-align: right;
            max-width: 200px;
            width: 156px;
            font-weight: bold;
        }
        .style4
        {
            width: 156px;
        }
        .hideGridColumn
        {
            display: none;
        }
        .style11
        {
            width: 149px;
        }
        .style20
        {
            width: 318px;
        }
        .style24
        {
            width: 619px;
        }
        
        .ui-tooltip, .arrow:after
        {
            background: #85c1e9;
            border: 0px #2980b9;
            border-bottom-style:hidden;
        }
        .ui-tooltip
        {
            padding: 10px 10px;
            color: black;
            border-radius: 10px;
            font: bold 10px "Helvetica Neue" , Sans-Serif;
            text-transform: uppercase;
            box-shadow: 0 0 7px black;
        }
        .arrow
        {
            width: 70px;
            height: 16px;
            overflow: hidden;
            position: absolute;
            left: 50%;
            margin-left: -35px;
            bottom: -16px;
        }
        .arrow.top
        {
            top: -25px;
            bottom: auto;
        }
        .arrow.left
        {
            left: 20%;
        }
        .arrow:after
        {
            content: "";
            position: absolute;
            left: 20px;
            top: -20px;
            width: 25px;
            height: 25px;
            box-shadow: 6px 5px 9px -9px black;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }
        .arrow.top:after
        {
            bottom: -20px;
            top: auto;
        }
        
        .clicked:active
        {
            
            display : none;
            }
        .style27
        {
            width: 415px;
        }
        .style28
        {
            width: 421px;
        }
        .style29
        {
            text-align: right;
            max-width: 200px;
            width: 153px;
            font-weight: bold;
        }
        .style31
        {
            text-align: right;
            max-width: 137px; /*width: 158px;*/;
            font-weight: bold;
            width: 186px;
        }
        .style32
        {
            text-align: right;
            max-width: 137px; /*width: 158px;*/;
            font-weight: bold;
            width: 199px;
        }
    </style>
    <script type="text/javascript">

        $(function () {
            $(document).tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
            .addClass("arrow")
            .addClass(feedback.vertical)
            .addClass(feedback.horizontal)
            .appendTo(this);
                    }
                }
            });
        });

        $(function () {
            $("[id*=gvTrainers] td").click(function () {
                tooltip.hide();
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" runat="server">
                <tr>
                    <td style="min-width: 49%; background-color: #f2f4f4; padding-top: 0;" align="center"
                        valign="top" class="style2">
                        <br />
                        <table id="tblProgrameDetail" style="margin-top: 0;">
                            <tr>
                                <td colspan="2">
                                    <br />
                                    Program<span> Details</span>
                                    <hr />
                                </td>
                            </tr>
                            <%-- Programe name--%>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label ID="lblNameTitle" runat="server" Text="Program Name : "></asp:Label>
                                </td>
                                <td class="style1" align="left">
                                    <asp:Label ID="lblProgrameName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <%--Programe Code--%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblCodeTitle" runat="server" Text="Program Code : "></asp:Label>
                                </td>
                                <td class="style1" align="left">
                                    <asp:Label ID="lblProgrameCode" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <%--Programe Type--%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblProgrameTypeTitle" runat="server" Text="Program Type : "></asp:Label>
                                </td>
                                <td class="style1" align="left">
                                    <asp:Label ID="lblProgrameType" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <%--Training Type--%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblTrainingTypeTitle" runat="server" Text="Training Type : "></asp:Label>
                                </td>
                                <td class="style1" align="left">
                                    <asp:Label ID="lblTrainingType" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </td>
                    <td style="background-color: white;">
                    </td>
                    <td style="min-width: 49%; background-color: #f2f4f4; vertical-align: top;" align="center">
                        <br />
                        <table id="Table2">
                            <tr>
                                <td colspan="2">
                                    <br />
                                    <span>Added Trainers </span>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvAddedTrainers" runat="server" Style="width: 400px;" OnRowDataBound="gvAddedTrainers_RowDataBound"
                                        OnSelectedIndexChanged="gvAddedTrainers_SelectedIndexChanged" AutoGenerateColumns="false"
                                        PageSize="3" AllowPaging="true" OnPageIndexChanging="gvAddedTrainers_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="TRAINER_ID" HeaderText="Trainer Id" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="NAME_WITH_INITIALS" HeaderText="Name" />
                                            <asp:BoundField DataField="COST_PER_SESSION" HeaderText="Cost Per Session" />
                                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
            <br />
            <table width="855px">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table id="tblTrainerDetails" style="background-color: #ebf5fb; min-width: 99.7%;
                padding-left: 5px;" align="center" runat="server" clientidmode="Static">
                <tr>
                    <td colspan="3">
                        <br />
                        Trainer Details
                    </td>
                    <td align="right" class="style28">
                        <asp:ImageButton ID="CloseButton1" runat="server" ImageUrl="../Images/close_button.png"
                            Height="18px" Width="20px" OnClick="CloseButton1_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr width="100%" />
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label1" runat="server" Text="Name : "></asp:Label>
                    </td>
                    <td class="style20">
                        <asp:Label ID="lblTrainerName" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="style29">
                        <asp:Label ID="Label3" runat="server" Text="External / Internal : "></asp:Label>
                    </td>
                    <td class="style28" align="left">
                        <asp:Label ID="lblIsExternal" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Contact No : "></asp:Label>
                    </td>
                    <td class="style20">
                        <asp:Label ID="lblContactNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="style29">
                        <asp:Label ID="Label4" runat="server" Text="Training Nature : "></asp:Label>
                    </td>
                    <td class="style28">
                        <asp:Label ID="lblTrainingNature" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3" valign="top">
                        <asp:Label ID="Label5" runat="server" Text="Qualifications : "></asp:Label>
                    </td>
                    <td class="style20" valign="top">
                        <asp:Label ID="lblQualifications" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="style29" valign="top">
                        <asp:Label ID="Label6" runat="server" Text="Competencies : "></asp:Label>
                    </td>
                    <td class="style28" valign="top">
                        <asp:Label ID="lblCompetencies" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3" valign="top" style="padding-top: 5px;">
                        <br />
                        <asp:Label ID="Label8" runat="server" Text="Description : "></asp:Label>
                    </td>
                    <td class="style20" valign="top" style="padding-top: 5px;">
                        <br />
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Height="52px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description is required"
                            ControlToValidate="txtDescription" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="style32" valign="top" style="min-width:120px;">
                                    <br />
                                    <asp:Label ID="Label7" ClientIDMode="Static" runat="server" Text="Cost Per Session : "></asp:Label>
                                </td>
                                <td class="style27" colspan="2" valign="top">
                                    <br />
                                    <asp:TextBox ID="txtCost" runat="server" ClientIDMode="Static" Width="150px"></asp:TextBox>&nbsp;(Rs.)&nbsp;
                                    <asp:FilteredTextBoxExtender ID="fteCost" FilterType="Numbers" TargetControlID="txtCost"
                                        runat="server">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Cost Per Session is required"
                                        ControlToValidate="txtCost" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style32" valign="bottom">
                                    <asp:Label ID="lblStatus" ClientIDMode="Static" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td class="style27" valign="bottom" 
                                    style="padding-top: 5px; min-width: 156px; max-width: 156px;">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="156PX" ClientIDMode="Static"
                                        Style="">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is required"
                                        ControlToValidate="ddlStatus" ValidationGroup="gvTrainerDetails" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style32" valign="bottom">
                                </td>
                                <td class="style27" valign="bottom" style="padding-top: 5px;">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" Width="75px" OnClick="btnAdd_Click"
                                        ValidationGroup="gvTrainerDetails" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="75px" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>
                    <td class="style3">
                    </td>
                    <td class="style20">
                    </td>
                    <td class="style29">
                    </td>
                    <td class="style28">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="gvTrainerDetails"
                            ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <br />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <table width="855px">
        <tr>
            <td>
                <asp:LinkButton ID="lblFindPrograme" runat="server" Style="text-decoration: none;"
                    OnClick="lblFindPrograme_Click"> <<< Find Program </asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="855px">
                <tr>
                    <td>
                        <span>Trainers </span>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="870px">
                            <tr>
                                <%--class="styleDdlSmall"--%>
                                <td class="style24" valign="middle" align="right">
                                    <asp:Label ID="Label9" runat="server" Text="Competency Area : "></asp:Label>
                                </td>
                                <td class="style11" align="left">
                                    <asp:DropDownList ID="ddlTrainerCompetency" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="">
                                    <asp:ImageButton ID="iBtnSearch" runat="server" Height="30px" ImageUrl="~/Images/Search.png"
                                        Width="30px" OnClick="iBtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvTrainers" runat="server" Style="width: 750px;" AutoGenerateColumns="false"
                            ClientIDMode="Static" AllowPaging="true" PageSize="10" OnSelectedIndexChanged="gvTrainers_SelectedIndexChanged"
                            OnRowDataBound="gvTrainers_RowDataBound" OnPageIndexChanging="gvTrainers_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="TRAINER_ID" HeaderText="Trainer Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="NAME_WITH_INITIALS" HeaderText="Name" />
                                <asp:BoundField DataField="CONTACT_MOBILE" HeaderText="Mobile No." />
                                <asp:BoundField DataField="COST_PER_SESSION" HeaderText="Cost Per Session" />
                                <asp:TemplateField ItemStyle-CssClass="">
                                    <HeaderTemplate>
                                        Competencies</HeaderTemplate>
                                    <ItemTemplate>
                                        <center>
                                            View</center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfSelectedProgrameId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfTrainerId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
