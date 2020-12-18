<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessedClaimByHr.aspx.cs" Inherits="ProcessedClaimByHr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title>Processed Claim</title>
    <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt1" runat="server">
        </asp:ScriptManager>
       
                <div id="wrapper">
                    <div id="cWrap">
                        <div id="pageWrap">
                            <div class="toggle_container">
                                <div>
                                    <asp:HiddenField ID="hfUserSAP" runat="server" />
                                   
                                    <asp:GridView ID="grdUser" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                                        HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" 
                                         CellPadding="6"
                                        CellSpacing="0" AutoGenerateColumns="false"  OnDataBound="OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a target="_blank" href="MediApproverClosed.aspx?onrs=<%#DoEncrypt(Eval("SERIAL_NO").ToString()) %>&flag=<%#DoEncrypt(("1").ToString()) %>">
                                                        <%#Eval("Ename")%>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
 <asp:TemplateField HeaderText="Ecode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEcode" runat="server" Text='<%#Eval("Ecode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MediClaim Year" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMediyear" runat="server" Text='<%#Eval("Mediyear") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created On" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
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
                                            <asp:TemplateField HeaderText="Name" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="left"
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
             <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript" src="js/quicksearch.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.search_textbox').each(function (i) {
                $(this).quicksearch("[id*=grdUser] tr:not(:has(th))", {
                    'testQuery': function (query, txt, row) {
                        return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
    </script>
    </form>
</body>
</html>