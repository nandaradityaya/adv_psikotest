Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Ext.Net

Public Class clsPaketSoal
#Region "Paket Soal"
    Public Shared Function cekStatusPenggunaanPaketSoal(NoPaket As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT TOP 1 'True' " +
                "FROM ADVPSIKOTEST.dbo.MS_PesertaDtl " +
                "WHERE NoPaket = " + NoPaket
            Dim obj = oHelper.ExecuteScalar
            If Not IsNothing(obj) Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekStatusPaketSoal(NoPaket As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT bAktif " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoal " +
                "WHERE NoPaket = " + NoPaket
            Dim obj = oHelper.ExecuteScalar
            If obj.ToString.Equals("True") Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekJumlahPaketSoalGroup(NoPaket As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "IF (SELECT COUNT(1) " +
                    "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup A JOIN ADVPSIKOTEST.dbo.MS_PaketSoal B ON A.NoPaket = B.NoPaket " +
                    "WHERE A.bAktif = 1 AND B.NoPaket = " + NoPaket + ") > 0 " +
                "BEGIN SELECT 1 END"
            Dim obj = oHelper.ExecuteScalar
            If IsNothing(obj) Then
                Return False
            Else
                Return True
            End If
        End Using
    End Function
    Public Shared Sub InputPaketSoal(NamaPaket As String, ToleransiWaktu As Int16, bAktif As String, UserInput As String, mcbPosisi As Object)
        Using oHelper As New clsSQLHelper
            Try
                oHelper.OpenConnectionAndBeginTransaction()
                oHelper.CommandType = CommandType.Text
                oHelper.CommandText = "INSERT INTO ADVPSIKOTEST..MS_PaketSoal " + vbCrLf +
                "(NamaPaket, ToleransiWaktu, bAktif, UserInput, TimeInput, UserEdit, TimeEdit) " + vbCrLf +
                "VALUES " + vbCrLf +
                "('" + NamaPaket + "', " + ToleransiWaktu.ToString + ", " + bAktif + ", '" + UserInput + "', GETDATE(), '" + UserInput + "', GETDATE())" + vbCrLf +
                "SELECT SCOPE_IDENTITY() 'msg'"
                Dim NoPaket As Integer = CInt(oHelper.ExecuteDataTable()(0)("msg"))

                TimpaPaketSoalChild(NoPaket, mcbPosisi, oHelper)

                oHelper.CommitTransaction()
            Catch ex As Exception
                oHelper.RollBackTransaction()
                Throw
            End Try
        End Using
    End Sub
    Public Shared Function GetPaketSoal(NoPaket As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT * " +
                "FROM ADVPSIKOTEST..MS_PaketSoal " +
                "WHERE NoPaket=" + NoPaket.ToString
            Return oHelper.ExecuteDataTable()
        End Using
    End Function
    Public Shared Sub EditPaketSoal(NoPaket As String, NamaPaket As String, ToleransiWaktu As String, bAktif As String, UserEdit As String, mcbPosisi As Object)
        Using oHelper As New clsSQLHelper
            Try
                oHelper.OpenConnectionAndBeginTransaction()
                oHelper.CommandType = CommandType.Text
                oHelper.CommandText = "UPDATE ADVPSIKOTEST..MS_PaketSoal " +
                    vbCrLf +
                    "SET " +
                    "NamaPaket='" + NamaPaket + "', " +
                    "ToleransiWaktu=" + ToleransiWaktu + ", " +
                    "bAktif=" + bAktif + ", " +
                    "UserEdit='" + UserEdit + "', TimeEdit=GETDATE() " +
                    vbCrLf +
                    "WHERE NoPaket=" + NoPaket
                oHelper.ExecuteNonQueryWithTransaction()

                TimpaPaketSoalChild(NoPaket, mcbPosisi, oHelper)

                oHelper.CommitTransaction()
            Catch ex As Exception
                oHelper.RollBackTransaction()
                Throw
            End Try
        End Using

    End Sub
    Public Shared Sub TimpaPaketSoalChild(NoPaket As String, cmb As Object, oHelper As clsSQLHelper)
        oHelper.CommandType = CommandType.Text
        oHelper.CommandText = "DELETE dbo.MS_PaketSoalChild " +
            "WHERE NoPaket = " + NoPaket
        oHelper.ExecuteNonQueryWithTransaction()

        For i As Integer = 0 To cmb.SelectedItems.Count - 1
            oHelper.CommandText = "INSERT INTO dbo.MS_PaketSoalChild " + vbCrLf +
                "(NoPaket, CodeJob) " + vbCrLf +
                "VALUES " + vbCrLf +
                "(" + NoPaket + ", '" + cmb.SelectedItems(i).Value.ToString + "')"
            oHelper.ExecuteNonQueryWithTransaction()
        Next
    End Sub
    Public Shared Function GetPaketSoalChild(NoPaket As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT CodePosisi = B.Code, NamaPosisi = B.Name " + vbCrLf +
                "FROM dbo.MS_PaketSoalChild A " + vbCrLf +
                "LEFT JOIN VW_ADVHR__REC_Job B ON A.CodeJob = B.Code" + vbCrLf +
                "WHERE A.NoPaket=" + NoPaket.ToString
            Return oHelper.ExecuteDataTable()
        End Using
    End Function
#End Region
#Region "Paket Soal Group"
    Public Shared Function cekStatusPaketSoalGroup(NoGroup As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT bAktif " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup " +
                "WHERE NoGroup = " + NoGroup
            Dim obj = oHelper.ExecuteScalar
            If obj.ToString.Equals("True") Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekJumlahPaketSoalGroupDtl(NoGroup As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT COUNT(1) JmlSoal, (SELECT C.MinimumJmlSoal FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup C WHERE C.NoGroup = " + NoGroup + ") MinimumJmlSoal " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl A JOIN ADVPSIKOTEST.dbo.MS_PaketSoalGroup B ON A.NoGroup = B.NoGroup " +
                "WHERE A.bAktif = 1 AND B.NoGroup = " + NoGroup
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getNoPaketSoal() As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT NoPaket, NamaPaket " +
                "FROM ADVPSIKOTEST..MS_PaketSoal"
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function GetPaketSoalGroup(NoGroup As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT * " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroup " +
                "WHERE NoGroup=" + NoGroup.ToString
            Return oHelper.ExecuteDataTable()
        End Using
    End Function
    Public Shared Sub ModifyPaketSoalGroup(NoPaket As String, NoGroup As String, NamaGroup As String, MinimumJmlSoal As String, NilaiStandar As String,
                                           WaktuPengerjaan As String, NoPetunjuk As String, bRandom As String, IsPrioritas As String, bAktif As String, User As String,
                                           tblNormaDtl As DataTable, HapusDataNormaDtl As List(Of String))
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroup "
            oHelper.AddParameter("@NoPaket", CType(NoPaket, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NamaGroup", NamaGroup, SqlDbType.VarChar)
            oHelper.AddParameter("@MinimumJmlSoal", CType(MinimumJmlSoal, Int32), SqlDbType.Int)
            oHelper.AddParameter("@NilaiStandar", CType(NilaiStandar, Int32), SqlDbType.Int)
            oHelper.AddParameter("@WaktuPengerjaan", CType(WaktuPengerjaan, Int32), SqlDbType.Int)
            oHelper.AddParameter("@NoPetunjuk", NoPetunjuk, SqlDbType.Int)
            oHelper.AddParameter("@bRandom", CType(bRandom, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@IsPrioritas", CType(IsPrioritas, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@bAktif", CType(bAktif, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            NoGroup = oHelper.ExecuteScalar()

            For Each s As String In HapusDataNormaDtl
                oHelper.ClearParameters()
                oHelper.CommandText = "ADVPSIKOTEST..SP_NormaDtl "
                oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
                oHelper.AddParameter("@SeqNo", s, SqlDbType.VarChar)
                oHelper.ExecuteNonQuery()
            Next

            For Each dr As DataRow In tblNormaDtl.Rows
                If CInt(dr("SeqNo")) > 0 Then
                    Continue For
                End If
                oHelper.ClearParameters()
                oHelper.CommandText = "ADVPSIKOTEST..SP_NormaDtl "
                oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
                oHelper.AddParameter("@Nama", dr("Nama").ToString, SqlDbType.VarChar)
                oHelper.AddParameter("@BatasAtas", CInt(dr("BatasAtas")), SqlDbType.Int)
                oHelper.AddParameter("@BatasBawah", CInt(dr("BatasBawah")), SqlDbType.Int)
                oHelper.AddParameter("@User", User, SqlDbType.VarChar)
                oHelper.AddParameter("@TimeInput", dr("TimeInput"), SqlDbType.DateTime)
                oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
                oHelper.ExecuteNonQuery()
            Next
        End Using
    End Sub
    Public Shared Sub ModifyPaketSoalGroupKreplin(NoPaket As String, NoGroup As String, NamaGroup As String, JmlKolom As String,
                                                  WaktuPengerjaan As String, NoPetunjuk As String, IsPrioritas As String, bAktif As String, User As String,
                                                  tblNormaDtl As DataTable, HapusDataNormaDtl As List(Of String))
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupKreplin "
            oHelper.AddParameter("@NoPaket", CType(NoPaket, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NamaGroup", NamaGroup, SqlDbType.VarChar)
            oHelper.AddParameter("@JmlKolom", CType(JmlKolom, Int32), SqlDbType.Int)
            oHelper.AddParameter("@WaktuPengerjaan", CType(WaktuPengerjaan, Int32), SqlDbType.Int)
            oHelper.AddParameter("@NoPetunjuk", NoPetunjuk, SqlDbType.Int)
            oHelper.AddParameter("@IsPrioritas", CType(IsPrioritas, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@bAktif", CType(bAktif, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.ExecuteNonQuery()

            For Each s As String In HapusDataNormaDtl
                oHelper.ClearParameters()
                oHelper.CommandText = "ADVPSIKOTEST..SP_NormaDtl "
                oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
                oHelper.AddParameter("@SeqNo", s, SqlDbType.VarChar)
                oHelper.ExecuteNonQuery()
            Next

            For Each dr As DataRow In tblNormaDtl.Rows
                If CInt(dr("SeqNo")) > 0 Then
                    Continue For
                End If
                oHelper.ClearParameters()
                oHelper.CommandText = "ADVPSIKOTEST..SP_NormaDtl "
                oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
                oHelper.AddParameter("@Nama", dr("Nama").ToString, SqlDbType.VarChar)
                oHelper.AddParameter("@BatasAtas", CInt(dr("BatasAtas")), SqlDbType.Int)
                oHelper.AddParameter("@BatasBawah", CInt(dr("BatasBawah")), SqlDbType.Int)
                oHelper.AddParameter("@User", User, SqlDbType.VarChar)
                oHelper.AddParameter("@TimeInput", dr("TimeInput"), SqlDbType.DateTime)
                oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
                oHelper.ExecuteNonQuery()
            Next
        End Using
    End Sub
    Public Shared Sub DeletePaketSoalGroup(NoGroup As String) 'Hapus tipe group soal Kreplin pakai SP_PaketSoalGroup juga
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroup"
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.Int)
            Dim dt = oHelper.ExecuteDataTable()
            DeleteFile(dt)
        End Using
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroup"
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.Int)
            Dim dt = oHelper.ExecuteDataTable()
            DeleteFile(dt)
        End Using
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroup"
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 3, SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getNamaPetunjuk(NoPetunjuk As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT Keterangan " +
                "FROM ADVPSIKOTEST..MS_Petunjuk " +
                "WHERE SeqNo = " + NoPetunjuk
            Return oHelper.ExecuteScalar()
        End Using
    End Function
#End Region
#Region "Paket Soal Group Dtl"
    Public Shared Function cekStatusPaketSoalGroupDtlBySeqNo(SeqNo As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT bAktif " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl " +
                "WHERE SeqNo = " + SeqNo
            Dim obj = oHelper.ExecuteScalar
            If obj.ToString.Equals("1") Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekStatusPaketSoalGroupDtlByNoGroup(NoGroup As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT bAktif " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl " +
                "WHERE NoGroup = " + NoGroup
            Dim obj = oHelper.ExecuteScalar
            If obj.ToString.Equals("1") Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekStatusPaketSoalGroupDtlByNoUrut(NoPaket As String, NoGroup As String, NoUrut As String) As Boolean
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT bAktif " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl " +
                "WHERE NoPaket = " + NoPaket + " AND NoGroup = " + NoGroup + " AND NoUrut = " + NoUrut
            Dim obj = oHelper.ExecuteScalar
            If obj.ToString.Equals("True") Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function
    Public Shared Function cekJumlahPaketSoalGroupDtlJawaban(SeqNo As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT COUNT(1) JmlJwb " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtlJawaban A Join ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl B ON A.NoPaket = B.NoPaket And A.NoGroup = B.NoGroup And A.NoUrut = B.NoUrut " +
                "Where B.SeqNo = " + SeqNo
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getNoPaketSoalGroup() As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT NoGroup, NamaGroup " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroup"
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getLastNoUrutPaketSoalGroupDtl(NoPaketSoal As String, NoPaketSoalGroup As String) As Int64
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT MAX(NoUrut) " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl " +
                "WHERE NoPaket=" + NoPaketSoal + " AND NoGroup=" + NoPaketSoalGroup
            Dim a = oHelper.ExecuteScalar
            Return IIf(IsDBNull(a), 0, a)
        End Using
    End Function
    Public Shared Sub ModifyPaketSoalGroupDtl(SeqNo As String, NoPaket As String, NoGroup As String, NoUrut As String, Judul As String, Deskripsi As String,
                                              IsDownload As String, bAktif As String, MediaFileName As String, UrlMedia As String, TipeMedia As String,
                                              User As String, FileUp As FileUploadField)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtl"

            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoPaket", NoPaket, SqlDbType.BigInt)
            oHelper.AddParameter("@NoGroup", NoGroup, SqlDbType.BigInt)
            oHelper.AddParameter("@NoUrut", NoUrut, SqlDbType.BigInt)
            oHelper.AddParameter("@Judul", Judul, SqlDbType.VarChar)
            oHelper.AddParameter("@Deskripsi", Deskripsi, SqlDbType.VarChar)
            oHelper.AddParameter("@IsDownload", CType(IsDownload, Int16), SqlDbType.Bit)
            oHelper.AddParameter("@bAktif", CType(bAktif, Int16), SqlDbType.Bit)
            oHelper.AddParameter("@MediaFileName", MediaFileName, SqlDbType.VarChar)
            oHelper.AddParameter("@UrlMedia", UrlMedia, SqlDbType.VarChar)
            oHelper.AddParameter("@TipeMedia", TipeMedia, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            Dim dt = oHelper.ExecuteDataTable()

            UploadFile(UrlMedia, dt, FileUp, "Soal")
        End Using
    End Sub
    Public Shared Sub DeletePaketSoalGroupDtl(SeqNo As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtl"
            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.Int)
            Dim dt = oHelper.ExecuteDataTable()
            DeleteFile(dt)
        End Using
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtl"
            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.Int)
            Dim dt = oHelper.ExecuteDataTable()
            DeleteFile(dt)
        End Using
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtl"
            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 3, SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function GetPaketSoalGroupDtl(SeqNo As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT * " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl " +
                "WHERE SeqNo=" + SeqNo.ToString
            Return oHelper.ExecuteDataTable()
        End Using
    End Function
#End Region
#Region "Paket Soal Group Dtl Jawaban"
    Public Shared Function getNoUrutDtl(SeqNo As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT NoUrut " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtl " +
                "WHERE SeqNo=" + SeqNo.ToString
            Return oHelper.ExecuteScalar()
        End Using
    End Function
    Public Shared Function getLastNoJawabanPaketSoalGroupDtlJawaban(NoPaket As String, NoGroup As String, NoUrut As String) As Int64
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT MAX(NoJawaban) " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtlJawaban " +
                "WHERE NoPaket=" + NoPaket + " AND NoGroup=" + NoGroup + " AND NoUrut=" + NoUrut
            Dim a = oHelper.ExecuteScalar
            Return IIf(IsDBNull(a), 0, a)
        End Using
    End Function
    Public Shared Sub ModifyPaketSoalGroupDtlJawaban(NoPaket As String, NoGroup As String, NoUrut As String, NoJawaban As String, Jawaban As String,
                                                     PoinJawaban As String, MediaFileName As String, UrlMedia As String, TipeMedia As String,
                                                     TextMedia As String, User As String, FileUp As FileUploadField)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtlJawaban"

            oHelper.AddParameter("@NoPaket", CType(NoPaket, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoGroup", CType(NoGroup, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoUrut", CType(NoUrut, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@NoJawaban", CType(NoJawaban, Int32), SqlDbType.Int)
            oHelper.AddParameter("@PoinJawaban", CType(PoinJawaban, Int32), SqlDbType.SmallInt)
            oHelper.AddParameter("@Jawaban", Jawaban, SqlDbType.VarChar)
            oHelper.AddParameter("@UrlMedia", UrlMedia, SqlDbType.VarChar)
            oHelper.AddParameter("@MediaFileName", MediaFileName, SqlDbType.VarChar)
            oHelper.AddParameter("@TipeMedia", TipeMedia, SqlDbType.VarChar)
            oHelper.AddParameter("@TextMedia", TextMedia, SqlDbType.VarChar)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            Dim dt = oHelper.ExecuteDataTable()

            UploadFile(UrlMedia, dt, FileUp, "Jwb")
        End Using
    End Sub
    Public Shared Sub DeletePaketSoalGroupDtljawaban(SeqNo As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtlJawaban"
            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 1, SqlDbType.Int)
            Dim dt = oHelper.ExecuteDataTable
            DeleteFile(dt)
        End Using
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_PaketSoalGroupDtlJawaban"
            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.BigInt)
            oHelper.AddParameter("@Act", "DEL", SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", 2, SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function GetPaketSoalGroupDtlJawaban(SeqNo As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText = "SELECT * " +
                "FROM ADVPSIKOTEST..MS_PaketSoalGroupDtlJawaban " +
                "WHERE SeqNo=" + SeqNo.ToString
            Return oHelper.ExecuteDataTable()
        End Using
    End Function
#End Region

#Region "Petunjuk"
    Public Shared Sub ModifyPetunjuk(SeqNo As String, Keterangan As String, bAktif As String, User As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_Petunjuk"

            oHelper.AddParameter("@SeqNo", CType(SeqNo, Int64), SqlDbType.Int)
            oHelper.AddParameter("@Keterangan", Keterangan, SqlDbType.VarChar)
            oHelper.AddParameter("@bAktif", CType(bAktif, Boolean), SqlDbType.Bit)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Act", "ADD", SqlDbType.VarChar)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getPetunjuk(SeqNo As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
            "SELECT SeqNo, REPLACE(keterangan, '''', '\''') Keterangan, bAktif, UserInput, TimeInput, UserEdit, TimeEdit " +
            "FROM ADVPSIKOTEST..MS_Petunjuk " +
            "WHERE SeqNo = " + SeqNo

            Return oHelper.ExecuteDataTable()
        End Using
    End Function
#End Region

#Region "Save File"
    Public Shared Sub UploadFile(UrlMedia As String, dt As DataTable, FileUp As FileUploadField, Tipe As String)
        If Trim(dt.Rows(0)("TipeMedia").ToString).Equals("NOMEDIA") Then
            Exit Sub
        ElseIf Not IsNothing(FileUp) Then
            If FileUp.PostedFile.FileName.Equals("") Then
                Exit Sub
            End If
            Dim NameFile As String = Tipe & "-" & dt.Rows(0)("SeqNo") & "_" & FileUp.PostedFile.FileName.Replace(" ", "")

            Dim getPathParrent As String = ConfigurationManager.AppSettings("ESSMasterPath").ToString
            If Right(getPathParrent, 1) <> "\" Then
                getPathParrent += "\"
            End If

            If Not Directory.Exists(getPathParrent & "/" & dt.Rows(0)("NoPaket").ToString & "/" & dt.Rows(0)("NoGroup").ToString) Then
                Directory.CreateDirectory(getPathParrent & "/" & dt.Rows(0)("NoPaket").ToString & "/" & dt.Rows(0)("NoGroup").ToString)
            End If

            Dim DirFile = getPathParrent & "/" & dt.Rows(0)("NoPaket").ToString & "/" & dt.Rows(0)("NoGroup").ToString & "/" & NameFile
            If System.IO.File.Exists(DirFile) Then
                System.IO.File.Delete(DirFile)
            End If

            Try
                FileUp.PostedFile.SaveAs(DirFile)
                Using oSqlHelper As New clsSQLHelper
                    If Tipe.Equals("Soal") Then
                        oSqlHelper.CommandText = "UPDATE ADVPSIKOTEST..MS_PaketSoalGroupDtl SET UrlMedia = @UrlMedia, MediaFileName = @MediaFileName, TipeMedia = @TipeMedia WHERE SeqNo = @SeqNo "
                    Else
                        oSqlHelper.CommandText = "UPDATE ADVPSIKOTEST..MS_PaketSoalGroupDtlJawaban SET UrlMedia = @UrlMedia, MediaFileName = @MediaFileName, TipeMedia = @TipeMedia WHERE SeqNo = @SeqNo"
                    End If
                    oSqlHelper.CommandType = CommandType.Text
                    oSqlHelper.AddParameter("@SeqNo", dt.Rows(0)("SeqNo"), SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@UrlMedia", "", SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@MediaFileName", NameFile, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@TipeMedia", Replace(System.IO.Path.GetExtension(FileUp.PostedFile.FileName), ".", "").ToString.ToUpper, SqlDbType.VarChar)
                    oSqlHelper.ExecuteNonQuery()
                End Using
                FileUp.Reset()
            Catch ex As Exception
                Ext.Net.X.Msg.Alert("Failed", ex.Message).Show()
                Return
            End Try
        End If
    End Sub
#End Region
#Region "Delete File"
    Public Shared Sub DeleteFile(dt As DataTable)
        Dim getPathParrent As String = ConfigurationManager.AppSettings("ESSMasterPath").ToString

        If Right(getPathParrent, 1) <> "\" Then
            getPathParrent += "\"
        End If

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim Dir = getPathParrent & "/" & dt.Rows(i)("NoPaket").ToString & "/" & dt.Rows(i)("NoGroup").ToString & "/" & dt.Rows(i)("MediaFileName").ToString

            If System.IO.File.Exists(Dir) Then
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(Dir)
            End If
        Next
    End Sub
#End Region
End Class
