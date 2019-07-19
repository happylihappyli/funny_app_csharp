Imports System.Security.Cryptography
Imports System.Text

Namespace Funny
    Friend Class C_MD5

        Public Shared Function MD5(ByVal SourceText As String) As String
            'Create an encoding object to ensure the encoding standard for the source text
            Dim Ue As New UnicodeEncoding
            'Retrieve a byte array based on the source text
            Dim ByteSourceText() As Byte = Ue.GetBytes(SourceText)
            'Instantiate an MD5 Provider object
            Dim pMd5 As New MD5CryptoServiceProvider
            'Compute the hash value from the source
            Dim ByteHash() As Byte = pMd5.ComputeHash(ByteSourceText)
            'And convert it to String format for return
            Return Convert.ToBase64String(ByteHash)
        End Function

        'MD5 加密函数
        Public Shared Function MD5(ByVal strSource As String, ByVal Code As Int16, ByVal bZero As Boolean) As String
            If strSource = "" Then Return "none"
            '这里用的是ascii编码密码原文，如果要用汉字做密码，可以用UnicodeEncoding，但会与ASP中的MD5函数不兼容
            Dim dataToHash As Byte() = (New System.Text.ASCIIEncoding).GetBytes(strSource)
            Dim hashvalue As Byte() = CType(System.Security.Cryptography.CryptoConfig.CreateFromName("MD5"), System.Security.Cryptography.HashAlgorithm).ComputeHash(dataToHash)
            Dim strTmp As String = ""
            Dim strReturn As String = ""
            Select Case Code
                Case 16  '选择16位字符的加密结果
                    For i As Integer = 4 To 11
                        strReturn += Hex(hashvalue(i)).ToLower
                    Next
                Case 32  '选择32位字符的加密结果
                    For i As Integer = 0 To 15
                        strTmp = Hex(hashvalue(i)).ToLower
                        If bZero = True AndAlso strTmp.Length = 1 Then
                            strTmp = "0" & strTmp
                        End If
                        strReturn += strTmp
                    Next
                Case Else   'Code错误时，返回全部字符串，即32位字符
                    For i As Integer = 0 To hashvalue.Length - 1
                        strReturn += Hex(hashvalue(i)).ToLower
                    Next
            End Select

            Return strReturn
        End Function
    End Class

End Namespace



