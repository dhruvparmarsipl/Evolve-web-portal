<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FIAdminInitiationStatus.aspx.cs"
    Inherits="InitiationStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FPA Initiation Status Report</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>

    <script type="text/javascript">
   function CloseWindowcan()
   {
    window.open('','_self','');
    window.close();
   }
   function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            window.opener.location.reload();
        }
        
  
  
 function showHide() {
      var ele = document.getElementById("showHideDiv");
      if(ele.style.display == "block") {
            ele.style.display = "none";
            $('#hrfHS').text("Show log");
        }
     else {
         ele.style.display = "block";
          $('#hrfHS').text("Hide log");
       }
   }

   $(document).ready(function() {
            $('#ExpCollBtn').click(function() {
                //alert('1');
                $('#ExpCollBtn > img').toggleClass('expcol');
                $('#Records').slideToggle("slow");
            });
        });
    
    </script>

    <style type="text/css">
.expcol{right:0;}
</style>
</head>
<body >
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scp" runat="server">
        </asp:ScriptManager>

        <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HFSAPID" runat="server" />
                <asp:HiddenField ID="HFID" runat="server" />
                <asp:HiddenField ID="HFPAID" runat="server" />
                <asp:HiddenField ID="HFCYCLEID" runat="server" />
                <asp:HiddenField ID="HFDESID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
                <asp:HiddenField ID="HFCSTATUS" runat="server" />
                <div id="wrappernew">
                    <div class="row greyBgnew">
                        <div class="pageWidthnew">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span>
                                </div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black16 wnor">
                                                FPA Initiation Status Report <span id="spnFormCreater" runat="server"></span>
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Initiator Name</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnIntName" runat="server"></span>
                                        </td>
                                        <td width="25%" align="left" valign="top">
                                            <strong>Employee code</strong></td>
                                        <td width="25%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>Location</strong></td>
                                        <td align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server">
                                            </span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>Designation</strong></td>
                                        <td align="left" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
                                        </td>
                                        <span id="spnSrNo" visible="false" runat="server"></span>
                                    </tr>
                                </table>
                            </div>
                            <div class="rownew mBtm20new">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formBnew brdnew">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">
                                                Employee Information</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%" align="left" valign="top" class="greyBgnew">
                                            <strong>Year</strong></td>
                                        <td width="30%" align="left" valign="top" class="greyBgnew">
                                            <strong>Status</strong></td>
                                        <td width="15%" align="left" valign="top" class="greyBgnew">
                                            <strong>&nbsp;</strong></td>
                                        <td width="35%" align="left" valign="top" class="greyBgnew">
                                            <strong>&nbsp;</strong></td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlYear" CssClass="sel" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="ddlStatus" CssClass="sel" runat="server">
                                            <asp:ListItem Value="0">Initiated</asp:ListItem>
                                            <asp:ListItem Value="1">Draft by user</asp:ListItem>
                                              <asp:ListItem Value="3">Accepted</asp:ListItem>
                                                <asp:ListItem Value="4" Selected="True">Submitted to FPA Admin</asp:ListItem>
                                                 <asp:ListItem Value="5">Rejected by FPA Admin</asp:ListItem>
                                              
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:LinkButton ID="btnShowDetail" runat="server" CssClass="button" OnClick="btnShowDetail_Click" >
                         Show details</asp:LinkButton>
                                        </td>
                                        <td align="left" valign="top">
                                          &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                             <div class="rownew mBtm20new">
                                 <asp:GridView ID="gvEmployeeList" CssClass="grdMain" runat="server"  Width="100%" EmptyDataText="No Record Found."
                                     AutoGenerateColumns="false">
                                     <Columns>
                                         <asp:TemplateField HeaderText="" Visible="false">
                                             <ItemTemplate>
                                                 <asp:Label ID="lblCycleId" runat="server" Text='<%#Eval("fpa_cycle_id") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Employee Name" HeaderStyle-HorizontalAlign="Left">
                                             <ItemTemplate>
                                                 <a target="_blank" href="ShowDetails.aspx?RID=<%#Eval("fpa_cycle_id") %>">
                                                     <%#Eval("Ename")%>
                                                 </a>
                                             </ItemTemplate>
                                             <HeaderStyle Width="60%" />
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                                             <ItemTemplate>
                                             <asp:Label ID="lblStatus" runat="server" Text='<%#GetStatus(Eval("CURRENT_STATUS")) %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Created Date" HeaderStyle-HorizontalAlign="Left">
                                             <ItemTemplate>
                                             <asp:Label ID="lblCreatedOn" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                 </asp:GridView>
                             </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </form>
</body>
</html>
