﻿<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeFile="MASTER_Pertanyaan.aspx.vb" Inherits="MASTER_Pertanyaan" ValidateRequest="false" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Master Pertanyaan - Psikotest</title>
    
    <script src="<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>/assets/plugins/ckeditor/ckeditor.js"></script>

    <style type="text/css" title="currentStyle">
        .kreplin_lbl {
            width: 65px;
        }
        .status_lbl {
            width: 90px;
        }
    </style>
    <script>
        function Loaded() {
            var roxyFileman = '<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>/assets/plugins/fileman/index.html';
            CKEDITOR.replace('txtDeskripsiPertanyaan', {
                language: 'en',
                height: '500px',
                filebrowserBrowseUrl: roxyFileman,
                filebrowserImageBrowseUrl: roxyFileman + '?type=image',
                removeDialogTabs: 'link:upload;image:upload',
                baseHref: '<% =(Request.Url.GetLeftPart(UriPartial.Authority) & Request.ApplicationPath) %>'
            });
        }
        function ResetZIndex() {
            App.ctl00_ContentPlaceHolder1_winAddEditPaketSoalGroupDtl.setZIndex("10000");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuHead" runat="server">
    <a href="#">Master Pertanyaan</a>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    <ext:FormPanel ID="FormPanelFilter" runat="server" Title=" " MarginSpec="0 0 0 0" BodyPadding="10"
        Frame="false" Width="500px" Height="120px" Header="false" AnchorHorizontal="100%" MonitorResize="true">
        <Items>
            <ext:Hidden ID="hdnCurrNoPaket" runat="server" />
            <ext:Hidden ID="hdnCurrNoGroup" runat="server" />
            <ext:Hidden ID="hdnCurrNoDtl" runat="server" />
            <ext:Hidden ID="hdnCurrNoJwb" runat="server" />

            <ext:Hidden ID="hdnFotoDtl" runat="server" />
            <ext:Hidden ID="hdnFotoDtlFName" runat="server" />
            <ext:Hidden ID="hdnFotoDtlJawaban" runat="server" />
            <ext:Hidden ID="hdnFotoDtlJawabanFName" runat="server" />
            <ext:FieldContainer ID="FieldContainer18" runat="server" LabelWidth="150" FieldLabel="Tanggal Dibuat" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                <Items>
                    <ext:Checkbox ID="chbFilterTimeInput" runat="server" />
                    <ext:Label ID="label1" Text="&nbsp;" runat="server" />
                    <ext:DateField FieldLabel="" ID="DtFilterFrom" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    <ext:Label ID="label2" Text="&nbsp; To &nbsp;" runat="server" />
                    <ext:DateField FieldLabel="" ID="DtFilterTo" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                </Items>
            </ext:FieldContainer>
            <ext:FieldContainer ID="FieldContainer23" runat="server" LabelWidth="150" FieldLabel="Status" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                <Items>
                    <ext:SelectBox ID="CmbFilterStatus"
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
                    <ext:Button ID="BtnFilter" runat="server" Height="30" Width="100" MarginSpec="0 5 5 0" Text="Filter" Icon="ArrowRefresh" StandOut="true">
                        <DirectEvents>
                            <Click OnEvent="BtnFilter_Click" />
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </DockedItems>
    </ext:FormPanel>
    <ext:Hidden runat="server" ID="PaketSoalFilter" />
    <ext:GridPanel ID="GridPanelPaketSoal" runat="server" Icon="Computer" Title="Paket Soal"
        MarginSpec="0 0 0 0" BodyPadding="0" Frame="false" AutoScroll="True" ColumnLines="true" Height="327">
        <Store>
            <ext:Store ID="StorePaketSoal" runat="server" RemoteSort="true" PageSize="10">
                <Proxy>
                    <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGridHandler.ashx">
                        <ActionMethods Read="GET" />
                        <Reader>
                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                        </Reader>
                    </ext:AjaxProxy>
                </Proxy>
                <Model>
                    <ext:Model ID="Model1" runat="server" IDProperty="NoPaket">
                        <Fields>
                            <ext:ModelField Name="NoPaket" Type="Int" />
                            <ext:ModelField Name="NamaPaket" Type="String" />
                            <ext:ModelField Name="ToleransiWaktu" Type="Int" />
                            <ext:ModelField Name="bAktif" Type="String" />
                            <ext:ModelField Name="JobName" Type="String" />
                            <ext:ModelField Name="UserInput" Type="String" />
                            <ext:ModelField Name="TimeInput" Type="Date" />
                            <ext:ModelField Name="UserEdit" Type="String" />
                            <ext:ModelField Name="TimeEdit" Type="Date" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Parameters>
                    <ext:StoreParameter Name="PaketSoalFilter" Mode="Raw" Value="#{PaketSoalFilter}.getValue()" />
                </Parameters>
                <Sorters>
                    <ext:DataSorter Property="NoPaket" Direction="DESC" />
                </Sorters>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:RowNumbererColumn ID="RowNumbererColumn1" Text="No" runat="server" Filterable="false" Width="40" />
                <ext:CommandColumn ID="CommandColumn1" runat="server" Width="50" Text="" Align="Center">
                    <Commands>
                        <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="btnEditPaketSoal_Click">
                            <ExtraParams>
                                <ext:Parameter Name="NoPaket" Value="record.data.NoPaket" Mode="Raw" />
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                </ext:CommandColumn>
                <ext:Column ID="Column1" runat="server" Text="No Paket" DataIndex="NoPaket" Width="100" />
                <ext:Column ID="Column2" runat="server" Text="Nama Paket Soal" DataIndex="NamaPaket" Width="150" />
                <ext:Column ID="Column50" runat="server" Text="Posisi" DataIndex="JobName" Width="200" />
                <ext:Column ID="Column3" runat="server" Text="Toleransi Waktu (Menit)" DataIndex="ToleransiWaktu" Width="140" />
                <ext:Column ID="Column4" runat="server" Text="Status" DataIndex="bAktif" Width="60" />
                <ext:Column ID="Column5" runat="server" Text="User Input" DataIndex="UserInput" Width="150" />
                <ext:DateColumn ID="DateColumn1" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                <ext:Column ID="Column6" runat="server" Text="User Edit" DataIndex="UserEdit" Width="150" />
                <ext:DateColumn ID="DateColumn2" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
            </Columns>
        </ColumnModel>
        <Plugins>
            <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
        </Plugins>
        <SelectionModel>
            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Single">
                <DirectEvents>
                    <Select OnEvent="SelectedPaketSoal_Click" />
                </DirectEvents>
            </ext:CheckboxSelectionModel>
        </SelectionModel>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnAddPaketSoal" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                        <DirectEvents>
                            <Click Onevent="btnAddPaketSoal_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
        </BottomBar>
    </ext:GridPanel>
    <ext:Hidden runat="server" ID="PaketSoalGroupFilter" />
    <ext:GridPanel ID="GridPanelPaketSoalGroup" runat="server" Icon="Folder" Title="Group Soal"
        MarginSpec="0 0 0 0" BodyPadding="0" Frame="false" AutoScroll="True" ColumnLines="true" Height="327">
        <Store>
            <ext:Store ID="StorePaketSoalGroup" runat="server" RemoteSort="true" PageSize="10">
                <Proxy>
                    <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGroupGridHandler.ashx">
                        <ActionMethods Read="GET" />
                        <Reader>
                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                        </Reader>
                    </ext:AjaxProxy>
                </Proxy>
                <Model>
                    <ext:Model ID="Model2" runat="server" IDProperty="NoGroup">
                        <Fields>
                            <ext:ModelField Name="NoGroup" Type="Int" />
                            <ext:ModelField Name="NoPaket" Type="Int" />
                            <ext:ModelField Name="NamaGroup" Type="String" />
                            <ext:ModelField Name="MinimumJmlSoal" Type="Int" />
                            <ext:ModelField Name="NilaiStandar" Type="Int" />
                            <ext:ModelField Name="WaktuPengerjaan" Type="Int" />
                            <ext:ModelField Name="bRandom" Type="String" />
                            <ext:ModelField Name="bAktif" Type="String" />
                            <ext:ModelField Name="NoPetunjuk" Type="String" />
                            <ext:ModelField Name="UserInput" Type="String" />
                            <ext:ModelField Name="TimeInput" Type="Date" />
                            <ext:ModelField Name="UserEdit" Type="String" />
                            <ext:ModelField Name="TimeEdit" Type="Date" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Parameters>
                    <ext:StoreParameter Name="PaketSoalGroupFilter" Mode="Raw" Value="#{PaketSoalGroupFilter}.getValue()" />
                </Parameters>
                <Sorters>
                    <ext:DataSorter Property="NoGroup" Direction="ASC" />
                </Sorters>
            </ext:Store>
        </Store>
        <ColumnModel ID="ColumnModel2" runat="server">
            <Columns>
                <ext:RowNumbererColumn ID="RowNumbererColumn2" Text="No" runat="server" Filterable="false" Width="40" />
                <ext:CommandColumn ID="CommandColumn2" runat="server" Width="50" Text="" Align="Center">
                    <Commands>
                        <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit">
                        </ext:GridCommand>
                    </Commands>
                    <DirectEvents>
                        <Command OnEvent="btnEditPaketSoalGroup_Click">
                            <ExtraParams>
                                <ext:Parameter Name="NoGroup" Value="record.data.NoGroup" Mode="Raw" />
                            </ExtraParams>
                        </Command>
                    </DirectEvents>
                </ext:CommandColumn>
                <ext:Column ID="Column8" runat="server" Text="No Group Soal" DataIndex="NoGroup" Width="100" />
                <ext:Column ID="Column9" runat="server" Text="Nama Group" DataIndex="NamaGroup" Width="120" />
                <ext:Column ID="Column10" runat="server" Text="Minimum Jumlah Soal" DataIndex="MinimumJmlSoal" Width="150" />
                <ext:Column ID="Column11" runat="server" Text="Nilai Standar" DataIndex="NilaiStandar" Width="90" />
                <ext:Column ID="Column12" runat="server" Text="Waktu Pengerjaan" DataIndex="WaktuPengerjaan" Width="120" />
                <ext:Column ID="Column13" runat="server" Text="Acak Nomor Soal" DataIndex="bRandom" Width="110" />
                <ext:Column ID="Column14" runat="server" Text="Status" DataIndex="bAktif" Width="60" />
                <ext:Column ID="Column32" runat="server" Text="Nomor Petunjuk" DataIndex="NoPetunjuk" Width="110" />
                <ext:Column ID="Column15" runat="server" Text="User Input" DataIndex="UserInput" Width="150" />
                <ext:DateColumn ID="DateColumn3" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                <ext:Column ID="Column16" runat="server" Text="User Edit" DataIndex="UserEdit" Width="150" />
                <ext:DateColumn ID="DateColumn4" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
            </Columns>
        </ColumnModel>
        <Plugins>
            <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
        </Plugins>
        <SelectionModel>
            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" Mode="Single">
                <DirectEvents>
                    <Select OnEvent="SelectedPaketSoalGroup_Click" />
                </DirectEvents>
            </ext:CheckboxSelectionModel>
        </SelectionModel>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnAddPaketSoalGroup" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                        <DirectEvents>
                            <Click Onevent="btnAddPaketSoalGroup_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnDeletePaketSoalGroup" Height="30" Width="100" runat="server" UI="Danger" Text="Delete" Icon="Delete" StandOut="true">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnDeletePaketSoalGroup_Click" IsUpload="true">
                                <Confirmation ConfirmRequest="true" Title="Delete" Message="Yakin hapus data?" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <BottomBar>
            <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
        </BottomBar>
    </ext:GridPanel>
    <ext:Hidden runat="server" ID="PaketSoalGroupDtlFilter" />
    <ext:Container ID="Container_Dtl__Jawaban" runat="server" AnchorHorizontal="100%" ActiveTabIndex="0" Plain="true" Layout="HBoxLayout">
        <Items>
            <ext:GridPanel ID="GridPanelPaketSoalGroupDtl" runat="server" Icon="Page" Title="Pertanyaan"
                MarginSpec="0 0 0 0" BodyPadding="0" Frame="false"  AutoScroll="True" ColumnLines="true" Height="327" Width="600">
                <Store>
                    <ext:Store ID="StorePaketSoalGroupDtl" runat="server" RemoteSort="true" PageSize="10">
                        <Proxy>
                            <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGroupDtlGridHandler.ashx">
                                <ActionMethods Read="GET" />
                                <Reader>
                                    <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                </Reader>
                            </ext:AjaxProxy>
                        </Proxy>
                        <Model>
                            <ext:Model ID="Model3" runat="server" IDProperty="SeqNo">
                                <Fields>
                                    <ext:ModelField Name="SeqNo" Type="Int" />
                                    <ext:ModelField Name="NoPaket" Type="Int" />
                                    <ext:ModelField Name="NoGroup" Type="Int" />
                                    <ext:ModelField Name="NoUrut" Type="Int" />
                                    <ext:ModelField Name="Judul" Type="String" />
                                    <ext:ModelField Name="Deskripsi" Type="String" />
                                    <ext:ModelField Name="IsDownload" Type="String" />
                                    <ext:ModelField Name="bAktif" Type="String" />
                                    <ext:ModelField Name="MediaFileName" Type="String" />
                                    <ext:ModelField Name="UrlMedia" Type="String" />
                                    <ext:ModelField Name="TipeMedia" Type="String" />
                                    <ext:ModelField Name="UserInput" Type="String" />
                                    <ext:ModelField Name="TimeInput" Type="Date" />
                                    <ext:ModelField Name="UserEdit" Type="String" />
                                    <ext:ModelField Name="TimeEdit" Type="Date" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Parameters>
                            <ext:StoreParameter Name="PaketSoalGroupDtlFilter" Mode="Raw" Value="#{PaketSoalGroupDtlFilter}.getValue()" />
                        </Parameters>
                        <Sorters>
                            <ext:DataSorter Property="SeqNo" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel3" runat="server">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn3" Text="No" runat="server" Filterable="false" Width="40" />
                        <ext:CommandColumn ID="CommandColumn3" runat="server" Width="50" Text="" Align="Center">
                            <Commands>
                                <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                            </Commands>
                            <DirectEvents>
                                <Command OnEvent="btnEditPaketSoalGroupDtl_Click">
                                    <ExtraParams>
                                        <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                    </ExtraParams>
                                </Command>
                            </DirectEvents>
                        </ext:CommandColumn>
                        <ext:Column ID="Column20" runat="server" Text="No Urut" DataIndex="NoUrut" Width="55" />
                        <ext:Column ID="Column21" runat="server" Text="Judul" DataIndex="Judul" Width="90" />
                        <ext:Column ID="Column22" runat="server" Text="Deskripsi" DataIndex="Deskripsi" Width="150" />
                        <ext:Column ID="Column23" runat="server" Text="IsDownload" DataIndex="IsDownload" Width="80" />
                        <ext:Column ID="Column24" runat="server" Text="Aktif" DataIndex="bAktif" Width="60" />
                        <ext:Column ID="Column26" runat="server" Text="Url Media" DataIndex="UrlMedia" Width="120" />
                        <ext:Column ID="Column27" runat="server" Text="Tipe Media" DataIndex="TipeMedia" Width="120" />
                        <ext:Column ID="Column28" runat="server" Text="User Input" DataIndex="UserInput" Width="150" />
                        <ext:DateColumn ID="DateColumn5" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                        <ext:Column ID="Column29" runat="server" Text="User Edit" DataIndex="UserEdit" Width="150" />
                        <ext:DateColumn ID="DateColumn6" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                        <ext:Column ID="Column7" runat="server" Text="No Soal" DataIndex="SeqNo" Width="80" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true" />
                </Plugins>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel3" runat="server" Mode="Multi">
                        <DirectEvents>
                            <Select OnEvent="SelectedPaketSoalGroupDtl_Click" />
                        </DirectEvents>
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnAddPaketSoalGroupDtl" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                                <DirectEvents>
                                    <Click Onevent="btnAddPaketSoalGroupDtl_Click" IsUpload="true" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnImportPaketSoalGroupDtl" Height="30" Width="100" runat="server" UI="Default" Text="Import" Icon="PageWhiteExcel" StandOut="true">
                                <DirectEvents>
                                    <Click OnEvent="btnImportPaketSoalGroupDtl_Click" IsUpload="true">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnDeletePaketSoalGroupDtl" Height="30" Width="100" runat="server" UI="Danger" Text="Delete" Icon="Delete" StandOut="true">
                                <LoadingState Text="Processing..." />
                                <DirectEvents>
                                    <Click OnEvent="btnDeletePaketSoalGroupDtl_Click" IsUpload="true">
                                        <Confirmation ConfirmRequest="true" Title="Delete" Message="Yakin hapus data?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCopyPaketSoalGroupDtl" Height="30" Width="120" runat="server" Text="Copy Pertanyaan" Icon="PageWhiteCopy" StandOut="true">
                                <DirectEvents>
                                    <Click OnEvent="btnCopyPaketSoalGroupDtl_Click" IsUpload="true">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar3" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
                </BottomBar>
            </ext:GridPanel>
            <ext:Hidden runat="server" ID="PaketSoalGroupDtlJawabanFilter" />
            <ext:GridPanel ID="GridPanelPaketSoalGroupDtlJawaban" runat="server" Icon="Page" Title="Jawaban"
                MarginSpec="0 0 0 0" BodyPadding="0" Frame="false" AutoScroll="True" ColumnLines="true" Height="327" Flex="1">
                <Store>
                    <ext:Store ID="StorePaketSoalGroupDtlJawaban" runat="server" RemoteSort="true" PageSize="10">
                        <Proxy>
                            <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGroupDtlJawabanGridHandler.ashx">
                                <ActionMethods Read="GET" />
                                <Reader>
                                    <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                </Reader>
                            </ext:AjaxProxy>
                        </Proxy>
                        <Model>
                            <ext:Model ID="Model4" runat="server" IDProperty="SeqNo">
                                <Fields>
                                    <ext:ModelField Name="SeqNo" Type="Int" />
                                    <ext:ModelField Name="NoPaket" Type="Int" />
                                    <ext:ModelField Name="NoGroup" Type="Int" />
                                    <ext:ModelField Name="NoUrut" Type="Int" />
                                    <ext:ModelField Name="NoJawaban" Type="Int" />
                                    <ext:ModelField Name="Jawaban" Type="String" />
                                    <ext:ModelField Name="NoJawabanBenar" Type="String" />
                                    <ext:ModelField Name="MediaFileName" Type="String" />
                                    <ext:ModelField Name="UrlMedia" Type="String" />
                                    <ext:ModelField Name="TipeMedia" Type="String" />
                                    <ext:ModelField Name="TextMedia" Type="String" />
                                    <ext:ModelField Name="UserInput" Type="String" />
                                    <ext:ModelField Name="TimeInput" Type="Date" />
                                    <ext:ModelField Name="UserEdit" Type="String" />
                                    <ext:ModelField Name="TimeEdit" Type="Date" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Parameters>
                            <ext:StoreParameter Name="PaketSoalGroupDtlJawabanFilter" Mode="Raw" Value="#{PaketSoalGroupDtlJawabanFilter}.getValue()" />
                        </Parameters>
                        <Sorters>
                            <ext:DataSorter Property="SeqNo" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel4" runat="server">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn4" Text="No" runat="server" Filterable="false" Width="40" />
                        <ext:CommandColumn ID="CommandColumn4" runat="server" Width="50" Text="" Align="Center">
                            <Commands>
                                <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                            </Commands>
                            <DirectEvents>
                                <Command OnEvent="btnEditPaketSoalGroupDtlJawaban_Click">
                                    <ExtraParams>
                                        <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                    </ExtraParams>
                                </Command>
                            </DirectEvents>
                        </ext:CommandColumn>
                        <ext:Column ID="Column34" runat="server" Text="No Jawaban" DataIndex="NoJawaban" Width="90" />
                        <ext:Column ID="Column35" runat="server" Text="Jawaban" DataIndex="Jawaban" Width="120" />
                        <ext:Column ID="Column36" runat="server" Text="Poin" DataIndex="NoJawabanBenar" Width="110" />
                        <ext:Column ID="Column30" runat="server" Text="Tipe Media" DataIndex="TipeMedia" Width="120" />
                        <ext:Column ID="Column41" runat="server" Text="Url Media" DataIndex="UrlMedia" Width="120" />
                        <ext:Column ID="Column42" runat="server" Text="User Input" DataIndex="UserInput" Width="150" />
                        <ext:DateColumn ID="DateColumn7" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                        <ext:Column ID="Column43" runat="server" Text="User Edit" DataIndex="UserEdit" Width="150" />
                        <ext:DateColumn ID="DateColumn8" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                        <ext:column ID="Column37" runat="server" Text="SeqNo" DataIndex="SeqNo" Width="90" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:FilterHeader ID="FilterHeader4" runat="server" Remote="true" />
                </Plugins>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel4" runat="server" Mode="Multi">
                        <DirectEvents>
                            <Select OnEvent="SelectedPaketSoalGroupDtlJawaban_Click" />
                        </DirectEvents>
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnAddPaketSoalGroupDtlJawaban" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                                <DirectEvents>
                                    <Click Onevent="btnAddPaketSoalGroupDtlJawaban_Click" IsUpload="true" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="BtnImportJawaban" Height="30" Width="100" runat="server" UI="Default" Text="Import" Icon="PageWhiteExcel" StandOut="true">
                                <DirectEvents>
                                    <Click OnEvent="btnImportPaketSoalGroupDtlJawaban_Click" IsUpload="true" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnDeletePaketSoalGroupDtlJawaban" Height="30" Width="100" runat="server" UI="Danger" Text="Delete" Icon="Delete" StandOut="true">
                                <LoadingState Text="Processing..." />
                                <DirectEvents>
                                    <Click OnEvent="btnDeletePaketSoalGroupDtlJawaban_Click" IsUpload="true">
                                        <Confirmation ConfirmRequest="true" Title="Delete" Message="Yakin hapus jawaban?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar4" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display">
                        <DirectEvents>
                            <Change OnEvent="GridPanelPaketSoalGroupDtlPageChanged" />
                        </DirectEvents>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Container>

    <%--     WINDOW    --%>



























    
    <ext:Window ID="winAddEditPaketSoal" runat="server" Title="Tambah Paket Soal" Icon="NoteEdit" Width="400" Height="240" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelAddEditPaketSoal" runat="server"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoal_PS" LabelWidth="100" FieldLabel="Nomor Paket Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoal_PS" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNamaPaketSoal_PS" LabelWidth="100" FieldLabel="Nama Paket Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNamaPaketSoal_PS" Width="230" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainernfToleransiWaktu" LabelWidth="100" FieldLabel="Toleransi Waktu" Layout="HBoxLayout">
                        <Items>
                            <ext:NumberField runat="server" ID="nfToleransiWaktu" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" />
                            <ext:Label runat="server" Text="Menit" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainercmbPosisi" LabelWidth="100" FieldLabel="Posisi" Layout="HBoxLayout">
                        <Items>
                            <ext:MultiCombo runat="server" ID="mcbPosisi" ClientIDMode="Static" Width="230" DisplayField="NamaPosisi" ValueField="CodePosisi">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                </Triggers>
                                <Listeners>
                                    <Select Handler="this.getTrigger(0).show();" />
                                    <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                    <TriggerClick Handler="if (index == 0) { this.clearValue(); this.getTrigger(0).hide();}" />
                                </Listeners>
                                <Store>
                                    <ext:Store runat="server">
                                        <Model>
                                            <ext:Model runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="CodePosisi" Type="String" />
                                                    <ext:ModelField Name="NamaPosisi" Type="String" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:MultiCombo>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalStatus" LabelWidth="100" FieldLabel="Status" Layout="HBoxLayout">
                        <Items>
                            <ext:Radio runat="server" Name="PaketSoalStatus" ID="rdoPaketSoalStatusOn" Checked="false" MarginSpec="0 5 0 0" BoxLabel="Aktif" />
                            <ext:Radio runat="server" Name="PaketSoalStatus" ID="rdoPaketSoalStatusOff" Checked="false" MarginSpec="0 5 0 0" BoxLabel="NonAktif" />
                            <%--<ext:Checkbox runat="server" ID="chbPaketSoalStatus" Checked="false" MarginSpec="0 5 0 0" BoxLabelCls="status_lbl" BoxLabel="Aktif/Nonaktif" ReadOnlyCls="ReadOnly" />--%>
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAddEditPaketSoal_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Simpan" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnAddEditPaketSoal_Save_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnAddEditPaketSoal_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddEditPaketSoalGroup" runat="server" Title="Tambah Group Soal" Icon="NoteAdd" Width="720" Height="650" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelAddEditPaketSoalGroup" runat="server"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoal_PSG" LabelWidth="130" FieldLabel="Nomor Paket Soal">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoal_PSG" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoalGroup_PSG" LabelWidth="130" FieldLabel="Nomor Group Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoalGroup_PSG" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNamaPaketSoalGroup_PSG" LabelWidth="130" FieldLabel="Nama Group Soal">
                        <Items>
                            <ext:TextField runat="server" ID="txtNamaPaketSoalGroup_PSG" Height="22" Width="230" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbKreplin" LabelWidth="130" FieldLabel="Group Soal Kreplin">
                        <Items>
                            <ext:Checkbox runat="server" ID="chbKreplin" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabelCls="kreplin_lbl" BoxLabel="Ya/Tidak" ReadOnlyCls="ReadOnly" >
                                <DirectEvents>
                                    <Change OnEvent="chbKreplin_Change" />
                                </DirectEvents>
                            </ext:Checkbox>
                        </Items>
                    </ext:FieldContainer>
                    <ext:GridPanel ID="GridPanelNormaDtl" runat="server" Icon="Computer" Title="Detail Norma" Height="260"
                        MarginSpec="0 0 0 0" BodyPadding="0" Frame="false" AutoScroll="True" ColumnLines="true">
                        <Store>
                            <ext:Store ID="StoreNormaDtl" runat="server" RemoteSort="true" PageSize="10">
                                <Model>
                                    <ext:Model ID="Model7" runat="server" IDProperty="SeqNo">
                                        <Fields>
                                            <ext:ModelField Name="SeqNo" Type="Int" />
                                            <ext:ModelField Name="Nama" Type="String" />
                                            <ext:ModelField Name="NoGroup" Type="Int" />
                                            <ext:ModelField Name="BatasBawah" Type="Int" />
                                            <ext:ModelField Name="BatasAtas" Type="Int" />
                                            <ext:ModelField Name="UserInput" Type="String" />
                                            <ext:ModelField Name="TimeInput" Type="Date" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter Property="BatasAtas" Direction="DESC" />
                                </Sorters>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel7" runat="server">
                            <Columns>
                                <ext:CommandColumn ID="CommandColumn5" runat="server" Width="60" Text="" Align="Center">
                                    <Commands>
                                        <ext:GridCommand Icon="NoteDelete" StandOut="true" CommandName="Delete" Text="Hapus" />
                                    </Commands>
                                    <DirectEvents>
                                        <Command OnEvent="btnDeleteNormaDtl_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                            </ExtraParams>
                                        </Command>
                                    </DirectEvents>
                                </ext:CommandColumn>
                                <ext:RowNumbererColumn ID="RowNumbererColumn6" Text="No" runat="server" Filterable="false" Width="40" />
                                <ext:Column ID="Column38" runat="server" Text="Label Norma" DataIndex="Nama" Flex="1" />
                                <ext:Column ID="Column39" runat="server" Text="Batas Atas" DataIndex="BatasAtas" Width="80" />
                                <ext:Column ID="Column40" runat="server" Text="Batas Bawah" DataIndex="BatasBawah" Width="80" />
                                <ext:Column ID="Column51" runat="server" Text="User Input" DataIndex="UserInput" Width="150" />
                                <ext:DateColumn ID="DateColumn11" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                            </Columns>
                        </ColumnModel>
                        <Plugins>
                            <ext:FilterHeader ID="FilterHeader7" runat="server" Remote="true" />
                        </Plugins>
<%--                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel5" runat="server" Mode="Multi" />
                        </SelectionModel>--%>
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button ID="btnAddNormaDtl" runat="server" UI="Info" Text="Add" Icon="NoteAdd" Height="30" Width="100" StandOut="true">
                                        <DirectEvents>
                                            <Click Onevent="btnAddNormaDtl_Click" IsUpload="true" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar7" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
                        </BottomBar>
                    </ext:GridPanel>
                    <ext:FieldContainer runat="server" ID="FldContainerJmlKolom" LabelWidth="130" FieldLabel="Jumlah Kolom" Layout="HBoxLayout" Height="27" Hidden="true">
                        <Items>
                            <ext:NumberField runat="server" ID="nfJmlKolom" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" Height="22" />
                            <ext:Label runat="server" Text="Kolom" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainernfMinimumJmlSoal" LabelWidth="130" FieldLabel="Minimum Jumlah Soal" Layout="HBoxLayout" Height="27">
                        <Items>
                            <ext:NumberField runat="server" ID="nfMinimumJmlSoal" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" Height="22" />
                            <ext:Label runat="server" Text="Soal" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainernfNilaiStandar" LabelWidth="130" FieldLabel="Nilai Standar" Layout="HBoxLayout" Height="27">
                        <Items>
                            <ext:NumberField runat="server" ID="nfNilaiStandar" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" Height="22" />
                            <ext:Label runat="server" Text="Poin" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainernfWaktuPengerjaan" LabelWidth="130" FieldLabel="Waktu Pengerjaan" Layout="HBoxLayout" Height="27">
                        <Items>
                            <ext:NumberField runat="server" ID="nfWaktuPengerjaan" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" Height="22" />
                            <ext:Label runat="server" Text="Menit" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerCmbPetunjuk" LabelWidth="130" FieldLabel="Nomor Petunjuk" Layout="HBoxLayout">
                        <Items>
                            <ext:DropDownField runat="server" ID="CmbPetunjuk" Editable="false" Mode="ValueText" TriggerIcon="Combo" EmptyText="Pilih Petunjuk" Width="180px">
                                <Listeners>
                                    <Expand Handler="this.picker.setWidth(450);" />
                                </Listeners>
                                <Component>
                                    <ext:GridPanel ID="GridPanelPetunjuk" ColumnLines="true" Cls="x-grid-dir" Flex="1" runat="server" Height="340" Width="350" Mode="ValueText" Title="Petunjuk" Frame="true">
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
                                                    <ext:Model ID="Model6" runat="server" IDProperty="SeqNo">
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
                                                    <ext:StoreParameter Name="filter" Mode="Value" Value="bAktif = 'Aktif'" />
                                                </Parameters>
                                                <Sorters>
                                                    <ext:DataSorter Property="SeqNo" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="ColumnModel6" runat="server">
                                            <Columns>
                                                <ext:Column ID="Column17" runat="server" Text="Nomor Petunjuk" DataIndex="SeqNo" Width="100" />
                                                <ext:Column ID="Column18" runat="server" Text="Keterangan" DataIndex="Keterangan" Width="150" />
                                                <ext:Column ID="Column19" runat="server" Text="Status" DataIndex="bAktif" Width="60" />
                                                <ext:Column ID="Column25" runat="server" Text="User Input" DataIndex="UserInput" Width="100" />
                                                <ext:DateColumn ID="DateColumn9" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                                <ext:Column ID="Column31" runat="server" Text="User Edit" DataIndex="UserEdit" Width="100" />
                                                <ext:DateColumn ID="DateColumn10" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                            </Columns>
                                        </ColumnModel>
<%--                                        <Plugins>
                                            <ext:FilterHeader ID="FilterHeader6" runat="server" Remote="true" />
                                        </Plugins>--%>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single">
                                                <DirectEvents>
                                                    <Select OnEvent="SelectNamaPetunjuk"></Select>
                                                </DirectEvents>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar6" runat="server" DisplayInfo="true" DisplayMsg="Displaying Entity(s) {0} - {1} of {2}" EmptyMsg="No Entity(s) to display" HideRefresh="true" />
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Component>
                            </ext:DropDownField>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalGroupRandom" LabelWidth="130" FieldLabel="Acak nomor" Layout="HBoxLayout">
                        <Items>
                            <ext:Checkbox runat="server" ID="chbPaketSoalGroupRandom" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Aktif/Nonaktif" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalGroupIsPrioritas" LabelWidth="130" FieldLabel="IsPrioritas" Layout="HBoxLayout">
                        <Items>
                            <ext:Checkbox runat="server" ID="chbPaketSoalGroupIsPrioritas" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="(Apakah kelulusan group soal ini penting sebagai bahan pertimbangan?)" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalGroupStatus" LabelWidth="130" FieldLabel="Status" Layout="HBoxLayout">
                        <Items>
                            <ext:Radio runat="server" Name="PaketSoalGroupStatus" ID="rdoPaketSoalGroupStatusOn" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Aktif" />
                            <ext:Radio runat="server" Name="PaketSoalGroupStatus" ID="rdoPaketSoalGroupStatusOff" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="NonAktif" />
                            <%--<ext:Checkbox runat="server" ID="chbPaketSoalGroupStatus" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabelCls="status_lbl" BoxLabel="Aktif/Nonaktif" ReadOnlyCls="ReadOnly" />--%>
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAddEditPaketSoalGroup_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Simpan" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnAddEditPaketSoalGroup_Save_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnAddEditPaketSoalGroup_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddNormaDtl" runat="server" Title="Tambah Detail Norma" Icon="NoteAdd" Width="400" Height="210" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelAddNormaDtl" runat="server"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FieldContainer runat="server" ID="FieldContainerNormaDtlNama" LabelWidth="100" FieldLabel="Nama Detail" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNormaDtlNama" Width="230" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FieldContainerNormaDtlBatasBawah" LabelWidth="100" FieldLabel="Batas Bawah" Layout="HBoxLayout">
                        <Items>
                            <ext:NumberField runat="server" ID="nfNormaDtlBatasBawah" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" />
                            <ext:Label runat="server" Text="Poin" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FieldContainerNormaDtlBatasAtas" LabelWidth="100" FieldLabel="Batas Atas" Layout="HBoxLayout">
                        <Items>
                            <ext:NumberField runat="server" ID="nfNormaDtlBatasAtas" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" />
                            <ext:Label runat="server" Text="Poin" />
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAddNormaDtl_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Simpan" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnAddNormaDtl_Save_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnAddNormaDtl_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddEditPaketSoalGroupDtl" runat="server" Title="Tambah Group Soal" Icon="NoteAdd" Width="870" Height="600" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelAddEditPaketSoalGroupDtl" runat="server"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoal_PSGD" LabelWidth="130" FieldLabel="Nomor Paket Soal">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoal_PSGD" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoalGroup_PSGD" LabelWidth="130" FieldLabel="Nomor Group Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoalGroup_PSGD" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoalGroupDtl_PSGD" LabelWidth="130" FieldLabel="Nomor Pertanyaan" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoalGroupDtl_PSGD" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FldContainertxtJudulPertannyaan" runat="server" LabelWidth="130" FieldLabel="Judul Pertanyaan">
                        <Items>
                            <ext:TextField runat="server" ID="txtJudulPertanyaan" Height="22" Width="380" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerContainerDeskripsiPertanyaan" LabelWidth="130" FieldLabel="Deskripsi Pertanyaan" Layout="HBoxLayout">
                        <Items>
                            <ext:Container runat="server" ID="ContainerDeskripsiPertanyaan" Html="<textarea id='txtDeskripsiPertanyaan' rows='10' cols='100' name='kontenfull'></textarea>" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerrdgTipeMedia_PSGD" LabelWidth="130" FieldLabel="Tipe Media" Layout="HBoxLayout">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rdgTipeMedia_PSGD" Width="380">
                                <Items>
                                    <ext:Radio runat="server" ID="RadioTipeMediaN_PSGD" BoxLabel="Tanpa Media" Checked="true" />
                                    <ext:Radio runat="server" ID="RadioTipeMediaL_PSGD" BoxLabel="Link Media" />
                                    <ext:Radio runat="server" ID="RadioTipeMediaU_PSGD" BoxLabel="Upload Media" />
                                </Items>
                                <DirectEvents>
                                    <Change OnEvent="RadioTipeMedia_PSGD_Change" />
                                </DirectEvents>
                            </ext:RadioGroup>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerTxtMediaDtl" LabelWidth="130" FieldLabel="Media" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="TxtMediaNDtl" runat="server" Width="380px" EmptyText="No Media" Hidden="false" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                            <ext:TextField ID="TxtMediaLDtl" runat="server" Width="380px" EmptyText="Link Media" Hidden="true" />
                            <ext:FileUploadField ID="FU_MediaPaketSoalGroupDtl" runat="server" AllowBlank="true" EmptyText="Choose File" Hidden="true"
                                LabelWidth="150" ButtonText="Browse" Icon="Attach" Width="380px" AnchorHorizontal="100%">
                                <DirectEvents>
                                    <Change OnEvent="UploadFileDtl" />
                                </DirectEvents>
                            </ext:FileUploadField>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="TampilanFilePaketSoalGroupDtl" FieldLabel="Tampilan File" Layout="HBoxLayout" LabelWidth="130">
                        <Items>
                            <ext:Image ID="imgPreviewDtl" runat="server" Height="200" Width="200" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalGroupDtlIsDownload" LabelWidth="130" FieldLabel="IsDownload" Layout="HBoxLayout">
                        <Items>
                            <ext:Checkbox runat="server" ID="chbPaketSoalGroupDtlIsDownload" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Aktif/Nonaktif" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbPaketSoalGroupDtlStatus" LabelWidth="130" FieldLabel="Status" Layout="HBoxLayout">
                        <Items>
                            <ext:Radio runat="server" Name="PaketSoalGroupDtl" ID="chbPaketSoalGroupDtlStatusOn" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Aktif" />
                            <ext:Radio runat="server" Name="PaketSoalGroupDtl" ID="chbPaketSoalGroupDtlStatusOff" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="NonAktif" />
                            <%--<ext:Checkbox runat="server" ID="chbPaketSoalGroupDtlStatus" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabelCls="status_lbl" BoxLabel="Aktif/Nonaktif" ReadOnlyCls="ReadOnly" />--%>
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAddEditPaketSoalGroupDtl_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Simpan" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnAddEditPaketSoalGroupDtl_Save_Click" IsUpload="true">
                                <ExtraParams>
                                    <ext:StoreParameter Name="DeskripsiPertanyaan" Mode="Raw" Value="CKEDITOR.instances['txtDeskripsiPertanyaan'].getData()" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnAddEditPaketSoalGroupDtl_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddEditPaketSoalGroupDtlJawaban" runat="server" Title="Tambah Group Soal" Icon="NoteAdd" Width="400" Height="440" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelAddEditPaketSoalGroupDtlJawaban" runat="server"
                Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                <Items>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoal_PSGDJ" LabelWidth="100" FieldLabel="Nomor Paket Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoal_PSGDJ" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorPaketSoalGroup_PSGDJ" LabelWidth="100" FieldLabel="Nomor Group Soal" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorPaketSoalGroup_PSGDJ" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorUrut_PSGDJ" LabelWidth="100" FieldLabel="Nomor Urut" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorUrut_PSGDJ" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtNomorJawaban" LabelWidth="100" FieldLabel="Nomor Jawaban" Width="180px">
                        <Items>
                            <ext:TextField runat="server" ID="txtNomorJawaban" Width="230" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtJawaban" LabelWidth="100" FieldLabel="Jawaban">
                        <Items>
                            <ext:TextArea runat="server" ID="txtJawaban" Width="230" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerrdgTipeMedia_PSGDJ" LabelWidth="100" FieldLabel="Tipe Media" Layout="HBoxLayout">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rdgTipeMedia_PSGDJ" Width="230" Height="52" ColumnsNumber="2">
                                <Items>
                                    <ext:Radio runat="server" ID="RadioTipeMediaN_PSGDJ" BoxLabel="Tanpa Media" Checked="true" />
                                    <ext:Radio runat="server" ID="RadioTipeMediaL_PSGDJ" BoxLabel="Link Media" />
                                    <ext:Radio runat="server" ID="RadioTipeMediaU_PSGDJ" BoxLabel="Upload Media" />
                                </Items>
                                <DirectEvents>
                                    <Change OnEvent="RadioTipeMedia_PSGDJ_Change" />
                                </DirectEvents>
                            </ext:RadioGroup>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerTxtMediaDtlJawaban" LabelWidth="100" FieldLabel="Media" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="TxtMediaNDtlJawaban" runat="server" Width="230px" EmptyText="No Media" Hidden="false" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                            <ext:TextField ID="TxtMediaLDtlJawaban" runat="server" Width="230px" EmptyText="Link Media" Hidden="true" />
                            <ext:FileUploadField ID="FU_MediaPaketSoalGroupDtlJawaban" runat="server" AllowBlank="true" EmptyText="Choose File" Hidden="true"
                                LabelWidth="150" ButtonText="Browse" Icon="Attach" Width="230px" AnchorHorizontal="100%">
                                <DirectEvents>
                                    <Change OnEvent="UploadFileDtlJawaban" />
                                </DirectEvents>
                            </ext:FileUploadField>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="TampilanFilePaketSoalGroupDtlJawaban" FieldLabel="TampilanFile" Layout="HBoxLayout" LabelWidth="100">
                        <Items>
                            <ext:Image ID="imgPreviewDtlJawaban" runat="server" Height="200" Width="200" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainertxtTextMedia_PSGDJ" LabelWidth="100" FieldLabel="Text Media" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="txtTextMedia_PSGDJ" runat="server" Width="230px" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer runat="server" ID="FldContainerchbJawabanBenar" LabelWidth="100" FieldLabel="Poin" Layout="HBoxLayout">
                        <Items>
                            <ext:NumberField runat="server" ID="nfPoinJawaban" Width="100" MarginSpec="0 5 0 0" MinValue="0" AllowDecimals="false" />
                            <ext:Label runat="server" Text="Poin" />
                            <%--<ext:Checkbox runat="server" ID="chbJawabanBenar" Checked="false" MarginSpec="0 5 0 0" Height="22" BoxLabel="Benar/Salah" ReadOnlyCls="ReadOnly" />--%>
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAddEditPaketSoalGroupDtlJawaban_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Simpan" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnAddEditPaketSoalGroupDtlJawaban_Save_Click" IsUpload="true" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnAddEditPaketSoalGroupDtlJawaban_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winImportPaketSoalGroupDtl" runat="server" Title="Import Pertanyaan" Icon="DiskUpload" Width="500" Height="150" Hidden="true" Layout="Fit">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelImportPaketSoalGroupDtl" runat="server" Frame="false"
                BodyPadding="5" AnchorHorizontal="100%" Scrollable="Both">
                <Items>
                    <ext:FieldContainer runat="server" LabelWidth="100"
                        FieldLabel="File Import" Layout="HBoxLayout" AnchorHorizontal="100%">
                        <Items>
                            <ext:FileUploadField ID="FU_ImportPaketSoalGroupDtl" runat="server" AllowBlank="true" EmptyText="Choose File"
                                LabelWidth="150" ButtonText="Browse" Icon="Attach" Width="300px" AnchorHorizontal="100%" />
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnImportPaketSoalGroupDtl_Import" Height="30" runat="server" UI="Success" Text="Import" Icon="DiskUpload">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnImportPaketSoalGroupDtl_Import_Click" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnImportPaketSoalGroupDtl_Template" Height="30" runat="server" Text="Template" Icon="PageWhiteExcel">
                        <DirectEvents>
                            <Click OnEvent="btnImportPaketSoalGroupDtl_Template_Click" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnImportPaketSoalGroupDtl_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="WinImportPaketSoalGroupDtlJawaban" runat="server" Title="Import Jawaban" Icon="DiskUpload" Width="500" Height="150" Hidden="true" Layout="Fit">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelImportPaketSoalGroupDtlJawaban" runat="server" Frame="false"
                BodyPadding="5" AnchorHorizontal="100%" Scrollable="Both">
                <Items>
                    <ext:FieldContainer runat="server" LabelWidth="100"
                        FieldLabel="File Import" Layout="HBoxLayout" AnchorHorizontal="100%">
                        <Items>
                            <ext:FileUploadField ID="FU_ImportPaketSoalGroupDtlJawaban" runat="server" AllowBlank="true" EmptyText="Choose File"
                                LabelWidth="150" ButtonText="Browse" Icon="Attach" Width="300px" AnchorHorizontal="100%" />
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="btnImportPaketSoalGroupDtlJawaban_Import" Height="30" runat="server" UI="Success" Text="Import" Icon="DiskUpload">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnImportPaketSoalGroupDtlJawaban_Import_Click" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnImportPaketSoalGroupDtlJawaban_Template" Height="30" runat="server" Text="Template" Icon="PageWhiteExcel">
                        <DirectEvents>
                            <Click OnEvent="btnImportPaketSoalGroupDtlJawaban_Template_Click" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnImportPaketSoalGroupDtlJawaban_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="WinCopyPaketSoalGroupDtl" runat="server" Title="Copy Soal ke GroupSoal" Icon="NoteEdit" Width="700" Height="380" Hidden="true" Layout="Fit">
        <Items>
            <ext:FormPanel Header="false" ID="FormPanelCopyPaketSoalGroupDtl" runat="server" Icon="FolderAdd" Title=""
                BodyPadding="0" AnchorHorizontal="100%" Scrollable="Both">
                <Items>
                    <ext:GridPanel ID="GridPanelCopyPaketSoalGroupDtl" runat="server" Icon="Computer" Title="Group Soal Tujuan"
                        MarginSpec="0 0 0 0" BodyPadding="0" Frame="false" Scrollable="Both" Height="300" ColumnLines="true">
                        <Store>
                            <ext:Store ID="StoreCopyPaketSoalGroupDtl" runat="server" RemoteSort="true" PageSize="10">
                                <Proxy>
                                    <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGroupGridHandler.ashx">
                                        <ActionMethods Read="GET" />
                                        <Reader>
                                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                        </Reader>
                                    </ext:AjaxProxy>
                                </Proxy>
                                <Model>
                                    <ext:Model ID="Model5" runat="server" IDProperty="NoGroup">
                                        <Fields>
                                            <ext:ModelField Name="NoGroup" Type="Int" />
                                            <ext:ModelField Name="NoPaket" Type="Int" />
                                            <ext:ModelField Name="NamaGroup" Type="String" />
                                            <ext:ModelField Name="MinimumJmlSoal" Type="Int" />
                                            <ext:ModelField Name="NilaiStandar" Type="Int" />
                                            <ext:ModelField Name="WaktuPengerjaan" Type="Int" />
                                            <ext:ModelField Name="bRandom" Type="String" />
                                            <ext:ModelField Name="bAktif" Type="String" />
                                            <ext:ModelField Name="UserInput" Type="String" />
                                            <ext:ModelField Name="TimeInput" Type="Date" />
                                            <ext:ModelField Name="UserEdit" Type="String" />
                                            <ext:ModelField Name="TimeEdit" Type="Date" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Parameters>
                                    <ext:StoreParameter Name="PaketSoalGroupFilter" Mode="Raw" Value="'1=1'" />
                                </Parameters>
                                <Sorters>
                                    <ext:DataSorter Property="NoGroup" Direction="ASC" />
                                </Sorters>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel5" runat="server">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn5" Text="No" runat="server" Filterable="false" Width="40" />
                                <ext:Column ID="Column44" runat="server" Text="Status" DataIndex="bAktif" Width="60" />
                                <ext:Column ID="Column45" runat="server" Text="Nama Group" DataIndex="NamaGroup" Width="120" />
                                <ext:Column ID="Column46" runat="server" Text="Minimum Jumlah Soal" DataIndex="MinimumJmlSoal" Width="150" />
                                <ext:Column ID="Column47" runat="server" Text="Nilai Standar" DataIndex="NilaiStandar" Width="90" />
                                <ext:Column ID="Column48" runat="server" Text="Waktu Pengerjaan" DataIndex="WaktuPengerjaan" Width="120" />
                                <ext:Column ID="Column49" runat="server" Text="Acak Nomor Soal" DataIndex="bRandom" Width="110" />
                                <ext:Column ID="Column33" runat="server" Text="No Group Soal" DataIndex="NoGroup" Width="100" />
                            </Columns>
                        </ColumnModel>
                        <Plugins>
                            <ext:FilterHeader ID="FilterHeader5" runat="server" Remote="true" />
                        </Plugins>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar5" runat="server" DisplayInfo="true" DisplayMsg="Displaying Entity(s) {0} - {1} of {2}" EmptyMsg="No Entity(s) to display" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCopyPaketSoalGroupDtl_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Save" Icon="Disk">
                        <LoadingState Text="Processing..." />
                        <DirectEvents>
                            <Click OnEvent="btnCopyPaketSoalGroupDtl_Save_Click" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnCopyPaketSoalGroupDtl_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="this.up('window').hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
        </form>
</asp:Content>
