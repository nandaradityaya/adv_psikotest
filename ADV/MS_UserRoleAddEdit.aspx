<%@ Page Title="Add/Edit Master User Role" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserRoleAddEdit.aspx.vb" Inherits="MS_UserRoleAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
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
                    <asp:literal id="ltlMessage" runat="server"></asp:literal>
                </div>
                <div>
                    <table class="monyet-entry-form">

                        <tr>
                            <td>Role ID
                        <asp:label id="lblRequired1" runat="server" forecolor="Red" text="*"></asp:label>
                            </td>
                            <td>:
                            </td>
                            <td>

                                <asp:textbox id="txtLoginRoleID" runat="server" maxlength="20" width="200px"></asp:textbox>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:requiredfieldvalidator id="rfvLoginRoleID" runat="server" controltovalidate="txtLoginRoleID"
                                    display="Dynamic" errormessage="* Role ID is required" cssclass="monyet-validator"></asp:requiredfieldvalidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Description
                        <asp:label id="lblRequired3" runat="server" forecolor="Red" text="*"></asp:label>
                            </td>
                            <td>:</td>
                            <td>

                                <asp:textbox id="txtRoleDescs" runat="server" maxlength="50" width="200px"></asp:textbox>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:requiredfieldvalidator id="rfvRoleDescs" runat="server" controltovalidate="txtRoleDescs" display="Dynamic" errormessage="* Role Description is required" cssclass="monyet-validator"></asp:requiredfieldvalidator>
                            </td>
                        </tr>

                        <tr>
                            <td class="vtop">Remarks</td>
                            <td class="vtop">:</td>
                            <td>

                                <asp:textbox id="txtRemarks" runat="server" maxlength="255" rows="3" textmode="MultiLine" width="322px"></asp:textbox>

                            </td>
                        </tr>

                    </table>
                </div>



                <hr />

                <div class="pull-left">
                    <asp:button id="btnSave" runat="server" text="Save" cssclass="btn btn-primary" />
                    <asp:button id="btnCancel" runat="server" causesvalidation="false" text="Cancel" cssclass="btn btn-danger" />
                </div>

            </div>
            <!-- /.span -->




        </div>


    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuHead" runat="Server">
</asp:Content>

