Namespace Funny

    Public Class C_Key_Value_ID
        Implements IComparable

        Private dbValue As Double
        Private lngID As Integer

        Public Sub New(ByVal Key As Double, ByVal ID As Integer)
            lngID = ID
            dbValue = Key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbValue > (CType(key, C_Key_Value_ID).dbValue) Then
                Return 1
            ElseIf dbValue < (CType(key, C_Key_Value_ID).dbValue) Then
                Return -1
            Else
                If lngID > (CType(key, C_Key_Value_ID).lngID) Then
                    Return 1
                ElseIf lngID < (CType(key, C_Key_Value_ID).lngID) Then
                    Return -1
                Else
                    Return 0
                End If
            End If

        End Function

        Public Overrides Function ToString() As String
            Return dbValue.ToString
        End Function
    End Class

End Namespace