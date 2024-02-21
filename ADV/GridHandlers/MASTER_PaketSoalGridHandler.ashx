﻿<%@ WebHandler Language="VB" Class="MASTER_PaketSoalGridHandler" %>

Imports System
Imports System.Web
Imports Ext.Net

Public Class MASTER_PaketSoalGridHandler : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'context.Response.ContentType = "text/plain"

        Dim prms As New StoreRequestParameters(context)

        ' Paging parameters:        
        Dim iDisplayLength As Integer = prms.Limit
        Dim iDisplayStart As Integer = prms.Start

        ' Sorting parameters
        Dim sSortCol As String = prms.Sort(0).Property
        Dim sSortDir As String = prms.Sort(0).Direction.ToString

        Dim oGridHelper As New GridHelper

        oGridHelper.ColumnsToDisplay = " * "
        oGridHelper.TableName = " ADVPSIKOTEST.dbo.VW_MASTER_PaketSoal "

        oGridHelper.WhereClause = ""
        Dim filter = context.Request("PaketSoalFilter")
        If Not filter Is Nothing Then
            oGridHelper.WhereClause += IIf(filter.Equals(""), "1=1 ", filter)
        End If
        Dim filterNoPeserta = context.Request("NoPesertaFilter")
        If Not filterNoPeserta Is Nothing AndAlso filterNoPeserta Then
            For Each NoPeserta In filterNoPeserta.Split(";")
                oGridHelper.WhereClause += " AND JobCodes LIKE (SELECT '%|' + IIF(LEN(CAST(B.LamarSebagai AS VARCHAR(3))) = 1, '0' + CAST(B.LamarSebagai AS VARCHAR(3)), CAST(B.LamarSebagai AS VARCHAR(3))) + '|%' FROM dbo.MS_Peserta B WHERE B.NoPeserta IN(" + NoPeserta + ")) "
            Next
        End If

        oGridHelper.SortColumn = sSortCol
        oGridHelper.SortDirection = sSortDir

        Dim fhc As FilterHeaderConditions = New FilterHeaderConditions(context.Request("filterheader"))

        For Each condition In fhc.Conditions
            Dim filterColumnName As String = condition.DataIndex
            Dim type As FilterType = condition.Type
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