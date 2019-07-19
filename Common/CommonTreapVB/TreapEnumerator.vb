'/<summary>
'/ The TreapEnumerator class returns the keys or objects of the treap in
'/ sorted order.
'/</summary>

Imports System.Collections

Namespace TreapVB

    Public Class TreapEnumerator

        ' the treap uses the stack to order the nodes 
        Private stack As Stack
        ' return the keys
        Private keys As Boolean
        ' return in ascending order (true) or descending (false)
        Private ascending As Boolean

        ' key
        Private ordKey As IComparable
        ' the data or value associated with the key
        Private objValue As Object

        '/<summary>
        '/Key 
        '/</summary>
        Public Property Key() As IComparable
            Get
                Return ordKey
            End Get

            Set(ByVal Value As IComparable)
                ordKey = Value
            End Set
        End Property
        '/<summary>
        '/Data
        '/</summary>
        Public Property Value() As Object
            Get
                Return objValue
            End Get

            Set(ByVal Value As Object)
                objValue = Value
            End Set
        End Property

        Public Sub New()
        End Sub
        '/<summary>
        '/ Determine order, walk the tree and push the nodes onto the stack
        '/</summary>
        Public Sub New(ByVal tnode As TreapNode, _
                        ByVal keys As Boolean, _
                        ByVal ascending As Boolean)

            stack = New Stack()
            Me.keys = keys
            Me.ascending = ascending

            ' find the lowest node
            If (ascending) Then
                While Not (tnode Is Nothing)
                    stack.Push(tnode)
                    tnode = tnode.Left
                End While
            Else
                ' find the highest or greatest node
                While Not (tnode Is Nothing)
                    stack.Push(tnode)
                    tnode = tnode.Right
                End While
            End If

        End Sub
        '/<summary>
        '/ HasMoreElements
        '/</summary>
        Public Function HasMoreElements() As Boolean
            Return (stack.Count() > 0)
        End Function
        '/<summary>
        '/ NextElement
        '/</summary>
        Public Function NextElement() As Object

            If (stack.Count = 0) Then
                Throw New TreapException("Element not found")
            End If

            ' the top of stack will always have the next item
            ' get top of stack but don't remove it as the next nodes in sequence
            ' may be pushed onto the top
            ' the stack will be popped after all the nodes have been returned
            Dim node As TreapNode = CType(stack.Peek(), TreapNode)

            If ascending Then
                ' if right node is nothing, the stack top is the lowest node
                ' if left node is nothing, the stack top is the highest node
                If node.Right Is Nothing Then
                    ' walk the tree
                    Dim tn As TreapNode = CType(stack.Pop(), TreapNode)
                    While (HasMoreElements()) AndAlso _
                        ((CType(stack.Peek(), TreapNode)).Right Is tn)
                        tn = CType(stack.Pop(), TreapNode)
                    End While
                Else
                    ' find the next items in the sequence
                    ' traverse to left; find lowest and push onto stack
                    Dim tn As TreapNode = node.Right
                    While Not (tn Is Nothing)
                        stack.Push(tn)
                        tn = tn.Left
                    End While
                End If
                ' descending
            ElseIf _
                   node.Left Is Nothing Then
                ' walk the tree
                Dim tn As TreapNode = CType(stack.Pop(), TreapNode)
                While (HasMoreElements()) AndAlso _
                    ((CType(stack.Peek(), TreapNode)).Left Is tn)
                    tn = CType(stack.Pop(), TreapNode)
                End While
            Else
                ' find the next items in the sequence
                ' traverse to right; find highest and push onto stack
                Dim tn As TreapNode = node.Left
                While Not (tn Is Nothing)
                    stack.Push(tn)
                    tn = tn.Right
                End While
            End If

            ' the following is for .NET compatibility (see MoveNext())
            Key = node.Key
            Value = node.Data

            Dim objValue As Object = IIf(keys = True, node.Key, node.Data)
            Return objValue

        End Function
        '/<summary>
        '/ MoveNext
        '/ For .NET compatibility
        '/</summary>
        Public Function MoveNext() As Boolean

            If HasMoreElements() Then
                NextElement()
                Return True
            Else
                Return False
            End If

        End Function
    End Class
End Namespace