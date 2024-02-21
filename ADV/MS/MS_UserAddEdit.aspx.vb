Imports System.Data
Imports Ext.Net
Imports System.IO

Partial Class MS_UserAddEdit
    Inherits System.Web.UI.Page
    Private LoginUserID As String

#Region " Properties"

    Private Property FormState() As String
        Get
            Return ViewState("FormState")
        End Get
        Set(ByVal value As String)
            ViewState("FormState") = value
        End Set
    End Property

    Private Property PrimaryKey() As String
        Get
            Return ViewState("PrimaryKey")
        End Get
        Set(ByVal value As String)
            ViewState("PrimaryKey") = value
        End Set
    End Property


    Private Property PreviousEmailAddress() As String
        Get
            Return ViewState("PreviousEmailAddress")
        End Get
        Set(ByVal value As String)
            ViewState("PreviousEmailAddress") = value
        End Set
    End Property

    Private Property PreviousLoginUserName() As String
        Get
            Return ViewState("PreviousLoginUserName")
        End Get
        Set(ByVal value As String)
            ViewState("PreviousLoginUserName") = value
        End Set
    End Property

    Private Property PreviousStatus() As String
        Get
            Return ViewState("PreviousStatus")
        End Get
        Set(ByVal value As String)
            ViewState("PreviousStatus") = value
        End Set
    End Property

    Private Property PreviousLoginStatus() As String
        Get
            Return ViewState("PreviousLoginStatus")
        End Get
        Set(ByVal value As String)
            ViewState("PreviousLoginStatus") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("../SessionTimeout.aspx")
        End If

        LoginUserID = Session("UserID")

        Me.FormState = Request.QueryString("FormState")

        If Not Page.IsPostBack Then
            Session("Status") = 1
            If Me.FormState = "Edit" Then
                Me.PrimaryKey = Request.QueryString("LoginUserID")
                LoadData()
                txtLoginUserID.ReadOnly = True
                chkChangePassword.Attributes.Add("onclick", "ChangePassword('" & chkChangePassword.ClientID & "', '" & txtPassword.ClientID & "', '" & txtReenterPassword.ClientID & "', '" & rfvPassword1.ClientID & "', '" & rfvPassword2.ClientID & "');")
                lblFormState.Text = "Edit User"
            ElseIf Me.FormState = "Add" Then
                chkChangePassword.Visible = False
                lblFormState.Text = "Add User"
            End If
        End If
    End Sub

    Private Sub LoadData()
        'Dim cmdtx As IDataContext = DataFactory.Initial()
        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        Dim q As String = String.Empty
        Try
            q = "SELECT a.LoginUserID,a.LoginPassword,a.LoginUserName,a.EmailAddress,a.Remarks,a.Status,a.LoginStatus,a.LastLoginDate,a.LastSuccessfulLoginIP,a.PhoneNumber,a.PhoneNumber" & vbCrLf
            q += " FROM MS_User a WITH(NOLOCK, NOWAIT)" & vbCrLf
            q += " WHERE a.LoginUserID=@LoginUserID" & vbCrLf
            oSqlHelper.CommandText = q
            oSqlHelper.CommandType = CommandType.Text
            oSqlHelper.AddParameter("@LoginUserID", PrimaryKey, SqlDbType.Char)
            Dim drow As DataRow = oSqlHelper.ExecuteDataRow
            If Not drow Is Nothing Then
                If Not drow Is Nothing Then

                    txtLoginUserID.Text = drow("LoginUserID")
                    txtLoginUserName.Text = drow("LoginUserName")

                    If IsDBNull(drow("EmailAddress")) Then
                        txtEmailAddress.Text = String.Empty
                    Else
                        txtEmailAddress.Text = drow("EmailAddress")
                    End If

                    If IsDBNull(drow("PhoneNumber")) Then
                        txtPhoneNumber.Text = String.Empty
                    Else
                        txtPhoneNumber.Text = drow("PhoneNumber")
                    End If

                    If IsDBNull(drow("Remarks")) Then
                        txtRemarks.Text = String.Empty
                    Else
                        txtRemarks.Text = drow("Remarks")
                    End If

                    chkChangePassword.Checked = False

                    If IsDBNull(drow("LastLoginDate")) Then
                        lblLastLoginDate.Text = "N/A"
                    Else
                        lblLastLoginDate.Text = drow("LastLoginDate")
                    End If

                    If IsDBNull(drow("LastSuccessfulLoginIP")) Then
                        lblLastLoginFrom.Text = "N/A"
                    Else
                        lblLastLoginFrom.Text = drow("LastSuccessfulLoginIP")
                    End If

                    If drow("Status") = "A" Then
                        rdbStatusActive.Checked = True
                        rdbStatusNonActive.Checked = False
                    Else
                        rdbStatusNonActive.Checked = True
                        rdbStatusActive.Checked = False
                    End If

                    If drow("LoginStatus") = "A" Then
                        rdbLoginStatusAllow.Checked = True
                        rdbLoginStatusLocked.Checked = False
                    Else
                        rdbLoginStatusLocked.Checked = True
                        rdbLoginStatusAllow.Checked = False
                    End If

                    Me.PreviousLoginUserName = drow("LoginUserName")
                    Me.PreviousEmailAddress = drow("EmailAddress")
                    Me.PreviousStatus = drow("Status")
                    Me.PreviousLoginStatus = drow("LoginStatus")

                    'Me.LastUpdateStamp = drow("LastUpdateStamp")
                End If
            End If
        Catch ex As Exception
            ltlMessage.Text = "<div class=""alert alert-alert"">" & ex.Message & "<br /> " & ex.StackTrace & "</div>"
        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As Boolean = False
        'Dim cmdtx As IDataContext = DataFactory.Initial()
        Dim q As String = String.Empty
        Using oSql As New MonyetSQLHelper
            oSql.CommandType = CommandType.Text
            Dim dt As DataTable = New DataTable
            If Me.FormState = "Add" Then
                q = " SELECT * FROM MS_User WITH(NOLOCK, NOWAIT) WHERE LoginUserID=@LoginUserId"
                oSql.CommandText = q
                oSql.AddParameter("@LoginUserId", txtLoginUserID.Text, SqlDbType.Char)
                dt = oSql.ExecuteDataTable
                If dt.Rows.Count > 0 Then
                    ltlMessage.Text = "<div class=""alert alert-alert"">User ID sudah pernah diinput</div>"
                    Return
                End If
            End If
        End Using

        Dim E64 = New Encryption64
        Dim e_Password = E64.Encrypt(txtPassword.Text, ConfigurationManager.AppSettings("EncryptionKey").ToString)
        Dim oSqlHelper As MonyetSQLHelper = New MonyetSQLHelper
        oSqlHelper.CommandType = CommandType.Text
        oSqlHelper.OpenConnectionAndBeginTransaction()

        If Me.FormState = "Add" Then
            q = "Insert Into MS_User" & vbCrLf
            q += "(LoginUserID,LoginPassword,LoginUserName,EmailAddress,Remarks,Status,LoginStatus,LoginRetries,UserInput,UserEdit,TimeInput,TimeEdit)" & vbCrLf
            q += "Values" & vbCrLf
            q += "(@LoginUserID,@LoginPassword,@LoginUserName,@EmailAddress,@Remarks,@Status,@LoginStatus,0,@UserInput,@UserInput,getdate(),getdate())" & vbCrLf & vbCrLf
            oSqlHelper.CommandText = q
            oSqlHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.Char)
            oSqlHelper.AddParameter("@LoginPassword", e_Password, SqlDbType.Char)
            oSqlHelper.AddParameter("@LoginUserName", txtLoginUserName.Text, SqlDbType.Char)
            oSqlHelper.AddParameter("@EmailAddress", txtEmailAddress.Text, SqlDbType.Char)
            oSqlHelper.AddParameter("@Remarks", txtRemarks.Text, SqlDbType.Char)
            oSqlHelper.AddParameter("@Status", IIf(rdbStatusActive.Checked, "A", "N"), SqlDbType.Char)
            oSqlHelper.AddParameter("@UserInput", LoginUserID, SqlDbType.Char)
            oSqlHelper.AddParameter("@LoginStatus", IIf(rdbLoginStatusAllow.Checked, "A", "N"), SqlDbType.Char)
        ElseIf Me.FormState = "Edit" Then
            If chkChangePassword.Checked = True Then
                q = "UPDATE MS_User SET " & vbCrLf
                q += " LoginPassword = @LoginPassword" & vbCrLf
                q += ",LoginUserName = @LoginUserName" & vbCrLf
                q += ",EmailAddress = @EmailAddress" & vbCrLf
                q += ",Remarks = @Remarks" & vbCrLf
                q += ",LoginStatus = @LoginStatus" & vbCrLf
                q += ",Status = @Status" & vbCrLf
                q += ",UserEdit= @UserEdit" & vbCrLf
                q += ",TimeEdit = getdate()" & vbCrLf
                q += "WHERE LoginUserID = @LoginUserID" & vbCrLf
                oSqlHelper.CommandText = q
                oSqlHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@LoginPassword", e_Password, SqlDbType.Char)
                oSqlHelper.AddParameter("@LoginUserName", txtLoginUserName.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@EmailAddress", txtEmailAddress.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@Remarks", txtRemarks.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@Status", IIf(rdbStatusActive.Checked, "A", "N"), SqlDbType.Char)
                oSqlHelper.AddParameter("@LoginStatus", IIf(rdbLoginStatusAllow.Checked, "A", "N"), SqlDbType.Char)
                oSqlHelper.AddParameter("@UserEdit", LoginUserID, SqlDbType.Char)
            Else
                q = "UPDATE MS_User SET " & vbCrLf
                q += "LoginUserName = @LoginUserName" & vbCrLf
                q += ",EmailAddress = @EmailAddress" & vbCrLf
                q += ",Remarks = @Remarks" & vbCrLf
                q += ",LoginStatus = @LoginStatus" & vbCrLf
                q += ",Status = @Status" & vbCrLf
                q += ",UserEdit= @UserEdit" & vbCrLf
                q += ",TimeEdit = getdate()" & vbCrLf
                q += "WHERE LoginUserID = @LoginUserID" & vbCrLf
                oSqlHelper.CommandText = q
                oSqlHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@LoginUserName", txtLoginUserName.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@EmailAddress", txtEmailAddress.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@Remarks", txtRemarks.Text, SqlDbType.Char)
                oSqlHelper.AddParameter("@Status", IIf(rdbStatusActive.Checked, "A", "N"), SqlDbType.Char)
                oSqlHelper.AddParameter("@LoginStatus", IIf(rdbLoginStatusAllow.Checked, "A", "N"), SqlDbType.Char)
                oSqlHelper.AddParameter("@UserEdit", LoginUserID, SqlDbType.Char)
            End If
        End If

        Try
            result = oSqlHelper.ExecuteNonQueryWithTransaction > 0
        Catch ex As Exception
            ltlMessage.Text = "<div class=""alert alert-danger"">" & ex.Message & "<br /> " & ex.StackTrace & "</div>"
        Finally
            If (result = False) Then oSqlHelper.RollBackTransaction() Else oSqlHelper.CommitTransaction()
        End Try

        If result = True Then
            Response.Redirect("MS_User.aspx")
        End If
    End Sub

    Private Sub MsUserAddEdit_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Me.FormState = "Edit" Then
            If Not Page.ClientScript.IsStartupScriptRegistered("changepass") Then
                Dim sScript As String = "ChangePassword('" & chkChangePassword.ClientID & "', '" & txtPassword.ClientID & "', '" & txtReenterPassword.ClientID & "', '" & rfvPassword1.ClientID & "', '" & rfvPassword2.ClientID & "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType, "changepass", sScript, True)
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("MS_User.aspx")
    End Sub

End Class
