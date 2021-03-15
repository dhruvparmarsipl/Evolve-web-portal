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
using System.Data.SqlClient;
using System.IO;
using Microsoft.SharePoint;

public partial class MyFPAHistory : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
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

    #region"FillYearDropdown
    public void bindDDlYear()
    {
        string Qry = "select distinct(datepart(year, a.CYCLE_CREATION_DATE)) as YEAR from fpa_cycle a inner join fpa_main b on a.fpa_id =b.fpa_id where b.EMP_ID = '" + hfUserSAP.Value.ToString() + "' and CURRENT_STATUS ='3' order by Year desc";
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
       // return "tbetestuser";
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

    #region"Status"
    protected string GetStatus(object status)
    {
        string cstatus = string.Empty;
        if (status.ToString() == "0")
        {
            cstatus = "Initiate By FPA ";
        }
        if (status.ToString() == "1")
        {
            cstatus = "Save as Draft";
        }
        if (status.ToString() == "3")
        {
            cstatus = "Rejected By Approver";
        }
        if (status.ToString() == "4")
        {
            cstatus = "Submitted to FPA Admin";
        }
        if (status.ToString() == "5")
        {
            cstatus = "Rejected by FPA Admin";
        }
        if (status.ToString() == "8")
        {
            cstatus = "Submitted to FI Admin";
        }
        if (status.ToString() == "9")
        {
            cstatus = "Rejected by FIAdmin";
        }
        if (status.ToString() == "10")
        {
            cstatus = "Close";
        }
        return cstatus;
    }
    #endregion
   
    #region"PageChangesOfProcessedClaim"
    protected void grdUser_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        grdUser.DataBind();
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
            DataTable DT_User = Dal.GetClosedProcessCalim(hfUserSAP.Value, YR);
            if (DT_User.Rows.Count == 0)
            {
                grdUser.DataBind();
            }
            else
            {
                FillGrid(grdUser, DT_User);       
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
}
