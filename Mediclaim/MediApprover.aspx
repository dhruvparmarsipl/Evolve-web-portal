<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MediApprover.aspx.cs" Inherits="MediApprover" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title>Approver Mediclaims</title>
    <script src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <link href="css/style_new3.css" rel="stylesheet" type="text/css" />
    <link href="Cal/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Cal/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateConfirm(msg) {
            var Comnts = document.getElementById('txtHRComments').value;
            if (Comnts == "" || Comnts == null) {
                alert('Please enter comments.');
                return false;
            }
            else {
                if (confirm(msg))
                    return true;
                else
                    return false;
            }
        }
        function showHide() {
            var ele = document.getElementById("Records");
            if (ele.style.display == "block") {
                ele.style.display = "none";
                $('#hrfHS').text("Show log");
            }
            else {
                ele.style.display = "block";
                $('#hrfHS').text("Hide log");
            }
        }
        function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            window.opener.location.reload();
        }
        function CloseWindow() {
            window.open('', '_self', '');
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <div class="row greyBg">
                <div class="pageWidth" id="PageContPrint">
                    <div class="row mBtm20">
                        <div class="floatRight Luser">
                            <asp:Label ID="lblCurrentLogin" runat="server"></asp:Label></div>


                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                            <tr>
                                <td colspan="4" align="left" valign="top" class="head">
                                    <h2 class="black18 wnor">MediClaim for   
                                        <asp:Label ID="lblName" runat="server"></asp:Label></h2>
                                </td>
                                <td align="left" valign="top" class="head">
                                    <strong>Serial No </strong></td>
                                <td colspan="3" align="left" valign="top" class="head">
                                    <asp:Label ForeColor="Black" ID="lblSerial" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="13%" align="left" valign="top"><strong>Gender</strong></td>
                                <td width="15%" align="left" valign="top"><span>
                                    <asp:Label ID="lblGender" runat="server"></asp:Label></span></td>
                                <td width="12%" align="left" valign="top"><strong>Employee code</strong></td>
                                <td width="13%" align="left" valign="top"><span>
                                    <asp:Label ID="lblEcode" runat="server"></asp:Label></span></td>
                                <td width="11%" align="left" valign="top"><strong>Designation</strong></td>
                                <td width="13%" align="left" valign="top"><span>
                                    <asp:Label ID="lblDesignation" runat="server"></asp:Label></span></td>
                                <td width="11%" align="left" valign="top"><strong>Location</strong></td>
                                <td width="12%" align="left" valign="top"><span>
                                    <asp:Label ID="lblLocation" runat="server"></asp:Label></span></td>
                            </tr>
                            <tr>
                                <td align="left" valign="top"><strong>Document Status</strong></td>
                                <td align="left" valign="top"><span>
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label></span></td>
                                <td align="left" valign="top"><strong>Mediclaim Admin</strong></td>
                                <td align="left" valign="top"><span>
                                    <asp:Label ID="lblHRName" runat="server"></asp:Label></span></td>
                                <td align="left" valign="top"><strong>Department</strong></td>
                                <td align="left" valign="top"><span>
                                    <asp:Label ID="lblDepartment" runat="server"></asp:Label></span></td>
                                <td align="left" valign="top"><strong>Age</strong></td>
                                <td align="left" valign="top"><span>
                                    <asp:Label ID="lblDOb" runat="server"></asp:Label><b>
                                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                                    </b></span></td>
                            </tr>
                        </table>
                    </div>
                    <asp:Label ID="lblMsg" CssClass="Msg" runat="server" Visible="false"></asp:Label>
                    <div class="row mBtm20" style="position: relative">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                            <tr>
                                <td colspan="6" align="left" valign="top" class="head">
                                    <h2 class="black18 wnor">Details of Dependents</h2>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" align="left" valign="top" class="greyBg">
                                    <strong>Plan<strong></td>
                                <td width="16%" align="left" valign="top" class="greyBg"><strong>Sum Insured</strong></td>
                                <td width="16%" align="left" valign="top" class="greyBg">
                                    <strong>Calculated Premium</strong></td>
                                <td width="20%" align="left" valign="top" class="greyBg">
                                    <strong>E-Mail ID</strong></td>
                                <td width="16%" align="left" valign="top" class="greyBg">
                                    <strong>Mobile No.</strong></td>
                                <td width="20%" align="left" valign="top" class="greyBg">
                                    <strong>Blood Group</strong></td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="ddlPlans" runat="server" Enabled="false"
                                        CssClass="sel" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="ddlSumInsured" runat="server" CssClass="sel"
                                        AutoPostBack="true" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtCalculatedPremium"
                                        ReadOnly="true" runat="server" CssClass="inpt inpt3"></asp:TextBox></td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtEmail" Enabled="false" runat="server" CssClass="inpt inpt3"></asp:TextBox>

                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMobile" ReadOnly="true" MaxLength="13" runat="server" CssClass="inpt inpt3">
                                    </asp:TextBox>

                                </td>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="ddlUserBloodGroup" Enabled="false" Width="80px" runat="server" CssClass="sel">
                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        <asp:ListItem Text="O-" Value="O-"></asp:ListItem>
                                        <asp:ListItem Text="O+" Value="O+"></asp:ListItem>
                                        <asp:ListItem Text="A-" Value="A-"></asp:ListItem>
                                        <asp:ListItem Text="A+" Value="A+"></asp:ListItem>
                                        <asp:ListItem Text="B-" Value="B-"></asp:ListItem>
                                        <asp:ListItem Text="B+" Value="B+"></asp:ListItem>
                                        <asp:ListItem Text="AB-" Value="AB-"></asp:ListItem>
                                        <asp:ListItem Text="AB+" Value="AB+"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="6">
                                    <div class="row">
                                        <asp:GridView ID="grdvDependantsDetail" Width="100%" runat="server"
                                            CssClass="grdMain" ForeColor="#333333" GridLines="Both" BorderWidth="1px"
                                            CellPadding="6" CellSpacing="0" AutoGenerateColumns="false" BorderColor="#cccccc"
                                            Visible="false">
                                            <Columns>
                                                <asp:BoundField DataField="RowNumber" HeaderText="Sr."
                                                    ItemStyle-Width="4%">
                                                    <ItemStyle Width="4%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="NAME" ItemStyle-Width="20%"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNameOfDependant" ReadOnly="true" CssClass="inpt inpt3" runat="server">
                                                        </asp:TextBox>

                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DOB(dd/mm/yyyy)" ItemStyle-Width="15%"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDOB" ReadOnly="true" onkeypress="return false" onkeydown="return false" oncut="return false" runat="server"
                                                            onpaste="return false" CssClass="inpt inpt3 Calendar">
                                                        </asp:TextBox>

                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Blood Group" ItemStyle-Width="10%"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlBloodGroup" Enabled="false" Width="80px"
                                                            runat="server" CssClass="sel">
                                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                            <asp:ListItem Text="O-" Value="O-"></asp:ListItem>
                                                            <asp:ListItem Text="O+" Value="O+"></asp:ListItem>
                                                            <asp:ListItem Text="A-" Value="A-"></asp:ListItem>
                                                            <asp:ListItem Text="A+" Value="A+"></asp:ListItem>
                                                            <asp:ListItem Text="B-" Value="B-"></asp:ListItem>
                                                            <asp:ListItem Text="B+" Value="B+"></asp:ListItem>
                                                            <asp:ListItem Text="AB-" Value="AB-"></asp:ListItem>
                                                            <asp:ListItem Text="AB+" Value="AB+"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Gender" ItemStyle-Width="17%" 
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                            <ItemTemplate>
                              <asp:RadioButtonList ID="rBtnLstGender" runat="server"
                              RepeatDirection="Horizontal" AutoPostBack="true" Enabled="false">
                                <asp:ListItem Text="Male" Selected="True" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                              </asp:RadioButtonList>
                            </ItemTemplate>
                                 <HeaderStyle HorizontalAlign="Left" />
                                 <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Relation" ItemStyle-Width="14%"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlRelation" runat="server"
                                                            CssClass="sel" Width="120px" AutoPostBack="true" Enabled="false">
                                                            <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                                            <asp:ListItem Text="Husband" Value="Husband"></asp:ListItem>
                                                            <asp:ListItem Text="Son" Value="Son"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" ReadOnly="true" Visible="false" runat="server" CssClass="inpt inpt3"></asp:TextBox>
                                                        <asp:Label ID="lblRemarks" runat="server" Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True"
                                                ForeColor="#333333" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White"
                                                HorizontalAlign="Center" />
                                            <HeaderStyle ForeColor="Black" BackColor="#dce8f6" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="6">
                                    <strong>HR Comments</strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="3">
                                    <asp:TextBox ID="txtHRComments" runat="server" TextMode="MultiLine" Height="60px" Width="360px">
                                    </asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                                <td align="left" valign="top" colspan="3">
                                    <span style="font-size: 14px; color: #000;">Instructions</span>
                                    <ul style="list-style-type: none;">
                                        <li><b style="color: red">*</b> Required Fields</li>
                                        <li>Please enter dependents' name and other information properly,<br />
                                            as it will appear on Insurance card.</li>
                                        <li>Mediclaim Admin comments can contain maximum 500 characters.</li>
                                    </ul>
                                </td>
                            </tr>

                            <tr>
                                <td align="center" valign="top" style="margin-top: 10px; margin-bottom: 10px;" colspan="6">
                                    <table class="formB brd" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="head" valign="top" align="left" colspan="4">
                                                    <h2 class="black18 wnor">Present Address Detail</h2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <strong>Care of </strong>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ReadOnly="true" ID="txtCareOf" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left" width="13%">
                                                    <strong>House No. and Street</strong></td>
                                                <td valign="top" align="left" width="15%">
                                                    <asp:TextBox ReadOnly="true" ID="txtStreet" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" width="12%">
                                                    <strong>2nd Address Line</strong></td>
                                                <td valign="top" align="left" width="30%">
                                                    <asp:TextBox ReadOnly="true" ID="txt2ndlineAdd" TextMode="MultiLine" Width="204px"
                                                        Height="30px" runat="server"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left" width="11%">
                                                    <strong>Pin Code/ City</strong></td>
                                                <td valign="top" align="left" width="30%">
                                                    <asp:TextBox ReadOnly="true" ID="txtPostalCode" CssClass="newtxt" MaxLength="7" runat="server"
                                                        Width="60px"></asp:TextBox>
                                                    <asp:TextBox ReadOnly="true" ID="txtCity" runat="server" Width="200px" CssClass="newtxt"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <strong>Bank Name</strong>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ReadOnly="true" ID="txtBankName" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left" width="13%">
                                                    <strong>IFSC Code</strong></td>
                                                <td valign="top" align="left" width="15%">
                                                    <asp:TextBox ReadOnly="true" ID="txtIfscCode" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <strong>Account Number</strong>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ReadOnly="true" ID="txtAcNo" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left" width="13%">
                                                    <strong>Account Holder Name</strong></td>
                                                <td valign="top" align="left" width="15%">
                                                    <asp:TextBox ReadOnly="true" ID="txtAcHName" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top" style="margin-top: 10px; margin-bottom: 10px;" colspan="6">
                                    <asp:LinkButton ID="lnkBtnApprove" runat="server" CssClass="btnSave" OnClick="lnkBtnApprove_Click"
                                        OnClientClick="return Confirm('You are going to approve, click OK to proceed.')">Approve </asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnReject" runat="server" CssClass="btnSave" OnClick="lnkBtnReject_Click"
                                        OnClientClick="return ValidateConfirm('You are going to reject, click OK to proceed.')">Reject </asp:LinkButton>
                                    <asp:LinkButton ID="BtnClose" runat="server" OnClientClick="CloseWindow()" CssClass="btnSave">Close
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnRevert" OnClick="lnkbtnRevert_Click" runat="server" OnClientClick="return Confirm('Are you sure you want to revert the user form status.')" CssClass="btnSave">Revert To User
                                    </asp:LinkButton>
                                </td>
                            </tr>

                        </table>

                    </div>
                    <a href="javascript:void(0);" id="hrfHS" onclick="return showHide();">Show log</a>&nbsp;
         <div id="Records" style="display: none">
             <asp:GridView ID="grdMediclaimLog" Width="100%" runat="server" BorderStyle="Solid" BorderColor="Gray"
                 BorderWidth="1" CellSpacing="0" CellPadding="2" AutoGenerateColumns="false" GridLines="None"
                 AllowPaging="False" HeaderStyle-HorizontalAlign="Left">
                 <EmptyDataRowStyle CssClass="greyBg" />
                 <RowStyle HorizontalAlign="Left" />
                 <HeaderStyle BackColor="#CCCECF" />
                 <AlternatingRowStyle BackColor="#f4f9ff" />
                 <Columns>
                     <asp:BoundField DataField="STATUS" HeaderText="Status" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" />
                     <asp:BoundField DataField="SAPID" HeaderText="Performed By" HeaderStyle-Width="20%"
                         HeaderStyle-HorizontalAlign="Left" />
                     <asp:BoundField DataField="DATE" HeaderText="Date Time" HeaderStyle-Width="15%"
                         HeaderStyle-HorizontalAlign="Left" />
                     <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-Width="45%"
                         HeaderStyle-HorizontalAlign="Left" />
                 </Columns>
             </asp:GridView>
         </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnOwnerSapID" runat="server" />
    <asp:HiddenField ID="hdnSERIALNO" runat="server" />
    <asp:HiddenField ID="hdnLeaveAR_RequestorSAPID" runat="server" />
    <asp:HiddenField ID="hdnUserEmail" runat="server" />
    <asp:HiddenField ID="hfBu" runat="server" />
    </form>
</body>
</html>
