<%@ WebHandler Language="VB" Class="MS_SysParameterGridhandler" %>

Imports System
Imports System.Web
Imports Ext.Net

Public Class MS_SysParameterGridhandler : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        ' Paging parameters:
        Dim prms As New StoreRequestParameters(context)

        ' Paging parameters:        
        Dim iDisplayLength As Integer = prms.Limit
        Dim iDisplayStart As Integer = prms.Start

        ' Sorting parameters
        Dim sSortCol As String = prms.Sort(0).Property
        Dim sSortDir As String = prms.Sort(0).Direction.ToString

        Dim oGridHelper As New GridHelper
        oGridHelper.ColumnsToDisplay = "*"
        Dim TableName As String = "VW_SYS_Parameter"

        oGridHelper.TableName = TableName
        Dim sSearch As String = context.Request("sSearch")

        oGridHelper.SortColumn = sSortCol
        oGridHelper.SortDirection = sSortDir

        Dim _where As String = ""

        oGridHelper.WhereClause = _where
        Dim fhc As FilterHeaderConditions = New FilterHeaderConditions(context.Request("filterheader"))

        For Each condition In fhc.Conditions
            Dim filterColumnName As String = condition.DataIndex
            Dim type As FilterType = condition.Type
            'Dim op As String = condition.Operator
            Dim filterColumnValue As String = condition.Value(Of String)()

            If type = FilterType.Date Then
                oGridHelper.AddSpecificFilterClause(filterColumnName, filterColumnValue, GridHelper.enSearchDataType.DataType_Date)
            Else
                oGridHelper.AddSpecificFilterClause(filterColumnName, filterColumnValue, GridHelper.enSearchDataType.DataType_StringAndNumber)
            End If
        Next
        context.Response.ContentType = "application/json"
        context.Response.Write(oGridHelper.GetJSONDataForExtJS(iDisplayStart, iDisplayLength))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class