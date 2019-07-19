
Namespace Funny
    Public Module S_Number

        Public Function GetNumeber(ByVal strInput As String) As String
            Dim strReturn As String = ""

            If strInput.Length = 1 Then
                Select Case strInput
                    Case "一"
                        strReturn = "1"
                    Case "二"
                        strReturn = "2"
                    Case "三"
                        strReturn = "3"
                    Case "四"
                        strReturn = "4"
                    Case "五"
                        strReturn = "5"
                    Case "六"
                        strReturn = "6"
                    Case "七"
                        strReturn = "7"
                    Case "八"
                        strReturn = "8"
                    Case "九"
                        strReturn = "9"
                    Case "十"
                        strReturn = "10"
                End Select
            ElseIf strInput.Length = 2 Then
                strReturn = ""
                Dim strTmp As String
                For i As Integer = 1 To 2
                    strTmp = S_Number.GetNumeber(strInput.Substring(0, 1))
                    strReturn += strTmp
                Next
            Else
                strReturn = strInput
            End If
            Return strReturn
        End Function
    End Module

End Namespace
