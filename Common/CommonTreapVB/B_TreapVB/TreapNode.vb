'/<summary>
'/ The TreapNode class encapsulates a node in the treap
'/</summary>

Imports System.Text

Namespace TreapVB

    Public Class TreapNode

        ' key provided by the calling class
        Private ordKey As IComparable
        ' the data or value associated with the key
        Private objData As Object
        ' random priority to balance the tree
        Private intPriority As Integer
        ' left node of the tree
        Private tnLeft As TreapNode
        ' right node of the tree
        Private tnRight As TreapNode

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
        Public Property Data() As Object
            Get
                Return objData
            End Get

            Set(ByVal Value As Object)
                objData = Value
            End Set
        End Property
        '/<summary>
        '/Priority
        '/</summary>
        Public Property Priority() As Integer
            Get
                Return intPriority
            End Get

            Set(ByVal Value As Integer)
                intPriority = Value
            End Set
        End Property
        '/<summary>
        '/Left
        '/</summary>
        Public Property Left() As TreapNode
            Get
                Return tnLeft
            End Get

            Set(ByVal Value As TreapNode)
                tnLeft = Value
            End Set
        End Property
        '/<summary>
        '/ Right
        '/</summary>
        Public Property Right() As TreapNode
            Get
                Return tnRight
            End Get

            Set(ByVal Value As TreapNode)
                tnRight = Value
            End Set
        End Property
        '/<summary>
        '/ RotateLeft
        '/ Rebalance the tree by rotating the nodes to the left
        '/</summary>
        Public Function RotateLeft() As TreapNode

            Dim temp As TreapNode = Right
            'If Right IsNot Nothing Then
            Right = Right.Left
            'End If
            temp.Left = Me

            Return temp

        End Function
        '/<summary>
        '/ RotateRight
        '/ Rebalance the tree by rotating the nodes to the right
        '/</summary>
        Public Function RotateRight() As TreapNode

            Dim temp As TreapNode = Left
            Left = Left.Right
            temp.Right = Me

            Return temp

        End Function

        '/<summary>
        '/ DeleteRoot
        '/ If one of the children is an empty subtree, remove the root and put the other
        '/ child in its place. If both children are nonempty, rotate the treapTree at
        '/ the root so that the child with the smallest priority number comes to the
        '/ top, then delete the root from the other subtee.
        '/
        '/ NOTE: This method is recursive
        '/</summary>
        Public Function DeleteRoot() As TreapNode

            Dim temp As TreapNode

            If (Left Is Nothing) Then
                Return Right
            End If

            If (Right Is Nothing) Then
                Return Left
            End If

            If (Left.Priority < Right.Priority) Then
                temp = RotateRight()
                temp.Right = DeleteRoot()
            Else _
              : temp = RotateLeft()
                temp.Left = DeleteRoot()
            End If

            Return temp

        End Function
    End Class
End Namespace