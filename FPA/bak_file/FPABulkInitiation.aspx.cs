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
using System.IO;

using System.Data.OleDb;
using Microsoft.SharePoint;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Mail;
using System.Net;


public partial class FPABulkInitiation : System.Web.UI.Page
{
    #region Variable
    FPADALL Dal = new FPADALL();
    DataTable _tempdt;
    UtilityNew.Service Util = new UtilityNew.Service();
#endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillHeader(getSAPID(ReturnLoginName()));
            FillLocation(getSAPID(ReturnLoginName()));
            ViewState["_tempdt"] = null;
        }
    }
    #endregion
     
    #region Download Excel
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedValue == "-1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please Select Location.');", true);
            }
            else
            {
                DataTable dtdata = Dal.GetEmployeeForBulkInitiate(ddlLocation.SelectedValue, getSAPID(ReturnLoginName()));
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=FpaInitiate.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                if (dtdata != null)
                {
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                    GridView gvFiles = new GridView();
                    gvFiles.DataSource = dtdata;
                    gvFiles.DataBind();
                    gvFiles.RenderControl(htw);
                    //Response.Write(sw.ToString());
                    string style = @"<style> TD { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Write(sw.ToString());
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('No Record Found')", true);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Upload Excel
    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        if (fileupload.HasFile)
        {

            string FileName = Path.GetFileName(fileupload.PostedFile.FileName);
            string Extension = Path.GetExtension(fileupload.PostedFile.FileName);
           // string FilePath = Path.GetFullPath(fileupload.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string FilePath = Server.MapPath(FolderPath + FileName);
            fileupload.SaveAs(FilePath);
            FillDataTable(FilePath, Extension);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('File Uploaded Successfully.')", true);
 
        }
    }
    #endregion

    #region Common Method
    private void FillDataTable(string FilePath, string Extension)
    {
        string conStr = string.Empty;
         if (Extension.ToLower()== ".xlsx")
        {
        conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 xml;HDR=YES;IMEX=1'";
        }
        else { conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'"; }
                     conStr = String.Format(conStr, FilePath, 1);
        using (OleDbConnection connExcel = new OleDbConnection(conStr))
        {
            using (OleDbCommand cmdExcel = new OleDbCommand())
            {
                using (OleDbDataAdapter oda = new OleDbDataAdapter())
                {
                    using (DataTable dt = new DataTable())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                        oda.SelectCommand = cmdExcel;
                        oda.Fill(dt);
                        connExcel.Close();
                        ViewState["Exceldt"] = dt;
                    }
                }
            }
        }
    }
    public static bool CheckStringPattern(string str)
    {
        Regex RgxUrl = new Regex("[a-z!@#$%&/{()}=?+]");
        bool value = RgxUrl.IsMatch(str);
        return value;
    }
    private string ReturnFpaAdminSapId(string UserSapId)
    {
        string FpaSapId = string.Empty;
        FpaSapId = Dal.ReturnFpaAdminSapId(UserSapId);
        return FpaSapId;
    }
    public static string StripHtml(string str)
    {
        StringBuilder sb = new StringBuilder();
        if (str == "")
        {
            str = "0.0";
        }
        // sb.Append(Regex.Replace(str, "[^0-9.]+", " ")); 
        return str.ToString();
    }
    private void CreateTable(string Ename,string Ecode, string Name, string Email)
    {

        DataRow dr;
        if (ViewState["_tempdt"] == null)
        {
            _tempdt = new DataTable();
            _tempdt.Columns.Add("Ename", typeof(string));
            _tempdt.Columns.Add("Ecode", typeof(string));
            _tempdt.Columns.Add("ErrorName", typeof(string));
            _tempdt.Columns.Add("Email", typeof(string));
            dr = _tempdt.NewRow();
            dr["Ename"] = Ename;
            dr["Ecode"] = Ecode;
            dr["ErrorName"] = Name;
            dr["Email"] = Email;
            _tempdt.Rows.Add(dr);
            ViewState["_tempdt"] = _tempdt;
        }
        else
        {
            dr = _tempdt.NewRow();
            dr["Ename"] = Ename;
            dr["Ecode"] = Ecode;
            dr["ErrorName"] = Name;
            dr["Email"] = Email;
            _tempdt.Rows.Add(dr);
            ViewState["_tempdt"] = _tempdt;

        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
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
            HFDESID.Value = Convert.ToString(dt_UInfo.Rows[0]["DES_ID"]);
            HFEMAILS.Value = Convert.ToString(dt_UInfo.Rows[0]["EMAILS"]);

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
         //return "spstesttrg";
    }
    private void FillLocation(string sapid)
    {
        ddlLocation.Items.Clear();
        ListItem li = new ListItem("--Select--", "-1");
        ddlLocation.Items.Add(li);
		ListItem li1 = new ListItem("Select All", "0");
        ddlLocation.Items.Add(li1);
        ddlLocation.AppendDataBoundItems = true;
        DataTable dt_Loc = Dal.getLocation(sapid);
        if (dt_Loc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dt_Loc;
            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location";
            ddlLocation.DataBind();

        }
        else
        {
        }

    }

    #endregion
   
    #region BulkInitiate
    protected void btnInitiate_Click(object sender, EventArgs e)
    {
        try
        {
            string path = Server.MapPath(ConfigurationManager.AppSettings["FolderPath"]);
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
            }

            using (DataTable _Fpadt = ((DataTable)ViewState["Exceldt"]))
            {
                if (_Fpadt.Rows.Count > 0)
                {
                    string DesId = string.Empty; string EmpSapId = string.Empty; int TransID = 0; string newUrl = string.Empty;
                    string EmailID = string.Empty;
                    double Fpa = 0, other1 = 0, other2 = 0, other3 = 0, totpetrol = 0, totAllow = 0, totSalAvail = 0;
                    double totreimavail = 0, clarental = 0, vehemi = 0, saf = 0, busdeduct = 0, totdeduct = 0;
                    double NetAmount = 0, Locfpa = 0;
                    foreach (DataRow dr in _Fpadt.Rows)
                    {
                        bool chechFpa = CheckStringPattern(Convert.ToString(dr["FPA"]));
                        if (chechFpa == false)
                        {
                            Fpa = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["FPA"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]),Convert.ToString(dr["ECODE"]), "FPA", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkother1 = CheckStringPattern(Convert.ToString(dr["Other1"]));
                        if (checkother1 == false)
                        {
                            other1 = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["Other1"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "Other1", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkother2 = CheckStringPattern(Convert.ToString(dr["Other2"]));
                        if (checkother2 == false)
                        {
                            other2 = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["Other2"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "Other2", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkother3 = CheckStringPattern(Convert.ToString(dr["Other3"]));
                        if (checkother3 == false)
                        {
                            other3 = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["Other3"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "Other3", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkToatalpetrolAmount = CheckStringPattern(Convert.ToString(dr["ToatalpetrolAmount"]));
                        if (checkToatalpetrolAmount == false)
                        {
                            totpetrol = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["ToatalpetrolAmount"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "ToatalpetrolAmount", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        //bool checkBasicCurrent = CheckStringPattern(Convert.ToString(dr["BasicCurrent"]));
                        //if (checkBasicCurrent == false)
                        //{
                        //    cBasic = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["BasicCurrent"]))), 0);
                        //}
                        //else
                        //{
                        //    CreateTable(Convert.ToString(dr["Sapid"]), "BasicCurrent", Convert.ToString(dr["Email"]));
                        //    continue;
                        //}
                        //bool checkCTC = CheckStringPattern(Convert.ToString(dr["CTC"]));
                        //if (checkCTC == false)
                        //{
                        //    CTC = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["CTC"]))), 0);
                        //}
                        //else
                        //{
                        //    CreateTable(Convert.ToString(dr["Sapid"]), "CTC", Convert.ToString(dr["Email"]));
                        //    continue;
                        //}
                        bool checkLocationFPA = CheckStringPattern(Convert.ToString(dr["LocationFPA"]));
                        if (checkLocationFPA == false)
                        {
                            Locfpa = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["LocationFPA"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "LocationFPA", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkTotalAllowance = CheckStringPattern(Convert.ToString(dr["TotalAllowance"]));
                        if (checkTotalAllowance == false)
                        {
                            totAllow = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["TotalAllowance"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "TotalAllowance", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkTotalSalaryAvailed = CheckStringPattern(Convert.ToString(dr["TotalSalaryAvailed"]));
                        if (checkTotalSalaryAvailed == false)
                        {
                            totSalAvail = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["TotalSalaryAvailed"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "TotalSalaryAvailed", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkTotalReimbursementAvailed = CheckStringPattern(Convert.ToString(dr["TotalReimbursementAvailed"]));
                        if (checkTotalReimbursementAvailed == false)
                        {
                            totreimavail = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["TotalReimbursementAvailed"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "TotalReimbursementAvailed", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkCLARental = CheckStringPattern(Convert.ToString(dr["CLARental"]));
                        if (checkTotalReimbursementAvailed == false)
                        {
                            clarental = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["CLARental"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "CLARental", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkVehicleEMI = CheckStringPattern(Convert.ToString(dr["VehicleEMI"]));
                        if (checkVehicleEMI == false)
                        {
                            vehemi = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["VehicleEMI"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "VehicleEMI", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkSAF = CheckStringPattern(Convert.ToString(dr["SAF"]));
                        if (checkSAF == false)
                        {
                            saf = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["SAF"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "SAF", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkBusDeduction = CheckStringPattern(Convert.ToString(dr["BusDeduction"]));
                        if (checkBusDeduction == false)
                        {
                            busdeduct = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["BusDeduction"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "BusDeduction", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        //bool checkBasicComponent = CheckStringPattern(Convert.ToString(dr["BasicComponent"]));
                        //if (checkBasicComponent == false)
                        //{
                        //    Basiccomp = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["BasicComponent"]))), 0);
                        //}
                        //else
                        //{
                        //    CreateTable(Convert.ToString(dr["Sapid"]), "BasicComponent", Convert.ToString(dr["Email"]));
                        //    continue;
                        //}
                        //bool checkRetrialBenefitComponent = CheckStringPattern(Convert.ToString(dr["RetrialBenefitComponent"]));
                        //if (checkRetrialBenefitComponent == false)
                        //{
                        //    retrialbcomp = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["RetrialBenefitComponent"]))), 0);
                        //}
                        //else
                        //{
                        //    CreateTable(Convert.ToString(dr["Sapid"]), "RetrialBenefitComponent", Convert.ToString(dr["Email"]));
                        //    continue;
                        //}
                        bool checkTotalDeduction = CheckStringPattern(Convert.ToString(dr["TotalDeduction"]));
                        if (checkTotalDeduction == false)
                        {
                            totdeduct = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["TotalDeduction"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "TotalDeduction", Convert.ToString(dr["Email"]));
                            continue;
                        }
                        bool checkNetAmount = CheckStringPattern(Convert.ToString(dr["NetAmount"]));
                        if (checkNetAmount == false)
                        {
                            NetAmount = Math.Round(Convert.ToDouble(StripHtml(Convert.ToString(dr["NetAmount"]))), 0);
                        }
                        else
                        {
                            CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "NetAmount", Convert.ToString(dr["Email"]));
                            continue;
                        }

                        DesId = Convert.ToString(dr["DesId"]);
                        EmailID = Convert.ToString(dr["Email"]);
                        EmpSapId = Convert.ToString(dr["Sapid"]);
                        string year = DateTime.Now.ToString();
                        HFID.Value = Dal.Serial_No("FPA");

                        newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
                        totAllow = Fpa + other1 + other2 + other3 + totpetrol + Locfpa;
                        totdeduct = totSalAvail + totreimavail + clarental + vehemi + saf + busdeduct;
                        NetAmount = totAllow - totdeduct;
                        if (NetAmount > 0)
                        {
                            if (EmailID.Trim() == "")
                            {
                                //EmailID="svhanda@vecv.in";
                                //TransID = Dal.InsertBulkFPANew(DesId, EmpSapId, year, "", Fpa, other1, other2, other3, Locfpa, totpetrol, totreimavail, clarental, vehemi, saf, busdeduct, HFID.Value, totSalAvail, EmailID.Trim(), "0", spnLoginUser.InnerText.ToString(), newUrl);
                            }
                            else
                            {
                                //string FpaSapid = ReturnFpaAdminSapId(EmpSapId);
                                //if (FpaSapid.Trim() == HFSAPID.Value.Trim())
                                //{
                                   TransID = Dal.InsertBulkFPANew(DesId, EmpSapId, year, "", Convert.ToInt32(Fpa), Convert.ToInt32(other1), Convert.ToInt32(other2), Convert.ToInt32(other3), Convert.ToInt32(Locfpa), Convert.ToInt32(totpetrol), Convert.ToInt32(totreimavail), Convert.ToInt32(clarental), Convert.ToInt32(vehemi), Convert.ToInt32(saf), Convert.ToInt32(busdeduct), HFID.Value, Convert.ToInt32(totSalAvail), EmailID.Trim(), "0", spnLoginUser.InnerText.ToString(), newUrl);
                               //}
                            }
                        }
                        else
                        {
			    CreateTable(Convert.ToString(dr["ENAME"]), Convert.ToString(dr["ECODE"]), "Net Amount is zero or negative", Convert.ToString(dr["Email"]));
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please note Net Amount cannot be Negative.');", true);
                        }
                        if (TransID == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Your form submitted successfully.');CloseRefreshWindow();", true);
                        }
                        else if (TransID == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                        }
                        else if (TransID == 2)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Please try after some time.');", true);
                        }
                        else if (TransID == 5)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('Data Already Submitted.');CloseRefreshWindow();", true);
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('No Record Found..')", true);
                }
                //SendMailWithAttachment(_tempdt);
                
            }
        }
        catch (Exception ex)
        {
// ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKeyD", "alert('"+ex.Message.ToString()+"');", true);
        }
    }
    #endregion

    protected void SendMailWithAttachment(DataTable _tempdt)
    {
        gvMissingFpa.DataSource = _tempdt;
        gvMissingFpa.DataBind();
        if (_tempdt.Rows.Count > 0)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            gvMissingFpa.RenderControl(htw);
            string renderedGridView = sw.ToString();
            //write file in to disk
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string name = "temp.xls";
            string FilePath = Server.MapPath(FolderPath + name);
            System.IO.File.WriteAllText(FilePath, renderedGridView);

            SmtpClient sMail = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"].ToString());//exchange or smtp server goes here.
            sMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage objmailmsg = new MailMessage();
            objmailmsg.From = new System.Net.Mail.MailAddress("ewadmin@eicher.in");
            objmailmsg.To.Add(HFEMAILS.Value.ToString().Split('#')[0]);

            string newUrl = "http://epicenter.vecv.net/mycorner/fpaandlimits/SitePages/Home.aspx";
            string Mailbody = "<table style='width:550px;background-color:#fffff5;  border:1px solid Gray;  font-family:Arial'><tr><td><div style=' background-color: #fffff5;  font-family:Arial'><div style='font-size:11px;background-color:#FFFFF5;padding:10px'>Dear " + spnLoginUser.InnerText + "<br/><br/>Please see the attachment for those employee whose FPA is not initiated due to some issue in excel file.<br />	<br/>Please  click on  the link below for allocation of your Flexi.<br/><br/><a href=\"" + newUrl + "\">Click here to go to FPA home Page</a><br/><br/>Thanks<br /><br /><span style='font-weight: normal; font-size: smaller; color: black; font-family: Arial; font-style:italic'>	This is system generated message</span></div></div></td></tr></table>";

            objmailmsg.IsBodyHtml = true;
            objmailmsg.Subject = "User FPA Initiation Error Issue...";
            objmailmsg.Body = Mailbody;
            //Attach that excel sheet
            objmailmsg.Attachments.Add(new Attachment(Server.MapPath(FolderPath + name)));
            sMail.Send(objmailmsg);
            objmailmsg.Dispose();
            //Delete that temp file after sent
            if (System.IO.File.Exists(Server.MapPath(FolderPath + name)))
            {
                System.IO.File.Delete(Server.MapPath(FolderPath + name));
            }

        }
    
    }
}
