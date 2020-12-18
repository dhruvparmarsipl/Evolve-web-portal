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
    MediMain objMedi = new MediMain();
    CommonFunction oclsCF = new CommonFunction();
    Utility.Service Util = new Utility.Service();
    int Totalamount = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillBU(oclsCF.GetSapIdByLoginName(ReturnLoginName()));
            FillYear();
            FillStatus();
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

    

    #region"GetUserLoginName"
    private string ReturnLoginName()
    {
        //string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        //string[] Result = RetrieveSplitValue(loginname);
        //return Result[1].ToLower();
        return "molly";
    }
    #endregion
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        Bindmastergrid(ddlBu.SelectedValue,ddlYear.SelectedValue,ddlStatus.SelectedValue);
    }

    private void FillBU(string sapid)
    {
        ddlBu.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlBu.Items.Add(li);
        ddlBu.AppendDataBoundItems = true;
        DataTable dt_Bu = objMedi.getBU(sapid);
        if (dt_Bu.Rows.Count > 0)
        {
            ddlBu.DataSource = dt_Bu;
            ddlBu.DataTextField = "bu";
            ddlBu.DataValueField = "bu";
            ddlBu.DataBind();

        }       
    }
    private void FillYear()
    {
        for (int i = 2014; i <= System.DateTime.Now.Year; i++)
        {
            ddlYear.Items.Add(i.ToString());
        }

        ddlYear.Items.Insert(0, new ListItem("Select", "0"));
       
    }
    private void FillStatus()
    {
        ddlStatus.Items.Clear();
        ListItem li = new ListItem("--Select--", "0");
        ddlStatus.Items.Add(li);
        ddlStatus.AppendDataBoundItems = true;
        DataTable dt_status = objMedi.getStatus();
        if (dt_status.Rows.Count > 0)
        {
            ddlStatus.DataSource = dt_status;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataBind();

        }  
    }
    private void Bindmastergrid(string Bu,string Year,string Status)
    {
        using (DataTable dt = objMedi.GetHeadClaimData(oclsCF.GetSapIdByLoginName(ReturnLoginName()), Bu, Year, Status))
        {
            if (dt.Rows.Count > 0)
            {
                btnReport.Visible = true;
                FillGrid(gvMaster, dt);
                Label lblTotal = (Label)gvMaster.FooterRow.FindControl("lblTotal");
                object sumObject;

                sumObject = Convert.ToInt32(dt.Compute("Sum(ActualPremium)", "[ActualPremium] > 0"));
                lblTotal.Text = Convert.ToString(sumObject);
                ViewState["Total"] = lblTotal.Text;
            }
            else
            {
                btnReport.Visible = false;
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
            string Id = gvMaster.DataKeys[e.Row.RowIndex].Values[0].ToString();
            string SapidId = gvMaster.DataKeys[e.Row.RowIndex].Values[1].ToString();
            GridView gvOrders = e.Row.FindControl("gvDetal") as GridView;
            using (DataTable dt = objMedi.FilterDataByHead(Id, SapidId))
            {
                if (dt.Rows.Count > 0)
                {
                   
                    FillGrid(gvOrders,dt);
                    //object sumObject;
                    //sumObject = dt.Compute("Sum(claim_amt)", "");
                    //Label lblAlCliam = (Label)gvOrders.FooterRow.FindControl("lblAlCliam");
                    //lblAlCliam.Text = sumObject.ToString();
                    //Totalamount =Totalamount+Convert.ToInt32(sumObject);
                    //ViewState["Totalamount"] = Totalamount;
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
        using (DataTable dt = objMedi.GetReport(oclsCF.GetSapIdByLoginName(ReturnLoginName()), ddlBu.SelectedValue, ddlYear.SelectedValue, ddlStatus.SelectedValue))
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

                for (int cellIndex = 0; cellIndex < 15; cellIndex++)
                {
                    if (row.Cells[cellIndex].Text == previousRow.Cells[cellIndex].Text)
                    {
                        row.Cells[cellIndex].RowSpan = previousRow.Cells[cellIndex].RowSpan < 2 ? 2 : previousRow.Cells[cellIndex].RowSpan + 1;
                        previousRow.Cells[cellIndex].Visible = false;
                        if ((row.Cells[10].Text == previousRow.Cells[10].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                             //row.Cells[4].RowSpan = 1;
                            // previousRow.Cells[4].Visible = false;
                        }
                        if ((row.Cells[11].Text == previousRow.Cells[11].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        if ((row.Cells[9].Text == previousRow.Cells[9].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        if ((row.Cells[5].Text == previousRow.Cells[5].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        if ((row.Cells[12].Text == previousRow.Cells[12].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        if ((row.Cells[14].Text == previousRow.Cells[14].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        if ((row.Cells[8].Text == previousRow.Cells[8].Text) && (row.Cells[3].Text == previousRow.Cells[3].Text))
                        {
                        }
                        else
                        {
                            row.Cells[8].RowSpan = 1;
                            previousRow.Cells[8].Visible = true;
                            row.Cells[9].RowSpan = 1;
                            previousRow.Cells[9].Visible = true;
                            row.Cells[5].RowSpan = 1;
                            previousRow.Cells[5].Visible = true;
                            row.Cells[10].RowSpan = 1;
                            previousRow.Cells[10].Visible = true;
                            row.Cells[11].RowSpan = 1;
                            previousRow.Cells[11].Visible = true;
                            row.Cells[12].RowSpan = 1;
                            previousRow.Cells[12].Visible = true;
                            row.Cells[14].RowSpan = 1;
                            previousRow.Cells[14].Visible = true;
                        }
                        
                    }
                 //break;
                }
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
            GridViewRow footerrow = new GridViewRow(1, 0, DataControlRowType.Footer, DataControlRowState.Insert);
            TableCell FooterCell = new TableCell();
            FooterCell.Text = "Total Actual Premium Amount:" + Convert.ToString(ViewState["Total"]);
            FooterCell.ColumnSpan = Convert.ToInt32(cols);
            FooterCell.ForeColor = Color.White;
            FooterCell.HorizontalAlign = HorizontalAlign.Center;
            FooterCell.Height = 20;
            FooterCell.Font.Size = 14;
            FooterCell.BackColor = Color.Gray;
            footerrow.Cells.Add(FooterCell);
            gridView.Controls[0].Controls.AddAt(0, footerrow);

            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gridView.AlternatingRowStyle.BackColor = Color.Maroon;
            gridView.RowStyle.BackColor =Color.MediumBlue;
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = "Report For Medi Claim Summary";
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
                        //cell.BackColor = gridView.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        ////cell.BackColor = gridView.RowStyle.BackColor;
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
