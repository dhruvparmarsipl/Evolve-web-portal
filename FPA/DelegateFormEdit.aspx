<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelegateFormEdit.aspx.cs" Inherits="DelegateFormEdit" %>

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
            //window.opener.location.reload();
            window.location = window.location;
        };
   
        $(document).ready(function(){
        
        $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
        });
        function callcalender() {
        $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
    }
    
    $(document).ready(function() {
            $('#ExpCollBtn').click(function() {
                $('#ExpCollBtn > img').toggleClass('expcol');
                $('#Records').slideToggle("slow");
            });
        });
    </script>
<style type="text/css">
        .expcol {
            left: 0;
        }
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
                  <asp:HiddenField ID="HFSERIAL" runat="server" />

                <asp:HiddenField ID="HFUploadFileNo" runat="server" />
                <asp:HiddenField ID="hfMainRowNo" runat="server" />
                <asp:HiddenField ID="hfUpOnlyFileNameN" runat="server" />

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
                                            <h2 class="black18 wnor">Delegate FPA Claim <span id="spnFormCreater" runat="server"></span></h2>
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
                                        <td  width="11%" align="left" valign="top">
                                            <strong>Level</strong>
                                        </td>
                                        <td align="left" width="13%" valign="top">
                                         <span id="spnDesignation" runat="server"></span>
                                           
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Location</strong>
                                        </td>
                                        <td width="12%" align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server"></span>
                                            
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
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                   
                                </table>
                                </div>
                                <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">Delegate Claim Detail</h2>
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


                                    <tr runat="server" id="trUploadControls" visible="false">
                                        <td colspan="2" align="left" valign="top">
                                            <asp:FileUpload ID="FindFile" runat="server" />
                                        </td>
                                        <td colspan="6" align="left" valign="top">
                                            <asp:Button ID="butUpload" runat="server" Text="Upload" Width="100px" OnClick="butUpload_Click" />
                                        </td>
                                    </tr>

                                    <tr runat="server" id="trUploadGrid" visible="false">
                                        <td colspan="8" align="left" valign="middle">
                                            <asp:GridView ID="gvEntryDocs" Width="74%" runat="server" CellPadding="4" ForeColor="#333333"
                                                AutoGenerateColumns="False" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnlyFileName" runat="server" Text='<%#Eval("OnlyFileName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" Visible="false"
                                                        ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("FileName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("pk_id") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDeleteDoc_Click">
                                                                <img src="images/delete.png" alt="Delete" />
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#f7fbff" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle CssClass="grdHead" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <RowStyle CssClass="grdRows" />
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="100%" align="left" valign="top" colspan="6">
                                            <asp:GridView ID="gvClaimDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="30px" HeaderStyle-BackColor="#cecfce" OnRowDataBound="gvClaimDetail_RowDataBound">
                                                <Columns>
                                                 <asp:TemplateField HeaderText="Row Number" Visible="false">
                                                        <ItemTemplate>
                                                          <asp:Label ID="lblRownumber" Text='<%#Eval("RowNumber") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="11%" HorizontalAlign="Left" />
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
                                                        
                                                            <asp:Button ID="ButtonAdd" ValidationGroup="add" runat="server" CssClass="but" Text="Add" OnClick="ButtonAdd_Click" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GL Code">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGlcode" CssClass="inptnew inpt2new" runat="server" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allocation">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAllocation" CssClass="inptnew inpt2new" runat="server" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtAllocation" runat="server" TargetControlID="txtAllocation"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Head Balance">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHeadBalance" CssClass="inptnew inpt2new" runat="server" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtExpenseDate" onkeypress="return false" onkeydown="return false"
                                                                CssClass="inpbox2" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
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

                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Bill No">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBillNo" CssClass="inptnew inpt2new" runat="server" AutoPostBack="true"></asp:TextBox>
															 <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtBillNo" ErrorMessage="Special characters are not allowed!" 
                                                ValidationExpression="^[a-zA-Z0-9\\/#_-]+$" ValidationGroup="add"/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="View Bill" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                        <HeaderStyle Width="135px" />
                                                        <ItemTemplate>
                                                            <asp:Label Width="120px" ID="du" runat="server"></asp:Label>
                                                            <asp:GridView ID="gvInnerDocs" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="0" GridLines="None" ShowHeader="False"
                                                                OnRowCommand="gvInnerDocs_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInnerID" runat="server" Text='<%#Eval("InnerID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInnerFileName" runat="server" Text='<%#Eval("InnerFileName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkInnerDoc" runat="server" Width="130px" CommandName="Open" CommandArgument='<%#Eval("InnerOnlyFileName")%>' Text='<%#Eval("InnerOnlyFileName")%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInnerRowNo" runat="server" Text='<%#Eval("InnerRowNo")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDetail" CssClass="inpt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtDetail"
                                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                             <asp:RequiredFieldValidator ID="rqvtxtDetail" runat="server" ControlToValidate="txtDetail"
                                                                Display="Dynamic" ErrorMessage="Please Enter Details" Text="Details are mandatory to fill" ValidationGroup="add">
                                                            </asp:RequiredFieldValidator>
                                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtDetail"
                                                                WatermarkText="Please Enter detail Here.">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete"  CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDelete_Click">
                                                                <img src="images/icon-delete.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LinkEditBill" runat="server" ToolTip="Upload/Edit Bill" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClick="LinkEditBill_Click">
                                                                 <img src="images/edit-bill.png" height="25" width="25" alt="BillUpload" />
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
                                <asp:ValidationSummary ID="validsumm" runat="server" ValidationGroup="add" ShowMessageBox="true" ShowSummary="false" />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong>Mode Of Payment</strong>
                                        </td>
                                        <td width="80%" align="left" valign="top" class="greyBg">
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
                                    <div id="ExpCollBtn" style="height: 15px; width: 15px; overflow: hidden; float: left;
                                        position: relative">
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
                         Save As Draft</asp:LinkButton>
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
            <Triggers>
                <asp:PostBackTrigger ControlID="butUpload" />
                <asp:PostBackTrigger ControlID="gvClaimDetail" />
            </Triggers>
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
