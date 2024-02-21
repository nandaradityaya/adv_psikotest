Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports Adv

Public Class clsConnDB

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("DCTWEB").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringUM() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("DCTWEBUM").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringDCT() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("DCT").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringDCT4() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("DCT4").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringHR() As String
        Get
            'If Adv.Data.DBBase.IsEncrypted Then 'dua2nya decrypt - khusus untuk ADVHR saja
            '    Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVHR").ConnectionString
            'Else
            '    Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVHR").ConnectionString
            'End If
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVHR").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringCCM() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("CCM").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringINS() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("WEBINSYST").ConnectionString.Decrypt
        End Get
    End Property


    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringACT() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ACTIVITIES").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringAdvAM() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVAM").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringAdvWH() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVWH").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringAdvFIN() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVFIN").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringAdvPAY() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVPAY").ConnectionString.Decrypt
        End Get
    End Property

    <Obsolete("ConnectionString will be prohibited. Use GetConnection method instead.")>
    Public Shared ReadOnly Property ConnectionStringADVPSIKOTEST() As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("ADVPSIKOTEST").ConnectionString.Decrypt
        End Get
    End Property

    Public Shared Function GetConnection() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionString)
    End Function

    Public Shared Function GetConnectionUM() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringUM)
    End Function

    Public Shared Function GetConnectionDCT() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringDCT)
    End Function

    Public Shared Function GetConnectionDCT4() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringDCT4)
    End Function

    Public Shared Function GetConnectionHR() As SqlConnection
        Return Adv.Data.DBBase.GetConnection(ConnectionStringHR)
    End Function

    Public Shared Function GetConnectionCCM() As SqlConnection
        Return Adv.Data.DBBase.GetConnection("CCM")
    End Function

    Public Shared Function GetConnectionINS() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringINS)
    End Function

    Public Shared Function GetConnectionACT() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringACT)
    End Function

    Public Shared Function GetConnectionAdvAM() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringAdvAM)
    End Function

    Public Shared Function GetConnectionAdvWH() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringAdvWH)
    End Function

    Public Shared Function GetConnectionAdvFIN() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringAdvFin)
    End Function
    Public Shared Function GetConnectionAdvPAY() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringAdvPay)
    End Function
	
    Public Shared Function GetConnectionADVPSIKOTEST() As SqlConnection
        Return ADV.Data.DBBase.GetConnection(ConnectionStringADVPSIKOTEST)
    End Function

    Public Shared Function Query(ByVal asStatement As String) As DataSet
        Using conn As SqlConnection = GetConnection()
            Dim da As New SqlDataAdapter(asStatement, conn)
            Dim ds As New DataSet
            da.Fill(ds, "Query")

            Return ds
        End Using
    End Function

    Public Shared Function QueryScalar(ByVal asStatement As String) As Object
        Using conn As SqlConnection = GetConnection()
            conn.Open()

            Dim cmd As New SqlCommand(asStatement, conn)
            Dim obj As Object = cmd.ExecuteScalar
            Return obj
        End Using

        Return Nothing
    End Function

    Public Shared Function Execute(ByVal asStatement As String) As Integer
        Try
            Using conn As SqlConnection = GetConnection()
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