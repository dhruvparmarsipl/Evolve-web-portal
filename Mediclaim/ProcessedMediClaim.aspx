<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessedMediClaim.aspx.cs"
    Inherits="ProcessedMediClaim" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Processed Claim</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
        <link href="css/intranetdash.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="wrapper">
                    <div id="cWrap">
                        <div id="pageWrap">
                            <div class="toggle_container">
                                <div class="data_row">
                                    <asp:HiddenField ID="hfUserSAP" runat="server" />

                                    <div>

                                        <div class="ms-standardheader ms-WPTitle">Processed Medi Claim</div>

                                        <div class="legentAlign">
                                             Approved/Closed&nbsp;&nbsp;<span class="legentStyle" style="background-color:#00ff00;">&nbsp;</span>&nbsp;|&nbsp;
                                             In Progress &nbsp;&nbsp;<span class="legentStyle" style="background-color:#ff9e10;">&nbsp;</span>&nbsp;|&nbsp;
                                             Draft &nbsp;&nbsp;<span style="background-color:#ffdb63;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
                                             Rejected&nbsp;&nbsp;<span style="background-color:#ff0000;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
                                             Initiated&nbsp;&nbsp;<span style="background-color:#0064ac;" class="legentStyle">&nbsp;</span>
                                           </div>


                                    </div>

                                    
                                    <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                        HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True"
                                        PageSize="15" OnPageIndexChanging="grdUser_PageIndexChanged" CellPadding="6"
                                        CellSpacing="0" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL No" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a target="_blank" href="MediApproverClosed.aspx?onrs=<%#DoEncrypt(Eval("SERIAL_NO").ToString()) %>&flag=<%#DoEncrypt(("0").ToString()) %>">
                                                        <%#Eval("Ename")%>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                                                    <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CREATED_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MediClaim Approver" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%#Eval("HR Approver")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="10%" HeaderText="Status">
                                                <ItemTemplate>
                                                    <span class="legentStyle" title="<%#Eval("StatusText")%>" style="cursor: pointer; background-color: <%#Eval("Color")%>;">
                                                        &nbsp;</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="grdRows" />
                                        <EmptyDataRowStyle CssClass="grdRows" />
                                        <HeaderStyle CssClass="grdHead" />
                                    </asp:GridView>
                                    <asp:GridView ID="GridView2" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                        HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="false"
                                        PageSize="5" CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL No" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left"
                                                ItemStyle-HorizontalAlign="left">
                                                <ItemTemplate>
                                                    <%#Eval("SERIAL_NO")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="grdRows" />
                                        <EmptyDataRowStyle CssClass="grdRows" />
                                        <HeaderStyle CssClass="grdHead" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
