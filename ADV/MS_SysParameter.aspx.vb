Imports Ext.Net
Imports System.Data
Imports System.Drawing
Imports OfficeOpenXml
Imports System.Data.SqlClient
Imports System.IO

Partial Class SysParameter
    Inherits System.Web.UI.Page

    Dim sql As String
    Private LoginUserID As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("../SessionTimeout.aspx")
        End If
        LoginUserID = Session("UserID")
        If Not Page.IsPostBack Then
            'LoadCombo()
            'LoadLastSessionData()
        End If

    End Sub

    <DirectMethod()>
    Public Sub FormAdd()
        FormPanel1.Reset()
        winAdd.Show()
        txtName.ReadOnly = False
        txtAction.Text = "add"
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click

        Try
            Using oSqlHelper As New MonyetSQLHelper
                If txtAction.Text = "add" Then
                    oSqlHelper.CommandText = "SP_IT_SysParameter_Insert"
                Else
                    oSqlHelper.CommandText = "SP_IT_SysParameter"
                End If
                oSqlHelper.CommandType = CommandType.StoredProcedure
                oSqlHelper.AddParameter("@Name", txtName.Text, SqlDbType.VarChar)
                oSqlHelper.AddParameter("@Remark", TxtRemark.Text, SqlDbType.VarChar)
                oSqlHelper.AddParameter("@Value", txtValue.Text, SqlDbType.VarChar)
                oSqlHelper.AddParameter("@Status", cmbStatus.Value, SqlDbType.TinyInt)
                oSqlHelper.AddParameter("@User", LoginUserID, SqlDbType.VarChar)
                oSqlHelper.ExecuteNonQuery()
            End Using

            Ext.Net.X.Msg.Alert("Information", "Berhasil diproses").Show()

            FormPanel1.Reset()
            winAdd.Close()

            Store1.Reload()
        Catch ex As Exception
            Ext.Net.X.Msg.Alert("Information", ex.Message).Show()
        End Try
    End Sub

    <DirectMethod()>
    Public Sub EditData(ByVal ID As String)
        Dim dt As DataTable
        Dim dr As DataRow
        Dim IsChecker As String = ""

        winAdd.Show()
        txtName.ReadOnly = True
        txtAction.Text = "edit"

        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        oSqlHelper.CommandType = CommandType.Text
        oSqlHelper.CommandText = "select * from SYS_Parameter where Name = '" & ID & "'"
        dt = oSqlHelper.ExecuteDataTable

        For Each dr In dt.Rows
            txtName.Text = dr("Name").ToString
            TxtRemark.Text = dr("Remark").ToString
            txtValue.Text = dr("Value").ToString
            cmbStatus.Value = dr("Status_Parameter").ToString
        Next
    End Sub

End Class