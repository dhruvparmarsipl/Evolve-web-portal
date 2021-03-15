<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FPAIntiation.aspx.cs" Inherits="FPAIntiation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA Initiation</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="LightBox/popup.js" type="text/javascript"></script>

    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
   
<style type="text/css">
    .search_textbox
    {
    	float:left;
    }
</style>
<script type="text/javascript">

    function UserDetail(Sapid, DesId, Ename, Email, Loc, Designation, Ecode, carscheme) {
        var HFUSERSAPID = document.getElementById('HFUSERSAPID');
        var HFDESID = document.getElementById('HFDESID');
        HFUSERSAPID.value = Sapid;
        HFDESID.value = DesId;
        document.getElementById('HFEMAILS').value = Email;
        document.getElementById('HFENAME').value = Ename;
        $("#spnUserName1").text(Ename);
        $("#spnEcode").text(Ecode);
        $("#spnLocUser").text(Loc);
        $("#spnDesig").text(Designation);
        var ltfpa = document.getElementById('<%=ltfpa.ClientID %>');
          var txtFPA = document.getElementById('<%=txtFPA.ClientID %>');
        var ltctc = document.getElementById('<%=ltctc.ClientID %>');
        var txtCTC = document.getElementById('<%=txtCTC.ClientID %>');
        var ltcbasic = document.getElementById('<%=ltcbasic.ClientID %>');
        var txtcBasic = document.getElementById('<%=txtcBasic.ClientID %>');
        if (parseInt(DesId) < 21) {
            ltfpa.style.display = 'none';
            txtFPA.style.display = 'none';
            ltctc.style.display = 'block';
            txtCTC.style.display = 'block';
            ltcbasic.style.display = 'block';
            txtcBasic.style.display = 'block';
        }
        else {
            txtFPA.style.display = 'block';
            ltfpa.style.display = 'block';
            ltctc.style.display = 'none';
            txtCTC.style.display = 'none';
            ltcbasic.style.display = 'none';
            txtcBasic.style.display = 'none';
            
        }
        SetVisible();
        var ddlCarScheme = document.getElementById('ddlCarScheme');
        ddlCarScheme.value = carscheme;

        Popup.show('FpaTab')
    }
    function SetVisible() {
        $("#FpaTab").find("input:text").val('0');
        var comment = document.getElementById('<%=txtComments.ClientID %>')
        comment.value = '';
        var txtFPA = document.getElementById('<%=txtFPA.ClientID %>');
        txtFPA.disabled = false;
        var txtcBasic = document.getElementById('<%=txtcBasic.ClientID %>');
        txtcBasic.disabled = false;
        var txtCTC = document.getElementById('<%=txtCTC.ClientID %>');
        txtCTC.disabled = false;
        var txtLocFPA = document.getElementById('<%=txtLocFPA.ClientID %>');
        txtLocFPA.disabled = false;
        var txtOther1 = document.getElementById('<%=txtOther1.ClientID %>');
        txtOther1.disabled = false;
        var txtOther2 = document.getElementById('<%=txtOther2.ClientID %>');
        txtOther2.disabled = false;

        var txtOther3 = document.getElementById('<%=txtOther3.ClientID %>');
        txtOther3.disabled = false;
        var txtTotAllownce = document.getElementById('<%=txtTotAllownce.ClientID %>');
        txtTotAllownce.disabled = true;
        var txtTotalsalavailed = document.getElementById('<%=txtTotalsalavailed.ClientID %>');
        txtTotalsalavailed.disabled = false;
        var txtTotDeduct = document.getElementById('<%=txtTotDeduct.ClientID %>');
        txtTotDeduct.disabled = true;
        var txtTotpcredited = document.getElementById('<%=txtTotpcredited.ClientID %>');
        txtTotpcredited.disabled = false;

        var txtTotReimAvailed = document.getElementById('<%=txtTotReimAvailed.ClientID %>');
        txtTotReimAvailed.disabled = false;
        var txtVehEmi = document.getElementById('<%=txtVehEmi.ClientID %>');
        txtVehEmi.disabled = false;
        var txtSAF = document.getElementById('<%=txtSAF.ClientID %>');
        txtSAF.disabled = false;
        var txtNetAmount = document.getElementById('<%=txtNetAmount.ClientID %>');
        txtNetAmount.disabled = true;
        var txtClaRental = document.getElementById('<%=txtClaRental.ClientID %>');
        txtClaRental.disabled = false;
        var txtBusDeduct = document.getElementById('<%=txtBusDeduct.ClientID %>');
        txtBusDeduct.disabled = false;

    }
    function SubmitFpa() {

        var Carscheme = $('#ddlCarScheme').val();
        var Ename = $('#HFENAME').val();
        var Sapid = $('#HFUSERSAPID').val();
        var Fpa = $('#txtFPA').val();
        var CTC = $('#txtCTC').val();
        var cBasic = $('#txtcBasic').val();
        var other1 = $('#txtOther1').val();
        var other2 = $('#txtOther2').val();
        var other3 = $('#txtOther3').val();
        var totpetrol = $('#txtTotpcredited').val();
        var Locfpa = $('#txtLocFPA').val();
        var busdeduct = $('#txtBusDeduct').val();
        var saf = $('#txtSAF').val();
        var vehemi = $('#txtVehEmi').val();
        var totSalAvail = $('#txtTotalsalavailed').val();
        var totreimavail = $('#txtTotReimAvailed').val();
        var clarental = $('#txtClaRental').val();
        var EamilId = $('#HFEMAILS').val();
        var DesId = $('#HFDESID').val();
        var NetAmount = $('#txtNetAmount').val();
        var Comment = $('#txtComments').val();
        var totdeduct = $('#txtTotDeduct').val();
        var totAllow = $('#txtTotAllownce').val();
        if (Sapid != '' && DesId != '' && NetAmount > 0) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FPAIntiation.aspx/SubmitData",
                data: "{Fpa:'" + Fpa + "',CTC:'" + CTC + "',cBasic:'" + cBasic + "',other1:'" + other1 + "',other2:'" + other2 + "',other3:'" + other3 + "',totpetrol:'" + totpetrol + "',Locfpa:'" + Locfpa + "',busdeduct:'" + busdeduct + "',saf:'" + saf + "',vehemi:'" + vehemi + "',totSalAvail:'" + totSalAvail + "',totreimavail:'" + totreimavail + "',clarental:'" + clarental + "',Sapid:'" + Sapid + "',DesId:'" + DesId + "',EamilId:'" + EamilId + "',NetAmount:'" + NetAmount + "',Comment:'" + Comment + "',totdeduct:'" + totdeduct + "',totAllow:'" + totAllow + "',Ename:'" + Ename + "',Carscheme:'" + Carscheme + "'}",
                dataType: "json",
                success: function (data) {

                    if (data.d == 1) {
                        alert("" + Ename + " FPA initiated successfully.")
                        Popup.hide('FpaTab');
                        location.reload();
                    }
                    else if (data.d == 5) {
                        alert("Data already submitted.");

                    }
                    else {
                        alert("Please try after some time.");
                    }

                },
                error: function (result) {
                    alert("Please try after some time.");
                }
            });
        }
        else {
            if (NetAmount <= 0) {
                alert("Please note Net Amount cannot be zero or negative.");
            }
        }
    }

</script>
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
        function fpaCompute() {
            var DesId = document.getElementById('<%=HFDESID.ClientID %>').value;
            if (parseInt(DesId) < 21) {
                var txtFPA = "";
                var txtcbasic = document.getElementById('<%=txtcBasic.ClientID %>').value;
                var txtctc = document.getElementById('<%=txtCTC.ClientID %>').value;
            }
            else {
                var txtcbasic = "";
                var txtctc = "";
                var txtFPA = document.getElementById('<%=txtFPA.ClientID %>').value;
            }


            var txtOther1 = document.getElementById('<%=txtOther1.ClientID %>').value;
            var txtOther2 = document.getElementById('<%=txtOther2.ClientID %>').value;
            var txtLocFPA = document.getElementById('<%=txtLocFPA.ClientID %>').value;
            var txtOther3 = document.getElementById('<%=txtOther3.ClientID %>').value;
            var txtTotpcredited = document.getElementById('<%=txtTotpcredited.ClientID %>').value;
            var netamount = document.getElementById('txtNetAmount');
            var txtTotAllownce = document.getElementById('txtTotAllownce');
            var txtTotDeduct = document.getElementById('txtTotDeduct').value;


            if (txtctc == "") {
                txtctc = parseInt(0);
            }
            else {
                txtctc = txtctc;
            }
            if (txtcbasic == "") {
                txtcbasic = parseInt(0);
            }
            else {
                txtcbasic = txtcbasic;
            }
            if (txtTotDeduct == "") {
                txtTotDeduct = parseInt(0);
            }
            else {
                txtTotDeduct = txtTotDeduct;
            }
            if (txtFPA == "") {
                txtFPA = parseInt(0);
            }
            else {
                txtFPA = txtFPA;
            }
            if (txtOther1 == "") {
                txtOther1 = parseInt(0);
            }
            else {
                txtOther1 = txtOther1;
            }
            if (txtOther2 == "") {
                txtOther2 = parseInt(0);
            }
            else {
                txtOther2 = txtOther2;
            }
            if (txtOther3 == "") {
                txtOther3 = parseInt(0);
            }
            else {
                txtOther3 = txtOther3;
            }
            if (txtLocFPA == "") {
                txtLocFPA = parseInt(0);
            }
            else {
                txtLocFPA = txtLocFPA;
            }
            if (txtTotpcredited == "") {
                txtTotpcredited = parseInt(0);
            }
            else {
                txtTotpcredited = txtTotpcredited;
            }

            if (parseInt(DesId) > 21) {
                if (parseInt(txtFPA) >= 35000000) {
                    alert("FPA should be less than Rs.35000000");
                    netamount.value = (parseInt(0) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(0) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    document.getElementById("<%=txtFPA.ClientID %>").focus();
                    //txtFPA.focus();
                    return false;
                }
                if (parseInt(txtOther1) >= 35000000) {
                    alert("OTHER1 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtFPA) + parseInt(0) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtFPA) + parseInt(0) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther1.focus();
                    return false;
                }
                if (parseInt(txtOther2) >= 35000000) {
                    alert("OTHER2 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtFPA) + parseInt(txtOther1) + parseInt(0) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtFPA) + parseInt(txtOther1) + parseInt(0) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther2.focus();
                    return false;
                }
                if (parseInt(txtOther3) >= 35000000) {
                    alert("OTHER3 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(0) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(0) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther3.focus();
                    return false;
                }
                if (parseInt(txtLocFPA) >= 35000000) {
                    alert("Locational FPA should be less than Rs.35000000");
                    netamount.value = (parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(0) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(0) + parseInt(txtTotpcredited);
                    txtLocFPA.focus();
                    return false;
                }
                if (parseInt(txtTotpcredited) >= 35000000) {
                    alert("Total Petrol Amount should be less than Rs.35000000");
                    netamount.value = (parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(0)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(0);
                    txtTotpcredited.focus();
                    return false;
                }
                netamount.value = (parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                txtTotAllownce.value = parseInt(txtFPA) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
            }
            else {

                if (parseInt(txtctc) >= 50000000) {
                    alert("CTC should be less than Rs.50000000");
                    netamount.value = (parseInt(0) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(0) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtFPA.focus();
                    return false;
                }
                if (parseInt(txtOther1) >= 35000000) {
                    alert("OTHER1 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtctc) + parseInt(0) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtctc) + parseInt(0) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther1.focus();
                    return false;
                }
                if (parseInt(txtOther2) >= 35000000) {
                    alert("OTHER2 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtctc) + parseInt(txtOther1) + parseInt(0) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtctc) + parseInt(txtOther1) + parseInt(0) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther2.focus();
                    return false;
                }
                if (parseInt(txtOther3) >= 35000000) {
                    alert("OTHER3 should be less than Rs.35000000");
                    netamount.value = (parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(0) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(0) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);
                    txtOther3.focus();
                    return false;
                }
                if (parseInt(txtLocFPA) >= 35000000) {
                    alert("Locational FPA should be less than Rs.35000000");
                    netamount.value = (parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(0) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(0) + parseInt(txtTotpcredited);
                    txtLocFPA.focus();
                    return false;
                }
                if (parseInt(txtTotpcredited) >= 35000000) {
                    alert("Total Petrol Amount should be less than Rs.35000000");
                    netamount.value = (parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(0)) - parseInt(txtTotDeduct);
                    txtTotAllownce.value = parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(0);
                    txtTotpcredited.focus();
                    return false;
                }
                netamount.value = (parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited)) - parseInt(txtTotDeduct);
                txtTotAllownce.value = parseInt(txtctc) + parseInt(txtOther1) + parseInt(txtOther2) + parseInt(txtOther3) + parseInt(txtLocFPA) + parseInt(txtTotpcredited);

            }
        }
        function checkCTC(element) {
            var txtFPA = element.value;
            if (parseInt(txtFPA) >= 50000000) {
                element.focus();
                element.style.border = "2px solid Red";
                return false;
            }
            else {
                element.style.border = "2px solid #E1E1E1";
                return true;
            }
        }

        function check(element) {
            var txtFPA = element.value;
            if (parseInt(txtFPA) >= 35000000) {
                element.focus();
                element.style.border = "2px solid Red";
                return false;
            }
            else {
                element.style.border = "2px solid #E1E1E1";
                return true;
            }
        }
        function deductionCompute() {
            var DesId = document.getElementById('<%=HFDESID.ClientID %>').value;


            var txtTotalsalavailed = document.getElementById('<%=txtTotalsalavailed.ClientID %>').value;
            var txtTotReimAvailed = document.getElementById('<%=txtTotReimAvailed.ClientID %>').value;
            var txtClaRental = document.getElementById('<%=txtClaRental.ClientID %>').value;
            var txtVehEmi = document.getElementById('<%=txtVehEmi.ClientID %>').value;
            var txtSAF = document.getElementById('<%=txtSAF.ClientID %>').value;
            var txtBusDeduct = document.getElementById('<%=txtBusDeduct.ClientID %>').value;

            if (txtTotalsalavailed == "") {
                txtTotalsalavailed = parseInt(0);
            }
            else {
                txtTotalsalavailed = txtTotalsalavailed;
            }

            if (txtTotReimAvailed == "") {
                txtTotReimAvailed = parseInt(0);
            }
            else {
                txtTotReimAvailed = txtTotReimAvailed;
            }
            if (txtClaRental == "") {
                txtClaRental = parseInt(0);
            }
            else {
                txtClaRental = txtClaRental;
            }
            if (txtVehEmi == "") {
                txtVehEmi = parseInt(0);
            }
            else {
                txtVehEmi = txtVehEmi;
            }
            if (txtSAF == "") {
                txtSAF = parseInt(0);
            }
            else {
                txtSAF = txtSAF;
            }
            if (txtBusDeduct == "") {
                txtBusDeduct = parseInt(0);
            }
            else {
                txtBusDeduct = txtBusDeduct;
            }

            var txtTotAllownce = document.getElementById('<%=txtTotAllownce.ClientID %>').value;
            if (txtTotAllownce == "") {
                txtTotAllownce = parseInt(0);
            }
            else {
                txtTotAllownce = txtTotAllownce;
            }
            var totnetamount = document.getElementById('txtNetAmount');
            var txtTotDeduct = document.getElementById('txtTotDeduct');

            if (parseInt(DesId) > 21) {
                if (parseInt(txtTotalsalavailed) >= 35000000) {
                    alert("Total Salary Availed should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

                    txtTotalsalavailed.focus();
                    return false;
                }
                if (parseInt(txtClaRental) >= 35000000) {
                    alert("CLA RENTAL should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtTotReimAvailed) + parseInt(0) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

                    txtClaRental.focus();
                    return false;
                }
                if (parseInt(txtTotReimAvailed) >= 35000000) {
                    alert("Total Salary Component should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

                    txtTotReimAvailed.focus();
                    return false;
                }
                if (parseInt(txtVehEmi) >= 35000000) {
                    alert("Vehicle EMI should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(0) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

                    txtVehEmi.focus();
                    return false;
                }
                if (parseInt(txtBusDeduct) >= 35000000) {
                    alert("Bus Deduction should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(0))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

                    txtBusDeduct.focus();
                    return false;
                }
                if (parseInt(txtSAF) >= 35000000) {
                    alert("saf Deduction should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(0) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtSAF.focus();
                    return false;
                }
                txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
            }
            else {

                if (parseInt(txtTotalsalavailed) >= 35000000) {
                    alert("Total Salary Availed should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(0) + parseInt(0) + parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtTotalsalavailed.focus();
                    return false;
                }
                if (parseInt(txtClaRental) >= 35000000) {
                    alert("CLA RENTAL should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(0) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtClaRental.focus();
                    return false;
                }
                if (parseInt(txtTotReimAvailed) >= 35000000) {
                    alert("Total Salary Component should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(0) + parseInt(0) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtTotReimAvailed.focus();
                    return false;
                }
                if (parseInt(txtVehEmi) >= 35000000) {
                    alert("Vehicle EMI should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(0) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtVehEmi.focus();
                    return false;
                }
                if (parseInt(txtBusDeduct) >= 35000000) {
                    alert("Bus Deduction should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(txtretrialbcomponent) + parseInt(txtBasiccomponent) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(0))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtBusDeduct.focus();
                    return false;
                }
                if (parseInt(txtSAF) >= 35000000) {
                    alert("saf Deduction should be less than Rs.35000000");
                    txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(0) + parseInt(txtBusDeduct))
                    totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);
                    txtSAF.focus();
                    return false;
                }
                txtTotDeduct.value = (parseInt(txtTotalsalavailed) + parseInt(0) + parseInt(0) + parseInt(txtTotReimAvailed) + parseInt(txtClaRental) + parseInt(txtVehEmi) + parseInt(txtSAF) + parseInt(txtBusDeduct))
                totnetamount.value = parseInt(txtTotAllownce) - parseInt(txtTotDeduct.value);

            }
        }

        function clearContents(txt, evt) {

            var defaultText = "0";
            if (txt.value.length == 0 && evt.type == "blur") {
                txt.style.color = "gray";
                txt.value = defaultText;
            }
            if (txt.value == defaultText && evt.type == "focus") {
                txt.style.color = "black";
                txt.value = "";
            }
        };

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scp" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="HFSAPID" runat="server" />
    <asp:HiddenField ID="HFUSERSAPID" runat="server" />
    <asp:HiddenField ID="HFID" runat="server" />
    <asp:HiddenField ID="HFEMAILS" runat="server" />
    <asp:HiddenField ID="HFDESID" runat="server" />
    <asp:HiddenField ID="HFLOC" runat="server" />
    <asp:HiddenField ID="HFBU" runat="server" />
      <asp:HiddenField ID="HFENAME" runat="server" />
       <asp:HiddenField ID="HFCARSCHEME" runat="server" />
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
                                    FPA Initiation <span id="spnFormCreater" runat="server"></span>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" align="left" valign="top">
                                <strong>Initiator Name</strong>
                            </td>
                            <td width="25%" align="left" valign="top">
                                <span id="spnIntName" runat="server"></span>
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
                
                <div class="row mBtm20" style="height: 300px; overflow-y: scroll;">
                    <asp:GridView ID="gvUser" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        runat="server" AutoGenerateColumns="false" OnDataBound="OnDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Ename" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%"
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate> 
                                 <asp:Label ID="lblSapid" Visible="false" runat="server" Text='<%#Eval("SapId") %>'></asp:Label>
                                  <asp:Label ID="lblEmail" Visible="false" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                  <asp:Label ID="lblDesid" Visible="false" runat="server" Text='<%#Eval("DesId") %>'></asp:Label>
                                    <asp:Label ID="lblEname" runat="server" Text='<%#Eval("ENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ecode" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:Label ID="lblEcode" runat="server" Text='<%#Eval("ECODE") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("Loc") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesig" runat="server" Text='<%#Eval("DESIGNATION") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Car Scheme" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                   <asp:Label ID="lblCarcsheme" runat="server" Text='<%#GetStatus(Eval("CAR_SCHEME")) %>'></asp:Label>
                                 
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"
                                ItemStyle-Width="10%" HeaderText="Initiate">
                                <ItemTemplate>
                                <input type="button" onclick="<%#string.Format("UserDetail('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",Eval("SapId"),Eval("DesId"),Eval("Ename"),Eval("Email"),Eval("Loc"),Eval("DESIGNATION"),Eval("ECODE"),Eval("CAR_SCHEME"))%>" id="btn" value="Initiate" />
                                    <%--<asp:LinkButton ID="btnInitiate" runat="server" OnClientClick="java OnClick="btnInitiate_Click">Initiate</asp:LinkButton>--%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                       
                    </asp:GridView>
                </div>
               <%--  <div id="dvFunGoalArea" class="row mBtm20" style="height: 650px; width: 850px; display: none;">--%>
                <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mBtm20" id="FpaTab" style="height: 650px; width: 850px; display: none;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brd">
                                <tr>
                                    <td colspan="3" align="left" valign="top" class="head">
                                        <h2 class="black16new wnornew">
                                            Initiation Detail of - <span id="spnUserName1" ></span></h2>
                                    </td>
                                    <td align="right" valign="top" class="head" style="width: 20%">
                                        <a style="border: none;" href="javascript:void(0);" onclick="Popup.hide('FpaTab')">
                                            <img alt="close" style="border: 0; cursor: pointer;" src="images/close-i.png" /></a>
                            </td>
                                </tr>
                                      <tr>
                            <td width="25%" align="left" valign="top" colspan="4">
                                <div class="row mBtm20">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brdnew">
                                       <%-- <tr>
                                            <td colspan="4" align="left" valign="top" class="head">
                                                <h2 class="black16new wnornew">
                                                    FPA Initiation 
                                                </h2>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td width="25%" align="left" valign="top">
                                                <strong>Ecode</strong>
                                            </td>
                                            <td width="25%" align="left" valign="top">
                                                <span id="spnEcode" ></span>
                                            </td>
                                            <td width="25%" align="left" valign="top">
                                                <strong> Location</strong>
                                            </td>
                                            <td width="25%" align="left" valign="top">
                                                <span id="spnLocUser" ></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <strong>Designation</strong>
                                            </td>
                                            <td align="left" valign="top">
                                                <span id="spnDesig" ></span>
                                               
                                            </td>
                                            <td align="left" valign="top">
                                                <strong></strong>
                                            </td>
                                            <td align="left" valign="top">
                                                <span id="" ></span>
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            
                        </tr>
                                <tr id="trgm" runat="server">
                                    <td width="33%" align="left" valign="center">
                                        <strong>
                                        <span ID="ltctc" runat="server">CTC</span>
                                           <%-- <asp:Literal ID="ltctc" runat="server" Text="CTC"></asp:Literal>--%></strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtCTC" runat="server" MaxLength="9" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="checkCTC(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtCTC"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Total Taxable Amount Availed</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtTotalsalavailed" MaxLength="8" runat="server" onfocus="clearContents(this, event);"
                                            CssClass="inptnew inpt2new" onblur="check(this);clearContents(this, event);"
                                            onkeyup="deductionCompute()"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTotalsalavailed"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="trgmctc" runat="server">
                                    <td id="Td1" width="33%" align="left" valign="center" runat="server">
                                        <strong>
                                           <span ID="ltcbasic" runat="server" >Current Basic</span>
                                           <%-- <asp:Literal ID="ltcbasic" runat="server" Text="Current Basic"></asp:Literal>--%></strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtcBasic" runat="server" MaxLength="9" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtcBasic"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Total Reimbursement Availed</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtTotReimAvailed" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            Enabled="false" ReadOnly="true" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong> <span ID="ltfpa" runat="server">FPA</span>
                                           <%-- <asp:Literal ID="ltfpa" runat="server" Text="FPA"></asp:Literal>--%></strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtFPA" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtFPA"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>CLA Rental</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtClaRental" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:deductionCompute();"
                                            onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtClaRental"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Others(1)</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtOther1" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txtOther1"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Vehicle EMI</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtVehEmi" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:deductionCompute()"
                                            onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtVehEmi"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Others(2)</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtOther2" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOther2"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>SAF</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtSAF" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:deductionCompute()"
                                            onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtSAF"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Location FPA</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtLocFPA" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtLocFPA"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Bus Deduction</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtBusDeduct" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:deductionCompute()"
                                            onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtBusDeduct"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Others(3)</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtOther3" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            onfocus="clearContents(this, event);" onkeyup="javascript:fpaCompute()" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtOther3"
                                            FilterType="Numbers" FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Total Deduction</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtTotDeduct" runat="server" Font-Bold="true" Enabled="false" CssClass="inptnew inpt2new"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Total Petrol Credited</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtTotpcredited" runat="server" MaxLength="8" CssClass="inptnew inpt2new"
                                            ReadOnly="true" Enabled="false" onblur="check(this);clearContents(this, event);"></asp:TextBox>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Net Amount</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtNetAmount" Enabled="false" Font-Bold="true" runat="server" CssClass="inptnew inpt2new"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" align="left" valign="center">
                                        <strong>Total Allowance</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                        <asp:TextBox ID="txtTotAllownce" runat="server" Font-Bold="true" CssClass="inptnew inpt2new"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                    <td width="33%" align="left" valign="center">
                                         <strong>Car Scheme</strong>
                                    </td>
                                    <td width="17%" align="left" valign="center">
                                         <asp:DropDownList ID="ddlCarScheme" Width="110px" Height="22px" runat="server" >
                                        <asp:ListItem Value="No Scheme" Text="No Scheme"></asp:ListItem>
                                        <asp:ListItem Value="New Scheme" Text="New Scheme"></asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="greyBgnew" colspan="4">
                                        <strong>Comments</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="4" valign="top">
                                        <asp:TextBox ID="txtComments" runat="server" CssClass="inptnew" Rows="3"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" FilterMode="InvalidChars"
                                            FilterType="Custom" InvalidChars="@,#,&lt;,&gt;,%,&amp;,^,$" TargetControlID="txtComments">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"
                                            WatermarkText="Please Enter Comments Here.">
                                        </cc1:TextBoxWatermarkExtender>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                <tr>
                                    <td width="35%" align="left" valign="top">
                                    </td>
                                    <td width="10%" align="left" valign="top">
                                      <input type="button" class="button" value="Initiate FPA" onclick="SubmitFpa()" />
                                      <%--  <asp:LinkButton ID="btnIntiation"  runat="server" CssClass="button"
                                            OnClick="btnIntiation_Click">
                        Initiate FPA</asp:LinkButton>--%>
                                    </td>
                                    <td width="10%" align="left" valign="top">
                                    <input type="button" class="button" value="Clear All" onclick="SetVisible()" />
                                        <%--<asp:LinkButton ID="btnClearAll" runat="server" CssClass="button" OnClick="btnClearAll_Click">
                         Clear All</asp:LinkButton>--%>
                                    </td>
                                    <td width="10%" align="left" valign="top">
                                      <input type="button" class="button" value="Close" onclick="Popup.hide('FpaTab')"/>
                                       <%-- <asp:LinkButton ID="LinkButton1" runat="server" CssClass="button" OnClientClick="javascript:CloseWindowcan();">
                         Close</asp:LinkButton>--%>
                                    </td>
                                    <td width="35%" align="left" valign="top">
                                    </td>
                                </tr>
                            </table>
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
            </div>
        </div>
    </div>
   

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript" src="js/quicksearch.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.search_textbox').each(function (i) {
                $(this).quicksearch("[id*=gvUser] tr:not(:has(th))", {
                    'testQuery': function (query, txt, row) {
                        return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
    </script>

    </form>
</body>
</html>
