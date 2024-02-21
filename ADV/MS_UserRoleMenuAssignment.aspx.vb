Imports System.Data

Partial Class MS_UserRoleMenuAssignment
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

                lblFormState.Text = "Edit Role Menu Assignment"
                LoadMenus()

                txtLoginRoleID.Enabled = False
                txtRoleDescs.Enabled = False

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
            oSqlHelper.CommandText = "Select LoginRoleID,RoleDescs From ADVPSIKOTEST.dbo.MS_UserRole Where LoginRoleID=@LoginRoleID"

            oSqlHelper.CommandType = CommandType.Text
            oSqlHelper.AddParameter("@LoginRoleID", PrimaryKey, SqlDbType.VarChar)

            Try
                Dim drow As DataRow = oSqlHelper.ExecuteDataRow
                If Not drow Is Nothing Then

                    txtLoginRoleID.Text = drow("LoginRoleID")
                    txtRoleDescs.Text = drow("RoleDescs")

                    'If Not IsDBNull(drow("HomeMenuID")) Then
                    '    ddlHomeMenuID.SelectedValue = drow("HomeMenuID")
                    'End If

                End If
            Catch ex As Exception
                ltlMessage.Text = ex.Message
            End Try
        End Using

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SuccessFlag As Boolean = False
        Dim lstCheckedMenuID As New List(Of Integer)


        Try

            If Me.FormState = "Edit" Then

                For Each L1Node As TreeNode In tvw1.Nodes

                    If L1Node.ShowCheckBox = True And L1Node.Checked = True Then
                        lstCheckedMenuID.Add(L1Node.Value)
                    End If

                    For Each L2Node As TreeNode In L1Node.ChildNodes

                        If L2Node.ShowCheckBox = True And L2Node.Checked = True Then
                            lstCheckedMenuID.Add(L2Node.Value)
                        End If


                        For Each L3Node As TreeNode In L2Node.ChildNodes
                            If L3Node.ShowCheckBox = True And L3Node.Checked = True Then
                                lstCheckedMenuID.Add(L3Node.Value)
                            End If
                        Next

                    Next
                Next
            End If

            Using oSqlHelper As New MonyetSQLHelper()
                Try
                    oSqlHelper.OpenConnectionAndBeginTransaction()

                    oSqlHelper.CommandType = CommandType.Text

                    oSqlHelper.CommandText = "Update ADVPSIKOTEST.dbo.MS_UserRole Set TimeEdit=GetDate(),UserEdit = @UserEdit Where LoginRoleID=@LoginRoleID"
                    oSqlHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)
                    oSqlHelper.AddParameter("@UserEdit", LoginUserID, SqlDbType.VarChar)
                    oSqlHelper.ExecuteNonQueryWithTransaction()

                    oSqlHelper.CommandText = "Delete From ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment Where LoginRoleID=@LoginRoleID"
                    oSqlHelper.ClearParameters()
                    oSqlHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)
                    oSqlHelper.ExecuteNonQueryWithTransaction()


                    oSqlHelper.CommandText = "Insert Into ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment (LoginRoleID,MenuID,UserInput,UserEdit,TimeInput,TimeEdit) " &
                                             "Values (@LoginRoleID,@MenuID,@UserInput,@UserInput,getdate(),getdate())"




                    For i As Integer = 0 To lstCheckedMenuID.Count - 1

                        oSqlHelper.ClearParameters()
                        oSqlHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)
                        oSqlHelper.AddParameter("@MenuID", lstCheckedMenuID(i), SqlDbType.Int)
                        oSqlHelper.AddParameter("@UserInput", LoginUserID, SqlDbType.VarChar)
                        oSqlHelper.ExecuteNonQueryWithTransaction()

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
            ltlMessage.Text = ex.Message
        End Try

        If SuccessFlag = True Then
            Response.Redirect("MS_UserRole.aspx")
        End If
    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("MS_UserRole.aspx")
    End Sub

    Private Sub LoadMenus()
        Dim dtAllMenu As DataTable


        Using oHelper As New MonyetSQLHelper
            oHelper.CommandType = CommandType.Text

            oHelper.CommandText = "Select A.MenuID,A.MenuLevel,A.ParentLevel1MenuID,A.ParentLevel2MenuID,A.MenuText,B.LoginRoleID From ADVPSIKOTEST.dbo.MS_Menu A " &
                                  "Left Join ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment B On A.MenuID=B.MenuID AND B.LoginRoleID=@LoginRoleID"
            oHelper.AddParameter("@LoginRoleID", txtLoginRoleID.Text, SqlDbType.VarChar)

            dtAllMenu = oHelper.ExecuteDataTable

        End Using

        Dim dRowLevel1() As DataRow = dtAllMenu.Select("MenuLevel=1")



        For Each L1Row As DataRow In dRowLevel1

            Dim L1Node As New TreeNode("&nbsp;&nbsp;" & L1Row("MenuText"), L1Row("MenuID"))
            L1Node.SelectAction = TreeNodeSelectAction.None
            tvw1.Nodes.Add(L1Node)

            Dim dRowLevel2() As DataRow = dtAllMenu.Select("MenuLevel=2 And ParentLevel1MenuID=" & L1Row("MenuID"))

            If dRowLevel2.Length > 0 Then
                For Each L2Row As DataRow In dRowLevel2

                    Dim L2Node As New TreeNode("&nbsp;&nbsp;" & L2Row("MenuText"), L2Row("MenuID"))
                    L2Node.SelectAction = TreeNodeSelectAction.None
                    L1Node.ChildNodes.Add(L2Node)

                    Dim dRowLevel3() As DataRow = dtAllMenu.Select("MenuLevel=3 And ParentLevel2MenuID=" & L2Row("MenuID"))

                    If dRowLevel3.Length > 0 Then
                        For Each L3Row As DataRow In dRowLevel3
                            Dim L3Node As New TreeNode("&nbsp;&nbsp;" & L3Row("MenuText"), L3Row("MenuID"))
                            L3Node.SelectAction = TreeNodeSelectAction.None
                            L2Node.ChildNodes.Add(L3Node)
                            L3Node.ShowCheckBox = True

                            If Not IsDBNull(L3Row("LoginRoleID")) Then
                                L3Node.Checked = True
                            End If

                        Next
                    Else
                        L2Node.ShowCheckBox = True
                        If Not IsDBNull(L2Row("LoginRoleID")) Then
                            L2Node.Checked = True
                        End If
                    End If

                Next
            Else
                L1Node.ShowCheckBox = True
                If Not IsDBNull(L1Row("LoginRoleID")) Then
                    L1Node.Checked = True
                End If
            End If


        Next



    End Sub


End Class
