Namespace Funny

    Public Class C_K_ID
        Implements IComparable

        Private dbKey As Integer

        Public Sub New(ByVal Key As Integer)
            dbKey = Key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbKey > (CType(key, C_K_ID).dbKey) Then
                Return 1
            ElseIf dbKey < (CType(key, C_K_ID).dbKey) Then
                Return -1
            Else
                Return 0
            End If
        End Function

        Public Overrides Function ToString() As String
            Return dbKey.ToString
        End Function
    End Class

End Namespace