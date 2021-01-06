<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FPAReIntiation.aspx.cs" Inherits="FPAReIntiation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA ReInitiation</title>
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
        
        
    function fpaCompute()
     {     
           
             var DesId=document.getElementById('<%=ddlDesignation.ClientID %>').value;
             if(parseInt(DesId)<21)
               {
                var txtFPA="";
               var txtcbasic=document.getElementById('<%=txtcBasic.ClientID %>').value;
               var txtctc=document.getElementById('<%=txtCTC.ClientID %>').value;
               }
               else
               { var txtcbasic="";
                var txtctc="";
                var txtFPA=document.getElementById('<%=txtFPA.ClientID %>').value;
                }
             
           
             var txtOther1=document.getElementById('<%=txtOther1.ClientID %>').value;
             var txtOther2=document.getElementById('<%=txtOther2.ClientID %>').value;  
             var txtLocFPA=document.getElementById('<%=txtLocFPA.ClientID %>').value;
             var txtOther3=document.getElementById('<%=txtOther3.ClientID %>').value;
             var txtTotpcredited=document.getElementById('<%=txtTotpcredited.ClientID %>').value;
             var netamount= document.getElementById('txtNetAmount');
             var txtTotAllownce= document.getElementById('txtTotAllownce');
             var txtTotDeduct= document.getElementById('txtTotDeduct').value;
            
             
               if(txtctc=="")
               {
               txtctc=parseInt(0);
               }
               else
               {
               txtctc=txtctc;  
               }
              if(txtcbasic=="")
               {
               txtcbasic=parseInt(0);
               }
               else
               {
               txtcbasic=txtcbasic;  
               }
               if(txtTotDeduct=="")
               {
               txtTotDeduct=parseInt(0);
               }
               else
               {
               txtTotDeduct=txtTotDeduct;  
               }
               if(txtFPA=="")
               {
               txtFPA=parseInt(0);
               }
               else
               {
               txtFPA=txtFPA;  
               }
               if(txtOther1=="")
               {
               txtOther1=parseInt(0);
               }
               else
               {
               txtOther1=txtOther1;  
               }
               if(txtOther2=="")
               {
               txtOther2=parseInt(0);
               }
               else
               {
               txtOther2=txtOther2;  
               }
                if(txtOther3=="")
               {
               txtOther3=parseInt(0);
               }
               else
               {
               txtOther3=txtOther3;  
               }
                if(txtLocFPA=="")
               {
               txtLocFPA=parseInt(0);
               }
               else
               {
               txtLocFPA=txtLocFPA;  
               }
                if(txtTotpcredited=="")
               {
               txtTotpcredited=parseInt(0);
               }
               else
               {
               txtTotpcredited=txtTotpcredited;  
               }
               
               if(parseInt(DesId)>21)
               {
                   if(parseInt(txtFPA)>=35000000)
                   {
                      alert("FPA should be less than Rs.35000000");
                      netamount.value=(parseInt(0)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(0)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                     // document.getElementById("<%=txtFPA.ClientID %>").focus();
                      txtFPA.focus();
                      return false;
                   }     
                   if(parseInt(txtOther1)>=35000000)
                   {
                      alert("OTHER1 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtFPA)+parseInt(0)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtFPA)+parseInt(0)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther1.focus();
                      return false;
                   }
                   if(parseInt(txtOther2)>=35000000)
                   {
                      alert("OTHER2 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtFPA)+parseInt(txtOther1)+parseInt(0)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtFPA)+parseInt(txtOther1)+parseInt(0)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther2.focus();
                      return false;
                   }
                   if(parseInt(txtOther3)>=35000000)
                   {
                      alert("OTHER3 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(0)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(0)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther3.focus();
                      return false;
                   }
                   if(parseInt(txtLocFPA)>=35000000)
                   {
                      alert("Locational FPA should be less than Rs.35000000");
                      netamount.value=(parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(0)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(0)+parseInt(txtTotpcredited);
                      txtLocFPA.focus();
                      return false;
                   }
                   if(parseInt(txtTotpcredited)>=35000000)
                   {
                      alert("Total Petrol Amount should be less than Rs.35000000");
                      netamount.value=(parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(0))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(0);
                      txtTotpcredited.focus();
                      return false;
                   }
                   netamount.value=(parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                   txtTotAllownce.value=parseInt(txtFPA)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                 }
             else
                 {
                  
                  if(parseInt(txtctc)>=50000000)
                   {
                      alert("CTC should be less than Rs.50000000");
                      netamount.value=(parseInt(0)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(0)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtFPA.focus();
                      return false;
                   }     
                   if(parseInt(txtOther1)>=35000000)
                   {
                      alert("OTHER1 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtctc)+parseInt(0)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtctc)+parseInt(0)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther1.focus();
                      return false;
                   }
                   if(parseInt(txtOther2)>=35000000)
                   {
                      alert("OTHER2 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtctc)+parseInt(txtOther1)+parseInt(0)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtctc)+parseInt(txtOther1)+parseInt(0)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther2.focus();
                      return false;
                   }
                   if(parseInt(txtOther3)>=35000000)
                   {
                      alert("OTHER3 should be less than Rs.35000000");
                      netamount.value=(parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(0)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(0)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                      txtOther3.focus();
                      return false;
                   }
                   if(parseInt(txtLocFPA)>=35000000)
                   {
                      alert("Locational FPA should be less than Rs.35000000");
                      netamount.value=(parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(0)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(0)+parseInt(txtTotpcredited);
                      txtLocFPA.focus();
                      return false;
                   }
                   if(parseInt(txtTotpcredited)>=35000000)
                   {
                      alert("Total Petrol Amount should be less than Rs.35000000");
                      netamount.value=(parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(0))-parseInt(txtTotDeduct);
                      txtTotAllownce.value=parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(0);
                      txtTotpcredited.focus();
                      return false;
                   }
                   netamount.value=(parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited))-parseInt(txtTotDeduct);
                   txtTotAllownce.value=parseInt(txtctc)+parseInt(txtOther1)+parseInt(txtOther2)+parseInt(txtOther3)+parseInt(txtLocFPA)+parseInt(txtTotpcredited);
                
                 }
  }
   function checkCTC(element)
          {
              var txtFPA=element.value;
              if(parseInt(txtFPA)>=50000000)
              {
              element.focus();
             element.style.border="2px solid Red";
              return false;
              }
              else
              {
              element.style.border="2px solid #E1E1E1";
              return true;
              }
          }
          function check(element)
          {
              var txtFPA=element.value;
              if(parseInt(txtFPA)>=35000000)
              {
              element.focus();
             element.style.border="2px solid Red";
              return false;
              }
              else
              {
              element.style.border="2px solid #E1E1E1";
              return true;
              }
          }
        function deductionCompute()
          {         
               var DesId=document.getElementById('<%=ddlDesignation.ClientID %>').value; 
               if(parseInt(DesId)<21)
               {
                var txtBasiccomponent=document.getElementById('<%=txtBasiccomponent.ClientID %>').value;
                var txtretrialbcomponent="";
               }
               else
               {
                var txtBasiccomponent="";
                var txtretrialbcomponent="";
               }
            
                 var txtTotalsalavailed=document.getElementById('<%=txtTotalsalavailed.ClientID %>').value;
                 var txtTotReimAvailed=document.getElementById('<%=txtTotReimAvailed.ClientID %>').value;
                 var txtClaRental=document.getElementById('<%=txtClaRental.ClientID %>').value;  
                 var txtVehEmi=document.getElementById('<%=txtVehEmi.ClientID %>').value;
                 var txtSAF=document.getElementById('<%=txtSAF.ClientID %>').value;
                 var txtBusDeduct=document.getElementById('<%=txtBusDeduct.ClientID %>').value;
            
               if(txtTotalsalavailed=="")
               {
               txtTotalsalavailed=parseInt(0);
               }
               else
               {
               txtTotalsalavailed=txtTotalsalavailed;  
               }
               if(txtretrialbcomponent=="")
               {
               txtretrialbcomponent=parseInt(0);
               }
               else
               {
               txtretrialbcomponent=txtretrialbcomponent;  
               }
               
               if(txtBasiccomponent=="")
               {
               txtBasiccomponent=parseInt(0);
               }
               else
               {
               txtBasiccomponent=txtBasiccomponent;  
               }
               
               if(txtTotReimAvailed=="")
               {
               txtTotReimAvailed=parseInt(0);
               }
               else
               {
               txtTotReimAvailed=txtTotReimAvailed;  
               }
               if(txtClaRental=="")
               {
               txtClaRental=parseInt(0);
               }
               else
               {
               txtClaRental=txtClaRental;  
               }
               if(txtVehEmi=="")
               {
               txtVehEmi=parseInt(0);
               }
               else
               {
               txtVehEmi=txtVehEmi;  
               }
               if(txtSAF=="")
               {
               txtSAF=parseInt(0);
               }
               else
               {
               txtSAF=txtSAF;  
               }
               if(txtBusDeduct=="")
               {
               txtBusDeduct=parseInt(0);
               }
               else
               {
               txtBusDeduct=txtBusDeduct;  
               }
              
               var txtTotAllownce=document.getElementById('<%=txtTotAllownce.ClientID %>').value;
               if(txtTotAllownce=="")
               {
                 txtTotAllownce=parseInt(0);
               }
               else
               {
               txtTotAllownce=txtTotAllownce;
               }
               var totnetamount= document.getElementById('txtNetAmount');
               var txtTotDeduct= document.getElementById('txtTotDeduct');
               
                if(parseInt(DesId)>21)
               {
                   if(parseInt(txtTotalsalavailed)>=35000000)
                   {
                      alert("Total Salary Availed should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(0)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                    
                      txtTotalsalavailed.focus();
                      return false;
                   }   
                   if(parseInt(txtClaRental)>=35000000)
                   {
                      alert("CLA RENTAL should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtTotReimAvailed)+parseInt(0)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                     
                      txtClaRental.focus();
                      return false;
                   } 
                   if(parseInt(txtTotReimAvailed)>=35000000)
                   {
                      alert("Total Salary Component should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(0)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      
                      txtTotReimAvailed.focus();
                      return false;
                   } 
                   if(parseInt(txtVehEmi)>=35000000)
                   {
                      alert("Vehicle EMI should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(0)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                       
                      txtVehEmi.focus();
                      return false;
                   }
                   if(parseInt(txtBusDeduct)>=35000000)
                   {
                      alert("Bus Deduction should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(0))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      
                      txtBusDeduct.focus();
                      return false;
                   } 
                   if(parseInt(txtSAF)>=35000000)
                   {
                      alert("saf Deduction should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(0)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtSAF.focus();
                      return false;
                   }    
                    txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                    totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                }
                else
                {
                  if(parseInt(txtBasiccomponent)>=35000000)
                   {
                      alert("Total Basic Component should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(0)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtTotalsalavailed.focus();
                      return false;
                   }
                  if(parseInt(txtretrialbcomponent)>=35000000)
                   {
                      alert("Total Basic Retriaval Component should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(0)+parseInt(0)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtTotalsalavailed.focus();
                      return false;
                   }
                  if(parseInt(txtTotalsalavailed)>=35000000)
                   {
                      alert("Total Salary Availed should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(0)+parseInt(txtBasiccomponent)+parseInt(txtretrialbcomponent)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtTotalsalavailed.focus();
                      return false;
                   }   
                   if(parseInt(txtClaRental)>=35000000)
                   {
                      alert("CLA RENTAL should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(txtTotReimAvailed)+parseInt(0)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtClaRental.focus();
                      return false;
                   } 
                   if(parseInt(txtTotReimAvailed)>=35000000)
                   {
                      alert("Total Salary Component should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(0)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtTotReimAvailed.focus();
                      return false;
                   } 
                   if(parseInt(txtVehEmi)>=35000000)
                   {
                      alert("Vehicle EMI should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(0)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtVehEmi.focus();
                      return false;
                   }
                   if(parseInt(txtBusDeduct)>=35000000)
                   {
                      alert("Bus Deduction should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(0))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtBusDeduct.focus();
                      return false;
                   } 
                   if(parseInt(txtSAF)>=35000000)
                   {
                      alert("saf Deduction should be less than Rs.35000000");
                      txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(0)+parseInt(txtBusDeduct))
                      totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                      txtSAF.focus();
                      return false;
                   }    
                    txtTotDeduct.value= (parseInt(txtTotalsalavailed)+parseInt(txtretrialbcomponent)+parseInt(txtBasiccomponent)+parseInt(txtTotReimAvailed)+parseInt(txtClaRental)+parseInt(txtVehEmi)+parseInt(txtSAF)+parseInt(txtBusDeduct))
                    totnetamount.value=parseInt(txtTotAllownce)- parseInt(txtTotDeduct.value);
                
                }  
       }
  
  
 
function clearContents(txt, evt) {

        var defaultText = "0";
        if(txt.value.length == 0 && evt.type == "blur")
        {
            txt.style.color = "gray";
            txt.value = defaultText;
        }
        if(txt.value == defaultText && evt.type == "focus")
        {
            txt.style.color = "black";
            txt.value="";
        }
};
    </script>

</head>
<body>
    <form id="form1" runat="server" >
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
        <asp:HiddenField ID="HFEMAILS" runat="server" />
         <asp:HiddenField ID="HFBU" runat="server" />
                <div id="wrappernew">
                    <div class="row greyBgnew">
                        <div class="pageWidthnew">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span></div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brdnew">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16new wnornew">
                                                FPA ReInitiation <span id="spnFormCreater" runat="server"></span>
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
                                    <%--<tr>
                                      <td width="25%" align="left" valign="top">
                                            <strong>Initiator Name</strong></td>
                                       <td width="25%" align="left" valign="top">
                                            <span id="spnIntName" runat="server"></span>
                                        </td>
                                       <td width="25%" align="left" valign="top">
                                            <strong>Employee code</strong></td>
                                        <td width="13%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Designation</strong></td>
                                        <td width="13%" align="left" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Location</strong></td>
                                        <td width="12%" align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                            </span>
                                        </td>
                                         <span id="spnSrNo" visible="false" runat="server"></span>
                                    </tr>--%>
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
                            </div>
                            <div class="row mBtm20">
                                   <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brd">
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
                                            <asp:TextBox ID="txtCTC" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="checkCTC(this);clearContents(this, event);" OnTextChanged="txtCTC_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtCTC"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltbasiccomp" runat="server" Text="Basic +Retrial Availed"></asp:Literal></strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtBasiccomponent" runat="server"    onfocus ="clearContents(this, event);"
                                                CssClass="inptnew inpt2new" onblur="check(this);clearContents(this, event);" onkeyup="javascript:deductionCompute()"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtBasiccomponent"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr id="trgmctc" runat="server">
                                     <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltcbasic" runat="server" Text="Annual Basic"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtcBasic" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtcBasic_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtcBasic"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                       
                                       <td width="33%" align="left" valign="center">
                                            <strong>
                                               <%-- <asp:Literal ID="ltretcopm" runat="server" Text="Retrial Benefit Component"></asp:Literal></strong>--%></td>
                                       <td width="17%" align="left" valign="center">
                                           <%-- <asp:TextBox ID="txtretrialbcomponent" runat="server" onfocus="clearContents(this);"
                                                CssClass="inptnew inpt2new" onblur="javascript:check(this)" onkeyup="javascript:deductionCompute()"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtretrialbcomponent"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>
                                                <asp:Literal ID="ltfpa" runat="server" Text="FPA"></asp:Literal></strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtFPA" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtFPA_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtFPA"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Taxable Amount Availed</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotalsalavailed" runat="server" onfocus="clearContents(this,event);"
                                                CssClass="inptnew inpt2new" onblur="check(this);clearContents(this, event);" onkeyup="javascript:deductionCompute()"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTotalsalavailed"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Others(1)</strong></td>
                                      <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther1" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtOther1_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txtOther1"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                      <td width="33%" align="left" valign="center">
                                            <strong>Total Reimbursement Availed</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotReimAvailed" runat="server" CssClass="inptnew inpt2new" Enabled="false" ReadOnly="true"
                                                onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtTotReimAvailed"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td width="33%" align="left" valign="center">
                                            <strong>Others(2)</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther2" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtOther2_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOther2"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                       <td width="33%" align="left" valign="center">
                                            <strong>CLA Rental</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtClaRental" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:deductionCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtClaRental"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td width="33%" align="left" valign="center">
                                            <strong>Location FPA</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtLocFPA" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtLocFPA_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtLocFPA"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                       <td width="33%" align="left" valign="center">
                                            <strong>Vehicle EMI</strong></td>
                                      <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtVehEmi" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:deductionCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtVehEmi"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Others(3)</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtOther3" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);" OnTextChanged="txtOther3_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtOther3"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                       <td width="33%" align="left" valign="center">
                                            <strong>SAF</strong></td>
                                        <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtSAF" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:deductionCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtSAF"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                            <strong>Total Petrol Credited</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtTotpcredited" runat="server" CssClass="inptnew inpt2new" Enabled="false" ReadOnly="true" 
                                             onblur="check(this);clearContents(this, event);" OnTextChanged="txtTotpcredited_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtTotpcredited"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                       <td width="33%" align="left" valign="center">
                                            <strong>Bus Deduction</strong></td>
                                       <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtBusDeduct" runat="server" CssClass="inptnew inpt2new"    onfocus ="clearContents(this, event);"
                                                onkeyup="javascript:deductionCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtBusDeduct"
                                                FilterType="Numbers" FilterMode="ValidChars">
                                            </cc1:FilteredTextBoxExtender>
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
                                    <tr>
                                        <td width="33%" align="left" valign="center">
                                        </td>
                                        <td width="17%" align="left" valign="center">
                                        </td>
                                       <td width="33%" align="left" valign="center">
                                            <strong>Net Amount</strong></td>
                                      <td width="17%" align="left" valign="center">
                                            <asp:TextBox ID="txtNetAmount" runat="server" Font-Bold="true" Enabled="false" CssClass="inptnew inpt2new"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="greyBgnew" colspan="4">
                                            <strong>Comments</strong></td>
                                    </tr>
                                    <td align="left" valign="top" colspan="4">
                                       <asp:TextBox ID="txtComments" runat="server" rows="3" CssClass="inptnew"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtComments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                                <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"   WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                            </td>
                                    
                                    
                                </table>
                                
                               <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="35%" align="left" valign="top">
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                            <asp:LinkButton ID="btnIntiation" runat="server" CssClass="button" OnClick="btnIntiation_Click">
                        ReInitiate FPA</asp:LinkButton>
                                        </td>
                                        <td width="10%" align="left" valign="top">
                                             <asp:LinkButton ID="btnClearAll" runat="server" CssClass="button" OnClick="btnClearAll_Click">
                         Clear All</asp:LinkButton>
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
                            <div class="row" align="center">
                                
                               
                               
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

