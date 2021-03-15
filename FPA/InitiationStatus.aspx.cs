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


public partial class InitiationStatus : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    string LogFilPath = "D:\\Eicher\\All Logs\\FPA";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillHeader(getSAPID(ReturnLoginName()));
            fillYear();
            //FillStatus();
           // FillGrid();
        }
    }
    private void FillStatus()
    {
        using (DataTable _statusdt = Dal.FillStatus())
        {
            if (_statusdt.Rows.Count > 0)
            {
                ddlStatus.Items.Clear();
                ddlStatus.AppendDataBoundItems = true;
                ddlStatus.DataTextField = "FPA_STATUS_NAME";
                ddlStatus.DataValueField = "FPA_ID";
                ddlStatus.DataSource = _statusdt;
                ddlStatus.DataBind();
            }
        }
    }
    private void fillYear()
    {
        for (int i = System.DateTime.Now.Year; i>= 2009; i--)
        {
            ddlYear.Items.Add(i.ToString());
        }

        //ddlYear.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getclaimUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFDESID.Value = Convert.ToString(dt_UInfo.Rows[0]["des_id"]);

        }  
    }
    public string[] RetrieveSplitValue(string s)
    {
        string[] str;
        char[] MyChar = { '\\' };
        str = s.Split(MyChar);
        return str;
    }
    public string getSAPID(string Gid)
    {
        string SAPID = Dal.Get_Single_DataByPassingQuery("SELECT SAPID FROM MASTER_EMPLOYEE_PROFILE WHERE LOGIN_NAME ='" + Gid.Trim() + "'");
        return SAPID;
    }
    private string ReturnLoginName()
    {
        string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
       string[] Result = RetrieveSplitValue(loginname);
       return Result[1].ToLower();
       // return "drpillai";
    }
    #region"GetFormStatus"
    protected object GetStatus(object status)
    {
        string cstatus = string.Empty;
        if (status.ToString() == "0")
        {
            cstatus= "Draft ";
        }
        if (status.ToString() == "1")
        {
            cstatus = "Save as Draft";
        }
        if (status.ToString() == "2")
        {
            cstatus = "Submitted to FI Admin";
        }
        if (status.ToString() == "3")
        {
            cstatus = "Close";
        }
        if (status.ToString() == "4")
        {
            cstatus = "Submitted to FPA Admin";
        }
        if (status.ToString() == "5")
        {
            cstatus = "Rejected by FPA Admin";
        }
        if (status.ToString() == "6")
        {
            cstatus = "Approve By FPA Admin";
        }
        if (status.ToString() == "7")
        {
            cstatus = "Rejected by FIAdmin";
        }

        return cstatus;
    }
    #endregion
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dt = Dal.GetFpaEmployeeStatusDetail(ddlYear.SelectedValue.ToString(), ddlStatus.SelectedValue.ToString(), spnLoc.InnerText,HFDESID.Value,getSAPID(ReturnLoginName())))
            {
                if (dt.Rows.Count > 0)
                {
                    gvEmployeeList.DataSource = dt;
                    gvEmployeeList.DataBind();
                }
                else { gvEmployeeList.DataBind(); }
            }

        }
        catch( Exception ex)
        {
		  Util.Log("-- catch inside Initiation --" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        if (gvEmployeeList.Rows.Count > 0)
        {
            for (int i = 1; i < 2; i++)
            {
                TableHeaderCell cell = new TableHeaderCell();
                TextBox txtSearch = new TextBox();
                txtSearch.Attributes["placeholder"] = gvEmployeeList.Columns[i].HeaderText;
                txtSearch.CssClass = "search_textbox";
                cell.Controls.Add(txtSearch);
                row.Controls.Add(cell);
            }
            gvEmployeeList.HeaderRow.Parent.Controls.AddAt(1, row);
        }

    }
}
