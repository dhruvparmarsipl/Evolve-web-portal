<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeleteFpaClaim.aspx.cs" Inherits="DeleteFpaClaim" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrappernew">
            <div class="row greyBgnew">
                <div class="pageWidthnewPage">
                    <div class="row mBtm20">
                        <div class="floatRight Luser">
                            <span id="spnLoginUser" runat="server"></span>
                        </div>
                        <div class="form-wrapper">
                            <div class="request-form">
                                <asp:GridView ID="gvDeleteHistory" Width="100%" runat="server" 
                                    HeaderStyle-BackColor="#cecfce" AllowPaging="True"
                                    PageSize="15" OnPageIndexChanging="gvDeleteHistory_PageIndexChanged" CellPadding="6" CellSpacing="0"
                                    AutoGenerateColumns="false" EmptyDataText="No record found.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Serial No." HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserialNo" Text='<%#Eval("SerialNo") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSapid" runat="server" Text='<%#Eval("SapId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("CLAIM_STATUS") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbloginname" runat="server" Text='<%#Eval("CreationDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Eval("Amount")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Eval("Status")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("SerialNo") %>'
                                                    OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDelete_Click">
                                                                 <img src="images/icon-delete.png" alt="Delete" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grdHeadcss" />
                                </asp:GridView>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
