<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelegateAssigned.aspx.cs" Inherits="DelegateAssigned" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delegate Assigned</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
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
                    <div class="data_row">
                        <div>
                          <div class="ms-standardheader ms-WPTitle">Delegate List</div>
                            <div class="legentAlign" style="margin-top:10px;">
             Approved/Closed&nbsp;&nbsp;<span class="legentStyle" style="background-color:#00ff00;">&nbsp;</span>&nbsp;|&nbsp;
             In Progress &nbsp;&nbsp;<span class="legentStyle" style="background-color:#ff9e10;">&nbsp;</span>&nbsp;|&nbsp;
             Draft &nbsp;&nbsp;<span style="background-color:#ffdb63;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
             Rejected&nbsp;&nbsp;<span style="background-color:#ff0000;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
             Initiated&nbsp;&nbsp;<span style="background-color:#0064ac;" class="legentStyle">&nbsp;</span>
           </div>

                        </div>


                   <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                            HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True"
                            PageSize="15" OnPageIndexChanging="grdUser_PageIndexChanged" CellPadding="6" CellSpacing="0"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Employee Name" HeaderStyle-HorizontalAlign="left" 
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a target="_blank" href="DelegateFormNew.aspx?EMPSAP=<%#Eval("SAPID") %>"><%#Eval("ename")%></a>
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
                                <asp:TemplateField HeaderText="Employee Name"  HeaderStyle-HorizontalAlign="left"
                                    ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%#Eval("ename")%>
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
