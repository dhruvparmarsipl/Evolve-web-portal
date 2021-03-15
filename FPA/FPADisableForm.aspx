<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FPADisableForm.aspx.cs" Inherits="FPADisableForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA Disable</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>

    <script type="text/javascript">
   function CloseWindowcan()
   {
    window.open('','_self','');
    window.close();
   }
   function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            window.opener.location.reload();
        }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scp" runat="server">
        </asp:ScriptManager>

        <script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        
        function BeginRequestHandler(sender, args) {
        //var popup = $find('<%= modalPopup.ClientID %>');
        var popup = $find('<%= modalPopup.ClientID %>');
        if (popup != null) {
        popup.show();
        }
        }

       function EndRequestHandler(sender, args) {
       var popup = $find('<%= modalPopup.ClientID %>');
       if (popup != null) {
       popup.hide();
       }
      }
      
        </script>

        <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HFSAPID" runat="server" />
                <asp:HiddenField ID="HFID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
                <div id="wrappernew">
                    <div class="row greyBgnew">
                        <div class="pageWidthnewPage">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span>
                                </div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brdnew">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                FPA Disable <span id="spnFormCreater" runat="server"></span>
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Initiator Name</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnIntName" runat="server"></span>
                                        </td>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Employee code</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>Location</strong></td>
                                        <td align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                            </span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>Designation</strong></td>
                                        <td align="left" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
                                        </td>
                                        <span id="spnSrNo" visible="false" runat="server"></span>
                                    </tr>
                                </table>
                            </div>
                            <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                                Employee Information
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Select Location</strong></td>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Select Designation</strong></td>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Select Employee</strong></td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlLocation" CssClass="selnew" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlDesignation" CssClass="selnew" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlEmployee" CssClass="selnew" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="12%" align="left" valign="top">
                                            <strong>Employee code</strong></td>
                                        <td width="13%" align="left" valign="top">
                                            <span id="spnECode" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Car Scheme</strong></td>
                                        <td width="12%" align="left" valign="top">
                                            <span id="spcarscheme" runat="server"></span>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="40%" align="left" valign="top">
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="btnDisable" runat="server" CssClass="button" OnClick="btnDisable_Click">
                         Disable</asp:LinkButton>
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="button" OnClientClick="javascript:CloseWindowcan();">
                         Close</asp:LinkButton>
                                        </td>
                                        <td width="40%" align="left" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="img/animated_loading.gif" AlternateText="Processing"
                    runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
            PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    </form>
</body>
</html>
