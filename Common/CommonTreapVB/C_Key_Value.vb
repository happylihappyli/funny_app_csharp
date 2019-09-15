

Namespace Funny

    Public Class C_Key_Value
        Implements IComparable

        Private dbKey As Double

        Public Sub New(ByVal Key As Double)
            dbKey = Key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbKey > (CType(key, C_Key_Value).dbKey) Then
                Return 1
            ElseIf dbKey < (CType(key, C_Key_Value).dbKey) Then
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