
Imports System.Data
Imports Ext.Net

Partial Class MASTER_Petunjuk
    Inherits System.Web.UI.Page
    Public Shared soloAssign As Boolean = False
    Public Shared multiAssign As Boolean = False

    Public Shared soloAssignNoPeserta As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CheckUserPermission()

            'Filter Peserta
            DF_FilterTimeInput_From.Text = New DateTime(Date.Now.Year, 1, 1).ToString("MM/dd/yyyy")
            DF_FilterTimeInput_To.Text = New DateTime(Date.Now.Year + 1, 1, 1).AddDays(-1).ToString("MM/dd/yyyy")
            Ext.Net.X.AddScript("Loaded();")
        End If
    End Sub

    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub
#Region "MainUI"
    Protected Sub btnFilterPeserta_Click()
        hdnFilterPetunjuk.Value = ""
        If chbFilterTimeInput.Checked Then
            Dim dtTo As Date = DF_FilterTimeInput_To.Text
            dtTo = dtTo.AddHours(23).AddMinutes(59).AddSeconds(59)
            Dim a = dtTo.ToString("M/dd/yyyy hh:mm:ss tt")
            hdnFilterPetunjuk.Value = " '" & DF_FilterTimeInput_From.Text & "' <= TimeInput AND TimeInput <= '" & a & "' AND "
        Else
            hdnFilterPetunjuk.Value += " 1=1 AND "
        End If
        hdnFilterPetunjuk.Value += IIf(CmbFilterStatusPeserta.Value.Equals("0"), " bAktif = 'Nonaktif' ", IIf(CmbFilterStatusPeserta.Value.Equals("1"), " bAktif = 'Aktif' ", " 1=1 "))
        GridPanelPetunjuk.GetStore.Reload()
    End Sub
    Protected Sub btnAddPetunjuk_Click()
        FormPanelAddEditPetunjuk.Reset()

        winAddEditPetunjuk.Title = "Add Petunjuk"
        winAddEditPetunjuk.Icon = Icon.NoteAdd
        winAddEditPetunjuk.Show()

        Ext.Net.X.AddScript("CKEDITOR.instances['txtKeteranganPetunjuk'].setData('');")
        Ext.Net.X.AddScript("ResetZIndex();")
    End Sub
    Protected Sub btnEditPetunjuk_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        FormPanelAddEditPetunjuk.Reset()
        Dim dt = clsPaketSoal.getPetunjuk(e.ExtraParams("SeqNo"))

        txtNomorPetunjuk.Text = dt.Rows(0)("SeqNo")
        chbPetunjukStatus.Checked = IIf(dt.Rows(0)("bAktif").ToString.Equals("True"), True, False)

        winAddEditPetunjuk.Title = "Edit Petunjuk"
        winAddEditPetunjuk.Icon = Icon.NoteEdit
        winAddEditPetunjuk.Show()

        Ext.Net.X.AddScript("CKEDITOR.instances['txtKeteranganPetunjuk'].setData('" + Replace(dt.Rows(0)("Keterangan"), Chr(10).ToString, "") + "');")
        Ext.Net.X.AddScript("ResetZIndex();")
    End Sub
#End Region
#Region "Window Add Edit Petunjuk"
    Public Sub btnAddEditPetunjuk_Save_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        Dim KeteranganPetunjuk = e.ExtraParams("KeteranganPetunjuk")
        If KeteranganPetunjuk.Equals("") Then
            Ext.Net.X.Msg.Alert("Alert", "Keterangan belum diisi").Show()
            Exit Sub
        End If
        clsPaketSoal.ModifyPetunjuk(IIf(txtNomorPetunjuk.Text.Equals(""), 0, txtNomorPetunjuk.Text), KeteranganPetunjuk,
                                    IIf(chbPetunjukStatus.Checked, "1", "0"), Session("UserID"))
        winAddEditPetunjuk.Close()
        StorePetunjuk.Reload()
    End Sub
#End Region
End Class
