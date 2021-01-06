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
using System.Drawing;
using Microsoft.SharePoint;
public partial class FPAReIntiation : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    #region"Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFPA.Focus();
            FillHeader(getSAPID(ReturnLoginName()));
            FillLocation(getSAPID(ReturnLoginName()));
            SetIntialValue();
           
        }
    }
    /// <summary>
    /// Event for bind Designation dropdown list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        string locname = ddlLocation.SelectedValue;
        FillDesignation(locname, getSAPID(ReturnLoginName()));
        ddlEmployee.SelectedValue = "0";


    }
    /// <summary>
    /// Event for bind employee dropdown list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlDesignation.SelectedValue) < 21)
        {
            ltfpa.Visible = false;
            txtFPA.Visible = false;

            trgmctc.Visible = true;
            trgm.Visible = true;
            ltctc.Visible = true;
            txtCTC.Visible = true;
            //txtretrialbcomponent.Visible = true;
            //ltretcopm.Visible = true;
            ltbasiccomp.Visible = true;
            ltcbasic.Visible = true;
            txtcBasic.Visible = true;
            txtBasiccomponent.Visible = true;
        }
        else
        {
            trgmctc.Visible = false;
            trgm.Visible = false;
            ltctc.Visible = false;
            txtCTC.Visible = false;
            //txtretrialbcomponent.Visible = false;
            //ltretcopm.Visible = false;
            ltbasiccomp.Visible = false;
            ltcbasic.Visible = false;
            txtcBasic.Visible = false;
            txtBasiccomponent.Visible = false;
            ltfpa.Visible = true;
            txtFPA.Visible = true;
        }

        string locname = ddlLocation.SelectedValue;
        int DesId = Convert.ToInt32(ddlDesignation.SelectedValue);
        FillEmployee(locname, DesId);
    }
    /// <summary>
    /// Event for bind employee Emplyee code and carscheme
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Empsapid = ddlEmployee.SelectedValue;
        DataTable dtecode = Dal.getEmployeeEcode(Empsapid);
        DataTable dt = (DataTable)ViewState["dt_Emp"];

        if (dtecode.Rows.Count > 0)
        {
            SetVisible();
            spnECode.InnerText = Convert.ToString(dtecode.Rows[0]["ECODE"].ToString());
            spcarscheme.InnerText = Convert.ToString(dtecode.Rows[0]["CAR_SCHEME"].ToString());
            DataRow[] drsel = dt.Select("sapid='" + Empsapid + "'");
            HFEMAILS.Value = drsel[0]["email"].ToString();

            DataTable fpadetail = Dal.GetFpaDetail(Empsapid);
            if (fpadetail.Rows.Count > 0)
            {
                txtCTC.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString());
                txtFPA.Text = Convert.ToString(fpadetail.Rows[0]["CTC_OR_FPA"].ToString());
                txtcBasic.Text = Convert.ToString(fpadetail.Rows[0]["CURRENT_BASIC"].ToString());
                txtOther1.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_1"].ToString());
                txtOther2.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_2"].ToString());
                txtOther3.Text = Convert.ToString(fpadetail.Rows[0]["OTHERS_3"].ToString());
                txtTotpcredited.Text = Convert.ToString(Dal.getTotalPterolAmountById(Empsapid)).Trim();
                txtLocFPA.Text = Convert.ToString(fpadetail.Rows[0]["LOCATIONAL_FPA"].ToString());
                txtBasiccomponent.Text = Convert.ToString(fpadetail.Rows[0]["BASIC_RETIRALS_AVAILED"].ToString());
				if (txtBasiccomponent.Text == "")
                {
                    txtBasiccomponent.Text="0";
                }
                txtTotalsalavailed.Text = Convert.ToString(fpadetail.Rows[0]["TOTAL_TAXABLE_AMT_AVAILED"].ToString());
                txtTotReimAvailed.Text = Convert.ToString(Dal.GetTotalRemAmount(Empsapid));
                txtClaRental.Text = Convert.ToString(fpadetail.Rows[0]["CLA_RENTAL"].ToString());
                txtVehEmi.Text = Convert.ToString(fpadetail.Rows[0]["VEHICLE_EMI"].ToString());
                txtSAF.Text = Convert.ToString(fpadetail.Rows[0]["SCHOOL_SUBSIDY_DEDUCTION"].ToString());
                txtBusDeduct.Text = Convert.ToString(fpadetail.Rows[0]["BUS_DEDUCTION"].ToString());
                //txtretrialbcomponent.Text = Convert.ToString(fpadetail.Rows[0]["RETIRAL_BENEFITS"].ToString());
                HFPAID.Value = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString());
                //HFID.Value = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString());
                ViewState["fpaid"] = Convert.ToString(fpadetail.Rows[0]["FPA_ID"].ToString());
                ViewState["srno"] = Convert.ToString(fpadetail.Rows[0]["SERIAL_NO"].ToString());
                //txtComments.Text = Convert.ToString(fpadetail.Rows[0]["COMMENT"].ToString());
                if (Convert.ToInt32(ddlDesignation.SelectedValue) > 21)
                {
                    txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtFPA.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                         Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                    txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                         Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                }
                else
                {
                    txtTotAllownce.Text = Convert.ToString(Convert.ToDouble(txtCTC.Text) + Convert.ToDouble(txtOther1.Text) + Convert.ToDouble(txtOther2.Text) +
                         Convert.ToDouble(txtOther3.Text) + Convert.ToDouble(txtLocFPA.Text) + Convert.ToDouble(txtTotpcredited.Text));
                    txtTotDeduct.Text = Convert.ToString(Convert.ToDouble(txtTotalsalavailed.Text) + Convert.ToDouble(txtTotReimAvailed.Text) + Convert.ToDouble(txtClaRental.Text) +
                        Convert.ToDouble(txtVehEmi.Text) + Convert.ToDouble(txtSAF.Text) + Convert.ToDouble(txtBusDeduct.Text)  +
                        Convert.ToDouble(txtBasiccomponent.Text));
                    txtNetAmount.Text = Convert.ToString(Convert.ToDouble(txtTotAllownce.Text) - Convert.ToDouble(txtTotDeduct.Text));
                }
                btnIntiation.Visible = true;
            }
            else
            {
                SetIntialValue();
                btnIntiation.Visible = false;
                
            }
        }
        else
        {
            SetIntialValue();
            spnECode.InnerText = "";
            spcarscheme.InnerText = "";
        }

    }

    /// <summary>
    /// Event for save the FPA record into database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIntiation_Click(object sender, EventArgs e)
    {
        double Fpa = 0, other1 = 0, other2 = 0, other3 = 0, totpetrol = 0, totAllow = 0, totSalAvail = 0, totreimavail = 0, clarental = 0;
        string newUrl = string.Empty;
        //string EmailID=string.Empty;

        string EmailID = string.Empty;
        if (HFEMAILS.Value.Trim() == "")
        {
            EmailID = "";
        }
        else { EmailID = HFEMAILS.Value.ToString(); }
        double vehemi = 0, saf = 0, busdeduct = 0, totdeduct = 0, NetAmount = 0, Locfpa = 0, Basiccomp = 0, retrialbcomp = 0, cBasic = 0, CTC = 0;
        if (ddlLocation.SelectedValue == "0" || ddlDesignation.SelectedValue == "0" || ddlEmployee.SelectedValue == "0" || Convert.ToDouble(txtNetAmount.Text) <= 0)
        {
            if (ddlLocation.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Location.');", true);
            }
            else if (ddlDesignation.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Designation);", true);
            }
            else if (ddlEmployee.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Employee.');", true);
            }
            else if (Convert.ToDouble(txtNetAmount.Text) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please note Net Amount cannot be Negative and also cnanot be zero.');", true);
            }
        }
        else
        {
            try
            {
                string EmpSapId = ddlEmployee.SelectedValue;
                string DesId = ddlDesignation.SelectedValue;
                if (txtcBasic.Text == "")
                {
                    cBasic = 0;
                }
                else
                {
                    cBasic = Convert.ToDouble(txtcBasic.Text);
                }
                //if (txtretrialbcomponent.Text == "")
                //{
                //    retrialbcomp = 0;
                //}
                //else
                //{
                //    retrialbcomp = Convert.ToDouble(txtretrialbcomponent.Text);
                //}
                if (txtBasiccomponent.Text == "")
                {
                    Basiccomp = 0;
                }
                else
                {
                    Basiccomp = Convert.ToDouble(txtBasiccomponent.Text);
                }
                if (txtCTC.Text == "")
                {
                    CTC = 0;
                }
                else
                {
                    CTC = Convert.ToDouble(txtCTC.Text);
                }
                if (txtFPA.Text == "")
                {
                    Fpa = 0;
                }
                else
                {
                    Fpa = Convert.ToDouble(txtFPA.Text);
                }
                if (txtOther1.Text == "")
                {
                    other1 = 0;
                }
                else
                {
                    other1 = Convert.ToDouble(txtOther1.Text);
                }
                if (txtOther2.Text == "")
                {
                    other2 = 0;
                }
                else
                {
                    other2 = Convert.ToDouble(txtOther2.Text);

                }
                if (txtOther3.Text == "")
                {
                    other3 = 0;
                }
                else
                {
                    other3 = Convert.ToDouble(txtOther3.Text);
                }
                if (txtLocFPA.Text == "")
                {
                    Locfpa = 0;
                }
                else
                {
                    Locfpa = Convert.ToDouble(txtLocFPA.Text);
                }
                if (txtTotpcredited.Text == "")
                {
                    totpetrol = 0;
                }
                else
                {
                    totpetrol = Convert.ToDouble(txtTotpcredited.Text);
                }
                if (txtTotAllownce.Text == "")
                {
                    totAllow = 0;
                }
                else
                {
                    totAllow = Convert.ToDouble(txtTotAllownce.Text);

                }
                if (txtTotalsalavailed.Text == "")
                {
                    totSalAvail = 0;
                }
                else
                {
                    totSalAvail = Convert.ToDouble(txtTotalsalavailed.Text);
                }
                if (txtTotReimAvailed.Text == "")
                {
                    totreimavail = 0;
                }
                else
                {
                    totreimavail = Convert.ToDouble(txtTotReimAvailed.Text);
                }
                if (txtClaRental.Text == "")
                {
                    clarental = 0;
                }
                else
                {
                    clarental = Convert.ToDouble(txtClaRental.Text);
                }
                if (txtVehEmi.Text == "")
                {
                    vehemi = 0;
                }
                else
                {
                    vehemi = Convert.ToDouble(txtVehEmi.Text);
                }
                if (txtSAF.Text == "")
                {
                    saf = 0;
                }
                else
                {
                    saf = Convert.ToDouble(txtSAF.Text);
                }
                if (txtBusDeduct.Text == "")
                {
                    busdeduct = 0;
                }
                else
                {
                    busdeduct = Convert.ToDouble(txtBusDeduct.Text);
                }
                if (txtTotDeduct.Text == "")
                {
                    totdeduct = 0;
                }
                else
                {
                    totdeduct = Convert.ToDouble(txtTotDeduct.Text);
                }
                if (txtNetAmount.Text == "")
                {
                    NetAmount = 0;
                }
                else
                {
                    NetAmount = Convert.ToDouble(txtNetAmount.Text);
                }
                string year = Convert.ToString(DateTime.Now);
                int TransID = 0;
				
				if (Convert.ToInt32(DesId) < 21)
                {
                    //Basiccomp = 0, retrialbcomp = 0,cBasic = 0,CTC=0;
                    TransID = Dal.reinitiationGMFPANew(Convert.ToString(ViewState["fpaid"]), DesId, EmpSapId, year, txtComments.Text,Convert.ToInt32(CTC), Convert.ToInt32(cBasic), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf),Convert.ToInt32( busdeduct), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), HFID.Value.ToString(), Convert.ToInt32(totSalAvail));
                }
                else
                {
                    TransID = Dal.reinitiation(Convert.ToString(ViewState["fpaid"]), DesId, EmpSapId, year, txtComments.Text, Convert.ToInt32(Fpa), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct),HFID.Value.ToString(),Convert.ToInt32( totSalAvail));

                }
				
                //if (Convert.ToInt32(DesId) < 21)
               // {
                    //Basiccomp = 0, retrialbcomp = 0,cBasic = 0,CTC=0;
                  //  TransID = Dal.reinitiationGMFPANew(Convert.ToString(ViewState["fpaid"]), DesId, EmpSapId, year, txtComments.Text, CTC, cBasic, other1, other2, other3, Locfpa, totpetrol, totreimavail, clarental, vehemi, saf, busdeduct, Basiccomp, retrialbcomp, HFID.Value.ToString(), totSalAvail);
                //}
               //else
               // {
                 //   TransID = Dal.reinitiation(Convert.ToString(ViewState["fpaid"]), DesId, EmpSapId, year, txtComments.Text, Fpa, other1, other2, other3, Locfpa, totpetrol, totreimavail, clarental, vehemi, saf, busdeduct,HFID.Value.ToString(), totSalAvail);

               // }
                if (TransID == 1)
                {
                    int yr = DateTime.Now.Year;
                    if (HFBU.Value == "REM")
                    {
                       newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
                    }
                    else
                      {
                            newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                      }
                    string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + ddlEmployee.SelectedItem.Text + "<br/><br/>Your FPA has been reinitiated for Financial Year " + yr + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";

                    Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", "", "Your FPA has been reinitiated ...", Mailbody);
                    Dal.CreateLogHistory("0", spnLoginUser.InnerText, txtComments.Text, HFID.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('" + ddlEmployee.SelectedItem.Text + " FPA ReInitiated Successfully');CloseRefreshWindow();", true);

                }
                else if (TransID == 0)
                {
                    btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                }
                else if (TransID == 2)
                {
                    btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                }
                else if (TransID == 5)
                {
                    btnIntiation.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data Already Submitted.');CloseRefreshWindow();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
            }
        }
    }
    /// <summary>
    /// Event for set the intial value of all control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        SetVisible();
    }
    protected void txtCTC_TextChanged(object sender, EventArgs e)
    {
       // ComputeTotalAllowance();
    }
    protected void txtcBasic_TextChanged(object sender, EventArgs e)
    {
       // ComputeTotalAllowance();
    }
    protected void txtFPA_TextChanged(object sender, EventArgs e)
    {
       // ComputeTotalAllowance();
    }
    protected void txtOther1_TextChanged(object sender, EventArgs e)
    {
        //ComputeTotalAllowance();
    }
    protected void txtOther2_TextChanged(object sender, EventArgs e)
    {
       // ComputeTotalAllowance();
    }
    protected void txtLocFPA_TextChanged(object sender, EventArgs e)
    {
        //ComputeTotalAllowance();
    }
    protected void txtTotpcredited_TextChanged(object sender, EventArgs e)
    {
        //ComputeTotalAllowance();
    }
    protected void txtOther3_TextChanged(object sender, EventArgs e)
    {
       //ComputeTotalAllowance();
    }
    #endregion

    #region"Method"
    public void SetVisible()
    {
        txtcBasic.Text = "0";
        txtcBasic.Enabled = true;
        txtCTC.Text = "0";
        txtCTC.Enabled = true;
        txtFPA.Text = "0";
        txtFPA.Enabled = true;
        txtLocFPA.Text = "0";
        txtLocFPA.Enabled = true;
        txtOther1.Text = "0";
        txtOther1.Enabled = true;
        txtOther2.Text = "0";
        txtOther2.Enabled = true;
        txtOther3.Text = "0";
        txtOther3.Enabled = true;
        txtTotAllownce.Text = "0";
        txtTotAllownce.Enabled = false;
        txtTotalsalavailed.Text = "0";
        txtTotalsalavailed.Enabled = true;
        txtTotDeduct.Text = "0";
        txtTotDeduct.Enabled = false;
        txtTotpcredited.Text = "0";
        txtTotpcredited.Enabled = false;
        txtTotReimAvailed.Text = "0";
        txtTotReimAvailed.Enabled = true;
        txtVehEmi.Text = "0";
        txtVehEmi.Enabled = true;
        txtSAF.Text = "0";
        txtSAF.Enabled = true;
        txtNetAmount.Text = "0";
        txtNetAmount.Enabled = false;
        txtClaRental.Text = "0";
        txtClaRental.Enabled = true;
        txtBusDeduct.Text = "0";
        txtBusDeduct.Enabled = true;
        txtBasiccomponent.Text = "0";
        txtBasiccomponent.Enabled = true;
       // txtretrialbcomponent.Text = "0";
       // txtretrialbcomponent.Enabled = true;

    }
    public void SetIntialValue()
    {
        txtcBasic.Text = "0";
        txtcBasic.Enabled = false;
        txtCTC.Text = "0";
        txtCTC.Enabled = false;
        txtFPA.Text = "0";
        txtFPA.Enabled = false;
        txtLocFPA.Text = "0";
        txtLocFPA.Enabled = false;
        txtOther1.Text = "0";
        txtOther1.Enabled = false;
        txtOther2.Text = "0";
        txtOther2.Enabled = false;
        txtOther3.Text = "0";
        txtOther3.Enabled = false;
        txtTotAllownce.Text = "0";
        txtTotAllownce.Enabled = false;
        txtTotalsalavailed.Text = "0";
        txtTotalsalavailed.Enabled = false;
        txtTotDeduct.Text = "0";
        txtTotDeduct.Enabled = false;
        txtTotpcredited.Text = "0";
        txtTotpcredited.Enabled = false;
        txtTotReimAvailed.Text = "0";
        txtTotReimAvailed.Enabled = false;
        txtVehEmi.Text = "0";
        txtVehEmi.Enabled = false;
        txtSAF.Text = "0";
        txtSAF.Enabled = false;
        txtNetAmount.Text = "0";
        txtNetAmount.Enabled = false;
        txtClaRental.Text = "0";
        txtClaRental.Enabled = false;
        txtBusDeduct.Text = "0";
        txtBusDeduct.Enabled = false;
        txtBasiccomponent.Text = "0";
        txtBasiccomponent.Enabled = false;
        //txtretrialbcomponent.Text = "0";
        //txtretrialbcomponent.Enabled = false;

    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            spnSrNo.InnerText = Dal.Serial_No("FPA");
            HFID.Value = spnSrNo.InnerText;
            HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
        }
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
    private void FillLocation(string sapid)
    {
        ddlLocation.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlLocation.Items.Add(li);
        ddlLocation.AppendDataBoundItems = true;
        DataTable dt_Loc = Dal.getLocation(sapid);
        if (dt_Loc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dt_Loc;
            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location";
            ddlLocation.DataBind();

        }
        else
        {
        }

    }
    private void FillDesignation(string locname, string Sapid)
    {
        ddlDesignation.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlDesignation.Items.Add(li);
        ddlDesignation.AppendDataBoundItems = true;
        DataTable dt_Des = Dal.getDesignation(locname, Sapid);
        if (dt_Des.Rows.Count > 0)
        {
            ddlDesignation.DataSource = dt_Des;
            ddlDesignation.DataTextField = "DESIGNATION";
            ddlDesignation.DataValueField = "DES_ID";
            ddlDesignation.DataBind();

        }
        else
        {
        }

    }
    private void FillEmployee(string locname, int Desid)
    {
        ddlEmployee.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlEmployee.Items.Add(li);
        ddlEmployee.AppendDataBoundItems = true;
        DataTable dt_Emp = Dal.getEmployee(locname, Desid);
        if (dt_Emp.Rows.Count > 0)
        {
            ddlEmployee.DataSource = dt_Emp;
            ddlEmployee.DataTextField = "ENAME";
            ddlEmployee.DataValueField = "sapid";
            ddlEmployee.DataBind();
            ViewState["dt_Emp"] = dt_Emp;

        }
        else
        {
        }

    }
    public void ComputeTotalAllowance()
    {
        double amount = 20000000;
        double ctc = 0;
        double cbasic = 0;
        double fpa = 0;
        double other1 = 0;
        double other2 = 0;
        double other3 = 0;
        double locfpa = 0;
        double totptamount = 0;
        double totdeduction = 0;
        if (txtTotDeduct.Text == "")
        {
            totdeduction = 0;
        }
        else
        {
            totdeduction = Convert.ToDouble(txtTotDeduct.Text);
        }
        if (txtcBasic.Text == "")
        {
            cbasic = 0;
        }
        else
        {
            cbasic = Convert.ToDouble(txtcBasic.Text);
        }
        if (txtCTC.Text == "")
        {
            ctc = 0;
        }
        else
        {
            ctc = Convert.ToDouble(txtCTC.Text);
        }
        if (txtFPA.Text == "")
        {
            fpa = 0;
        }
        else
        {
            fpa = Convert.ToDouble(txtFPA.Text);
        }
        if (txtOther1.Text == "")
        {
            other1 = 0;
        }
        else
        {
            other1 = Convert.ToDouble(txtOther1.Text);
        }
        if (txtOther2.Text == "")
        {
            other2 = 0;
        }
        else
        {
            other2 = Convert.ToDouble(txtOther2.Text);
        }
        if (txtOther3.Text == "")
        {
            other3 = 0;
        }
        else
        {
            other3 = Convert.ToDouble(txtOther3.Text);
        }
        if (txtLocFPA.Text == "")
        {
            locfpa = 0;
        }
        else
        {
            locfpa = Convert.ToDouble(txtLocFPA.Text);
        }
        if (txtTotpcredited.Text == "")
        {
            totptamount = 0;
        }
        else
        {
            totptamount = Convert.ToDouble(txtTotpcredited.Text);
        }
        if (Convert.ToInt32(ddlDesignation.SelectedValue) < 21)
        {
            if (ctc >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('FPA should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((0 + cbasic + other1 + other2 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((0 + cbasic + other1 + other2 + other3 + locfpa + totptamount));
                txtFPA.Focus();
            }
            if (other1 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER1 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + 0 + cbasic + other2 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + 0 + cbasic + other2 + other3 + locfpa + totptamount));
                txtOther1.Focus();

            }
            if (other2 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER2 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + other1 + cbasic + 0 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + other1 + cbasic + 0 + other3 + locfpa + totptamount));
                txtOther2.Focus();

            }
            if (other3 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER3 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + other1 + other2 + cbasic + 0 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + other1 + other2 + cbasic + 0 + locfpa + totptamount));
                txtOther3.Focus();

            }
            if (locfpa >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Locational FPA should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + other1 + other2 + cbasic + other3 + 0 + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + other1 + other2 + cbasic + other3 + 0 + totptamount));
                txtLocFPA.Focus();

            }
            if (totptamount >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Total Petrol Amount should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + other1 + other2 + cbasic + other3 + locfpa + 0) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + other1 + other2 + cbasic + other3 + locfpa + 0));
                txtTotpcredited.Focus();

            }
            if (cbasic >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Current Basic should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((ctc + other1 + other2 + 0 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((ctc + other1 + other2 + 0 + other3 + locfpa + totptamount));
                txtcBasic.Focus();

            }
            txtNetAmount.Text = Convert.ToString((ctc + other1 + other2 + other3 + cbasic + locfpa + totptamount) - (totdeduction));
            txtTotAllownce.Text = Convert.ToString((ctc + other1 + other2 + other3 + cbasic + locfpa + totptamount));
        }
        else
        {
            if (fpa >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('FPA should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((0 + other1 + other2 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((0 + other1 + other2 + other3 + locfpa + totptamount));
                txtFPA.Focus();
            }
            if (other1 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER1 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((fpa + 0 + other2 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((fpa + 0 + other2 + other3 + locfpa + totptamount));
                txtOther1.Focus();

            }
            if (other2 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER2 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((fpa + other1 + 0 + other3 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((fpa + other1 + 0 + other3 + locfpa + totptamount));
                txtOther2.Focus();

            }
            if (other3 >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('OTHER3 should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((fpa + other1 + other2 + 0 + locfpa + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((fpa + other1 + other2 + 0 + locfpa + totptamount));
                txtOther3.Focus();

            }
            if (locfpa >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Locational FPA should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((fpa + other1 + other2 + other3 + 0 + totptamount) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((fpa + other1 + other2 + other3 + 0 + totptamount));
                txtLocFPA.Focus();

            }
            if (totptamount >= amount)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Total Petrol Amount should be less than Rs.20000000.');", true);
                txtNetAmount.Text = Convert.ToString((fpa + other1 + other2 + other3 + locfpa + 0) - (totdeduction));
                txtTotAllownce.Text = Convert.ToString((fpa + other1 + other2 + other3 + locfpa + 0));
                txtTotpcredited.Focus();

            }
            txtNetAmount.Text = Convert.ToString((fpa + other1 + other2 + other3 + locfpa + totptamount) - (totdeduction));
            txtTotAllownce.Text = Convert.ToString((fpa + other1 + other2 + other3 + locfpa + totptamount));
        }

    }
    public void ComputeTotalDeduction()
    {
    }
    #endregion
}
