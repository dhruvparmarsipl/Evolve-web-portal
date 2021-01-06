#region"Namespace"
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
#endregion

public partial class PetrolApprover : System.Web.UI.Page
{
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    #endregion

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                int checkstatus =0;// Dal.CheckUserStatus(getSAPID(ReturnLoginName()));
                if (checkstatus == 0)
                {
                    string serialNumber = DoDecrypt(Request.QueryString["RID"]);
                    FillHeaderonquerystring(getSAPID(ReturnLoginName()), serialNumber);
                    GetRate(spnLoc.InnerText.Trim());
                    SetInitialRow();
                    FillGrid(serialNumber);
                    BindHistory();
					if (HFSTATUS.Value == "3")
                    {
                    }
                    else
                    {
					int YearEnd = Dal.checkYearEndBySerailNoPterol(HFSAPID.Value, serialNumber);
                    if (YearEnd == 0)
                    {
                        btnSubmit.Visible = false;
                        btnReject.Visible = false;
                        btnDelete.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD1", "alert('Your year end has been marked. Please contact your FPA Admin .');", true);
                    }
					}
					checkstatus = Dal.CheckUserStatus(HFSAPID.Value);
                    if (checkstatus == 0)
                    {

                    }
                    else
                    {
                        btnSubmit.Visible = false;
                        btnReject.Visible = false;
                        btnDelete.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Not Applicable Please Contact Fpa Admin.');CloseRefreshWindow();", true);
                    }
                }
                else
                {
                    // do nothing 
                }
            }         
        }
    }
    #endregion

    #region"FillRateByLocation"
    private void GetRate(string Loc)
    {
        txtRatePerKm.InnerText = Dal.GetRateByLocation(Loc);
    }
    #endregion

    #region"FillHeaderBySapId"
    private void FillHeader(string sapid)
    {
        using (DataTable dt_UInfo = Dal.getclaimUserInfo(sapid))
        {
            if (dt_UInfo.Rows.Count > 0)
            {
                spnLoginUser.InnerText = spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ENAME from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'"); 
                spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
                spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
                spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
                spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
                spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIAdminLogin"]);
                spndepartment.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DEPARTMENT"]);
                spnSrNo.InnerText = Dal.Serial_No("PCL");
                HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
                HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
                HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
                HFCOSTCENTER.Value = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
                spnApprover.InnerText = Convert.ToString(dt_UInfo.Rows[0]["Approver"]);
                HFCARSCHEME.Value = Convert.ToString(dt_UInfo.Rows[0]["CAR_SCHEME"]).Trim();
                HFCARNO.Value = Convert.ToString(dt_UInfo.Rows[0]["car_no"]).Trim();
            }
        }
        validateForm();
    }
    #endregion

    #region"ValidateForm"
    private void validateForm()
    {
        if (spnFIAdmin.InnerText == "" || spnEmpCode.InnerText == "" || spnApprover.InnerText == "")
        {
            btnReject.Visible = false;
            btnSubmit.Visible = false;
        }
    }
    #endregion

    #region"FillHeaderBySerialNumber"
    private void FillHeaderonquerystring(string sapid, string serialNumber)
    {
        DataTable dt_UInfo = Dal.getPetrolClaimSerialNo(sapid, serialNumber);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ENAME from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'"); ;
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIAdminLogin"]);
            spndepartment.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DEPARTMENT"]);
            SpnStatus.InnerText = GetStatus(Convert.ToString(dt_UInfo.Rows[0]["CURRENT_STATUS"]));
            HFSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["CURRENT_STATUS"]);
            spnSrNo.InnerText = DoDecrypt(Request.QueryString["RID"]);
            HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
            HFCOSTCENTER.Value = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
            spnApprover.InnerText = Convert.ToString(dt_UInfo.Rows[0]["Approver"]);

        }
        validateForm();
        string Owner = Convert.ToString(dt_UInfo.Rows[0]["Owner"]).ToLower().Trim();
        if (Owner == ReturnLoginName().ToLower())
        {
            if (HFSTATUS.Value == "8")
            {
               //gvPetrolClaimDetail.Columns[2].ReadOnly = true;
                //gvPetrolClaimDetail.Enabled = false;
                btnSubmit.Visible = true;
                btnReject.Visible = true;
                txtComments.ReadOnly = true;
                txtFIAdminreject.ReadOnly = true;
                txtAppRejectComment.ReadOnly = false;
            }
            else if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
            {
                gvPetrolClaimDetail.Enabled = true;
                txtComments.ReadOnly = true;
                txtFIAdminreject.ReadOnly = false;
                txtAppRejectComment.ReadOnly = true;
                btnSubmit.Visible = true;
                btnReject.Visible = true;
                btnDelete.Visible = true;
                btnHold.Visible = true;
            }
            else
            {
                gvPetrolClaimDetail.Enabled = false;
                txtComments.ReadOnly = true;
                txtFIAdminreject.ReadOnly = true;
                txtAppRejectComment.ReadOnly = true;
                btnReject.Visible = false;
                btnSubmit.Visible = false;
            }
        }
        else
        {
            gvPetrolClaimDetail.Enabled = false;
            txtComments.ReadOnly = true;
            txtFIAdminreject.ReadOnly = true;
            txtAppRejectComment.ReadOnly = true;
            btnReject.Visible = false;
            btnSubmit.Visible = false;
        }

    }
    #endregion

    #region"GetFormStatus"
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
        if (status == "8")
        {
            cstatus = "Submitted to Approver";
        }
        if (status == "9")
        {
            cstatus = "Rejected by Approver";
        }
        if (status == "10")
        {
            cstatus = "On Hold";
        }
        return cstatus;
    }
    #endregion

    #region"FillClaimGrodviewBySerialNumber"
    private void FillGrid(string serialNumber)
    {
        using (DataTable fpadt = Dal.getPetroldetailData(serialNumber))
        {

            if (fpadt.Rows.Count > 0)
            {
                gvPetrolClaimDetail.DataSource = fpadt;
                gvPetrolClaimDetail.DataBind();
                int rownumber = 1;
                int rowIndex = 0;
                for (int i = 0; i < fpadt.Rows.Count; i++)
                {
                    Label lblRownumber = (Label)gvPetrolClaimDetail.Rows[rowIndex].Cells[1].FindControl("lblRownumber");
                    TextBox txtJourneyDate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[2].FindControl("txtJourneyDate");
                    TextBox txtRate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtRate");
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");
                     Label lblDetail = (Label)gvPetrolClaimDetail.Rows[rowIndex].Cells[9].FindControl("lblDetail");

                    lblRownumber.Text = Convert.ToString(rownumber);
                    txtJourneyDate.Text = Convert.ToString(fpadt.Rows[i]["DateOfJourney"]).Trim();
                    txtRate.Text = Convert.ToString(fpadt.Rows[i]["Rate"]).Trim();
                    txtStartPoint.Text = Convert.ToString(fpadt.Rows[i]["StartPoint"]).Trim();
                    txtEndPoint.Text = Convert.ToString(fpadt.Rows[i]["EndPoint"]).Trim();
                    txtTotalKM.Text = Convert.ToString(fpadt.Rows[i]["TotalKM"]).Trim();
                    txtBillAmount.Text = Convert.ToString(fpadt.Rows[i]["BillAmount"]).Trim();
                    txtDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]).Trim();
					 lblDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]).Trim();
                    HFCARSCHEME.Value = Convert.ToString(fpadt.Rows[i]["CAR_SCHEME"]).Trim();
                    HFCARNO.Value = Convert.ToString(fpadt.Rows[i]["CAR_NO"]).Trim();
                    rownumber++;
                    rowIndex++;

                }

                txtComments.Text = Convert.ToString(fpadt.Rows[0]["UserComment"]);
                txtAppRejectComment.Text = Convert.ToString(fpadt.Rows[0]["ApproverComment"]);
                txtFIAdminreject.Text = Convert.ToString(fpadt.Rows[0]["FIAdminComment"]);
                object totamount = fpadt.Compute("Sum(BillAmount)", "");
                spnTolalBillAmount.InnerText = Convert.ToString(totamount);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = fpadt;

                if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7" || HFSTATUS.Value == "9" || HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
                {
                }
                else
                {
                    foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
                    {
                        LinkButton lnkDelete = (LinkButton)grow.FindControl("lnkDelete");
                        TextBox txtJourneyDate = (TextBox)grow.FindControl("txtJourneyDate");
                        TextBox txtStartPoint = (TextBox)grow.FindControl("txtStartPoint");
                        TextBox txtEndPoint = (TextBox)grow.FindControl("txtEndPoint");
                        TextBox txtTotalKM = (TextBox)grow.FindControl("txtTotalKM");
                        TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
                        TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");    
						Label lblDetail = (Label)grow.FindControl("lblDetail"); 
                        lnkDelete.Visible = false;
                        txtJourneyDate.Enabled = false;
                        txtStartPoint.ReadOnly = true;
                        txtEndPoint.ReadOnly = true;
                        txtTotalKM.Enabled = false;
                        txtBillAmount.Enabled = false;
                        txtDetail.ReadOnly = true;
						txtDetail.Visible = false;
                        lblDetail.Visible = true;
                    }
                    Button lnkadd = (Button)gvPetrolClaimDetail.FooterRow.FindControl("ButtonAdd");
                    lnkadd.Visible = false;
                }
            }
        }
    }
    #endregion

    #region"DecryptSerialNumber"
    private string DoDecrypt(string EncrptedString)
    {
        string Decrypted = string.Empty;
        try
        {
            EncriporDecript.EncriporDecrip oDecrypt = new EncriporDecript.EncriporDecrip();
            Decrypted = oDecrypt.decryptQueryString(EncrptedString.Replace(" ", "+"));
        }
        catch (Exception ex)
        {

        }
        return Decrypted;
    }
    #endregion

    #region"SplitRetrieveLoginName"
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    #endregion

    #region"GetUserSapId"
    public string getSAPID(string Gid)
    {
        string SAPID = Dal.Get_Single_DataByPassingQuery("SELECT SAPID FROM MASTER_EMPLOYEE_PROFILE WHERE LOGIN_NAME ='" + Gid.Trim() + "'");
        return SAPID;
    }
    #endregion

    #region"AddNewRowInFPAClaimGridview"
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        try
        {
            AddNewRowToGrid();
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region"SetIntialRowOfGridView"
    private void SetInitialRow()
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("DateOfJourney", typeof(string)));
        dt.Columns.Add(new DataColumn("Rate", typeof(string)));
        dt.Columns.Add(new DataColumn("StartPoint", typeof(string)));
        dt.Columns.Add(new DataColumn("EndPoint", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalKM", typeof(string)));
        dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("DETAILS", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["DateOfJourney"] = string.Empty;
        dr["Rate"] = string.Empty;
        dr["StartPoint"] = string.Empty;
        dr["EndPoint"] = string.Empty;
        dr["TotalKM"] = string.Empty;
        dr["BillAmount"] = string.Empty;
        dr["DETAILS"] = string.Empty;
        dt.Rows.Add(dr);
        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        gvPetrolClaimDetail.DataSource = dt;
        gvPetrolClaimDetail.DataBind();
    }
    #endregion

    #region"AddRowInGridView"
    private void AddNewRowToGrid()
    {

        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    //DropDownList ddlClaimtype = (DropDownList)gvPetrolClaimDetail.Rows[rowIndex].Cells[1].FindControl("ddlClaimType");
                    TextBox txtJourneyDate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[2].FindControl("txtJourneyDate");
                    TextBox txtRate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtRate");
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["DateOfJourney"] = Convert.ToString(txtJourneyDate.Text);
                    dtCurrentTable.Rows[i - 1]["Rate"] = Convert.ToString(txtRate.Text);
                    dtCurrentTable.Rows[i - 1]["StartPoint"] = Convert.ToString(txtStartPoint.Text);
                    dtCurrentTable.Rows[i - 1]["EndPoint"] = Convert.ToString(txtEndPoint.Text);
                    dtCurrentTable.Rows[i - 1]["TotalKM"] = Convert.ToString(txtTotalKM.Text);
                    dtCurrentTable.Rows[i - 1]["BillAmount"] = Convert.ToString(txtBillAmount.Text);
                    dtCurrentTable.Rows[i - 1]["DETAILS"] = Convert.ToString(txtDetail.Text);

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;
                gvPetrolClaimDetail.DataSource = dtCurrentTable;
                gvPetrolClaimDetail.DataBind();
            }
        }
        else
        {

        }
        SetPreviousData();
    }
    #endregion

    #region"SetPreviousDataOfGridview"
    private void SetPreviousData()
    {

        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtJourneyDate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[2].FindControl("txtJourneyDate");
                    TextBox txtRate = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtRate");
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");


                    txtJourneyDate.Text = Convert.ToString(dt.Rows[i]["DateOfJourney"]);
                    txtRate.Text = Convert.ToString(dt.Rows[i]["Rate"]);
                    txtStartPoint.Text = Convert.ToString(dt.Rows[i]["StartPoint"]);
                    txtEndPoint.Text = Convert.ToString(dt.Rows[i]["EndPoint"]);
                    txtTotalKM.Text = Convert.ToString(dt.Rows[i]["TotalKM"]);
                    txtBillAmount.Text = Convert.ToString(dt.Rows[i]["BillAmount"]);
                    txtDetail.Text = Convert.ToString(dt.Rows[i]["DETAILS"]);

                    rowIndex++;

                }
            }
        }
    }
    #endregion

    #region"DeleteClaimFromGridview"
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["CurrentTable"] != null)
            {
                string rownumber = (sender as LinkButton).CommandArgument;
                using (DataTable dt = (DataTable)ViewState["CurrentTable"])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["RowNumber"].ToString() == rownumber)
                        {
                            dt.Rows.Remove(dr);
                            dt.AcceptChanges();
                            ViewState["CurrentTable"] = dt;
                            break;
                        }
                    }
                    //object totamount = dt.Compute("Sum(CLAIM_AMT)", "");
                    //spnTotalFpaClaim.InnerText = Convert.ToString(totamount);
                    if (dt.Rows.Count > 0)
                    {
                        gvPetrolClaimDetail.DataSource = dt;
                        gvPetrolClaimDetail.DataBind();
                    }
                    else
                    {
                        SetInitialRow();
                    }
                    SetPreviousData();
                }
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region"ClaimAmountChangeFromGridview"
    protected void txtTotalKM_TextChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        double BillAmount = 0.0;
        TextBox txt = (sender as TextBox);
        GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
        TextBox txtTotalKM = (TextBox)gvrow.FindControl("txtTotalKM");
        TextBox txtBillAmount = (TextBox)gvrow.FindControl("txtBillAmount");

        TextBox txtRate = (TextBox)gvrow.FindControl("txtRate");
        if (txtRate.Text == "")
        {
            //txtRatePerKm.InnerText = "0";
            txtRate.Text = "0";
        }
        if (txtTotalKM.Text == "")
        {
            txtTotalKM.Text = "0";
        }
        //txtBillAmount.Text = Convert.ToString(Convert.ToDouble(txtTotalKM.Text) * Convert.ToDouble(txtRatePerKm.InnerText));
        txtBillAmount.Text = Convert.ToString(Convert.ToDouble(txtTotalKM.Text) * Convert.ToDouble(txtRate.Text));
        foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
        {
            TextBox PetrolBillAmount = (TextBox)grow.FindControl("txtBillAmount");
            if (PetrolBillAmount.Text == "")
            {
                PetrolBillAmount.Text = "0";
            }
            BillAmount = Convert.ToDouble(BillAmount) + Convert.ToDouble(PetrolBillAmount.Text);
            spnTolalBillAmount.InnerText = Convert.ToString(BillAmount);

        }

    }
    #endregion

    #region"Approve By Approver or FI Admin"
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnReject.Enabled = false;
            btnSubmit.Enabled = false;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string status = string.Empty;
            string EmailID = string.Empty;  // FI Admin Email ID
            int TransID = 0;
            if (Convert.ToDouble(spnTolalBillAmount.InnerText) >= 500)
            {
                if (HFSTATUS.Value == "8")
                {
                   EmailID= HFEMAILS.Value.ToString().Split('#')[2];
                    status = "2";
                    TransID = Dal.UpdatePterolClaimDataBySRNO(spnSrNo.InnerText, status,txtAppRejectComment.Text); 
                    if (TransID == 1)
                    {
                        CommonMail(EmailID.Trim(), status);
                        Dal.CreateRejectLogHistory(status, spnLoginUser.InnerText.ToString(), Convert.ToString(txtAppRejectComment.Text), spnSrNo.InnerText.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is submitted to FI Admin for Approval.');CloseRefreshWindow();", true);
                    }
                    else if (TransID == 0)
                    {
                        btnSubmit.Enabled = true;
                        btnReject.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                    }
                    else if (TransID == 5)
                    {
                        btnSubmit.Enabled = true;
                        btnReject.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                    }
                }
                else if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
                {
                    status = "3";
                    EmailID = HFEMAILS.Value.ToString().Split('#')[0];
                    string XmlFpaClaimDetail = GetPetrolClaimDetail(status);
                    string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
                    DataTable cdt = (DataTable)ViewState["dt"];
                    object Totalamount = cdt.Compute("sum(AMOUNT)","");
                    TransID = Dal.InsertPetrolClaimDetail(XmlFpaClaimDetail, spnSrNo.InnerText, "600200", ReturnLoginName(), HFSAPID.Value, txtComments.Text, status, FpaCyleId, Convert.ToDouble(Totalamount), HFBU.Value);
                   
                    if (TransID == 1)
                    {
                        CommonMail(EmailID.Trim(), status);
                        Dal.CreateRejectLogHistory(status, spnLoginUser.InnerText.ToString(), Convert.ToString(txtComments.Text), spnSrNo.InnerText.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is Approved Suucessfully.');CloseRefreshWindow();", true);
                    }
                    else if (TransID == 0)
                    {
                        btnSubmit.Enabled = true;
                        btnReject.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                    }
                    else if (TransID == 5)
                    {
                        btnSubmit.Enabled = true;
                        btnReject.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                    }
                }
            }
            else
            {
                btnSubmit.Enabled = true;
                btnReject.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum  Petrol Claim amount is INR 500 ')", true);
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = true;
            btnReject.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }


    }
    #endregion

    #region"Reject by approver Or FI Admin"
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnReject.Enabled = false;
            btnSubmit.Enabled = false;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string status = string.Empty;
            string EmailID = string.Empty;  // FI Admin Email ID
            int TransID = 0;
            if (Convert.ToDouble(spnTolalBillAmount.InnerText) >= 500)
            {
                if (HFSTATUS.Value == "8")
                {
                    if (string.IsNullOrEmpty(txtAppRejectComment.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please enter comments for rejection.');", true);
                    }
                    {
                        status = "9";
                        EmailID= HFEMAILS.Value.ToString().Split('#')[0];
                        TransID = Dal.UpdatePterolClaimDataBySRNO(spnSrNo.InnerText, status, txtAppRejectComment.Text);
                        if (TransID == 1)
                        {
                            CommonMail(EmailID.Trim(), status);
                            Dal.CreateRejectLogHistory(status, spnLoginUser.InnerText.ToString(), Convert.ToString(txtAppRejectComment.Text), spnSrNo.InnerText.ToString());
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Form rejected successfully.');CloseRefreshWindow();", true);
                        }
                        else if (TransID == 0)
                        {
                            btnSubmit.Enabled = true;
                            btnReject.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                        }
                        else if (TransID == 5)
                        {
                            btnSubmit.Enabled = true;
                            btnReject.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                        }
                    }
                }
                else if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
                {
                    status = "7";
                    EmailID = HFEMAILS.Value.ToString().Split('#')[0];
                    if (string.IsNullOrEmpty(txtFIAdminreject.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please enter comments for rejection.');", true);
                    }
                    {
                        TransID = Dal.UpdatePterolClaimDataBySRNO(spnSrNo.InnerText, status, txtFIAdminreject.Text);
                        if (TransID == 1)
                        {
                            CommonMail(EmailID.Trim(), status);
                            Dal.CreateRejectLogHistory(status, spnLoginUser.InnerText.ToString(), Convert.ToString(txtFIAdminreject.Text), spnSrNo.InnerText.ToString());
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Form rejected successfully.');CloseRefreshWindow();", true);
                        }
                        else if (TransID == 0)
                        {
                            btnSubmit.Enabled = true;
                            btnReject.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                        }
                        else if (TransID == 5)
                        {
                            btnSubmit.Enabled = true;
                            btnReject.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                        }
                    }
                }
            }
            else
            {
                btnSubmit.Enabled = true;
                btnReject.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum  Petrol Claim amount is INR 500 ')", true);
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = true;
            btnReject.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }


    }
    #endregion

    #region"UserPetrolClaimDetail"
    private string GetPetrolClaimDetail(string status)
    {
        string XmlDataGridItems = string.Empty;
        using (DataTable dt = new DataTable("PetrolClaim"))
        {
            dt.Columns.Add("FPA_CYCLE_ID", typeof(string));
            dt.Columns.Add("START_KM", typeof(string));
            dt.Columns.Add("END_KM", typeof(string));
            dt.Columns.Add("AMOUNT", typeof(float));
            dt.Columns.Add("CAR_SCHEME", typeof(string));
            dt.Columns.Add("CAR_NO", typeof(string));
            dt.Columns.Add("CURRENT_STATUS", typeof(string));
            dt.Columns.Add("SERIAL_NO", typeof(string));
            dt.Columns.Add("TotalKM", typeof(int));
            dt.Columns.Add("Detail", typeof(string));
            dt.Columns.Add("JourneyDate", typeof(string));
            dt.Columns.Add("Rate", typeof(string));

            string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
            foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
            {
                TextBox txtJourneyDate = (TextBox)grow.FindControl("txtJourneyDate");
                TextBox txtRate = (TextBox)grow.FindControl("txtRate");
                TextBox txtStartPoint = (TextBox)grow.FindControl("txtStartPoint");
                TextBox txtEndPoint = (TextBox)grow.FindControl("txtEndPoint");
                TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
                TextBox txtTotalKM = (TextBox)grow.FindControl("txtTotalKM");
                TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");

                DataRow dr = dt.NewRow();
                dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
                dr["JourneyDate"] = Convert.ToString(txtJourneyDate.Text);
                dr["Rate"] = Convert.ToString(txtRate.Text);
                dr["START_KM"] = Convert.ToString(txtStartPoint.Text);
                dr["END_KM"] = Convert.ToString(txtEndPoint.Text);
                dr["AMOUNT"] = Convert.ToDouble(txtBillAmount.Text);
                dr["CAR_SCHEME"] = Convert.ToString(HFCARSCHEME.Value);
                if (string.IsNullOrEmpty(HFCARNO.Value))
                {
                    HFCARNO.Value = "0";
                }
                dr["CAR_NO"] = Convert.ToString(HFCARNO.Value);
                dr["SERIAL_NO"] = Convert.ToString(spnSrNo.InnerText);
                dr["TotalKM"] = Convert.ToInt32(txtTotalKM.Text);
                dr["Detail"] = Convert.ToString(txtDetail.Text); 
                dr["CURRENT_STATUS"] = '3';
                dt.Rows.Add(dr);

            }
             ViewState["dt"]=dt;

            StringWriter strFpaHeadWriter = new StringWriter();
            dt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
            XmlDataGridItems = strFpaHeadWriter.ToString();
        }
        return XmlDataGridItems;
    }
    #endregion

    #region"CommonMailFunction"
    public void CommonMail(string MailTo, string Status)
    {
        string MailbodyUser = string.Empty;
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string subjectUser = string.Empty;
        string newUrl = string.Empty;
        string SerailNo = spnSrNo.InnerText.ToString();
        string Employeename = Convert.ToString(spnLoginUser.InnerText);
        string Approvername = Convert.ToString(spnLoginUser.InnerText);
        string comment = string.Empty;
        if (spnBU.InnerText.Trim() == "REM")
        {
            if (Status == "9" || Status == "7" || Status == "8" || Status == "2")
            {
                newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
            }
            else
            {
                newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/ProcessedClaim.aspx";
            }
        }
        else
        {
            if (Status == "9" || Status == "7" || Status == "8" || Status == "2")
            {
                newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
            }
            else
            {
                newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/Pages/FPAProcessedClaims.aspx";
            }
        }
        try
        {
            if (Status == "9")
            {
                if (txtAppRejectComment.Text.Length <= 250)
                {
                    comment = Convert.ToString(txtAppRejectComment.Text);
                }
                else
                {
                    comment = Convert.ToString(txtAppRejectComment.Text).Substring(0, 250);
                } 
                //comment = Convert.ToString(txtApproverComment.Text);
                Subject = "FPA Petrol Claim request " + SerailNo + " has been rejected...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Petrol Claim request <b>" + SerailNo + "</b> has been rejected by " + Approvername + ". Reason for rejecting the request is <br/><br/> \"<span style='color:red;'>" + comment + "</span>\"<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            else if (Status == "7")
            {
                if (txtFIAdminreject.Text.Length <= 250)
                {
                    comment = Convert.ToString(txtFIAdminreject.Text);
                }
                else
                {
                    comment = Convert.ToString(txtFIAdminreject.Text).Substring(0, 250);
                }
                //comment = Convert.ToString(txtApproverComment.Text);
                Subject = "FPA Petrol Claim request " + SerailNo + " has been rejected...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Petrol Claim request <b>" + SerailNo + "</b> has been rejected by " + Approvername + ". Reason for rejecting the request is <br/><br/> \"<span style='color:red;'>" + comment + "</span>\"<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            else if (Status == "3")
            {
                Subject = "FPA Petrol Claim request " + SerailNo + " has been approved...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Petrol Claim request has been approved by " + Approvername + ".<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            else if (Status == "2")
            {
                subjectUser = "FPA Petrol Claim request " + SerailNo + " has been approved...";
                MailbodyUser = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Petrol Claim request  <b>" + SerailNo + "</b> has been approved by " + Approvername + ".<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear Sir/Madam,<br /><br />A FPA Petrol Claim request has been submitted by <strong>" + spnFormCreater.InnerText + "</strong> for your approval. Please <a href=\"" + newUrl + "\">click here</a> to take appropriate action.<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Subject = "FPA Petrol Claim request for your approval...";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
                Util.SendMail(Convert.ToString(HFEMAILS.Value.Split('#')[0].Trim()), "ewadmin@eicher.in", "", subjectUser, MailbodyUser);
            }
            
        }
        catch (Exception ex)
        {
            // Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion

    #region"ShowHistory"
    public void BindHistory()
    {
        string Srno = Convert.ToString(spnSrNo.InnerText);
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
    #endregion

    #region"Delete Claim"
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string UserType = string.Empty;
        string Comment = string.Empty;
        string owner = getSAPID(ReturnLoginName());
        if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
        {
            UserType = "FIAdmin";
            Comment = txtFIAdminreject.Text;
            if (!string.IsNullOrEmpty(Comment))
            {
                int DelId = Dal.DeleteFpaClaim(spnSrNo.InnerText, "1");
                if (DelId == 1)
                {
                    int RetId = Dal.DeleteTravelLogHistory(spnSrNo.InnerText, owner, Comment, "PCL", UserType, HFSAPID.Value);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('Your claim deleted successfully');CloseRefreshWindow();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please try after sometime.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please enter deletion comment.');", true);
            }
        }
    }
    #endregion

    protected void btnHold_Click(object sender, EventArgs e)
    {
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string newUrl = string.Empty;
        string comment = string.Empty;
        string SerailNo = spnSrNo.InnerText.ToString();
        newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
        string EmailID = HFEMAILS.Value.ToString().Split('#')[0];
        string FiEmailID = HFEMAILS.Value.ToString().Split('#')[2];
        if (txtFIAdminreject.Text.Length <= 250)
        {
            comment = Convert.ToString(txtFIAdminreject.Text);
        }
        else
        {
            comment = Convert.ToString(txtFIAdminreject.Text).Substring(0, 250);
        }
        if (!string.IsNullOrEmpty(comment)) 
        {
            int DelId = Dal.PCLUpdateClaimstatustoHold(spnSrNo.InnerText, "10", txtFIAdminreject.Text);
            if (DelId == 1)  
            {
                Dal.CreateRejectLogHistory("10", spnLoginUser.InnerText.ToString(), comment, spnSrNo.InnerText.ToString());
                Subject = "Petrol Claim request " + SerailNo + " has been put on hold...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your Petrol Claim request with serial no. <b>" + SerailNo + "</b> has been put on hold .<br/>FI Admin comment: \"<span style='color:red;'>" + comment + "</span>\"<br />Please  click on  the link below for detail of claim.<br/><br/><a href=\"" + newUrl + "\">Click here to go to PCL home Page</a><br/>Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", FiEmailID.Trim(), Subject, Mailbody);
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Form onhold successfully.');CloseRefreshWindow();", true);
            }
            else
            { 
                 ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please try after sometime.');", true);
            } 

        }
        else
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please enter comment.');", true);
        }

    }

    protected void txtJourneyDate_TextChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        double BillAmount = 0.0;
        TextBox txt = (sender as TextBox);
        GridViewRow gvrow = (GridViewRow)txt.NamingContainer;

        string jnDate = ((TextBox)gvrow.FindControl("txtJourneyDate")).Text;
        TextBox txtTotalKM = (TextBox)gvrow.FindControl("txtTotalKM");
        TextBox txtBillAmount = (TextBox)gvrow.FindControl("txtBillAmount");
        TextBox txtRate = (TextBox)gvrow.FindControl("txtRate");
        string RateAmt = Dal.GetPetrolRate(HFBU.Value, jnDate);
        if (string.IsNullOrEmpty(RateAmt))
            txtRate.Text = "0";
        else
            txtRate.Text = RateAmt;    

        if (txtRate.Text == "")
        {
            //txtRatePerKm.InnerText = "0";
            txtRate.Text = "0";
        }
        if (txtTotalKM.Text == "")
        {
            txtTotalKM.Text = "0";
        }
        txtBillAmount.Text = Convert.ToString(Convert.ToDouble(txtTotalKM.Text) * Convert.ToDouble(txtRate.Text));
        foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
        {
            TextBox PetrolBillAmount = (TextBox)grow.FindControl("txtBillAmount");
            if (PetrolBillAmount.Text == "")
            {
                PetrolBillAmount.Text = "0";
            }
            BillAmount = Convert.ToDouble(BillAmount) + Convert.ToDouble(PetrolBillAmount.Text);
            spnTolalBillAmount.InnerText = Convert.ToString(BillAmount);

        }
    }

    #region"GetUserLoginName"
    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "spstestnr";
        //return "Tbetestuser";
    }
    #endregion
   
}
