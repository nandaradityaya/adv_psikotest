Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography

Public Class Encryption64
    Private key() As Byte = {}
    Private IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

    Public Function Decrypt(ByVal stringToDecrypt1 As String,
        ByVal sEncryptionKey As String) As String
        Dim stringToDecrypt As String = Replace(stringToDecrypt1, " ", "+")
        If Not String.IsNullOrEmpty(stringToDecrypt) Then
            Dim inputByteArray(stringToDecrypt.Length) As Byte
            Try
                key = System.Text.Encoding.UTF8.GetBytes(Left(sEncryptionKey, 8))
                Dim des As New DESCryptoServiceProvider()
                inputByteArray = Convert.FromBase64String(stringToDecrypt)
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV),
                    CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
                Return encoding.GetString(ms.ToArray())
            Catch e As Exception
                Return e.Message
            End Try
        End If
        Return String.Empty
    End Function

    Public Function Encrypt(ByVal stringToEncrypt As String,
        ByVal SEncryptionKey As String) As String
        If Not String.IsNullOrEmpty(stringToEncrypt) Then
            Try
                key = System.Text.Encoding.UTF8.GetBytes(Left(SEncryptionKey, 8))
                Dim des As New DESCryptoServiceProvider()
                Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(
                    stringToEncrypt)
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV),
                    CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Return Convert.ToBase64String(ms.ToArray())
            Catch e As Exception
                Return e.Message
            End Try
        End If
        Return String.Empty
    End Function

End Class