Imports System.Data

Partial Class MS_UserRoleAddEdit
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

#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckUserPermission()
        LoginUserID = Session("UserID")

        Me.FormState = Request.QueryString("FormState")

        If Not Page.IsPostBack Then


            If Me.FormState = "Edit" Then

                Me.PrimaryKey = Request.QueryString("LoginRoleID")
                LoadData()

                txtLoginRoleID.Enabled = False
                lblFormState.Text = "Edit Role"

            ElseIf Me.FormState = "Add" Then

                lblFormState.Text = "Add Role"

            End If


        End If


    End Sub
    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub



    Private Sub LoadData()

        Using oSqlHelper As New MonyetSQLHelper()
            oSqlHelper.CommandText = "Select LoginRoleID,RoleDescs,Remarks From ADVPSIKOTEST.dbo.MS_UserRole Where LoginRoleID=@LoginRoleID"

            oSqlHelper.CommandType = CommandType.Text
            oSqlHelper.AddParameter("@LoginRoleID", PrimaryKey, SqlDbType.VarChar)

            Try
                Dim drow As DataRow = oSqlHelper.ExecuteDataRow
                If Not drow Is Nothing Then

                    txtLoginRoleID.Text = drow("LoginRoleID")
                    txtRoleDescs.Text = drow("RoleDescs")
                    txtRemarks.Text = drow("Remarks")


                    'Me.LastUpdateStamp = drow("LastUpdateStamp")

                End If
            Catch ex As Exception
                ltlMessage.Text = ex.Message
            End Try
        End Using

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SuccessFlag As Boolean = False



        Try
            Using oSqlHelper As New MonyetSQLHelper()

                If Me.FormState = "Add" Then



                    oSqlHelper.CommandText = "Insert Into ADVPSIKOTEST.dbo.MS_UserRole " &
                                             "(LoginRoleID,RoleDescs,Remarks,UserInput,UserEdit,TimeInput,TimeEdit) " &
                                             "Values " &
                                             "(@LoginRoleID,@RoleDescs,@Remarks,@UserInput,@UserInput,getdate(),getdate())"

                    oSqlHelper.CommandType = CommandType.Text
                    oSqlHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@RoleDescs", txtRoleDescs.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@Remarks", txtRemarks.Text, SqlDbType.VarChar)

                    oSqlHelper.AddParameter("@UserInput", LoginUserID, SqlDbType.VarChar)

                ElseIf Me.FormState = "Edit" Then

                    oSqlHelper.CommandText = "Update ADVPSIKOTEST.dbo.MS_UserRole SET " &
                                                "	 LoginRoleID = @LoginRoleID " &
                                                "	,RoleDescs = @RoleDescs " &
                                                "	,Remarks = @Remarks " &
                                                "	,UserEdit= @UserEdit " &
                                                "	,TimeEdit = getdate() " &
                                                "Where LoginRoleID = @LoginRoleID "

                    oSqlHelper.CommandType = CommandType.Text
                    oSqlHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@RoleDescs", txtRoleDescs.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@Remarks", txtRemarks.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@UserEdit", LoginUserID, SqlDbType.VarChar)

                End If

                oSqlHelper.ExecuteNonQuery()
                SuccessFlag = True
            End Using
        Catch ex As Exception
            ltlMessage.Text = ex.Message
        End Try

        If SuccessFlag = True Then
            Response.Redirect("MS_UserRole.aspx")
        End If
    End Sub




    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("MS_UserRole.aspx")
    End Sub

End Class
