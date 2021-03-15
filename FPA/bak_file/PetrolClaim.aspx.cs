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
public partial class PetrolClaim : System.Web.UI.Page
{
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
	static string logfile = "D:\\Eicher\\All Logs\\FPA";
    #endregion

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                int chkFPAisActive = Dal.CheckFPA_IsActive(getSAPID(ReturnLoginName())); //  check for FPA Activation
                int checkstatus = Dal.CheckUserStatus(getSAPID(ReturnLoginName()));
                if (chkFPAisActive == 1)
                {
                    if (checkstatus == 0)
                    {
                        string serialNumber = DoDecrypt(Request.QueryString["RID"]);
                        FillHeaderonquerystring(getSAPID(ReturnLoginName()), serialNumber);
                        GetRate(spnLoc.InnerText.Trim());
                        SetInitialRow();
                        FillGrid(serialNumber);
                        BindHistory();
                        int YearEnd = Dal.checkYearEndBySerailNoPterol(HFSAPID.Value, serialNumber);
                        if (YearEnd == 0)
                        {
                            btnSaveDraft.Visible = false;
                            btnSubmit.Visible = false;
                            btnDelete.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD1", "alert('Your year end has been marked. Please contact your FPA Admin .');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Not Applicable Please Contact Fpa Admin.');CloseRefreshWindow();", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyDisact", "alert('New FPA Claim Creation is disabled for Year-end FPA Closure/Re-initiation. For more clarification pls. contact your HR Partner.');CloseRefreshWindow();", true);
                }
            }
            else
            {
                int chkFPAisActive = Dal.CheckFPA_IsActive(getSAPID(ReturnLoginName())); //  check for FPA Activation
			    int checkYearEnd = Dal.CheckYearEnd(getSAPID(ReturnLoginName()));
                int InitiateStatus = Dal.CheckInitiateStatus(getSAPID(ReturnLoginName()));
                string carscheme = Dal.CheckCarscheme(getSAPID(ReturnLoginName()));
                if (chkFPAisActive == 1)
                {
                    if (checkYearEnd != 0 && InitiateStatus == 0 && carscheme == "New Scheme")
                    {
                        FillHeader(getSAPID(ReturnLoginName()));
                        GetRate(spnLoc.InnerText.Trim());
                        SetInitialRow();
                    }
                    else
                    {
                        if (checkYearEnd == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD1", "alert('Your year end has been marked. Please contact your FPA Admin .');CloseRefreshWindow();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Not Applicable Please Contact Fpa Admin.');CloseRefreshWindow();", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyDisact", "alert('New FPA Claim Creation is disabled for Year-end FPA Closure/Re-initiation. For more clarification pls. contact your HR Partner.');CloseRefreshWindow();", true);
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
                spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
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
            btnSaveDraft.Visible = false;
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
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
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
            if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7"||HFSTATUS.Value == "9")
            {
			 btnDelete.Visible = true;  
            }
            else
            {
                gvPetrolClaimDetail.Enabled = false;
                txtComments.ReadOnly = true;
                btnSaveDraft.Visible = false;
                btnSubmit.Visible = false;
				 btnDelete.Visible = false;  
            }
        }
        else
        {
            gvPetrolClaimDetail.Enabled = false;
            txtComments.ReadOnly = true;
            btnSaveDraft.Visible = false;
            btnSubmit.Visible = false;
			btnDelete.Visible = false;  
        }

    }
    #endregion

    #region"GetFormStatus"
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
        if (status == "8")
        {
            cstatus = "Submitted to Approver";
        }
        if (status == "9")
        {
            cstatus = "Rejected by Approver";
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
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtDetail");


                    lblRownumber.Text = Convert.ToString(rownumber);
                    txtJourneyDate.Text = Convert.ToString(fpadt.Rows[i]["DateOfJourney"]).Trim();
                    txtStartPoint.Text = Convert.ToString(fpadt.Rows[i]["StartPoint"]).Trim();
                    txtEndPoint.Text = Convert.ToString(fpadt.Rows[i]["EndPoint"]).Trim();
                    txtTotalKM.Text = Convert.ToString(fpadt.Rows[i]["TotalKM"]).Trim();
                    txtBillAmount.Text = Convert.ToString(fpadt.Rows[i]["BillAmount"]).Trim();
                    txtDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]).Trim();
                    HFCARSCHEME.Value = Convert.ToString(fpadt.Rows[i]["CAR_SCHEME"]).Trim();
                    HFCARNO.Value = Convert.ToString(fpadt.Rows[i]["CAR_NO"]).Trim();
                    rownumber++;
                    rowIndex++;

                }

                txtComments.Text = Convert.ToString(fpadt.Rows[0]["UserComment"]);
                object totamount = fpadt.Compute("Sum(BillAmount)", "");
                spnTolalBillAmount.InnerText = Convert.ToString(totamount);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = fpadt;

                if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7")
                {
                }
                else
                {
                    foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
                    {
                        LinkButton lnkDelete = (LinkButton)grow.FindControl("lnkDelete");
                        lnkDelete.Visible = false;
                    }
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

    #region"GetUserLoginName"
    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        // return "akhanna";
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
        dt.Columns.Add(new DataColumn("StartPoint", typeof(string)));
        dt.Columns.Add(new DataColumn("EndPoint", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalKM", typeof(string)));
        dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("DETAILS", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["DateOfJourney"] = string.Empty;
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
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtDetail");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["DateOfJourney"] = Convert.ToString(txtJourneyDate.Text);
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
                    TextBox txtStartPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtStartPoint");
                    TextBox txtEndPoint = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtEndPoint");
                    TextBox txtTotalKM = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtTotalKM");
                    TextBox txtBillAmount = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvPetrolClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtDetail");


                    txtJourneyDate.Text = Convert.ToString(dt.Rows[i]["DateOfJourney"]);
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
        if (txtRatePerKm.InnerText == "")
        {
            txtRatePerKm.InnerText = "0";
        }
        if (txtTotalKM.Text == "")
        {
            txtTotalKM.Text = "0";
        }
        txtBillAmount.Text = Convert.ToString(Convert.ToDouble(txtTotalKM.Text) * Convert.ToDouble(txtRatePerKm.InnerText));
        foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
        {
            TextBox PetrolBillAmount = (TextBox)grow.FindControl("txtBillAmount");
            if (PetrolBillAmount.Text == "")
            {
                PetrolBillAmount.Text = "0";
            }
            BillAmount = Convert.ToDouble(BillAmount) + Convert.ToDouble(PetrolBillAmount.Text);
            spnTolalBillAmount.InnerText=Convert.ToString(BillAmount);

        }

    }
    #endregion

    #region"SubmitByUser"
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnSaveDraft.Enabled = false;
            btnSubmit.Enabled = false;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string EmailID = HFEMAILS.Value.ToString().Split('#')[1]; // FI Admin Email ID
            string XmlFpaClaimDetail = GetPetrolClaimDetail("8");
            int TransID = 0;
            if (Convert.ToDouble(spnTolalBillAmount.InnerText) >= 500)
            {
                TransID = Dal.InsertPetrolClaimD(XmlFpaClaimDetail, spnSrNo.InnerText, "600200", ReturnLoginName(), HFSAPID.Value, txtComments.Text,"8");
                if (TransID == 1)
                {
                    CommonMail(EmailID.Trim(), "8");
                    Dal.CreateRejectLogHistory("8", spnLoginUser.InnerText.ToString(), Convert.ToString(txtComments.Text), spnSrNo.InnerText.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is submitted to Approver for Approval.');CloseRefreshWindow();", true);
                }
                else if (TransID == 0)
                {
                    btnSubmit.Enabled = true;
                    btnSaveDraft.Enabled = true;
					Util.Log("-- Transaction Failed inside submit --" + ReturnLoginName(), logfile);
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                }
                else if (TransID == 5)
                {
                    btnSubmit.Enabled = true;
                    btnSaveDraft.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                }
            }
            else
            {
                btnSubmit.Enabled = true;
                btnSaveDraft.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum  Pterol Claim amount is INR 500 ')", true);
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
			Util.Log("-- catch inside submit --" + ex.Message.ToString() + "--" + ReturnLoginName(), logfile);
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

            string FpaCyleId = Dal.GetFpaCyleId((getSAPID(ReturnLoginName())));
            foreach (GridViewRow grow in gvPetrolClaimDetail.Rows)
            {
                TextBox txtJourneyDate = (TextBox)grow.FindControl("txtJourneyDate");
                TextBox txtStartPoint = (TextBox)grow.FindControl("txtStartPoint");
                TextBox txtEndPoint = (TextBox)grow.FindControl("txtEndPoint");
                TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
                TextBox txtTotalKM = (TextBox)grow.FindControl("txtTotalKM");
                TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");

                DataRow dr = dt.NewRow();
                dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
                dr["JourneyDate"] = Convert.ToString(txtJourneyDate.Text);
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
                if (status == "8")
                {
                    dr["CURRENT_STATUS"] = '8';
                }
                else
                {
                    dr["CURRENT_STATUS"] = '1';
                }
                dt.Rows.Add(dr);

            }


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
            newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
        else
            newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
        try
        {
            if (txtComments.Text.Length <= 250)
            {
                comment = Convert.ToString(txtComments.Text);
            }
            else
            {
                comment = Convert.ToString(txtComments.Text).Substring(0, 250);
            }
            Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnFormCreater.InnerText.ToString() + "<br/><br/>Your Petrol Claim request <b>" + SerailNo + "</b> has been Submitted <br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
            Subject = "FPA Petrol Claim request " + SerailNo + " has been Submitted...";
            Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
        }
        catch (Exception ex)
        {
            // Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion

    #region"SaveasDraftByUser"
    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnSaveDraft.Enabled = false;
            btnSubmit.Enabled = false;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string XmlFpaClaimDetail = GetPetrolClaimDetail("1");
            int TransID = 0;
            TransID = Dal.InsertPetrolClaimD(XmlFpaClaimDetail, spnSrNo.InnerText, "600200", ReturnLoginName(), HFSAPID.Value, txtComments.Text,"1");
            if (TransID == 1)
            {
                Dal.CreateRejectLogHistory("1", spnLoginUser.InnerText.ToString(), Convert.ToString(txtComments.Text), spnSrNo.InnerText.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is saved successfully.');CloseRefreshWindow();", true);
            }
            else if (TransID == 0)
            {
                btnSubmit.Enabled = true;
                btnSaveDraft.Enabled = true;
				Util.Log("-- Transaction Failed inside save as draft --" + ReturnLoginName(), logfile);
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
            }
            else if (TransID == 5)
            {
                btnSubmit.Enabled = true;
                btnSaveDraft.Enabled = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
			Util.Log("-- catch inside save as draft --" + ex.Message.ToString() + "--" + ReturnLoginName(), logfile);
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
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
	
	  #region "Delete Claim"
      protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
  string UserType = string.Empty;
            string Comment = string.Empty;
            int DelId = 0;
            if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7" || HFSTATUS.Value == "9")
            {
 UserType = "User";
                Comment = "Deleted by User";
                DelId = Dal.DeleteFpaClaim(spnSrNo.InnerText, "1");
                if (DelId == 1)
                {
int RetId = Dal.DeleteTravelLogHistory(spnSrNo.InnerText, HFSAPID.Value, Comment, "FPA", UserType, HFSAPID.Value);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('Your claim deleted successfully');CloseRefreshWindow();", true);
                }
                else
                {
                    Util.Log("Sqlserver return exception -- " + spnSrNo.InnerText.ToString(), logfile);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please try after sometime.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Check", "alert('You can not delete this claim .');", true);
            }

        }
        catch (Exception ex)
        {
            Util.Log("catch exception -- " + spnSrNo.InnerText.ToString(), logfile);
            ScriptManager.RegisterStartupScript(this, GetType(), "Delete", "alert('Please try after sometime.');", true);
        }
    }
    #endregion
}
