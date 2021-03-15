#region"NameSpace"
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
#endregion
public partial class ProcessPetrolClaim : System.Web.UI.Page
{
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    #endregion

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string qSAP = "Select SAPID from master_employee_profile where login_name ='" + ReturnLoginName() + "'";
            hfUserSAP.Value = Dal.Get_Single_DataByPassingQuery(qSAP).Trim();
            bindDDlYear();
            ddlYear_SelectedIndexChanged(null, null);
        }
    }
    #endregion

    #region"FillYearDropdown
    public void bindDDlYear()
    {
        string Qry = "select distinct(datepart(year, CLAIM_CREATION_DATE)) as YEAR from fpa_petrol_claims where EMP_ID = '" + hfUserSAP.Value.ToString() + "' and current_status ='3' order by Year desc";
        // string Qry = "select distinct(datepart(year, NL_Exp_main_Created_ON)) as YEAR from NON_LIMIT_EXPENSE_MAIN where CREATEDBY = '" + ReturnLoginName() + "' order by Year desc";
        DataTable DT_Year = Dal.Get_DataByPassingQuery(Qry);
        if (DT_Year.Rows.Count == 0)
        {
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
        }
        else
        {
            ddlYear.DataSource = DT_Year;
            ddlYear.DataTextField = "YEAR";
            ddlYear.DataValueField = "YEAR";
            ddlYear.DataBind();
        }
    }
    #endregion

    #region"BindProcessedClaimGridByYearChanges"
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string DatabyYear = ddlYear.SelectedValue;
        BindClaims(DatabyYear);
    }
    #endregion

    #region"BindProcessedClaimGridByYear"
    public void BindClaims(string YR)
    {
        try
        {
            DataSet DT_User = Dal.GetProcessCalim(hfUserSAP.Value, YR);

            if (DT_User.Tables[1].Rows.Count == 0)
            {
                DT_User.Tables[1].Rows.Add("No Document");
                FillGrid(GridView4, DT_User.Tables[1]);
                gvuserPetrolClaim.Visible = false;
                GridView4.Visible = true;
            }
            else
            {
                FillGrid(gvuserPetrolClaim, DT_User.Tables[1]);
				ViewState["DATA"] =DT_User.Tables[1];
                gvuserPetrolClaim.Visible = true;
                GridView4.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region"FillClaimGrid"
    private void FillGrid(GridView grdView, DataTable dtSource)
    {
        grdView.DataSource = dtSource;
        grdView.DataBind();
    }
    #endregion

    #region"PageChangingOfUserPetrolGrid"
    protected void gvuserPetrolClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvuserPetrolClaim.PageIndex = e.NewPageIndex;
		gvuserPetrolClaim.DataSource = (DataTable)ViewState["DATA"];
        gvuserPetrolClaim.DataBind();

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
        //return "Tbetestuser";
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
