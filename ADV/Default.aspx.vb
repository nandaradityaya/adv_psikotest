Imports System.Data
Imports Ext.Net

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Session("SessCheck") = ""
        Session("AuthID") = ""
        'cek license Ext.NET dll jika ada yg lain tambah disini
        Select Case Request.QueryString("license")
            Case "1", "?"
                Dim res As New Ext.Net.ResourceManager
                'Response.Write("License Ext.NET : " & res.LicenseKey & "<br>")
                Response.Write("License Ext.NET is valid : " & res.LicenseKey.ToString)
        End Select
        'cek Maintenance Mode, jika sedang maintenance maka tampilkan screen Maintenance

        Session("UserID") = Request.QueryString("UserID")
        Session("RoleID") = Request.QueryString("RoleID")
        If IsNothing(Session("UserID")) Then
            Exit Sub
        End If

        Dim E64 = New Encryption64
        Session("UserID") = E64.Decrypt(Session("UserID"), ConfigurationManager.AppSettings("EncryptionKey").ToString)
        Session("RoleID") = E64.Decrypt(Session("RoleID"), ConfigurationManager.AppSettings("EncryptionKey").ToString)
        Session("mADMIN") = True
        Response.Redirect("MASTER_AssignUjian.aspx", True)
    End Sub


    Protected Sub btnlogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnlogin.Click
        Dim Username As String = Request.Form("useid")
        Dim Password As String = Request.Form("passwd")

        Dim msg As String = ""
        Try
            Dim E64 = New Encryption64
            Dim e_Password = E64.Encrypt(Password, ConfigurationManager.AppSettings("EncryptionKey").ToString)
            'Login Ujian
            Dim dt_Login = clsTrxTest.loginUjian(Username, e_Password, 1, 0)
            If dt_Login.Rows(0)("msg") = "" Then
                Session("mADMIN") = False
                Dim T = "U=" + Username + "&P=" + Password
                Dim s = E64.Encrypt(T, "Beprocureit123")
                Response.Redirect("TRX_Ujian.aspx?Token=" + s, True)
            Else
                msg = dt_Login.Rows(0)("msg")
                If InStr(msg, ".") Then
                    Response.Write(msg)
                    Response.End()
                End If
            End If

            Dim dr As DataRow
            'Login MS_User
            Using oHelper As New clsSQLHelper
                oHelper.CommandType = CommandType.StoredProcedure
                oHelper.CommandText = "AdminKuisLogin"
                oHelper.AddParameter("@LoginUserId", Username, SqlDbType.VarChar)
                oHelper.AddParameter("@Password", e_Password, SqlDbType.VarChar)
                oHelper.AddParameter("@IPAddress", Request.UserHostAddress.ToString, SqlDbType.VarChar)
                dr = oHelper.ExecuteDataRow
            End Using

            'Response.Write(e_Password + JSON.Serialize(dr))
            'Response.End()

            If dr("Msg").ToString = "" Then
                Session("UserID") = dr("LoginUserID").ToString
                Session("UserName") = dr("LoginUserName").ToString
                Session("RoleID") = dr("LoginRoleID").ToString
                Session("mADMIN") = True
                Response.Redirect("MASTER_AssignUjian.aspx", True)
                Response.End()
            Else
                msg = dt_Login.Rows(0)("msg")
                If InStr(msg, ".") Then
                    Response.Write(msg)
                    Response.End()
                End If
            End If

        Catch ex As Exception
            Response.Write(msg + If(ex.Message.Contains("Thread was being aborted."), "", ex.Message))
            Response.End()
        End Try
    End Sub
End Class