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

public partial class ProcessedMediClaim : System.Web.UI.Page
{
    #region"GlobalVariable"
    MediMain Dal = new MediMain();
    #endregion

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // string qSAP = "Select SAPID from master_employee_profile where login_name ='" + ReturnLoginName() + "'";
            hfUserSAP.Value = getSAPID(ReturnLoginName());
            BindClaims();
        }
    }
    #endregion

    

   
    #region"BindProcessedClaimGridByYear"
    public void BindClaims()
    {
        try
        {
            DataTable DT_User = Dal.GetProcessCalim(hfUserSAP.Value);
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
    #endregion

    #region"FillClaimGrid"
    private void FillGrid(GridView grdView, DataTable dtSource)
    {ViewState["dtSource"]=dtSource;
        grdView.DataSource = dtSource;
        grdView.DataBind();
    }
    #endregion

    #region"PageChangesOfProcessedClaim"
    protected void grdUser_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
		 grdUser.DataSource = (DataTable)ViewState["dtSource"];
        grdUser.DataBind();
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
        string SAPID = Dal.Get_Single_DataByPassingQuery(Gid);
        return SAPID;
    }
    #endregion

    #region"GetUserLoginName"
    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "spstestss";
        // return "svhanda";
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
