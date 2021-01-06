using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using Microsoft.SharePoint;

public partial class TDSFORMnew : System.Web.UI.Page
{
    public string FolderAddress = "D:\\Eicher\\All Logs\\TDS";
    Utility.Service Util = new Utility.Service();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillUserinfo();

        }
    }

    private void FillUserinfo()
    {
        TDS objtds = new TDS();
        try
        {
            DataTable DT_Userinfo = objtds.ReturnEmployeeInfo(ReturnLoginName());
            lblname.Text = DT_Userinfo.Rows[0]["ename"].ToString();
            lblEmployeeId.Text = DT_Userinfo.Rows[0]["Ecode"].ToString();
            lblbu.Text = DT_Userinfo.Rows[0]["BU"].ToString().Trim();
            lblFiadmin.Text = DT_Userinfo.Rows[0]["FIAdmin"].ToString().Trim();
            lbldate.Text = objtds.ReturnActiveSubmssion_Date(DT_Userinfo.Rows[0]["BU"].ToString().Trim());
            lbldate1.Text = lbldate.Text;
            lblpan.Text = DT_Userinfo.Rows[0]["PANCard"].ToString();

            if (Request.QueryString.ToString() == "")
            {
                lblyear.Text = objtds.ReturnActiveyear(DT_Userinfo.Rows[0]["BU"].ToString().Trim());
                // lblyearrent.Text = objtds.ReturnActiveyear(DT_Userinfo.Rows[0]["BU"].ToString().Trim());

                EnableVerifyCol(false);

                CheckIfopen(DT_Userinfo.Rows[0]["BU"].ToString().Trim());
                CheckIfformSubmittedforyear(ReturnSAPID(), lblyear.Text);

            }

            else
            {
                NameValueCollection querystring = StringHelpers.DecryptQueryString(Request.QueryString.ToString());

                string Year = querystring[0];
                string Formstatus = querystring[1];
                string FI_Year = querystring[2];
                string SAPID = querystring[3];
                string ToVerifier = querystring[4];
                decimal _dec = 0;

                lblyear.Text = FI_Year;
                //lblyearrent.Text = FI_Year;

                if (Formstatus == "0")
                {
                    lblStatus.Text = "Save as Draft";

                    EnableVerifyCol(false);
                    FillDatafromDB(Year, SAPID);
                    //MakeControlsReadOnly(this.Controls);
                    if (IsNumeric(txthradec.Text))
                    { _dec = Convert.ToDecimal(txthradec.Text); }
                    if (_dec > 0) { txthradec.Enabled = true; txthrasub.Enabled = true; }

                }
                else if (Formstatus == "1")
                {
                    lblStatus.Text = "Declared";

                    //MakeControlsReadOnly(this.Controls);
                    FillDatafromDB(Year, SAPID);

                }
                else if (Formstatus == "2" || Formstatus == "3")
                {
                    btnDraft.Visible = false;
                    btnSubmit.Visible = false;
                    MakeControlsReadOnly(this.Controls);
                    FillDatafromDB(Year, SAPID);
                    if (Formstatus == "2")
                    {
                        lblStatus.Text = "Submitted";
                    }
                    if (Formstatus == "3")
                    {
                        lblStatus.Text = "Verified";
                    }

                    if (ToVerifier == "Yes")
                    {

                        if (Formstatus == "2")
                        {
                            butVerify.Visible = true;
                            butVerify.Enabled = true;

                            CopySubmittedAmountInVerifyCol();
                            EnableVerifyCol(true);
                        }
                        if (Formstatus == "3")
                        {
                            butVerify.Visible = false;
                            butVerify.Enabled = false;

                            butReVerify.Visible = true;
                            butReVerify.Enabled = true;
                        }
                    }
                    else
                    {
                        EnableVerifyCol(false);
                    }
                }
                else
                {
                    lblStatus.Text = "Close";
                    btnDraft.Visible = false;
                    btnSubmit.Visible = false;
                    MakeControlsReadOnly(this.Controls);
                    FillDatafromDB(Year, SAPID);
                }
            }
        }
        catch (Exception ex)
        {
            objtds.Log("Inside exception of FillUserinfo " + ex.Message.ToString() + "", FolderAddress);
        }

    }

    private void CheckIfformSubmittedforyear(string SAPID, string FIYEAR)
    {
        TDS objtds = new TDS();

        try
        {
            if (objtds.ReturnCheckCount(SAPID, FIYEAR) > 0)
            {
                string strScript = "<script>";
                strScript += "alert('You have already submitted TDS for the Financial Year " + FIYEAR + "');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);
            }
        }
        catch (Exception ex)
        {
            objtds.Log("Inside exception of CheckIfformSubmittedforyear " + ex.Message.ToString() + "", FolderAddress);
        }
    }

    private void FillDatafromDB(string Year, string SAPID)
    {
        TDS objtds = new TDS();
        try
        {
            DataTable DT_TDSINFO = objtds.ReturnUserTDSDatabySAPIDnYear(Year, SAPID);

            int _count = 1;
            string _subhead = "", _subhead2 = "", _subhead3 = "", _subhead4 = "", _subhead5 = "", _subhead6 = "";
            if (DT_TDSINFO.Rows.Count > 0)
            {
                foreach (DataRow _row in DT_TDSINFO.Rows)
                {
                    _count = Convert.ToInt32(_row.ItemArray[2].ToString());
                    _subhead = _row.ItemArray[3].ToString();
                    _subhead2 = _row.ItemArray[7].ToString();
                    _subhead3 = _row.ItemArray[8].ToString();
                    _subhead4 = _row.ItemArray[9].ToString();
                    _subhead5 = _row.ItemArray[10].ToString();
                    _subhead6 = _row.ItemArray[11].ToString();

                    if (_count == 101)
                    { txt80D1dec.Text = _row.ItemArray[4].ToString(); txt80D1sub.Text = _row.ItemArray[5].ToString(); txt80D1ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 102)
                    { txt80D2dec.Text = _row.ItemArray[4].ToString(); txt80D2sub.Text = _row.ItemArray[5].ToString(); txt80D2ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 103)
                    { txt80D3dec.Text = _row.ItemArray[4].ToString(); txt80D3sub.Text = _row.ItemArray[5].ToString(); txt80D3ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 104)
                    { txt80D4dec.Text = _row.ItemArray[4].ToString(); txt80D4sub.Text = _row.ItemArray[5].ToString(); txt80D4ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 105)
                    {
                        txt80D5dec.Text = _row.ItemArray[4].ToString(); txt80D5sub.Text = _row.ItemArray[5].ToString(); txt80D5ver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            if (_subhead == "Less than 80")
                            { rbl80D5.SelectedIndex = 0; }
                            else
                            { rbl80D5.SelectedIndex = 1; }
                        }
                    }
                    else if (_count == 106)
                    {
                        txt80D6dec.Text = _row.ItemArray[4].ToString(); txt80D6sub.Text = _row.ItemArray[5].ToString(); txt80D6ver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            if (_subhead == "Sr Citizen")
                            { rbl80D6.SelectedIndex = 0; }
                            else
                            { rbl80D6.SelectedIndex = 1; }
                        }
                    }
                    else if (_count == 107)
                    {
                        txt80D7dec.Text = _row.ItemArray[4].ToString(); txt80D7sub.Text = _row.ItemArray[5].ToString(); txt80D7ver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            if (_subhead == "Less than 80")
                            { rbl80D7.SelectedIndex = 0; }
                            else
                            { rbl80D7.SelectedIndex = 1; }
                        }
                    }
                    else if (_count == 108)
                    { txt80D8dec.Text = _row.ItemArray[4].ToString(); txt80D8sub.Text = _row.ItemArray[5].ToString(); txt80D8ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 109)
                    { txt80D9dec.Text = _row.ItemArray[4].ToString(); txt80D9sub.Text = _row.ItemArray[5].ToString(); txt80D9ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 110)
                    { txt80D10dec.Text = _row.ItemArray[4].ToString(); txt80D10sub.Text = _row.ItemArray[5].ToString(); txt80D10ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 111)
                    { txt80D11dec.Text = _row.ItemArray[4].ToString(); txt80D11sub.Text = _row.ItemArray[5].ToString(); txt80D11ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 112)
                    { txt80D12dec.Text = _row.ItemArray[4].ToString(); txt80D12sub.Text = _row.ItemArray[5].ToString(); txt80D12ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 201)
                    {
                        txthradec.Text = _row.ItemArray[4].ToString(); txthrasub.Text = _row.ItemArray[5].ToString(); txthraver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            ddlhrametro.SelectedValue = _subhead;
                        }
                        if (_subhead2.Length > 0)
                        {
                            ddlhraNoonMonth.SelectedValue = _subhead2;
                        }
                        if (_subhead3.Length > 0)
                        {
                            txtLandlordName.Text = _subhead3;
                        }
                        if (_subhead4.Length > 0)
                        {
                            txtLandlordAddress.Text = _subhead4;
                        }
                        if (_subhead5.Length > 0)
                        {
                            txtLandlordPAN.Text = _subhead5;
                        }
                    }
                    else if (_count == 301)
                    { txtifos1dec.Text = _row.ItemArray[4].ToString(); txtifos1sub.Text = _row.ItemArray[5].ToString(); txtifos1ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 302)
                    { txtifos2dec.Text = _row.ItemArray[4].ToString(); txtifos2sub.Text = _row.ItemArray[5].ToString(); txtifos2ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 401)
                    {
                        txtsec101dec.Text = _row.ItemArray[4].ToString(); txtsec101sub.Text = _row.ItemArray[5].ToString(); txtsec101ver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            if (_subhead == "0")
                            { rblsec101.SelectedIndex = 0; }
                            else if (_subhead == "1")
                            { rblsec101.SelectedIndex = 1; }
                            else if (_subhead == "2")
                            { rblsec101.SelectedIndex = 2; }
                        }
                    }
                    else if (_count == 402)
                    {
                        txtsec102dec.Text = _row.ItemArray[4].ToString(); txtsec102sub.Text = _row.ItemArray[5].ToString(); txtsec102ver.Text = _row.ItemArray[6].ToString();
                        if (_subhead.Length > 0)
                        {
                            if (_subhead == "0")
                            { rblsec102.SelectedIndex = 0; }
                            else if (_subhead == "1")
                            { rblsec102.SelectedIndex = 1; }
                            else if (_subhead == "2")
                            { rblsec102.SelectedIndex = 2; }
                        }
                    }
                    else if (_count == 403)
                    { txtsec103dec.Text = _row.ItemArray[4].ToString(); txtsec103sub.Text = _row.ItemArray[5].ToString(); txtsec103ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 404)
                    { txtsec104dec.Text = _row.ItemArray[4].ToString(); txtsec104sub.Text = _row.ItemArray[5].ToString(); txtsec104ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 405)
                    { txtsec105dec.Text = _row.ItemArray[4].ToString(); txtsec105sub.Text = _row.ItemArray[5].ToString(); txtsec105ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 501)
                    {
                        txt05adec.Text = _row.ItemArray[4].ToString(); txt05asub.Text = _row.ItemArray[5].ToString(); txt05aver.Text = _row.ItemArray[6].ToString();

                        if (_subhead3.Length > 0)
                        {
                            txtFinName05.Text = _subhead3;
                        }
                        if (_subhead4.Length > 0)
                        {
                            txtFinAdd05.Text = _subhead4;
                        }
                        if (_subhead5.Length > 0)
                        {
                            txtFinPAN05.Text = _subhead5;
                        }
                        if (_subhead6.Length > 0)
                        {
                            if (_subhead6 == "Financial Institutions") { ddlFinType05.SelectedValue = "1"; }
                            if (_subhead6 == "Employer") { ddlFinType05.SelectedValue = "2"; }
                            if (_subhead6 == "Other") { ddlFinType05.SelectedValue = "3"; }
                            //ddlFinType05.SelectedValue = _subhead6;
                        }
                    }
                    else if (_count == 502)
                    { txt05bdec.Text = _row.ItemArray[4].ToString(); txt05bsub.Text = _row.ItemArray[5].ToString(); txt05bver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 503)
                    { txt05cdec.Text = _row.ItemArray[4].ToString(); txt05csub.Text = _row.ItemArray[5].ToString(); txt05cver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 504)
                    { txt05ddec.Text = _row.ItemArray[4].ToString(); txt05dsub.Text = _row.ItemArray[5].ToString(); txt05dver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 505)
                    { txt05edec.Text = _row.ItemArray[4].ToString(); txt05esub.Text = _row.ItemArray[5].ToString(); txt05ever.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 506)
                    { txt05fdec.Text = _row.ItemArray[4].ToString(); txt05fsub.Text = _row.ItemArray[5].ToString(); txt05fver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 507)
                    { txt05gdec.Text = _row.ItemArray[4].ToString(); txt05gsub.Text = _row.ItemArray[5].ToString(); txt05gver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 601)
                    {
                        txt061dec.Text = _row.ItemArray[4].ToString(); txt061sub.Text = _row.ItemArray[5].ToString(); txt061ver.Text = _row.ItemArray[6].ToString();

                        if (_subhead3.Length > 0)
                        {
                            txtFinName.Text = _subhead3;
                        }
                        if (_subhead4.Length > 0)
                        {
                            txtFinAdd.Text = _subhead4;
                        }
                        if (_subhead5.Length > 0)
                        {
                            txtFinPAN.Text = _subhead5;
                        }
                        if (_subhead6.Length > 0)
                        {
                            if (_subhead6 == "Financial Institutions") { ddlFinType.SelectedValue = "1"; }
                            if (_subhead6 == "Employer") { ddlFinType.SelectedValue = "2"; }
                            if (_subhead6 == "Other") { ddlFinType.SelectedValue = "3"; }
                            //ddlFinType.SelectedValue = _subhead6;
                        }

                    }
                    else if (_count == 602)
                    { txt062dec.Text = _row.ItemArray[4].ToString(); txt062sub.Text = _row.ItemArray[5].ToString(); txt062ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 701)
                    { txt071dec.Text = _row.ItemArray[4].ToString(); txt071sub.Text = _row.ItemArray[5].ToString(); txt071ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 702)
                    { txt072dec.Text = _row.ItemArray[4].ToString(); txt072sub.Text = _row.ItemArray[5].ToString(); txt072ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 703)
                    { txt073dec.Text = _row.ItemArray[4].ToString(); txt073sub.Text = _row.ItemArray[5].ToString(); txt073ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 704)
                    { txt074dec.Text = _row.ItemArray[4].ToString(); txt074sub.Text = _row.ItemArray[5].ToString(); txt074ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 705)
                    { txt075dec.Text = _row.ItemArray[4].ToString(); txt075sub.Text = _row.ItemArray[5].ToString(); txt075ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 706)
                    { txt076dec.Text = _row.ItemArray[4].ToString(); txt076sub.Text = _row.ItemArray[5].ToString(); txt076ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 707)
                    { }
                    else if (_count == 708)
                    { txt078dec.Text = _row.ItemArray[4].ToString(); txt078sub.Text = _row.ItemArray[5].ToString(); txt078ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 709)
                    { txt079dec.Text = _row.ItemArray[4].ToString(); txt079sub.Text = _row.ItemArray[5].ToString(); txt079ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 710)
                    { txt0710dec.Text = _row.ItemArray[4].ToString(); txt0710sub.Text = _row.ItemArray[5].ToString(); txt0710ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 711)
                    { txt0711dec.Text = _row.ItemArray[4].ToString(); txt0711sub.Text = _row.ItemArray[5].ToString(); txt0711ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 712)
                    { txt0712dec.Text = _row.ItemArray[4].ToString(); txt0712sub.Text = _row.ItemArray[5].ToString(); txt0712ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 713)
                    { txt0713dec.Text = _row.ItemArray[4].ToString(); txt0713sub.Text = _row.ItemArray[5].ToString(); txt0713ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 714)
                    { txt0714dec.Text = _row.ItemArray[4].ToString(); txt0714sub.Text = _row.ItemArray[5].ToString(); txt0714ver.Text = _row.ItemArray[6].ToString(); }
                    else if (_count == 715)
                    { txt0715dec.Text = _row.ItemArray[4].ToString(); txt0715sub.Text = _row.ItemArray[5].ToString(); txt0715ver.Text = _row.ItemArray[6].ToString(); }
                }
            }

            Showtotal80DDec(); ShowtotalifosDec(); Showtotalsec10Dec(); Showtotal05Dec(); Showtotal06Dec(); Showtotal07Dec();
        }
        catch (Exception ex)
        {
            objtds.Log("Inside exception of FillDatafromDB" + ex.Message.ToString() + "", FolderAddress);
        }

    }
    private void Showtotal07Dec()
    {
        var tSum07Dec = Returnval(txt071dec) + Returnval(txt072dec) + Returnval(txt073dec) + Returnval(txt074dec) + Returnval(txt075dec) + Returnval(txt076dec) + Returnval(txt078dec) + Returnval(txt079dec) + Returnval(txt0710dec) + Returnval(txt0711dec) + Returnval(txt0712dec) + Returnval(txt0713dec) + Returnval(txt0714dec) + Returnval(txt0715dec);
        txt07dectot.Text = tSum07Dec.ToString();
        var tSum07Sub = Returnval(txt071sub) + Returnval(txt072sub) + Returnval(txt073sub) + Returnval(txt074sub) + Returnval(txt075sub) + Returnval(txt076sub) + Returnval(txt078sub) + Returnval(txt079sub) + Returnval(txt0710sub) + Returnval(txt0711sub) + Returnval(txt0712sub) + Returnval(txt0713sub) + Returnval(txt0714sub) + Returnval(txt0715sub);
        txt07subtot.Text = tSum07Sub.ToString();
        var tSum07Ver = Returnval(txt071ver) + Returnval(txt072ver) + Returnval(txt073ver) + Returnval(txt074ver) + Returnval(txt075ver) + Returnval(txt076ver) + Returnval(txt078ver) + Returnval(txt079ver) + Returnval(txt0710ver) + Returnval(txt0711ver) + Returnval(txt0712ver) + Returnval(txt0713ver) + Returnval(txt0714ver) + Returnval(txt0715ver);
        txt07vertot.Text = tSum07Ver.ToString();
    }

    private void Showtotal06Dec()
    {
        var tSum06Dec = Returnval(txt061dec) + Returnval(txt062dec);
        txt06dectot.Text = tSum06Dec.ToString();
        var tSum06Sub = Returnval(txt061sub) + Returnval(txt062sub);
        txt06subtot.Text = tSum06Sub.ToString();
        var tSum06Ver = Returnval(txt061ver) + Returnval(txt062ver);
        txt06vertot.Text = tSum06Ver.ToString();
    }
    private void Showtotal05Dec()
    {
        decimal var_C = Returnval(txt05adec) - Returnval(txt05bdec);

        if (var_C < 0) { var_C = 0; }

        decimal var_D = Math.Round((var_C) * Convert.ToDecimal("0.3"));

        txt05cdec.Text = var_C.ToString();
        txt05ddec.Text = var_D.ToString();
        decimal var_E = var_C - var_D;

        if (var_E < 0) { var_E = 0; }

        decimal var_F = Returnval(txt05fdec);
        txt05edec.Text = var_E.ToString();

        decimal var_G = var_E - var_F;
        if (var_G < 0) { var_G = 0; }

        txt05gdec.Text = var_G.ToString();

        var_C = Returnval(txt05asub) - Returnval(txt05bsub);

        if (var_C < 0) { var_C = 0; }

        var_D = Math.Round((var_C) * Convert.ToDecimal("0.3"));

        txt05csub.Text = var_C.ToString();
        txt05dsub.Text = var_D.ToString();
        var_E = var_C - var_D;

        if (var_E < 0) { var_E = 0; }

        var_F = Returnval(txt05fsub);
        txt05esub.Text = var_E.ToString();

        var_G = var_E - var_F;
        if (var_G < 0) { var_G = 0; }

        txt05gsub.Text = var_G.ToString();

        var_C = Returnval(txt05aver) - Returnval(txt05bver);

        if (var_C < 0) { var_C = 0; }

        var_D = Math.Round((var_C) * Convert.ToDecimal("0.3"));

        txt05cver.Text = var_C.ToString();
        txt05dver.Text = var_D.ToString();
        var_E = var_C - var_D;

        if (var_E < 0) { var_E = 0; }

        var_F = Returnval(txt05fver);
        txt05ever.Text = var_E.ToString();

        var_G = var_E - var_F;
        if (var_G < 0) { var_G = 0; }

        txt05gver.Text = var_G.ToString();
    }
    private void Showtotalsec10Dec()
    {
        var tSumsec10Dec = Returnval(txtsec101dec) + Returnval(txtsec102dec) + Returnval(txtsec103dec) + Returnval(txtsec104dec) + Returnval(txtsec105dec);
        txtsec10dectot.Text = tSumsec10Dec.ToString();

        var tSumsec10Sub = Returnval(txtsec101sub) + Returnval(txtsec102sub) + Returnval(txtsec103sub) + Returnval(txtsec104sub) + Returnval(txtsec105sub);
        txtsec10subtot.Text = tSumsec10Sub.ToString();

        var tSumsec10Ver = Returnval(txtsec101ver) + Returnval(txtsec102ver) + Returnval(txtsec103ver) + Returnval(txtsec104ver) + Returnval(txtsec105ver);
        txtsec10vertot.Text = tSumsec10Ver.ToString();
    }
    private void ShowtotalifosDec()
    {
        var tSumifosDec = Returnval(txtifos1dec) + Returnval(txtifos2dec);
        txtifosdectot.Text = tSumifosDec.ToString();

        var tSumifosSub = Returnval(txtifos1sub) + Returnval(txtifos2sub);
        txtifossubtot.Text = tSumifosSub.ToString();

        var tSumifosVer = Returnval(txtifos1ver) + Returnval(txtifos2ver);
        txtifosvertot.Text = tSumifosVer.ToString();
    }
    private void Showtotal80DDec()
    {
        var tSumDec = Returnval(txt80D1dec) + Returnval(txt80D2dec) + Returnval(txt80D3dec) + Returnval(txt80D4dec) + Returnval(txt80D5dec)
            + Returnval(txt80D6dec) + Returnval(txt80D7dec) + Returnval(txt80D8dec) + Returnval(txt80D9dec) + Returnval(txt80D10dec)
            + Returnval(txt80D11dec) + Returnval(txt80D12dec);

        txt80Ddectot.Text = tSumDec.ToString();

        var tSumSub = Returnval(txt80D1sub) + Returnval(txt80D2sub) + Returnval(txt80D3sub) + Returnval(txt80D4sub) + Returnval(txt80D5sub)
                   + Returnval(txt80D6sub) + Returnval(txt80D7sub) + Returnval(txt80D8sub) + Returnval(txt80D9sub) + Returnval(txt80D10sub)
                   + Returnval(txt80D11sub) + Returnval(txt80D12sub);

        txt80Dsubtot.Text = tSumSub.ToString();
        var tSumVer = Returnval(txt80D1ver) + Returnval(txt80D2ver) + Returnval(txt80D3ver) + Returnval(txt80D4ver) + Returnval(txt80D5ver)
            + Returnval(txt80D6ver) + Returnval(txt80D7ver) + Returnval(txt80D8ver) + Returnval(txt80D9ver) + Returnval(txt80D10ver)
            + Returnval(txt80D11ver) + Returnval(txt80D12ver);

        txt80Dvertot.Text = tSumVer.ToString();
    }

    private decimal Returnval(TextBox _textBox)
    {
        if (IsNumeric(_textBox.Text))
        {
            return Convert.ToDecimal(_textBox.Text);
        }
        else
        {
            return 0;
        }
    }

    private void MakeControlsReadOnly(ControlCollection controls)
    {
        foreach (Control c in controls)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Enabled = false;
            }
            else if (c is RadioButton)
            {
                ((RadioButton)c).Enabled = false;
            }
            else if (c is DropDownList)
            {
                ((DropDownList)c).Enabled = false;
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = false;
            }
            else if (c is RadioButtonList)
            {
                ((RadioButtonList)c).Enabled = false;
            }
            else if (c is Button)
            {
                ((Button)c).Enabled = false;
            }
            if (c.HasControls())
            {
                MakeControlsReadOnly(c.Controls);
            }
        }
    }

    private void CheckIfopen(string BU)
    {
        TDS objtds = new TDS();
        try
        {
            if (objtds.ReturnOpenStatusCount(BU) == 0)
            {
                string strScript = "<script>";
                strScript += "alert('TDS Admin has not initiated TDS for your BU..');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

            }
        }
        catch (Exception ex)
        {
            objtds.Log("Inside exception of CheckIfopen" + ex.Message.ToString() + "", FolderAddress);
        }
    }

    protected string ReturnSAPID()
    {
        TDS objtds = new TDS();
        return objtds.ReturnSAPID(ReturnLoginName());
    }

    private string PANCARD(string pancard)
    {
        if (pancard == "")
            return "N/A";
        else
            return pancard;
    }

    private int ChildCount()
    {
        return 0;// Convert.ToInt32(ddlChildhostel.SelectedValue) + Convert.ToInt32(ddlChildschool.SelectedValue);          
    }

    public int ReturnIntfromDropdown(DropDownList ddldata)
    {
        return Convert.ToInt32(ddldata.SelectedValue);
    }

    public int ReturnIntfromtextbox(TextBox txtdata)
    {
        if (txtdata.Text == "")
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(txtdata.Text);
        }
    }

    public int ReturnIntfromtextboxInp(string txtdata)
    {
        if (txtdata == "")
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(txtdata);
        }
    }

    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }

    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "ajain10";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Save("2");
    }

    protected void btnDraft_Click(object sender, EventArgs e)
    {
        Save("0");
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        Save("3");
    }

    protected void btnReVerify_Click(object sender, EventArgs e)
    {
        EnableVerifyCol(true);
        butReVerify.Visible = false;
        butReVerify.Enabled = false;

        butVerify.Visible = true;
        butVerify.Enabled = true;

    }

    bool ValidateHRA()
    {
        decimal _dec = 0, _sub = 0, _ver = 0;
        string strScript = "<script>";

        if (IsNumeric(txthradec.Text)) { _dec = Convert.ToDecimal(txthradec.Text); }
        if (IsNumeric(txthrasub.Text)) { _sub = Convert.ToDecimal(txthrasub.Text); }
        if (IsNumeric(txthraver.Text)) { _ver = Convert.ToDecimal(txthraver.Text); }

        if (_dec > 100000 || _sub > 100000 || _ver > 100000)
        {
            if (txtLandlordName.Text.Length == 0)
            {
                strScript += "alert('Landlord Name is mandatory if HRA Amount > 100000 ...');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtLandlordName.Focus();
                return false;
            }
            if (txtLandlordAddress.Text.Length == 0)
            {
                strScript += "alert('Landlord Address is mandatory if HRA Amount > 100000 ...');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtLandlordAddress.Focus();
                return false;
            }
            if (txtLandlordPAN.Text.Length != 10)
            {
                strScript += "alert('Landlord PAN is mandatory if HRA Amount > 100000 ...');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtLandlordPAN.Focus();
                return false;
            }
        }

        return true;
    }

    bool ValidateFinancer()
    {
        decimal _dec = 0, _sub = 0, _ver = 0;
        string strScript = "<script>";

        if (IsNumeric(txt061dec.Text)) { _dec = Convert.ToDecimal(txt061dec.Text); }
        if (IsNumeric(txt061sub.Text)) { _sub = Convert.ToDecimal(txt061sub.Text); }
        if (IsNumeric(txt061ver.Text)) { _ver = Convert.ToDecimal(txt061ver.Text); }

        if (_dec > 0 || _sub > 0 || _ver > 0)
        {
            if (txtFinName.Text.Length == 0)
            {
                strScript += "alert('Financer Name is mandatory if Amount > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinName.Focus();
                return false;
            }
            if (txtFinAdd.Text.Length == 0)
            {
                strScript += "alert('Financer Address is mandatory if Amount > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinAdd.Focus();
                return false;
            }
            if (txtFinPAN.Text.Length != 10)
            {
                strScript += "alert('Financer PAN is mandatory if Amount > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinPAN.Focus();
                return false;
            }
            else
            {
                if (txtFinPAN.Text.Length != 10)
                {
                    strScript += "alert('INVALID PAN NO !');";
                    strScript += "SucessMessage();";
                    strScript += "</script>";
                    Page.RegisterClientScriptBlock("strScript", strScript);

                    txtFinPAN.Focus();
                    return false;
                }
            }
            if (ddlFinType.SelectedIndex < 1)
            {
                strScript += "alert('Invalid Financer Type !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                ddlFinType.Focus();
                return false;
            }

        }

        return true;
    }

    bool ValidateFinancer05()
    {
        decimal _dec = 0, _sub = 0, _ver = 0;
        string strScript = "<script>";

        if (IsNumeric(txt05adec.Text)) { _dec = Convert.ToDecimal(txt05adec.Text); }
        if (IsNumeric(txt05asub.Text)) { _sub = Convert.ToDecimal(txt05asub.Text); }
        if (IsNumeric(txt05aver.Text)) { _ver = Convert.ToDecimal(txt05aver.Text); }

        if (_dec > 0 || _sub > 0 || _ver > 0)
        {
            if (txtFinName05.Text.Length == 0)
            {
                strScript += "alert('Financer Name is mandatory if Let-Out Property Amount Declared is > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinName05.Focus();
                return false;
            }
            if (txtFinAdd05.Text.Length == 0)
            {
                strScript += "alert('Financer Address is mandatory if Let-Out Property Amount Declared is > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinAdd05.Focus();
                return false;
            }
            if (txtFinPAN05.Text.Length == 0)
            {
                strScript += "alert('Financer PAN is mandatory if Let-Out Property Amount Declared is > 0 !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                txtFinPAN05.Focus();
                return false;
            }
            else
            {
                if (txtFinPAN05.Text.Length != 10)
                {
                    strScript += "alert('INVALID PAN NO !');";
                    strScript += "SucessMessage();";
                    strScript += "</script>";
                    Page.RegisterClientScriptBlock("strScript", strScript);

                    txtFinPAN05.Focus();
                    return false;
                }
            }
            if (ddlFinType05.SelectedIndex < 1)
            {
                strScript += "alert('Invalid Financer Type !');";
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);

                ddlFinType05.Focus();
                return false;
            }


        }

        return true;
    }

    void Save(string _status)
    {
        TDS objtds = new TDS();
        int _success = 0;

        if (!ValidateHRA())
        { return; }

        if (!ValidateFinancer())
        { return; }

        if (!ValidateFinancer05())
        { return; }

        try
        {
            string xmldata = genxml();

            string SapID = ReturnSAPID();
            DataTable _dt = objtds.InsertTDS_Declaration(SapID, _status, lblyear.Text, xmldata);
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    if (_dt.Rows[0].ItemArray[0].ToString() == "1")
                    {
                        _success = 1;
                    }
                }
            }

            if (_success == 1)
            {
                if (_status == "2")
                {
                    DataTable DT_ApproInfo = objtds.ReturnApproverDetails(lblbu.Text);
                    //MailOnSubmit(DT_ApproInfo.Rows[0]["EMail"].ToString().Split('#')[1]);
                    MailOnSubmit("Ghanshyam.vats@convergenttec.com");
                }
                if (_status == "3")
                {
                    MailOnVerify(objtds.ReturnEMail(ReturnSAPID()).ToString().Split('#')[1]);
                    //MailOnVerify("Ghanshyam.vats@convergenttec.com");
                }

                string strScript = "<script>";
                if (_status == "0") { strScript += "alert('Your form has been saved as draft successfully...');"; }
                else if (_status == "2") { strScript += "alert('Your form has been submitted successfully...');"; }
                else if (_status == "3") { strScript += "alert('Your form has been verified successfully...');"; }
                strScript += "SucessMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);
            }
            else
            {
                string strScript = "<script>";
                strScript += "alert('Your form has not been saved !');";
                strScript += "FailMessage();";
                strScript += "</script>";
                Page.RegisterClientScriptBlock("strScript", strScript);
            }

        }
        catch (Exception ex)
        {

            objtds.Log("Inside exception of btnSubmit_Click" + ex.Message.ToString() + "", FolderAddress);
        }
        finally
        {
            if (_success == 1)
            { ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true); }
        }
    }

    private string genxml()
    {
        string _subhead = "";
        string _subhead2 = "";
        string _subhead3 = "";
        string _subhead4 = "";
        string _subhead5 = "";
        string _subhead6 = "";

        StringWriter strdata = new StringWriter();
        strdata.Write("<tds>");
        strdata.Write("<item><Head>101</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D1dec.Text + "</DecAmt><SubAmt>" + txt80D1sub.Text +
            "</SubAmt><VerAmt>" + txt80D1ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>102</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D2dec.Text + "</DecAmt><SubAmt>" + txt80D2sub.Text +
            "</SubAmt><VerAmt>" + txt80D2ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>103</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D3dec.Text + "</DecAmt><SubAmt>" + txt80D3sub.Text +
            "</SubAmt><VerAmt>" + txt80D3ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>104</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D4dec.Text + "</DecAmt><SubAmt>" + txt80D4sub.Text +
            "</SubAmt><VerAmt>" + txt80D4ver.Text + "</VerAmt></item>");

        _subhead = "";
        if (rbl80D5.SelectedIndex == 0) { _subhead = "Less than 80"; }
        else if (rbl80D5.SelectedIndex == 1) { _subhead = "More than 80"; }
        strdata.Write("<item><Head>105</Head><SubHead>" + _subhead + "</SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D5dec.Text + "</DecAmt><SubAmt>" + txt80D5sub.Text +
            "</SubAmt><VerAmt>" + txt80D5ver.Text + "</VerAmt></item>");

        _subhead = "";
        if (rbl80D6.SelectedIndex == 0) { _subhead = "Sr Citizen"; }
        else if (rbl80D6.SelectedIndex == 1) { _subhead = "Other"; }
        strdata.Write("<item><Head>106</Head><SubHead>" + _subhead + "</SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D6dec.Text + "</DecAmt><SubAmt>" + txt80D6sub.Text +
            "</SubAmt><VerAmt>" + txt80D6ver.Text + "</VerAmt></item>");

        _subhead = "";
        if (rbl80D7.SelectedIndex == 0) { _subhead = "Less than 80"; }
        else if (rbl80D7.SelectedIndex == 1) { _subhead = "More than 80"; }
        strdata.Write("<item><Head>107</Head><SubHead>" + _subhead + "</SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D7dec.Text + "</DecAmt><SubAmt>" + txt80D7sub.Text +
            "</SubAmt><VerAmt>" + txt80D7ver.Text + "</VerAmt></item>");

        strdata.Write("<item><Head>108</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D8dec.Text + "</DecAmt><SubAmt>" + txt80D8sub.Text +
            "</SubAmt><VerAmt>" + txt80D8ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>109</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D9dec.Text + "</DecAmt><SubAmt>" + txt80D9sub.Text +
            "</SubAmt><VerAmt>" + txt80D9ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>110</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D10dec.Text + "</DecAmt><SubAmt>" + txt80D10sub.Text +
            "</SubAmt><VerAmt>" + txt80D10ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>111</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D11dec.Text + "</DecAmt><SubAmt>" + txt80D11sub.Text +
            "</SubAmt><VerAmt>" + txt80D11ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>112</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt80D12dec.Text + "</DecAmt><SubAmt>" + txt80D12sub.Text +
            "</SubAmt><VerAmt>" + txt80D12ver.Text + "</VerAmt></item>");

        _subhead = "";
        _subhead = ddlhrametro.SelectedItem.ToString();
        _subhead2 = "";
        _subhead2 = ddlhraNoonMonth.SelectedItem.ToString();
        _subhead3 = "";
        _subhead3 = txtLandlordName.Text;
        _subhead4 = "";
        _subhead4 = txtLandlordAddress.Text;
        _subhead5 = "";
        _subhead5 = txtLandlordPAN.Text;
        strdata.Write("<item><Head>201</Head><SubHead>" + _subhead + "</SubHead><SubHead2>" + _subhead2 + "</SubHead2><SubHead3>" + _subhead3 + "</SubHead3><SubHead4>" + _subhead4 + "</SubHead4><SubHead5>" + _subhead5 + "</SubHead5><SubHead6></SubHead6><DecAmt>" + txthradec.Text + "</DecAmt><SubAmt>" + txthrasub.Text +
            "</SubAmt><VerAmt>" + txthraver.Text + "</VerAmt></item>");

        strdata.Write("<item><Head>301</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtifos1dec.Text + "</DecAmt><SubAmt>" + txtifos1sub.Text +
            "</SubAmt><VerAmt>" + txtifos1ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>302</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtifos2dec.Text + "</DecAmt><SubAmt>" + txtifos2sub.Text +
            "</SubAmt><VerAmt>" + txtifos2ver.Text + "</VerAmt></item>");

        _subhead = "";
        if (rblsec101.SelectedIndex == 0) { _subhead = "0"; }
        else if (rblsec101.SelectedIndex == 1) { _subhead = "1"; }
        else if (rblsec101.SelectedIndex == 2) { _subhead = "2"; }
        strdata.Write("<item><Head>401</Head><SubHead>" + _subhead + "</SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtsec101dec.Text + "</DecAmt><SubAmt>" + txtsec101sub.Text +
            "</SubAmt><VerAmt>" + txtsec101ver.Text + "</VerAmt></item>");

        _subhead = "";
        if (rblsec102.SelectedIndex == 0) { _subhead = "0"; }
        else if (rblsec102.SelectedIndex == 1) { _subhead = "1"; }
        else if (rblsec102.SelectedIndex == 2) { _subhead = "2"; }
        strdata.Write("<item><Head>402</Head><SubHead>" + _subhead + "</SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtsec102dec.Text + "</DecAmt><SubAmt>" + txtsec102sub.Text +
            "</SubAmt><VerAmt>" + txtsec102ver.Text + "</VerAmt></item>");

        strdata.Write("<item><Head>403</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtsec103dec.Text + "</DecAmt><SubAmt>" + txtsec103sub.Text +
            "</SubAmt><VerAmt>" + txtsec103ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>404</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtsec104dec.Text + "</DecAmt><SubAmt>" + txtsec104sub.Text +
            "</SubAmt><VerAmt>" + txtsec104ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>405</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txtsec105dec.Text + "</DecAmt><SubAmt>" + txtsec105sub.Text +
            "</SubAmt><VerAmt>" + txtsec105ver.Text + "</VerAmt></item>");

        _subhead = "";
        _subhead2 = "";
        _subhead3 = "";
        _subhead3 = txtFinName05.Text;
        _subhead4 = "";
        _subhead4 = txtFinAdd05.Text;
        _subhead5 = "";
        _subhead5 = txtFinPAN05.Text;
        _subhead6 = "";
        _subhead6 = ddlFinType05.SelectedItem.ToString();
        strdata.Write("<item><Head>501</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3>" + _subhead3 + "</SubHead3><SubHead4>" + _subhead4 + "</SubHead4><SubHead5>" + _subhead5 + "</SubHead5><SubHead6>" + _subhead6 + "</SubHead6><DecAmt>" + txt05adec.Text + "</DecAmt><SubAmt>" + txt05asub.Text +
            "</SubAmt><VerAmt>" + txt05aver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>502</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05bdec.Text + "</DecAmt><SubAmt>" + txt05bsub.Text +
            "</SubAmt><VerAmt>" + txt05bver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>503</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05cdec.Text + "</DecAmt><SubAmt>" + txt05csub.Text +
            "</SubAmt><VerAmt>" + txt05cver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>504</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05ddec.Text + "</DecAmt><SubAmt>" + txt05dsub.Text +
            "</SubAmt><VerAmt>" + txt05dver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>505</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05edec.Text + "</DecAmt><SubAmt>" + txt05esub.Text +
            "</SubAmt><VerAmt>" + txt05ever.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>506</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05fdec.Text + "</DecAmt><SubAmt>" + txt05fsub.Text +
            "</SubAmt><VerAmt>" + txt05fver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>507</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt05gdec.Text + "</DecAmt><SubAmt>" + txt05gsub.Text +
            "</SubAmt><VerAmt>" + txt05gver.Text + "</VerAmt></item>");

        _subhead = "";
        _subhead2 = "";
        _subhead3 = "";
        _subhead3 = txtFinName.Text;
        _subhead4 = "";
        _subhead4 = txtFinAdd.Text;
        _subhead5 = "";
        _subhead5 = txtFinPAN.Text;
        _subhead6 = "";
        _subhead6 = ddlFinType.SelectedItem.ToString();
        strdata.Write("<item><Head>601</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3>" + _subhead3 + "</SubHead3><SubHead4>" + _subhead4 + "</SubHead4><SubHead5>" + _subhead5 + "</SubHead5><SubHead6>" + _subhead6 + "</SubHead6><DecAmt>" + txt061dec.Text + "</DecAmt><SubAmt>" + txt061sub.Text +
            "</SubAmt><VerAmt>" + txt061ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>602</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt062dec.Text + "</DecAmt><SubAmt>" + txt062sub.Text +
            "</SubAmt><VerAmt>" + txt062ver.Text + "</VerAmt></item>");

        strdata.Write("<item><Head>701</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt071dec.Text + "</DecAmt><SubAmt>" + txt071sub.Text +
            "</SubAmt><VerAmt>" + txt071ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>702</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt072dec.Text + "</DecAmt><SubAmt>" + txt072sub.Text +
            "</SubAmt><VerAmt>" + txt072ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>703</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt073dec.Text + "</DecAmt><SubAmt>" + txt073sub.Text +
            "</SubAmt><VerAmt>" + txt073ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>704</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt074dec.Text + "</DecAmt><SubAmt>" + txt074sub.Text +
            "</SubAmt><VerAmt>" + txt074ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>705</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt075dec.Text + "</DecAmt><SubAmt>" + txt075sub.Text +
            "</SubAmt><VerAmt>" + txt075ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>706</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt076dec.Text + "</DecAmt><SubAmt>" + txt076sub.Text +
            "</SubAmt><VerAmt>" + txt076ver.Text + "</VerAmt></item>");

        strdata.Write("<item><Head>708</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt078dec.Text + "</DecAmt><SubAmt>" + txt078sub.Text +
            "</SubAmt><VerAmt>" + txt078ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>709</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt079dec.Text + "</DecAmt><SubAmt>" + txt079sub.Text +
            "</SubAmt><VerAmt>" + txt079ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>710</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0710dec.Text + "</DecAmt><SubAmt>" + txt0710sub.Text +
            "</SubAmt><VerAmt>" + txt0710ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>711</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0711dec.Text + "</DecAmt><SubAmt>" + txt0711sub.Text +
            "</SubAmt><VerAmt>" + txt0711ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>712</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0712dec.Text + "</DecAmt><SubAmt>" + txt0712sub.Text +
            "</SubAmt><VerAmt>" + txt0712ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>713</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0713dec.Text + "</DecAmt><SubAmt>" + txt0713sub.Text +
            "</SubAmt><VerAmt>" + txt0713ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>714</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0714dec.Text + "</DecAmt><SubAmt>" + txt0714sub.Text +
            "</SubAmt><VerAmt>" + txt0714ver.Text + "</VerAmt></item>");
        strdata.Write("<item><Head>715</Head><SubHead></SubHead><SubHead2></SubHead2><SubHead3></SubHead3><SubHead4></SubHead4><SubHead5></SubHead5><SubHead6></SubHead6><DecAmt>" + txt0715dec.Text + "</DecAmt><SubAmt>" + txt0715sub.Text +
            "</SubAmt><VerAmt>" + txt0715ver.Text + "</VerAmt></item>");


        strdata.Write("</tds>");
        return strdata.ToString();
    }

    #region"Common Mail"
    public void MailOnSubmit(string MailTo)
    {
        string MailbodyUser = string.Empty;
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string subjectUser = string.Empty;
        string newUrl = string.Empty;
        TDS objtds = new TDS();

        DataTable DT_ApproInfo = objtds.ReturnApproverDetails(lblbu.Text);

        string Employeename = lblname.Text;
        string Approvername = DT_ApproInfo.Rows[0]["AName"].ToString();

        string comment = string.Empty;

        //newUrl = "http://10.201.1.97/TDS/AdminDashboard.aspx";
        newUrl = "http://localhost:51598/AdminDashboard.aspx";

        try
        {

            Mailbody = "<div style='width:100%;height:auto; font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'>" +
          "<table width='100%' cellspacing='0' cellpadding='0' style='font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'><tr><td align='left' valign='top'>Dear " + Approvername + ",<br/></td></tr><tr><td align='left' valign='top'>TDS Declaration of " + Employeename + " for Financial Year : <b>" + lblyear.Text + "</b> has been Submitted for your approval. <br /><br/></td></tr></table><br/>";
            Mailbody = Mailbody + "<table width='100%' cellspacing='0' cellpadding='0' style='font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'><tr><td align='left' valign='top'>&nbsp;</td></tr><tr><td align='left' valign='top'> <br /><a href=\"" + newUrl + "\">Click here to go to TDS home Page</a> </td><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>This is system generated message</span><br/></tr></table></div>";

            Subject = "TDS Declaration Approval Request ...";
            Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
        }
        catch (Exception ex)
        {
            // Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    public void MailOnVerify(string MailTo)
    {
        string MailbodyUser = string.Empty;
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string subjectUser = string.Empty;
        string newUrl = string.Empty;
        TDS objtds = new TDS();

        //DataTable DT_ApproInfo = objtds.ReturnApproverDetails(lblbu.Text);

        string Employeename = lblname.Text;
        //string Approvername = DT_ApproInfo.Rows[0]["AName"].ToString();

        string comment = string.Empty;

        //newUrl = "http://10.201.1.97/mycorner/taxandpay/SitePages/TDSdashbaord.aspx";
        newUrl = "http://localhost:51598/UserDashboard.aspx";

        try
        {

            Mailbody = "<div style='width:100%;height:auto; font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'>" +
          "<table width='100%' cellspacing='0' cellpadding='0' style='font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'><tr><td align='left' valign='top'>Dear " + Employeename + ",<br/></td></tr><tr><td align='left' valign='top'>Your TDS Declaration for Financial Year : <b>" + lblyear.Text + "</b> has been Verified Successfully. <br /><br/></td></tr></table><br/>";
            Mailbody = Mailbody + "<table width='100%' cellspacing='0' cellpadding='0' style='font-size:11px; font-family:Arial, Helvetica,Segoe UI; color:#333333;'><tr><td align='left' valign='top'>&nbsp;</td></tr><tr><td align='left' valign='top'> <br /><a href=\"" + newUrl + "\">Click here to go to TDS home Page</a> </td><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>This is system generated message</span><br/></tr></table></div>";

            Subject = "TDS Declaration Verification ...";
            Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
        }
        catch (Exception ex)
        {
            // Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion

    void CopySubmittedAmountInVerifyCol()
    {
        txt80D1ver.Text = txt80D1sub.Text;
        txt80D2ver.Text = txt80D2sub.Text;
        txt80D3ver.Text = txt80D3sub.Text;
        txt80D4ver.Text = txt80D4sub.Text;
        txt80D5ver.Text = txt80D5sub.Text;
        txt80D6ver.Text = txt80D6sub.Text;
        txt80D7ver.Text = txt80D7sub.Text;
        txt80D8ver.Text = txt80D8sub.Text;
        txt80D9ver.Text = txt80D9sub.Text;
        txt80D10ver.Text = txt80D10sub.Text;
        txt80D11ver.Text = txt80D11sub.Text;
        txthraver.Text = txthrasub.Text;
        txtifos1ver.Text = txtifos1sub.Text;
        txtifos2ver.Text = txtifos2sub.Text;
        txtsec101ver.Text = txtsec101sub.Text;
        txtsec102ver.Text = txtsec102sub.Text;
        txtsec103ver.Text = txtsec103sub.Text;
        txtsec104ver.Text = txtsec104sub.Text;
        txtsec105ver.Text = txtsec105sub.Text;
        txt05aver.Text = txt05asub.Text;
        txt05bver.Text = txt05bsub.Text;
        txt05cver.Text = txt05csub.Text;
        txt05dver.Text = txt05dsub.Text;
        txt05ever.Text = txt05esub.Text;
        txt05fver.Text = txt05fsub.Text;
        txt05gver.Text = txt05gsub.Text;
        txt061ver.Text = txt061sub.Text;
        txt062ver.Text = txt062sub.Text;
        txt071ver.Text = txt071sub.Text;
        txt072ver.Text = txt072sub.Text;
        txt073ver.Text = txt073sub.Text;
        txt074ver.Text = txt074sub.Text;
        txt075ver.Text = txt075sub.Text;
        txt076ver.Text = txt076sub.Text;
        txt078ver.Text = txt078sub.Text;
        txt079ver.Text = txt079sub.Text;
        txt0710ver.Text = txt0710sub.Text;
        txt0711ver.Text = txt0711sub.Text;
        txt0712ver.Text = txt0712sub.Text;
        txt0713ver.Text = txt0713sub.Text;
        txt0714ver.Text = txt0714sub.Text;
        txt0715ver.Text = txt0715sub.Text;
        txt80D1ver.Text = txt80D1sub.Text;
        txt80D2ver.Text = txt80D2sub.Text;
        txt80D3ver.Text = txt80D3sub.Text;
        txt80D4ver.Text = txt80D4sub.Text;
        txt80D5ver.Text = txt80D5sub.Text;
        txt80D6ver.Text = txt80D6sub.Text;
        txt80D7ver.Text = txt80D7sub.Text;
        txt80D8ver.Text = txt80D8sub.Text;
        txt80D9ver.Text = txt80D9sub.Text;
        txt80D10ver.Text = txt80D10sub.Text;
        txt80D11ver.Text = txt80D11sub.Text;
        txthraver.Text = txthrasub.Text;
        txtifos1ver.Text = txtifos1sub.Text;
        txtifos2ver.Text = txtifos2sub.Text;
        txtsec101ver.Text = txtsec101sub.Text;
        txtsec102ver.Text = txtsec102sub.Text;
        txtsec103ver.Text = txtsec103sub.Text;
        txtsec104ver.Text = txtsec104sub.Text;
        txtsec105ver.Text = txtsec105sub.Text;
        txt05aver.Text = txt05asub.Text;
        txt05bver.Text = txt05bsub.Text;
        txt05cver.Text = txt05csub.Text;
        txt05dver.Text = txt05dsub.Text;
        txt05ever.Text = txt05esub.Text;
        txt05fver.Text = txt05fsub.Text;
        txt05gver.Text = txt05gsub.Text;
        txt061ver.Text = txt061sub.Text;
        txt062ver.Text = txt062sub.Text;
        txt071ver.Text = txt071sub.Text;
        txt072ver.Text = txt072sub.Text;
        txt073ver.Text = txt073sub.Text;
        txt074ver.Text = txt074sub.Text;
        txt075ver.Text = txt075sub.Text;
        txt076ver.Text = txt076sub.Text;
        txt078ver.Text = txt078sub.Text;
        txt079ver.Text = txt079sub.Text;
        txt0710ver.Text = txt0710sub.Text;
        txt0711ver.Text = txt0711sub.Text;
        txt0712ver.Text = txt0712sub.Text;
        txt0713ver.Text = txt0713sub.Text;
        txt0714ver.Text = txt0714sub.Text;
        txt0715ver.Text = txt0715sub.Text;

    }

    void EnableVerifyCol(bool _enable)
    {
        txt80D1ver.Enabled = _enable;
        txt80D2ver.Enabled = _enable;
        txt80D3ver.Enabled = _enable;
        txt80D4ver.Enabled = _enable;
        txt80D5ver.Enabled = _enable;
        txt80D6ver.Enabled = _enable;
        txt80D7ver.Enabled = _enable;
        txt80D8ver.Enabled = _enable;
        txt80D9ver.Enabled = _enable;
        txt80D10ver.Enabled = _enable;
        txt80D11ver.Enabled = _enable;
        txt80D12ver.Enabled = _enable;
        txthraver.Enabled = _enable;
        txtifos1ver.Enabled = _enable;
        txtifos2ver.Enabled = _enable;
        txtsec101ver.Enabled = _enable;
        txtsec102ver.Enabled = _enable;
        txtsec103ver.Enabled = _enable;
        txtsec104ver.Enabled = _enable;
        txtsec105ver.Enabled = _enable;
        txt05aver.Enabled = _enable;
        txt05bver.Enabled = _enable;
        txt05cver.Enabled = _enable;
        txt05dver.Enabled = _enable;
        txt05ever.Enabled = _enable;
        txt05fver.Enabled = _enable;
        txt05gver.Enabled = _enable;
        txt061ver.Enabled = _enable;
        txt062ver.Enabled = _enable;
        txt071ver.Enabled = _enable;
        txt072ver.Enabled = _enable;
        txt073ver.Enabled = _enable;
        txt074ver.Enabled = _enable;
        txt075ver.Enabled = _enable;
        txt076ver.Enabled = _enable;
        txt078ver.Enabled = _enable;
        txt079ver.Enabled = _enable;
        txt0710ver.Enabled = _enable;
        txt0711ver.Enabled = _enable;
        txt0712ver.Enabled = _enable;
        txt0713ver.Enabled = _enable;
        txt0714ver.Enabled = _enable;
        txt0715ver.Enabled = _enable;
        txt80D1ver.Enabled = _enable;
        txt80D2ver.Enabled = _enable;
        txt80D3ver.Enabled = _enable;
        txt80D4ver.Enabled = _enable;
        txt80D5ver.Enabled = _enable;
        txt80D6ver.Enabled = _enable;
        txt80D7ver.Enabled = _enable;
        txt80D8ver.Enabled = _enable;
        txt80D9ver.Enabled = _enable;
        txt80D10ver.Enabled = _enable;
        txt80D11ver.Enabled = _enable;
        txthraver.Enabled = _enable;
        txtifos1ver.Enabled = _enable;
        txtifos2ver.Enabled = _enable;
        txtsec101ver.Enabled = _enable;
        txtsec102ver.Enabled = _enable;
        txtsec103ver.Enabled = _enable;
        txtsec104ver.Enabled = _enable;
        txtsec105ver.Enabled = _enable;
        txt05aver.Enabled = _enable;
        txt05bver.Enabled = _enable;
        txt05cver.Enabled = _enable;
        txt05dver.Enabled = _enable;
        txt05ever.Enabled = _enable;
        txt05fver.Enabled = _enable;
        txt05gver.Enabled = _enable;
        txt061ver.Enabled = _enable;
        txt062ver.Enabled = _enable;
        txt071ver.Enabled = _enable;
        txt072ver.Enabled = _enable;
        txt073ver.Enabled = _enable;
        txt074ver.Enabled = _enable;
        txt075ver.Enabled = _enable;
        txt076ver.Enabled = _enable;
        txt078ver.Enabled = _enable;
        txt079ver.Enabled = _enable;
        txt0710ver.Enabled = _enable;
        txt0711ver.Enabled = _enable;
        txt0712ver.Enabled = _enable;
        txt0713ver.Enabled = _enable;
        txt0714ver.Enabled = _enable;
        txt0715ver.Enabled = _enable;

    }

    bool txtCheckOnSubmit()
    {
        if (Convert.ToInt32(txt80D1dec.Text) > 0)
        { }

        txt80D1sub.Text = "";
        txt80D1ver.Text = "";

        txt80D2dec.Text = "";
        txt80D2sub.Text = "";
        txt80D2ver.Text = "";

        txt80D3dec.Text = "";
        txt80D3sub.Text = "";
        txt80D3ver.Text = "";

        txt80D4dec.Text = "";
        txt80D4sub.Text = "";
        txt80D4ver.Text = "";

        //rbl80D5.SelectedIndex
        txt80D5dec.Text = "";
        txt80D5sub.Text = "";
        txt80D5ver.Text = "";

        //rbl80D6.SelectedIndex
        txt80D6dec.Text = "";
        txt80D6sub.Text = "";
        txt80D6ver.Text = "";

        //rbl80D7.SelectedIndex
        txt80D7dec.Text = "";
        txt80D7sub.Text = "";
        txt80D7ver.Text = "";

        txt80D8dec.Text = "";
        txt80D8sub.Text = "";
        txt80D8ver.Text = "";

        txt80D9dec.Text = "";
        txt80D9sub.Text = "";
        txt80D9ver.Text = "";

        txt80D10dec.Text = "";
        txt80D10sub.Text = "";
        txt80D10ver.Text = "";

        txt80D11dec.Text = "";
        txt80D11sub.Text = "";
        txt80D11ver.Text = "";


        //ddlhrametro.SelectedItem.ToString();
        //ddlhraNoonMonth.SelectedItem.ToString();
        //txtLandlordName.Text;
        //txtLandlordAddress.Text;
        //txtLandlordPAN.Text;
        txthradec.Text = "";
        txthrasub.Text = "";
        txthraver.Text = "";

        txtifos1dec.Text = "";
        txtifos1sub.Text = "";
        txtifos1ver.Text = "";

        txtifos2dec.Text = "";
        txtifos2sub.Text = "";
        txtifos2ver.Text = "";

        //rblsec101.SelectedIndex
        txtsec101dec.Text = "";
        txtsec101sub.Text = "";
        txtsec101ver.Text = "";

        //rblsec102.SelectedIndex
        txtsec102dec.Text = "";
        txtsec102sub.Text = "";
        txtsec102ver.Text = "";

        txtsec103dec.Text = "";
        txtsec103sub.Text = "";
        txtsec103ver.Text = "";

        txtsec104dec.Text = "";
        txtsec104sub.Text = "";
        txtsec104ver.Text = "";

        txtsec105dec.Text = "";
        txtsec105sub.Text = "";
        txtsec105ver.Text = "";

        txt05adec.Text = "";
        txt05asub.Text = "";
        txt05aver.Text = "";

        txt05bdec.Text = "";
        txt05bsub.Text = "";
        txt05bver.Text = "";

        txt05cdec.Text = "";
        txt05csub.Text = "";
        txt05cver.Text = "";

        txt05ddec.Text = "";
        txt05dsub.Text = "";
        txt05dver.Text = "";

        txt05edec.Text = "";
        txt05esub.Text = "";
        txt05ever.Text = "";

        txt05fdec.Text = "";
        txt05fsub.Text = "";
        txt05fver.Text = "";

        txt05gdec.Text = "";
        txt05gsub.Text = "";
        txt05gver.Text = "";

        //txtFinName.Text;
        //txtFinAdd.Text;
        //txtFinPAN.Text;
        //ddlFinType.SelectedItem.ToString();
        txt061dec.Text = "";
        txt061sub.Text = "";
        txt061ver.Text = "";

        txt062dec.Text = "";
        txt062sub.Text = "";
        txt062ver.Text = "";

        txt071dec.Text = "";
        txt071sub.Text = "";
        txt071ver.Text = "";

        txt072dec.Text = "";
        txt072sub.Text = "";
        txt072ver.Text = "";

        txt073dec.Text = "";
        txt073sub.Text = "";
        txt073ver.Text = "";

        txt074dec.Text = "";
        txt074sub.Text = "";
        txt074ver.Text = "";

        txt075dec.Text = "";
        txt075sub.Text = "";
        txt075ver.Text = "";

        txt076dec.Text = "";
        txt076sub.Text = "";
        txt076ver.Text = "";

        txt078dec.Text = "";
        txt078sub.Text = "";
        txt078ver.Text = "";

        txt079dec.Text = "";
        txt079sub.Text = "";
        txt079ver.Text = "";

        txt0710dec.Text = "";
        txt0710sub.Text = "";
        txt0710ver.Text = "";

        txt0711dec.Text = "";
        txt0711sub.Text = "";
        txt0711ver.Text = "";

        txt0712dec.Text = "";
        txt0712sub.Text = "";
        txt0712ver.Text = "";

        txt0713dec.Text = "";
        txt0713sub.Text = "";
        txt0713ver.Text = "";

        txt0714dec.Text = "";
        txt0714sub.Text = "";
        txt0714ver.Text = "";

        txt0715dec.Text = "";
        txt0715sub.Text = "";
        txt0715ver.Text = "";

        return true;
    }

    bool IsNumeric(string text)
    {
        double test;
        return double.TryParse(text, out test);
    }


}
