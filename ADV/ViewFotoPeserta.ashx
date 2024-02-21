<%@ WebHandler Language="VB" Class="ViewAttachment" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Drawing
Imports System.Data

Public Class ViewAttachment : Implements IHttpHandler, IReadOnlySessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim r As HttpResponse = context.Response
        If Not context.Session("userid") Is Nothing Then
            Dim fotoPath As String = ConfigurationManager.AppSettings("SnapshotPath").ToString
            Dim sLoc As String = context.Request.QueryString("Loc")

            Dim sFileToWrite As String = ""

            If File.Exists(Path.Combine(fotoPath, sLoc)) Then
                sFileToWrite = Path.Combine(fotoPath, sLoc)
            Else
                r.Write("")
                Return
            End If

            r.WriteFile(sFileToWrite)
        End If
    End Sub

    Private Function ScaleImage(srcImage As Image, maxWidth As Integer, maxHeight As Integer) As Byte()
        Dim ratioX As Double = maxWidth / srcImage.Width
        Dim ratioY As Double = maxHeight / srcImage.Height
        Dim ratio As Double = Math.Min(ratioX, ratioY)
        Dim newWidth As Integer = srcImage.Width * ratio
        Dim newHeight As Integer = srcImage.Height * ratio
        Dim newimage As New Bitmap(newWidth, newHeight)

        Using g As Graphics = Graphics.FromImage(newimage)
            g.DrawImage(srcImage, 0, 0, newWidth, newHeight)
        End Using

        Using oStream As New MemoryStream
            newimage.Save(oStream, Drawing.Imaging.ImageFormat.Jpeg)
            Return oStream.GetBuffer()
        End Using

    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class