<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FIAdminApp.aspx.cs" Inherits="FIAdminApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FI Admin Approval</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/intranetdash.css" rel="stylesheet" />
    <style type="text/css">
    .btnclass
    {
    	background-color:#123575;
    	color:White;
    	border:1px solid #f3f6fc;
        font-weight:bold;
    }
   .btnclass:hover
   {
   	background-color:#9cc1f5;
   }
       .a99999999
        {
color:red;
        }
    </style>
    <script type="text/javascript">
        function Callonloaddata() {
            var elems = document.getElementsByClassName('a99999999');
            var confirmit = function (e) {
                if (!confirm('Employee is seperated. Do you want to continue ?'))
                    e.preventDefault();
            };
            for(var i=0,l=elems.length;i<l;i++)
            {
                elems[i].addEventListener('click',confirmit,false);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hfSechVal" runat="server" />
                <div id="wrapper">
                    <div id="cWrap">
                        <div id="pageWrap">
                            <div class="toggle_container">
                                <div class="data_row">
                                    <div style="padding-bottom: 10px;">
                                        <b>FI Admin Approval<br />
                                            Please Enter Name or Voucher Serial Number to search</b><br />
                                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="*" ID="RFVCLAIMAMT" Display="Dynamic" runat="server"
                                            ControlToValidate="txtSearch" SetFocusOnError="true" ValidationGroup="Search" />
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btnclass" OnClick="btnSearch_OnClick"
                                            ValidationGroup="Search" />
                                    </div>
                                    <asp:GridView ID="GrdCoachApproval" Width="100%" runat="server" CssClass="grdMain"
                                        EmptyDataRowStyle-CssClass="grdRows" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows"
                                        GridLines="None" AllowPaging="True" PageSize="15" OnPageIndexChanging="GrdCoachApproval_PageIndexChanged"
                                        CellPadding="6" CellSpacing="0" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Waiting for Approval" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a target="_blank" class="a<%#Eval("Positionid") %>" href="FpaApprover.aspx?RID=<%#DoEncrypt(Eval("APP_SERIAL_NO").ToString()) %>">
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
                                                    <span class="legentStyle" title="<%#Eval("Status")%>" style="cursor: pointer; background-color: #<%#Eval("COLOR")%>;">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
