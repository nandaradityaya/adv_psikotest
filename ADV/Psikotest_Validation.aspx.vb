
Imports System.Data
Imports Ext.Net
Imports System.Data.SqlClient
Imports System.Linq
Imports System.IO
Imports OfficeOpenXml
Imports System.Web.UI.Page
Imports Excel

Partial Class Psikotest_Start
    Inherits System.Web.UI.Page
    Dim tblNormaDtl As DataTable
#Region "Default"
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CheckUserPermission()

            DtFilterTo.Text = Date.Now.ToString("MM/dd/yyyy")
            DtFilterFrom.Text = (Date.Now).AddMonths(-3).ToString("MM/dd/yyyy")

            mcbPosisi.GetStore().DataSource = clsAssignTest.getPosisi()

            Ext.Net.X.AddScript("Loaded();")
        End If
    End Sub

    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Sub RefreshGrid(lvl As Int16)
        If lvl > 0 Then
            GridPanelPaketSoalGroupDtlJawaban.GetStore.Reload()
            GridPanelPaketSoalGroupDtlJawaban.GetSelectionModel().DeselectAll()
            GridPanelPaketSoalGroupDtlJawaban.Call("clearMemory")
            If lvl > 1 Then
                GridPanelPaketSoalGroupDtl.GetStore.Reload()
                GridPanelPaketSoalGroupDtl.GetSelectionModel().DeselectAll()
                GridPanelPaketSoalGroupDtl.Call("clearMemory")
                If lvl > 2 Then
                    GridPanelPaketSoalGroup.GetStore.Reload()
                    GridPanelPaketSoalGroup.GetSelectionModel().DeselectAll()
                    GridPanelPaketSoalGroup.Call("clearMemory")
                    If lvl > 3 Then
                        GridPanelPaketSoal.GetStore.Reload()
                        GridPanelPaketSoal.GetSelectionModel().DeselectAll()
                        GridPanelPaketSoal.Call("clearMemory")
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "Main UI"
#Region "GridPanel PaketSoal"
    Public Sub BtnFilter_Click()
        PaketSoalFilter.Value = ""
        If chbFilterTimeInput.Checked Then
            Dim dtTo As Date = DtFilterTo.Text
            dtTo = dtTo.AddHours(23).AddMinutes(59).AddSeconds(59)
            Dim a = dtTo.ToString("M/dd/yyyy hh:mm:ss tt")
            PaketSoalFilter.Value += " '" & DtFilterFrom.Text & "' <= TimeInput AND TimeInput <= '" & a & "' AND "
        Else
            PaketSoalFilter.Value += " 1=1 AND "
        End If
        PaketSoalFilter.Value += IIf(CmbFilterStatus.Value.Equals("0"), " bAktif = 'Nonaktif' ", IIf(CmbFilterStatus.Value.Equals("1"), " bAktif = 'Aktif' ", " 1=1 "))

        PaketSoalGroupFilter.Value = ""
        PaketSoalGroupDtlFilter.Value = ""
        PaketSoalGroupDtlJawabanFilter.Value = ""
        RefreshGrid(4)

    End Sub
    Protected Sub btnAddPaketSoal_Click()
        FormPanelAddEditPaketSoal.Reset()

        winAddEditPaketSoal.Title = "Tambah Paket Soal"
        winAddEditPaketSoal.Icon = Icon.NoteAdd
        winAddEditPaketSoal.Show()

        rdoPaketSoalStatusOn.ReadOnly = True
        rdoPaketSoalStatusOff.ReadOnly = True
    End Sub
    Protected Sub SelectedPaketSoal_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoal.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    PaketSoalGroupFilter.Value = "NoPaket=" + r.RecordID.ToString
                    hdnCurrNoPaket.Value = r.RecordID.ToString

                    PaketSoalGroupDtlFilter.Value = ""
                    PaketSoalGroupDtlJawabanFilter.Value = ""
                    RefreshGrid(3)
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
    Public Sub btnEditPaketSoal_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        FormPanelAddEditPaketSoal.Reset()

        Dim dt = clsPaketSoal.GetPaketSoal(e.ExtraParams("NoPaket").ToString)
        If clsPaketSoal.cekStatusPenggunaanPaketSoal(dt.Rows(0)("NoPaket")) Then
            Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal sedang digunakan dalam proses ujian, tidak bisa merubah paket soal").Show()
            Return
        End If

        txtNomorPaketSoal_PS.Text = dt.Rows(0)("NoPaket")
        txtNamaPaketSoal_PS.Text = dt.Rows(0)("NamaPaket")
        nfToleransiWaktu.Text = dt.Rows(0)("ToleransiWaktu")
        If dt.Rows(0)("bAktif").ToString.Equals("True") Then
            rdoPaketSoalStatusOn.Checked = True
        Else
            rdoPaketSoalStatusOff.Checked = True
        End If

        dt = clsPaketSoal.GetPaketSoalChild(dt.Rows(0)("NoPaket").ToString)

        Dim s As String = ""
        For Each dr As DataRow In dt.Rows
            s += "'" + dr("CodePosisi") + "',"
        Next

        If s.Length > 0 Then
            Ext.Net.X.AddScript("App.mcbPosisi.setValue([" + s.Substring(0, s.Length - 1) + "]);")
        End If

        winAddEditPaketSoal.Title = "Edit Paket Soal"
        winAddEditPaketSoal.Icon = Icon.NoteEdit
        winAddEditPaketSoal.Show()

        rdoPaketSoalStatusOn.ReadOnly = False
        rdoPaketSoalStatusOff.ReadOnly = False
    End Sub
#End Region
#Region "GridPanel PaketSoalGroup"
    Protected Sub btnAddPaketSoalGroup_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoal.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            If clsPaketSoal.cekStatusPaketSoal(rsm.SelectedRows(0).RecordID) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal masih aktif. Group Soal tidak bisa ditambah").Show()
                Return
            End If
            FormPanelAddEditPaketSoalGroup.Reset()

            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    txtNomorPaketSoal_PSG.Text = r.RecordID.ToString
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try

            reloadtblNormaDtl("0")
            chbKreplin.ReadOnly = False
            winAddEditPaketSoalGroup.Title = "Tambah Group Soal"
            winAddEditPaketSoalGroup.Icon = Icon.NoteAdd
            winAddEditPaketSoalGroup.Show()

            rdoPaketSoalGroupStatusOn.ReadOnly = True
            rdoPaketSoalGroupStatusOff.ReadOnly = True
        Else
            Ext.Net.X.Msg.Alert("Alert", "Pilih Paket Soal").Show()
        End If
    End Sub
    'Protected Sub btnSubmitPaketSoalGroup_Click()
    'End Sub
    'Protected Sub btnCancelPaketSoalGroup_Click()
    'End Sub
    Protected Sub btnDeletePaketSoalGroup_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroup.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            Dim dt = clsPaketSoal.GetPaketSoalGroup(rsm.SelectedRows(0).RecordID)
            If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal masih aktif. Group Soal tidak bisa dihapus").Show()
                Return
            End If
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    clsPaketSoal.DeletePaketSoalGroup(r.RecordID.ToString)
                Next
                RefreshGrid(3)

                Ext.Net.X.Msg.Alert("Delete", "Hapus Sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try
        Else
            Ext.Net.X.Msg.Alert("Failed", "Pilih Pertanyaan").Show()
        End If
    End Sub
    Protected Sub SelectedPaketSoalGroup_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroup.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    Dim NoGroup = r.RecordID.ToString
                    Dim Tipe = clsAssignTest.getTipeGroupSoal(NoGroup)
                    If Not IsNothing(Tipe) Then
                        If Trim(Tipe).Equals("PG") Then
                            btnAddPaketSoalGroupDtl.Enable()
                            btnImportPaketSoalGroupDtl.Enable()
                            btnDeletePaketSoalGroupDtl.Enable()
                            btnCopyPaketSoalGroupDtl.Enable()
                            btnAddPaketSoalGroupDtlJawaban.Enable()
                            BtnImportJawaban.Enable()
                            btnDeletePaketSoalGroupDtlJawaban.Enable()
                        ElseIf Trim(Tipe).Equals("Kreplin") Then
                            btnAddPaketSoalGroupDtl.Disable()
                            btnImportPaketSoalGroupDtl.Disable()
                            btnDeletePaketSoalGroupDtl.Disable()
                            btnCopyPaketSoalGroupDtl.Disable()
                            btnAddPaketSoalGroupDtlJawaban.Disable()
                            BtnImportJawaban.Disable()
                            btnDeletePaketSoalGroupDtlJawaban.Disable()
                        End If
                    End If

                    PaketSoalGroupDtlFilter.Value = "NoGroup=" + NoGroup
                    hdnCurrNoGroup.Value = NoGroup

                    PaketSoalGroupDtlJawabanFilter.Value = ""
                    RefreshGrid(2)
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
    Protected Sub btnEditPaketSoalGroup_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        FormPanelAddEditPaketSoalGroup.Reset()

        Dim dt = clsPaketSoal.GetPaketSoalGroup(e.ExtraParams("NoGroup").ToString)
        If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Then
            Ext.Net.X.Msg.Alert("Alert", "Tidak dapat mengedit group soal karena data paket soal masih aktif").Show()
            Return
        End If

        reloadtblNormaDtl(dt.Rows(0)("NoGroup").ToString)
        txtNomorPaketSoal_PSG.Text = dt.Rows(0)("NoPaket")
        txtNomorPaketSoalGroup_PSG.Text = dt.Rows(0)("NoGroup")

        txtNamaPaketSoalGroup_PSG.Text = dt.Rows(0)("NamaGroup")
        chbKreplin.ReadOnly = True
        If Trim(dt.Rows(0)("Tipe")).Equals("Kreplin") Then 'Group Soal kreplin tandanya nilai standar = -1
            chbKreplin.Checked = True
            nfJmlKolom.Text = dt.Rows(0)("MinimumJmlSoal")
        Else
            nfMinimumJmlSoal.Text = dt.Rows(0)("MinimumJmlSoal")
        End If
        nfNilaiStandar.Text = dt.Rows(0)("NilaiStandar")
        nfWaktuPengerjaan.Text = dt.Rows(0)("WaktuPengerjaan")
        CmbPetunjuk.SetValue(dt.Rows(0)("NoPetunjuk"), dt.Rows(0)("NoPetunjuk"), True)
        chbPaketSoalGroupRandom.Checked = IIf(dt.Rows(0)("bRandom").ToString.Equals("True"), True, False)
        chbPaketSoalGroupIsPrioritas.Checked = IIf(dt.Rows(0)("IsPrioritas").ToString.Equals("True"), True, False)

        rdoPaketSoalGroupStatusOn.ReadOnly = False
        rdoPaketSoalGroupStatusOff.ReadOnly = False
        If dt.Rows(0)("bAktif").ToString.Equals("True") Then
            rdoPaketSoalGroupStatusOn.Checked = True
        Else
            rdoPaketSoalGroupStatusOff.Checked = True
        End If

        winAddEditPaketSoalGroup.Title = "Edit Group Soal"
        winAddEditPaketSoalGroup.Icon = Icon.NoteEdit
        winAddEditPaketSoalGroup.Show()
    End Sub
    Public Sub reloadtblNormaDtl(NoGroup As String)
        Using iSqlHelper As New clsSQLHelper
            iSqlHelper.CommandText = "SELECT * " +
                                     "FROM ADVPSIKOTEST..MS_NormaDtl " +
                                     "WHERE NoGroup = " & NoGroup
            iSqlHelper.CommandType = CommandType.Text
            tblNormaDtl = iSqlHelper.ExecuteDataTable
        End Using
        tblNormaDtl.AcceptChanges()
        Session("tblNormaDtl") = tblNormaDtl
        StoreNormaDtl.DataSource = tblNormaDtl
        StoreNormaDtl.DataBind()

        Session("HapusDataNormaDtl") = New List(Of String)
    End Sub
#End Region
#Region "GridPanel PaketSoalGroupDtl"
    Protected Sub btnAddPaketSoalGroupDtl_Click()
        Dim rsm1 As RowSelectionModel = GridPanelPaketSoal.GetSelectionModel()
        Dim rsm2 As RowSelectionModel = GridPanelPaketSoalGroup.GetSelectionModel()
        If rsm1.SelectedRows.Count = 1 And rsm2.SelectedRows.Count = 1 Then
            If clsPaketSoal.cekStatusPaketSoal(rsm1.SelectedRows(0).RecordID) Or
               clsPaketSoal.cekStatusPaketSoalGroup(rsm2.SelectedRows(0).RecordID) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal atau Group Soal masih aktif. Pertanyaan tidak bisa ditambah").Show()
                Return
            End If
            FormPanelAddEditPaketSoalGroupDtl.Reset()
            hdnFotoDtl.Value = "N"
            'hdnFotoDtl.Value -N = Belum upload foto dan sebelumnya memang kosong
            '                 -Y = Sudah upload atau ganti foto baru
            '                 -S = Foto sudah ada dan tidak diganti
            Try
                For Each r As SelectedRow In rsm1.SelectedRows
                    txtNomorPaketSoal_PSGD.Text = r.RecordID.ToString
                Next
                For Each r As SelectedRow In rsm2.SelectedRows
                    txtNomorPaketSoalGroup_PSGD.Text = r.RecordID.ToString
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try

            winAddEditPaketSoalGroupDtl.Title = "Tambah Pertanyaan"
            winAddEditPaketSoalGroupDtl.Icon = Icon.NoteAdd
            winAddEditPaketSoalGroupDtl.Show()

            TampilanFilePaketSoalGroupDtl.Hide()
            imgPreviewDtl.ImageUrl = "ViewAttachment.ashx?FileName=" + ConfigurationManager.AppSettings("ESSMasterPath").ToString + "nophoto.jpg"
            chbPaketSoalGroupDtlStatusOn.ReadOnly = True
            chbPaketSoalGroupDtlStatusOff.ReadOnly = True
            Ext.Net.X.AddScript("CKEDITOR.instances['txtDeskripsiPertanyaan'].setData('');")
            Ext.Net.X.AddScript("ResetZIndex();")
        Else
            Ext.Net.X.Msg.Alert("Alert", "Pilih Paket Soal dan Group Soal").Show()
        End If
    End Sub
    Protected Sub btnImportPaketSoalGroupDtl_Click()
        FormPanelImportPaketSoalGroupDtl.Reset()
        winImportPaketSoalGroupDtl.Show()
    End Sub
    Protected Sub btnDeletePaketSoalGroupDtl_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroupDtl.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            Dim dt = clsPaketSoal.GetPaketSoalGroupDtl(rsm.SelectedRows(0).RecordID)
            If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Or
               clsPaketSoal.cekStatusPaketSoalGroup(dt.Rows(0)("NoGroup")) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal atau Group Soal masih aktif. Pertanyaan tidak bisa dihapus").Show()
                Return
            End If
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    clsPaketSoal.DeletePaketSoalGroupDtl(r.RecordID.ToString)
                Next
                RefreshGrid(2)

                Ext.Net.X.Msg.Alert("Delete", "Hapus Sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try
        Else
            Ext.Net.X.Msg.Alert("Failed", "Pilih Pertanyaan").Show()
        End If
    End Sub
    Protected Sub btnCopyPaketSoalGroupDtl_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroupDtl.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            WinCopyPaketSoalGroupDtl.Show()
            FormPanelCopyPaketSoalGroupDtl.Reset()

            GridPanelCopyPaketSoalGroupDtl.GetSelectionModel.ClearSelection()
            GridPanelCopyPaketSoalGroupDtl.GetStore.Reload()
        Else
            Ext.Net.X.Msg.Alert("Failed", "Pilih Pertanyaan").Show()
        End If
    End Sub
    Protected Sub SelectedPaketSoalGroupDtl_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroupDtl.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    PaketSoalGroupDtlJawabanFilter.Value = "NoPaket=(SELECT NoPaket FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl WHERE SeqNo=" + r.RecordID.ToString + ") AND " +
                                                          "NoGroup=(SELECT NoGroup FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl WHERE SeqNo=" + r.RecordID.ToString + ") AND " +
                                                          "NoUrut=(SELECT NoUrut FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl WHERE SeqNo=" + r.RecordID.ToString + ")"
                    hdnCurrNoDtl.Value = r.RecordID.ToString

                    RefreshGrid(1)
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
    Protected Sub GridPanelPaketSoalGroupDtlPageChanged()
        GridPanelPaketSoalGroupDtl.GetSelectionModel.ClearSelection()
        GridPanelPaketSoalGroupDtl.Call("clearMemory")
    End Sub
    Protected Sub btnEditPaketSoalGroupDtl_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        FormPanelAddEditPaketSoalGroupDtl.Reset()
        hdnFotoDtl.Value = "N"

        Dim dt = clsPaketSoal.GetPaketSoalGroupDtl(e.ExtraParams("SeqNo").ToString)

        If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Or
           clsPaketSoal.cekStatusPaketSoalGroup(dt.Rows(0)("NoGroup")) Then
            Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal atau Group Soal masih aktif. Pertanyaan tidak bisa diedit").Show()
            Return
        End If

        txtNomorPaketSoal_PSGD.Text = dt.Rows(0)("NoPaket")
        txtNomorPaketSoalGroup_PSGD.Text = dt.Rows(0)("NoGroup")
        txtNomorPaketSoalGroupDtl_PSGD.Text = dt.Rows(0)("SeqNo")

        txtJudulPertanyaan.Text = dt.Rows(0)("Judul")
        chbPaketSoalGroupDtlIsDownload.Checked = IIf(dt.Rows(0)("IsDownload").ToString.Equals("1"), True, False)
        chbPaketSoalGroupDtlStatusOn.ReadOnly = False
        chbPaketSoalGroupDtlStatusOff.ReadOnly = False
        imgPreviewDtl.ImageUrl = "ViewAttachment.ashx?FileName=" + ConfigurationManager.AppSettings("ESSMasterPath").ToString + "nophoto.jpg"

        Dim TMedia = Trim(dt.Rows(0)("TipeMedia").ToString)
        If TMedia.Equals("NOMEDIA") Then
            RadioTipeMediaN_PSGD.Checked = True
            TampilanFilePaketSoalGroupDtl.Hide()
            imgPreviewDtl.ImageUrl = ""
        ElseIf TMedia.Equals("YOUTUBE") Then
            RadioTipeMediaL_PSGD.Checked = True
            TxtMediaLDtl.Text = dt.Rows(0)("UrlMedia")
            TampilanFilePaketSoalGroupDtl.Hide()
            imgPreviewDtl.ImageUrl = ""
        Else
            Dim getPathParrent As String = ConfigurationManager.AppSettings("ESSMasterPath").ToString
            If Right(getPathParrent, 1) <> "/" Then
                getPathParrent += "/"
            End If
            RadioTipeMediaU_PSGD.Checked = True
            imgPreviewDtl.ImageUrl = "ViewAttachment.ashx?FileName=" + getPathParrent + dt.Rows(0)("NoPaket").ToString + "/" + dt.Rows(0)("NoGroup").ToString + "/" + dt.Rows(0)("MediaFileName").ToString
            hdnFotoDtl.Value = "S"
            hdnFotoDtlFName.Value = dt.Rows(0)("MediaFileName").ToString
        End If
        If dt.Rows(0)("bAktif").ToString.Equals("True") Then
            chbPaketSoalGroupDtlStatusOn.Checked = True
        Else
            chbPaketSoalGroupDtlStatusOn.Checked = True
        End If

        winAddEditPaketSoalGroupDtl.Title = "Edit Pertanyaan"
        winAddEditPaketSoalGroupDtl.Icon = Icon.NoteEdit
        winAddEditPaketSoalGroupDtl.Show()

        Ext.Net.X.AddScript("CKEDITOR.instances['txtDeskripsiPertanyaan'].setData('" + Replace(dt.Rows(0)("Deskripsi"), Chr(10).ToString, "") + "');")
        Ext.Net.X.AddScript("ResetZIndex();")
    End Sub
#End Region
#Region "GridPanel PaketSoalGroupDtlJawaban"
    Protected Sub btnAddPaketSoalGroupDtlJawaban_Click()
        Dim rsm1 As RowSelectionModel = GridPanelPaketSoal.GetSelectionModel()
        Dim rsm2 As RowSelectionModel = GridPanelPaketSoalGroup.GetSelectionModel()
        Dim rsm3 As RowSelectionModel = GridPanelPaketSoalGroupDtl.GetSelectionModel()
        If rsm1.SelectedRows.Count = 1 And rsm2.SelectedRows.Count = 1 And rsm3.SelectedRows.Count = 1 Then
            If clsPaketSoal.cekStatusPaketSoal(rsm1.SelectedRows(0).RecordID) Or
               clsPaketSoal.cekStatusPaketSoalGroup(rsm2.SelectedRows(0).RecordID) Or
               clsPaketSoal.cekStatusPaketSoalGroupDtlBySeqNo(rsm3.SelectedRows(0).RecordID) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal, Group Soal, atau Detail Soal masih aktif. Jawaban tidak bisa ditambah").Show()
                Return
            End If
            FormPanelAddEditPaketSoalGroupDtlJawaban.Reset()
            hdnFotoDtlJawaban.Value = "N"
            'hdnFotoDtlJawaban.Value -N = Belum upload foto dan sebelumnya memang kosong
            '                        -Y = Sudah upload atau ganti foto baru
            '                        -S = Foto sudah ada dan tidak diganti
            Try
                For Each r As SelectedRow In rsm1.SelectedRows
                    txtNomorPaketSoal_PSGDJ.Text = r.RecordID.ToString
                Next
                For Each r As SelectedRow In rsm2.SelectedRows
                    txtNomorPaketSoalGroup_PSGDJ.Text = r.RecordID.ToString
                Next
                For Each r As SelectedRow In rsm3.SelectedRows
                    txtNomorUrut_PSGDJ.Text = clsPaketSoal.getNoUrutDtl(r.RecordID.ToString)
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try

            TampilanFilePaketSoalGroupDtlJawaban.Hide()
            imgPreviewDtlJawaban.ImageUrl = "ViewAttachment.ashx?FileName=" + ConfigurationManager.AppSettings("ESSMasterPath").ToString + "nophoto.jpg"

            winAddEditPaketSoalGroupDtlJawaban.Title = "Tambah Jawaban"
            winAddEditPaketSoalGroupDtlJawaban.Icon = Icon.NoteAdd
            winAddEditPaketSoalGroupDtlJawaban.Show()
        Else
            Ext.Net.X.Msg.Alert("Alert", "Pilih Paket Soal, Group Soal, dan Pertanyaan").Show()
        End If
    End Sub
    Protected Sub btnImportPaketSoalGroupDtlJawaban_Click()
        If hdnCurrNoPaket.Value.ToString.Equals("") Or hdnCurrNoGroup.Value.ToString.Equals("") Or hdnCurrNoDtl.Value.ToString.Equals("") Then
            Ext.Net.X.Msg.Alert("Alert", "Pilih Paket Soal, Group Soal, dan Pertanyaan").Show()
        Else
            FormPanelImportPaketSoalGroupDtlJawaban.Reset()
            WinImportPaketSoalGroupDtlJawaban.Show()
        End If
    End Sub
    Protected Sub btnDeletePaketSoalGroupDtlJawaban_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroupDtlJawaban.GetSelectionModel()
        If rsm.SelectedRows.Count > 0 Then
            Dim dt = clsPaketSoal.GetPaketSoalGroupDtlJawaban(rsm.SelectedRows(0).RecordID)
            If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Or
               clsPaketSoal.cekStatusPaketSoalGroup(dt.Rows(0)("NoGroup")) Then
                Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal, Group Soal, atau Detail Soal masih aktif. Jawaban tidak bisa dihapus").Show()
                Return
            End If
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    clsPaketSoal.DeletePaketSoalGroupDtljawaban(r.RecordID.ToString)
                Next
                RefreshGrid(1)

                Ext.Net.X.Msg.Alert("Delete", "Hapus Sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try
        Else
            Ext.Net.X.Msg.Alert("Failed", "Pilih Jawaban").Show()
        End If
    End Sub
    Protected Sub SelectedPaketSoalGroupDtlJawaban_Click()
        Dim rsm As RowSelectionModel = GridPanelPaketSoalGroupDtlJawaban.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Try
                For Each r As SelectedRow In rsm.SelectedRows
                    hdnCurrNoJwb.Value = r.RecordID.ToString
                Next
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
        End If
    End Sub
    Protected Sub btnEditPaketSoalGroupDtlJawaban_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim SeqNo = e.ExtraParams("SeqNo").ToString
        Dim dt = clsPaketSoal.GetPaketSoalGroupDtlJawaban(SeqNo)
        'If clsPaketSoal.cekStatusPaketSoal(dt.Rows(0)("NoPaket")) Or
        '       clsPaketSoal.cekStatusPaketSoalGroup(dt.Rows(0)("NoGroup")) Or
        '       clsPaketSoal.cekStatusPaketSoalGroupDtlByNoUrut(dt.Rows(0)("NoPaket"), dt.Rows(0)("NoGroup"), dt.Rows(0)("NoUrut")) Then

        If clsPaketSoal.cekStatusPaketSoalGroup(dt.Rows(0)("NoGroup")) Then
            Ext.Net.X.Msg.Alert("Alert", "Data Paket Soal, Group Soal, atau Detail Soal masih aktif. Jawaban tidak bisa edit").Show()
            Return
        End If
        FormPanelAddEditPaketSoalGroupDtlJawaban.Reset()
        hdnFotoDtlJawaban.Value = "N"

        txtNomorPaketSoal_PSGDJ.Text = dt.Rows(0)("NoPaket")
        txtNomorPaketSoalGroup_PSGDJ.Text = dt.Rows(0)("NoGroup")
        txtNomorUrut_PSGDJ.Text = dt.Rows(0)("NoUrut")
        txtNomorJawaban.Text = dt.Rows(0)("NoJawaban")

        txtJawaban.Text = dt.Rows(0)("Jawaban")
        imgPreviewDtlJawaban.ImageUrl = "ViewAttachment.ashx?FileName=" + ConfigurationManager.AppSettings("ESSMasterPath").ToString + "nophoto.jpg"

        Dim TMedia = dt.Rows(0)("TipeMedia").ToString
        If TMedia.Equals("NOMEDIA") Then
            RadioTipeMediaN_PSGDJ.Checked = True
            TampilanFilePaketSoalGroupDtlJawaban.Hide()
            imgPreviewDtlJawaban.ImageUrl = ""
        ElseIf TMedia.Equals("YOUTUBE") Then
            RadioTipeMediaL_PSGDJ.Checked = True
            TxtMediaLDtlJawaban.Text = dt.Rows(0)("UrlMedia")
            TampilanFilePaketSoalGroupDtlJawaban.Hide()
            imgPreviewDtlJawaban.ImageUrl = ""
        Else
            Dim getPathParrent As String = ConfigurationManager.AppSettings("ESSMasterPath").ToString
            If Right(getPathParrent, 1) <> "/" Then
                getPathParrent += "/"
            End If
            RadioTipeMediaU_PSGDJ.Checked = True
            imgPreviewDtlJawaban.ImageUrl = "ViewAttachment.ashx?FileName=" + getPathParrent + dt.Rows(0)("NoPaket").ToString + "/" + dt.Rows(0)("NoGroup").ToString + "/" + dt.Rows(0)("MediaFileName").ToString
            hdnFotoDtlJawaban.Value = "S"
            hdnFotoDtlJawabanFName.Value = dt.Rows(0)("MediaFileName").ToString
        End If
        txtTextMedia_PSGDJ.Text = dt.Rows(0)("TextMedia")
        nfPoinJawaban.Text = dt.Rows(0)("NoJawabanBenar").ToString
        'chbJawabanBenar.Checked = IIf(dt.Rows(0)("NoJawabanBenar").ToString.Equals(dt.Rows(0)("NoJawaban").ToString), True, False)

        winAddEditPaketSoalGroupDtlJawaban.Title = "Edit Jawaban"
        winAddEditPaketSoalGroupDtlJawaban.Icon = Icon.NoteEdit
        winAddEditPaketSoalGroupDtlJawaban.Show()
    End Sub
#End Region
#End Region


#Region "Window Add Edit Paket Soal"
    Public Sub btnAddEditPaketSoal_Save_Click()
        If txtNamaPaketSoal_PS.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Nama paket soal belum diisi").Show()
            Return
        End If
        If nfToleransiWaktu.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Toleransi waktu belum diisi").Show()
            Return
        End If

        If Not txtNomorPaketSoal_PS.Text.Equals("") And rdoPaketSoalStatusOn.Checked Then
            If Not clsPaketSoal.cekJumlahPaketSoalGroup(txtNomorPaketSoal_PS.Text) Then
                Ext.Net.X.Msg.Alert("Alert", "Paket Soal harus memiliki minimal memiliki 1 Group Soal yang aktif, sebelum diaktifkan").Show()
                Return
            End If
        End If

        Try
            If txtNomorPaketSoal_PS.Text.Equals("") Then
                clsPaketSoal.InputPaketSoal(txtNamaPaketSoal_PS.Text, nfToleransiWaktu.Text, IIf(rdoPaketSoalStatusOn.Checked, "1", "0"), Session("UserID"), mcbPosisi)
            Else
                clsPaketSoal.EditPaketSoal(txtNomorPaketSoal_PS.Text, txtNamaPaketSoal_PS.Text, nfToleransiWaktu.Text, IIf(rdoPaketSoalStatusOn.Checked, "1", "0"), Session("UserID"), mcbPosisi)
            End If

            winAddEditPaketSoal.Close()
            RefreshGrid(4)

            Ext.Net.X.Msg.Alert("Save", "Simpan sukses !").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Failed", ex.Message + "<br />" + ex.StackTrace).Show()
            Return
        End Try
    End Sub
#End Region
#Region "Window Add Edit Paket Soal Group"
    Public Sub chbKreplin_Change(sender As Object, e As DirectEventArgs)
        If chbKreplin.Checked Then
            FldContainerJmlKolom.Show()
            FldContainernfMinimumJmlSoal.Hide()
            FldContainernfNilaiStandar.Hide()
            FldContainerchbPaketSoalGroupRandom.Hide()
        Else
            FldContainerJmlKolom.Hide()
            FldContainernfMinimumJmlSoal.Show()
            FldContainernfNilaiStandar.Show()
            FldContainerchbPaketSoalGroupRandom.Show()
        End If
    End Sub
    Public Sub btnAddNormaDtl_Click()
        FormPanelAddNormaDtl.Reset()
        winAddNormaDtl.Show()
        txtNormaDtlNama.Focus()
    End Sub
    Public Sub btnDeleteNormaDtl_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim SeqNo = e.ExtraParams("SeqNo")

        Dim HapusDataNormaDtl As List(Of String) = Session("HapusDataNormaDtl")
        HapusDataNormaDtl.Add(SeqNo)
        Session("HapusDataNormaDtl") = HapusDataNormaDtl

        tblNormaDtl = Session("tblNormaDtl")
        tblNormaDtl.AcceptChanges()

        For Each dr As DataRow In tblNormaDtl.Rows
            If dr("SeqNo") = SeqNo Then
                tblNormaDtl.Rows.Remove(dr)
                Exit For
            End If
        Next
        tblNormaDtl.AcceptChanges()
        Session("tblNormaDtl") = tblNormaDtl
        StoreNormaDtl.DataSource = tblNormaDtl
        StoreNormaDtl.DataBind()
    End Sub
    Public Sub SelectNamaPetunjuk()
        Dim rsm As RowSelectionModel = GridPanelPetunjuk.GetSelectionModel()
        If rsm.SelectedRows.Count = 1 Then
            Dim NoPetunjuk = rsm.SelectedRecordID
            CmbPetunjuk.SetValue(NoPetunjuk, NoPetunjuk, True)
        End If
        rsm.ClearSelection()
    End Sub
    Public Sub btnAddEditPaketSoalGroup_Save_Click()
        If Not chbKreplin.Checked Then
            If txtNamaPaketSoalGroup_PSG.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Nama Group soal belum diisi").Show()
                Return
            End If
            If nfMinimumJmlSoal.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Minimum Jumlah Soal belum diisi").Show()
                Return
            End If
            If nfNilaiStandar.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Nilai Standar belum diisi").Show()
                Return
            End If
            If nfWaktuPengerjaan.Text = "" Or nfWaktuPengerjaan.Text = "0" Then
                Ext.Net.X.Msg.Alert("Alert", "Waktu Pengerjaan belum diisi").Show()
                Return
            End If
            If CmbPetunjuk.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Petunjuk belum diisi").Show()
                Return
            End If

            If Not txtNomorPaketSoalGroup_PSG.Text.Equals("") And rdoPaketSoalGroupStatusOn.Checked Then
                Dim dt_MinimumJmlSoal = clsPaketSoal.cekJumlahPaketSoalGroupDtl(txtNomorPaketSoalGroup_PSG.Text)
                Dim JmlSoal = CType(dt_MinimumJmlSoal.Rows(0)("JmlSoal"), Int32)
                Dim MinimumJmlSoal = CType(dt_MinimumJmlSoal.Rows(0)("MinimumJmlSoal"), Int32)

                If JmlSoal < MinimumJmlSoal Then
                    Ext.Net.X.Msg.Alert("Alert", "Paket Soal belum memenuhi minimum jumlah pertanyaan: " + JmlSoal.ToString + "/" + MinimumJmlSoal.ToString).Show()
                    Return
                End If
            End If
            If Not txtNomorPaketSoalGroup_PSG.Text.Equals("") And Not rdoPaketSoalGroupStatusOn.Checked Then
                If clsPaketSoal.cekStatusPaketSoal(txtNomorPaketSoal_PSG.Text) Then
                    Ext.Net.X.Msg.Alert("Alert", "Paket Soal harus Nonaktif sebelum me-nonaktifkan Group Soal").Show()
                    Return
                End If
            End If

            Try
                clsPaketSoal.ModifyPaketSoalGroup(txtNomorPaketSoal_PSG.Text, IIf(txtNomorPaketSoalGroup_PSG.Text.Equals(""), 0, txtNomorPaketSoalGroup_PSG.Text),
                                                  txtNamaPaketSoalGroup_PSG.Text, nfMinimumJmlSoal.Text, nfNilaiStandar.Text, nfWaktuPengerjaan.Text, CmbPetunjuk.Text,
                                                  IIf(chbPaketSoalGroupIsPrioritas.Checked, "1", "0"), IIf(chbPaketSoalGroupRandom.Checked, "1", "0"), IIf(rdoPaketSoalGroupStatusOn.Checked, "1", "0"), Session("UserID"),
                                                  Session("tblNormaDtl"), Session("HapusDataNormaDtl"))
                winAddEditPaketSoalGroup.Close()
                RefreshGrid(3)

                Ext.Net.X.Msg.Alert("Save", "Simpan sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        Else
            If txtNamaPaketSoalGroup_PSG.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Nama Group soal belum diisi").Show()
                Return
            End If
            If nfJmlKolom.Text = "" Or nfJmlKolom.Text = "0" Then
                Ext.Net.X.Msg.Alert("Alert", "Jumlah kolom belum diisi").Show()
                Return
            End If
            If nfWaktuPengerjaan.Text = "" Or nfWaktuPengerjaan.Text = "0" Then
                Ext.Net.X.Msg.Alert("Alert", "Waktu Pengerjaan belum diisi").Show()
                Return
            End If
            If CmbPetunjuk.Text = "" Then
                Ext.Net.X.Msg.Alert("Alert", "Petunjuk belum diisi").Show()
                Return
            End If

            If Not txtNomorPaketSoalGroup_PSG.Text.Equals("") And Not rdoPaketSoalGroupStatusOff.Checked Then
                If clsPaketSoal.cekStatusPaketSoal(txtNomorPaketSoal_PSG.Text) Then
                    Ext.Net.X.Msg.Alert("Alert", "Paket Soal harus Nonaktif sebelum me-nonaktifkan Group Soal").Show()
                    Return
                End If
            End If
            Try
                clsPaketSoal.ModifyPaketSoalGroupKreplin(txtNomorPaketSoal_PSG.Text, IIf(txtNomorPaketSoalGroup_PSG.Text.Equals(""), 0, txtNomorPaketSoalGroup_PSG.Text),
                                                         txtNamaPaketSoalGroup_PSG.Text, nfJmlKolom.Text, nfWaktuPengerjaan.Text, CmbPetunjuk.Text,
                                                         IIf(chbPaketSoalGroupIsPrioritas.Checked, "1", "0"), IIf(rdoPaketSoalGroupStatusOn.Checked, "1", "0"), Session("UserID"),
                                                         Session("tblNormaDtl"), Session("HapusDataNormaDtl"))
                winAddEditPaketSoalGroup.Close()
                RefreshGrid(3)

                Ext.Net.X.Msg.Alert("Save", "Simpan sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            End Try
        End If
    End Sub
#End Region
#Region "Window Add Norma Dtl"
    Public Sub btnAddNormaDtl_Save_Click()
        If txtNormaDtlNama.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Nama belum diisi").Show()
            Return
        End If
        If nfNormaDtlBatasAtas.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Batas Atas belum diisi").Show()
            Return
        End If
        If nfNormaDtlBatasBawah.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Batas Bawah belum diisi").Show()
            Return
        End If
        If CInt(nfNormaDtlBatasBawah.Text) > CInt(nfNormaDtlBatasAtas.Text) Then
            Ext.Net.X.Msg.Alert("Alert", "Batas Bawah harus lebih kecil dari Batas Atas").Show()
            Return
        End If
        tblNormaDtl = Session("tblNormaDtl")
        tblNormaDtl.AcceptChanges()

        For Each dr As DataRow In tblNormaDtl.Rows
            If CInt(dr("BatasBawah")) <= CInt(nfNormaDtlBatasAtas.Text) And CInt(nfNormaDtlBatasAtas.Text) <= CInt(dr("BatasAtas")) Then
                Ext.Net.X.Msg.Alert("Alert", "Batas Atas tidak boleh bersinggungan dengan batas lain").Show()
                Return
            End If
            If CInt(dr("BatasBawah")) <= CInt(nfNormaDtlBatasBawah.Text) And CInt(nfNormaDtlBatasBawah.Text) <= CInt(dr("BatasAtas")) Then
                Ext.Net.X.Msg.Alert("Alert", "Batas Bawah tidak boleh bersinggungan dengan batas lain").Show()
                Return
            End If
        Next

        Dim newSeqNo = 0
        For Each dr As DataRow In tblNormaDtl.Rows
            If CInt(dr("SeqNo")) < newSeqNo Then
                newSeqNo = CInt(dr("SeqNo"))
            End If
        Next
        'If tblNormaDtl.Rows.Count <> 0 Then
        '    SeqNo = CInt(tblNormaDtl.Rows(tblNormaDtl.Rows.Count - 1)("SeqNo")) + 1
        'End If
        tblNormaDtl.Rows.Add(newSeqNo - 1, txtNormaDtlNama.Text, "0", nfNormaDtlBatasAtas.Text, nfNormaDtlBatasBawah.Text, Session("UserID").ToString, DateTime.Now)
        tblNormaDtl.AcceptChanges()

        Session("tblNormaDtl") = tblNormaDtl
        StoreNormaDtl.DataSource = tblNormaDtl
        StoreNormaDtl.DataBind()
        winAddNormaDtl.Hide()
    End Sub
#End Region
#Region "Window Add Edit Paket Soal Group Dtl"
    Public Sub RadioTipeMedia_PSGD_Change(ByVal sender As Object, ByVal e As DirectEventArgs)
        If RadioTipeMediaN_PSGD.Checked Then
            TxtMediaNDtl.Show()
            TxtMediaLDtl.Hide()
            FU_MediaPaketSoalGroupDtl.Hide()
            TampilanFilePaketSoalGroupDtl.Hide()
        ElseIf RadioTipeMediaL_PSGD.Checked Then
            TxtMediaNDtl.Hide()
            TxtMediaLDtl.Show()
            FU_MediaPaketSoalGroupDtl.Hide()
            TampilanFilePaketSoalGroupDtl.Hide()
        Else
            TxtMediaNDtl.Hide()
            TxtMediaLDtl.Hide()
            FU_MediaPaketSoalGroupDtl.Show()
            TampilanFilePaketSoalGroupDtl.Show()
        End If
    End Sub
    Protected Sub UploadFileDtl()
        If FU_MediaPaketSoalGroupDtl.HasFile Then
            hdnFotoDtl.Value = "Y"

            Dim filePath As String
            Dim filename As String = Path.GetFileName(FU_MediaPaketSoalGroupDtl.PostedFile.FileName)
            Dim extension As String = Path.GetExtension(filename)

            If FU_MediaPaketSoalGroupDtl.PostedFile.FileName.Contains("%") Or FU_MediaPaketSoalGroupDtl.PostedFile.FileName.Contains("&") Then
                Ext.Net.X.Msg.Alert("Alert", "Nama File tidak boleh ada karakter '&' atau '%'").Show()
                Return
            End If
            If (FU_MediaPaketSoalGroupDtl.PostedFile.ContentLength / 3072) > 3072 Then
                Ext.Net.X.Msg.Alert("Information", "Maksimal 3 MB").Show()
                Return
                Exit Sub
            End If
            If InStr(".jpg.jpeg.png.JPG.JPEG.PNG", extension) = 0 Then
                Ext.Net.X.Msg.Alert("Information", "Pastikan type file jpg,jpeg,png").Show()
                Return
                Exit Sub
            End If

            Dim getPathParrent As String = ConfigurationManager.AppSettings("TempPhoto").ToString
            If Right(getPathParrent, 1) <> "\" Then
                getPathParrent += "\"
            End If
            filePath = getPathParrent & (Session("UserID") + Date.Now.ToString("yyyyMMddHHmmss") + extension)
            FU_MediaPaketSoalGroupDtl.PostedFile.SaveAs(filePath)
            imgPreviewDtl.ImageUrl = "ViewAttachment.ashx?MaxHeight=200&FileName=" & filePath
        Else
            hdnFotoDtl.Value = "N"
        End If
    End Sub
    Public Sub btnAddEditPaketSoalGroupDtl_Save_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        If txtJudulPertanyaan.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Judul Pertanyaan belum diisi").Show()
            Return
        End If
        If e.ExtraParams("DeskripsiPertanyaan") = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Deskripsi Pertanyaan belum diisi").Show()
            Return
        End If

        If RadioTipeMediaU_PSGD.Checked And hdnFotoDtl.Value.ToString.Equals("N") Then
            Ext.Net.X.Msg.Alert("Alert", "Media belum dipilih").Show()
            Return
        End If

        If Not txtNomorPaketSoalGroupDtl_PSGD.Text.Equals("") And chbPaketSoalGroupDtlStatusOn.Checked Then
            Dim dt_JmlJwb = clsPaketSoal.cekJumlahPaketSoalGroupDtlJawaban(txtNomorPaketSoalGroupDtl_PSGD.Text)
            Dim JmlJwb = CType(dt_JmlJwb.Rows(0)("JmlJwb"), Int32)

            If JmlJwb < 2 Then
                Ext.Net.X.Msg.Alert("Alert", "Pertanyaan harus memiliki minimal memiliki 2 Pilihan").Show()
                Return
            End If
        End If
        If Not txtNomorPaketSoalGroupDtl_PSGD.Text.Equals("") And Not chbPaketSoalGroupDtlStatusOn.Checked Then
            If clsPaketSoal.cekStatusPaketSoalGroup(txtNomorPaketSoalGroup_PSGD.Text) Then
                Ext.Net.X.Msg.Alert("Alert", "Group Soal harus Nonaktif sebelum me-nonaktifkan Detail Soal").Show()
                Return
            End If
        End If

        Try
            Dim newNoUrut = (CType(clsPaketSoal.getLastNoUrutPaketSoalGroupDtl(txtNomorPaketSoal_PSGD.Text, txtNomorPaketSoalGroup_PSGD.Text), Int64) + 1).ToString
            Dim TipeMedia = IIf(RadioTipeMediaL_PSGD.Checked, "YOUTUBE", IIf(RadioTipeMediaN_PSGD.Checked, "NOMEDIA", ""))
            Dim MediaFileName = ""
            If hdnFotoDtl.Value.ToString.Equals("S") Then
                MediaFileName = hdnFotoDtlFName.Value.ToString
                TipeMedia = Path.GetExtension(MediaFileName)
                TipeMedia = TipeMedia.Substring(1).ToUpper()
            End If
            Dim Media = IIf(RadioTipeMediaL_PSGD.Checked, TxtMediaLDtl.Text, "")
            clsPaketSoal.ModifyPaketSoalGroupDtl(IIf(txtNomorPaketSoalGroupDtl_PSGD.Text.Equals(""), 0, txtNomorPaketSoalGroupDtl_PSGD.Text), txtNomorPaketSoal_PSGD.Text,
                                                txtNomorPaketSoalGroup_PSGD.Text, newNoUrut, txtJudulPertanyaan.Text, e.ExtraParams("DeskripsiPertanyaan"),
                                                IIf(chbPaketSoalGroupDtlIsDownload.Checked, "1", "0"), IIf(chbPaketSoalGroupDtlStatusOn.Checked, "1", "0"),
                                                MediaFileName, Media, TipeMedia, Session("UserID"), FU_MediaPaketSoalGroupDtl)
            winAddEditPaketSoalGroupDtl.Close()
            RefreshGrid(2)

            Ext.Net.X.Msg.Alert("Save", "Simpan sukses !").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
        End Try
    End Sub
#End Region
#Region "Window Add Edit Paket Soal Group Dtl Jawaban"
    Public Sub RadioTipeMedia_PSGDJ_Change(ByVal sender As Object, ByVal e As DirectEventArgs)
        If RadioTipeMediaN_PSGDJ.Checked Then
            TxtMediaNDtlJawaban.Show()
            TxtMediaLDtlJawaban.Hide()
            FU_MediaPaketSoalGroupDtlJawaban.Hide()
            TampilanFilePaketSoalGroupDtlJawaban.Hide()
        ElseIf RadioTipeMediaL_PSGDJ.Checked Then
            TxtMediaNDtlJawaban.Hide()
            TxtMediaLDtlJawaban.Show()
            FU_MediaPaketSoalGroupDtlJawaban.Hide()
            TampilanFilePaketSoalGroupDtlJawaban.Hide()
        Else
            TxtMediaNDtlJawaban.Hide()
            TxtMediaLDtlJawaban.Hide()
            FU_MediaPaketSoalGroupDtlJawaban.Show()
            TampilanFilePaketSoalGroupDtlJawaban.Show()
        End If
    End Sub
    Protected Sub UploadFileDtlJawaban()
        If FU_MediaPaketSoalGroupDtlJawaban.HasFile Then
            hdnFotoDtlJawaban.Value = "Y"

            Dim filePath As String
            Dim filename As String = Path.GetFileName(FU_MediaPaketSoalGroupDtlJawaban.PostedFile.FileName)
            Dim extension As String = Path.GetExtension(filename)

            If FU_MediaPaketSoalGroupDtlJawaban.PostedFile.FileName.Contains("%") Or FU_MediaPaketSoalGroupDtlJawaban.PostedFile.FileName.Contains("&") Then
                Ext.Net.X.Msg.Alert("Alert", "Nama File tidak boleh ada karakter '&' atau '%'").Show()
                Return
            End If
            If (FU_MediaPaketSoalGroupDtlJawaban.PostedFile.ContentLength / 3072) > 3072 Then
                Ext.Net.X.Msg.Alert("Information", "Maksimal 3 MB").Show()
                Return
                Exit Sub
            End If
            If InStr(".jpg.jpeg.png.JPG.JPEG.PNG", extension) = 0 Then
                Ext.Net.X.Msg.Alert("Information", "Pastikan type file jpg,jpeg,png").Show()
                Return
                Exit Sub
            End If

            Dim getPathParrent As String = ConfigurationManager.AppSettings("TempPhoto").ToString
            If Right(getPathParrent, 1) <> "\" Then
                getPathParrent += "\"
            End If
            filePath = getPathParrent & (Session("UserID") + Date.Now.ToString("yyyyMMddHHmmss") + extension)
            FU_MediaPaketSoalGroupDtlJawaban.PostedFile.SaveAs(filePath)
            imgPreviewDtlJawaban.ImageUrl = "ViewAttachment.ashx?MaxHeight=200&FileName=" & filePath
        Else
            hdnFotoDtlJawaban.Value = "N"
        End If
    End Sub
    Public Sub btnAddEditPaketSoalGroupDtlJawaban_Save_Click()
        If txtJawaban.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Jawaban belum diisi").Show()
            Return
        End If

        If RadioTipeMediaU_PSGDJ.Checked And hdnFotoDtlJawaban.Value.ToString.Equals("N") Then
            Ext.Net.X.Msg.Alert("Alert", "Media belum dipilih").Show()
            Return
        End If
        If nfPoinJawaban.Text = "" Then
            Ext.Net.X.Msg.Alert("Alert", "Jumlah poin belum diisi").Show()
            Return
        End If

        Try
            Dim newNoJawaban = (CType(clsPaketSoal.getLastNoJawabanPaketSoalGroupDtlJawaban(txtNomorPaketSoal_PSGDJ.Text, txtNomorPaketSoalGroup_PSGDJ.Text, txtNomorUrut_PSGDJ.Text), Int64) + 1).ToString
            Dim TipeMedia As String = IIf(RadioTipeMediaL_PSGDJ.Checked, "YOUTUBE", IIf(RadioTipeMediaN_PSGDJ.Checked, "NOMEDIA", ""))
            Dim MediaFileName = ""
            If hdnFotoDtlJawaban.Value.ToString.Equals("S") Then
                MediaFileName = hdnFotoDtlJawabanFName.Value.ToString
                TipeMedia = Path.GetExtension(MediaFileName)
                TipeMedia = TipeMedia.Substring(1).ToUpper()
            End If
            Dim Media = IIf(RadioTipeMediaL_PSGDJ.Checked, TxtMediaLDtlJawaban.Text, "")
            clsPaketSoal.ModifyPaketSoalGroupDtlJawaban(txtNomorPaketSoal_PSGDJ.Text, txtNomorPaketSoalGroup_PSGDJ.Text, txtNomorUrut_PSGDJ.Text,
                                                       IIf(txtNomorJawaban.Text.Equals(""), newNoJawaban, txtNomorJawaban.Text),
                                                       txtJawaban.Text, nfPoinJawaban.Text, MediaFileName,
                                                       Media, TipeMedia, txtTextMedia_PSGDJ.Text, Session("UserID"), FU_MediaPaketSoalGroupDtlJawaban)
            winAddEditPaketSoalGroupDtlJawaban.Close()
            RefreshGrid(1)

            Ext.Net.X.Msg.Alert("Save", "Simpan sukses !").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
        End Try
    End Sub
#End Region

#Region "Window Import Paket Soal Group Dtl"
    Protected Sub btnImportPaketSoalGroupDtl_Import_Click()
        If FU_ImportPaketSoalGroupDtl.HasFile = True Then
            Dim namafile As String = FU_ImportPaketSoalGroupDtl.PostedFile.FileName
            Dim ds As DataSet = getdsFromFU(FU_ImportPaketSoalGroupDtl, namafile)
            If IsNothing(ds) Then
                Exit Sub
            End If

            Dim dt = ds.Tables(0)

            Try
                If Not dt.Columns.Contains("NoPaket") Then
                    Throw New Exception("Kolom NoPaket tidak ada.")
                End If
                If Not dt.Columns.Contains("NoGroup") Then
                    Throw New Exception("Kolom NoGroup tidak ada.")
                End If
                If Not dt.Columns.Contains("Judul") Then
                    Throw New Exception("Kolom Judul tidak ada.")
                End If
                If Not dt.Columns.Contains("Deskripsi") Then
                    Throw New Exception("Kolom Deskripsi tidak ada.")
                End If

                Dim row = 1
                Dim endtbl As Boolean = False
                For Each dr As DataRow In dt.Rows
                    If endtbl Then
                        Exit For
                    End If
                    row += 1

                    For Each col As DataColumn In dt.Columns
                        If IsDBNull(dr(col.ColumnName)) Then
                            endtbl = True
                            Continue For
                        End If

                        If col.ColumnName = "NoPaket" Or col.ColumnName = "NoGroup" Or col.ColumnName = "Judul" Or
                        col.ColumnName = "Deskripsi" Then
                            If IsDBNull(dr(col.ColumnName)) Then
                                Throw New Exception("Baris: " + row.ToString + " Kolom: '" & col.ColumnName & "' Tidak boleh kosong.")
                            End If

                            If String.IsNullOrWhiteSpace(dr(col.ColumnName)) Then
                                Throw New Exception("Baris: " + row.ToString + " Kolom: '" & col.ColumnName & "' Tidak boleh kosong.")
                            End If
                        End If

                        If col.DataType = GetType(Integer) Or col.DataType = GetType(String) Then
                            dr(col.ColumnName) = Trim(dr(col.ColumnName).ToString)
                        End If
                    Next
                Next

                For Each dr As DataRow In dt.Rows
                    If IsDBNull(dr("NoPaket")) Then
                        Exit For
                    End If
                    Dim newNoUrut = (CType(clsPaketSoal.getLastNoUrutPaketSoalGroupDtl(dr("NoPaket").ToString, dr("NoGroup").ToString), Int64) + 1).ToString
                    clsPaketSoal.ModifyPaketSoalGroupDtl(0, dr("NoPaket").ToString, dr("NoGroup").ToString, newNoUrut, dr("Judul").ToString,
                                                        dr("Deskripsi").ToString, "0", "0", "", "", "NOMEDIA", Session("UserID"), FU_MediaPaketSoalGroupDtl)
                Next

                RefreshGrid(2)

                File.Delete(Server.MapPath("~/temp/" & namafile))
                winImportPaketSoalGroupDtl.Hide()

                Ext.Net.X.Msg.Alert("Save", "Import sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try

        Else
            Ext.Net.X.Msg.Alert("Save", "File belum dipilih !").Show()
        End If
    End Sub
    Protected Sub btnImportPaketSoalGroupDtl_Template_Click()
        Dim excelPackage = New ExcelPackage
        Dim excelWorksheet = excelPackage.Workbook.Worksheets.Add("Import Pertanyaan")

        excelWorksheet.Row(1).Style.Font.Bold = True
        excelWorksheet.Row(1).Style.Font.Size = 12

        excelWorksheet.Column(1).Width = 11
        excelWorksheet.Column(2).Width = 16
        excelWorksheet.Column(3).Width = 37
        excelWorksheet.Column(4).Width = 51

        excelWorksheet.Cells(1, 1, 1, 1).Value = "NoPaket"
        excelWorksheet.Cells(1, 2, 1, 2).Value = "NoGroup"
        excelWorksheet.Cells(1, 3, 1, 3).Value = "Judul"
        excelWorksheet.Cells(1, 4, 1, 4).Value = "Deskripsi"

        For iRowIndex As Integer = 2 To 4
            excelWorksheet.Cells(iRowIndex, 1, iRowIndex, 1).Value = hdnCurrNoPaket.Value
            excelWorksheet.Cells(iRowIndex, 2, iRowIndex, 2).Value = hdnCurrNoGroup.Value
            excelWorksheet.Cells(iRowIndex, 3, iRowIndex, 3).Value = "Contoh judul soal ke-" + (iRowIndex - 1).ToString
            excelWorksheet.Cells(iRowIndex, 4, iRowIndex, 4).Value = "Contoh deskripsi soal ke-" + (iRowIndex - 1).ToString
        Next
        excelWorksheet.Cells(2, 1, 4, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right

        Dim stream As MemoryStream = New MemoryStream(excelPackage.GetAsByteArray())

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=TemplateImportPaketSoalGroupDtl.xlsx")
        Response.AddHeader("content-length", stream.ToArray().Length)

        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)
        Response.Flush()
        Response.Close()
    End Sub
#End Region
#Region "Window Import Paket Soal Group Dtl Jawaban"
    Protected Sub btnImportPaketSoalGroupDtlJawaban_Import_Click()
        If FU_ImportPaketSoalGroupDtlJawaban.HasFile = True Then
            Dim namafile As String = FU_ImportPaketSoalGroupDtlJawaban.PostedFile.FileName
            Dim ds As DataSet = getdsFromFU(FU_ImportPaketSoalGroupDtlJawaban, namafile)
            If IsNothing(ds) Then
                Exit Sub
            End If

            Dim dt = ds.Tables(0)

            Try
                If Not dt.Columns.Contains("NoPaket") Then
                    Throw New Exception("Kolom NoPaket tidak ada.")
                End If
                If Not dt.Columns.Contains("NoGroup") Then
                    Throw New Exception("Kolom NoGroup tidak ada.")
                End If
                If Not dt.Columns.Contains("NoSoal") Then
                    Throw New Exception("Kolom NoSoal tidak ada.")
                End If
                If Not dt.Columns.Contains("Jawaban") Then
                    Throw New Exception("Kolom Jawaban tidak ada.")
                End If
                If Not dt.Columns.Contains("Poin") Then
                    Throw New Exception("Kolom Value tidak ada.")
                End If

                Dim row = 1
                Dim endtbl As Boolean = False
                For Each dr As DataRow In dt.Rows
                    If endtbl Then
                        Exit For
                    End If
                    row += 1

                    For Each col As DataColumn In dt.Columns
                        If IsDBNull(dr(col.ColumnName)) Then
                            endtbl = True
                            Continue For
                        End If

                        If col.ColumnName = "NoPaket" Or col.ColumnName = "NoGroup" Or col.ColumnName = "NoSoal" Or
                        col.ColumnName = "Jawaban" Or col.ColumnName = "Poin" Then
                            If IsDBNull(dr(col.ColumnName)) Then
                                Throw New Exception("Baris: " + row.ToString + " Kolom: '" & col.ColumnName & "' Tidak boleh kosong.")
                            End If

                            If String.IsNullOrWhiteSpace(dr(col.ColumnName)) Then
                                Throw New Exception("Baris: " + row.ToString + " Kolom: '" & col.ColumnName & "' Tidak boleh kosong.")
                            End If
                        End If

                        If col.DataType = GetType(String) Then
                            dr(col.ColumnName) = Trim(dr(col.ColumnName).ToString)
                        End If
                        If (col.ColumnName = "NoPaket" Or col.ColumnName = "NoGroup" Or col.ColumnName = "NoSoal" Or col.ColumnName = "Poin") And Not IsNumeric(dr(col.ColumnName)) Then
                            Throw New Exception("Baris: " + row.ToString + " Kolom: '" & col.ColumnName & "' Harus diisi angka.")
                        End If
                    Next
                Next

                For Each dr As DataRow In dt.Rows
                    If IsDBNull(dr("NoSoal")) Then
                        Exit For
                    End If
                    Dim NoUrut = clsPaketSoal.getNoUrutDtl(dr("NoSoal"))
                    Dim newNoJawaban = (clsPaketSoal.getLastNoJawabanPaketSoalGroupDtlJawaban(dr("NoPaket").ToString, dr("NoGroup").ToString, NoUrut.ToString) + 1).ToString
                    clsPaketSoal.ModifyPaketSoalGroupDtlJawaban(dr("NoPaket").ToString, dr("NoGroup").ToString, NoUrut.ToString,
                                                               newNoJawaban, dr("Jawaban").ToString, dr("Poin").ToString, "", "",
                                                               "NOMEDIA", "", Session("UserID"), Nothing)
                Next

                RefreshGrid(1)

                File.Delete(Server.MapPath("~/temp/" & namafile))
                WinImportPaketSoalGroupDtlJawaban.Hide()

                Ext.Net.X.Msg.Alert("Save", "Import sukses !").Show()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try

        Else
            Ext.Net.X.Msg.Alert("Failed", "File belum dipilih !").Show()
        End If
    End Sub
    Protected Sub btnImportPaketSoalGroupDtlJawaban_Template_Click()
        Dim excelPackage = New ExcelPackage
        Dim excelWorksheet = excelPackage.Workbook.Worksheets.Add("Import Jawaban")

        excelWorksheet.Row(1).Style.Font.Bold = True
        excelWorksheet.Row(1).Style.Font.Size = 12

        excelWorksheet.Column(1).Width = 11
        excelWorksheet.Column(2).Width = 16
        excelWorksheet.Column(3).Width = 16
        excelWorksheet.Column(4).Width = 37
        excelWorksheet.Column(5).Width = 11

        excelWorksheet.Cells(1, 1, 1, 1).Value = "NoPaket"
        excelWorksheet.Cells(1, 2, 1, 2).Value = "NoGroup"
        excelWorksheet.Cells(1, 3, 1, 3).Value = "NoSoal"
        excelWorksheet.Cells(1, 4, 1, 4).Value = "Jawaban"
        excelWorksheet.Cells(1, 5, 1, 5).Value = "Poin"

        For iRowIndex As Integer = 2 To 4
            excelWorksheet.Cells(iRowIndex, 1, iRowIndex, 1).Value = hdnCurrNoPaket.Value
            excelWorksheet.Cells(iRowIndex, 2, iRowIndex, 2).Value = hdnCurrNoGroup.Value
            excelWorksheet.Cells(iRowIndex, 3, iRowIndex, 3).Value = hdnCurrNoDtl.Value
            excelWorksheet.Cells(iRowIndex, 4, iRowIndex, 4).Value = "Contoh jawaban ke-" + (iRowIndex - 1).ToString
            excelWorksheet.Cells(iRowIndex, 5, iRowIndex, 5).Value = IIf(iRowIndex = 3, "10", "0")
        Next
        excelWorksheet.Cells(2, 1, 4, 3).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right

        Dim stream As MemoryStream = New MemoryStream(excelPackage.GetAsByteArray())

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=TemplateImportPaketSoalGroupDtlJawaban.xlsx")
        Response.AddHeader("content-length", stream.ToArray().Length)

        Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)
        Response.Flush()
        Response.Close()
    End Sub
#End Region

#Region "Window Copy Paket Soal Group Dtl"
    Protected Sub btnCopyPaketSoalGroupDtl_Save_Click()
        Dim rsmDtl As RowSelectionModel = GridPanelPaketSoalGroupDtl.GetSelectionModel()
        Dim rsmGroup As RowSelectionModel = GridPanelCopyPaketSoalGroupDtl.GetSelectionModel()
        If rsmDtl.SelectedRows.Count = 0 Then
            Ext.Net.X.Msg.Alert("Failed", "Pilih Pertanyaan").Show()
            Exit Sub
        End If
        If rsmGroup.SelectedRows.Count <> 1 Then
            Ext.Net.X.Msg.Alert("Failed", "Pilih 1 Group Soal").Show()
            Exit Sub
        End If
        If rsmGroup.SelectedRows(0).RecordID.ToString.Equals(hdnCurrNoGroup.Value) Then
            Ext.Net.X.Msg.Alert("Failed", "Tidak dapat copy pertanyaan ke Group soal yang sama").Show()
            Exit Sub
        End If
        If clsPaketSoal.cekStatusPaketSoalGroupDtlByNoGroup(rsmGroup.SelectedRows(0).RecordID.ToString) Then
            Ext.Net.X.Msg.Alert("Failed", "Group soal tujuan sedang aktif, tidak bisa copy pertanyaan ke GroupSoal aktif").Show()
            Exit Sub
        End If

        Try
            For Each rAsal As SelectedRow In rsmDtl.SelectedRows
                For Each rTujuan As SelectedRow In rsmGroup.SelectedRows
                    Using oSqlHelper As New clsSQLHelper
                        oSqlHelper.CommandText = "ADVPSIKOTEST..PaketSoalGroupDtlCopy"
                        oSqlHelper.CommandType = CommandType.StoredProcedure
                        oSqlHelper.AddParameter("@SeqNo", CType(rAsal.RecordID.ToString, Int64), SqlDbType.BigInt)
                        oSqlHelper.AddParameter("@TujuanNoGroup", CType(rTujuan.RecordID.ToString, Int64), SqlDbType.BigInt)
                        oSqlHelper.AddParameter("@User", Session("UserID"), SqlDbType.VarChar)
                        Dim Dt As DataTable = oSqlHelper.ExecuteDataTable

                        Dim getPathParrent As String = ConfigurationManager.AppSettings("ESSMasterPath").ToString
                        If Right(getPathParrent, 1) <> "\" Then
                            getPathParrent += "\"
                        End If

                        Dim dtAsal As DataTable
                        Using iSqlHelper As New clsSQLHelper
                            iSqlHelper.CommandText = "SELECT A.MediaFileName MediaFileNameSoal, B.MediaFileName " +
                                                     "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl A  LEFT JOIN ADVPSIKOTEST..MS_PaketSoalGroupDtlJawaban B ON A.NoPaket = B.NoPaket AND A.NoGroup = B.NoGroup AND A.NoUrut = B.NoUrut " +
                                                     "WHERE A.NoPaket = " & Dt.Rows(0)("AsalNoPaket").ToString & " AND A.NoGroup = " & Dt.Rows(0)("AsalNoGroup").ToString & " AND A.NoUrut = " & Dt.Rows(0)("AsalNoUrut").ToString
                            iSqlHelper.CommandType = CommandType.Text
                            dtAsal = iSqlHelper.ExecuteDataTable
                        End Using

                        Dim DirFrom = ""
                        Dim DirTo = ""

                        DirFrom = getPathParrent & "/" & Dt.Rows(0)("AsalNoPaket").ToString & "/" & Dt.Rows(0)("AsalNoGroup").ToString & "/" & dtAsal.Rows(0)("MediaFileNameSoal").ToString
                        DirTo = getPathParrent & "/" & Dt.Rows(0)("TujuanNoPaket").ToString & "/" & Dt.Rows(0)("TujuanNoGroup").ToString & "/" & Dt.Rows(0)("MediaFileNameSoal").ToString

                        If System.IO.File.Exists(DirFrom) = True And System.IO.File.Exists(DirTo) = False Then
                            Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(DirFrom, DirTo)
                        End If

                        For i As Integer = 0 To dtAsal.Rows.Count - 1
                            DirFrom = getPathParrent & "/" & Dt.Rows(i)("AsalNoPaket").ToString & "/" & Dt.Rows(i)("AsalNoGroup").ToString & "/" & dtAsal.Rows(i)("MediaFileName").ToString
                            DirTo = getPathParrent & "/" & Dt.Rows(i)("TujuanNoPaket").ToString & "/" & Dt.Rows(i)("TujuanNoGroup").ToString & "/" & Dt.Rows(i)("MediaFileName").ToString

                            If System.IO.File.Exists(DirFrom) = True And System.IO.File.Exists(DirTo) = False Then
                                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(DirFrom, DirTo)
                            End If
                        Next

                    End Using
                Next
            Next
            WinCopyPaketSoalGroupDtl.Close()
            RefreshGrid(2)

            Ext.Net.X.Msg.Alert("Save", "Copy sukses !").Show()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
            Return
        End Try
    End Sub
#End Region

#Region "Private Method"
    Protected Function getdsFromFU(FileUp As FileUploadField, namafile As String) As DataSet
        If Not namafile.EndsWith(".xls") And Not namafile.EndsWith(".xlsx") Then
            Ext.Net.X.Msg.Alert("Import Failed", "Format file hanya mendukung .xls /.xlsx").Show()
            Return Nothing
        End If

        If namafile.Contains("%") = True Or namafile.Contains("&") Then
            Ext.Net.X.Msg.Alert("Alert", "Nama File tidak boleh ada karakter '&' atau '%' ").Show()
            Return Nothing
        End If

        If Not Directory.Exists(Server.MapPath("~/temp/")) Then
            Directory.CreateDirectory(Server.MapPath("~/temp/"))
        End If
        Dim filepath As String = Server.MapPath("~/temp/" & namafile)
        FileUp.PostedFile.SaveAs(filepath)

        Dim fileIOStream As FileStream = File.OpenRead(filepath)
        Dim ds As DataSet

        If namafile.EndsWith(".xls") Then
            Using excelReader As IExcelDataReader = ExcelReaderFactory.CreateBinaryReader(fileIOStream)
                excelReader.IsFirstRowAsColumnNames = True
                ds = excelReader.AsDataSet(True)
            End Using
        Else
            Using excelReader As IExcelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileIOStream)
                excelReader.IsFirstRowAsColumnNames = True
                ds = excelReader.AsDataSet(True)
            End Using
        End If
        Return ds
    End Function
#End Region

End Class
