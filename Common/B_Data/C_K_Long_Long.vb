Namespace Funny

    Public Class C_K_Long_Long
        Implements IComparable

        Private dbKey As Long
        Private dbKey2 As Long

        Public Sub New(ByVal Key As Long, ByVal Key2 As Long)
            dbKey = Key
            dbKey2 = Key2
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbKey > (CType(key, C_K_Long_Long).dbKey) Then
                Return 1
            ElseIf dbKey < (CType(key, C_K_Long_Long).dbKey) Then
                Return -1
            Else
                If dbKey2 > (CType(key, C_K_Long_Long).dbKey2) Then
                    Return 1
                ElseIf dbKey2 < (CType(key, C_K_Long_Long).dbKey2) Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        End Function

        Public Overrides Function ToString() As String
            Return dbKey.ToString
        End Function
    End Class

End Namespace