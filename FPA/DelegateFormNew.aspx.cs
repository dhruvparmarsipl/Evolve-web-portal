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
using System.Xml;
#endregion

public partial class DelegateFormNew : System.Web.UI.Page
{
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    DataTable _table = null;
    #endregion

    int maxFileLimit = 0;
    static int Size = 0;
    int maxfileSize = 0;
    int ttlmaxfileSize = 0;

    string AttachFileName = string.Empty;
    string AttachFileName1 = string.Empty;
    string FileName = string.Empty;
    string FilePath = string.Empty;
    static string FileNameUpload = string.Empty;
    static string FilePathUpload = string.Empty;
    public System.Collections.Generic.List<string> arrValidExtention = new System.Collections.Generic.List<string>();

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                string Sapid = Request.QueryString["EMPSAP"];
                int checkstatus = Dal.CheckUserStatus(Sapid);
                int InitiateStatus = Dal.CheckInitiateStatus(Sapid);
                DataTable ClosedStatus = Dal.CheckClosedStatus(Sapid);
                if (checkstatus == 0 && InitiateStatus == 0 && Convert.ToString(ClosedStatus.Rows[0]["current_status"]) == "3")
                {
                    FillHeader(Sapid);
                    SetInitialRow();
                    rdbtnModeOfPayment.SelectedValue = "1";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Not Applicable Please Contact Fpa Admin.');CloseRefreshWindow();", true);
                }
            }
            HFUploadFileNo.Value = "0";
        }
        arrValidExtention.Add(".jpg");
        arrValidExtention.Add(".jpeg");
        arrValidExtention.Add(".png");
        arrValidExtention.Add(".pdf");
        string FilePath = Server.MapPath("~/App_Data/Config.xml");
        XmlDocument ConfigXml = new XmlDocument();
        ConfigXml.Load(FilePath);
        maxfileSize = Convert.ToInt32(ConfigXml.SelectSingleNode("/Elements/FileSize").InnerText.ToString());
        maxFileLimit = Convert.ToInt32(ConfigXml.SelectSingleNode("/Elements/FileLimit").InnerText.ToString());
        ttlmaxfileSize = Convert.ToInt32(ConfigXml.SelectSingleNode("/Elements/FileSizeTtl").InnerText.ToString());
        AttachFileName = ConfigXml.SelectSingleNode("/Elements/FilePathAttachement").InnerText.ToString();
        AttachFileName1 = ConfigXml.SelectSingleNode("/Elements/FilePathAttachementA").InnerText.ToString();
        int sizMb = (maxfileSize / 1024) / 1024;
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

        return cstatus;
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
                spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ENAME from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'"); ;
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
                spnTotalFpaBalance.InnerText = Convert.ToString(GetTotalFpaBalance(sapid));
                SpnProcessClaim.InnerText = Convert.ToString(GetClaimInProcess(spnEmpCode.InnerText, spnSrNo.InnerText));
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
        }
    }
    #endregion

    #region"GetClaimProcessByEcode"
    private int GetClaimInProcess(string Ecode, string SerialNumber)
    {
        int ClaimInProcessAmount = Dal.ClaimInProcess(Ecode, SerialNumber);
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
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "spstestak";
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
        dt.Columns.Add(new DataColumn("FPA_HEAD_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("GL_CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("AllocateAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("HEAD_BAL", typeof(string)));
        dt.Columns.Add(new DataColumn("EXPENSE_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("CLAIM_AMT", typeof(double)));
        dt.Columns.Add(new DataColumn("DETAILS", typeof(string)));
        dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("RowNo", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["FPA_HEAD_ID"] = string.Empty;
        dr["GL_CODE"] = string.Empty;
        dr["AllocateAmount"] = string.Empty;
        dr["HEAD_BAL"] = string.Empty;
        dr["EXPENSE_DATE"] = string.Empty;
        dr["CLAIM_AMT"] = 0.0;
        dr["DETAILS"] = string.Empty;
        dr["BillNo"] = string.Empty;
        dr["RowNo"] = 1;
        dt.Rows.Add(dr);

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
                    TextBox txtBillNo = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtBillNo");
                    TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[9].FindControl("txtDetail");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    drCurrentRow["RowNo"] = i + 1;
                    //dtCurrentTable.Rows[i - 1]["Column1"] = Convert.ToString(ddlClaimtype.SelectedItem.Text);
                    dtCurrentTable.Rows[i - 1]["FPA_HEAD_ID"] = Convert.ToString(ddlHeads.SelectedValue);
                    dtCurrentTable.Rows[i - 1]["GL_CODE"] = Convert.ToString(Glcode.Text);
                    dtCurrentTable.Rows[i - 1]["AllocateAmount"] = Convert.ToString(Allocation.Text);
                    dtCurrentTable.Rows[i - 1]["HEAD_BAL"] = Convert.ToString(txtHeadBalance.Text);
                    dtCurrentTable.Rows[i - 1]["EXPENSE_DATE"] = Convert.ToString(txtExpenseDate.Text);
                    dtCurrentTable.Rows[i - 1]["CLAIM_AMT"] = Convert.ToString(txtBillAmount.Text);
                    dtCurrentTable.Rows[i - 1]["DETAILS"] = Convert.ToString(txtDetail.Text);
                    dtCurrentTable.Rows[i - 1]["BillNo"] = Convert.ToString(txtBillNo.Text);
                    dtCurrentTable.Rows[i - 1]["RowNo"] = i;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;
                gvClaimDetail.DataSource = dtCurrentTable;
                gvClaimDetail.DataBind();


                if (ViewState["ClaimDetailGridDocs"] == null)
                {
                    _table = new DataTable();
                    _table.Columns.Add(new DataColumn("ClaimDetailRowNo", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerID", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));
                }
                else
                {
                    _table = ((DataTable)ViewState["ClaimDetailGridDocs"]);
                }

                foreach (GridViewRow _row1 in gvClaimDetail.Rows)
                {
                    //--------------
                    GridView _grd = (GridView)_row1.FindControl("gvInnerDocs");
                    Label _lbl = (Label)_row1.FindControl("lblRowNo");

                    if (_lbl.Text != "")
                    {
                        DataRow _row3 = _table.NewRow();
                        int _rowIndex0 = Convert.ToInt32(_lbl.Text);

                        DataRow[] drsel = _table.Select("ClaimDetailRowNo='" + _rowIndex0 + "'");
                        //===============================
                        DataTable _table1 = new DataTable();
                        _table1.Columns.Add(new DataColumn("InnerID", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));
                        _row3 = _table1.NewRow();
                        foreach (DataRow dr in drsel)
                        {
                            _row3 = _table1.NewRow();
                            _row3["InnerID"] = dr["InnerID"];
                            _row3["InnerFileName"] = dr["InnerFileName"];
                            _row3["InnerOnlyFileName"] = dr["InnerOnlyFileName"];
                            _row3["InnerRowNo"] = dr["InnerRowNo"];
                            _table1.Rows.Add(_row3);
                        }
                        _grd.DataSource = _table1;
                        _grd.DataBind();
                    }
                }
                //==============================================================


                hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value) + 1).ToString();
            }
        }
        else
        {

        }
        ////hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value) + 1).ToString();
        HFUploadFileNo.Value = "0";

        trUploadControls.Visible = false;
        trUploadGrid.Visible = false;
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
                    DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                    // ddlHeads.Items.Clear();
                    TextBox Glcode = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
                    TextBox Allocation = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
                    TextBox txtHeadBalance = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
                    TextBox txtExpenseDate = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate");
                    TextBox txtBillAmount = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    TextBox txtBillNo = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtBillNo");
                    TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[9].FindControl("txtDetail");
                    Label lblRowNo = (Label)gvClaimDetail.Rows[rowIndex].Cells[9].FindControl("lblRowNo");

                    ddlHeads.ClearSelection();
                    ddlHeads.Items.FindByValue(dt.Rows[i]["FPA_HEAD_ID"].ToString()).Selected = true;
                    Glcode.Text = Convert.ToString(dt.Rows[i]["GL_CODE"]);
                    Allocation.Text = Convert.ToString(dt.Rows[i]["AllocateAmount"]);
                    txtHeadBalance.Text = Convert.ToString(dt.Rows[i]["HEAD_BAL"]);
                    txtExpenseDate.Text = Convert.ToString(dt.Rows[i]["EXPENSE_DATE"]);
                    txtBillAmount.Text = Convert.ToString(dt.Rows[i]["CLAIM_AMT"]);
                    txtBillNo.Text = Convert.ToString(dt.Rows[i]["BillNo"]);
                    lblRowNo.Text = Convert.ToString(dt.Rows[i]["RowNo"]);
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
            gvEntryDocs.DataSource = null;
            gvEntryDocs.DataBind();
            trUploadControls.Visible = false;
            trUploadGrid.Visible = false;

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

                            foreach (DataRow drForUpdation in dt.Rows)
                            {
                                if (Convert.ToInt32(drForUpdation["RowNumber"].ToString()) >= Convert.ToInt32(rownumber) + 1)
                                {
                                    drForUpdation["RowNumber"] = Convert.ToInt32(drForUpdation["RowNumber"].ToString()) - 1;
                                    drForUpdation["RowNo"] = Convert.ToInt32(drForUpdation["RowNo"].ToString()) - 1;
                                }
                            }
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

                    int _rowIndex = Convert.ToInt32(rownumber);
                    _table = (DataTable)ViewState["ClaimDetailGridDocs"];
                    DataRow[] drselD = _table.Select("ClaimDetailRowNo='" + _rowIndex + "'");
                    foreach (DataRow dr in drselD)
                    {
                        string _file = dr["InnerFileName"].ToString();
                        string _str = Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and File_Name = '" + _file + "' and MainRowNo = '" + _rowIndex + "'");
                        string fileName = Path.Combine(AttachFileName, _file);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            FileInfo TheFile = new FileInfo(fileName);
                            if (TheFile.Exists)
                            {
                                File.Delete(fileName);
                            }
                        }

                        _table.Rows.Remove(dr);
                    }

                    foreach (DataRow drForUpdation in _table.Rows)
                    {
                        if (Convert.ToInt32(drForUpdation["ClaimDetailRowNo"].ToString()) >= Convert.ToInt32(rownumber) + 1)
                        {
                            drForUpdation["ClaimDetailRowNo"] = Convert.ToInt32(drForUpdation["ClaimDetailRowNo"].ToString()) - 1;                            
                        }
                    }

                    _table.AcceptChanges();

                    ////int RowIndexToRemove = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[1].ToString() == "" && dr[2].ToString() == "" && dr[3].ToString() == "" && dr[4].ToString() == "" && dr[5].ToString() == "")
                        {
                            DataRow[] _TableArray = _table.Select();
                            foreach (DataRow Innerdr in _TableArray)
                            {
                                if (Convert.ToInt32(dr["RowNumber"]) == Convert.ToInt32(Innerdr["ClaimDetailRowNo"]))
                                {
                                    string _file = Innerdr["InnerFileName"].ToString();
                                    string _str = Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and File_Name = '" + _file + "' and MainRowNo = '" + Innerdr["ClaimDetailRowNo"] + "'");
                                    string fileName = Path.Combine(AttachFileName, _file);
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        FileInfo TheFile = new FileInfo(fileName);
                                        if (TheFile.Exists)
                                        {
                                            File.Delete(fileName);
                                        }
                                    }
                                    _table.Rows.Remove(Innerdr);
                                }
                            }
                            _table.AcceptChanges();

                        }
                        /////RowIndexToRemove++;
                    }

                    //==============================================================
                    _table = (DataTable)ViewState["ClaimDetailGridDocs"];
                    foreach (GridViewRow _row1 in gvClaimDetail.Rows)
                    {
                        //--------------
                        GridView _grd = (GridView)_row1.FindControl("gvInnerDocs");
                        DataRow _row2 = _table.NewRow();
                        Label lbl = (Label)_row1.FindControl("lblRowNo");
                        _rowIndex = Convert.ToInt32(lbl.Text);

                        DataRow[] drsel = _table.Select("ClaimDetailRowNo='" + _rowIndex + "'");
                        //===============================
                        DataTable _table1 = new DataTable();
                        _table1.Columns.Add(new DataColumn("InnerID", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                        _table1.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));
                        _row2 = _table1.NewRow();
                        foreach (DataRow dr in drsel)
                        {
                            _row2 = _table1.NewRow();
                            _row2["InnerID"] = dr["InnerID"];
                            _row2["InnerFileName"] = dr["InnerFileName"];
                            _row2["InnerOnlyFileName"] = dr["InnerOnlyFileName"];
                            _row2["InnerRowNo"] = dr["InnerRowNo"];
                            _table1.Rows.Add(_row2);
                        }
                        _grd.DataSource = _table1;
                        _grd.DataBind();
                    }
                    //==============================================================

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
        {
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
            if (txtBillAmount.Text == "")
            {
                txtBillAmount.Text = "0";
            }
            DropDownList ddlHeads = (DropDownList)gvrow.FindControl("ddlHeads");
            TextBox txtAllocation = (TextBox)gvrow.FindControl("txtAllocation");
            TextBox txtGlcode = (TextBox)gvrow.FindControl("txtGlcode");
            TextBox txtHeadBalance = (TextBox)gvrow.FindControl("txtHeadBalance");
            TextBox txtExpenseDate = (TextBox)gvrow.FindControl("txtExpenseDate");
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Bill Amount is not greater than Total Fpa Claim Amount Balance')", true);
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

                if (Convert.ToDouble(spnTotalFpaClaim.InnerText) >= 500)
                {
                    TransID = Dal.InsertDeleSubmitClaimDetail(spnEmpCode.InnerText, spnSrNo.InnerText, modeofpayment, "1", txtComments.Text, HFBU.Value, HFSAPID.Value, getLoginName(), ReturnLoginName(), HFCOSTCENTER.Value, spnTotalFpaClaim.InnerText, XmlFpaClaimDetail);
                    if (TransID == 1)
                    {
                        CommonMail(HFEMAILS.Value.Split('#')[0].ToString());
                        Dal.CreateRejectLogHistory("1", spnLoginUser.InnerText.ToString(), Convert.ToString(txtComments.Text), spnSrNo.InnerText.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is submitted suucessfully.');CloseRefreshWindow();", true);
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
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }

    }
    #endregion

    #region"GetLogin"
    public string getLoginName()
    {
        string Login = Dal.Get_Single_DataByPassingQuery("SELECT login_name FROM MASTER_EMPLOYEE_PROFILE WHERE SAPID ='" + Convert.ToString(HFSAPID.Value) + "'");
        return Login;
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
                    TransID = Dal.InsertDeleFpaClaimDetail(spnEmpCode.InnerText, spnSrNo.InnerText, modeofpayment, "1", txtComments.Text, HFBU.Value, HFSAPID.Value, "Delegate", ReturnLoginName(), HFCOSTCENTER.Value, spnTotalFpaClaim.InnerText, XmlFpaClaimDetail);
                    if (TransID == 1)
                    {
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
            btnSubmit.Enabled = true;
            btnSaveDraft.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }
    }
    #endregion

    #region"UserFPADetail"
    private string GetFpaDetail()
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
            dt.Columns.Add("BillNo", typeof(string));
            dt.Columns.Add("RowNo", typeof(int));

            string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
            foreach (GridViewRow grow in gvClaimDetail.Rows)
            {
                DropDownList ddlhead = (DropDownList)grow.FindControl("ddlHeads");
                TextBox Glcode = (TextBox)grow.FindControl("txtGlcode");
                TextBox Allocation = (TextBox)grow.FindControl("txtAllocation");
                TextBox txtHeadBalance = (TextBox)grow.FindControl("txtHeadBalance");
                TextBox txtExpenseDate = (TextBox)grow.FindControl("txtExpenseDate");
                TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
                TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");
                TextBox txtBillNo = (TextBox)grow.FindControl("txtBillNo");
                Label lblRowNo = (Label)grow.FindControl("lblRowNo");

                DataRow dr = dt.NewRow();
                dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
                dr["FPA_HEAD_ID"] = Convert.ToString(ddlhead.SelectedValue);
                dr["HEAD_BAL"] = Convert.ToString(txtHeadBalance.Text);
                dr["CLAIM_AMT"] = Convert.ToString(txtBillAmount.Text);
                dr["EXPENSE_DATE"] = Convert.ToString(txtExpenseDate.Text);
                dr["GL_CODE"] = Convert.ToString(Glcode.Text);
                dr["DETAILS"] = Convert.ToString(txtDetail.Text);
                dr["BillNo"] = Convert.ToString(txtBillNo.Text);
                dr["RowNo"] = Convert.ToString(lblRowNo.Text);

                dt.Rows.Add(dr);

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
            DataTable fpaheads = Dal.getFpaClaimHead(Request.QueryString["EMPSAP"]);
            DropDownList ddlLine = (DropDownList)e.Row.FindControl("ddlHeads");
            ddlLine.Items.Clear();
            ddlLine.AppendDataBoundItems = true;
            ListItem li = new ListItem("--Select--", "0");
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
            txtAllocation.Text = Convert.ToString(GetAllocateBalance(Request.QueryString["EMPSAP"], ddlHeads.SelectedValue));
            txtAllocation.Enabled = false;

            txtGlcode.Text = Convert.ToString(GetGlCode((Request.QueryString["EMPSAP"]), ddlHeads.SelectedValue));
            txtGlcode.Enabled = false;

            txtHeadBalance.Text = Convert.ToString(GetHeadBalance((Request.QueryString["EMPSAP"]), ddlHeads.SelectedValue));
            txtHeadBalance.Enabled = false;
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
    private string GetGlCode(string SapId, string FpaheadId)
    {
        string Glcode = Dal.GetGlcode(SapId, FpaheadId);
        return Glcode;
    }
    #endregion

    #region"GetAllocateBalance"
    private int GetAllocateBalance(string SapId, string FpaheadId)
    {
        int AllocateAmount = Dal.GetAllocateAmount(SapId, FpaheadId);
        return AllocateAmount;
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

    #region"CommonMailFunction"
    public void CommonMail(string MailTo)
    {
        string logfile = "D:\\Eicher\\All Logs\\FPA";
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string newUrl = string.Empty;
        string SerailNo = spnSrNo.InnerText.ToString();
        if (HFBU.Value.Trim() == "REM")
            newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
        else
            newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";

        try
        {
            Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'>" +
                "<tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>" +
                "Dear Sir,<br/><br/>Your FPA Claim  <b>" + SerailNo + "</b> has been submitted by Delegatee. <br />	" +
                "<br/>Please  click on  the link below to submit the FPA claim.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a>" +
                "<br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>" +
                "This is system generated message</span></div></div></td></tr></table>";
            Subject = "Your FPA Claim " + SerailNo + " has been save as Draft...";
            Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
        }
        catch (Exception ex)
        {
            Util.Log("Delegate Form ----- Catch inside mail send  -- " + SerailNo, logfile);
        }
    }
    #endregion

    protected void gvInnerDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Open")
        {
            try
            {
                string filename = e.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblFileName = (Label)row.FindControl("lblfileName");
                LinkButton hprlnk = (LinkButton)row.FindControl("lnkDoc");
                if (hprlnk.Text.Length > 0)
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + hprlnk.Text + "");
                    Response.TransmitFile(AttachFileName1 + lblFileName.Text);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('" + "No file exists." + "');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('" + ex.Message + "');", true);
            }

        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton _but = (LinkButton)sender;
            GridViewRow _row = (GridViewRow)_but.NamingContainer;

            Label id = (Label)_row.FindControl("lblID");
            Label rowNo = (Label)_row.FindControl("lblRowNo");

            string selID = Convert.ToString(id.Text);
            _table = ((DataTable)ViewState["DocEntryData"]);
            DataRow[] drsel = _table.Select("ID='" + selID + "'");
            Label _file = (Label)_row.FindControl("lblFileName");

            int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
            int _rowIndexP = Convert.ToInt32(rowNo.Text);

            foreach (DataRow dr in drsel)
            {
                _table.Rows.Remove(dr);
            }
            _table.AcceptChanges();
            ViewState["DocEntryData"] = _table;

            gvEntryDocs.EditIndex = -1;
            gvEntryDocs.DataSource = _table;
            gvEntryDocs.DataBind();

            string _str = Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and MainRowNo = " + _rowIndex + " and FileNoPrefix = " + _rowIndexP);

            string fileName = Path.Combine(AttachFileName, _file.Text);
            if (!string.IsNullOrEmpty(fileName))
            {
                FileInfo TheFile = new FileInfo(fileName);
                if (TheFile.Exists)
                {
                    File.Delete(fileName);
                }
            }

            DataTable _ClaimDetailGridDocsTable = new DataTable();
            _ClaimDetailGridDocsTable = ((DataTable)ViewState["ClaimDetailGridDocs"]);
            DataRow[] ClaimDetailGridDocsdr = _ClaimDetailGridDocsTable.Select("InnerID='" + selID + "' AND ClaimDetailRowNo='" + hfMainRowNo.Value.ToString() + "'");

            foreach (DataRow dr in ClaimDetailGridDocsdr)
            {
                _ClaimDetailGridDocsTable.Rows.Remove(dr);
            }
            _ClaimDetailGridDocsTable.AcceptChanges();
            ViewState["ClaimDetailGridDocs"] = _ClaimDetailGridDocsTable;

            foreach (GridViewRow dr in gvClaimDetail.Rows)
            {
                GridView _grd = (GridView)dr.FindControl("gvInnerDocs");
                Label _lbl = (Label)dr.FindControl("lblRowNo");

                if (_lbl.Text != "")
                {
                    _table = ((DataTable)ViewState["ClaimDetailGridDocs"]);
                    int _rowIndex0 = Convert.ToInt32(_lbl.Text);

                    DataRow[] drsel1 = _table.Select("ClaimDetailRowNo='" + _rowIndex0 + "'");

                    DataTable _table1 = new DataTable();
                    _table1.Columns.Add(new DataColumn("InnerID", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));


                    foreach (DataRow Innerdr in drsel1)
                    {
                        DataRow _row3 = _table1.NewRow();
                        _row3["InnerID"] = Innerdr["InnerID"];
                        _row3["InnerFileName"] = Innerdr["InnerFileName"];
                        _row3["InnerOnlyFileName"] = Innerdr["InnerOnlyFileName"];
                        _row3["InnerRowNo"] = Innerdr["InnerRowNo"];
                        _table1.Rows.Add(_row3);
                    }
                    _grd.DataSource = _table1;
                    _grd.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LinkEditBill_Click(object sender, EventArgs e)
    {
        try
        {
            trUploadControls.Visible = true;
            trUploadGrid.Visible = true;

            _table = ((DataTable)ViewState["DocEntryData"]);
            if (_table != null)
            {
                _table.Rows.Clear();
                gvEntryDocs.DataSource = null;
                gvEntryDocs.DataBind();
            }

            LinkButton _btnE = (LinkButton)sender;
            GridViewRow _row = (GridViewRow)_btnE.NamingContainer;

            Label _rowNo = (Label)_row.FindControl("lblRowNo");
            Label _id = (Label)_row.FindControl("lblRownumber");

            hfMainRowNo.Value = _rowNo.Text;


            //==============================================================
            int _rowIndex = 0;
            if (hfMainRowNo.Value.ToString().Length > 0)
            { _rowIndex = Convert.ToInt32(hfMainRowNo.Value); }

            if (ViewState["ClaimDetailGridDocs"] != null)
            {
                _table = (DataTable)ViewState["ClaimDetailGridDocs"];
                DataRow[] drsel = _table.Select("ClaimDetailRowNo='" + _rowIndex + "'");
                //===============================
                foreach (DataRow dr in drsel)
                {
                    AddDocEntryData(dr["InnerOnlyFileName"].ToString(), dr["InnerFileName"].ToString(), dr["InnerRowNo"].ToString());

                    string _file = dr["InnerFileName"].ToString();
                    string fileName = Path.Combine(AttachFileName, _file);
                    string fileNameDest = Path.Combine(AttachFileName + "/Buffer/", _file);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        FileInfo TheFile = new FileInfo(fileName);
                        if (TheFile.Exists)
                        {
                            File.Copy(fileName, fileNameDest, true);
                        }
                    }
                }
                DataTable _dt000 = (DataTable)ViewState["DocEntryData"];
                HFUploadFileNo.Value = "0";
                if (_dt000 != null && _dt000.Rows.Count > 0)
                {
                    HFUploadFileNo.Value = _dt000.Rows[_dt000.Rows.Count - 1]["RowNo"].ToString();
                }

                Dal.Get_Single_DataByPassingQuery("Insert into NL_UPLOADEDDOCS_Buffer(User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
                "App_Serial_No, FileNoPrefix, MainRowNo) (select User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
                "App_Serial_No, FileNoPrefix, MainRowNo from NL_UPLOADEDDOCS where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "')");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    string UploadDoc()
    {
        Int64 sumAll = 0;
        try
        {
            if (FindFile.HasFile)
            {
                string FileExt = Path.GetExtension(FindFile.PostedFile.FileName).ToLower();
                if (arrValidExtention.Contains(FileExt.ToLower()))
                {
                    string uplFilename = Path.GetFileName(FindFile.PostedFile.FileName).Split('.')[0].ToString();

                    if (hfMainRowNo.Value.ToString().Length == 0) { hfMainRowNo.Value = "0"; }
                    int _mainRowNo = Convert.ToInt32(hfMainRowNo.Value);
                    if (_mainRowNo == 0) { _mainRowNo += 1; }
                    hfMainRowNo.Value = _mainRowNo.ToString();

                    //if (Convert.ToInt32(hfEditedIndex.Value) > 0) _mainRowNo = Convert.ToInt32(hfEditedIndex.Value);

                    int _fileNo = Convert.ToInt32(HFUploadFileNo.Value) + 1;

                    string PerFix = spnSrNo.InnerText.ToString() + "_" + _mainRowNo.ToString() + "_" + _fileNo.ToString();
                    HFUploadFileNo.Value = _fileNo.ToString();

                    FileName = Path.GetFileName(FindFile.PostedFile.FileName).Replace(uplFilename, PerFix).ToString();
                    FileNameUpload = FileName;
                    string OnlyFileName = Path.GetFileName(FindFile.PostedFile.FileName);
                    hfUpOnlyFileNameN.Value = OnlyFileName;
                    FilePath = Convert.ToString(Path.GetFileName(FindFile.PostedFile.FileName));
                    FilePathUpload = FilePath;
                    Int64 TotalFileLen = Convert.ToInt64(FindFile.PostedFile.ContentLength);

                    DataTable _dt__ = (DataTable)ViewState["ClaimDetailGridDocs"];
                    if (_dt__ != null && _dt__.Rows.Count > 0)
                    {
                        foreach (DataRow _row in _dt__.Rows)
                        {
                            if (hfUpOnlyFileNameN.Value.ToString().Replace(" ", "_") == _row["InnerOnlyFileName"].ToString())
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqudoc", "alert('" + hfUpOnlyFileNameN.Value.ToString() + " already uploaded.');", true);
                                return "";
                            }
                        }
                    }
                    string ttlFileSizeS = Dal.Get_Single_DataByPassingQuery("select sum(filesize) from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "'");
                    int ttlFileSize = 0;
                    if (ttlFileSizeS.Length > 0)
                    { ttlFileSize = Convert.ToInt32(ttlFileSizeS); }
                    ttlFileSizeS = "";

                    if ((TotalFileLen + ttlFileSize) <= ttlmaxfileSize)
                    {
                        sumAll = TotalFileLen + sumAll;
                        if (sumAll <= maxfileSize)
                        {
                            string _resp = SaveFile(FindFile);
                            if (_resp == "Saved")
                            {
                                string insert = Dal.InsertPhoto(HFSAPID.Value, FileName.Replace(" ", "_"), "", HFSAPID.Value, Convert.ToString(TotalFileLen), OnlyFileName.Replace(" ", "_"), spnSrNo.InnerText, _fileNo.ToString(), (_mainRowNo).ToString());

                                return FileNameUpload;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Error : " + _resp + "');", true);
                            }
                        }
                        else
                        {
                            int sizMb = (maxfileSize / 1024) / 1024;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Maximum file size: " + Convert.ToString(sizMb) + " MB allowed.');", true);
                        }
                    }
                    else
                    {
                        int sizMb = (ttlmaxfileSize / 1024) / 1024;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Maximum file size: " + Convert.ToString(sizMb) + " MB allowed.');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Only .jpg, .jpeg, .png and .pdf are allowed.');", true);
                }

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Please Browse First.');", true);
            }
        }

        catch (Exception ex)
        {
            //Util.Log("Catch on file upload -- SAPID-- " + HFSAPID.Value + " error msg -- " + ex.Message, LogFilPath);
        }

        return "";
    }

    protected void butUpload_Click(object sender, EventArgs e)
    {
        try
        {
            trUploadGrid.Visible = true;
            string _file = UploadDoc();
            if (_file.Length > 0)
            {
                AddDocEntryData(hfUpOnlyFileNameN.Value, _file, HFUploadFileNo.Value);
                BindingInnerGrid();

                if (ViewState["ClaimDetailGridDocs"] == null)
                {
                    _table = new DataTable();
                    _table.Columns.Add(new DataColumn("ClaimDetailRowNo", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerID", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                    _table.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));
                }
                else
                {
                    _table = ((DataTable)ViewState["ClaimDetailGridDocs"]);
                }
                DataRow _row2 = _table.NewRow();

                int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
                int k0 = 1;
                foreach (GridViewRow _row0 in gvEntryDocs.Rows)
                {
                    Label _lbl0 = (Label)_row0.FindControl("lblFileName");
                    Label _lbl = (Label)_row0.FindControl("lblOnlyFileName");
                    Label _lblRow = (Label)_row0.FindControl("lblRowNo");

                    _row2 = _table.NewRow();
                    _row2["ClaimDetailRowNo"] = _rowIndex;
                    _row2["InnerID"] = k0;
                    _row2["InnerFileName"] = _lbl0.Text;
                    _row2["InnerOnlyFileName"] = Path.GetFileName(_lbl.Text);
                    _row2["InnerRowNo"] = _lblRow.Text;
                    _table.Rows.Add(_row2);
                    k0 += 1;
                }

                DataTable distinctTable = _table.DefaultView.ToTable( /*distinct*/ true);
                ViewState["ClaimDetailGridDocs"] = distinctTable;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void BindingInnerGrid()
    {
        try
        {
            if (ViewState["ClaimDetailGridDocs"] == null)
            {
                _table = new DataTable();
                _table.Columns.Add(new DataColumn("ClaimDetailRowNo", typeof(string)));
                _table.Columns.Add(new DataColumn("InnerID", typeof(string)));
                _table.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                _table.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                _table.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));
            }
            else
            {
                _table = ((DataTable)ViewState["ClaimDetailGridDocs"]);
            }

            DataRow _row2 = _table.NewRow();

            int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
            int k0 = 1;
            foreach (GridViewRow _row0 in gvEntryDocs.Rows)
            {
                Label _lbl0 = (Label)_row0.FindControl("lblFileName");
                Label _lbl = (Label)_row0.FindControl("lblOnlyFileName");
                Label _lblRow = (Label)_row0.FindControl("lblRowNo");

                _row2 = _table.NewRow();
                _row2["ClaimDetailRowNo"] = _rowIndex;
                _row2["InnerID"] = k0;
                _row2["InnerFileName"] = _lbl0.Text;
                _row2["InnerOnlyFileName"] = _lbl.Text;
                _row2["InnerRowNo"] = _lblRow.Text;
                _table.Rows.Add(_row2);
                k0 += 1;
            }

            DataTable distinctTable = _table.DefaultView.ToTable( /*distinct*/ true);
            ViewState["ClaimDetailGridDocs"] = distinctTable;

            foreach (GridViewRow dr in gvClaimDetail.Rows)
            {
                GridView _grd = (GridView)dr.FindControl("gvInnerDocs");
                Label _lbl = (Label)dr.FindControl("lblRowNo");

                if (_lbl.Text != "")
                {
                    _table = ((DataTable)ViewState["ClaimDetailGridDocs"]);
                    int _rowIndex0 = Convert.ToInt32(_lbl.Text);

                    DataRow[] drsel = _table.Select("ClaimDetailRowNo='" + _rowIndex0 + "'");

                    DataTable _table1 = new DataTable();
                    _table1.Columns.Add(new DataColumn("InnerID", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerFileName", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerOnlyFileName", typeof(string)));
                    _table1.Columns.Add(new DataColumn("InnerRowNo", typeof(string)));


                    foreach (DataRow Innerdr in drsel)
                    {
                        DataRow _row3 = _table1.NewRow();
                        _row3["InnerID"] = Innerdr["InnerID"];
                        _row3["InnerFileName"] = Innerdr["InnerFileName"];
                        _row3["InnerOnlyFileName"] = Innerdr["InnerOnlyFileName"];
                        _row3["InnerRowNo"] = Innerdr["InnerRowNo"];
                        _table1.Rows.Add(_row3);
                    }
                    _grd.DataSource = _table1;
                    _grd.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void AddDocEntryData(string OnlyFileName, string FileName, string UploadedFileNo)
    {
        int k = gvEntryDocs.Rows.Count + 1;

        if (ViewState["DocEntryData"] == null)
        {
            _table = new DataTable();

            _table.Columns.Add(new DataColumn("ID", typeof(string)));
            _table.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
            _table.Columns.Add(new DataColumn("FileName", typeof(string)));
            _table.Columns.Add(new DataColumn("RowNo", typeof(string)));
            _table.Columns.Add(new DataColumn("pk_id", typeof(string)));

            DataRow _row = _table.NewRow();

            _row["ID"] = k;
            _row["OnlyFileName"] = Path.GetFileName(OnlyFileName);
            _row["FileName"] = FileName;
            _row["RowNo"] = UploadedFileNo;
            _row["pk_id"] = k;

            _table.Rows.Add(_row);
            ViewState["DocEntryData"] = _table;
        }
        else
        {
            _table = ((DataTable)ViewState["DocEntryData"]);
            DataRow _row = _table.NewRow();

            _row["ID"] = k;
            _row["OnlyFileName"] = Path.GetFileName(OnlyFileName);
            _row["FileName"] = FileName;
            _row["RowNo"] = UploadedFileNo;
            _row["pk_id"] = k;

            _table.Rows.Add(_row);
            ViewState["DocEntryData"] = _table;
        }
        gvEntryDocs.DataSource = _table;
        gvEntryDocs.DataBind();
    }

    private string SaveFile(FileUpload FindFile)
    {
        try
        {
            string fileName = Path.Combine(AttachFileName, FileName.Replace(" ", "_"));
            FindFile.SaveAs(fileName);
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message.ToString(); }
    }
}
