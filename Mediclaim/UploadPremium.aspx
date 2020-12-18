<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadPremium.aspx.cs" Inherits="UploadPremium" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title></title>
    <link href="css/css_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
    <table width="100%" cellpadding="0" cellspacing="4" border="0">
        <tr>
            <th class="pageHead" align="center">Upload Premium Master Sheet</th>
        </tr>
        <tr>
            <td align="left" valign="top">
               To download template <a href="Files/File1.xls">Click</a> here.<br/>
               <p class="txtRed">Instructions:</p>&nbsp;&nbsp;Please change necessary fields only and upload.
            </td>
        </tr>
        <tr>
            <td height="20px"></td>
        </tr>
        <tr>
            <td><asp:FileUpload ID="fuExcel"  runat="server" BorderColor="#cccccc" BorderWidth="1px"  /></td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Button ID="BtnUpload" runat="server" CssClass="button"
            Text="Click To Upload Execl To Database" onclick="BtnUpload_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:GridView ID="grdUploaded" runat="server" CellPadding="4" 
                CssClass="grdMain" ForeColor="#333333" GridLines="Both"  
                BorderColor="#cccccc" BorderWidth="1px">
                <FooterStyle  Font-Bold="True" HorizontalAlign="Right" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" 
                ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" 
                HorizontalAlign="Center" />
                <HeaderStyle  ForeColor="Black" BackColor="#dce8f6" />
                <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>