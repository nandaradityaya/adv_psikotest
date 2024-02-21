Imports System.Data
Imports CORESQL
Imports System.Web.Services
Imports System.Web.Script.Serialization


Partial Class MS_UserRole
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckUserPermission()
    End Sub
    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub
    <WebMethod>
    Public Shared Function Delete(ByVal LoginRoleID As String) As String
        Dim dt As DataTable = New DataTable
        Dim result As Boolean = False
        Dim str As String = String.Empty
        'Dim cmdtx As IDataContext = DataFactory.Initial()
        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        oSqlHelper.OpenConnectionAndBeginTransaction()
        Dim dict = New Dictionary(Of String, String)
        Dim JSSerialize As New JavaScriptSerializer
        oSqlHelper.CommandText = "SELECT * FROM MS_UserRoleAssignment WHERE LoginRoleID=@LoginRoleID"
        oSqlHelper.AddParameter("@LoginRoleID", LoginRoleID, SqlDbType.Char)
        dt = oSqlHelper.ExecuteDataTable()
        If dt.Rows.Count > 0 Then
            dict.Add("status", "SDA")
        Else
            oSqlHelper.CommandText = "Delete From MS_UserRole Where LoginRoleID=@LoginRoleID"
            oSqlHelper.AddParameter("@LoginRoleID", LoginRoleID, SqlDbType.Char)
            Try
                result = oSqlHelper.ExecuteNonQuery() > 0
            Catch ex As Exception
                oSqlHelper.RollBackTransaction()
                dict.Add("status", ex.Message)
            Finally
                If (result = False) Then oSqlHelper.RollBackTransaction() Else oSqlHelper.CommitTransaction() : dict.Add("status", "OKE")
            End Try
        End If
        Return JSSerialize.Serialize(dict)
    End Function

End Class
