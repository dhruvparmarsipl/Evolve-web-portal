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

public partial class MyFlexiDeclartion : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    double amount = 0;
    double amount1 = 0;
    string Sapid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string Sapid = getSAPID(ReturnLoginName());
            FillHeader(Sapid);
            gvFPADistribution.Enabled = false;
            gvsalarydist.Enabled = false;
            bindHistory();

        }
    }
    private string GetStatus(string status)
    {
        string cstatus = string.Empty;
        if (status == "0")
        {
            cstatus = "Initiate By FPA ";
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
                HFCYCLEID.Value = Convert.ToString(dt_UInfo.Rows[0]["fpa_cycle_id"]);
                HFCSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["current_status"]);
                HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["FpaEmail"]);
                SpnStatus.InnerText = GetStatus(HFCSTATUS.Value);
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
                }

                GetFpaDetail( HFCYCLEID.Value);
                FillSalaryHeads();
                FillFpaHeads();
                SetIntialValue();
            }
            else
            {
                SetIntialValue();
            }
        }
    }
    private void GetFpaDetail(string CycleId)
    {
        using (DataTable fpadetail = Dal.GetCurrentCycleUser(CycleId))
        {
            if (fpadetail.Rows.Count > 0)
            {
                txtCTC.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtFPA.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString().Trim());
                txtcBasic.Text = Convert.ToString(fpadetail.Rows[0]["CURRENT_BASIC"].ToString().Trim());
                txtOther1.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_1"].ToString().Trim());
                txtOther2.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_2"].ToString().Trim());
                txtOther3.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_3"].ToString().Trim());
                txtTotpcredited.Text = Convert.ToString(Dal.getTotalPterolAmount(CycleId)).Trim();
            
                txtLocFPA.Text = Convert.ToString(fpadetail.Rows[0]["LOCATIONAL_FPA"].ToString().Trim());
                txtBasiccomponent.Text = Convert.ToString(fpadetail.Rows[0]["BASIC_COMPONENT"].ToString().Trim());
                txtTotalsalavailed.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_TAXABLE_AMT_AVAILED"].ToString().Trim());
                txtTotReimAvailed.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_REIMBURSEMENTS_AMT_AVAILED"].ToString().Trim());
                txtClaRental.Text = Convert.ToString(fpadetail.Rows[0]["CLA_RENTAL"].ToString().Trim());
                txtVehEmi.Text = Convert.ToString(fpadetail.Rows[0]["VEHICLE_EMI"].ToString().Trim());
                txtBasicRetrials.Text = Convert.ToString(fpadetail.Rows[0]["basic_retirals_availed"].ToString().Trim());
                txtSAF.Text = Convert.ToString(fpadetail.Rows[0]["SCHOOL_SUBSIDY_DEDUCTION"].ToString().Trim());
                txtBusDeduct.Text = Convert.ToString(fpadetail.Rows[0]["BUS_DEDUCTION"].ToString().Trim());
                txtretrialbcomponent.Text = Convert.ToString(fpadetail.Rows[0]["RETIRAL_BENEFITS"].ToString().Trim());
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
                         Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                    txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text));
                }
                else
                {
                    txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                         Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                    txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                        Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text) + Convert.ToDouble(txtretrialbcomponent.Text) +
                        Convert.ToDouble(txtBasiccomponent.Text) + Convert.ToDouble(txtBasicRetrials.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                    txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtNetAmount.Text));
                }
                SetIntialValue();
            }

        }
    }
    public void SetIntialValue()
    {
        txtBasicRetrials.Enabled = false;
        txtBasiccomponent.Enabled = false;
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
        txtIndiComments.Enabled = false;
        txtFPAcomments.Enabled = false;
        txtReject.Enabled = false;

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
        //return "spstestpkg";
    }
    private void FillFpaHeads()
    {
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6" || HFCSTATUS.Value == "3")
        {
            using (DataTable __FpaheadsDraft = Dal.GetFpaheadsDraftDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
            {
                if (__FpaheadsDraft.Rows.Count > 0)
                {
                   DataRow[] PetrolAmount = __FpaheadsDraft.Select("FPA_HEAD = 'Fuel & Oil'");
                    if (PetrolAmount.Length==0)
                    {
                       
                    }
                    else
                    {
                        PetrolAmount[0]["AMOUNT"] = Convert.ToString(Convert.ToDouble(PetrolAmount[0].ItemArray[2].ToString()) + (Convert.ToDouble(txtTotpcredited.Text)));
                        __FpaheadsDraft.AcceptChanges();
                    }
                    gvFPADistribution.DataSource = __FpaheadsDraft;
                    gvFPADistribution.DataBind();
                    object sumObject;
                    sumObject = __FpaheadsDraft.Compute("Sum(AMOUNT)", "");
                    TextBox txtTotalfpaamount = (TextBox)gvFPADistribution.FooterRow.FindControl("txtTotalFPAamount");
                    txtTotalfpaamount.Text = sumObject.ToString();
                    txtTotalfpaamount.Enabled = false;
                    if (txtYAllocate.Text == "")
                    {
                        txtYAllocate.Text = "0";
                    }
                    txtYAllocate.Text = Convert.ToString(Convert.ToDouble(txtYAllocate.Text) - Convert.ToDouble(txtTotalfpaamount.Text));
                }
                else
                {
                    gvFPADistribution.DataBind();
                }
            }
        }

    }
    private void FillSalaryHeads()
    {
        if (HFCSTATUS.Value == "1" || HFCSTATUS.Value == "2" || HFCSTATUS.Value == "4" || HFCSTATUS.Value == "5" || HFCSTATUS.Value == "7" || HFCSTATUS.Value == "6" || HFCSTATUS.Value == "3")
        {
            using (DataTable _SalheadsDraft = Dal.GetSalaryHeadsDraft(spnLoc.InnerText, Convert.ToInt32(HFDESID.Value), Convert.ToInt32(HFCYCLEID.Value)))
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
