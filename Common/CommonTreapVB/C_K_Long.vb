Namespace Funny

    Public Class C_K_Long
        Implements IComparable

        Private dbKey As Long

        Public Sub New(ByVal Key As Long)
            dbKey = Key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbKey > (CType(key, C_K_Long).dbKey) Then
                Return 1
            ElseIf dbKey < (CType(key, C_K_Long).dbKey) Then
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