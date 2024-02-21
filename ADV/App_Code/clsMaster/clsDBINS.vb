Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsDBINS

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return clsConnDB.ConnectionStringINS
        End Get
    End Property


    Public Shared Function GetConnectionINS() As SqlConnection
        Return clsConnDB.GetConnectionINS
    End Function


    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using ___SQLConnection As SqlConnection = GetConnectionINS()
            Dim da As New SqlDataAdapter(asStatement, ___SQLConnection)
            Dim ds As New DataSet
            da.Fill(ds, "Query")
            Return ds
        End Using
    End Function

    Public Shared Function Execute(ByVal asStatement As String) As Integer
        Try
            Using ___SQLConnection As SqlConnection = GetConnectionINS()
                ___SQLConnection.Open()

                Dim cmd As New SqlCommand(asStatement, ___SQLConnection)
                cmd.CommandTimeout = 30
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Return -1
        End Try

        Return 1
    End Function

End Class