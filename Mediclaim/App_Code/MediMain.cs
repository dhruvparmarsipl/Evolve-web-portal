using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for MediMain
/// </summary>
public class MediMain
{
    #region GLOBAL DATA MEMBERS
    SqlConnection oSqlConn;
    SqlCommand ocmd;
    CommonFunction oclsCF;
    Utility.Service Util = new Utility.Service();
    string logfile = "C:\\Eicher\\All Logs\\Medi";
    public const string Mailbody = "<table width='552' border='0' align='left' cellpadding='0' cellspacing='0' style='border:#CCCCCC solid 1px;'><tr><td align='left' valign='top'>" +
                               "<table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='18' align='left' valign='top'>&nbsp;</td><td align='left' valign='top'>" +
                               "<table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='64%' align='left' valign='top'>" +
                               "<p style='font-family:Arial, Geneva, sans-serif; font-size:11px; line-height:22px; color:#303030; margin:0; padding:0;'>" +
                               "##Name ##Cont<br/><a href='#' target='_blank'>Click to view</a> <br />Regards,<br>HR Team" +
                               "</td></tr><tr><td width='64%' align='left' valign='top'><p style='font-family:Arial, Geneva, sans-serif; font-size:10px; line-height:22px; color:#303030; margin:0; padding:0;'>" +
                               "Please do not reply as this is a system generated mail.</p></td></tr></table></td></tr></table></td></tr></table>";
    #endregion

    #region CONSTRUCTOR
    public MediMain()
    {
        oSqlConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["Sqlconn"].ToString());
    }
    #endregion

    #region MEDICLAIM METHODS
  public Int64 InsertMediclaim(string SapID, int PlanID, string NUMOF_DEPENDANTS, string SumInsured, int Status, string Premium, string SrNo, string YearOfClaim, string Mobile, string Email, string ActPrem, string OwnerSAPID, string XmlDataGridItems,string hrcomments,string UserBloodgoup,
        string careof,string street,string address2,string postalcode,string city,string Bname,string Ifsccode,string AcNo,string AchName)
    {
        //Return Inserted Record's ID
        Int64 DBID = 0;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spInsertMediclaimMain";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapID));
        ocmd.Parameters.Add(new SqlParameter("@PLANID", PlanID));
        ocmd.Parameters.Add(new SqlParameter("@NUMOF_DEPENDANTS", NUMOF_DEPENDANTS));
        ocmd.Parameters.Add(new SqlParameter("@SUM_INSURED_AMOUNT", SumInsured));
        ocmd.Parameters.Add(new SqlParameter("@STATUS", Status));
        ocmd.Parameters.Add(new SqlParameter("@EMP_PREMIUM", Premium));
        ocmd.Parameters.Add(new SqlParameter("@SERIAL_NO", SrNo));
        ocmd.Parameters.Add(new SqlParameter("@YEAROFCLAIM", YearOfClaim));
        ocmd.Parameters.Add(new SqlParameter("@MOBILE_NO", Mobile));
        ocmd.Parameters.Add(new SqlParameter("@EMAIL", Email));
        ocmd.Parameters.Add(new SqlParameter("@ACTUAL_PREMIUM_AMOUNT", ActPrem));
        ocmd.Parameters.Add(new SqlParameter("@OWNER_SAPID", OwnerSAPID));
        ocmd.Parameters.Add(new SqlParameter("@griddependent", XmlDataGridItems));
        ocmd.Parameters.Add(new SqlParameter("@hrcomments", hrcomments));
        ocmd.Parameters.Add(new SqlParameter("@UserBloodgoup", UserBloodgoup));
        // change on 23/12/2014
        
        ocmd.Parameters.Add(new SqlParameter("@careof", careof));
        ocmd.Parameters.Add(new SqlParameter("@street", street));
        ocmd.Parameters.Add(new SqlParameter("@address2", address2));
        ocmd.Parameters.Add(new SqlParameter("@postalcode", postalcode));
        ocmd.Parameters.Add(new SqlParameter("@city", city));

       // Account Detail 22/12/2016

        ocmd.Parameters.Add(new SqlParameter("@Bname", Bname));
        ocmd.Parameters.Add(new SqlParameter("@Ifsccode", Ifsccode));
        ocmd.Parameters.Add(new SqlParameter("@AcNo", AcNo));
        ocmd.Parameters.Add(new SqlParameter("@AchName", AchName));


        oclsCF = new CommonFunction();
        DBID = Convert.ToInt64(oclsCF.ExecuteScalar(ocmd));
        return DBID;
    }
    public int InsertDependantDetails(DataTable dtDependants, Int64 MainClaimID, string MainSapID, string SerialNo)
    {
        int res = 0;
        if (dtDependants.Rows.Count > 0)
        {
            for (int i = 0; i < dtDependants.Rows.Count; i++)
            {
                ocmd = new SqlCommand();
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.CommandText = "MEDI_spInsertDependants";
                ocmd.Parameters.Add(new SqlParameter("@MEDI_MAIN_ID", MainClaimID));
                ocmd.Parameters.Add(new SqlParameter("@MEDI_MAIN_SAPID", MainSapID));
                ocmd.Parameters.Add(new SqlParameter("@NAME", dtDependants.Rows[i]["Name_of_Dependant"].ToString()));
                ocmd.Parameters.Add(new SqlParameter("@DOB", dtDependants.Rows[i]["DOB(dd/mm/yyyy)"].ToString()));
                ocmd.Parameters.Add(new SqlParameter("@GENDER", dtDependants.Rows[i]["Gender"].ToString()));
                ocmd.Parameters.Add(new SqlParameter("@RELATION", dtDependants.Rows[i]["Relation"].ToString()));
                ocmd.Parameters.Add(new SqlParameter("@REMARKS", dtDependants.Rows[i]["Remarks"].ToString()));
                ocmd.Parameters.Add(new SqlParameter("@SERIAL_NO", SerialNo));
                ocmd.Parameters.Add(new SqlParameter("@BLOODGROUP", dtDependants.Rows[i]["Blood_Group"].ToString()));
                oclsCF = new CommonFunction();
                res = oclsCF.ExecuteProc(ocmd);
            }
        }
        return res;
    }
	public int InsertLogsByApp(string SerialNo, string Status, string SapID,string Comments)
    {
        int res = 0;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spInsertLOGbyApp";
        ocmd.Parameters.Add(new SqlParameter("@SERIALNO", SerialNo));
        ocmd.Parameters.Add(new SqlParameter("@STATUS", Status));
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapID));
        ocmd.Parameters.Add(new SqlParameter("@Comments", Comments));
        oclsCF = new CommonFunction();
        res = oclsCF.ExecuteProc(ocmd);
        return res;
    }
    public int InsertLogs(string SerialNo, string Status, string SapID)
    {
        int res = 0;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spInsertLOG";
        ocmd.Parameters.Add(new SqlParameter("@SERIALNO", SerialNo));
        ocmd.Parameters.Add(new SqlParameter("@STATUS", Status));
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapID));
        oclsCF = new CommonFunction();
        res = oclsCF.ExecuteProc(ocmd);
        return res;
    }
    public int CheckUserExistInMediclaim(string SAPID, string spName,int year)
    {
        int count = 0;
        string Year = DateTime.Now.Year.ToString();
        //Add Status below to check submitted or not
        oclsCF = new CommonFunction();
        ocmd = new SqlCommand();
        ocmd.CommandText = spName;
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SAPID));
        ocmd.Parameters.Add(new SqlParameter("@Year", year));
        ocmd.CommandType = CommandType.StoredProcedure;
        count = Convert.ToInt32(oclsCF.ExecuteScalar(ocmd));
        return count;
    }

    public int ApproveRejectMediclaim(string SerialNo, string OwnerSapID, string Comments, int Status)
    {
        int res = 0;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spApproveRejectMediclaim";
        ocmd.Parameters.Add(new SqlParameter("@SERIAL_NO", SerialNo));
        ocmd.Parameters.Add(new SqlParameter("@OWNER_SAPID", OwnerSapID));
        ocmd.Parameters.Add(new SqlParameter("@COMMENTS", Comments));
        ocmd.Parameters.Add(new SqlParameter("@STATUS", Status));
        oclsCF = new CommonFunction();
        res = oclsCF.ExecuteProc(ocmd);
        return res;
    }
    public DataTable BindPlans()
    {
        DataTable dt = new DataTable();
        string Query = "SELECT ID,PLAN_NAME,NUM_OF_DEPENDANTS FROM MEDI_MASTER_PLAN";
        oclsCF = new CommonFunction();
        dt = oclsCF.GetDataBySqlQuery(Query);
        return dt;
    }
    public DataTable BindHeader(string SapID)
    {
        DataTable dt = new DataTable();
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecSpGetDataByOneDBParam("@sapid", SapID, "MEDI_spGET_EMPDETAILS");
        return dt;
    }
    public string GetNewSRNumber(string key)
    {
        string SR_Number = string.Empty;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spCREATE_SRNO";
        ocmd.Parameters.Add(new SqlParameter("@KEY", key));
        oclsCF = new CommonFunction();
        SR_Number = oclsCF.ExecuteScalar(ocmd);
        return SR_Number;
    }
    public int GetAgeIDByAge(int Age)
    {
        int AgeID = 0;
        if (Age <= 35)
        {
            AgeID = 1;
        }
        else if (Age > 35 && Age <= 45)
        {
            AgeID = 2;
        }
        else if (Age > 45 && Age <= 55)
        {
            AgeID = 3;
        }
        else if (Age > 55 && Age <= 65)
        {
            AgeID = 4;
        }
        else
            AgeID = 5;

        return AgeID;
    }
    public DataTable GetDataSumInsured(int PlanID, int AGEID, string StaffOrExec)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spGetSumInsured";
        ocmd.Parameters.Add(new SqlParameter("@PlanID", PlanID));
        ocmd.Parameters.Add(new SqlParameter("@AgeID", AGEID));
        ocmd.Parameters.Add(new SqlParameter("@SorE", StaffOrExec));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
    public DataSet MediclaimBySerialNo(string SerialNo)
    {
        DataSet ds = new DataSet();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spMeidclaimBySerialNo";
        ocmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
        oclsCF = new CommonFunction();
        ds = oclsCF.ExecuteProcGetDataSet(ocmd);
        return ds;
    }
    public int DeleteMediclaim(string SerialNo)
    {
        int res = 0;
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spDeleteMediclaim";
        ocmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
        oclsCF = new CommonFunction();
        res = oclsCF.ExecuteProc(ocmd);
        return res;
    }
    public DataTable GetLogs(string SerialNo)
    {
        string Query = "SELECT ID,SERIALNO,STATUS,DATE,SAPID,Comments FROM MEDI_LOGS Where SERIALNO='" + SerialNo + "' order by DATE desc ";
        DataTable dt = new DataTable(); 
        oclsCF = new CommonFunction();
        dt = oclsCF.GetDataBySqlQuery(Query);
        return dt;
    }
    public int SendEmail(string ToAddress, string Content, string Subject, string Name)
    {
        int res = 0;
        try
        {
            string myMailBody = Mailbody.Replace("##Name", Name);
            myMailBody = myMailBody.Replace("##Cont", Content);
            //Util.Service objsvc = new WebReference.Service();
            Util.SendMail(ToAddress, "ewadmin@vecv.in", "", Subject, myMailBody);
        }
        catch (Exception ex)
        {
            res = 1;
            Util.Log("Mail Send  form ID -- " + ToAddress, logfile);
            //CommonFunction.InsertErrorLogs("Sending Email Error: " + ex.Message.ToString());
        }
        return res;
    }

    public int InsertPremiumTable(DataTable dt)
    {
        int ret = 0, ret1 = 0;
        //SqlTransaction transaction;
        oSqlConn.Open();
        //transaction = oSqlConn.BeginTransaction();
        try
        {
            SqlCommand ocmd = new SqlCommand("Truncate Table dbo.MEDI_PREMIUM_MASTER", oSqlConn);
            ocmd.CommandType = CommandType.Text;
            ret1 = ocmd.ExecuteNonQuery();
            if (ret1 != 0)
            {
                using (SqlBulkCopy osqlBulkCopy = new SqlBulkCopy(oSqlConn, SqlBulkCopyOptions.TableLock,null))
                {

                    osqlBulkCopy.BatchSize = dt.Rows.Count;
                    osqlBulkCopy.DestinationTableName = "MEDI_PREMIUM_MASTER";
                    foreach (var column in dt.Columns)
                        osqlBulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());

                    osqlBulkCopy.WriteToServer(dt);
                    ret = 1;
                    dt.Dispose();
                    oSqlConn.Close();
                }
            }
        }
        catch (Exception e)
        {
            ret = 0;
            Util.Log("Bulk Copy Section Error: -- " + e.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("Bulk Copy Section Error: " + e.Message.ToString());
            try
            {
                //transaction.Rollback();
            }
            catch (Exception ex)
            {
                //Enter in ErrorLog Table
                Util.Log("Rollback Issue: -- " + e.Message.ToString(), logfile);
               //CommonFunction.InsertErrorLogs("Rollback Issue: " + ex.Message.ToString());
            }
        }
        return ret;
    }
    public int InsertMasterPlanTable(DataTable dt)
    {
        int ret = 0, ret1 = 0;
        //SqlTransaction transaction;
        oSqlConn.Open();
        //transaction = oSqlConn.BeginTransaction();
        try
        {
            SqlCommand ocmd = new SqlCommand("Truncate Table dbo.MEDI_MASTER_PLAN", oSqlConn);
            ocmd.CommandType = CommandType.Text;
            ret1 = ocmd.ExecuteNonQuery();
            if (ret1 != 0)
            {
                using (SqlBulkCopy osqlBulkCopy = new SqlBulkCopy(oSqlConn, SqlBulkCopyOptions.TableLock, null))
                {
                    osqlBulkCopy.BatchSize = dt.Rows.Count;
                    osqlBulkCopy.DestinationTableName = "MEDI_MASTER_PLAN";
                    osqlBulkCopy.ColumnMappings.Add("MASTER_PLAN_ID", "ID");
                    osqlBulkCopy.ColumnMappings.Add("No_of_Dependent", "NUM_OF_DEPENDANTS");
                    osqlBulkCopy.ColumnMappings.Add("PlanName", "PLAN_NAME");
                    osqlBulkCopy.WriteToServer(dt);
                    ret = 1;
                    dt.Dispose();
                    oSqlConn.Close();
                }
            }
        }
        catch (Exception e)
        {
            ret = 0;
            Util.Log("Bulk Copy Section Error: -- " + e.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("Bulk Copy Section Error: " + e.Message.ToString());
            try
            {
                //transaction.Rollback();
            }
            catch (Exception ex)
            {
                //Enter in ErrorLog Table
                Util.Log("Rollback Issue: -- " + e.Message.ToString(), logfile);
                //CommonFunction.InsertErrorLogs("Rollback Issue: " + ex.Message.ToString());
            }
        }
        return ret;
    }
    #endregion

    #region DASHBOARD METHODS
    public DataTable GetUsersActiveMediclaim(string SAPID)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spUserActiveMediclaim";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SAPID));

        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
    public DataTable GetApproverActiveMediclaim(string OwnerSapID)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spApproverActiveMediclaim";
        ocmd.Parameters.Add(new SqlParameter("@OWNER_SAPID", OwnerSapID));

        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public DataTable MainMasterPlan(int PlanID)
    {
        CommonFunction oclsCF = new CommonFunction();
        string ques = "select distinct Master_Plan_ID,FAMILY_MEMBERS from MEDI_PREMIUM_MASTER where MASTER_PLAN_ID=" + PlanID;
        DataTable dt = oclsCF.GetDataBySqlQuery(ques);
        return dt;
    }
    #endregion

    public DataTable GetProcessCalim(string SapId)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spProcessClaim";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapId));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
    public string Get_Single_DataByPassingQuery(string Gid)
    {
        string query = string.Empty;
        SqlCommand ocmd = new SqlCommand();
        ocmd.CommandText = "Retrun_SAPID";
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.Parameters.Add(new SqlParameter("@LoginName", Gid));
        oclsCF = new CommonFunction();
        query = oclsCF.ExecuteScalar(ocmd);
        return query;
    }


    public DataTable Get_DataByPassingQuery(string Sapid)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spGetYear";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", Sapid));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public string GetEname(string loginname)
    {
        string query = string.Empty;
        SqlCommand ocmd = new SqlCommand();
        ocmd.CommandText = "Retrun_Ename";
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.Parameters.Add(new SqlParameter("@LoginName", loginname));
        oclsCF = new CommonFunction();
        query = oclsCF.ExecuteScalar(ocmd);
        return query;
        
    }
	 public string Get_Multy_DataByPassingQuery(string Bu,string Desid)
    {
        string query = string.Empty;
        SqlCommand ocmd = new SqlCommand();
        ocmd.CommandText = "Medi_SpGetEType";
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.Parameters.Add(new SqlParameter("@Bu", Bu));
        ocmd.Parameters.Add(new SqlParameter("@Desid", Desid));
        oclsCF = new CommonFunction();
        query = oclsCF.ExecuteScalar(ocmd);
        return query;
    }
	public int CheckPremiumExist(int year, string spName)
    {
        int count = 0;
        //Add Status below to check submitted or not
        oclsCF = new CommonFunction();
        ocmd = new SqlCommand();
        ocmd.CommandText = spName;
        ocmd.Parameters.Add(new SqlParameter("@Year", year));
        ocmd.CommandType = CommandType.StoredProcedure;
        count = Convert.ToInt32(oclsCF.ExecuteScalar(ocmd));
        return count;
    }
	public int DeleteMediClaim(string SerialNo)
    {
        int count = 0;
        oclsCF = new CommonFunction();
        ocmd = new SqlCommand();
        ocmd.CommandText = "sp_Medi_DeleteMediClaim" ;
        ocmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
        ocmd.CommandType = CommandType.StoredProcedure;
        count = Convert.ToInt32(oclsCF.ExecuteScalar(ocmd));
        return count;
    }
	 public string GetEmptype(string LoginId)
    {
        string query = string.Empty;
        SqlCommand ocmd = new SqlCommand();
        ocmd.CommandText = "Medi_SpGetEmpType";
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.Parameters.Add(new SqlParameter("@LoginId", LoginId));
        oclsCF = new CommonFunction();
        query = oclsCF.ExecuteScalar(ocmd);
        return query;
    }
	public DataTable GetProcessCalimbyHr(string SapId)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spProcessClaimByHr";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapId));

        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
	public int GetMediYear(string Sapid)
    {
        int count = 0;
        oclsCF = new CommonFunction();
        ocmd = new SqlCommand();
        ocmd.CommandText = "sp_Medi_GetMediYear";
        ocmd.Parameters.Add(new SqlParameter("@Sapid", Sapid));
        ocmd.CommandType = CommandType.StoredProcedure;
        count = Convert.ToInt32(oclsCF.ExecuteScalar(ocmd));
        return count;
    }
	 public DataTable GetHeadClaimData(string SapId,string Bu,string Year,string Status)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spGetMasterData";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", SapId));
        ocmd.Parameters.Add(new SqlParameter("@Bu", Bu));
        ocmd.Parameters.Add(new SqlParameter("@Year", Year));
        ocmd.Parameters.Add(new SqlParameter("@Status", Status));

        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public DataTable FilterDataByHead(string Id, string Sapid)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spGetDetailData";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", Sapid));
        ocmd.Parameters.Add(new SqlParameter("@MainId", Id));

        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public DataTable GetReport(string Sapid, string Bu, string Year, string Status)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "MEDI_spGetReportData";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", Sapid));
        ocmd.Parameters.Add(new SqlParameter("@Bu", Bu));
        ocmd.Parameters.Add(new SqlParameter("@Year", Year));
        ocmd.Parameters.Add(new SqlParameter("@Status", Status));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public DataTable getBU(string Sapid)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "Medi_getBu";
        ocmd.Parameters.Add(new SqlParameter("@SAPID", Sapid));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }

    public DataTable getStatus()
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "Medi_getStatus";     
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
    public DataTable GetMasterData(string sapid)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "Medi_GetMasterdata";
        ocmd.Parameters.Add(new SqlParameter("@SapId", sapid));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
    public DataTable GetDataForRelation(string Relation, String SapId)
    {
        DataTable dt = new DataTable();
        ocmd = new SqlCommand();
        ocmd.CommandType = CommandType.StoredProcedure;
        ocmd.CommandText = "Medi_getPrevDataRelation";
        ocmd.Parameters.Add(new SqlParameter("@Relation", Relation));
        ocmd.Parameters.Add(new SqlParameter("@SapId", SapId));
        oclsCF = new CommonFunction();
        dt = oclsCF.ExecuteProcGetData(ocmd);
        return dt;
    }
public int RevertMediclaimToUserbyAdmin(string SerialNo, string LoginUser, string Comment)
    {
        int count = 0;
        oclsCF = new CommonFunction();
        ocmd = new SqlCommand();
        ocmd.CommandText = "sp_Medi_MediRevertToUser";
        ocmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
        ocmd.Parameters.Add(new SqlParameter("@LoginUser", LoginUser));
        ocmd.Parameters.Add(new SqlParameter("@Comment", Comment));
        ocmd.CommandType = CommandType.StoredProcedure;
        count = Convert.ToInt32(oclsCF.ExecuteScalar(ocmd));
        return count;
    }
}
