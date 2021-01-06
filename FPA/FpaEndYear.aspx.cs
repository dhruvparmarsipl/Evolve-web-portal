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
public partial class FpaEndYear : System.Web.UI.Page
{
    #region Variable
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillHeader(getSAPID(ReturnLoginName()));
            FillLocation(getSAPID(ReturnLoginName()));

        }
    }
    #endregion

    #region CommonMethod
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
            ddlLocation.DataBind();
        }

    }
    #endregion

    #region Location
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt_Des = Dal.getDesignation(ddlLocation.SelectedValue, getSAPID(ReturnLoginName()));
        if (dt_Des.Rows.Count > 0)
        {
            gvDesignation.DataSource = dt_Des;
            gvDesignation.DataBind();
        }
        else
        {
        }
    }
    #endregion

    #region EndYear
    protected void btnEndYear_Click(object sender, EventArgs e)
    {
        try
        {
            string desId = string.Empty;
            foreach (GridViewRow gvrow in gvDesignation.Rows)
            {
                Label DesId = (Label)gvrow.FindControl("lblDesId");
                CheckBox chkbox = (CheckBox)gvrow.FindControl("chkdesg");
                if (chkbox.Checked == true)
                {
                     desId = DesId.Text + "," + desId;
                }
            }
            desId = desId.TrimEnd(',');
            if (desId != null)
            {
                 Dal.FPA_Insert_year_end(desId, ddlLocation.SelectedValue,ReturnLoginName());
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert(' FPA Year End is updated Successfully .');", true);
            }
        }
        catch (Exception ex)
        { }

    }
    #endregion
}
