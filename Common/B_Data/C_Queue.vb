
Namespace Funny

    Public Class C_Queue

        Public Class C_Queue_Item
            '���� ˫��ָ��
            Public pObject As Object
            Public pBefore As C_Queue_Item 'ǰһ��
            Public pNext As C_Queue_Item '��һ��
        End Class


        Dim pHead As C_Queue_Item '����ͷ ���ָ��һ������²�Ҫ��
        Dim pTail As C_Queue_Item '����β�� ���ָ��һ������²�Ҫ��
        Dim iCount As Integer 'һ�㲻Ҫ���������


        Public Sub New()
            '��ͷ�ͽ�β����2��ItemΪ���㷨����Ҫ,ʵ�ʼ����ʱ��Ҫ��ȥ��2��Item
            pHead = New C_Queue_Item
            pTail = New C_Queue_Item

            pHead.pNext = pTail
            pTail.pBefore = pHead
        End Sub

        Public ReadOnly Property Count() As Integer
            Get
                Return iCount
            End Get
        End Property

        Public Sub EnQue(ByRef pObject As Object)
            '�������,��β�Ͳ���

            Dim pItem As C_Queue_Item
            pItem = New C_Queue_Item
            pItem.pObject = pObject

            iCount += 1 '����+1

            'ע�� pHead pTail ��2�������Ľڵ�
            pItem.pNext = pTail
            pItem.pBefore = pTail.pBefore

            pItem.pBefore.pNext = pItem
            pTail.pBefore = pItem
        End Sub

        Public Function DeQue() As Object
            'ע�� pHead pTail ��2�������Ľڵ�
            Dim pItem As C_Queue_Item

            If pHead.pNext Is pTail Then
                Return Nothing
            Else
                pItem = pHead.pNext
            End If

            Remove(pItem)

            If pItem IsNot Nothing Then
                Return pItem.pObject
            Else
                Return Nothing
            End If
        End Function

        Public Sub Remove(ByRef pItem As C_Queue_Item)
            'ɾ��pItem �ڵ�
            Try
                'zzzzzzzzzzzzzzzzzzzzz ���д���
                pItem.pBefore.pNext = pItem.pNext
                pItem.pNext.pBefore = pItem.pBefore

                'pItem ��ǰ��ָ�붼����
                pItem.pBefore = Nothing
                pItem.pNext = Nothing
            Catch ex As Exception
                Exit Sub
            End Try

            iCount -= 1 '����-1
        End Sub


        Public Function Peek_Queue() As C_Queue_Item
            'ע�� pHead pTail ��2�������Ľڵ�
            If pHead.pNext Is pTail Then
                Return Nothing
            Else
                Return pHead.pNext
            End If
        End Function

    End Class
End Namespace
