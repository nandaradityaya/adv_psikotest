<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeFile="MASTER_AssignUjian.aspx.vb" Inherits="MASTER_AssignUjian" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Master Assign Ujian - Psikotest</title>
    <script type="text/javascript">
        var rendererColorLblRek = function (value, meta, record, rowIndex, colIndex, store) {
            if (record.data.LblRek == "DISARANKAN") {
                meta.style = "color: #008000; font-weight: bold;";
            } else if (record.data.LblRek == "DIPERTIMBANGKAN") {
                meta.style = "color: #E67E00; font-weight: bold;";
            } else if (record.data.LblRek == "TIDAK DISARANKAN") {
                meta.style = "color: #FF0000; font-weight: bold;";
            }
            return value;
        }
        var rendererColorStatusPengerjaan = function (value, meta, record, rowIndex, colIndex, store) {
            if (record.data.StatusPengerjaan == "Selesai Ujian") {
                meta.style = "color: #008000; font-weight: bold;";
            } else if (record.data.StatusPengerjaan == "Sedang Ujian") {
                meta.style = "color: #E67E00; font-weight: bold;";
            } else if (record.data.StatusPengerjaan == "Belum Ujian") {
                meta.style = "color: #FF0000; font-weight: bold;";
            }
            return value;
        }
        function renderCekNilai(grid, toolbar, rowIndex, record) {
            var btnCekNilai = toolbar.items.get(0);
            if (record.data.StatusPengerjaan != 'Selesai Ujian') {
                btnCekNilai.hide();
            }
        }
        function renderEditPesertaDtl(grid, toolbar, rowIndex, record) {
            var btnEditPesertaDtl = toolbar.items.get(0);
            if (record.data.StatusPengerjaan == 'Selesai Ujian') {
                btnEditPesertaDtl.hide();
            }
        }
        function renderResendUndanganPesertaDtl(grid, toolbar, rowIndex, record) {
            var btnResendUndangan = toolbar.items.get(0);
            if (record.data.StatusPengerjaan == 'Selesai Ujian') {
                btnResendUndangan.hide();
            }
        }
        //function renderAssignPeserta(grid, toolbar, rowIndex, record) {
        //    var btnEditPesertaDtl = toolbar.items.get(0);
        //    if (record.data.Alamat == '' || record.data.TglLahir == '' || record.data.LamarSebagai == '') {
        //        btnEditPesertaDtl.hide();
        //    }
        //}
        function renderPesanTerkirim(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.bKirim == "Terbaca") {
                meta.style = "background-color: #00FF00;"; //Green
            } else if (record.data.bKirim == "Diterima") {
                meta.style = "background-color: #87CEFA;"; //Blue
            } else if (record.data.bKirim == "Terkirim") {
                meta.style = "background-color: #FFFF00;"; //Yellow
            } else {
                meta.style = "background-color: #D3D3D3;"; //Grey
            }
            return value;
        }
        function renderIsPrioritas(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.IsPrioritas == -1) {
                meta.style = "background-color: #FFDF00;"; //Gold
            }
            return value;
        }
        function renderNilaiGroupResult(value, meta, record, rowIndex, colIndex, store) {
            if (parseInt(record.data.NilaiStandard) < parseInt(record.data.NilaiGroupResult)) {
                meta.style = "background-color: lime;";
            } else if (record.data.InStandardNorma !== null && record.data.IsPrioritas !== '1') {
                meta.style = "background-color: gold;";
            } else {
                meta.style = "background-color: red;";
            }
            return value;
        }
        function renderJawabanDipilih(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.JawabanBenar == 1) {
                meta.style = "background-color: #28A745;"; //Green
            } else if (record.data.Jawaban == "") {
                meta.style = "background-color: #808080;"; //Grey
            } else {
                meta.style = "background-color: #DC3545;"; //Red
            }
            return value;
        }

        function copyToClipboard(text) {
            // Create a temporary contenteditable element
            const tempElement = document.createElement('div');
            tempElement.contentEditable = true;
            tempElement.innerHTML = text;

            // Append the element to the DOM
            document.body.appendChild(tempElement);

            // Select the text in the element
            const range = document.createRange();
            range.selectNodeContents(tempElement);
            const selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);

            // Copy the selected text to the clipboard
            document.execCommand('copy');

            // Remove the temporary element
            document.body.removeChild(tempElement);
        }
        function reloadKaloAda(store) {
            if (App[store] != undefined) {
                App[store].reload();
            }
        }


        const slideWindowWidth = 325;
        const slideWidth = 325;
        let currTrsX = 0;
        let maxTrsX = 0;
        let totalSlidesWidth = 0;


        function setupBoxFotoPeserta(s) {
            var data = JSON.parse(s);

            var container = document.getElementById("ContainerFotoPeserta");
            while (container.firstChild) {
                container.removeChild(container.lastChild);
            }

            if (data.length == 0) {
                var lblTidakAdaFoto = document.createElement("span");
                lblTidakAdaFoto.classList.add("lblTidakAdaFoto");
                lblTidakAdaFoto.innerHTML = "TIDAK ADA FOTO";

                container.appendChild(lblTidakAdaFoto);
                return
            }

            var contFotoPeserta = document.createElement("div");
            contFotoPeserta.id = "cont-FotoPeserta";
            for (var i = 0; i < data.length; i++) {
                let deteksiWajah = document.createElement("span");
                deteksiWajah.classList.add("LblWajahTidakTerdeteksi");

                fetch("ViewDeteksiWajahHasilUjian.ashx?Loc=" + data[i].Loc)
                    .then(response => response.text())
                    .then(data => {
                        deteksiWajah.innerHTML += data;
                    })
                    .catch(error => {
                        console.error('Error loading data for parameter "ViewDeteksiWajahHasilUjian.ashx"' + data[i].Loc, error);
                    });


                var img = document.createElement("img");
                img.classList.add("FotoPeserta");
                img.src = "ViewFotoPeserta.ashx?Loc=" + data[i].Loc;

                var p = document.createElement("p");
                p.classList.add("LblFotoPeserta");
                p.innerHTML = data[i].ts;

                var innerDiv = document.createElement("div");
                innerDiv.appendChild(deteksiWajah);
                innerDiv.appendChild(img);
                innerDiv.appendChild(p);

                contFotoPeserta.appendChild(innerDiv);
            }
            container.appendChild(contFotoPeserta);

            var prevBtn = document.createElement("button");
            prevBtn.id = "prevBtn";
            prevBtn.classList.add("navFotoBtn");
            prevBtn.addEventListener("click", prevSlide);
            prevBtn.innerText = "❮";

            var nextBtn = document.createElement("button");
            nextBtn.id = "nextBtn";
            nextBtn.classList.add("navFotoBtn");
            nextBtn.addEventListener("click", nextSlide);
            nextBtn.innerText = "❯";

            container.appendChild(prevBtn);
            container.appendChild(nextBtn);

            totalSlidesWidth = data.length * slideWidth;
            maxTrsX = Math.max(totalSlidesWidth - slideWindowWidth, 0);

            currTrsX = 0;
            showSlide();
        }

        function showSlide() {
            const slider = document.getElementById('cont-FotoPeserta');
            slider.style.transform = `translateX(${-currTrsX}px)`;
        }

        function nextSlide() {
            event.preventDefault();
            currTrsX = Math.min(currTrsX + slideWindowWidth, maxTrsX);
            showSlide();
        }

        function prevSlide() {
            event.preventDefault();
            currTrsX = Math.max(currTrsX - slideWindowWidth, 0);
            showSlide();
        }
    </script>
    <style type="text/css">
        .card-header {
            background-color: rgb(0 0 0 / 3%) !important;
        }

        #ctl00_ContentPlaceHolder1_ContainerFotoPeserta-innerCt {
            padding: 0px 10px !important;
        }

        #cont-FotoPeserta {
            display: flex;
        }

        .FotoPeserta {
            width: 315px;
            margin: 5px;
        }

        .LblFotoPeserta {
            display: flex;
            justify-content: center;
        }

        .lblTidakAdaFoto {
            display: flex;
            justify-content: center;
            font-size: 100px;
            height: 235px;
            align-items: center;
            color: red;
        }

        .LblWajahTidakTerdeteksi {
            display: flex;
            justify-content: center;
            font-size: 23px;
            height: 30px;
            align-items: center;
            color: red;
        }

        .navFotoBtn {
            position: absolute;
            top: 50%;
            padding: 10px;
            font-size: 18px;
            cursor: pointer;
            border: none;
            background-color: transparent;
            color: white;
        }

        #prevBtn {
            left: 10px;
        }

        #nextBtn {
            right: 10px;
        }

        .fancybox-slide--html {
            padding: 44px 2px;
        }

        .card-body {
            -webkit-box-flex: 1;
            -ms-flex: 1 1 auto;
            flex: 1 1 auto;
            padding: 1.25rem;
        }

        .text-center {
            text-align: center;
        }

        .vis-hide {
            visibility: hidden;
        }
        /*Tombol Bawah Undang*/
        #ToolbarUndangPsikotest, #ToolbarUndangInterview {
            border: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuHead" runat="server">
    <a href="#">Data Pelamar</a>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
        <ext:Hidden ID="hdnSoloAssign" runat="server" />
        <ext:Hidden ID="hdnMultiAssign" runat="server" />

        <ext:Hidden ID="hdnSoloAssignNoPeserta" runat="server" />

        <ext:Hidden ID="hdnUserID" runat="server" />
        <ext:Hidden ID="hdnSeqNo" runat="server" />

        <ext:Hidden ID="hdnFilterPeserta" runat="server" />
        <ext:Hidden ID="hdnFilterPesertaDtl" runat="server" />
        <ext:Hidden ID="hdnFilterPesertaInterview" runat="server" />
        <ext:Hidden ID="hdnFilterNilai" runat="server" />
        <ext:Hidden ID="hdnFilterNilaiDtl" runat="server" />
        <ext:FormPanel ID="FormPanelFilterPeserta" runat="server" Title=" " MarginSpec="0 0 0 0" BodyPadding="10"
            Frame="false" Width="500px" Height="200px" Header="false" AnchorHorizontal="100%" AutoScroll="false" MonitorResize="true">
            <Items>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Tanggal Input" Height="22" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:Checkbox ID="chbFilterTimeInput" runat="server" />
                        <ext:Label ID="label5" Text="&nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTimeInput_From" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                        <ext:Label ID="label6" Text="&nbsp; To &nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTimeInput_To" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Posisi" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:TextField runat="server" ID="cmbFilterPosisi" ReadOnly="true" EmptyText="Semua Posisi" Width="200" />
                        <ext:Hidden runat="server" ID="hdnFilterPosisi" />
                        <ext:Button runat="server" ID="btnSelectFilterPosisi" Text="" Icon="Zoom">
                            <DirectEvents>
                                <Click OnEvent="btnSelectFilterPosisi_Click" />
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="BKK/Cabang" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:TextField runat="server" ID="cmbFilterCabang" ReadOnly="true" EmptyText="Semua Cabang" Width="200" />
                        <ext:Hidden runat="server" ID="hdnFilterCabang" />
                        <ext:Button runat="server" ID="btnSelectFilterCabang" Text="" Icon="Zoom">
                            <DirectEvents>
                                <Click OnEvent="btnSelectFilterCabang_Click" />
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Tanggal Lahir" Height="22" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:Checkbox ID="chbFilterTglLahir" runat="server" />
                        <ext:Label ID="label7" Text="&nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglLahir_From" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                        <ext:Label ID="label8" Text="&nbsp; To &nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglLahir_To" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Hasil Psikotest" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:SelectBox ID="CmbFilterhasilPsikotestPeserta" runat="server" Width="200" EmptyText="Semua Hasil">
                            <Items>
                                <ext:ListItem Text="DISARANKAN" Value="3" />
                                <ext:ListItem Text="DIPERTIMBANGKAN" Value="2" />
                                <ext:ListItem Text="TIDAK DISARANKAN" Value="1" />
                                <ext:ListItem Text="BELUM UJIAN" Value="0" />
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
        <ext:GridPanel ID="GridPanelPeserta" runat="server" Icon="Application" Title="Peserta"
            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="399">
            <Store>
                <ext:Store ID="StorePeserta" runat="server" RemoteSort="true" PageSize="10">
                    <Proxy>
                        <ext:AjaxProxy Url="GridHandlers/MASTER_PesertaGridHandler.ashx">
                            <ActionMethods Read="GET" />
                            <Reader>
                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                            </Reader>
                        </ext:AjaxProxy>
                    </Proxy>
                    <Model>
                        <ext:Model ID="Model4" runat="server" IDProperty="NoPeserta">
                            <Fields>
                                <ext:ModelField Name="NoPeserta" Type="String" />
                                <ext:ModelField Name="NoKTP" Type="String" />
                                <ext:ModelField Name="NamaPeserta" Type="String" />
                                <ext:ModelField Name="TglLahir" Type="Date" />
                                <ext:ModelField Name="Alamat" Type="String" />
                                <ext:ModelField Name="NoHp" Type="String" />
                                <ext:ModelField Name="Email" Type="String" />
                                <ext:ModelField Name="AsalRekrutan" Type="String" />
                                <ext:ModelField Name="DaftarKe" Type="String" />
                                <ext:ModelField Name="LamarSebagai" Type="String" />
                                <ext:ModelField Name="LblRek" Type="String" />
                                <ext:ModelField Name="UserInput" Type="String" />
                                <ext:ModelField Name="TimeInput" Type="Date" />
                                <ext:ModelField Name="UserEdit" Type="String" />
                                <ext:ModelField Name="TimeEdit" Type="Date" />
                            </Fields>
                        </ext:Model>
                    </Model>
                    <Parameters>
                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterPeserta}.getValue()" />
                    </Parameters>
                    <Sorters>
                        <ext:DataSorter Property="TimeInput" Direction="DESC" />
                    </Sorters>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel4" runat="server">
                <Columns>
                    <ext:RowNumbererColumn runat="server" Text="No" Filterable="false" Width="40" />
                    <ext:CommandColumn ID="CommandColumn1" runat="server" Width="68" Text="" Align="Center">
                        <%--<PrepareToolbar Fn="renderAssignPeserta" />--%>
                        <Commands>
                            <ext:GridCommand Icon="FolderAdd" StandOut="true" CommandName="Edit" Text="Undang" />
                        </Commands>
                        <DirectEvents>
                            <Command OnEvent="btnAssignPeserta_Click">
                                <ExtraParams>
                                    <ext:Parameter Name="NoPeserta" Value="record.data.NoPeserta" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:CommandColumn>
                    <ext:Column ID="Column3" runat="server" Text="Nomor Peserta" DataIndex="NoPeserta" Width="100" />
                    <ext:Column ID="Column23" runat="server" Text="Nama Peserta" DataIndex="NamaPeserta" Width="180" />
                    <ext:DateColumn ID="DateColumn9" runat="server" Text="Tgl Lahir" DataIndex="TglLahir" Width="80" Format="dd-MMM-yyyy" Filterable="false" />
                    <ext:Column ID="Column24" runat="server" Text="Alamat" DataIndex="Alamat" Width="100" />
                    <ext:Column ID="Column25" runat="server" Text="Nomor Hp" DataIndex="NoHp" Width="100" />
                    <ext:Column ID="Column26" runat="server" Text="Email" DataIndex="Email" Width="100" />
                    <ext:Column ID="Column19" runat="server" Text="Asal Rekrutan" DataIndex="AsalRekrutan" Width="150" />
                    <ext:Column ID="Column27" runat="server" Text="Daftar Ke" DataIndex="DaftarKe" Width="100" />
                    <ext:Column ID="Column28" runat="server" Text="Posisi" DataIndex="LamarSebagai" Width="100" />
                    <ext:Column ID="Column29" runat="server" Text="Nomor KTP" DataIndex="NoKTP" Width="100" />
                    <ext:Column ID="Column46" runat="server" Text="Hasil Psikotest" DataIndex="LblRek" Width="100">
                        <Renderer Fn="rendererColorLblRek" />
                    </ext:Column>
                    <ext:Column ID="Column30" runat="server" Text="User Input" DataIndex="UserInput" Width="100" />
                    <ext:DateColumn ID="DateColumn10" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                    <ext:Column ID="Column31" runat="server" Text="User Edit" DataIndex="UserEdit" Width="100" />
                    <ext:DateColumn ID="DateColumn11" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                </Columns>
            </ColumnModel>
            <Plugins>
                <ext:FilterHeader ID="FilterHeader4" runat="server" Remote="true" />
            </Plugins>
            <SelectionModel>
                <ext:CheckboxSelectionModel runat="server" Mode="Multi">
                    <DirectEvents>
                        <Select OnEvent="SelectedPeserta_Click" />
                    </DirectEvents>
                </ext:CheckboxSelectionModel>
            </SelectionModel>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button ID="btnAssignBaris" runat="server" UI="Info" Text="Undang Banyak" Icon="NoteAdd" Height="30" Width="130" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnAssignBarisUjian_Click" IsUpload="true" />
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolbar4" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display">
                    <Items>
                        <ext:NumberField runat="server" ID="nfGPPesertaTotalRow" FieldLabel="Total Baris" HideTrigger="true" LabelWidth="70" Width="105" Height="22">
                            <Listeners>
                                <Change Handler="#{GridPanelPeserta}.getStore().pageSize = #{nfGPPesertaTotalRow}.getValue();" />
                            </Listeners>
                        </ext:NumberField>
                    </Items>
                    <DirectEvents>
                        <Change OnEvent="GridPanelPesertaPageChanged" />
                    </DirectEvents>
                </ext:PagingToolbar>
            </BottomBar>
        </ext:GridPanel>
        <ext:FormPanel ID="FormPanelFilterPeserta2" runat="server" Title=" " MarginSpec="0 0 0 0" BodyPadding="10"
            Frame="false" Width="500px" Height="200px" Header="false" AnchorHorizontal="100%" AutoScroll="false" MonitorResize="true">
            <Items>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Tanggal Input" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:Checkbox ID="chbFilterTglInput" runat="server" />
                        <ext:Label ID="label3" Text="&nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglInput_From" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                        <ext:Label ID="label1" Text="&nbsp; To &nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglInput_To" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Tanggal Ujian" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:Checkbox ID="chbFilterTglUjian" runat="server" />
                        <ext:Label ID="label4" Text="&nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglUjian_From" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                        <ext:Label ID="label2" Text="&nbsp; To &nbsp;" runat="server" />
                        <ext:DateField ID="DF_FilterTglUjian_To" runat="server" Format="dd-MMM-yyyy" Width="100px" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Status Pengerjaan" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:SelectBox ID="CmbFilterStatusPesertaDtl" runat="server" Width="200" EmptyText="Semua Status">
                            <Items>
                                <ext:ListItem Text="Belum Ujian" Value="0" />
                                <ext:ListItem Text="Sedang Ujian" Value="1" />
                                <ext:ListItem Text="Selesai Ujian" Value="2" />
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
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Status Interview" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:SelectBox ID="CmbFilterStatusPesertaInterview" runat="server" Width="200" EmptyText="Semua Status">
                            <Items>
                                <ext:ListItem Text="Belum Interview" Value="0" />
                                <ext:ListItem Text="Sedang Interview" Value="1" />
                                <ext:ListItem Text="Selesai Interview" Value="2" />
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
                <ext:FieldContainer runat="server" LabelWidth="150" FieldLabel="Hasil Psikotest" Layout="HBoxLayout" Margins="0 5 5 5" BodyPadding="2">
                    <Items>
                        <ext:SelectBox ID="CmbFilterhasilPsikotestUjian" runat="server" Width="200" EmptyText="Semua Hasil">
                            <Items>
                                <ext:ListItem Text="DISARANKAN" Value="0" />
                                <ext:ListItem Text="DIPERTIMBANGKAN" Value="1" />
                                <ext:ListItem Text="TIDAK DISARANKAN" Value="2" />
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
                <ext:Toolbar ID="Toolbar1" runat="server" Dock="Bottom">
                    <Items>
                        <ext:Button ID="btnFilterPesertaDtl" runat="server" Height="30" Width="100" MarginSpec="0 5 5 0" Text="Filter" Icon="ArrowRefresh" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnFilterPeserta2_Click">
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnExportPsikotest" runat="server" Height="30" Width="120" MarginSpec="0 5 5 0" Text="Export Psikotest" Icon="PageWhiteExcel" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnExportPsikotest_Click" IsUpload="true">
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnExportInterview" runat="server" Height="30" Width="120" MarginSpec="0 5 5 0" Text="Export Interview" Icon="PageWhiteExcel" StandOut="true">
                            <DirectEvents>
                                <Click OnEvent="btnExportInterview_Click" IsUpload="true">
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:FormPanel>
        <ext:TabPanel runat="server" ID="TabPanel1">
            <Items>
                <ext:Panel runat="server" ID="PanelPsikotest" Title="Psikotest">
                    <TabConfig runat="server" UI="Primary" />
                    <Items>
                        <ext:GridPanel ID="GridPanelPesertaDtl" runat="server" Icon="Application" Title="Peserta Ujian"
                            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="364">
                            <Store>
                                <ext:Store ID="StorePesertaDtl" runat="server" RemoteSort="true" PageSize="10">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GridHandlers/MASTER_PesertaDtlGridHandler.ashx">
                                            <ActionMethods Read="GET" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model1" runat="server" IDProperty="UserId">
                                            <Fields>
                                                <ext:ModelField Name="Batch" Type="String" />
                                                <ext:ModelField Name="NoPerserta" Type="Int" />
                                                <ext:ModelField Name="NamaPeserta" Type="String" />
                                                <ext:ModelField Name="UserId" Type="String" />
                                                <ext:ModelField Name="Password" Type="String" />
                                                <ext:ModelField Name="block" Type="String" />
                                                <ext:ModelField Name="UndangPsikotestKe" Type="String" />
                                                <ext:ModelField Name="StatusPengerjaan" Type="String" />
                                                <ext:ModelField Name="LblRek" Type="String" />
                                                <ext:ModelField Name="StartTest" Type="Date" />
                                                <ext:ModelField Name="URL" Type="String" />
                                                <ext:ModelField Name="NamaPaket" Type="String" />
                                                <ext:ModelField Name="WaktuTest" Type="Date" />
                                                <ext:ModelField Name="bKirim" Type="String" />
                                                <ext:ModelField Name="WaktuKirim" Type="Date" />
                                                <ext:ModelField Name="WaktuTerbaca" Type="Date" />
                                                <ext:ModelField Name="UserInput" Type="String" />
                                                <ext:ModelField Name="TimeInput" Type="Date" />
                                                <ext:ModelField Name="UserEdit" Type="String" />
                                                <ext:ModelField Name="TimeEdit" Type="Date" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterPesertaDtl}.getValue()" />
                                    </Parameters>
                                    <Sorters>
                                        <ext:DataSorter Property="TimeInput" Direction="DESC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Text="No" runat="server" Filterable="false" Width="40" />
                                    <ext:CommandColumn ID="CommandColumn2" runat="server" Width="49" Text="" Align="Center">
                                        <PrepareToolbar Fn="renderEditPesertaDtl" />
                                        <Commands>
                                            <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="btnEditPesertaUjian_Click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="UserId" Value="record.data.UserId" Mode="Raw" />
                                                </ExtraParams>
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn ID="CommandColumn4" runat="server" Width="66" Text="" Align="Center">
                                        <PrepareToolbar Fn="renderResendUndanganPesertaDtl" />
                                        <Commands>
                                            <ext:GridCommand Icon="BuildingGo" StandOut="true" CommandName="Edit" Text="Resend" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="btnResendUndanganPsikotest_Click">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="UserId" Value="record.data.UserId" Mode="Raw" />
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmation" Message="Anda yakin kirim ulang undangan?" />
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn ID="CommandColumn3" runat="server" Width="48" Text="" Align="Center">
                                        <PrepareToolbar Fn="renderCekNilai" />
                                        <Commands>
                                            <ext:GridCommand Icon="FolderMagnify" StandOut="true" CommandName="Cek" Text="Cek" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="btnCekPesertaUjian_Click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="UserId" Value="record.data.UserId" Mode="Raw" />
                                                </ExtraParams>
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:Column ID="Column33" runat="server" Text="Batch" DataIndex="Batch" Width="100" />
                                    <ext:Column ID="Column1" runat="server" Text="Nomor Peserta" DataIndex="NoPeserta" Width="100" />
                                    <ext:Column ID="Column37" runat="server" Text="Nama Peserta" DataIndex="NamaPeserta" Width="180" />
                                    <ext:Column ID="Column2" runat="server" Text="UserID" DataIndex="UserId" Width="100" />
                                    <ext:Column ID="Column32" runat="server" Text="Block" DataIndex="block" Width="80" />
                                    <ext:Column ID="Column4" runat="server" Text="Undang Ke" DataIndex="UndangPsikotestKe" Width="80" />
                                    <ext:Column ID="Column10" runat="server" Text="Status Pengerjaan" DataIndex="StatusPengerjaan" Width="150">
                                        <Renderer Fn="rendererColorStatusPengerjaan" />
                                    </ext:Column>
                                    <ext:Column ID="Column47" runat="server" Text="Hasil Psikotest" DataIndex="LblRek" Width="150">
                                        <Renderer Fn="rendererColorLblRek" />
                                    </ext:Column>
                                    <ext:Column ID="Column5" runat="server" Text="Link Login" DataIndex="Url" Width="100" />
                                    <ext:Column ID="Column6" runat="server" Text="Nama Paket Soal" DataIndex="NamaPaket" Width="125" />
                                    <ext:DateColumn ID="DateColumn2" runat="server" Text="Waktu Test" DataIndex="WaktuTest" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:DateColumn ID="DateColumn1" runat="server" Text="Waktu dimulai" DataIndex="StartTest" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column7" runat="server" Text="Status Pesan" DataIndex="bKirim" Width="100">
                                        <Renderer Fn="renderPesanTerkirim" />
                                    </ext:Column>
                                    <ext:DateColumn ID="DateColumn6" runat="server" Text="Waktu terkirim" DataIndex="WaktuKirim" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:DateColumn ID="DateColumn5" runat="server" Text="Waktu terbaca" DataIndex="WaktuTerbaca" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column8" runat="server" Text="User Input" DataIndex="UserInput" Width="100" />
                                    <ext:DateColumn ID="DateColumn3" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column9" runat="server" Text="User Edit" DataIndex="UserEdit" Width="100" />
                                    <ext:DateColumn ID="DateColumn4" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Single" />
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display">
                                    <Items>
                                        <ext:Button ID="btnGetLink" runat="server" Text="Salin Link" Icon="Clipboard" StandOut="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnGetLink_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:TextField ID="txtCopyPlaceholder" runat="server" Cls="vis-hide" ClientIDMode="Static" />
                                    </Items>
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" ID="PanelInterview" Title="Interview">
                    <TabConfig runat="server" UI="Warning" />
                    <Items>
                        <ext:GridPanel ID="GridPanelPesertaInterview" runat="server" Icon="Application" Title="Peserta Interview"
                            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="364">
                            <Store>
                                <ext:Store ID="StorePesertaInterview" runat="server" RemoteSort="true" PageSize="10">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GridHandlers/MASTER_PesertaInterviewGridHandler.ashx">
                                            <ActionMethods Read="GET" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model8" runat="server" IDProperty="SeqNo">
                                            <Fields>
                                                <ext:ModelField Name="SeqNo" Type="String" />
                                                <ext:ModelField Name="NoPeserta" Type="String" />
                                                <ext:ModelField Name="NamaPeserta" Type="String" />
                                                <ext:ModelField Name="WaktuInterview" Type="Date" />
                                                <ext:ModelField Name="Lokasi" Type="String" />
                                                <ext:ModelField Name="Batch" Type="String" />
                                                <ext:ModelField Name="StatusInterview" Type="String" />
                                                <ext:ModelField Name="UndangInterviewKe" Type="String" />
                                                <ext:ModelField Name="bKirim" Type="String" />
                                                <ext:ModelField Name="WaktuKirim" Type="Date" />
                                                <ext:ModelField Name="WaktuTerbaca" Type="Date" />
                                                <ext:ModelField Name="UserInput" Type="String" />
                                                <ext:ModelField Name="TimeInput" Type="Date" />
                                                <ext:ModelField Name="UserEdit" Type="String" />
                                                <ext:ModelField Name="TimeEdit" Type="Date" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterPesertaInterview}.getValue()" />
                                    </Parameters>
                                    <Sorters>
                                        <ext:DataSorter Property="TimeInput" Direction="DESC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel6" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Text="No" runat="server" Filterable="false" Width="40" />
                                    <ext:CommandColumn ID="CommandColumn5" runat="server" Width="49" Text="" Align="Center">
                                        <PrepareToolbar Fn="renderEditPesertaDtl" />
                                        <Commands>
                                            <ext:GridCommand Icon="FolderEdit" StandOut="true" CommandName="Edit" Text="Edit" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="btnEditPesertaInterview_Click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                                </ExtraParams>
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn ID="CommandColumn6" runat="server" Width="66" Text="" Align="Center">
                                        <PrepareToolbar Fn="renderResendUndanganPesertaDtl" />
                                        <Commands>
                                            <ext:GridCommand Icon="BuildingGo" StandOut="true" CommandName="Edit" Text="Resend" />
                                        </Commands>
                                        <DirectEvents>
                                            <Command OnEvent="btnResendUndanganInterview_Click">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="SeqNo" Value="record.data.SeqNo" Mode="Raw" />
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmation" Message="Anda yakin kirim ulang undangan?" />
                                            </Command>
                                        </DirectEvents>
                                    </ext:CommandColumn>
                                    <ext:Column ID="Column34" runat="server" Text="Batch" DataIndex="Batch" Width="100" />
                                    <ext:Column ID="Column35" runat="server" Text="Nomor Peserta" DataIndex="NoPeserta" Width="100" />
                                    <ext:Column ID="Column39" runat="server" Text="Nama Peserta" DataIndex="NamaPeserta" Width="180" />
                                    <ext:DateColumn ID="DateColumn7" runat="server" Text="Waktu Interview" DataIndex="WaktuInterview" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column41" runat="server" Text="Lokasi" DataIndex="Lokasi" Width="125" />
                                    <ext:Column ID="Column40" runat="server" Text="Status Interview" DataIndex="StatusInterview" Width="120" />
                                    <ext:Column ID="Column38" runat="server" Text="Undang Ke" DataIndex="UndangInterviewKe" Width="80" />
                                    <ext:Column ID="Column36" runat="server" Text="Status Pesan" DataIndex="bKirim" Width="100">
                                        <Renderer Fn="renderPesanTerkirim" />
                                    </ext:Column>
                                    <ext:DateColumn ID="DateColumn8" runat="server" Text="Waktu terkirim" DataIndex="WaktuKirim" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:DateColumn ID="DateColumn12" runat="server" Text="Waktu terbaca" DataIndex="WaktuTerbaca" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column43" runat="server" Text="User Input" DataIndex="UserInput" Width="100" />
                                    <ext:DateColumn ID="DateColumn14" runat="server" Text="Time Input" DataIndex="TimeInput" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                    <ext:Column ID="Column44" runat="server" Text="User Edit" DataIndex="UserEdit" Width="100" />
                                    <ext:DateColumn ID="DateColumn15" runat="server" Text="Time Edit" DataIndex="TimeEdit" Width="110" Format="dd-MMM-yyyy HH:mm" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader6" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Single" />
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar6" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
                            </BottomBar>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:TabPanel>


        <%--WINDOW--%>
        <ext:Window ID="winUndangPeserta" runat="server" Title="Undang" Icon="NoteAdd" Hidden="true" Width="330" Height="330" Layout="FitLayout">
            <Items>
                <ext:TabPanel runat="server" ID="TabPanelAssignUjian" AnchorHorizontal="100%" MonitorResize="true" Layout="FitLayout">
                    <Items>
                        <ext:Panel runat="server" ID="PanelAssignPsikotest" Title="Psikotest" Layout="FitLayout">
                            <TabConfig runat="server" UI="Primary" />
                            <Items>
                                <ext:FormPanel Header="false" ID="FormPanelAssignUjian" runat="server" BodyPadding="5">
                                    <Items>
                                        <ext:FieldContainer ID="FldContainerCmbTipeBatchPsikotest" runat="server" LabelWidth="100" FieldLabel="Batch" Layout="HBoxLayout">
                                            <Items>
                                                <ext:SelectBox ID="CmbTipeBatchPsikotest" runat="server" EmptyText="Pilih Tipe Batch" Width="180px">
                                                    <Items>
                                                        <ext:ListItem Text="Batch" Value="1" />
                                                        <ext:ListItem Text="NonBatch" Value="0" />
                                                    </Items>
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.getTrigger(0).show();" />
                                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.getTrigger(0).hide();}" />
                                                    </Listeners>
                                                    <DirectEvents>
                                                        <Change OnEvent="CmbTipeBatchPsikotest_Change" />
                                                    </DirectEvents>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FldContainerBatchPsikotest" runat="server" LabelWidth="100" FieldLabel="Nama Batch" Layout="HBoxLayout" Hidden="true">
                                            <Items>
                                                <ext:TextArea ID="txtBatchPsikotest" runat="server" Width="180px" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Tanggal" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DateField ID="DF_WaktuAssignUjian" runat="server" Format="dddd, dd MMM yyyy" Width="180px" EmptyText="Tanggal Ujian" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Waktu" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField ID="TF_WaktuAssignUjian" runat="server" Width="40px">
                                                    <Plugins>
                                                        <ext:InputMask runat="server" Mask="ab:cd" AlwaysShow="true">
                                                            <MaskSymbols>
                                                                <ext:MaskSymbol Name="a" Regex="[012]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="b" Regex="[0-9]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="c" Regex="[0-5]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="d" Regex="[0-9]" Placeholder="_" />
                                                            </MaskSymbols>
                                                        </ext:InputMask>
                                                    </Plugins>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:Hidden runat="server" ID="hdnFilterPaketSoalAssignUjian" />
                                        <ext:Hidden runat="server" ID="hdnFilterPaketSoalAssignUjianNoPeserta" />
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Nama Paket Soal" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DropDownField runat="server" ID="CmbPaketSoalAssignUjian" Editable="false" Mode="ValueText" TriggerIcon="Combo" EmptyText="Pilih Paket Soal" Width="180px" ReadOnlyCls="ReadOnly">
                                                    <Listeners>
                                                        <Expand Handler="this.picker.setWidth(450);" />
                                                    </Listeners>
                                                    <Component>
                                                        <ext:GridPanel ID="GridPanelPaketSoalAssignUjian" ColumnLines="true" Cls="x-grid-dir" Flex="1" runat="server" Height="340" Width="350" Mode="ValueText" Title="Paket Soal" Frame="true">
                                                            <Store>
                                                                <ext:Store ID="StorePaketSoalAssignUjian" runat="server" RemoteSort="true" PageSize="10">
                                                                    <Proxy>
                                                                        <ext:AjaxProxy Url="GridHandlers/MASTER_PaketSoalGridHandler.ashx">
                                                                            <ActionMethods Read="GET" />
                                                                            <Reader>
                                                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                                            </Reader>
                                                                        </ext:AjaxProxy>
                                                                    </Proxy>
                                                                    <Model>
                                                                        <ext:Model ID="Model3" runat="server" IDProperty="NoPaket">
                                                                            <Fields>
                                                                                <ext:ModelField Name="NoPaket" Type="String" />
                                                                                <ext:ModelField Name="NamaPaket" Type="String" />
                                                                                <ext:ModelField Name="ToleransiWaktu" Type="String" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                    <Parameters>
                                                                        <ext:StoreParameter Name="PaketSoalFilter" Mode="Raw" Value="#{hdnFilterPaketSoalAssignUjian}.getValue()" />
                                                                        <ext:StoreParameter Name="NoPesertaFilter" Mode="Raw" Value="#{hdnFilterPaketSoalAssignUjianNoPeserta}.getValue()" />
                                                                    </Parameters>
                                                                    <Sorters>
                                                                        <ext:DataSorter Property="NoPaket" Direction="ASC" />
                                                                    </Sorters>
                                                                </ext:Store>
                                                            </Store>
                                                            <ColumnModel ID="ColumnModel3" runat="server">
                                                                <Columns>
                                                                    <ext:Column ID="Column20" runat="server" Text="Nomor Paket Soal" DataIndex="NoPaket" Width="110" />
                                                                    <ext:Column ID="Column21" runat="server" Text="Nama Paket Soal" DataIndex="NamaPaket" Flex="1" />
                                                                    <ext:Column ID="Column22" runat="server" Text="Toleransi Waktu (Menit)" DataIndex="ToleransiWaktu" Width="140" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <Plugins>
                                                                <ext:FilterHeader ID="FilterHeader3" runat="server" Remote="true" />
                                                            </Plugins>
                                                            <SelectionModel>
                                                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" Mode="Single">
                                                                    <DirectEvents>
                                                                        <Select OnEvent="SelectNamaPaketAssignUjian"></Select>
                                                                    </DirectEvents>
                                                                </ext:RowSelectionModel>
                                                            </SelectionModel>
                                                            <BottomBar>
                                                                <ext:PagingToolbar ID="PagingToolbar3" runat="server" DisplayInfo="true" DisplayMsg="Displaying Entity(s) {0} - {1} of {2}" EmptyMsg="No Entity(s) to display" />
                                                            </BottomBar>
                                                        </ext:GridPanel>
                                                    </Component>
                                                </ext:DropDownField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FldContainerChbBlock" runat="server" LabelWidth="100" FieldLabel="Block" Layout="HBoxLayout">
                                            <Items>
                                                <ext:Checkbox runat="server" ID="chbBlock" Checked="false" MarginSpec="0 5 0 0" BoxLabelCls="status_lbl" ReadOnlyCls="ReadOnly" BoxLabel="Aktif/Nonaktif" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                    <BottomBar>
                                        <ext:Toolbar runat="server" ID="ToolbarUndangPsikotest" Frame="false" ClientIDMode="Static">
                                            <LayoutConfig>
                                                <ext:HBoxLayoutConfig Pack="End" />
                                            </LayoutConfig>
                                            <Items>
                                                <ext:Button ID="btnAssignUjian_GC" Height="30" runat="server" UI="Info" StandOut="true" Text="Atur Sekarang" Icon="Clock" MarginSpec="2 3" Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAssignUjian_GC_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="btnAssignUjian_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Save & Kirim" Icon="Disk" Width="100" MarginSpec="2 3" AutoLoadingState="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAssignUjian_Save_Click">
                                                            <Confirmation ConfirmRequest="true" Title="Confirmation" Message="Anda yakin dengan data Psikotest ini?" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="btnAssignUjian_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel" Width="75" MarginSpec="2 3">
                                                    <Listeners>
                                                        <Click Handler="this.up('window').hide();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </BottomBar>
                                </ext:FormPanel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" ID="PanelAssignInterview" Title="Interview" Layout="FitLayout">
                            <TabConfig runat="server" UI="Warning" />
                            <Items>
                                <ext:FormPanel Header="false" ID="FormPanelAssignInterview" runat="server" BodyPadding="5">
                                    <Items>
                                        <ext:FieldContainer ID="FldContainerCmbTipeBatchInterview" runat="server" LabelWidth="100" FieldLabel="Batch" Layout="HBoxLayout">
                                            <Items>
                                                <ext:SelectBox ID="CmbTipeBatchInterview" runat="server" EmptyText="Pilih Tipe Batch" Width="180px">
                                                    <Items>
                                                        <ext:ListItem Text="Batch" Value="1" />
                                                        <ext:ListItem Text="NonBatch" Value="0" />
                                                    </Items>
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" Hidden="true" Weight="-1" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.getTrigger(0).show();" />
                                                        <BeforeQuery Handler="this.getTrigger(0)[this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.getTrigger(0).hide();}" />
                                                    </Listeners>
                                                    <DirectEvents>
                                                        <Change OnEvent="CmbTipeBatchInterview_Change" />
                                                    </DirectEvents>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer ID="FldContainerBatchInterview" runat="server" LabelWidth="100" FieldLabel="Nama Batch" Layout="HBoxLayout" Hidden="true">
                                            <Items>
                                                <ext:TextArea ID="txtBatchInterview" runat="server" Width="180px" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Tanggal" Layout="HBoxLayout">
                                            <Items>
                                                <ext:DateField ID="DF_WaktuAssignInterview" runat="server" Format="dddd, dd MMM yyyy" Width="180px" EmptyText="Tanggal Interview" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Waktu" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextField ID="TF_WaktuAssignInterview" runat="server" Width="40px">
                                                    <Plugins>
                                                        <ext:InputMask runat="server" Mask="ab:cd" AlwaysShow="true">
                                                            <MaskSymbols>
                                                                <ext:MaskSymbol Name="a" Regex="[012]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="b" Regex="[0-9]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="c" Regex="[0-5]" Placeholder="_" />
                                                                <ext:MaskSymbol Name="d" Regex="[0-9]" Placeholder="_" />
                                                            </MaskSymbols>
                                                        </ext:InputMask>
                                                    </Plugins>
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" LabelWidth="100" FieldLabel="Lokasi" Layout="HBoxLayout">
                                            <Items>
                                                <ext:TextArea ID="txtLokasiInterview" runat="server" Width="180px" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                    <BottomBar>
                                        <ext:Toolbar runat="server" ID="ToolbarUndangInterview" Frame="false" ClientIDMode="Static">
                                            <LayoutConfig>
                                                <ext:HBoxLayoutConfig Pack="End" />
                                            </LayoutConfig>
                                            <Items>
                                                <ext:Button ID="btnAssignInterview_GC" Height="30" runat="server" UI="Info" StandOut="true" Text="Atur Sekarang" Icon="Clock" MarginSpec="2 3" Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAssignInterview_GC_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="btnAssignInterview_Save" Height="30" runat="server" UI="Success" StandOut="true" Text="Save & Kirim" Icon="Disk" Width="100" MarginSpec="2 3" AutoLoadingState="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnAssignInterview_Save_Click">
                                                            <Confirmation ConfirmRequest="true" Title="Confirmation" Message="Anda yakin dengan data Interview ini?" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="btnAssignInterview_Close" Height="30" runat="server" UI="Danger" StandOut="true" Text="Close" Icon="Cancel" Width="75" MarginSpec="2 3">
                                                    <Listeners>
                                                        <Click Handler="this.up('window').hide();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </BottomBar>
                                </ext:FormPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="winGetLink" runat="server" Title="CopyLink" Icon="Clipboard" Hidden="true" Layout="FitLayout" Width="730">
            <Items>
                <ext:FormPanel Header="false" runat="server" Icon="FolderAdd"
                    Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                    <Items>
                        <ext:TextArea ID="areaGetLink" runat="server" ReadOnly="true" FieldLabel="Detail Login" LabelWidth="70" Width="500" />
                        <ext:TextField ID="lblGetLink" runat="server" ReadOnly="true" FieldLabel="Link Direct" LabelWidth="70" Width="700" />
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="winCekNilai" runat="server" Title="CekNilai" Icon="ReportMagnify" Width="900" Hidden="true" Modal="true" Layout="FitLayout" Maximizable="true" Maximized="true">
            <Items>
                <ext:FormPanel Header="false" runat="server" Icon="FolderAdd" Height="600"
                    Margins="0 5 5 5" BodyPadding="5" AnchorHorizontal="100%" AutoScroll="True" MonitorResize="true">
                    <Items>
                        <ext:Label runat="server" ID="LblRekomendasi" Height="60" AnchorHorizontal="100%" />
                        <ext:Container runat="server" Layout="HBoxLayout">
                            <Items>
                                <ext:Container runat="server" ID="ContainerDataDiriPeserta" Width="320">
                                    <LayoutConfig>
                                        <ext:VBoxLayoutConfig Align="Center" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:FieldContainer runat="server" ID="FC_NoPeserta" LabelWidth="100" FieldLabel="Nomor Peserta">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtNoPeserta" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_NamaPeserta" LabelWidth="100" FieldLabel="Nama Peserta">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtNamaPeserta" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_NoKTP" LabelWidth="100" FieldLabel="Nomor KTP">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtNoKTP" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_TglLahir" LabelWidth="100" FieldLabel="Tanggal Lahir">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtTglLahir" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_Alamat" LabelWidth="100" FieldLabel="Alamat">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtAlamat" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_NoHP" LabelWidth="100" FieldLabel="Nomor Telepon">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtNoHP" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_Email" LabelWidth="100" FieldLabel="Email">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtEmail" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FC_LamarSebagai" LabelWidth="100" FieldLabel="Lamar Sebagai">
                                            <Items>
                                                <ext:TextField runat="server" ID="txtLamarSebagai" Width="200" ReadOnly="true" ReadOnlyCls="ReadOnly" FieldCls="ReadOnly" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:Container>
                                <ext:PolarChart runat="server" ID="ChartNilaiHasilUjian" Height="425" Flex="1" InsetPaddingSpec="40 40 60 40">
                                    <LegendConfig runat="server" Dock="Right" />
                                    <Store>
                                        <ext:Store ID="StoreNilaiUjian" runat="server" RemoteSort="true" PageSize="10">
                                            <Proxy>
                                                <ext:AjaxProxy Url="GridHandlers/MASTER_NilaiHasilUjianGridHandler.ashx">
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Model>
                                                <ext:Model ID="Model2" runat="server" IDProperty="SeqNo">
                                                    <Fields>
                                                        <ext:ModelField Name="NilaiStandard" />
                                                        <ext:ModelField Name="NilaiGroupResult" />
                                                        <ext:ModelField Name="NamaGroup" />
                                                        <ext:ModelField Name="NamaNormaDtl" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                            <Parameters>
                                                <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterNilai}.getValue()" />
                                            </Parameters>
                                            <Sorters>
                                                <ext:DataSorter Property="NamaGroup" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                    <Interactions>
                                        <ext:RotateInteraction />
                                    </Interactions>
                                    <Axes>
                                        <ext:NumericAxis Position="Radial" Grid="true" />
                                    </Axes>
                                    <Series>
                                        <ext:RadarSeries Title="Nilai" AngleField="NamaGroup" RadiusField="NilaiGroupResult">
                                            <StyleSpec>
                                                <ext:Sprite LineWidth="2" FillOpacity="0.4" />
                                            </StyleSpec>
                                            <Marker>
                                                <ext:Sprite Radius="4" SpriteType="Triangle" />
                                            </Marker>
                                            <HighlightConfig>
                                                <ext:Sprite Radius="8" LineWidth="1" FillStyle="#000" StrokeStyle="#888" />
                                            </HighlightConfig>
                                            <Tooltip runat="server" TrackMouse="true">
                                                <Renderer Handler="toolTip.setHtml(record.get('NamaGroup') + ': ' + record.get('NilaiGroupResult'));" />
                                            </Tooltip>
                                        </ext:RadarSeries>
                                        <ext:RadarSeries Title="Norma" AngleField="NamaGroup" RadiusField="NilaiStandard">
                                            <StyleSpec>
                                                <ext:Sprite LineWidth="2" FillOpacity="0.4" />
                                            </StyleSpec>
                                            <Marker>
                                                <ext:Sprite Radius="4" SpriteType="Square" />
                                            </Marker>
                                            <HighlightConfig>
                                                <ext:Sprite Radius="8" LineWidth="1" FillStyle="#000" StrokeStyle="#888" />
                                            </HighlightConfig>
                                            <Tooltip runat="server" TrackMouse="true">
                                                <Renderer Handler="toolTip.setHtml(record.get('NamaGroup') + ': ' + record.get('NilaiStandard'));" />
                                            </Tooltip>
                                        </ext:RadarSeries>
                                    </Series>
                                </ext:PolarChart>
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" ID="ContainerBoxNilai" Scrollable="Both" PaddingSpec="10 0 0 0" />
                        <ext:Panel runat="server" ID="PnlFotoPeserta" Icon="Application" Title="Foto Peserta Psikotest" Height="350">
                            <Items>
                                <ext:Container runat="server" ID="extContainerFotoPeserta" ClientIDMode="Static" PaddingSpec="10 0 0 0">
                                    <Content>
                                        <div id="ContainerFotoPeserta"></div>
                                    </Content>
                                </ext:Container>
                            </Items>
                        </ext:Panel>
                        <ext:GridPanel ID="GridPanelNilaiHasilUjian" runat="server" Icon="Application" Title="Hasil Ujian"
                            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="300">
                            <Store>
                                <ext:Store ID="StoreNilaiHasilUjian" runat="server" RemoteSort="true" PageSize="10">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GridHandlers/MASTER_NilaiHasilUjianGridHandler.ashx">
                                            <ActionMethods Read="GET" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model5" runat="server" IDProperty="NoGroup">
                                            <Fields>
                                                <ext:ModelField Name="NoGroup" Type="String" />
                                                <ext:ModelField Name="UserId" Type="String" />
                                                <ext:ModelField Name="Tipe" Type="String" />
                                                <ext:ModelField Name="NamaGroup" Type="String" />
                                                <ext:ModelField Name="NilaiStandard" Type="String" />
                                                <ext:ModelField Name="NilaiGroupResult" Type="String" />
                                                <ext:ModelField Name="NamaNormaDtl" Type="String" />
                                                <ext:ModelField Name="IsPrioritas" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterNilai}.getValue()" />
                                    </Parameters>
                                    <Sorters>
                                        <ext:DataSorter Property="TimeInput" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn runat="server" Text="No" Filterable="false" Width="40" />
                                    <ext:Column ID="Column11" runat="server" Text="Nama Group Soal" DataIndex="NamaGroup" Flex="1">
                                        <Renderer Fn="renderIsPrioritas" />
                                    </ext:Column>
                                    <ext:Column ID="Column12" runat="server" Text="Nilai" DataIndex="NilaiGroupResult" Width="50">
                                        <Renderer Fn="renderNilaiGroupResult" />
                                    </ext:Column>
                                    <ext:Column ID="Column13" runat="server" Text="Norma" DataIndex="NilaiStandard" Width="50" />
                                    <ext:Column ID="Column17" runat="server" Text="Nama Norma" DataIndex="NamaNormaDtl" Width="100" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader2" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" Mode="Multi">
                                    <DirectEvents>
                                        <Select OnEvent="SelectedNilaiHasilUjian_Click" />
                                    </DirectEvents>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar2" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
                            </BottomBar>
                        </ext:GridPanel>
                        <ext:GridPanel ID="GridPanelNilaiHasilUjianDtl" runat="server" Icon="Application" Title="Hasil Ujian"
                            Margins="0 10 10 0" BodyPadding="0" Frame="true" AutoScroll="True" ColumnLines="true" Height="327">
                            <Store>
                                <ext:Store ID="StoreGridNilaiHasilUjianDtl" runat="server" RemoteSort="true" PageSize="10">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GridHandlers/MASTER_NilaiHasilUjianDtlGridHandler.ashx">
                                            <ActionMethods Read="GET" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model6" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="Judul" Type="String" />
                                                <ext:ModelField Name="Deskripsi" Type="String" />
                                                <ext:ModelField Name="Jawaban" Type="String" />
                                                <ext:ModelField Name="Poin" Type="String" />
                                                <ext:ModelField Name="JawabanBenar" Type="String" />
                                                <ext:ModelField Name="PoinBenar" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterNilaiDtl}.getValue()" />
                                    </Parameters>
                                    <Sorters>
                                        <ext:DataSorter Property="NoGroup, NoUrut" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel5" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn runat="server" Text="No" Filterable="false" Width="40" />
                                    <ext:Column ID="Column14" runat="server" Text="Judul Soal" DataIndex="Judul" Width="120" />
                                    <ext:Column ID="Column15" runat="server" Text="Deskripsi Soal" DataIndex="Deskripsi" Width="300" />
                                    <ext:Column ID="Column16" runat="server" Text="Jawaban Dipilih" DataIndex="Jawaban" Flex="1" />
                                    <ext:Column ID="Column18" runat="server" Text="Poin" DataIndex="Poin" Width="80" />
                                    <ext:Column ID="Column42" runat="server" Text="Jawaban Benar" DataIndex="JawabanBenar" Flex="1" />
                                    <ext:Column ID="Column45" runat="server" Text="Poin Benar" DataIndex="PoinBenar" Width="80" />
                                </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:FilterHeader ID="FilterHeader5" runat="server" Remote="true" />
                            </Plugins>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolbar5" runat="server" DisplayInfo="true" DisplayMsg="Displaying Data(s) {0} - {1} of {2}" EmptyMsg="No Data(s) to display" />
                            </BottomBar>
                        </ext:GridPanel>
                        <ext:CartesianChart runat="server" ID="ChartNilaiHasilUjianDtl" Height="300" InsetPaddingSpec="40 40 60 40" Hidden="true">
                            <LegendConfig runat="server" Dock="Right" />
                            <Store>
                                <ext:Store ID="StoreChartNilaiHasilUjianDtl" runat="server" RemoteSort="true" PageSize="10">
                                    <Proxy>
                                        <ext:AjaxProxy Url="GridHandlers/MASTER_NilaiHasilUjianDtlGridHandler.ashx">
                                            <ActionMethods Read="GET" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Model>
                                        <ext:Model ID="Model7" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="Judul" Type="String" />
                                                <ext:ModelField Name="Deskripsi" Type="String" />
                                                <ext:ModelField Name="Jawaban" Type="String" />
                                                <ext:ModelField Name="JawabanDiPilih" Type="String" />
                                                <ext:ModelField Name="JmlSalah" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Parameters>
                                        <ext:StoreParameter Name="Filter" Mode="Raw" Value="#{hdnFilterNilaiDtl}.getValue()" />
                                    </Parameters>
                                    <Sorters>
                                        <ext:DataSorter Property="NoUrut" Direction="ASC" />
                                    </Sorters>
                                </ext:Store>
                            </Store>
                            <Axes>
                                <ext:NumericAxis Position="Left" Grid="true" AdjustByMajorUnit="true" />
                                <ext:CategoryAxis Fields="Judul" Position="Bottom" Grid="true" />
                            </Axes>
                            <Series>
                                <ext:BarSeries XField="Judul" YField="JawabanDiPilih,JmlSalah" Titles="Benar,Salah" Stacked="true">
                                    <StyleSpec>
                                        <ext:Sprite Opacity="0.8" />
                                    </StyleSpec>
                                    <HighlightConfig>
                                        <ext:Sprite FillStyle="yellow" />
                                    </HighlightConfig>
                                    <Tooltip runat="server" TrackMouse="true">
                                        <Renderer Handler="var browser = context.series.getTitle()[Ext.Array.indexOf(context.series.getYField(), context.field)]; toolTip.setHtml(browser + ' sebanyak: ' + record.get(context.field));" />
                                    </Tooltip>
                                </ext:BarSeries>
                            </Series>
                        </ext:CartesianChart>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>

        <ext:Window ID="WinFilterPosisi" runat="server" Title="Pilih Posisi" Icon="NoteAdd" Width="320" Height="355" Hidden="true" Layout="Fit">
            <Items>
                <ext:GridPanel ID="GPCmbFilterPosisi" runat="server" Height="360"
                    ColumnLines="true" Width="280" Frame="false" Header="false">
                    <Store>
                        <ext:Store runat="server" RemoteSort="true" PageSize="1000">
                            <Model>
                                <ext:Model runat="server" IDProperty="NamaPosisi">
                                    <Fields>
                                        <ext:ModelField Name="NamaPosisi" Type="String" />
                                        <ext:ModelField Name="CodePosisi" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="NamaPosisi" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn3" Text="No" runat="server" Filterable="false" Width="40" Align="Center" />
                            <ext:Column runat="server" Text="Posisi " DataIndex="NamaPosisi" Width="230" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="false" IsDynamic="true" />
                    </Plugins>
                    <View>
                        <ext:GridView runat="server" LoadMask="true" />
                    </View>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:Toolbar ID="PagingToolbar11" runat="server">
                            <Items>
                                <ext:Button ID="WinFilterPosisi_Select" runat="server" Text="Select" StandOut="true" Icon="Accept">
                                    <DirectEvents>
                                        <Click OnEvent="WinFilterPosisi_Select_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="WinFilterPosisi_Clear" runat="server" Text="Clear" StandOut="true" Icon="Delete">
                                    <DirectEvents>
                                        <Click OnEvent="WinFilterPosisi_Clear_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="WinFilterCabang" runat="server" Title="Pilih Cabang" Icon="NoteAdd" Width="320" Height="355" Hidden="true" Layout="Fit">
            <Items>
                <ext:GridPanel ID="GPCmbFilterCabang" runat="server" Height="360"
                    ColumnLines="true" Width="280" Frame="false" Header="false">
                    <Store>
                        <ext:Store runat="server" RemoteSort="true" PageSize="1000">
                            <Model>
                                <ext:Model runat="server" IDProperty="AsalRekrutan">
                                    <Fields>
                                        <ext:ModelField Name="NoAsalRekrutan" Type="String" />
                                        <ext:ModelField Name="AsalRekrutan" Type="String" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="AsalRekrutan" Direction="ASC" />
                            </Sorters>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" Text="No" runat="server" Filterable="false" Width="40" Align="Center" />
                            <ext:Column runat="server" Text="Asal Rekrutan " DataIndex="AsalRekrutan" Width="230" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" Remote="false" IsDynamic="true" />
                    </Plugins>
                    <View>
                        <ext:GridView runat="server" LoadMask="true" />
                    </View>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                    </SelectionModel>
                    <BottomBar>
                        <ext:Toolbar ID="Toolbar3" runat="server">
                            <Items>
                                <ext:Button ID="WinFilterCabang_Select" runat="server" Text="Select" StandOut="true" Icon="Accept">
                                    <DirectEvents>
                                        <Click OnEvent="WinFilterCabang_Select_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="WinFilterCabang_Clear" runat="server" Text="Clear" StandOut="true" Icon="Delete">
                                    <DirectEvents>
                                        <Click OnEvent="WinFilterCabang_Clear_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Window>
    </form>


    <!--breadcrumb-->
    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
        <div class="breadcrumb-title pe-3">Applicant Data</div>
        <div class="ps-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 p-0">
                    <li class="breadcrumb-item"><a href="javascript:;"><i class="bx bx-home-alt"></i></a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">Applicant Data</li>
                </ol>
            </nav>
        </div>
    </div>
    <!--end breadcrumb-->

    <div class="card radius-10">
        <div class="card-header">
            <div class="row mb-3">
                <div class="col-12 col-lg-2">
                    <label for="filerDateAwal" class="form-label">
                        Start Date
									Filter</label>
                    <div class="input-group mb-2">
                        <input type="text" class="form-control" id="startDateFilter"
                            placeholder="dd/mm/yyyy">
                    </div>
                </div>
                <div class="col-12 col-lg-2">
                    <label for="filerDateAwal" class="form-label">
                        End Date
									Filter</label>
                    <div class="input-group mb-2">
                        <input type="text" class="form-control" id="endDateFilter"
                            placeholder="dd/mm/yyyy">
                    </div>
                </div>
                <div class="col-12 col-lg-2">
                    <label class="form-label">Psikotest Result</label>
                    <select class="single-select form-select">
                        <option value="all">All</option>
                        <option value="recommended">Recomended</option>
                        <option value="notRecommended">Not Recommended</option>
                        <option value="considered">Considered</option>
                        <option value="notYet">Not Yet</option>
                    </select>
                </div>
                <div class="col-12 col-lg-3">
                    <label class="form-label">Position</label>
                    <select class="multiple-select" data-placeholder="Choose anything" multiple="multiple">
                        <option value="all" selected>All</option>
                        <option value="it">IT</option>
                        <option value="hrd">HRD</option>
                        <option value="finance">Finance</option>
                        <option value="audit">Audit</option>
                        <option value="legal">Legal</option>
                    </select>
                </div>
                <div class="col-12 col-lg-3">
                    <label class="form-label">BKK/Branch</label>
                    <select class="multiple-select" data-placeholder="Choose anything" multiple="multiple">
                        <option value="all" selected>All</option>
                        <option value="audit">Audit</option>
                        <option value="legal">Legal</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="d-flex align-items-center">
                <div>
                    <h5 class="font-weight-bold mb-0">Participant</h5>
                </div>
                <div class="ms-auto mt-2">
                    <button type="button" class="btn btn-primary radius-8 d-flex align-items-center" data-bs-toggle="modal" data-bs-target="#modalInvitation">
                        <img src="../assets/newAssets/images/icons/ic_mail_open.svg" alt="mail_open" class="me-2">Invite</button>
                </div>
            </div>
            <div class="table-responsive">
                <div id="printbar" style="float: right"></div>
                <br>
                <table id="participant" class="table mb-0 align-middle" style="width: 100%">
                    <thead class="table-light">
                        <tr>
                            <th>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></th>
                            <th>No</th>
                            <th>ID Participant</th>
                            <th>Name</th>
                            <th>Date of Birth</th>
                            <th>Address</th>
                            <th>Phone Number</th>
                            <th>Email</th>
                            <th>Source Recruit</th>
                            <th>Registration</th>
                            <th>Position</th>
                            <th>No. KTP</th>
                            <th>Psikotest Result</th>
                            <th>Time</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>1</td>
                            <td>345243</td>
                            <td>Nanda Raditya</td>
                            <td>05 September 2000</td>
                            <td>Jalan Bumi nomor 77</td>
                            <td>082124776437</td>
                            <td>nandaraditya80@gmail.com</td>
                            <td>Reguler</td>
                            <td>1</td>
                            <td>UI/UX Designer</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Recommended
                                </div>
                            </td>
                            <td>1 Sep 2023 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>2</td>
                            <td>453676</td>
                            <td>Indra Jumawan</td>
                            <td>08 Agustus 2000</td>
                            <td>Jalan Hijau nomor 80</td>
                            <td>081234567890</td>
                            <td>indra@gmail.com</td>
                            <td>Reguler</td>
                            <td>2</td>
                            <td>Back-end Developer</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Recommended
                                </div>
                            </td>
                            <td>1 Feb 2024 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>3</td>
                            <td>876743</td>
                            <td>Andre Jhons</td>
                            <td>12 December 2000</td>
                            <td>Jalan Biru nomor 80</td>
                            <td>081234567890</td>
                            <td>andre@gmail.com</td>
                            <td>Reguler</td>
                            <td>1</td>
                            <td>Front-end Developer</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-success bg-light-success p-2 text-uppercase px-3">
                                    Recommended
                                </div>
                            </td>
                            <td>1 Feb 2024 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>4</td>
                            <td>876743</td>
                            <td>Rizky Rama</td>
                            <td>25 January 2000</td>
                            <td>Jalan Biru nomor 80</td>
                            <td>081234567890</td>
                            <td>rizky@gmail.com</td>
                            <td>Reguler</td>
                            <td>1</td>
                            <td>Dev Ops</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-danger bg-light-danger p-2 text-uppercase px-3">
                                    Not Recommended
                                </div>
                            </td>
                            <td>1 Feb 2024 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>5</td>
                            <td>876743</td>
                            <td>Natasha Bella</td>
                            <td>02 June 2000</td>
                            <td>Jalan Merah nomor 80</td>
                            <td>081234567890</td>
                            <td>natasha@gmail.com</td>
                            <td>Reguler</td>
                            <td>1</td>
                            <td>Dev Ops</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-warning bg-light-warning p-2 text-uppercase px-3">
                                    Considered
                                </div>
                            </td>
                            <td>1 Feb 2024 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                        <tr>
                            <td>
                                <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked"></td>
                            <td>6</td>
                            <td>876743</td>
                            <td>Bella Swan</td>
                            <td>10 July 2000</td>
                            <td>Jalan Angsa nomor 80</td>
                            <td>081234567890</td>
                            <td>bella@gmail.com</td>
                            <td>Reguler</td>
                            <td>1</td>
                            <td>Dev Ops</td>
                            <td>5678938678998473</td>
                            <td>
                                <div class="badge rounded-pill text-secondary bg-light-secondary p-2 text-uppercase px-3">
                                    Not Yet
                                </div>
                            </td>
                            <td>1 Feb 2024 15:02</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm radius-30 px-4" data-bs-toggle="modal" data-bs-target="#modalInvitation">Invite</button></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
