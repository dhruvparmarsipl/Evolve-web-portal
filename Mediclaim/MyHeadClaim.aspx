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
        <link href="css/css_style.css" rel="stylesheet" type="text/css" />

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
        <div style="position: relative" class="row mBtm20">
            <table class="formB brd" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="head" valign="top" align="left" colspan="5">
                            <h2 class="black18 wnor">
                                Details </h2>
                        </td>
                    </tr>
                    <tr>
                        <td class="greyBg" valign="top" align="left" width="12%">
                            <strong>BU</strong></td>
                        <td class="greyBg" valign="top" align="left" width="16%">
                            <strong>Year</strong></td>
                        <td class="greyBg" valign="top" align="left" width="16%">
                            <strong>Status</strong></td>
                        <td class="greyBg" valign="top" align="left" width="7%" >
                            <strong></strong>
                        </td>
                        <td class="greyBg" valign="top" align="left" >
                            <strong></strong>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="ddlBu" runat="server" CssClass="sel">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPlan" runat="server" Display="Dynamic" Text="*"
                                ErrorMessage="Please Select BU" InitialValue="0" ControlToValidate="ddlBu" ValidationGroup="vg1">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="sel">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvddlSumInsured" runat="server" Display="Dynamic"
                                Text="*" ErrorMessage="Please select year" InitialValue="0" ControlToValidate="ddlBu"
                                ValidationGroup="vg1">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="sel">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                                Text="*" ErrorMessage="Please select status" InitialValue="0" ControlToValidate="ddlStatus"
                                ValidationGroup="vg1">
                            </asp:RequiredFieldValidator></td>
                        <td valign="top" align="left">
                           <asp:Button ID="btnGetData" ValidationGroup="vg1" runat="server" Text="Get Data" CssClass="button" OnClick="btnGetData_Click"
                                />
                        </td>
                        <td valign="top" align="left">
                         <asp:Button ID="btnReport" runat="server" Text="Export to Excel" Visible="false" CssClass="button"
                                OnClick="btnReport_Click" />
                            
                        </td>
                    </tr>
                    
                </tbody>
            </table>
        </div>
        <div>
           
        </div>&nbsp;
        <asp:GridView ID="gvMaster" runat="server" EmptyDataText="No reord found.." ShowFooter="true"
            AutoGenerateColumns="false" CssClass="Grid" OnRowDataBound="OnRowDataBound" DataKeyNames="Id,Sapid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:GridView ID="gvDetal" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                CssClass="ChildGrid">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="NAME" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Dependant Name" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="DOB" HeaderText="Date Of Birth"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="GENDER" HeaderText="Gender" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="RELATION" HeaderText="Relation"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="BLOODGROUP" HeaderText="Blood Group"
                                        HeaderStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Ename" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Ecode" HeaderText="Ecode" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DOJ" HeaderText="DOJ" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DOB" HeaderText="DOB" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Age" HeaderText="Age" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Employee Type" HeaderText="Employee Type" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Sum Insured" HeaderText="Sum Insured" HeaderStyle-HorizontalAlign="Left" />
                   <asp:TemplateField HeaderText="Actual Premiun" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblActPreuim" runat="server" Text='<%#Eval("ActualPremium") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right; font-size: 11px;">
                            <b>Total: </b>
                            <asp:Label Style="text-align: right" ID="lblTotal" Font-Bold="true" runat="server"></asp:Label></div>
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="Employee Premium" HeaderText="Employee Premium" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Mobile No" HeaderText="Mobile No" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Dependency Nos." HeaderText="Dependency Nos." HeaderStyle-HorizontalAlign="Left" />

            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
