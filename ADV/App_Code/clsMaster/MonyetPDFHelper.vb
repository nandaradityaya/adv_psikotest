Imports System.Diagnostics
Imports System.IO

Public Class MonyetPDFHelper

    'Helper class untuk convert dari Docx/ODT/lain-lain ke PDF.
    'Caranya adalah menjalankan LibreOffice secara command line untuk open dan save (Jadi harus install libreoffice dulu)
    'File Input dan File Output akan terbuat di folder yang sama. Namanya sama hanya beda extension
    'Source : https://www.libreofficehelp.com/batch-convert-writer-documents-pdf-libreoffice/

    Const CONST_LIBREOFFICE_WRITER_PATH As String = "C:\Program Files\LibreOffice\program\swriter.exe"


    Public Shared Sub ConvertToPDF(InputOutputDirectory As String, SourceFileName As String)


        Using oProcess As New Process

            Dim oStartInfo As New ProcessStartInfo



            If Not System.IO.File.Exists(CONST_LIBREOFFICE_WRITER_PATH) Then
                Throw New Exception("LibreOffice Executable File Not Found")
            End If

            If Not System.IO.Directory.Exists(InputOutputDirectory) Then
                Throw New ArgumentException("Input/Output Directory Not Found")
            End If

            If Not System.IO.File.Exists(InputOutputDirectory & Path.DirectorySeparatorChar & SourceFileName) Then
                Throw New ArgumentException("Input File Not Found in Input/Output Directory")
            End If

            'https://ask.libreoffice.org/t/not-able-to-launch-soffice-within-an-application-on-the-iis-server/39184/7
            'intinya, dia perlu Path untuk userprofile dari user yang run libreoffice tersebut (kayaknya untuk temporary file)
            'karena usernya IIS itu gak ada path userprofile kayak user biasa, jadinya kita pakai argument
            '-env:UserInstallation=file:///D:/TEMP
            Dim UserProfilePath As String = InputOutputDirectory.Replace("\", "/")


            oStartInfo.FileName = CONST_LIBREOFFICE_WRITER_PATH
            oStartInfo.WorkingDirectory = InputOutputDirectory 'pakai working directory
            oStartInfo.Arguments = "-env:UserInstallation=file:///" & UserProfilePath & "  --headless --convert-to pdf " & SourceFileName

            oStartInfo.UseShellExecute = False
            oStartInfo.RedirectStandardError = True
            oStartInfo.CreateNoWindow = True

            oProcess.StartInfo = oStartInfo
            oProcess.Start()

            Dim sError As String


            Using oStreamReader As System.IO.StreamReader = oProcess.StandardError
                sError = oStreamReader.ReadToEnd()

                If sError <> "" Then
                    Throw New Exception(sError)
                End If

            End Using

        End Using

    End Sub




End Class
