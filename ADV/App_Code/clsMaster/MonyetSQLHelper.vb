Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System


Public Class MonyetSQLHelper
    Implements IDisposable


#Region "Variable & Constants"

    Private oCmd As SqlCommand
    Private oSqlTrans As SqlTransaction

#End Region

#Region "Property"

    Public Property ActiveTransaction() As SqlClient.SqlTransaction
        Get
            Return oSqlTrans
        End Get
        Set(ByVal Value As SqlClient.SqlTransaction)
            oSqlTrans = Value
        End Set
    End Property

#End Region

#Region "Common"

    Public Enum ConnectionTo As Integer
        DA = 0
        UM = 1
        Advantage = 2
        INSYST = 3
        ACTIVITIES = 4
        ADVHR = 5
        ADVAM = 6
        ADVWH = 7
        DCT4 = 8
        ADVFIN = 9
        ADVPAY = 10
		ADVPSIKOTEST = 11
    End Enum


    Public Sub New()
        oCmd = New SqlCommand
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringADVPSIKOTEST)
    End Sub

    Public Sub New(pConnectionTo As ConnectionTo)
        oCmd = New SqlCommand
        oCmd.CommandType = CommandType.StoredProcedure
        If pConnectionTo = ConnectionTo.DA Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionString)
        ElseIf pConnectionTo = ConnectionTo.UM Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringUM)
        ElseIf pConnectionTo = ConnectionTo.Advantage Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringDCT)
        ElseIf pConnectionTo = ConnectionTo.INSYST Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringINS)
        ElseIf pConnectionTo = ConnectionTo.ACTIVITIES Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringACT)
        ElseIf pConnectionTo = ConnectionTo.ADVHR Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringHR)
        ElseIf pConnectionTo = ConnectionTo.ADVAM Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringAdvAM)
        ElseIf pConnectionTo = ConnectionTo.ADVWH Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringAdvWH)
        ElseIf pConnectionTo = ConnectionTo.ADVFIN Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringAdvFIN)
        ElseIf pConnectionTo = ConnectionTo.ADVPAY Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringAdvPAY)
        ElseIf pConnectionTo = ConnectionTo.DCT4 Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringDCT4)
        ElseIf pConnectionTo = ConnectionTo.ADVPSIKOTEST Then
            oCmd.Connection = New SqlConnection(clsConnDB.ConnectionStringADVPSIKOTEST)
        End If

    End Sub

    Public Sub AddParameter(ByVal paramName As String, ByVal paramValue As Object, ByVal paramType As SqlDbType, Optional ByVal ParamDirection As ParameterDirection = ParameterDirection.Input, Optional ByVal ParameterSize As Integer = 0)
        Dim param As New SqlParameter(paramName, paramType)
        param.Value = paramValue
        param.Direction = ParamDirection

        If ParameterSize <> 0 Then
            param.Size = ParameterSize
        End If

        oCmd.Parameters.Add(param)
    End Sub

    Public Sub ClearParameters()
        oCmd.Parameters.Clear()
    End Sub

    Public Property CommandText() As String
        Get
            Return oCmd.CommandText
        End Get
        Set(ByVal Value As String)
            oCmd.CommandText = Value
        End Set
    End Property

    Public Property CommandType() As Data.CommandType
        Get
            Return oCmd.CommandType
        End Get
        Set(ByVal Value As Data.CommandType)
            oCmd.CommandType = Value
        End Set
    End Property

    Public Property CommandTimeout() As Integer
        Get
            Return oCmd.CommandTimeout
        End Get
        Set(ByVal Value As Integer)
            oCmd.CommandTimeout = Value
        End Set
    End Property


    Public Function GetParameterValue(ByVal ParamName As String) As Object
        Return oCmd.Parameters(ParamName).Value
    End Function

#End Region

#Region "Execute Without Transaction"


    Public Function ExecuteNonQuery() As Integer
        Dim hasil As Integer
        Try
            oCmd.Connection.Open()
            hasil = oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Connection.Close()
        End Try
        Return hasil
    End Function


    Public Function ExecuteDataRow() As DataRow
        Dim hasil As New DataTable


        Using da As New SqlDataAdapter(oCmd)
            Try
                da.Fill(hasil)
                If hasil.Rows.Count > 0 Then
                    Return hasil.Rows(0)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return Nothing
    End Function

    Public Function ExecuteDataTable() As DataTable
        Dim hasil As New DataTable


        Using da As New SqlDataAdapter(oCmd)
            Try
                da.Fill(hasil)
                Return hasil
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return Nothing
    End Function

    Public Function ExecuteScalar() As Object
        Dim hasil As Object
        Try
            oCmd.Connection.Open()
            hasil = oCmd.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Connection.Close()
        End Try
        Return hasil
    End Function

    Public Sub Fill(ByRef ds As DataSet, ByVal TableName As String)
        Using da As New SqlDataAdapter
            da.SelectCommand = oCmd
            Try
                da.Fill(ds, TableName)
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

    Public Sub Fill(ByRef ds As DataSet)
        Using da As New SqlDataAdapter
            da.SelectCommand = oCmd
            Try
                da.Fill(ds)
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Execute With Transaction"

    Public Sub OpenConnectionAndBeginTransaction()
        oCmd.Connection.Open()
        oSqlTrans = oCmd.Connection.BeginTransaction
        oCmd.Transaction = oSqlTrans
    End Sub

    Public Function ExecuteNonQueryWithTransaction() As Integer
        Dim hasil As Integer
        hasil = oCmd.ExecuteNonQuery()
        Return hasil
    End Function

    Public Sub RollBackTransaction()
        oSqlTrans.Rollback()
    End Sub

    Public Sub CommitTransaction()
        oSqlTrans.Commit()
    End Sub

    Public Sub CloseConnection()
        oCmd.Connection.Close()
    End Sub

#End Region


#Region " Shared Functions"

    Public Shared Function DBNull2BLank(ByVal value As Object) As String

        If IsDBNull(value) Then
            Return ""
        Else
            Return value.ToString
        End If

    End Function
#End Region



    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region


End Class
