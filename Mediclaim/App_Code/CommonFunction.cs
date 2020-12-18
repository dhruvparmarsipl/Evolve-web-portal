using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for CommonFunction
/// </summary>
public class CommonFunction
{
    //SqlConnection oSqlCon;
    SqlDataAdapter oDA;
    SqlCommand oCmd;
    Utility.Service Util = new Utility.Service();
    string logfile = "C:\\Eicher\\All Logs\\Medi";
	public CommonFunction()
	{
        //oSqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MEDI_ConString"].ToString());
	}
    //Status :--used
    public DataTable GetDataBySqlQuery(string strQuery)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection oSqlCon = ReturnSqlConnection())
            {
                oDA = new SqlDataAdapter(strQuery, oSqlCon);
                oDA.Fill(dt);
            }
        }
        catch (Exception e)
        {
            //Util.Log("Database Interaction Error -- " + e.Message.ToString(), logfile);
           // CommonFunction.InsertErrorLogs("Database Interaction Error: " + e.Message.ToString());
        }
        return dt;
    }
    //Function will return one records from first row and first column
    public string ExecuteScalar(SqlCommand ocmd)
    {
        string Result = string.Empty;
        try
        {
            using (SqlConnection conn = ReturnSqlConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else
                {
                    conn.Close();
                }
                ocmd.Connection = conn;
                //oCmd.Connection.Open();
                Result = ocmd.ExecuteScalar().ToString();
                ocmd.Connection.Close();
            }
        }
        catch (Exception ex)
        {
            //Util.Log("Database Interaction Error -- " + ex.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("Database Interaction Error: " + ex.Message.ToString());
            return ex.Message.ToString();
        }
        return Result;

    }
    public int ExecuteProc(SqlCommand ocmd)
    {
        int res = 0;
        try
        {
            using (SqlConnection conn = ReturnSqlConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else
                {
                    conn.Close();
                }
                ocmd.Connection = conn;
                res = ocmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            //Util.Log("Database Interaction Error -- " + ex.Message.ToString(), logfile);
           //CommonFunction.InsertErrorLogs("Database Interaction Error: " + ex.Message.ToString());
        }
        return res;
    }
    public DataTable ExecuteProcGetData(SqlCommand ocmd)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection oSqlCon = ReturnSqlConnection())
            {
                if (oSqlCon.State == ConnectionState.Closed)
                {
                    oSqlCon.Open();
                }
                else
                {
                    oSqlCon.Close();
                }
                ocmd.Connection = oSqlCon;
                oDA = new SqlDataAdapter(ocmd);
                oDA.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            //Util.Log("Database Interaction Error -- " + ex.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("Database Interaction Error: " + ex.Message.ToString());
        }
        return dt;
    }
    public DataSet ExecuteProcGetDataSet(SqlCommand ocmd)
    {
        DataSet DS = new DataSet();
        try
        {

            using (SqlConnection oSqlCon = ReturnSqlConnection())
            {
                if (oSqlCon.State == ConnectionState.Closed)
                {
                    oSqlCon.Open();
                }
                else
                {
                    oSqlCon.Close();
                }
                ocmd.Connection = oSqlCon;
                oDA = new SqlDataAdapter(ocmd);
                oDA.Fill(DS);
            }
        }
        catch (Exception e)
        {
            //Util.Log("Database Interaction Error -- " + e.Message.ToString(), logfile);
           // CommonFunction.InsertErrorLogs("Database Interaction Error: " + e.Message.ToString());
        }
        return DS;
    }
    public DataTable ExecSpGetDataByOneDBParam(string DBParamName, string DBParamValue, string spName)
    {
        DataTable dt = new DataTable();
        SqlCommand ocmd = new SqlCommand();
        using (SqlConnection oSqlCon = ReturnSqlConnection())
        {
            ocmd.Connection = oSqlCon;
            ocmd.CommandType = CommandType.StoredProcedure;
            ocmd.CommandText = spName;
            ocmd.Parameters.Add(new SqlParameter(DBParamName, DBParamValue));
            CommonFunction oCF = new CommonFunction();
            dt = oCF.ExecuteProcGetData(ocmd);
        }
        return dt;
    }
    public  SqlConnection ReturnSqlConnection()
    {
        SqlConnection con = null;
        try
        {
            string strConn = ConfigurationManager.AppSettings["Sqlconn"].ToString();
            con = new SqlConnection(strConn);
            con.Open();
        }
        catch (Exception ex)
        {
            //Util.Log("Database Interaction Error -- " + ex.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("Connection Related Exception: " + ex.Message.ToString());
        }
        finally
        {
           con.Close();
        }
        return con;
    }
    public string GetSapIdByLoginName(string LoginName)
    {
        string SAPID = string.Empty;
        try
        {
            using (SqlConnection con = ReturnSqlConnection())
            {
                SqlCommand ocmd = new SqlCommand();
                ocmd.Connection = con;
                ocmd.CommandText = "Retrun_SAPID";
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.Parameters.Add(new SqlParameter("@LoginName", LoginName));
                SAPID = ExecuteScalar(ocmd);
            }
        }
        catch (Exception ex)
        {
            //Util.Log("Database Interaction Error -- " + ex.Message.ToString(), logfile);
            //CommonFunction.InsertErrorLogs("GetSapIdByLoginName Exception: " + ex.Message.ToString());
        }
        return SAPID;
    }

   
}
