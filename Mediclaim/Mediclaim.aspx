<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mediclaim.aspx.cs" Inherits="Mediclaim" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="asp" Namespace="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title>Mediclaim Form::Eicher</title>
    <link href="css/style_new3.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <%-- <link href="css/css_style.css" rel="stylesheet" type="text/css" />--%>
    <link href="Cal/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Cal/jquery-ui.min.js" type="text/javascript"></script>
    <script src="LightBox/popup.js" type="text/javascript"></script>



    <script type="text/javascript">
        $(document).ready(function () {
            $("input.Calendar").datepicker({
                yearRange: '1800:' + new Date(),
                maxDate: new Date(),
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                onSelect: function () { }
            });
        });
        //After Gridrefresh by any control in it(like radio button) 
        //Lost the Datepicker So we have write below function
        function CallCalender() {
            $("input.Calendar").datepicker({
                yearRange: '1800:' + new Date(),
                maxDate: new Date(),
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                onSelect: function () { }
            });
        }
    </script>

    <script type="text/javascript">
        function CloseWindow() {
            window.open('', '_self', '');
            window.close();
        }

        function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            window.opener.location.reload();
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
        function ShowMessage(msg) {
            //debugger;
            msg = replaceAll('_', '\n', msg);
            alert(msg);
            return false;
        }
        function replaceAll(find, replace, str) {
            while (str.indexOf(find) > -1) {
                str = str.replace(find, replace);
            }
            return str;
        }
        function ValidateSomeFields() {
            var sPlan = document.getElementById('ddlPlans').selectedIndex;
            var sInsured = document.getElementById('ddlSumInsured').selectedIndex;
            var Email = document.getElementById('txtEmail').value;
            var Mobile = document.getElementById('txtMobile').value;
            if (sPlan == 0) {
                alert('Please select your plan');
                return false;
            }
            else if (sInsured == 0) {
                alert("Please select Sum Insured");
                return false;
            }
            else if (Email == "") {
                alert('Please enter email');
                return false;
            }
            else if (Mobile == "") {
                alert('Please enter Mobile No.');
                return false;
            }
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scp" runat="server">
        </asp:ScriptManager>

        <script type="text/javascript">
            Sys.Application.add_load(CallCalender);

        </script>

        <asp:UpdatePanel ID="updcommon" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdtProgress" runat="server" AssociatedUpdatePanelID="updcommon"
                    DisplayAfter="1">
                    <ProgressTemplate>
                        <div id="loading">
                            Loading..
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div id="wrapper">
                    <div class="row greyBg">
                        <div class="pageWidth" id="PageContPrint">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <asp:Label ID="lblName" runat="server"></asp:Label>
                                </div>

                                <table class="formB brd" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td class="head" valign="top" align="left" colspan="4">
                                                <h2 class="black18 wnor">MediClaim Form</h2>
                                            </td>
                                            <td class="head" valign="top" align="left">
                                                <strong>Serial No </strong>
                                            </td>
                                            <td class="head" valign="top" align="left" colspan="3">
                                                <asp:Label ID="lblSerial" runat="server" Font-Bold="true" ForeColor="#000000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" width="13%">
                                                <strong>Gender</strong></td>
                                            <td valign="top" align="left" width="15%">
                                                <span>
                                                    <asp:Label ID="lblGender" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left" width="12%">
                                                <strong>Employee code</strong></td>
                                            <td valign="top" align="left" width="13%">
                                                <span>
                                                    <asp:Label ID="lblEcode" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left" width="11%">
                                                <strong>Designation</strong></td>
                                            <td valign="top" align="left" width="13%">
                                                <span>
                                                    <asp:Label ID="lblDesignation" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left" width="11%">
                                                <strong>Location</strong></td>
                                            <td valign="top" align="left" width="12%">
                                                <span>
                                                    <asp:Label ID="lblLocation" runat="server"></asp:Label></span></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <strong>Document Status</strong></td>
                                            <td valign="top" align="left">
                                                <span>
                                                    <asp:Label ID="lblStatus" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left">
                                                <strong>Mediclaim Admin</strong></td>
                                            <td valign="top" align="left">
                                                <span>
                                                    <asp:Label ID="lblHRName" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left">
                                                <strong>Department</strong></td>
                                            <td valign="top" align="left">
                                                <span>
                                                    <asp:Label ID="lblDepartment" runat="server"></asp:Label></span></td>
                                            <td valign="top" align="left">
                                                <strong>DOB/ Age</strong></td>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblDOb" runat="server"></asp:Label>/ <span><b>
                                                    <asp:Label ID="lblAge" runat="server"></asp:Label>
                                                </b></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="Msg"></asp:Label>
                            <div style="position: relative" class="row mBtm20">
                                <table class="formB brd" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td class="head" valign="top" align="left" colspan="6">
                                                <h2 class="black18 wnor">Details of Dependents   
                                                    <asp:Label ID="lblFmember" runat="server"></asp:Label></h2>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="greyBg" valign="top" align="left" width="12%">
                                                <strong>Plan</strong></td>
                                            <td class="greyBg" valign="top" align="left" width="16%">
                                                <strong>Sum Insured</strong></td>
                                            <td class="greyBg" valign="top" align="left" width="16%">
                                                <strong>Calculated Premium</strong></td>
                                            <td class="greyBg" valign="top" align="left" width="20%">
                                                <strong>E-Mail ID</strong></td>
                                            <td class="greyBg" valign="top" align="left" width="16%">
                                                <strong>Mobile No.</strong></td>
                                            <td class="greyBg" valign="top" align="left" width="20%">
                                                <strong>Blood Group</strong></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <asp:DropDownList ID="ddlPlans" runat="server" CssClass="sel" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPlans_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvPlan" runat="server" Display="Dynamic" Text="*"
                                                    ErrorMessage="Please Select Plan" InitialValue="0" ControlToValidate="ddlPlans"
                                                    ValidationGroup="vg1">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:DropDownList ID="ddlSumInsured" runat="server" CssClass="sel" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlSumInsured_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlSumInsured" runat="server" Display="Dynamic"
                                                    Text="*" ErrorMessage="Please Select Insured Amount" InitialValue="0" ControlToValidate="ddlSumInsured"
                                                    ValidationGroup="vg1">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtCalculatedPremium" runat="server" CssClass="inpt inpt3" Enabled="false"></asp:TextBox></td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="inpt inpt3"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ajaxEmail" runat="server" TargetControlID="txtEmail"
                                                    ValidChars=".@" FilterType="Numbers, LowercaseLetters, Custom">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" Display="Dynamic" Text="*"
                                                    ErrorMessage="Please enter valid Email ID" ControlToValidate="txtEmail" ValidationGroup="vg1"
                                                    ValidationExpression="^([a-z0-9_-]+)(@[a-z0-9-]+)(\.[a-z]+|\.[a-z]+\.[a-z]+)?$"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" Text="*"
                                                    ErrorMessage="Enter Email Address" ControlToValidate="txtEmail" ValidationGroup="vg1">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtMobile" runat="server" CssClass="inpt inpt3" MaxLength="10">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ajaxMobile" runat="server" TargetControlID="txtMobile"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" Display="Dynamic" Text="*"
                                                    ErrorMessage="Enter Mobile Number" ControlToValidate="txtMobile" ValidationGroup="vg1">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:DropDownList ID="ddlUserBloodGroup" runat="server" CssClass="sel" Width="80px">
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
                                            <td valign="top" align="left" colspan="6">
                                                <div class="row">
                                                    <asp:GridView ID="grdvDependantsDetail" runat="server" ForeColor="#333333" Visible="false"
                                                        CssClass="grdMain" Width="100%" BorderColor="#cccccc" AutoGenerateColumns="false"
                                                        CellSpacing="0" CellPadding="6" BorderWidth="1px" GridLines="Both">
                                                        <RowStyle BackColor="#EFF3FB"></RowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="RowNumber" HeaderText="Sr.">
                                                                <ItemStyle Width="4%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="NAME">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNameOfDependant" runat="server" CssClass="inpt inpt3" MaxLength="100">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvNameOfDep" runat="server" Display="Dynamic" Text="*"
                                                                        ErrorMessage="Enter Name of Dependant" ControlToValidate="txtNameOfDependant"
                                                                        ValidationGroup="vg1">
                                                                    </asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DOB">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDOB" onkeypress="return false" onkeydown="return false" oncut="return false" onpaste="return false" runat="server"
                                                                        CssClass="inpt inpt3 Calendar">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvDOB" runat="server" Display="Dynamic" Text="*"
                                                                        ErrorMessage="Please Select Date of Birth" ControlToValidate="txtDOB" ValidationGroup="vg1">
                                                                    </asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Blood Group">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlBloodGroup" Width="80px" runat="server" CssClass="sel">
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
                                                                    <%-- <asp:RequiredFieldValidator ID="rfvBG" runat="server" ValidationGroup="vg1" ControlToValidate="ddlBloodGroup"
                                                InitialValue="0" ErrorMessage="Please Select Blood Group" Text="*" Display="Dynamic">
                                            </asp:RequiredFieldValidator>--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Relation">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlRelation" runat="server" CssClass="sel" Width="120px" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlRelation_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvRelation" runat="server" ValidationGroup="vg1"
                                                                        ControlToValidate="ddlRelation" InitialValue="0" ErrorMessage="Please Select Relation"
                                                                        Text="*" Display="Dynamic">
                                                                    </asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="14%"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Height="30px" onkeypress="if (this.value.length > 100) { return false; }"
                                                                        runat="server" CssClass="inpt inpt3">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle HorizontalAlign="Right" Font-Bold="True"></FooterStyle>
                                                        <PagerStyle HorizontalAlign="Center" BackColor="#2461BF" ForeColor="White"></PagerStyle>
                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                                                        <HeaderStyle BackColor="#DCE8F6" ForeColor="Black"></HeaderStyle>
                                                        <EditRowStyle BackColor="#2461BF"></EditRowStyle>
                                                        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trMessage" runat="server" visible="false">
                                            <td valign="top" align="left" colspan="6">
                                                <strong>Error Message</strong> :
                                                <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" ForeColor="red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="6">
                                                <strong>HR Comments</strong> :
                                                <asp:Label ID="lblComments" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="6">
                                                <span style="font-size: 13px; color: red">Instructions</span>
                                                <ul style="list-style-type: none">
                                                    <li>- <b style="color: red">*</b> Required Fields.</li>
                                                    <li>- Please enter dependents'
                                                        name and other information properly, as it will appear on Insurance card.</li>
                                                    <li>-
                                                            Do not prefix +91 or 0 in mobile no.</li>
                                                    <li>- Remarks can contain maximum 100 characters.</li>
                                                </ul>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="margin-top: 10px; margin-bottom: 10px" valign="top" align="center" colspan="6">
                                                <table class="formB brd" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="head" valign="top" align="left" colspan="4">
                                                                <h2 class="black18 wnor">Present Address and Account Detail</h2>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <strong>Care of </strong>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:TextBox ID="txtCareOf" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td valign="top" align="left" width="13%">
                                                                <strong>House No. and Street  </strong></td>
                                                            <td valign="top" align="left" width="15%">
                                                                <asp:TextBox ID="txtStreet" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rqvtxtStreet" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter House No. and Street " ControlToValidate="txtStreet" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" width="12%">
                                                                <strong>2nd Address Line</strong></td>
                                                            <td valign="top" align="left" width="30%">
                                                                <asp:TextBox ID="txt2ndlineAdd" TextMode="MultiLine" Width="204px" Height="30px"
                                                                    runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rqvtxt2ndlineAdd" runat="server" Display="Dynamic"
                                                                    Text="*" ErrorMessage="Please enter 2nd Address Line" ControlToValidate="txt2ndlineAdd"
                                                                    ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top" align="left" width="11%">
                                                                <strong>Pin Code/ City</strong></td>
                                                            <td valign="top" align="left" width="30%">
                                                                <asp:TextBox ID="txtPostalCode" CssClass="newtxt" MaxLength="6" runat="server" Width="60px"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="flttxtPostalCode" runat="server" FilterType="Numbers"
                                                                    TargetControlID="txtPostalCode">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rqvtxtPostalCode" runat="server" Display="Dynamic"
                                                                    Text="*" ErrorMessage="Please enter pin code " ControlToValidate="txtPostalCode"
                                                                    ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>/
                                                                <asp:TextBox ID="txtCity" runat="server" Width="200px" CssClass="newtxt"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rqvtxtCity" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter city " ControlToValidate="txtCity" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <strong>Bank Name </strong>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:TextBox ID="txtBankName" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter Bank Name" ControlToValidate="txtBankName" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top" align="left" width="13%">
                                                                <strong>IFSC Code </strong></td>
                                                            <td valign="top" align="left" width="15%">
                                                                <asp:TextBox ID="txtIfscCode" Width="200px" CssClass="newtxt" runat="server" MaxLength="16"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter Ifsc Code " ControlToValidate="txtIfscCode" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <strong>Account Number</strong>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:TextBox ID="txtAcNo" Width="200px" CssClass="newtxt" runat="server" MaxLength="16"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter account number" ControlToValidate="txtAcNo" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAcNo"
                                                                    FilterType="Numbers">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td valign="top" align="left" width="13%">
                                                                <strong>Account Holder Name </strong></td>
                                                            <td valign="top" align="left" width="15%">
                                                                <asp:TextBox ID="txtAcHName" Width="200px" CssClass="newtxt" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" Text="*"
                                                                    ErrorMessage="Please enter account holder name " ControlToValidate="txtAcHName" ValidationGroup="vg1">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="margin-top: 10px; margin-bottom: 10px" valign="top" align="center" colspan="6">
                                                <asp:LinkButton ID="btnSaveAsDraft" OnClick="btnSaveAsDraft_Click" runat="server"
                                                    CssClass="btnSave" OnClientClick="return ValidateSomeFields()">
               Save As Draft
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CssClass="btnSave"
                                                    ValidationGroup="vg1" OnClientClick="return confirm('Please recheck all information before submission of  form. Are you sure you want to proceed? Click OK to proceed.')">
               Submit to HR
                                                </asp:LinkButton>
                                                <asp:ValidationSummary ID="vsGrdControl" runat="server" ValidationGroup="vg1" ShowSummary="false"
                                                    ShowMessageBox="true"></asp:ValidationSummary>
                                                <asp:LinkButton ID="btnClose" runat="server" CssClass="btnSave" OnClientClick="CloseWindow()">
               Close
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkBtnDelete" OnClick="lnkBtnDelete_Click" runat="server" Visible="false"
                                                    CssClass="btnSave" OnClientClick="return confirm('You are going to Delete? Click OK to proceed.')">
               Delete
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <a id="hrfHS" onclick="return showHide();" href="javascript:void(0);">Show log</a>&nbsp;
                            <div style="display: none" id="Records">
                                <asp:GridView ID="grdMediclaimLog" runat="server" Width="100%" BorderColor="Gray"
                                    AutoGenerateColumns="false" CellSpacing="0" CellPadding="2" BorderWidth="1" GridLines="None"
                                    HeaderStyle-HorizontalAlign="Left" AllowPaging="False" BorderStyle="Solid">
                                    <EmptyDataRowStyle CssClass="greyBg" />
                                    <RowStyle HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#CCCECF" />
                                    <AlternatingRowStyle BackColor="#f4f9ff" />
                                    <Columns>
                                        <asp:BoundField DataField="STATUS" HeaderText="Status" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="SAPID" HeaderText="Performed By" HeaderStyle-Width="20%"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DATE" HeaderText="Date Time" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-Width="45%"
                                            HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div id="divBackground" class="row mBtm20" style="height: 650px; width: 500px; display: none;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                        <tr>
                                            <td align="left" valign="top" class="head" style="width: 80%">
                                                <h2 class="black18 wnor">Child Relation Detail</h2>
                                            </td>
                                            <td align="right" valign="top" class="head" style="width: 20%">
                                                <a style="border: none;" href="javascript:void(0);" onclick="Popup.hide('divBackground')">
                                                    <img alt="close" style="border: 0; cursor: pointer;" src="img/close-i.png" /></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>

                                <td colspan="2" align="left" valign="top" style="width: 100%;">
                                    <span id="spnTest" runat="server" style="color: Red;"></span>
                                    <asp:RadioButtonList ID="rdbSdName" runat="server" OnSelectedIndexChanged="rdbSdName_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="hdnOwnerSapID" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSERIALNO" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnHREmail" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnNumOfDependants" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnFatherFlag" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdnCurrentLoginSapID" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnActualPrem" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnDesID" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfBu" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdfYear" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnClaimYear" runat="server"></asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
