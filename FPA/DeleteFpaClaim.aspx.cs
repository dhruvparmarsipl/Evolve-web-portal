using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
public partial class DeleteFpaClaim : System.Web.UI.Page
{ 
    #region"GlobalVariable"
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    static string logfile = "D:\\Eicher\\All Logs\\FPA";
    DataTable dt;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            spnLoginUser.InnerText = Dal.Get_Single_DataByPassingQuery("select ename from master_employee_profile where login_name='" + ReturnLoginName()+ "'"); 
            FillGridview();
        }
    }
    private void FillGridview()
    {
        string Qyery="select  FPM.APP_SERIAL_NO  as SerialNo ,FPM.FPA_CLAIM_MAIN_ID as ID,Convert(varchar(11),FPM.CLAIM_CREATION_DATE,106) as CreationDate,"+
         " Sum(CLAIM_AMT) As Amount, FPM.EMP_Id as 'SapId',CLAIM_STATUS,BU,(select FPA_STATUS_NAME from  MASTER_FPA_STATUS where FPA_ID=  FPM.CLAIM_STATUS) as Status from dbo.FPA_CLAIM_MAIN as FPM " +                                 
         "left join dbo.FPA_CLAIM_DETAIL as FPD  on FPD.APP_SERIAL_NO = FPM.APP_SERIAL_NO"+                 
         " where (FPM.CREATED_BY = '" + ReturnLoginName() + "' or  FPM.DelegateLogin='"+ReturnLoginName()+"')  and FPM.CLAIM_STATUS  in('1' ,'7') group by FPM.EMP_ID,FPM.CLAIM_CREATION_DATE,FPM.CLAIM_STATUS,FPM.FPA_CLAIM_MAIN_ID ,BU,FPM.APP_SERIAL_NO "+      
         " order by FPM.FPA_CLAIM_MAIN_ID desc  ";
        dt = Dal.Get_DataByPassingQuery(Qyery);
        if (dt.Rows.Count > 0)
        {
            gvDeleteHistory.DataSource = dt;
            ViewState["dt"] = dt;
            gvDeleteHistory.DataBind();
        }
        else
        {
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
        //return "mkrajput";
        //return "Tbetestuser";
    }
    #endregion
    #region "Delete Claim"
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string SerialNo = (sender as LinkButton).CommandArgument;
        try
        {
           
            LinkButton lnk = (sender as LinkButton);
            GridViewRow gvrow = (GridViewRow)lnk.NamingContainer;
            Label lblSapid = (Label)gvrow.FindControl("lblSapid");
            Label lblStatus = (Label)gvrow.FindControl("lblStatus");
            string UserType = string.Empty;
            string Comment = string.Empty;
            int DelId = 0;
            if (lblStatus.Text == "1" || lblStatus.Text == "7" )
            {
                UserType = "User";
                Comment = "Deleted by User";
                DelId = Dal.DeleteFpaClaim(SerialNo, "0");
                if (DelId == 1)
                {
                    int RetId = Dal.DeleteTravelLogHistory(SerialNo, lblSapid.Text, Comment, "FPA", UserType, lblSapid.Text);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('Your claim deleted successfully');CloseRefreshWindow();", true);
                    FillGridview();
                }
                else
                {
                    Util.Log("Sqlserver return exception -- " + SerialNo.ToString(), logfile);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Please try after sometime.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Check", "alert('You can not delete this claim .');", true);
            }

        }
        catch (Exception ex)
        {
            Util.Log("catch exception -- " + SerialNo.ToString(), logfile);
            ScriptManager.RegisterStartupScript(this, GetType(), "Delete", "alert('Please try after sometime.');", true);
        }
    }
    #endregion
}