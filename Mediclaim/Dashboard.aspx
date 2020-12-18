<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="ApproverDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DashBoard</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updgrid" runat="server">
   <ContentTemplate>
    <div id="wrapper">
       <div id="cWrap">
         <div id="pageWrap">
           <div class="toggle_container">
           <div> 
            <div style=" margin-bottom:10px;float: left;width: 100%;">
            <div style="float:left;font-family:Segoe UI,Calibri, Arial, Helvetica, sans-serif !important; font-size:10pt;"><b>My Mediclaims Status</b>
            </div>
           <div style="display:block; font-size:10px;float:right; vertical-align:top; padding-bottom:5px; padding-left:25px; padding-right:2px;">
             Approved/Closed&nbsp;&nbsp;<span class="legentStyle" style="background-color:#00ff00;" >&nbsp;</span>&nbsp;|&nbsp;
             In Progress &nbsp;&nbsp;<span class="legentStyle" style="background-color:#ff9e10;" >&nbsp;</span>&nbsp;|&nbsp;
             Draft &nbsp;&nbsp;<span style="background-color:#ffdb63;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
             Rejected&nbsp;&nbsp;<span style="background-color:#ff0000;" class="legentStyle">&nbsp;</span>
             
           </div>
           </div>
		   <div style="margin-bottom:10px;"> 
           <asp:HyperLink ID="Premium_Table" runat="server" Text="Click here to see Premium Table (2016-17)" style="color:#bc653a; font-size:11px; font-weight:bold; " Target="_blank"></asp:HyperLink>
           </div>
            <div style="margin-bottom:10px;float: left;width: 100%;">
             <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain"
                EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" 
                RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True" PageSize="5"
                OnPageIndexChanging="GrdApproval_PageIndexChanged" CellPadding="6" CellSpacing="0"
                AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#f3f6f9">
                <Columns>
                    <asp:TemplateField HeaderText="Serial No." ItemStyle-Width="25%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                          <a target="_blank" title="Click To Approve"
                           href="Mediclaim.aspx?onrs=<%#DoEncrypt(Eval("SERIAL_NO").ToString()) %>">
                      
                           <%#Eval("SERIAL_NO")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Year of Claim" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <%#Eval("YEAROFCLAIM") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Created On" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                           <%#Eval("CREATED_DATE") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                         <span class="legentStyle" title="<%#Eval("StatusText")%>" style="cursor:pointer; background-color:<%#Eval("COLOR")%>;" >&nbsp;</span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="grdRows" />
                <EmptyDataRowStyle CssClass="grdRows" />
                <HeaderStyle CssClass="grdHead" />
            </asp:GridView>
            
             <asp:GridView ID="GridView2" Width="100%" runat="server" CssClass="grdMain"
                EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" 
                RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="false" PageSize="5" 
                CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="SL No." ItemStyle-Width="25%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                         <%#Eval("CREATED_DATE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="From Date" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="To Date" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
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
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
