Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsCCM

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return clsConnDB.ConnectionStringCCM
        End Get
    End Property


    Public Shared Function GetConnectionCCM() As SqlConnection
        Return clsConnDB.GetConnectionCCM
    End Function


    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using ___SQLConnection As SqlConnection = GetConnectionCCM()
            Dim da As New SqlDataAdapter(asStatement, ___SQLConnection)
            Dim ds As New DataSet
            da.Fill(ds, "Query")
            Return ds
        End Using
    End Function

    Public Shared Function QueryScalar(ByVal asStatement As String) As Object
        Using ___SQLConnection As SqlConnection = GetConnectionCCM()
            ___SQLConnection.Open()

            Dim cmd As New SqlCommand(asStatement, ___SQLConnection)
            Dim obj As Object = cmd.ExecuteScalar
            Return obj
        End Using

        Return Nothing
    End Function

    Public Shared Function Execute(ByVal asStatement As String) As Integer
        Try
            Using ___SQLConnection As SqlConnection = GetConnectionCCM()
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