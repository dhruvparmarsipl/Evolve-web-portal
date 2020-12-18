<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyApproval.aspx.cs" Inherits="MyApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title>My Approval</title>
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
            
           <div style="display:block; font-size:10px;float:right; vertical-align:top; padding-bottom:5px; padding-left:25px; padding-right:2px;">
             Approved/Closed&nbsp;&nbsp;<span class="legentStyle" style="background-color:#00ff00;" >&nbsp;</span>&nbsp;|&nbsp;
             In Progress &nbsp;&nbsp;<span class="legentStyle" style="background-color:#ff9e10;" >&nbsp;</span>&nbsp;|&nbsp;
             Draft &nbsp;&nbsp;<span style="background-color:#ffdb63;" class="legentStyle">&nbsp;</span>&nbsp;|&nbsp;
             Rejected&nbsp;&nbsp;<span style="background-color:#ff0000;" class="legentStyle">&nbsp;</span>
            
           </div>
           </div>
            </div>
            
            
            <div style="float:left; padding-top:5px; padding-bottom:5px; font-family:Segoe UI,Calibri, Arial, Helvetica, sans-serif !important; font-size:10pt;"><b>Waiting For My Approval</b>
            </div>
            <div style="float: left;width: 100%;"><%--GrdApproval--%>
              <asp:GridView ID="GrdApproval" Width="100%" runat="server" CssClass="grdMain"
                EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" 
                RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="true" PageSize="15"
                OnPageIndexChanging="grdUser_PageIndexChanged" CellPadding="5" CellSpacing="0" 
                AutoGenerateColumns="false">
                  <Columns>
                    <asp:TemplateField HeaderText="SL No." ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                        <a id="aBtnRedirectTo" target="_blank" 
                        href='MediApprover.aspx?onrs=<%#DoEncrypt(Eval("SERIAL_NO").ToString()) %>'>
                        <%#Eval("EmpName")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Year of Claim" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                           <%#Eval("YEAROFCLAIM")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Created Date" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                           <%#Eval("CREATED_DATE") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                         <span class="legentStyle" title="<%#Eval("StatusText")%>" style="cursor:pointer; background-color:<%#Eval("COLOR")%>;" >&nbsp;</span>
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
                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
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
                    <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left"  ItemStyle-Width="20%" HeaderText="Status">
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
