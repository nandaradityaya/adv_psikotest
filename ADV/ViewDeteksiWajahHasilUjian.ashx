<%@ WebHandler Language="VB" Class="ViewDeteksiWajahHasilUjian" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Drawing
Imports System.Data

Imports System.Net

Public Class ViewDeteksiWajahHasilUjian : Implements IHttpHandler, IReadOnlySessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim r As HttpResponse = context.Response
        If Not context.Session("userid") Is Nothing Then
            Dim fotoPath As String = ConfigurationManager.AppSettings("SnapshotPath").ToString
            Dim sLoc As String = context.Request.QueryString("Loc")


            Dim sFileToWrite As String = ""
            Try
                sFileToWrite = Path.Combine(fotoPath, sLoc)

                If File.Exists(sFileToWrite) Then
                    Dim url As String = "http://10.10.0.223/PCML/api/Image/Predict"

                    Dim postReq As WebRequest = WebRequest.Create(url)
                    postReq.Method = "POST"

                    Dim postData As String = "{""ModelType"": ""face"", ""ResultImage"": ""N"", ""base64"": """ + Convert.ToBase64String(File.ReadAllBytes(sFileToWrite)) + """}"
                    Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)

                    postReq.ContentType = "application/json"
                    postReq.ContentLength = byteArray.Length

                    Using dataStream As Stream = postReq.GetRequestStream()
                        dataStream.Write(byteArray, 0, byteArray.Length)
                    End Using

                    Using response As WebResponse = postReq.GetResponse()
                        Using dataStream As Stream = response.GetResponseStream()
                            Using reader As New StreamReader(dataStream)
                                Dim responseFromServer As String = reader.ReadToEnd()
								dim json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer)
			
                                Dim jumlahTerdeteksi = CLng(json("Data")("PersonCount"))
								
                                If jumlahTerdeteksi > 0 Then
                                    Return
                                Else
                                    r.Write("WAJAH TIDAK TERDETEKSI")
                                    Return
                                End If
                            End Using
                        End Using
                    End Using
                Else
                    Throw New FileNotFoundException("File not found: " + sFileToWrite)
                End If

            Catch ex As Exception
                r.Write(ex.Message + "<br/><br/>" + ex.StackTrace)
                Return
            End Try

        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class