Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsDBHR

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return clsConnDB.ConnectionStringHR
        End Get
    End Property


    Public Shared Function GetConnectionHR() As SqlConnection
        Return clsConnDB.GetConnectionHR
    End Function


    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using ___SQLConnection As SqlConnection = GetConnectionHR()
            Dim da As New SqlDataAdapter(asStatement, ___SQLConnection)
            Dim ds As New DataSet
            da.Fill(ds, "Query")
            Return ds
        End Using
    End Function

	Public Shared Function Execute(ByVal asStatement As String) As Integer
		Try
            Using ___SQLConnection As SqlConnection = GetConnectionHR()
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

    Public Shared Function GetDataTable(ByVal strSQL As String) As DataTable
        Dim dt As New DataTable
        Dim conn As SqlConnection = GetConnectionHR()
        Dim da As New SqlDataAdapter(strSQL, conn)
        da.SelectCommand.CommandTimeout = 6000
        da.Fill(dt)
        Return dt
    End Function

    Public Shared Function QueryScalar(ByVal asStatement As String) As Object
        Using conn As SqlConnection = GetConnectionHR()
            conn.Open()   
            Dim cmd As New SqlCommand(asStatement, conn)
            Dim obj As Object = cmd.ExecuteScalar
            Return obj
        End Using       
        Return Nothing
    End Function

End Class