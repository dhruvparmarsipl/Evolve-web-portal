#region"NameSpace"
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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using System.Xml;
using System.Net;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endregion


public partial class FpaApprover : System.Web.UI.Page
{
    #region"GlaobalVariable"
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    string logfile = "D:\\Eicher\\All Logs\\FPA";

    int maxFileLimit = 0;
    int maxfileSize = 0;
    int ttlmaxfileSize = 0;
    int fileSize = 0;
    int minfileCompressSize = 512000;
    int maxfileCompressSize = 12582912;

    string AttachFileName = string.Empty;
    string AttachFileName1 = string.Empty;
    string FileName = string.Empty;
    string FilePath = string.Empty;
    static string FileNameUpload = string.Empty;
    static string FilePathUpload = string.Empty;

    DataTable _table = null;
    public System.Collections.Generic.List<string> arrValidExtention = new System.Collections.Generic.List<string>();
    #endregion

    #region"Pageload"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                BindCD();
                string serialNumber = DoDecrypt(Request.QueryString["RID"]);
                HFSERIAL.Value = serialNumber;
                FillHeaderonquerystring(getSAPID(ReturnLoginName()), serialNumber);
                BindHeaderDDL();
                ////SetInitialRow();
                FillGrid(serialNumber);
                BindHistory();
                if (HFSTATUS.Value == "3")
                {
                    btnAdd.Visible = false;
                    butUpload.Visible = false;
                    FindFile.Visible = false;
                    gvClaimDetail.Columns[14].Visible = false;
                }
                else
                {
                    int YearEnd = Dal.checkYearEndBySerailNo(HFSAPID.Value, serialNumber);
                    if (YearEnd == 0)
                    {
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnDelete.Visible = false;
                        if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
                        {
                            btnDelete.Visible = true;
                        }

                        btnHold.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD1", "alert('Your year end has been marked. Please contact your FPA Admin .');", true);
                    }
                }
                FillOnHoldDocsGrid();
                SetDocData();
            }
            else
            {
                btnApprove.Visible = false;
                btnReject.Visible = false;
            }
            BindHeaderDDL();
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

        if (HFBU.Value.ToString() == "TBE" || HFBU.Value.ToString() == "EEC" || HFBU.Value.ToString() == "VBI")
        {
            rdbtnModeOfPayment.Items[0].Selected = true;
            rdbtnModeOfPayment.Enabled = false;
        }
    }
    #endregion

    #region"FillHeaderBySerialNumber"
    private void FillHeaderonquerystring(string sapid, string serialNumber)
    {
        DataTable dt_UInfo = Dal.getclaimUserInfoquerystring(sapid, serialNumber);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ENAME from master_employee_profile where Login_Name ='" + ReturnLoginName() + "'"); ;
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            spnFIAdmin.InnerText = Convert.ToString(dt_UInfo.Rows[0]["FIAdminLogin"]);
            spndepartment.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DEPARTMENT"]);
            SpnStatus.InnerText = GetStatus(Convert.ToString(dt_UInfo.Rows[0]["CLAIM_STATUS"]));
            spnSrNo.InnerText = DoDecrypt(Request.QueryString["RID"]);
            HFSTATUS.Value = Convert.ToString(dt_UInfo.Rows[0]["CLAIM_STATUS"]);
            HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            HFBU.Value = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
            HFCOSTCENTER.Value = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
            spnTotalFpaBalance.InnerText = Convert.ToString(GetTotalFpaBalance(HFSAPID.Value));
            SpnProcessClaim.InnerText = Convert.ToString(GetClaimInProcess(HFSAPID.Value, HFSERIAL.Value));

            ddlCD.SelectedValue = Convert.ToString(dt_UInfo.Rows[0]["Deviation"]);
            txtddlCD.Text = Convert.ToString(dt_UInfo.Rows[0]["DComments"]);
            spnccn.InnerText = Convert.ToString(dt_UInfo.Rows[0]["COSTCENTER"]);
            spnBU.InnerText = Convert.ToString(dt_UInfo.Rows[0]["BU"]);
        }
        validateForm();
        string Owner = Convert.ToString(dt_UInfo.Rows[0]["Owner"]).ToLower();
        if (Owner == ReturnLoginName())
        {
            if (HFSTATUS.Value == "2" || HFSTATUS.Value == "4" || HFSTATUS.Value == "6" || HFSTATUS.Value == "10")
            {
                txtComments.Enabled = false;
                btnDelete.Visible = true;
                btnHold.Visible = true;
                trcd.Visible = true;
                dvOnHoldDocumentsUpload.Visible = true;
                if (HFSTATUS.Value == "2")
                {
                    btnUpload.Visible = false;
                    FileUpload1.Visible = false;
                }
            }
            else
            {
                txtComments.Enabled = false;
                gvClaimDetail.Enabled = false;
                txtComments.ReadOnly = true;
                rdbtnModeOfPayment.Enabled = false;
                btnApprove.Visible = false;
                btnReject.Visible = false;
                txtFiadminComments.Enabled = false;
            }
        }
        else
        {
            txtComments.Enabled = false;
            //gvClaimDetail.Enabled = false;
            txtComments.ReadOnly = true;
            rdbtnModeOfPayment.Enabled = false;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            txtFiadminComments.Enabled = false;
            ddlHeads.Enabled = false;
            dvOnHoldDocumentsUpload.Visible = true;
            if (HFSTATUS.Value == "2" || HFSTATUS.Value == "3")
            {
                btnUpload.Visible = false;
                FileUpload1.Visible = false;
                if (HFSTATUS.Value == "3")
                {
                    gvOnholdDocs.Columns[3].Visible = false;
                    gvOnholdDocs.Columns[4].Visible = false;
                }
            }
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

    #region"FillClaimGridviewBySerialNumber"

    private void FillGrid(string serialNumber)
    {
        using (DataTable fpadt = Dal.getFpadetailData(serialNumber))
        {
            if (fpadt.Rows.Count > 0)
            {
                foreach (DataRow _row in fpadt.Rows)
                {
                    AddRowData(_row["HeadName"].ToString(), Convert.ToInt32(_row["FPA_HEAD_ID"].ToString()), _row["GL_CODE"].ToString(), Convert.ToDecimal(_row["AllocateAmount"].ToString()), Convert.ToDecimal(_row["HEAD_BAL"].ToString()), _row["EXPENSE_DATE"].ToString(), Convert.ToDecimal(_row["CLAIM_AMT"].ToString()), _row["Details"].ToString(), _row["BillNo"].ToString(), Convert.ToInt32(_row["RowNo"].ToString() == "" ? "0" : _row["RowNo"].ToString()));


                    /////AddRowData(_row["HeadName"].ToString(), Convert.ToInt32(_row["FPA_HEAD_ID"].ToString()), _row["GL_CODE"].ToString(), Convert.ToDecimal(_row["AllocateAmount"].ToString()), Convert.ToDecimal(_row["HEAD_BAL"].ToString()), _row["EXPENSE_DATE"].ToString(), Convert.ToDecimal(_row["CLAIM_AMT"].ToString()), _row["Details"].ToString(), "123", 0);
                }


                gvClaimDetail.DataSource = fpadt;
                gvClaimDetail.DataBind();
                int rownumber = 1;
                int rowIndex = 0;
                for (int i = 0; i < fpadt.Rows.Count; i++)
                {
                    Label lblRownumber = (Label)gvClaimDetail.Rows[rowIndex].Cells[1].FindControl("lblRownumber");
                    DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                    Label Glcode = (Label)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
                    Label Allocation = (Label)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
                    Label txtHeadBalance = (Label)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
                    Label txtExpenseDate1 = (Label)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate1");
                    Label txtBillAmount = (Label)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
                    Label txtDetail = (Label)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");
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
                    txtExpenseDate1.Text = Convert.ToString(fpadt.Rows[i]["EXPENSE_DATE"]);
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
                    foreach (GridViewRow grow in gvClaimDetail.Rows)
                    {
                        LinkButton lnkDelete = (LinkButton)grow.FindControl("lnkDelete");
                        lnkDelete.Visible = false;
                    }
                }
            }
        }
    }

    //private void FillGrid(string serialNumber)
    //{
    //    DataTable fpadt = Dal.getFpadetailData(serialNumber);

    //    if (fpadt.Rows.Count > 0)
    //    {
    //        gvClaimDetail.DataSource = fpadt;
    //        gvClaimDetail.DataBind();
    //        int rownumber = 1;
    //        int rowIndex = 0;
    //        for (int i = 0; i < fpadt.Rows.Count; i++)
    //        {
    //            Label lblRownumber = (Label)gvClaimDetail.Rows[rowIndex].Cells[1].FindControl("lblRownumber");
    //            DropDownList ddlHeads = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
    //            TextBox Glcode = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[3].FindControl("txtGlcode");
    //            TextBox Allocation = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[4].FindControl("txtAllocation");
    //            TextBox txtHeadBalance = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[5].FindControl("txtHeadBalance");
    //            TextBox txtExpenseDate = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[6].FindControl("txtExpenseDate");
    //            TextBox txtBillAmount = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[7].FindControl("txtBillAmount");
    //            TextBox txtDetail = (TextBox)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("txtDetail");
    //            Label lblDetail = (Label)gvClaimDetail.Rows[rowIndex].Cells[8].FindControl("lblDetail");
    //            Glcode.Enabled = false;
    //            Allocation.Enabled = false;
    //            txtHeadBalance.Enabled = false;
    //            lblRownumber.Text = Convert.ToString(rownumber);
    //            ddlHeads.ClearSelection();
    //            ddlHeads.Items.FindByValue(fpadt.Rows[i]["FPA_HEAD_ID"].ToString()).Selected = true;
    //            Glcode.Text = Convert.ToString(fpadt.Rows[i]["GL_CODE"]);
    //            Allocation.Text = Convert.ToString(fpadt.Rows[i]["AllocateAmount"]);
    //            txtHeadBalance.Text = Convert.ToString(fpadt.Rows[i]["HEAD_BAL"]);
    //            txtExpenseDate.Text = Convert.ToString(fpadt.Rows[i]["EXPENSE_DATE"]);
    //            txtBillAmount.Text = Convert.ToString(fpadt.Rows[i]["CLAIM_AMT"]);
    //            txtDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]);
    //            lblDetail.Text = Convert.ToString(fpadt.Rows[i]["DETAILS"]);
    //                if (HFSTATUS.Value == "1" || HFSTATUS.Value == "5" || HFSTATUS.Value == "7")
    //                {
    //                    txtDetail.Visible = true;
    //                    lblDetail.Visible = false;
    //                }
    //                else
    //                {  
    //                    lblDetail.Visible = true;
    //                    txtDetail.Visible = false;
    //                }
    //            rownumber++;
    //            rowIndex++;

    //        }
    //        //rdbtnModeOfPayment.Items.FindByValue
    //        if (fpadt.Rows[0]["VOUCHER_TYPE"].ToString().Trim() == "KE")
    //        {
    //            rdbtnModeOfPayment.SelectedValue = "1";
    //        }
    //        else
    //        {
    //            rdbtnModeOfPayment.SelectedValue = "2";
    //        }
    //        txtComments.Text = Convert.ToString(fpadt.Rows[0]["COMMENT"]);
    //        txtFiadminComments.Text = Convert.ToString(fpadt.Rows[0]["FIADMIN_COMMENT"]);
    //        object totamount = fpadt.Compute("Sum(CLAIM_AMT)", "");
    //        spnTotalFpaClaim.InnerText = Convert.ToString(totamount);
    //        //Store the DataTable in ViewState
    //        ViewState["CurrentTable"] = fpadt;
    //        if (HFSTATUS.Value == "3")
    //        {
    //            foreach (GridViewRow grow in gvClaimDetail.Rows)
    //            {
    //                LinkButton lnkDelete = (LinkButton)grow.FindControl("lnkDelete");
    //                lnkDelete.Visible = false;
    //            }
    //        }


    //    }
    //}
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

    #region"ValidateForm"
    private void validateForm()
    {
        if (spnFIAdmin.InnerText == "" || spnEmpCode.InnerText == "")
        {
            btnApprove.Visible = false;
            btnReject.Visible = false;
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

    #region"UserTotalFPABalanceBySapId"
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

        ////DataTable dt = new DataTable();
        ////DataRow dr = null;
        ////dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        //////dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        ////dt.Columns.Add(new DataColumn("FPA_HEAD_ID", typeof(string)));
        ////dt.Columns.Add(new DataColumn("GL_CODE", typeof(string)));
        ////dt.Columns.Add(new DataColumn("AllocateAmount", typeof(string)));
        ////dt.Columns.Add(new DataColumn("HEAD_BAL", typeof(string)));
        ////dt.Columns.Add(new DataColumn("EXPENSE_DATE", typeof(string)));
        ////dt.Columns.Add(new DataColumn("CLAIM_AMT", typeof(double)));
        ////dt.Columns.Add(new DataColumn("DETAILS", typeof(string)));

        ////dr = dt.NewRow();
        ////dr["RowNumber"] = 1;
        //////dr["Column1"] = string.Empty;
        ////dr["FPA_HEAD_ID"] = string.Empty;
        ////dr["GL_CODE"] = string.Empty;
        ////dr["AllocateAmount"] = string.Empty;
        ////dr["HEAD_BAL"] = string.Empty;
        ////dr["EXPENSE_DATE"] = string.Empty;
        ////dr["CLAIM_AMT"] = 0.0;
        ////dr["DETAILS"] = string.Empty;
        ////dt.Rows.Add(dr);

        //////Store the DataTable in ViewState
        ////ViewState["CurrentTable"] = dt;

        ////gvClaimDetail.DataSource = dt;
        ////gvClaimDetail.DataBind();
        AddRow();
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
                    gvClaimDetail.DataSource = dt;
                    gvClaimDetail.DataBind();
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
            txtGlcode.Enabled = false;
            txtAllocation.Enabled = false;
            txtHeadBalance.Enabled = false;
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

    #region"ApprovedByFIadmin"
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnApprove.Visible = false;
            btnReject.Visible = false;
            string Comment = string.Empty;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string EmailID = HFEMAILS.Value.ToString().Split('#')[0]; // User Email ID
            string modeofpayment = string.Empty;
            if (rdbtnModeOfPayment.SelectedValue == "1")
            {
                modeofpayment = "KE";
            }
            else
            {
                modeofpayment = "SK";
            }

            if (txtFiadminComments.Text.Length <= 500)
            {
                Comment = Convert.ToString(txtFiadminComments.Text);
            }
            else
            {
                Comment = Convert.ToString(txtFiadminComments.Text).Substring(0, 500);
            }
            string XmlFpaClaimDetail = GetFpaDetail();
            string FpaEmplyeeDistribution = GetFpaDistribution();

            int TransID = 0;
            if (spnTotalFpaClaim.InnerText == "")
            {
                spnTotalFpaClaim.InnerText = "0";
            }
            if ((Convert.ToDouble(spnTotalFpaClaim.InnerText)) + Convert.ToDouble(SpnProcessClaim.InnerText) <= Convert.ToDouble(spnTotalFpaBalance.InnerText))
            {
                if (Convert.ToDouble(spnTotalFpaClaim.InnerText) >= 500)
                {
                    if (IsDocsUploadedInDMS())
                        TransID = Dal.InsertFpaClaimDetailByApprover(spnEmpCode.InnerText, spnSrNo.InnerText, modeofpayment, "3", Comment, HFBU.Value, HFSAPID.Value, "", HFCOSTCENTER.Value, spnTotalFpaClaim.InnerText, XmlFpaClaimDetail, FpaEmplyeeDistribution, ddlCD.SelectedValue, txtddlCD.Text);
                    if (TransID == 1)
                    {
                        Util.Log("Approve by FI Admin  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
                        CommonMail(EmailID.Trim(), "3");
                        Util.Log("Mail Send  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
                        Dal.CreateRejectLogHistory("3", spnLoginUser.InnerText.ToString(), Convert.ToString(Comment), spnSrNo.InnerText.ToString());
                        Util.Log("Log History  form ID -- " + spnSrNo.InnerText.ToString(), logfile);
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Claim is approved successfully.');CloseRefreshWindow();", true);
                    }
                    else if (TransID == 0)
                    {
                        Util.Log("Transaction failed -- " + spnSrNo.InnerText.ToString(), logfile);
                        btnApprove.Visible = true;
                        btnReject.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                    }
                    else if (TransID == 5)
                    {
                        Util.Log("Data Already Submit -- " + spnSrNo.InnerText.ToString(), logfile);
                        btnApprove.Visible = true;
                        btnReject.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Request already submitted.');CloseRefreshWindow();", true);
                    }
                }
                else
                {
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Minimum Claim amount is INR 500 ')", true);

                }
            }
            else
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Bill Amount is not greater than Total Fpa Claim Amount Balance')", true);
            }
        }
        catch (Exception ex)
        {
            Util.Log("catch exception -- " + spnSrNo.InnerText.ToString(), logfile);
            btnApprove.Visible = true;
            btnReject.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }


    }
    #endregion

    #region"RejectByFIadmin"
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(1000);
            btnApprove.Visible = false;
            btnReject.Visible = false;
            string Comment = string.Empty;
            string CStatus = string.Empty;
            string newUrl = string.Empty;
            string Owner = string.Empty;
            string EmailID = HFEMAILS.Value.ToString().Split('#')[0]; // User Email ID
            int RetStatus = 0;
            if (txtFiadminComments.Text.Length <= 250)
            {
                Comment = Convert.ToString(txtFiadminComments.Text);
            }
            else
            {
                Comment = Convert.ToString(txtFiadminComments.Text).Substring(0, 250);
            }
            if (txtFiadminComments.Text == "")
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please enter comments for rejection.');", true);
            }

            else
            {
                RetStatus = Dal.UpdateClaimDataBySRNO(spnSrNo.InnerText, "7", Comment);
                if (RetStatus == 1)
                {
                    CStatus = "7";
                    CommonMail(EmailID.Trim(), CStatus);
                    Dal.CreateRejectLogHistory(CStatus, spnLoginUser.InnerText.ToString(), Convert.ToString(Comment), spnSrNo.InnerText.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Form rejected successfully.');CloseRefreshWindow();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            btnApprove.Visible = true;
            btnReject.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
        }
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
                newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/FPAProcessedClaim.aspx";
            }
        }
        try
        {
            if (Status == "7")
            {
                if (txtFiadminComments.Text.Length <= 500)
                {
                    comment = Convert.ToString(txtFiadminComments.Text);
                }
                else
                {
                    comment = Convert.ToString(txtFiadminComments.Text).Substring(0, 500);
                }
                //comment = Convert.ToString(txtApproverComment.Text);
                Subject = "FPA Claim request " + SerailNo + " has been rejected...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Claim request <b>" + SerailNo + "</b> has been rejected by " + Approvername + ". Reason for rejecting the request is <br/><br/> \"<span style='color:red;'>" + comment + "</span>\"<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            else if (Status == "3")
            {
                Subject = "FPA Claim request " + SerailNo + " has been processed...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Claim request has been processed by " + Approvername + ".<br /><br />Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            Util.Log("Try in CommonMail Sent to : " + MailTo.ToString() + "-----------MailBody<br/>", logfile);
        }
        catch (Exception ex)
        {
            Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion

    #region"UpdateEployeeFPADistribution"
    private string GetFpaDistribution()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<root>");
        int ClaimAmount = 0;
        DropDownList ddlhead = new DropDownList();
        Label txtBillAmount = new Label();
        string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
        Label txtFPA_HEAD_ID = new Label();
        List<string> checklist = new List<string>();
        List<EmployeeFpaDistribution> FinalList = new List<EmployeeFpaDistribution>();

        foreach (GridViewRow grow in gvClaimDetail.Rows)
        {
            EmployeeFpaDistribution empf = new EmployeeFpaDistribution();

            ddlhead = (DropDownList)grow.FindControl("ddlHeads");
            txtFPA_HEAD_ID = (Label)grow.FindControl("txtFPA_HEAD_ID");
            txtBillAmount = (Label)grow.FindControl("txtBillAmount");
            if (checklist.Contains(ddlhead.SelectedValue))
            {
                ClaimAmount = ClaimAmount + Convert.ToInt32(txtBillAmount.Text);
                foreach (var account in FinalList.FindAll(a => a.Fpa_Head_Id == ddlhead.SelectedValue))
                {
                    // account.ClaimAmount = ClaimAmount;
                    account.ClaimAmount = account.ClaimAmount + Convert.ToInt32(txtBillAmount.Text);
                }
            }
            else
            {
                checklist.Add(ddlhead.SelectedValue);
                ClaimAmount = Convert.ToInt32(txtBillAmount.Text);
                empf.Fpa_Cycle_Id = FpaCyleId;
                empf.Fpa_Head_Id = txtFPA_HEAD_ID.Text;
                empf.ClaimAmount = ClaimAmount;
                FinalList.Add(empf);
            }
        }
        foreach (var a in FinalList)
        {
            sb.Append("<row Fpa_Cycle_Id='" + a.Fpa_Cycle_Id + "' Fpa_Head_Id='" + a.Fpa_Head_Id + "' ClaimAmount='" + a.ClaimAmount + "' />");
        }
        sb.Append("</root>");

        return sb.ToString();
    }
    #endregion

    #region"EployeeFPADistributionClass"
    public class EmployeeFpaDistribution
    {
        public string Fpa_Cycle_Id { get; set; }
        public string Fpa_Head_Id { get; set; }
        public int ClaimAmount { get; set; }
    }
    #endregion

    ////#region"UserFPADetailByCycleID"
    ////private string GetFpaDetail()
    ////{
    ////    string XmlDataGridItems = string.Empty;
    ////    using (DataTable dt = new DataTable("FpaClaim"))
    ////    {
    ////        dt.Columns.Add("FPA_CYCLE_ID", typeof(string));
    ////        dt.Columns.Add("FPA_HEAD_ID", typeof(string));
    ////        dt.Columns.Add("HEAD_BAL", typeof(string));
    ////        dt.Columns.Add("CLAIM_AMT", typeof(string));
    ////        dt.Columns.Add("EXPENSE_DATE", typeof(string));
    ////        dt.Columns.Add("GL_CODE", typeof(string));
    ////        dt.Columns.Add("DETAILS", typeof(string));
    ////        dt.Columns.Add("CYCLENO", typeof(int));
    ////        string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
    ////        int i = 0;
    ////        foreach (GridViewRow grow in gvClaimDetail.Rows)
    ////        {
    ////            DropDownList ddlhead = (DropDownList)grow.FindControl("ddlHeads");
    ////            TextBox Glcode = (TextBox)grow.FindControl("txtGlcode");
    ////            TextBox Allocation = (TextBox)grow.FindControl("txtAllocation");
    ////            TextBox txtHeadBalance = (TextBox)grow.FindControl("txtHeadBalance");
    ////            TextBox txtExpenseDate = (TextBox)grow.FindControl("txtExpenseDate");
    ////            TextBox txtBillAmount = (TextBox)grow.FindControl("txtBillAmount");
    ////            TextBox txtDetail = (TextBox)grow.FindControl("txtDetail");
    ////            DataRow dr = dt.NewRow();
    ////            dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
    ////            dr["FPA_HEAD_ID"] = Convert.ToString(ddlhead.SelectedValue);
    ////            dr["HEAD_BAL"] = Convert.ToString(Convert.ToDouble(txtHeadBalance.Text) - Convert.ToDouble(txtBillAmount.Text));
    ////            dr["CLAIM_AMT"] = Convert.ToString(txtBillAmount.Text);
    ////            dr["EXPENSE_DATE"] = Convert.ToString(txtExpenseDate.Text);
    ////            dr["GL_CODE"] = Convert.ToString(Glcode.Text);
    ////            dr["DETAILS"] = Convert.ToString(txtDetail.Text);
    ////            dr["CYCLENO"] = Convert.ToString(i);
    ////            dt.Rows.Add(dr);
    ////            i++;

    ////        }
    ////        StringWriter strFpaHeadWriter = new StringWriter();
    ////        dt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
    ////        XmlDataGridItems = strFpaHeadWriter.ToString();
    ////    }
    ////    return XmlDataGridItems;
    ////}
    ////#endregion

    #region"BindHeadInClaimGridview"
    protected void gvClaimDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // using (DataTable fpaheads = Dal.getFpaheadsDetailByUser(HFSAPID.Value)) //
                using (DataTable fpaheads = Dal.getFpaheadsDetailByUser(spnSrNo.InnerText))
                {
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
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region"AllocateBalance,Glcode,HeadBalance"
    protected void ddlHeads_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtAllocation.Text = Convert.ToString(GetAllocateBalance(HFSAPID.Value, ddlHeads.SelectedValue));
            txtAllocation.Enabled = false;
            txtGlcode.Text = Convert.ToString(GetGlCode(HFSAPID.Value, ddlHeads.SelectedValue));
            txtHeadBalance.Text = Convert.ToString(GetHeadBalance((HFSAPID.Value), ddlHeads.SelectedValue));
            txtHeadBalance.Enabled = false;
            if (!string.IsNullOrEmpty(txtGlcode.Text))
            {
                txtGlcode.Enabled = false;
                ////btnSaveDraft.Visible = true;
                btnApprove.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Your FPA has not been reinitiated. Please contact FPA admin. ')", true);
                ////btnSaveDraft.Visible = false;
                btnApprove.Visible = false;
                txtGlcode.Enabled = false;
                txtAllocation.Text = "";
                txtHeadBalance.Text = "";
            }
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
        if (txtFiadminComments.Text.Length <= 250)
        {
            comment = Convert.ToString(txtFiadminComments.Text);
        }
        else
        {
            comment = Convert.ToString(txtFiadminComments.Text).Substring(0, 250);
        }
        if (!string.IsNullOrEmpty(comment))
        {
            int DelId = Dal.UpdateClaimDataBySRNO(spnSrNo.InnerText, "10", txtFiadminComments.Text);
            if (DelId == 1)
            {
                Dal.CreateRejectLogHistory("10", spnLoginUser.InnerText.ToString(), comment, spnSrNo.InnerText.ToString());
                Subject = "FPA Claim request " + SerailNo + " has been put on hold...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + spnFormCreater.InnerText + ",<br /><br />Your FPA Claim request with serial no. <b>" + SerailNo + "</b> has been put on hold .<br/>FI Admin comment: \"<span style='color:red;'>" + comment + "</span>\"<br />Please  click on  the link below for detail of claim.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/>Thanks and Regards,<br /><br />Finance Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                ////Util.SendMail(EmailID.Trim(), "ewadmin@eicher.in", FiEmailID.Trim(), Subject, Mailbody);
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

    #region"Delete Claim"
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string UserType = string.Empty;
        string Comment = string.Empty;
        string owner = getSAPID(ReturnLoginName());
        if (HFSTATUS.Value == "2" || HFSTATUS.Value == "10")
        {
            UserType = "FIAdmin";
            Comment = txtFiadminComments.Text;
            if (!string.IsNullOrEmpty(Comment))
            {
                int DelId = Dal.DeleteFpaClaim(spnSrNo.InnerText, "0");
                if (DelId == 1)
                {
                    int RetId = Dal.DeleteTravelLogHistory(spnSrNo.InnerText, owner, Comment, "FPA", UserType, HFSAPID.Value);
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

    private void BindCD()
    {
        string sqlgetCD = "select Id,Name from [dbo].[master_claimDeviation] where app='FPA' and isactive =1";
        DataTable dtCD = Dal.Get_DataByPassingQuery(sqlgetCD);
        ddlCD.DataSource = dtCD;
        ddlCD.DataTextField = "Name";
        ddlCD.DataValueField = "Id";
        ddlCD.DataBind();
        ddlCD.Items.Insert(0, new ListItem("--Select deviation--", "0"));
    }

    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        //return "fiadminfpa";
        return "spstestnr";
        //return "spstestss";
    }

    #region FPAClaimCreatedBySharad

    #region EntrySectionControls
    protected void txtBillAmount_TextChanged1(object sender, EventArgs e)
    {

    }
    protected void txtBillAmount_TextChanged2(object sender, EventArgs e)
    {

    }
    protected void butUpload_Click(object sender, EventArgs e)
    {
        string _file = UploadDoc();
        if (_file.Length > 0)
        {
            AddDocEntryData(hfUpOnlyFileNameN.Value, _file, HFUploadFileNo.Value, null);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ////var enCulture = new System.Globalization.CultureInfo("en-us");
        ////DateTime _dt = DateTime.ParseExact(txtExpenseDate.Text, "dd/MMM/yyyy", enCulture);

        string _dt = txtExpenseDate.Text;

        DataSet ExistingClaims = Dal.IsExpExists_InDB(HFSAPID.Value.ToString(),
            Convert.ToInt32(Convert.ToString(ddlHeads.SelectedValue.Split('#')[0])),
            Convert.ToInt32(txtBillAmount.Text), _dt, txtBillNo.Text, spnSrNo.InnerText);

        if (Convert.ToInt32(ExistingClaims.Tables[0].Rows[0][0]) > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "UniquDataForFPAClaim", "alert('Claim for " + ddlHeads.SelectedItem.Text + " for Rs. " + txtBillAmount.Text + " with bill no : " + txtBillNo.Text + " dated " + _dt + " for Serial No " + ExistingClaims.Tables[1].Rows[0][0] + " already exists !');", true);
            return;
        }

        AddRow();
    }
    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string stHead = ddlHeads.SelectedItem.Text;
            string stHeadCode = ddlHeads.SelectedValue.ToString();

            string stAll = txtAllocation.Text;
            string stGL = txtGlcode.Text;
            string stHdBal = txtHeadBalance.Text;

            string stClaim = txtBillAmount.Text;
            string stDate = txtExpenseDate.Text;
            string stBillNo = txtBillNo.Text;
            string stDetail = txtDetail.Text;

            DateTime _dteT = Convert.ToDateTime(txtExpenseDate.Text);
            string _dt = txtExpenseDate.Text.ToString();
            _dt = _dt.Substring(6, 4) + "-" + _dt.Substring(0, 2) + "-" + _dt.Substring(3, 2);


            if (!string.IsNullOrEmpty(stClaim) && !string.IsNullOrEmpty(stDate) && !string.IsNullOrEmpty(stBillNo) && !string.IsNullOrEmpty(stDetail))
            {
                _table = ((DataTable)ViewState["ItemGrid"]);
                string selID = Convert.ToString(HFIDEdit.Value);
                DataRow[] drsel = _table.Select("RowNumber='" + selID + "'");
                foreach (DataRow dr in drsel)
                {
                    //dr["RowNumber"] = hfMainRowNo.Value.ToString();
                    dr["HeadName"] = stHead;
                    dr["FPA_HEAD_ID"] = stHeadCode;
                    dr["GL_CODE"] = stGL;
                    dr["AllocateAmount"] = string.Format("{0:0}", stAll);
                    dr["HEAD_BAL"] = string.Format("{0:0}", stHdBal);
                    dr["EXPENSE_DATE"] = stDate;
                    dr["CLAIM_AMT"] = string.Format("{0:0}", stClaim);
                    dr["Details"] = stDetail;
                    dr["BillNo"] = stBillNo;
                    dr["RowNo"] = hfMainRowNo.Value.ToString();
                }
                _table.AcceptChanges();
                ViewState["ItemGrid"] = _table;
                object sumObject = _table.Compute("Sum(CLAIM_AMT)", "");
                spnTotalFpaClaim.InnerText = Convert.ToString(sumObject.ToString());
                gvClaimDetail.EditIndex = -1;

                gvClaimDetail.DataSource = _table;
                gvClaimDetail.DataBind();

                int rownumber = 1;
                int rowIndex = 0;
                for (int i = 0; i < _table.Rows.Count; i++)
                {
                    DropDownList ddlHeads_gv = (DropDownList)gvClaimDetail.Rows[rowIndex].Cells[2].FindControl("ddlHeads");
                    ddlHeads_gv.ClearSelection();
                    ddlHeads_gv.Items.FindByValue(_table.Rows[i]["FPA_HEAD_ID"].ToString()).Selected = true;

                    rownumber++;
                    rowIndex++;
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Claim Amount, Expense Date and Details can not be blank.');", true);
            }

            int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
            hfMainRowNo.Value = (Convert.ToInt32(_table.Rows[_table.Rows.Count - 1]["RowNo"].ToString()) + 1).ToString();
            //===============================
            if (ViewState["DocEntryDataAll"] == null)
            {
                _table = new DataTable();
                _table.Columns.Add(new DataColumn("MainID", typeof(string)));
                _table.Columns.Add(new DataColumn("ID", typeof(string)));
                _table.Columns.Add(new DataColumn("File_Name", typeof(string)));
                _table.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));

                _table.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));

                _table.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
            }
            else
            {
                _table = (DataTable)ViewState["DocEntryDataAll"];
                DataRow[] drselD = _table.Select("MainID='" + _rowIndex + "'");
                foreach (DataRow dr in drselD)
                {
                    _table.Rows.Remove(dr);
                }
                _table.AcceptChanges();

                //Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where MainRowNo = " + _rowIndex);
            }
            DataRow _row = _table.NewRow();

            int k = 1;
            foreach (GridViewRow _row0 in dgvEntryDocs.Rows)
            {
                Label _lbl0 = (Label)_row0.FindControl("lblFileName");
                Label _lbl = (Label)_row0.FindControl("lblOnly_File_Name");
                Label _lblRowNo = (Label)_row0.FindControl("lblRowNo");
                Label lblDocIDInDMS = (Label)_row0.FindControl("lblDocIDInDMS");

                _row = _table.NewRow();
                _row["MainID"] = _rowIndex;
                _row["ID"] = k;
                _row["File_Name"] = _lbl0.Text;
                _row["OnlyFileName"] = Path.GetFileName(_lbl.Text);
                _row["DocIDInDMS"] = lblDocIDInDMS.Text;
                _row["NL_RowNo"] = _lblRowNo.Text;
                _table.Rows.Add(_row);
            }
            ViewState["DocEntryDataAll"] = _table;
            //===============================

            //==============================================================
            _table = (DataTable)ViewState["DocEntryDataAll"];
            foreach (GridViewRow _row1 in gvClaimDetail.Rows)
            {
                //--------------
                GridView _grd = (GridView)_row1.FindControl("dgvEntryDocs");
                Label _lbl = (Label)_row1.FindControl("lblRowNo");

                int _rowIndex0 = Convert.ToInt32(_lbl.Text);
                DataRow _row2 = _table.NewRow();

                DataRow[] drsel = _table.Select("MainID='" + _rowIndex0 + "'");
                //===============================
                DataTable _table1 = new DataTable();
                _table1.Columns.Add(new DataColumn("ID", typeof(string)));
                _table1.Columns.Add(new DataColumn("File_Name", typeof(string)));
                _table1.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
                _table1.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));
                _table1.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
                _row2 = _table1.NewRow();
                foreach (DataRow dr in drsel)
                {
                    _row2 = _table1.NewRow();
                    _row2["ID"] = dr["ID"];
                    _row2["File_Name"] = dr["File_Name"];
                    _row2["OnlyFileName"] = dr["OnlyFileName"];
                    _row2["DocIDInDMS"] = dr["DocIDInDMS"];
                    _row2["NL_RowNo"] = dr["NL_RowNo"];
                    _table1.Rows.Add(_row2);
                }
                _grd.DataSource = _table1;
                _grd.DataBind();
            }
            //==============================================================

            //==============================================================
            //Delete from buffer
            //==============================================================
            string _str = "";
            _str = "Select File_Name from NL_UPLOADEDDOCS_buffer where App_Serial_No='" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            DataTable _dta = Dal.Get_DataByPassingQuery(_str);
            foreach (DataRow _rowD in _dta.Rows)
            {
                string _file = _rowD["File_Name"].ToString();
                //string fileName = Path.Combine(AttachFileName, _file);
                string fileNameSrc = Path.Combine(AttachFileName + "/Buffer/", _file);
                if (!string.IsNullOrEmpty(fileNameSrc))
                {
                    FileInfo TheFile = new FileInfo(fileNameSrc);
                    if (TheFile.Exists)
                    {
                        File.Delete(fileNameSrc);
                    }
                }
            }
            _dta.Dispose();

            _str = "Delete from NL_UPLOADEDDOCS_Buffer where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            Dal.Get_Single_DataByPassingQuery(_str);
            //==============================================================

            _table = ((DataTable)ViewState["DocEntryData"]);
            if (_table != null)
            {
                _table.Rows.Clear();
                dgvEntryDocs.DataSource = null;
                dgvEntryDocs.DataBind();
            }

            gvClaimDetail.Columns[14].Visible = true;

            lnkUpdate.Visible = false;
            lnkCancel.Visible = false;

            ddlHeads.Enabled = true;
            btnAdd.Visible = true;

            hfEditedIndex.Value = "0";
            HFIDEdit.Value = "0";
            HFUploadFileNo.Value = "0";

            ddlHeads.SelectedIndex = 0;
            txtAllocation.Text = "";
            txtGlcode.Text = "";
            txtHeadBalance.Text = "";
            txtExpenseDate.Text = "";
            txtBillAmount.Text = "";
            txtBillNo.Text = "";
            txtDetail.Text = "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        LinkButton _btnU = (LinkButton)sender;
        GridViewRow _row = gvClaimDetail.Rows[Convert.ToInt32(hfEditedIndex.Value)];

        Label lblRowNo = (Label)_row.FindControl("lblRowNo");
        int _rowIndex = Convert.ToInt32(lblRowNo.Text);

        string _str = "";

        _str = "Select File_Name from NL_UPLOADEDDOCS_Buffer where App_Serial_No='" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
        DataTable _dt = Dal.Get_DataByPassingQuery(_str);
        if (_dt.Rows.Count > 0)
        {
            _str = "Select File_Name from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            _dt = Dal.Get_DataByPassingQuery(_str);
            foreach (DataRow _rowD in _dt.Rows)
            {
                string _file = _rowD["File_Name"].ToString();
                string fileName = Path.Combine(AttachFileName, _file);
                string fileNameSrc = Path.Combine(AttachFileName + "/Buffer/", _file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    FileInfo TheFile = new FileInfo(fileName);
                    if (TheFile.Exists)
                    {
                        File.Delete(fileName);
                    }
                }
            }
            _dt.Dispose();

            _str = "Select File_Name from NL_UPLOADEDDOCS_Buffer where App_Serial_No='" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            _dt = Dal.Get_DataByPassingQuery(_str);
            foreach (DataRow _rowD in _dt.Rows)
            {
                string _file = _rowD["File_Name"].ToString();
                string fileName = Path.Combine(AttachFileName, _file);
                string fileNameSrc = Path.Combine(AttachFileName + "/Buffer/", _file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    FileInfo TheFileB = new FileInfo(fileNameSrc);
                    if (TheFileB.Exists)
                    {
                        File.Copy(fileNameSrc, fileName);
                        File.Delete(fileNameSrc);
                    }
                }
            }
            _dt.Dispose();

            _str = "Delete from NL_UPLOADEDDOCS where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            Dal.Get_Single_DataByPassingQuery(_str);

            _str = "Insert into NL_UPLOADEDDOCS(User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
            "App_Serial_No, FileNoPrefix, MainRowNo) (select User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
            "App_Serial_No, FileNoPrefix, MainRowNo from NL_UPLOADEDDOCS_Buffer where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "')";
            Dal.Get_Single_DataByPassingQuery(_str);

            _str = "Delete from NL_UPLOADEDDOCS_Buffer where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "'";
            Dal.Get_Single_DataByPassingQuery(_str);
        }

        _table = ((DataTable)ViewState["DocEntryData"]);
        if (_table != null)
        {
            _table.Rows.Clear();
            dgvEntryDocs.DataSource = null;
            dgvEntryDocs.DataBind();
        }

        gvClaimDetail.Columns[14].Visible = true;

        lnkUpdate.Visible = false;
        lnkCancel.Visible = false;

        ddlHeads.Enabled = true;
        btnAdd.Visible = true;

        //=============================================================================================
        _table = (DataTable)ViewState["ItemGrid"];
        hfMainRowNo.Value = (Convert.ToInt32(_table.Rows[_table.Rows.Count - 1]["RowNo"].ToString()) + 1).ToString();
        //=============================================================================================

        hfEditedIndex.Value = "";
        HFIDEdit.Value = "";

        ddlHeads.SelectedIndex = -1;
        txtGlcode.Text = "";
        txtAllocation.Text = "";
        txtHeadBalance.Text = "";
        txtExpenseDate.Text = "";
        txtBillAmount.Text = "";
        txtBillNo.Text = "";
        txtDetail.Text = "";
    }
    #endregion

    #region gvClaimDetails
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton _btnE = (LinkButton)sender;
            GridViewRow _row = (GridViewRow)_btnE.NamingContainer;
            Label _hd = (Label)_row.FindControl("txtHead");
            Label _expDt = (Label)_row.FindControl("txtExpenseDate1");

            Label _gl = (Label)_row.FindControl("txtGlcode");
            Label _all = (Label)_row.FindControl("txtAllocation");
            Label _hdbal = (Label)_row.FindControl("txtHeadBalance");

            Label _amt = (Label)_row.FindControl("txtBillAmount");
            Label _billNo = (Label)_row.FindControl("txtBillNo");
            Label _detail = (Label)_row.FindControl("txtDetail");

            Label _rowNo = (Label)_row.FindControl("lblRowNo");
            Label _id = (Label)_row.FindControl("lblRownumber");

            hfMainRowNo.Value = _rowNo.Text;

            HFIDEdit.Value = _id.Text.ToString();
            hfEditedIndex.Value = _row.RowIndex.ToString();

            foreach (ListItem _item in ddlHeads.Items)
            {
                _item.Selected = false;
            }
            foreach (ListItem _item in ddlHeads.Items)
            {
                if (_item.Text == _hd.Text)
                {
                    _item.Selected = true; break;
                }
            }

            txtGlcode.Text = _gl.Text;
            txtAllocation.Text = _all.Text;
            txtHeadBalance.Text = _hdbal.Text;

            txtExpenseDate.Text = _expDt.Text;
            txtBillAmount.Text = _amt.Text;
            txtBillNo.Text = _billNo.Text;
            txtDetail.Text = _detail.Text;

            //==============================================================
            int _rowIndex = 0;
            if (hfMainRowNo.Value.ToString().Length > 0)
            { _rowIndex = Convert.ToInt32(hfMainRowNo.Value); }

            _table = (DataTable)ViewState["DocEntryDataAll"];
            DataRow[] drsel = _table.Select("MainID='" + _rowIndex + "'");
            //===============================
            foreach (DataRow dr in drsel)
            {
                AddDocEntryData(dr["OnlyFileName"].ToString(), dr["File_Name"].ToString(), dr["NL_RowNo"].ToString(), dr["DocIDInDMS"].ToString());

                string _file = dr["File_Name"].ToString();
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
                HFUploadFileNo.Value = _dt000.Rows[_dt000.Rows.Count - 1]["NL_RowNo"].ToString();
            }

            Dal.Get_Single_DataByPassingQuery("Insert into NL_UPLOADEDDOCS_Buffer(User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
            "App_Serial_No, FileNoPrefix, MainRowNo) (select User_SapId,File_Name,File_Path,Create_By,FileSize,Only_File_Name," +
            "App_Serial_No, FileNoPrefix, MainRowNo from NL_UPLOADEDDOCS where App_Serial_No = '" + spnSrNo.InnerText + "' and MainRowNo = '" + _rowIndex + "')");
            //==============================================================

            //gvClaimDetail.Columns[14].Visible = false; for hiding edit and delete buttons from grid

            btnAdd.Visible = false;
            lnkUpdate.Visible = true;
            lnkCancel.Visible = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkDeleteRow_Click(object sender, EventArgs e)
    {
        try
        {
            string rownumber = (sender as LinkButton).CommandArgument;

            if (ViewState["ItemGrid"] != null)
            {
                using (DataTable dt = (DataTable)ViewState["ItemGrid"])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["RowNo"].ToString() == rownumber)
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
                        gvClaimDetail.DataSource = null;
                        gvClaimDetail.DataBind();
                    }
                    //SetPreviousData();
                }
            }

            int _rowIndex = Convert.ToInt32(rownumber);
            _table = (DataTable)ViewState["DocEntryDataAll"];
            DataRow[] drselD = _table.Select("MainID='" + _rowIndex + "'");
            foreach (DataRow dr in drselD)
            {
                string _file = dr["File_Name"].ToString();
                string fileName = Path.Combine(AttachFileName, _file);
                string Attachment_ID = Dal.Get_Single_DataByPassingQuery("Select Attachment_ID from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and File_Name = '" + _file + "' and MainRowNo = '" + _rowIndex + "'");

                string _Deletestr = Dal.Get_Single_DataByPassingQuery("Insert into DocumentDeletionTrack(ApplicationName, SapId_DeletedBy, DeletedOn, DocumentID, SerialNo, Filename, OnlyFileName, RowNo) Values('FPA', " + getSAPID(ReturnLoginName()) + ", '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.ToLongTimeString() + "', " + Attachment_ID + ",'" + spnSrNo.InnerText + "', '" + _file + "', '" + dr["OnlyFileName"].ToString() + "', " + _rowIndex + ")");

                string _str = Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and File_Name = '" + _file + "' and MainRowNo = '" + _rowIndex + "'");
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
            _table.AcceptChanges();

            //==============================================================
            _table = (DataTable)ViewState["DocEntryDataAll"];
            foreach (GridViewRow _row1 in gvClaimDetail.Rows)
            {
                //--------------
                GridView _grd = (GridView)_row1.FindControl("dgvEntryDocs");
                DataRow _row2 = _table.NewRow();
                Label lbl = (Label)_row1.FindControl("lblRowNo");
                _rowIndex = Convert.ToInt32(lbl.Text);

                DataRow[] drsel = _table.Select("MainID='" + _rowIndex + "'");
                //===============================
                DataTable _table1 = new DataTable();
                _table1.Columns.Add(new DataColumn("ID", typeof(string)));
                _table1.Columns.Add(new DataColumn("File_Name", typeof(string)));
                _table1.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));

                _table1.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));

                _table1.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
                _row2 = _table1.NewRow();
                foreach (DataRow dr in drsel)
                {
                    _row2 = _table1.NewRow();
                    _row2["ID"] = dr["ID"];
                    _row2["File_Name"] = dr["File_Name"];
                    _row2["OnlyFileName"] = dr["OnlyFileName"];
                    _row2["NL_RowNo"] = dr["NL_RowNo"];
                    _row2["DocIDInDMS"] = dr["DocIDInDMS"];
                    _table1.Rows.Add(_row2);
                }
                _grd.DataSource = _table1;
                _grd.DataBind();
            }
            //==============================================================

        }
        catch (Exception ex)
        { }
    }

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
            dt.Columns.Add("CYCLENO", typeof(int));
            dt.Columns.Add("BillNo", typeof(string));
            dt.Columns.Add("RowNo", typeof(int));
            //string FpaCyleId = Dal.GetFpaCyleId((getSAPID(ReturnLoginName())));
            string FpaCyleId = Dal.GetFpaCyleId(HFSAPID.Value);
            int i = 0;
            foreach (GridViewRow grow in gvClaimDetail.Rows)
            {
                DropDownList ddlhead = (DropDownList)grow.FindControl("ddlHeads");
                Label Glcode = (Label)grow.FindControl("txtGlcode");
                Label txtFPA_HEAD_ID = (Label)grow.FindControl("txtFPA_HEAD_ID");
                Label Allocation = (Label)grow.FindControl("txtAllocation");
                Label txtHeadBalance = (Label)grow.FindControl("txtHeadBalance");
                Label txtExpenseDate = (Label)grow.FindControl("txtExpenseDate1");
                Label txtBillAmount = (Label)grow.FindControl("txtBillAmount");
                Label txtDetail = (Label)grow.FindControl("txtDetail");
                Label txtBillNo = (Label)grow.FindControl("txtBillNo");
                Label lblRowNo = (Label)grow.FindControl("lblRowNo");
                DataRow dr = dt.NewRow();
                dr["FPA_CYCLE_ID"] = Convert.ToString(FpaCyleId);
                dr["FPA_HEAD_ID"] = Convert.ToString(txtFPA_HEAD_ID.Text);
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

                dr["BillNo"] = Convert.ToString(txtBillNo.Text);
                dr["RowNo"] = Convert.ToString(lblRowNo.Text);

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

    #region dgvEntryDocs
    protected void dgvEntryDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Open")
        {
            try
            {
                string filename = e.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblFileName = (Label)row.FindControl("lblfileName");
                Label lblDocIDInDMS = (Label)row.FindControl("lblDocIDInDMS");
                LinkButton hprlnk = (LinkButton)row.FindControl("lnkDoc");

                int _DocIDInDMS = 0;
                if (lblDocIDInDMS.Text.Length > 0) { _DocIDInDMS = Convert.ToInt32(lblDocIDInDMS.Text); }

                if (_DocIDInDMS > 0)
                {

                    StringBuilder _arr = new StringBuilder();

                    _arr.Append("{" +
                    "'status':'Download'," +
                      "'FolderName':'FPA'," +
                      "'FileName':'" + lblFileName.Text.Substring(0, Path.GetExtension(lblFileName.Text) == ".xlsx" || Path.GetExtension(lblFileName.Text) == ".jpeg" ? lblFileName.Text.Length - 5 : lblFileName.Text.Length - 4) + "'," +
                      "'docID' : " + _DocIDInDMS +
                    "}");

                    //string resp = UploadDocInAPI(_arr);

                    //string URL = @"https://vault.vecv.net/DMSUtility/webresources/dmsupload";
                    string URL = @"http://10.201.1.198:8080/DMSUtility/webresources/dmsupload";
                    HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(URL);
                    _request.Method = "PUT";
                    _request.ContentType = "application/json";
                    _request.ContentLength = _arr.Length;
                    using (Stream webStream = _request.GetRequestStream())
                    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                    {
                        requestWriter.Write(_arr);
                    }

                    try
                    {
                        WebResponse webResponse = _request.GetResponse();
                        using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(responseReader.ReadToEnd());
                            string _res = jsonData.document[0].txtuplode;

                            byte[] imageBytes = Convert.FromBase64String(_res);

                            string filePath = AttachFileName1 + lblFileName.Text;

                            File.WriteAllBytes(filePath, imageBytes);

                            Response.ContentType = "application/octet-stream";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + hprlnk.Text + "");
                            Response.TransmitFile(filePath);
                            Response.End();

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('" + ex.Message + "');", true);
            }
        }

        //if (e.CommandName == "Open")
        //{
        //    try
        //    {
        //        string filename = e.CommandArgument.ToString();
        //        GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        //        Label lblFileName = (Label)row.FindControl("lblfileName");
        //        LinkButton hprlnk = (LinkButton)row.FindControl("lnkDoc");
        //        if (hprlnk.Text.Length > 0)
        //        {
        //            Response.ContentType = "application/octet-stream";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + hprlnk.Text + "");
        //            Response.TransmitFile(AttachFileName1 + lblFileName.Text);
        //            Response.End();
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('" + "No file exists." + "');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('" + ex.Message + "');", true);
        //    }

        //}
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
            Label _OnlyfileName = (Label)_row.FindControl("lblOnly_File_Name");

            int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
            int _rowIndexP = Convert.ToInt32(rowNo.Text);

            foreach (DataRow dr in drsel)
            {
                _table.Rows.Remove(dr);
            }
            _table.AcceptChanges();
            ViewState["DocEntryData"] = _table;
            dgvEntryDocs.EditIndex = -1;
            dgvEntryDocs.DataSource = _table;
            dgvEntryDocs.DataBind();

            string Attachment_ID = Dal.Get_Single_DataByPassingQuery("Select Attachment_ID from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "' and File_Name = '" + _file.Text + "' and MainRowNo = '" + _rowIndex + "'and FileNoPrefix = " + _rowIndexP);

            string _Deletestr = Dal.Get_Single_DataByPassingQuery("Insert into DocumentDeletionTrack(ApplicationName, SapId_DeletedBy, DeletedOn, DocumentID, SerialNo, Filename, OnlyFileName, RowNo) Values('FPA', " + getSAPID(ReturnLoginName()) + ", '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.ToLongTimeString() + "', " + Attachment_ID + ",'" + spnSrNo.InnerText + "', '" + _file.Text + "', '" + _OnlyfileName.Text + "', " + _rowIndex + ")");

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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    void AddRowData(string HeadName, int HeadCode, string GLCode, decimal AlloAmt,
       decimal HeadBal, string ExpDate, decimal ClaimAmt, string Detail, string BillNo, int RowNo)
    {
        int k = 0;
        if (ViewState["ItemGrid"] == null)
        {
            _table = new DataTable();

            _table.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            _table.Columns.Add(new DataColumn("HeadName", typeof(string)));
            _table.Columns.Add(new DataColumn("FPA_HEAD_ID", typeof(string)));
            _table.Columns.Add(new DataColumn("GL_CODE", typeof(string)));
            _table.Columns.Add(new DataColumn("AllocateAmount", typeof(string)));
            _table.Columns.Add(new DataColumn("HEAD_BAL", typeof(string)));
            _table.Columns.Add(new DataColumn("EXPENSE_DATE", typeof(string)));
            _table.Columns.Add(new DataColumn("CLAIM_AMT", typeof(decimal)));
            _table.Columns.Add(new DataColumn("Details", typeof(string)));
            _table.Columns.Add(new DataColumn("BillNo", typeof(string)));
            _table.Columns.Add(new DataColumn("RowNo", typeof(string)));

            hfMainRowNo.Value = "1";
            k = Convert.ToInt32(hfMainRowNo.Value.ToString());
        }
        else
        {
            _table = ((DataTable)ViewState["ItemGrid"]);

            hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value.ToString())).ToString();
            k = Convert.ToInt32(hfMainRowNo.Value.ToString());
        }

        DataRow _row = _table.NewRow();

        _row["RowNumber"] = k;
        _row["HeadName"] = HeadName;
        _row["FPA_HEAD_ID"] = HeadCode;
        _row["GL_CODE"] = GLCode;
        _row["AllocateAmount"] = string.Format("{0:0}", AlloAmt);
        _row["HEAD_BAL"] = string.Format("{0:0}", HeadBal);
        _row["EXPENSE_DATE"] = ExpDate;
        _row["CLAIM_AMT"] = string.Format("{0:0}", ClaimAmt);
        _row["Details"] = Detail;
        _row["BillNo"] = BillNo;
        _row["RowNo"] = RowNo;

        _table.Rows.Add(_row);
        ViewState["ItemGrid"] = _table;

        gvClaimDetail.DataSource = _table;
        gvClaimDetail.DataBind();

        decimal totalValue = 0;
        foreach (GridViewRow grow in gvClaimDetail.Rows)
        {
            Label txtAmount = (Label)grow.FindControl("txtBillAmount");
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            totalValue = totalValue + Convert.ToDecimal(txtAmount.Text);
        }

        spnTotalFpaClaim.InnerText = Convert.ToString(totalValue);

        //===============================
        if (ViewState["DocEntryDataAll"] == null)
        {
            _table = new DataTable();
            _table.Columns.Add(new DataColumn("MainID", typeof(string)));
            _table.Columns.Add(new DataColumn("ID", typeof(string)));
            _table.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
            _table.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
        }
        else
        {
            _table = ((DataTable)ViewState["DocEntryDataAll"]);
        }
        DataRow _row2 = _table.NewRow();

        int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
        int k0 = 1;
        ////foreach (GridViewRow _row0 in dgvEntryDocs.Rows)
        ////{
        ////    Label _lbl0 = (Label)_row0.FindControl("lblFileName");
        ////    Label _lbl = (Label)_row0.FindControl("lblOnly_File_Name");
        ////    Label _lblRow = (Label)_row0.FindControl("lblRowNo");

        ////    _row2 = _table.NewRow();
        ////    _row2["MainID"] = _rowIndex;
        ////    _row2["ID"] = k0;
        ////    _row2["File_Name"] = _lbl0.Text;
        ////    _row2["OnlyFileName"] = _lbl.Text;
        ////    _row2["NL_RowNo"] = _lblRow.Text;
        ////    _table.Rows.Add(_row2);
        ////    k0 += 1;
        ////}

        ViewState["DocEntryDataAll"] = _table;
        //===============================

        _table = ((DataTable)ViewState["DocEntryData"]);
        if (_table != null)
        {
            _table.Rows.Clear();
            ////dgvEntryDocs.DataSource = null;
            ////dgvEntryDocs.DataBind();
        }

        //==============================================================
        _table = (DataTable)ViewState["DocEntryDataAll"];
        foreach (GridViewRow _row1 in gvClaimDetail.Rows)
        {
            //--------------
            GridView _grd = (GridView)_row1.FindControl("dgvEntryDocs");
            Label _lbl = (Label)_row1.FindControl("lblRowNo");

            DataRow _row3 = _table.NewRow();
            int _rowIndex0 = Convert.ToInt32(_lbl.Text);

            DataRow[] drsel = _table.Select("MainID='" + _rowIndex0 + "'");
            //===============================
            DataTable _table1 = new DataTable();
            _table1.Columns.Add(new DataColumn("ID", typeof(string)));
            _table1.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table1.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));

            _table1.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));

            _table1.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
            _row3 = _table1.NewRow();
            foreach (DataRow dr in drsel)
            {
                _row3 = _table1.NewRow();
                _row3["ID"] = dr["ID"];
                _row3["File_Name"] = dr["File_Name"];
                _row3["OnlyFileName"] = dr["OnlyFileName"];

                _row2["DocIDInDMS"] = dr["DocIDInDMS"];

                _row3["NL_RowNo"] = dr["NL_RowNo"];
                _table1.Rows.Add(_row3);
            }
            _grd.DataSource = _table1;
            _grd.DataBind();
        }
        //==============================================================
        hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value) + 1).ToString();

        HFUploadFileNo.Value = "0";

        ddlHeads.SelectedIndex = -1;
        txtGlcode.Text = "";
        txtAllocation.Text = "";
        txtHeadBalance.Text = "";
        txtExpenseDate.Text = "";
        txtBillAmount.Text = "";
        txtBillNo.Text = "";
        txtDetail.Text = "";

    }

    void BindHeaderDDL()
    {
        DataTable fpaheads = Dal.getFpaClaimHead(HFSAPID.Value.ToString());
        DropDownList ddlLine = ddlHeads; //(DropDownList)e.Row.FindControl("ddlHeads");
        ddlLine.Items.Clear();
        ddlLine.AppendDataBoundItems = true;
        ListItem li = new ListItem("--Select--", "0");
        ddlLine.Items.Add(li);
        ddlLine.DataSource = fpaheads;
        ddlLine.DataTextField = "fpa_head";
        ddlLine.DataValueField = "fpa_head_id";
        ddlLine.DataBind();
    }

    public void AddDocEntryData(string OnlyFileName, string FileName, string UploadedFileNo, string DocIDInDMS)
    {
        int k = dgvEntryDocs.Rows.Count + 1;

        if (ViewState["DocEntryData"] == null)
        {
            _table = new DataTable();

            _table.Columns.Add(new DataColumn("ID", typeof(string)));
            _table.Columns.Add(new DataColumn("Only_File_Name", typeof(string)));
            _table.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));
            _table.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
            _table.Columns.Add(new DataColumn("pk_id", typeof(string)));

            DataRow _row = _table.NewRow();

            _row["ID"] = k;
            _row["Only_File_Name"] = OnlyFileName;
            _row["File_Name"] = FileName;
            _row["DocIDInDMS"] = DocIDInDMS;
            _row["NL_RowNo"] = UploadedFileNo;
            _row["pk_id"] = k;

            _table.Rows.Add(_row);
            ViewState["DocEntryData"] = _table;
        }
        else
        {
            _table = ((DataTable)ViewState["DocEntryData"]);
            DataRow _row = _table.NewRow();

            _row["ID"] = k;
            _row["Only_File_Name"] = OnlyFileName;
            _row["File_Name"] = FileName;
            _row["DocIDInDMS"] = DocIDInDMS;
            _row["NL_RowNo"] = UploadedFileNo;
            _row["pk_id"] = k;

            _table.Rows.Add(_row);
            ViewState["DocEntryData"] = _table;
        }
        dgvEntryDocs.DataSource = _table;
        dgvEntryDocs.DataBind();
    }
    private string ReduceImageSize(double TotalFileLen, Stream sourcePath)
    {
        double scaleFactor = 0;
        //take image size in byte
        scaleFactor = TotalFileLen < 1048576 ? 1.35 : (TotalFileLen < 2097152 && TotalFileLen > 1048576) ? 0.90 : (TotalFileLen < 5242880 && TotalFileLen > 2097152) ? 0.60 : 0.40;
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            string targetPath = string.Empty;
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            targetPath = Path.Combine(AttachFileName.Replace(@"\\", @"\"), FileName.Replace(" ", "_"));
            thumbnailImg.Save(@"" + targetPath + "", ImageFormat.Jpeg);
            fileSize = Convert.ToInt32(new FileInfo(targetPath).Length);
            thumbnailImg.Dispose();

            return "Saved";

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
                    Stream strm = FindFile.PostedFile.InputStream;

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

                    DataTable _dt__ = (DataTable)ViewState["DocEntryDataAll"];
                    if (_dt__ != null && _dt__.Rows.Count > 0)
                    {
                        foreach (DataRow _row in _dt__.Rows)
                        {
                            if (hfUpOnlyFileNameN.Value.ToString().Replace(" ", "_") == _row["OnlyFileName"].ToString())
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

                    var allowedExtention = new[] { ".jpg", ".jpeg", ".png" };
                    if (TotalFileLen < maxfileCompressSize)
                    {
                        if (TotalFileLen > minfileCompressSize && allowedExtention.Contains(FileExt.ToLower()))
                        {
                            string reducedSize = ReduceImageSize(TotalFileLen, strm);

                            if ((fileSize + ttlFileSize) <= ttlmaxfileSize)
                            {
                                if (reducedSize == "Saved")
                                {
                                    if (fileSize < maxfileSize)
                                    {
                                        string insert = Dal.InsertPhoto(HFSAPID.Value, FileName.Replace(" ", "_"), "", HFSAPID.Value, Convert.ToString(fileSize), OnlyFileName.Replace(" ", "_"), spnSrNo.InnerText, _fileNo.ToString(), (_mainRowNo).ToString());
                                        return FileNameUpload;
                                    }
                                    else
                                    {
                                        string fileName = Path.Combine(AttachFileName, FileName.Replace(" ", "_"));
                                        if (File.Exists(fileName))
                                        {
                                            File.Delete(fileName);
                                            int sizMb = (maxfileSize / 1024) / 1024;
                                            ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('File size is too large, please upload smaller file. Recommended file size is upto " + Convert.ToString(sizMb) + " MB.');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int sizMb = (ttlmaxfileSize / 1024) / 1024;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Maximum file size: " + Convert.ToString(sizMb) + " MB allowed.');", true);
                            }
                        }

                        else if ((TotalFileLen + ttlFileSize) <= ttlmaxfileSize)
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('file size is very large and smaller file size should be uploaded.');", true);
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

    void AddRow()
    {
        int k = 0;
        if (ViewState["ItemGrid"] == null)
        {
            _table = new DataTable();

            _table.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            _table.Columns.Add(new DataColumn("HeadName", typeof(string)));
            _table.Columns.Add(new DataColumn("FPA_HEAD_ID", typeof(string)));
            _table.Columns.Add(new DataColumn("GL_CODE", typeof(string)));
            _table.Columns.Add(new DataColumn("AllocateAmount", typeof(string)));
            _table.Columns.Add(new DataColumn("HEAD_BAL", typeof(string)));
            _table.Columns.Add(new DataColumn("EXPENSE_DATE", typeof(string)));
            _table.Columns.Add(new DataColumn("CLAIM_AMT", typeof(decimal)));
            _table.Columns.Add(new DataColumn("Details", typeof(string)));
            _table.Columns.Add(new DataColumn("BillNo", typeof(string)));
            _table.Columns.Add(new DataColumn("RowNo", typeof(string)));

            DataRow _row = _table.NewRow();

            hfMainRowNo.Value = "1";
            k = Convert.ToInt32(hfMainRowNo.Value.ToString());

            _row["RowNumber"] = k;
            _row["HeadName"] = ddlHeads.SelectedItem.Text;
            _row["FPA_HEAD_ID"] = ddlHeads.SelectedValue.ToString();
            _row["GL_CODE"] = txtGlcode.Text;
            _row["AllocateAmount"] = txtAllocation.Text;
            _row["HEAD_BAL"] = txtHeadBalance.Text;
            _row["EXPENSE_DATE"] = txtExpenseDate.Text;
            _row["CLAIM_AMT"] = txtBillAmount.Text == "" ? 0 : Convert.ToDecimal(txtBillAmount.Text);
            _row["Details"] = txtDetail.Text;
            _row["BillNo"] = txtBillNo.Text;
            _row["RowNo"] = k;

            _table.Rows.Add(_row);
            ViewState["ItemGrid"] = _table;
        }
        else
        {
            _table = ((DataTable)ViewState["ItemGrid"]);

            foreach (DataRow _dtRow in _table.Rows)
            {
                if (_dtRow["HeadName"].ToString() == ddlHeads.SelectedItem.ToString()
                    && _dtRow["CLAIM_AMT"].ToString() == txtBillAmount.Text
                    && _dtRow["EXPENSE_DATE"].ToString() == txtExpenseDate.Text
                    && _dtRow["BillNo"].ToString().ToLower() == txtBillNo.Text.ToLower())
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "UniquData", "alert('Claim for " + ddlHeads.SelectedItem.Text + " for Rs. " + txtBillAmount.Text + " with bill no : " + txtBillNo.Text + " dated " + txtExpenseDate.Text + " already exists in current claim !');", true);
                    return;
                }
            }

            DataRow _row = _table.NewRow();

            hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value.ToString())).ToString();
            k = Convert.ToInt32(hfMainRowNo.Value.ToString());

            _row["RowNumber"] = k;
            _row["HeadName"] = ddlHeads.SelectedItem.Text;
            _row["FPA_HEAD_ID"] = ddlHeads.SelectedValue.ToString();
            _row["GL_CODE"] = txtGlcode.Text;
            _row["AllocateAmount"] = txtAllocation.Text;
            _row["HEAD_BAL"] = txtHeadBalance.Text;
            _row["EXPENSE_DATE"] = txtExpenseDate.Text;
            _row["CLAIM_AMT"] = txtBillAmount.Text;
            _row["Details"] = txtDetail.Text;
            _row["BillNo"] = txtBillNo.Text;
            _row["RowNo"] = k;

            _table.Rows.Add(_row);
            ViewState["ItemGrid"] = _table;
        }
        gvClaimDetail.DataSource = _table;
        gvClaimDetail.DataBind();

        decimal totalValue = 0;
        foreach (GridViewRow grow in gvClaimDetail.Rows)
        {
            Label txtAmount = (Label)grow.FindControl("txtBillAmount");
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            totalValue = totalValue + Convert.ToDecimal(txtAmount.Text);
        }

        spnTotalFpaClaim.InnerText = Convert.ToString(totalValue);

        //===============================
        if (ViewState["DocEntryDataAll"] == null)
        {
            _table = new DataTable();
            _table.Columns.Add(new DataColumn("MainID", typeof(string)));
            _table.Columns.Add(new DataColumn("ID", typeof(string)));
            _table.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
            _table.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));
            _table.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
        }
        else
        {
            _table = ((DataTable)ViewState["DocEntryDataAll"]);
        }
        DataRow _row2 = _table.NewRow();

        int _rowIndex = Convert.ToInt32(hfMainRowNo.Value);
        int k0 = 1;
        foreach (GridViewRow _row0 in dgvEntryDocs.Rows)
        {
            Label _lbl0 = (Label)_row0.FindControl("lblFileName");
            Label _lbl = (Label)_row0.FindControl("lblOnly_File_Name");
            Label _lblRow = (Label)_row0.FindControl("lblRowNo");
            Label _lblDocIDInDMS = (Label)_row0.FindControl("lblDocIDInDMS");
            _row2 = _table.NewRow();
            _row2["MainID"] = _rowIndex;
            _row2["ID"] = k0;
            _row2["File_Name"] = _lbl0.Text;
            _row2["OnlyFileName"] = Path.GetFileName(_lbl.Text);
            _row2["NL_RowNo"] = _lblRow.Text;
            _row2["DocIDInDMS"] = _lblDocIDInDMS.Text;
            _table.Rows.Add(_row2);
            k0 += 1;
        }

        ViewState["DocEntryDataAll"] = _table;
        //===============================

        _table = ((DataTable)ViewState["DocEntryData"]);
        if (_table != null)
        {
            _table.Rows.Clear();
            dgvEntryDocs.DataSource = null;
            dgvEntryDocs.DataBind();
        }

        //==============================================================
        _table = (DataTable)ViewState["DocEntryDataAll"];
        foreach (GridViewRow _row1 in gvClaimDetail.Rows)
        {
            //--------------
            GridView _grd = (GridView)_row1.FindControl("dgvEntryDocs");
            Label _lbl = (Label)_row1.FindControl("lblRowNo");

            DataRow _row3 = _table.NewRow();
            int _rowIndex0 = Convert.ToInt32(_lbl.Text);

            DataRow[] drsel = _table.Select("MainID='" + _rowIndex0 + "'");
            //===============================
            DataTable _table1 = new DataTable();
            _table1.Columns.Add(new DataColumn("ID", typeof(string)));
            _table1.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table1.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));

            _table1.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));

            _table1.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
            _row3 = _table1.NewRow();
            foreach (DataRow dr in drsel)
            {
                _row3 = _table1.NewRow();
                _row3["ID"] = dr["ID"];
                _row3["File_Name"] = dr["File_Name"];
                _row3["OnlyFileName"] = dr["OnlyFileName"];
                _row2["DocIDInDMS"] = dr["DocIDInDMS"];
                _row3["NL_RowNo"] = dr["NL_RowNo"];
                _table1.Rows.Add(_row3);
            }
            _grd.DataSource = _table1;
            _grd.DataBind();
        }
        //==============================================================
        hfMainRowNo.Value = (Convert.ToInt32(hfMainRowNo.Value) + 1).ToString();

        HFUploadFileNo.Value = "0";

        ddlHeads.SelectedIndex = -1;
        txtGlcode.Text = "";
        txtAllocation.Text = "";
        txtHeadBalance.Text = "";
        txtExpenseDate.Text = "";
        txtBillAmount.Text = "";
        txtBillNo.Text = "";
        txtDetail.Text = "";

    }

    void SetDocData()
    {
        //=============================================================================================
        _table = new DataTable();
        _table.Columns.Add(new DataColumn("MainID", typeof(string)));
        _table.Columns.Add(new DataColumn("ID", typeof(string)));
        _table.Columns.Add(new DataColumn("File_Name", typeof(string)));
        _table.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
        _table.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));
        _table.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
        foreach (GridViewRow _row in gvClaimDetail.Rows)
        {
            Label lbl = (Label)_row.FindControl("lblRowNo");
            int _rowIndex = 0;
            if (lbl.Text.Length > 0)
            { _rowIndex = Convert.ToInt32(lbl.Text); }
            DataTable _dt = Dal.GetPhotoDetail(spnSrNo.InnerText.ToString(), (_rowIndex));
            foreach (DataRow _row1 in _dt.Rows)
            {
                DataRow _row2 = _table.NewRow();

                _row2 = _table.NewRow();
                _row2["MainID"] = Convert.ToInt32(_row1["NL_RowNo"]);
                _row2["ID"] = _row1["FileNoPrefix"];
                _row2["File_Name"] = _row1["File_Name"];
                _row2["OnlyFileName"] = _row1["Only_File_Name"];
                _row2["DocIDInDMS"] = _row1["DocIDInDMS"];
                _row2["NL_RowNo"] = _row1["FileNoPrefix"];
                _table.Rows.Add(_row2);
            }
        }
        ViewState["DocEntryDataAll"] = _table;
        //=============================================================================================
        _table = (DataTable)ViewState["DocEntryDataAll"];
        foreach (GridViewRow _row1 in gvClaimDetail.Rows)
        {
            Label lbl = (Label)_row1.FindControl("lblRowNo");

            int _rowIndex = 0;
            if (lbl.Text.Length > 0)
            { _rowIndex = Convert.ToInt32(lbl.Text); }

            //--------------
            GridView _grd = (GridView)_row1.FindControl("dgvEntryDocs");
            DataRow _row2 = _table.NewRow();

            DataRow[] drsel = _table.Select("MainID='" + _rowIndex + "'");
            //===============================
            DataTable _table1 = new DataTable();
            _table1.Columns.Add(new DataColumn("ID", typeof(string)));
            _table1.Columns.Add(new DataColumn("File_Name", typeof(string)));
            _table1.Columns.Add(new DataColumn("OnlyFileName", typeof(string)));
            _table1.Columns.Add(new DataColumn("DocIDInDMS", typeof(string)));
            _table1.Columns.Add(new DataColumn("NL_RowNo", typeof(string)));
            _row2 = _table1.NewRow();
            foreach (DataRow dr in drsel)
            {
                _row2 = _table1.NewRow();
                _row2["ID"] = dr["ID"];
                _row2["File_Name"] = dr["File_Name"];
                _row2["OnlyFileName"] = dr["OnlyFileName"];
                _row2["DocIDInDMS"] = dr["DocIDInDMS"];
                _row2["NL_RowNo"] = dr["NL_RowNo"];
                _table1.Rows.Add(_row2);
            }
            _grd.DataSource = _table1;
            _grd.DataBind();
        }
        //=============================================================================================
        _table = (DataTable)ViewState["ItemGrid"];

        hfMainRowNo.Value = "0";
        if (_table.Rows.Count > 0)
        {
            if (_table.Rows[_table.Rows.Count - 1]["RowNo"].ToString().Length > 0)
            { hfMainRowNo.Value = (Convert.ToInt32(_table.Rows[_table.Rows.Count - 1]["RowNo"].ToString()) + 1).ToString(); }
        }
        //=============================================================================================
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Int64 sumAll = 0;
        try
        {
            if (FileUpload1.HasFile)
            {
                string FileExt = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
                if (arrValidExtention.Contains(FileExt.ToLower()))
                {
                    bool IsExist = false;
                    string uplFilename = Path.GetFileName(FileUpload1.FileName).Split('.')[0].ToString();
                    Stream strm = FileUpload1.PostedFile.InputStream;

                    int _fileNo = gvOnholdDocs.Rows.Count == 0 ? 1 : gvOnholdDocs.Rows.Count + 1;

                    string PerFix = spnSrNo.InnerText.ToString() + "_999_" + _fileNo.ToString();
                    HFUploadFileNo.Value = _fileNo.ToString();

                    FileName = Path.GetFileName(FileUpload1.FileName).Replace(uplFilename, PerFix).ToString();
                    FileNameUpload = FileName;
                    string OnlyFileName = Path.GetFileName(FileUpload1.FileName);
                    hfUpOnlyFileNameN.Value = OnlyFileName;
                    FilePath = Convert.ToString(Path.GetFileName(FileUpload1.FileName));
                    FilePathUpload = FilePath;
                    Int64 TotalFileLen = Convert.ToInt64(FileUpload1.PostedFile.ContentLength);

                    DataTable _dt__ = new DataTable();
                    _dt__.Columns.Add("OnlyFileName");

                    foreach (GridViewRow _row1 in gvOnholdDocs.Rows)
                    {
                        DataRow dr = _dt__.NewRow();
                        LinkButton lblFileName = (LinkButton)_row1.FindControl("lnkDoc");
                        dr["OnlyFileName"] = lblFileName.Text;
                        _dt__.Rows.Add(dr);
                    }

                    if (_dt__ != null && _dt__.Rows.Count > 0)
                    {
                        foreach (DataRow _row in _dt__.Rows)
                        {
                            if (hfUpOnlyFileNameN.Value.ToString().Replace(" ", "_") == _row["OnlyFileName"].ToString())
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqudoc", "alert('" + hfUpOnlyFileNameN.Value.ToString() + " or with same filename already exist, so cannot be upload again. Either rename it or remove previous file');", true);
                                IsExist = true;
                            }
                        }
                    }
                    if (IsExist == false)
                    {
                        string ttlFileSizeS = Dal.Get_Single_DataByPassingQuery("select sum(filesize) from NL_UPLOADEDDOCS where App_Serial_No='" + spnSrNo.InnerText + "'");
                        int ttlFileSize = 0;
                        if (ttlFileSizeS.Length > 0)
                        { ttlFileSize = Convert.ToInt32(ttlFileSizeS); }
                        ttlFileSizeS = "";

                        var allowedExtention = new[] { ".jpg", ".jpeg", ".png" };
                        if (TotalFileLen < maxfileCompressSize)
                        {
                            if (TotalFileLen > minfileCompressSize && allowedExtention.Contains(FileExt.ToLower()))
                            {
                                string reducedSize = ReduceImageSize(TotalFileLen, strm);

                                if ((fileSize + ttlFileSize) <= ttlmaxfileSize)
                                {
                                    if (reducedSize == "Saved")
                                    {
                                        if (fileSize < maxfileSize)
                                        {
                                            string insert = Dal.InsertPhoto(HFSAPID.Value, FileName.Replace(" ", "_"), "", HFSAPID.Value, Convert.ToString(fileSize), OnlyFileName.Replace(" ", "_"), spnSrNo.InnerText, _fileNo.ToString(), "999");
                                        }
                                        else
                                        {
                                            string fileName = Path.Combine(AttachFileName, FileName.Replace(" ", "_"));
                                            if (File.Exists(fileName))
                                            {
                                                File.Delete(fileName);
                                                int sizMb = (maxfileSize / 1024) / 1024;
                                                ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('File size is too large, please upload smaller file. Recommended file size is upto " + Convert.ToString(sizMb) + " MB.');", true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int sizMb = (ttlmaxfileSize / 1024) / 1024;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Maximum file size: " + Convert.ToString(sizMb) + " MB allowed.');", true);
                                }
                            }
                            else if ((TotalFileLen + ttlFileSize) <= ttlmaxfileSize)
                            {
                                sumAll = TotalFileLen + sumAll;

                                if (sumAll <= maxfileSize)
                                {
                                    string _resp = SaveFile(FileUpload1);
                                    if (_resp == "Saved")
                                    {
                                        string insert = Dal.InsertPhoto(HFSAPID.Value, FileName.Replace(" ", "_"), "", HFSAPID.Value, Convert.ToString(TotalFileLen), OnlyFileName.Replace(" ", "_"), spnSrNo.InnerText, _fileNo.ToString(), "999");

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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('file size is very large and smaller file size should be uploaded.');", true);

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Uniqu", "alert('Only .jpg, .jpeg, .png, .pdf and Excel file are allowed.');", true);
                }
                FillOnHoldDocsGrid();
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void FillOnHoldDocsGrid()
    {
        try
        {
            string Srno = Convert.ToString(spnSrNo.InnerText);
            DataTable dt = Dal.getOnHoldDocs(HFSAPID.Value, Srno);
            if (dt.Rows.Count > 0)
            {
                gvOnholdDocs.DataSource = dt;
                gvOnholdDocs.DataBind();
            }
            else
            {
                gvOnholdDocs.DataSource = null;
                gvOnholdDocs.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOnholdDocs_RowCommand(object sender, GridViewCommandEventArgs e)
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

    protected void lnkOnHoldDeleteRow_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton _but = (LinkButton)sender;
            GridViewRow _row = (GridViewRow)_but.NamingContainer;

            Label id = (Label)_row.FindControl("lblID");
            string selID = Convert.ToString(id.Text);
            Label lblFileName = (Label)_row.FindControl("lblFileName");
            string FName = Convert.ToString(lblFileName.Text);

            string _str = Dal.Get_Single_DataByPassingQuery("delete from NL_UPLOADEDDOCS where Attachment_id=" + selID);

            string fileName = Path.Combine(AttachFileName, FName);
            if (!string.IsNullOrEmpty(fileName))
            {
                FileInfo TheFile = new FileInfo(fileName);
                if (TheFile.Exists)
                {
                    File.Delete(fileName);
                }
            }
            string serialNumber = DoDecrypt(Request.QueryString["RID"]);
            string OnHoldDocs = Dal.UpdateFileNoPrefix_OnHoldDocs(serialNumber);
            FillOnHoldDocsGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool IsDocsUploadedInDMS()
    {
        Boolean _ret = true;
        StringBuilder _arr = new StringBuilder();

        string _bu = "", _ecode = "";
        string _ename = Dal.Get_Single_DataByPassingQuery("select dbo.Find_ENAME_ByEcode('" + spnEmpCode.InnerText + "')");

        _bu = spnBU.InnerText;
        _ecode = spnEmpCode.InnerText;
        try
        {
            foreach (GridViewRow _row in gvClaimDetail.Rows)
            {
                Label lbl = (Label)_row.FindControl("lblRowNo");
                int _rowIndex = 0;
                if (lbl.Text.Length > 0)
                { _rowIndex = Convert.ToInt32(lbl.Text); }
                DataTable _dt = Dal.GetPhotoDetail(spnSrNo.InnerText.ToString(), (_rowIndex));

                foreach (DataRow _row1 in _dt.Rows)
                {
                    string fileName = Path.Combine(AttachFileName, _row1["File_Name"].ToString());

                    byte[] AsBytes = File.ReadAllBytes(@fileName);
                    String AsBase64String = Convert.ToBase64String(AsBytes);

                    _arr = new StringBuilder();

                    _arr.Append("{" +
                    "'status':'Upload'," +
                      "'FolderName':'FPA'," +
                      "'document': [");

                    _arr.Append("{" +

                    "'Parameter': 'BU,ECode,EName,SrNo,RowNo,DocNo'," +

                     "'ParameterValue':'" + _bu + "," + _ecode + "," + _ename + "," + spnSrNo.InnerText.ToString() + "," +
                        _row1["NL_RowNo"].ToString() + "," + _row1["FileNoPrefix"].ToString() + "'," +
                    "'FileName': '" + _row1["File_Name"] + "'," +

                    "'txtuplode': '" + Convert.ToBase64String(AsBytes) + "'},");

                    _arr.Append("]" +
                    "}");

                    string resp = UploadDocInAPI(_arr);

                    var details = JObject.Parse(resp);

                    string _status = details["status"].ToString();
                    string _docID = details["docID"].ToString();

                    if (_status != "0")
                    { _ret = false; }
                    else
                    {
                        string _str = "update NL_UPLOADEDDOCS set DocIDInDMS = " + _docID + " where App_Serial_No = '" + spnSrNo.InnerText +
                            "' and MainRowNo = '" + _row1["NL_RowNo"].ToString() + "' and FileNoPrefix = '" + _row1["FileNoPrefix"].ToString() + "'";
                        Dal.Get_Single_DataByPassingQuery(_str);
                    }
                }
            }


            DataTable _dtOnHoldDocs = Dal.GetPhotoDetail(spnSrNo.InnerText.ToString(), 999);

            foreach (DataRow _row1 in _dtOnHoldDocs.Rows)
            {
                string fileName = Path.Combine(AttachFileName, _row1["File_Name"].ToString());

                byte[] AsBytes = File.ReadAllBytes(@fileName);
                String AsBase64String = Convert.ToBase64String(AsBytes);

                _arr = new StringBuilder();

                _arr.Append("{" +
                "'status':'Upload'," +
                  "'FolderName':'FPA'," +
                  "'document': [");

                _arr.Append("{" +

                "'Parameter': 'BU,ECode,EName,SrNo,RowNo,DocNo'," +

                 "'ParameterValue':'" + _bu + "," + _ecode + "," + _ename + "," + spnSrNo.InnerText.ToString() + "," +
                    _row1["NL_RowNo"].ToString() + "," + _row1["FileNoPrefix"].ToString() + "'," +
                "'FileName': '" + _row1["File_Name"] + "'," +

                "'txtuplode': '" + Convert.ToBase64String(AsBytes) + "'},");

                _arr.Append("]" +
                "}");

                string resp = UploadDocInAPI(_arr);

                var details = JObject.Parse(resp);

                string _status = details["status"].ToString();
                string _docID = details["docID"].ToString();

                if (_status != "0")
                { _ret = false; }
                else
                {
                    string _str = "update NL_UPLOADEDDOCS set DocIDInDMS = " + _docID + " where App_Serial_No = '" + spnSrNo.InnerText +
                        "' and MainRowNo = '" + _row1["NL_RowNo"].ToString() + "' and FileNoPrefix = '" + _row1["FileNoPrefix"].ToString() + "'";
                    Dal.Get_Single_DataByPassingQuery(_str);
                }
            }
            //}            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('" + ex.Message + "');", true);
        }
        return _ret;
    }

    private string UploadDocInAPI(StringBuilder _arr)
    {

        string URL = @"http://10.201.1.198:8080/DMSUtility/webresources/dmsupload";
        HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(URL);
        _request.Method = "PUT";
        _request.ContentType = "application/json";
        _request.ContentLength = _arr.Length;
        _request.UseDefaultCredentials = true;
        using (Stream webStream = _request.GetRequestStream())
        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
        {
            requestWriter.Write(_arr);
        }

        try
        {
            WebResponse webResponse = _request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
            using (StreamReader responseReader = new StreamReader(webStream))
            {
                //StringBuilder resp = (StringBuilder)responseReader.ReadToEnd();
                string resp = responseReader.ReadToEnd();

                return resp;
            }
        }
        catch (Exception e)
        {
            //Console.Out.WriteLine("-----------------");
            //Console.Out.WriteLine(e.Message);

            throw e;
        }
    }

    #endregion

}

