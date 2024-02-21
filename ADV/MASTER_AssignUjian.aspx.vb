
Imports System.Data
Imports System.IO
Imports Ext.Net
Imports Newtonsoft.Json
Imports OfficeOpenXml

Partial Class MASTER_AssignUjian
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        CheckUserPermission()
        If Not Page.IsPostBack Then
			If Session("UserID").Equals("9220312051") Then
                btnAssignUjian_GC.Show()
                'btnGetLink.Show()
            End If

            Dim tglFromstr = New DateTime(Date.Now.Year, Date.Now.Month, 1).ToString("MM/dd/yyyy")
            Dim tglTostr = New DateTime(Date.Now.Year, Date.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("MM/dd/yyyy")

            'Filter Peserta
            DF_FilterTimeInput_From.Text = tglFromstr
            DF_FilterTimeInput_To.Text = tglTostr

            DF_FilterTglLahir_From.Text = New DateTime(2000, 1, 1).ToString("MM/dd/yyyy")
            DF_FilterTglLahir_To.Text = Date.Now.ToString("MM/dd/yyyy")

            'Filter Peserta Dtl
            DF_FilterTglInput_From.Text = tglFromstr
            DF_FilterTglInput_To.Text = tglTostr

            DF_FilterTglUjian_From.Text = tglFromstr
            DF_FilterTglUjian_To.Text = tglTostr

            DF_WaktuAssignUjian.MinDate = Date.Now

            hdnFilterPosisi.Text = ""
            GPCmbFilterPosisi.GetStore().DataSource = clsAssignTest.getPosisi()

            hdnFilterCabang.Text = ""
            GPCmbFilterCabang.GetStore().DataSource = clsAssignTest.getBKKBranch()

            hdnFilterPaketSoalAssignUjian.Value = "0=1"

            CmbFilterhasilPsikotestPeserta.SelectedItems.Add(New ListItem("BELUM UJIAN", "0"))

            nfGPPesertaTotalRow.Value = 10

            btnFilterPeserta_Click()
        End If
    End Sub

    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnSelectFilterPosisi_Click()
        WinFilterPosisi.Show()
        WinFilterPosisi.Center()
    End Sub

    Protected Sub btnSelectFilterCabang_Click()
        WinFilterCabang.Show()
        WinFilterCabang.Center()
    End Sub

#Region "Window Filter Posisi"
    Public Sub WinFilterPosisi_Select_Click(sender As Object, e As DirectEventArgs)
        Dim rsm As RowSelectionModel = GPCmbFilterPosisi.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            Try
                Dim filterText = ""
                Dim filterValue = ""

                For Each r As SelectedRow In rsm.SelectedRows
                    filterText += r.RecordID.ToString + ", "
                    filterValue += "'" + r.RecordID.ToString + "', "
                Next

                cmbFilterPosisi.Text = filterText.Substring(0, filterText.Length - 2)
                hdnFilterPosisi.Text = filterValue.Substring(0, filterValue.Length - 2)

                WinFilterPosisi.Hide()
                GPCmbFilterPosisi.GetSelectionModel().DeselectAll()
                GPCmbFilterPosisi.Call("clearMemory")
            Catch ex As Exception
                Helper.handleEx(ex)
                Return
            End Try
        Else
            Helper.ShowMsg("Failed", "Pilih data terlebih dahulu")
        End If
    End Sub

    Public Sub WinFilterPosisi_Clear_Click(sender As Object, e As DirectEventArgs)
        WinFilterPosisi.Close()
        GPCmbFilterPosisi.GetSelectionModel().DeselectAll()
        GPCmbFilterPosisi.Call("clearMemory")
        cmbFilterPosisi.Text = ""
        hdnFilterPosisi.Text = ""
    End Sub
#End Region

#Region "Window Filter Cabang"
    Public Sub WinFilterCabang_Select_Click(sender As Object, e As DirectEventArgs)
        Dim rsm As RowSelectionModel = GPCmbFilterCabang.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            Try
                Dim filterText = ""
                Dim filterValue = ""

                For Each r As SelectedRow In rsm.SelectedRows
                    filterText += r.RecordID.ToString + ", "
                    filterValue += "'" + r.RecordID.ToString + "', "
                Next

                cmbFilterCabang.Text = filterText.Substring(0, filterText.Length - 2)
                hdnFilterCabang.Text = filterValue.Substring(0, filterValue.Length - 2)

                WinFilterCabang.Hide()
                GPCmbFilterCabang.GetSelectionModel().DeselectAll()
                GPCmbFilterCabang.Call("clearMemory")
            Catch ex As Exception
                Helper.handleEx(ex)
                Return
            End Try
        Else
            Helper.ShowMsg("Failed", "Pilih data terlebih dahulu")
        End If
    End Sub

    Public Sub WinFilterCabang_Clear_Click(sender As Object, e As DirectEventArgs)
        WinFilterCabang.Close()
        GPCmbFilterCabang.GetSelectionModel().DeselectAll()
        GPCmbFilterCabang.Call("clearMemory")
        cmbFilterCabang.Text = ""
        hdnFilterCabang.Text = ""
    End Sub
#End Region

#Region "MainUI"

#Region "GridPanel Peserta"
    Protected Sub btnFilterPeserta_Click()
        hdnFilterPeserta.Value = " 1=1 "
        If chbFilterTimeInput.Checked Then
            hdnFilterPeserta.Value += "AND '" & DF_FilterTimeInput_From.Text & "' <= CAST(TimeInput AS DATE) AND CAST(TimeInput AS DATE) <= '" & DF_FilterTimeInput_To.Text & "' "
        End If

        If hdnFilterPosisi.Text.Length > 0 Then
            hdnFilterPeserta.Value += "AND LamarSebagai IN(" + hdnFilterPosisi.Text + ") "
        End If

        If hdnFilterCabang.Text.Length > 0 Then
            hdnFilterPeserta.Value += "AND AsalRekrutan IN(" + hdnFilterCabang.Text + ") "
        End If

        If chbFilterTglLahir.Checked Then
            hdnFilterPeserta.Value += "AND '" & DF_FilterTglLahir_From.Text & "' <= TglLahir AND TglLahir <= '" & DF_FilterTglLahir_To.Text & "' "
        End If

        'Filter Status Peserta
        If CmbFilterhasilPsikotestPeserta.SelectedItem.Value = "3" Then
            hdnFilterPeserta.Value += "AND LblRek = 'DISARANKAN' "
        ElseIf CmbFilterhasilPsikotestPeserta.SelectedItem.Value = "2" Then
            hdnFilterPeserta.Value += "AND LblRek = 'DIPERTIMBANGKAN' "
        ElseIf CmbFilterhasilPsikotestPeserta.SelectedItem.Value = "1" Then
            hdnFilterPeserta.Value += "AND LblRek = 'TIDAK DISARANKAN' "
        ElseIf CmbFilterhasilPsikotestPeserta.SelectedItem.Value = "0" Then
            hdnFilterPeserta.Value += "AND LblRek = 'BELUM UJIAN' "
        End If

        GridPanelPeserta.GetStore.Reload()
    End Sub
    Protected Sub btnAssignBarisUjian_Click()
        Dim rsm As RowSelectionModel = GridPanelPeserta.GetSelectionModel
        If rsm.SelectedRows.Count > 0 Then
            hdnSoloAssign.Value = False
            hdnMultiAssign.Value = True

            TabPanelAssignUjian.HideTab(PanelAssignInterview) 'Hide interview dulu biar urutannya ga kacau
            TabPanelAssignUjian.ShowTab(PanelAssignPsikotest)
            TabPanelAssignUjian.ShowTab(PanelAssignInterview)
            TabPanelAssignUjian.ActiveTab = PanelAssignPsikotest

            FormPanelAssignUjian.Reset()
            FormPanelAssignInterview.Reset()

            CmbTipeBatchPsikotest.ReadOnly = False
            txtBatchPsikotest.ReadOnly = False
            txtBatchPsikotest.FieldStyle = ""
            FldContainerChbBlock.Hide()
            chbBlock.ReadOnly = True
            CmbPaketSoalAssignUjian.ReadOnly = False
            btnAssignUjian_Save.Width = 100
            btnAssignUjian_Save.Text = "Save & Kirim"

            CmbTipeBatchInterview.ReadOnly = False
            txtBatchInterview.ReadOnly = False
            txtBatchInterview.FieldStyle = ""
            btnAssignInterview_Save.Width = 100
            btnAssignInterview_Save.Text = "Save & Kirim"

            Dim KumpulanNoPeserta As String = ""
            For i As Integer = 0 To rsm.SelectedRows.Count - 1
                KumpulanNoPeserta += rsm.SelectedRows(i).RecordID + ";"
            Next
            KumpulanNoPeserta = KumpulanNoPeserta.Substring(0, KumpulanNoPeserta.Length - 1)
            hdnFilterPaketSoalAssignUjianNoPeserta.Value = KumpulanNoPeserta

            hdnFilterPaketSoalAssignUjian.Value = "bAktif = 'Aktif' "
            Helper.AddSkrip("reloadKaloAda('" + GridPanelPaketSoalAssignUjian.GetStore().BaseClientID + "');")

            winUndangPeserta.Title = "Undang Peserta"
            winUndangPeserta.Icon = Icon.NoteAdd
            winUndangPeserta.Show()
        Else
            Ext.Net.X.Msg.Alert("Alert", "Pilih Peserta dahulu.").Show()
        End If
    End Sub
    Protected Sub btnAssignPeserta_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim NoPeserta = e.ExtraParams("NoPeserta")
        hdnSoloAssign.Value = True
        hdnMultiAssign.Value = False
        hdnSoloAssignNoPeserta.Value = NoPeserta

        TabPanelAssignUjian.HideTab(PanelAssignInterview) 'Hide interview dulu biar urutannya ga kacau
        TabPanelAssignUjian.ShowTab(PanelAssignPsikotest)
        TabPanelAssignUjian.ShowTab(PanelAssignInterview)
        TabPanelAssignUjian.ActiveTab = PanelAssignPsikotest

        FormPanelAssignUjian.Reset()
        FormPanelAssignInterview.Reset()

        CmbTipeBatchPsikotest.ReadOnly = False
        txtBatchPsikotest.ReadOnly = False
        txtBatchPsikotest.FieldStyle = ""
        FldContainerChbBlock.Hide()
        chbBlock.ReadOnly = True
        CmbPaketSoalAssignUjian.ReadOnly = False
        btnAssignUjian_Save.Width = 100
        btnAssignUjian_Save.Text = "Save & Kirim"

        CmbTipeBatchInterview.ReadOnly = False
        txtBatchInterview.ReadOnly = False
        txtBatchInterview.FieldStyle = ""
        btnAssignInterview_Save.Width = 100
        btnAssignInterview_Save.Text = "Save & Kirim"

        Dim KumpulanNoPeserta As String = NoPeserta
        hdnFilterPaketSoalAssignUjianNoPeserta.Value = KumpulanNoPeserta

        hdnFilterPaketSoalAssignUjian.Value = "bAktif = 'Aktif' "
        Helper.AddSkrip("reloadKaloAda('" + GridPanelPaketSoalAssignUjian.GetStore().BaseClientID + "');")

        winUndangPeserta.Title = "Undang Peserta"
        winUndangPeserta.Icon = Icon.NoteAdd
        winUndangPeserta.Show()
    End Sub
    Protected Sub SelectedPeserta_Click()
        Dim rsm As RowSelectionModel = GridPanelPeserta.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    btnFilterPeserta2_Click()
                    hdnFilterPesertaDtl.Value += " AND NoPeserta = '" & r.RecordID.ToString & "'"
                    hdnFilterPesertaInterview.Value += " AND NoPeserta = '" & r.RecordID.ToString & "'"

                    GridPanelPesertaDtl.GetStore().Reload()
                    GridPanelPesertaInterview.GetStore().Reload()
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
    Protected Sub GridPanelPesertaPageChanged() 'Sengaja coba aja
        'GridPanelPeserta.GetSelectionModel.ClearSelection()
        'GridPanelPeserta.Call("clearMemory")
    End Sub
#End Region

#Region "GridPanel Peserta2"

    Protected Sub btnFilterPeserta2_Click()
        hdnFilterPesertaDtl.Value = "1=1"
        hdnFilterPesertaInterview.Value = "1=1"

        'Filter Time Input
        If chbFilterTglInput.Checked Then
            hdnFilterPesertaDtl.Value = " AND '" & DF_FilterTglInput_From.Text & "' <= CAST(TimeInput AS DATE) AND CAST(TimeInput AS DATE) <= '" & DF_FilterTglInput_To.Text & "'"
        End If

        'Copy Filter
        hdnFilterPesertaInterview.Value = hdnFilterPesertaDtl.Value

        'Filter Tanggal Ujian
        If chbFilterTglUjian.Checked Then
            hdnFilterPesertaDtl.Value += " AND '" & DF_FilterTglUjian_From.Text & "' <= CAST(WaktuTest AS DATE) AND CAST(WaktuTest AS DATE) <= '" & DF_FilterTglUjian_To.Text & "'"
        End If

        'Filter Status Pengerjaan PesertaDtl
        If CmbFilterStatusPesertaDtl.SelectedItem.Value = "0" Then
            hdnFilterPesertaDtl.Value += " AND StatusPengerjaan = 'Belum Ujian'"
        ElseIf CmbFilterStatusPesertaDtl.SelectedItem.Value = "1" Then
            hdnFilterPesertaDtl.Value += " AND StatusPengerjaan = 'Sedang Ujian'"
        ElseIf CmbFilterStatusPesertaDtl.SelectedItem.Value = "2" Then
            hdnFilterPesertaDtl.Value += " AND StatusPengerjaan = 'Selesai Ujian'"
        End If

        'Filter Status PesertaInterview
        If CmbFilterStatusPesertaInterview.SelectedItem.Value = "0" Then
            hdnFilterPesertaInterview.Value += " AND StatusInterview = 'Belum Interview'"
        ElseIf CmbFilterStatusPesertaInterview.SelectedItem.Value = "1" Then
            hdnFilterPesertaInterview.Value += " AND StatusInterview = 'Sedang Interview'"
        ElseIf CmbFilterStatusPesertaInterview.SelectedItem.Value = "2" Then
            hdnFilterPesertaInterview.Value += " AND StatusInterview = 'Selesai Interview'"
        End If

        'Filter Status Ujian
        If CmbFilterhasilPsikotestUjian.SelectedItem.Value = "0" Then
            hdnFilterPesertaDtl.Value += " AND LblRek = 'DISARANKAN'"
        ElseIf CmbFilterhasilPsikotestUjian.SelectedItem.Value = "1" Then
            hdnFilterPesertaDtl.Value += " AND LblRek = 'DIPERTIMBANGKAN'"
        ElseIf CmbFilterhasilPsikotestUjian.SelectedItem.Value = "2" Then
            hdnFilterPesertaDtl.Value += " AND LblRek = 'TIDAK DISARANKAN'"
        End If

        GridPanelPesertaDtl.GetStore.Reload()
        GridPanelPesertaInterview.GetStore.Reload()
    End Sub
    Protected Sub btnExportPsikotest_Click()
        Dim filterExportPsikotest = "ISNULL(NoGroup, '') <> '' "
        Dim dt As DataTable

        Try
            If Not (chbFilterTglInput.Checked Or chbFilterTglUjian.Checked) Then
                Throw New Exception("Pilih minimal salah satu tanggal sebagai filter!!!")
            End If

            'Filter Time Input
            'If chbFilterTglInput.Checked AndAlso HasDiffMoreThanOneMonth(CDate(DF_FilterTglInput_From.Text), CDate(DF_FilterTglInput_To.Text)) Then
            '    Throw New Exception("Untuk export maksimal interval Tanggal Input 1 bulan")
            'End If
            If chbFilterTglInput.Checked Then
                filterExportPsikotest += "AND '" & CDate(DF_FilterTglInput_From.Text).ToString("yyyy-MM-dd") & "' <= CAST(A.TimeInput AS DATE) AND CAST(A.TimeInput AS DATE) <= '" & CDate(DF_FilterTglInput_To.Text).ToString("yyyy-MM-dd") & "' "
            End If

            'Filter Tanggal Ujian
            'If chbFilterTglUjian.Checked AndAlso HasDiffMoreThanOneMonth(CDate(DF_FilterTglUjian_From.Text), CDate(DF_FilterTglUjian_To.Text)) Then
            '    Throw New Exception("Untuk export maksimal interval Tanggal Ujian 1 bulan")
            'End If
            If chbFilterTglUjian.Checked Then
                filterExportPsikotest += "AND '" & CDate(DF_FilterTglUjian_From.Text).ToString("yyyy-MM-dd") & "' <= CAST(A.WaktuTest AS DATE) AND CAST(A.WaktuTest AS DATE) <= '" & CDate(DF_FilterTglUjian_To.Text).ToString("yyyy-MM-dd") & "' "
            End If

            'Filter Status PesertaDtl
            If CmbFilterStatusPesertaDtl.SelectedItem.Value = "0" Then
                filterExportPsikotest += "AND StatusPengerjaan = 'Belum Ujian' "
            ElseIf CmbFilterStatusPesertaDtl.SelectedItem.Value = "1" Then
                filterExportPsikotest += "AND StatusPengerjaan = 'Sedang Ujian' "
            ElseIf CmbFilterStatusPesertaDtl.SelectedItem.Value = "2" Then
                filterExportPsikotest += "AND StatusPengerjaan = 'Selesai Ujian' "
            End If

            dt = clsAssignTest.ExportPsikotest(filterExportPsikotest)
            If dt.Rows.Count < 1 Then
                Throw New Exception("Tidak ada data!!!")
                Return
            End If
        Catch ex As Exception
            Helper.handleEx(ex)
            Return
        End Try

        GenerateExportPsikotest(dt)
    End Sub
    Protected Sub btnExportInterview_Click()
        Dim filterExportInterview = "1=1 "
        Dim dt As DataTable

        Try
            If Not (chbFilterTglInput.Checked Or chbFilterTglUjian.Checked) Then
                Throw New Exception("Pilih minimal salah satu tanggal sebagai filter!!!")
            End If

            'Filter Time Input
            'If chbFilterTglInput.Checked AndAlso HasDiffMoreThanOneMonth(CDate(DF_FilterTglInput_From.Text), CDate(DF_FilterTglInput_To.Text)) Then
            '    Throw New Exception("Untuk export maksimal interval Tanggal Input 1 bulan")
            'End If
            If chbFilterTglInput.Checked Then
                filterExportInterview += "AND '" & CDate(DF_FilterTglInput_From.Text).ToString("yyyy-MM-dd") & "' <= CAST(A.TimeInput AS DATE) AND CAST(A.TimeInput AS DATE) <= '" & CDate(DF_FilterTglInput_To.Text).ToString("yyyy-MM-dd") & "' "
            End If

            'Filter Tanggal Ujian
            'If chbFilterTglUjian.Checked AndAlso HasDiffMoreThanOneMonth(CDate(DF_FilterTglUjian_From.Text), CDate(DF_FilterTglUjian_To.Text)) Then
            '    Throw New Exception("Untuk export maksimal interval Tanggal Ujian 1 bulan")
            'End If
            If chbFilterTglUjian.Checked Then
                filterExportInterview += "AND '" & CDate(DF_FilterTglUjian_From.Text).ToString("yyyy-MM-dd") & "' <= CAST(WaktuInterview AS DATE) AND CAST(WaktuInterview AS DATE) <= '" & CDate(DF_FilterTglUjian_To.Text).ToString("yyyy-MM-dd") & "' "
            End If

            'Filter Status PesertaDtl
            If CmbFilterStatusPesertaInterview.SelectedItem.Value = "0" Then
                filterExportInterview += "AND StatusInterview = 'Belum Interview'"
            ElseIf CmbFilterStatusPesertaInterview.SelectedItem.Value = "1" Then
                filterExportInterview += "AND StatusInterview = 'Sedang Interview'"
            ElseIf CmbFilterStatusPesertaInterview.SelectedItem.Value = "2" Then
                filterExportInterview += "AND StatusInterview = 'Selesai Interview'"
            End If

            dt = clsAssignTest.ExportInterview(filterExportInterview)
            If dt.Rows.Count < 1 Then
                Throw New Exception("Tidak ada data!!!")
                Return
            End If
        Catch ex As Exception
            Helper.handleEx(ex)
            Return
        End Try

        GenerateExportInterview(dt)
    End Sub

#Region "Tab Psikotest"
    Protected Sub btnEditPesertaUjian_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        hdnSoloAssign.Value = False
        hdnMultiAssign.Value = False
        hdnUserID.Value = e.ExtraParams("UserId")
        Dim dt = clsAssignTest.getUjian(hdnUserID.Value)

        TabPanelAssignUjian.ShowTab(PanelAssignPsikotest)
        TabPanelAssignUjian.HideTab(PanelAssignInterview)

        FormPanelAssignUjian.Reset()
        winUndangPeserta.Title = "Edit Ujian"
        winUndangPeserta.Icon = Icon.NoteEdit

        CmbTipeBatchPsikotest.ReadOnly = True
        txtBatchPsikotest.ReadOnly = True
        'txtBatchPsikotest.FieldStyle = READONLYCLS
        If dt.Rows(0)("Batch").ToString = "" Then
            CmbTipeBatchPsikotest.Select(1)
        Else
            CmbTipeBatchPsikotest.Select(0)
            txtBatchPsikotest.Text = dt.Rows(0)("Batch")
        End If
        Dim WaktuTest = CDate(dt.Rows(0)("WaktuTest"))
        DF_WaktuAssignUjian.SetValue(WaktuTest)
        TF_WaktuAssignUjian.Text = WaktuTest.ToString("HH:mm")

        FldContainerChbBlock.Show()
        chbBlock.ReadOnly = False
        chbBlock.Checked = IIf(dt.Rows(0)("block").ToString.Equals("True"), True, False)

        CmbPaketSoalAssignUjian.ReadOnly = True
        CmbPaketSoalAssignUjian.SetValue(dt.Rows(0)("NoPaket"), dt.Rows(0)("NamaPaket"), True)

        btnAssignUjian_Save.Width = 75
        btnAssignUjian_Save.Text = "Save"
        winUndangPeserta.Show()
    End Sub
    Protected Sub btnResendUndanganPsikotest_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Try
            clsAssignTest.resendUjian(e.ExtraParams("UserId"), Session("UserID"))
            clsAssignTest.sendWAInvitaion(e.ExtraParams("UserId"), "P")
            Ext.Net.X.Msg.Alert("Success", "Pesan telah terkirim!").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert(ex.Message, ex.StackTrace).Show()
        End Try
    End Sub
    Protected Sub btnCekPesertaUjian_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim UserId = e.ExtraParams("UserId")
        hdnUserID.Value = UserId
        hdnFilterNilai.Value = "UserId = '" + UserId + "'"
        StoreNilaiUjian.Reload()

        Dim dt = clsAssignTest.getPeserta(UserId)
        FormPanelAssignUjian.Reset()
        GridPanelNilaiHasilUjian.GetSelectionModel.ClearSelection()

        hdnFilterNilaiDtl.Text = ""
        GridPanelNilaiHasilUjian.Show()
        GridPanelNilaiHasilUjian.GetStore.Reload()
        GridPanelNilaiHasilUjianDtl.Show()
        GridPanelNilaiHasilUjianDtl.GetStore.Reload()
        ChartNilaiHasilUjianDtl.Hide()
        ChartNilaiHasilUjianDtl.GetStore.Reload()

        txtNoPeserta.Text = dt.Rows(0)("NoPeserta").ToString
        txtNamaPeserta.Text = dt.Rows(0)("NamaPeserta").ToString
        txtNoKTP.Text = dt.Rows(0)("NoKTP").ToString
        txtTglLahir.Text = If(IsDBNull(dt.Rows(0)("TglLahir")), "", getTglIndo(dt.Rows(0)("TglLahir")))
        txtAlamat.Text = dt.Rows(0)("Alamat").ToString
        txtNoHP.Text = dt.Rows(0)("NoHP").ToString
        txtEmail.Text = dt.Rows(0)("Email").ToString
        txtLamarSebagai.Text = dt.Rows(0)("LamarSebagaiJob").ToString
        LblRekomendasi.Html = clsAssignTest.getLblRekomendasi(UserId)

        setupBoxNilaiGroup(UserId)
        setupBoxFotoPeserta(UserId)

        winCekNilai.Show()
    End Sub
    Protected Sub setupBoxNilaiGroup(UserId As String)
        Dim dt As DataTable
        Using iSqlHelper As New clsSQLHelper
            iSqlHelper.CommandType = CommandType.Text
            iSqlHelper.CommandText = "SELECT * " +
                                     "FROM ADVPSIKOTEST.dbo.VW_MASTER_TR_PsikotestResult " +
                                     "WHERE " + hdnFilterNilai.Value
            dt = iSqlHelper.ExecuteDataTable
        End Using
        Dim s = "<div class=""card-deck"">"
        For Each dr As DataRow In dt.Rows
            Dim style As String
            If dr("NilaiGroupResult") > dr("NilaiStandard") Then
                style = " card-success "
            ElseIf Not IsDBNull(dr("InStandardNorma")) And Not dr("IsPrioritas") Then
                style = " card-warning "
            Else
                style = " card-danger "
            End If
            s += "<div class=""card text-center" + style + """ style=""width: 12rem;"">" +
                  "<div class=""card-header"" style=""height: 113px;""><h3 Class=""card-title"">" + dr("NamaGroup").ToString + "</h3></div>" +
                  "<div Class=""card-body"">" +
                   "<h3 Class=""card-title"">" + dr("NamaNormaDtl").ToString + "</h3>" +
                   "<h3 Class=""card-text"">" + dr("NilaiGroupResult").ToString + "/" + dr("NilaiStandard").ToString + "</h3>" +
                  "</div>" +
                 "</div>"
        Next
        s += "</div>"
        ContainerBoxNilai.Html = s
    End Sub
    Protected Sub setupBoxFotoPeserta(UserId As String)
        Dim fotoPath As String = Path.Combine(ConfigurationManager.AppSettings("SnapshotPath").ToString, UserId)

        Dim s = "["
        If Directory.Exists(fotoPath) Then

            Dim filePaths = From f In Directory.EnumerateFiles(fotoPath)
                            Let fileCreationTime = File.GetCreationTime(f)
                            Order By fileCreationTime
                            Select Path.GetFileName(f)
            For Each filePath In filePaths.ToList
                s += "{"
                s += """Loc"": """ + UserId + "/" + Path.GetFileName(filePath) + ""","
                s += """ts"": """ + Path.GetFileName(filePath).Replace(".jpg", "").Replace("-", " ").Replace("_", ":") + """"
                s += "},"
            Next
            s = s.Substring(0, s.Length - 1)
        End If
        s += "]"

        Ext.Net.X.AddScript("setupBoxFotoPeserta('" + s + "');")
    End Sub
    Protected Sub btnGetLink_Click()
        Dim rsm As RowSelectionModel = GridPanelPesertaDtl.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Dim UserId = rsm.SelectedRecordID
            Dim dt = clsAssignTest.getLink(UserId)

            Dim E64 = New Encryption64()

            areaGetLink.Text = "Username: " + dt.Rows(0)("UserId").ToString + vbCrLf + vbCrLf +
                               "Password: " + E64.Decrypt(dt.Rows(0)("Password").ToString, ConfigurationManager.AppSettings("EncryptionKey").ToString)

            lblGetLink.Text = dt.Rows(0)("Url").ToString

            txtCopyPlaceholder.Text = dt.Rows(0)("Url").ToString

            Ext.Net.X.AddScript("copyToClipboard('" + dt.Rows(0)("Url").ToString + "');")
            Ext.Net.X.Toast("Link telah disalin")

            If Session("UserID").Equals("9220312051") Then
                winGetLink.Show()
            End If

        Else
            Ext.Net.X.Toast("Silahkan pilih baris terlebih dahulu!")
        End If
    End Sub
#End Region
#Region "Tab Interview"
    Protected Sub btnEditPesertaInterview_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        hdnSoloAssign.Value = False
        hdnMultiAssign.Value = False
        hdnSeqNo.Value = e.ExtraParams("SeqNo")
        Dim dt = clsAssignTest.getInterview(hdnSeqNo.Value)

        TabPanelAssignUjian.HideTab(PanelAssignPsikotest)
        TabPanelAssignUjian.ShowTab(PanelAssignInterview)

        FormPanelAssignInterview.Reset()
        winUndangPeserta.Title = "Edit Interview"
        winUndangPeserta.Icon = Icon.NoteEdit

        CmbTipeBatchInterview.ReadOnly = True
        txtBatchInterview.ReadOnly = True
        'txtBatchInterview.FieldStyle = READONLYCLS
        If dt.Rows(0)("Batch").ToString = "" Then
            CmbTipeBatchInterview.Select(1)
        Else
            CmbTipeBatchInterview.Select(0)
            txtBatchInterview.Text = dt.Rows(0)("Batch")
        End If
        Dim WaktuInterview = CDate(dt.Rows(0)("WaktuInterview"))
        DF_WaktuAssignInterview.SetValue(WaktuInterview)
        TF_WaktuAssignInterview.Text = WaktuInterview.ToString("HH:mm")
        txtLokasiInterview.Text = dt.Rows(0)("Lokasi")

        btnAssignInterview_Save.Width = 75
        btnAssignInterview_Save.Text = "Save"
        winUndangPeserta.Show()
    End Sub
    Protected Sub btnResendUndanganInterview_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Try
            clsAssignTest.sendWAInvitaion(e.ExtraParams("SeqNo"), "I")
            clsAssignTest.resendInterview(e.ExtraParams("SeqNo"), Session("UserID"))
            Ext.Net.X.Msg.Alert("Success", "Pesan telah terkirim!").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert(ex.Message, ex.StackTrace).Show()
        End Try
    End Sub
#End Region
#End Region

#End Region

#Region "Window Assign Ujian"

#Region "Panel Psikotest"
    Protected Sub CmbTipeBatchPsikotest_Change()
        If CmbTipeBatchPsikotest.Value.Equals("1") Then
            FldContainerBatchPsikotest.Show()
        Else
            FldContainerBatchPsikotest.Hide()
        End If
    End Sub
    Protected Sub selectNamaPaketAssignUjian()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalAssignUjian.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Dim NoPaket = rsm.SelectedRecordID
            Dim NamaPaket = clsAssignTest.getNamaPaket(NoPaket)
            CmbPaketSoalAssignUjian.SetValue(NoPaket, NamaPaket, True)
        End If
        rsm.ClearSelection()
    End Sub
    Public Sub btnAssignUjian_GC_Click()
        Dim now = DateAndTime.Now
        DF_WaktuAssignUjian.Value = now
        TF_WaktuAssignUjian.Value = Format(now, "HH:mm")
        chbBlock.Checked = False
    End Sub
    Public Sub btnAssignUjian_Save_Click()
        If DF_WaktuAssignUjian.Text.Equals("1/1/0001 12:00:00 AM") Or TF_WaktuAssignUjian.Text.Equals("__:__") Then
            Ext.Net.X.Msg.Alert("Alert", "Waktu Ujian belum diisi").Show()
            Return
        End If
        If CType(TF_WaktuAssignUjian.Text.Substring(0, 2), Int16) >= 24 Then
            Ext.Net.X.Msg.Alert("Alert", "Waktu Ujian tidak valid").Show()
            Return
        End If
        If CmbPaketSoalAssignUjian.Value.Equals("undefined") Or CmbPaketSoalAssignUjian.Value.Equals("") Then
            Ext.Net.X.Msg.Alert("Alert", "Pilih Paket Soal").Show()
            Return
        End If
        If CmbTipeBatchPsikotest.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Pilih Tipe Batch").Show()
            Return
        End If
        If CmbTipeBatchPsikotest.Text = "1" And txtBatchPsikotest.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Nama Batch belum diisi").Show()
            Return
        End If
        Dim WaktuUjian = Mid(DF_WaktuAssignUjian.Text, 1, InStr(DF_WaktuAssignUjian.Text, " ")) + TF_WaktuAssignUjian.Text

        If CType(hdnSoloAssign.Value, Boolean) And Not CType(hdnMultiAssign.Value, Boolean) Then     'Assign Solo
            If Not IsNothing(clsAssignTest.getStatusPengerjaanNoPeserta(hdnSoloAssignNoPeserta.Value)) Then
                Ext.Net.X.Msg.Alert("Alert", "Peserta " + hdnSoloAssignNoPeserta.Value + " sudah dijadwalkan untuk ujian atau sedang ujian").Show()
                Exit Sub
            End If

            clsAssignTest.assignUjian(txtBatchPsikotest.Text, hdnSoloAssignNoPeserta.Value, WaktuUjian, CmbPaketSoalAssignUjian.Value, Session("UserID"))
            Ext.Net.X.Msg.Alert("Success", "Undang ujian berhasil").Show()
        ElseIf Not CType(hdnSoloAssign.Value, Boolean) And CType(hdnMultiAssign.Value, Boolean) Then 'Assign Multi
            Dim rsm As RowSelectionModel = GridPanelPeserta.GetSelectionModel

            For Each r As SelectedRow In rsm.SelectedRows
                If Not IsNothing(clsAssignTest.getStatusPengerjaanNoPeserta(r.RecordID.ToString)) Then
                    Ext.Net.X.Msg.Alert("Alert", "Peserta " + r.RecordID.ToString + " sudah dijadwalkan untuk ujian atau sedang ujian").Show()
                    Exit Sub
                End If
            Next

            If rsm.SelectedRows.Count > 0 Then
                For Each r As SelectedRow In rsm.SelectedRows
                    clsAssignTest.assignUjian(txtBatchPsikotest.Text, r.RecordID, WaktuUjian, CmbPaketSoalAssignUjian.Value, Session("UserID"))
                Next
            End If
            Ext.Net.X.Msg.Alert("Success", "Undang ujian berhasil").Show()
        Else
            clsAssignTest.editUjian(WaktuUjian, CmbPaketSoalAssignUjian.Value, IIf(chbBlock.Checked, True, False), hdnUserID.Value.ToString, Session("UserID"))
            Ext.Net.X.Msg.Alert("Success", "Edit ujian berhasil").Show()
        End If
        winUndangPeserta.Close()
        StorePeserta.Reload()
        StorePesertaDtl.Reload()

        'Ext.Net.X.Msg.Alert("Success", "Undang ujian berhasil").Show()
    End Sub
#End Region
#Region "Panel Interview"
    Protected Sub CmbTipeBatchInterview_Change()
        If CmbTipeBatchInterview.Value.Equals("1") Then
            FldContainerBatchInterview.Show()
        Else
            FldContainerBatchInterview.Hide()
        End If
    End Sub
    Public Sub btnAssignInterview_GC_Click()
        Dim now = DateAndTime.Now
        DF_WaktuAssignInterview.Value = now
        TF_WaktuAssignInterview.Value = Format(now, "HH:mm")
    End Sub
    Public Sub btnAssignInterview_Save_Click()
        If DF_WaktuAssignInterview.Text.Equals("1/1/0001 12:00:00 AM") Or TF_WaktuAssignInterview.Text.Equals("__:__") Then
            Ext.Net.X.Msg.Alert("Alert", "Waktu Interview belum diisi").Show()
            Return
        End If
        If CType(TF_WaktuAssignInterview.Text.Substring(0, 2), Int16) >= 24 Then
            Ext.Net.X.Msg.Alert("Alert", "Waktu Interview tidak valid").Show()
            Return
        End If
        If txtLokasiInterview.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Pilih Lokasi").Show()
            Return
        End If
        If CmbTipeBatchInterview.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Pilih Tipe Batch").Show()
            Return
        End If
        If CmbTipeBatchInterview.Text = "1" And txtBatchInterview.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Nama Batch belum diisi").Show()
            Return
        End If
        Dim WaktuInterview = Mid(DF_WaktuAssignInterview.Text, 1, InStr(DF_WaktuAssignInterview.Text, " ")) + TF_WaktuAssignInterview.Text

        If CType(hdnSoloAssign.Value, Boolean) And Not CType(hdnMultiAssign.Value, Boolean) Then     'Assign Solo
            If Not clsAssignTest.getKelulusanPeserta(hdnSoloAssignNoPeserta.Value) Then
                Ext.Net.X.Msg.Alert("Alert", "Peserta " + hdnSoloAssignNoPeserta.Value + " belum bisa melanjutkan tahap interview karena belum lulus ujian").Show()
                Exit Sub
            End If

            clsAssignTest.assignInterview(txtBatchInterview.Text, hdnSoloAssignNoPeserta.Value, WaktuInterview, txtLokasiInterview.Text, Session("UserID"))
            Ext.Net.X.Msg.Alert("Success", "Undang interview berhasil").Show()
        ElseIf Not CType(hdnSoloAssign.Value, Boolean) And CType(hdnMultiAssign.Value, Boolean) Then 'Assign Multi
            Dim rsm As RowSelectionModel = GridPanelPeserta.GetSelectionModel

            For Each r As SelectedRow In rsm.SelectedRows
                If Not clsAssignTest.getKelulusanPeserta(r.RecordID) Then
                    Ext.Net.X.Msg.Alert("Alert", "Peserta " + r.RecordID + " belum bisa melanjutkan tahap interview karena belum lulus ujian").Show()
                    Exit Sub
                End If
            Next
            If rsm.SelectedRows.Count > 0 Then
                For Each r As SelectedRow In rsm.SelectedRows
                    clsAssignTest.assignInterview(txtBatchInterview.Text, r.RecordID, WaktuInterview, txtLokasiInterview.Text, Session("UserID"))
                Next
            End If
            Ext.Net.X.Msg.Alert("Success", "Undang interview berhasil").Show()
        Else
            clsAssignTest.editInterview("", WaktuInterview, hdnSeqNo.Value, txtLokasiInterview.Text, Session("UserID"))
            Ext.Net.X.Msg.Alert("Success", "Edit interview berhasil").Show()
        End If
        winUndangPeserta.Close()
        StorePeserta.Reload()
        StorePesertaInterview.Reload()

        'Ext.Net.X.Msg.Alert("Success", "Undang interview berhasil").Show()
    End Sub
#End Region
#End Region

#Region "Window Cek Nilai"
    Protected Sub SelectedNilaiHasilUjian_Click()
        Dim rsm As RowSelectionModel = GridPanelNilaiHasilUjian.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    hdnFilterNilaiDtl.Text = "UserId = '" + hdnUserID.Value.ToString + "' AND NoGroup = " + r.RecordID.ToString
                    hdnFilterNilaiDtl.Text += " AND JawabanDiPilih IS NOT NULL"
                    Dim Tipe = clsAssignTest.getTipeGroupSoal(r.RecordID.ToString)
                    If Not IsNothing(Tipe) Then
                        If Trim(Tipe).Equals("PG") Then
                            GridPanelNilaiHasilUjianDtl.Show()
                            ChartNilaiHasilUjianDtl.Hide()

                            GridPanelNilaiHasilUjianDtl.GetStore.Reload()
                        ElseIf Trim(Tipe).Equals("Kreplin") Then
                            GridPanelNilaiHasilUjianDtl.Hide()
                            ChartNilaiHasilUjianDtl.Show()

                            ChartNilaiHasilUjianDtl.GetStore.Reload()
                        End If
                    End If
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
#End Region

#Region "Private Method"

    Private Function GetSelectedMultiCombo(cmb As Object) As String
        Dim s As New List(Of String)
        For i As Integer = 0 To cmb.SelectedItems.Count - 1
            s.Add(" '" & cmb.SelectedItems(i).Value & "'")
        Next
        Return String.Join(",", s.ToArray)
    End Function
    Private Function DataTableToJSONArrPerColumn(dt As DataTable, ColName As String) As String
        Dim s As String = ""
        If dt.Rows.Count > 0 Then
            s += "["
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim cell = dt.Rows(i)(ColName).ToString
                If dt.Columns(ColName).DataType.ToString.Equals("System.String") Then
                    cell = "\""" + cell + "\"""
                End If

                If i < dt.Rows.Count - 1 Then
                    s += cell + ","
                ElseIf i = dt.Rows.Count - 1 Then
                    s += cell
                End If
            Next
            s += "]"
        End If
        Return s
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable, filename As String)

        Dim excelPackage = New ExcelPackage
        Dim excelWorksheet = excelPackage.Workbook.Worksheets.Add(filename)

        Dim rangeTable = excelWorksheet.Cells("A1").LoadFromDataTable(tbl, True)
        'set Groups
        Dim totRow = tbl.Rows.Count + 1

        Dim iRowIndex As Integer = 1
        If tbl.Rows.Count > 0 Then
            For iColumn As Integer = 0 To tbl.Columns.Count - 1
                If tbl.Columns(iColumn).DataType.ToString = "System.DateTime" Then
                    excelWorksheet.Cells(2, iColumn + 1, totRow, iColumn + 1).Style.Numberformat.Format = "dd-mmm-yyyy HH:mm"
                    excelWorksheet.Cells(2, iColumn + 1, totRow, iColumn + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                End If
                If tbl.Columns(iColumn).DataType.ToString = "System.Decimal" Then
                    excelWorksheet.Cells(2, iColumn + 1, totRow, iColumn + 1).Style.Numberformat.Format = "_(* #,##0_);_(* \(#,##0\);_(* ""-""??_);_(@_)"
                    excelWorksheet.Cells(2, iColumn + 1, totRow, iColumn + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
                End If
            Next
            'Border
            For i As Integer = 0 To totRow - 1
                For iColumn As Integer = 0 To tbl.Columns.Count - 1
                    excelWorksheet.Cells(iRowIndex, iColumn + 1).Style.Border.BorderAround(Style.ExcelBorderStyle.Thin)
                Next
                iRowIndex += 1
            Next
        End If
        excelWorksheet.Row(1).Style.Font.Bold = True
        excelWorksheet.Row(1).Style.Font.Size = 12

        'set warna judul
        If tbl.Columns.Count >= 26 Then
            excelWorksheet.Cells(Left(rangeTable.Address, 5) & "1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            excelWorksheet.Cells(Left(rangeTable.Address, 5) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue)
        Else
            excelWorksheet.Cells(Left(rangeTable.Address, 4) & "1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            excelWorksheet.Cells(Left(rangeTable.Address, 4) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue)
        End If

        excelWorksheet.Cells(excelWorksheet.Dimension.Address).AutoFitColumns()
        excelWorksheet.Cells(excelWorksheet.Dimension.Address).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center

        Dim stream As MemoryStream = New MemoryStream(excelPackage.GetAsByteArray())

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=""" & filename & ".xlsx""")

        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub GenerateExportPsikotest(ByVal dt As DataTable)
        Dim eP = New ExcelPackage
        Dim eW = eP.Workbook.Worksheets.Add("Psikotest")

        Dim currRow = 0
        Dim currCol = 0

        Dim MaxGroup = dt.Rows(0)("MaxGroup")
        Dim NoGroups = From row In dt.AsEnumerable()
                       Order By row.Field(Of Date)("TimeInput")
                       Select row.Field(Of Int32)("NoGroup") Distinct


        Dim enNoGroup As IEnumerator = NoGroups.GetEnumerator

        'set judul
        currRow += 1
        eW.Cells(currRow, 1).Value = "Report Export Psikotest"
        eW.Cells(currRow, 1).Style.Font.Bold = True
        eW.Cells(currRow, 1).Style.Font.Size = 12
        eW.Cells(currRow, 1, currRow, 3).Merge = True

        'set col width & mulai table
        currRow += 1
        Dim tableRowStart = currRow
        currCol = 0
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(14) : eW.Cells(currRow, currCol).Value = "Nomor Peserta"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Nama Peserta"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "No KTP"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Batch"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Status Pesan"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Status Pengerjaan"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Nama Paket"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Undang Ke"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Tgl Lahir"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Usia"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Jenis Kelamin"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Alamat"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Pendidikan Terakhir"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "SIM yang dimiliki"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "No HP"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Lokasi Test"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Posisi Dilamar"
        While enNoGroup.MoveNext
            Dim NoGroup = enNoGroup.Current

            Dim dr = dt.Select("NoGroup = " + NoGroup.ToString, "Nama Group").FirstOrDefault
            Dim NamaGroup = If(dr Is Nothing, "", dr.Item("Nama Group"))

            currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = NamaGroup
        End While
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "HASIL AKHIR"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Waktu Test"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Waktu Dimulai"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(12) : eW.Cells(currRow, currCol).Value = "User Input"

        Dim tableColumnStop = currCol
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Font.Bold = True
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Font.Color.SetColor(System.Drawing.Color.White)
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0070c0"))

        'isi tabel
        Dim UserIds = From row In dt.AsEnumerable()
                      Select row.Field(Of String)("UserId") Distinct

        Dim rowcount = 0
        Dim enUserIds As IEnumerator = UserIds.GetEnumerator
        While enUserIds.MoveNext
            currRow += 1
            rowcount += 1
            Dim UserId = enUserIds.Current

            Dim Rows = dt.Select("UserId = '" + UserId + "'")
            Dim TEMPdt = Rows.CopyToDataTable

            currCol = 0
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = Rows(0)("Nomor Peserta")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Nama Peserta")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = Rows(0)("No KTP")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Batch")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Status Pesan")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Status Pengerjaan")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Nama Paket")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = Rows(0)("Undang Ke")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = If(IsDBNull(Rows(0)("Tgl Lahir")), "", CDate(Rows(0)("Tgl Lahir")).ToString("dd/MM/yyyy"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = Rows(0)("Usia")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = Rows(0)("Jenis Kelamin")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Alamat")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = Rows(0)("Pendidikan Terakhir")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = Rows(0)("SIM yang dimiliki")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("No HP")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Lokasi Test")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("Posisi Dilamar")

            enNoGroup = NoGroups.GetEnumerator
            While enNoGroup.MoveNext
                Dim NoGroup = enNoGroup.Current
                Dim drGroupRes = TEMPdt.Select("UserId = '" + UserId + "' AND NoGroup = " + NoGroup.ToString, "GroupRes").FirstOrDefault
                If drGroupRes Is Nothing Then
                    currCol += 1
                    Continue While
                End If
                Dim GroupRes = drGroupRes.Item("GroupRes")

                currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = GroupRes
            End While

            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("HASIL AKHIR")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = If(IsDBNull(Rows(0)("Waktu Test")), "", CDate(Rows(0)("Waktu Test")).ToString("dd-MMM-yyyy HH:mm"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = If(IsDBNull(Rows(0)("Waktu Dimulai")), "", CDate(Rows(0)("Waktu Dimulai")).ToString("dd-MMM-yyyy HH:mm"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = Rows(0)("UserInput")

        End While

        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin


        Dim stream As MemoryStream = New MemoryStream(eP.GetAsByteArray())

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=ExportHasilUjian.xlsx")
        Response.AddHeader("content-length", stream.ToArray().Length)

        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub GenerateExportInterview(ByVal dt As DataTable)
        Dim eP = New ExcelPackage
        Dim eW = eP.Workbook.Worksheets.Add("Interview")

        Dim currRow = 0
        Dim currCol = 0

        'set judul
        currRow += 1
        eW.Cells(currRow, 1).Value = "Report Export Interview"
        eW.Cells(currRow, 1).Style.Font.Bold = True
        eW.Cells(currRow, 1).Style.Font.Size = 12
        eW.Cells(currRow, 1, currRow, 3).Merge = True

        'set col width & mulai table
        currRow += 1
        Dim tableRowStart = currRow
        currCol = 0
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(14) : eW.Cells(currRow, currCol).Value = "Nomor Peserta"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Nama Peserta"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "No KTP"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Batch"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Status Pesan"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Status Interview"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Waktu Interview"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Lokasi"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Undang Ke"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Tgl Lahir"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Usia"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "Jenis Kelamin"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Alamat"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Pendidikan Terakhir"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "SIM yang dimiliki"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(16) : eW.Cells(currRow, currCol).Value = "No HP"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Lokasi Test"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(20) : eW.Cells(currRow, currCol).Value = "Posisi Dilamar"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(12) : eW.Cells(currRow, currCol).Value = "User Input"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Time Input"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(12) : eW.Cells(currRow, currCol).Value = "User Edit"
        currCol += 1 : eW.Column(currCol).Width = GetTrueColumnWidth(18) : eW.Cells(currRow, currCol).Value = "Time Edit"

        Dim tableColumnStop = currCol
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Font.Bold = True
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Font.Color.SetColor(System.Drawing.Color.White)
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        eW.Cells(currRow, 1, currRow, tableColumnStop).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0070c0"))

        'isi tabel
        'Dim NoPesertas = From row In dt.AsEnumerable()
        '                 Select row.Field(Of String)("Nomor Peserta")

        Dim rowcount = 0
        'Dim enNoPesertas As IEnumerator = NoPesertas.GetEnumerator
        'While enNoPrsertas.MoveNext
        For Each dr As DataRow In dt.Rows
            currRow += 1
            rowcount += 1

            'Dim Rows = dt.Select("UserId = '" + UserId + "'")
            'Dim TEMPdt = Rows.CopyToDataTable

            currCol = 0
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = dr("Nomor Peserta")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Nama Peserta")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = dr("No KTP")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Batch")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Status Pesan")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Status Interview")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = If(IsDBNull(dr("Waktu Interview")), "", CDate(dr("Waktu Interview")).ToString("dd-MMM-yyyy HH:mm"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Lokasi")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = dr("Undang Ke")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = If(IsDBNull(dr("Tgl Lahir")), "", CDate(dr("Tgl Lahir")).ToString("dd/MM/yyyy"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right : eW.Cells(currRow, currCol).Value = dr("Usia")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = dr("Jenis Kelamin")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Alamat")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = dr("Pendidikan Terakhir")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center : eW.Cells(currRow, currCol).Value = dr("SIM yang dimiliki")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("No HP")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Lokasi Test")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("Posisi Dilamar")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("UserInput")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = If(IsDBNull(dr("TimeInput")), "", CDate(dr("TimeInput")).ToString("dd-MMM-yyyy HH:mm"))
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = dr("UserEdit")
            currCol += 1 : eW.Cells(currRow, currCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left : eW.Cells(currRow, currCol).Value = If(IsDBNull(dr("TimeEdit")), "", CDate(dr("TimeEdit")).ToString("dd-MMM-yyyy HH:mm"))

        Next

        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        eW.Cells(tableRowStart, 1, currRow, tableColumnStop).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin


        Dim stream As MemoryStream = New MemoryStream(eP.GetAsByteArray())

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=ExportHasilInterview.xlsx")
        Response.AddHeader("content-length", stream.ToArray().Length)

        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)
        Response.Flush()
        Response.Close()
    End Sub
    Private Function getTglIndo(dt As Date) As String
        Dim month = dt.Month
        Dim bulan As String = ""
        If month < 7 Then
            If month < 4 Then
                If month = 1 Then
                    bulan = " Januari "
                ElseIf month = 2 Then
                    bulan = " Februari "
                Else
                    bulan = " Maret "
                End If
            Else
                If month = 4 Then
                    bulan = " April "
                ElseIf month = 5 Then
                    bulan = " Mei "
                Else
                    bulan = " Juni "
                End If
            End If
        Else
            If month < 10 Then
                If month = 7 Then
                    bulan = " Juli "
                ElseIf month = 8 Then
                    bulan = " Agustus "
                Else
                    bulan = " September "
                End If
            Else
                If month = 10 Then
                    bulan = " Oktober "
                ElseIf month = 11 Then
                    bulan = " November "
                Else
                    bulan = " Desember "
                End If
            End If
        End If
        Return dt.Day.ToString + bulan + dt.Year.ToString
    End Function
    Function HasDiffMoreThanOneMonth(date1 As Date, date2 As Date) As Boolean
        Dim diffMonths As Integer = (date2.Year - date1.Year) * 12 + (date2.Month - date1.Month)

        If diffMonths > 1 OrElse (diffMonths = 1 AndAlso date2.Day >= date1.Day) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GetTrueColumnWidth(ByVal width As Double) As Double
        Dim z As Double = 1.0R

        If width >= (1 + 2 / 3) Then
            z = Math.Round((Math.Round(7 * (width - 1 / 256), 0) - 5) / 7, 2)
        Else
            z = Math.Round((Math.Round(12 * (width - 1 / 256), 0) - Math.Round(5 * width, 0)) / 12, 2)
        End If

        Dim errorAmt As Double = width - z
        Dim adj As Double = 0R

        If width >= (1 + 2 / 3) Then
            adj = (Math.Round(7 * errorAmt - 7 / 256, 0)) / 7
        Else
            adj = ((Math.Round(12 * errorAmt - 12 / 256, 0)) / 12) + (2 / 12)
        End If

        If z > 0 Then
            Return width + adj
        End If

        Return 0R
    End Function

#End Region
End Class
