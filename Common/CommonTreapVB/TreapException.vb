'/<summary>
'/ The TreapException class distinguishes treap exceptions from .NET 
'/ exceptions. It notifies the caller that an exception has occurred in the
'/ treap class.
'/</summary>

Namespace TreapVB

    Public Class TreapException
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

    End Class

End Namespace