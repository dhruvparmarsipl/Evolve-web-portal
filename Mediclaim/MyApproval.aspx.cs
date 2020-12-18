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

public partial class MyApproval : System.Web.UI.Page
{
    #region PAGE EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindApproverGrid();
        }
    }
    protected void grdUser_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdApproval.PageIndex = e.NewPageIndex;
        //GrdApproval.DataBind();
        BindApproverGrid();
    }
    #endregion
    #region PRIVATE METHODS
    private void FillGrid(GridView ARDataGrid, DataTable dtSource)
    {
        ARDataGrid.DataSource = dtSource;
        ARDataGrid.DataBind();
    }
    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "spstestss";
        //return "spstesttrg";
        //--return "TestApprover";
        //return "testssBoss";
        //return "svhanda";
    }
     #endregion
    #region PUBLIC METHODS
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    public string DoEncrypt(string AnyString)
    {
        string Encrypted = string.Empty;
        EncriporDecript.EncriporDecrip oEncryptString = new EncriporDecript.EncriporDecrip();
        Encrypted = oEncryptString.encryptQueryString(AnyString);
        return Encrypted;
    }
    
    public void BindApproverGrid()
    {
        MediMain oclsMM = new MediMain();
        CommonFunction oCF = new CommonFunction();
        DataTable dtApprover = new DataTable();
        dtApprover = oclsMM.GetApproverActiveMediclaim(oCF.GetSapIdByLoginName(ReturnLoginName()));
        if (dtApprover.Rows.Count == 0)
        {
            dtApprover.Rows.Add("No Records Found.");
            FillGrid(GridView1, dtApprover);
        }
        else
        {
            FillGrid(GrdApproval, dtApprover);
        }
    }
    #endregion
}
