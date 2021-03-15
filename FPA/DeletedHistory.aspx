<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeletedHistory.aspx.cs" Inherits="DeletedHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <link rel="stylesheet" href="css/global.css" />
   <link href="css/GET.css" rel="stylesheet" type="text/css" />
    <link href="css/uc.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/intranetdash.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
       <%-- <div id="wrapper">
            <div id="cWrap">
                <div id="pageWrap">
                    <div class="toggle_container">--%>
         <div class="form-wrapper">
             <div class="request-form">
                        <asp:GridView ID="gvDeleteHistory" Width="100%" runat="server" CssClass="grdMain" EmptyDataRowStyle-CssClass="grdRows"
                            HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRows" GridLines="None" AllowPaging="True" 
                            PageSize="15" OnPageIndexChanging="gvDeleteHistory_PageIndexChanged" CellPadding="6" CellSpacing="0"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField DataField="Ecode" HeaderText="Ecode" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DeletedBy" HeaderText="Deleted By" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DeletedDate" HeaderText="Deleted Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />

                    </Columns>
                    <HeaderStyle CssClass="grdHeadcss" />
                </asp:GridView>
				</br>
                <div style="text-align: right;">
                    <asp:Button ID="btnExporttoExcel" runat="server" CssClass="btnSave alignRight" Text="Export To Excel" OnClick="btnExporttoExcel_Click" Visible="false" />
                </div>
            </div>
        </div>
        <%--  </div>
            </div>
        </div>--%>
    </form>
</body>
</html>
