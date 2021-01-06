using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

public partial class DeletedHistory : System.Web.UI.Page
{
    DataTable dt;
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGridview();
        }

    }

    private void FillGridview()
    {
        dt = Dal.GetFiAdminDeleteHistory(getSAPID(ReturnLoginName()));
        if (dt.Rows.Count > 0)
        {
            gvDeleteHistory.DataSource = dt;
            ViewState["dt"] = dt;
            gvDeleteHistory.DataBind();
        }
    }
    protected void gvDeleteHistory_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvDeleteHistory.PageIndex = e.NewPageIndex;
        gvDeleteHistory.DataSource = (DataTable)ViewState["dt"];
        gvDeleteHistory.DataBind();
    }
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
        //return "zzmektare";
        //return "Tbetestuser";
    }
    #endregion
}