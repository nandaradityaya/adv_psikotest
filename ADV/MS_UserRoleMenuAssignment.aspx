<%@ Page Title="Master User Role Menu Assign" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserRoleMenuAssignment.aspx.vb" Inherits="MS_UserRoleMenuAssignment" %>

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
                            <td>Role ID                        
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoginRoleID" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Description
                        
                            </td>
                            <td>:</td>
                            <td>

                                <asp:TextBox ID="txtRoleDescs" runat="server" MaxLength="50" Width="200px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvRoleDescs" runat="server" ControlToValidate="txtRoleDescs" Display="Dynamic" ErrorMessage="* Role Description is required" CssClass="monyet-validator"></asp:RequiredFieldValidator>
                            </td>
                        </tr>

                        <tr>
                            <td class="vtop">Menu Assignment</td>
                            <td class="vtop">:</td>
                            <td>

                                <asp:TreeView runat="server" ID="tvw1" NodeIndent="30" ShowLines="True"></asp:TreeView>

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

