<%@ WebHandler Language="VB" Class="MS_UserGridHandler" %>

Imports System
Imports System.Web

Public Class MS_UserGridHandler : Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If context.Session("UserID") Is Nothing Then
            context.Response.End()
        End If


        ' Paging parameters:
        Dim sEcho As Integer = context.Request("sEcho")
        Dim iDisplayLength As Integer = context.Request("iDisplayLength")
        Dim iDisplayStart As Integer = context.Request("iDisplayStart")

        ' Sorting parameters
        Dim iSortCol As Integer = context.Request("iSortCol_0")
        Dim sSortDir As String = context.Request("sSortDir_0")

        'Search parameters
        Dim sSearch As String = context.Request("sSearch")

        Dim oGridHelper As New GridHelper

        oGridHelper.ColumnsToDisplay = "LoginUserID,LoginUserName,EmailAddress,Remarks,TimeEdit"
        oGridHelper.TableName = "MS_User"

        oGridHelper.WhereClause = "Status = 'A'"

        oGridHelper.DefaultSortColumn = "LoginUserID"
        oGridHelper.SetFilterClause(sSearch)
        oGridHelper.SortColIndex = iSortCol
        oGridHelper.SortDirection = sSortDir
            
        context.Response.ContentType = "application/json"
        context.Response.Write(oGridHelper.GetJSONData(sEcho, iDisplayStart, iDisplayLength))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class