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
using System.Linq;

public partial class MyFPAdeclration : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfNPSAmt.Value = "0";
            FillHeader(getSAPID(ReturnLoginName()));
            bindHistory();
        }
    }
    private string GetStatus(string status)
    {
        string cstatus = string.Empty;
        if (status == "0")
        {
            cstatus = "Draft ";
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
    private void FillHeader(string sapid)
    
    {
        using (DataTable dt_UInfo = Dal.getCurrentCycleUser(sapid))
        {
            if (dt_UInfo.Rows.Count > 0)
            {
                spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                // spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
                spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
                spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
               // spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIADMIN"]);
                spnFPAAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FPAADMIN"]);
                spncreatedon.InnerText = Convert.ToString(dt_UInfo.Rows[0]["Date"]);
                HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
                HFDESID.Value = Convert.ToString(dt_UInfo.Rows[0]["des_id"]);
                HFCYCLEID.Value = Convert.ToString(dt_UInfo.Rows[0]["fpa_cycle_id"]);
                HFCSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["current_status"]);
                HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["FpaEmail"]);
                SpnStatus.InnerText = GetStatus(HFCSTATUS.Value);
                HFCARSCHEME.Value = Convert.ToString(dt_UInfo.Rows[0]["CAR_SCHEME"]);
                HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
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

                GetFpaDetail(HFCYCLEID.Value);
                FillSalaryHeads();
                FillFpaHeads();
               // btnSubmit.Visible = true;
                //btnSaveDraft.Visible = true;
            }
            else
            {
                SetIntialValue();
                txtBasicRetrials.Enabled = false;
                txtBasiccomponent.Enabled = false;
                txtIndiComments.Enabled = false;
                txtFPAcomments.Enabled = false;
                btnSubmit.Visible = false;
                btnSaveDraft.Visible = false;
            }
        }
    }
    private void GetFpaDetail(string cycleid)
    {
        using (DataTable fpadetail = Dal.GetUserFpaDetail(cycleid))
        {
            if (fpadetail.Rows.Count > 0)
            {

                // NPS Detail

                txtPranNo.Text = Convert.ToString(fpadetail.Rows[0]["PranNo"].ToString().Trim());
               // txtNominee.Text = Convert.ToString(fpadetail.Rows[0]["Nominee"].ToString().Trim());
                txtNPSValue.Text = Convert.ToString(fpadetail.Rows[0]["NPS"].ToString().Trim());
                
                if (txtNPSValue.Text=="0")
                {
                    txtNPSValue.Text = "";
                }
                txtAsBank.Text = Convert.ToString(fpadetail.Rows[0]["AssociatedBank"].ToString().Trim());
               // ddlRelationShip.SelectedValue = Convert.ToString(fpadetail.Rows[0]["Relationship"].ToString().Trim());

                txtCTC.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtFPA.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtcBasic.Text = Convert.ToString(fpadetail.Rows[0]["CURRENT_BASIC"].ToString().Trim());
                txtOther1.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_1"].ToString().Trim());
                txtOther2.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_2"].ToString().Trim());
                txtOther3.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_3"].ToString().Trim());
               // txtTotpcredited.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_PETROL_AMT_CREDITED"].ToString().Trim());

                txtTotpcredited.Text =Convert.ToString (Dal.getTotalPterolAmount(HFSAPID.Value)).Trim();
                txtLocFPA.Text = Convert.ToString(fpadetail.Rows[0]["LOCATIONAL_FPA"].ToString().Trim());
                txtBasiccomponent.Text = Convert.ToString(fpadetail.Rows[0]["BASIC_COMPONENT"].ToString().Trim());
                 if (txtBasiccomponent.Text == "")
                {
                    txtBasiccomponent.Text="0";
                }
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
                if (txtretrialbcomponent.Text == "")
                {
                    txtretrialbcomponent.Text = "0";
                }
                txtBasicSalary.Text = Convert.ToString(fpadetail.Rows[0]["BasicSalary"].ToString().Trim());
                HFPAID.Value = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString().Trim());
                HFID.Value = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString().Trim());
                ViewState["fpaid"] = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString().Trim());
                ViewState["srno"] = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString().Trim());
                txtFPAcomments.Text = Convert.ToString(fpadetail.Rows[0]["COMMENT"].ToString().Trim());
                txtIndiComments.Text = Convert.ToString(fpadetail.Rows[0]["UserComment"].ToString().Trim());
				 txtReject.Text = Convert.ToString(fpadetail.Rows[0]["REASON_FOR_REJECTION"].ToString().Trim());
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
                SetIntialValue();
            }
            if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "0" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7")
            {
                btnSaveDraft.Visible = true;
                btnSubmit.Visible = true;
                txtFPAcomments.Enabled = false;
                txtIndiComments.Enabled = true;
                setNps(HFCSTATUS.Value);

                // for NPS disable 
                var vNPS = 0;
                if(string.IsNullOrWhiteSpace(txtNPS.Text))
                {
                    vNPS = 0;
                }
                else
                {
                    vNPS = Convert.ToInt32(txtNPS.Text);
                }
                if (vNPS > 0)
                {
                    rdbNPSMode.SelectedValue = "1";
                    rdbNPSMode.Enabled = false;
                    rdbNPSMode_SelectedIndexChanged(null, null);
                }
                hfNPSAmt.Value = Convert.ToString(vNPS);
                   
            }
            else
            {
                btnSaveDraft.Visible = false;
                txtFPAcomments.Enabled = false;
                txtIndiComments.Enabled = false;
                btnSubmit.Visible = false;
                gvFPADistribution.Enabled = false;
                gvsalarydist.Enabled = false;
                txtBasiccomponent.Enabled = false;
                txtBasicRetrials.Enabled = false;
                txtretrialbcomponent.Enabled = false;
                setNps(HFCSTATUS.Value);
                
                rdbNPSMode.Enabled = false;
                txtAsBank.Enabled = false;
               // ddlAsBank.Enabled = false;
                ddlRelationShip.Enabled = false;
                txtNominee.Enabled = false;
                txtNPSValue.Enabled = false;
                txtPranNo.Enabled = false;
            }
        }
    }

     public void setNps(string Status)
    {
        if (Status == "0")
        {
            trFirstRow.Visible = false;
            trThirdRow.Visible = false;
        }
        else
        {
           // int val = txtNPSValue.Text;
            if (txtNPSValue.Text == "")
            {
                txtNPSValue.Text = "0";
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
            txtNPSValue.Text = "";
        }
    }
    public void SetIntialValue()
    {
        txtcBasic.Enabled = false;
        txtCTC.Enabled = false;
        txtFPA.Enabled = false;
        txtBasicRetrials.Enabled = false;
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
        // txtBasiccomponent.Enabled = false;
        //txtretrialbcomponent.Enabled = false;

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
   
     private void FillFpaHeads()
    {
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6"|| HFCSTATUS.Value == "3")
        {
            if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "5")
            {
                using (DataTable __FpaheadsDraft = Dal.GetFpaheadsUserDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
                {
                    FpaHeadData(__FpaheadsDraft);
                    gvFPADistribution.Columns[3].Visible = false;
                    gvFPADistribution.Columns[4].Visible = false;
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
        
        else
        {
            using (DataTable _Fpaheads = Dal.GetFpaHeads(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), HFCARSCHEME.Value,HFBU.Value))
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
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6"||HFCSTATUS.Value == "3")
        {
            if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6" || HFCSTATUS.Value == "3")
            {
                if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "5")
                {
                    using (DataTable _SalheadsDraft = Dal.GetSalaryHeadsDraftUser(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
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
      public void txtBasiccomponent_onchange(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtYAllocate.Text) > 0)
        {
            double retrialbcomponent = 0, BasicComponent = 0, ctcAmount = 0, annualBasic = 0;
            if (txtBasiccomponent.Text == "")
            {
                txtBasiccomponent.Text = "0";
            }
            BasicComponent = Convert.ToDouble(txtBasiccomponent.Text);
            ctcAmount = (Convert.ToDouble(txtCTC.Text) * 25) / 100;

            if (txtNPS.Text == "")
            {
                txtNPS.Text = "0";
            }
            if (txtcBasic.Text == "")
            {
                annualBasic = 0;
            }
            else
            {
                annualBasic = Convert.ToDouble(txtcBasic.Text);
            }
           // change date 28-08-2014
            //By Bipin 

            if (annualBasic > ctcAmount)
            {
                if (Convert.ToDouble(txtBasiccomponent.Text) > annualBasic)
                {
                    txtBasiccomponent.Text = "0";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "basic", "alert('Basic component value can not greater than annual basic');", true);
                }
                else
                {
                    BasicComponent = Convert.ToDouble(txtBasiccomponent.Text);
                }  
            }
            else
            {
                if (Convert.ToDouble(txtBasiccomponent.Text) > ctcAmount)
                {
                    txtBasiccomponent.Text = "0";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "basic11", "alert('Basic component value can not greater than 25% of CTC');", true);
                }
                else
                {
                    BasicComponent = Convert.ToDouble(txtBasiccomponent.Text);
                }  
            }
            
                retrialbcomponent = Math.Round(((Convert.ToDouble(txtBasiccomponent.Text) * 16.81) / 100), 0);
                if (retrialbcomponent == 0)
                {
                    txtretrialbcomponent.Text = "0";
                }
                else
                {
                    txtretrialbcomponent.Text = Convert.ToString(retrialbcomponent);
                }
                txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                            Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                    Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtretrialbcomponent.Text) +
                    Convert.ToDouble(BasicComponent) + Convert.ToDouble(txtBasicRetrials.Text) + Convert.ToDouble(txtNPS.Text));
                txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));


                string SalAmount = string.Empty;
                string FpaAmount = string.Empty;
                if (gvsalarydist.Rows.Count > 0)
                {
                    TextBox Totallsaamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
                    SalAmount = Totallsaamount.Text;
                }
                else
                {
                    SalAmount = "0";
                }
                if (gvFPADistribution.Rows.Count > 0)
                {
                    TextBox Totallfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
                    FpaAmount = Totallfpaamount.Text;
                }
                else
                {
                    FpaAmount = "0";
                }
                if (SalAmount == "")
                {
                    SalAmount = "0";
                }

                if (FpaAmount == "")
                {
                    FpaAmount = "0";
                }
              
                txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - (Convert.ToDouble(txtTotDeduct.Text) + Convert.ToDouble(SalAmount) + Convert.ToDouble(FpaAmount)));
            
        }
        else
        {
            txtBasiccomponent.Text = "0";
            txtretrialbcomponent.Text = "0";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "key", "alert('Yet To Allocate Amount is zero');", true);
        }
    }
    protected void txtBasicRetrials_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtYAllocate.Text) > 0)
        {
            if (txtBasicRetrials.Text == "")
            {
                txtBasicRetrials.Text = "0";
            }
            if (txtNPS.Text == "")
            {
                txtNPS.Text = "0";
            }
            txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                            Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
            txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtretrialbcomponent.Text) +
                Convert.ToDouble(txtBasiccomponent.Text) + Convert.ToDouble(txtBasicRetrials.Text) + Convert.ToDouble(txtNPS.Text));
            txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
            string SalAmount= string.Empty;
            string FpaAmount=string.Empty;
            if (gvsalarydist.Rows.Count > 0)
            {
                TextBox Totallsaamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
                SalAmount = Totallsaamount.Text;
            }
            else
            {
                SalAmount = "0";
            }
            if (gvFPADistribution.Rows.Count > 0)
            {
                TextBox Totallfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
                FpaAmount = Totallfpaamount.Text;
            }
            else
            {
                FpaAmount = "0";
            }


            if (SalAmount== "")
            {
                SalAmount = "0";
            }

            if (FpaAmount == "")
            {
                FpaAmount = "0";
            }
            txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - (Convert.ToDouble(txtTotDeduct.Text) + Convert.ToDouble(SalAmount) + Convert.ToDouble(FpaAmount)));
        }
        else
        {
            txtBasicRetrials.Text = "0";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "key", "alert('Yet To Allocate Amount is zero');", true);
        }
    }
     protected void txtFPAAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double amount = 0;
            double amount1 = 0;
            string ToatlaSalAmount = string.Empty;
            TextBox txt = (sender as TextBox);
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            TextBox Totallfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
            if (gvsalarydist.Rows.Count > 0)
            {
                TextBox Totallsaamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
                ToatlaSalAmount = Totallsaamount.Text;
            }
            else { ToatlaSalAmount = "0"; }

          
            TextBox fpaamount = (TextBox)gvrow.FindControl("txtFPAAmount");
            if (fpaamount.Text == "")
            { fpaamount.Text = "0"; }

            bool fpalblmorethan1600cc = false, fpalblbelow1600cc = false;

            foreach (GridViewRow grow in gvFPADistribution.Rows)
            {
                TextBox fpatototalamount = (TextBox)grow.FindControl("txtFPAAmount");
                Label fpalabel = (Label)grow.FindControl("lblFpaid");
                if (fpalabel.Text == "61")
                {
                    if (fpatototalamount.Text != "" && Convert.ToDouble(fpatototalamount.Text) > 0)
                    {
                        fpalblbelow1600cc = true;
                    }
                    if (fpatototalamount.Text != "" && Convert.ToDouble(fpatototalamount.Text) > 21600)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Monthly limit for Fuel & Oil up to 1600 CC is INR 1800. Please fill annual limit accordingly.')", true);
                    }
                }
                if (fpalabel.Text == "62")
                {
                    if (fpatototalamount.Text != "" && Convert.ToDouble(fpatototalamount.Text) > 0)
                    {
                        fpalblmorethan1600cc = true;
                    }
                    if (fpatototalamount.Text != "" && Convert.ToDouble(fpatototalamount.Text) > 28800)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Monthly limit for Fuel & Oil more than 1600 CC is INR 2400. Please fill annual limit accordingly.')", true);
                    }
                }
                if (fpatototalamount.Text == "")
                { }
                else
                {
                    amount1 += Convert.ToDouble(fpatototalamount.Text);
                    Totallfpaamount.Text = Convert.ToString(Convert.ToDouble(amount1));
                }
            }
            if (fpalblmorethan1600cc == true && fpalblbelow1600cc == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Only one head for Fuel and Oil can be assigned.')", true);
            }
            amount = Convert.ToDouble(fpaamount.Text);
            if (ToatlaSalAmount == "")
            {
                ToatlaSalAmount = "0";
            }
            txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text) - (Convert.ToDouble(Totallfpaamount.Text) + Convert.ToDouble(ToatlaSalAmount)));
            if (Convert.ToDouble(txtYAllocate.Text) < 0)
            {
                fpaamount.Text = "0";
                // txtYAllocate.Text = "Distribution is not allow";
                Totallfpaamount.Text = Convert.ToString(Convert.ToDouble(Totallfpaamount.Text) - amount);
                txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text) - (Convert.ToDouble(Totallfpaamount.Text) + Convert.ToDouble(ToatlaSalAmount)));
				                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Amount is exceed than yet to allocate amount')", true);

            }
            if (Convert.ToDouble(txtYAllocate.Text) == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Please Submit FPA')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void txtSalAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double amount = 0;
            double amount1 = 0;
            TextBox txt = (sender as TextBox);
            string TotalFpaAmount = string.Empty; 
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            TextBox salamount = (TextBox)gvrow.FindControl("txtSalAmount");
            if (salamount.Text == "")
            { salamount.Text = "0"; }
            TextBox Totallsaamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
            if (gvFPADistribution.Rows.Count > 0)
            {
                TextBox Totallfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
                TotalFpaAmount = Totallfpaamount.Text;
            }
            else { TotalFpaAmount = "0"; }
            foreach (GridViewRow grow in gvsalarydist.Rows)
            {
                TextBox salaryamount = (TextBox)grow.FindControl("txtSalAmount");
                if (salaryamount.Text == "")
                { }
                else
                {
                    amount1 += Convert.ToDouble(salaryamount.Text);

                    Totallsaamount.Text = Convert.ToString(Convert.ToDouble(amount1));

                }
            }
            amount = Convert.ToDouble(salamount.Text);
            if (TotalFpaAmount == "")
            {
                TotalFpaAmount = "0";
            }

            txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text) - (Convert.ToDouble(Totallsaamount.Text) + Convert.ToDouble(TotalFpaAmount)));
            if (Convert.ToDouble(txtYAllocate.Text) < 0)
            {
                salamount.Text = "0";
                // txtYAllocate.Text = "Distribution is not allow";
                Totallsaamount.Text = Convert.ToString(Convert.ToDouble(Totallsaamount.Text) - amount);
                txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text) - (Convert.ToDouble(Totallsaamount.Text) + Convert.ToDouble(TotalFpaAmount)));
				                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key121", "alert('Amount is exceed than yet to allocate amount')", true);

            }
            if (Convert.ToDouble(txtYAllocate.Text) == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "keySalAMT", "alert('Please Submit FPA')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (rdbNPSMode.SelectedValue == "1" || rdbNPSMode.SelectedValue == "2")
        {
            try
            {
                string EmailID = string.Empty;
                string newUrl = string.Empty;
                if (HFEMAILS.Value.Trim() == "")
                {
                    EmailID = "";
                }
                else
                {
                    EmailID = HFEMAILS.Value.ToString().Split('#')[2];
                }

                if (txtIndiComments.Text.Length <= 250)
                {
                    txtIndiComments.Text = Convert.ToString(txtIndiComments.Text);
                }
                else
                {
                    txtIndiComments.Text = Convert.ToString(txtIndiComments.Text).Substring(0, 250);
                }
                if (txtIndiComments.Text.Length <= 250)
                {
                    txtIndiComments.Text = Convert.ToString(txtIndiComments.Text);
                }
                else
                {
                    txtIndiComments.Text = Convert.ToString(txtIndiComments.Text).Substring(0, 250);
                }
                if ((Convert.ToDouble(txtYAllocate.Text) == 0) || (Convert.ToDouble(txtYAllocate.Text) == 0.0))
                //if (Convert.ToDouble(txtYAllocate.Text) == 0)
                {
                    //Salary Head Distribution 
                    string gridSalaryHeadXml = string.Empty;
                    string gridFPAHeadXml = string.Empty;
                    StringWriter strSalHeadWriter = new StringWriter();
                    StringWriter strFpaHeadWriter = new StringWriter();

                    DataTable _saldt = new DataTable("SalaryHead");
                    _saldt.Columns.Add("FpaCycleId", typeof(string));
                    _saldt.Columns.Add("FpaSalHeadId", typeof(string));
                    _saldt.Columns.Add("SalAmount", typeof(double));
                    if (gvsalarydist.Rows.Count > 0)
                    {
                        foreach (GridViewRow grow in gvsalarydist.Rows)
                        {
                            DataRow _salrow = _saldt.NewRow();
                            Label FpaSalHeadId = (Label)grow.FindControl("lblSalHeadId");
                            TextBox SalAmount = (TextBox)grow.FindControl("txtSalAmount");
                            if (SalAmount.Text == "")
                            {
                                SalAmount.Text = "0";
                            }
                            _salrow["FpaCycleId"] = HFCYCLEID.Value;
                            _salrow["FpaSalHeadId"] = FpaSalHeadId.Text;
                            _salrow["SalAmount"] = Convert.ToDouble(SalAmount.Text);
                            _saldt.Rows.Add(_salrow);
                        }
                        _saldt.WriteXml(strSalHeadWriter, XmlWriteMode.IgnoreSchema);
                        gridSalaryHeadXml = strSalHeadWriter.ToString();
                    }

                    //FPA Head Distribution

                    bool fpalblmorethan1600cc = false, fpalblbelow1600cc = false;

                    DataTable _Fpadt = new DataTable("FpaHead");
                    _Fpadt.Columns.Add("FpaCycleId", typeof(string));
                    _Fpadt.Columns.Add("FpaHeadId", typeof(string));
                    _Fpadt.Columns.Add("FpaAmount", typeof(double));
                    _Fpadt.Columns.Add("FpaBalanceAmount", typeof(double));
                    if (gvFPADistribution.Rows.Count > 0)
                    {
                        foreach (GridViewRow grow in gvFPADistribution.Rows)
                        {
                            DataRow _fparow = _Fpadt.NewRow();
                            Label FpaHeadId = (Label)grow.FindControl("lblFpaid");
                            TextBox FpaAmount = (TextBox)grow.FindControl("txtFPAAmount");
                            if (FpaAmount.Text == "")
                            {
                                FpaAmount.Text = "0";
                            }
                            _fparow["FpaCycleId"] = HFCYCLEID.Value;
                            _fparow["FpaHeadId"] = FpaHeadId.Text;
                            _fparow["FpaAmount"] = Convert.ToDouble(FpaAmount.Text);
                            _fparow["FpaBalanceAmount"] = Convert.ToDouble(FpaAmount.Text);
                            _Fpadt.Rows.Add(_fparow);

                            if (FpaHeadId.Text == "61")
                            {
                                if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 0)
                                {
                                    fpalblbelow1600cc = true;
                                }
                                if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 21600)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Monthly limit for Fuel & Oil up to 1600 CC is INR 1800. Please fill annual limit accordingly.');", true);
                                    return;
                                }
                            }
                            if (FpaHeadId.Text == "62")
                            {
                                if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 0)
                                {
                                    fpalblmorethan1600cc = true;
                                }
                                if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 28800)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Monthly limit for Fuel & Oil more than 1600 CC is INR 2400. Please fill annual limit accordingly.');", true);
                                    return;
                                }
                            }
                        }
                        _Fpadt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
                        gridFPAHeadXml = strFpaHeadWriter.ToString();

                        if (fpalblbelow1600cc == true && fpalblmorethan1600cc == true)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Only one head for Fuel and Oil can be assigned.');", true);
                            return;
                        }
                    }



                    int TransID = 0;
                    double Basiccomp = 0, retrialbcomp = 0, basicretrial = 0,NPS=0;
                    if (txtretrialbcomponent.Text == "")
                    {
                        retrialbcomp = 0;
                    }
                    else
                    {
                        retrialbcomp = Convert.ToDouble(txtretrialbcomponent.Text);
                    }
                    if (txtBasiccomponent.Text == "")
                    {
                        Basiccomp = 0;
                    }
                    else
                    {
                        Basiccomp = Convert.ToDouble(txtBasiccomponent.Text);
                    }
                    if (txtBasicRetrials.Text == "")
                    {
                        basicretrial = 0;
                    }
                    else
                    {
                        basicretrial = Convert.ToDouble(txtBasicRetrials.Text);
                    }

                    if (txtNPS.Text == "")
                    {
                        NPS = 0;
                    }
                    else
                    {
                        NPS = Convert.ToDouble(txtNPS.Text);
                    }
                    if (string.IsNullOrEmpty(gridSalaryHeadXml) || string.IsNullOrEmpty(gridFPAHeadXml))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You have not declare your FPA.');", true);
                    }
                    else
                    {

                        if (Convert.ToInt32(HFDESID.Value) < 21)
                        {
                            if (Basiccomp > 0)
                            {
                                TransID = Dal.InsertMyApprovaltoFPAForGM(gridSalaryHeadXml, gridFPAHeadXml, Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "4", getSAPID(ReturnLoginName()), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), Convert.ToInt32(basicretrial), txtIndiComments.Text, Convert.ToInt32(NPS), txtPranNo.Text, txtNominee.Text, txtAsBank.Text, ddlRelationShip.SelectedValue.ToString());
                            }
                        }
                        else
                        {
                            TransID = Dal.InsertMyApprovaltoFPA(gridSalaryHeadXml, gridFPAHeadXml, Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "4", getSAPID(ReturnLoginName()), txtIndiComments.Text, Convert.ToInt32(NPS), txtPranNo.Text, txtNominee.Text, txtAsBank.Text.ToString(), ddlRelationShip.SelectedValue.ToString());
                        }
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


                            string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnLoginUser.InnerText.ToString() + "<br/><br/>Your FPA has been initiated for Financial Year " + year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                            Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", "", "FPA is awaiting for your Approval...", Mailbody);
                            Dal.CreateLogHistory("4", spnLoginUser.InnerText.ToString(), txtIndiComments.Text, HFID.Value.ToString());
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('FPA submitted successfully.');CloseRefreshWindow();", true);

                        }
                        else if (TransID == 0)
                        {
                            if (Convert.ToInt32(HFDESID.Value) < 21)
                            {
                                if (Basiccomp <= 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please fill your basic salary before submitting');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                            }
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
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Yet To Allocate Amount Is not zero')", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please select NPS Detail Yes or No.');", true);
        }
    }
    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        try
        {
            string newUrl = string.Empty;
           // if (Convert.ToDouble(txtYAllocate.Text) == 0)
           // {
                string gridSalaryHeadXml = string.Empty;
                string gridFPAHeadXml = string.Empty;
                StringWriter strSalHeadWriter = new StringWriter();
                StringWriter strFpaHeadWriter = new StringWriter();
                //Salary Head Distribution 

                DataTable _saldt = new DataTable("SalaryHead");
                _saldt.Columns.Add("FpaCycleId", typeof(string));
                _saldt.Columns.Add("FpaSalHeadId", typeof(string));
                _saldt.Columns.Add("SalAmount", typeof(double));
                if (gvsalarydist.Rows.Count > 0)
                {
                    foreach (GridViewRow grow in gvsalarydist.Rows)
                    {
                        DataRow _salrow = _saldt.NewRow();
                        Label FpaSalHeadId = (Label)grow.FindControl("lblSalHeadId");
                        TextBox SalAmount = (TextBox)grow.FindControl("txtSalAmount");
                        if (SalAmount.Text == "")
                        {
                            SalAmount.Text = "0";
                        }
                        _salrow["FpaCycleId"] = HFCYCLEID.Value;
                        _salrow["FpaSalHeadId"] = FpaSalHeadId.Text;
                        _salrow["SalAmount"] = Convert.ToDouble(SalAmount.Text);
                        _saldt.Rows.Add(_salrow);
                    }
                    _saldt.WriteXml(strSalHeadWriter, XmlWriteMode.IgnoreSchema);
                    gridSalaryHeadXml = strSalHeadWriter.ToString();
                }
                //FPA Head Distribution

            bool fpalblmorethan1600cc = false, fpalblbelow1600cc = false;

                DataTable _Fpadt = new DataTable("FpaHead");
                _Fpadt.Columns.Add("FpaCycleId", typeof(string));
                _Fpadt.Columns.Add("FpaHeadId", typeof(string));
                _Fpadt.Columns.Add("FpaAmount", typeof(double));
                if (gvFPADistribution.Rows.Count > 0)
                {
                    foreach (GridViewRow grow in gvFPADistribution.Rows)
                    {
                        DataRow _fparow = _Fpadt.NewRow();
                        Label FpaHeadId = (Label)grow.FindControl("lblFpaid");
                        TextBox FpaAmount = (TextBox)grow.FindControl("txtFPAAmount");
                        if (FpaAmount.Text == "")
                        {
                            FpaAmount.Text = "0";
                        }
                        _fparow["FpaCycleId"] = HFCYCLEID.Value;
                        _fparow["FpaHeadId"] = FpaHeadId.Text;
                        _fparow["FpaAmount"] = Convert.ToDouble(FpaAmount.Text);
                        _Fpadt.Rows.Add(_fparow);

                    if (FpaHeadId.Text == "61")
                    {
                        if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 0)
                        {
                            fpalblbelow1600cc = true;
                        }
                        if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 21600)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Monthly limit for Fuel & Oil up to 1600 CC is INR 1800. Please fill annual limit accordingly.');", true);
                            return;
                        }
                    }
                    if (FpaHeadId.Text == "62")
                    {
                        if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 0)
                        {
                            fpalblmorethan1600cc = true;
                        }
                        if (FpaAmount.Text != "" && Convert.ToDouble(FpaAmount.Text) > 28800)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Monthly limit for Fuel & Oil more than 1600 CC is INR 2400. Please fill annual limit accordingly.');", true);
                            return;
                        }
                    }
                    }
                    _Fpadt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
                    gridFPAHeadXml = strFpaHeadWriter.ToString();
                }

            if (fpalblbelow1600cc == true && fpalblmorethan1600cc == true)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Only one head for Fuel and Oil can be assigned.');", true);
                return;
            }

                int TransID = 0;
                double Basiccomp = 0, retrialbcomp = 0, basicretrial = 0, NPS = 0;
                if (txtretrialbcomponent.Text == "")
                {
                    retrialbcomp = 0;
                }
                else
                {
                    retrialbcomp = Convert.ToDouble(txtretrialbcomponent.Text);
                }
                if (txtBasiccomponent.Text == "")
                {
                    Basiccomp = 0;
                }
                else
                {
                    Basiccomp = Convert.ToDouble(txtBasiccomponent.Text);
                }
                if (txtBasicRetrials.Text == "")
                {
                    basicretrial = 0;
                }
                else
                {
                    basicretrial = Convert.ToDouble(txtBasicRetrials.Text);
                }
                if (txtNPS.Text == "")
                {
                    NPS = 0;
                }
                else
                {
                    NPS = Convert.ToDouble(txtNPS.Text);
                }
               // if (string.IsNullOrEmpty(gridFPAHeadXml) || string.IsNullOrEmpty(gridSalaryHeadXml))
               // {
               //     ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You have not declare your Fpa.');", //true);
               // }
               // else
               // {

                    if (Convert.ToInt32(HFDESID.Value) < 21)
                    {
                        TransID = Dal.InsertMyApprovaltoFPAForGM(gridSalaryHeadXml, gridFPAHeadXml, Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "1", getSAPID(ReturnLoginName()), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), Convert.ToInt32(basicretrial), txtIndiComments.Text, Convert.ToInt32(NPS), txtPranNo.Text, txtNominee.Text, txtAsBank.Text, ddlRelationShip.SelectedValue.ToString());
                    }
                    else
                    {
                        TransID = Dal.InsertMyApprovalClaim(gridSalaryHeadXml, gridFPAHeadXml, Convert.ToInt32(HFCYCLEID.Value), Convert.ToInt32(HFPAID.Value), "1", getSAPID(ReturnLoginName()), Convert.ToInt32(NPS), txtPranNo.Text, txtNominee.Text, txtAsBank.Text, ddlRelationShip.SelectedValue.ToString());
                    }
                    if (TransID == 1)
                    {
                        //newUrl = "http://vedcsapp/sites/kmportal/mycorner/fpaandlimits/Pages/home.aspx";
                        // string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnLoginUser.InnerText.ToString() + "<br/><br/>Your FPA has been initiated for Financial Year " + year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                        // Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", "", "A fpa Claim is awaiting your Approval...", Mailbody);
                        Dal.CreateLogHistory("1", spnLoginUser.InnerText.ToString(), txtIndiComments.Text, HFID.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('FPA Saved successfully.');CloseRefreshWindow();", true);

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
               // }


           // }
            //else
           // {
           //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Yet To Allocate Amount Is not zero')", true);
            //}
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

    protected void txtBasiccomponent_TextChanged(object sender, EventArgs e)
    {
        txtBasiccomponent_onchange(null, null);
    }

    public void txtNPSValue_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtYAllocate.Text) > 0)
        {
            double NpsEnterAmount = 0, annualBasic = 0, AnnualNpsAmount = 0, BasicSal = 0, oldNPSAmt = 0;
            oldNPSAmt = Convert.ToDouble(hfNPSAmt.Value);
            if (txtNPSValue.Text == "")
            {
                txtNPSValue.Text = "0";
            }

            NpsEnterAmount = Convert.ToDouble(txtNPSValue.Text);
            if (txtcBasic.Text == "")
            {
                annualBasic = 0;
            }
            else
            {
                annualBasic = Convert.ToDouble(txtcBasic.Text);
            }

            if (txtBasicSalary.Text == "")
            {
                BasicSal = 0;
            }
            else
            {
                BasicSal = Convert.ToDouble(txtBasicSalary.Text);
            }

            if (Convert.ToInt32(HFDESID.Value) > 21)
            {
                AnnualNpsAmount = (BasicSal * 10) / 100;
            }
            else
            {
                AnnualNpsAmount = (annualBasic * 10) / 100;
            }


            if (NpsEnterAmount >= oldNPSAmt)
            {
                if (NpsEnterAmount >= 1000)
                {
                    if (NpsEnterAmount > AnnualNpsAmount)
                    {
                        txtNPS.Text = "0";
                        txtNPSValue.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "basic", "alert('NPS value can not greater than 10% of annual basic.');", true);
                    }
                    else
                    {
                        txtNPS.Text = Convert.ToString(NpsEnterAmount);
                        txtNPSValue.Text = Convert.ToString(NpsEnterAmount);
                    }
                    CalculateYetToAllocate();
                }
                else
                {
                    txtNPS.Text = "0";
                    txtNPSValue.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "key", "alert('NPS amount can not be less than 1000.');", true);
                }
            }
            else  // case for Current NPS is less then Old NPS amt
            {
                txtNPS.Text = Convert.ToString(hfNPSAmt.Value);
                txtNPSValue.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "NPS", "alert('Current NPS amount can not be less than previous NPS amount - " + txtNPS.Text + "');", true);
            }
        }

        else
        {
            txtNPS.Text = "0";
            txtNPSValue.Text = "";
            CalculateYetToAllocate(); 
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "key", "alert(' You can not assign any value in NPS as yet to allocate amount is zero.');", true);
        }
    }

    public void CalculateYetToAllocate()
    {
        string SalAmount = string.Empty;
        string FpaAmount = string.Empty;
        txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                           Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
        txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
            Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtretrialbcomponent.Text) +
            Convert.ToDouble(txtBasiccomponent.Text) + Convert.ToDouble(txtBasicRetrials.Text) + Convert.ToDouble(txtNPS.Text));
        txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));



        if (gvsalarydist.Rows.Count > 0)
        {
            TextBox Totallsaamount = (TextBox)gvsalarydist.FooterRow.FindControl("txtTotalSalaryamount");
            SalAmount = Totallsaamount.Text;
        }
        else
        {
            SalAmount = "0";
        }
        if (gvFPADistribution.Rows.Count > 0)
        {
            TextBox Totallfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
            FpaAmount = Totallfpaamount.Text;
        }
        else
        {
            FpaAmount = "0";
        }
        if (SalAmount == "")
        {
            SalAmount = "0";
        }

        if (FpaAmount == "")
        {
            FpaAmount = "0";
        }

        txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - (Convert.ToDouble(txtTotDeduct.Text) + Convert.ToDouble(SalAmount) + Convert.ToDouble(FpaAmount)));

    }

    protected void rdbNPSMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbNPSMode.SelectedValue == "1")
        {
            trFirstRow.Visible = true;
           // trSecondRow.Visible = true;
            trThirdRow.Visible = true;
        }
        else
        {
            EmptyNps();
            CalculateYetToAllocate();
            trFirstRow.Visible = false;
            trThirdRow.Visible = false;

        }
    }

    public void EmptyNps()
    {
        txtAsBank.Text = string.Empty;
        ddlRelationShip.SelectedValue = "0";
        txtNominee.Text = string.Empty;
        txtNPSValue.Text = "";
        txtNPS.Text = "0";
        txtPranNo.Text = string.Empty;
    }

    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "spstestpkg";
    }
}

