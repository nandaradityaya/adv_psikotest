<%@ WebHandler Language="VB" Class="Handler" %>

Imports System.IO
Imports System.Web

Public Class Handler : Implements IHttpHandler, System.Web.SessionState.IReadOnlySessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If String.IsNullOrEmpty(context.Session("UserIDPsikotest")) Then
            context.Response.End()
        End If

        Dim ImgB64 As String = context.Request.Form("imageData")
        ImgB64 = ImgB64.Replace("data:image/png;base64,", "")
        Dim imageBytes As Byte() = Convert.FromBase64String(ImgB64)

        Dim UserID = context.Session("UserIDPsikotest")
        Dim savepath As String = Path.Combine(ConfigurationManager.AppSettings("SnapshotPath").ToString, UserID)

        Dim fileName As String = Date.Now.ToString("dd-MMM-yyyy--HH_mm_ss") + ".jpg"

        If Not Directory.Exists(savepath) Then
            Directory.CreateDirectory(savepath)
        End If

        Dim filePath As String = savepath + "/" + fileName

        File.WriteAllBytes(filePath, imageBytes)

        context.Response.Clear()
        context.Response.ClearHeaders()
        context.Response.ContentType = "text/plain"
        context.Response.Write(filePath)
        context.Response.End()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class