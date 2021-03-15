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
using Microsoft.SharePoint;
using System.Web.Services;


public partial class FPAIntiation : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    Utility.Service Util = new Utility.Service();
    static string FpaAdmin = string.Empty;
    #region"Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFPA.Focus();
            // HFID.Value = "0";
            HFSAPID.Value = getSAPID(ReturnLoginName());
            FillHeader(HFSAPID.Value);
            //  FillLocation(Sapid);
            FillUser();
            SetIntialValue();
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
 if (gvUser.Rows.Count > 0)
        {
        for (int i = 0; i < 2; i++)
        {
            TableHeaderCell cell = new TableHeaderCell();
            TextBox txtSearch = new TextBox();
            txtSearch.Attributes["placeholder"] = gvUser.Columns[i].HeaderText;
            txtSearch.CssClass = "search_textbox";
            cell.Controls.Add(txtSearch);
            row.Controls.Add(cell);
        }
        gvUser.HeaderRow.Parent.Controls.AddAt(1, row);
}

    }

    public void FillUser()
    {
        DataTable UserDetail = Dal.GetUserFpaInitiateDetail("", HFSAPID.Value);
        if (UserDetail.Rows.Count > 0)
        {
            gvUser.DataSource = UserDetail;
            gvUser.DataBind();
        }
else
        {
            UserDetail.Rows.Add("No Document");
            gvUser.DataSource = UserDetail;
            gvUser.DataBind();
        }
    }
    [WebMethod]
    public static int SubmitData(double Fpa, double CTC, double cBasic, double other1, double other2, double other3, double totpetrol,
      double Locfpa, double busdeduct, double saf, double vehemi, double totSalAvail, double totreimavail, double clarental, string Sapid, string DesId, string EamilId, double NetAmount, string Comment, double totdeduct, double totAllow, string Ename, string Carscheme)
    {
        int RetId = 0;
        string newUrl = string.Empty;
        FPADALL Dal = new FPADALL();
        Utility.Service Util = new Utility.Service();
        string SerialNo = Dal.Serial_No("FPA");
        double retrialbcomp = 0, Basiccomp = 0;
        string year = Convert.ToString(DateTime.Now);
        //if (string.IsNullOrEmpty(Carscheme))
        //{
        //    Carscheme = "No Scheme";
        //}
        try
        {
            if (Convert.ToInt32(DesId) < 21)
            {
                RetId = Dal.InsertGMFPANewTest(DesId, Sapid, year, Comment, Convert.ToInt32(CTC), Convert.ToInt32(cBasic), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), SerialNo, Convert.ToInt32(totSalAvail), Carscheme);
            }
            else
            {
                RetId = Dal.InsertFPANewTest(DesId, Sapid, year, Comment, Convert.ToInt32(Fpa), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), SerialNo, Convert.ToInt32(totSalAvail), Carscheme);

            }

            if (RetId == 1)
            {
                newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + Ename + "<br/><br/>Your FPA has been initiated for Financial Year " + DateTime.Now.Year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                Util.SendMail(EamilId, "ewadmin@eicher.in", "", "Your FPA has been initiated ...", Mailbody);
                Dal.CreateLogHistory("0", FpaAdmin, Comment, SerialNo);
            }
        }
        catch (Exception ex)
        {

        }

        return RetId;
    }




    //  protected void btnIntiation_Click(object sender, EventArgs e)
    //    {
    //        double Fpa = 0, other1 = 0, other2 = 0, other3 = 0, totpetrol = 0, totAllow = 0, totSalAvail = 0, totreimavail = 0, clarental = 0;
    //        string newUrl = string.Empty;
    //        string EmailID = string.Empty;
    //        if (HFEMAILS.Value.Trim() == "")
    //        {
    //            EmailID = "";
    //        }
    //        else {EmailID =HFEMAILS.Value.ToString(); }

    //        double vehemi = 0, saf = 0, busdeduct = 0, totdeduct = 0, NetAmount = 0, Locfpa = 0, Basiccomp = 0, retrialbcomp = 0, cBasic = 0, CTC = 0;
    //        if (string.IsNullOrEmpty(HFLOC.Value) || string.IsNullOrEmpty(HFDESID.Value) || string.IsNullOrEmpty(HFUSERSAPID.Value) || Convert.ToDouble(txtNetAmount.Text) <= 0)
    //        {
    //            //if (string.IsNullOrEmpty(HFLOC.Value))
    //            //{
    //            //    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please select Location.');", true);
    //            //}
    //            //else if (ddlDesignation.SelectedValue == "0")
    //            //{
    //            //    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please select Designation);", true);
    //            //}
    //            //else if (ddlEmployee.SelectedValue == "0")
    //            //{
    //            //    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please select Employee.');", true);
    //            //}
    //            //else if (Convert.ToDouble(txtNetAmount.Text) <= 0)
    //            //{
    //                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please note Net Amount cannot be zero or negative.');", true);
    //            //}

    //        }
    //        else
    //        {
    //            try
    //            {
    //                string EmpSapId = HFUSERSAPID.Value;
    //                string DesId = HFDESID.Value;
    //                if (txtcBasic.Text == "")
    //                {
    //                    cBasic = 0;
    //                }
    //                else
    //                {
    //                    cBasic = Convert.ToDouble(txtcBasic.Text);
    //                }
    //                //if (txtretrialbcomponent.Text == "")
    //                //{
    //                //    retrialbcomp = 0;
    //                //}
    //                //else
    //                //{
    //                //    retrialbcomp = Convert.ToDouble(txtretrialbcomponent.Text);
    //                //}
    //                //if (txtBasiccomponent.Text == "")
    //                //{
    //                //    Basiccomp = 0;
    //                //}
    //                //else
    //                //{
    //                //    Basiccomp = Convert.ToDouble(txtBasiccomponent.Text);
    //                //}
    //                if (txtCTC.Text == "")
    //                {
    //                    CTC = 0;
    //                }
    //                else
    //                {
    //                    CTC = Convert.ToDouble(txtCTC.Text);
    //                }
    //                if (txtFPA.Text == "")
    //                {
    //                    Fpa = 0;
    //                }
    //                else
    //                {
    //                    Fpa = Convert.ToDouble(txtFPA.Text);
    //                }
    //                if (txtOther1.Text == "")
    //                {
    //                    other1 = 0;
    //                }
    //                else
    //                {
    //                    other1 = Convert.ToDouble(txtOther1.Text);
    //                }
    //                if (txtOther2.Text == "")
    //                {
    //                    other2 = 0;
    //                }
    //                else
    //                {
    //                    other2 = Convert.ToDouble(txtOther2.Text);

    //                }
    //                if (txtOther3.Text == "")
    //                {
    //                    other3 = 0;
    //                }
    //                else
    //                {
    //                    other3 = Convert.ToDouble(txtOther3.Text);
    //                }
    //                if (txtLocFPA.Text == "")
    //                {
    //                    Locfpa = 0;
    //                }
    //                else
    //                {
    //                    Locfpa = Convert.ToDouble(txtLocFPA.Text);
    //                }
    //                if (txtTotpcredited.Text == "")
    //                {
    //                    totpetrol = 0;
    //                }
    //                else
    //                {
    //                    totpetrol = Convert.ToDouble(txtTotpcredited.Text);
    //                }
    //                if (txtTotAllownce.Text == "")
    //                {
    //                    totAllow = 0;
    //                }
    //                else
    //                {
    //                    totAllow = Convert.ToDouble(txtTotAllownce.Text);

    //                }
    //                if (txtTotalsalavailed.Text == "")
    //                {
    //                    totSalAvail = 0;
    //                }
    //                else
    //                {
    //                    totSalAvail = Convert.ToDouble(txtTotalsalavailed.Text);
    //                }
    //                if (txtTotReimAvailed.Text == "")
    //                {
    //                    totreimavail = 0;
    //                }
    //                else
    //                {
    //                    totreimavail = Convert.ToDouble(txtTotReimAvailed.Text);
    //                }
    //                if (txtClaRental.Text == "")
    //                {
    //                    clarental = 0;
    //                }
    //                else
    //                {
    //                    clarental = Convert.ToDouble(txtClaRental.Text);
    //                }
    //                if (txtVehEmi.Text == "")
    //                {
    //                    vehemi = 0;
    //                }
    //                else
    //                {
    //                    vehemi = Convert.ToDouble(txtVehEmi.Text);
    //                }
    //                if (txtSAF.Text == "")
    //                {
    //                    saf = 0;
    //                }
    //                else
    //                {
    //                    saf = Convert.ToDouble(txtSAF.Text);
    //                }
    //                if (txtBusDeduct.Text == "")
    //                {
    //                    busdeduct = 0;
    //                }
    //                else
    //                {
    //                    busdeduct = Convert.ToDouble(txtBusDeduct.Text);
    //                }
    //                if (txtTotDeduct.Text == "")
    //                {
    //                    totdeduct = 0;
    //                }
    //                else
    //                {
    //                    totdeduct = Convert.ToDouble(txtTotDeduct.Text);
    //                }
    //                if (txtNetAmount.Text == "")
    //                {
    //                    NetAmount = 0;
    //                }
    //                else
    //                {
    //                    NetAmount = Convert.ToDouble(txtNetAmount.Text);
    //                }

    //                int TransID = 0;
    //                string year = Convert.ToString(DateTime.Now);

    //                if (Convert.ToInt32(DesId) < 21)
    //                {
    //                    TransID = Dal.InsertGMFPANew(DesId, EmpSapId, year, txtComments.Text, Convert.ToInt32(CTC), Convert.ToInt32(cBasic), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), HFID.Value, Convert.ToInt32(totSalAvail), "Carcsheme");
    //                }
    //                else
    //                {
    //                    TransID = Dal.InsertFPANew(DesId, EmpSapId, year, txtComments.Text, Convert.ToInt32(Fpa), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), HFID.Value, Convert.ToInt32(totSalAvail),"carscheme");

    //                }
    //                //if (Convert.ToInt32(DesId) < 21)
    //                //{
    //                    //Basiccomp = 0, retrialbcomp = 0,cBasic = 0,CTC=0;
    //                 //   TransID = Dal.InsertGMFPANew(DesId, EmpSapId, year, txtComments.Text, CTC, cBasic, other1, other2, other3, Locfpa, totpetrol, //totreimavail, clarental, vehemi, saf, busdeduct, Basiccomp, retrialbcomp, HFID.Value, totSalAvail);
    ////}
    //               // else
    //               // {
    //                //    TransID = Dal.InsertFPANew(DesId, EmpSapId, year, txtComments.Text, Fpa, other1, other2, other3, Locfpa, totpetrol, //totreimavail, clarental, vehemi, saf, busdeduct, HFID.Value, totSalAvail);

    //                //}
    //                if (TransID == 1)
    //                {
    //                    int yr =DateTime.Now.Year;
    //                    if (HFBU.Value == "REM")
    //                    {
    //                        newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
    //                    }
    //                    else
    //                    {
    //                        newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
    //                    }

    //                   // string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnEname.InnerText + "<br/><br/>Your FPA has been initiated for Financial Year " + yr + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";

    //                   // Util.SendMail("mkrajput@vecv.in", "ewadmin@eicher.in", "", "Your FPA has been initiated ...", Mailbody);
    //                   // Dal.CreateLogHistory("0", spnLoginUser.InnerText.ToString(), txtComments.Text,HFID.Value.ToString());
    //                   // ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('" + spnEname.InnerText + " FPA initiated successfully.');CloseRefreshWindow();", true);

    //                }
    //                else if (TransID == 0)
    //                {
    //                    btnIntiation.Enabled = true;
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
    //                }
    //                else if (TransID == 2)
    //                {
    //                    btnIntiation.Enabled = true;
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
    //                }
    //                else if (TransID == 5)
    //                {
    //                    btnIntiation.Enabled = true;
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data already submitted.');CloseRefreshWindow();", true);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
    //            }
    //        }
    //    }

    #endregion

    #region"Method"
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
        //txtBasiccomponent.Text = "0";
        //txtBasiccomponent.Enabled = false;
        //txtretrialbcomponent.Text = "0";
        //txtretrialbcomponent.Enabled = false;

    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            FpaAdmin = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            //spnSrNo.InnerText = Dal.Serial_No("FPA");
            //HFID.Value = spnSrNo.InnerText;
            HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
        }
    }
    protected object GetStatus(object Value)
    {
        string strStatus = string.Empty;
        if (Value.ToString() == "New Scheme")
        {
            strStatus = "Yes";
        }
        else
        {
            strStatus = "";
        }
        return strStatus;
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
        // return "drpillai";
    }
    #endregion
}
