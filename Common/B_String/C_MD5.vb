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

        'MD5 ���ܺ���
        Public Shared Function MD5(ByVal strSource As String, ByVal Code As Int16, ByVal bZero As Boolean) As String
            If strSource = "" Then Return "none"
            '�����õ���ascii��������ԭ�ģ����Ҫ�ú��������룬������UnicodeEncoding��������ASP�е�MD5����������
            Dim dataToHash As Byte() = (New System.Text.ASCIIEncoding).GetBytes(strSource)
            Dim hashvalue As Byte() = CType(System.Security.Cryptography.CryptoConfig.CreateFromName("MD5"), System.Security.Cryptography.HashAlgorithm).ComputeHash(dataToHash)
            Dim strTmp As String = ""
            Dim strReturn As String = ""
            Select Case Code
                Case 16  'ѡ��16λ�ַ��ļ��ܽ��
                    For i As Integer = 4 To 11
                        strReturn += Hex(hashvalue(i)).ToLower
                    Next
                Case 32  'ѡ��32λ�ַ��ļ��ܽ��
                    For i As Integer = 0 To 15
                        strTmp = Hex(hashvalue(i)).ToLower
                        If bZero = True AndAlso strTmp.Length = 1 Then
                            strTmp = "0" & strTmp
                        End If
                        strReturn += strTmp
                    Next
                Case Else   'Code����ʱ������ȫ���ַ�������32λ�ַ�
                    For i As Integer = 0 To hashvalue.Length - 1
                        strReturn += Hex(hashvalue(i)).ToLower
                    Next
            End Select

            Return strReturn
        End Function
    End Class

End Namespace



