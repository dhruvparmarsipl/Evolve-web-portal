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

public partial class EmployeeTransaction : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
	string logfile = "C:\\Eicher\\All Logs\\FPA";
    protected void Page_Load(object sender, EventArgs e)
    {
         if (!IsPostBack)
         {
            FillHeader(getSAPID(ReturnLoginName()));
            FillLocation(getSAPID(ReturnLoginName()));
         }
    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnSerialNo.InnerText = Dal.Serial_No("LC");
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            HFID.Value = spnSrNo.InnerText;
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
       // return "spstesttrg";
    }
    private void FillLocation(string sapid)
    {
        ddlLocation.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlLocation.Items.Add(li);
        ddlLocation.AppendDataBoundItems = true;
        DataTable dt_Loc = Dal.getLocationTrans(sapid);
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
        DataTable dt_Des = Dal.getDesignationTrans(locname, Sapid);
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
        DataTable dt_Emp = Dal.getFPAEmployeeTrans(locname, Desid);
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
     protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        string locname = ddlLocation.SelectedValue;
        FillDesignation(locname, getSAPID(ReturnLoginName()));
        ddlEmployee.SelectedValue = "0";
    }
     protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        string locname = ddlLocation.SelectedValue;
        int DesId = Convert.ToInt32(ddlDesignation.SelectedValue);
        FillEmployee(locname, DesId);
    }

     protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
     {
         int checkYearEnd = Dal.CheckYearEnd(ddlEmployee.SelectedValue);
         int checkstatus = Dal.CheckUserStatus(ddlEmployee.SelectedValue);
         int InitiateStatus = Dal.CheckInitiateStatus(ddlEmployee.SelectedValue);
         DataTable ClosedStatus = Dal.CheckClosedStatus(ddlEmployee.SelectedValue);
         if (checkYearEnd != 0 && checkstatus == 0 && InitiateStatus == 0 && Convert.ToString(ClosedStatus.Rows[0]["current_status"]) == "3")
         {
             HFCYCLEID.Value = Dal.GetFpaCyleId(ddlEmployee.SelectedValue);
             DataTable fpaheads = Dal.getFpaClaimHead(ddlEmployee.SelectedValue);
             if (fpaheads.Rows.Count > 0)
             {
                 ddlHead.Items.Clear();
                 ddlHead.AppendDataBoundItems = true;
                 ListItem li = new ListItem("--Select--", "0");
                 ddlHead.Items.Add(li);
                 ddlHead.DataSource = fpaheads;
                 ddlHead.DataTextField = "fpa_head";
                 ddlHead.DataValueField = "fpa_head_id";
                 ddlHead.DataBind();
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('No Fpa Head');CloseRefreshWindow();", true);
             }
         }
         else
         {
             if (checkstatus != 0)
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('You can not make claim untill your FPA is closed by FPA admin .');CloseRefreshWindow();", true);
             }
             else if (checkYearEnd == 0)
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
     protected void ddlHead_SelectedIndexChanged(object sender, EventArgs e)
     {
         lblAnAllocation.Text = Convert.ToString(GetAllocateBalance(ddlEmployee.SelectedValue, ddlHead.SelectedValue));
         lblGlCode.Text = Convert.ToString(GetGlCode(ddlEmployee.SelectedValue, ddlHead.SelectedValue));
         lblCurBalance.Text = Convert.ToString(GetHeadBalance(ddlEmployee.SelectedValue, ddlHead.SelectedValue));
         if (!string.IsNullOrEmpty(lblGlCode.Text))
         {
             btnUpdate.Visible = true;
         }
         else
         {
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Your FPA has not been reinitiated. Please contact FPA admin. ')", true);
             btnUpdate.Visible = false;
         }
     }
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

     protected void btnUpdate_Click(object sender, EventArgs e)
     {
         try
         {
             int RetId = 0;
string  amount="0";
             if ((txtAmount.Text == "") || txtAmount.Text == null)
             {
                 amount = "0";
             }
             else
             {
                 amount= (txtAmount.Text);
             }
             RetId = Dal.UpdateEmployeeTransaction(ddlEmployee.SelectedValue, HFCYCLEID.Value, ddlDesignation.SelectedValue, ddlHead.SelectedValue, amount, spnSerialNo.InnerText);
             if (RetId == 1)
             {
                 
                 string MailTo = Dal.Get_Single_DataByPassingQuery("select email from master_employee_profile where sapid='" + ddlEmployee.SelectedValue + "'");
                 SendMail(txtAmount.Text, ddlHead.SelectedItem.Text, txtDetail.Text, MailTo, spnSerialNo.InnerText);
				 EmptyControl();
                 ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Employee's FPA data changed successfully.');CloseRefreshWindow();", true);
             }
             if (RetId == 2)
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please refresh the page.');", true);
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after sometime.');", true);
             }
         }
         catch (Exception ex)
         {
		      Util.Log("------------Employee Transatcion -- " + ex.Message.ToString(), logfile);
         }
     }

     private void SendMail(string Amount, string Head, string Detail, string MailTo,string SerialNo)
     {
         string newUrl = string.Empty;
         string Subject = string.Empty;
         if (spnBU.InnerText.Trim() == "REM")
             newUrl = "http://epicenter.royalenfield.com/mycorner/fpa/Pages/home.aspx";
         else
             newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
         string Mailbody = "Dear User " + "<br>" + "<br>" + "You  have to Inform That " + "<br>" + "<br>" + "Amount " + Amount + " has been Deducted From Your FPA Head " + Head + "<br>" + "<br>" + "<a href=\"" + newUrl + "\">Click here to go to Processed Claim Page</a>" + "<br>" + "<br>" + "<br>" + "<br>" + "Details :- " + Detail + "<br>" + "<br>" + "Thanks ";
         Subject = "Employee Transaction ...";
         //Util.SendMail(MailTo.Trim(), "ewadmin@eicher.in", "", Subject, Mailbody);
     }

     private void EmptyControl()
     {
         ddlLocation.SelectedValue = "0";
         ddlDesignation.SelectedValue = "0";
         ddlEmployee.SelectedValue = "0";
         ddlHead.SelectedValue = "0";
         lblAnAllocation.Text = "0";
         lblCurBalance.Text = "0";
         lblGlCode.Text = "0";
         txtAmount.Text = "0";
ddlHead.DataBind();
     }
}
