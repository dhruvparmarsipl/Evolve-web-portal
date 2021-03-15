using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SharePoint;
public partial class ShowDetails : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    double amount = 0;
    double amount1 = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                string CycleId = Request.QueryString["RID"];
                FillHeader(CycleId);
                gvFPADistribution.Enabled = false;
                gvsalarydist.Enabled = false;
                bindHistory();
            }
        }
    }
    private string GetStatus(string status)
    {
        string cstatus = string.Empty;
        if (status == "0")
        {
            cstatus = "Draft";
        }
        if (status == "1")
        {
            cstatus = "Save as Draft";
        }
        if (status == "2")
        {
            cstatus = "Submitted to FI Admin";
        }
        if (status == "3")
        {
            cstatus = "Close";
        }
        if (status == "4")
        {
            cstatus = "Submitted to FPA Admin";
        }
        if (status == "5")
        {
            cstatus = "Rejected by FPA Admin";
        }
        if (status == "6")
        {
            cstatus = "Approve By FPA Admin";
        }
        if (status == "7")
        {
            cstatus = "Rejected by FIAdmin";
        }

        return cstatus;
    }
    private void FillHeader(string CycleId)
    {
        using (DataTable dt_UInfo = Dal.getFpaUserInfoclosedCycleId(CycleId))
        {
            if (dt_UInfo.Rows.Count > 0)
            {
                spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ENAME from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'"); 
                spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                // spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
                spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
                spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
                spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIADMIN"]);
                spnFPAAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FPAADMIN"]);
                spncreatedon.InnerText = Convert.ToString(dt_UInfo.Rows[0]["Date"]);
                HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
                HFDESID.Value = Convert.ToString(dt_UInfo.Rows[0]["des_id"]);
                HFCYCLEID.Value = Convert.ToString(CycleId);
                HFCSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["current_status"]);
                HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
                SpnStatus.InnerText = GetStatus(HFCSTATUS.Value);
				HFCARSCHEME.Value = Convert.ToString(dt_UInfo.Rows[0]["CAR_SCHEME"]);
                HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
				HFPOSITIONID.Value = Convert.ToString(dt_UInfo.Rows[0]["POSITIONID"]).Trim();
                if (Convert.ToInt32(HFDESID.Value) < 21)
                {
                    ltfpa.Visible = false;
                    txtFPA.Visible = false;
                    trgmctc.Visible = true;
                    trgm.Visible = true;
                    tr1.Visible = true;
                    ltctc.Visible = true;
                    txtCTC.Visible = true;
                    txtretrialbcomponent.Visible = true;
                    ltretcopm.Visible = true;
                    ltbasiccomp.Visible = true;
                    ltcbasic.Visible = true;
                    txtcBasic.Visible = true;
                    txtBasiccomponent.Visible = true;
                    trBasic.Visible = false;
                }
                else
                {
                    trgmctc.Visible = false;
                    tr1.Visible = false;
                    trgm.Visible = false;
                    ltctc.Visible = false;
                    txtCTC.Visible = false;
                    txtretrialbcomponent.Visible = false;
                    ltretcopm.Visible = false;
                    ltbasiccomp.Visible = false;
                    ltcbasic.Visible = false;
                    txtcBasic.Visible = false;
                    txtBasiccomponent.Visible = false;
                    ltfpa.Visible = true;
                    txtFPA.Visible = true;
                    trBasic.Visible = true;
                }


                 string CULoginSap = Dal.Get_Single_DataByPassingQuery("select SAPID from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'");
              if (dt_UInfo.Rows[0]["Fpaadminlogin"].ToString() == ReturnLoginName() || HFSAPID.Value == CULoginSap || dt_UInfo.Rows[0]["FIAdminLogin"].ToString() == ReturnLoginName())

                {
                   GetFpaDetail(HFCYCLEID.Value);
                   FillSalaryHeads();
                   FillFpaHeads();
                 }

                if (HFCSTATUS.Value == "1"||HFCSTATUS.Value == "3" || HFCSTATUS.Value == "0" || HFCSTATUS.Value=="5")
                {
                    SetIntialValue();
                    txtFPAcomments.Enabled = false;
                    txtIndiComments.Enabled = false;
                    txtReject.Enabled = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                }
                else if ( HFCSTATUS.Value == "4")
                {
				   if (HFPOSITIONID.Value == "99999999")
                    {
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('this employee are seperated from organization.so you can only view the form. ');", true);
                    }
                    else
                    {
                        btnApprove.Visible = true;
                        btnReject.Visible = true;
                    }
                    SetIntialValue();
                    txtFPAcomments.Enabled = false;
                    txtIndiComments.Enabled = false;
                    
                }
            }
            else
            {
                SetIntialValue();
                txtBasicRetrials.Enabled = false;
                txtBasiccomponent.Enabled = false;
                txtIndiComments.Enabled = false;
                txtFPAcomments.Enabled = false;
                txtReject.Enabled = false;
                //btnSubmit.Visible = false;
                //btnSaveDraft.Visible = false;
            }
        }
    }
    private void GetFpaDetail(string CycleId)
    {
        using (DataTable fpadetail = Dal.GetUserFpaDetailCloesdCycleId(CycleId))
        {
            if (fpadetail.Rows.Count > 0)
            {
                txtPranNo.Text = Convert.ToString(fpadetail.Rows[0]["PranNo"].ToString().Trim());
                // txtNominee.Text = Convert.ToString(fpadetail.Rows[0]["Nominee"].ToString().Trim());
                txtNPSValue.Text = Convert.ToString(fpadetail.Rows[0]["NPS"].ToString().Trim());
                txtAsBank.Text = Convert.ToString(fpadetail.Rows[0]["AssociatedBank"].ToString().Trim());

                txtCTC.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtFPA.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtcBasic.Text = Convert.ToString(fpadetail.Rows[0]["CURRENT_BASIC"].ToString().Trim());
                txtOther1.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_1"].ToString().Trim());
                txtOther2.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_2"].ToString().Trim());
                txtOther3.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_3"].ToString().Trim());
               // txtTotpcredited.Text = Convert.ToString(Dal.getTotalPterolAmountbycycle( CycleId)).Trim();
               //txtTotpcredited.Text = Convert.ToString(Dal.getTotalPterolAmount(HFSAPID.Value)).Trim();
                txtLocFPA.Text = Convert.ToString(fpadetail.Rows[0]["LOCATIONAL_FPA"].ToString().Trim());
                txtBasiccomponent.Text = Convert.ToString(fpadetail.Rows[0]["BASIC_COMPONENT"].ToString().Trim());
                txtTotalsalavailed.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_TAXABLE_AMT_AVAILED"].ToString().Trim());
                txtTotReimAvailed.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_REIMBURSEMENTS_AMT_AVAILED"].ToString().Trim());
                txtClaRental.Text = Convert.ToString(fpadetail.Rows[0]["CLA_RENTAL"].ToString().Trim());
                txtVehEmi.Text = Convert.ToString(fpadetail.Rows[0]["VEHICLE_EMI"].ToString().Trim());
                txtNPS.Text = Convert.ToString(fpadetail.Rows[0]["NPS"].ToString().Trim());
                txtBasicRetrials.Text = Convert.ToString(fpadetail.Rows[0]["basic_retirals_availed"].ToString().Trim());
                if (txtBasicRetrials.Text == "")
                {
                    txtBasicRetrials.Text = "0";
                }
                txtSAF.Text = Convert.ToString(fpadetail.Rows[0]["SCHOOL_SUBSIDY_DEDUCTION"].ToString().Trim());
                txtBusDeduct.Text = Convert.ToString(fpadetail.Rows[0]["BUS_DEDUCTION"].ToString().Trim());
                txtretrialbcomponent.Text = Convert.ToString(fpadetail.Rows[0]["RETIRAL_BENEFITS"].ToString().Trim());
                HFPAID.Value = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString().Trim());

  txtTotpcredited.Text = Convert.ToString(Dal.getTotalPterolAmount1(HFSAPID.Value, HFPAID.Value)).Trim();
  txtBasicSalary.Text = Convert.ToString(fpadetail.Rows[0]["BasicSalary"].ToString().Trim());
                HFID.Value = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString().Trim());
                ViewState["fpaid"] = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString().Trim());
                ViewState["srno"] = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString().Trim());
                txtFPAcomments.Text = Convert.ToString(fpadetail.Rows[0]["COMMENT"].ToString().Trim());
                txtReject.Text = Convert.ToString(fpadetail.Rows[0]["REASON_FOR_REJECTION"].ToString().Trim());
                txtIndiComments.Text = Convert.ToString(fpadetail.Rows[0]["UserComment"].ToString().Trim());
                if (Convert.ToInt32(HFDESID.Value) > 21)
                {
                    txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtFPA.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                         Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                    txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                         Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtNPS.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                    txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text));
                }
                else
                {
                    txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                         Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                    txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                        Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtretrialbcomponent.Text) +
                        Convert.ToDouble(txtBasiccomponent.Text) + Convert.ToDouble(txtBasicRetrials.Text) + Convert.ToDouble(txtNPS.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                    txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text));
                }
                if (Convert.ToInt32(txtNPSValue.Text) > 0)
                {
                    rdbNPSMode.SelectedValue = "1";
                    trFirstRow.Visible = true;
                    trThirdRow.Visible = true;
                }
                else
                {
                    rdbNPSMode.SelectedValue = "2";
                    trFirstRow.Visible = false;
                    trThirdRow.Visible = false;
                }
                SetIntialValue();
            }
            
        }
    }
    public void SetIntialValue()
    {
        txtcBasic.Enabled = false;
        txtCTC.Enabled = false;
        txtFPA.Enabled = false;
        txtLocFPA.Enabled = false;
        txtOther1.Enabled = false;
        txtOther2.Enabled = false;
        txtOther3.Enabled = false;
        txtTotAllownce.Enabled = false;
        txtTotalsalavailed.Enabled = false;
        txtTotDeduct.Enabled = false;
        txtTotpcredited.Enabled = false;
        txtTotReimAvailed.Enabled = false;
        txtVehEmi.Enabled = false;
        txtSAF.Enabled = false;
        txtNetAmount.Enabled = false;
        txtClaRental.Enabled = false;
        txtBusDeduct.Enabled = false;
        txtBasiccomponent.Enabled = false;
        txtretrialbcomponent.Enabled = false;
        txtBasicRetrials.Enabled = false;

    }
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    public string getSAPID(string Gid)
    {
        string SAPID = Dal.Get_Single_DataByPassingQuery("SELECT SAPID FROM MASTER_EMPLOYEE_PROFILE WHERE LOGIN_NAME ='" + Gid.Trim() + "'");
        return SAPID;
    }
    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "spstesttrg";
    }
    private void FillFpaHeads()
    {
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6"||HFCSTATUS.Value == "3")
        {
            if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6" || HFCSTATUS.Value == "3")
            {
                if (HFCSTATUS.Value == "1")
                {
                    using (DataTable __FpaheadsDraft = Dal.GetFpaheadsUserDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                    {
                        FpaHeadData(__FpaheadsDraft);
                        gvFPADistribution.Columns[3].Visible = false;
                        gvFPADistribution.Columns[4].Visible = false;
                    }
                }
 else if (HFCSTATUS.Value == "3")
                {
                    using (DataTable __FpaheadsDraft = Dal.GetFpaheadsClosed(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                    {
                        FpaHeadData(__FpaheadsDraft);
                    }
                }
                else
                {
                    using (DataTable __FpaheadsDraft = Dal.GetFpaheadsDraftDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                    {
                        FpaHeadData(__FpaheadsDraft);
                    }
                }
            }
        }
		 else
        {
            using (DataTable _Fpaheads = Dal.GetFpaHeads(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), HFCARSCHEME.Value, HFBU.Value))
            {
                if (_Fpaheads.Rows.Count > 0)
                {
                    gvFPADistribution.Columns[3].Visible = false;
                    gvFPADistribution.Columns[4].Visible = false;
                    gvFPADistribution.DataSource = _Fpaheads;
                    gvFPADistribution.DataBind();
                }
                else
                {
                    gvFPADistribution.DataBind();
                }
            }
        }
		
        
    }
    public void FpaHeadData(DataTable __FpaheadsDraft)
    {
        if (__FpaheadsDraft.Rows.Count > 0)
        {
            gvFPADistribution.DataSource = __FpaheadsDraft;
            gvFPADistribution.DataBind();
            object sumObject, sumalredy, sumbalance;
            sumObject = __FpaheadsDraft.Compute("Sum(AMOUNT)", "");
            sumalredy = __FpaheadsDraft.Compute("Sum(Alredyclaimed)", "");
            sumbalance = __FpaheadsDraft.Compute("Sum(Balance)", "");
            TextBox txtTotalfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
            Label lblAlCliam = (Label)gvFPADistribution.FooterRow.FindControl("lblAlCliam");
            Label lblBalance = (Label)gvFPADistribution.FooterRow.FindControl("lblBalance");
            txtTotalfpaamount.Text = sumObject.ToString().Trim();
            txtTotalfpaamount.Enabled = false;
            lblAlCliam.Text = sumalredy.ToString();
            lblBalance.Text = sumbalance.ToString();
            if (txtYAllocate.Text == "")
            {
                txtYAllocate.Text = "0";
            }
            txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtYAllocate.Text) - Convert.ToDouble(txtTotalfpaamount.Text)).Trim();
        }
        else
        {
            gvFPADistribution.DataBind();
        }
    }
    public void SalaryHeadData(DataTable _SalheadsDraft)
    {
        if (_SalheadsDraft.Rows.Count > 0)
        {
            gvsalarydist.DataSource = _SalheadsDraft;
            gvsalarydist.DataBind();
            object sumObject;
            sumObject = _SalheadsDraft.Compute("Sum(AMOUNT)", "");

            TextBox txtTotalSalamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
            txtTotalSalamount.Text = sumObject.ToString();
            txtTotalSalamount.Enabled = false;
            if (txtYAllocate.Text == "")
            {
                txtYAllocate.Text = "0";
            }
            txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtYAllocate.Text) - Convert.ToDouble(txtTotalSalamount.Text));

        }
        else
        {
            gvsalarydist.DataBind();
        }
    }
    private void FillSalaryHeads()
    {
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6" || HFCSTATUS.Value == "3")
        {
            if (HFCSTATUS.Value == "1")
            {
                using (DataTable _SalheadsDraft = Dal.GetSalaryHeadsDraftUser(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                {
                    SalaryHeadData(_SalheadsDraft);
                }
            }
else if (HFCSTATUS.Value == "3")
            {
                using (DataTable _SalheadsDraft = Dal.GetSalaryClosed(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                {
                    SalaryHeadData(_SalheadsDraft);
                }
            }
            else
            {
                using (DataTable _SalheadsDraft = Dal.GetSalaryHeadsDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                {
                    SalaryHeadData(_SalheadsDraft);
                }
            }

        }
        else
        {
            using (DataTable _Salheads = Dal.GetSalaryHeads(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value)))
            {
                if (_Salheads.Rows.Count > 0)
                {
                    gvsalarydist.DataSource = _Salheads;
                    gvsalarydist.DataBind();
                }
                else
                {
                    gvsalarydist.DataBind();
                }
            }
        }
       

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            string newUrl = string.Empty;
            string EmailID = string.Empty;
            if (HFEMAILS.Value.Trim() == "")
            {
                EmailID = "";
            }
            else { EmailID = HFEMAILS.Value.ToString().Split('#')[0]; }
            //int TransID = 0;
            if (Convert.ToDouble(txtYAllocate.Text) == 0)
            {
                int TransID = Dal.InsertMyApprovaltoFPA(Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "3");
                if (TransID == 1)
                {
                    int year = DateTime.Now.Year;
					
					if (HFBU.Value == "REM")
                    {
                       newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
                    }
                    else
                      {
                            newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                      }
					
                    string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnFormCreater.InnerText + ",<br/><br/>Your FPA has been closed by FPA Admin for Year " + year + ".<br /><br/><br/>Thanks,<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message.</span></div></div></td></tr></table>";
                    //string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnFormCreater.InnerText + "<br/><br/>Your FPA has been closed by FPA Admin for Year " + year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                    Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", "", "FPA Intiation Request Approval.....", Mailbody);
					Dal.LogSummary("Accepted", spnLoginUser.InnerText.ToString(), txtFPAcomments.Text, HFCYCLEID.Value.ToString());
                    Dal.CreateLogHistory("3", spnLoginUser.InnerText.ToString(), txtFPAcomments.Text, HFID.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('" + spnFormCreater.InnerText + " FPA  Approved successfully.');CloseRefreshWindow();", true);

                }
                else if (TransID == 0)
                {
                    //btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                }
                else if (TransID == 2)
                {
                    //btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                }
                else if (TransID == 5)
                {
                    //btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data Already Submitted.');CloseRefreshWindow();", true);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            string newUrl = string.Empty;
            string EmailID = string.Empty;
            if (HFEMAILS.Value.Trim() == "")
            {
                EmailID = "";
            }
            else { EmailID = HFEMAILS.Value.ToString().Split('#')[0]; }
            if (Convert.ToDouble(txtYAllocate.Text) == 0)
            {
                if (txtReject.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Enter Rejection comment.');", true);
                }
                else
                {
                    if (txtReject.Text.Length <= 500)
                    {
                        txtReject.Text = Convert.ToString(txtReject.Text);
                    }
                    else
                    {
                        string st =Convert.ToString(txtReject.Text);
                        txtReject.Text = st.Substring(0, 500).Substring(0,st.LastIndexOf(' '))+"..";
                    }
                    int TransID = Dal.UpdateRejectionByFPA(Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "5", txtReject.Text);
                    if (TransID == 1)
                    {
                        int year = DateTime.Now.Year;
                        if (HFBU.Value == "REM")
                        {
                            newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
                        }
                        else
                        {
                            newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                        }
                        string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnFormCreater.InnerText + "<br/><br/>Your FPA has been rejected by FPA Admin  for Financial Year " + year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                        Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", "", "FPA Intiation Request Rejected.....", Mailbody);
                        Dal.CreateLogHistory("5", spnLoginUser.InnerText.ToString(), txtReject.Text, HFID.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('" + spnFormCreater.InnerText + " FPA is rejected');CloseRefreshWindow();", true);

                    }
                    else if (TransID == 0)
                    {
                        //btnIntiation.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                    }
                    else if (TransID == 2)
                    {
                        //btnIntiation.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                    }
                    else if (TransID == 5)
                    {
                        //btnIntiation.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data Already Submitted.');CloseRefreshWindow();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
        }
    }
    public void bindHistory()
    {
        string Srno = Convert.ToString(HFID.Value);
        int i = 0;
        DataTable dt_his = Dal.getHistorybySRNO(Srno);
        if (dt_his.Rows.Count > 0)
        {
            grdHistory.DataSource = dt_his;
            grdHistory.DataBind();
        }
        else
        {
            grdHistory.Visible = false;
        }
    }
   
}
