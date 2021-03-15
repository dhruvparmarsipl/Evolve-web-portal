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
using System.Xml.Linq;
using Microsoft.SharePoint;
using System.Drawing;
using System.IO;
public partial class FpaInitiateStatus : System.Web.UI.Page
{
    FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    string LogFilPath = "D:\\Eicher\\All Logs\\FPA";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string SapId = getSAPID(ReturnLoginName());
            FillHeader(SapId);
            FillBu(SapId);
        }
    }

    private void FillBu(string SapId)
    {
        DataTable dt = Dal.Get_DataByPassingQuery("SELECT distinct BU FROM master_designation WHERE Fpa_admin_sapid ='" + SapId + "'");
        if (dt.Rows.Count > 0)
        {
            ddlBu.Items.Clear();
            ddlBu.AppendDataBoundItems = true;
            ddlBu.DataTextField = "BU";
            ddlBu.DataValueField = "BU";
            ddlBu.DataSource = dt;
            ddlBu.DataBind();
        }
    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getclaimUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            //spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
        }
    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dt = Dal.GetFpaInitationStatus(ddlStatus.SelectedValue.ToString(),ddlBu.SelectedValue.ToString(), getSAPID(ReturnLoginName())))
            {
                if (dt.Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    gvEmployeeList.DataSource = dt;
                    gvEmployeeList.DataBind();
                }
                else {
                    btnExport.Visible = false;
                    gvEmployeeList.DataBind(); }
            }

        }
        catch (Exception ex)
        {
            Util.Log("-- catch inside Initiation --" + ex.Message.ToString() + "--" + ReturnLoginName(), LogFilPath);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string columns = gvEmployeeList.Rows[0].Cells.Count.ToString();
        GenereateExcel(gvEmployeeList, columns);
    }
    public void GenereateExcel(GridView gridView, string cols)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=FpaReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridView.HeaderRow.BackColor = Color.White;
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gridView.AlternatingRowStyle.BackColor = Color.Maroon;
            gridView.RowStyle.BackColor = Color.MediumBlue;
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = " FPA Initiation Staus Report";
            HeaderCell2.ColumnSpan = Convert.ToInt32(cols);
            HeaderCell2.ForeColor = Color.White;
            HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell2.Height = 20;
            HeaderCell2.Font.Size = 14;
            HeaderCell2.BackColor = Color.Gray;
            HeaderRow.Cells.Add(HeaderCell2);
            gridView.Controls[0].Controls.AddAt(0, HeaderRow);
            foreach (TableCell cell in gridView.HeaderRow.Cells)
            {
                cell.BackColor = gridView.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gridView.Rows)
            {
                row.BackColor = Color.White;
            }
            gridView.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
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
        //return "pspandey";
    }
}
