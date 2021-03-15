using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;

public partial class DelegatorStatus : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindApproverGrid();
        }
    }

    public void BindApproverGrid()
    {
        try
        {
            string QryApp = "select FC.APP_SERIAL_NO, dbo.FIND_ENAME_BYSAPID(FC.EMP_ID)as NAME,FC.FPA_CLAIM_MAIN_ID as ID,Convert(varchar(11),FC.CLAIM_CREATION_DATE,106) as CreationDate," +
            "Sum(CLAIM_AMT) As Amount,FS.FPA_STATUS_NAME  as Status,MCC.ColorCode as Color from dbo.FPA_CLAIM_MAIN as FC inner join " +
            "dbo.Master_Fpa_Color as MCC on MCC.ColorStatus = FC.CLAIM_STATUS inner join dbo.FPA_CLAIM_DETAIL as FD " +
            "on FD.APP_SERIAL_NO = FC.APP_SERIAL_NO  inner join MASTER_FPA_STATUS FS ON FC.CLAIM_STATUS=FS.FPA_ID  where FC.DelegateLogin = '" + ReturnLoginName() + "' " +
            "group by FC.EMP_ID,FC.CLAIM_CREATION_DATE,FS.FPA_STATUS_NAME,MCC.ColorCode,FC.FPA_CLAIM_MAIN_ID,FC.APP_SERIAL_NO  order by FC.CLAIM_CREATION_DATE desc";
            DataTable DT_Approver = Dal.Get_DataByPassingQuery(QryApp);
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
        catch (Exception ex)
        {
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
        GrdCoachApproval.DataSource = (DataTable)ViewState["ApprGrid"];
        GrdCoachApproval.DataBind();
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
