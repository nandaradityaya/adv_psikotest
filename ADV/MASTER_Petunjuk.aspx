<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeFile="MASTER_Petunjuk.aspx.vb" Inherits="MASTER_Petunjuk" ValidateRequest="false" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Master Petunjuk - Psikotest</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuHead" runat="server">
    <a href="#">Master Petunjuk</a>

    <script src="<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>/assets/plugins/ckeditor/ckeditor.js"></script>

    <script>
        function Loaded() {
            var roxyFileman = '<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>/assets/plugins/fileman/index.html';
            CKEDITOR.replace('txtKeteranganPetunjuk', {
                language: 'en',
                height: '500px',
                filebrowserBrowseUrl: roxyFileman,
                filebrowserImageBrowseUrl: roxyFileman + '?type=image',
                removeDialogTabs: 'link:upload;image:upload',
                baseHref: '<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>'
            });
        }
        function ResetZIndex() {
            App.ctl00_ContentPlaceHolder1_winAddEditPetunjuk.setZIndex("10000");
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
        <ext:FormPanel ID="FormPanelFilterPetunjuk" runat="server" Title=" " MarginSpec="0 0 0 0" BodyPadding="10"
            Frame="false" Width="500px" Height="120px" Header="false" AnchorHorizontal="100%" MonitorResize="true">
            <Items>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Tanggal Input" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:Checkbox ID="chbFilterTimeInput" runat="server" />
                        <ext:Label ID="label1" Text="&nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTimeInput_From" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                        <ext:Label ID="label2" Text="&nbsp; To &nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTimeInput_To" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Status" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:SelectBox ID="CmbFilterStatusPeserta"
                            runat="server" EmptyText="Semua Status">
                            <Items>
                                <ext:ListItem Text="Aktif" Value="1" />
                                <ext:ListItem Text="NonAktif" Value="0" />
                            </Items>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.getTrigger(0).show();" />
                                <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.getTrigger(0).hide();}" />
                            </Listeners>
                        </ext:SelectBox>
                    </Items>
                </ext:FieldContainer>
            </Items>
            <DockedItems>
                <ext:Toolbar ID="Toolbar2" runat="server" Dock="Bottom">
                    <Items>
                        <ext:Button ID="btnFilterPeserta" runat="server" Height="30" Width="100" MarginSpec="0 5 5 0" Text="Filter" Icon="ArrowRefresh" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnFilterPeserta_Click">
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:FormPanel>
        <ext:Hidden ID="hdnFilterPetunjuk" runat="server" />
        <ext:GridPanel ID="GridPanelPetunjuk" runat="server" Icon="Application" Title="Peserta"
            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="700">
            <Store>
                <ext:Store ID="StorePetunjuk" runat="server" RemoteSort="true" PageSize="10">
                    <Proxy>
                        <ext:AjaxProxy Url="GridHandlers/MASTER_PetunjukGridHandler.ashx">
                            <ActionMethods Read="GET" />
                            <Reader>
                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                            </Reader>
                        </ext:AjaxProxy>
                    </Proxy>
                    <Model>
                        <ext:Model ID="Model1" runat="server" IDProperty="SeqNo">
                            <Fields>
                                <ext:ModelField Name="SeqNo" Type="String" />
                                <ext:ModelField Name="Keterangan" Type="String" />
                                <ext:ModelField Name="bAktif" Type="String" />
                                <ext:ModelField Name="UserInput" Type="String" />
                                <ext:ModelField Name="TimeInput" Type="Date" />
                                <ext:ModelField Name="UserEdit" Type="String" />
                                <ext:ModelField Name="TimeEdit" Type="Date" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Parameters>
                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterPetunjuk}.getValue()" />
                    </Parameters>
                    <Sorters>
                        <ext:DataSorter Property="SeqNo" Direction="ASC" />
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:RowNumbererColumn runat="server" Text="No" Filterable="false" Width="40" />
                    <ext:CommandColumn ID="CommandColumn1" runat="server" Width="50" Text="" Align="Center">
                        <Commands>
                            <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                        </Commands>
                        <DirectEvents>
                            <Command OnEvent="btnEditPetunjuk_Click">
                                <ExtraParams>
                                    <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:CommandColumn>
                    <ext:Column ID="Column1" runat="server" Text="Nomor Petunjuk" DataIndex="SeqNo" Width="100" />
                    <ext:Column ID="Column2" runat="server" Text="Keterangan" DataIndex="Keterangan" Flex="1" />
                    <ext:Column ID="Column3" runat="server" Text="Status" DataIndex="bAktif" Width="60" />
                    <ext:Column ID="Column4" runat="server" Text="User Input" DataIndex="UserInput" Width="100" />
                    <ext:DateColumn ID="DateColumn1" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                    <ext:Column ID="Column5" runat="server" Text="User Edit" DataIndex="UserEdit" Width="100" />
                    <ext:DateColumn ID="DateColumn2" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
            </SelectionModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button ID="btnAddPetunjuk" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnAddPetunjuk_Click" IsUpload="true" />
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
            </BottomBar>
        </ext:GridPanel>

        <%--WINDOW--%>
        <ext:Window ID="winAddEditPetunjuk" runat="server" Title="Assign Ujian" Icon="NoteAdd" Width="870" Height="600" Hidden="true" Layout="FitLayout">
            <Items>
                <ext:FormPanel Header="false" ID="FormPanelAddEditPetunjuk" runat="server" Icon="FolderAdd"
                    Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                    <Items>
                        <ext:FieldContainer runat="server" LabelWidth="130" FieldLabel="Nomor Petunjuk" Width="180px">
                            <Items>
                                <ext:TextField runat="server" ID="txtNomorPetunjuk" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" LabelWidth="130" FieldLabel="Keterangan Petunjuk" Layout="HBoxLayout">
                            <Items>
                                <ext:Container runat="server" Html="<textarea id='txtKeteranganPetunjuk' rows='10' cols='100' name='kontenfull'></textarea>" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:FieldContainer runat="server" LabelWidth="130" FieldLabel="Status" Layout="HBoxLayout">
                            <Items>
                                <ext:Checkbox runat="server" ID="chbPetunjukStatus" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Aktif/Nonaktif" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnAddEditPetunjuk_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnAddEditPetunjuk_Save_Click">
                                    <ExtraParams>
                                        <ext:Parameter Name="KeteranganPetunjuk" Mode="Raw" Value="CKEDITOR.instances['txtKeteranganPetunjuk'].getData()" />
                                    </ExtraParams>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnAddEditPetunjuk_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="this.up('window').hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>


    <!--breadcrumb-->
    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
        <div class="breadcrumb-title pe-3">Instruction</div>
        <div class="ps-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 p-0">
                    <li class="breadcrumb-item"><a href="javascript:;"><i class="bx bx-home-alt"></i></a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">Instruction</li>
                </ol>
            </nav>
        </div>
    </div>
    <!--end breadcrumb-->

    <div class="card radius-10">
        <div class="card-header">
            <div class="row mb-3">
                <div class="col-12 col-lg-4">
                    <label for="filerDateAwal" class="form-label">
                        Start Date
							Filter</label>
                    <div class="input-group mb-2">
                        <input type="text" class="form-control" id="startDateFilterInstruction"
                            placeholder="dd/mm/yyyy">
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <label for="filerDateAwal" class="form-label">
                        End Date
							Filter</label>
                    <div class="input-group mb-2">
                        <input type="text" class="form-control" id="endDateFilterInstruction"
                            placeholder="dd/mm/yyyy">
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <label class="form-label">Status</label>
                    <select class="single-select form-select">
                        <option value="all">All</option>
                        <option value="active">Active</option>
                        <option value="nonActive">Non Active</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="card-body">
            <%-- Show this if success add new --%>
            <div class="alert border-0 border-start border-5 border-success alert-dismissible fade show py-2">
                <div class="d-flex align-items-center">
                    <div class="font-35 text-success">
                        <i class="bx bxs-check-circle"></i>
                    </div>
                    <div class="ms-3">
                        <h6 class="mb-0 text-success">Success</h6>
                        <div>Congrats, you successfully add new instruction!</div>
                    </div>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
            <%-- END --%>
            <div class="d-flex align-items-center">
                <div>
                    <h5 class="mb-0">Instruction</h5>
                </div>
                <div class="ms-auto mt-2">
                    <!-- <button type="button" class="btn btn-primary radius-8 d-flex align-items-center" data-bs-toggle="modal" data-bs-target="#addNewQuestion"><i class="bx bx-plus"></i>Add New</button> -->
                    <a type="button" class="btn btn-primary radius-8 d-flex align-items-center text-white" data-bs-toggle="modal" data-bs-target="#modalAddNewInstruction"><i class="bx bx-plus"></i>Add New</a>
                </div>
            </div>
            <div class="">
                <div id="printbar" style="float: right"></div>
                <br>
                <table id="tableInstruction" class="table mb-0 align-middle" style="width: 100%">
                    <thead class="table-light">
                        <tr>
                            <th>No. Instruction</th>
                            <th>Instruction</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>Petunjuk baru input dari 223
											<br>
                                <br>
                                1. Pilih salah satu pilihan sj!
											<br>
                                2. Hasil ujian tidak bisa diganggugugat!
											<br>
                                3. Hanya boleh tanya hal-hal yang sangat penting.
											<br>
                                <br>
                                Berdoa sebelum memulai ujian agar mendapat nilai yang memuaskan!
											<br>
                                <br>
                                HORE!!!
                            </td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Active
                                </div>
                            </td>
                            <td>
                                <div class="d-flex order-actions">
                                    <a type="button" class="text-primary bg-light-primary border-0 me-3" data-bs-toggle="modal" data-bs-target="#modalEditInstruction"><i class="bx bxs-edit"></i></a>
                                    <a type="button" class="text-danger bg-light-danger border-0" data-bs-toggle="modal" data-bs-target="#modalDelete"><i class="bx bxs-trash"></i></a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>


    <!-- Modal Box Add New Instruction -->
    <div class="modal fade" id="modalAddNewInstruction" tabindex="-1"
        aria-labelledby="modalAddNewInstructionLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalAddNewInstructionLabel">Add New Instruction</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <div id="div_editor1">
                            <!-- <p>This is a default toolbar demo of Rich text editor.</p> -->
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Status</label>
                        <div class="d-flex align-items-center">
                            <div class="form-check me-2">
                                <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioActiveStatus">
                                <label class="form-check-label ps-0" for="radioActiveStatus">Active</label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioInactiveStatus">
                                <label class="form-check-label ps-0" for="radioInactiveStatus">Inactive</label>
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

    <!-- Modal Box Edit Instruction -->
    <div class="modal fade" id="modalEditInstruction" tabindex="-1"
        aria-labelledby="modalEditInstructionLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalEditInstructionLabel">Edit Instruction</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <div id="div_editor2">
                            <!-- <p>This is a default toolbar demo of Rich text editor.</p> -->
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Status</label>
                        <div class="d-flex align-items-center">
                            <div class="form-check me-2">
                                <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioActiveStatus">
                                <label class="form-check-label ps-0" for="radioActiveStatus">Active</label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="flexRadioStatus" id="radioInactiveStatus">
                                <label class="form-check-label ps-0" for="radioInactiveStatus">Inactive</label>
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
                    <h5 class="modal-title" id="modalDeleteLabel">Delete Instruction</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure want to delete this instruction?
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
</asp:Content>
