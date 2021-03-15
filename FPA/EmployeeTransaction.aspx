<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeTransaction.aspx.cs"
    Inherits="EmployeeTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA Initiation</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>

    <script type="text/javascript">
        function CloseWindowcan() {
            window.open('', '_self', '');
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
             <asp:HiddenField ID="HFCYCLEID" runat="server" />
            <div id="wrappernew">
                <div class="row greyBgnew">
                    <div class="pageWidthnew">
                        <div class="row mBtm20">
                            <div class="floatRight Luser">
                                <span id="spnLoginUser" runat="server"></span>
                            </div>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brdnew">
                                <tr>
                                    <td colspan="4" align="left" valign="top" class="head">
                                        <h2 class="black16new wnornew">
                                            Make Transaction For Employee <span id="spnFormCreater" runat="server" visible="false">
                                            </span>
                                        </h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="25%" align="left" valign="top">
                                        <strong>Serial No</strong>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <span id="spnSerialNo" runat="server"></span>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <strong>Employee code</strong>
                                    </td>
                                    <td width="25%" align="left" valign="top">
                                        <span id="spnEmpCode" runat="server"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <strong>Location</strong>
                                    </td>
                                    <td align="left" valign="top">
                                        <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                        </span>
                                    </td>
                                    <td align="left" valign="top">
                                        <strong>Designation</strong>
                                    </td>
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
                                        <h2 class="black16new wnornew">
                                            Employee Information
                                        </h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%" align="left" valign="top" class="greyBg">
                                        <strong>Select Location</strong>
                                    </td>
                                    <td width="30%" align="left" valign="top" class="greyBg">
                                        <strong>Select Designation</strong>
                                    </td>
                                    <td width="30%" align="left" valign="top" class="greyBg">
                                        <strong>Select Employee</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlLocation" CssClass="selnew" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLocation"
                                            Display="Dynamic" ErrorMessage="Please select location" InitialValue="0" Text="*" ValidationGroup="add">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlDesignation" CssClass="selnew" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDesignation"
                                            Display="Dynamic" ErrorMessage="Please select designation" InitialValue="0" Text="*" ValidationGroup="add">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlEmployee" CssClass="selnew" runat="server" 
                                            AutoPostBack="true" onselectedindexchanged="ddlEmployee_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEmployee"
                                            Display="Dynamic" ErrorMessage="Please select employee" InitialValue="0" Text="*" ValidationGroup="add">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row mBtm20">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                <tr>
                                    <td colspan="5" align="left" valign="top" class="head">
                                        <h2 class="black16new wnornew">
                                            Head Balance Information
                                        </h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%" align="left" valign="top" class="greyBg">
                                        <strong>Head</strong>
                                    </td>
                                    <td width="10%" align="left" valign="top" class="greyBg">
                                        <strong>GL Code</strong>
                                    </td>
                                    <td width="20%" align="left" valign="top" class="greyBg">
                                        <strong>Annual Allocation</strong>
                                    </td>
                                    <td width="20%" align="left" valign="top" class="greyBg">
                                        <strong>Current Balance</strong>
                                    </td>
                                    <td width="30%" align="left" valign="top" class="greyBg">
                                        <strong>Amount</strong>
                                    </td>
                                     
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlHead" Width="120px" runat="server" AutoPostBack="true"
                                       onselectedindexchanged="ddlHead_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlHead"
                                            Display="Dynamic" ErrorMessage="Please select head" InitialValue="0" Text="*" ValidationGroup="add">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblGlCode" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblAnAllocation" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblCurBalance" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="9" Width="80px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredtxtBillAmount" runat="server" TargetControlID="txtAmount"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rqvtxtBillAmount" runat="server" ControlToValidate="txtAmount"
                                            Display="Dynamic" ErrorMessage="Please enter amount" Text="*" ValidationGroup="add">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                <tr>
                                 <strong>Detail</strong>:
                                    <td width="20%" align="left" valign="top" class="greyBg" colspan="5">
                                       <asp:TextBox ID="txtDetail" runat="server" TextMode="MultiLine" Width="250px" Height="40px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="40%" align="left" valign="top">
                                    </td>
                                    <td width="10%" align="left" valign="top">
                                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="button" 
                                            ValidationGroup="add" onclick="btnUpdate_Click">
                         Update</asp:LinkButton>
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
