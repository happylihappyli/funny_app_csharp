Namespace Funny

    Public Class C_K_Double_Str
        Implements IComparable

        Private dbValue As Double
        Private mStr As String

        Public Sub New(ByVal Key As Double, ByVal str As String)
            mStr = str
            dbValue = Key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer Implements IComparable.CompareTo

            If dbValue > (CType(key, C_K_Double_Str).dbValue) Then
                Return 1
            ElseIf dbValue < (CType(key, C_K_Double_Str).dbValue) Then
                Return -1
            Else
                Dim iReturn As Integer = String.Compare(mStr, CType(key, C_K_Double_Str).mStr, True)
                If iReturn > 0 Then
                    Return 1
                ElseIf iReturn < 0 Then
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

