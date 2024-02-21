<%@ Page Title="Add/Edit Master User" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserAddEdit.aspx.vb" Inherits="MS_UserAddEdit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
    <link href="../plugins/monyetsoft/monyetsoft.css" rel="stylesheet" />
    <script type="text/javascript">

        function ChangePassword(chkChangePassword, txt1, txt2, rfv1, rfv2) {

            var ochkChangePassword = document.getElementById(chkChangePassword);
            var otxt1 = document.getElementById(txt1);
            var otxt2 = document.getElementById(txt2);

            var orfv1 = document.getElementById(rfv1);
            var orfv2 = document.getElementById(rfv2);

            if (ochkChangePassword.checked) {

                otxt1.disabled = false;
                otxt2.disabled = false;
                //otxt1.style.backgroundColor = "#FFFFFF";
                //otxt2.style.backgroundColor = "#FFFFFF";

                ValidatorEnable(orfv1, true);
                ValidatorEnable(orfv2, true);

            }
            else {
                otxt1.disabled = true;
                otxt2.disabled = true;
                //otxt1.style.backgroundColor = "#FFFFCC";
                //otxt2.style.backgroundColor = "#FFFFCC";

                otxt1.value = "";
                otxt2.value = "";

                ValidatorEnable(orfv1, false);
                ValidatorEnable(orfv2, false);

            }
        }

        function confirmation() {
           if (confirm('Simpan data ini ?')) {
           return true;
           }else{
           return false;
           }
        }

        function isFloatNumber(e, t) {
           var n;
           var r;
           if (navigator.appName == "Microsoft Internet Explorer" || navigator.appName == "Netscape") {
              n = t.keyCode;
              r = 1;
              if (navigator.appName == "Netscape") {
                 n = t.charCode;
                 r = 0
              }
           } else {
              n = t.charCode;
              r = 0
           }
           if (r == 1) {
              if (!(n >= 48 && n <= 57 || n == 46)) {
                 t.returnValue = false
              }
           } else {
              if (!(n >= 48 && n <= 57 || n == 0)) {
                 t.preventDefault()
              }
           }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <form runat="server" id="form1">
   <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <div class="page-header">
        <h1>
            <asp:Label runat="server" ID="lblFormState"></asp:Label>            
        </h1>
    </div>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div>
                    <asp:literal id="ltlMessage" runat="server"></asp:literal>
                </div>
                <div>
 <table  class="monyet-entry-form">

                <tr>
                    <td class="right-nowrap">
                        User ID
                        <asp:Label ID="lblRequired1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLoginUserID" runat="server" MaxLength="20" Width="150px"  ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvLoginUserID" runat="server" ControlToValidate="txtLoginUserID"
                            Display="Dynamic" ErrorMessage="* User ID is required" CssClass="monyet-validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">User Name
                        <asp:Label ID="lblRequired3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td>:</td>
                    <td>

                            <asp:TextBox ID="txtLoginUserName" runat="server" MaxLength="50" Width="300px" ></asp:TextBox>
       
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvLoginUserName" runat="server" ControlToValidate="txtLoginUserName" Display="Dynamic" ErrorMessage="* User Name is required" CssClass="monyet-validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">Email Address</td>
                    <td>:</td>
                    <td>

                            <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="50" Width="300px" ></asp:TextBox>
         
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">Phone Number</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20" Width="300px" onkeypress="return isFloatNumber(this,event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">Status</td>
                    <td>:</td>
                    <td>

                            <asp:RadioButton ID="rdbStatusActive" Enabled="false" runat="server" Text="Active" Checked="True" GroupName="rdbStatus"  />
                            <asp:RadioButton ID="rdbStatusNonActive" Enabled="false" runat="server" Text="Non Active" GroupName="rdbStatus"  />
       
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">
                        Login Status 
                    </td>
                    <td>:</td>
                    <td>
 
                            <asp:RadioButton ID="rdbLoginStatusAllow" runat="server" Checked="True" GroupName="rdbLoginStatus" Text="Allow"  />
                            <asp:RadioButton ID="rdbLoginStatusLocked" runat="server" GroupName="rdbLoginStatus" Text="Locked"  />                        
                        

                    </td>
                </tr>
     
                <tr>
                    <td class="right-nowrap">Password
                        <asp:Label ID="lblRequired5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td >:</td>
                    <td>
                        
                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" Width="100px" TextMode="Password" ></asp:TextBox>
                            &nbsp;
                            <asp:CheckBox ID="chkChangePassword" runat="server" Text="Change Password"  />
                        
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="* Password is required" CssClass="monyet-validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="right-nowrap">Reenter Password
                        <asp:Label ID="lblRequired6" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td>:</td>
                    <td>
                        
                            <asp:TextBox ID="txtReenterPassword" runat="server" MaxLength="20" Width="100px" TextMode="Password" ></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtReenterPassword" Display="Dynamic" ErrorMessage="* Reenter Password is required" CssClass="monyet-validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:CompareValidator ID="cmpvPassword" runat="server" ControlToCompare="txtReenterPassword" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="* Password  is different with reenter password" CssClass="monyet-validator"></asp:CompareValidator>
                    </td>
                </tr>
                <tr></tr>
                <tr>
                    
                    <td class="vtop">Remarks</td>
                    <td class="vtop">:</td>
                    <td>
                        
                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="255" Rows="3" TextMode="MultiLine" Width="322px" ></asp:TextBox>
                        
                    </td>
                </tr>
                
                
                <tr>
                    <td class="right-nowrap">
                        Last Login Date</td>
                    <td>
                        :</td>
                    <td>
                        
                            <asp:Label ID="lblLastLoginDate" runat="server"></asp:Label>
                        
                        </td>
                </tr>
                <tr>
                    <td class="right-nowrap">Last Login From</td>
                    <td>:</td>
                    <td>
                        
                            <asp:Label ID="lblLastLoginFrom" runat="server"></asp:Label>
                        
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td>

                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                    </td>
                </tr>
            </table>              
                </div>



                <hr />

                <div class="pull-left">
                    <asp:button id="btnSave" runat="server" text="Save" cssclass="btn btn-primary" 
                        OnClientClick="return confirmation();" onclick="BtnSave_Click"/>

                    <asp:button id="btnCancel" runat="server" causesvalidation="false" text="Cancel" cssclass="btn btn-danger" />
                </div>

            </div>
            <!-- /.span -->




        </div>


    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuHead" Runat="Server">
</asp:Content>

