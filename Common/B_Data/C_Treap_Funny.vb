Imports CommonTreapVB.TreapVB

Namespace Funny
    Public Class C_Treap_Funny(Of T)
        Inherits Treap(Of T)

        Public pExtend As Object
        Public bNeedRefresh As Boolean = False '是否需要刷新 比如是否不想要保存到文件
        Public strFile As String '当前Treap对应的文件

        Private mArrayList As ArrayList 'Treap 对应的数组

        Public ReadOnly Property pArrayList() As ArrayList
            Get
                If mArrayList Is Nothing Then
                    mArrayList = New ArrayList
                    Dim pItem As Object
                    Dim p As TreapEnumerator = Me.Elements(True)
                    While (p.HasMoreElements())
                        pItem = p.NextElement()
                        mArrayList.Add(pItem)
                    End While
                End If
                Return mArrayList
            End Get
        End Property

        Public Sub pArrayList_Clear()
            mArrayList = Nothing
        End Sub
    End Class

End Namespace
