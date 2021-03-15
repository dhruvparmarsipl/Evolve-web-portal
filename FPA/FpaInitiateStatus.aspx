<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FpaInitiateStatus.aspx.cs"
    Inherits="FpaInitiateStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>FPA Initiation Status Report</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scp" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="wrappernew">
                <div class="row greyBgnew">
                    <div class="pageWidthnew">
                        <div class="row mBtm20">
                            <div class="floatRight Luser">
                                <span id="spnLoginUser" runat="server"></span>
                            </div>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                <tr>
                                    <td colspan="4" align="left" valign="top" class="head">
                                        <h2 class="black16 wnor">
                                            FPA Initiation Staus
                                        </h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="25%" align="left" valign="top">
                                        <strong>Initiator Name</strong>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <span id="spnIntName" runat="server"></span>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <strong>Employee code</strong>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <span id="spnEmpCode" runat="server"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <strong>Location</strong>
                                    </td>
                                    <td align="left" valign="top">
                                        <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                        </span>
                                    </td>
                                    <td align="left" valign="top">
                                        <strong>Designation</strong>
                                    </td>
                                    <td align="left" valign="top">
                                        <span id="spnDesignation" runat="server"></span>
                                    </td>
                                    <span id="spnSrNo" visible="false" runat="server"></span>
                                </tr>
                            </table>
                        </div>
                        <div class="rownew mBtm20new">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brdnew">
                                <tr>
                                    <td colspan="4" align="left" valign="top" class="head">
                                        <h2 class="black18 wnor">
                                            Employee Information</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%" align="left" valign="top" class="greyBgnew">
                                        <strong>BU</strong>
                                    </td>
                                    <td width="30%" align="left" valign="top" class="greyBgnew">
                                        <strong>Status</strong>
                                    </td>
                                    <td width="15%" align="left" valign="top" class="greyBgnew">
                                        <strong>&nbsp;</strong>
                                    </td>
                                    <td width="35%" align="left" valign="top" class="greyBgnew">
                                        <strong>&nbsp;</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlBu" CssClass="sel" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlStatus" CssClass="sel" runat="server">
                                         <asp:ListItem Value="00" Selected="True">Select All</asp:ListItem>
                                            <asp:ListItem Value="0">Initiated</asp:ListItem>
                                            <asp:ListItem Value="1">Draft by user</asp:ListItem>
                                            <asp:ListItem Value="3">Accepted</asp:ListItem>
                                            <asp:ListItem Value="4">Submitted to FPA Admin</asp:ListItem>
                                            <asp:ListItem Value="5">Rejected by FPA Admin</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:LinkButton ID="btnShowDetail" runat="server" CssClass="button" OnClick="btnShowDetail_Click">
                         Show details</asp:LinkButton>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:LinkButton ID="btnExport" Visible="false" runat="server" CssClass="button" OnClick="btnExport_Click">
                        Export To Excel</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="rownew mBtm20new">
                            <asp:GridView ID="gvEmployeeList" CssClass="grdMain" runat="server" Width="100%"
                                EmptyDataText="No Record Found." AutoGenerateColumns="false">
                                <Columns>
                                   <%-- <asp:TemplateField HeaderText="SapId" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSapId" runat="server" Text='<%#Eval("SapId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Ecode" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEcode" runat="server" Text='<%#Eval("Ecode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Ename" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEname" runat="server" Text='<%#Eval("Ename") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Loc" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("Loc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="SapId" HeaderText="SapId" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Ecode" HeaderText="Ecode" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Ename" HeaderText="Ename" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Loc" HeaderText="Loc" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
