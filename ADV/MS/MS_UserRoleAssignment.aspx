<%@ Page Title="Master User Role Assign" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserRoleAssignment.aspx.vb" Inherits="MS_UserRoleAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../plugins/monyetsoft/monyetsoft.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server" id="form1">

        <div class="page-header">
            <h1>
                <asp:Label runat="server" ID="lblFormState"></asp:Label>
            </h1>
        </div>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div>
                    <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
                </div>
                <div>

                    <table class="monyet-entry-form">
                        <tr>
                            <td>User ID                        
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoginUserID" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>User Name
                        
                            </td>
                            <td>:</td>
                            <td>

                                <asp:TextBox ID="txtLoginUserName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td class="vtop">Role Assignment</td>
                            <td class="vtop">:</td>
                            <td>
                                <asp:CheckBoxList runat="server" ID="cblRole"></asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>

                </div>



                <hr />

                <div class="pull-left">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-danger" />
                </div>

            </div>
            <!-- /.span -->




        </div>


    </form>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuHead" runat="Server">
</asp:Content>

