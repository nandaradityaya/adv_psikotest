<%@ Page Title="Master User" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_User.aspx.vb" Inherits="MS_User" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server" id="Form1">

        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <ext:ResourceManager runat="server" />
        <div class="page-header">
        </div>
        <!-- /.page-header -->





        <br />
        <div class="row">
            <div class="col-xs-12">
                <div class="table-header">
                    User Master
                </div>

                <!-- div.table-responsive -->

                <!-- div.dataTables_borderWrap -->
                <div>
                    <table id="GridTable" class="table table-striped table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Extra No</th>
                                <th>User ID </th>
                                <th>User Name </th>
                                <th>Email Address </th>
                                <th>Remarks </th>
                                <th>Last Update</th>
                                <th>Edit</th>
                                <th>Assign Role</th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
    </form>

    <ext:Window ID="winImport" runat="server" Title="Import" Icon="ApplicationAdd" Width="450" Height="170" Hidden="true" Layout="Fit">
        <Items>
            <ext:FormPanel Header="false" ID="frmImport" runat="server" Icon="FolderAdd" Title="Import User"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FileUploadField runat="server" ID="fuplExcel" FieldLabel="Upload Excel" Width="400" EmptyText="Pilih File ...">
                    </ext:FileUploadField>
                    <ext:Label runat="server" Html="<i><b>*Dipastikan data user sudah benar sesuai contoh.</b></i>" />
                </Items>
                <Buttons>
                    <ext:Button ID="Button9" Height="30" runat="server" Text="Save" Width="100" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnSaveImport_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>

    <%-- NEW FRONT END HERE --%>
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
            <%-- Show this if success add new --%>
            <div class="alert border-0 border-start border-5 border-success alert-dismissible fade show py-2">
                <div class="d-flex align-items-center">
                    <div class="font-35 text-success">
                        <i class="bx bxs-check-circle"></i>
                    </div>
                    <div class="ms-3">
                        <h6 class="mb-0 text-success">Success</h6>
                        <div>Congrats, you successfully add new user!</div>
                    </div>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
            <%-- END --%>
            <div class="d-flex align-items-center">
                <div>
                    <h5 class="mb-0">Management User</h5>
                </div>
                <div class="ms-auto mt-2">
                    <!-- <button type="button" class="btn btn-primary radius-8 d-flex align-items-center" data-bs-toggle="modal" data-bs-target="#addNewQuestion"><i class="bx bx-plus"></i>Add New</button> -->
                    <a type="button" class="btn btn-primary radius-8 d-flex align-items-center text-white" data-bs-toggle="modal" data-bs-target="#modalAddNewUser"><i class="bx bx-plus"></i>Add New</a>
                </div>
            </div>
            <div class="">
                <div id="printbar" style="float: right"></div>
                <br>
                <table id="tableManagementUser" class="table mb-0 align-middle" style="width: 100%">
                    <thead class="table-light">
                        <tr>
                            <th>No</th>
                            <th>User ID</th>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Remarks</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>w-888</td>
                            <td>w-888</td>
                            <td>cahyadi@advantagescm.com</td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Active
                                </div>
                            </td>
                            <td>Lorem Ipsum</td>
                            <td>
                                <div class="d-flex order-actions">
                                    <a type="button" class="text-primary bg-light-primary border-0 me-3" data-bs-toggle="modal" data-bs-target="#modalEditUser"><i class="bx bxs-edit"></i></a>
                                    <a type="button" class="text-danger bg-light-danger border-0" data-bs-toggle="modal" data-bs-target="#modalDelete"><i class="bx bxs-trash"></i></a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>w-69</td>
                            <td>nandaradityaya</td>
                            <td>nanda.raditya@advantagescm.com</td>
                            <td>
                                <div class="badge rounded-pill text-secondary bg-light-secondary p-2 text-uppercase px-3">
                                    Inactive
                                </div>
                            </td>
                            <td>Lorem Ipsum</td>
                            <td>
                                <div class="d-flex order-actions">
                                    <a type="button" class="text-primary bg-light-primary border-0 me-3" data-bs-toggle="modal" data-bs-target="#modalEditUser"><i class="bx bxs-edit"></i></a>
                                    <a type="button" class="text-danger bg-light-danger border-0" data-bs-toggle="modal" data-bs-target="#modalDelete"><i class="bx bxs-trash"></i></a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>


    <!-- Modal Box Add New User -->
    <div class="modal fade" id="modalAddNewUser" tabindex="-1"
        aria-labelledby="modalAddNewUserLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalAddNewUserLabel">Add New User</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="userId" class="form-label">User ID</label>
                            <input type="text" class="form-control" id="userId" placeholder="User ID">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="username" class="form-label">Username</label>
                            <input type="text" class="form-control" id="username" placeholder="Username">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="email" class="form-label">Email</label>
                            <input type="text" class="form-control" id="email" placeholder="Email">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="phoneNumber" class="form-label">Phone Number</label>
                            <input type="number" class="form-control" id="phoneNumber" placeholder="Phone Number">
                        </div>
                        <div class="col-12 col-lg-6 mb-3">
                            <label for="inputChoosePassword" class="form-label">Password</label>
                            <div class="input-group" id="show_hide_password">
                                <input type="password" class="form-control border-end-0" id="inputChoosePassword" placeholder="Enter Password">
                                <a href="javascript:;" class="input-group-text bg-transparent"><i class='bx bx-hide'></i></a>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6 mb-3">
                            <label for="confirmPassword" class="form-label">Confirm Password</label>
                            <div class="input-group" id="show_hide_confirm">
                                <input type="password" class="form-control border-end-0" id="confirmPassword" placeholder="Confirm Password">
                                <a href="javascript:;" class="input-group-text bg-transparent"><i class='bx bx-hide'></i></a>
                            </div>
                        </div>
                        <div class="col-12 col-lg-12 mb-3">
                            <label for="remarks" class="form-label">Remarks</label>
                            <textarea class="form-control" id="remarks" placeholder="Remarks" rows="3"></textarea>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label">Status</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-2">
                                    <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioActiveStatus">
                                    <label class="form-check-label" for="radioActiveStatus">Active</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioInactiveStatus">
                                    <label class="form-check-label" for="radioInactiveStatus">Inactive</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label">Login Status</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-2">
                                    <input class="form-check-input" type="radio" name="flexRadioLogin" id="radioAllowLogin">
                                    <label class="form-check-label" for="radioAllowLogin">Allow</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="flexRadioLogin" id="radioLockedLogin">
                                    <label class="form-check-label" for="radioLockedLogin">Locked</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label" for="">Role</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-3">
                                    <input class="form-check-input" type="checkbox" value="" id="flexCheckAdmin">
                                    <label class="form-check-label" for="flexCheckAdmin">Admin</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" value="" id="flexCheckAdminQuiz">
                                    <label class="form-check-label" for="flexCheckAdminQuiz">Admin Quiz</label>
                                </div>
                            </div>
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

    <!-- Modal Box Edit User -->
    <div class="modal fade" id="modalEditUser" tabindex="-1"
        aria-labelledby="modalEditUserLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalEditUserLabel">Add New User</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="userId" class="form-label">User ID</label>
                            <input type="text" class="form-control" id="userId" value="w-69">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="username" class="form-label">Username</label>
                            <input type="text" class="form-control" id="username" value="nandaradityaya">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="email" class="form-label">Email</label>
                            <input type="text" class="form-control" id="email" value="nanda.raditya@advantagescm.com">
                        </div>
                        <div class="col-12 col-lg-3 mb-3">
                            <label for="phoneNumber" class="form-label">Phone Number</label>
                            <input type="number" class="form-control" id="phoneNumber" value="081234567890">
                        </div>
                        <div class="col-12 col-lg-6 mb-3">
                            <label for="inputChoosePassword" class="form-label">Password</label>
                            <div class="input-group" id="show_hide_password">
                                <input type="password" class="form-control border-end-0" id="inputChoosePassword" value="12345678">
                                <a href="javascript:;" class="input-group-text bg-transparent"><i class='bx bx-hide'></i></a>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6 mb-3">
                            <label for="confirmPassword" class="form-label">Confirm Password</label>
                            <div class="input-group" id="show_hide_confirm">
                                <input type="password" class="form-control border-end-0" id="confirmPassword" value="12345678">
                                <a href="javascript:;" class="input-group-text bg-transparent"><i class='bx bx-hide'></i></a>
                            </div>
                        </div>
                        <div class="col-12 col-lg-12 mb-3">
                            <label for="remarks" class="form-label">Remarks</label>
                            <textarea class="form-control" id="remarks" placeholder="Remarks" rows="3">Lorem Ipsum</textarea>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label">Status</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-2">
                                    <input class="form-check-input" type="radio" name="flexRadioStatusEdit" id="radioActiveStatusEdit">
                                    <label class="form-check-label" for="radioActiveStatusEdit">Active</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="flexRadioStatusEdit" id="radioInactiveStatusEdit">
                                    <label class="form-check-label" for="radioInactiveStatusEdit">Inactive</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label">Login Status</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-2">
                                    <input class="form-check-input" type="radio" name="flexRadioLoginEdit" id="radioAllowLoginEdit">
                                    <label class="form-check-label" for="radioAllowLoginEdit">Allow</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="flexRadioLoginEdit" id="radioLockedLoginEdit">
                                    <label class="form-check-label" for="radioLockedLoginEdit">Locked</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4 mb-3">
                            <label class="form-label" for="radioLockedLogin">Role</label>
                            <div class="d-flex align-items-center">
                                <div class="form-check me-3">
                                    <input class="form-check-input" type="checkbox" value="" id="flexCheckAdminEdit">
                                    <label class="form-check-label" for="flexCheckAdminEdit">Admin</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" value="" id="flexCheckAdminQuizEdit">
                                    <label class="form-check-label" for="flexCheckAdminQuizEdit">Admin Quiz</label>
                                </div>
                            </div>
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
    <div class="modal fade" id="modalDelete" tabindex="-1" aria-labelledby="modalDeleteLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalDeleteLabel">Delete User</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure want to delete this user?
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


