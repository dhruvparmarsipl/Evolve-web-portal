#region"Namespace"
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


public partial class DashBoard : System.Web.UI.Page
{
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    //UtilityNew.Service Util = new UtilityNew.Service();
    string LogFilPath = "D:\\Eicher\\All Logs\\FPA";
#endregion

    #region"PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
		
		    //Util.Log("-- FPA Dashboard Page Load start --", LogFilPath);
			//Util.Log("-- get Login name  ----"+ReturnLoginName(), LogFilPath);
            string loginname=ReturnLoginName();
            //Util.Log("-- get Login name  ----"+loginname, LogFilPath);
            BindApproverGrid(loginname);
            BindUserGrid(loginname);
            BindPetrolApproverGrid(loginname);
            BindUserPetrolGrid(loginname);
			//Util.Log("-- FPA Dashboard Page Load End --", LogFilPath);
        }

    }
    #endregion

    #region"FPA Claim Detail"

    #region"BindApproverGrid"
    public void BindApproverGrid(string loginname)
    {
        try
        {
            using (DataTable DT_Approver = Dal.ApproverDashBoard(loginname))
            {
                if (DT_Approver.Rows.Count == 0)
                {
                    DT_Approver.Rows.Add("No Document");
                    FillGrid(GridView1, DT_Approver);
                }
                else
                {
                    FillGrid(GrdCoachApproval, DT_Approver);
                    ViewState["ApprGrid"] = DT_Approver;
                }
            }
        }
        catch (Exception ex)
        {
		   //Util.Log("-- catch inside BindApproverGrid --" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
           // Util.Log("-- catch inside BindApproverGrid --", LogFilPath);
        }
    }
     #endregion

    #region"BindUserGrid"
    public void BindUserGrid(string loginname)
    {
        try
        {
            using (DataTable DT_User = Dal.getMyProClaimDocs(loginname))
            {
                if (DT_User.Rows.Count == 0)
                {
                    DT_User.Rows.Add("No Document");
                    FillGrid(GridView2, DT_User);
                }
                else
                {
                    FillGrid(grdUser, DT_User);
                    ViewState["UserGrid"] = DT_User;
                }
            }
        }
        catch (Exception ex)
        {
           // Util.Log("--catch inside BindUserGrid--", LogFilPath);
			//Util.Log("--catch inside BindUserGrid--" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
        }
    }
    #endregion

    #region"BindGrid"
    private void FillGrid(GridView grdApprover, DataTable dtSource)
    {
        grdApprover.DataSource = dtSource;
        grdApprover.DataBind();
		//Util.Log("-- Dashboard fillGrid --"+ReturnLoginName(), LogFilPath);
    }
    #endregion

    #region"PageChangingOfApproverGrid"
    protected void GrdCoachApproval_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdCoachApproval.PageIndex = e.NewPageIndex;
        GrdCoachApproval.DataSource = (DataTable)ViewState["ApprGrid"];
        GrdCoachApproval.DataBind();
    }
    #endregion

    #region"PageChangingOfUserGrid"
    protected void grdUser_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        grdUser.DataSource = (DataTable)ViewState["UserGrid"];
        grdUser.DataBind();
    }
    #endregion
    #endregion

    #region"Petrol Claim Detail"

    #region"BindPetrolApproverGrid"
    public void BindPetrolApproverGrid(string loginname)
    {
        try
        {
            using (DataTable DT_Approver = Dal.PetrolApproverDashBoard(loginname))
            {
                if (DT_Approver.Rows.Count == 0)
                {
                    DT_Approver.Rows.Add("No Document");
                    FillGrid(GridView6, DT_Approver);
                }
                else
                {
                    FillGrid(gvCoachPetrolClaim, DT_Approver);
                    ViewState["PetrolApprGrid"] = DT_Approver;
                }
            }
        }
        catch (Exception ex)
        {
           // Util.Log("-- catch inside BindApproverGrid --", LogFilPath);
			 //Util.Log("-- catch inside BindPetrolApproverGrid --" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
        }
    }
     #endregion

    #region"BindUserPetrolGrid"
    public void BindUserPetrolGrid(string loginname)
    {
        try
        {
            using (DataTable DT_User = Dal.getMyPetrolClaimDocs(loginname))
            {
                if (DT_User.Rows.Count == 0)
                {
                    DT_User.Rows.Add("No Document");
                    FillGrid(GridView4, DT_User);
                }
                else
                {
                    FillGrid(gvuserPetrolClaim, DT_User);
                    ViewState["PetrolUserGrid"] = DT_User;
                }
            }
        }
        catch (Exception ex)
        {
            //Util.Log("--catch inside BindUserGrid--", LogFilPath);
			//Util.Log("--catch inside BindPetrolUserGrid--" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
        }
    }
    #endregion

    #region"PageChangingOfUserPetrolGrid"
    protected void gvuserPetrolClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvuserPetrolClaim.PageIndex = e.NewPageIndex;
        gvuserPetrolClaim.DataSource = (DataTable)ViewState["PetrolUserGrid"];
        gvuserPetrolClaim.DataBind();

    }
    #endregion

    #region"PageChangingOfPetrolApproverGrid"
    protected void gvCoachPetrolClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCoachPetrolClaim.PageIndex = e.NewPageIndex;
        gvCoachPetrolClaim.DataSource = (DataTable)ViewState["PetrolApprGrid"];
        gvCoachPetrolClaim.DataBind();
    }
    #endregion
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
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "spstestss";
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
