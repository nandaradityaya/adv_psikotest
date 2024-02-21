Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System


Public Class clsSQLHelper
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

    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return Adv.Decrypt(ConfigurationManager.ConnectionStrings("ADVPSIKOTEST").ConnectionString)
        End Get
    End Property

    Public Sub New()
        oCmd = New SqlCommand
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Connection = New SqlConnection(ConnectionString())
    End Sub

    Public Sub AddParameter(ByVal paramName As String, ByVal paramValue As Object, ByVal paramType As SqlDbType,
                            Optional ByVal ParamDirection As ParameterDirection = ParameterDirection.Input, Optional ByVal ParameterSize As Integer = 0)
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

        If IsDBNull(value) Or value Is Nothing Then
            Return ""
        Else
            Return value.ToString
        End If

    End Function

    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using ___SQLConnection As New SqlConnection(clsSQLHelper.ConnectionString)
            Dim da As New SqlDataAdapter(asStatement, ___SQLConnection)
            Dim ds As New DataSet
            da.Fill(ds, "Query")
            Return ds
        End Using
    End Function

    Public Shared Function QueryScalar(ByVal asStatement As String) As Object
        Using ___SQLConnection As New SqlConnection(clsSQLHelper.ConnectionString)
            ___SQLConnection.Open()

            Dim cmd As New SqlCommand(asStatement, ___SQLConnection)
            Dim obj As Object = cmd.ExecuteScalar
            Return obj
        End Using
        Return Nothing
    End Function

    Public Shared Function Execute(ByVal asStatement As String) As Integer
        Try
            Using ___SQLConnection As New SqlConnection(clsSQLHelper.ConnectionString)
                ___SQLConnection.Open()
                Dim cmd As New SqlCommand(asStatement, ___SQLConnection)
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Return -1
        End Try

        Return 1
    End Function

    Public Shared Function GetDataTable(ByVal asStatement As String) As DataTable
        Dim dt As New DataTable
        Dim conn As New SqlConnection(clsSQLHelper.ConnectionString)
        Dim da As New SqlDataAdapter(asStatement, conn)
        da.SelectCommand.CommandTimeout = 0
        da.Fill(dt)
        Return dt
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
