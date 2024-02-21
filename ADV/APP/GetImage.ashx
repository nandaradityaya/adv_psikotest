<%@ WebHandler Language="VB" Class="GetImage" %>

Imports System
Imports System.Web
Imports System.IO

Public Class GetImage : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState

    Public Sub ViewProfilePIC(ByVal ctx As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            If ctx.Session("UserID") IsNot Nothing Then
                Dim FileName As String = ctx.Request.QueryString("FileName")
                Dim FileLocation As String = Path.Combine(ConfigurationManager.AppSettings("Magazine").ToString, FileName)

                If File.Exists(FileLocation) Then
                    ctx.Response.Clear()
                    If Right(FileName, 3) = "pdf" Then
                        ctx.Response.ContentType = "application/pdf"
                    Else
                        ctx.Response.ContentType = "image/jpg"
                    End If
                    ctx.Response.AddHeader("Content-Disposition", String.Format("inline; filename={0}", FileName))
                    ctx.Response.BinaryWrite(File.ReadAllBytes(FileLocation))
                    'ctx.Response.TransmitFile(FileLocation)
                    ctx.Response.Flush()
                    ctx.Response.Close()
                Else
                    ctx.Response.Clear()
                    ctx.Response.ContentType = "image/jpg"
                    ctx.Response.AddHeader("Content-Disposition", "inline; filename={Unknown-person.jpg")
                    ctx.Response.BinaryWrite(File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings("SpecimenImagesPath").ToString, "Unknown-person.jpg")))
                    'ctx.Response.TransmitFile(Path.Combine(ConfigurationManager.AppSettings("SpecimenImagesPath").ToString, "Unknown-person.jpg"))
                    ctx.Response.Flush()
                    ctx.Response.Close()
                End If
            Else
                ctx.Response.Write("Anda tidak berhak mengakses file ini!")
                ctx.Response.End()
            End If
        Catch ex As Exception
            ctx.Response.Write("Anda tidak berhak mengakses file ini!")
            ctx.Response.End()
        End Try
    End Sub


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class