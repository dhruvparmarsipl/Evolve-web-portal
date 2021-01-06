<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Graph.aspx.cs" Inherits="Graph" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
        <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />
   
    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <link href="Date/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Date/jquery-ui.min.js" type="text/javascript"></script>
     <script type="text/javascript">
      function myJS(myVar) {
              window.alert(myVar);
          } 
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
        
         $(document).ready(function(){
           $("[id$=txtFromdate]").datepicker({  changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
         $("[id$=txtTodate]").datepicker({  changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
        });
        
        
        function callcalender(){
        $("[id$=txtFromdate]").datepicker({  changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
         $("[id$=txtTodate]").datepicker({  changeMonth: true, changeYear: true,showHour:false,showMinute:false, dateFormat: 'dd M yy' });
       }
              </script>
</head>
<body>
    
    
    
      <form id="form2" runat="server">
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
      }     Sys.Application.add_load(callcalender);
      
        </script>

        <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HFSAPID" runat="server" />
                <asp:HiddenField ID="HFID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
               <div id="wrapper">
                    <div class="row greyBg">
                        <div class="pageWidth">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span></div>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                                FPA Admin  <span id="spnFormCreater" runat="server"></span>
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
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong>Ecode</strong></td>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong>From Date</strong></td>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong>To Date</strong></td>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong></strong>
                                        </td>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtEcode" runat="server"  CssClass="inptnew inpt2new"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rqvtxtEcode" ValidationGroup="aa" ControlToValidate="txtEcode" Display="Dynamic" ErrorMessage="Please enter ecode" Text="*">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" valign="top">
                                             <asp:TextBox ID="txtFromdate" runat="server"  CssClass="inpbox2"></asp:TextBox>
                                              <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="aa" ControlToValidate="txtFromdate" Display="Dynamic" ErrorMessage="Please enter fromdate" Text="*">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" valign="top">
                                             <asp:TextBox ID="txtTodate" runat="server" CssClass="inpbox2"></asp:TextBox>
                                              <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="aa" ControlToValidate="txtTodate" Display="Dynamic" ErrorMessage="Please enter todate" Text="*">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" valign="top">
                                         <asp:LinkButton ID="btnSearch" runat="server" ValidationGroup="aa" CssClass="button" OnClick="btnSearch_Click" >
                                          Search</asp:LinkButton>
                                        </td>
                                         <td align="left" valign="top">
                                         <asp:LinkButton ID="btnReport" runat="server" Visible="false"  CssClass="button" OnClick="btnReport_Click"  >
                                          Generate Report</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:ValidationSummary ID="valid" runat="server" DisplayMode="BulletList"  ValidationGroup="aa" ShowMessageBox="true" ShowSummary="false" />
                           <div class="row mBtm20">
                               <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                   <tr>
                                       <td align="left" valign="top" class="head">
                                           <h2 class="black16new wnornew">
                                               Date Wise Claim Amount
                                           </h2>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td width="100%" align="left" valign="top">
                                           <asp:GridView ID="gvReport" runat="server" HeaderStyle-Height="30px" Width="100%" HeaderStyle-BackColor="#cecfce" EmptyDataText="No Record Found."
                                               AutoGenerateColumns="false">
                                               <Columns>
                                                   <asp:BoundField DataField="Creation Date" HeaderText="Creation Date" HeaderStyle-HorizontalAlign="Left" />
                                                   <asp:BoundField DataField="Claim Amount" HeaderText="Claim Amount" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="DETAILS" HeaderText="Detail" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Payment Type" HeaderText="Payment Type" HeaderStyle-HorizontalAlign="Left" />
                                                   <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblStatus" runat="server" Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                           </asp:GridView>
                                       </td>
                                   </tr>
                               </table>
                            </div>
                            
                            
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
          
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
