<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelegateForm.aspx.cs" Inherits="DelegateForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA Claim</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <link href="Date/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Date/jquery-ui.min.js" type="text/javascript"></script>

    <%-- <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>--%>

    <script type="text/javascript">
        function CloseWindowcan() {
            window.open('', '_self', '');
            window.close();
        }
        function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            window.opener.location.reload();
        };
   
        $(document).ready(function(){
        
        $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
        });
        function callcalender()
    {
        $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
    }
    
   
        $(document).ready(function() {
            $('#ExpCollBtn').click(function() {
                //alert('1');
                $('#ExpCollBtn > img').toggleClass('expcol');
                $('#Records').slideToggle("slow");
            });
        });
    
    </script>
<style type="text/css">
.expcol{right:0;}
</style>
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
            Sys.Application.add_load(callcalender);
      
        </script>

        <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HFSAPID" runat="server" />
                <asp:HiddenField ID="HFID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
                <asp:HiddenField ID="HFBU" runat="server" />
                <asp:HiddenField ID="HFCOSTCENTER" runat="server" />
                <asp:HiddenField ID="HFSTATUS" runat="server" />
                <div id="wrapper">
                    <div class="row greyBg">
                        <div class="pageWidth">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span>
                                </div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="8" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                               Delegate FPA Claim <span id="spnFormCreater" runat="server"></span></h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="13%" align="left" valign="top">
                                            <strong>Serial No</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnSrNo" runat="server"></span>
                                        </td>
                                        <td width="12%" align="left" valign="top">
                                            <strong>Employee Code</strong>
                                        </td>
                                        <td width="13%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Level</strong>
                                        </td>
                                        <td align="left" width="13%" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Location</strong>
                                        </td>
                                        <td width="12%" align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>Current Status</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="SpnStatus" runat="server">Draft</span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>Department</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="spndepartment" runat="server"></span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>FI Admin</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="spnFIAdmin" runat="server"></span>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    
                                </table>
                                 </div>
                                  <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                               Delegate Claim Detail</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Total FPA Balance</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnTotalFpaBalance" runat="server"></span>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Claim In Process</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="SpnProcessClaim" runat="server"></span>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Total FPA Claim</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnTotalFpaClaim" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="left" valign="top" colspan="6">
                                            <asp:GridView ID="gvClaimDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="30px" HeaderStyle-BackColor="#cecfce"  OnRowDataBound="gvClaimDetail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Row Number" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRownumber" Text='<%#Eval("RowNumber") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="False" />
                                                    <asp:TemplateField HeaderText="Heads">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlHeads" AutoPostBack="true" CssClass="sel" runat="server"
                                                                OnSelectedIndexChanged="ddlHeads_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rqvddlHeads" runat="server" ControlToValidate="ddlHeads"
                                                                InitialValue="0" Display="Dynamic" ValidationGroup="add" Text="*" ErrorMessage="Please Select FPA Heads">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Button ID="ButtonAdd" ValidationGroup="add" runat="server" CssClass="but" Text="Add"
                                                                OnClick="ButtonAdd_Click" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GL Code">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGlcode" Enabled="false" CssClass="inptnew inpt2new" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allocation">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAllocation" CssClass="inptnew inpt2new" runat="server" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtAllocation" runat="server" TargetControlID="txtAllocation"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Head Balance">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHeadBalance" CssClass="inptnew inpt2new" Enabled="false" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Expense Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtExpenseDate" onkeypress="return false" onkeydown="return false"
                                                                CssClass="inpbox2" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBillAmount" CssClass="inptnew inpt2new" runat="server" AutoPostBack="true"
                                                                OnTextChanged="txtBillAmount_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtBillAmount" runat="server" TargetControlID="txtBillAmount"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rqvtxtBillAmount" runat="server" ControlToValidate="txtBillAmount"
                                                                Display="Dynamic" ErrorMessage="Please Enter Amount" Text="*" ValidationGroup="add">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDetail" CssClass="inpt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtDetail"
                                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"
                                                                WatermarkText="Please Enter Comments Here.">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDelete_Click">
                                                                 <img src="images/icon-delete.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="6%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <asp:ValidationSummary ID="validsumm" runat="server" ValidationGroup="add" ShowMessageBox="true"
                                    ShowSummary="false" />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Mode Of Payment</strong>
                                        </td>
                                        <td width="70%" align="left" valign="top" class="greyBg">
                                            <strong>Comments</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:RadioButtonList ID="rdbtnModeOfPayment" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">Bank</asp:ListItem>
                                                <asp:ListItem Value="2">Cash</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtComments" runat="server" Rows="3" CssClass="inpt"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtComments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                                
                                
                                      <div id="ExpCollBtn" style="height:15px;width:15px;overflow:hidden;float:left;position:relative">
                                          <img src="images/icon_ecp.gif" style="position: absolute;" alt="Click to see all records"
                                              title="Click Here" />
                                      </div>
                                      <div style="float: left; color: #4285f4; margin-left: 2px;">
                                          <b>Show History</b></div><br />&nbsp;
                                      <div id="Records" style="display:none">
                                          <asp:GridView ID="grdHistory" Width="100%" runat="server" BorderStyle="Solid" BorderColor="Gray"
                                              BorderWidth="1" CellSpacing="0" CellPadding="2" AutoGenerateColumns="false" GridLines="None"
                                              AllowPaging="False" HeaderStyle-HorizontalAlign="Left">
                                              <EmptyDataRowStyle CssClass="greyBg" />
                                              <RowStyle HorizontalAlign="Left" />
                                              <HeaderStyle BackColor="#CCCECF" />
                                              <AlternatingRowStyle BackColor="#f4f9ff" />
                                              <Columns>
                                                  <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" />
                                                  <asp:BoundField DataField="Performed By" HeaderText="Performed By" HeaderStyle-Width="15%"
                                                      HeaderStyle-HorizontalAlign="Left" />
                                                  <asp:BoundField DataField="Date Time" HeaderText="Date Time" HeaderStyle-Width="15%"
                                                      HeaderStyle-HorizontalAlign="Left" />
                                                  <asp:BoundField DataField="COMMENTS" HeaderText="Comments" HeaderStyle-Width="55%"
                                                      HeaderStyle-HorizontalAlign="Left" />
                                              </Columns>
                                          </asp:GridView>
                                      </div>
                                      <br />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="35%" align="left" valign="top">
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="button" ValidationGroup="add" OnClick="btnSubmit_Click">
                         Submit</asp:LinkButton>
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="btnSaveDraft" runat="server" CssClass="button" OnClick="btnSaveDraft_Click">
                         Save as Draft</asp:LinkButton>
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="button" OnClientClick="javascript:CloseWindowcan();">
                         Close</asp:LinkButton>
                                        </td>
                                        <td width="35%" align="left" valign="top">
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
