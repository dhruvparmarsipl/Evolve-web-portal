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
using System.Web.Services;
using System.Collections.Generic;
public partial class FPAReIntiation : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    static string FpaAdmin = string.Empty;
    UtilityNew.Service Util = new UtilityNew.Service();
    #region"Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFPA.Focus();
            HFSAPID.Value = getSAPID(ReturnLoginName());
            FillHeader(HFSAPID.Value);
            SetIntialValue();
            FillUser();
           
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
    protected void gvUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
              //Label lvlCarscheme = (Label)e.Row.FindControl("lblCarcsheme");
            //string  carscheme ="New Scheme";// Dal.getFpaClaimHead(getSAPID(ReturnLoginName()));
            //DropDownList ddlCarScheme = (DropDownList)e.Row.FindControl("ddlCarScheme");
            //ddlCarScheme.SelectedValue = lvlCarscheme.Text;
            string getValue = (string)((GridView)sender).DataKeys[e.Row.RowIndex].Value;
            if (getValue == "1")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
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
    protected object GetPendingStatus(object Value)
    {
        string strStatus = string.Empty;
        if (Value.ToString() == "1")
        {
            strStatus = "Claim Detail";
        }
        else
        {
            strStatus = "";
        }
        return strStatus;
    }
    public void FillUser()
    {
        DataTable UserDetail = Dal.GetUserFpaReInitiateDetail( HFSAPID.Value);
        if (UserDetail.Rows.Count > 0)
        {
            gvUser.DataSource = UserDetail;
            gvUser.DataBind();
        }
    }
    [WebMethod]
    public static ClaimDetail[] FillPendingDetail(string Sapid)
    {
        FPADALL Dal = new FPADALL();
        List<ClaimDetail> objUserclaimdetainlist = new List<ClaimDetail>();
        DataTable fpadetail = Dal.GetPendingFpaDetail(Sapid);
        if (fpadetail.Rows.Count > 0)
        {
            foreach (DataRow dr in fpadetail.Rows)
            {
                ClaimDetail Userclaim = new ClaimDetail();
                Userclaim.Ename = dr["Ename"].ToString();
                Userclaim.Ecode = dr["Ecode"].ToString();
                Userclaim.SerialNo = dr["SerialNo"].ToString();
                Userclaim.CreatedDate = dr["CreateDate"].ToString();
                Userclaim.Status = dr["Status"].ToString();
                objUserclaimdetainlist.Add(Userclaim);
            }
        }
        return objUserclaimdetainlist.ToArray();
    }
    [WebMethod]
    public static UserFpa[] FillFpaDetail(string Sapid)
    {
        FPADALL Dal = new FPADALL();
        List<UserFpa> objUserFpalist = new List<UserFpa>();
        DataTable fpadetail = Dal.GetFpaDetail(Sapid);
        if (fpadetail.Rows.Count > 0)
        {
            foreach (DataRow dr in fpadetail.Rows)
            {
                UserFpa UserFpa = new UserFpa();
                UserFpa.FpaId = dr["FPA_ID"].ToString();
                UserFpa.Fpa = dr["CTC_OR_FPA"].ToString().Trim();
                UserFpa.Ctc = dr["CTC_OR_FPA"].ToString().Trim();
                UserFpa.CBasic = dr["CURRENT_BASIC"].ToString().Trim();
                UserFpa.Other1 = dr["OTHERS_1"].ToString().Trim();
                UserFpa.Other2 = dr["OTHERS_2"].ToString().Trim();
                UserFpa.Other3 = dr["OTHERS_3"].ToString().Trim();
                UserFpa.Totpcredited = Convert.ToString(Dal.getTotalPterolAmountById(Sapid)).Trim();
                UserFpa.LocFPA = dr["LOCATIONAL_FPA"].ToString().Trim();
                UserFpa.Basiccomponent = Convert.ToString(dr["BASIC_RETIRALS_AVAILED"].ToString().Trim());
                UserFpa.Totalsalavailed = dr["TOTAL_TAXABLE_AMT_AVAILED"].ToString().Trim();
                UserFpa.TotReimAvailed = Convert.ToString(Dal.GetTotalRemAmount(Sapid)).Trim();
                UserFpa.ClaRental = dr["CLA_RENTAL"].ToString().Trim();
                UserFpa.VehEmi = dr["VEHICLE_EMI"].ToString().Trim();
                UserFpa.SAF = dr["SCHOOL_SUBSIDY_DEDUCTION"].ToString().Trim();
                UserFpa.BusDeduct = dr["BUS_DEDUCTION"].ToString().Trim();
                UserFpa.BasicSalary = dr["BasicSalary"].ToString().Trim();
               // UserFpa.NPS = dr["NPS"].ToString().Trim();
                objUserFpalist.Add(UserFpa);
            }
        }
        return objUserFpalist.ToArray();
    }

    [WebMethod]
    public static int SubmitData(double Fpa, double CTC, double cBasic, double other1, double other2, double other3, double totpetrol,
      double Locfpa, double busdeduct, double saf, double vehemi, double totSalAvail, double totreimavail, double clarental, string Sapid, string DesId, string EamilId, double NetAmount, string Comment, double totdeduct, double totAllow, string Ename, string FpaId, double Basiccomponent, string Carscheme,double BasicSalary)
    {
        int RetId = 0;
        string newUrl = string.Empty;
        FPADALL Dal = new FPADALL();
        Utility.Service Util = new Utility.Service();
        string SerialNo = Dal.Serial_No("FPARE");
        double retrialbcomp = 0,Basiccomp = Basiccomponent;
        string year = Convert.ToString(DateTime.Now);
        if (string.IsNullOrEmpty(Carscheme))
        {
            Carscheme = "No Scheme";
        }
        try
        {
            if (Convert.ToInt32(DesId) < 21)
            {
                RetId = Dal.reinitiationGMFPANewTest(FpaId, DesId, Sapid, year, Comment, Convert.ToInt32(CTC), Convert.ToInt32(cBasic), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), Convert.ToInt32(Basiccomp), Convert.ToInt32(retrialbcomp), SerialNo, Convert.ToInt32(totSalAvail), Carscheme);
            }
            else
            {
                RetId = Dal.reinitiationTest(FpaId, DesId, Sapid, year, Comment, Convert.ToInt32(Fpa), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), SerialNo, Convert.ToInt32(totSalAvail), Carscheme, Convert.ToInt32(BasicSalary));

            }

            if (RetId == 1)
            {
                newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + Ename + "<br/><br/>Your FPA has been reinitiated for Financial Year " + DateTime.Now.Year + "<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks.<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
                Util.SendMail(EamilId, "ewadmin@eicher.in", "", "Your FPA has been reinitiated...", Mailbody);
                Dal.CreateLogHistory("0", FpaAdmin, Comment, SerialNo);
            }
        }
        catch (Exception ex)
        {

        }
        return RetId;
    }



   
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
        txtBasiccomponent.Text = "0";
        txtBasiccomponent.Enabled = false;
        txtBasicSalary.Text = "0";
        txtBasicSalary.Enabled = false;
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
        //return "spstestss";
    }
    
  
    #endregion
   
}
