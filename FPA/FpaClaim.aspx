<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FpaClaim.aspx.cs" Inherits="FpaClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
    <title>FPA Claim</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>
    <link href="Date/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Date/jquery-ui.min.js" type="text/javascript"></script>

    <%-- <script type="text/javascript" src="js/jquery-ui-1.8.12.custom.min.js"></script>--%>

    <script type="text/javascript">
        function CloseWindowcan() {
            window.open('', '_self', '');
            window.close();
        }
        function CloseRefreshWindow() {
            window.open('', '_self', '');
            window.close();
            //window.opener.location.reload();
            window.location = window.location;
        };

        $(document).ready(function () {

            $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true, showHour: false, showMinute: false, dateFormat: 'dd M yy' });
        });
        function callcalender() {
            $("[id$=txtExpenseDate]").datepicker({ maxDate: new Date(), changeMonth: true, changeYear: true, showHour: false, showMinute: false, dateFormat: 'dd M yy' });
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

        $(document).ready(function () {
            $('#ExpCollBtn').click(function () {
                //alert('1');
                $('#ExpCollBtn > img').toggleClass('expcol');
                $('#Records').slideToggle("slow");
            });
        });

    </script>
    <style type="text/css">
        .expcol {
            right: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSaveDraft">
        <asp:ScriptManager ID="scp" runat="server">
        </asp:ScriptManager>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                //var popup = $find('<%= modalPopup.ClientID %>');
                var popup = $find('<%= modalPopup.ClientID %>');
                if (popup != null) {
                    popup.show();
                }
            }

            function EndRequestHandler(sender, args) {
                var popup = $find('<%= modalPopup.ClientID %>');
                if (popup != null) {
                    popup.hide();
                }
            }
            Sys.Application.add_load(callcalender);

        </script>

        <asp:UpdatePanel ID="updcommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HFSAPID" runat="server" />
                <asp:HiddenField ID="HFID" runat="server" />
                <asp:HiddenField ID="HFEMAILS" runat="server" />
                <asp:HiddenField ID="HFBU" runat="server" />
                <asp:HiddenField ID="HFCOSTCENTER" runat="server" />
                <asp:HiddenField ID="HFSTATUS" runat="server" />

                <asp:HiddenField ID="HFUploadFileNo" runat="server" />
                <asp:HiddenField ID="hfMainRowNo" runat="server" />
                <asp:HiddenField ID="hfEditedIndex" runat="server" />
                <asp:HiddenField ID="hfUpOnlyFileNameN" runat="server" />
                <asp:HiddenField ID="hfCtlSet" runat="server" />
                <asp:HiddenField ID="HFIDEdit" runat="server" />

                <div id="wrapper">
                    <div class="row greyBg">
                        <div class="pageWidth">
                            <div class="row mBtm20">
                                <div class="floatRight Luser">
                                    <span id="spnLoginUser" runat="server"></span>
                                </div>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="8" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">FPA Claim <span id="spnFormCreater" runat="server"></span></h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="13%" align="left" valign="top">
                                            <strong>Serial No</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnSrNo" runat="server"></span>
                                        </td>
                                        <td width="12%" align="left" valign="top">
                                            <strong>Employee Code</strong>
                                        </td>
                                        <td width="13%" align="left" valign="top">
                                            <span id="spnEmpCode" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Level</strong>
                                        </td>
                                        <td align="left" width="13%" valign="top">
                                            <span id="spnDesignation" runat="server"></span>
                                        </td>
                                        <td width="11%" align="left" valign="top">
                                            <strong>Location</strong>
                                        </td>
                                        <td width="12%" align="left" valign="top">
                                            <span id="spnBU" visible="false" runat="server"></span><span id="spnLoc" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>Current Status</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="SpnStatus" runat="server">Draft</span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>Department</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="spndepartment" runat="server"></span>
                                        </td>
                                        <td align="left" valign="top">
                                            <strong>FI Admin</strong>
                                        </td>
                                        <td align="left" valign="top">
                                            <span id="spnFIAdmin" runat="server"></span>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>

                                </table>
                            </div>
                            <div class="row mBtm20">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td colspan="8" align="left" valign="top" class="head">
                                            <h2 class="black18 wnor">Claim Detail</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Total FPA Balance</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnTotalFpaBalance" runat="server"></span>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Claim In Process</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="SpnProcessClaim" runat="server"></span>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <strong>Total FPA Claim</strong>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <span id="spnTotalFpaClaim" runat="server"></span>
                                        </td>

                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td><strong>Heads</strong></td>
                                        <td><strong>GL Code</strong></td>
                                        <td><strong>Allocation</strong></td>
                                        <td><strong>Head Balance</strong></td>
                                        <td><strong>Bill Date</strong></td>
                                        <td><strong>Bill Amount</strong></td>
                                        <td><strong>Bill No</strong></td>
                                        <td><strong>Detail</strong></td>
                                    </tr>

                                    <tr>

                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:DropDownList ID="ddlHeads" runat="server" AutoPostBack="true" CssClass="sel" OnSelectedIndexChanged="ddlHeads_SelectedIndexChanged" Style="margin-top: 5px;">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlHeads" Display="Dynamic" ErrorMessage="Please select FPA heads" InitialValue="0" Text="*" ValidationGroup="add">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtGlcode" runat="server" CssClass="inptnew inpt2new" Enabled="false" Style="margin-top: 5px;"></asp:TextBox>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtAllocation" runat="server" CssClass="inptnew inpt2new" Enabled="false" Style="margin-top: 5px;"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtAllocation" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtAllocation">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtHeadBalance" runat="server" CssClass="inptnew inpt2new" Enabled="false" Style="margin-top: 5px;"></asp:TextBox>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtExpenseDate" runat="server" CssClass="inpbox2" onkeydown="return false" onkeypress="return false" autocomplete="off" Style="margin-top: 5px;"></asp:TextBox>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtBillAmount" runat="server" AutoPostBack="true" CausesValidation="true" CssClass="inptnew inpt2new" OnTextChanged="txtBillAmount_TextChanged1" TabIndex="1" autocomplete="off" Style="margin-top: 5px;"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtBillAmount" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtBillAmount">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rqvtxtBillAmount" runat="server" ControlToValidate="txtBillAmount" Display="Dynamic" ErrorMessage="Please enter amount" Text="*" ValidationGroup="add">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="10%">
                                            <asp:TextBox ID="txtBillNo" runat="server" CssClass="inptnew inpt5new" onmove="return false" Style="margin-top: 5px;"
                                                autocomplete="off" MaxLength="20" Width="80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ErrorMessage="*" ID="RequiredFieldValidator2" Display="Dynamic" runat="server"
                                                ControlToValidate="txtBillNo" SetFocusOnError="true" ValidationGroup="ADD" />
                                            <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtBillNo" ErrorMessage="Special characters are not allowed!" 
                                                ValidationExpression="^[a-zA-Z0-9\\/#_-]+$" ValidationGroup="ADD"/>
                                        </td>
                                        <td align="left" class="top_bdr" valign="top" width="30%">
                                            <asp:TextBox ID="txtDetail" runat="server" Columns="4" CssClass="inpt" Rows="3" Width="200px" TextMode="MultiLine" Style="margin-top: 5px;"></asp:TextBox>
                                            <asp:Label ID="lblDetail" runat="server" Visible="false"></asp:Label>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="@,#,&lt;,&gt;,%,&amp;,^,$" TargetControlID="txtDetail">
                                            </cc1:FilteredTextBoxExtender>
                                             <asp:RequiredFieldValidator ErrorMessage="Details are mandatory to fill" ID="RequiredFieldValidator3" Display="Dynamic" runat="server"
                                                ControlToValidate="txtDetail" SetFocusOnError="true" ValidationGroup="ADD" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" align="left" valign="top">
                                            <asp:FileUpload ID="FindFile" runat="server" />
                                        </td>
                                        <td colspan="6" align="left" valign="top">
                                            <asp:Button ID="butUpload" runat="server" Text="Upload" Width="100px" OnClick="butUpload_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="8" align="left" valign="middle">
                                            <asp:GridView ID="dgvEntryDocs" Width="74%" runat="server" CellPadding="4" ForeColor="#333333"
                                                AutoGenerateColumns="False" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnly_File_Name" runat="server" Text='<%#Eval("Only_File_Name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" Visible="false"
                                                        ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("File_Name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("NL_RowNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("pk_id") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDeleteDoc_Click">
                                                                <img src="images/delete.png" alt="Delete" />
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#f7fbff" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle CssClass="grdHead" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <RowStyle CssClass="grdRows" />
                                            </asp:GridView>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="8" align="center" valign="middle">

                                            <asp:LinkButton ID="lnkUpdate" runat="server" ToolTip="Update" CommandArgument='<%#Eval("pk_id") %>'
                                                OnClick="lnkUpdate_Click" ValidationGroup="ADD" Visible="false">
                                                                             <img src="images/update.jpg" alt="Update" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" ToolTip="Cancel" CommandArgument='<%#Eval("pk_id") %>'
                                                OnClick="lnkCancel_Click" Visible="false">
                                                                             <img src="images/cancel.jpg" alt="Cancel" />
                                            </asp:LinkButton>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="8" align="left" valign="top" class="top_bdr">
                                            <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="but" OnClick="btnAdd_OnClick"
                                                ValidationGroup="ADD" />
                                            <span class="red11">Please click on Add button to include the above line item</span></td>
                                    </tr>

                                    <tr>
                                        <td width="100%" align="left" valign="top" colspan="8">
                                            <asp:GridView ID="gvClaimDetail" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="30px" HeaderStyle-BackColor="#cecfce" OnRowDataBound="gvClaimDetail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Row Number" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRownumber" Text='<%#Eval("RowNumber") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="False" />
                                                    <asp:TemplateField HeaderText="Heads" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtFPA_HEAD_ID" CausesValidation="true" Text='<%#Eval("FPA_HEAD_ID")%>' runat="server" AutoPostBack="true" Enabled="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Heads" ItemStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlHeads" AutoPostBack="true" CssClass="sel" runat="server"
                                                                OnSelectedIndexChanged="ddlHeads_SelectedIndexChanged" Enabled="false" Visible="false">
                                                            </asp:DropDownList>
                                                            <%--                                                            <asp:RequiredFieldValidator ID="rqvddlHeads" runat="server" ControlToValidate="ddlHeads"
                                                                InitialValue="0" Display="Dynamic" ValidationGroup="add" Text="*" ErrorMessage="Please select FPA heads">
                                                            </asp:RequiredFieldValidator>--%>

                                                            <asp:Label ID="txtHead" CausesValidation="true" Text='<%#Eval("HeadName")%>' Height="11px" Width="120px" runat="server" AutoPostBack="true" Enabled="false"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Button ID="ButtonAdd" ValidationGroup="add" runat="server" CssClass="but" Text="Add"
                                                                OnClick="ButtonAdd_Click" Visible="false" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GL Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtGlcode" Enabled="false" Text='<%#Eval("GL_CODE")%>' Height="11px" Width="50px" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allocation">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtAllocation" Text='<%#Eval("AllocateAmount")%>' Height="11px" Width="67px" runat="server" Enabled="false"></asp:Label>
                                                            <%--                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtAllocation" runat="server" TargetControlID="txtAllocation"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Head Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtHeadBalance" Text='<%#Eval("HEAD_BAL")%>' Height="11px" Width="67px" Enabled="false" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtExpenseDate1" Text='<%#Eval("EXPENSE_DATE")%>' Height="11px" Width="65px" Enabled="false" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtBillAmount" Text='<%#Eval("CLAIM_AMT")%>' CausesValidation="true" Height="11px" Width="67px" runat="server" AutoPostBack="true" TabIndex="1"
                                                                Enabled="false"></asp:Label>
                                                            <%--                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtBillAmount" runat="server" TargetControlID="txtBillAmount"
                                                                FilterType="Numbers" FilterMode="ValidChars">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rqvtxtBillAmount" runat="server" ControlToValidate="txtBillAmount"
                                                                Display="Dynamic" ErrorMessage="Please enter amount" Text="*" ValidationGroup="add">
                                                            </asp:RequiredFieldValidator>--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtBillNo" Text='<%#Eval("BillNo")%>' CausesValidation="true" Height="11px" Width="100px" runat="server" AutoPostBack="true" Enabled="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="View Bill" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                        <HeaderStyle Width="135px" />
                                                        <ItemTemplate>
                                                            <asp:Label Width="135px" ID="du" runat="server"></asp:Label>
                                                            <asp:GridView ID="dgvEntryDocs" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="0" GridLines="None" ShowHeader="False"
                                                                OnRowCommand="dgvEntryDocs_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("File_Name")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDoc" runat="server" Width="130px" CommandName="Open" CommandArgument='<%#Eval("OnlyFileName")%>' Text='<%#Eval("OnlyFileName")%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("NL_RowNo")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Row No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtDetail" Enabled="false" Text='<%#Eval("Details")%>' TextMode="MultiLine" Rows="3" Columns="4" Width="180px" runat="server"></asp:Label>
                                                            <asp:Label ID="lblDetail" Visible="false" runat="server"></asp:Label>
                                                            <%--                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtDetail"
                                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                                            </cc1:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDelete_Click">
                                                                 <img src="images/icon-delete.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="6%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField Visible="true" HeaderText="Action" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="Edit" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClick="lnkEdit_Click">
                                                    <img src="images/edit.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDeleteRow" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("RowNumber") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkDeleteRow_Click">
                                                    <img src="images/delete.png" alt="Delete" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="8" align="center" valign="middle">
                                            <div class="infoSummary" runat="server" id="dvOnHoldDocumentsUpload" visible="false">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <%--<tr>
                                                        <td colspan="2" align="left" valign="top">
                                                            <img alt="" src="images/form_top.gif" /></td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td style="background: #f4f9ff; padding: 10px;">
                                                            <asp:FileUpload ID="FileUpload1" runat="server" class="multi" AllowMultiple="true" />
                                                            <asp:Button ID="btnUpload" runat="server" Text="Upload"
                                                                OnClick="btnUpload_Click" />
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td valign="top" class="form_mid">
                                                            <asp:GridView ID="gvOnholdDocs" Width="100%" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False"
                                                                OnRowCommand="gvOnholdDocs_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="File Name" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("File_Name")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="File Name" Visible="true" ItemStyle-Width="29%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDoc" runat="server" CommandName="Open" CommandArgument='<%#Eval("OnlyFileName")%>' Text='<%#Eval("OnlyFileName")%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="true" HeaderText="Action" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-Width="10%">
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton ID="lnkOnHoldDeleteRow" runat="server" ToolTip="Delete" CommandArgument='<%#Eval("ID") %>'
                                                                                OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="lnkOnHoldDeleteRow_Click">
                                                                    <img src="images/delete.png" alt="Delete" />
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="6%" />
                                                                    </asp:TemplateField>

                                                                    <asp:CommandField HeaderText="Action" HeaderStyle-Font-Bold="true" ShowEditButton="true" Visible="false"
                                                                        ShowDeleteButton="true" EditImageUrl="/images/edit.png" DeleteImageUrl="images/delete.gif"
                                                                        ItemStyle-Width="11%" />
                                                                </Columns>
                                                                <EditRowStyle BackColor="#f7fbff" />
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle CssClass="grdHead" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                <RowStyle BackColor="#EFF3FB" />
                                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <RowStyle CssClass="grdRows" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>

                                                    <%--<tr>
                                                        <td colspan="2" align="left" valign="top">
                                                            <img alt="" src="images/form_btm.gif" />
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                                <asp:TextBox ID="txtdefault" Visible="false" CssClass="inpt" runat="server"></asp:TextBox>
                                <asp:ValidationSummary ID="validsumm" runat="server" ValidationGroup="add" ShowMessageBox="true"
                                    ShowSummary="false" />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">
                                    <tr>
                                        <td width="20%" align="left" valign="top" class="greyBg">
                                            <strong>Mode Of Payment</strong>
                                        </td>
                                        <td width="40%" align="left" valign="top" class="greyBg">
                                            <strong>Comments</strong>
                                        </td>
                                        <td width="40%" align="left" valign="top" class="greyBg">
                                            <strong>FI Admin Comments</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:RadioButtonList ID="rdbtnModeOfPayment" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">Bank</asp:ListItem>
                                                <asp:ListItem Value="2">Cash</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtComments" runat="server" Rows="3" TextMode="MultiLine" CssClass="inpt"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtComments"
                                                FilterType="Custom" InvalidChars="@,#,<,>,%,&,^,$" FilterMode="InvalidChars">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="watermark" runat="server" TargetControlID="txtComments"
                                                WatermarkText="Please Enter Comments Here.">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="txtFiadminComments" runat="server"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">


                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formB brd">

                                                <tr>
                                                    <td width="30%" align="left" valign="top"></td>
                                                    <td width="10%" align="left" valign="top">
                                                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="button"
                                                            OnClick="btnSubmit_Click">
                         Submit</asp:LinkButton>
                                                    </td>
                                                    <td width="10%" align="left" valign="top">
                                                        <asp:LinkButton ID="btnSaveDraft" runat="server" CssClass="button"
                                                            OnClick="btnSaveDraft_Click">
                         Save as Draft</asp:LinkButton>
                                                    </td>

                                                    <td width="10%" align="left" valign="top">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="button" OnClientClick="javascript:CloseWindowcan();" OnClick="LinkButton1_Click">
                         Close</asp:LinkButton>
                                                    </td>
                                                    <td width="10%" align="left" valign="top">
                                                        <asp:LinkButton ID="btnDelete" Visible="false" runat="server" CssClass="button" OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="btnDelete_Click">
                                                Delete</asp:LinkButton>
                                                    </td>
                                                    <td width="30%" align="left" valign="top"></td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                </table>

                                <a href="javascript:void(0);" id="hrfHS" onclick="return showHide();">Show log</a>&nbsp;
                                <div id="Records" style="display: none">
                                    <asp:GridView ID="grdHistory" Width="100%" runat="server" BorderStyle="Solid" BorderColor="Gray"
                                        BorderWidth="1" CellSpacing="0" CellPadding="2" AutoGenerateColumns="false" GridLines="None"
                                        AllowPaging="False" HeaderStyle-HorizontalAlign="Left">
                                        <EmptyDataRowStyle CssClass="greyBg" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <HeaderStyle BackColor="#CCCECF" />
                                        <AlternatingRowStyle BackColor="#f4f9ff" />
                                        <Columns>
                                            <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Performed By" HeaderText="Performed By" HeaderStyle-Width="20%"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Date Time" HeaderText="Date Time" HeaderStyle-Width="15%"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="COMMENTS" HeaderText="Comments" HeaderStyle-Width="45%"
                                                HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="butUpload" />
                <asp:PostBackTrigger ControlID="gvClaimDetail" />
                <asp:PostBackTrigger ControlID="gvOnholdDocs" />
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>

        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="img/animated_loading.gif" AlternateText="Processing"
                    runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
            PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    </form>
</body>
</html>
