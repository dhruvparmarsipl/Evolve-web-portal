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

public partial class ApproverDashboard : System.Web.UI.Page
{ 
     MediMain oclsMM = new MediMain();
    #region PAGE EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUserGrid();
			GetEmployeeType();
        }
    }
    public void GetEmployeeType()
    {
        string EMPTYPE = string.Empty;
        EMPTYPE = oclsMM.GetEmptype(ReturnLoginName());
        if (EMPTYPE == "S")
        {
            Premium_Table.NavigateUrl = "http://epicenter.vecv.net/mycorner/mediclaim/Shared%20Documents/Staff_Plan_2017.pdf";
        }
        else
        {
            Premium_Table.NavigateUrl = "http://epicenter.vecv.net/mycorner/mediclaim/Shared%20Documents/Exe_Plan_2017.pdf";
        }
    }
    protected void GrdApproval_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        //grdUser.DataBind();
        BindUserGrid();
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
        // return "spstesttrg";
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
    public void BindUserGrid()
    {
        DataTable dtUser = new DataTable();
        MediMain oclsMM = new MediMain();
        CommonFunction oCF = new CommonFunction();
        dtUser = oclsMM.GetUsersActiveMediclaim(oCF.GetSapIdByLoginName(ReturnLoginName()));
        if (dtUser.Rows.Count == 0)
        {
            dtUser.Rows.Add("No Records Found.");
            FillGrid(GridView2, dtUser);
        }
        else
        {
            FillGrid(grdUser, dtUser);
        }
    }
   
    #endregion
}
