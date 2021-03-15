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


public partial class FpaClaim : System.Web.UI.Page
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
                int checkstatus = Dal.CheckUserStatus(getSAPID(ReturnLoginName()));
                if (checkstatus == 0)
                {
                    string serialNumber = DoDecrypt(Request.QueryString["RID"]);
                    FillHeaderonquerystring(getSAPID(ReturnLoginName()), serialNumber);
                    int YearEnd = Dal.checkYearEndBySerailNo(HFSAPID.Value, serialNumber);
                    if (YearEnd == 0)
                    {
                        BindHistory();
                        btnSaveDraft.Visible = false;
                        btnSubmit.Visible = false;
                       // btnDelete.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD1", "alert('Your year end has been marked. Please contact your FPA Admin .');", true);
                    }
                    else
                    {
                        SetInitialRow();
                        FillGrid(serialNumber);
                        BindHistory();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You can not make claim untill your FPA is closed by FPA admin .');CloseRefreshWindow();", true);
                }
            }
            else
            {   int checkYearEnd = Dal.CheckYearEnd(getSAPID(ReturnLoginName()));
                int checkstatus = Dal.CheckUserStatus(getSAPID(ReturnLoginName()));
                int InitiateStatus = Dal.CheckInitiateStatus(getSAPID(ReturnLoginName()));
                DataTable ClosedStatus = Dal.CheckClosedStatus(getSAPID(ReturnLoginName()));
                if (checkYearEnd!=0 && checkstatus == 0 && InitiateStatus == 0 && Convert.ToString(ClosedStatus.Rows[0]["current_status"]) == "3")
                {
                    FillHeader(getSAPID(ReturnLoginName()));
                    SetInitialRow();
                    rdbtnModeOfPayment.SelectedValue = "1";
                }
                
                else
                {
                    if (checkstatus != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You can not make claim untill your FPA is closed by FPA admin .');CloseRefreshWindow();", true);
                    }
					else if (checkYearEnd ==0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your year end has been marked. Please contact your FPA Admin .');CloseRefreshWindow();", true);
                    }
                    else if (InitiateStatus != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your FPA has not been initaited please contact FPA admin.');CloseRefreshWindow();", true);
                    }
                    else if (Convert.ToString(ClosedStatus.Rows[0]["current_status"]) != "3")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You can not make claim untill your FPA is closed by FPA admin .');CloseRefreshWindow();", true);
                    }
                }
            }
        }
    }
    #endregion

    #region"FillHeaderBySerialNumber"
    private void FillHeaderonquerystring(string sapid, string serialNumber)
    {
        DataTable dt_UInfo = Dal.getclaimUserInfoquerystring(sapid, serialNumber);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIAdminLogin"]);
            spndepartment.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DEPARTMENT"]);
            SpnStatus.InnerText = GetStatus(Convert.ToString(dt_UInfo.Rows[0]["CLAIM_STATUS"]));
            HFSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["CLAIM_STATUS"]);
            spnSrNo.InnerText = DoDecrypt(Request.QueryString["RID"]);

            HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
            HFCOSTCENTER.Value = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
            spnTotalFpaBalance.InnerText = Convert.ToString(GetTotalFpaBalance(getSAPID(ReturnLoginName())));
            SpnProcessClaim.InnerText = Convert.ToString(GetClaimInProcess(HFSAPID.Value, spnSrNo.InnerText));
        }
        validateForm();
        string Owner = Convert.ToString(dt_UInfo.Rows[0]["Owner"]).ToLower();
        if (Owner == ReturnLoginName())
        {
            if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7")
            {
			   // btnDelete.Visible = true;
            }
            else
            {
                gvClaimDetail.Enabled = false;
                txtComments.ReadOnly = true;
                rdbtnModeOfPayment.Enabled = false;
                btnSaveDraft.Visible = false;
                btnSubmit.Visible = false;
				 btnDelete.Visible = false;
                
                
            }
        }
        else
        {
            gvClaimDetail.Enabled = false;
            txtComments.ReadOnly = true;
            rdbtnModeOfPayment.Enabled = false;
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
        using (DataTable fpadt = Dal.getFpadetailData(serialNumber))
        {

            if (fpadt.Rows.Count > 0)
            {
                gvClaimDetail.DataSource = fpadt;
                gvClaimDetail.DataBind();
                int rownumber = 1;
                int rowIndex = 0;
                for (int i = 0; i < fpadt.Rows.Count; i++)
                {
                    Label lblRownumber = (Label)gvClaimDetail.Rows[rowIndex].Cells[1].FindControl("lblRownumber");
                    DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                    TextBox Glcode = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
                    TextBox Allocation = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
                    TextBox txtHeadBalance = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
                    TextBox txtExpenseDate = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate");
                    TextBox txtBillAmount = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");
                    Label lblDetail = (Label)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("lblDetail");
                    Glcode.Enabled = false;
                    Allocation.Enabled = false;
                    txtHeadBalance.Enabled = false;
                    lblRownumber.Text = Convert.ToString(rownumber);
                    ddlHeads.ClearSelection();
                    ddlHeads.Items.FindByValue(fpadt.Rows[i]["FPA_HEAD_ID"].ToString()).Selected = true;
                    Glcode.Text = Convert.ToString(fpadt.Rows[i]["GL_CODE"]);
                    Allocation.Text = Convert.ToString(fpadt.Rows[i]["AllocateAmount"]);
                    txtHeadBalance.Text = Convert.ToString(fpadt.Rows[i]["HEAD_BAL"]);
                    txtExpenseDate.Text = Convert.ToString(fpadt.Rows[i]["EXPENSE_DATE"]);
                    txtBillAmount.Text = Convert.ToString(fpadt.Rows[i]["CLAIM_AMT"]);
                    txtDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]);
					lblDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]);
					if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7")
					{
						txtDetail.Visible = true;
						lblDetail.Visible = false;
					}
					else
					{  
					    lblDetail.Visible = true;
						txtDetail.Visible = false;
					}
                    rownumber++;
                    rowIndex++;

                }
                //rdbtnModeOfPayment.Items.FindByValue
                if (fpadt.Rows[0]["VOUCHER_TYPE"].ToString().Trim() == "KE")
                {
                    rdbtnModeOfPayment.SelectedValue = "1";
                }
                else
                {
                    rdbtnModeOfPayment.SelectedValue = "2";
                }
                txtComments.Text = Convert.ToString(fpadt.Rows[0]["COMMENT"]);
 txtFiadminComments.Text = Convert.ToString(fpadt.Rows[0]["FIADMIN_COMMENT"]);
                object totamount = fpadt.Compute("Sum(CLAIM_AMT)", "");
                spnTotalFpaClaim.InnerText = Convert.ToString(totamount);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = fpadt;

                if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7")
                {
                }
                else
                {
                    foreach(GridViewRow grow in gvClaimDetail.Rows)
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

                spnSrNo.InnerText = Dal.Serial_No("LC");

                HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
                HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
                HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
                HFCOSTCENTER.Value = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
                spnTotalFpaBalance.InnerText = Convert.ToString(GetTotalFpaBalance(getSAPID(ReturnLoginName())));
                SpnProcessClaim.InnerText = Convert.ToString(GetClaimInProcess(HFSAPID.Value, spnSrNo.InnerText));
            }
        }
        validateForm();
    }
    #endregion

    #region"ValidateForm"
    private void validateForm()
    {
        if (spnFIAdmin.InnerText == "" || spnEmpCode.InnerText == "")
        {
            btnSaveDraft.Visible = false;
            btnSubmit.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Some header information is missing.');CloseRefreshWindow();", true);

        }
    }
    #endregion

    #region"UserProcessClaimByEcode"
    private int GetClaimInProcess(string SapId, string SerialNumber)
    {
        int ClaimInProcessAmount = Dal.ClaimInProcess(SapId, SerialNumber);
        return ClaimInProcessAmount;
    }
    #endregion

    #region"GetTotalFpaBalance"
    private int GetTotalFpaBalance(string Sapid)
    {
        int TotalFpaBalance = Dal.GetTotalFpaBalance(Sapid);
        return TotalFpaBalance;
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
        //return "rnahar";
    }
    #endregion

    #region"AddNewRowInFPAClaimGridview"
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (spnTotalFpaClaim.InnerText == "")
            {
                spnTotalFpaClaim.InnerText = "0";
            }
            if (Convert.ToDouble(spnTotalFpaClaim.InnerText) < Convert.ToDouble(spnTotalFpaBalance.InnerText))
            {
                AddNewRowToGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('TotalFpaBalance Is ')", true);
            }
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
        //dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("FPA_HEAD_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("GL_CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("AllocateAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("HEAD_BAL", typeof(string)));
        dt.Columns.Add(new DataColumn("EXPENSE_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("CLAIM_AMT", typeof(double)));
        dt.Columns.Add(new DataColumn("DETAILS", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        //dr["Column1"] = string.Empty;
        dr["FPA_HEAD_ID"] = string.Empty;
        dr["GL_CODE"] = string.Empty;
        dr["AllocateAmount"] = string.Empty;
        dr["HEAD_BAL"] = string.Empty;
        dr["EXPENSE_DATE"] = string.Empty;
        dr["CLAIM_AMT"] = 0.0;
        dr["DETAILS"] = string.Empty;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        gvClaimDetail.DataSource = dt;
        gvClaimDetail.DataBind();
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
                    //DropDownList ddlClaimtype = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[1].FindControl("ddlClaimType");
                    DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                    TextBox Glcode = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
                    TextBox Allocation = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
                    TextBox txtHeadBalance = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
                    TextBox txtExpenseDate = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate");
                    TextBox txtBillAmount = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    //dtCurrentTable.Rows[i - 1]["Column1"] = Convert.ToString(ddlClaimtype.SelectedItem.Text);
                    dtCurrentTable.Rows[i - 1]["FPA_HEAD_ID"] = Convert.ToString(ddlHeads.SelectedValue);
                    dtCurrentTable.Rows[i - 1]["GL_CODE"] = Convert.ToString(Glcode.Text);
                    dtCurrentTable.Rows[i - 1]["AllocateAmount"] = Convert.ToString(Allocation.Text);
                    dtCurrentTable.Rows[i - 1]["HEAD_BAL"] = Convert.ToString(txtHeadBalance.Text);
                    dtCurrentTable.Rows[i - 1]["EXPENSE_DATE"] = Convert.ToString(txtExpenseDate.Text);
                    dtCurrentTable.Rows[i - 1]["CLAIM_AMT"] = Convert.ToString(txtBillAmount.Text);
                    dtCurrentTable.Rows[i - 1]["DETAILS"] = Convert.ToString(txtDetail.Text);

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;
                gvClaimDetail.DataSource = dtCurrentTable;
                gvClaimDetail.DataBind();
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
                    //DropDownList ddlClaimtype = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[1].FindControl("ddlClaimType");

                    DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                   // ddlHeads.Items.Clear();
                    TextBox Glcode = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
                    TextBox Allocation = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
                    TextBox txtHeadBalance = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
                    TextBox txtExpenseDate = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate");
                    TextBox txtBillAmount = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");

                   // ddlClaimtype.SelectedItem.Text = Convert.ToString(dt.Rows[i]["Column1"]);
                    ddlHeads.ClearSelection();
                    ddlHeads.Items.FindByValue(dt.Rows[i]["FPA_HEAD_ID"].ToString()).Selected = true;
                    //ddlHeads.SelectedItem.Text = Convert.ToString(dt.Rows[i]["Column2"]);
                    Glcode.Text = Convert.ToString(dt.Rows[i]["GL_CODE"]);
                    Allocation.Text = Convert.ToString(dt.Rows[i]["AllocateAmount"]);
                    txtHeadBalance.Text = Convert.ToString(dt.Rows[i]["HEAD_BAL"]);
                    txtExpenseDate.Text = Convert.ToString(dt.Rows[i]["EXPENSE_DATE"]);
                    txtBillAmount.Text = Convert.ToString(dt.Rows[i]["CLAIM_AMT"]);
                    txtDetail.Text = Convert.ToString(dt.Rows[i]["DETAILS"]);

                    rowIndex++;

                }
            }
            //gvClaimDetail.DataSource = dt;
            //gvClaimDetail.DataBind();
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
                    object totamount = dt.Compute("Sum(CLAIM_AMT)", "");
                    spnTotalFpaClaim.InnerText = Convert.ToString(totamount);
                    if (dt.Rows.Count > 0)
                    {
                        gvClaimDetail.DataSource = dt;
                        gvClaimDetail.DataBind();
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
    protected void txtBillAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {   //txtBillAmount.Focus();
            System.Threading.Thread.Sleep(500);
            if (spnTotalFpaClaim.InnerText == "")
            {
                spnTotalFpaClaim.InnerText = "0";
            }
            if (SpnProcessClaim.InnerText == "")
            {
                SpnProcessClaim.InnerText = "0";
            }

            TextBox txt = (sender as TextBox);
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            TextBox txtBillAmount = (TextBox)gvrow.FindControl("txtBillAmount");
			//txtBillAmount.Focus();

            if (txtBillAmount.Text == "")
            {
                txtBillAmount.Text = "0";
            }
                DropDownList ddlHeads = (DropDownList)gvrow.FindControl("ddlHeads");
                TextBox txtAllocation = (TextBox)gvrow.FindControl("txtAllocation");
                TextBox txtGlcode = (TextBox)gvrow.FindControl("txtGlcode");
                TextBox txtHeadBalance = (TextBox)gvrow.FindControl("txtHeadBalance");
                TextBox txtExpenseDate = (TextBox)gvrow.FindControl("txtExpenseDate");
				TextBox txtDetail = (TextBox)gvrow.FindControl("txtDetail");
                double totalValue = 0.0;
                if (spnTotalFpaClaim.InnerText == "")
                {
                    totalValue = 0.0;
                }
                else
                {
                    foreach (GridViewRow grow in gvClaimDetail.Rows)
                    {
                        TextBox txtAmount = (TextBox)grow.FindControl("txtBillAmount");
						 if (txtAmount.Text == "")
                        {
                            txtAmount.Text = "0";
                        }
                        totalValue = totalValue + Convert.ToDouble(txtAmount.Text);
                    }

                    spnTotalFpaClaim.InnerText = Convert.ToString(totalValue);
                }

                if ((Convert.ToDouble(spnTotalFpaClaim.InnerText)) + Convert.ToDouble(SpnProcessClaim.InnerText) <= Convert.ToDouble(spnTotalFpaBalance.InnerText))
                {
                    spnTotalFpaClaim.InnerText = Convert.ToString(totalValue);
		     //txtBillAmount.Focus();
                }
                else
                {
                    double totalRejectedValue = 0.0;
                    txtBillAmount.Text = "";
                    //txtGlcode.Text = "";
                    //txtHeadBalance.Text = "";
                    //txtAllocation.Text = "";
                    //txtExpenseDate.Text = "";
                    //ddlHeads.SelectedValue = "0";
                    foreach (GridViewRow grow in gvClaimDetail.Rows)
                    {
                        TextBox txtAmount = (TextBox)grow.FindControl("txtBillAmount");
                        if (txtAmount.Text == "")
                        {
                            txtAmount.Text = "0";
                        }
                        totalRejectedValue = totalRejectedValue + Convert.ToDouble(txtAmount.Text);
                    }
                    spnTotalFpaClaim.InnerText = Convert.ToString(totalRejectedValue);
                    //ddlHeads.SelectedValue=
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Claim amount cannot be greater than your FPA balance')", true);
                }
				foreach (GridViewRow row in gvClaimDetail.Rows)
                {
                    TextBox txtaa = (TextBox)row.FindControl("txtDetail");
                    scp.RegisterAsyncPostBackControl(txtaa);
                }
            }
            
        catch (Exception ex)
        {
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
            string EmailID = HFEMAILS.Value.ToString().Split('#')[2]; // Approver Email ID
            string modeofpayment = string.Empty;
            if (rdbtnModeOfPayment.SelectedValue == "1")
            {
                modeofpayment = "KE";
            }
            else
            {
                modeofpayment = "SK";
            }


            string XmlFpaClaimDetail = GetFpaDetail();

            int TransID = 0;
            if ((Convert.ToDouble(spnTotalFpaClaim.InnerText)) + Convert.ToDouble(SpnProcessClaim.InnerText) <= Convert.ToDouble(spnTotalFpaBalance.InnerText))
            {
                 Util.Log("try check limit FpaClaim Page total amt check-- ", logfile);
                  if (Convert.ToDouble(spnTotalFpaClaim.InnerText) >= 500)
                {  
				   Util.Log("try check limit FpaClaim Page total amt check 661-- ", logfile);
                    TransID = Dal.InsertFpaClaimDetail(spnEmpCode.InnerText, spnSrNo.InnerText, modeofpayment, "2", txtComments.Text, HFBU.Value, HFSAPID.Value, ReturnLoginName(), HFCOSTCENTER.Value, spnTotalFpaClaim.InnerText, XmlFpaClaimDetail);
					
                    if (TransID == 1)
                    {
                        Util.Log("Submit by user  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
					    CommonMail(EmailID.Trim(), "2");
                        Util.Log("After mail send -- " + spnSrNo.InnerText.ToString(), logfile);
                        Dal.CreateRejectLogHistory("2", spnLoginUser.InnerText.ToString(), Convert.ToString(txtComments.Text), spnSrNo.InnerText.ToString());
                        Util.Log("log History -- " + spnSrNo.InnerText.ToString(), logfile);
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is submitted to FI Admin for Approval.');CloseRefreshWindow();", true);
                    }
                    else if (TransID == 0)
                    {
                        Util.Log("Transaction failed -- " + spnSrNo.InnerText.ToString(), logfile);
                        btnSubmit.Enabled = true;
                        btnSaveDraft.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                    }
                    else if (TransID == 5)
                    {
                        Util.Log("Data Already Submit -- " + spnSrNo.InnerText.ToString(), logfile);
                        btnSubmit.Enabled = true;
                        btnSaveDraft.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                    }
                }

                else
                {
                    btnSubmit.Enabled = true;
                    btnSaveDraft.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum Claim amount is INR 500 ')", true);

                }
            }
            else
            {
                btnSubmit.Enabled = true;
                btnSaveDraft.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Bill Amount is not greater than Total Fpa Claim Amount Balance')", true);
            }
        }
        catch (Exception ex)
        {
            Util.Log("catch exception -- " + spnSrNo.InnerText.ToString()+ex.Message, logfile);
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime  .');", true);
        }

    }
    #endregion

    #region"SaveAsDraftByUser"
    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnSaveDraft.Enabled = false;
            btnSubmit.Enabled = false;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string EmailID = HFEMAILS.Value.ToString().Split('#')[1]; // Approver Email ID
            string modeofpayment = string.Empty;
            if (rdbtnModeOfPayment.SelectedValue == "1")
            {
                modeofpayment = "KE";
            }
            else
            {
                modeofpayment = "SK";
            }


            string XmlFpaClaimDetail = GetFpaDetail();

            int TransID = 0;
            if (spnTotalFpaClaim.InnerText == "")
            {
                spnTotalFpaClaim.InnerText = "0";
            }
            if ((Convert.ToDouble(spnTotalFpaClaim.InnerText)) + Convert.ToDouble(SpnProcessClaim.InnerText) <= Convert.ToDouble(spnTotalFpaBalance.InnerText))
            {
                if (Convert.ToDouble(spnTotalFpaClaim.InnerText) >= 500)
                {
                    
                    TransID = Dal.InsertFpaClaimDetail(spnEmpCode.InnerText, spnSrNo.InnerText, modeofpayment, "1", txtComments.Text, HFBU.Value, HFSAPID.Value, ReturnLoginName(), HFCOSTCENTER.Value, spnTotalFpaClaim.InnerText, XmlFpaClaimDetail);
                    if (TransID == 1)
                    {  
					    Util.Log("Save as by user  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is saved successfully.');CloseRefreshWindow();", true);
                    }
                    else if (TransID == 0)
                    {
                        btnSubmit.Enabled = true;
                        btnSaveDraft.Enabled = true;
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum  Claim amount is INR 500 ')", true);

                }
            }
            else
            {
                btnSubmit.Enabled = true;
                btnSaveDraft.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Bill Amount is not greater than Total Fpa Claim Amount Balance')", true);
            }
        }
        catch (Exception ex)
        { 
		    Util.Log("Exception at Save as by user  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }
    }
    #endregion  

    #region"UserFPADetail"
    private string  GetFpaDetail()
    {
        string XmlDataGridItems = string.Empty;
        using (DataTable dt = new DataTable("FpaClaim"))
        {
            dt.Columns.Add("FPA_CYCLE_ID", typeof(string));
            dt.Columns.Add("FPA_HEAD_ID", typeof(string));
            dt.Columns.Add("HEAD_BAL", typeof(string));
            dt.Columns.Add("CLAIM_AMT", typeof(string));
            dt.Columns.Add("EXPENSE_DATE", typeof(string));
            dt.Columns.Add("GL_CODE", typeof(string));
            dt.Columns.Add("DETAILS", typeof(string));
            dt.Columns.Add("CYCLENO", typeof(int));
            string FpaCyleId = Dal.GetFpaCyleId((getSAPID(ReturnLoginName())));
            int i = 0;
            foreach (GridViewRow grow in gvClaimDetail.Rows)
            {
                DropDownList ddlhead = (DropDownList)grow.FindControl("ddlHeads");
                TextBox Glcode = (TextBox)grow.FindControl("txtGlcode");
                TextBox Allocation = (TextBox)grow.FindControl("txtAllocation");
                TextBox txtHeadBalance = (TextBox)grow.FindControl("txtHeadBalance");
                TextBox txtExpenseDate = (TextBox)grow.FindControl("txtExpenseDate");
                TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
                TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");
                DataRow dr = dt.NewRow();
                dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
                dr["FPA_HEAD_ID"] = Convert.ToString(ddlhead.SelectedValue);
                dr["HEAD_BAL"] = Convert.ToString(txtHeadBalance.Text);
                dr["CLAIM_AMT"] = Convert.ToString(txtBillAmount.Text);
                if (string.IsNullOrEmpty(txtExpenseDate.Text))
                {
                    dr["EXPENSE_DATE"] = DateTime.Now.ToString();
                }
                else
                {
                    dr["EXPENSE_DATE"] = Convert.ToString(txtExpenseDate.Text);
                }
                dr["GL_CODE"] = Convert.ToString(Glcode.Text);
                dr["DETAILS"] = Convert.ToString(txtDetail.Text);
                dr["CYCLENO"] = Convert.ToString(i);
                dt.Rows.Add(dr);
                i++;

            }


            
            StringWriter strFpaHeadWriter = new StringWriter();
            dt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
            XmlDataGridItems = strFpaHeadWriter.ToString();
        }
        return XmlDataGridItems;
    }
    #endregion

    #region"BindHeadInClaimGridview"
    protected void gvClaimDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable fpaheads = Dal.getFpaClaimHead(getSAPID(ReturnLoginName()));
            DropDownList ddlLine = (DropDownList)e.Row.FindControl("ddlHeads");
            ddlLine.Items.Clear();
            ddlLine.AppendDataBoundItems = true;
            ListItem li = new ListItem( "--Select--","0");
            ddlLine.Items.Add(li);
            ddlLine.DataSource = fpaheads;
            ddlLine.DataTextField = "fpa_head";
            ddlLine.DataValueField = "fpa_head_id";
            ddlLine.DataBind();
        }
    }
    #endregion

    #region"AllocateBalance,Glcode,HeadBalance"
    protected void ddlHeads_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (sender as DropDownList);
            GridViewRow gvrow = (GridViewRow)ddl.NamingContainer;
            DropDownList ddlHeads = (DropDownList)gvrow.FindControl("ddlHeads");
            TextBox txtAllocation = (TextBox)gvrow.FindControl("txtAllocation");
            TextBox txtGlcode = (TextBox)gvrow.FindControl("txtGlcode");
            TextBox txtHeadBalance = (TextBox)gvrow.FindControl("txtHeadBalance");


            txtAllocation.Text = Convert.ToString(GetAllocateBalance(getSAPID(ReturnLoginName()), ddlHeads.SelectedValue));
            txtAllocation.Enabled = false;
            txtGlcode.Text=Convert.ToString(GetGlCode((getSAPID(ReturnLoginName())), ddlHeads.SelectedValue));
            txtHeadBalance.Text = Convert.ToString(GetHeadBalance((getSAPID(ReturnLoginName())), ddlHeads.SelectedValue));
            txtHeadBalance.Enabled = false;
            if (!string.IsNullOrEmpty(txtGlcode.Text))
            {
                txtGlcode.Enabled = false;
                btnSaveDraft.Visible = true;
                btnSubmit.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Your FPA has not been reinitiated. Please contact FPA admin. ')", true);
                btnSaveDraft.Visible = false;
                btnSubmit.Visible = false;
                txtGlcode.Enabled = false;
                txtAllocation.Text = "";
                txtHeadBalance.Text = "";
            }





           //txtAllocation.Text = Convert.ToString(GetAllocateBalance(getSAPID(ReturnLoginName()), ddlHeads.SelectedValue));
            //txtAllocation.Enabled = false;

            //txtGlcode.Text=Convert.ToString(GetGlCode((getSAPID(ReturnLoginName())), ddlHeads.SelectedValue));
           // txtGlcode.Enabled=false;

           // txtHeadBalance.Text = Convert.ToString(GetHeadBalance((getSAPID(ReturnLoginName())), ddlHeads.SelectedValue));
           // txtHeadBalance.Enabled = false;
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region"GetHeadBalance"
    private int GetHeadBalance(string SapId, string FpaheadId)
    {
        int HeadBalance = Dal.GetHeadAmount(SapId, FpaheadId);
        return HeadBalance;
    }
    #endregion

    #region"GetGlcode"
    private string GetGlCode(string SapId,string FpaheadId)
    {
        string  Glcode = Dal.GetGlcode(SapId, FpaheadId);
        return Glcode;
    }
    #endregion

    #region"GetAllocateBalance"
    private int GetAllocateBalance(string SapId,string FpaheadId)
    {
        int AllocateAmount = Dal.GetAllocateAmount(SapId, FpaheadId);
        return AllocateAmount;
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
                        Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear Sir/Ma'am,<br/><br/>FPA Claim request of <b>" + spnFormCreater.InnerText + "</b> with serial no <b>" + SerailNo + "</b> has been Submitted for your approval <br /><br />Please click on the link below for taking necessary action<br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";


            Subject = "FPA Claim request " + SerailNo + " has been Submitted...";
            Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
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
	
	 #region "Delete Claim"
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        { string UserType = string.Empty;
            string Comment = string.Empty;
            int DelId = 0;
            if (HFSTATUS.Value == "1" || HFSTATUS.Value == "7" || HFSTATUS.Value == "2")
            {
   UserType = "User";
                Comment = "Deleted by User";
                DelId = Dal.DeleteFpaClaim(spnSrNo.InnerText,"0");
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
