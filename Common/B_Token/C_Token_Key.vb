
Imports B_Token.Funny

Namespace Funny

    Public Class C_Token_Key
        Public strKey As String
        Public pToken As C_Token

        Public Sub New( _
                ByVal strKey As String, _
                ByRef pToken As C_Token)

            Me.strKey = strKey
            'Me.strContent = strContent
            Me.pToken = pToken
        End Sub
    End Class

End Namespace