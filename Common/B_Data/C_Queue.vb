
Namespace Funny

    Public Class C_Queue

        Public Class C_Queue_Item
            '队列 双向指针
            Public pObject As Object
            Public pBefore As C_Queue_Item '前一个
            Public pNext As C_Queue_Item '后一个
        End Class


        Dim pHead As C_Queue_Item '队列头 这个指针一般情况下不要用
        Dim pTail As C_Queue_Item '队列尾巴 这个指针一般情况下不要用
        Dim iCount As Integer '一般不要改这个数字


        Public Sub New()
            '开头和结尾的这2个Item为了算法简化需要,实际计算的时候要减去这2个Item
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
            '插入队列,从尾巴插入

            Dim pItem As C_Queue_Item
            pItem = New C_Queue_Item
            pItem.pObject = pObject

            iCount += 1 '个数+1

            '注意 pHead pTail 是2个保留的节点
            pItem.pNext = pTail
            pItem.pBefore = pTail.pBefore

            pItem.pBefore.pNext = pItem
            pTail.pBefore = pItem
        End Sub

        Public Function DeQue() As Object
            '注意 pHead pTail 是2个保留的节点
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
            '删除pItem 节点
            Try
                'zzzzzzzzzzzzzzzzzzzzz 这有错误
                pItem.pBefore.pNext = pItem.pNext
                pItem.pNext.pBefore = pItem.pBefore

                'pItem 的前后指针都砍断
                pItem.pBefore = Nothing
                pItem.pNext = Nothing
            Catch ex As Exception
                Exit Sub
            End Try

            iCount -= 1 '个数-1
        End Sub


        Public Function Peek_Queue() As C_Queue_Item
            '注意 pHead pTail 是2个保留的节点
            If pHead.pNext Is pTail Then
                Return Nothing
            Else
                Return pHead.pNext
            End If
        End Function

    End Class
End Namespace
