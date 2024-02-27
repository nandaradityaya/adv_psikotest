Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports System.Runtime.CompilerServices

Imports Ext.Net
Imports System.IO
Imports Excel

Partial Class MS_User
    Inherits System.Web.UI.Page
    Private LoginUserID As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("../SessionTimeout.aspx")
        End If
        LoginUserID = Session("UserID")
        If Not Page.IsPostBack Then
            'LoadCombo()
            'LoadLastSessionData()
        End If

    End Sub


    <WebMethod(EnableSession:=True)>
    Public Shared Function AssignStudent(id As String) As String

        Dim dict = New Dictionary(Of String, String)
        Dim JSSerialize As New JavaScriptSerializer
        Dim dt As DataTable = New DataTable
        'Dim cmdtx As DataContext = DataFactory.Initial
        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        oSqlHelper.CommandText = "SELECT * FROM MS_UserRoleAssignment WHERE LoginUserID=@LoginUserID AND LoginRoleID LIKE '%WALI%'"
        oSqlHelper.AddParameter("@LoginUserID", id, SqlDbType.Char)
        dt = oSqlHelper.ExecuteDataTable
        If dt.Rows.Count <= 0 Then
            dict.Add("status", False)
            Return JSSerialize.Serialize(dict)
        Else
            dict.Add("status", True)
            Return JSSerialize.Serialize(dict)
        End If
    End Function



    'Private Sub LoadCombo()

    '    Using oHelper As New MonyetSQLHelper

    '        oHelper.CommandType = CommandType.Text
    '        oHelper.CommandText = "Select SubportfolioCd,SubportfolioName From CM_Subportfolio Where Status='" & CConstants.CM_SUBPORTFOLIO_STATUS_ACTIVE & "'"

    '        Dim dt As DataTable = oHelper.ExecuteDataTable

    '        ddlStatus.Items.Add(New ListItem("ALL", ""))


    '        For i As Integer = 0 To dt.Rows.Count - 1
    '            ddlStatus.Items.Add(New ListItem(dt.Rows(i)("SubportfolioName").ToString, dt.Rows(i)("SubportfolioCd").ToString))
    '        Next

    '        ddlStatus.Items.Add(New ListItem("NONE", "<>"))

    '    End Using

    'End Sub

    'Private Sub LoadLastSessionData()
    '    If IsNothing(Session("_UserFilter")) Then
    '        ddlStatus.SelectedIndex = 0
    '    Else
    '        ddlStatus.SelectedValue = Session("_UserFilter").ToString
    '    End If
    'End Sub


    'Private Sub btnApplyFilter_Click(sender As Object, e As EventArgs) Handles btnApplyFilter.Click
    '    Session("_UserFilter") = ddlStatus.SelectedValue
    'End Sub

#Region "Event Handler"
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        frmImport.Reset()
        winImport.Center()
        winImport.Show()
    End Sub
    Protected Sub btnSaveImport_Click(ByVal sender As Object, ByVal e As DirectEventArgs)
        If fuplExcel.HasFile = False Then
            Ext.Net.X.Msg.Alert("Failed", "File harus diisi").Show()
            Return
        End If

        Dim exts As String = Path.GetExtension(fuplExcel.FileName)
        If Not (exts = ".xls" Or exts = ".xlsx") Then
            Ext.Net.X.Msg.Alert("Failed", "Import hanya berlaku untuk extension .xls /.xlsx").Show()
            Return
        End If
        Try
            ImportData()
            Ext.Net.X.Msg.Alert("Succcess", "Import telah berhasil.").Show()
            Response.Redirect("MS_User.aspx")
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Import Failed", ex.Message).Show()
        End Try
    End Sub
    Private Sub ImportData()
        Dim fStream As Stream = fuplExcel.PostedFile.InputStream
        Dim ds As DataSet = Nothing
        Dim dt1 As DataTable = New DataTable

        Using excelReader As IExcelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fStream)
            excelReader.IsFirstRowAsColumnNames = True
            ds = excelReader.AsDataSet(True)
        End Using

        If ds.Tables.Count = 0 Then
            Throw New Exception("Data yang di import kosong.")
        End If

        Dim dt As DataTable = ds.Tables(0)

        Dim dtCompare As DataTable = CreateExportData("1=0")

        For Each col As DataColumn In dtCompare.Columns
            'Check Columnnya 
            If Not dt.Columns.Contains(col.ColumnName) Then
                Throw New Exception("[Kolom :'" & col.ColumnName & "'] Kolom tidak ada.")
            End If
        Next


        Using oSqlHelper As New MonyetSQLHelper()
            oSqlHelper.CommandText = "TRX_SP_Insert_User"
            oSqlHelper.CommandType = CommandType.StoredProcedure
            oSqlHelper.OpenConnectionAndBeginTransaction()
            Dim i As Integer = 1
            Try
                For Each dr As DataRow In dt.Rows
                    'Flag untuk baris data
                    i += 1

                    For Each col As DataColumn In dtCompare.Columns
                        'Check Value Kosong yang wajib diisi
                        If col.ColumnName = "Login User ID" Or col.ColumnName = "Login User Name" Or col.ColumnName = "Status" _
                            Or col.ColumnName = "Login Status" Or col.ColumnName = "Login Password" Then

                            If IsDBNull(dr(col.ColumnName)) Then
                                Throw New Exception("[Kolom :'" & col.ColumnName & "'] Tidak boleh kosong.")
                            End If

                            If String.IsNullOrWhiteSpace(dr(col.ColumnName)) Then
                                Throw New Exception("[Kolom :'" & col.ColumnName & "'] Tidak boleh kosong.")
                            End If
                        End If

                        'Trim data
                        If col.DataType = GetType(Integer) Or col.DataType = GetType(String) Then
                            dr(col.ColumnName) = Trim(dr(col.ColumnName).ToString)
                        End If

                        'Check Integer
                        If col.DataType = GetType(Integer) Then
                            If Not IsNumeric(dr(col.ColumnName)) Then
                                Throw New Exception("[Kolom :'" & col.ColumnName & "'] Angka tidak valid.")
                            End If
                        End If
                    Next

                    Using oHelper As New MonyetSQLHelper()
                        dt1 = New DataTable
                        oHelper.CommandText = "SELECT * FROM MS_User WITH(NOLOCK, NOWAIT) WHERE LoginUserID='" + dr("Login User ID").ToString + "'"
                        oHelper.CommandType = CommandType.Text
                        dt1 = oHelper.ExecuteDataTable()
                        If dt1.Rows.Count > 0 Then
                            Throw New Exception("LoginUserID :'" & dr("Login User ID").ToString & "' sudah terdaftar di master!")
                        End If
                    End Using

                    Dim E64 = New Encryption64
                    Dim e_Password = E64.Encrypt(dr("Login Password"), ConfigurationManager.AppSettings("EncryptionKey").ToString)
                    oSqlHelper.ClearParameters()
                    oSqlHelper.AddParameter("@LoginUserID", dr("Login User ID"), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@LoginPassword", e_Password, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@LoginUserName", dr("Login User Name"), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@EmailAddress", IIf(Not String.IsNullOrEmpty(dr("Email Address").ToString), dr("Email Address"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@PhoneNumber", IIf(Not String.IsNullOrEmpty(dr("Phone Number").ToString), dr("Phone Number"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@Profession", IIf(Not String.IsNullOrEmpty(dr("Pekerjaan").ToString), dr("Pekerjaan"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@Address", IIf(Not String.IsNullOrEmpty(dr("Alamat").ToString), dr("Alamat"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@AndroidID", IIf(Not String.IsNullOrEmpty(dr("Android ID").ToString), dr("Android ID"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@Remarks", IIf(Not String.IsNullOrEmpty(dr("Remarks").ToString), dr("Remarks"), ""), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@IdEmployee", IIf(Not String.IsNullOrEmpty(dr("Id Karyawan").ToString), dr("Id Karyawan"), 0), SqlDbType.BigInt)
                    oSqlHelper.AddParameter("@UserAccessArticle", IIf(dr("Akses Artikel").ToString = "Inputter", "INPT", IIf(dr("Akses Artikel").ToString = "Approver", "APV", "")), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@UserInput", LoginUserID, SqlDbType.VarChar)
                    oSqlHelper.ExecuteNonQueryWithTransaction()
                Next
            Catch ex As Exception
                oSqlHelper.RollBackTransaction()
                Throw New Exception("[Baris :'" & i & "'] " & ex.Message)
            End Try
            oSqlHelper.CommitTransaction()
        End Using
    End Sub
    Private Function CreateExportData(Optional whereClause As String = "") As DataTable
        Dim sql As String
        'cmdtx = DataFactory.Initial
        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        sql = " SELECT [Login User ID] = a.LoginUserID," & vbCrLf
        sql += " [Login Password] = a.LoginPassword," & vbCrLf
        sql += " [Login User Name] = a.LoginUserName," & vbCrLf
        sql += " [Email Address] = a.EmailAddress," & vbCrLf
        sql += " [Phone Number] = a.PhoneNumber," & vbCrLf
        sql += " [Pekerjaan] = a.Profession," & vbCrLf
        sql += " [Alamat] = a.Address," & vbCrLf
        sql += " [Android ID] = a.AndroidID," & vbCrLf
        sql += " [Remarks] = a.Remarks," & vbCrLf
        sql += " [Id Karyawan] = a.IdEmployee," & vbCrLf
        sql += " [Akses Artikel] = a.UserAccessArticle" & vbCrLf
        sql += " FROM MS_User a " & vbCrLf
        oSqlHelper.CommandText = sql
        Return oSqlHelper.ExecuteDataTable
    End Function
    Public Sub BtnDownload_Click(ByVal sender As Object, e As DirectEventArgs)
        Dim getPathParrent As String = ConfigurationManager.AppSettings("TempPath").ToString
        If Right(getPathParrent, 1) <> "\" Then
            getPathParrent += "\"
        End If
        Dim DirFile As String = getPathParrent & "TemplateUser.xlsx"
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=TemplateUser.xlsx")
        Response.TransmitFile(DirFile)
        Response.End()
    End Sub
#End Region
End Class
