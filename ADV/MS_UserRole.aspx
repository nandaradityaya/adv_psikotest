<%@ Page Title="Master User Role" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserRole.aspx.vb" Inherits="MS_UserRole" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server" id="Form1">

        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

        <div class="page-header">
        </div>
        <!-- /.page-header -->




        <div class="row">
            <div class="col-sm-6">
                <div class="widget-box">
                    <div class="widget-header">
                        <h4 class="smaller">Action													
                        </h4>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                            <input type="button" class="btn btn-primary" value="Add Role" onclick="Add();" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.col -->


        </div>

        <br />
        <div class="row">
            <div class="col-xs-12">
                <div class="table-header">
                    User Role Master
                </div>

                <!-- div.table-responsive -->

                <!-- div.dataTables_borderWrap -->
                <div class="table-responsive">
                    <table id="table1" class="table table-striped table-bordered table-hover">
                        <thead class="thead">
                            <tr>
                                <th>No</th>
                                <th>Extra No</th>
                                <th>User Role</th>
                                <th>Description</th>
                                <th>Last Update</th>
                                <th>ACTION</th>
                                <th>ASSIGN MENU</th>
                            </tr>
                        </thead>


                    </table>
                </div>
            </div>
        </div>
    </form>

    <%-- NEW FRONT END --%>

    <!--breadcrumb-->
    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
        <div class="breadcrumb-title pe-3">Management User</div>
        <div class="ps-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 p-0">
                    <li class="breadcrumb-item"><a href="applicant-data.html"><i class="bx bx-home-alt"></i></a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">Management User</li>
                </ol>
            </nav>
        </div>
    </div>
    <!--end breadcrumb-->

    <div class="card radius-10">
        <div class="card-body">
            <div class="d-flex align-items-center">
                <div>
                    <h5 class="mb-0">User Role</h5>
                </div>
                <div class="ms-auto mt-2">
                    <!-- <button type="button" class="btn btn-primary radius-8 d-flex align-items-center" data-bs-toggle="modal" data-bs-target="#addNewQuestion"><i class="bx bx-plus"></i>Add New</button> -->
                    <a type="button" class="btn btn-primary radius-8 d-flex align-items-center text-white" data-bs-toggle="modal" data-bs-target="#modalAddNewRole"><i class="bx bx-plus"></i>Add New</a>
                </div>
            </div>
            <div class="">
                <div id="printbar" style="float: right"></div>
                <br>
                <table id="tableUserRole" class="table mb-0 align-middle" style="width: 100%">
                    <thead class="table-light">
                        <tr>
                            <th>No</th>
                            <th>Role</th>
                            <th>Description</th>
                            <th>Last Update</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>Admin</td>
                            <td>Master Admin</td>
                            <td>23 Feb 2024</td>
                            <td>
                                <div class="d-flex order-actions">
                                    <a type="button" class="text-primary bg-light-primary border-0 me-3" data-bs-toggle="modal" data-bs-target="#modalEditRole"><i class="bx bxs-edit"></i></a>
                                    <a type="button" class="text-danger bg-light-danger border-0" data-bs-toggle="modal" data-bs-target="#modalDelete"><i class="bx bxs-trash"></i></a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>


    <!-- Modal Box Add New Role -->
    <div class="modal" id="modalAddNewRole" tabindex="-1"
        aria-labelledby="modalAddNewRoleLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalAddNewRoleLabel">Add New Role</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="userRole" class="form-label">Role</label>
                        <input type="text" class="form-control" id="userRole" placeholder="Role">
                    </div>
                    <div class="mb-3">
                        <label for="description" class="form-label">Description</label>
                        <textarea class="form-control" id="description" placeholder="Remarks" rows="3"></textarea>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" for="">Assign Menu</label>
                        <div class="form-check me-3">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckDataApplicant">
                            <label class="form-check-label" for="flexCheckDataApplicant">Data Applicant</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckQuestion">
                            <label class="form-check-label" for="flexCheckQuestion">Question</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckInstruction">
                            <label class="form-check-label" for="flexCheckInstruction">Instruction</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckManagementUser">
                            <label class="form-check-label" for="flexCheckManagementUser">Management User</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckRole">
                            <label class="form-check-label" for="flexCheckRole">Management Role</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckParameter">
                            <label class="form-check-label" for="flexCheckParameter">Parameter</label>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        Close</button>
                    <button type="button" class="btn btn-primary">Save</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Box Edit Role -->
    <div class="modal" id="modalEditRole" tabindex="-1"
        aria-labelledby="modalEditRoleLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalEditRoleLabel">Edit Role</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="userRole" class="form-label">Role</label>
                        <input type="text" class="form-control" id="userRole" value="Admin">
                    </div>
                    <div class="mb-3">
                        <label for="description" class="form-label">Description</label>
                        <textarea class="form-control" id="description" placeholder="remarks" rows="3">Master Admin</textarea>
                    </div>
                    <div class="mb-3">
                        <label class="form-label" for="">Assign Menu</label>
                        <div class="form-check me-3">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckDataApplicantEdit">
                            <label class="form-check-label" for="flexCheckDataApplicantEdit">Data Applicant</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckQuestionEdit">
                            <label class="form-check-label" for="flexCheckQuestionEdit">Question</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckInstructionEdit">
                            <label class="form-check-label" for="flexCheckInstructionEdit">Instruction</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckManagementUserEdit">
                            <label class="form-check-label" for="flexCheckManagementUserEdit">Management User</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckRoleEdit">
                            <label class="form-check-label" for="flexCheckRoleEdit">Management Role</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckParameterEdit">
                            <label class="form-check-label" for="flexCheckParameterEdit">Parameter</label>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        Close</button>
                    <button type="button" class="btn btn-primary">Save</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal Box Delete -->
    <div class="modal" id="modalDelete" tabindex="-1" aria-labelledby="modalDeleteLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalDeleteLabel">Delete Question</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure want to delete this package?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary radius-8"
                        data-bs-dismiss="modal">
                        Cancel</button>
                    <button type="button" class="btn btn-danger radius-8">Delete</button>
                </div>
            </div>
        </div>
    </div>

    <%-- END NEW FRONT END --%>

</asp:Content>
