
Namespace FunnyAI

    Public Class C_Queue_Key
        Public Que As Queue = New Queue
        Public strKey As String

        Public Overridable Function CloneMe() As C_Queue_Key
            Dim pQue2 As C_Queue_Key = New C_Queue_Key
            pQue2.Que = Que.Clone
            pQue2.strKey = strKey
            Return pQue2
        End Function


    End Class

End Namespace