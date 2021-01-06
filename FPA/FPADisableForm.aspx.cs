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

public partial class FPADisableForm : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
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
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
           
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
        //return "spstesttrg";
    }
    private void FillLocation(string sapid)
    {
        ddlLocation.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlLocation.Items.Add(li);
        ddlLocation.AppendDataBoundItems = true;
        DataTable dt_Loc = Dal.getLocation(sapid);
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
        DataTable dt_Des = Dal.getDesignation(locname, Sapid);
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
        DataTable dt_Emp = Dal.getFPAEmployeeDisable(locname, Desid);
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
        string Empsapid = ddlEmployee.SelectedValue;
        DataTable dtecode = Dal.getEmployeeEcode(Empsapid);
        DataTable dt = (DataTable)ViewState["dt_Emp"];

        if (dtecode.Rows.Count > 0)
        {
            spnECode.InnerText = Convert.ToString(dtecode.Rows[0]["ECODE"].ToString());
            spcarscheme.InnerText = Convert.ToString(dtecode.Rows[0]["CAR_SCHEME"].ToString());
            //DataRow[] drsel = dt.Select("sapid='" + Empsapid + "'");
            //HFEMAILS.Value = drsel[0]["email"].ToString();
        }
        else
        {
            spnECode.InnerText = "";
            spcarscheme.InnerText = "";
        }
    }
    protected void btnDisable_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedValue == "0" || ddlDesignation.SelectedValue == "0" || ddlEmployee.SelectedValue == "0")
            {
                if (ddlLocation.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Location.');", true);
                }
                else if (ddlDesignation.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Designation);", true);
                }
                else if (ddlEmployee.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Employee.');", true);
                }

            }
            else
            {
                int TransID = 0;
                TransID = Dal.InsertFpaDisable(ddlEmployee.SelectedValue);
                if (TransID == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your Emploee Disable successfully.');CloseRefreshWindow();", true);
                }
                else if (TransID == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                }
                
                else if (TransID == 5)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data Already Submitted.');CloseRefreshWindow();", true);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}
