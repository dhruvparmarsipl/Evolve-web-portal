using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for UserFpa
/// </summary>
public class UserFpa
{
    public UserFpa()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string FpaId { get; set; }
    public string Ctc { get; set; }
    public string Fpa { get; set; }
    public string CBasic { get; set; }
    public string Other1 { get; set; }
    public string Other2 { get; set; }
    public string Other3 { get; set; }
    public string Totpcredited { get; set; }
    public string LocFPA { get; set; }
    public string Basiccomponent { get; set; }
    public string Totalsalavailed { get; set; }
    public string TotReimAvailed { get; set; }
    public string ClaRental { get; set; }
    public string VehEmi { get; set; }
    public string SAF { get; set; }
    public string BusDeduct { get; set; }
    public string BasicSalary { get; set; }
    public string NPS { get; set; }

    // Fpa Declaration Page Property
    public string BasicRetrials { get; set; }
    public string Retrialbcomponent { get; set; }
    public string SerialNo { get; set; }
    public string FPAcomments { get; set; }
    public string IndiComments { get; set; }
    public string RejectComment { get; set; }

}

public class UserDetail
{
    public UserDetail()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string LoginUser { get; set; }
    public string EmpCode { get; set; }
    public string Designation { get; set; }
    public string Loc { get; set; }
    public string FPAAdmin { get; set; }
    public string Createdon { get; set; }
    public string UserSapId { get; set; }
    public string DesId { get; set; }
    public string CycleId { get; set; }
    public string Emails { get; set; }
    public string Status { get; set; }
    public string CarScheme { get; set; }
    public string Bu { get; set; }
    public string FIAdmin { get; set; }
    public string Department { get; set; }
    public string SerialNo { get; set; }
    public string Costcenter { get; set; }
    public string TotalFpaBalance { get; set; }
    public string ProcessClaim { get; set; }
    public string Owner { get; set; }
    public string COwner { get; set; }
}
public class FpaDistribution
{
    public FpaDistribution()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string FpaHeadId { get; set; }
    public string FpaHead { get; set; }
    public string Amount { get; set; }
    public string Alredyclaimed { get; set; }
    public string Balance { get; set; }
   
}
public class SalaryDistribution
{
    public SalaryDistribution()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string SalHeadId { get; set; }
    public string SalHead { get; set; }
    public string Amount { get; set; }

}
public class LogHistory
{
    public LogHistory()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Status { get; set; }
    public string PerformedBy { get; set; }
    public string DateTime { get; set; }
    public string Comments { get; set; }

}
public class AdminPage
{
    public AdminPage()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string CycleId { get; set; }
    public string Ename { get; set; }
    public string Ecode { get; set; }
    public string Status { get; set; }
    public string CreatedDate { get; set; }

}
public class ClaimDetail
{
    public ClaimDetail()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Ename { get; set; }
    public string Ecode { get; set; }
    public string SerialNo { get; set; }
    public string CreatedDate { get; set; }
    public string Status { get; set; }
}
