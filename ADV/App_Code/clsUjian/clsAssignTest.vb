Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Public Class clsAssignTest

#Region "Fill MultiCombo"
    Public Shared Function getPosisi() As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT CodePosisi = Code, NamaPosisi = Name " &
            "FROM dbo.VW_ADVHR__REC_Job"
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getBKKBranch() As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT SeqNo NoAsalRekrutan, AsalRekrutan " &
            "FROM ADVPSIKOTEST..VW_ADVHR__FPTK_Rekrutan"
            Return oHelper.ExecuteDataTable
        End Using
    End Function
#End Region

#Region "Psikotest"
    Public Shared Function getStatusPengerjaanNoPeserta(NoPeserta As String) As Object
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT TOP (1) 1 " &
            "FROM ADVPSIKOTEST.dbo.VW_MASTER_PesertaDtl " &
            "WHERE NoPeserta = '" & NoPeserta & "' AND LvlStatusPengerjaan IN(0, 1)"
            Return oHelper.ExecuteScalar
        End Using
    End Function
    Public Shared Function getNamaPaket(NoPaket As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT NamaPaket " &
            "FROM ADVPSIKOTEST..MS_PaketSoal " &
            "WHERE NoPaket=" & NoPaket

            Return oHelper.ExecuteScalar
        End Using
    End Function
    Public Shared Sub assignUjian(Batch As String, NoPeserta As String, WaktuUjian As String, NoPaket As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaDtl"

            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.SmallInt)
            Dim dt = oHelper.ExecuteDataTable()

            Dim UserID = dt.Rows(0)("NewUserID").ToString
            Dim Password = dt.Rows(0)("NewPassword").ToString

            Dim E64 = New Encryption64
            Dim e_Password = E64.Encrypt(Password, ConfigurationManager.AppSettings("EncryptionKey").ToString)

            Dim Token = "U=" & UserID & "&P=" & Password
            Dim e_Token = E64.Encrypt(Token, ConfigurationManager.AppSettings("EncryptionKey").ToString)

            Dim Url = ConfigurationManager.AppSettings("DomainName").ToString & "TRX_Ujian?Token=" & e_Token


            oHelper.ClearParameters()
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaDtl"

            oHelper.AddParameter("@Batch", Batch, SqlDbType.VarChar)
            oHelper.AddParameter("@NoPeserta", CType(NoPeserta, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoPaket", CType(NoPaket, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@UserID", UserID, SqlDbType.VarChar)
            oHelper.AddParameter("@Password", e_Password, SqlDbType.VarChar)
            oHelper.AddParameter("@Url", Url, SqlDbType.VarChar)
            oHelper.AddParameter("@WaktuUjian", WaktuUjian, SqlDbType.DateTime)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.SmallInt)
            Dim returndt = oHelper.ExecuteDataTable()
            If returndt.Rows.Count > 0 Then
                sendWAInvitaion(UserID, "P")
            End If
        End Using
    End Sub
    Public Shared Function ExportPsikotest(Filter As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                 "DECLARE @FPTK_Rekrutan TABLE (SeqNo BIGINT, AsalRekrutan VARCHAR(200)) " +
                 "INSERT @FPTK_Rekrutan " +
                 "SELECT SeqNo, AsalRekrutan FROM dbo.VW_ADVHR__FPTK_Rekrutan " +
                 "" +
                 "DECLARE @REC_Job TABLE (Code VARCHAR(10), Name VARCHAR(100)) " +
                 "INSERT @REC_Job " +
                 "SELECT Code, Name FROM dbo.VW_ADVHR__REC_Job " +
                 "" +
                 "SELECT A.NoPeserta 'Nomor Peserta' " +
                 ", B.NamaPeserta 'Nama Peserta' " +
                 ", B.NoKTP 'No KTP' " +
                 ", CASE WHEN A.Batch IS NULL OR A.Batch = '' THEN 'Non Batch' " +
                        "ELSE A.Batch " +
                   "END 'Batch' " +
                 ", A.bKirim 'Status Pesan' " +
                 ", A.StatusPengerjaan 'Status Pengerjaan' " +
                 ", A.NamaPaket 'Nama Paket' " +
                 ", A.UndangPsikotestKe 'Undang Ke' " +
                 ", B.TglLahir 'Tgl Lahir' " +
                 ", (CAST(DATEDIFF(YEAR, B.TglLahir, GETDATE()) AS VARCHAR(5)) + ' Tahun') 'Usia' " +
                 ", CASE WHEN B.JenisKelamin = 'P' THEN 'Perempuan' " +
                        "ELSE 'Laki-laki' " +
                   "END 'Jenis Kelamin' " +
                 ", B.Alamat " +
                 ", (SELECT TOP(1) PendidikanTerakhir FROM dbo.TF_ADVHR__REC_Education(B.Email)) 'Pendidikan Terakhir' " +
                 ", COALESCE((SELECT TOP(1) SIM FROM dbo.TF_ADVHR__REC_SIMOwned(B.Email)), 'TIDAK MEMILIKI SIM') 'SIM yang dimiliki' " +
                 ", B.NoHp 'No HP' " +
                 ", C.AsalRekrutan 'Lokasi Test' " +
                 ", E.Name 'Posisi Dilamar' " +
                 ", D.NamaGroup 'Nama Group' " +
                 ", ISNULL(D.NamaNormaDtl,'') + ' ' + CAST(D.NilaiGroupResult AS VARCHAR(10)) + '/' + CAST(D.NilaiStandard AS VARCHAR(10)) 'GroupRes' " +
                 ", ADVPSIKOTEST.dbo.fn_LblRekomendasi(A.UserId, 'T') 'HASIL AKHIR' " +
                 ", A.WaktuTest 'Waktu Test' " +
                 ", A.StartTest 'Waktu Dimulai' " +
                 ", A.UserInput " +
                 ", D.NoGroup " +
                 ", (SELECT COUNT(1) FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup AA WHERE AA.NoPaket IN(SELECT AB.nopaket FROM ADVPSIKOTEST.dbo.VW_MASTER_TR_PsikotestResult AB WHERE " + Filter + ")) 'MaxGroup' " +
                 ", A.TimeInput " +
                 ", A.UserId " +
            "FROM ADVPSIKOTEST..VW_MASTER_PesertaDtl A " +
            "JOIN ADVPSIKOTEST..MS_Peserta B ON A.NoPeserta = B.NoPeserta " +
            "JOIN @FPTK_Rekrutan C ON B.NoAsalRekrutan = C.seqNo " +
            "LEFT JOIN ADVPSIKOTEST.dbo.VW_MASTER_TR_PsikotestResult D ON A.UserId = D.UserId AND A.NoPaket = D.NoPaket " +
            "JOIN @REC_Job E ON B.LamarSebagai = E.Code " +
            "WHERE " + Filter + " ORDER BY A.TimeInput DESC"

            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Sub resendUjian(UserID As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaDtl"

            oHelper.AddParameter("@UserID", UserID, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 3, SqlDbType.SmallInt)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getLink(UserID As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT * " &
            "FROM ADVPSIKOTEST..MS_PesertaDtl " &
            "WHERE UserID='" & UserID & "'"

            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getUjian(UserId As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT * " &
            "FROM ADVPSIKOTEST..MS_PesertaDtl A LEFT JOIN ADVPSIKOTEST..MS_PaketSoal B ON  A.NoPaket = B.NoPaket " &
            "WHERE UserId='" & UserId & "'"

            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Sub editUjian(WaktuUjian As String, NoPaket As String, block As Boolean, UserID As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaDtl"

            oHelper.AddParameter("@NoPaket", CType(NoPaket, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@UserID", UserID, SqlDbType.VarChar)
            oHelper.AddParameter("@WaktuUjian", WaktuUjian, SqlDbType.DateTime)
            oHelper.AddParameter("@block", block, SqlDbType.Bit)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.SmallInt)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
#End Region

#Region "Cek Nilai"
    Public Shared Function getPeserta(NoPeserta As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT B.*, ISNULL(C.Name,B.LamarSebagai) LamarSebagaiJob " &
            "FROM ADVPSIKOTEST.dbo.MS_Pesertadtl A " &
            "JOIN ADVPSIKOTEST.dbo.MS_Peserta B ON A.NoPeserta = B.NoPeserta " &
            "LEFT JOIN ADVPSIKOTEST.dbo.VW_ADVHR__REC_Job C ON REPLACE(B.LamarSebagai,'0','') = REPLACE(C.Code,'0','') " &
            "WHERE A.UserId = '" & NoPeserta & "'"

            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getTipeGroupSoal(NoGroup As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT Tipe FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup WHERE NoGroup = " + NoGroup
            Return oHelper.ExecuteScalar()
        End Using
    End Function
    Public Shared Function getLblRekomendasi(UserId As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT ADVPSIKOTEST.dbo.fn_LblRekomendasi(@User,'L')"
            oHelper.AddParameter("@User", UserId, SqlDbType.VarChar)
            Return oHelper.ExecuteScalar()
        End Using
    End Function
#End Region

#Region "Interview"
    Public Shared Function getKelulusanPeserta(NoPeserta As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT COUNT(a.LblRekomendasi) " &
                                  "FROM(SELECT ADVPSIKOTEST.dbo.fn_LblRekomendasi(UserId,'L') LblRekomendasi " &
                                       "From ADVPSIKOTEST.dbo.MS_PesertaDtl " &
                                       "WHERE NoPeserta = @User) a " &
                                  "Where CHARINDEX('DISARANKAN', a.LblRekomendasi) <> 0 OR CHARINDEX('DIPERTIMBANGKAN', a.LblRekomendasi) <> 0"
            oHelper.AddParameter("@User", NoPeserta, SqlDbType.VarChar)
            Dim JmlKelulusan = oHelper.ExecuteScalar()
            If JmlKelulusan > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Sub assignInterview(Batch As String, NoPeserta As String, WaktuInterview As String, Lokasi As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaInterview"

            oHelper.AddParameter("@Batch", Batch, SqlDbType.VarChar)
            oHelper.AddParameter("@NoPeserta", CType(NoPeserta, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@WaktuInterview", WaktuInterview, SqlDbType.DateTime)
            oHelper.AddParameter("@Lokasi", Lokasi, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.SmallInt)
            Dim returndt = oHelper.ExecuteDataTable()
            If returndt.Rows.Count > 0 Then
                sendWAInvitaion(returndt.Rows(0)("SeqNo").ToString, "I")
            End If
        End Using
    End Sub
    Public Shared Sub editInterview(Batch As String, WaktuInterview As String, SeqNo As String, Lokasi As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaInterview"

            oHelper.AddParameter("@SeqNo", CInt(SeqNo), SqlDbType.BigInt)
            oHelper.AddParameter("@Batch", Batch, SqlDbType.VarChar)
            oHelper.AddParameter("@WaktuInterview", WaktuInterview, SqlDbType.DateTime)
            oHelper.AddParameter("@Lokasi", Lokasi, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.SmallInt)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getInterview(SeqNo As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT * " &
            "FROM ADVPSIKOTEST..MS_PesertaInterview " &
            "WHERE SeqNo='" & SeqNo & "'"

            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Sub resendInterview(SeqNo As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PesertaInterview"

            oHelper.AddParameter("@SeqNo", SeqNo, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.SmallInt)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function ExportInterview(Filter As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT A.NoPeserta 'Nomor Peserta' " +
                 ", B.NamaPeserta 'Nama Peserta' " +
                 ", B.NoKTP 'No KTP' " +
                 ", CASE WHEN A.Batch IS NULL OR A.Batch = '' THEN 'Non Batch' " +
                        "ELSE A.Batch " +
                   "END 'Batch' " +
                 ", A.bKirim 'Status Pesan' " +
                 ", A.StatusInterview 'Status Interview' " +
                 ", A.WaktuInterview 'Waktu Interview' " +
                 ", A.Lokasi " +
                 ", A.UndangInterviewKe 'Undang Ke' " +
                 ", B.TglLahir 'Tgl Lahir' " +
                 ", (CAST(DATEDIFF(YEAR, B.TglLahir, GETDATE()) AS VARCHAR(5)) + ' Tahun') 'Usia' " +
                 ", CASE WHEN B.JenisKelamin = 'P' THEN 'Perempuan' " +
                        "ELSE 'Laki-laki' " +
                   "END 'Jenis Kelamin' " +
                 ", B.Alamat " +
                 ", F.PendidikanTerakhir 'Pendidikan Terakhir' " +
                 ", E.SIM 'SIM yang dimiliki' " +
                 ", B.NoHp 'No HP' " +
                 ", C.AsalRekrutan 'Lokasi Test' " +
                 ", D.Name 'Posisi Dilamar' " +
                 ", A.UserInput, A.TimeInput, A.UserEdit, A.TimeEdit " &
            "FROM dbo.VW_MASTER_PesertaInterview A " &
            "JOIN dbo.MS_Peserta B ON A.NoPeserta = B.NoPeserta " &
            "JOIN dbo.VW_ADVHR__FPTK_Rekrutan C ON B.NoAsalRekrutan = C.seqNo " &
            "JOIN dbo.VW_ADVHR__REC_Job D ON B.LamarSebagai = D.Code " +
            "LEFT JOIN dbo.VW_ADVHR__REC_Biodata E ON B.Email = E.UserEmail " +
            "LEFT JOIN dbo.VW_ADVHR__REC_Education F ON B.Email = F.UserEmail " +
            "WHERE " & Filter

            Return oHelper.ExecuteDataTable
        End Using
    End Function
#End Region



#Region "Private Method"
    Public Shared Function tglIndo(tgl As Date) As String
        Dim hari = "", bulan = ""
        If tgl.DayOfWeek < 4 Then
            If tgl.DayOfWeek = 1 Then
                hari = "Senin"
            End If
            If tgl.DayOfWeek = 2 Then
                hari = "Selasa"
            End If
            If tgl.DayOfWeek = 3 Then
                hari = "Rabu"
            End If
        Else
            If tgl.DayOfWeek < 6 Then
                If tgl.DayOfWeek = 4 Then
                    hari = "Kamis"
                End If
                If tgl.DayOfWeek = 5 Then
                    hari = "Jumat"
                End If
            Else
                If tgl.DayOfWeek = 6 Then
                    hari = "Sabtu"
                End If
                If tgl.DayOfWeek = 7 Then
                    hari = "Minggu"
                End If
            End If
        End If

        If tgl.Month < 7 Then
            If tgl.Month < 4 Then
                If tgl.Month = 1 Then
                    bulan = "Januari"
                End If
                If tgl.Month = 2 Then
                    bulan = "Februari"
                End If
                If tgl.Month = 3 Then
                    bulan = "Maret"
                End If
            Else
                If tgl.Month = 4 Then
                    bulan = "April"
                End If
                If tgl.Month = 5 Then
                    bulan = "Mei"
                End If
                If tgl.Month = 6 Then
                    bulan = "Juni"
                End If
            End If
        Else
            If tgl.Month < 10 Then
                If tgl.Month = 7 Then
                    bulan = "Juli"
                End If
                If tgl.Month = 8 Then
                    bulan = "Agustus"
                End If
                If tgl.Month = 9 Then
                    bulan = "September"
                End If
            Else
                If tgl.Month = 10 Then
                    bulan = "Oktober"
                End If
                If tgl.Month = 11 Then
                    bulan = "November"
                End If
                If tgl.Month = 12 Then
                    bulan = "Desember"
                End If
            End If
        End If

        Return hari & ", " & tgl.Day & " " & bulan & " " & tgl.Year
    End Function
    Public Shared Function sendWAInvitaion(UserId As String, Tipe As String) As String
        Dim json As Object = Nothing
        Dim errTitle As String = Nothing

        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ' Untuk hindarin error: The underlying connection was closed: An unexpected error occurred on a send.

            Dim App_Id = "bvbte-l7s13pq32tpmowc"
            Dim Channel_Id = "2873"
            Dim Secret_Key = "2d9ed9339e6768d68eb4344fa6758371"
            Dim url As String = "https://multichannel.qiscus.com/whatsapp/v1/" & App_Id & "/" & Channel_Id & "/messages"

            Dim postReq As WebRequest = WebRequest.Create(url)
            postReq.Method = "POST"

            Dim webHeader As WebHeaderCollection = postReq.Headers
            webHeader.Add("Qiscus-App-Id", App_Id)
            webHeader.Add("Qiscus-Secret-Key", Secret_Key)

            Dim postData As String
            Using oHelper As New clsSQLHelper
                oHelper.CommandType = CommandType.StoredProcedure
                oHelper.CommandText = "ADVPSIKOTEST..SP_bodySendWA_v1"

                oHelper.AddParameter("@UserId", UserId, SqlDbType.VarChar)
                oHelper.AddParameter("@Tipe", Tipe, SqlDbType.VarChar)
                postData = oHelper.ExecuteScalar
            End Using

            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            postReq.ContentType = "application/json"
            postReq.ContentLength = byteArray.Length

            Dim dataStream As Stream = postReq.GetRequestStream
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Dim response As WebResponse = postReq.GetResponse
            dataStream = response.GetResponseStream
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd

            json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer)

            reader.Close()
            dataStream.Close()
            response.Close()
        Catch ex As Exception
            errTitle = ex.Message + "->" + ex.StackTrace
        Finally
            Using oHelper As New clsSQLHelper
                oHelper.CommandType = CommandType.StoredProcedure
                oHelper.CommandText = "ADVPSIKOTEST..SP_saveWABroadcastId"

                If IsNothing(json) Then
                    oHelper.AddParameter("@ErrCode", "027", SqlDbType.VarChar)
                    oHelper.AddParameter("@ErrTitle", IIf(IsNothing(errTitle), "", errTitle), SqlDbType.VarChar)
                Else
                    oHelper.AddParameter("@wa_id", json("contacts")(0)("wa_id"), SqlDbType.VarChar)
                    oHelper.AddParameter("@Id", json("messages")(0)("id"), SqlDbType.VarChar)
                End If
                oHelper.AddParameter("@UserId", UserId, SqlDbType.VarChar)
                oHelper.AddParameter("@Tipe", Tipe, SqlDbType.VarChar)
                oHelper.ExecuteNonQuery()
            End Using
        End Try
        Return ""
    End Function
#End Region

End Class
