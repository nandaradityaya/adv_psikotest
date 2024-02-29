Imports System.Configuration
Imports System.IO
Imports Adv
Imports System.Data
Imports System.Web
Imports System.Net

Partial Class ADVNet
    Inherits System.Web.UI.MasterPage

    Public SiteRoot As String
    Public oes As New Encryption64
    'Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
    '    LblVersion.Text = ConfigurationManager.AppSettings("AppVer").ToString
    'End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'setupSaldoKredit()
            CheckUserPermission()
            CreateMenuAndBreadcrumbs()
            lblName.Text = Session("UserName")
            'lblUID.Text = Session("UserID")

            Session("BaseURL") = ConfigurationManager.AppSettings("BaseURL").ToString
        End If

    End Sub
    Private Sub CheckUserPermission()
        If Session("UserID") Is Nothing Or Session("mADMIN") = False Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Function ActiveMenu(ByVal url As String, ParamArray pages() As String) As String
        Dim Respon As String = ""

        If pages.Contains(Path.GetFileName(url)) Then
            Respon = "class=""active"""
        Else
            Respon = ""
        End If

        'If Path.GetFileName(url).ToUpper = Page.ToUpper Then
        '	Respon = "class=""active"""
        'Else
        '	Respon = ""
        'End If

        Return Respon
    End Function

    Private Sub CreateMenuAndBreadcrumbs()

        Dim sbMenu As New StringBuilder
        Dim BreadCrumbText As String = ""

        Dim CurrentPath As String

        If ConfigurationManager.AppSettings("SiteRoot") = "" Then
            CurrentPath = Strings.Right(Request.Path, Len(Request.Path) - Len(ConfigurationManager.AppSettings("SiteRoot")) - 1)
        Else
            Dim i As Integer = Request.Path.LastIndexOf("/")
            CurrentPath = Strings.Right(Request.Path, Len(Request.Path) - Request.Path.LastIndexOf("/") - 1)
        End If

        Using oHelper As New clsSQLHelper
            oHelper.CommandType = CommandType.Text

            Dim dRowActiveMenu As DataRow = Nothing

            Dim ActivePath As String = ""
            Dim P1ActiveMenuID As String = ""
            Dim P2ActiveMenuID As String = ""


            oHelper.CommandText =
            "Select REPLACE(B.LocationURL, 'KUIS/', '') LocationURL, P1.MenuID As P1ActiveMenuID, P2.MenuID As P2ActiveMenuID, B.MenuLevel, B.MenuText, B.MenuIcon, P1.MenuText As P1MenuText, P1.MenuIcon As P1MenuIcon, P2.MenuText As P2MenuText, P2.MenuIcon As P2MenuIcon " &
            "From MS_MenuPages A " &
            "Inner Join MS_Menu B On A.MenuID=B.MenuID " &
            "Left Join MS_Menu P1 On B.ParentLevel1MenuID = P1.MenuID " &
            "Left Join MS_Menu P2 On B.ParentLevel2MenuID = P2.MenuID " &
            "WHERE SUBSTRING(A.LocationURL, CHARINDEX('/',A.LocationURL)+1,LEN(A.LocationURL)) ='" & CurrentPath & "' " &
            "UNION ALL " &
            "Select REPLACE(A.LocationURL, 'KUIS/', '') LocationURL,P1.MenuID as P1ActiveMenuID,P2.MenuID as P2ActiveMenuID,A.MenuLevel,A.MenuText,A.MenuIcon,P1.MenuText as P1MenuText,P1.MenuIcon as P1MenuIcon,P2.MenuText as P2MenuText,P2.MenuIcon as P2MenuIcon " &
            "From ADVPSIKOTEST.dbo.MS_Menu A " &
            "Left Join ADVPSIKOTEST.dbo.MS_Menu P1 On A.ParentLevel1MenuID = P1.MenuID " &
            "Left Join ADVPSIKOTEST.dbo.MS_Menu P2 On A.ParentLevel2MenuID = P2.MenuID " &
            "WHERE SUBSTRING(A.LocationURL,CHARINDEX('/',A.LocationURL)+1,LEN(A.LocationURL)) ='" & CurrentPath & "'"

            dRowActiveMenu = oHelper.ExecuteDataRow

            If dRowActiveMenu Is Nothing Then
                oHelper.CommandText =
                "Select REPLACE(B.LocationURL, 'KUIS/', '') LocationURL, P1.MenuID As P1ActiveMenuID, P2.MenuID As P2ActiveMenuID, B.MenuLevel, B.MenuText, B.MenuIcon, P1.MenuText As P1MenuText, P1.MenuIcon As P1MenuIcon, P2.MenuText As P2MenuText, P2.MenuIcon As P2MenuIcon " &
                "From MS_MenuPages A " &
                "Inner Join MS_Menu B On A.MenuID=B.MenuID " &
                "Left Join MS_Menu P1 On B.ParentLevel1MenuID = P1.MenuID " &
                "Left Join MS_Menu P2 On B.ParentLevel2MenuID = P2.MenuID " &
                "WHERE RIGHT(A.LocationURL, CHARINDEX('/',REVERSE(A.LocationURL))-1) ='" & CurrentPath & "' " &
                "UNION ALL " &
                "Select REPLACE(A.LocationURL, 'KUIS/', '') LocationURL,P1.MenuID as P1ActiveMenuID,P2.MenuID as P2ActiveMenuID,A.MenuLevel,A.MenuText,A.MenuIcon,P1.MenuText as P1MenuText,P1.MenuIcon as P1MenuIcon,P2.MenuText as P2MenuText,P2.MenuIcon as P2MenuIcon " &
                "From ADVPSIKOTEST.dbo.MS_Menu A " &
                "Left Join ADVPSIKOTEST.dbo.MS_Menu P1 On A.ParentLevel1MenuID = P1.MenuID " &
                "Left Join ADVPSIKOTEST.dbo.MS_Menu P2 On A.ParentLevel2MenuID = P2.MenuID " &
                "WHERE RIGHT(A.LocationURL, CHARINDEX('/',REVERSE(A.LocationURL))-1) ='" & CurrentPath & "'"


                ' lblSaldoKredit.Text = oHelper.CommandText
                ' Return

                dRowActiveMenu = oHelper.ExecuteDataRow
            End If

            'If Not dRowActiveMenu Is Nothing Then
            '    ActivePath = MonyetSQLHelper.DBNull2BLank(dRowActiveMenu("LocationURL"))
            '    P1ActiveMenuID = MonyetSQLHelper.DBNull2BLank(dRowActiveMenu("P1ActiveMenuID"))
            '    P2ActiveMenuID = MonyetSQLHelper.DBNull2BLank(dRowActiveMenu("P2ActiveMenuID"))
            'End If

            'If dRowActiveMenu("MenuLevel") = 1 Then
            '    BreadCrumbText = "<li class=""active""><a href=""../" & dRowActiveMenu("LocationURL") & """><i class=""fa " & dRowActiveMenu("MenuIcon") & """></i> " & dRowActiveMenu("MenuText") & "</a></li>"
            'ElseIf dRowActiveMenu("MenuLevel") = 2 Then
            '    BreadCrumbText = "<li><i class=""fa " & dRowActiveMenu("P1MenuIcon") & """></i> " & dRowActiveMenu("P1MenuText") & "</li>"
            '    BreadCrumbText &= "<li class=""active""><a href=""../" & dRowActiveMenu("LocationURL") & """> " & dRowActiveMenu("MenuText") & "</a></li>"

            'ElseIf dRowActiveMenu("MenuLevel") = 3 Then
            '    BreadCrumbText = "<li><i class=""fa " & dRowActiveMenu("P1MenuIcon") & """></i> " & dRowActiveMenu("P1MenuText") & "</li>"
            '    BreadCrumbText &= "<li>" & dRowActiveMenu("P2MenuText") & "</li>"
            '    BreadCrumbText &= "<li class=""active""><a href=""../" & dRowActiveMenu("LocationURL") & """>" & dRowActiveMenu("MenuText") & "</a></li>"
            'End If




            'ini untuk ALL
            'oHelper.CommandText = "Select MenuID,MenuOrder,MenuLevel,ParentLevel1MenuID,ParentLevel2MenuID,MenuText,MenuIcon,LocationURL From MS_Menu Where ShowMenu='Y' Order By MenuOrder"

            'Ini Sesuai Role
            oHelper.CommandText = "Select Q.MenuID,Q.MenuOrder,Q.MenuLevel,Q.ParentLevel1MenuID,Q.ParentLevel2MenuID,Q.MenuText,Q.MenuIcon,REPLACE(Q.LocationURL, 'KUIS/', '') LocationURL " & vbCrLf
            oHelper.CommandText += "From ADVPSIKOTEST.dbo.MS_Menu Q " & vbCrLf
            oHelper.CommandText += "Inner Join  " & vbCrLf
            oHelper.CommandText += "( " & vbCrLf
            oHelper.CommandText += "	Select B.MenuID  " & vbCrLf
            oHelper.CommandText += "	From ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment A   " & vbCrLf
            oHelper.CommandText += "	Inner Join ADVPSIKOTEST.dbo.MS_Menu B on A.MenuID=B.MenuID   " & vbCrLf
            'oHelper.CommandText += "	Inner Join MS_UserRoleAssignment U On A.LoginRoleID = U.LoginRoleID  " & vbCrLf
            oHelper.CommandText += "	Where A.LoginRoleID='" & Session("RoleID") & "' " & vbCrLf
            oHelper.CommandText += "	UNION    " & vbCrLf
            oHelper.CommandText += "	Select B.ParentLevel2MenuID AS MenuID  " & vbCrLf
            oHelper.CommandText += "	From ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment A   " & vbCrLf
            oHelper.CommandText += "	Inner Join ADVPSIKOTEST.dbo.MS_Menu B on A.MenuID=B.MenuID   " & vbCrLf
            'oHelper.CommandText += "	Inner Join MS_UserRoleAssignment U On A.LoginRoleID = U.LoginRoleID  " & vbCrLf
            oHelper.CommandText += "	Where A.LoginRoleID='" & Session("RoleID") & "' " & vbCrLf
            oHelper.CommandText += "	AND B.ParentLevel2MenuID IS NOT NULL  " & vbCrLf
            oHelper.CommandText += "	UNION   " & vbCrLf
            oHelper.CommandText += "	Select B.ParentLevel1MenuID AS MenuID  " & vbCrLf
            oHelper.CommandText += "	From ADVPSIKOTEST.dbo.MS_UserRoleMenuAssignment A   " & vbCrLf
            oHelper.CommandText += "	Inner Join ADVPSIKOTEST.dbo.MS_Menu B on A.MenuID=B.MenuID   " & vbCrLf
            'oHelper.CommandText += "	Inner Join MS_UserRoleAssignment U On A.LoginRoleID = U.LoginRoleID  " & vbCrLf
            oHelper.CommandText += "	Where A.LoginRoleID='" & Session("RoleID") & "'  " & vbCrLf
            oHelper.CommandText += "	AND B.ParentLevel1MenuID IS NOT NULL  " & vbCrLf
            oHelper.CommandText += ") R On Q.MenuID = R.MenuID " & vbCrLf
            oHelper.CommandText += "Where Q.ShowMenu='Y' Order By Q.MenuOrder " & vbCrLf



            Dim dt As DataTable = oHelper.ExecuteDataTable

            Dim drMenuLevel1() As DataRow = dt.Select("MenuLevel=1")

            For i As Integer = 0 To UBound(drMenuLevel1)

                Dim drMenuLevel2() As DataRow = dt.Select("MenuLevel=2 And ParentLevel1MenuID=" & drMenuLevel1(i)("MenuID"))


                If drMenuLevel2.Length = 0 Then

                    If ActivePath = drMenuLevel1(i)("LocationUrl") Then
                        sbMenu.AppendLine("<li class=""active""><a href=""../../" & drMenuLevel1(i)("LocationUrl") & """><i class=""menu-icon fa " & drMenuLevel1(i)("MenuIcon") & """></i> <span class=""menu-text""> " & drMenuLevel1(i)("MenuText") & " </span></a> </li>")
                    Else
                        sbMenu.AppendLine("<li class=""""><a href=""../../" & drMenuLevel1(i)("LocationUrl") & """><i class=""menu-icon fa " & drMenuLevel1(i)("MenuIcon") & """></i> <span class=""menu-text""> " & drMenuLevel1(i)("MenuText") & " </span></a> </li>")
                    End If


                Else

                    If drMenuLevel1(i)("MenuID").ToString = P1ActiveMenuID Or ActivePath = drMenuLevel1(i)("LocationUrl").ToString Then
                        sbMenu.AppendLine("<li class=""open active""><a class=""dropdown-toggle"" href=""../../" & drMenuLevel1(i)("LocationUrl") & """><i class=""menu-icon fa " & drMenuLevel1(i)("MenuIcon") & """></i> <span class=""menu-text""> " & drMenuLevel1(i)("MenuText") & " </span> <b class=""arrow fa fa-angle-down""></b>  </a>")
                    Else
                        sbMenu.AppendLine("<li class=""""><a class=""dropdown-toggle"" href=""../../" & drMenuLevel1(i)("LocationUrl") & """><i class=""menu-icon fa " & drMenuLevel1(i)("MenuIcon") & """></i> <span class=""menu-text""> " & drMenuLevel1(i)("MenuText") & " </span> <b class=""arrow fa fa-angle-down""></b> </a>")
                    End If

                    sbMenu.AppendLine("<ul class=""submenu"">")

                    For j As Integer = 0 To UBound(drMenuLevel2)

                        Dim drMenuLevel3() As DataRow = dt.Select("MenuLevel=3 And ParentLevel2MenuID=" & drMenuLevel2(j)("MenuID"))

                        If drMenuLevel3.Length = 0 Then


                            If drMenuLevel2(j)("MenuID").ToString = P2ActiveMenuID Or ActivePath = drMenuLevel2(j)("LocationURL").ToString Then
                                sbMenu.AppendLine("<li class=""active""> <a href=""../../" & drMenuLevel2(j)("LocationURL").ToString & """><i class=""menu-icon fa " & drMenuLevel2(j)("MenuIcon") & """> </i> " & drMenuLevel2(j)("MenuText").ToString & " </a> </li>")
                            Else
                                sbMenu.AppendLine("<li class=""""> <a href=""../../" & drMenuLevel2(j)("LocationURL").ToString & """><i class=""menu-icon fa " & drMenuLevel2(j)("MenuIcon") & """></i> " & drMenuLevel2(j)("MenuText").ToString & " </a> </li>")
                            End If

                        Else
                            If drMenuLevel2(j)("MenuID").ToString = P2ActiveMenuID Then
                                sbMenu.AppendLine("<li class=""open active""><a class=""dropdown-toggle"" href=""#""><i class=""menu-icon fa " & drMenuLevel2(j)("MenuIcon") & """> </i> " & drMenuLevel2(j)("MenuText").ToString & " <b class=""arrow fa fa-angle-down""></b> </a>")
                            Else
                                sbMenu.AppendLine("<li class=""""><a  class=""dropdown-toggle"" href=""#""><i class=""menu-icon fa " & drMenuLevel2(j)("MenuIcon") & """> </i> " & drMenuLevel2(j)("MenuText").ToString & " <b class=""arrow fa fa-angle-down""></b> </a>")
                            End If


                            sbMenu.AppendLine("<ul class=""submenu"">")

                            For k As Integer = 0 To UBound(drMenuLevel3)
                                If ActivePath = drMenuLevel3(k)("LocationURL").ToString Then
                                    sbMenu.AppendLine("<li class=""active""><a href=""../../" & drMenuLevel3(k)("LocationURL").ToString & """><i class=""menu-icon fa " & drMenuLevel3(k)("MenuIcon") & """></i><span style=""white-space: normal;"">" & drMenuLevel3(k)("MenuText").ToString & "</span></a></li>")
                                Else
                                    sbMenu.AppendLine("<li><a href=""../../" & drMenuLevel3(k)("LocationURL").ToString & """><i Class=""menu-icon fa " & drMenuLevel3(k)("MenuIcon") & """></i><span style=""white-space: normal;"">" & drMenuLevel3(k)("MenuText").ToString & "</span></a></li>")
                                End If

                            Next

                            sbMenu.AppendLine("</ul>")
                            sbMenu.AppendLine("</li>")
                        End If




                    Next

                    sbMenu.AppendLine("</ul>")
                    sbMenu.AppendLine("</li>")
                End If

            Next

        End Using

        'ltlmenu1.Text = sbMenu.ToString
        'ltlBreadCrumb.Text = BreadCrumbText
    End Sub
    'Private Sub setupSaldoKredit()

    '    Dim saldoKreditLng As Long = 0
    '    Dim batasSaldoMerah As Long = 0
    '    Dim biayaPerUndang As Long = 0
    '    Try
    '        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ' Untuk hindarin error: The underlying connection was closed: An unexpected error occurred on a send.

    '        Dim App_Id = "bvbte-l7s13pq32tpmowc"
    '        Dim Secret_Key = "2d9ed9339e6768d68eb4344fa6758371"
    '        Dim url As String = "https://multichannel-api.qiscus.com/api/v2/admin/wa_pricing/balance"

    '        Dim postReq As WebRequest = WebRequest.Create(url)
    '        postReq.Method = "GET"

    '        Dim webHeader As WebHeaderCollection = postReq.Headers
    '        webHeader.Add("Qiscus-App-Id", App_Id)
    '        webHeader.Add("Qiscus-Secret-Key", Secret_Key)

    '        Dim response As WebResponse = postReq.GetResponse
    '        Dim dataStream As Stream = response.GetResponseStream
    '        Dim reader As New StreamReader(dataStream)
    '        Dim responseFromServer As String = reader.ReadToEnd

    '        Dim json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer)

    '        saldoKreditLng = CLng(json("data")("channels")(0)("balance").ToString)

    '        reader.Close()
    '        dataStream.Close()
    '        response.Close()

    '        Using oHelper As New clsSQLHelper
    '            oHelper.CommandType = CommandType.Text

    '            oHelper.CommandText = "SELECT Value FROM SYS_Parameter WHERE Name = 'Batas Saldo Merah Qiscus'"
    '            batasSaldoMerah = CLng(oHelper.ExecuteScalar)

    '            oHelper.CommandText = "SELECT Value FROM SYS_Parameter WHERE Name = 'Biaya per Undangan'"
    '            biayaPerUndang = CLng(oHelper.ExecuteScalar)
    '        End Using
    '    Catch ex As Exception
    '        lblSaldoKredit.Text = ex.Message + vbCrLf + ex.StackTrace
    '        Return
    '    End Try

    '    Dim saldoKredit As String = CLng(saldoKreditLng).ToString("#,##0")
    '    Dim jumlahOrgBisaDiundang As String = Math.Floor(saldoKredit / biayaPerUndang).ToString

    '    lblSaldoKredit.Text = "<span style=""color: "
    '    If saldoKredit > batasSaldoMerah Then
    '        lblSaldoKredit.Text += "#33FF00;"">WhatsApp Kredit: " + saldoKredit + " (Bisa Mengundang " + jumlahOrgBisaDiundang + " Orang)</span>"
    '    Else
    '        lblSaldoKredit.Text += "#FFBC00;"">WhatsApp Kredit: " + saldoKredit + " (Bisa Mengundang " + jumlahOrgBisaDiundang + " Orang)</span>"
    '    End If
    'End Sub

End Class
