using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FpaClaim
/// </summary>
public class FpaClaimUser
{
    public FpaClaimUser()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string FpaHeadId { get; set; }
    public string FpaHeadName { get; set; }
}
public class FpaClaimGrid
{
    public FpaClaimGrid()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string RowNo { get; set; }
    public string HeadId { get; set; }
    public string GlCode { get; set; }
    public string Allocation { get; set; }
    public string HeadBalance { get; set; }
    public string ExpenseDate { get; set; }
    public string BillAmount { get; set; }
    public string Detail { get; set; }
    public string VoucherType { get; set; }
    public string Comment { get; set; }
}