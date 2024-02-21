Imports System.Data
Imports Microsoft.VisualBasic

Public Class clsTrxTest
    Public Shared Function loginUjian(UserId As String, Password As String, level As Int16, isLoggedIn As Int16) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_LoginUjian"
            oHelper.AddParameter("@UserId", UserId, SqlDbType.VarChar)
            oHelper.AddParameter("@Password", Password, SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", level, SqlDbType.Int)
            oHelper.AddParameter("@isLoggedIn", isLoggedIn, SqlDbType.Int)
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function loginUjian(UserId As String, level As String, NoGroup As Int32) As DataRow
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_LoginUjian"
            oHelper.AddParameter("@UserId", UserId, SqlDbType.VarChar)
            oHelper.AddParameter("@lvl", level, SqlDbType.VarChar)
            oHelper.AddParameter("@NoGroup", NoGroup, SqlDbType.VarChar)
            Return oHelper.ExecuteDataRow
        End Using
    End Function
    Public Shared Function CekNoKTP(UserId As String, TglLahir As Date, NoKTP As String, isLoggedIn As Int16) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST..SP_CekNoKTP"
            oHelper.AddParameter("@UserId", UserId, SqlDbType.VarChar)
            oHelper.AddParameter("@TglLahir", TglLahir, SqlDbType.Date)
            oHelper.AddParameter("@NoKTP", NoKTP, SqlDbType.VarChar)
            oHelper.AddParameter("@isLoggedIn", isLoggedIn, SqlDbType.VarChar)
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getPaketSoalGroup(NoPaket As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT A.*, B.Keterangan " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup A JOIN ADVPSIKOTEST.dbo.MS_Petunjuk B ON A.NoPetunjuk = B.SeqNo " +
                "WHERE NoPaket = " + NoPaket
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getPaketSoalGroupDtl(NoPaket As String, NoGroup As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT * " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtl " +
                "WHERE NoPaket = " + NoPaket + " AND NoGroup = " + NoGroup
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Function getPaketSoalGroupDtlJawaban(NoPaket As String, NoGroup As String, NoUrut As String) As DataTable
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT * " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroupDtlJawaban " +
                "WHERE NoPaket = " + NoPaket + " AND NoGroup = " + NoGroup + " AND NoUrut = " + NoUrut
            Return oHelper.ExecuteDataTable
        End Using
    End Function
    Public Shared Sub submitJawabanRadio(NoPeserta As String, User As String, NoPaket As String, NoGroup As String, NoUrut As String, JawabanDiPilih As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST.dbo.SP_SubmitJawaban"
            oHelper.AddParameter("@NoPeserta", NoPeserta, SqlDbType.BigInt)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Tipe", "PG", SqlDbType.VarChar)
            oHelper.AddParameter("@NoPaket", CInt(NoPaket), SqlDbType.Int)
            oHelper.AddParameter("@NoGroup", CInt(NoGroup), SqlDbType.Int)
            oHelper.AddParameter("@NoUrut", CInt(NoUrut), SqlDbType.Int)
            oHelper.AddParameter("@JawabanDiPilih", CInt(JawabanDiPilih), SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub submitJawabanKreplin(NoPeserta As String, User As String, NoPaket As String, NoGroup As String, NoUrut As String, JmlBenar As String, JmlSalah As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST.dbo.SP_SubmitJawaban"
            oHelper.AddParameter("@NoPeserta", NoPeserta, SqlDbType.BigInt)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@Tipe", "Kreplin", SqlDbType.VarChar)
            oHelper.AddParameter("@NoPaket", CInt(NoPaket), SqlDbType.Int)
            oHelper.AddParameter("@NoGroup", CInt(NoGroup), SqlDbType.Int)
            oHelper.AddParameter("@NoUrut", CInt(NoUrut), SqlDbType.Int)
            oHelper.AddParameter("@JawabanDiPilih", CInt(JmlBenar), SqlDbType.Int)
            oHelper.AddParameter("@JmlSalah", CInt(JmlSalah), SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub timeoutSoalRadio(NoPeserta As String, User As String, NoPaket As String, NoGroup As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST.dbo.SP_IsiJawabanTimeOut"
            oHelper.AddParameter("@NoPeserta", NoPeserta, SqlDbType.BigInt)
            oHelper.AddParameter("@User", User, SqlDbType.VarChar)
            oHelper.AddParameter("@NoPaket", NoPaket, SqlDbType.BigInt)
            oHelper.AddParameter("@NoGroup", NoGroup, SqlDbType.Int)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getAnswered(UserID As String, NoPaket As String, NoGroup As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT SUBSTRING((SELECT (',' + CAST(NOURUT AS VARCHAR(MAX))) " +
                "FROM ADVPSIKOTEST.dbo.TR_Psikotest " +
                "WHERE NoUrut <> -1 AND UserId = '" + UserID + "' AND NoPaket = " + NoPaket + " AND NoGroup = " + NoGroup + " FOR XML PATH('')), 2, 1000)"
            Dim obj As Object = oHelper.ExecuteScalar()
            If Not (IsDBNull(obj)) Then
                Return CType(obj, String)
            Else
                Return String.Empty
            End If
        End Using
    End Function
    Public Shared Function getJawaban(UserID As String, NoPaket As String, NoGroup As String, NoUrut As String) As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT JawabanDiPilih " +
                "FROM ADVPSIKOTEST.dbo.TR_Psikotest " +
                "WHERE UserId = '" + UserID + "' AND NoPaket = " + NoPaket + "AND NoGroup = " + NoGroup + " AND NoUrut = " + NoUrut
            Dim obj As Object = oHelper.ExecuteScalar()
            If Not (IsDBNull(obj)) Then
                Return CType(obj, String)
            Else
                Return String.Empty
            End If
        End Using
    End Function
    Public Shared Sub selesaiUjian(UserID As String)
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.StoredProcedure
            oHelper.CommandText = "ADVPSIKOTEST.dbo.SP_HitungNilaiUjian"
            oHelper.AddParameter("@UserId", UserID, SqlDbType.VarChar)
            oHelper.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function getJmlGroupSoal(NoPaket As String) As Int16
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT COUNT(1) " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup " +
                "WHERE NoPaket = " + NoPaket
            Dim obj As Object = oHelper.ExecuteScalar()
            If Not (IsDBNull(obj)) Then
                Return CInt(obj)
            Else
                Return 0
            End If
        End Using
    End Function
    Public Shared Function getJmlSoal(UrutanGroupSoal As String, NoPaket As String) As Int16
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT MinimumJmlSoal " +
                "FROM ADVPSIKOTEST.dbo.MS_PaketSoalGroup " +
                "WHERE NoPaket = " + NoPaket + " " +
                "ORDER BY NoGroup " +
                "OFFSET " + UrutanGroupSoal + " ROWS FETCH NEXT 1 ROWS ONLY"
            Dim obj As Object = oHelper.ExecuteScalar()
            If Not (IsDBNull(obj)) Then
                Return CInt(obj)
            Else
                Return 0
            End If
        End Using
    End Function
    Public Shared Function getIntervalWaktuFoto() As String
        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text
            oHelper.CommandText =
                "SELECT TOP(1) Value " +
                "FROM dbo.SYS_Parameter " +
                "WHERE Name = 'Interval_Waktu_Foto' "
            Dim obj As Object = oHelper.ExecuteScalar()
            If Not (IsDBNull(obj)) Then
                Return obj.ToString
            Else
                Return ""
            End If
        End Using
    End Function
End Class
