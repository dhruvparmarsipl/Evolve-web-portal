using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.IO;

public partial class DelegateAssigned : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getDelegateAss();
        }
    }
    public void getDelegateAss()
    {
        try
        {
            string Qry = "SELECT dbo.FIND_ENAME_BYSAPID(EID) ename,EID SAPID FROM [EICHER].[dbo].[MASTER_DELEGATE] where [LIMIT]='" + getSAPID()+ "'";
            DataTable DT_User = Dal.Get_DataByPassingQuery(Qry);
            if (DT_User.Rows.Count == 0)
            {
                DT_User.Rows.Add("No Document");
                FillGrid(GridView2, DT_User);
                grdUser.Visible = false;
                GridView2.Visible = true;
            }
            else
            {
                FillGrid(grdUser, DT_User);
                grdUser.Visible = true;
                GridView2.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public string getSAPID()
    {
        string SAPID = Dal.Get_Single_DataByPassingQuery("SELECT SAPID FROM MASTER_EMPLOYEE_PROFILE WHERE LOGIN_NAME ='" + ReturnLoginName() + "'");
        return SAPID;
    }

    private void FillGrid(GridView grdView, DataTable dtSource)
    {
        grdView.DataSource = dtSource;
        grdView.DataBind();
    }

    protected void grdUser_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        grdUser.DataBind();
    }


    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "rvarghese";
    }
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
}
