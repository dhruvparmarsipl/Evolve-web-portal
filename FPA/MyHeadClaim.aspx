<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyHeadClaim.aspx.cs" Inherits="MyHeadClaim" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid td
        {
            background-color: White;
            color: black;
            font-size: 10pt;
            line-height:100%
        }
        .Grid th
        {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height:200%
        }
        .ChildGrid td
        {
            background-color: #eee !important;
            color: black;
            font-size: 10pt;
            line-height:100%
           
        }
        .ChildGrid th
        {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height:100%
        }
    </style>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.8.3.js"></script>

    <script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "img/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "img/plus.png");
        $(this).closest("tr").next().remove();
    });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnReport" runat="server" Text="Generate Report" CssClass="button" OnClick="btnReport_Click" />
        </div>&nbsp;
        <asp:GridView ID="gvMaster" runat="server" EmptyDataText="No reord found.." ShowFooter="true" AutoGenerateColumns="false"
            CssClass="Grid" OnRowDataBound="OnRowDataBound" DataKeyNames="fpa_head_id">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:GridView ID="gvDetal" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="ChildGrid">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="sap_voucherno" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Voucher No" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="claim_creation_date" HeaderText="Date Created"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="DETAILS" HeaderText="Claim Detail"
                                        HeaderStyle-HorizontalAlign="Left" />
                                   <asp:TemplateField HeaderText="Claim amount">
                                        <ItemTemplate>
                                            <div style="text-align: right; font-size: 11px;">
                                                <asp:Label ID="lblAlreadyClaimed" runat="server" Text='<%#Eval("claim_amt") %>'></asp:Label></div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right; font-size: 11px;">
                                                <asp:Label Style="text-align: right" ID="lblAlCliam" Font-Bold="true" runat="server"></asp:Label></div>
                                        </FooterTemplate>
                                        <ItemStyle Width="150px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Head Name" >
                    <ItemTemplate>
                        <asp:Label ID="lblAlreadyClaimed" runat="server" Text='<%#Eval("fpa_head") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right; font-size: 11px;">
                           <b>Total: </b>   <asp:Label Style="text-align: right" ID="lblTotal" Font-Bold="true" runat="server"></asp:Label></div>
                    </FooterTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
