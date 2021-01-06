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
using System.Drawing;
using System.IO;
using Microsoft.SharePoint;

public partial class MyHeadClaim : System.Web.UI.Page
{
    #region"GlaobalVariable"
   FPADALL Dal = new FPADALL();
    UtilityNew.Service Util = new UtilityNew.Service();
    static string logfile = "D:\\Eicher\\All Logs\\FPA";
    int Totalamount = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bindmastergrid();
        }
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
       //return "trgrover";
    }
    #endregion

    private void Bindmastergrid()
    {
        using (DataTable dt = Dal.GetHeadClaimData(getSAPID(ReturnLoginName())))
        {
            if (dt.Rows.Count > 0)
            {
                FillGrid(gvMaster, dt);
                Label lblTotal = (Label)gvMaster.FooterRow.FindControl("lblTotal");
                lblTotal.Text =Convert.ToString(ViewState["Totalamount"]);
            }
            else
            {
                gvMaster.DataBind();
            }
        }
    }

    private void FillGrid(GridView gvMaster, DataTable dt)
    {
        gvMaster.DataSource = dt;
        gvMaster.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string HeadId = gvMaster.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvOrders = e.Row.FindControl("gvDetal") as GridView;
            using (DataTable dt = Dal.FilterDataByHead(HeadId, getSAPID(ReturnLoginName())))
            {
                if (dt.Rows.Count > 0)
                {
                   
                    FillGrid(gvOrders,dt);
                    object sumObject;
                    sumObject = dt.Compute("Sum(claim_amt)", "");
                    Label lblAlCliam = (Label)gvOrders.FooterRow.FindControl("lblAlCliam");
                    lblAlCliam.Text = sumObject.ToString();
                    Totalamount =Totalamount+Convert.ToInt32(sumObject);
                    ViewState["Totalamount"] = Totalamount;
                }
                else
                {
                    gvOrders.DataBind();
                }
            }
        }
         
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        GridView gvnew = new GridView();
        gvnew.ShowFooter = true;
        gvnew.AutoGenerateColumns = true;
        TemplateField tf = null;
        tf = new TemplateField();
        gvnew.EmptyDataText = "No Reocrd Found...";
        using (DataTable dt = Dal.GetReport(getSAPID(ReturnLoginName())))
        {
            if (dt.Rows.Count > 0)
            {
                FillGrid(gvnew, dt);
                string columns = gvnew.Rows[0].Cells.Count.ToString();
                GroupRowsByShift(gvnew);
                GenereateExcel(gvnew, columns);
            }
            else
            {
                gvnew.DataBind();
            }
        }
        
    }
    public void GroupRowsByShift(GridView gridView)
    {
        try
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                //for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                //{
                if (row.Cells[0].Text == previousRow.Cells[0].Text)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 : previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }
                // break;
                //}
            }
        }
        catch (Exception ex)
        { }
    }



    public void GenereateExcel(GridView gridView,string cols)
    {


        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=abc.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridView.HeaderRow.BackColor = Color.White;

            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = "Report For FPA Claim Head Wise Summary";
            HeaderCell2.ColumnSpan =Convert.ToInt32( cols);
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
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gridView.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gridView.RowStyle.BackColor;
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    
}
