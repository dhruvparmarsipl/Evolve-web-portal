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
using InfoSoftGlobal;
using Utilities;
using System.Drawing;
using System.IO;
//using Utilities;

public partial class Graph : System.Web.UI.Page
{
    string GraphWidth = "550";
    string GraphHeight = "370";
    Util ult = new Util();
    FPADAL Dal = new FPADAL();
    Utility.Service Util = new Utility.Service();
    static string logfile = "D:\\Eicher\\All Logs\\FPA";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillHeader(getSAPID(ReturnLoginName()));
          
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
        ////string loginname = SPContext.Current.Web.CurrentUser.LoginName.ToString();
        ////string[] Result = RetrieveSplitValue(loginname);
        ////return Result[1].ToLower();
        return "spstesttrg";
    }
    private void FillHeader(string sapid)
    {
        DataTable dt_UInfo = Dal.getUserInfo(sapid);
        if (dt_UInfo.Rows.Count > 0)
        {
            spnLoginUser.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnFormCreater.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnIntName.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ENAME"]);
            spnEmpCode.InnerText = Convert.ToString(dt_UInfo.Rows[0]["ECODE"]);
            spnDesignation.InnerText = Convert.ToString(dt_UInfo.Rows[0]["DESIGNATION"]);
            spnLoc.InnerText = Convert.ToString(dt_UInfo.Rows[0]["LOC"]);
            HFSAPID.Value = Convert.ToString(dt_UInfo.Rows[0]["SAPID"]);
            spnSrNo.InnerText = Dal.Serial_No("FPA");
            HFID.Value = spnSrNo.InnerText;
        }
    }
  
    protected object GetStatus(object status)
    {
        string cstatus = string.Empty;
        if (status.ToString() == "0")
        {
            cstatus = "Initiate By FPA ";
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        using (DataTable dt = Dal.getGraph(txtEcode.Text,txtFromdate.Text,txtTodate.Text))
        {
            if (dt.Rows.Count > 0)
            {
                gvReport.DataSource = dt;
                gvReport.DataBind();
                btnReport.Visible = true;
            }
            else
            {
                gvReport.DataBind();
                btnReport.Visible = false;

            }
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        string columns = gvReport.Rows[0].Cells.Count.ToString();
        GenereateExcel(gvReport, columns);
    }
    public void GenereateExcel(GridView gridView, string cols)
    {

        string Ename = Dal.getEname(txtEcode.Text);
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename="+Ename+".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridView.HeaderRow.BackColor = Color.White;

            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell2 = new TableCell();
          
            HeaderCell2.Text = Ename +"claim detail reort from "+txtFromdate.Text +   "to"  + txtTodate.Text;
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
