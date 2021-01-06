using System;
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

public partial class FIAdminApp : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            callonReload();
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        string Srhtxt = Convert.ToString(txtSearch.Text);
        hfSechVal.Value = Srhtxt;
        ViewState["SerVal"] = Srhtxt;
        BindApproverGrid(Srhtxt);
    }

    public void callonReload()
    {
        string srhVal = Convert.ToString(ViewState["SerVal"]);
        if (!string.IsNullOrEmpty(srhVal))
        {
            BindApproverGrid(srhVal);
        }
    }
    public void BindApproverGrid(string srhtxt)
    {
        try
        {
            DataTable DT_Approver = Dal.getDataForFIAdmin(ReturnLoginName(), "", srhtxt);//Submitted to FI Admin
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
        // return "sknair";
        //return "tbetestcoach";
        //return "smtestuser";

    }
    #endregion

    #region"EncryptSerialNumber"
    public string DoEncrypt(string AnyString)
    {
        string Encrypted = string.Empty;
        EncriporDecript.EncriporDecrip oEncryptString = new EncriporDecript.EncriporDecrip();
        Encrypted = oEncryptString.encryptQueryString(AnyString);
        return Encrypted;
    }
    #endregion
   
}
