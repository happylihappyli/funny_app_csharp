
Namespace Funny
    Public Class C_Key_Value_Value_ID
        Implements IComparable

        Private dbKey, dbKey2 As Double
        Private lngID As Integer

        Public Sub New(ByVal Key As Double, ByVal Key2 As Double, ByVal ID As Integer)
            lngID = ID
            dbKey = Key
            dbKey2 = Key2
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer _
        Implements IComparable.CompareTo

            If dbKey > (CType(key, C_Key_Value_Value_ID).dbKey) Then
                Return 1
            ElseIf dbKey < (CType(key, C_Key_Value_Value_ID).dbKey) Then
                Return -1
            Else
                If dbKey2 > (CType(key, C_Key_Value_Value_ID).dbKey2) Then
                    Return 1
                ElseIf dbKey2 < (CType(key, C_Key_Value_Value_ID).dbKey2) Then
                    Return -1
                Else
                    If lngID > (CType(key, C_Key_Value_Value_ID).lngID) Then
                        Return 1
                    ElseIf lngID < (CType(key, C_Key_Value_Value_ID).lngID) Then
                        Return -1
                    Else
                        Return 0
                    End If
                End If
            End If

        End Function

        Public Overrides Function ToString() As String
            Return dbKey.ToString
        End Function
    End Class
End Namespace