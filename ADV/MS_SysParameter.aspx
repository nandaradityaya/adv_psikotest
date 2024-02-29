<%@ Page Title="Master Parameter" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_SysParameter.aspx.vb" Inherits="SysParameter" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" title="currentStyle">
        .modalBackground {
            background-image: none;
            background-color: #F6F9AB;
        }
    </style>

    <script type="text/javascript" charset="utf-8">
        function CommandHandler(commandname, IDno) {
            if (commandname == "Edit") {
                Edit(IDno);
            }
        }
        function Edit(IDno) {
            App.direct.EditData(IDno);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuHead" runat="Server">
    <a href="#">IT</a> - Parameter
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
            <%--<StartupMask Message="Loading DCT Web ..." />--%>
        </ext:ResourceManager>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
        <div class="box">
            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout" PaddingSpec="0 0 5 0">
                <Items>
                    <ext:Button ID="btnAdd" runat="server" Height="30" Icon="DatabaseAdd" Text="Tambah" StandOut="true" Hidden="true">
                        <DirectEvents>
                            <Click OnEvent="FormAdd">
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:FieldContainer>

            <asp:Panel ID="GridPanel" runat="server">
                <ext:GridPanel ID="GridPanel1" runat="server" ColumnLines="true" Cls="x-grid-dir" Flex="1" Icon="Table" Frame="true" Title="Parameter" Height="370">
                    <Store>
                        <ext:Store ID="Store1" runat="server" RemoteSort="true" PageSize="10">
                            <Proxy>
                                <ext:AjaxProxy Url="GridHandlers/MS_SysParameterGridhandler.ashx">
                                    <ActionMethods Read="GET" />
                                    <Reader>
                                        <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ID">
                                    <Fields>
                                        <ext:ModelField Name="Name" Type="String" />
                                        <ext:ModelField Name="Remark" Type="String" />
                                        <ext:ModelField Name="Value" Type="String" />
                                        <ext:ModelField Name="Name_Status" Type="String" />
                                        <ext:ModelField Name="NameInput" Type="String" />
                                        <ext:ModelField Name="TimeInput" Type="Date" />
                                        <ext:ModelField Name="NameEdit" Type="String" />
                                        <ext:ModelField Name="TimeEdit" Type="Date" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Name" Direction="Desc" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" Text="No" runat="server" Filterable="false" Width="40" />
                            <ext:CommandColumn ID="CommandColumn1" runat="server" Width="50" Text="Action" DataIndex="ID" Sortable="false" Align="Center">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" CommandName="Edit" Text="Edit">
                                        <ToolTip Text="Edit" />
                                    </ext:GridCommand>
                                </Commands>
                                <Listeners>
                                    <Command Handler="CommandHandler(command, record.data.Name);" />
                                </Listeners>
                            </ext:CommandColumn>
                            <ext:Column ID="Column16" runat="server" Text="Name" DataIndex="Name" Width="220px" />
                            <ext:Column ID="Column2" runat="server" Text="Remark" DataIndex="Remark" Width="280px" />
                            <ext:Column ID="Column5" runat="server" Text="Value" DataIndex="Value" Width="350px" />
                            <ext:Column ID="Column3" runat="server" Text="Status" DataIndex="Name_Status" Width="50px" />
                            <ext:Column ID="Column1" runat="server" Text="User Input" DataIndex="NameInput" Width="120px" />
                            <ext:DateColumn ID="Column4" runat="server" Text="Time Input" Format="dd MMM yyyy HH:mm" DataIndex="TimeInput" Width="110px" />
                            <ext:Column ID="Column6" runat="server" Text="User Edit" DataIndex="NameEdit" Width="120px" />
                            <ext:DateColumn ID="Column7" runat="server" Text="Time Edit" Format="dd MMM yyyy HH:mm" DataIndex="TimeEdit" Width="110px" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar3" runat="server" DisplayInfo="true" DisplayMsg="Displaying Order(s) {0} - {1} of {2}"
                            EmptyMsg="No Order(s) to display" />
                    </BottomBar>
                </ext:GridPanel>
            </asp:Panel>
        </div>

        <ext:Window ID="winAdd" runat="server" Title="Parameter" Icon="DatabaseAdd" Width="490" Height="450" Hidden="true" Layout="Fit">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5" BodyPadding="2"
                    Frame="true" Width="320" DefaultAnchor="100%" AutoScroll="True" Height="445">
                    <Items>
                        <ext:TextField ID="txtAction" runat="server" Hidden="true" />
                        <ext:TextField ID="txtName" runat="server" FieldLabel="Name" ReadOnly="true" FieldCls="ReadOnly" />
                        <ext:TextField ID="TxtRemark" runat="server" FieldLabel="Remark" />
                        <ext:TextArea ID="txtValue" runat="server" FieldLabel="Value" Height="250px" StyleHtmlContent="true" />
                        <ext:SelectBox ID="cmbStatus" runat="server" AnchorHorizontal="100%" FieldLabel="Status">
                            <Items>
                                <ext:ListItem Text="Yes" Value="1"></ext:ListItem>
                                <ext:ListItem Text="No" Value="0"></ext:ListItem>
                            </Items>
                        </ext:SelectBox>
                    </Items>

                    <Buttons>
                        <ext:Button ID="btnUpdate" runat="server" Text="Update" Icon="Disk">
                            <LoadingState Text="Processing..." />
                            <DirectEvents>
                                <Click OnEvent="btnUpdate_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button3" runat="server" StandOut="true" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="this.up('window').hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>


    <%-- NEW FRONT END --%>

    <!--breadcrumb-->
    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
        <div class="breadcrumb-title pe-3">Parameter</div>
        <div class="ps-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 p-0">
                    <li class="breadcrumb-item"><a href="javascript:;"><i class="bx bx-home-alt"></i></a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">Parameter</li>
                </ol>
            </nav>
        </div>
    </div>
    <!--end breadcrumb-->

    <div class="card radius-10">
        <div class="card-body">
            <div class="d-flex align-items-center">
                <div>
                    <h5 class="mb-0">Parameter</h5>
                </div>
                <!-- <div class="ms-auto mt-2">
								<a type="button" class="btn btn-primary radius-8 d-flex align-items-center" data-bs-toggle="modal" data-bs-target="#modalAddNewRole"><i class="bx bx-plus"></i>Add New</a>
							</div> -->
            </div>
            <div class="">
                <div id="printbar" style="float: right"></div>
                <br>
                <table id="tableParameter" class="table mb-0 align-middle" style="width: 100%">
                    <thead class="table-light">
                        <tr>
                            <th>No</th>
                            <th>Name</th>
                            <th>Remarks</th>
                            <th>Value</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>Interval Waktu Foto</td>
                            <td>Jarak Waktu antara foto peserta pada saat ujian (Menit)</td>
                            <td>1</td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Active
                                </div>
                            </td>
                            <td>
                                <div class="d-flex order-actions">
                                    <a type="button" class="text-primary bg-light-primary border-0 me-3" data-bs-toggle="modal" data-bs-target="#modalEditParameter"><i class="bx bxs-edit"></i></a>
                                    <a type="button" class="text-danger bg-light-danger border-0" data-bs-toggle="modal" data-bs-target="#modalDelete"><i class="bx bxs-trash"></i></a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>


    <!-- Modal Box Edit Role -->
    <div class="modal fade" id="modalEditParameter" tabindex="-1"
        aria-labelledby="modalEditParameterLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalEditParameterLabel">Edit Parameter</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="parameterName" class="form-label">Name</label>
                        <input type="text" class="form-control" id="parameterName" value="Interval Waktu Foto">
                    </div>
                    <div class="mb-3">
                        <label for="remarks" class="form-label">Remarks</label>
                        <textarea class="form-control" id="remarks" placeholder="remarks" rows="3">Jarak Waktu antara foto peserta pada saat ujian (Menit)</textarea>
                    </div>
                    <div class="mb-3">
                        <label for="valueParameter" class="form-label">Value</label>
                        <input type="number" class="form-control" id="valueParameter" value="1">
                    </div>
                    <div class="mb-3">
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
                    <h5 class="modal-title" id="modalDeleteLabel">Delete Parameter</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close">
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure want to delete this parameter?
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
