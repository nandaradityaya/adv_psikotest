Imports System.Data
Imports CORESQL

Partial Class MS_UserRoleAssignment
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
        If Session("UserID") Is Nothing Then
            Response.Redirect("../SessionTimeout.aspx")
        End If
        LoginUserID = Session("UserID")
        Me.FormState = Request.QueryString("FormState")
        If Not Page.IsPostBack Then
            If Me.FormState = "Edit" Then

                Me.PrimaryKey = Request.QueryString("LoginUserID")
                LoadData()

                lblFormState.Text = "Edit User Role Assignment"
                LoadRoles()

                txtLoginUserID.Enabled = False
                txtLoginUserName.Enabled = False

            End If
        End If
    End Sub


    Private Sub LoadData()
        Using oSqlHelper As New MonyetSQLHelper()
            oSqlHelper.CommandText = "Select LoginUserID,LoginUserName From MS_User Where LoginUserID=@LoginUserID"

            oSqlHelper.CommandType = CommandType.Text
            oSqlHelper.AddParameter("@LoginUserID", PrimaryKey, SqlDbType.VarChar)

            Try
                Dim drow As DataRow = oSqlHelper.ExecuteDataRow
                If Not drow Is Nothing Then
                    txtLoginUserID.Text = drow("LoginUserID")
                    txtLoginUserName.Text = drow("LoginUserName")
                End If
            Catch ex As Exception
                ltlMessage.Text = "<div class=""alert alert-danger"">" & ex.Message & "</div>"
            End Try
        End Using

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SuccessFlag As Boolean = False
        Dim lstCheckedMenuID As New List(Of Integer)
        If Me.FormState = "Edit" Then
            Try
                Dim Flag As Boolean = False
                For i As Integer = 0 To cblRole.Items.Count - 1
                    If cblRole.Items(i).Selected Then
                        If Flag Then
                            Throw New Exception("Role yang dipilih hanya boleh satu")
                        Else
                            Flag = True
                        End If
                    End If
                Next
                Using oSqlHelper As New MonyetSQLHelper()
                    Try
                        oSqlHelper.OpenConnectionAndBeginTransaction()
                        oSqlHelper.CommandType = CommandType.Text

                        For i As Integer = 0 To cblRole.Items.Count - 1


                            If cblRole.Items(i).Selected = False Then
                                oSqlHelper.CommandText = "Delete From MS_UserRoleAssignment Where LoginUserID=@LoginUserID AND LoginRoleID=@LoginRoleID"
                                oSqlHelper.ClearParameters()
                                oSqlHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.VarChar)
                                oSqlHelper.AddParameter("@LoginRoleID", cblRole.Items(i).Value, SqlDbType.VarChar)
                                oSqlHelper.ExecuteNonQueryWithTransaction()

                            Else
                                oSqlHelper.CommandText = "IF Not Exists (Select 1 From MS_UserRoleAssignment Where LoginUserID=@LoginUserID AND LoginRoleID=@LoginRoleID) " &
                                                         "BEGIN " &
                                                         "Insert Into MS_UserRoleAssignment(LoginUserID,LoginRoleID,UserInput,UserEdit,TimeInput,TimeEdit) Values (@LoginUserID,@LoginRoleID,@UserInput,@UserInput,GETDATE(),GETDATE()) " &
                                                         "END "
                                oSqlHelper.ClearParameters()
                                oSqlHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.VarChar)
                                oSqlHelper.AddParameter("@LoginRoleID", cblRole.Items(i).Value, SqlDbType.VarChar)
                                oSqlHelper.AddParameter("@UserInput", Me.LoginUserID, SqlDbType.VarChar)
                                oSqlHelper.ExecuteNonQueryWithTransaction()
                            End If

                        Next
                        oSqlHelper.CommitTransaction()
                        SuccessFlag = True
                    Catch ex As Exception
                        oSqlHelper.RollBackTransaction()
                        Throw ex
                    Finally
                        oSqlHelper.CloseConnection()
                    End Try
                End Using
            Catch ex As Exception
                ltlMessage.Text = "<div class=""alert alert-danger"">" & ex.Message & "</div>"
            End Try

            If SuccessFlag = True Then
                Response.Redirect("MS_User.aspx")
            End If
        End If
    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("MS_User.aspx")
    End Sub

    Private Sub LoadRoles()
        Dim dtAllRoles As DataTable
        Try
            Using oHelper As New MonyetSQLHelper
                oHelper.CommandType = CommandType.Text

                oHelper.CommandText =
                "Select A.LoginRoleID,A.RoleDescs,Case When B.LoginRoleID IS NULL Then 'N' Else 'Y' End As Assigned " &
                "From MS_UserRole A " &
                "Left Join MS_UserRoleAssignment B on A.LoginRoleID=B.LoginRoleID AND B.LoginUserID=@LoginUserID  "

                oHelper.AddParameter("@LoginUserID", txtLoginUserID.Text, SqlDbType.VarChar)
                dtAllRoles = oHelper.ExecuteDataTable

                For i As Integer = 0 To dtAllRoles.Rows.Count - 1
                    cblRole.Items.Add(New ListItem(dtAllRoles.Rows(i)("RoleDescs").ToString, dtAllRoles.Rows(i)("LoginRoleID").ToString))

                    If dtAllRoles.Rows(i)("Assigned").ToString = "Y" Then
                        cblRole.Items(i).Selected = True
                    End If
                Next
            End Using
        Catch ex As Exception
            ltlMessage.Text = "<div class=""alert alert-danger"">" & ex.Message & "</div>"
        End Try
    End Sub

End Class
