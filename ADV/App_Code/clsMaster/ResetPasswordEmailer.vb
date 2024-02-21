Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.Net
Imports System.Data

Public Class ResetPasswordEmailer

    Const FROM_EMAIL_ADDRESS As String = "dct@advantagescm.com"
    Const FROM_EMAIL_ADDRESS_DISPLAY_NAME As String = "DCT System"
    Const FROM_EMAIL_PASSWORD As String = "pass@word1adv"
    Const SMTP_HOST As String = "mail.advantagescm.com"
    Const SMTP_PORT As Integer = 447
    Const ENABLE_SSL As Boolean = True

    Public Sub SendResetPasswordEmail(ByVal ResetLink As String, ByVal VerificationCode As String, ByVal ToEmailAddress As String, ByVal ToEmailName As String)

        Dim message As New MailMessage

        Dim oEmailAddress As New MailAddress(ToEmailAddress, ToEmailName)
        message.To.Add(oEmailAddress)

        'If Not CCEmailAddress Is Nothing Then
        '    For Each addr As String In CCEmailAddress
        '        message.CC.Add(addr)
        '    Next
        'End If

        'If Not BCCEmailAddress Is Nothing Then
        '    For Each addr As String In BCCEmailAddress
        '        message.Bcc.Add(addr)
        '    Next
        'End If

        Dim MessageBody As New StringBuilder
        MessageBody.AppendLine("Anda telah meminta untuk me-reset password untuk aplikasi DCT WEB")
        MessageBody.AppendLine("Silahkan copy & paste link dibawah ke browser anda untuk melanjutkan proses reset password")
        MessageBody.AppendLine()
        MessageBody.AppendLine("URL DCTWEB http://" & ResetLink)
        MessageBody.AppendLine()
        MessageBody.AppendLine("Kemudian Masukkan Kode Ini di kotak isian yang disediakan : " & VerificationCode)
        MessageBody.AppendLine()
        MessageBody.AppendLine("Email pemberitahuan ini dikirim otomatis oleh system")
        MessageBody.AppendLine()
        MessageBody.AppendLine("Mohon untuk tidak membalas email ini, dan harap abaikan bila anda merasa tidak pernah meminta reset password")
        MessageBody.AppendLine()
        MessageBody.AppendLine("Regards,")
        MessageBody.AppendLine("Admin DCT")



        message.From = New MailAddress(FROM_EMAIL_ADDRESS, FROM_EMAIL_ADDRESS_DISPLAY_NAME)
        message.Subject = "Permohonan Reset Password Aplikasi DCT WEB PT.Advantage SCM"
        message.Body = MessageBody.ToString
        message.IsBodyHtml = False


        Dim credentials As New NetworkCredential
        credentials.UserName = FROM_EMAIL_ADDRESS
        credentials.Password = FROM_EMAIL_PASSWORD

        Dim client As New SmtpClient

        'client.DeliveryMethod = SmtpDeliveryMethod.Network
        client.Port = SMTP_PORT
        client.Host = SMTP_HOST
        client.UseDefaultCredentials = False
        client.EnableSsl = ENABLE_SSL
        client.Credentials = credentials
        client.Timeout = 100 * 600 '100 ms * 600 detik
        client.Send(message)

    End Sub



End Class
