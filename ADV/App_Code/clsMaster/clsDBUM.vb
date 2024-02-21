Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsDBUM

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return clsConnDB.ConnectionStringUM
        End Get
    End Property


    Public Shared Function GetConnectionUM() As SqlConnection
        Return clsConnDB.GetConnectionUM
    End Function


    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using conn As SqlConnection = GetConnectionUM()
            Dim da As New SqlDataAdapter(asStatement, conn)
            Dim ds As New DataSet
            da.Fill(ds, "Query")
            Return ds
        End Using
    End Function

    Public Shared Function Execute(ByVal asStatement As String) As Integer
        Try
            Using conn As SqlConnection = GetConnectionUM()
                conn.Open()

                Dim cmd As New SqlCommand(asStatement, conn)
                cmd.CommandTimeout = 30
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Return -1
        End Try

        Return 1
    End Function

End Class