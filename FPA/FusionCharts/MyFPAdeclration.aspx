<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyFPAdeclration.aspx.cs"
    Inherits="MyFPAdeclration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>My FPA Declaration</title>
    <%-- <link href="css/css_Style.css" rel="stylesheet" type="text/css" />--%>
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
        
      
   function clearContents(element) {
if(element.value=='0')
{
 element.value = '';
}
 
};
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
  
  function isNumberKey(evt) {
    
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }  
    </script>

    <style type="text/css">
.expcol{right:0;}
</style>
</head>
<body >
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
                <asp:HiddenField ID="HFPAID" runat="server" />
                <asp:HiddenField ID="HFCYCLEID" runat="server" />
                <asp:HiddenField ID="HFDESID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
                <asp:HiddenField ID="HFCSTATUS" runat="server" />
				<asp:HiddenField ID="HFCARSCHEME" runat="server" />
				<asp:HiddenField ID="HFBU" runat="server" />
                <div id="wrappernew">
                    <div class="row greyBgnew">
                        <div class="pageWidthnewPage">
                            <div class="rownew mBtm20new">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span>
                                </div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brdnew">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                Flexi Package Allowance for <span id="spnFormCreater" runat="server"></span></h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Employee code</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Designation</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
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
                                            <strong>Current Status</strong></td>
                                        <td align="left" valign="top">
                                            <span id="SpnStatus" runat="server">Draft</span></td>
                                        <span id="spnSrNo" visible="false" runat="server"></span>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>Created on</strong></td>
                                        <td align="left" valign="top">
										<span id="spncreatedon" runat="server"></span>
                                           
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>FPA Admin</strong></td>
                                        <td align="left" valign="top">
                                            <span id="spnFPAAdmin" runat="server"></span>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </div>
                            <div class="rownew mBtm20new">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brdnew">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                Details</h2>
                                        </td>
                                    </tr>
                                      <tr id="trgm" runat="server">
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                
                                            <asp:Literal ID="ltctc" runat="server" Text="CTC"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtCTC" runat="server" CssClass="inptnew inpt2new"></asp:TextBox></td>
                                             <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltBasicRetrials" runat="server" Text="Basic + Retiral Availed"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                             <asp:TextBox ID="txtBasicRetrials" runat="server" AutoPostBack="true"  onkeypress="return isNumberKey(event);" MaxLength="8"
                                            CssClass="inptnew inpt2new" OnTextChanged="txtBasicRetrials_TextChanged"></asp:TextBox>
                                        </td>
                                      
                                    </tr>
                                     
                                    <tr id="trgmctc" runat="server">
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                            
                                            <asp:Literal ID="ltcbasic" runat="server" Text="Annual Basic"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtcBasic" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                                
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltretcopm" runat="server" Text="Retiral Benefit Component"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtretrialbcomponent"  Enabled="false"  runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltfpa" runat="server" Text="FPA"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtFPA" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Taxable Amount Availed</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotalsalavailed" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Others(1)</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther1" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Reimbursement Availed</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotReimAvailed" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Others(2)</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther2" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>CLA Rental</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtClaRental" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Location FPA</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtLocFPA" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Vehicle EMI</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtVehEmi" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Others(3)</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther3" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>SAF</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtSAF" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Petrol Credited</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotpcredited" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Bus Deduction</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtBusDeduct" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Allowance</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotAllownce" runat="server" Font-Bold="true" CssClass="inptnew inpt2new" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Deduction</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotDeduct" runat="server" Font-Bold="true" Enabled="false" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltbasiccomp" runat="server" Text="Basic Component"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <%--<asp:TextBox ID="txtBasiccomponent" AutoPostBack="true" MaxLength="8" runat="server" CssClass="inptnew inpt2new" OnTextChanged="txtBasiccomponent_onchange"></asp:TextBox>
                                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtBasiccomponent"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>--%>
                                            <asp:TextBox ID="txtBasiccomponent" MaxLength="8" runat="server" CssClass="inptnew inpt2new" onpaste="return false"
                                                onkeypress="return isNumberKey(event);" AutoPostBack="True" OnTextChanged="txtBasiccomponent_TextChanged"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                        </td>
                                        <td width="17%" align="left" valign="center">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Net Amount</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtNetAmount" runat="server" Font-Bold="true" Enabled="false" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Yet To Allocate</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtYAllocate" runat="server" Enabled="false" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brdnew">
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                Salary distribution (The amount in each head should be divisible by balance month
                                                of flexi cycle)</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="left" valign="center" class="black12new greyBgnew">
                                            <asp:GridView ID="gvsalarydist" runat="server" ShowFooter="true" Width="100%" EmptyDataText="No Record Found."
                                                AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalHeadId" runat="server" Text='<%#Eval("SALARY_HEAD_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Salary Head" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalHead" runat="server" Text='<%#Eval("SALARY_HEAD") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <span style="font-size: large; float: right;">Total:</span>
                                                        </FooterTemplate>
                                                       <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Yearly Allocation" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSalAmount" MaxLength="9" Text='<%#Eval("AMOUNT") %>' runat="server" AutoPostBack="true"
                                                                CssClass="inptnew inpt2new" onfocus="clearContents(this);"  OnTextChanged="txtSalAmount_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtSalAmount"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                          
                                                        </ItemTemplate>
														 <ItemStyle HorizontalAlign="Right"  />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTotalSalaryamount" runat="server" Font-Bold="true"  Enabled="false"
                                                                CssClass="inptnew inpt2new"></asp:TextBox>
                                                        </FooterTemplate>
														<FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                FPA distribution</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="center" class="black12new greyBgnew">
                                            <asp:GridView ID="gvFPADistribution" runat="server" Width="100%" AutoGenerateColumns="false"
                                                EmptyDataText="No Record Found." ShowFooter="true">
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFpaid" runat="server" Text='<%#Eval("FPA_HEAD_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FPA Head">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFpaHead" runat="server" Text='<%#Eval("FPA_HEAD") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <span style="font-size: large; float: right;">Total:</span>
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Yearly Allocation">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFPAAmount" MaxLength="9" Text='<%#Eval("AMOUNT") %>' runat="server" CssClass="inptnew inpt2new"
                                                                AutoPostBack="true" onfocus="clearContents(this);" OnTextChanged="txtFPAAmount_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtFPAAmount"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            
                                                        </ItemTemplate>
														 <ItemStyle HorizontalAlign="Right"  />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTotalFPAamount"  Enabled="false" Font-Bold="true" runat="server"
                                                                CssClass="inptnew inpt2new"></asp:TextBox>
                                                        </FooterTemplate>
														<FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" Width="20%"/>
                                                    </asp:TemplateField>
													
													 <asp:TemplateField HeaderText="Already Claimed">
                                                        <ItemTemplate>
                                                           <div style="text-align:right;font-size:11px;"><asp:Label ID="lblAlreadyClaimed"  runat="server" Text='<%#Eval("Alredyclaimed") %>'></asp:Label></div> 
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                              <div style="text-align:right;font-size:11px;"><asp:Label style="text-align:right" ID="lblAlCliam" Font-Bold="true"   runat="server" ></asp:Label></div> 
                                                        </FooterTemplate>
                                                           <HeaderStyle HorizontalAlign="Right" Width="20%"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Amount">
                                                        <ItemTemplate>
                                                          <div style="text-align:right;font-size:11px;"><asp:Label ID="lblBalanceAmount"  runat="server" Text='<%#Eval("Balance") %>'></asp:Label></div> 
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                           <div style="text-align:right;font-size:11px;"> <asp:Label ID="lblBalance" CssClass="rightalign" Font-Bold="true"  runat="server" ></asp:Label></div> 
                                                        </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="20%"/>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="center" colspan="4">
                                            &nbsp;</td>
                                    </tr>
                                       </table>
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brdnew">
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td style="width: 33%" align="left" valign="top" class="greyBgnew" colspan="2">
                                            <strong>Comments from individual</strong></td>
                                        <td style="width: 33%" align="left" valign="top" class="greyBgnew" colspan="2">
                                            <strong>Comments by FPA Admin</strong></td>
                                        <td style="width: 33%" align="left" valign="top" class="greyBgnew" colspan="2">
                                            <strong>Rejection Comments</strong></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <asp:TextBox ID="txtIndiComments" Height="60px" runat="server" CssClass="inptnew1"
                                                TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtIndiComments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtIndiComments"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left" valign="top" colspan="2">
                                            <asp:TextBox ID="txtFPAcomments" Height="60px" runat="server" CssClass="inptnew1"
                                                TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtFPAcomments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtFPAcomments"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
										 <td align="left" valign="top" colspan="2">
                                            <asp:TextBox ID="txtReject" Height="60px" Enabled="false" runat="server" CssClass="inptnew1"
                                                TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtReject"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtReject"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
									 <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                         <tr>
                                             <td  align="center" valign="top" style="margin-top:10px; margin-bottom:10px;">
                                             
                                                 <asp:LinkButton ID="btnSubmit" runat="server" CssClass="butnew" OnClick="btnSubmit_Click">
                                                  Submit To FPA Admin</asp:LinkButton>
                                             
                                                 <asp:LinkButton ID="btnSaveDraft" runat="server" CssClass="butnew" OnClick="btnSaveDraft_Click">
                                                  Save as Draft</asp:LinkButton>
                                            
                                            
                                                 <asp:LinkButton ID="LinkButton1" runat="server" CssClass="butnew" OnClientClick="javascript:CloseWindowcan();">
                                                  Close</asp:LinkButton>
                                             </td>
                                             
                                         </tr>
                            </table>
                                </table>
                            </div>
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
