using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.SharePoint;

public partial class MediApproverClosed : System.Web.UI.Page
{
    MediMain Dal = new MediMain();
    Utility.Service Util = new Utility.Service();
    #region PAGE EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                BindPlans();
                hdnSERIALNO.Value = DoDecrypt(Request.QueryString["onrs"].ToString());
   hdnFlag.Value= DoDecrypt(Request.QueryString["flag"].ToString());
                lblSerial.Text = hdnSERIALNO.Value;
                MediclaimBySerialNo(hdnSERIALNO.Value);
                BindLogGrid(hdnSERIALNO.Value.Trim());
            }
        }
    }
   
    #endregion
 protected void lnkbtnRevert_Click(object sender, EventArgs e)
    {
        int Retstatus = 0;
        MediMain oclsMM = new MediMain();
        Retstatus = oclsMM.RevertMediclaimToUserbyAdmin(hdnSERIALNO.Value, ReturnLoginName(), txtHRComments.Text);
        if (Retstatus == 1)
        {
            ShowMessageAndClose("Form reverted to user successfully");
        }

    }

    #region"CommonMailFunction"
    public void CommonMail(string MailTo, string Status)
    {
        string MailbodyUser = string.Empty;
        string Mailbody = string.Empty;
        string Subject = string.Empty;
        string subjectUser = string.Empty;
        string newUrl = string.Empty;
        string SerailNo = hdnSERIALNO.Value;
        string Employeename = Convert.ToString(lblCurrentLogin.Text);
        string Approvername = Convert.ToString(lblCurrentLogin.Text);
        string comment = string.Empty;

        if (hfBu.Value.Trim() == "REM")
        {
            if (Status == "5" || Status == "4")
            {
                newUrl = "http://epicenter.vecv.net/mycorner/mediclaim/SitePages/rehome.aspx";
            }
        }
        else
        {
            if (Status == "5" || Status == "4")
            {
                newUrl = "http://epicenter.vecv.net/mycorner/mediclaim/SitePages/home.aspx";
            }
        }
        try
        {
            if (Status == "5")
            {
                if (txtHRComments.Text.Length <= 250)
                {
                    comment = Convert.ToString(txtHRComments.Text);
                }
                else
                {
                    comment = Convert.ToString(txtHRComments.Text).Substring(0, 250);
                }
                //comment = Convert.ToString(txtApproverComment.Text);
                Subject = "Mediclaim request " + SerailNo + " has been rejected...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + lblName.Text + ",<br /><br />Your Mediclaim request <b>" + SerailNo + "</b> has been rejected by " + Approvername + ". Reason for rejecting the request is <br/><br/> \"<span style='color:red;'>" + comment + "</span>\"<br /><br />Thanks,<br /><br />Mediclaim Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            else if (Status == "4")
            {
                Subject = "Mediclaim request " + SerailNo + " has been approved...";
                Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color:#FFFFF5; padding:10px;  font-family:Arial;font-size:11px'><div>Dear " + lblName.Text + ",<br /><br />Your Mediclaim  request has been approved by " + Approvername + ".<br /><br />Thanks,<br /><br />Mediclaim Team<br /><br /></div><div style='font-size:10px; font-style:italic'>This is system generated message</div></div></td></tr></table>";
                Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
            }
            //Util.Log("Try in CommonMail Sent to : " + MailTo.ToString() + "-----------MailBody<br/>", logfile);
        }
        catch (Exception ex)
        {
            //Util.Log("Catch in CommonMail" + ex.Message, logfile);
        }
    }
    #endregion
    #region PRIVATE SECTION
    private void MediclaimBySerialNo(string SerialNo)
    {
        DataSet ds = new DataSet();
        MediMain oclsMM = new MediMain();
        ds = oclsMM.MediclaimBySerialNo(SerialNo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ddlPlans.Items.FindByValue(dt.Rows[0]["PLANID"].ToString()).Selected = true;
            ddlSumInsured.Items.Add(new ListItem(dt.Rows[0]["SUM_INSURED_AMOUNT"].ToString(), dt.Rows[0]["SUM_INSURED_AMOUNT"].ToString()));
            ddlSumInsured.Items.FindByValue(dt.Rows[0]["SUM_INSURED_AMOUNT"].ToString()).Selected = true;
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
            hdnLeaveAR_RequestorSAPID.Value = dt.Rows[0]["SAPID"].ToString();
            lblStatus.Text = GetStatusTextByStatusID(Convert.ToInt16(dt.Rows[0]["STATUS"].ToString()));
            BindHeader(dt.Rows[0]["SAPID"].ToString(), (dt.Rows[0]["STATUS"].ToString()));
            BindDependantsDetail(ds.Tables[1], dt.Rows[0]["PLANID"].ToString());
            txtHRComments.Text = dt.Rows[0]["HRCOMMENTS"].ToString();
            ddlUserBloodGroup.Items.FindByValue(dt.Rows[0]["BloodGroup"].ToString()).Selected = true;
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
    private void BindHeader(string SAPID, string StatusId)
    {
        CommonFunction oclsCF = new CommonFunction();
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.BindHeader(SAPID);
        if (dt.Rows.Count > 0)
        {
            lblCurrentLogin.Text = Dal.GetEname(ReturnLoginName());

            lblName.Text = dt.Rows[0]["ENAME"].ToString();
            lblLocation.Text = dt.Rows[0]["LOC"].ToString();
            lblEcode.Text = dt.Rows[0]["ECODE"].ToString();
            lblDesignation.Text = dt.Rows[0]["DESIGNATION"].ToString();
            lblDepartment.Text = dt.Rows[0]["DEPARTMENT"].ToString();
            lblHRName.Text = dt.Rows[0]["HRNAME"].ToString();
            lblAge.Text = dt.Rows[0]["AGE"].ToString();
            lblDOb.Text = dt.Rows[0]["DOB"].ToString();
            hdnUserEmail.Value = dt.Rows[0]["EMAIL"].ToString();
            hdnOwnerSapID.Value = dt.Rows[0]["HRSAPID"].ToString();
            hfBu.Value = dt.Rows[0]["BU"].ToString();
            if (dt.Rows[0]["GENDER"].ToString() == "True")
                lblGender.Text = "Male";
            else
                lblGender.Text = "Female";
        }
        if (hdnFlag.Value == "1")
        {
            lnkbtnRevert.Visible = true;
            txtHRComments.Enabled = true;
        }
        else
        {
            lnkbtnRevert.Visible = false;
            txtHRComments.ReadOnly = true;
        }
    }
    public string getSAPID(string Gid)
    {
        string SAPID = Dal.Get_Single_DataByPassingQuery(Gid);
        return SAPID;
    }
    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "drpillai";
        //return "mkrajput";
    }
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    private void BindDependantsDetail(DataTable dt, string PlanID)
    {
        grdvDependantsDetail.Visible = true;
        grdvDependantsDetail.DataSource = dt;
        grdvDependantsDetail.DataBind();
       // FillRelationDropdown(PlanID);
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //        TextBox TextDependantName = (TextBox)grdvDependantsDetail.Rows[i].Cells[1].FindControl("txtNameOfDependant");
        //        TextDependantName.Text = dt.Rows[i]["NAME"].ToString();
        //        TextBox TextDOB = (TextBox)grdvDependantsDetail.Rows[i].Cells[2].FindControl("txtDOB");
        //        TextDOB.Text = dt.Rows[i]["DOB"].ToString();
        //        DropDownList DDLBloodGroup = (DropDownList)grdvDependantsDetail.Rows[i].Cells[3].FindControl("ddlBloodGroup");
        //        DDLBloodGroup.Items.FindByText(dt.Rows[i]["BLOODGROUP"].ToString()).Selected = true;
        //        //RadioButtonList RBtnLstGender = (RadioButtonList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("rBtnLstGender");
        //        //RBtnLstGender.Items.FindByValue(dt.Rows[i]["GENDER"].ToString()).Selected = true;

        //        DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[i].Cells[4].FindControl("ddlRelation");

        //        DDLRelation.Items.FindByValue(dt.Rows[i]["RELATION"].ToString()).Selected = true;
        //        TextBox TextRemarks = (TextBox)grdvDependantsDetail.Rows[i].Cells[5].FindControl("txtRemarks");
        //        Label lblRemarks = (Label)grdvDependantsDetail.Rows[i].Cells[5].FindControl("lblRemarks");
        //        TextRemarks.Text = dt.Rows[i]["REMARKS"].ToString();
        //        lblRemarks.Text = dt.Rows[i]["REMARKS"].ToString();
        //    }
        //}

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
    private int DoProcess(int Status, string OwnerSAPID)
    {
        int res = 0;
        MediMain oclsMM = new MediMain();
        //And Status is 4 means closed
        res = oclsMM.ApproveRejectMediclaim(hdnSERIALNO.Value, OwnerSAPID, txtHRComments.Text, Status);
        return res;
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
            Status = "Submitted To Mediclaim Admin";
        }
        else if (StatusID == 5)
        {
            Status = "Rejected By Mediclaim Admin";
        }
        else if (StatusID == 4)
        {
            Status = "Close";
        }
        return Status;
    }
    private void FillRelationDropdown(string PlanID)
    {
        MediMain oclsMM = new MediMain();
        DataTable dt = new DataTable();
        dt = oclsMM.MainMasterPlan(Convert.ToInt16(PlanID));

        if (dt.Rows.Count > 0)
        {
            string MainPlan = dt.Rows[0]["FAMILY_MEMBERS"].ToString();
            int grdRowIndex = 0;

            if (MainPlan.ToLower().Contains("mother/mother-in-law or father/father-in-law") || MainPlan.ToLower().Contains("father/father-in-law or mother/mother-in-law"))
            {
                //if (MainPlan.ToLower().Contains("father or mother") || MainPlan.ToLower().Contains("mother or father"))
                //{
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFatherOrMother(DDLRelation);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("father or mother") || MainPlan.ToLower().Contains("mother or father"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFatherOrMother(DDLRelation);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("mother/mother-in-law and father/father-in-law") || MainPlan.ToLower().Contains("father/father-in-law and mother/mother-in-law"))
            {
                //if (MainPlan.ToLower().Contains("father and mother") || MainPlan.ToLower().Contains("mother and father"))
                //{
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFather(DDLRelation);
                grdRowIndex++;
                DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddMother(DDLRelation1);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("father and mother") || MainPlan.ToLower().Contains("mother and father"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddFather(DDLRelation);
                grdRowIndex++;
                DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddMother(DDLRelation1);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("spouse"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddSpouse(DDLRelation);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("1 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                grdRowIndex++;
            }

            if (MainPlan.ToLower().Contains("2 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                grdRowIndex++;
                DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation1);
                grdRowIndex++;
            }
            if (MainPlan.ToLower().Contains("3 child"))
            {
                DropDownList DDLRelation = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation);
                grdRowIndex++;
                DropDownList DDLRelation1 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation1);
                grdRowIndex++;
                DropDownList DDLRelation2 = (DropDownList)grdvDependantsDetail.Rows[grdRowIndex].FindControl("ddlRelation");
                AddChild(DDLRelation2);
                grdRowIndex++;
            }

        }
    }

    private void AddSpouse(DropDownList ddlRelation)
    {
        ddlRelation.Items.Add(new ListItem("Husband", "Husband"));
        ddlRelation.Items.Add(new ListItem("Wife", "Wife"));
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

    protected object CheckBloodGroup(object value)
    {
        string str = string.Empty;
        if (value.ToString() == "Select")
        {
            str = "NA";
        }
        else
        {
            str = value.ToString();
        }
        return str;
    }
    #endregion

    #region PUBLIC METHODS
    public void ShowMessageAndClose(string mess)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Key2", "alert('" + mess + "');CloseRefreshWindow();", true);
    }

    #endregion
}
