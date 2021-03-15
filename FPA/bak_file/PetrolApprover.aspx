<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PetrolApprover.aspx.cs" Inherits="PetrolApprover" %>

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
        
        $("[id$=txtJourneyDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
        });
        function callcalender()
    {
        $("[id$=txtJourneyDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
    }
      function CheckOnFocus()
       {
      
        $("[id$=txtTotalKM]").click(function() {
        var rateperkm=document. getElementById('<%=txtRatePerKm.ClientID %>').value;
        if(rateperkm=="" ||rateperkm=="0")
        { 
           $("[id$=txtTotalKM]").val="";
           alert("Please Enter Rate");
           return false;
        }
        
        });
      }
   function showHide() {
      var ele = document.getElementById("Records");
      if(ele.style.display == "block") {
            ele.style.display = "none";
            $('#hrfHS').text("Show log");
        }
     else {
         ele.style.display = "block";
          $('#hrfHS').text("Hide log");
       }
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
                <asp:HiddenField ID="HFCARNO" runat="server" />
                <asp:HiddenField ID="HFCARSCHEME" runat="server" />
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
                                                Petrol Claim For&nbsp;<span id="spnFormCreater" runat="server"></span></h2>
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
                                            <strong>Approver</strong></td>
                                        <td>
                                            <span id="spnApprover" runat="server"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                                Claim Detail</h2>
                                        </td>
                                        <td align="right" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                                 Rate: <span>&#8377;</span> <span id="txtRatePerKm" runat="server"></span><span> per KM </span>
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="left" valign="top" colspan="2">
                                            <asp:GridView ID="gvPetrolClaimDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left" width="100%" HeaderStyle-Height="30px" HeaderStyle-BackColor="#cecfce">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Row Number" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRownumber" Text='<%#Eval("RowNumber") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="False" />
                                                    <asp:TemplateField HeaderText="Date Of Journey">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtJourneyDate" onkeypress="return false" onkeydown="return false"
                                                                CssClass="inpbox2" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rqvddlHeads" runat="server" ControlToValidate="txtJourneyDate"
                                                                Display="Dynamic" ValidationGroup="add" Text="*" ErrorMessage="Please Fill Date Of Journey">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Button ID="ButtonAdd" ValidationGroup="add" runat="server" CssClass="but" Text="Add"
                                                                OnClick="ButtonAdd_Click" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Point">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtStartPoint" CssClass="inpt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rqvtxtStartPoint" runat="server" ControlToValidate="txtStartPoint"
                                                                Display="Dynamic" ValidationGroup="add" Text="*" ErrorMessage="Please Enter Start Point">
                                                            </asp:RequiredFieldValidator>
                                                            <cc1:TextBoxWatermarkExtender ID="wattxtStartPoint" runat="server" TargetControlID="txtStartPoint"
                                                                WatermarkText="Please Enter Start Point Here.">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Point">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtEndPoint" CssClass="inpt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rqvtxtEndPoint" runat="server" ControlToValidate="txtEndPoint"
                                                                Display="Dynamic" ValidationGroup="add" Text="*" ErrorMessage="Please Enter End Point">
                                                            </asp:RequiredFieldValidator>
                                                            <cc1:TextBoxWatermarkExtender ID="wattxtEndPoint" runat="server" TargetControlID="txtEndPoint"
                                                                WatermarkText="Please Enter End Point Here.">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total KM">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTotalKM" CssClass="inptnew inpt2new" onfocus="javascript:CheckOnFocus();"
                                                                MaxLength="8" AutoPostBack="true" runat="server" OnTextChanged="txtTotalKM_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtBillAmount" runat="server" TargetControlID="txtTotalKM"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rqvtxtBillAmount" runat="server" ControlToValidate="txtTotalKM"
                                                                Display="Dynamic" ErrorMessage="Please Enter Kilometer" Text="*" ValidationGroup="add">
                                                            </asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBillAmount" CssClass="inptnew inpt2new" runat="server" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemTemplate>
														 <asp:Label ID="lblDetail" runat="server" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtDetail" CssClass="inpt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtDetail"
                                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtDetail"
                                                                WatermarkText="Please Enter Comments Here.">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="24%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDelete_Click">
                                                                 <img src="images/icon-delete.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" BackColor="#CECFCE" Height="30px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <asp:ValidationSummary ID="validsumm" runat="server" ValidationGroup="add" ShowMessageBox="true"
                                    ShowSummary="false" />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="10%" align="left" valign="top" class="greyBg">
                                            <strong>Total</strong>
                                        </td>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Comments</strong>
                                        </td>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>Approver Rejection Comments</strong>
                                        </td>
                                        <td width="30%" align="left" valign="top" class="greyBg">
                                            <strong>FI Admin Rejection Comments</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <span runat="server" id="spnTolalBillAmount"></span>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtComments" runat="server" Rows="3" TextMode="MultiLine" CssClass="inpt"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtComments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtAppRejectComment" runat="server" Rows="3" TextMode="MultiLine" CssClass="inpt"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtAppRejectComment" runat="server" TargetControlID="txtAppRejectComment"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarktxtAppRejectComment" runat="server"
                                                TargetControlID="txtAppRejectComment" WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtFIAdminreject" runat="server" Rows="3" CssClass="inpt" TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtFIAdminreject" runat="server" TargetControlID="txtFIAdminreject"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarktxtFIAdminreject" runat="server"
                                                TargetControlID="txtFIAdminreject" WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                        <tr>
                                            <td width="35%" align="left" valign="top">
                                            </td>
                                            <td width="10%" align="left" valign="top">
                                                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="button" ValidationGroup="add"
                                                    OnClick="btnSubmit_Click">
                         Approve</asp:LinkButton>
                                            </td>
                                            <td width="10%" align="left" valign="top">
                                                <asp:LinkButton ID="btnReject" runat="server" CssClass="button" OnClick="btnReject_Click">
                         Reject</asp:LinkButton>
                                            </td>
 <td width="10%" align="left" valign="top">
                                                <asp:LinkButton ID="btnDelete" runat="server" Visible="false" OnClientClick="return confirm('Are you sure you want to delete  Petrol claim');" CssClass="button">
                         Delete</asp:LinkButton></td>
                                            <td width="10%" align="left" valign="top">
                                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="button" OnClientClick="javascript:CloseWindowcan();">
                         Close</asp:LinkButton>
                                            </td>
                                            <td width="35%" align="left" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                </table>
                                  <a href="javascript:void(0);" id="hrfHS" onclick="return showHide();">Show log</a>&nbsp;
                                <div id="Records" style="display: none">
                                    <asp:GridView ID="grdHistory" Width="100%" runat="server" BorderStyle="Solid" BorderColor="Gray"
                                        BorderWidth="1" CellSpacing="0" CellPadding="2" AutoGenerateColumns="false" GridLines="None"
                                        AllowPaging="False" HeaderStyle-HorizontalAlign="Left">
                                        <EmptyDataRowStyle CssClass="greyBg" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <HeaderStyle BackColor="#CCCECF" />
                                        <AlternatingRowStyle BackColor="#f4f9ff" />
                                        <Columns>
                                            <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Performed By" HeaderText="Performed By" HeaderStyle-Width="20%"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Date Time" HeaderText="Date Time" HeaderStyle-Width="15%"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="COMMENTS" HeaderText="Comments" HeaderStyle-Width="45%"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
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
