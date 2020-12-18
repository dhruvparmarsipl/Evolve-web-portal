using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml.XPath;
using Microsoft.SharePoint;
using System.Globalization;
using System.Threading;

public partial class Mediclaim : System.Web.UI.Page
{
    #region PAGE EVENTS
    //Utility.Service Util = new Utility.Service();
    string ErrorMessage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPlans();
            InitializeDependantsGrid();
            CommonFunction oclsCF = new CommonFunction();
            hdnCurrentLoginSapID.Value = oclsCF.GetSapIdByLoginName(ReturnLoginName());
            if (Request.QueryString.HasKeys())
            {
                //Serial number resist in hidden field, label
                //decrypt query string before use
                hdnSERIALNO.Value = DoDecrypt(Request.QueryString["onrs"].ToString());
                lblSerial.Text = hdnSERIALNO.Value;
                MediclaimBySerialNo(hdnSERIALNO.Value);
                BindLogGrid(hdnSERIALNO.Value);
                GetPrevSerialNo(hdnCurrentLoginSapID.Value);
            }
            else
            {
                int month = DateTime.Now.Month;
                  MediMain oclsMM = new MediMain();
                 int MediYear = oclsMM.GetMediYear(hdnCurrentLoginSapID.Value);
				 BindHeader(hdnCurrentLoginSapID.Value);
                 string StaffExec1 = oclsMM.Get_Multy_DataByPassingQuery(hfBu.Value, hdnDesID.Value);
                if (month < 11)
                {
                    int year = DateTime.Now.Year;
                    CheckAnyMediclaim(MediYear);
                    int count =  CheckPlanForThisYear(year-1);
                    if (count <= 0)
                    {
                        ShowMessageAndClose("Mediclaim Premium is not uploaded for this year. Please contact mediclaim admin");
                    }
					else if (StaffExec1 == "Object reference not set to an instance of an object.")
                    {
                        ShowMessageAndClose("Mediclaim Premium is not uploaded for this year. Please contact mediclaim admin");
                    }
                    else
                    {
                        BindHeader(hdnCurrentLoginSapID.Value);
                        lblSerial.Text = GetNewSRNo("MED");
                        hdnSERIALNO.Value = lblSerial.Text;
                        GetPrevSerialNo(hdnCurrentLoginSapID.Value);
                        lblStatus.Text = "Draft";
                    }
                }
                else
                {
                    int year = DateTime.Now.Year;
                    CheckAnyMediclaim(MediYear);
                    int count = CheckPlanForThisYear(year);
                    if (count <= 0)
                    {
                        ShowMessageAndClose("Mediclaim Premium is not uploaded for this year. Please contact mediclaim admin");
                    }
					else if (StaffExec1 == "Object reference not set to an instance of an object.")
                    {
                        ShowMessageAndClose("Mediclaim Premium is not uploaded for this year. Please contact mediclaim admin");
                    }
                    else
                    {
                        BindHeader(hdnCurrentLoginSapID.Value);
                        lblSerial.Text = GetNewSRNo("MED");
                        hdnSERIALNO.Value = lblSerial.Text;
                        GetPrevSerialNo(hdnCurrentLoginSapID.Value);
                        lblStatus.Text = "Draft";
                    }
                }
            }
        }
    }
    private void GetPrevSerialNo(string sapid)
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = oclsMM.GetMasterData(sapid);
        if (dt.Rows.Count > 0)
        {
            txtMobile.Text = dt.Rows[0]["MOBILE_NO"].ToString();
            txtCareOf.Text = dt.Rows[0]["CARE_OF"].ToString();
            txtCity.Text = dt.Rows[0]["CITY"].ToString();
            txtPostalCode.Text = dt.Rows[0]["POSTALCODE"].ToString();
            txtStreet.Text = dt.Rows[0]["STREET_HOUSENO"].ToString();
            txt2ndlineAdd.Text = dt.Rows[0]["ADDRESS2"].ToString();
            ddlUserBloodGroup.Items.FindByText(dt.Rows[0]["BloodGroup"].ToString()).Selected = true;


            txtBankName.Text = dt.Rows[0]["BName"].ToString();
            txtIfscCode.Text = dt.Rows[0]["IfscCode"].ToString();
            txtAcNo.Text = dt.Rows[0]["AcNumber"].ToString();
            txtAcHName.Text = dt.Rows[0]["AcCorrectName"].ToString();
        }
    }

    private int CheckPlanForThisYear(int year)
    {
        MediMain oclsMM = new MediMain();
        CommonFunction oclsCF = new CommonFunction();
        int Count = 0;
        Count = oclsMM.CheckPremiumExist(year, "MEDI_spCheckExitency");
        return Count;
        
    }
    protected void ddlSumInsured_SelectedIndexChanged(object sender, EventArgs e)
    { 
        MediMain oclsMM = new MediMain();

        if (ViewState["ForPremium"] != null)
        {
            DataTable dt = ViewState["ForPremium"] as DataTable;
            if (ddlSumInsured.SelectedValue == dt.Rows[0]["Insured_Amount"].ToString().Trim())
            {
                txtCalculatedPremium.Text = dt.Rows[0]["Premium"].ToString();
                hdnActualPrem.Value = dt.Rows[0]["ActPremium"].ToString();
            }
            else if (ddlSumInsured.SelectedValue == dt.Rows[1]["Insured_Amount"].ToString().Trim())
            {
                txtCalculatedPremium.Text = dt.Rows[1]["Premium"].ToString();
                hdnActualPrem.Value = dt.Rows[1]["ActPremium"].ToString();
            }
        }
        //if (ViewState["ForPremium"] != null)
        //{
        //    DataTable dt = ViewState["ForPremium"] as DataTable;
        //    if (ddlSumInsured.SelectedValue == dt.Rows[0]["Insured_Amount"].ToString().Trim())
        //    {
        //        string Premamt = oclsMM.GetPremiumAmount(hdnCurrentLoginSapID.Value, Convert.ToInt32(dt.Rows[0]["Premium"].ToString()));
        //        txtCalculatedPremium.Text = Premamt;// dt.Rows[0]["Premium"].ToString();
        //        hdnActualPrem.Value = dt.Rows[0]["ActPremium"].ToString();
        //    }
        //    else if (ddlSumInsured.SelectedValue == dt.Rows[1]["Insured_Amount"].ToString().Trim())
        //    {
        //        string Premamt = oclsMM.GetPremiumAmount(hdnCurrentLoginSapID.Value, Convert.ToInt32(dt.Rows[1]["Premium"].ToString()));
        //        txtCalculatedPremium.Text = Premamt;
        //        hdnActualPrem.Value = dt.Rows[1]["ActPremium"].ToString();
        //    }
        //}
    }

    protected void ddlRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        MediMain oclsMM = new MediMain();
        DropDownList ddlstRelation = (sender as DropDownList);
        GridViewRow grdvRow = (GridViewRow)ddlstRelation.NamingContainer;
        DropDownList ddlRelation = (DropDownList)grdvRow.FindControl("ddlRelation");
        TextBox dob = (TextBox)grdvRow.FindControl("txtDOB");
        TextBox TextDependantName = (TextBox)grdvRow.FindControl("txtNameOfDependant");
        DropDownList DDLBloodGroup = (DropDownList)grdvRow.FindControl("ddlBloodGroup");
        TextBox TextRemarks = (TextBox)grdvRow.FindControl("txtRemarks");
        if (ddlRelation.SelectedValue == "Father")
        {
            //  hdnFatherFlag.Value = "0";
            if (hdnFatherFlag.Value == "0")
            {
                hdnFatherFlag.Value = "1";
            }
            else
            {
                ddlRelation.ClearSelection();
                ShowMessage("You can not select father twice.");
            }
        }
        else
        {
            hdnFatherFlag.Value = "0";
        }
        DataTable dt = oclsMM.GetDataForRelation(ddlRelation.SelectedValue, hdnCurrentLoginSapID.Value);
        if (dt.Rows.Count > 0)
        {
            int Flag = 0;
            foreach (GridViewRow grow in grdvDependantsDetail.Rows)
            {
                DropDownList DDLRelation = (DropDownList)grow.Cells[4].FindControl("ddlRelation");
                if (DDLRelation.SelectedValue == ddlRelation.SelectedValue)
                {
                    Flag++;
                }
            }

            if (dt.Rows.Count > 1)
            { 
                if (Flag <= 3)
                {
                    if (dt.Rows.Count == Flag)
                    {
                        ViewState["dt"] = dt;
                        ViewState["grdRowIndex"] = grdvRow.RowIndex.ToString();
                        rdbSdName.DataSource = dt;
                        rdbSdName.DataTextField = "NAME";
                        rdbSdName.DataValueField = "RowNumber";
                        rdbSdName.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "KeyFun", "Popup.show('divBackground')", true);
                    }
                    else if (Flag == 1 && dt.Rows.Count > 1)
                    {
                        ViewState["dt"] = dt;
                        ViewState["grdRowIndex"] = grdvRow.RowIndex.ToString();
                        rdbSdName.DataSource = dt;
                        rdbSdName.DataTextField = "NAME";
                        rdbSdName.DataValueField = "RowNumber";
                        rdbSdName.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "KeyFun", "Popup.show('divBackground')", true);
                    }
                    else if (Flag == 3 && dt.Rows.Count ==2)
                    {
                        ViewState["dt"] = dt;
                        ViewState["grdRowIndex"] = grdvRow.RowIndex.ToString();
                        rdbSdName.DataSource = dt;
                        rdbSdName.DataTextField = "NAME";
                        rdbSdName.DataValueField = "RowNumber";
                        rdbSdName.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "KeyFun", "Popup.show('divBackground')", true);
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            else
            {
                if (dt.Rows.Count >= Flag)
                {
                    TextDependantName.Text = dt.Rows[0]["NAME"].ToString();
                    dob.Text = dt.Rows[0]["DOB"].ToString();
                    DDLBloodGroup.ClearSelection();
                    DDLBloodGroup.Items.FindByText(dt.Rows[0]["BLOODGROUP"].ToString()).Selected = true;
                    ddlRelation.ClearSelection();
                    ddlRelation.Items.FindByValue(dt.Rows[0]["RELATION"].ToString()).Selected = true;
                    TextRemarks.Text = dt.Rows[0]["REMARKS"].ToString();
                }
                else
                {
                    ShowMessage("No information for selected realtionship is avilable in last year form.");
                    TextDependantName.Text = "";
                    dob.Text = "";
                    TextRemarks.Text = "";
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(TextDependantName.Text) || !string.IsNullOrEmpty(dob.Text))
            {

            }
            else
            {
                ShowMessage("No information for selected realtionship is avilable in last year form.");
                TextDependantName.Text = "";
                dob.Text = "";
                TextRemarks.Text = "";
            }
            //DDLBloodGroup.SelectedValue = "0";
        }
        string mainplan =(string)ViewState["MainPlan"];
        if (mainplan.ToLower().Contains("and"))
        {
            foreach (GridViewRow grow in grdvDependantsDetail.Rows)
            {
                DropDownList DDLRelation = (DropDownList)grow.Cells[4].FindControl("ddlRelation");
                //  if(ddlRelation.SelectedValue=="
                if (ddlRelation.SelectedValue == "Father" || ddlRelation.SelectedValue == "Mother")
                {
                    if (ddlRelation.SelectedValue == "Father")
                    {
                        if (grow.RowIndex == 0)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[1].FindControl("ddlRelation");
                            DDLRelation1.SelectedValue = "Mother";
                            //break;
                        }
                    }
                    else
                    {
                        if (grow.RowIndex == 1)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[0].FindControl("ddlRelation");
                            DDLRelation1.SelectedValue = "Father";
                            // break;
                        }
                    }
                }
                if (ddlRelation.SelectedValue == "Father-In-Law" || ddlRelation.SelectedValue == "Mother-In-Law")
                {
                    if (ddlRelation.SelectedValue == "Father-In-Law")
                    {
                        if (grow.RowIndex == 0)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[1].FindControl("ddlRelation");
                            DDLRelation1.SelectedValue = "Mother-In-Law";
                            // break;
                        }
                    }
                    else
                    {
                        if (grow.RowIndex == 1)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[0].FindControl("ddlRelation");
                            DDLRelation1.SelectedValue = "Father-In-Law";
                            // break;
                        }
                    }

                }

            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
            int month = DateTime.Now.Month;
            int isExist = 0;
            int year = DateTime.Now.Year;
            int cyear = 0;
            if (month < 11)
            {
                cyear = year - 1;
            }
            else
            {
                cyear = year;
            }
            isExist = CheckAlreadyClaimed(year);
            if (isExist > 0)
            {
                ShowMessageAndClose("You have already submitted Mediclaim for the year");
            }
            else
            {
                string XmlDataGridItems = string.Empty;
                if (hdnNumOfDependants.Value == "0")
                {
                    SubmitMediclaim(XmlDataGridItems);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = GetGridViewData();
                    string son = "Son";
                    DataRow[] dr = dt.Select("Relation='" + son + "'");
                    int chechdob = CheckDOB(dr);
                    if (chechdob == 1)
                    {
                        StringWriter strFpaHeadWriter = new StringWriter();
                        dt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
                        XmlDataGridItems = strFpaHeadWriter.ToString();
                        SubmitMediclaim(XmlDataGridItems);
                    }
                    else
                    {
                        trMessage.Visible = true;
                        lblErrorMessage.Text = "Son age can not be more than 25 years.";
                    }
                }
          }
    }

    public void SubmitMediclaim(string XmlDataGridItems)
    {
        trMessage.Visible = false;        
        int resDep = 0;
        MediMain oclsMM = new MediMain();
        CommonFunction oclsCF = new CommonFunction();
        string SapID = oclsCF.GetSapIdByLoginName(ReturnLoginName());
        Int64 DBIndentity = 0;
        DataSet ds = new DataSet();
        ds = oclsMM.MediclaimBySerialNo(lblSerial.Text);
        //IF Draft Already Exist in DB then Delete first and SAVE again
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                oclsMM.DeleteMediclaim(lblSerial.Text);
                DBIndentity = oclsMM.InsertMediclaim(SapID, Convert.ToInt16(ddlPlans.SelectedValue), hdnNumOfDependants.Value, ddlSumInsured.SelectedValue, 2, txtCalculatedPremium.Text, lblSerial.Text, hdnClaimYear.Value.ToString(), txtMobile.Text, txtEmail.Text, hdnActualPrem.Value, hdnOwnerSapID.Value, XmlDataGridItems, lblComments.Text, ddlUserBloodGroup.SelectedItem.Text, txtCareOf.Text, txtStreet.Text, txt2ndlineAdd.Text, txtPostalCode.Text, txtCity.Text,txtBankName.Text,txtIfscCode.Text,txtAcNo.Text,txtAcHName.Text);

                if (DBIndentity == 1)
                {
                    oclsMM.InsertLogs(lblSerial.Text, "Submitted To Mediclaim Admin", lblName.Text);
                    CommonMail(hdnHREmail.Value, "2");
                    ShowMessageAndClose("Your Mediclaim has been Submitted");
                }
                else if (DBIndentity == 0)
                {
                    ShowMessage("Please try after some time");
                }
            }
            else
            {
                DBIndentity = oclsMM.InsertMediclaim(SapID, Convert.ToInt16(ddlPlans.SelectedValue), hdnNumOfDependants.Value, ddlSumInsured.SelectedValue, 2, txtCalculatedPremium.Text, lblSerial.Text, hdnClaimYear.Value.ToString(), txtMobile.Text, txtEmail.Text, hdnActualPrem.Value, hdnOwnerSapID.Value, XmlDataGridItems, lblComments.Text, ddlUserBloodGroup.SelectedItem.Text, txtCareOf.Text, txtStreet.Text, txt2ndlineAdd.Text, txtPostalCode.Text, txtCity.Text, txtBankName.Text, txtIfscCode.Text, txtAcNo.Text, txtAcHName.Text);

                if (DBIndentity == 1)
                {
                    oclsMM.InsertLogs(lblSerial.Text, "Submitted To Mediclaim Admin", lblName.Text);
                    CommonMail(hdnHREmail.Value, "2");
                    ShowMessageAndClose("Your Mediclaim has been Submitted");
                }
                else if (DBIndentity == 0)
                {
                    ShowMessage("Please try after some time");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessageAndClose("Unexpected Error: " + ex.Message.ToString());
        }
    }
                    
	private int CheckDOB(DataRow[] dr)
    {
        int status = 1;
        //CultureInfo cinfo = new CultureInfo("en-US");
        //Thread.CurrentThread.CurrentCulture = cinfo;
        DateTime Checkdate = DateTime.ParseExact(hdfYear.Value, "dd/MM/yyyy", null);
        foreach (DataRow cdr in dr)
        {
            string dob = cdr["DOB"].ToString();
			dob = GetDateddMMyy(dob);
            DateTime ActDate = DateTime.ParseExact(dob, "dd/MM/yyyy", null);
            int Chkyear = Checkdate.Year;
            int eYear = Chkyear - 25;
            string UserDob = "01/11/" + eYear;
            DateTime UserActDOB = DateTime.ParseExact(UserDob, "dd/MM/yyyy", null);

            if (ActDate <= UserActDOB)
            {
                status = 0;
            }


        }
        return status;
    }
    protected void btnSaveAsDraft_Click(object sender, EventArgs e)
    {
        string XmlDataGridItems = string.Empty;
        if (hdnNumOfDependants.Value == "0")
        {
        }
        else
        {
            DataTable dt = new DataTable();
            dt = GetGridViewData();
            StringWriter strFpaHeadWriter = new StringWriter();
            dt.WriteXml(strFpaHeadWriter, XmlWriteMode.IgnoreSchema);
            XmlDataGridItems = strFpaHeadWriter.ToString();
        }
       
        int resDep = 0;
        MediMain oclsMM = new MediMain();
        CommonFunction oclsCF = new CommonFunction();
        string SapID = oclsCF.GetSapIdByLoginName(ReturnLoginName());
        Int64 DBIndentity = 0;
        DataSet ds = new DataSet();
        ds = oclsMM.MediclaimBySerialNo(lblSerial.Text);
        //IF Draft Already Exist in DB then Delete first and SAVE again
        if (ds.Tables[0].Rows.Count > 0)
        {
            oclsMM.DeleteMediclaim(lblSerial.Text);
            DBIndentity = oclsMM.InsertMediclaim(SapID, Convert.ToInt16(ddlPlans.SelectedValue), hdnNumOfDependants.Value, ddlSumInsured.SelectedValue, 1, txtCalculatedPremium.Text, lblSerial.Text, hdnClaimYear.Value.ToString(), txtMobile.Text, txtEmail.Text, hdnActualPrem.Value, hdnOwnerSapID.Value, XmlDataGridItems, lblComments.Text, ddlUserBloodGroup.SelectedItem.Text, txtCareOf.Text, txtStreet.Text, txt2ndlineAdd.Text, txtPostalCode.Text, txtCity.Text, txtBankName.Text, txtIfscCode.Text, txtAcNo.Text, txtAcHName.Text);

            //Check dt has rows or not AND Gridview is visible or not
            if (DBIndentity != 0)
            {
                ShowMessageAndClose("Form Saved Sucessfully");
            }
        }
        else
        {
            DBIndentity = oclsMM.InsertMediclaim(SapID, Convert.ToInt16(ddlPlans.SelectedValue), hdnNumOfDependants.Value, ddlSumInsured.SelectedValue, 1, txtCalculatedPremium.Text, lblSerial.Text, hdnClaimYear.Value.ToString(), txtMobile.Text, txtEmail.Text, hdnActualPrem.Value, hdnOwnerSapID.Value, XmlDataGridItems, lblComments.Text, ddlUserBloodGroup.SelectedItem.Text, txtCareOf.Text, txtStreet.Text, txt2ndlineAdd.Text, txtPostalCode.Text, txtCity.Text, txtBankName.Text, txtIfscCode.Text, txtAcNo.Text, txtAcHName.Text);

            if (DBIndentity != 0)
            {
                ShowMessageAndClose("Form Saved Sucessfully");
            }
        }
    }
    protected void lnkBtnDelete_Click(object sender, EventArgs e)
    {
        int DelId = 0;
        MediMain oclsMM = new MediMain();
        DelId = oclsMM.DeleteMediClaim(hdnSERIALNO.Value);
        if (DelId == 1)
        {
            ShowMessageAndClose("Your Mediclaim has been deleted.");
        }
        else
        {
            ShowMessage("Please try after some time");
        }
    }
    protected void ddlPlans_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnFatherFlag.Value = "0";
        txtCalculatedPremium.Text = "";
        GetSumInsured();
        FillRelationDropdown(Convert.ToInt16(ddlPlans.SelectedValue));
    }
    #endregion

    #region PRIVATE SECTION
    
    //ADD NUMBER OF DEPENDANTS ACCORDING TO THE PLAN
    private void AddNewRow(int Dependants)
    {
        int rowIndex = 0;
        if (Dependants > 1)
        {
            DataTable dtControlsOnly = new DataTable();
            dtControlsOnly.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dtControlsOnly.Columns.Add(new DataColumn("Col1", typeof(string)));
            dtControlsOnly.Columns.Add(new DataColumn("Col2", typeof(string)));
            //dtControlsOnly.Columns.Add(new DataColumn("Col3", typeof(string)));
            dtControlsOnly.Columns.Add(new DataColumn("Col3", typeof(string)));
            dtControlsOnly.Columns.Add(new DataColumn("Col4", typeof(string)));
            dtControlsOnly.Columns.Add(new DataColumn("Col5", typeof(string)));
            DataRow drCurrentRow = null;

            for (int i = 1; i <= Dependants; i++)
            {
                TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[rowIndex].Cells[1].FindControl("txtNameOfDependant");
                TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[rowIndex].Cells[2].FindControl("txtDOB");
                DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[rowIndex].Cells[3].FindControl("ddlBloodGroup");

                //RadioButtonList RBtnLstGender = (RadioButtonList)grdvDependantsDetail.Rows[rowIndex].Cells[4].FindControl("rBtnLstGender");
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[rowIndex].Cells[4].FindControl("ddlRelation");
                TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[rowIndex].Cells[5].FindControl("txtRemarks");

                drCurrentRow = dtControlsOnly.NewRow();
                drCurrentRow["RowNumber"] = i;

                drCurrentRow["Col1"] = TextDependantName;
                drCurrentRow["Col2"] = TextDOB;
                drCurrentRow["Col3"] = DDLBloodGroup;
                //drCurrentRow["Col4"] = RBtnLstGender;
                drCurrentRow["Col4"] = DDLRelation;
                drCurrentRow["Col5"] = TextRemarks;
                dtControlsOnly.Rows.Add(drCurrentRow);
            }
            grdvDependantsDetail.Visible = true;
            grdvDependantsDetail.DataSource = null;
            grdvDependantsDetail.DataBind();
            grdvDependantsDetail.DataSource = dtControlsOnly;
            grdvDependantsDetail.DataBind();
        }
        else if (Dependants == 1)
        {
            InitializeDependantsGrid();
            grdvDependantsDetail.Visible = true;
        }
        else
        {
            grdvDependantsDetail.Visible = false;
        }
        //FillRelationDropdown(Convert.ToInt16(ddlPlans.SelectedValue));
    }
    private void BindHeader(string SAPID)
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.BindHeader(SAPID);
        if (dt.Rows.Count > 0)
        {
            lblName.Text = dt.Rows[0]["ENAME"].ToString();
            lblLocation.Text = dt.Rows[0]["LOC"].ToString();
            lblEcode.Text = dt.Rows[0]["ECODE"].ToString();
            lblDesignation.Text = dt.Rows[0]["DESIGNATION"].ToString();
            lblDepartment.Text = dt.Rows[0]["DEPARTMENT"].ToString();
            lblHRName.Text = dt.Rows[0]["HRNAME"].ToString();
            lblAge.Text = dt.Rows[0]["AGE"].ToString();
            lblDOb.Text = dt.Rows[0]["DOB"].ToString();
            txtEmail.Text = dt.Rows[0]["EMAIL"].ToString();
            hdnHREmail.Value = dt.Rows[0]["HREMAIL"].ToString();
            hdnOwnerSapID.Value = dt.Rows[0]["HRSAPID"].ToString();
            hdnDesID.Value = dt.Rows[0]["DES_ID"].ToString();
            hfBu.Value = dt.Rows[0]["BU"].ToString();
            hdfYear.Value = dt.Rows[0]["Cyear"].ToString();
			hdnClaimYear.Value = dt.Rows[0]["ClaimYear"].ToString();
            if (dt.Rows[0]["GENDER"].ToString() == "True")
                lblGender.Text = "Male";
            else
                lblGender.Text = "Female";
        }
    }
    private void BindPlans()
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.BindPlans();
        if (dt.Rows.Count > 0)
        {
            ListItem li = new ListItem("Select", "0");
            ddlPlans.Items.Add(li);
            ddlPlans.AppendDataBoundItems = true;
            ddlPlans.DataTextField = "PLAN_NAME";
            ddlPlans.DataValueField = "ID";
            ddlPlans.DataSource = dt;
            ddlPlans.DataBind();
        }
    }
    private void GetSumInsured()
    {
        int AgeID = 0;
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        AgeID = oclsMM.GetAgeIDByAge(Convert.ToInt16(lblAge.Text));
        //classify = (input > 0) ? "positive" : "negative";
        //Check for Staff or Executive
        string StaffExec = string.Empty;
        //StaffExec = (((Convert.ToInt16(hdnDesID.Value)) > 75) ? "S" : "E");

        StaffExec = oclsMM.Get_Multy_DataByPassingQuery(hfBu.Value, hdnDesID.Value);
        dt = oclsMM.GetDataSumInsured(Convert.ToInt16(ddlPlans.SelectedValue), AgeID, StaffExec);
        ViewState["ForPremium"] = dt;

        if (dt.Rows.Count > 0)
        {
            hdnNumOfDependants.Value = dt.Rows[0]["NUM_OF_DEPENDANTS"].ToString();
            ddlSumInsured.Items.Clear();
            ListItem li = new ListItem("Select", "0");
            ddlSumInsured.Items.Add(li);
            ddlSumInsured.AppendDataBoundItems = true;
            ddlSumInsured.DataTextField = "Insured_Amount";
            ddlSumInsured.DataValueField = "Insured_Amount";
            ddlSumInsured.DataSource = dt;
            ddlSumInsured.DataBind();
        }
        AddNewRow(Convert.ToInt16(dt.Rows[0]["NUM_OF_DEPENDANTS"].ToString()));
    }
    private void InitializeDependantsGrid()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Col1", typeof(string)));
        dt.Columns.Add(new DataColumn("Col2", typeof(string)));
        dt.Columns.Add(new DataColumn("Col3", typeof(string)));
        //dt.Columns.Add(new DataColumn("Col4", typeof(string)));
        dt.Columns.Add(new DataColumn("Col5", typeof(string)));
        dt.Columns.Add(new DataColumn("Col6", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Col1"] = string.Empty;
        dr["Col2"] = string.Empty;
        dr["Col3"] = string.Empty;
        //dr["Col4"] = string.Empty;
        dr["Col5"] = string.Empty;
        dr["Col6"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["dtInitial"] = dt;
        grdvDependantsDetail.DataSource = dt;
        grdvDependantsDetail.DataBind();
    }
    private DataTable GetGridViewData()
    {
        DataTable dt = new DataTable("Dependent");
        if (grdvDependantsDetail.HeaderRow != null)
        {
            //start from i=1 because sr no. dont required
            for (int i = 1; i < grdvDependantsDetail.HeaderRow.Cells.Count; i++)
            {
                dt.Columns.Add(grdvDependantsDetail.HeaderRow.Cells[i].Text.Replace(" ", "_"));
            }
            //Add one more column for gender
            dt.Columns.Add("Gender", typeof(String)).SetOrdinal(3);
        }
        foreach (GridViewRow row in grdvDependantsDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            TextBox TextDependantName = (TextBox)row.Cells[1].FindControl("txtNameOfDependant");
            TextBox TextDOB = (TextBox)row.Cells[2].FindControl("txtDOB");
            DropDownList DDLBloodGroup = (DropDownList)row.Cells[3].FindControl("ddlBloodGroup");

            //RadioButtonList RBtnLstGender = (RadioButtonList)row.Cells[4].FindControl("rBtnLstGender");
            DropDownList DDLRelation = (DropDownList)row.Cells[4].FindControl("ddlRelation");
            TextBox TextRemarks = (TextBox)row.Cells[5].FindControl("txtRemarks");

            dr[0] = TextDependantName.Text;
            dr[1] = TextDOB.Text;
            dr[2] = DDLBloodGroup.SelectedItem.Text;
            dr[3] = ReturnGender(DDLRelation.SelectedValue);
            //dr[3] = RBtnLstGender.SelectedValue;
            dr[4] = DDLRelation.SelectedValue;
            dr[5] = TextRemarks.Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
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
    private void MediclaimBySerialNo(string SerialNo)
    {
        DataSet ds = new DataSet();
        MediMain oclsMM = new MediMain();
        ds = oclsMM.MediclaimBySerialNo(SerialNo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            BindHeader(dt.Rows[0]["SAPID"].ToString());

            ddlPlans.Items.FindByValue(dt.Rows[0]["PLANID"].ToString()).Selected = true;
            GetSumInsured();
            ddlSumInsured.Items.FindByValue(dt.Rows[0]["SUM_INSURED_AMOUNT"].ToString()).Selected = true;
			hdnActualPrem.Value = dt.Rows[0]["ACTUAL_PREMIUM_AMOUNT"].ToString(); 
            txtCalculatedPremium.Text = dt.Rows[0]["EMP_PREMIUM"].ToString();
			
			 txtCareOf.Text = dt.Rows[0]["CARE_OF"].ToString();
            txtStreet.Text = dt.Rows[0]["STREET_HOUSENO"].ToString();
            txt2ndlineAdd.Text = dt.Rows[0]["ADDRESS2"].ToString();
            txtPostalCode.Text = dt.Rows[0]["POSTALCODE"].ToString();
            txtCity.Text = dt.Rows[0]["CITY"].ToString();

            //Account Detail 

            txtBankName.Text = dt.Rows[0]["BName"].ToString();
            txtIfscCode.Text = dt.Rows[0]["IfscCode"].ToString();
            txtAcNo.Text = dt.Rows[0]["AcNumber"].ToString();
            txtAcHName.Text = dt.Rows[0]["AcCorrectName"].ToString();
			
            txtEmail.Text = dt.Rows[0]["EMAIL"].ToString();
            txtMobile.Text = dt.Rows[0]["MOBILE_NO"].ToString();
            lblComments.Text = dt.Rows[0]["HRCOMMENTS"].ToString();
            ddlUserBloodGroup.Items.FindByValue(dt.Rows[0]["BloodGroup"].ToString()).Selected = true;
            lblStatus.Text = GetStatusTextByStatusID(Convert.ToInt16(dt.Rows[0]["STATUS"].ToString()));
            BindDependantsDetail(ds.Tables[1], Convert.ToInt16(dt.Rows[0]["PLANID"]));
            //--No-Need-Now-FillRelationDropdown(Convert.ToInt16(dt.Rows[0]["PLANID"]));
            CommonFunction oclsCF = new CommonFunction();
            if (oclsCF.GetSapIdByLoginName(ReturnLoginName()) == dt.Rows[0]["OWNER_SAPID"].ToString() && dt.Rows[0]["STATUS"].ToString() == "2")
            {
                //This is the only case when MY APPROVAL(second grid) will process
                //Everthing will not editable and Approve and reject button will shown
                EnableDisableAllControls(ds.Tables[1], false);
            }
            else if (oclsCF.GetSapIdByLoginName(ReturnLoginName()) == dt.Rows[0]["SAPID"].ToString() && (dt.Rows[0]["STATUS"].ToString() == "5" || dt.Rows[0]["STATUS"].ToString() == "1"))
            {
                //Three button will show and the first grid(from Dashboard) will process
                //Everything will be editable
                EnableDisableAllControls(ds.Tables[1], true);
                //txtHRComments.Enabled = false;
                lnkBtnDelete.Visible = true;
            }
            else if (oclsCF.GetSapIdByLoginName(ReturnLoginName()) == dt.Rows[0]["SAPID"].ToString() && dt.Rows[0]["STATUS"].ToString() == "2")
            {
                //Submit to Approver you can only see So everything will be 
                //non eidtable and Close button will show
                EnableDisableAllControls(ds.Tables[1], false);
                btnClose.Visible = true;
            }
        }
    }
    private void BindDependantsDetail(DataTable dt,int PlanID)
    {
        grdvDependantsDetail.Visible = true;
        grdvDependantsDetail.DataSource = dt;
        grdvDependantsDetail.DataBind();
        FillRelationDropdown(PlanID);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
                TextDependantName.Text = dt.Rows[i]["NAME"].ToString();
                TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
                TextDOB.Text = dt.Rows[i]["DOB"].ToString();
                DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
                DDLBloodGroup.ClearSelection();
                DDLBloodGroup.Items.FindByText(dt.Rows[i]["BLOODGROUP"].ToString()).Selected = true;
                //RadioButtonList RBtnLstGender = (RadioButtonList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("rBtnLstGender");
                //RBtnLstGender.Items.FindByValue(dt.Rows[i]["GENDER"].ToString()).Selected = true;

                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");
                DDLRelation.ClearSelection();
                if (i >= 3)
                {
                    DDLRelation.Items.Clear();
                    AddChild(DDLRelation);
                }
                DDLRelation.Items.FindByValue(dt.Rows[i]["RELATION"].ToString()).Selected = true;
                TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
                TextRemarks.Text = dt.Rows[i]["REMARKS"].ToString();
            }
        }
    }
    //--Make All Controls Disable OR Enable
    private void EnableDisableAllControls(DataTable dt, bool Action)
    {
	    txtCareOf.Enabled = Action;
        txtStreet.Enabled = Action;
        txt2ndlineAdd.Enabled = Action;
        txtPostalCode.Enabled = Action;
        txtCity.Enabled = Action;


        txtBankName.Enabled = Action;
        txtIfscCode.Enabled = Action;
        txtAcNo.Enabled = Action;
        txtAcHName.Enabled = Action;

        ddlPlans.Enabled = Action;
        ddlSumInsured.Enabled = Action;
        txtEmail.Enabled = Action;
        txtMobile.Enabled = Action;
        ddlUserBloodGroup.Enabled = Action;
       // txtHRComments.Enabled = Action;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
            TextDependantName.Enabled = Action;
            TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
            TextDOB.Enabled = Action;
            DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
            DDLBloodGroup.Enabled = Action;
            //RadioButtonList RBtnLstGender = (RadioButtonList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("rBtnLstGender");
            //RBtnLstGender.Enabled = Action;
            DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");
            DDLRelation.Enabled = Action;
            TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
            TextRemarks.Enabled = Action;
        }
        btnSaveAsDraft.Visible = Action;
        btnSubmit.Visible = Action;
    }
    //--Still Unused 
    private void MakeClearFields()
    {
        lblSerial.Text = "";
        txtEmail.Text = "";
        txtMobile.Text = "";
        txtCalculatedPremium.Text = "";
        ddlSumInsured.Enabled = false;
        ddlPlans.Enabled = false;
        grdvDependantsDetail.Dispose();
    }
    private string GetStatusTextByStatusID(int StatusID)
    {
        string Status = string.Empty;
        if (StatusID == 1)
        {
            Status = "Save As Draft";
        }
        else if (StatusID == 2)
        {
            Status = "Submitted to Mediclaim Admin";
        }
        else if (StatusID == 5)
        {
            Status = "Rejected by Mediclaim Admin";
        }
        return Status;
    }
    private void BindLogGrid(string SerialNo)
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.GetLogs(SerialNo);
        if (dt.Rows.Count > 0)
        {
            grdMediclaimLog.DataSource = dt;
            grdMediclaimLog.DataBind();
        }
    }
    //Check Whether the user has already submitted mediclaim or not
    private int CheckAlreadyClaimed(int year)
    {
        //Logically there is no need for below DB check now because I have created one more check on Load
        MediMain oclsMM = new MediMain();
        CommonFunction oclsCF = new CommonFunction();
        int Count = 0;
        Count = oclsMM.CheckUserExistInMediclaim(oclsCF.GetSapIdByLoginName(ReturnLoginName()), "MEDI_spCheckClaimAlready", year);
        return Count;
    }
    private void CheckAnyMediclaim(int year)
    {
        MediMain oclsMM = new MediMain();
        CommonFunction oclsCF = new CommonFunction();
        int Count = 0;
        Count = oclsMM.CheckUserExistInMediclaim(oclsCF.GetSapIdByLoginName(ReturnLoginName()), "MEDI_spCheckAnyMediclaim",year);
        if (Count > 0)
        {
            ShowMessageAndClose("You have already submitted mediclaim for this year");
        }
    }
    #endregion

    #region PUBLIC METHODS
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    public void ShowMessage(string mess)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Key1", "ShowMessage('" + mess + "')", true);
    }
    public void ShowMessageAndClose(string mess)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Key2", "alert('" + mess + "');CloseRefreshWindow();", true);
    }
    public string GetNewSRNo(string NameOFAppliaction)
    {
        string SR_Number = string.Empty;
        MediMain oclsMM = new MediMain();
        SR_Number = oclsMM.GetNewSRNumber(NameOFAppliaction);
        return SR_Number;
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
        string SerailNo = hdnSERIALNO.Value;
        string Employeename = Convert.ToString(lblName.Text);
        string Approvername = Convert.ToString(lblName.Text);
        string comment = string.Empty;

        if (hfBu.Value.Trim() == "REM")
            newUrl = "http://epicenter.vecv.net/mycorner/mediclaim/SitePages/home.aspx";
        else
            newUrl = "http://epicenter.vecv.net/mycorner/mediclaim/SitePages/home.aspx";

        try
        {
            if (lblComments.Text.Length <= 250)
            {
                comment = Convert.ToString(lblComments.Text);
            }
            else
            {
                comment = Convert.ToString(lblComments.Text).Substring(0, 250);
            }
            Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + lblHRName.Text.ToString() + "<br/><br/>"+lblName.Text+" has submitted mediclaim form <b>" + SerailNo + "</b> for your approval. <br /><br/><a href=\"" + newUrl + "\">Click here </a> to open the form.<br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";
            Subject = "Mediclaim request " + SerailNo + " has been Submitted...";
            //Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
        }
        catch (Exception ex)
        {
            // Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion


    private void FillRelationDropdown(int PlanID)
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.MainMasterPlan(PlanID);
        if (dt.Rows.Count > 0)
        {//"SELF, 3 CHILDREN, MOTHER/MOTHER-IN-LAW AND FATHER/FATHER-IN-LAW"
            string MainPlan = dt.Rows[0]["FAMILY_MEMBERS"].ToString();
            lblFmember.Text = MainPlan;
            ViewState["MainPlan"] = MainPlan;
            int grdRowIndex = 0;

            

            if (MainPlan.ToLower().Contains("mother/mother-in-law or father/father-in-law") || MainPlan.ToLower().Contains("father/father-in-law or mother/mother-in-law"))
            {
			    int i=0;
                
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFatherOrMother(DDLRelation);
                foreach (ListItem li in DDLRelation.Items)
                { 
				    i++;
                    int RetId = FillAllDetail(li.Value.ToString(), grdRowIndex, "1FM");
                    if (RetId == 1)
                    {
                        
                        grdRowIndex++;
                        break;
                       
                    }
					if(i == 4)
					{
					  grdRowIndex++;
					}
                }
            }
            if (MainPlan.ToLower().Contains("mother/mother-in-law and father/father-in-law") || MainPlan.ToLower().Contains("father/father-in-law and mother/mother-in-law"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFather(DDLRelation);
                FillAllDetail(DDLRelation.SelectedValue, grdRowIndex, "1FM");
                grdRowIndex++;
                DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddMother(DDLRelation1);
                FillAllDetail(DDLRelation1.SelectedValue, grdRowIndex, "1FM");
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("spouse"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddSpouse(DDLRelation);
                FillAllDetail(DDLRelation.SelectedValue, grdRowIndex, "1HW");
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("1 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                foreach (ListItem li in DDLRelation.Items)
                {
                    int RetId = FillAllDetail(li.Value.ToString(), grdRowIndex, "1c");
                    if (RetId == 1)
                    {
                        grdRowIndex++;
                        break;
                    }
                }
            }

            if (MainPlan.ToLower().Contains("2 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                foreach (ListItem li in DDLRelation.Items)
                {
                    int RetId = FillAllDetail(li.Value.ToString(), grdRowIndex, "2c");
                    if (RetId == 2)
                    {
                        grdRowIndex++;
                        if (grdvDependantsDetail.Rows.Count - 1 >= grdRowIndex)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                            // AddChild(DDLRelation1);
                        }
                    }
                    else
                    {
                        if (grdvDependantsDetail.Rows.Count - 1 > grdRowIndex)
                        {
                            grdRowIndex++;
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                            AddChild(DDLRelation1);
                        }
                    }
                }
            }

            ///  3 child logic
             if (MainPlan.ToLower().Contains("3 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                foreach (ListItem li in DDLRelation.Items)
                {
                    int RetId = FillAllDetail(li.Value.ToString(), grdRowIndex, "3c");
                    if (RetId == 3)
                    {
                        grdRowIndex++;
                        if (grdvDependantsDetail.Rows.Count - 1 >= grdRowIndex)
                        {
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                            AddChild(DDLRelation1);
                        }
                    }
                    else
                    {
                        if (grdvDependantsDetail.Rows.Count - 1 >= grdRowIndex)
                        {
                            grdRowIndex++;
                            DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                            DDLRelation1.Items.Clear();
                            AddChild(DDLRelation1);
                        }
                    }
                }
            }
        }

    }
    private int FillAllDetail(string Value, int grdRowIndex, string MainPlan)
    {
        int RetId = 0;
        MediMain oclsMM = new MediMain();
        DataTable dt = oclsMM.GetDataForRelation(Value, hdnCurrentLoginSapID.Value);
        if (dt.Rows.Count > 0)
        {
            int j = 0;
            int i = grdRowIndex;
            if (dt.Rows.Count > 1)
            {

                if (MainPlan == "2c")
                {
                    if (grdvDependantsDetail.Rows.Count - 1 >= grdRowIndex)
                    {
                        if (Value == "Daughter")
                        {
                            i = grdRowIndex;
                        }
                        else
                        {
                            i = grdRowIndex - 1;
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
                            TextDependantName.Text = dr["NAME"].ToString();
                            TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
                            TextDOB.Text = dr["DOB"].ToString();
                            DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
                            DDLBloodGroup.ClearSelection();
                            DDLBloodGroup.Items.FindByText(dr["BLOODGROUP"].ToString()).Selected = true;

                            DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");
                            if (Value == "Daughter" && j == 1)
                            {
                                AddChild(DDLRelation);
                            }
                            DDLRelation.ClearSelection();
                            DDLRelation.Items.FindByValue(dr["RELATION"].ToString()).Selected = true;
                            TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
                            TextRemarks.Text = dr["REMARKS"].ToString();
                            i++;
                            j++;
                            RetId = 2;

                        }

                    }
                }

                    /// 3 child
                else if (MainPlan == "3c")
                {
                    if (grdvDependantsDetail.Rows.Count - 1 >= grdRowIndex)
                    {
                        if (Value == "Daughter")
                        {
                            i = grdRowIndex;
                        }
                        else
                        {
                            i = grdRowIndex-1 ;
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
                            TextDependantName.Text = dr["NAME"].ToString();
                            TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
                            TextDOB.Text = dr["DOB"].ToString();
                            DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
                            DDLBloodGroup.ClearSelection();
                            DDLBloodGroup.Items.FindByText(dr["BLOODGROUP"].ToString()).Selected = true;

                            DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");
                            if ((Value == "Daughter" || Value == "Son") && (j >= 1 && j <= 2))
                            {
                                DDLRelation.Items.Clear();
                                AddChild(DDLRelation); 
                            }
                            
                            DDLRelation.ClearSelection();
                            DDLRelation.Items.FindByValue(dr["RELATION"].ToString()).Selected = true;
                            TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
                            TextRemarks.Text = dr["REMARKS"].ToString();
                            i++;
                            j++;
                            RetId = 3;

                        }

                    }
                }
                else if (MainPlan == "1c")
                {
                    rdbSdName.DataSource = dt;
                    rdbSdName.DataTextField = "NAME";
                    rdbSdName.DataValueField = "RowNumber";
                    rdbSdName.DataBind();
                    ViewState["grdRowIndex"] = grdRowIndex;
                    ViewState["dt"] = dt;
                    spnTest.InnerText = "Please select one child as this plan allows only one child.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "KeyFun", "Popup.show('divBackground')", true);
                }
                else
                {
                    FillDepTable(grdvDependantsDetail, dt, i, 0);
                    RetId = 1;
                }
            }

            else
            {
                FillDepTable(grdvDependantsDetail, dt, i, 0);
                RetId = 1;
            }

        }
        else
        {

            RetId = 0;
        }
        return RetId;
    }
    protected void rdbSdName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dt"];
        int i = Convert.ToInt32(ViewState["grdRowIndex"].ToString());
        if (rdbSdName.SelectedValue == "1")
        {
            FillDepTable(grdvDependantsDetail, dt, i, 0);
        }
        else if (rdbSdName.SelectedValue == "2")
        { FillDepTable(grdvDependantsDetail, dt, i, 1); }
        else 
        {
            FillDepTable(grdvDependantsDetail, dt, i, 2);
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "KeyFun", "Popup.hide('divBackground')", true);
    }
    private void FillDepTable(GridView grdvDependantsDetail, DataTable dt, int i, int j)
    {
        if (dt.Rows.Count > 0)
        {
            TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
            TextDependantName.Text = dt.Rows[j]["NAME"].ToString();
            TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
            TextDOB.Text = dt.Rows[j]["DOB"].ToString();
            DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
            DDLBloodGroup.ClearSelection();
            DDLBloodGroup.Items.FindByText(dt.Rows[j]["BLOODGROUP"].ToString()).Selected = true;

            DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");
            DDLRelation.ClearSelection();
            //DDLRelation.SelectedValue = dt.Rows[j]["RELATION"].ToString();
            DDLRelation.Items.FindByValue(dt.Rows[j]["RELATION"].ToString()).Selected = true;
            TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
            TextRemarks.Text = dt.Rows[j]["REMARKS"].ToString();
        }
    }
    private void AddSpouse(DropDownList ddlRelation)
    {
        if (lblGender.Text == "Male")
        {
            ddlRelation.Items.Add(new ListItem("Wife", "Wife"));
            ddlRelation.Items.Add(new ListItem("Husband", "Husband"));
        }
        else
        {
            ddlRelation.Items.Add(new ListItem("Husband", "Husband"));
            ddlRelation.Items.Add(new ListItem("Wife", "Wife"));
        }

    }
    private void AddFather(DropDownList ddlRelation)
    {
        ddlRelation.Items.Add(new ListItem("Father", "Father"));
        ddlRelation.Items.Add(new ListItem("Father-In-Law", "Father-In-Law"));
    }
    private void AddMother(DropDownList ddlRelation)
    {
        ddlRelation.Items.Add(new ListItem("Mother", "Mother"));
        ddlRelation.Items.Add(new ListItem("Mother-In-Law", "Mother-In-Law"));
    }
    private void AddChild(DropDownList ddlRelation)
    {
        ddlRelation.Items.Add(new ListItem("Daughter", "Daughter"));
        ddlRelation.Items.Add(new ListItem("Son", "Son"));
    }
    private void AddFatherOrMother(DropDownList ddlRelation)
    {
        ddlRelation.Items.Add(new ListItem("Father", "Father"));
        ddlRelation.Items.Add(new ListItem("Father-In-Law", "Father-In-Law"));
        ddlRelation.Items.Add(new ListItem("Mother", "Mother"));
        ddlRelation.Items.Add(new ListItem("Mother-In-Law", "Mother-In-Law"));
    }
    private string ReturnGender(string Relation)
    {
        string Gender = string.Empty;
        if (Relation.Contains("Husband") || Relation.Contains("Father") || Relation.Contains("Son") || Relation.Contains("Father-In-Law"))
        {
            Gender = "M";
        }
        else if (Relation.Contains("Wife") || Relation.Contains("Mother") || Relation.Contains("Daughter") || Relation.Contains("Mother-In-Law"))
        {
            Gender = "F";
        }
        return Gender;
    }
    public void CheckSonDateofBirth(DropDownList ddlRelation, TextBox dob)
    {
        CultureInfo cinfo = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = cinfo;
        if (ddlRelation.SelectedItem.Text == "Son")
        {
            if (dob.Text == "" || ddlRelation.SelectedValue == "")
            {
                ddlRelation.SelectedValue = "Daughter";
            }
            else
            {
                DateTime dt = DateTime.ParseExact(dob.Text, "MM/dd/yyyy", null);
                DateTime Cdate = DateTime.ParseExact(hdfYear.Value, "MM/dd/yyyy", null);
                double year = (Cdate - dt).Days / 365;
                if (year > 25.0)
                {
                    dob.Text = "";
                    ddlRelation.SelectedValue = "Daughter";
					  trMessage.Visible = true;
                    lblErrorMessage.Text = "Son age can not more than 25 years.";
                }
                else
                {  
                    trMessage.Visible = false;
                    lblErrorMessage.Text = ""; 
                }
            }
        }
        else
        {  
            trMessage.Visible = false;
            lblErrorMessage.Text = "";

        }
    }
	public string GetDateddMMyy(string date)
    {
        string[] dateSplit = date.Split('/');
        string S1 = dateSplit[0];
        string S2 = dateSplit[1];
        string S3 = dateSplit[2];
        string S4 = string.Empty;
        string S5 = string.Empty;
        string NewDate = string.Empty;
        if (S2 == "1" || S2 == "2" || S2 == "3" || S2 == "4" || S2 == "5" || S2 == "6" || S2 == "7" || S2 == "8" || S2 == "9")
        {
            S2 = "0" + S2;
        }

        if (S1 == "1" || S1 == "2" || S1 == "3" || S1 == "4" || S1 == "5" || S1 == "6" || S1 == "7" || S1 == "8" || S1 == "9")
        {
            S1 = "0" + S1;
        }

        NewDate = S1 + "/" + S2 + "/" + S3;
        return NewDate;
    }
    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "mkrajput";
        //return "spstestss";
    }
}
