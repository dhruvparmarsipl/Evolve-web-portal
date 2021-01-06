<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DashBoard</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/intranetdash.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <div id="cWrap">
                <div id="pageWrap">
                    <div class="toggle_container">
                        <div class="data_row">

                        <div>



                            <div class="ms-standardheader ms-WPTitle">FPA and Limits</div>
                            <div class="legentAlign">
                                Approved/Closed&nbsp;
            <span style="BACKGROUND-COLOR: #00ff00" class="legentStyle">&nbsp;</span>&nbsp;&nbsp;|&nbsp;&nbsp;In Progress&nbsp;
            <span style="BACKGROUND-COLOR: #ff9e10" class="legentStyle">&nbsp;</span>&nbsp;&nbsp;|&nbsp;&nbsp;Draft&nbsp;
            <span style="BACKGROUND-COLOR: #ffdb63" class="legentStyle">&nbsp;</span>&nbsp;&nbsp;|&nbsp;&nbsp;Rejected&nbsp;
            <span style="BACKGROUND-COLOR: #ff0000" class="legentStyle">&nbsp;</span>&nbsp;&nbsp;|&nbsp;&nbsp;On Hold&nbsp;
            <span style="BACKGROUND-COLOR: #3385FF" class="legentStyle">&nbsp;</span>
                            </div>

                            <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True"
                                PageSize="5" OnPageIndexChanging="grdUser_PageIndexChanged" CellPadding="6" CellSpacing="0"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL No" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%"
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <a target="_blank" href="FpaClaim.aspx?RID=<%#DoEncrypt(Eval("SerialNo").ToString()) %>">
                                                <%#Eval("SerialNo")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CreationDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="10%" HeaderText="Status">
                                        <ItemTemplate>
                                            <span class="legentStyle" title="<%#Eval("Status")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">&nbsp;</span>
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
                                            <%#Eval("SERIALNO")%>
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
                        <br />
                        <asp:GridView ID="GrdCoachApproval" Width="100%" runat="server" CssClass="grdMain"
                            EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows"
                            GridLines="None" AllowPaging="True" PageSize="5" OnPageIndexChanging="GrdCoachApproval_PageIndexChanged"
                            CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="left"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a target="_blank" href="FpaApprover.aspx?RID=<%#DoEncrypt(Eval("APP_SERIAL_NO").ToString()) %>">
                                            <%#Eval("NAME")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CreationDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Eval("Amount")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%" HeaderText="Status">
                                    <ItemTemplate>
                                        <span class="legentStyle" title="<%#Eval("Status")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">&nbsp;</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="grdRows" />
                            <EmptyDataRowStyle CssClass="grdRows" />
                            <HeaderStyle CssClass="grdHead" />
                        </asp:GridView>
                        <asp:GridView ID="GridView1" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                            HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="false"
                            PageSize="5" CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left"
                                    ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%#Eval("NAME")%>
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
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="grdRows" />
                            <EmptyDataRowStyle CssClass="grdRows" />
                            <HeaderStyle CssClass="grdHead" />
                        </asp:GridView>

                        </div>


                         <div class="data_row">
                        <div>
                          <div class="ms-standardheader ms-WPTitle">Petrol Claims</div>
                            <asp:GridView ID="gvuserPetrolClaim" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True"
                                PageSize="5" CellPadding="6" CellSpacing="0"
                                AutoGenerateColumns="false" OnPageIndexChanging="gvuserPetrolClaim_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL No" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%"
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <a target="_blank" href="PetrolClaim.aspx?RID=<%#DoEncrypt(Eval("SerialNo").ToString()) %>">
                                                <%#Eval("SerialNo")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                                            <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CreationDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="10%" HeaderText="Status">
                                        <ItemTemplate>
                                            <span class="legentStyle" title="<%#Eval("Status")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">&nbsp;</span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="grdRows" />
                                <EmptyDataRowStyle CssClass="grdRows" />
                                <HeaderStyle CssClass="grdHead" />
                            </asp:GridView>
                            <asp:GridView ID="GridView4" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="false"
                                PageSize="5" CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL No" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left"
                                        ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <%#Eval("SERIALNO")%>
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
                        <br />
                        <asp:GridView ID="gvCoachPetrolClaim" Width="100%" runat="server" CssClass="grdMain"
                            EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows"
                            GridLines="None" AllowPaging="True" PageSize="5"
                            CellPadding="6" CellSpacing="0" AutoGenerateColumns="false" OnPageIndexChanging="gvCoachPetrolClaim_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="left"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a target="_blank" href="PetrolApprover.aspx?RID=<%#DoEncrypt(Eval("SERIAL_NO").ToString()) %>">
                                            <%#Eval("NAME")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%--  <asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                                        <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CreationDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#Eval("Amount")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%" HeaderText="Status">
                                    <ItemTemplate>
                                        <span class="legentStyle" title="<%#Eval("Status")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">&nbsp;</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="grdRows" />
                            <EmptyDataRowStyle CssClass="grdRows" />
                            <HeaderStyle CssClass="grdHead" />
                        </asp:GridView>
                        <asp:GridView ID="GridView6" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                            HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="false"
                            PageSize="5" CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left"
                                    ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%#Eval("NAME")%>
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
                                    ItemStyle-HorizontalAlign="Right">
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
    </form>
</body>
</html>
