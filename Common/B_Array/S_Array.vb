


Namespace Funny
    Public Class S_Array
        '这里Array就是ArrayList
        Public Shared Function Combine(ByVal ParamArray strArgs() As String) As ArrayList
            Dim pArray As New ArrayList
            For i As Int32 = 0 To strArgs.Length - 1
                pArray.Add(strArgs(i))
            Next
            Return pArray
        End Function


        Public Shared Function Array_Rnd_Line(ByRef pArray As ArrayList) As String
            Randomize()
            Dim Index As Int32 = Math.Floor(Rnd() * pArray.Count)
            If Index < pArray.Count Then
                Return pArray(Index)
            End If
            Return ""
        End Function

        ''' <summary>
        ''' 数组转为字符串
        ''' </summary>
        ''' <param name="pArray"></param>
        ''' <param name="strSeg"></param>
        Public Shared Function Array_ToString(ByRef pArray As ArrayList, ByVal strSeg As String) As String
            Dim strReturn As String = ""
            For i As Integer = 0 To pArray.Count - 1
                strReturn &= pArray(i) & vbCrLf
            Next

            Return strReturn
        End Function

        ''' <summary>
        ''' 数组转为字符串
        ''' </summary>
        ''' <param name="pArray"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Array_ToString(ByRef pArray As ArrayList) As String
            Dim strReturn As String = ""
            For i As Integer = 0 To pArray.Count - 1
                strReturn &= pArray(i) & vbCrLf
            Next
            Return strReturn
        End Function
    End Class



End Namespace