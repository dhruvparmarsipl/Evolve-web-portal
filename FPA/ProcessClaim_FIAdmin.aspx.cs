﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

public partial class ProcessClaim_FIAdmin : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindYear();
        }
    }

    public void bindYear()
    {
        string  Cyear = System.DateTime.Now.Year.ToString();
        int yer = Convert.ToInt32(Cyear);
        for (int i = 0; i < 5; i++)
        {
            int j = yer - i;
            ddlYear.Items.Insert(i, new ListItem(Convert.ToString(j),Convert.ToString(j)));
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        string Srhtxt = Convert.ToString(txtSearch.Text).Trim();
        BindApproverGrid(ddlSearchCriteria.SelectedValue.ToString(),Srhtxt);
    }
    
    public void BindApproverGrid(string SerchType,string srhtxt)
    {
        try
        {   
            string ForYear = Convert.ToString(ddlYear.SelectedValue);
            DataTable DT_Approver = Dal.getClosedDataForFIAdmin(ReturnLoginName(),SerchType,srhtxt, ForYear);//Submitted to FI Admin

            if (DT_Approver.Rows.Count == 0)
            {
                DT_Approver.Rows.Add("No Document");
                FillGrid(GridView2, DT_Approver);
                GridView2.Visible = true;
                GrdCoachApproval.Visible = false;
            }
            else
            {
                FillGrid(GrdCoachApproval, DT_Approver);
                GridView2.Visible = false;
                GrdCoachApproval.Visible = true;
                ViewState["Users"] = DT_Approver;
            }
ScriptManager.RegisterStartupScript(this, GetType(), "abc", "Callonloaddata();", true);
        }
        catch (Exception ex)
        {
            //Util.Log("-- catch inside BindApproverGrid --", LogFilPath);
        }
    }

    private void FillGrid(GridView grdApprover, DataTable dtSource)
    {
        grdApprover.DataSource = dtSource;
        grdApprover.DataBind();
    }

    protected void GrdCoachApproval_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdCoachApproval.PageIndex = e.NewPageIndex;
        GrdCoachApproval.DataSource = ((DataTable)ViewState["Users"]);
        GrdCoachApproval.DataBind();
    }
    public string DoEncrypt(string AnyString)
    {
        string Encrypted = string.Empty;
        EncriporDecript.EncriporDecrip oEncryptString = new EncriporDecript.EncriporDecrip();
        Encrypted = oEncryptString.encryptQueryString(AnyString);
        return Encrypted;
    }

    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        string[] Result = RetrieveSplitValue(loginname);
        return Result[1].ToLower();
        //return "zzmektare";
    }

    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }



}
