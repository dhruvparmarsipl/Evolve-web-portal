using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel; 
public partial class UploadPremium : System.Web.UI.Page
{ 
    Utility.Service Util = new Utility.Service();
    string logfile = "C:\\Eicher\\All Logs\\Medi";
	 string[] strArray;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region ANOTHER CODE
    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        if (fuExcel.HasFile)
        {
            string filename = Path.GetFileName(fuExcel.PostedFile.FileName);
            string path = Server.MapPath("") + @"\Files\" + filename; //Server.MapPath(fileUpload_Excel.PostedFile.FileName);
            fuExcel.SaveAs(path);
            try
            {
                FillDataTable(path);
            }
            catch (Exception ex)
            {
			    Util.Log("Mail Send  form ID -- " + ex.Message.ToString(), logfile);
                ShowMessage("Please try after some time");
            }
        }
    }
    private void FillDataTable1111(string FilePath)
    {
        int res = 0, main = 0;
        OleDbConnection connExcel = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0");
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
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
        
        if (dt.Rows.Count > 0)
        {
            MediMain oclsMM = new MediMain();
            DataTable NewDT = new DataTable();
            NewDT = GetOptimezed(dt);
            string[] TobeDistinct = { "MASTER_PLAN_ID", "No_of_Dependent" };
            DataTable dtDistinct = GetDistinctRecords(NewDT, TobeDistinct);

            DataTable Plandt = new DataTable();
            Plandt = GetMasterPlan(dtDistinct);
            NewDT.Columns.Remove("No_of_Dependent");
            res = oclsMM.InsertPremiumTable(NewDT);
            main = oclsMM.InsertMasterPlanTable(Plandt);
            if (res != 0 && main!=0)
            {
                grdUploaded.Caption = "Successfull Uploaded Records";
                grdUploaded.DataSource = dt;
                grdUploaded.DataBind();
            }
            else
            {
                if (res == 0)
                {
                    ShowMessage("ERROR in uploading Process. Try again later");
                }
                else
                {
                    ShowMessage("ERROR in uploading Process. Try again later");
                }
               
            }
        }
    }
    public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
    {
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
        return dtUniqRecords;
    }
    private DataTable GetMasterPlan(DataTable cdt)
    {
        cdt.Columns.Add("PlanName", typeof(string));

        foreach (DataRow rows in cdt.Rows)
        {
            switch (rows["MASTER_PLAN_ID"].ToString().TrimEnd())
            {
                case "1":
                    rows["PlanName"] = "A"; 
                    break;
                case "2":
                    rows["PlanName"] = "B";
                    break;
                case "3":
                    rows["PlanName"] = "C";
                    break;
                case "4":
                    rows["PlanName"] = "D";
                    break;
                case "5":
                    rows["PlanName"] = "E";
                    break;
                case "6":
                    rows["PlanName"] = "F";
                    break;
                case "7":
                    rows["PlanName"] = "G";
                    break;
                case "8":
                    rows["PlanName"] = "H";
                    break;
                case "9":
                    rows["PlanName"] = "I";
                    break;
                case "10":
                    rows["PlanName"] = "J";
                    break;
                case "11":
                    rows["PlanName"] = "K";
                    break;
                case "12":
                    rows["PlanName"] = "L";
                    break;
                case "13":
                    rows["PlanName"] = "M";
                    break;
                case "14":
                    rows["PlanName"] = "N";
                    break;
                case "15":
                    rows["PlanName"] = "O";
                    break;
                case "16":
                    rows["PlanName"] = "P";
                    break;
                case "17":
                    rows["PlanName"] = "Q";
                    break;
                case "18":
                    rows["PlanName"] = "R";
                    break;
                case "19":
                    rows["PlanName"] = "S";
                    break;
                case "20":
                    rows["PlanName"] = "T";
                    break;
                case "21":
                    rows["PlanName"] = "U";
                    break;
                case "22":
                    rows["PlanName"] = "V";
                    break;
                case "23":
                    rows["PlanName"] = "W";
                    break;
                case "24":
                    rows["PlanName"] = "X";
                    break;
            }
        }
        return cdt;
    }
    //public int Dependent(string Depenedent)
    //{
    //    int Count = 0;
    //    Count = Depenedent.Split(',').Length - 1;
    //    if (Count == 0)
    //    {
    //        Count = 1;
    //    }
    //    return Count+1;
    //}
    private DataTable GetOptimezed(DataTable dt)
    {
        foreach(DataRow rows in dt.Rows)
        {
            switch (rows["MASTER_PLAN_ID"].ToString())
            {
                case "Plan A":
                    rows["MASTER_PLAN_ID"] = 1;
                    break;
                case "Plan B":
                    rows["MASTER_PLAN_ID"] = 2;
                    break;
                case "Plan C":
                    rows["MASTER_PLAN_ID"] = 3;
                    break;
                case "Plan D":
                    rows["MASTER_PLAN_ID"] = 4;
                    break;
                case "Plan E":
                    rows["MASTER_PLAN_ID"] = 5;
                    break;
                case "Plan F":
                    rows["MASTER_PLAN_ID"] = 6;
                    break;
                case "Plan G":
                    rows["MASTER_PLAN_ID"] = 7;
                    break;
                case "Plan H":
                    rows["MASTER_PLAN_ID"] = 8;
                    break;
                case "Plan I":
                    rows["MASTER_PLAN_ID"] = 9;
                    break;
                case "Plan J":
                    rows["MASTER_PLAN_ID"] = 10;
                    break;
                case "Plan K":
                    rows["MASTER_PLAN_ID"] = 11;
                    break;
                case "Plan L":
                    rows["MASTER_PLAN_ID"] = 12;
                    break;


                case "Plan M":
                    rows["MASTER_PLAN_ID"] = 13;
                    break;
                case "Plan N":
                    rows["MASTER_PLAN_ID"] = 14;
                    break;
                case "Plan O":
                    rows["MASTER_PLAN_ID"] = 15;
                    break;
                case "Plan P":
                    rows["MASTER_PLAN_ID"] = 16;
                    break;
                case "Plan Q":
                    rows["MASTER_PLAN_ID"] = 17;
                    break;
                case "Plan R":
                    rows["MASTER_PLAN_ID"] = 18;
                    break;
                case "Plan S":
                    rows["MASTER_PLAN_ID"] = 19;
                    break;
                case "Plan T":
                    rows["MASTER_PLAN_ID"] = 20;
                    break;
                case "Plan U":
                    rows["MASTER_PLAN_ID"] = 21;
                    break;
                case "Plan V":
                    rows["MASTER_PLAN_ID"] = 22;
                    break;
                case "Plan W":
                    rows["MASTER_PLAN_ID"] = 23;
                    break;
                case "Plan X":
                    rows["MASTER_PLAN_ID"] = 24;
                    break;
            }
            switch (rows["MASTER_AGE_ID"].ToString().TrimEnd())
            {
                case "Below 35":
                    rows["MASTER_AGE_ID"] = 1;
                    break;
                case "36 to 45":
                    rows["MASTER_AGE_ID"] = 2;
                    break;
                case "36  to 45 ":
                    rows["MASTER_AGE_ID"] = 2;
                    break;
                case "46 to 55":
                    rows["MASTER_AGE_ID"] = 3;
                    break;
                case "46  to 55 ":
                    rows["MASTER_AGE_ID"] = 3;
                    break;
                case "56 to 65":
                    rows["MASTER_AGE_ID"] = 4;
                    break;
                case "56  to 65 ":
                    rows["MASTER_AGE_ID"] = 4;
                    break;
                case "66 to 70":
                    rows["MASTER_AGE_ID"] = 5;
                    break;
                case "66 & above":
                    rows["MASTER_AGE_ID"] = 5;
                    break;

                     
            }
            switch (rows["EMPTYPE"].ToString())
            {
                case "Staff":
                    rows["EMPTYPE"] = "S";
                    break;
                case "Executive":
                    rows["EMPTYPE"] = "E";
                    break;
                case "GM":
                    rows["EMPTYPE"] = "GM";
                    break;
                case "SVP":
                    rows["EMPTYPE"] = "SVP";
                    break;
            }

        }
        return dt;
    }
    public void ShowMessage(string mess)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Key1", "alert('" + mess + "')", true);
    }
	
	
	   protected string ConvertToLetter(int iCol)
    {
        //Dim iAlpha As Integer
        int iAlpha = 0;
        // Dim iRemainder As Integer
        int iRemainder = 0;
        string ConvertToLetter = string.Empty;
        iAlpha = Convert.ToInt32(iCol / 26);
        iRemainder = iCol - (iAlpha * 26);
        if (iAlpha > 0)
        {
            //((char)i).ToString() 
            ConvertToLetter = ((char)(iAlpha + 64)).ToString();
        }
        if (iRemainder > 0)
        {
            ConvertToLetter = ConvertToLetter + ((char)(iRemainder + 64)).ToString();
        }
        return ConvertToLetter;
    }
    string[] ConvertToStringArray(System.Array values)
    {
        string[] theArray = new string[values.Length];
        for (int i = 1; i <= values.Length; i++)
        {
            if (values.GetValue(1, i) == null)
                theArray[i - 1] = "";
            else
                theArray[i - 1] = (string)values.GetValue(1, i).ToString();
        }
        return theArray;
    }
    public void FillDataTable(string Path1)
    {
        int res = 0, main = 0;
        string path = Path1;
        Microsoft.Office.Interop.Excel.Application ExcelObj = null;
        ExcelObj = new Microsoft.Office.Interop.Excel.Application();
        Microsoft.Office.Interop.Excel.Workbook theWorkbook = ExcelObj.Workbooks.Open(path, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                  true, false, 0, true, false, false);
        Microsoft.Office.Interop.Excel.Sheets sheets = theWorkbook.Worksheets;
        Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);
        string cntexcel = worksheet.UsedRange.Columns.Count.ToString();
        int cnyexcelint = Convert.ToInt32(cntexcel);
        string cntrowexcel = worksheet.UsedRange.Rows.Count.ToString(); //  .Rows.Count.ToString();  
        int cntrowexcelint = Convert.ToInt32(cntrowexcel);
        //changing here
        DataTable dt = new DataTable("Pay_All_Data");
        DataRow dr = null;
        DataColumn dc = null;
        // change
        for (int q = 1; q <= cntrowexcelint; q++)
        {
            string val = ConvertToLetter(cnyexcelint);
            Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("A" + q.ToString(), val + q.ToString());
            System.Array myvalues = (System.Array)range.Cells.Value2;
            strArray = ConvertToStringArray(myvalues);
            for (int i = 0; i < strArray.Length; i++)
            {
                //Create columns
                if (q == 1)
                {
                    dc = new DataColumn(strArray[i].ToString(), typeof(System.String));
                    dt.Columns.Add(dc);
                }
                //Create Rows
                if (q > 1)
                {

                    if (i == 0)
                    {
                        dr = dt.NewRow();
                    }

                    dr[dt.Columns[i].ToString()] = strArray[i].ToString();

                }


            }

            if (q > 1)
            {
                dt.Rows.Add(dr);
            }
        }
	   theWorkbook.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value); //Excel close
        ViewState["Exceldt"] = dt;

        if (dt.Rows.Count > 0)
        {
            MediMain oclsMM = new MediMain();
            DataTable NewDT = new DataTable();
            NewDT = GetOptimezed(dt);
            string[] TobeDistinct = { "MASTER_PLAN_ID", "No_of_Dependent" };
            DataTable dtDistinct = GetDistinctRecords(NewDT, TobeDistinct);

            DataTable Plandt = new DataTable();
            Plandt = GetMasterPlan(dtDistinct);
            NewDT.Columns.Remove("No_of_Dependent");
            res = oclsMM.InsertPremiumTable(NewDT);
            main = oclsMM.InsertMasterPlanTable(Plandt);
            if (res != 0 && main != 0)
            {
                grdUploaded.Caption = "Successfull Uploaded Records";
                grdUploaded.DataSource = dt;
                grdUploaded.DataBind();
            }
            else
            {
                if (res == 0)
                {
                    ShowMessage("ERROR in uploading Process. Try again later");
                }
                else
                {
                    ShowMessage("ERROR in uploading Process. Try again later");
                }

            }
        }
        //myCommand.Fill(myDataSet, tableName);
       
    }
    #endregion
}
