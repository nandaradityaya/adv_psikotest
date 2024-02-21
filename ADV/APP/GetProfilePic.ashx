<%@ WebHandler Language="VB" Class="GetProfilePic" %>

Imports System.IO
Imports System.Web


Public Class GetProfilePic : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState


    Public Sub ViewProfilePIC(ByVal ctx As HttpContext) Implements IHttpHandler.ProcessRequest
        'Try
        ' If ctx.Session("UserID") IsNot Nothing Then
        Dim FileName As String = ctx.Request.QueryString("UserID") & ".jpg"
        Dim FileLocation As String = Path.Combine(ConfigurationManager.AppSettings("SpecimenImagesPath").ToString, "PHOTO", FileName)


        If File.Exists(FileLocation) Then
            ctx.Response.Clear()
            ctx.Response.ContentType = "image/jpg"
            ctx.Response.AddHeader("Content-Disposition", String.Format("inline; filename={0}", FileName))
            ctx.Response.BinaryWrite(File.ReadAllBytes(FileLocation))
            ' ctx.Response.TransmitFile(FileLocation)
            ctx.Response.Flush()
            ctx.Response.Close()
        Else
            ctx.Response.Clear()
            ctx.Response.ContentType = "image/jpg"
            ctx.Response.AddHeader("Content-Disposition", "inline; filename={Unknown-person.jpg")
            ctx.Response.BinaryWrite(File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings("SpecimenImagesPath").ToString, "Unknown-person.jpg")))
            ctx.Response.TransmitFile(Path.Combine(ConfigurationManager.AppSettings("SpecimenImagesPath").ToString, "Unknown-person.jpg"))
            ctx.Response.Flush()
            ctx.Response.Close()
        End If
        '    ctx.Response.Write("Anda tidak berhak mengakses file ini!")
        '    ctx.Response.End()
        'End If
        ''Catch ex As Exception
        ''    ctx.Response.Write("Anda tidak berhak mengakses file ini!")
        ''    ctx.Response.End()
        ''End Try
    End Sub


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
