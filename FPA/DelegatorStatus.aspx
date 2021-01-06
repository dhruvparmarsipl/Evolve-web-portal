<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelegatorStatus.aspx.cs" Inherits="DelegatorStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delegator's Status</title>
     <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="js/uc.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/intranetdash.css" rel="stylesheet" />
</head>
<body>
     <form id="form1" runat="server">
    <asp:ScriptManager ID="scrpt1" runat="server"></asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
     <div id="wrapper">
        <div id="cWrap">
            <div id="pageWrap">
                <div class="toggle_container">
                   <asp:GridView ID="GrdCoachApproval" Width="100%" runat="server" CssClass="grdMain"
                EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows"
                GridLines="None" AllowPaging="True" PageSize="15" OnPageIndexChanging="GrdCoachApproval_PageIndexChanged" CellPadding="6" CellSpacing="0"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Employee Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                          <a target="_blank" href="DelegateFormEdit.aspx?RID=<%#Eval("APP_SERIAL_NO") %>"> <%#Eval("NAME")%></a>
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
                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Status">
                        <ItemTemplate>
                         <span class="legentStyle" title="<%#Eval("Status")%>" style=" cursor:pointer; background-color:#<%#Eval("COLOR")%>;" >&nbsp;</span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <RowStyle CssClass="grdRows" />
                <EmptyDataRowStyle CssClass="grdRows" />
                <HeaderStyle CssClass="grdHead" />
            </asp:GridView>            
             <asp:GridView ID="GridView1" Width="100%" runat="server" CssClass="grdMain"
                EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows"
                GridLines="None" AllowPaging="false" PageSize="5" CellPadding="6" CellSpacing="0"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
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
                    <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
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
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
