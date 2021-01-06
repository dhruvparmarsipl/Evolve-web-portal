using System;
using System.Data;
using System.Web.UI;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for BusinessLayer
/// </summary>
public class FPADALL
{
    public const int IncomeTaxAmt = 10000;
    public FPADALL()
	{
	}

   public  SqlConnection ReturnSqlconnection()
    {
        SqlConnection vConnection = null;
        try
        {
            String vConnectionString = ConfigurationManager.AppSettings["Sqlconn"].ToString();
            vConnection = new SqlConnection(vConnectionString);
            vConnection.Open();
        }
        catch (Exception xe)
        {
            xe.ToString();
        }
        finally
        {
            vConnection.Close();
        }
        return vConnection;
    }
   
   //public SqlConnection ReturnSqlconnection()
    //{
     //   SqlConnection vConnection = null;
     //   try
     //   {
     //       String vConnectionString = ConfigurationManager.AppSettings["Sqlconn"].ToString();
     //       vConnection = new SqlConnection(vConnectionString);
     //       vConnection.Open();
      //  }
      //  catch (Exception xe) { xe.ToString(); }
      //  return vConnection;
    //}
    
    public string Serial_No(string claimtype)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "SP_GENERATE_SLNO", new SqlParameter("@appName", claimtype)));
        }
    }
    //---------------------------------------------------------
    public DataTable Get_DataByPassingQuery(string query)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(con, CommandType.Text, query)).Tables[0];
        }
    }

    public string Get_Single_DataByPassingQuery(string query)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString((SqlHelper.ExecuteScalar(con, CommandType.Text, query)));
        }
    }
   
    //public DataTable getFpaUserInfoclosed(string SAPID)
    //{
    //    using (SqlConnection con = ReturnSqlconnection())
    //    {
    //        return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
    //            CommandType.StoredProcedure,
    //            "FPA_getUserInfoclosed", new SqlParameter("@SAPID", SAPID))).Tables[0];
    //    }
    //}
     public DataTable getFpaUserInfoclosedCycleId(string CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getUserInfoclosedCylceId", new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
    public DataTable getFpaUserInfo(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getUserInfo", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getCurrentCycleUser(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getCURRENTCYCLEUSER", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getclaimUserInfo(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFPAClaimUser", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getclaimUserInfoquerystring(string SAPID, string serialNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFPAClaimUserquerystring", new SqlParameter("@SAPID", SAPID), new SqlParameter("@SERIALNO", serialNumber))).Tables[0];
        }
    }
    public DataTable getUserInfo(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFPAAdminInfo", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getLocation(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getLocation", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getDesignation(string Locname, string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getDesignation", new SqlParameter("@Loc", Locname), new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getYear()
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure, "FPA_spgetyear")).Tables[0];
        }

    }
    public DataTable getFPAEmployee(string Locname, int DesId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFpaEmployee", new SqlParameter("@Loc", Locname), new SqlParameter("@desid", DesId))).Tables[0];
        }

    }
    public DataTable getFPAEmployeeDisable(string Locname, int DesId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getEmployeeDisable", new SqlParameter("@loc", Locname), new SqlParameter("@DesId", DesId))).Tables[0];
        }

    }
    public DataTable getApproveEmployee(string Locname, int DesId,string Year,string Status)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_spgetApproveEmployee", new SqlParameter("@loc", Locname), new SqlParameter("@DesId", DesId),
                new SqlParameter("@Year", Year), new SqlParameter("@Status", Status))).Tables[0];
        }

    }
     public DataTable getEmployee(string Locname, int DesId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getEmployee", new SqlParameter("@Loc", Locname), new SqlParameter("@desid", DesId))).Tables[0];
        }

    }
    public DataTable getEmployeeEcode(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getEmployeeEcode", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
	public int InsertFPANew(string DesId, string EmpSapId, string year, string commnets, int Fpa, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, string serial, int totSalAvail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaDataInsert",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets),
            new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
    //public int InsertFPANew(string DesId, string EmpSapId,string year, string commnets, double Fpa, double other1,double other2,double other3,double Locfpa,double totpetrol,double totreimavail,double clarental,double vehemi,double saf,double busdeduct,string serial,double totSalAvail)
    //{
      //  using (SqlConnection con = ReturnSqlconnection())
      //  {
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaDataInsert",
        //    new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
         //   new SqlParameter("@commnets", commnets),
         //   new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
         //   new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
         //   new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
         //   new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf),
         //   new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
       // }
    //}
    public int InsertBulkFPANew(string DesId, string EmpSapId, string year, string commnets, int Fpa, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, string serial, int totSalAvail, string Email, string Status, string Loginuser, string newUrl,int basicsalary)
    {
        int value = 0;
        try
        {
            using (SqlConnection con = ReturnSqlconnection())
            {
                value = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewBulkFpaDataInsert",
                new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
                new SqlParameter("@commnets", commnets), new SqlParameter("@Email", Email), new SqlParameter("@ACTION_PERFORMED", Status),
                new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2), new SqlParameter("@COMMENTS", commnets),
                new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
                new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental), new SqlParameter("@SERIAL_NO", serial),
                new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@PERFORMED_BY", Loginuser), new SqlParameter("@url", newUrl),
                new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail),
                new SqlParameter("@BasicSalary", basicsalary)));
            }
        }
        catch (Exception ex)
        {
        }
        return value;
    }
	
	 public int reinitiation(string FpaId, string DesId, string EmpSapId, string year, string commnets, int Fpa, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, string serial, int totSalAvail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaDataInsert",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId),
            new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
    //public int reinitiation(string FpaId,string DesId, string EmpSapId, string year, string commnets, double Fpa, double other1, double other2, //double other3, double Locfpa, double totpetrol, double totreimavail, double clarental, double vehemi, double saf, double busdeduct, string //serial, double totSalAvail)
   // {
     ////   using (SqlConnection con = ReturnSqlconnection())
        //{
          //  return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaDataInsert",
          //  new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
          //  new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId),
          //  new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
          //  new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
          //  new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
          //  new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf),
          //  new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        //}
   // }
   // public int reinitiationGMFPANew(string FpaId,string DesId, string EmpSapId, string year, string commnets, double ctc, double cBasic, double //other1, double other2, double other3, double Locfpa, double totpetrol, double totreimavail, double clarental, double vehemi, double saf, //double busdeduct, double Basiccomp, double retrialbcomp, string serial, double totSalAvail)
    //{
       // using (SqlConnection con = ReturnSqlconnection())
       // {
         //   return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaGMDataInsert",
          //  new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
         //   new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId),
          //  new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
         //   new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
          //  new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
          //  new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicRetrialAvailed", Basiccomp), new //SqlParameter("@Retrivalcomp", retrialbcomp),
           //// new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        //}
    //}
    //public int InsertGMFPANew(string DesId, string EmpSapId, string year, string commnets, double ctc, double cBasic, double other1, double other2, double other3, double Locfpa, double totpetrol, double totreimavail, double clarental, double vehemi, double saf, double busdeduct, double Basiccomp, double retrialbcomp, string serial, double totSalAvail)
    //{
//using (SqlConnection con = ReturnSqlconnection())
       // {
          //  return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaGMDataInsert",
//new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
           // new SqlParameter("@commnets", commnets),
           // new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
           // new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
           // new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
           // new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicComp", Basiccomp), new //SqlParameter("@Retrivalcomp", retrialbcomp),
           /// new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
       // }
    //}
	
	public int reinitiationGMFPANew(string FpaId, string DesId, string EmpSapId, string year, string commnets, int ctc, int cBasic, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, int Basiccomp, int retrialbcomp, string serial, int totSalAvail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaGMDataInsert",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId),
            new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicRetrialAvailed", Basiccomp), new SqlParameter("@Retrivalcomp", retrialbcomp),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
	public int InsertGMFPANew(string DesId, string EmpSapId, string year, string commnets, int ctc, int cBasic, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, int Basiccomp, int retrialbcomp, string serial, int totSalAvail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaGMDataInsert",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets),
            new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicComp", Basiccomp), new SqlParameter("@Retrivalcomp", retrialbcomp),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
	
	
    public void CreateLogHistory(string ActTaken, string ActTakenBy, string Comment, string SrNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "FPA_Insert_PETROL_CLAIM_LOG_SUMMARY", new SqlParameter("@ACTION_PERFORMED", ActTaken),
                new SqlParameter("@PERFORMED_BY", ActTakenBy), new SqlParameter("@COMMENTS", Comment), new SqlParameter("@SERIAL_NO", SrNo));
        }
    }
    public void CreateRejectLogHistory(string ActTaken, string ActTakenBy, string Comment, string SrNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "FPA_InsertCLAIMLOGSUMMARY", new SqlParameter("@ACTION_PERFORMED", ActTaken),
                new SqlParameter("@PERFORMED_BY", ActTakenBy), new SqlParameter("@COMMENTS", Comment), new SqlParameter("@SERIAL_NO", SrNo));
        }
    }
    public DataTable getHistorybySRNO(string SNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getLogbySRNO", new SqlParameter("@SRNO", SNo))).Tables[0];
        }
    }
    public DataTable GetFpaDetail(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getFPaDetail", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable GetUserFpaDetail(string cycleid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getUserFPaDetail ", new SqlParameter("@cycleid", cycleid))).Tables[0];
        }
    }
    public DataTable GetUserFpaDetailCloesd(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getUserFPaDetailclosed ", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable GetCurrentCycleUser(string CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getCurrentCycleDetail ", new SqlParameter("@CYCLEID", CycleId))).Tables[0];
        }
    }
     public DataTable GetUserFpaDetailCloesdCycleId(string CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getUserFPaDetailclosedCycleId ", new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
    public DataTable FPA_getUserFPaDetailForStatus(string SAPID,string status)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getUserFPaDetailForStatus ", new SqlParameter("@SAPID", SAPID), new SqlParameter("@status", status))).Tables[0];
        }
    }
   public DataTable GetFpaHeads(string location, int DesId,string Carscheme,string Bu)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getFpaHeadsV1", new SqlParameter("@Loc", location), new SqlParameter("@Desid", DesId),
            new SqlParameter("@Carscheme", Carscheme), new SqlParameter("@Bu", Bu))).Tables[0];
        }
    }
    public DataTable GetSalaryHeads(string location, int DesId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getSalHeads", new SqlParameter("@Loc", location), new SqlParameter("@Desid", DesId))).Tables[0];
        }
    }

    public int InsertMyApprovalClaim(string gridSalaryHeadXml, string gridFPAHeadXml, int CycleId, int FpaId, string status, string SapId, int NPS, string PmrNo, string Nominee, string AsBank, string Relationship)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_insMyApprovalFpa",
            new SqlParameter("@CycleId", CycleId), new SqlParameter("@SAPID", SapId), new SqlParameter("@NPS", NPS),
            new SqlParameter("@gridFPAHeadXml", gridFPAHeadXml), new SqlParameter("@FPAID", FpaId),new SqlParameter("@PranNo", PmrNo),
             new SqlParameter("@Nominee", Nominee), new SqlParameter("@AssociatedBank", AsBank), new SqlParameter("@Relationship", Relationship),
            new SqlParameter("@gridSalaryHeadXml", gridSalaryHeadXml), new SqlParameter("@status", status)));
        }
    }
    //public int InsertMyApprovaltoFPA(string gridSalaryHeadXml, string gridFPAHeadXml, int CycleId, int FpaId, string status, string SapId)
    //{
    //    using (SqlConnection con = ReturnSqlconnection())
    //    {
    //        return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_InsertToFPA",
    //        new SqlParameter("@CycleId", CycleId), new SqlParameter("@SAPID", SapId),
    //        new SqlParameter("@gridFPAHeadXml", gridFPAHeadXml), new SqlParameter("@FPAID", FpaId),
    //        new SqlParameter("@gridSalaryHeadXml", gridSalaryHeadXml), new SqlParameter("@status", status)));
    //    }
    //}
  
    
    public DataTable GetSalaryHeadsDraft(string Loc, int DesId, int CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getSalHeadsDraft", new SqlParameter("@Loc", Loc),
                new SqlParameter("@Desid", DesId), new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
    public DataTable GetFpaheadsDraftDraft(string Loc, int DesId, int CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getFPAHeadsDraft", new SqlParameter("@Loc", Loc),
                new SqlParameter("@Desid", DesId), new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
    
    public int InsertMyApprovaltoFPA(int CycleId, int FpaId, string Status)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_UpdateFPACycle",
            new SqlParameter("@CycleId", CycleId), new SqlParameter("@FPAID", FpaId),
            new SqlParameter("@status", Status)));
        }
    }
     public int UpdateRejectionByFPA(int CycleId, int FpaId, string Status,string RejectionComment)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_UpdateFPACycleRejection",
            new SqlParameter("@CycleId", CycleId), new SqlParameter("@FPAID", FpaId),
            new SqlParameter("@status", Status), new SqlParameter("@rejectioncomment", RejectionComment)));
        }
    }

    public int InsertFpaDisable(string Sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_InsertDisable",
            new SqlParameter("@SAPID", Sapid)));
        }
    }

    public DataTable GetEmployeeForBulkInitiate(string Locname,string sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getBulkInitiateemp", new SqlParameter("@Loc", Locname), new SqlParameter("@Sapid", sapid))).Tables[0];
        }

    }



  public int FPA_Insert_year_end(string desId, string Loc,string Owner)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_Insert_year_end",
            new SqlParameter("@DisId", desId), new SqlParameter("@Loc", Loc), new SqlParameter("@owner", Owner)));
        }
    }

    public string ReturnFpaAdminSapId(string UserSapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_ReturnFpaAdminSapId",
            new SqlParameter("@UserSapId", UserSapId)));
        }
    }

    public DataTable getFpaheadsDetailByUser(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_GetFpaHeadsByEmployee", new SqlParameter("@SapId", SapId))).Tables[0];
        }
    }

    public int GetTotalFpaBalance(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_GetTotalFpaBalance",
            new SqlParameter("@EmpId", SapId)));
        }
    }

    public int ClaimInProcess(string SapId, string SerialNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_ClaimInProcess",
            new SqlParameter("@SapId", SapId), new SqlParameter("@SerialNumber", SerialNumber)));
        }
    }

    public int GetAllocateAmount(string SapId, string FpaHeadId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_GetAllocateAmount",
            new SqlParameter("@EmpId", SapId), new SqlParameter("@Fpa_Head_Id", FpaHeadId)));
        }
    }

    public string GetGlcode(string SapId, string FpaHeadId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_GetGlcode",
            new SqlParameter("@EmpId", SapId), new SqlParameter("@Fpa_Head_Id", FpaHeadId)));
        }
    }

    public int GetHeadAmount(string SapId, string FpaHeadId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_GetHeadBalance",
            new SqlParameter("@EmpId", SapId), new SqlParameter("@Fpa_Head_Id", FpaHeadId)));
        }
    }
    
    public int InsertFpaClaimDetail(string ECode,string SerialNo, string PayMod, string CurrStatus, string comments, string BU,string SAPID, string owner,string CC,string TotalAmount, string XmlFpaClaimDetail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFPACLAIMDETAIL",
            new SqlParameter("@Ecode", ECode), new SqlParameter("@CostCenter", CC), new SqlParameter("@CurrentStatus", CurrStatus), new SqlParameter("@Serial_No", SerialNo),
            new SqlParameter("@ModeOfPayment", PayMod), new SqlParameter("@BU", BU), new SqlParameter("@SAPID", SAPID),
            new SqlParameter("@OWNER", owner.Trim()), new SqlParameter("@Comments", comments), new SqlParameter("@TotAmt", TotalAmount), new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail)));
        }
    }
    public int InsertDeleSubmitClaimDetail(string ECode, string SerialNo, string PayMod, string CurrStatus, string comments, string BU, string SAPID, string owner,string Delegate ,string CC, string TotalAmount, string XmlFpaClaimDetail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_DeleFPACLAIMDETAIL",
            new SqlParameter("@Ecode", ECode), new SqlParameter("@CostCenter", CC), new SqlParameter("@CurrentStatus", CurrStatus), new SqlParameter("@Serial_No", SerialNo),
            new SqlParameter("@ModeOfPayment", PayMod), new SqlParameter("@BU", BU), new SqlParameter("@SAPID", SAPID), new SqlParameter("@Loginuser", Delegate),
            new SqlParameter("@OWNER", owner.Trim()), new SqlParameter("@Comments", comments), new SqlParameter("@TotAmt", TotalAmount), new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail)));
        }
    }
    public int InsertDeleFpaClaimDetail(string ECode, string SerialNo, string PayMod, string CurrStatus, string comments, string BU, string SAPID, string owner,string LoginUser, string CC, string TotalAmount, string XmlFpaClaimDetail)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_DeleFPACLAIMDETAIL",
            new SqlParameter("@Ecode", ECode), new SqlParameter("@CostCenter", CC), new SqlParameter("@CurrentStatus", CurrStatus), new SqlParameter("@Serial_No", SerialNo),
            new SqlParameter("@ModeOfPayment", PayMod), new SqlParameter("@BU", BU), new SqlParameter("@SAPID", SAPID),new SqlParameter("@Loginuser", LoginUser),
            new SqlParameter("@OWNER", owner.Trim()), new SqlParameter("@Comments", comments), new SqlParameter("@TotAmt", TotalAmount), new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail)));
        }
    }

    public int InsertFpaClaimDetailByApprover(string ECode, string SerialNo, string PayMod, string CurrStatus, string comments, string BU, string SAPID, string owner, string CC, string TotalAmount, string XmlFpaClaimDetail, string FpaEmplyeeDistribution,
        string Deviation, string DComments) 
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFPACLAIMDETAILByApprover",
            new SqlParameter("@Ecode", ECode), new SqlParameter("@CostCenter", CC), new SqlParameter("@CurrentStatus", CurrStatus), new SqlParameter("@Serial_No", SerialNo),
            new SqlParameter("@ModeOfPayment", PayMod), new SqlParameter("@BU", BU), new SqlParameter("@SAPID", SAPID), new SqlParameter("@XmlFpaEmplyeeDistribution", FpaEmplyeeDistribution),
            new SqlParameter("@OWNER", owner), new SqlParameter("@Comments", comments), new SqlParameter("@TotAmt", TotalAmount), new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail),
            new SqlParameter("@Deviation", Deviation),new SqlParameter("@DComments", DComments)));
        }
    }

    public string GetFpaCyleId(object SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "sp_GetCyleId",
            new SqlParameter("@EmpId", SapId)));
        }
    }

    public DataTable ApproverDashBoard(string owner)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_FPAapproverDashBoard", new SqlParameter("@OWNER", owner))).Tables[0];
        }
    }

    public DataTable getMyProClaimDocs(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_MyClaimDocuments", new SqlParameter("@login", SAPID))).Tables[0];
        }
    }

    public DataTable getFpadetailData(string SerialNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_FpaDetailData",
                new SqlParameter("@SerialNo", SerialNumber))).Tables[0];
        }
    }

    public int UpdateClaimDataBySRNO(string serialno, string status,string Comments)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_UpdatestatusDataBySRNO",
            new SqlParameter("@SerialNo", serialno), new SqlParameter("@Status", status), new SqlParameter("@Comment", Comments)));
        }
    }

    public int UPD(string fpadistribution)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "UPDATE_EMPLOYEE",
            new SqlParameter("@XmlFpaEmplyeeDistribution", fpadistribution)));
        }
    }

    public DataSet GetProcessCalim(string EmpId, string Year)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_GetProcessClaim ", new SqlParameter("@EmpId", EmpId), new SqlParameter("@Year", Year)));
        }
    }
    public DataTable GetClosedProcessCalim(string EmpId, string Year)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_GetclosedProcessClaim ", new SqlParameter("@EmpId", EmpId), new SqlParameter("@Year", Year))).Tables[0];
        }
    }

    public string GetTotalRemAmount(string Empsapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {

            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_GetTotalRemAmount",
                 new SqlParameter("@SapId", Empsapid)));
        }
    }
    public int InsertMyApprovaltoFPA(string gridSalaryHeadXml, string gridFPAHeadXml, int CycleId, int FpaId, string status, string SapId, string UserComment, int NPS, string PmrNo, string Nominee, string AsBank, string Relationship)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_InsertToFPA",
            new SqlParameter("@CycleId", CycleId), new SqlParameter("@SAPID", SapId),new SqlParameter("@UserComment",UserComment),
            new SqlParameter("@gridFPAHeadXml", gridFPAHeadXml), new SqlParameter("@FPAID", FpaId), new SqlParameter("@NPS", NPS),
            new SqlParameter("@PranNo", PmrNo),
             new SqlParameter("@Nominee", Nominee), new SqlParameter("@AssociatedBank", AsBank), new SqlParameter("@Relationship", Relationship),
            new SqlParameter("@gridSalaryHeadXml", gridSalaryHeadXml), new SqlParameter("@status", status)));
        }
    }
    public int InsertMyApprovaltoFPAForGM(string gridSalaryHeadXml, string gridFPAHeadXml, int CycleId, int FpaId, string status, string SapId, int Basiccomp, int retrialbcomp, int basicretrial, string UserComment,int NPS,string PmrNo,string Nominee,string AsBank,string Relationship)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_InsertToFPAForGM",
            new SqlParameter("@CycleId", CycleId), new SqlParameter("@SAPID", SapId), new SqlParameter("@UserComment", UserComment),
            new SqlParameter("@gridFPAHeadXml", gridFPAHeadXml), new SqlParameter("@FPAID", FpaId), new SqlParameter("@NPS", NPS),
            new SqlParameter("@gridSalaryHeadXml", gridSalaryHeadXml), new SqlParameter("@status", status), new SqlParameter("@PranNo", PmrNo),
             new SqlParameter("@Nominee", Nominee), new SqlParameter("@AssociatedBank", AsBank), new SqlParameter("@Relationship", Relationship),
           new SqlParameter("@BASIC_COMPONENT", Basiccomp), new SqlParameter("@BASIC_RETIRALS_AVAILED", basicretrial), new SqlParameter("@RETIRAL_BENEFITS", retrialbcomp)));
        }
    }

    public DataTable FillStatus()
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_GetProcessStatus ")).Tables[0];
        }
    }

    public int CheckUserStatus(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_CheckUserStatus",
                 new SqlParameter("@SapId", SapId)));
        }
    }

   public DataTable GetFpaEmployeeStatusDetail(string Year, string status,string loc,string Desid,string sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_GetFpaEmployeeStatusDetail ", new SqlParameter("@Year", Year), new SqlParameter("@Status", status),
                  new SqlParameter("@Loc", loc),new SqlParameter("@Sapid", sapid)
                , new SqlParameter("@DesId", Desid))).Tables[0];
        }
    }
    
    public int CheckInitiateStatus(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_CheckInitiateStatus",
                 new SqlParameter("@SapId", SapId)));
        }
    }
    public DataTable CheckClosedStatus(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_SP_CheckClosedStatus ", new SqlParameter("@SapId", SapId))).Tables[0];
        }
        
    }


    public DataTable getCarSchemeEmployee(string locname, int Desid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_CarSchemeEmployee", new SqlParameter("@Loc", locname), new SqlParameter("@desid", Desid))).Tables[0];
        }
    }

    public int UpdateCarscheme(string EmpSapId, string CarSheme, string CarNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_UpdateCarscheme",
                 new SqlParameter("@EmpSapId", EmpSapId), new SqlParameter("@CarSheme", CarSheme), new SqlParameter("@CarNumber", CarNumber)));
        }
    }
   // Petrol Claim Section -----------------------------//
    public string getTotalPterolAmountById(string EmpId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_TotalPterolAmountById",
                 new SqlParameter("@EmpId", EmpId)));
        }
    }
 public string getTotalPterolAmount1(string EMPID,string FpaId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_TotalPterolAmountV1",
                 new SqlParameter("@empid", EMPID), new SqlParameter("@FpaId", FpaId)));
        }
    }
     public string getTotalPterolAmount(string EMPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_TotalPterolAmount",
                 new SqlParameter("@empid", EMPID)));
        }
    }
public string getTotalPterolAmountbycycle(string CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_TotalPterolAmountCycleId",
                 new SqlParameter("@CycleId", CycleId)));
        }
    }

    public string CheckCarscheme(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_CheckCarscheme",
                 new SqlParameter("@SapId", SapId)));
        }
    }



    public int InsertPetrolClaimD(string XmlFpaClaimDetail,string SerialNo,string Glcode,string Login,string SapId,string Comments,string Status)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_INSPETROLCLAIM",
                 new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail), new SqlParameter("@SERIAL_NO", SerialNo),
                 new SqlParameter("@Glcode", Glcode), new SqlParameter("@Login", Login), new SqlParameter("@SapId", SapId),
                 new SqlParameter("@Comments", Comments), new SqlParameter("@Status", Status)));
        }
    }
	
	
	public int InsertPetrolClaimDetail(string XmlFpaClaimDetail, string SerialNo, string Glcode, string Login, string SapId, string Comments, string Status,string CycleId,double ToTAmt,string BU)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_INSPETROLCLAIMApp",
                 new SqlParameter("@XmlDataGridItems", XmlFpaClaimDetail), new SqlParameter("@SERIAL_NO", SerialNo),
                 new SqlParameter("@Glcode", Glcode), new SqlParameter("@Login", Login), new SqlParameter("@SapId", SapId),
                 new SqlParameter("@Comments", Comments), new SqlParameter("@Status", Status), new SqlParameter("@CycleId", CycleId),
                 new SqlParameter("@ToTAmt", ToTAmt), new SqlParameter("@BU", BU)));
        }
    }

    public DataTable getMyPetrolClaimDocs(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_MyPetrolClaimDocuments", new SqlParameter("@login", SAPID))).Tables[0];
        }
    }

    public DataTable PetrolApproverDashBoard(string owner)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "sp_FPAPetrolapproverDashBoard", new SqlParameter("@OWNER", owner))).Tables[0];
        }
    }

    public DataTable getPetrolClaimSerialNo(string SAPID, string serialNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getPetrolClaimSerialNo", new SqlParameter("@SAPID", SAPID), new SqlParameter("@SERIALNO", serialNumber))).Tables[0];
        }
    }

    public DataTable getPetroldetailData(string SerialNumber)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_PetrolDetailData",
                new SqlParameter("@SerialNo", SerialNumber))).Tables[0];
        }
    }

    public string  GetRateByLocation(string Loc)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_GetRateByLocation",
                 new SqlParameter("@Loc", Loc)));
        }
    }
    public int UpdatePterolClaimDataBySRNO(string serialno, string status, string Comments)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_PetrolClaimBySRNO",
            new SqlParameter("@SerialNo", serialno), new SqlParameter("@Status", status), new SqlParameter("@Comment", Comments)));
        }
    }



    public DataTable GetHeadClaimData(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_GetHeadClaimData",
                new SqlParameter("@SapId", SapId))).Tables[0];
        }
    }

    public DataTable FilterDataByHead(string HeadId, string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_FilterDataByHead",
                new SqlParameter("@SapId", SapId), new SqlParameter("@HeadId", HeadId))).Tables[0];
        }
    }

    public DataTable GetReport(string SapId)
    {
         using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_ReportByHead",
                new SqlParameter("@SapId", SapId))).Tables[0];
        }
    }
	public int DeleteFpaClaim(string SerialNo,string Type)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_DeleteFpaClaim",
            new SqlParameter("@SerialNo", SerialNo), new SqlParameter("@Type", Type)));
        }
    }
	///////Fi Admin

    public DataTable getDataForFIAdmin(string owner, string ststus, string Stext)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_DocsForFIAdmi", new SqlParameter("@owner", owner),
                new SqlParameter("@Ststus", ststus), new SqlParameter("@SText", Stext))).Tables[0];
        }
    }
	 public DataTable getFpaClaimHead(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFpaClaimHead", new SqlParameter("@SapId", SapId))).Tables[0];
        }
    }
	
	public DataTable GetFpaheadsUserDraft(string Loc, int DesId, int CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_GetFpaheadsUserDraft", new SqlParameter("@Loc", Loc),
                new SqlParameter("@Desid", DesId), new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
    public DataTable GetSalaryHeadsDraftUser(string Loc, int DesId, int CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_GetSalaryHeadsDraftUser", new SqlParameter("@Loc", Loc),
                new SqlParameter("@Desid", DesId), new SqlParameter("@CycleId", CycleId))).Tables[0];
        }
    }
	public void LogSummary(string ActTaken, string ActTakenBy, string Comment, string CycleId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "FPA_LOG_SUMMARY", new SqlParameter("@ACTION_PERFORMED", ActTaken),
                new SqlParameter("@PERFORMED_BY", ActTakenBy), new SqlParameter("@COMMENTS", Comment), new SqlParameter("@CycleId", CycleId));
        }
    }
	public int CheckYearEnd(string Sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "Fpa_sp_CheckYearEnd",
                 new SqlParameter("@Sapid", Sapid)));
        }
    }
	public int checkYearEndBySerailNo(string Sapid, string serialNumber)
    {

        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "Fpa_sp_CheckYearEndBySerailNo",
            new SqlParameter("@SerailNo", serialNumber), new SqlParameter("@Sapid", Sapid)));
        }
    }
	public int checkYearEndBySerailNoPterol(string Sapid, string serialNumber)
    {

        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "Fpa_sp_CheckYearEndBySerailNoPetrol",
            new SqlParameter("@SerailNo", serialNumber), new SqlParameter("@Sapid", Sapid)));
        }
    }
 public DataTable getLocationTrans(string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getLocationFiAdmin", new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    public DataTable getDesignationTrans(string Locname, string SAPID)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getDesignationTrans", new SqlParameter("@Loc", Locname), new SqlParameter("@SAPID", SAPID))).Tables[0];
        }
    }
    
	 public DataTable getFPAEmployeeTrans(string Locname, int DesId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getFpaEmployeeTrans", new SqlParameter("@Loc", Locname), new SqlParameter("@desid", DesId))).Tables[0];
        }

    }

     public int UpdateEmployeeTransaction(string Sapid, string CycleId, string DesId, string HeadId, string Amount, string SerialNo)
     {
         using (SqlConnection con = ReturnSqlconnection())
         {
             return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_InsertEmpoyeeTransaction",
             new SqlParameter("@SerailNo", SerialNo), new SqlParameter("@Sapid", Sapid), new SqlParameter("@CycleId", CycleId),
              new SqlParameter("@DesId", DesId), new SqlParameter("@HeadId", HeadId), new SqlParameter("@Amount", Amount)));
         }
     }
	 
	  public DataTable getClosedDataForFIAdmin(string UserLogin,string SrchTxt, string year)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "sp_GetProcessClaim_FIADMIN", 
                new SqlParameter("@UserLogin", UserLogin),new SqlParameter("@SearchText", SrchTxt),
                new SqlParameter("@Year", year))).Tables[0];
        }
    }
public DataTable GetFpaheadsClosed(string Loc, int DesId, int CycleId)
     {
         using (SqlConnection con = ReturnSqlconnection())
         {
             return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getFPAHeadsClosed",
                  new SqlParameter("@CycleId", CycleId))).Tables[0];
         }
     }
     public DataTable GetSalaryClosed(string Loc, int DesId, int CycleId)
     {
         using (SqlConnection con = ReturnSqlconnection())
         {
             return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_getSalHeadsClosed", new SqlParameter("@CycleId", CycleId))).Tables[0];
         }
     }

 public DataTable GetFpaInitationStatus(string status,string Bu, string FpaSapid)
     {
         using (SqlConnection con = ReturnSqlconnection())
         {
             return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure,
                 "FPA_InitiateStatusReport", new SqlParameter("@status", status), new SqlParameter("@BU", Bu), new SqlParameter("@FpaadminSapid", FpaSapid))).Tables[0];
         }
     }

public DataTable getDataForFIAdminPetrol(string owner, string ststus, string Stext)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "FPA_PetrolDocsForFIAdmi", new SqlParameter("@owner", owner),
                new SqlParameter("@Ststus", ststus), new SqlParameter("@SText", Stext))).Tables[0];
        }
    }
    public int DeleteTravelLogHistory(string SerialNo, string DeletedBy, string Comment, string AppType, string UserType, string Sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "Sp_FIAdmin_DeleteHistory",
                new SqlParameter("@SerialNo", SerialNo), new SqlParameter("@DeletedBy", DeletedBy), new SqlParameter("@Comment", Comment), new SqlParameter("@AppType", AppType),
     new SqlParameter("@UserType", UserType), new SqlParameter("@DeletedUser", Sapid)));
        }
    }

    public DataTable GetFiAdminDeleteHistory(string FiSapaid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
              CommandType.StoredProcedure,
              "FPA_FiAdminDeleteHistory", new SqlParameter("@FiSapid", FiSapaid))).Tables[0];
        }
    }
 public DataTable GetUserFpaReInitiateDetail(string FpaSapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure,
                  "FPA_getEmployeeTest", new SqlParameter("@FpaSapId", FpaSapId))).Tables[0];
        }

    }

    public DataTable GetPendingFpaDetail(string Sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
              CommandType.StoredProcedure,
              "FPA_GetPendingFpaDetail", new SqlParameter("@Sapid", Sapid))).Tables[0];
        }
    }
    
  public int reinitiationTest(string FpaId, string DesId, string EmpSapId, string year, string commnets, int Fpa, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, string serial, int totSalAvail, string Carscheme,int BasicSalary)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaDataInsertTest",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId), new SqlParameter("@Carscheme", Carscheme),
            new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicSalary", BasicSalary),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }

    public int reinitiationGMFPANewTest(string FpaId, string DesId, string EmpSapId, string year, string commnets, int ctc, int cBasic, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, int Basiccomp, int retrialbcomp, string serial, int totSalAvail, string Carscheme)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_reFpaGMDataInsertTest",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets), new SqlParameter("@FPAID", FpaId), new SqlParameter("@Carscheme", Carscheme),
            new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicRetrialAvailed", Basiccomp), new SqlParameter("@Retrivalcomp", retrialbcomp),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
 public DataTable GetUserFpaInitiateDetail(string Ecode, string FpaSapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure,
                  "FPA_getFpaEmployeeTest", new SqlParameter("@Ecode", Ecode), new SqlParameter("@FpaSapId", FpaSapId))).Tables[0];
        }
     
    }
    public int InsertGMFPANewTest(string DesId, string EmpSapId, string year, string commnets, int ctc, int cBasic, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, int Basiccomp, int retrialbcomp, string serial, int totSalAvail, string Carscheme)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaGMDataInsertTest",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets), new SqlParameter("@Carsheme", Carscheme),
            new SqlParameter("@CTC", ctc), new SqlParameter("@CBasic", cBasic), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@BasicComp", Basiccomp), new SqlParameter("@Retrivalcomp", retrialbcomp),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail)));
        }
    }
    public int InsertFPANewTest(string DesId, string EmpSapId, string year, string commnets, int Fpa, int other1, int other2, int other3, int Locfpa, int totpetrol, int totreimavail, int clarental, int vehemi, int saf, int busdeduct, string serial, int totSalAvail, string Carscheme, int BasicSalary)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_NewFpaDataInsertTest",
            new SqlParameter("@DESID", DesId), new SqlParameter("@SAPID", EmpSapId), new SqlParameter("@YEAR", year),
            new SqlParameter("@commnets", commnets),
            new SqlParameter("@Fpa", Fpa), new SqlParameter("@other1", other1), new SqlParameter("@other2", other2),
            new SqlParameter("@other3", other3), new SqlParameter("@Locfpa", Locfpa), new SqlParameter("@totpetrol", totpetrol),
            new SqlParameter("@totreimavail", totreimavail), new SqlParameter("@clarental", clarental),
            new SqlParameter("@vehemi", vehemi), new SqlParameter("@saf", saf), new SqlParameter("@Carsheme", Carscheme),
            new SqlParameter("@busdeduct", busdeduct), new SqlParameter("@serial", serial), new SqlParameter("@totSalAvail", totSalAvail),new SqlParameter("@BasicSalary", BasicSalary)));
        }
    }

  public DataTable GetEmployeeForBulkReInitiate(string Locname, string sapid)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
                CommandType.StoredProcedure,
                "FPA_getBulkREInitiate", new SqlParameter("@Loc", Locname), new SqlParameter("@FpaSapId", sapid))).Tables[0];
        }

    }
    public string GetFpaid(string UserSapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_getFPAid",
            new SqlParameter("@SAPID", UserSapId)));
        }
    }

    public int CheckFPA_IsActive(string SapId)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_IsActive",
                 new SqlParameter("@SapId", SapId)));
        }
    }

    public string GetPetrolRate(string BU, string JDate)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_GetRate",
            new SqlParameter("@BU", BU), new SqlParameter("@JDate", JDate)));
        }
    }

    public int PCLUpdateClaimstatustoHold(string serialno, string status, string Comments)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "FPA_SP_PCLUpdateHoldstatus",
            new SqlParameter("@SerialNo", serialno), new SqlParameter("@Status", status), new SqlParameter("@Comment", Comments)));
        }
    } 

    public string InsertPhoto(string sapid, string filename, string filepath, string createby, string filesize, string onlyfileName, string SrNo, string LastFileNo, string MainFileNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString((SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "NL_PHOTO_INS",
                new SqlParameter("@User_SapId", sapid), new SqlParameter("@FileName", filename),
                new SqlParameter("@FilePath", filepath), new SqlParameter("@Create_By", createby), new SqlParameter("@FileSize", filesize),
                new SqlParameter("@OnlyFileName", onlyfileName), new SqlParameter("@App_SerialNo", SrNo),
                new SqlParameter("@LastFileNo", LastFileNo), new SqlParameter("@MainRowNo", MainFileNo))));
        }
    }
    //
    public string InsertPhoto_Buffer(string sapid, string filename, string filepath, string createby, string filesize, string onlyfileName, string SrNo, string LastFileNo, string MainFileNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString((SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "NL_PHOTO_INS_Buffer",
                new SqlParameter("@User_SapId", sapid), new SqlParameter("@FileName", filename),
                new SqlParameter("@FilePath", filepath), new SqlParameter("@Create_By", createby), new SqlParameter("@FileSize", filesize),
                new SqlParameter("@OnlyFileName", onlyfileName), new SqlParameter("@App_SerialNo", SrNo),
                new SqlParameter("@LastFileNo", LastFileNo), new SqlParameter("@MainRowNo", MainFileNo))));
        }
    }

    public DataTable GetPhotoDetail(string SrNo, int RowNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(),
              CommandType.StoredProcedure,
              "NL_GetDecDetails", new SqlParameter("@SerialNo", SrNo), new SqlParameter("@MainRowNo", RowNo))).Tables[0];
        }
    }

    public DataSet IsExpExists_InDB(string SAPID, int HeadID, int ExpAmt, string ExpDate, string BillNo, string AppSrNo)
    {
        ///string srno = string.Empty;
        using (SqlConnection con = ReturnSqlconnection())
        {
            return SqlHelper.ExecuteDataset(ReturnSqlconnection(),
           CommandType.StoredProcedure,
           "FPA_IsExpExists_InDB", new SqlParameter("@SAPID", SAPID),
           new SqlParameter("@HeadID", HeadID), new SqlParameter("@ExpAmt", ExpAmt),
           new SqlParameter("@ExpDate", ExpDate), new SqlParameter("@BillNo", BillNo),
           new SqlParameter("@AppSrNo", AppSrNo));
        }
    }

    public DataTable getOnHoldDocs(string SAPID, string Srno)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return (SqlHelper.ExecuteDataset(ReturnSqlconnection(), CommandType.StoredProcedure, "NL_OnHoldDocuments", new SqlParameter("@SapId", SAPID), new SqlParameter("@App_Serial_No", Srno))).Tables[0];
        }
    }

    public string UpdateFileNoPrefix_OnHoldDocs(string SrNo)
    {
        using (SqlConnection con = ReturnSqlconnection())
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "NL_UpdateFileNoPrefix_OnHoldDocs",
                 new SqlParameter("@App_SerialNo", SrNo)));
        }
    }

}
