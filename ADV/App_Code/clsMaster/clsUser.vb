Imports System.Data
Imports System.Data.SqlClient

Public Class clsUser

    Private _uid As String
    Private _role As String
    Private _groupid As String
    Private _bLoginSuccess As Boolean = False
    Private _LastLoginDate As Date
    Private _Last_Password_Change_Date As Date

    Public ReadOnly Property UserID As String
        Get
            Return _uid
        End Get
    End Property

    Public ReadOnly Property LastLoginDate As Date
        Get
            Return _LastLoginDate
        End Get
    End Property

    Public ReadOnly Property Last_Password_Change_Date As Date
        Get
            Return _Last_Password_Change_Date
        End Get
    End Property

    Public ReadOnly Property IsLoginSuccess As Boolean
        Get
            Return _bLoginSuccess
        End Get
    End Property

    Public Sub Login(ByVal userid As String, ByVal password As String)
        Using conn As SqlConnection = clsConnDB.GetConnectionUM
            conn.Open()

            Dim sql As String = "EXEC spUM_LoginAuthenticationWeb @UserId, @Password"

            Dim sqlcmd As New SqlCommand(sql, conn)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userid.Trim
            sqlcmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = password.Trim

            Using rd As SqlDataReader = sqlcmd.ExecuteReader
                While rd.Read
                    _bLoginSuccess = True
                    _uid = rd("UserID").ToString

                    If rd("Last_Login_Date").ToString = "" Then
                        _LastLoginDate = Date.Now
                    Else
                        _LastLoginDate = rd("Last_Login_Date")
                    End If

                    _Last_Password_Change_Date = rd("Last_Password_Change_Date")
                End While
            End Using
        End Using
    End Sub

    Public Shared Function LookUpRole(ByVal UserID As String, ByVal _code_application As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = @CodeApplication AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID
            sqlcmd.Parameters.Add("@CodeApplication", SqlDbType.VarChar).Value = _code_application

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpRoleWeb(ByVal UserID As String, RoleID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_USER WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%" & RoleID & "[_]%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCabangCLAIM(ByVal UserID As String) As String
        Using cn As SqlConnection = clsConnDB.GetConnectionUM
            cn.Open()

            Using ocmd As New SqlCommand("SELECT GroupID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID", cn)
                ocmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID

                Return ocmd.ExecuteScalar
            End Using
        End Using
    End Function

    Public Shared Function LookUpRoleID(ByVal UserID As String, RoleID As String) As String
        If RoleID.Contains("AUDITDCT") = True Then
            Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
                ___SQLConnection.Open()
                Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '" & RoleID & "%'", ___SQLConnection)
                sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID
                Return CStr(sqlcmd.ExecuteScalar)
            End Using
        ElseIf clsDBDCT.GetDataTable("SELECT UserID, RoleID FROM UM_DB.dbo.UM_User WHERE Code_Application = 'WEB' AND UserID = '" & UserID & "'" &
                                      " AND (LEFT(RoleID,3) = 'HRD' OR LEFT(RoleID,3) = 'ROS' OR LEFT(RoleID,4)='FPTK' OR LEFT(RoleID,4)='FPDK' " &
                                      " OR LEFT(RoleID,3) = 'BAA' OR LEFT(RoleID,2) = 'FR' OR LEFT(RoleID,3) = 'SPO' OR LEFT(RoleID,2) = 'IZ') AND RoleID LIKE '" & RoleID.Replace("[_]", "") & "[_]%'  GROUP BY UserID, RoleID ").Rows.Count > 1 Then
            Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
                ___SQLConnection.Open()
                Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '" & RoleID & "[_]%' AND IsUse = 1 ", ___SQLConnection)
                sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID
                Return CStr(sqlcmd.ExecuteScalar)
            End Using
        ElseIf RoleID.Contains("_") = True Then
            Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
                ___SQLConnection.Open()
                Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '" & RoleID & "%'", ___SQLConnection)
                sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID
                Return CStr(sqlcmd.ExecuteScalar)
            End Using
        Else
            Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
                ___SQLConnection.Open()
                Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '" & RoleID & "[_]%'", ___SQLConnection)
                sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID
                Return CStr(sqlcmd.ExecuteScalar)
            End Using
        End If
    End Function
    Public Shared Function LookUpMultiRoleID(ByVal UserID As String, RoleID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            'Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%" & RoleID & "%'", ___SQLConnection)
            Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '" & RoleID & "%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID

            Dim da As New SqlDataAdapter(sqlcmd)
            Dim dt As New DataTable
            da.Fill(dt)

            Dim value As String = ""
            For i As Integer = 0 To dt.Rows.Count - 1
                value += dt.Rows(i).Item(0).ToString() & ","
            Next
            Return CStr(value)

        End Using
    End Function

    Public Shared Function LookUpName(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT [UserName] FROM  UM_User WHERE UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpRoleNew(ByVal UserID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT RoleID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpUserId(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT [UserID] FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpGroupIDBON(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("spUM_Groupss 'WEB', @UserID ", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpGroupDA(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT TOP 1 GroupID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%DA%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpGroupID(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT TOP 1 GroupID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpGroupIDDA(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT TOP 1 GroupID FROM UM_User WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpUserGroupID(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("spUM_Groups 'WEB', @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpUserGroupIDDA(ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("spUM_Groups 'WEB', @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = _empid

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCab(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function



    Public Shared Function LookUpCab(ByVal ID As String, ByVal RoleID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UM_User.RoleID='" & RoleID & "' AND UM_User.UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpMultiCab(ByVal ID As String, ByVal RoleID As String) As List(Of String)

        Using oSqlHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.UM)
            oSqlHelper.CommandType = CommandType.Text
            oSqlHelper.CommandText = "SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UM_User.RoleID=@1 AND UM_User.UserID = @2"
            oSqlHelper.AddParameter("@1", RoleID, SqlDbType.VarChar)
            oSqlHelper.AddParameter("@2", ID, SqlDbType.VarChar)

            Dim multiCab As New List(Of String)
            For Each dr As DataRow In oSqlHelper.ExecuteDataTable().Rows
                multiCab.Add(dr(0))
            Next

            Return multiCab
        End Using
    End Function

    Public Shared Function LookUpBranchHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%HRD%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpNameBranchHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT Branch From DA.dbo.HRD_Employee WHERE EmployeeCode = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpdivisionHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT B.Divisi FROM UM_User A INNER JOIN " &
                                         "UM_Employee B ON B.NIK = A.UserID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%HRD%' AND A.GroupID IN ('1','2','3')", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpCodedivisionHRD(ByVal _nik As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT CodeDivision From HRD_Employee Where EmployeeCode = @Nik", ___SQLConnection)
            sqlcmd.Parameters.Add("@Nik", SqlDbType.VarChar).Value = _nik

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpCabangHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT DISTINCT STUFF((SELECT ',''' + CAST(A.GroupID AS VARCHAR(50)) + '''' " &
                                         "FROM UM_User A WHERE A.Code_Application = 'WEB' AND A.Userid = @UserID AND A.RoleID LIKE '%HRD%' " &
                                          "FOR XML PATH('')),1,1,'') " &
                                         "FROM UM_User", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCabClaim(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE 'CLAIM%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCabSIMULATOR(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE 'ASS%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCabCPC(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.GroupID AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%CPC%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpDiv(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT DISTINCT Divisi FROM UM_User " &
                                         "INNER JOIN UM_Employee ON UM_Employee.NIK = UM_User.UserID WHERE UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpNameDiv(ByVal _Nik As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT B.Name FROM HRD_Employee A INNER JOIN HRD_MsDivision B ON B.Code=A.CodeDivision WHERE A.EmployeeCode = @Nik", ___SQLConnection)
            sqlcmd.Parameters.Add("@Nik", SqlDbType.VarChar).Value = _Nik

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpCodeSection(ByVal _nik As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT CodeSection From HRD_Employee WHERE EmployeeCode = @Nik", ___SQLConnection)
            sqlcmd.Parameters.Add("@Nik", SqlDbType.VarChar).Value = _nik

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpSubUnit(ByVal _nik As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT ISNULL(SubUnit, '') From HRD_Employee WHERE EmployeeCode = @Nik", ___SQLConnection)
            sqlcmd.Parameters.Add("@Nik", SqlDbType.VarChar).Value = _nik

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpSection(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT DISTINCT Sector FROM UM_User " &
                                         "INNER JOIN UM_Employee ON UM_Employee.NIK = UM_User.UserID WHERE UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpDepartment(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT DISTINCT Department FROM UM_User " &
                                         "INNER JOIN UM_Employee ON UM_Employee.NIK = UM_User.UserID WHERE UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpCodeDepartment(ByVal _nik As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT CodeDepartment From HRD_Employee WHERE EmployeeCode = @Nik", ___SQLConnection)
            sqlcmd.Parameters.Add("@Nik", SqlDbType.VarChar).Value = _nik

            Return CStr(MonyetSQLHelper.DBNull2BLank(sqlcmd.ExecuteScalar))
        End Using
    End Function

    Public Shared Function LookUpCabHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT NamaCabang = LEFT(B.Name,9) FROM UM_User A LEFT JOIN " &
                      "UM_Group B ON A.GroupID = B.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE '%HRD%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpBranch(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT Upper(UM_Group.Name) AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpCode(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT UM_Group.Code_Group AS Cabang FROM UM_User LEFT JOIN " &
                                         "UM_Group ON UM_User.GroupID = UM_Group.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function LookUpBranchCodeHRD(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnection
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT BranchID From HRD_Employee WHERE EmployeeCode = @UserID", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function Update(ByVal user As String, ByVal pass As String, ByVal oldpass As String) As Integer
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand
            sqlcmd.Connection = ___SQLConnection
            sqlcmd.CommandText = "Select [Password] From UM_User Where Code_Application = 'WEB' AND UserID = @UserID"
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = user

            Dim _oldpass As Object = sqlcmd.ExecuteScalar()
            Dim iResult As Integer = 0

            If IsDBNull(_oldpass) OrElse _oldpass.ToString.ToUpper <> oldpass.ToUpper Then
                iResult = 0
            Else
                iResult = 1

                sqlcmd.CommandText = "UPDATE UM_User SET [Password] = @pass, Last_Password_Change_Date=GetDate() WHERE Code_Application = 'WEB' AND UserID = @UserID"
                sqlcmd.Parameters.Clear()
                sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = user
                sqlcmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = pass
                sqlcmd.ExecuteNonQuery()
            End If

            Return iResult
        End Using
    End Function

    Public Shared Function LookUpCabBBM(ByVal ID As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("SELECT NamaCabang = LEFT(B.Name,7) FROM UM_User A LEFT JOIN " &
                      "UM_Group B ON A.GroupID = B.GroupID WHERE Code_Application = 'WEB' AND UserID = @UserID AND RoleID LIKE 'BBM%'", ___SQLConnection)
            sqlcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = ID

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function

    Public Shared Function RetrieveBranchAll() As DataSet
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            Dim da As New SqlDataAdapter("Select '%' AS GroupID,'ALL' AS Name UNION ALL SELECT CAST(GroupID AS VARCHAR(20)), Name FROM UM_Group", ___SQLConnection)
            Dim ds As New DataSet
            da.Fill(ds, "TBL_MS_BRANCH")
            Return ds
        End Using
    End Function

    Public Shared Function LookUpUserGroupRole(_Role As String, ByVal _empid As String) As String
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            ___SQLConnection.Open()

            Dim sqlcmd As New SqlCommand("spUM_Groups_Role 'WEB', '" & _Role & "', '" & _empid & "'", ___SQLConnection)

            Return CStr(sqlcmd.ExecuteScalar)
        End Using
    End Function


    Public Sub UpdateLastLoginDate()
        Using ___SQLConnection As SqlConnection = clsConnDB.GetConnectionUM
            Dim sqlcmd As New SqlCommand("Update UM_User Set Last_Login_Date=GetDate() Where Code_Application='WEB' AND UserID='" & _uid & "'", ___SQLConnection)
            ___SQLConnection.Open()
            sqlcmd.ExecuteNonQuery()
        End Using
    End Sub


    Public Shared Function GetAksesNews(ByVal _empid As String) As String
        Dim hasil As String = ""
        Try
            Using oSqlHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.ACTIVITIES)
                oSqlHelper.CommandType = CommandType.Text
                oSqlHelper.CommandText = "select distinct top 2 Akses from News_AuthorMember where nik = '" & _empid & "'"
                Dim dt As DataTable = oSqlHelper.ExecuteDataTable()
                For Each dr As DataRow In dt.Rows
                    hasil += dr.Item("Akses")
                Next
                Return hasil
            End Using
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class