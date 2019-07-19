Imports CommonTreapVB.TreapVB

Namespace Funny
    Public Class C_Treap_Funny(Of T)
        Inherits Treap(Of T)

        Public pExtend As Object
        Public bNeedRefresh As Boolean = False '�Ƿ���Ҫˢ�� �����Ƿ���Ҫ���浽�ļ�
        Public strFile As String '��ǰTreap��Ӧ���ļ�

        Private mArrayList As ArrayList 'Treap ��Ӧ������

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
