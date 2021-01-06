<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyFPAHistory.aspx.cs" Inherits="MyFPAHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>My FPA History</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/intranetdash.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrpt1" runat="server"></asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
     <div id="Div1">
        <div id="Div2">
            <div id="Div3">
                <div class="toggle_container">
                    <div class="data_row">
                    <asp:HiddenField ID="hfUserSAP" runat="server" />
                        <div class="selectYear">
                      <div class="yr_01"> Filter by Year </div><div class="yr_02"> <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="ddlYear_SelectedIndexChanged"></asp:DropDownList></div>

                            </div>
                       
                           <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True"
                                PageSize="5" OnPageIndexChanging="grdUser_PageIndexChanged" CellPadding="6" CellSpacing="0"
                                AutoGenerateColumns="false" EmptyDataText="No record found..">
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <a target="_blank" href="ShowDetails.aspx?RID=<%#Eval("fpa_cycle_id") %>">
                                                <%#Eval("Ename")%>
                                            </a>
                                        </ItemTemplate>
                                        <HeaderStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created Date" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedOn" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="10%" HeaderText="Status">
                                    <ItemTemplate>
                                        <span class="legentStyle" title="<%#Eval("CURRENT_STATUS")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">
                                            &nbsp;</span>
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
