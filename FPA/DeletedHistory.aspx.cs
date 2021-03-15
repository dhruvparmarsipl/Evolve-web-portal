using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.IO;
using System.Drawing;

public partial class DeletedHistory : System.Web.UI.Page
{
    DataTable dt;
    FPADALL Dal = new FPADALL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGridview();
        }

    }

    private void FillGridview()
    {
        dt = Dal.GetFiAdminDeleteHistory(getSAPID(ReturnLoginName()));
        if (dt.Rows.Count > 0)
        {
            btnExporttoExcel.Visible = true;
            gvDeleteHistory.DataSource = dt;
            ViewState["dt"] = dt;
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
        //return "zzmektare";
        //return "Tbetestuser";
    }
    #endregion

    protected void btnExporttoExcel_Click(object sender, EventArgs e)
    {
        DataTable DT_ExportReport = (DataTable)ViewState["dt"];
        DT_ExportReport.Columns.Remove("AppType");
        GenereateExcel(DT_ExportReport);
    }

    public void GenereateExcel(DataTable dt)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=FPA_Deleted_Claim_Report.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        GridView gridView = new GridView();
        gridView.DataSource = dt;
        gridView.DataBind();
        string cols = gridView.Rows[0].Cells.Count.ToString();
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridView.HeaderRow.BackColor = Color.White;

            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell2 = new TableCell();

            HeaderCell2.Text = "FPA deleted claim report ";
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
                // cell.BackColor = gridView.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gridView.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        //cell.BackColor = gridView.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        //cell.BackColor = gridView.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gridView.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();


        }
    }
}