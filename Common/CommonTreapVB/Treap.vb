'/<summary>
'/   An implementation of a treap data structure.
'/  
'/   A treap is based upon a randomized binary Tree whose nodes have keys and
'/   which also have priorities associated with them in a heap - hence the
'/   name (tre)e h(eap).
'/  
'/   A Tree is a `heap' if each node has a priority such that the priority of 
'/   each child is greater than the priority of its parent (trees that are heaps 
'/   can also called "priority queues", and are the basis of a heap sort).
'/  
'/   The priorities are a means to randomize the Tree. Randomized trees are better
'/   balanced (essentially meaning less depth) so that seach time is minimized.
'/  
'/   Treaps were first introduced by Seidel and Aragon in <blockquote>
'/   R. Seidel and C. R. Aragon. Randomized Binary Search Trees.
'/   <em>Algorithmica</em>, 16(4/5):464-497, 1996. </blockquote>
'/  
'/   Most methods run in O(log n) randomized time, where n is the number of keys in 
'/   the treap. The exceptions are clone() and toString() that run in time
'/   proportional to the intCount of their output.
'/  
'/   This code was was ultimately based upon the Java treap implementation by Stefan Nilsson 
'/   published in Dr. Dobb's Journal, pp. 40-44, Vol. 267, July 1997
'/</summary>
Imports System.Text
Imports System.Collections
Imports B_TreapVB.Funny

Namespace TreapVB

    Public Class Treap(Of T)
        Inherits Object

        ' random priority to keep the treap balanced
        Private rndPriority As Random = New Random
        ' the number of key-and-value pairs contained in the treap
        Private intCount As Integer
        ' used for quick comparisons
        Private intHashCode As Integer = rndPriority.Next()
        ' identIfies the owner of the treap
        Private strIdentIfier As String
        ' the treap
        Private treapTree As TreapNode
        Private boolKeyFound As Boolean
        Private prevData As Object

        Public KeyMaxLen As Integer '代表Key的字符串最长为多少，一般不用

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal strIdentIfier As String)
            MyBase.New()
            Me.strIdentIfier = strIdentIfier
        End Sub


        Public Function insert(ByVal key As String, ByVal data As T) As TreapNode
            Return insert(New C_K_Str(key), data)
        End Function




        '/<summary>
        '/ Add
        '/ args: ByVal key As IComparable, ByVal data As Object
        '/ key is object that implements IComparable interface
        '/</summary>
        Public Function insert(ByVal key As IComparable, ByVal data As T) As TreapNode

            If (key Is Nothing Or data Is Nothing) Then
                Throw New TreapException("Treap key and data must not be Nothing")
            End If

            ' create New node
            Dim node As TreapNode = New TreapNode
            If node Is Nothing Then
                Throw New TreapException("Unable to create Treap Node")
            End If

            node.Key = key
            node.Data = data
            ' generate random priority
            node.Priority = rndPriority.Next
            boolKeyFound = False

            ' Insert node into treapTree
            treapTree = InsertNode(node, treapTree)

            If (boolKeyFound) Then
                'Throw New TreapException("A Node with the same key already exists")
            Else
                intCount = intCount + 1
            End If

            Return treapTree
        End Function

        '/<summary>
        '/ InsertNode
        '/ inserts a node into the tree - note recursive method
        '/ this method rebalances the tree using the priorities
        '/
        '/ Note: The lower the number, the higher the priority    
        '/<summary>
        Private Function InsertNode(ByVal node As TreapNode,
            ByVal tree As TreapNode) As TreapNode

            If (tree Is Nothing) Then
                Return node
            End If

            Dim result As Integer = node.Key.CompareTo(tree.Key)

            If (result < 0) Then
                tree.Left = InsertNode(node, tree.Left)
                If (tree.Left.Priority < tree.Priority) Then
                    tree = tree.RotateRight()
                End If
            ElseIf _
              (result > 0) Then
                tree.Right = InsertNode(node, tree.Right)
                If (tree.Right.Priority < tree.Priority) Then
                    tree = tree.RotateLeft()
                End If
            Else
                boolKeyFound = True
                prevData = tree.Data
                tree.Data = node.Data
            End If

            Return tree

        End Function

        Public Function find(ByVal key As String) As T
            Return find(New C_K_Str(key))
        End Function
        '/<summary>
        '/ GetData
        '/ Gets the data associated with the specified key
        '/<summary>
        Public Function find(ByVal key As IComparable) As T

            Dim treeNode As TreapNode = treapTree
            Dim result As Integer

            'Dim treeNode_old As TreapNode
            While (Not treeNode Is Nothing)
                result = key.CompareTo(treeNode.Key)
                If result = 0 Then
                    Return treeNode.Data
                End If

                'treeNode_old = treeNode
                If (result < 0) Then
                    treeNode = treeNode.Left
                Else
                    treeNode = treeNode.Right
                End If
            End While

            'Throw New TreapException("Treap key was not found")

            Return Nothing

        End Function
        '/<summary>
        '/ GetMinKey
        '/ Returns the minimum key value
        '/<summary>
        Public Function GetMinKey() As IComparable

            Dim treeNode As TreapNode = treapTree

            If (treeNode Is Nothing) Then
                'Throw New TreapException("Treap is empty")
                Return Nothing
            End If

            While Not (treeNode.Left Is Nothing)
                treeNode = treeNode.Left
            End While

            Return treeNode.Key

        End Function

        '/<summary>
        '/ GetMaxKey
        '/ Returns the maximum key value
        '/<summary>
        Public Function GetMaxKey() As IComparable

            Dim treeNode As TreapNode = treapTree

            If (treeNode Is Nothing) Then
                'Throw New TreapException("Treap is empty")
                Return Nothing
            End If

            Do While treeNode.Right IsNot Nothing
                treeNode = treeNode.Right
            Loop

            If treeNode Is Nothing Then
                Return Nothing
            End If

            Return treeNode.Key

        End Function
        '/<summary>
        '/ GetMinValue
        '/ Returns the object having the minimum key value
        '/<summary>
        Public Function GetMinValue() As Object
            Return find(GetMinKey())
        End Function
        '/<summary>
        '/ GetMaxValue
        '/ Returns the object having the maximum key
        '/<summary>
        Public Function GetMaxValue() As Object
            Return find(GetMaxKey())
        End Function
        '/<summary>
        '/ GetEnumerator
        '/<summary>
        Public Function GetEnumerator() As TreapEnumerator
            Return Elements(True)
        End Function
        '/<summary>
        '/ Keys
        '/ If ascending is True, the keys will be returned in ascending order, else
        '/ the keys will be returned in descending order.
        '/<summary>
        Public Function Keys() As TreapEnumerator
            Return Keys(True)
        End Function
        Public Function Keys(ByVal ascending As Boolean) As TreapEnumerator
            Return New TreapEnumerator(treapTree, True, ascending)
        End Function
        '/<summary>
        '/ Values
        '/ .NET compatibility
        '/<summary>
        Public Function Values() As TreapEnumerator
            Return Elements(True)
        End Function
        '/<summary>
        '/ Elements
        '/ Returns an enumeration of the data objects.
        '/ If ascending is true, the objects will be returned in ascending order,
        '/ else the objects will be returned in descending order.
        '/<summary>
        Public Function Elements() As TreapEnumerator
            Return Elements(True)
        End Function

        Public Function Elements(ByVal ascending As Boolean) As TreapEnumerator
            Return New TreapEnumerator(treapTree, False, ascending)
        End Function
        '/<summary>
        '/ IsEmpty
        '/<summary>
        Public Function IsEmpty() As Boolean
            Return (treapTree Is Nothing)
        End Function
        '/<summary>
        '/ Remove
        '/ removes the key and Object
        '/<summary>
        Public Function Remove(ByVal key As IComparable) As Boolean

            boolKeyFound = False

            treapTree = Delete(key, treapTree)

            If boolKeyFound Then
                intCount = intCount - 1
            End If

            Return boolKeyFound
        End Function

        Public Function Remove(ByVal key As String) As Boolean
            Return Remove(New C_K_Str(key))
        End Function


        '/<summary>
        '/ RemoveMin
        '/ removes the node with the minimum key
        '/<summary>
        Public Function RemoveMin() As Object

            ' start at top
            Dim treeNode As TreapNode = treapTree
            Dim prevTreapNode As TreapNode

            If (treeNode Is Nothing) Then
                Throw New TreapException("Treap is null")
            End If

            If (treeNode.Left Is Nothing) Then
                ' remove top node by replacing with right
                treapTree = treeNode.Right
            Else
                Do
                    ' find the minimum node
                    prevTreapNode = treeNode
                    treeNode = treeNode.Left
                Loop While Not treeNode.Left Is Nothing
                ' remove left node by replacing with right node
                prevTreapNode.Left = treeNode.Right
            End If

            intCount = intCount - 1

            Return treeNode.Data

        End Function

        '/<summary>
        '/ RemoveMax
        '/ removes the node with the maximum key
        '/<summary>
        Public Function RemoveMax() As Object

            ' start at top
            Dim treeNode As TreapNode = treapTree
            Dim prevTreapNode As TreapNode

            If (treeNode Is Nothing) Then
                Throw New TreapException("Treap is null")
            End If

            If (treeNode.Right Is Nothing) Then
                ' remove top node by replacing with left
                treapTree = treeNode.Left
            Else
                Do
                    ' find the maximum node
                    prevTreapNode = treeNode
                    treeNode = treeNode.Right
                Loop While Not treeNode.Right Is Nothing
                ' remove right node by replacing with left node
                prevTreapNode.Right = treeNode.Left
            End If

            intCount = intCount - 1

            Return treeNode.Data

        End Function
        '/<summary>
        '/ Clear
        '/<summary>
        Public Sub Clear()
            treapTree = Nothing
            intCount = 0
        End Sub
        '/<summary>
        '/ Size
        '/<summary>
        Public Function Size() As Integer
            ' number of keys
            Return intCount
        End Function
        '/<summary>
        '/ Delete
        '/ deletes a node - note recursive function
        '/ Deletes works by "bubbling down" the node until it is a leaf, and then 
        '/ pruning it off the tree
        '/<summary>
        Private Function Delete(ByVal key As IComparable, ByVal tNode As TreapNode) As TreapNode

            If (tNode Is Nothing) Then
                Return Nothing
            End If

            Dim result As Integer = key.CompareTo(tNode.Key)

            If (result < 0) Then
                tNode.Left = Delete(key, tNode.Left)
            ElseIf _
              (result > 0) Then
                tNode.Right = Delete(key, tNode.Right)
            Else
                boolKeyFound = True
                prevData = tNode.Data
                tNode = tNode.DeleteRoot()
            End If

            Return tNode

        End Function
        '/<summary>
        '/ Equals
        '/<summary>
        Public Overloads Function Equals(ByVal obj As Object) As Boolean

            If obj Is Nothing Then
                Return False
            End If

            If (Not (TypeOf obj Is Treap(Of T))) Then
                Return False
            End If

            If (Me Is obj) Then
                Return True
            End If

            Return (ToString().Equals((CType(obj, Treap(Of T))).ToString()))

        End Function
        '/<summary>
        '/ HashCode
        '/<summary>
        Public Function HashCode() As Integer
            Return intHashCode
        End Function
        '/<summary>
        '/ ToString
        '/<summary>
        Public Overrides Function ToString() As String
            If strIdentIfier Is Nothing Then
                Return ""
            Else
                Return strIdentIfier.ToString()
            End If
        End Function
    End Class
End Namespace